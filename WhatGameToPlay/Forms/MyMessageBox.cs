using System;
using System.Drawing;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class MyMessageBox : Form
    {
        public MyMessageBox()
        {
            InitializeComponent();
        }

        private void MyMessageBox_Load(object sender, EventArgs e)
        {
            buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.DialogResult = DialogResult.Cancel;
        }

        private void SetButtonsVisibility(bool visible)
        {
            buttonYes.Visible = visible;
            buttonNo.Visible = visible;
            buttonOK.Visible = !visible;
            AcceptButton = visible ? buttonYes : buttonOK;
        }

        public DialogResult Show(string text)
        {
            return SetMessageBox(text, "", yesNoMessageBox: false);
        }

        public DialogResult Show(string text, string caption, MessageBoxButtons YesNo)
        {
            return SetMessageBox(text, caption, yesNoMessageBox: true);
        }

        private DialogResult SetMessageBox(string text, string caption, bool yesNoMessageBox)
        {
            labelMessage.Text = text;
            Text = caption;
            SetFormDimensions();
            SetButtonsVisibility(visible: yesNoMessageBox);
            if (yesNoMessageBox)
            {
                SetButtonLocation(buttonYes);
                SetButtonLocation(buttonNo);
            }
            else
            {
                SetButtonLocation(buttonOK);
            }
            RefreshTheme();
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

        private void RefreshTheme()
        {
            ThemeController.SetFormControlsTheme(form: this);
            Button[] allButtons = {
                buttonOK,
                buttonYes,
                buttonNo
            };
            ThemeController.SetButtonsColors(allButtons);
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
