using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class AdvancedMessageBox : Form
    {
        private readonly FilesReader _filesReader;

        public AdvancedMessageBox(FilesReader filesReader)
        {
            InitializeComponent();
            _filesReader = filesReader;
        }

        private void AdvancedMessageBox_Load(object sender, EventArgs e)
        {
            buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.DialogResult = DialogResult.Cancel;
            if (!_filesReader.StandartFilesExist) CenterToScreen();
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
            SetFormButtons(yesNoMessageBox);

            if (!_filesReader.StandartFilesExist)
                SetTimerRelatedControlsEnables(enable: true);
            else RefreshTheme();

            return ShowDialog();
        }

        private void SetFormButtons(bool yesNoMessageBox)
        {
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
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!AdvancedMessageBoxModel.TimerTick(ref labelTimer))
                SetTimerRelatedControlsEnables(enable: false);
        }

        private void SetTimerRelatedControlsEnables(bool enable)
        {
            buttonYes.Enabled = !enable;
            labelTimer.Visible = enable;
            timer.Enabled = enable;
        }

        private void SetButtonLocation(Button button)
        {
            AdvancedMessageBoxModel.SetButtonLocation(ref button, buttonYes, panel.Height, Width);
        }

        private void SetFormDimensions()
        {
            Width = AdvancedMessageBoxModel.GetFormWidth(labelMessage.Width);
            Height = AdvancedMessageBoxModel.GetFormHeight(labelMessage.Height);
        }

        private void RefreshTheme()
        {
            FormsTheme.ColorControls(form: this);

            var allButtons = new HashSet<Button>();
            GetAllButtons(Controls, allButtons);
            FormsTheme.ColorButtons(allButtons);
        }

        private void GetAllButtons(Control.ControlCollection controls, HashSet<Button> buttons)
        {
            foreach (Control control in controls)
            {
                if (control is Button button)
                {
                    buttons.Add(button);
                }

                if (control.HasChildren)
                {
                    GetAllButtons(control.Controls, buttons);
                }
            }
        }

        private void ButtonConfirm_Click(object sender, EventArgs e) => Close();
    }
}
