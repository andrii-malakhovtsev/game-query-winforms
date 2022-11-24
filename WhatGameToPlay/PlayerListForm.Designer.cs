namespace WhatGameToPlay
{
    partial class PlayerListForm
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
            this.labelPlayerList = new System.Windows.Forms.Label();
            this.listBoxPlayers = new System.Windows.Forms.ListBox();
            this.labelGamesPlaying = new System.Windows.Forms.Label();
            this.buttonSet = new System.Windows.Forms.Button();
            this.textBoxSelectedPlayer = new System.Windows.Forms.TextBox();
            this.checkedListBoxGamesPlaying = new System.Windows.Forms.CheckedListBox();
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.buttonAddPerson = new System.Windows.Forms.Button();
            this.buttonDeletePerson = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelPlayerList
            // 
            this.labelPlayerList.AutoSize = true;
            this.labelPlayerList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerList.Location = new System.Drawing.Point(44, 9);
            this.labelPlayerList.Name = "labelPlayerList";
            this.labelPlayerList.Size = new System.Drawing.Size(105, 24);
            this.labelPlayerList.TabIndex = 11;
            this.labelPlayerList.Text = "Player List";
            // 
            // listBoxPlayers
            // 
            this.listBoxPlayers.FormattingEnabled = true;
            this.listBoxPlayers.Location = new System.Drawing.Point(12, 41);
            this.listBoxPlayers.Name = "listBoxPlayers";
            this.listBoxPlayers.Size = new System.Drawing.Size(175, 199);
            this.listBoxPlayers.TabIndex = 10;
            this.listBoxPlayers.DoubleClick += new System.EventHandler(this.ListBoxPlayers_DoubleClick);
            // 
            // labelGamesPlaying
            // 
            this.labelGamesPlaying.AutoSize = true;
            this.labelGamesPlaying.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGamesPlaying.Location = new System.Drawing.Point(233, 12);
            this.labelGamesPlaying.Name = "labelGamesPlaying";
            this.labelGamesPlaying.Size = new System.Drawing.Size(127, 20);
            this.labelGamesPlaying.TabIndex = 13;
            this.labelGamesPlaying.Text = "Games playing";
            // 
            // buttonSet
            // 
            this.buttonSet.Location = new System.Drawing.Point(204, 276);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(110, 23);
            this.buttonSet.TabIndex = 14;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.ButtonGamesPlayingSet_Click);
            // 
            // textBoxSelectedPlayer
            // 
            this.textBoxSelectedPlayer.Location = new System.Drawing.Point(12, 250);
            this.textBoxSelectedPlayer.Name = "textBoxSelectedPlayer";
            this.textBoxSelectedPlayer.Size = new System.Drawing.Size(175, 20);
            this.textBoxSelectedPlayer.TabIndex = 15;
            this.textBoxSelectedPlayer.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // checkedListBoxGamesPlaying
            // 
            this.checkedListBoxGamesPlaying.FormattingEnabled = true;
            this.checkedListBoxGamesPlaying.Location = new System.Drawing.Point(204, 41);
            this.checkedListBoxGamesPlaying.Name = "checkedListBoxGamesPlaying";
            this.checkedListBoxGamesPlaying.Size = new System.Drawing.Size(193, 229);
            this.checkedListBoxGamesPlaying.TabIndex = 16;
            // 
            // checkBoxSelectAll
            // 
            this.checkBoxSelectAll.AutoSize = true;
            this.checkBoxSelectAll.Location = new System.Drawing.Point(320, 280);
            this.checkBoxSelectAll.Name = "checkBoxSelectAll";
            this.checkBoxSelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBoxSelectAll.TabIndex = 17;
            this.checkBoxSelectAll.Text = "Select All";
            this.checkBoxSelectAll.UseVisualStyleBackColor = true;
            this.checkBoxSelectAll.CheckedChanged += new System.EventHandler(this.CheckBoxSelectAll_CheckedChanged);
            // 
            // buttonAddPerson
            // 
            this.buttonAddPerson.Enabled = false;
            this.buttonAddPerson.Location = new System.Drawing.Point(13, 276);
            this.buttonAddPerson.Name = "buttonAddPerson";
            this.buttonAddPerson.Size = new System.Drawing.Size(84, 23);
            this.buttonAddPerson.TabIndex = 18;
            this.buttonAddPerson.Text = "Add";
            this.buttonAddPerson.UseVisualStyleBackColor = true;
            this.buttonAddPerson.Click += new System.EventHandler(this.ButtonAddPerson_Click);
            // 
            // buttonDeletePerson
            // 
            this.buttonDeletePerson.Enabled = false;
            this.buttonDeletePerson.Location = new System.Drawing.Point(103, 276);
            this.buttonDeletePerson.Name = "buttonDeletePerson";
            this.buttonDeletePerson.Size = new System.Drawing.Size(84, 23);
            this.buttonDeletePerson.TabIndex = 19;
            this.buttonDeletePerson.Text = "Delete";
            this.buttonDeletePerson.UseVisualStyleBackColor = true;
            this.buttonDeletePerson.Click += new System.EventHandler(this.ButtonDeletePerson_Click);
            // 
            // FormPlayerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 307);
            this.Controls.Add(this.buttonDeletePerson);
            this.Controls.Add(this.buttonAddPerson);
            this.Controls.Add(this.checkBoxSelectAll);
            this.Controls.Add(this.checkedListBoxGamesPlaying);
            this.Controls.Add(this.textBoxSelectedPlayer);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.labelGamesPlaying);
            this.Controls.Add(this.labelPlayerList);
            this.Controls.Add(this.listBoxPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPlayerList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "People list";
            this.Load += new System.EventHandler(this.FormPlayerList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPlayerList;
        public System.Windows.Forms.ListBox listBoxPlayers;
        private System.Windows.Forms.Label labelGamesPlaying;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.TextBox textBoxSelectedPlayer;
        private System.Windows.Forms.CheckedListBox checkedListBoxGamesPlaying;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private System.Windows.Forms.Button buttonAddPerson;
        private System.Windows.Forms.Button buttonDeletePerson;
    }
}