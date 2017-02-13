using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;

namespace Unitec.Middleware.Services
{
    public enum DeviceStatus
    {
        NotInitialized,
        Initialized,
        Connected,
        Disconnected
    }
    public abstract class GenericDevice : IGenericDevice
    {
        #region Properties
        private DeviceStatus currentStatus;
        public string LogFile { get; set; }
        public string PeripheralsConfigFile { get; set; }
        public bool IsConnected { get; set; }
        public bool IsEnabled { get; set; }
        public string FirmwareVersion { get; set; }
        #endregion

        #region Methods
        public GenericDevice()
        {
            currentStatus = DeviceStatus.NotInitialized;
        }
        abstract protected bool InitializeDevice();
        abstract protected bool ConnectToDevice();
        abstract protected bool DisconnectFromDevice();
        abstract protected bool EnableDevice();
        abstract protected bool DisableDevice();
        abstract protected bool ResetHardware();
        abstract protected bool CheckHealth(out int code, out string status);
        abstract protected bool RunDiagnosticTests(out List<string> symptomsCodes, string deviceInfo);
        abstract protected bool TerminateDevice();
        #endregion

        #region Events
        public event EventHandler DeviceErrorOccurred;
        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;

        protected virtual void OnDeviceErrorOccurred(EventArgs e)
        {
            EventHandler handler = DeviceErrorOccurred;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        protected virtual void OnDeviceConnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Connected;
            EventHandler handler = DeviceErrorOccurred;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        protected virtual void OnDeviceDisconnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Disconnected;
            EventHandler handler = DeviceErrorOccurred;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion


    }
}
