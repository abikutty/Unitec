using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;

namespace Unitec.Middleware.Services
{
    public class PeripheralConnection
    {
        public string  ComPort { get; private set; }
        public int BaudRate { get; private set; }
        public Parity Parity { get; private set; }
        public int DataBits { get; private set; }
        public StopBits StopBits { get; private set; }

        private string peripheralsConfigFile = "";
        public PeripheralConnection(string file )
        {
            peripheralsConfigFile = file;
            //TODO:- Read Config file to populate the fields

            var configFile = ConfigurationManager.OpenExeConfiguration(file);
            var settings = configFile.AppSettings.Settings;

            ComPort = "";
            BaudRate = 38400;
            Parity = Parity.None;
            DataBits = 8;
            StopBits = StopBits.One;
        }
    }
    public abstract class GenericDevice : IGenericDevice
    {

        #region Properties
        private DeviceStatus currentStatus;
        private string firmwareVersion;
        public string LogFile { get; set; }
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

        protected virtual void HandleException(Exception ex, DeviceErrors error)
        {
            if (ex != null)
            {
                var errorArg = ex.Create(error);
                OnDeviceErrorOccurred(errorArg);

            }
        }


        protected virtual void WriteLog(Exception ex)
        {

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
                handler(this, e);
            }
        }


        protected virtual void OnDeviceConnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Connected;
            EventHandler handler = DeviceConnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        protected virtual void OnDeviceDisconnected(EventArgs e)
        {
            currentStatus = DeviceStatus.Disconnected;
            EventHandler handler = DeviceDisconnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion


    }
}
