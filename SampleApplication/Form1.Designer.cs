namespace SampleApplication
{
    partial class Form1
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
            this.buttonAsynk = new System.Windows.Forms.Button();
            this.buttonSynk = new System.Windows.Forms.Button();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.textBoxToEcho = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonAsynk
            // 
            this.buttonAsynk.Location = new System.Drawing.Point(64, 37);
            this.buttonAsynk.Name = "buttonAsynk";
            this.buttonAsynk.Size = new System.Drawing.Size(75, 23);
            this.buttonAsynk.TabIndex = 0;
            this.buttonAsynk.Text = "Asynk";
            this.buttonAsynk.UseVisualStyleBackColor = true;
            this.buttonAsynk.Click += new System.EventHandler(this.buttonAsynk_Click);
            // 
            // buttonSynk
            // 
            this.buttonSynk.Location = new System.Drawing.Point(145, 37);
            this.buttonSynk.Name = "buttonSynk";
            this.buttonSynk.Size = new System.Drawing.Size(75, 23);
            this.buttonSynk.TabIndex = 1;
            this.buttonSynk.Text = "Synk";
            this.buttonSynk.UseVisualStyleBackColor = true;
            this.buttonSynk.Click += new System.EventHandler(this.buttonSynk_Click);
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(13, 66);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.Size = new System.Drawing.Size(259, 186);
            this.textBoxStatus.TabIndex = 2;
            // 
            // textBoxToEcho
            // 
            this.textBoxToEcho.Location = new System.Drawing.Point(66, 5);
            this.textBoxToEcho.Name = "textBoxToEcho";
            this.textBoxToEcho.Size = new System.Drawing.Size(153, 20);
            this.textBoxToEcho.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.textBoxToEcho);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.buttonSynk);
            this.Controls.Add(this.buttonAsynk);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAsynk;
        private System.Windows.Forms.Button buttonSynk;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.TextBox textBoxToEcho;
    }
}

