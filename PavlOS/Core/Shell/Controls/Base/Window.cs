using PavlOS.Core;
using PavlOS.Core.Shell;
using PavlOS.Core.Shell.Controls;
using PavlOS.Core.Shell.Controls.Base;
using PavlOS.Core.Shell.Rendering;
using PavlOS.Core.Shell.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static PavlOS.Core.Shell.Utility.ShellUtil;

namespace PavlOS_Dev.Core.Shell.Controls.Base
{
    public class Window : Control
    {
        // Window
        public Color BackgroundColor { get; set; }
        public string Handle { get; private set; }
        public Point MouseDelta;
        public bool WindowDrag = false, WindowResizeX = false, WindowResizeY = false;
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public const int BorderWeight = 3;
        public const int ResizeBorderWeight = 6;

        // Children (controls)
        public List<Control> Controls = new List<Control>();
        
        // Title bar
        public string Title { get; set; }
        public const int TitleBarHeight = 17;

        public Window()
        {
            Handle = SystemUtilities.GenerateID(20);

            BackgroundColor = Color.White;
            Title = "New Window";

            X = 100;
            Y = 100;
            Width = 200;
            Height = 100;
            MinWidth = 100;
            MinHeight = 100;

            InitNavButtons();

            ShellCore.AddWindow(this);
        }

        private void InitNavButtons()
        {
            Button CloseBtn = new Button();
            CloseBtn.Width = 14;
            CloseBtn.Height = TitleBarHeight - 4;
            CloseBtn.X = Width - CloseBtn.Width - BorderWeight * 2 - 2;
            CloseBtn.Y = -CloseBtn.Height - 2;
            CloseBtn.OnClick += (o) => Close();
            //CloseBtn.X = Width - CloseBtn.Width - BorderWeight - 2;
            //CloseBtn.Y = BorderWeight + 2;
            //CloseBtn.Tag = "-!validpos";

            AddControl(CloseBtn);
        }

        public void Center()
        {
            X = GraphicsDriver.Width / 2 - Width / 2;
            Y = GraphicsDriver.Height / 2 - Height / 2;
        }

        public void Close()
        {
            ShellCore.RemoveWindow(this);
        }

        public void AddControl(Control Control) => Controls.Add(Control);
        public void RemoveControl(Control Control) => Controls.Remove(Control);

        private void MoveToTop()
        {
            if (IndexZ == ShellCore.AllWindows.Count - 1) return;

            int ThisIndexZ = IndexZ;
            foreach (Window window in ShellCore.AllWindows)
            {
                if (window.IndexZ > ThisIndexZ) window.IndexZ--;
            }

            IndexZ = ShellCore.AllWindows.Count - 1;
        }

        public void Focus()
        {
            // Move to top in renderer
            MoveToTop();

            // Set focus
            ShellCore.FocusedWindow = this;

            // Events
            _Focused();
            _OnClick();
            _MousePressed();
        }
    }
}
