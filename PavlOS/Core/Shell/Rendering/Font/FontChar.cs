using System.Drawing;

namespace PavlOS.Core.Shell.Rendering.FontRendering
{
    public class FontChar
    {
        public Point[] Pixels;
        public char Char;
        public int Width, Height;

        public FontChar(char Char, int Width, int Height, Point[] Pixels)
        {
            this.Char = Char;
            this.Pixels = Pixels;
            this.Width = Width;
            this.Height = Height;
        }
    }
}