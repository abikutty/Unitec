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

        private DeviceStatus currentStatus;
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
            currentStatus = DeviceStatus.NotInitialized;
        }

        public GenericDevice(int timeOut)
        {
             this.timeOut = timeOut;
             currentStatus = DeviceStatus.NotInitialized;
        }

        public bool IsConnected
        {
            get
            {
                return currentStatus == DeviceStatus.Connected;
            }
        }

        public bool IsDisconnected
        {
            get
            {
                return currentStatus == DeviceStatus.Disconnected;
            }
        }

        public bool IsEnabled
        {

            get
            {
                return currentStatus == DeviceStatus.Enabled;
            }
        }

        public string FirmwareVersion
        {

            get
            {
                return firmwareVersion;
            }
        }

        #endregion

        #region Interface Methods

        public virtual bool InitializeDevice()
        {
            try
            {
                "Attempting to Initialize Device...".Log(LogFile);
                if (String.IsNullOrEmpty(PeripheralsConfigFile) && File.Exists(PeripheralsConfigFile))
                {
                    throw new FileNotFoundException("Config file missing.", PeripheralsConfigFile);
                }
                var conn = new PeripheralConnection(PeripheralsConfigFile);

                DevicePort = new SerialPort(conn.ComPort, conn.BaudRate,
                                                 conn.Parity, conn.DataBits, conn.StopBits);
                currentStatus = DeviceStatus.Initialized;
                "Successfully Initialized the Device...".Log(LogFile);
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
                if (currentStatus == DeviceStatus.NotInitialized)
                {
                    throw new InvalidOperationException("Device is not initialized..");
                }
                "Attempting to Connect Device...".Log(LogFile);
                if (!DevicePort.IsOpen)
                {
                    DevicePort.Open();
                    DevicePort.DataReceived += HandleSerialDataReceived;
                    DevicePort.ErrorReceived += HandleErrorReceived;
                }
                currentStatus = DeviceStatus.Connected;
                OnDeviceConnected(new EventArgs());
                "Successfully Connected to Device...".Log(LogFile);
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
                "Attempting to Disconnect Device...".Log(LogFile);
                if (DevicePort.IsOpen)
                {
                    DevicePort.Close();
                }
                currentStatus = DeviceStatus.Disconnected;
                OnDeviceDisconnected(new EventArgs());
                "Successfully Disconnected the Device...".Log(LogFile);
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
                if (currentStatus != DeviceStatus.Connected)
                {
                    "Device is not connected..".Log(LogFile);
                    return false;
                }
                if (currentStatus == DeviceStatus.Enabled)
                {
                    return true;
                }
                DevicePort.DtrEnable = true;
                currentStatus = DeviceStatus.Enabled;
                "Successfully Enabled the Device...".Log(LogFile);
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

                if (IsEnabled || IsConnected)
                {
                    //If connected, should i disconnect before disabiling?

                    "Attempting to Disable Device...".Log(LogFile);
                    DevicePort.DtrEnable = false;
                    "Successfully Disabled the Device...".Log(LogFile);
                    currentStatus = DeviceStatus.Disabled;
                }
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

        protected virtual void OnDeviceErrorOccurred(DeviceErrorEventArgs e)
        {
            DeviceErrorEventHandler handler = DeviceErrorOccurred;
            if (handler != null)
            {
                "device error occured".Log(LogFile);
                handler(this, e);
            }
        }


        protected virtual void OnDeviceConnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Connected;
            EventHandler handler = DeviceConnected;
            if (handler != null)
            {
                "device connected".Log(LogFile);
                handler(this, e);
            }
        }


        protected virtual void OnDeviceDisconnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Disconnected;
            EventHandler handler = DeviceDisconnected;
            if (handler != null)
            {
                "device disconnected".Log(LogFile);
                handler(this, e);
            }
        }
        #endregion

        #region Helper Methods
        protected virtual void HandleException(Exception ex, DeviceErrorType error)
        {
            if (ex != null)
            {
                var errorArg = ex.Create(error,LogFile);
                OnDeviceErrorOccurred(errorArg);

            }
        }


        protected virtual void WriteLog(Exception ex)
        {
            ex.Log(LogFile);
        }

        protected byte[] ReadResponse()
        {
            if (ReadSignaled() != ReadStatus.Timeout)
            {
                if(DevicePort.Read(message, 0, message.Count()) > 0)
                {
                    String.Format("Received response {0}", message).Log(LogFile);
                    if (message[0] != (int) ResponseStatus.ACK)
                    {
                        throw new InvalidOperationException(String.Format("ACK wasn't received", BitConverter.ToString(message)));
                    }
                    return message;
                }
            }
            throw new InvalidOperationException("Read timeout...");
        }


        protected void WriteCommand(byte[] command)
        {
            String.Format("Sending command {0}", command).Log(LogFile);
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
            var eventArgs = ex.Create(DeviceErrorType.ErrorReceving, LogFile);
            OnDeviceErrorOccurred(eventArgs);
            errorReceived.Set();
        }

        public void Dispose()
        {
            
        }
        #endregion
    }
}
