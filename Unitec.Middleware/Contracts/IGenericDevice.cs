using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Helpers;

namespace Unitec.Middleware.Contracts
{

    public interface IGenericDevice : IDisposable
    {
        #region Properties
        string LogFile { get; set; }
        string PeripheralsConfigFile { get; set; }
        bool IsConnected { get;  }
        bool IsEnabled { get; }
        string FirmwareVersion { get; }
        #endregion

        #region Events
        event DeviceErrorEventHandler DeviceErrorOccurred;
        event EventHandler DeviceConnected;
        event EventHandler DeviceDisconnected;
        //Added to see the logs in the Test harness
        event DataLoggedEventHandler DataLogged;
        #endregion
        bool InitializeDevice();
        bool ConnectToDevice();
        bool DisconnectFromDevice();
        bool EnableDevice();
        bool DisableDevice();
        bool ResetHardware();
        bool CheckHealth(out int code, out string status, out string hardwareIdentity, out string report);
        bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo);
        bool TerminateDevice();
    }
}
