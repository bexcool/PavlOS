using Mosa.External.x86.Driver;
using Mosa.External.x86.Driver.Input;
using PavlOS.Core.Shell;
using PavlOS.Core.Shell.Controls.Base;
using PavlOS.Core.Shell.Rendering;
using PavlOS_Dev.Core.Shell.Controls.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PavlOS.Core
{
    public class InputListener
    {
        public static bool LeftMousePressed;
        private Point LastMousePos;

        public void CheckInput()
        {
            foreach (Window window in ShellCore.AllWindows)
            {
                #region Window Drag
                // Check title bar
                // Pressing
                if (    PS2Mouse.X >= window.X + 3 &&
                        PS2Mouse.X <= window.X + window.Width - 4 &&
                        PS2Mouse.Y >= window.Y + 3 &&
                        PS2Mouse.Y <= window.Y + 17 &&
                        PS2Mouse.MouseStatus == MouseStatus.Left)
                {
                    if (!window.Pressed)
                    {
                        window.Pressed = true;
                        window.MouseDelta = new Point(PS2Mouse.X - window.X, PS2Mouse.Y - window.Y);
                    }
                }
                else
                {
                    if (window.Pressed)
                    {
                        window.Pressed = false;
                    }
                }

                // Drag
                if (   (PS2Mouse.X >= window.X + 3 &&
                        PS2Mouse.X <= window.X + window.Width - 4 &&
                        PS2Mouse.Y >= window.Y + 3 &&
                        PS2Mouse.Y <= window.Y + 17 &&
                        !LeftMousePressed &&
                        PS2Mouse.MouseStatus == MouseStatus.Left)
                        || 
                        window.WindowDrag)
                {
                    window.WindowDrag = true;

                    if (window.X - 1 < 0 || window.Y - 1 < 0 || window.X + window.Width + 1 > GraphicsDriver.Width || window.Y + window.Height + 1 > GraphicsDriver.Height)
                    {
                        window.X = Math.Clamp(PS2Mouse.X - window.MouseDelta.X, 0, GraphicsDriver.Width - window.Width);
                        window.Y = Math.Clamp(PS2Mouse.Y - window.MouseDelta.Y, 0, GraphicsDriver.Height - window.Height);
                    }
                    else
                    {
                        window.X = PS2Mouse.X - window.MouseDelta.X;
                        window.Y = PS2Mouse.Y - window.MouseDelta.Y;
                    }
                }

                if (PS2Mouse.MouseStatus != MouseStatus.Left)
                    window.WindowDrag = false;

                #endregion

                #region Window Resize

                #region

                foreach (Control control in window.Controls)
                {

                    // Clicked
                    if (PS2Mouse.MouseStatus != MouseStatus.Left && LeftMousePressed &&
                            PS2Mouse.X >= window.X + control.X + 3 &&
                            PS2Mouse.X <= window.X + control.X + 3 + control.Width + control.Padding.LeftRight - 1 &&
                            PS2Mouse.Y >= window.Y + control.Y + 17 &&
                            PS2Mouse.Y <= window.Y + control.Y + 17 + control.Height + control.Padding.TopBottom - 1)
                    {
                        control._OnClick();
                    }

                    // Pressed
                    if (PS2Mouse.MouseStatus != MouseStatus.None &&
                            PS2Mouse.X >= window.X + control.X + 3 &&
                            PS2Mouse.X <= window.X + control.X + 3 + control.Width + control.Padding.LeftRight - 1 &&
                            PS2Mouse.Y >= window.Y + control.Y + 17 &&
                            PS2Mouse.Y <= window.Y + control.Y + 17 + control.Height + control.Padding.TopBottom - 1)
                    {
                        if (!control.Pressed)
                        {
                            control.Pressed = true;
                            control._MousePressed();
                        }
                    }
                    else
                    {
                        if (control.Pressed)
                        {
                            control.Pressed = false;
                            control._MouseReleased();
                        }
                    }

                    // Hovered
                    if (    PS2Mouse.X >= window.X + control.X + 3 &&
                            PS2Mouse.X <= window.X + control.X + 3 + control.Width + control.Padding.LeftRight - 1 &&
                            PS2Mouse.Y >= window.Y + control.Y + 17 &&
                            PS2Mouse.Y <= window.Y + control.Y + 17 + control.Height + control.Padding.TopBottom - 1)
                    {
                        if (!control.Hovered)
                        {
                            control.Hovered = true;
                            control._MouseEnter();
                        }
                    }
                    else
                    {
                        if (control.Hovered)
                        {
                            control.Hovered = false;
                            control._MouseLeave();
                        }
                    }
                }
            }

            LastMousePos = new Point(PS2Mouse.X - 1, PS2Mouse.Y - 1);
            LeftMousePressed = PS2Mouse.MouseStatus == MouseStatus.Left;
        }
    }
}
