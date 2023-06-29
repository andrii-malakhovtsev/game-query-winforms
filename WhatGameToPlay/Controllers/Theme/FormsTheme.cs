using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public static class FormsTheme
    {
        private static Theme _theme = Theme.Standart;

        private static string CurrentThemeName => FilesReader.CurrentThemeFromFile;

        private static HashSet<Theme> Themes { get; } = new HashSet<Theme>()
        {
            Theme.Standart,
            new Theme(name: "Dark",
                          textColor: Color.White,
                        buttonColor: Color.FromArgb(56, 52, 57),
                    backgroundColor: Color.FromArgb(70, 65, 72),
              secondBackgroundColor: Color.FromArgb(50, 46, 52)),
            new Theme(name: "Telegram",
                          textColor: Color.FromArgb(245, 245, 245),
                        buttonColor: Color.FromArgb(32, 43, 54),
                    backgroundColor: Color.FromArgb(23, 33, 43),
              secondBackgroundColor: Color.FromArgb(14, 22, 33)),
            new Theme(name: "Discord",
                          textColor: Color.FromArgb(241, 236, 235),
                        buttonColor: Color.FromArgb(66, 70, 77),
                    backgroundColor: Color.FromArgb(54, 57, 63),
              secondBackgroundColor: Color.FromArgb(47, 49, 54)),
            new Theme(name: "YouTube",
                          textColor: Color.FromArgb(254, 254, 254),
                        buttonColor: Color.FromArgb(56, 56, 56),
                    backgroundColor: Color.FromArgb(33, 33, 33),
              secondBackgroundColor: Color.FromArgb(24, 24, 24))
        };

        public static void ColorTextBox(TextBox textbox, bool win)
            => textbox.ForeColor = win ? Color.Green : _theme.TextColor;

        public static void ColorToolStripMenuItemDropDowns(ToolStripMenuItem toolStripMenuItem)
           => toolStripMenuItem.ForeColor = _theme.TextColor;

        public static void SetChosenThemeColors() 
        {
            foreach (Theme theme in Themes)
            {
                if (theme.Name == CurrentThemeName)
                    _theme = theme;
            }
        }

        public static void ColorToolStripMenuItem(ToolStripMenuItem toolStripMenuItem)
        {
            if (CurrentThemeName != Theme.Standart.Name)
            {
                toolStripMenuItem.ForeColor = _theme.BackgroundColor;
            }
        }

        public static void ColorToolStripMenuItems(IEnumerable<ToolStripMenuItem> toolStripMenuItems)
        {
            foreach (ToolStripMenuItem toolStripMenuItem in toolStripMenuItems)
            {
                ColorToolStripMenuItemDropDowns(toolStripMenuItem);
                toolStripMenuItem.BackColor = _theme.SecondBackgroundColor;
            }
        }

        public static void ColorButtons(IEnumerable<Button> buttons)
        {
            foreach (Button button in buttons)
            {
                button.ForeColor = _theme.TextColor;
                button.BackColor = _theme.ButtonColor;
            }
        }

        public static void ColorControls(Form form)
        {
            form.BackColor = _theme.BackgroundColor;

            var backColorClearTypes = new List<System.Type>()
            {
                typeof(Label),
                typeof(GroupBox),
                typeof(CheckBox)
            };
            foreach (Control control in form.Controls)
            {
                System.Type controlType = control.GetType();
                control.ForeColor = _theme.TextColor;
                // if add typeof(PictureBox) to backColorClearTypes - program detects as a virus
                if (!backColorClearTypes.Contains(controlType) && controlType != typeof(PictureBox))
                {
                    control.BackColor = controlType == typeof(Button) ?
                        _theme.ButtonColor : _theme.SecondBackgroundColor;
                }
            }
            backColorClearTypes.Clear();
        }
    }
}
