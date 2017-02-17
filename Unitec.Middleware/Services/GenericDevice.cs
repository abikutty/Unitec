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

namespace Unitec.Middleware.Services
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

        virtual public bool InitializeDevice()
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
            return true;
        }
        virtual public bool ConnectToDevice()
        {
            if(currentStatus == DeviceStatus.NotInitialized)
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
            "Successfully Connected to Device...".Log(LogFile);
            return true;
        }
        virtual public bool DisconnectFromDevice()
        {
            "Attempting to Disconnect Device...".Log(LogFile);
            if (DevicePort.IsOpen)
            {
                DevicePort.Close();
            }
            currentStatus = DeviceStatus.Disconnected;
            "Successfully Disconnected the Device...".Log(LogFile);
            return true;
        }
        virtual public bool EnableDevice()
        {
            "Attempting to Enable Device...".Log(LogFile);
            DevicePort.DtrEnable = true;
            currentStatus = DeviceStatus.Enabled;
            "Successfully Enabled the Device...".Log(LogFile);
            return true;
        }
        virtual public bool DisableDevice()
        {
            if (IsEnabled)
            {
                "Attempting to Disable Device...".Log(LogFile);
                DevicePort.DtrEnable = false;
                "Successfully Disabled the Device...".Log(LogFile);
                currentStatus = DeviceStatus.Disabled;
            }
            return true;
        }
        abstract public bool ResetHardware();
        abstract public bool CheckHealth(out int code, out string status, out string hardwareIdentity, out string report);
        abstract public bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo);
        abstract public bool TerminateDevice();
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
            if (ReadSignaled())
            {
                if(DevicePort.Read(message, 0, message.Count()) > 0)
                {
                    return message;
                }
            }
            return null;
        }


        protected void WriteCommand(byte[] command)
        {
            if (command != null)
            {
                DevicePort.Write(command, 0, command.Count());
            }
        }


        private bool ReadSignaled()
        {
            if (WaitHandle.WaitAny(new WaitHandle[] { messageReceived, errorReceived }, timeOut) == 0)
                return true;
            return false;
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


    }
}
