using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;
using Unitec.Middleware.Devices;
using System.IO.Ports;
using System.IO;
using System.Threading;
using Unitec.Middleware.Helpers;

namespace Unitec.Middleware.Devices
{
    public class CreditCardReader : GenericDevice, ICreditCardReader
    {

        #region Properties
        private readonly byte[] CmdResetDefault = new byte[] { 0x60, 0x00, 0x02, 0x53, 0x18, 0x29, 0x03 };
        private readonly byte[] CmdGetFirmwareVer = new byte[] { 0x60, 0x00, 0x01, 0x39, 0x58, 0x03 };
        private readonly byte[] CmdResetReader = new byte[] { 0x60, 0x00, 0x01, 0x49, 0x28, 0x03 };
        private readonly byte[] CmdRestoreSettings = new byte[] { 0x60, 0x00, 0x02, 0x53, 0x18, 0x29,0x03};
        private readonly byte[] CmdReadAllConfig = new byte[] { 0x60, 0x00, 0x02, 0x52, 0x1F, 0x2F, 0x03 };
        private readonly byte[] CmdReadSerialNumber = new byte[] { 0x60, 0x00, 0x02, 0x52, 0x4E, 0x7E, 0x03 };
        private readonly byte[] CmdReaderStatus = new byte[] { 0x60, 0x00, 0x01, 0x24, 0x45, 0x03 };
        private readonly byte[] CmdSetReadyToRead = new byte[] { 0x60, 0x00, 0x03, 0x50, 0x01, 0x30, 0x02, 0x03 };

        private readonly byte[] CmdReaderOptionMask = new byte[] { 0x60, 0x00, 0x04, 0x53, 0x11, 0x01, 0xFF, 0XFF, 0x03 };
        private readonly byte[] CmdReaderOption2Mask = new byte[] { 0x60, 0x00, 0x04, 0x53, 0x2F, 0x01,0xFF,0XFF,0x03 };
        private readonly byte[] CmdSetTrackSelectionMask = new byte[] { 0x60, 0x00, 0x04, 0x53, 0x13, 0x01, 0xFF, 0xFF, 0x03 };
        private readonly byte[] CmdSetTransmitModeMask = new byte[] { 0x60, 0x00, 0x04, 0x53, 0x1A, 0x01, 0xFF, 0xFF, 0x03};
        private readonly byte[] CmdSetReadDirectionMask = new byte[] { 0x60, 0x00, 0x04, 0x53, 0x1D, 0x01, 0xFF, 0xFF, 0x03 };
        private readonly byte[] CmdSetSendOptionMask = new byte[] { 0x60, 0x00, 0x04, 0x53, 0x19, 0x01, 0xFF, 0xFF, 0x03 };
        private readonly byte[] CmdSetTrackSeparatorMask = new byte[] { 0x60, 0x00, 0x04, 0x53, 0x17, 0x01, 0xFF, 0xFF, 0x03 };
        private readonly byte[] CmdReadMSRDataMask = new byte[] { 0x60, 0x00, 0x03, 0x51, 0x01, 0xFF, 0xFF, 0x03 };
        
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
            bool resp = false;
            try
            {
                resp = base.ConnectToDevice();
                if(resp)
                {
                    if(String.IsNullOrEmpty(FirmwareVersion))
                    {
                        FirmwareVersion = GetReaderIndentity();
                    }
                    
                }
            }
            catch (Exception e)
            {
                HandleException(e, DeviceErrorType.ConnectionFailed);
            }
            return resp;
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
                this.Log("Attempting to Reset Device...");
                WriteCommand(CmdResetDefault);
                var message = ReadResponse();
                if (message[3] == 0x90 && message[4] == 0x00)
                {
                    this.Log("Successfully Reset the Device...");
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
                this.Log("Attempting to check device health...");
                if(!IsConnected)
                {
                    this.Log("Device not connected to check health...");
                    return false;
                }
                if (!IsEnabled)
                {
                    this.Log("Device not enabled to check health...");
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


        public override bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo)
        {
           symptomsCodes = new List<string>();
            deviceInfo = "NO_INFO";
            deviceInfo = "";
            try
            {
                this.Log("Attempting to run diagnostic...");
                if (!IsConnected)
                {
                    this.Log("Device not connected to run diagnostic...");
                    return false;
                }
                if (!IsEnabled)
                {
                    this.Log("Device not enabled to run diagnostic..");
                    return false;
                }
                var info = GetReaderDeviceInformation();
                deviceInfo = !String.IsNullOrEmpty(info) ? info : "NO_INFO";
                //Set MSR Transmit Modes
                symptomsCodes.Add(RunSettingCommandMask("Set MSR Transmit Mode disabled", CmdSetTransmitModeMask, 0x00, 6, 7)
                    ? "Set MSR Transmit Mode disabled - failed" : "Set MSR Transmit Mode disabled - succeded");

                symptomsCodes.Add(RunSettingCommandMask("MSR Reading Auto Transmit Mode", CmdSetTransmitModeMask, 0x01, 6, 7)
                ? "MSR Reading Auto Transmit Mode - failed" : "MSR Reading Auto Transmit Mode - succeded");

                symptomsCodes.Add(RunSettingCommandMask("MSR Reading in Buffered Mode", CmdSetTransmitModeMask, 0x02, 6, 7)
                ? "MSR Reading in Buffered Mode - failed" : "MSR Reading in Buffered Mode - succeded");

                //Set MSR Read Directions

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Read Direction both insertion and withdrawal", CmdSetReadDirectionMask, 0x01, 5, 6)
                ? "Set MSR Read Direction - both insertion and withdrawal failed" : "Set MSR Read Direction both insertion and withdrawal succeded");

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Read Direction Read on insertion only", CmdSetReadDirectionMask, 0x02, 6, 7)
                ? "Set MSR Read Direction Read on insertion only failed" : "Set MSR Read Direction Read on insertion only succeded");

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Read Direction Report on withdrawal", CmdSetReadDirectionMask, 0x03, 6, 7)
                 ? "Set MSR Read Direction Report on withdrawal failed" : "Set MSR Read Direction Report on withdrawal succeded");
                
