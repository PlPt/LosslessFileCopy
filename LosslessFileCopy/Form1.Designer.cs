namespace LosslessFileCopy
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.tbPathInput = new System.Windows.Forms.TextBox();
            this.tbPathDestination = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(553, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Copy";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbPathInput
            // 
            this.tbPathInput.Location = new System.Drawing.Point(51, 56);
            this.tbPathInput.Name = "tbPathInput";
            this.tbPathInput.Size = new System.Drawing.Size(435, 20);
            this.tbPathInput.TabIndex = 1;
            this.tbPathInput.Text = "F:\\Predators.m2t";
            // 
            // tbPathDestination
            // 
            this.tbPathDestination.Location = new System.Drawing.Point(51, 91);
            this.tbPathDestination.Name = "tbPathDestination";
            this.tbPathDestination.Size = new System.Drawing.Size(435, 20);
            this.tbPathDestination.TabIndex = 2;
            this.tbPathDestination.Text = "D:\\temp\\ddd";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(51, 192);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(620, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(51, 230);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(620, 22);
            this.tbLog.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(571, 156);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "15";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 289);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tbPathDestination);
            this.Controls.Add(this.tbPathInput);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbPathInput;
        private System.Windows.Forms.TextBox tbPathDestination;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox textBox1;
    }
}

