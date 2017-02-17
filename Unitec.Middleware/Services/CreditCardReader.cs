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
        private readonly byte[] CmdResetDefault = new byte[] { 0x60, 0x00, 0x02, 0x53, 0x18, 0x29, 0x03 };
        private readonly byte[] CmdGetFirmwareVer = new byte[] { 0x60, 0x00, 0x01, 0x39, 0x58, 0x03 };
        private readonly byte[] CmdResetReader = new byte[] { 0x60, 0x00, 0x01, 0x49, 0x28, 0x03 };
        private readonly byte[] CmdRestoreSettings = new byte[] { 0x60, 0x00, 0x02, 0x53, 0x18, 0x29,0x03};
        private readonly byte[] CmdReadAllConfig = new byte[] { 0x60, 0x00, 0x02, 0x52, 0x1F, 0x2F, 0x03 };
        private readonly byte[] CmdReadSerialNumber = new byte[] { 0x60, 0x00, 0x02, 0x52, 0x4E, 0x7E, 0x03 };
        private readonly byte[] CmdReadyToRead = new byte[] { 0x60, 0x00, 0x03, 0x50, 0x01, 0x30, 0x02,0x03 };
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



        #region Event
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
        #endregion

        #region implementation of Generic Device


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
        
        public override bool ResetHardware()
        {
            try
            {
                "Attempting to Reset Device...".Log(LogFile);
                WriteCommand(CmdResetDefault);
                var message = ReadResponse();
                if (message[3] == 0x90 && message[4] == 0x00)
                {
                    "Successfully Reset the Device...".Log(LogFile);
                    return true;
                }
                return DisconnectFromDevice();
            }
            catch (Exception ex)
            {
                HandleException(ex, DeviceErrorType.UnableToClosePort);
                return false;
            }
            return false;
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

        public override bool CheckHealth(out int code, out string status, out string hardwareIdentity, out string report)
        {
            throw new NotImplementedException();
        }

        public override bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo)
        {
            if (!DisconnectFromDevice())
                throw new InvalidOperationException("unable to disconnect the device.");
            return base.RunDiagnosticTests(out symptomsCodes, out deviceInfo);
        }
        #endregion
    }
}