                symptomsCodes.Add(RunSettingCommandMask("Set MSR Read Direction Read on withdrawal only", CmdSetReadDirectionMask, 0x04, 6, 7)
                 ? "Set MSR Read Direction Read on withdrawal only failed" : "Set MSR Read Direction Read on withdrawal only succeded");

                //Set MSR Send Option

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option No Start/End Sentinel, all data,no error", CmdSetSendOptionMask, 0x1E, 5, 6)
                ? "Set MSR Send Option No Start/End Sentinel, all data,no error failed" : "Set MSR Send Option No Start/End Sentinel, all data,no error succeded");

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option Start/End Sentinel, all data, no error", CmdSetSendOptionMask, 0x1F, 5, 6)
                ? "Set MSR Send Option Start/End Sentinel, all data, no error failed" : "Set MSR Send Option Start/End Sentinel, all data, no error succeded");

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option No Start/End Sentinel, account on t2, no error", CmdSetSendOptionMask, 0x20, 5, 6)
                ? "Set MSR Send Option No Start/End Sentinel, account on t2, no error failed" : "Set MSR Send Option No Start/End Sentinel, account on t2, no error succeded");

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option Start/End Sentinel t1/t2 for credit card and t1/t3 for other card, no error", CmdSetSendOptionMask, 0x21, 5, 6)
                ? "Set MSR Send Option Start / End Sentinel t1 / t2 for credit card and t1 / t3 for other card, no error failed" : "Set MSR Send Option Start/End Sentinel t1/t2 for credit card and t1/t3 for other card, no error succeded");
                
                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option No Start/End Sentinel, all data, error", CmdSetSendOptionMask, 0x22, 5, 6)
                ? "Set MSR Send Option No Start/End Sentinel, all data, error failed" : "Set MSR Send Option No Start / End Sentinel, all data, error succeded");

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option Start/End Sentinel, all data, error", CmdSetSendOptionMask, 0x23, 5, 6)
                ? "Set MSR Send Option Start/End Sentinel, all data, error failed" : "Set MSR Send Option Start/End Sentinel, all data, error succeded");
                
                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option No Start/End Sentinel, account on t2, error", CmdSetSendOptionMask, 0x24, 5, 6)
                ? "Set MSR Send Option No Start / End Sentinel, account on t2, error failed" : "Set MSR Send Option No Start / End Sentinel, account on t2, error succeded");

                symptomsCodes.Add(RunSettingCommandMask("Set MSR Send Option tart/End Sentinel, account on t2 for credit, t1/t3 for other, error", CmdSetSendOptionMask, 0x25, 5, 6)
                ? "Set MSR Send Option tart/End Sentinel, account on t2 for credit, t1/t3 for other, error failed" : "Set MSR Send Option tart/End Sentinel, account on t2 for credit, t1/t3 for other, error succeded");

                return true;

            }
            catch (Exception ex)
            {
                HandleException(ex, DeviceErrorType.UnableToClosePort);
            }
            return false;
        }





        #endregion

        #region Events
        protected virtual void OnCardDataObtained(CardDataObtainedEventArgs e)
        {
            CardDataObtainedEventHandler handler = CardDataObtained;
            if (handler != null)
            {
                this.Log("credit card data obtained");
                handler(this, e);
            }
        }
        protected virtual void OnCardInserted(EventArgs e)
        {
            EventHandler handler = CardInserted;
            if (handler != null)
            {
                this.Log("credit card inserted");
                handler(this, e);
            }
        }
        protected virtual void OnCardInsertTimeout(EventArgs e)
        {
            EventHandler handler = CardInsertTimeout;
            if (handler != null)
            {
                this.Log("credit card insert timeout");
                handler(this, e);
            }
        }
        protected virtual void OnCardReadFailure(CardReadFailureEventArgs e)
        {
            DeviceErrorEventHandler handler = CardReadFailure;
            if (handler != null)
            {
                this.Log("credit card read failed");
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

        #region Helper Methods

        private string GetReaderIndentity()
        {
            this.Log("Attempting to read hardware information...");
            WriteCommand(CmdGetFirmwareVer);
            var message = ReadResponse();
            var length = message[2] - message[1];
            if (length <= 0)
            {
                this.Log("failed to read hardware information...");
                return "";
            }
            var response = BitConverter.ToString(message, 3, length);
            return response;
        }

        private void GetReaderStatus(out int code, out string status)
        {
            code = 0;
            status = "";
            WriteCommand(CmdReaderStatus);
            var message = ReadResponse();
            code = message[3];
            status = (code & 1) == 1 ? "No data in a reader" : "Others";
            status = (code & 2) == 1 ? "Card seated" : "Card not seated";
            status = (code & 4) == 1 ? "Media detected" : "Others";
            status = (code & 8) == 1 ? "Card present" : "Card not present";
            status = (code & 16) == 1 ? "Magnetic data present" : "No magnetic data";
            status = (code & 32) == 1 ? "Card in Slot" : "All other conditions";
            status = (code & 64) == 1 ? "Incomplete Insertion" : "All other conditions";
            if (String.IsNullOrEmpty(status))
            {
                status = "Unexpected status";
            }
        }

        public override bool TerminateDevice()
        {
            try
            {
                this.Log("Attempting to Reset Device...");
                if (!IsDisconnected)
                {
                    this.Log("Device is not disconnected");
                    return false;
                }
                WriteCommand(CmdResetReader);
                var message = ReadResponse();
                if (message[3] == 0x90 && message[4] == 0x00)
                {
                    this.Log("Successfully Reset the Device...");
                    DeviceContainer.DisposeDevice(this);
                    return true;
                }

            }
            catch (Exception ex)
            {

                HandleException(ex, DeviceErrorType.UnableToClosePort);
                var resp = DisableDevice();
                resp = resp && DisconnectFromDevice();
            }
            return false;
        }

        private bool RunSettingCommandMask(string commandName, byte[] command, byte commandID, int cmdOffset, int lrcOffset)
        {
            this.Log(String.Format("Attempting to set {0}...", commandName));
            command[cmdOffset] = (byte)(command[cmdOffset] & commandID);
            byte lrc = 0x00;
            for (int i = 0; i < lrcOffset; i++)
            {
                lrc = (byte)(lrc ^ command[i]);
            }
            command[lrcOffset] = lrc;
            WriteCommand(command);
            var response = ReadResponse();
            if (response[3] == 0x90 && response[4] == 0x00)
            {
                this.Log(String.Format("Successfully executed {0}", commandName));
                return true;
            }
            else
            {
                this.Log(String.Format("Failed running {0}", commandName));
            }
            return false;
        }


        private byte[] RunCommandMask(string commandName, byte[] command, byte commandID, int cmdOffset, int lrcOffset)
        {
            this.Log(String.Format("Attempting to set {0}...", commandName));
            command[cmdOffset] = (byte)(command[2] & commandID);
            byte lrc = 0x00;
            for (int i = 0; i < lrcOffset; i++)
            {
                lrc = (byte)(lrc ^ command[i]);
            }
            command[lrcOffset] = lrc;
            WriteCommand(command);
            return ReadResponse();
        }

        private string GetReaderDeviceInformation()
        {
            this.Log("Attempting to read device information...");
            WriteCommand(CmdReadAllConfig);
            var message = ReadResponse();
            var length = message[2] - message[1];
            if (length <= 0)
            {
                this.Log("Failed to read device information...");
                return "";
            }
            var response = BitConverter.ToString(message, 3, length);
            return response;
        }

        private string GetReaderReport()
        {
            return "";
        }


        public Task SetReaderReady()
        {

            return Task.Run(new Action(() =>
            {
                this.Log("Attempting set the reader...");

                if (!IsConnected || !IsEnabled)
                {
                    this.Log("Reader is not connected and/or enabled...");
                    return;
                }
                int code;
                string status;

                if (RunSettingCommandMask("Set reader option 2", CmdReaderOption2Mask, 0X1F, 6, 7))
                {
                    GetReaderStatus(out code, out status);
                    if ((code & 32) == 1)
                        OnCardInserted(new EventArgs());
                    if ((code & 64) == 0)
                        OnCardInsertTimeout(new EventArgs());
                }

                if (!RunSettingCommandMask("Set Tracks 1,2 and 3", CmdSetTrackSelectionMask, 0X07, 6, 7))
                {
                    this.Log("Failed to set tracks...");
                    return;
                }

                if (!RunSettingCommandMask("Set to buffered mode", CmdSetTransmitModeMask, 0X02, 6, 7))
                {
                    this.Log("Failed to set buffered mode...");
                    return;
                }

                if (!RunSettingCommandMask("Set reader direction", CmdSetReadDirectionMask, 0X01, 6, 7))
                {
                    this.Log("Failed to set reader direction...");
                    return;
                }
                if (!RunSettingCommandMask("Set MSR Send Option Start/End Sentinel t1/t2 for credit card and t1/t3 for other card, no error", CmdSetSendOptionMask, 0x21, 5, 6))
                {
                    this.Log("Failed to set the read mode...");
                    return;
                }

                if (!RunSettingCommandMask("Set | as track separator ", CmdSetTrackSeparatorMask, 0XA6, 6, 7))
                {
                    this.Log("Failed to set | as track separator...");
                    return;
                }
                AttempToRead();

            }));
        }

        private void AttempToRead()
        {
            int code;
            string status;
            CardReadFailureEventArgs failEventArgs = null;
            CardDataObtainedEventArgs successEventArgs = null;
            this.Log("Setting the host to wait...");
            WriteCommand(CmdSetReadyToRead);
            var response = ReadResponse();
            if (response[0] == 0xE0)
            {
                code = BitConverter.ToInt16(response, 3);
                switch (code)
                {
                    case 0x6912:
                        status = "'P' command length must be 1";
                        break;
                    case 0x6916:
                        status = "'P' command data must be 0x30 or 0x32";
                        break;
                    case 0x6920:
                        status = "Reader not configured for buffered mode";
                        break;
                    case 0x6922:
                        status = "Reader not configured for magstripe read";
                        break;
                    default:
                        status = "Unknow error while setting to wait";
                        break;
                }
                failEventArgs = new CardReadFailureEventArgs() { DeviceErrors = new List<DeviceError>() };
                failEventArgs.DeviceErrors.Add(new DeviceError() { Code = code, Description = status });
            }

            response = RunCommandMask("Read tracks 1,2 and 3", CmdReadMSRDataMask, 0X07, 5, 6);

            if (response[0] == 0xE0)
            {
                code = BitConverter.ToInt16(response, 3);
                switch (code)
                {
                    case 0x6911:
                        status = "'Q' command length must be 1";
                        break;
                    case 0x6921:
                        status = "reader not configured for buffered mode";
                        break;
                    case 0xC000:
                        status = "no magstripe data available";
                        break;
                    default:
                        status = "Unknow read error";
                        break;
                }
                if (failEventArgs == null)
                {
                    failEventArgs = new CardReadFailureEventArgs() { DeviceErrors = new List<DeviceError>() };
                }
                failEventArgs.DeviceErrors.Add(new DeviceError() { Code = code, Description = status });
            }
            else
            {

                var dataLength = BitConverter.ToInt16(response, 3);
                var data = BitConverter.ToString(response, 5, dataLength);
                if (String.IsNullOrEmpty(data))
                {
                    this.Log("Fail to read data...");
                    return;
                }
                var tracks = data.Split('|');
                successEventArgs.Track1Data = tracks.Count() >= 0 ? tracks[0] : "";
                successEventArgs.Track2Data = tracks.Count() >= 2 ? tracks[1] : "";
                successEventArgs.Track3Data = tracks.Count() >= 3 ? tracks[2] : "";
                OnCardDataObtained(successEventArgs);
                return;
            }
            OnCardReadFailure(failEventArgs);
        }
        #endregion
    }
}
