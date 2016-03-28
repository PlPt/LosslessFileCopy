namespace LosslessFileCopy
{
    partial class CopyForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyForm));
            this.btnCopy = new System.Windows.Forms.Button();
            this.tbPathInput = new System.Windows.Forms.TextBox();
            this.tbPathDestination = new System.Windows.Forms.TextBox();
            this.pB_CopyProcess = new System.Windows.Forms.ProgressBar();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tbPacketSize = new System.Windows.Forms.TextBox();
            this.lblPacketSizeDescr = new System.Windows.Forms.Label();
            this.lblPacketSizeUnit = new System.Windows.Forms.Label();
            this.btnSelectPathSource = new System.Windows.Forms.Button();
            this.btnSelectPathDest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(651, 74);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbPathInput
            // 
            this.tbPathInput.Location = new System.Drawing.Point(51, 56);
            this.tbPathInput.Name = "tbPathInput";
            this.tbPathInput.Size = new System.Drawing.Size(435, 20);
            this.tbPathInput.TabIndex = 1;
            // 
            // tbPathDestination
            // 
            this.tbPathDestination.Location = new System.Drawing.Point(51, 91);
            this.tbPathDestination.Name = "tbPathDestination";
            this.tbPathDestination.Size = new System.Drawing.Size(435, 20);
            this.tbPathDestination.TabIndex = 2;
            // 
            // pB_CopyProcess
            // 
            this.pB_CopyProcess.Location = new System.Drawing.Point(51, 192);
            this.pB_CopyProcess.Name = "pB_CopyProcess";
            this.pB_CopyProcess.Size = new System.Drawing.Size(620, 23);
            this.pB_CopyProcess.TabIndex = 3;
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(51, 230);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(620, 22);
            this.tbLog.TabIndex = 4;
            // 
            // tbPacketSize
            // 
            this.tbPacketSize.Location = new System.Drawing.Point(571, 156);
            this.tbPacketSize.Name = "tbPacketSize";
            this.tbPacketSize.Size = new System.Drawing.Size(71, 20);
            this.tbPacketSize.TabIndex = 5;
            this.tbPacketSize.Text = "15";
            this.tbPacketSize.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblPacketSizeDescr
            // 
            this.lblPacketSizeDescr.AutoSize = true;
            this.lblPacketSizeDescr.Location = new System.Drawing.Point(495, 159);
            this.lblPacketSizeDescr.Name = "lblPacketSizeDescr";
            this.lblPacketSizeDescr.Size = new System.Drawing.Size(70, 13);
            this.lblPacketSizeDescr.TabIndex = 6;
            this.lblPacketSizeDescr.Text = "Packet Size: ";
            // 
            // lblPacketSizeUnit
            // 
            this.lblPacketSizeUnit.AutoSize = true;
            this.lblPacketSizeUnit.Location = new System.Drawing.Point(648, 159);
            this.lblPacketSizeUnit.Name = "lblPacketSizeUnit";
            this.lblPacketSizeUnit.Size = new System.Drawing.Size(23, 13);
            this.lblPacketSizeUnit.TabIndex = 7;
            this.lblPacketSizeUnit.Text = "MB";
            // 
            // btnSelectPathSource
            // 
            this.btnSelectPathSource.Location = new System.Drawing.Point(498, 56);
            this.btnSelectPathSource.Name = "btnSelectPathSource";
            this.btnSelectPathSource.Size = new System.Drawing.Size(75, 23);
            this.btnSelectPathSource.TabIndex = 8;
            this.btnSelectPathSource.Text = "Select";
            this.btnSelectPathSource.UseVisualStyleBackColor = true;
            this.btnSelectPathSource.Click += new System.EventHandler(this.btnSelectPathSource_Click);
            // 
            // btnSelectPathDest
            // 
            this.btnSelectPathDest.Location = new System.Drawing.Point(498, 88);
            this.btnSelectPathDest.Name = "btnSelectPathDest";
            this.btnSelectPathDest.Size = new System.Drawing.Size(75, 23);
            this.btnSelectPathDest.TabIndex = 9;
            this.btnSelectPathDest.Text = "Select";
            this.btnSelectPathDest.UseVisualStyleBackColor = true;
            this.btnSelectPathDest.Click += new System.EventHandler(this.btnSelectPathDest_Click);
            // 
            // CopyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 289);
            this.Controls.Add(this.btnSelectPathDest);
            this.Controls.Add(this.btnSelectPathSource);
            this.Controls.Add(this.lblPacketSizeUnit);
            this.Controls.Add(this.lblPacketSizeDescr);
            this.Controls.Add(this.tbPacketSize);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.pB_CopyProcess);
            this.Controls.Add(this.tbPathDestination);
            this.Controls.Add(this.tbPathInput);
            this.Controls.Add(this.btnCopy);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CopyForm";
            this.Text = "LosslessFileCopy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox tbPathInput;
        private System.Windows.Forms.TextBox tbPathDestination;
        public System.Windows.Forms.ProgressBar pB_CopyProcess;
        public System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbPacketSize;
        private System.Windows.Forms.Label lblPacketSizeDescr;
        private System.Windows.Forms.Label lblPacketSizeUnit;
        private System.Windows.Forms.Button btnSelectPathSource;
        private System.Windows.Forms.Button btnSelectPathDest;
    }
}

