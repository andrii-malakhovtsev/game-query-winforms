namespace WhatGameToPlay
{
    partial class GamesListForm
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
            this.listBoxGames = new System.Windows.Forms.ListBox();
            this.buttonAddGame = new System.Windows.Forms.Button();
            this.labelGamesList = new System.Windows.Forms.Label();
            this.labelEnterGameName = new System.Windows.Forms.Label();
            this.textBoxGameName = new System.Windows.Forms.TextBox();
            this.buttonDeleteGame = new System.Windows.Forms.Button();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.checkBoxPeopleNumberLimit = new System.Windows.Forms.CheckBox();
            this.numericUpDownMin = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMax = new System.Windows.Forms.NumericUpDown();
            this.buttonSet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMax)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxGames
            // 
            this.listBoxGames.FormattingEnabled = true;
            this.listBoxGames.Location = new System.Drawing.Point(12, 41);
            this.listBoxGames.Name = "listBoxGames";
            this.listBoxGames.Size = new System.Drawing.Size(312, 212);
            this.listBoxGames.TabIndex = 7;
            this.listBoxGames.DoubleClick += new System.EventHandler(this.ListBoxGames_DoubleClick);
            // 
            // buttonAddGame
            // 
            this.buttonAddGame.Enabled = false;
            this.buttonAddGame.Location = new System.Drawing.Point(361, 100);
            this.buttonAddGame.Name = "buttonAddGame";
            this.buttonAddGame.Size = new System.Drawing.Size(84, 24);
            this.buttonAddGame.TabIndex = 8;
            this.buttonAddGame.Text = "Add";
            this.buttonAddGame.UseVisualStyleBackColor = true;
            this.buttonAddGame.Click += new System.EventHandler(this.ButtonAddGame_Click);
            // 
            // labelGamesList
            // 
            this.labelGamesList.AutoSize = true;
            this.labelGamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGamesList.Location = new System.Drawing.Point(105, 9);
            this.labelGamesList.Name = "labelGamesList";
            this.labelGamesList.Size = new System.Drawing.Size(112, 24);
            this.labelGamesList.TabIndex = 9;
            this.labelGamesList.Text = "Games List";
            // 
            // labelEnterGameName
            // 
            this.labelEnterGameName.AutoSize = true;
            this.labelEnterGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnterGameName.Location = new System.Drawing.Point(380, 41);
            this.labelEnterGameName.Name = "labelEnterGameName";
            this.labelEnterGameName.Size = new System.Drawing.Size(151, 20);
            this.labelEnterGameName.TabIndex = 10;
            this.labelEnterGameName.Text = "Enter game name";
            // 
            // textBoxGameName
            // 
            this.textBoxGameName.Location = new System.Drawing.Point(361, 74);
            this.textBoxGameName.Name = "textBoxGameName";
            this.textBoxGameName.Size = new System.Drawing.Size(184, 20);
            this.textBoxGameName.TabIndex = 14;
            this.textBoxGameName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxGameName.TextChanged += new System.EventHandler(this.TextBoxGameName_TextChanged);
            // 
            // buttonDeleteGame
            // 
            this.buttonDeleteGame.Enabled = false;
            this.buttonDeleteGame.Location = new System.Drawing.Point(461, 100);
            this.buttonDeleteGame.Name = "buttonDeleteGame";
            this.buttonDeleteGame.Size = new System.Drawing.Size(84, 24);
            this.buttonDeleteGame.TabIndex = 15;
            this.buttonDeleteGame.Text = "Delete";
            this.buttonDeleteGame.UseVisualStyleBackColor = true;
            this.buttonDeleteGame.Click += new System.EventHandler(this.ButtonDeleteGame_Click);
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMin.Location = new System.Drawing.Point(397, 182);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(28, 15);
            this.labelMin.TabIndex = 18;
            this.labelMin.Text = "Min";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMax.Location = new System.Drawing.Point(472, 182);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(31, 15);
            this.labelMax.TabIndex = 20;
            this.labelMax.Text = "Max";
            // 
            // checkBoxPeopleNumberLimit
            // 
            this.checkBoxPeopleNumberLimit.AutoSize = true;
            this.checkBoxPeopleNumberLimit.Enabled = false;
            this.checkBoxPeopleNumberLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPeopleNumberLimit.Location = new System.Drawing.Point(363, 143);
            this.checkBoxPeopleNumberLimit.Name = "checkBoxPeopleNumberLimit";
            this.checkBoxPeopleNumberLimit.Size = new System.Drawing.Size(182, 20);
            this.checkBoxPeopleNumberLimit.TabIndex = 21;
            this.checkBoxPeopleNumberLimit.Text = "Number of people limit";
            this.checkBoxPeopleNumberLimit.UseVisualStyleBackColor = true;
            this.checkBoxPeopleNumberLimit.CheckedChanged += new System.EventHandler(this.CheckBoxPeopleNumberLimit_CheckedChanged);
            this.checkBoxPeopleNumberLimit.Click += new System.EventHandler(this.CheckBoxPeopleNumberLimit_Click);
            // 
            // numericUpDownMin
            // 
            this.numericUpDownMin.Enabled = false;
            this.numericUpDownMin.Location = new System.Drawing.Point(384, 200);
            this.numericUpDownMin.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMin.Name = "numericUpDownMin";
            this.numericUpDownMin.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownMin.TabIndex = 23;
            this.numericUpDownMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownMax
            // 
            this.numericUpDownMax.Enabled = false;
            this.numericUpDownMax.Location = new System.Drawing.Point(462, 200);
            this.numericUpDownMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMax.Name = "numericUpDownMax";
            this.numericUpDownMax.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownMax.TabIndex = 24;
            this.numericUpDownMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // buttonSet
            // 
            this.buttonSet.Enabled = false;
            this.buttonSet.Location = new System.Drawing.Point(384, 226);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(139, 24);
            this.buttonSet.TabIndex = 25;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.ButtonSet_Click);
            // 
            // FormGamesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 262);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.numericUpDownMax);
            this.Controls.Add(this.numericUpDownMin);
            this.Controls.Add(this.checkBoxPeopleNumberLimit);
            this.Controls.Add(this.labelMax);
            this.Controls.Add(this.labelMin);
            this.Controls.Add(this.buttonDeleteGame);
            this.Controls.Add(this.textBoxGameName);
            this.Controls.Add(this.labelEnterGameName);
            this.Controls.Add(this.labelGamesList);
            this.Controls.Add(this.buttonAddGame);
            this.Controls.Add(this.listBoxGames);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormGamesList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Games List";
            this.Load += new System.EventHandler(this.FormGamesList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMax)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox listBoxGames;
        private System.Windows.Forms.Button buttonAddGame;
        private System.Windows.Forms.Label labelGamesList;
        private System.Windows.Forms.Label labelEnterGameName;
        private System.Windows.Forms.TextBox textBoxGameName;
        private System.Windows.Forms.Button buttonDeleteGame;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.CheckBox checkBoxPeopleNumberLimit;
        private System.Windows.Forms.NumericUpDown numericUpDownMin;
        private System.Windows.Forms.NumericUpDown numericUpDownMax;
        private System.Windows.Forms.Button buttonSet;
    }
}