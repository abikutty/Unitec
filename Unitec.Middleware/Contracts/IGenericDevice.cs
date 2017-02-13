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
        bool IsConnected { get; set; }
        bool IsEnabled { get; set; }
        string FirmwareVersion { get; set; }
        #endregion

        #region Events
        event EventHandler DeviceErrorOccurred;
        event EventHandler DeviceConnected;
        event EventHandler DeviceDisconnected;
        #endregion
    }
}
