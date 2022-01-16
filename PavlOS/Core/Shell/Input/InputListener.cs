using Mosa.External.x86.Driver;
using Mosa.External.x86.Driver.Input;
using PavlOS.Core.Shell;
using PavlOS.Core.Shell.Controls.Base;
using PavlOS.Core.Shell.Rendering;
using PavlOS.Core.Utility;
using PavlOS_Dev.Core.Shell.Controls.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static PavlOS.Core.Shell.Utility.ShellUtil;

namespace PavlOS.Core
{
    public class InputListener
    {
        public static bool LeftMousePressed;

        private bool AlreadyChangedWindowFocus = false, AlreadyWindowInteraction = false;

        int OffsetDragX, OffsetDragY, OffsetResizeX, OffsetResizeY;

        public void CheckInput()
        {
            AlreadyChangedWindowFocus = false;

            // Check window input
            for (int i = ShellCore.AllWindows.Count - 1; i >= 0; i--)
                for (int j = ShellCore.AllWindows.Count - 1; j >= 0; j--)
                {
                    Window window = (Window)ShellCore.AllWindows[j];

                    if (window.IndexZ == i)
                    {
                        #region Window Focus

                        // Focused
                        if (!AlreadyWindowInteraction &&
                            !LeftMousePressed &&
                            !AlreadyChangedWindowFocus &&
                            PS2Mouse.MouseStatus == MouseStatus.Left &&
                            PS2Mouse.X >= window.X &&
                            PS2Mouse.X <= window.X + window.Width &&
                            PS2Mouse.Y >= window.Y &&
                            PS2Mouse.Y <= window.Y + window.Height)
                        {
                            AlreadyChangedWindowFocus = true;
                            window.Focus();
                        }

                        #endregion

                        #region Window Drag and Resize

                        // Reset window variables
                        if (window == ShellCore.FocusedWindow && PS2Mouse.MouseStatus != MouseStatus.Left)
                        {
                            window.WindowDrag = false;
                            window.WindowResizeX = false;
                            window.WindowResizeY = false;
                            AlreadyWindowInteraction = false;

                            Renderer.CurrentCursor = Cursor.Arrow;
                        }

                        // Check drag
                        if (window == ShellCore.FocusedWindow && PS2Mouse.MouseStatus == MouseStatus.Left && !LeftMousePressed && PS2Mouse.X > window.X + Window.BorderWeight && PS2Mouse.X < window.X + window.Width - Window.BorderWeight && PS2Mouse.Y > window.Y + 3 && PS2Mouse.Y < window.Y + Window.TitleBarHeight && !window.WindowDrag)
                        {
                            window.WindowDrag = true;
                            AlreadyWindowInteraction = true;

                            OffsetDragX = PS2Mouse.X - window.X;
                            OffsetDragY = PS2Mouse.Y - window.Y;
                        }

                        // Check resize X
                        if (window == ShellCore.FocusedWindow && PS2Mouse.X > window.X + window.Width - Window.BorderWeight && PS2Mouse.X < window.X + window.Width + Window.ResizeBorderWeight && PS2Mouse.Y > window.Y && PS2Mouse.Y < window.Y + window.Height + Window.ResizeBorderWeight)
                        {
                            Renderer.CurrentCursor = Cursor.HorizontalResize;

                            if (PS2Mouse.MouseStatus == MouseStatus.Left && !LeftMousePressed && !window.WindowResizeX)
                            {
                                window.WindowResizeX = true;
                                AlreadyWindowInteraction = true;

                                OffsetResizeX = PS2Mouse.X - window.X - window.Width;
                            }
                        }

                        // Check resize Y
                        if (window == ShellCore.FocusedWindow && PS2Mouse.Y > window.Y + window.Height - Window.BorderWeight && PS2Mouse.Y < window.Y + window.Height + Window.ResizeBorderWeight && PS2Mouse.X > window.X && PS2Mouse.X < window.X + window.Width + Window.ResizeBorderWeight)
                        {
                            Renderer.CurrentCursor = Cursor.VerticalResize;

                            if (PS2Mouse.MouseStatus == MouseStatus.Left && !LeftMousePressed && !window.WindowResizeY)
                            {
                                window.WindowResizeY = true;
                                AlreadyWindowInteraction = true;

                                OffsetResizeY = PS2Mouse.Y - window.Y - window.Height;
                            }
                        }

                        // Set diagonal cursor
                        if (window == ShellCore.FocusedWindow && PS2Mouse.Y > window.Y + window.Height - Window.BorderWeight && PS2Mouse.Y < window.Y + window.Height + Window.ResizeBorderWeight && PS2Mouse.X > window.X && PS2Mouse.X < window.X + window.Width + Window.ResizeBorderWeight &&
                            PS2Mouse.X > window.X + window.Width - Window.BorderWeight && PS2Mouse.X < window.X + window.Width + Window.ResizeBorderWeight && PS2Mouse.Y > window.Y && PS2Mouse.Y < window.Y + window.Height + Window.ResizeBorderWeight)
                            Renderer.CurrentCursor = Cursor.DiagonalResize;

                        if (window.WindowDrag)
                        {
                            window.X = Math.Clamp(PS2Mouse.X - OffsetDragX, 0, GraphicsDriver.Width - window.Width);
                            window.Y = Math.Clamp(PS2Mouse.Y - OffsetDragY, 0, GraphicsDriver.Height - window.Height);
                        }

                        if (window.WindowResizeX)
                        {
                            window.Width = Math.Clamp(PS2Mouse.X - window.X - OffsetResizeX, window.MinWidth, GraphicsDriver.Width);
                        }

                        if (window.WindowResizeY)
                        {
                            window.Height = Math.Clamp(PS2Mouse.Y - window.Y - OffsetResizeY, window.MinHeight, GraphicsDriver.Height);
                        }

                        #endregion

                        foreach (Control control in window.Controls)
                        {

                            // Clicked
                            if (PS2Mouse.MouseStatus != MouseStatus.Left && LeftMousePressed &&
                                    PS2Mouse.X >= window.X + control.X + Window.BorderWeight &&
                                    PS2Mouse.X <= window.X + control.X + Window.BorderWeight + control.Width + control.Padding.LeftRight - 1 &&
                                    PS2Mouse.Y >= window.Y + control.Y + Window.TitleBarHeight &&
                                    PS2Mouse.Y <= window.Y + control.Y + Window.TitleBarHeight + control.Height + control.Padding.TopBottom - 1)
                            {
                                control._OnClick();
                            }

                            // Pressed
                            if (PS2Mouse.MouseStatus != MouseStatus.None &&
                                    PS2Mouse.X >= window.X + control.X + Window.BorderWeight &&
                                    PS2Mouse.X <= window.X + control.X + Window.BorderWeight + control.Width + control.Padding.LeftRight - 1 &&
                                    PS2Mouse.Y >= window.Y + control.Y + Window.TitleBarHeight &&
                                    PS2Mouse.Y <= window.Y + control.Y + Window.TitleBarHeight + control.Height + control.Padding.TopBottom - 1)
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
                            if (PS2Mouse.X >= window.X + control.X + Window.BorderWeight &&
                                    PS2Mouse.X <= window.X + control.X + Window.BorderWeight + control.Width + control.Padding.LeftRight - 1 &&
                                    PS2Mouse.Y >= window.Y + control.Y + Window.TitleBarHeight &&
                                    PS2Mouse.Y <= window.Y + control.Y + Window.TitleBarHeight + control.Height + control.Padding.TopBottom - 1)
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
                }

            foreach (Control control in ShellCore.AllSystemControls)
            {
                // Clicked
                if (PS2Mouse.MouseStatus != MouseStatus.Left && LeftMousePressed &&
                        PS2Mouse.X >= control.X &&
                        PS2Mouse.X <= control.X + control.Width + control.Padding.LeftRight - 1 &&
                        PS2Mouse.Y >= control.Y &&
                        PS2Mouse.Y <= control.Y + control.Height + control.Padding.TopBottom - 1)
                {
                    control._OnClick();
                }

                // Pressed
                if (PS2Mouse.MouseStatus != MouseStatus.None &&
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
                if (PS2Mouse.X >= control.X &&
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
