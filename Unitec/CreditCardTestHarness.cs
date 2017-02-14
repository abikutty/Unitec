using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unitec.Middleware;

namespace Unitec
{
    public partial class CreditCardTestHarness : Form
    {
        private CreditCardReader cardReader;
        public CreditCardTestHarness()
        {
            InitializeComponent();
            cardReader = new CreditCardReader();
        }

        private void CreditCardReader_Load(object sender, EventArgs e)
        {

        }

        private void btnCheckHealth_Click(object sender, EventArgs e)
        {

        }

        private void btnRunDiagnostic_Click(object sender, EventArgs e)
        {
            //var res = cardReader.RunDiagnosticTests();
           // txtResult.Text = res.ToString();
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
    }
}
