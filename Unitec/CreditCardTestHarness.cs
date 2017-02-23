using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unitec.Middleware;
using Unitec.Middleware.Contracts;
using Unitec.Middleware.Devices;
using Unitec.Middleware.Helpers;


namespace Unitec
{
    public partial class CreditCardTestHarness : Form
    {
        delegate void SetTextCallback(string text);
        private ICreditCardReader cardReader;
        delegate void StringArgReturningVoidDelegate(string text);
        public CreditCardTestHarness()
        {
            InitializeComponent();
            txtLogFilePath.Text = "C:\\Unitec\\Log\\Unitec.Middleware.log";
            txtConfigFilePath.Text = "..\\..\\CreditCardReader.config";
            CreateNewCardReaderDevice();
        }

        #region Events
        private void CardReader_CardReadFailure(object sender, DeviceErrorEventArgs e)
        {
            SetText("Could not read the card:\r\n");
            foreach (var err in e.DeviceErrors)
            {
                SetText(String.Format("Code: {0}  Desc: {1} \r\n", err.Code, err.Description));
            }
        }

        private void CardReader_CardInsertTimeout(object sender, EventArgs e)
        {
            SetText("Card reader timeout");
        }

        private void CardReader_CardInserted(object sender, EventArgs e)
        {
            SetText("Card Inserted");
        }

        private void CardReader_CardDataObtained(object sender, CardDataObtainedEventArgs e)
        {
            SetText(e.Track1Data);
            SetText(e.Track2Data);
            SetText(e.Track3Data);

        }

        private void CardReader_DeviceDisconnected(object sender, EventArgs e)
        {
            txtResult.Text = "Device Disconnected";
        }

        private void CardReader_DeviceConnected(object sender, EventArgs e)
        {
            txtResult.Text = "Device Connected";
        }

        private void CardReader_DeviceErrorOccurred(object sender,DeviceErrorEventArgs e)
        {
            foreach (var err in e.DeviceErrors)
            {
                SetText(String.Format("Code: {0}  Desc: {1} \r\n", err.Code,err.Description));
            }
        }

        private void CardReader_DataLogged(object sender, LogEventArgs e)
        {
            SetLogText(e.Data);
            SetLogText("\r\n");
        }
        #endregion

        #region Methods

        private void CreateNewCardReaderDevice()
        {
            cardReader = DeviceContainer.GetDevice<ICreditCardReader>();
            cardReader.DeviceErrorOccurred += CardReader_DeviceErrorOccurred;
            cardReader.DeviceConnected += CardReader_DeviceConnected;
            cardReader.DeviceDisconnected += CardReader_DeviceDisconnected;
            cardReader.CardDataObtained += CardReader_CardDataObtained;
            cardReader.CardInserted += CardReader_CardInserted;
            cardReader.CardInsertTimeout += CardReader_CardInsertTimeout;
            cardReader.CardReadFailure += CardReader_CardReadFailure;
            cardReader.DataLogged += CardReader_DataLogged;
        }
        private void btnCheckHealth_Click(object sender, EventArgs e)
        {
            int code;
            string status;
            string hardwareIdentity;
            string report;
            var res = cardReader.CheckHealth(out code,out status,out hardwareIdentity,out report);
            txtResult.Text = res.ToString();
            txtResult.AppendText(String.Format("Code {0} : Status {1} \r\n", code, status));
            txtResult.AppendText(String.Format("HardwareIdentity : \r\n {0}", hardwareIdentity));
            txtResult.AppendText(String.Format("Report : \r\n {0}", report));
        }

        private void btnRunDiagnostic_Click(object sender, EventArgs e)
        {
            List<string> symptomsCodes = null;
            string deviceInfo = "";
           var res = cardReader.RunDiagnosticTests(out symptomsCodes, out deviceInfo);

           txtResult.AppendText(res.ToString());
           txtResult.AppendText(String.Format("Device Info : \r\n {0}", deviceInfo));
           txtResult.AppendText(String.Format("Symptom Codes : \r\n"));
            if(symptomsCodes != null && symptomsCodes.Count > 0)
            {
                symptomsCodes.ForEach(x =>
                {
                    txtResult.AppendText(String.Format("{0} r\n ",x));
                });
            }

       }

        private void btnTerminate_Click(object sender, EventArgs e)
        {
            var res = cardReader.TerminateDevice();
            txtResult.Text = res.ToString();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            cardReader.PeripheralsConfigFile = txtConfigFilePath.Text;
            cardReader.LogFile = txtLogFilePath.Text;
            var res = cardReader.InitializeDevice();
            txtResult.Text = res.ToString();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var res = cardReader.ConnectToDevice();
            txtResult.Text = res.ToString();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            var res = cardReader.DisconnectFromDevice();
            txtResult.Text = res.ToString();
        }
        private void btnEnable_Click(object sender, EventArgs e)
        {
            var res = cardReader.EnableDevice();
            txtResult.Text = res.ToString();
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            var res = cardReader.DisableDevice();
            txtResult.Text = res.ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            var res = cardReader.ResetHardware();
            txtResult.Text = res.ToString();
        }

        private void btnCardReady_Click(object sender, EventArgs e)
        {
            cardReader.SetReaderReady();
        }


        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtResult.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtResult.AppendText(text);
            }
        }


        private void SetLogText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLogText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtLog.AppendText(text);
            }
        }
        #endregion

        private void btnNewInstance_Click(object sender, EventArgs e)
        {
            CreateNewCardReaderDevice();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
            txtResult.Text = "";
        }
    }
}
