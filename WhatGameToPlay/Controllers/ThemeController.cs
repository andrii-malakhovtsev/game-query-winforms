using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public static class ThemeController
    {
        private static readonly List<System.Type> s_backColorClearTypes = 
            new List<System.Type>() { typeof(Label), typeof(GroupBox), typeof(CheckBox) };
        private static readonly string s_standartTheme = "White";
        private static string s_currentTheme;
        private static Color s_textColor;
        private static Color s_buttonColor;
        private static Color s_backgroundColor;
        private static Color s_secondBackgroundColor;

        private static void RefreshCurrentThemeFromFile()
        {
            s_currentTheme = FilesController.GetCurrentTheme();
        }

        private static void SetThemeColors(Color textColor, Color buttonColor, 
            Color backgroundColor, Color secondBackgroundColor)
        {
            s_textColor = textColor;
            s_buttonColor = buttonColor;
            s_backgroundColor = backgroundColor;
            s_secondBackgroundColor = secondBackgroundColor;
        }

        public static void SetTextBoxForeColor(TextBox textbox, bool win)
        {
            textbox.ForeColor = win ? Color.Green : s_textColor;
        }

        public static string GetStandartThemeName()
        {
            return s_standartTheme;
        }

        private static bool IsCurrentThemeStandart()
        {
            return s_currentTheme == s_standartTheme;
        }

        public static void SetChosenThemeColors()
        {
            RefreshCurrentThemeFromFile();
            switch (s_currentTheme)
            {
                case "White":
                    SetThemeColors(textColor: Color.Black,
                                 buttonColor: Color.FromArgb(225, 225, 225),
                             backgroundColor: Color.FromArgb(240, 240, 240),
                       secondBackgroundColor: Color.White);
                    break;
                case "Dark":
                    SetThemeColors(textColor: Color.White,
                                 buttonColor: Color.FromArgb(56, 52, 57),
                             backgroundColor: Color.FromArgb(70, 65, 72),
                       secondBackgroundColor: Color.FromArgb(50, 46, 52));
                    break;
                case "Telegram":
                    SetThemeColors(textColor: Color.FromArgb(245, 245, 245),
                                 buttonColor: Color.FromArgb(32, 43, 54),
                             backgroundColor: Color.FromArgb(23, 33, 43),
                       secondBackgroundColor: Color.FromArgb(14, 22, 33));
                    break;
                case "Discord":
                    SetThemeColors(textColor: Color.FromArgb(241, 236, 235),
                                 buttonColor: Color.FromArgb(66, 70, 77),
                             backgroundColor: Color.FromArgb(54, 57, 63),
                       secondBackgroundColor: Color.FromArgb(47, 49, 54));
                    break;
                case "YouTube":
                    SetThemeColors(textColor: Color.FromArgb(254, 254, 254),
                                 buttonColor: Color.FromArgb(56, 56, 56),
                             backgroundColor: Color.FromArgb(33, 33, 33),
                       secondBackgroundColor: Color.FromArgb(24, 24, 24));
                    break;
            }
        }

        public static void SetTextForeColor(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.ForeColor = s_textColor;
        }

        public static void SetBackgroundForeColor(ToolStripMenuItem toolStripMenuItem)
        {
            RefreshCurrentThemeFromFile();
            if (!IsCurrentThemeStandart()) toolStripMenuItem.ForeColor = s_backgroundColor;
        }

        public static void SetToolStripMenuItemsFullColor(List<ToolStripMenuItem> toolStripMenuItems)
        {
            foreach (ToolStripMenuItem toolStripMenuItem in toolStripMenuItems)
            {
                toolStripMenuItem.ForeColor = s_textColor;
                toolStripMenuItem.BackColor = s_secondBackgroundColor;
            }
        }

        public static void SetButtonsBackColor(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                button.ForeColor = s_textColor;
                button.BackColor = s_secondBackgroundColor;
            }
        }

        public static void SetFormControlsTheme(Form form)
        {
            form.BackColor = s_backgroundColor;
            foreach (Control control in form.Controls)
            {
                System.Type controlType = control.GetType();
                control.ForeColor = s_textColor;
                if (!s_backColorClearTypes.Contains(controlType))
                    control.BackColor = controlType == typeof(Button) ?
                        s_buttonColor : s_secondBackgroundColor;
            }
        }
    }
}
