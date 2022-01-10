using Mosa.External.x86.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PavlOS.Core.Shell.Rendering
{
    public class GraphicsDriver
    {

        public static int Width { get; set; }
        public static int Height { get; set; }
        public static Graphics Graphics;

        public static void Init(int Width, int Height)
        {
            Graphics = GraphicsSelector.GetGraphics(Width, Width);

            GraphicsDriver.Width = Graphics.Width;
            GraphicsDriver.Height = Graphics.Height;
        }
    }
}
