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
        private ICreditCardReader cardReader;
        delegate void StringArgReturningVoidDelegate(string text);
        public CreditCardTestHarness()
        {
            InitializeComponent();
            cardReader =  DeviceContainer.GetDevice<ICreditCardReader>();

            txtLogFilePath.Text = "C:\\Unitec\\Log\\Unitec.Middleware.log";
            txtConfigFilePath.Text = "..\\..\\CreditCardReader.config";
            cardReader.DeviceErrorOccurred += CardReader_DeviceErrorOccurred;
            cardReader.DeviceConnected += CardReader_DeviceConnected;
            cardReader.DeviceDisconnected += CardReader_DeviceDisconnected;
            cardReader.CardDataObtained += CardReader_CardDataObtained;
            cardReader.CardInserted += CardReader_CardInserted;
            cardReader.CardInsertTimeout += CardReader_CardInsertTimeout;
            cardReader.CardReadFailure += CardReader_CardReadFailure;
            cardReader.DataLogged += CardReader_DataLogged;
        }

        private void CardReader_DataLogged(object sender, LogEventArgs e)
        {
            txtLog.AppendText(e.Data);
            txtLog.AppendText("\r\n");
        }

        private void CardReader_CardReadFailure(object sender, DeviceErrorEventArgs e)
        {
            txtResult.AppendText("Could not read the card");
            foreach (var err in e.DeviceErrors)
            {
                txtResult.AppendText(String.Format("Code: {0}  Desc: {1} \r\n", err.Code, err.Description));
            }
        }

        private void CardReader_CardInsertTimeout(object sender, EventArgs e)
        {
            txtResult.Text = "Card reader timeout";
        }

        private void CardReader_CardInserted(object sender, EventArgs e)
        {
            txtResult.Text = "Card Inserted";
        }

        private void CardReader_CardDataObtained(object sender, CardDataObtainedEventArgs e)
        {
            txtResult.AppendText(e.Track1Data);
            txtResult.AppendText(e.Track2Data);
            txtResult.AppendText(e.Track3Data);

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
                txtResult.AppendText(String.Format("Code: {0}  Desc: {1} \r\n", err.Code,err.Description));
            }
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

        private void btnCardData_Click(object sender, EventArgs e)
        {

        }

        private void btnCardFailure_Click(object sender, EventArgs e)
        {
      
        }

        private void btnInsertTimeout_Click(object sender, EventArgs e)
        {

        }

        private void btnCardInserted_Click(object sender, EventArgs e)
        {
            
        }

    }
}
