using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WhatGameToPlay
{
    public partial class MyMessageBox : Form
    {
        private readonly MainForm _mainForm;
        private Color _colorWhite;
        private Color _colorBlack;
        private Color _colorBlackLighter;
        private Color _buttonColor;
        //private Color _colorWhite = Color.Black;
        //private Color _colorBlack = Color.White;
        //private Color _colorBlackLighter = Color.FromArgb(240, 240, 240);
        //private Color _buttonColor = Color.FromArgb(225, 225, 225);

        public MyMessageBox(MainForm MainForm)
        {
            _mainForm = MainForm;
            InitializeComponent();
        }

        private void RefreshColors()
        {
            _colorBlack = _mainForm._colorTexts;
            _colorBlackLighter = _mainForm._colorTextsLighter;
            _colorWhite = _mainForm._colorBackgrounds;
            _buttonColor = _mainForm._buttonColor;
            string[] lines = File.ReadAllLines("Theme.txt");
            List<Button> listOfButtons = new List<Button>()
            {
                buttonOK,
                buttonYes,
                buttonNo
            };
            foreach(Button button in listOfButtons)
            {
                button.ForeColor = _colorWhite;
                button.BackColor = _buttonColor;
            }
            labelMessage.ForeColor = _colorWhite;
            string savedTheme = lines[0];
            if (savedTheme != "White")
            {
                BackColor = _colorBlackLighter;
                panel.BackColor = _colorBlack;
            }
            else
            {
                BackColor = _colorBlack;
                panel.BackColor = _colorBlackLighter;
            }
        }

        private void MyMessageBox_Load(object sender, EventArgs e)
        {
            buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.DialogResult = DialogResult.Cancel;
        }

        public DialogResult Show(string text)
        {
            labelMessage.Text = text;
            Text = "";
            Width = labelMessage.Width + 60;
            Height = labelMessage.Height + 135;
            buttonNo.Visible = false;
            buttonYes.Visible = false;
            buttonOK.Visible = true;
            buttonOK.Location = new Point(Width - 105, panel.Height / 2 - 10);
            RefreshColors();
            return ShowDialog();
        }

        public DialogResult Show(string text, string caption, MessageBoxButtons YesNo)
        {
            labelMessage.Text = text;
            Text = caption;
            Width = labelMessage.Width + 60;
            Height = labelMessage.Height + 135;
            buttonNo.Visible = true;
            buttonYes.Visible = true;
            buttonOK.Visible = false;
            buttonYes.Location = new Point(Width - 190, panel.Height / 2 - 10);
            buttonNo.Location = new Point(Width - 105, panel.Height / 2 - 10);
            RefreshColors();
            return ShowDialog();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
