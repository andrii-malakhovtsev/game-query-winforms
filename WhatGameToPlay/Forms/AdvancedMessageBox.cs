using System;
using System.Drawing;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class AdvancedMessageBox : Form
    {
        public AdvancedMessageBox()
        {
            InitializeComponent();
        }

        private void AdvancedMessageBox_Load(object sender, EventArgs e)
        {
            buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.DialogResult = DialogResult.Cancel;
            if (!FilesReader.StandartFilesExist) CenterToScreen();
        }

        private void SetButtonsVisibility(bool visible)
        {
            buttonYes.Visible = visible;
            buttonNo.Visible = visible;
            buttonOK.Visible = !visible;
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
            AcceptButton = yesNoMessageBox ? buttonYes : buttonOK;
            if (yesNoMessageBox)
            {
                SetButtonLocation(buttonYes);
                SetButtonLocation(buttonNo);
            }
            else
            {
                SetButtonLocation(buttonOK);
            }
            if (!FilesReader.StandartFilesExist)
                SetTimerRelatedControlsEnables(enable: true);
            else RefreshTheme();
            return ShowDialog();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int secondsLeft = Convert.ToInt32(labelTimer.Text);
            if (secondsLeft != 0)
            {
                secondsLeft--;
                labelTimer.Text = Convert.ToString(secondsLeft);
            }
            else SetTimerRelatedControlsEnables(enable: false);
        }

        private void SetTimerRelatedControlsEnables(bool enable)
        {
            buttonYes.Enabled = !enable;
            labelTimer.Visible = enable;
            timer.Enabled = enable;
        }

        private void SetButtonLocation(Button button)
        {
            const int lastButtonX = 105, firstButtonX = lastButtonX + 85;
            int buttonHeight = panel.Height / 2 - 10, buttonWidth = Width;
            buttonWidth -= button == buttonYes ? firstButtonX : lastButtonX;
            button.Location = new Point(buttonWidth, buttonHeight);
        }

        private void SetFormDimensions()
        {
            const int widthAfterLabel = 60, heightAfterLabel = 135;
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
            ThemeController.SetButtonsFullColor(allButtons);
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
