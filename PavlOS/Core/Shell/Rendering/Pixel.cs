using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PavlOS.Core.Shell.Rendering
{
    public class Pixel
    {
        public uint X { get; set; }
        public uint Y { get; set; }
        public Color Color { get; set; }

        public Pixel(uint X, uint Y, Color Color)
        {
            this.X = X;
            this.Y = Y;
            this.Color = Color;
        }
    }
}
