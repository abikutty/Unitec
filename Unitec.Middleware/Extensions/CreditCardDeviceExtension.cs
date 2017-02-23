using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitec.Middleware.Helpers
{
    public delegate void CardDataObtainedEventHandler(object sender, CardDataObtainedEventArgs e);

    public enum ResponseStatus
    {
       ACK = 6,
       NAK = 21,

    }
    public class CardDataObtainedEventArgs : EventArgs
    {
        public string Track1Data { get; set; }
        public string Track2Data { get; set; }
        public string Track3Data { get; set; }
    }

    public class CardReadFailureEventArgs : DeviceErrorEventArgs
    {

    }

}
