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

namespace Unitec
{
    public partial class CreditCardTestHarness : Form
    {
        private CreditCardReader cardReader;
        delegate void StringArgReturningVoidDelegate(string text);
        public CreditCardTestHarness()
        {
            InitializeComponent();
            cardReader = new CreditCardReader();
            txtLogFilePath.Text = "C:\\Unitec\\Log\\Unitec.Middleware.log";
            txtConfigFilePath.Text = "C:\\Unitec\\Config\\CreditCardReader.config";
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
            Program.CreateFileWatcher(cardReader.LogFile, OnLogUpdated);
        }

        private void SetLogText(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.txtLog.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetLogText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtLog.ResetText();
                this.txtLog.AppendText(text);
            }
        }


        private void OnLogUpdated(object obj, FileSystemEventArgs args)
        {
            try
            {
                StringBuilder lines = new StringBuilder();
                var logs = File.ReadLines(args.FullPath).Reverse().Take(10);
                foreach (var line in logs)
                {
                    lines.Append(line);
                    lines.Append("\r\n");
                }
                SetLogText(lines.ToString());
            }
            catch
            {

            }
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
