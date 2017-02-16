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

        
        public static DeviceErrorEventArgs Create(this Exception e, DeviceErrorType error, string logPath)
        {
            var deviceEventArg = new DeviceErrorEventArgs();
            deviceEventArg.DeviceErrors = new List<DeviceError>();
            deviceEventArg.DeviceErrors.Add(new DeviceError { Code = (int)error, Description = error.GetDescription() } );
            while(e != null)
            {
                deviceEventArg.DeviceErrors.Add(new DeviceError { Code = e.HResult, Description = String.Format("Source: {0} Message: {1}", e.Source,e.Message)});
                e = e.InnerException;
            }
            deviceEventArg.DeviceErrors.Log(logPath);
            return deviceEventArg;
        }

        public static void Log(this List<DeviceError> errors, string filePath)
        {
            if (errors != null && errors.Count > 0)
            {
                errors.ForEach(x =>
                {
                    var exDetails = String.Format("Exceptin Code {0} Description {1}",x.Code,x.Description);
                    exDetails.Log(filePath, LogType.Error);
                });
            }
        }


        public static void Log(this Exception e, string filePath)
        {
            var exDetails = String.Format("Exceptin {0} at Trace {1}", e.Message, e.StackTrace);
            exDetails.Log(filePath, LogType.Error);
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
