using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitec.Middleware.Contracts
{
    public delegate void CardDataObtainedEventHandler(object sender, CardDataObtainedEventArgs e);

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
