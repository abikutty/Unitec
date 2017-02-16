using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;

namespace Unitec.Middleware.Services
{

    public abstract class GenericDevice : IGenericDevice
    {

        #region Properties
        private DeviceStatus currentStatus;
        private string firmwareVersion;
        private string logFile;
        public string LogFile
        {
            get
            {
                return logFile;
            }
             set
            {
                if(logFile != value)
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

        #region Methods
        public GenericDevice()
        {
            currentStatus = DeviceStatus.NotInitialized;
        }
        abstract public bool InitializeDevice();
        abstract public bool ConnectToDevice();
        abstract public bool DisconnectFromDevice();
        abstract public bool EnableDevice();
        abstract public bool DisableDevice();
        abstract public bool ResetHardware();
        abstract public bool CheckHealth(out int code, out string status, out string hardwareIdentity, out string report);
        abstract public bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo);
        abstract public bool TerminateDevice();

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
