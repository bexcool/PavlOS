using PavlOS.Core.Shell.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static PavlOS.Core.Shell.Utility.ShellUtil;

namespace PavlOS.Core.Shell.Utility
{
    public static class Styles
    {
        public static Pixel[] Button(uint Width, uint Height, ButtonState ButtonState)
        {
            List<Pixel> Pixels = new List<Pixel>();

            for (uint X = 0; X < Width - 1; X++)
            {
                Pixels.Add(new Pixel(X, 0, Color.FromArgb(230, 230, 230)));
                Pixels.Add(new Pixel(X, Height - 1, Color.Black));
            }

            Pixels.Add(new Pixel(Width -1, Height - 1, Color.Black));
            Pixels.Add(new Pixel(0, Height - 1, Color.Black));

            for (uint Y = 0; Y < Height - 1; Y++)
            {
                Pixels.Add(new Pixel(0, Y, Color.FromArgb(230, 230, 230)));
                Pixels.Add(new Pixel(Width - 1, Y, Color.Black));
            }

            for (uint Y = 1; Y < Height - 1; Y++)
                for (uint X = 1; X < Width - 1; X++)
                    Pixels.Add(new Pixel(X, Y, Color.FromArgb(180, 180, 180)));

            return Pixels.ToArray();
        }
    }
}
