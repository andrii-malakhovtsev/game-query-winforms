using System.Drawing;

namespace WhatGameToPlay
{
    public class Theme
    {
        public Theme(string name, Color textColor, Color buttonColor, Color backgroundColor, Color secondBackgroundColor)
        {
            Name = name;
            TextColor = textColor;
            ButtonColor = buttonColor;
            BackgroundColor = backgroundColor;
            SecondBackgroundColor = secondBackgroundColor;
        }

        public string Name { get; set; }

        public Color TextColor { get; set; }

        public Color ButtonColor { get; set; }

        public Color BackgroundColor { get; set; }

        public Color SecondBackgroundColor { get; set; }

        public static Theme Standard => new Theme(name: "White",
                                             textColor: Color.Black,
                                           buttonColor: Color.FromArgb(225, 225, 225),
                                       backgroundColor: Color.FromArgb(240, 240, 240),
                                 secondBackgroundColor: Color.White);
    }
}
