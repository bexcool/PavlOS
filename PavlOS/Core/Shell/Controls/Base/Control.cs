using PavlOS.Core.Shell.Rendering.FontRendering;
using PavlOS.Core.Shell.Rendering.FontRendering.Libraries;
using PavlOS.Core.Shell.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using static PavlOS.Core.Shell.Utility.Utility;

namespace PavlOS.Core.Shell.Controls.Base
{
    public class Control
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

        private PixelFont _Font = new PixelFont(new FontPixel(), 1);
        public PixelFont Font
        {
            get { return _Font; }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Font = value;
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

        private string _Content = "";
        public string Content
        {
            get { return _Content; }

            set
            {
                _ValueChanged();
                _Content = value;
            }
        }

        private Control _Parent;
        public Control Parent
        {
            get
            {
                return _Parent;
            }

            set
            {
                if (!ChangingByCore) _ValueChanged();
                _Parent = value;
            }
        }

        public bool ValueChanged = false, ChangingByCore = false, RemoveRequest = false, ParentChanged = false, BlankApplied = true;
        public bool Hovered = false, Pressed = false;

        public Rectangle Old;

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

        public Control()
        {
            Old = new Rectangle(X, Y, CalculateActualWidth(), CalculateActualHeight());

            ShellCore.AddControl(this);
        }

        // Called when any value changes
        public void _ValueChanged()
        {
            if (!ValueChanged)
            {
                Old = new Rectangle(X, Y, CalculateActualWidth(), CalculateActualHeight());

                ValueChanged = true;
                BlankApplied = false;
            }
        }

        // Events
        public void _OnClick()
        {
            if (OnClick == null) return;

            OnClick(this);
        }
        public void _MouseEnter()
        {
            if (MouseEnter == null) return;

            MouseEnter(this);
        }
        public void _MouseLeave()
        {
            if (MouseLeave == null) return;

            MouseLeave(this);
        }
        public void _MousePressed()
        {
            if (MousePressed == null) return;

            MousePressed(this);
        }
        public void _MouseReleased()
        {

            if (MouseReleased == null) return;

            MouseReleased(this);
        }
        public void _Focused()
        {
            if (Focused == null) return;

            Focused(this);
        }
        public void _ContentChanged()
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
            ShellCore.RemoveControl(this);
        }

        /// <summary>
        /// Calculates controls actual width.
        /// </summary>
        /// <returns>Actual width.</returns>
        public int CalculateActualWidth()
        {
            return Width + Padding.LeftRight;
        }

        /// <summary>
        /// Calculates controls actual height.
        /// </summary>
        /// <returns>Actual height.</returns>
        public int CalculateActualHeight()
        {
            return Height + Padding.TopBottom;
        }
    }
}
