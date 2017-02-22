namespace Unitec
{
    partial class CreditCardTestHarness
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtConfigFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLogFilePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCheckHealth = new System.Windows.Forms.Button();
            this.btnRunDiagnostic = new System.Windows.Forms.Button();
            this.btnTerminate = new System.Windows.Forms.Button();
            this.btnCardData = new System.Windows.Forms.Button();
            this.btnCardInserted = new System.Windows.Forms.Button();
            this.btnInsertTimeout = new System.Windows.Forms.Button();
            this.btnCardFailure = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtConfigFilePath
            // 
            this.txtConfigFilePath.Location = new System.Drawing.Point(96, 28);
            this.txtConfigFilePath.Name = "txtConfigFilePath";
            this.txtConfigFilePath.Size = new System.Drawing.Size(231, 20);
            this.txtConfigFilePath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Config File Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Log File Path";
            // 
            // txtLogFilePath
            // 
            this.txtLogFilePath.Location = new System.Drawing.Point(96, 58);
            this.txtLogFilePath.Name = "txtLogFilePath";
            this.txtLogFilePath.Size = new System.Drawing.Size(231, 20);
            this.txtLogFilePath.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Response";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(96, 84);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(231, 77);
            this.txtResult.TabIndex = 13;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(87, 267);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(874, 115);
            this.txtLog.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 305);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Log";
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(340, 24);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(149, 70);
            this.btnInitialize.TabIndex = 1;
            this.btnInitialize.Text = "Initialize";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(340, 180);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(149, 70);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(340, 100);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(149, 70);
            this.btnEnable.TabIndex = 3;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(499, 180);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(149, 70);
            this.btnDisable.TabIndex = 4;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(499, 100);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(149, 70);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnCheckHealth
            // 
            this.btnCheckHealth.Location = new System.Drawing.Point(820, 24);
            this.btnCheckHealth.Name = "btnCheckHealth";
            this.btnCheckHealth.Size = new System.Drawing.Size(149, 70);
            this.btnCheckHealth.TabIndex = 7;
            this.btnCheckHealth.Text = "Check Health";
            this.btnCheckHealth.UseVisualStyleBackColor = true;
            this.btnCheckHealth.Click += new System.EventHandler(this.btnCheckHealth_Click);
            // 
            // btnRunDiagnostic
            // 
            this.btnRunDiagnostic.Location = new System.Drawing.Point(820, 104);
            this.btnRunDiagnostic.Name = "btnRunDiagnostic";
            this.btnRunDiagnostic.Size = new System.Drawing.Size(149, 70);
            this.btnRunDiagnostic.TabIndex = 8;
            this.btnRunDiagnostic.Text = "Run Diagnostic";
            this.btnRunDiagnostic.UseVisualStyleBackColor = true;
            this.btnRunDiagnostic.Click += new System.EventHandler(this.btnRunDiagnostic_Click);
            // 
            // btnTerminate
            // 
            this.btnTerminate.Location = new System.Drawing.Point(499, 24);
            this.btnTerminate.Name = "btnTerminate";
            this.btnTerminate.Size = new System.Drawing.Size(149, 70);
            this.btnTerminate.TabIndex = 9;
            this.btnTerminate.Text = "Terminate";
            this.btnTerminate.UseVisualStyleBackColor = true;
            this.btnTerminate.Click += new System.EventHandler(this.btnTerminate_Click);
            // 
            // btnCardData
            // 
            this.btnCardData.Location = new System.Drawing.Point(820, 180);
            this.btnCardData.Name = "btnCardData";
            this.btnCardData.Size = new System.Drawing.Size(149, 70);
            this.btnCardData.TabIndex = 14;
            this.btnCardData.Text = "Card Data Obtained";
            this.btnCardData.UseVisualStyleBackColor = true;
            this.btnCardData.Click += new System.EventHandler(this.btnCardData_Click);
            // 
            // btnCardInserted
            // 
            this.btnCardInserted.Location = new System.Drawing.Point(661, 180);
            this.btnCardInserted.Name = "btnCardInserted";
            this.btnCardInserted.Size = new System.Drawing.Size(149, 70);
            this.btnCardInserted.TabIndex = 15;
            this.btnCardInserted.Text = "Card Inserted";
            this.btnCardInserted.UseVisualStyleBackColor = true;
            this.btnCardInserted.Click += new System.EventHandler(this.btnCardInserted_Click);
            // 
            // btnInsertTimeout
            // 
            this.btnInsertTimeout.Location = new System.Drawing.Point(661, 100);
            this.btnInsertTimeout.Name = "btnInsertTimeout";
            this.btnInsertTimeout.Size = new System.Drawing.Size(149, 70);
            this.btnInsertTimeout.TabIndex = 16;
            this.btnInsertTimeout.Text = "Card Insert Timeout";
            this.btnInsertTimeout.UseVisualStyleBackColor = true;
            this.btnInsertTimeout.Click += new System.EventHandler(this.btnInsertTimeout_Click);
            // 
            // btnCardFailure
            // 
            this.btnCardFailure.Location = new System.Drawing.Point(661, 24);
            this.btnCardFailure.Name = "btnCardFailure";
            this.btnCardFailure.Size = new System.Drawing.Size(149, 70);
            this.btnCardFailure.TabIndex = 19;
            this.btnCardFailure.Text = "Card Read Failure";
            this.btnCardFailure.UseVisualStyleBackColor = true;
            this.btnCardFailure.Click += new System.EventHandler(this.btnCardFailure_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(185, 180);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(149, 70);
            this.btnDisconnect.TabIndex = 20;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // CreditCardTestHarness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 393);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnCardFailure);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnInsertTimeout);
            this.Controls.Add(this.btnCardInserted);
            this.Controls.Add(this.btnCardData);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLogFilePath);
            this.Controls.Add(this.btnTerminate);
            this.Controls.Add(this.btnRunDiagnostic);
            this.Controls.Add(this.btnCheckHealth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDisable);
            this.Controls.Add(this.btnEnable);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnInitialize);
            this.Controls.Add(this.txtConfigFilePath);
            this.Name = "CreditCardTestHarness";
            this.Text = "Credit Card Reader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConfigFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLogFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCheckHealth;
        private System.Windows.Forms.Button btnRunDiagnostic;
        private System.Windows.Forms.Button btnTerminate;
        private System.Windows.Forms.Button btnCardData;
        private System.Windows.Forms.Button btnCardInserted;
        private System.Windows.Forms.Button btnInsertTimeout;
        private System.Windows.Forms.Button btnCardFailure;
        private System.Windows.Forms.Button btnDisconnect;
    }
}

