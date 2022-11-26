using System;
using System.Drawing;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class MyMessageBox : Form
    {
        private readonly ThemeController _theme;

        public MyMessageBox(ThemeController theme)
        {
            _theme = theme;
            InitializeComponent();
        }

        private void MyMessageBox_Load(object sender, EventArgs e)
        {
            buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.DialogResult = DialogResult.Cancel;
        }

        private void SetButtonsEnables(bool enable)
        {
            buttonYes.Visible = enable;
            buttonNo.Visible = enable;
            buttonOK.Visible = !enable;
        }

        public DialogResult Show(string text)
        {
            labelMessage.Text = text;
            Text = "";
            SetFormDimensions();
            SetButtonsEnables(enable: false);
            SetButtonLocation(buttonOK);
            RefreshColors();
            return ShowDialog();
        }

        public DialogResult Show(string text, string caption, MessageBoxButtons YesNo)
        {
            labelMessage.Text = text;
            Text = caption;
            SetFormDimensions();
            SetButtonsEnables(enable: true);
            SetButtonLocation(buttonYes);
            SetButtonLocation(buttonNo);
            RefreshColors();
            return ShowDialog();
        }

        private void SetButtonLocation(Button button)
        {
            int buttonHeight = panel.Height / 2 - 10,
                lastButtonX = 105, firstButtonX = lastButtonX + 85,
                buttonWidth = Width;
            buttonWidth -= button == buttonYes ? firstButtonX : lastButtonX;
            button.Location = new Point(buttonWidth, buttonHeight);
        }

        private void SetFormDimensions()
        {
            int widthAfterLabel = 60, heightAfterLabel = 135;
            Width = labelMessage.Width + widthAfterLabel;
            Height = labelMessage.Height + heightAfterLabel;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RefreshColors()
        {
            Button[] allButtons = {
                buttonOK,
                buttonYes,
                buttonNo
            };
            _theme.SetButtonsColor(allButtons);
            _theme.SetBackgroundForeColor(labelMessage);
            _theme.SetTextForeColor(labelMessage);
            _theme.SetFormWithPanelBackgroundColor(form: this, panel);
        }
    }
}
