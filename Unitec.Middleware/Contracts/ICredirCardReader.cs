using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Helpers;

namespace Unitec.Middleware.Contracts
{
    public interface ICreditCardReader : IGenericDevice
    {
        event CardDataObtainedEventHandler CardDataObtained;
        event EventHandler CardInserted;
        event EventHandler CardInsertTimeout;
        event DeviceErrorEventHandler CardReadFailure;
        //Added to wait for card events in a separate thread
        Task SetReaderReady();
    }
}
