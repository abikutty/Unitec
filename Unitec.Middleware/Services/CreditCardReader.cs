using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;
using Unitec.Middleware.Services;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace Unitec.Middleware
{
    public class CreditCardReader : GenericDevice, ICredirCardReader
    {

        #region Properties
        //private readonly UInt64 CmdResetDefault = 0x60000253182903;
        //private readonly UInt64 CmdGetFirmwareVer = 0x600001395803;
        //private readonly UInt64 CmdResetReader = 0x600001492803;
        //private readonly UInt64 CmdRestoreSettings = 0x60000253182903;
        //private readonly UInt64 CmdReadAllConfig = 0x600002521F2F03;
        //private readonly UInt64 CmdReadSerialNumber = 0x600002524E7E03;
        //private readonly UInt64 CmdReadyToRead = 0x6000035001300203;

        private readonly byte[] CmdResetDefault = new byte[] { 0x60, 0x00, 0x02, 0x53, 0x18, 0x29, 0x03 };
        private readonly byte[] CmdGetFirmwareVer = new byte[] { 0x60, 0x00, 0x01, 0x39, 0x58, 0x03 };
        private readonly byte[] CmdResetReader = new byte[] { 0x60, 0x00, 0x01, 0x49, 0x28, 0x03 };
        private readonly byte[] CmdRestoreSettings = new byte[] { 0x60, 0x00, 0x02, 0x53, 0x18, 0x29,0x03};
        private readonly byte[] CmdReadAllConfig = new byte[] { 0x60, 0x00, 0x02, 0x52, 0x1F, 0x2F, 0x03 };
        private readonly byte[] CmdReadSerialNumber = new byte[] { 0x60, 0x00, 0x02, 0x52, 0x4E, 0x7E, 0x03 };
        private readonly byte[] CmdReaderStatus = new byte[] { 0x60, 0x00, 0x01, 0x24, 0x45, 0x03 };


        private readonly byte[] CmdBfrModeReadyToRead = new byte[] { 0x60, 0x00, 0x03, 0x50, 0x01, 0x30, 0x02,0x03 };
        private readonly byte[] CmdBfrModeReset = new byte[] { 0x60, 0x00, 0x03, 0x50, 0x01, 0x32, 0x02, 0x03 };
        private readonly byte[] CmdBfrModeRead = new byte[] { 0x60, 0x00, 0x03, 0x51, 0x01, 0x32, 0x02, 0x03 };
        //Error 2 byte data after header
        //6912 'P' command length must be 1
        //6916 'P' command data must be 0x30 or 0x32
        //6920 Reader not configured for buffered mode
        //6922 Reader not configured for magstripe read
        //Page 29
        private readonly byte[] ResponseSuccessMask = new byte[] { 0x90, 0x00 };

        private const int timeOut = 5000;
        public event CardDataObtainedEventHandler CardDataObtained;
        public event EventHandler CardInserted;
        public event EventHandler CardInsertTimeout;
        public event DeviceErrorEventHandler CardReadFailure;


        #endregion
        
        #region implementation of Generic Device
        public override bool InitializeDevice()
        {
            try
            {
                return base.InitializeDevice();
            }
            catch (Exception ex)
            {
                HandleException(ex, DeviceErrorType.DeviceInitFailed);
                return false;
            }

        }

        public override bool ConnectToDevice()
        {
            try
            {
               return base.ConnectToDevice();
            }
            catch (Exception e)
            {
                HandleException(e, DeviceErrorType.ConnectionFailed);
            }
            return false;
        }

        public override bool DisconnectFromDevice()
        {
            try
            {
                return base.DisconnectFromDevice();
            }
            catch (Exception e)
            {
                HandleException(e, DeviceErrorType.UnableToClosePort);
            }
            return false;
        }

        public override bool EnableDevice()
        {
            try
            {
                return base.EnableDevice();
            }
            catch (Exception e)
            {
                HandleException(e, DeviceErrorType.NotEnabled);
            }
            return false;
        }

        public override bool DisableDevice()
        {
            try
            {
                return base.DisableDevice();
            }
            catch (Exception e)
            {
                HandleException(e, DeviceErrorType.UnableToClosePort);
            }
            return false;
        }
       
        public override bool ResetHardware()
        {
            bool isConnected = IsConnected;
            bool isEnabled = IsEnabled;
            try
            {
                "Attempting to Reset Device...".Log(LogFile);
                WriteCommand(CmdResetDefault);
                var message = ReadResponse(5);
                if (message[3] == 0x90 && message[4] == 0x00)
                {
                    "Successfully Reset the Device...".Log(LogFile);
                    return true;
                }
               
            }
            catch (Exception ex)
            {
                
                HandleException(ex, DeviceErrorType.UnableToClosePort);
                var resp = DisableDevice();
                resp = resp && DisconnectFromDevice();
                resp = isConnected && ConnectToDevice();
                resp = isEnabled && EnableDevice();
            }
            return false;
        }

        public override bool CheckHealth(out int code, out string status, out string hardwareIdentity, out string report)
        {
            code = 0;
            status = "";
            hardwareIdentity = "";
            report = "";
            try
            {
                "Attempting to check device health...".Log(LogFile);
                if(!IsConnected)
                {
                    "Device not connected to check health...".Log(LogFile);
                    return false;
                }
                if (!IsEnabled)
                {
                    "Device not enabled to check health...".Log(LogFile);
                    return false;
                }
                GetReaderStatus(out code, out status);
                hardwareIdentity = GetReaderIndentity();
                report = GetReaderReport();
                return true;
   
            }
            catch (Exception ex)
            {
                HandleException(ex, DeviceErrorType.UnableToClosePort);
            }
            return false;
        }


        private string GetReaderIndentity()
        {
            "Attempting to read hardware information...".Log(LogFile);
            WriteCommand(CmdGetFirmwareVer);
            var message = ReadResponse();
            if (message[3] == 0x90 && message[4] == 0x00)
            {
                "Successfully Reset the Device...".Log(LogFile);
            }
        }

        private void GetReaderStatus(out int code, out string status)
        {
            code = 0;
            status = "";
            WriteCommand(CmdReaderStatus);
            var message = ReadResponse(5);
            code = message[3] & 0x0F;
            status = (code & 1) == 1 ? "No data in a reader" : "Others";
            status = (code & 2) == 1 ? "Card seated" : "Card not seated";
            status = (code & 4) == 1 ? "Media detected" : "Others";
            status = (code & 8) == 1 ? "Card present" : "Card not present";
            status = (code & 16) == 1 ? "Magnetic data present" : "No magnetic data";
            status = (code & 32) == 1 ? "Card in Slot" : "All other conditions";
            status = (code & 64) == 1 ? "Incomplete Insertion" : "All other conditions";
            if (String.IsNullOrEmpty(status))
            {
                status = "enexpected status";
            }
        }


        private string GetReaderReport()
        {
            return "";
        }


        public override bool TerminateDevice()
        {
            try
            {

                return true;
            }
            catch (Exception e)
            {
                HandleException(e, DeviceErrorType.UnableToClosePort);
            }
            return false;
        }

        public override bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo)
        {
          //  if (!DisconnectFromDevice())
                throw new InvalidOperationException("unable to disconnect the device.");
           // return base.RunDiagnosticTests(out symptomsCodes, out deviceInfo);
        }
        #endregion

        #region Events
        protected virtual void OnCardDataObtained(CardDataObtainedEventArgs e)
        {
            CardDataObtainedEventHandler handler = CardDataObtained;
            if (handler != null)
            {
                "credit card data obtained".Log(LogFile);
                handler(this, e);
            }
        }
        protected virtual void OnCardInserted(EventArgs e)
        {
            EventHandler handler = CardInserted;
            if (handler != null)
            {
                "credit card inserted".Log(LogFile);
                handler(this, e);
            }
        }
        protected virtual void OnCardInsertTimeout(EventArgs e)
        {
            EventHandler handler = CardInsertTimeout;
            if (handler != null)
            {
                "credit card insert timeout".Log(LogFile);
                handler(this, e);
            }
        }
        protected virtual void OnCardReadFailure(CardReadFailureEventArgs e)
        {
            DeviceErrorEventHandler handler = CardReadFailure;
            if (handler != null)
            {
                "credit card read failed".Log(LogFile);
                handler(this, e);
            }
        }

        protected override void OnDeviceErrorOccurred(DeviceErrorEventArgs e)
        {

            base.OnDeviceErrorOccurred(e);
        }


        protected override void OnDeviceConnected(EventArgs e)
        {
            base.OnDeviceConnected(e);
        }


        protected override void OnDeviceDisconnected(EventArgs e)
        {
            base.OnDeviceDisconnected(e);
        }

        #endregion
    }
}
