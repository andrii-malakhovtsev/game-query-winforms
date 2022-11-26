using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class ThemeController
    {
        private static readonly string s_standartTheme = "White";
        private Color _backgroundColor;
        private Color _secondBackgroundColor;
        private Color _textColor;
        private Color _buttonsColor;
        private string _currentTheme;

        private void SetThemeColors(Color textsColor, Color secondBackgoroundColor, Color backgroundsColor,
           Color buttonsColor)
        {
            _textColor = textsColor;
            _secondBackgroundColor = secondBackgoroundColor;
            _backgroundColor = backgroundsColor;
            _buttonsColor = buttonsColor;
        }

        public void SetTextBoxForeColor(TextBox textbox, bool win)
        {
            textbox.ForeColor = win ? Color.Green : _textColor;
        }

        public static string GetStandartThemeName()
        {
            return s_standartTheme;
        }

        private void RefreshCurrentThemeFromFile()
        {
            _currentTheme = FilesController.GetCurrentTheme();
        }

        public void SetChosenThemeColors()
        {
            RefreshCurrentThemeFromFile();
            switch (_currentTheme)
            {
                case "White":
                    SetThemeColors(Color.Black, Color.White, Color.FromArgb(240, 240, 240),
                    Color.FromArgb(225, 225, 225));
                    break;
                case "Dark":
                    SetThemeColors(Color.White, Color.FromArgb(50, 46, 52), Color.FromArgb(70, 65, 72),
                        Color.FromArgb(56, 52, 57));
                    break;
                case "Telegram":
                    SetThemeColors(Color.FromArgb(245, 245, 245), Color.FromArgb(14, 22, 33),
                        Color.FromArgb(23, 33, 43), Color.FromArgb(32, 43, 54));
                    break;
                case "Discord":
                    SetThemeColors(Color.FromArgb(241, 236, 235), Color.FromArgb(47, 49, 54),
                        Color.FromArgb(54, 57, 63), Color.FromArgb(66, 70, 77));
                    break;
                case "YouTube":
                    SetThemeColors(Color.FromArgb(254, 254, 254), Color.FromArgb(24, 24, 24),
                        Color.FromArgb(33, 33, 33), Color.FromArgb(56, 56, 56));
                    break;
            }
        }

        public void SetBackgroundForeColor(Control control)
        {
            control.ForeColor = _backgroundColor;
        }

        public void SetTextForeColor(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.ForeColor = _textColor;
        }

        public void SetToolStripBackColor(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.BackColor = _textColor;
        }

        public void SetControlsForeColor(Control[] controls)
        {
            foreach (Control control in controls)
                control.ForeColor = _textColor;
        }

        public void SetControlsForeColor(List<Control> controls)
        {
            foreach (Control control in controls)
                control.ForeColor = _textColor;
        }

        public void SetSecondBackgroundBackColor(Control[] controls)
        {
            foreach (Control control in controls)
                control.BackColor = _secondBackgroundColor;
        }

        public void SetTextForeColor(Label label)
        {
            label.ForeColor = _textColor;
        }

        public void SetTextForeColor(Label[] labels)
        {
            foreach (Label label in labels) SetTextForeColor(label);
        }

        public void SetBackgroundForeColor(ToolStripMenuItem toolStripMenuItem)
        {
            RefreshCurrentThemeFromFile();
            if (CurrentThemeIsNotStandart()) toolStripMenuItem.ForeColor = _backgroundColor;
        }

        public void SetControlsFullColor(Control[] controls)
        {
            SetControlsForeColor(controls);
            SetSecondBackgroundBackColor(controls);
        }

        public void SetToolStripMenuItemsFullColor(List<ToolStripMenuItem> toolStripMenuItems)
        {
            foreach (ToolStripMenuItem toolStripMenuItem in toolStripMenuItems)
            {
                toolStripMenuItem.ForeColor = _textColor;
                toolStripMenuItem.BackColor = _secondBackgroundColor;
            }
        }

        public void SetFormBackgroundColor(Form form)
        {
            form.BackColor = _backgroundColor;
        }

        private bool CurrentThemeIsNotStandart()
        {
            return _currentTheme != s_standartTheme;
        }

        public void SetFormWithPanelBackgroundColor(Form form, Panel panel)
        {
            RefreshCurrentThemeFromFile();
            if (CurrentThemeIsNotStandart())
            {
                form.BackColor = _backgroundColor;
                panel.BackColor = _secondBackgroundColor;
            }
            else
            {
                form.BackColor = _secondBackgroundColor;
                panel.BackColor = _backgroundColor;
            }
        }

        public void SetButtonsColor(Button[] buttons)
        {
            foreach (Button button in buttons)
                SetButtonColor(button);
        }

        public void SetButtonColor(Button button)
        {
            button.ForeColor = _textColor;
            button.BackColor = _buttonsColor;
        }
    }
}
