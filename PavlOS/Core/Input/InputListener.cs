using Mosa.External.x86.Driver;
using Mosa.External.x86.Driver.Input;
using PavlOS.Core.Shell;
using PavlOS.Core.Shell.Controls.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PavlOS.Core
{
    public static class InputListener
    {
        public static bool LeftMousePressed;

        public static void CheckInput()
        {
            foreach (Control control in ShellCore.AllControls)
            {
                // Clicked
                if (    PS2Mouse.MouseStatus != MouseStatus.Left && LeftMousePressed &&
                        PS2Mouse.X >= control.X &&
                        PS2Mouse.X <= control.X + control.Width + control.Padding.LeftRight - 1 &&
                        PS2Mouse.Y >= control.Y &&
                        PS2Mouse.Y <= control.Y + control.Height + control.Padding.TopBottom - 1)
                {
                    control._OnClick();
                }

                // Pressed
                if (    PS2Mouse.MouseStatus != MouseStatus.None &&
                        PS2Mouse.X >= control.X &&
                        PS2Mouse.X <= control.X + control.Width + control.Padding.LeftRight - 1 &&
                        PS2Mouse.Y >= control.Y &&
                        PS2Mouse.Y <= control.Y + control.Height + control.Padding.TopBottom - 1)
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
                if (    PS2Mouse.X >= control.X &&
                        PS2Mouse.X <= control.X + control.Width + control.Padding.LeftRight - 1 &&
                        PS2Mouse.Y >= control.Y &&
                        PS2Mouse.Y <= control.Y + control.Height + control.Padding.TopBottom - 1)
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

            LeftMousePressed = PS2Mouse.MouseStatus == MouseStatus.Left;
        }
    }
}
