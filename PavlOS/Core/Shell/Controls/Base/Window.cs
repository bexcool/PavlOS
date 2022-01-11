using PavlOS.Core;
using PavlOS.Core.Shell;
using PavlOS.Core.Shell.Controls.Base;
using PavlOS.Core.Shell.Rendering;
using PavlOS.Core.Shell.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static PavlOS.Core.Shell.Utility.Utility;

namespace PavlOS_Dev.Core.Shell.Controls.Base
{
    public class Window : Control
    {
        // Window
        public Color BackgroundColor { get; set; }
        public string Handle { get; private set; }
        public Point MouseDelta;

        // Title bar
        public string Title { get; set; }

        public Window()
        {
            Handle = SystemUtilities.GenerateID(20);

            BackgroundColor = Color.White;
            Title = "New Window";

            X = 100;
            Y = 100;
            Width = 200;
            Height = 100;

            ShellCore.AddControl(this);
        }

        public void Center()
        {
            X = GraphicsDriver.Width / 2 - Width / 2;
            Y = GraphicsDriver.Height / 2 - Height / 2;
        }
    }
}
