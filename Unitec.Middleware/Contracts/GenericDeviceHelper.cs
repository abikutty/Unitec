using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitec.Middleware.Contracts
{

    public delegate void DeviceErrorEventHandler(object sender, DeviceErrorEventArgs e);


    public enum ReadStatus
    {
        Failed = 0,
        Timeout = 1,
        Success = 2
    }

    public enum LogType
    {
        Error = 1,
        Status = 2,
        Warning = 3
    }


    public enum DeviceStatus
    {
        NotInitialized,
        Initialized,
        Enabled,
        Disabled,
        Connected,
        Disconnected
    }

    public class DeviceError
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }


    public enum DeviceErrorType
    {
        [Description("Unexpected Error")]
        UnexpectedError = 0,
        [Description("Could not find any device")]
        CouldNotFind = 1,
        [Description("Device creation failed")]
        CreationFailed = 2,
        [Description("Device configuration file does not exist")]
        ConfigFileMissing = 3,
        [Description("Failed to connect device")]
        ConnectionFailed = 4,
        [Description("Device is not connected")]
        NotConnected = 5,
        [Description("Device is not enabled")]
        NotEnabled = 6,
        [Description("Device is not initialized")]
        NotInitialized = 7,
        [Description("Error under device_name in Peripherals.XML file")]
        ErrorDeviceName = 8,
        [Description("Unable to close COM port")]
        UnableToClosePort = 9,
        [Description("Expected response not received")]
        ExpectedResponseFailed = 10,
        [Description("COM port is not open to send data")]
        ComPortNotOpened = 12,
        [Description("Access to the COM port is denied")]
        PortAccessDenied = 13,
        [Description("Error occurred in receiving data")]
        ErrorReceving = 14,
        [Description("Error occurred in sending data")]
        ErrorSending = 15,
        [Description("Device initialization failed")]
        DeviceInitFailed = 16,
        [Description("The specified log file path or name is invalid")]
        InvalidLogPath = 90,
        [Description("Cannot complete save the log file due to a file permission error")]
        SaveLogPermissionError = 91,
        [Description("Does not have permission to write the log file")]
        WriteLogPermissionError = 92,
        [Description("I/O error occurred while writing to the log file")]
        LogIOError = 93,
        [Description("Track 1 read error")]
        Track1ReadError = 101,
        [Description("Track 2 read error")]
        Track2ReadError = 102,
        [Description("Track 3 read error")]
        Track3ReadError = 103,

    }

    public class DeviceErrorEventArgs : EventArgs
    {
        public List<DeviceError> DeviceErrors { get; set; }
    }

    public static class GenericDeviceHelper
    {

        public static DeviceErrorEventArgs Create(this Exception e, DeviceErrorType error, string logPath)
        {
            var deviceEventArg = new DeviceErrorEventArgs();
            deviceEventArg.DeviceErrors = new List<DeviceError>();
            deviceEventArg.DeviceErrors.Add(new DeviceError { Code = (int)error, Description = error.GetDescription() });
            while (e != null)
            {
                deviceEventArg.DeviceErrors.Add(new DeviceError { Code = e.HResult, Description = String.Format("Source: {0} Message: {1}", e.Source, e.Message) });
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
                    var exDetails = String.Format("Exceptin Code {0} Description {1}", x.Code, x.Description);
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
                w.WriteLine("{0} {1} : {2}:{3}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), logType.ToString(), logMessage);
            }
        }


        public static string GetDescription(this DeviceErrorType error)
        {
            var type = typeof(DeviceErrorType);
            var field = type.GetField(error.ToString());

            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (attribute != null)
            {
                return attribute.Description;
            }
            return error.ToString();
        }

    }

    public class PeripheralConnection
    {
        public string ComPort { get; private set; }
        public int BaudRate { get; private set; }
        public Parity Parity { get; private set; }
        public int DataBits { get; private set; }
        public StopBits StopBits { get; private set; }

        private string peripheralsConfigFile = "";
        public PeripheralConnection(string file)
        {
            peripheralsConfigFile = file;
            //TODO:- Read Config file to populate the fields
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = file;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;
            ComPort = settings["ComPort"].Value ?? "COM3";
            int temp = 0;
            BaudRate = int.TryParse(settings["BaudRate"].Value, out temp) ? temp : 38400;
            Parity = (Parity)(int.TryParse(settings["Parity"].Value, out temp) ? temp : 0);
            DataBits = int.TryParse(settings["DataBits"].Value, out temp) ? temp : 8;
            StopBits = (StopBits)(int.TryParse(settings["StopBits"].Value, out temp) ? temp : 1);
        }
    }

}
