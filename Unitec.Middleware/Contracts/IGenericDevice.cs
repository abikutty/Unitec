using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitec.Middleware.Contracts
{

    interface IGenericDevice
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
        #endregion
    }
}
