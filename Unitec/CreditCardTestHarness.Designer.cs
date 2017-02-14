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
            this.btnInitialize = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckHealth = new System.Windows.Forms.Button();
            this.btnRunDiagnostic = new System.Windows.Forms.Button();
            this.btnTerminate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLogFilePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtConfigFilePath
            // 
            this.txtConfigFilePath.Location = new System.Drawing.Point(96, 28);
            this.txtConfigFilePath.Name = "txtConfigFilePath";
            this.txtConfigFilePath.Size = new System.Drawing.Size(231, 20);
            this.txtConfigFilePath.TabIndex = 0;
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(18, 151);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(149, 70);
            this.btnInitialize.TabIndex = 1;
            this.btnInitialize.Text = "Initialize";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(182, 151);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(149, 70);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(346, 151);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(149, 70);
            this.btnEnable.TabIndex = 3;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(510, 151);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(149, 70);
            this.btnDisable.TabIndex = 4;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(674, 151);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(149, 70);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
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
            // btnCheckHealth
            // 
            this.btnCheckHealth.Location = new System.Drawing.Point(346, 33);
            this.btnCheckHealth.Name = "btnCheckHealth";
            this.btnCheckHealth.Size = new System.Drawing.Size(149, 70);
            this.btnCheckHealth.TabIndex = 7;
            this.btnCheckHealth.Text = "Check Health";
            this.btnCheckHealth.UseVisualStyleBackColor = true;
            this.btnCheckHealth.Click += new System.EventHandler(this.btnCheckHealth_Click);
            // 
            // btnRunDiagnostic
            // 
            this.btnRunDiagnostic.Location = new System.Drawing.Point(510, 32);
            this.btnRunDiagnostic.Name = "btnRunDiagnostic";
            this.btnRunDiagnostic.Size = new System.Drawing.Size(149, 70);
            this.btnRunDiagnostic.TabIndex = 8;
            this.btnRunDiagnostic.Text = "Run Diagnostic";
            this.btnRunDiagnostic.UseVisualStyleBackColor = true;
            this.btnRunDiagnostic.Click += new System.EventHandler(this.btnRunDiagnostic_Click);
            // 
            // btnTerminate
            // 
            this.btnTerminate.Location = new System.Drawing.Point(674, 32);
            this.btnTerminate.Name = "btnTerminate";
            this.btnTerminate.Size = new System.Drawing.Size(149, 70);
            this.btnTerminate.TabIndex = 9;
            this.btnTerminate.Text = "Terminate";
            this.btnTerminate.UseVisualStyleBackColor = true;
            this.btnTerminate.Click += new System.EventHandler(this.btnTerminate_Click);
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
            this.label3.Location = new System.Drawing.Point(26, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Response";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(96, 264);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(727, 20);
            this.txtResult.TabIndex = 13;
            // 
            // CreditCardTestHarness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 334);
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
        private System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckHealth;
        private System.Windows.Forms.Button btnRunDiagnostic;
        private System.Windows.Forms.Button btnTerminate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLogFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtResult;
    }
}

