using System.Drawing;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class Theme
    {
        private Color _colorBackgrounds = Color.Black;
        private Color _colorTexts = Color.White;
        private Color _colorTextsLighter = Color.FromArgb(240, 240, 240);
        private Color _buttonColor = Color.FromArgb(225, 225, 225);
        //public Color ColorWhite = Color.Black;
        //public Color ColorBlack = Color.White;
        //public Color ColorBlackLighter = Color.FromArgb(240, 240, 240);
        //public Color ButtonColor = Color.FromArgb(225, 225, 225);

        public void SetControlsBackgroundsColor(Control[] controls)
        {
            foreach (Control control in controls)
                control.BackColor = _colorBackgrounds;
        }
    }
}
