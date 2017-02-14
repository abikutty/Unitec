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
        private SerialPort creditcardPort = null;
        private byte[] message = new byte[8];
        private readonly byte[] Resethardware = new byte[] { 60, 00, 02, 53, 18, 29, 03 };
        private const int timeOut = 5000;
        public event CardDataObtainedEventHandler CardDataObtained;
        public event EventHandler CardInserted;
        public event EventHandler CardInsertTimeout;
        public event DeviceErrorEventHandler CardReadFailure;
        private AutoResetEvent messageReceived = new AutoResetEvent(false);
        private AutoResetEvent errorReceived = new AutoResetEvent(false);

        #region Event
        protected virtual void OnCardDataObtained(CardDataObtainedEventArgs e)
        {
            CardDataObtainedEventHandler handler = CardDataObtained;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnCardInserted(EventArgs e)
        {
            EventHandler handler = CardInserted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnCardInsertTimeout(EventArgs e)
        {
            EventHandler handler = CardInsertTimeout;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnCardReadFailure(CardReadFailureEventArgs e)
        {
            DeviceErrorEventHandler handler = CardReadFailure;
            if (handler != null)
            {
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
            var error = DeviceErrors.UnexpectedError;
            Exception exception = null;
            try
            {
                if (!creditcardPort.IsOpen)
                {
                    creditcardPort.Open();
                    creditcardPort.DataReceived += HandleSerialDataReceived;
                    creditcardPort.ErrorReceived += HandleErrorReceived;
                }
                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                error = DeviceErrors.PortAccessDenied;
                exception = e;
            }
            catch (ArgumentOutOfRangeException e)
            {
                error = DeviceErrors.PortAccessDenied;
                exception = e;
            }
            catch (ArgumentException e)
            {
                error = DeviceErrors.PortAccessDenied;
                exception = e;
            }
            catch (IOException e)
            {
                exception = e;
                error = DeviceErrors.PortAccessDenied;
            }
            catch (InvalidOperationException e)
            {
                exception = e;
                error = DeviceErrors.PortAccessDenied;
            }
            catch (Exception e)
            {
                exception = e;
                error = DeviceErrors.UnexpectedError;
            }
            finally
            {
                if (exception != null)
                {
                    var errorArg = exception.Create(error);
                    OnDeviceErrorOccurred(errorArg);

                }

            }
            return false;
        }

        public override bool DisableDevice()
        {
            throw new NotImplementedException();
        }

        public override bool DisconnectFromDevice()
        {
            var error = DeviceErrors.UnexpectedError;
            Exception exception = null;
            try
            {
                if (creditcardPort.IsOpen)
                {
                    creditcardPort.Close();
                }
                return true;
            }
            catch (IOException e)
            {
                exception = e;
                error = DeviceErrors.UnableToClosePort;
            }
            catch (Exception e)
            {
                exception = e;
                error = DeviceErrors.UnableToClosePort;
            }
            finally
            {
                if (exception != null)
                {
                    var errorArg = exception.Create(error);
                    OnDeviceErrorOccurred(errorArg);

                }

            }
            return false;
        }

        public override bool EnableDevice()
        {
            throw new NotImplementedException();
        }

        public override bool InitializeDevice()
        {
            try
            {
                if (String.IsNullOrEmpty(PeripheralsConfigFile) || File.Exists(PeripheralsConfigFile))
                {
                    throw new FileNotFoundException("Config file missing.", PeripheralsConfigFile);
                }
                var conn = new PeripheralConnection(PeripheralsConfigFile);

                creditcardPort = new SerialPort(conn.ComPort, conn.BaudRate,
                                                 conn.Parity, conn.DataBits, conn.StopBits);
                return true;
            }
            catch (Exception ex)
            {
                var error = ex.Create(DeviceErrors.DeviceInitFailed);
                OnDeviceErrorOccurred(error);
                return false;
            }

        }


        public override bool ResetHardware()
        {
            var error = DeviceErrors.UnexpectedError;
            Exception exception = null;
            try
            {
                if (creditcardPort != null)
                {
                    creditcardPort.Write(Resethardware, 0, Resethardware.Count());

                    if (ReadSignaled())
                    {
                        creditcardPort.Read(message, 0, message.Count());
                        if (message[3] == 0x90 && message[4] == 0x00)
                        {
                            return true;
                        }
                    }
                    return DisconnectFromDevice();
                }
            }
            catch (IOException e)
            {
                exception = e;
                error = DeviceErrors.UnableToClosePort;
            }
            catch (Exception e)
            {
                exception = e;
                error = DeviceErrors.UnableToClosePort;
            }
            finally
            {
                if (exception != null)
                {
                    var errorArg = exception.Create(error);
                    OnDeviceErrorOccurred(errorArg);

                }

            }
            return false;
        }

        public override bool TerminateDevice()
        {
            throw new NotImplementedException();
        }

        public override bool CheckHealth(out int code, out string status, out string hardwareIdentity, out string report)
        {
            throw new NotImplementedException();
        }

        public override bool RunDiagnosticTests(out List<string> symptomsCodes, out string deviceInfo)
        {
            throw new NotImplementedException();
        }
        #endregion

        private bool ReadSignaled()
        {
            if (WaitHandle.WaitAny(new WaitHandle[] { messageReceived, errorReceived }, timeOut) == 0)
                return true;
            return false;
        }


        protected void HandleSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof)
                messageReceived.Set();
        }

        protected void HandleErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            var ex = new Exception(String.Format("Internal Error Code {0}", e.EventType));
            var eventArgs = ex.Create(DeviceErrors.ErrorReceving);
            OnDeviceErrorOccurred(eventArgs);
            errorReceived.Set();
        }
    }
}
