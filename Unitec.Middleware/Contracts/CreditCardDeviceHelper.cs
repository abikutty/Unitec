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

    public enum LogType
    {
        Error = 1,
        Status = 2,
        Warning = 3
    }

    public static class CreditCardDeviceHelper
     {

        
         public static DeviceErrorEventArgs Create(this Exception e, DeviceErrors error, string logPath)
        {
            var deviceEventArg = new DeviceErrorEventArgs();
            deviceEventArg.Code = (int)error;
            deviceEventArg.Description = String.Format("Error {0} - Underlying Exception {1}", error.GetDescription(), e.Message);
            deviceEventArg.Description.Log(logPath, LogType.Error);
            return deviceEventArg;
        }

        public static void Log(this Exception e, string filePath)
        {
            var exDetails = String.Format("Exceptin {0} at Trace {1}", e.Message, e.StackTrace);
            exDetails.Log(filePath,LogType.Error);
        }


        public static void Log(this string logMessage, string filePath, LogType logType = LogType.Status)
        {
            using (StreamWriter w = File.AppendText(filePath))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1} : {2}:{3}", DateTime.Now.ToLongTimeString(),    DateTime.Now.ToLongDateString(), logType.ToString(), logMessage);
            }
        }

    }


}
