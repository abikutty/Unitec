using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;
using Unitec.Middleware.Helpers;

namespace Unitec.Middleware.Devices
{

    public abstract class GenericDevice : IGenericDevice
    {

        #region Properties
        protected SerialPort DevicePort { get; set; }
        private byte[] message = new byte[8];
        private AutoResetEvent messageReceived = new AutoResetEvent(false);
        private AutoResetEvent errorReceived = new AutoResetEvent(false);

        private int currentStatus;
        private string firmwareVersion;
        private string logFile;
        private int timeOut;
        public string LogFile
        {
            get
            {
                return logFile;
            }
            set
            {
                if (logFile != value)
                {
                    logFile = value;
                    var dir = Path.GetDirectoryName(logFile);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory((dir));
                    }
                }
            }
        }

        public string PeripheralsConfigFile { get; set; }

        public GenericDevice()
        {
            this.timeOut = 5000; //default
            currentStatus = DeviceStatus.NotInitialized.SetDeviceStatus(currentStatus);
        }

        public GenericDevice(int timeOut)
        {
             this.timeOut = timeOut;
            currentStatus = DeviceStatus.NotInitialized.SetDeviceStatus(currentStatus);
        }

        public bool IsConnected
        {
            get
            {
                return DeviceStatus.Connected.CheckDeviceStatus(currentStatus);
            }
        }

        public bool IsDisconnected
        {
            get
            {
                return DeviceStatus.Disconnected.CheckDeviceStatus(currentStatus);
            }
        }

        public bool IsEnabled
        {

            get
            {
                return DeviceStatus.Enabled.CheckDeviceStatus(currentStatus);
            }
        }

        public string FirmwareVersion
        {

            get
            {
                return firmwareVersion;
            }
            protected set
            {
                firmwareVersion = value;
            }
        }

        #endregion

        #region Interface Methods

        public virtual bool InitializeDevice()
        {
            try
            {
                this.Log("Attempting to Initialize Device...");
                if(currentStatus == (int) DeviceStatus.Disposed)
                {
                    throw new InvalidOperationException("Invalid Instance, Get new instance from container");
                }
                if (String.IsNullOrEmpty(PeripheralsConfigFile) && File.Exists(PeripheralsConfigFile))
                {
                    throw new FileNotFoundException("Config file missing.", PeripheralsConfigFile);
                }
                var conn = new PeripheralConnection(PeripheralsConfigFile);

                DevicePort = new SerialPort(conn.ComPort, conn.BaudRate,
                                                 conn.Parity, conn.DataBits, conn.StopBits);
                currentStatus = DeviceStatus.Initialized.SetDeviceStatus(currentStatus);
                currentStatus = DeviceStatus.NotInitialized.SetDeviceStatus(currentStatus,false);
                this.Log("Successfully Initialized the Device...");
            }
            catch
            {
                throw;
            }
            return true;
        }
        public virtual bool ConnectToDevice()
        {
            try
            {
                if (DeviceStatus.NotInitialized.CheckDeviceStatus(currentStatus))
                {
                    throw new InvalidOperationException("Device is not initialized..");
                }
                this.Log("Attempting to Connect Device...");
                if (!DevicePort.IsOpen)
                {
                    DevicePort.Open();
                    DevicePort.DataReceived += HandleSerialDataReceived;
                    DevicePort.ErrorReceived += HandleErrorReceived;
                }
                currentStatus = DeviceStatus.Connected.SetDeviceStatus(currentStatus);
                currentStatus = DeviceStatus.Disconnected.SetDeviceStatus(currentStatus, false);
                OnDeviceConnected(new EventArgs());
                this.Log("Successfully Connected to Device...");
            }
            catch
            {
                throw;
            }
            return true;
        }


        public virtual bool DisconnectFromDevice()
        {
            try
            {
                this.Log("Attempting to Disconnect Device...");
                if (DevicePort.IsOpen)
                {
                    DevicePort.Close();
                }
                currentStatus = DeviceStatus.Disconnected.SetDeviceStatus(currentStatus);
                currentStatus = DeviceStatus.Connected.SetDeviceStatus(currentStatus, false);
                OnDeviceDisconnected(new EventArgs());
                this.Log("Successfully Disconnected the Device...");
            }
            catch
            {
                throw;
            }
            return true;
        }


