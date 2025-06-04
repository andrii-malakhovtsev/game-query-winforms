namespace WhatGameToPlay
{
    partial class AdvancedMessageBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedMessageBox));
            this.labelMessage = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.labelTimer = new System.Windows.Forms.Label();
            this.buttonNo = new System.Windows.Forms.Button();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(12, 28);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(72, 13);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "labelMessage";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.labelTimer);
            this.panel.Controls.Add(this.buttonNo);
            this.panel.Controls.Add(this.buttonYes);
            this.panel.Controls.Add(this.buttonOK);
            this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel.Location = new System.Drawing.Point(0, 67);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(229, 46);
            this.panel.TabIndex = 2;
            // 
            // labelTimer
            // 
            this.labelTimer.AutoSize = true;
            this.labelTimer.Location = new System.Drawing.Point(108, 21);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(13, 13);
            this.labelTimer.TabIndex = 3;
            this.labelTimer.Text = "8";
            this.labelTimer.Visible = false;
            // 
            // buttonNo
            // 
            this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonNo.Location = new System.Drawing.Point(133, 11);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(75, 23);
            this.buttonNo.TabIndex = 2;
            this.buttonNo.Text = "No";
            this.buttonNo.UseVisualStyleBackColor = true;
            this.buttonNo.Visible = false;
            // 
            // buttonYes
            // 
            this.buttonYes.Location = new System.Drawing.Point(52, 11);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(75, 23);
            this.buttonYes.TabIndex = 1;
            this.buttonYes.Text = "Yes";
            this.buttonYes.UseVisualStyleBackColor = true;
            this.buttonYes.Visible = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(144, 11);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonConfirm_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // MyMessageBox
            // 
            this.AcceptButton = this.buttonYes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.buttonNo;
            this.ClientSize = new System.Drawing.Size(229, 113);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MyMessageBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.AdvancedMessageBox_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label labelTimer;
    }
}