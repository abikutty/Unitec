using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitec.Middleware.Contracts
{
    public delegate void DeviceErrorEventHandler(object sender, DeviceErrorEventArgs e);

    public enum DeviceStatus
    {
        NotInitialized,
        Initialized,
        Enabled,
        Connected,
        Disconnected
    }

    public enum DeviceErrors
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
        Track1ReadError = 101 ,
        [Description("Track 2 read error")]
        Track2ReadError = 102,
        [Description("Track 3 read error")]
        Track3ReadError = 103,

    }

    public class DeviceErrorEventArgs : EventArgs
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public static class GenericDeviceHelper
    {
        public static string GetDescription(this DeviceErrors error)
        {
            var type = typeof(DeviceErrors);
            var field = type.GetField(error.ToString());

            var attribute = (DescriptionAttribute) Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (attribute != null)
            {
                return attribute.Description;
            }
            return error.ToString();
        }
    }
       


}
