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
        private static Color s_backgroundColor;
        private static Color s_secondBackgroundColor;
        private static Color s_textColor;
        private static Color s_buttonsColor;

        private static void RefreshCurrentThemeFromFile()
        {
            s_currentTheme = FilesController.GetCurrentTheme();
        }

        private static void SetThemeColors(Color textsColor, Color secondBackgoroundColor, 
            Color backgroundsColor, Color buttonsColor)
        {
            s_textColor = textsColor;
            s_secondBackgroundColor = secondBackgoroundColor;
            s_backgroundColor = backgroundsColor;
            s_buttonsColor = buttonsColor;
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
                    SetThemeColors(textsColor: Color.Black,
                       secondBackgoroundColor: Color.White,
                             backgroundsColor: Color.FromArgb(240, 240, 240),
                                 buttonsColor: Color.FromArgb(225, 225, 225));
                    break;
                case "Dark":
                    SetThemeColors(textsColor: Color.White,
                       secondBackgoroundColor: Color.FromArgb(50, 46, 52),
                             backgroundsColor: Color.FromArgb(70, 65, 72),
                                 buttonsColor: Color.FromArgb(56, 52, 57));
                    break;
                case "Telegram":
                    SetThemeColors(textsColor: Color.FromArgb(245, 245, 245),
                       secondBackgoroundColor: Color.FromArgb(14, 22, 33),
                             backgroundsColor: Color.FromArgb(23, 33, 43),
                                 buttonsColor: Color.FromArgb(32, 43, 54));
                    break;
                case "Discord":
                    SetThemeColors(textsColor: Color.FromArgb(241, 236, 235),
                       secondBackgoroundColor: Color.FromArgb(47, 49, 54),
                             backgroundsColor: Color.FromArgb(54, 57, 63),
                                 buttonsColor: Color.FromArgb(66, 70, 77));
                    break;
                case "YouTube":
                    SetThemeColors(textsColor: Color.FromArgb(254, 254, 254),
                       secondBackgoroundColor: Color.FromArgb(24, 24, 24), 
                             backgroundsColor: Color.FromArgb(33, 33, 33), 
                                 buttonsColor: Color.FromArgb(56, 56, 56));
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

        public static void SetButtonsColors(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                button.ForeColor = s_textColor;
                button.BackColor = s_buttonsColor;
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
                        s_buttonsColor : s_secondBackgroundColor;
            }
        }
    }
}
