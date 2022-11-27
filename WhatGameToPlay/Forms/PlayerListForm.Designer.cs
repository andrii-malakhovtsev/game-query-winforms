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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerListForm));
            this.labelPlayerList = new System.Windows.Forms.Label();
            this.listBoxPlayers = new System.Windows.Forms.ListBox();
            this.labelGamesPlaying = new System.Windows.Forms.Label();
            this.buttonSet = new System.Windows.Forms.Button();
            this.textBoxSelectedPlayer = new System.Windows.Forms.TextBox();
            this.checkedListBoxGamesPlaying = new System.Windows.Forms.CheckedListBox();
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.buttonAddPlayer = new System.Windows.Forms.Button();
            this.buttonDeletePlayer = new System.Windows.Forms.Button();
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
            this.listBoxPlayers.TabIndex = 0;
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
            // buttonAddPlayer
            // 
            this.buttonAddPlayer.Enabled = false;
            this.buttonAddPlayer.Location = new System.Drawing.Point(13, 276);
            this.buttonAddPlayer.Name = "buttonAddPlayer";
            this.buttonAddPlayer.Size = new System.Drawing.Size(84, 23);
            this.buttonAddPlayer.TabIndex = 18;
            this.buttonAddPlayer.Text = "Add";
            this.buttonAddPlayer.UseVisualStyleBackColor = true;
            this.buttonAddPlayer.Click += new System.EventHandler(this.ButtonAddPlayer_Click);
            // 
            // buttonDeletePlayer
            // 
            this.buttonDeletePlayer.Enabled = false;
            this.buttonDeletePlayer.Location = new System.Drawing.Point(103, 276);
            this.buttonDeletePlayer.Name = "buttonDeletePlayer";
            this.buttonDeletePlayer.Size = new System.Drawing.Size(84, 23);
            this.buttonDeletePlayer.TabIndex = 19;
            this.buttonDeletePlayer.Text = "Delete";
            this.buttonDeletePlayer.UseVisualStyleBackColor = true;
            this.buttonDeletePlayer.Click += new System.EventHandler(this.ButtonDeletePlayer_Click);
            // 
            // PlayerListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 307);
            this.Controls.Add(this.buttonDeletePlayer);
            this.Controls.Add(this.buttonAddPlayer);
            this.Controls.Add(this.checkBoxSelectAll);
            this.Controls.Add(this.checkedListBoxGamesPlaying);
            this.Controls.Add(this.textBoxSelectedPlayer);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.labelGamesPlaying);
            this.Controls.Add(this.labelPlayerList);
            this.Controls.Add(this.listBoxPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlayerListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Player List";
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
        private System.Windows.Forms.Button buttonAddPlayer;
        private System.Windows.Forms.Button buttonDeletePlayer;
    }
}