        public virtual bool EnableDevice()
        {
            try
            {
                if (!IsConnected)
                {
                    this.Log("Device is not connected..");
                    return false;
                }
                if (IsEnabled)
                {
                    return true;
                }
                DevicePort.DtrEnable = true;
                currentStatus = DeviceStatus.Enabled.SetDeviceStatus(currentStatus);
                currentStatus = DeviceStatus.Disabled.SetDeviceStatus(currentStatus, false);
                this.Log("Successfully Enabled the Device...");
            }
            catch
            {
                throw;
            }
            return true;
        }


        public virtual bool DisableDevice()
        {
            try
            {
                this.Log("Attempting to Disable Device...");
                if (IsEnabled || IsConnected)
                {
                    DevicePort.DtrEnable = false;
                    currentStatus = DeviceStatus.Disabled.SetDeviceStatus(currentStatus);
                    currentStatus = DeviceStatus.Enabled.SetDeviceStatus(currentStatus, false);
                }
                this.Log("Successfully Disabled the Device...");
            }
            catch
            {
                throw;
            }
            return true;
        }

        abstract public bool ResetHardware();
        abstract public bool CheckHealth(out int code, out string status, out string hardwareIdentity, out string report);
        abstract public bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo);
        abstract public bool TerminateDevice();
        #endregion

        #region Events

        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;
        public event DeviceErrorEventHandler DeviceErrorOccurred;
        public event DataLoggedEventHandler DataLogged;

        protected virtual void OnDeviceErrorOccurred(DeviceErrorEventArgs e)
        {
            DeviceErrorEventHandler handler = DeviceErrorOccurred;
            if (handler != null)
            {
                this.Log("device error occured");
                handler(this, e);
            }
        }


        protected virtual void OnDeviceConnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Connected.SetDeviceStatus(currentStatus);
            currentStatus = DeviceStatus.Disconnected.SetDeviceStatus(currentStatus, false);
            EventHandler handler = DeviceConnected;
            if (handler != null)
            {
                this.Log("device connected");
                handler(this, e);
            }
        }


        protected virtual void OnDeviceDisconnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Disconnected.SetDeviceStatus(currentStatus);
            currentStatus = DeviceStatus.Connected.SetDeviceStatus(currentStatus, false);
            EventHandler handler = DeviceDisconnected;
            if (handler != null)
            {
                this.Log("device disconnected");
                handler(this, e);
            }
        }


        internal virtual void OnDataLogged(LogEventArgs e)
        {
            DataLogged?.Invoke(this, e);
        }

        #endregion

        #region Helper Methods
        protected virtual void HandleException(Exception ex, DeviceErrorType error)
        {
            if (ex != null)
            {
                var errorArg = this.Create(ex,error);
                OnDeviceErrorOccurred(errorArg);

            }
        }


        protected virtual void WriteLog(Exception ex)
        {
            this.Log(ex);
        }

        protected byte[] ReadResponse()
        {
            if (ReadSignaled() != ReadStatus.Timeout)
            {
                if(DevicePort.Read(message, 0, message.Count()) > 0)
                {
                    this.Log(String.Format("Received response {0}", BitConverter.ToString(message)));
                    if (message[0] != (int) ResponseStatus.ACK)
                    {
                        throw new InvalidOperationException(String.Format("ACK wasn't received", BitConverter.ToString(message)));
                    }
                    return message;
                }
            }
            this.Log("Read timeout...");
            throw new InvalidOperationException("Read timeout...");
        }


        protected void WriteCommand(byte[] command)
        {
            this.Log(String.Format("Sending command {0}", BitConverter.ToString(command)));
            if (command != null)
            {
                DevicePort.Write(command, 0, command.Count());
            }
        }


        protected void WriteCommand(UInt64 command)
        {
            if (command > 0)
            {
                WriteCommand(BitConverter.GetBytes(command));
            }
        }


        private ReadStatus ReadSignaled()
        {
            int response = WaitHandle.WaitAny(new WaitHandle[] { messageReceived, errorReceived }, timeOut);
            if (response == 0) return ReadStatus.Success;
            if (response == 1) return ReadStatus.Failed;
            return ReadStatus.Timeout;
        }


        protected void HandleSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof)
                messageReceived.Set();
        }

        protected void HandleErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            var ex = new Exception(String.Format("Internal Error Code {0}", e.EventType));
            var eventArgs = this.Create(ex,DeviceErrorType.ErrorReceving);
            OnDeviceErrorOccurred(eventArgs);
            errorReceived.Set();
        }

        public void Dispose()
        {
            currentStatus = 0;
        }
        #endregion
    }
}
