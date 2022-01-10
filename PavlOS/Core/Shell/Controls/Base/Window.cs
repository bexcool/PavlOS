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

namespace PavlOS_Dev.Core.Shell.Controls
{
    public class Window
    {
        private int _X;
        public int X
        {
            get { return _X; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _X = value;
            }
        }

        private int _Y;
        public int Y
        {
            get { return _Y; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Y = value;
            }
        }

        private int _Width = 1;
        public int Width
        {
            get { return _Width; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Width = value;
            }
        }

        private int _Height = 1;
        public int Height
        {
            get { return _Height; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Height = value;
            }
        }

        private Thickness _Padding = new Thickness();
        public Thickness Padding
        {
            get { return _Padding; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Padding = value;
            }
        }

        private Thickness _Margin = new Thickness();
        public Thickness Margin
        {
            get { return _Margin; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Margin = value;
            }
        }

        private Line _Line;
        public Line Line
        {
            get { return _Line; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Line = value;
            }
        }

        private HorizontalAlignment _ContentHorizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment ContentHorizontalAlignment
        {
            get { return _ContentHorizontalAlignment; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _ContentHorizontalAlignment = value;
            }
        }

        private VerticalAlignment _ContentVerticalAlignment = VerticalAlignment.Center;
        public VerticalAlignment ContentVerticalAlignment
        {
            get { return _ContentVerticalAlignment; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _ContentVerticalAlignment = value;
            }
        }

        private ConsoleColor _ForegroundColor = ConsoleColor.White;
        public ConsoleColor ForegroundColor
        {
            get { return _ForegroundColor; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _ForegroundColor = value;
            }
        }

        private Visibility _Visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _Visibility; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Visibility = value;
            }
        }

        internal bool ValueChanged = false, ChangingByCore = false;
        public bool Hovered = false, Pressed = false;

        // Window handle
        public string Handle { get; set; }

        // Title bar
        public string Title { get; set; }

        // Background
        public Color BackgroundColor { get; set; }

        #region Event handlers
        // Click handler
        public delegate void ClickHandler(object sender);
        public event ClickHandler OnClick;
        // Mouse enter handler
        public delegate void MouseEnterHandler(object sender);
        public event MouseEnterHandler MouseEnter;
        // Mouse enter leave
        public delegate void MouseLeaveHandler(object sender);
        public event MouseLeaveHandler MouseLeave;
        // Mouse pressed handler
        public delegate void MousePressedHandler(object sender);
        public event MousePressedHandler MousePressed;
        // Mouse released leave
        public delegate void MouseReleasedHandler(object sender);
        public event MouseReleasedHandler MouseReleased;
        // Focused
        public delegate void FocusedHandler(object sender);
        public event FocusedHandler Focused;
        // Content changed
        public delegate void ContentChangedHandler(object sender);
        public event ContentChangedHandler ContentChanged;
        #endregion
        public Window()
        {
            Handle = SystemUtilities.GenerateID(20);

            BackgroundColor = Color.White;
            Title = "New Window";

            X = 200;
            Y = 300;
            Width = 200;
            Height = 100;

            ShellCore.AddWindow(this);
        }

        // Called when any value changes
        internal void _ValueChanged()
        {
            if (!ValueChanged)
            {
                ValueChanged = true;
            }
        }

        // Events
        internal void _OnClick()
        {
            if (OnClick == null) return;

            OnClick(this);
        }
        internal void _MouseEnter()
        {
            if (MouseEnter == null) return;

            MouseEnter(this);
        }
        internal void _MouseLeave()
        {
            if (MouseLeave == null) return;

            MouseLeave(this);
        }
        internal void _MousePressed()
        {
            if (MousePressed == null) return;

            MousePressed(this);
        }
        internal void _MouseReleased()
        {

            if (MouseReleased == null) return;

            MouseReleased(this);
        }
        internal void _Focused()
        {
            if (Focused == null) return;

            Focused(this);
        }
        internal void _ContentChanged()
        {
            if (ContentChanged == null) return;

            ContentChanged(this);
        }

        // Public functions
        /// <summary>
        /// Remove control.
        /// </summary>
        public void Remove()
        {
            ShellCore.RemoveWindow(this);
        }

        public void Center()
        {
            X = GraphicsDriver.Width / 2 - Width / 2;
            Y = GraphicsDriver.Height / 2 - Height / 2;
        }
    }
}
