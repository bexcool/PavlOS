using PavlOS.Core.Shell.Rendering;
using PavlOS.Core.Shell.Rendering.FontRendering;
using PavlOS.Core.Shell.Rendering.FontRendering.Libraries;
using PavlOS.Core.Shell.Controls.Base;
using PavlOS.Core.Shell.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using static PavlOS.Core.Shell.Utility.ShellUtil;
using PavlOS.Core.Shell.Utility;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Driver;
using PavlOS.Core.Utility;
using Mosa.External.x86;
using Mosa.External.x86.Driver.Input;
using Convert = Mosa.External.x86.Convert;
using Mosa.Kernel.x86;
using PavlOS_Dev.Core.Shell.Controls;
using PavlOS_Dev.Core.Shell.Controls.Base;

namespace PavlOS.Core.Shell.Rendering
{
    public class Renderer
    {
        // Cursors
        private int[] CursorArrow, CursorVerticalResize, CursorHorizontalResize, CursorDiagonalResize;
        private int CursorWidth, CursorHeight, CursorOffsetX, CursorOffsetY;
        public static Cursor CurrentCursor = Cursor.Arrow;
        // System icons
        private IconGallery IconGallery = new IconGallery();
        public static bool LeftMousePressed;

        public void Init()
        {
            CursorArrow = new int[]
            {
                1,0,0,0,0,0,0,0,0,0,0,0,
                1,1,0,0,0,0,0,0,0,0,0,0,
                1,2,1,0,0,0,0,0,0,0,0,0,
                1,2,2,1,0,0,0,0,0,0,0,0,
                1,2,2,2,1,0,0,0,0,0,0,0,
                1,2,2,2,2,1,0,0,0,0,0,0,
                1,2,2,2,2,2,1,0,0,0,0,0,
                1,2,2,2,2,2,2,1,0,0,0,0,
                1,2,2,2,2,2,2,2,1,0,0,0,
                1,2,2,2,2,2,2,2,2,1,0,0,
                1,2,2,2,2,2,2,2,2,2,1,0,
                1,2,2,2,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,1,1,1,1,1,
                1,2,2,2,1,2,2,1,0,0,0,0,
                1,2,2,1,0,1,2,2,1,0,0,0,
                1,2,1,0,0,1,2,2,1,0,0,0,
                1,1,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,0,1,2,2,1,0,
                0,0,0,0,0,0,0,1,2,2,1,0,
                0,0,0,0,0,0,0,0,1,1,0,0
            };

            CursorVerticalResize = new int[]
            {
                0,0,0,1,0,0,0,
                0,0,1,2,1,0,0,
                0,1,2,2,2,1,0,
                1,2,2,2,2,2,1,
                1,1,1,2,1,1,1,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                0,0,1,2,1,0,0,
                1,1,2,2,1,1,1,
                1,2,2,2,2,2,1,
                0,1,2,2,2,1,0,
                0,0,1,2,1,0,0,
                0,0,0,1,0,0,0,
            };

            CursorHorizontalResize = new int[]
            {
                0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,
                0,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1,0,0,
                0,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,0,
                1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,
                0,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,0,
                0,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1,0,0,
                0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0
            };

            CursorDiagonalResize = new int[]
            {
                1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,
                1,2,2,2,2,1,0,0,0,0,0,0,0,0,0,
                1,2,2,2,1,0,0,0,0,0,0,0,0,0,0,
                1,2,2,2,1,0,0,0,0,0,0,0,0,0,0,
                1,2,1,1,2,1,0,0,0,0,0,0,0,0,0,
                1,1,0,0,1,2,1,0,0,0,0,0,0,0,0,
                0,0,0,0,0,1,2,1,0,0,0,0,0,0,0,
                0,0,0,0,0,0,1,2,1,0,0,0,0,0,0,
                0,0,0,0,0,0,0,1,2,1,0,0,0,0,0,
                0,0,0,0,0,0,0,0,1,2,1,0,0,1,1,
                0,0,0,0,0,0,0,0,0,1,2,1,1,2,1,
                0,0,0,0,0,0,0,0,0,0,1,2,2,2,1,
                0,0,0,0,0,0,0,0,0,0,1,2,2,2,1,
                0,0,0,0,0,0,0,0,0,1,2,2,2,2,1,
                0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,
            };
        }

        public void Render()
        {
            GraphicsDriver.Graphics.Clear(ShellCore.DesktopColor);
            
            // Draw controls
            for (int i = 0; i < ShellCore.AllWindows.Count; i++)
                for (int j = 0; j < ShellCore.AllWindows.Count; j++)
                {
                    Window window = (Window)ShellCore.AllWindows[j];

                    if (window.IndexZ == i)
                    {
                        // Rendering window

                        Draw3DBorder(window.X, window.Y, window.Width, window.Height);

                        // Rendering title
                        GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(16, 131, 207).ToArgb(), window.X + 3, window.Y + 3, window.Width - 6, Window.TitleBarHeight);
                        if (!string.IsNullOrEmpty(window.Title)) GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), window.Title, window.X + 6, window.Y + 1, false);

                        // Rendering window controls
                        foreach (Control control in window.Controls)
                        {
                            if (control is Label)
                            {
                                Label label = control as Label;

                                GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", label.ForegroundColor.ToArgb(), label.Content, window.X + label.X + 3, window.Y + label.Y + 17, false);
                            }
                            else if (control is Button)
                            {
                                Button button = control as Button;

                                if (control.Pressed && PS2Mouse.MouseStatus == MouseStatus.Left)
                                {
                                    Draw3DBorder(window.X + control.X + Window.BorderWeight, window.Y + control.Y + Window.TitleBarHeight + Window.BorderWeight, control.Width, control.Height, true);
                                }
                                else
                                {
                                    Draw3DBorder(window.X + control.X + Window.BorderWeight, window.Y + control.Y + Window.TitleBarHeight + Window.BorderWeight, control.Width, control.Height);
                                }

                                if (!string.IsNullOrEmpty(control.Content)) GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.Black.ToArgb(), button.Content, window.X + button.X + 3, window.Y + button.Y + 17, false);
                            }
                        }

                        //GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.Black.ToArgb(), ShellCore.AllWindows.Count.ToString() + " X " + window.X + " I  Y " + window.Y + " I  Width " + window.Width, (int)window.X + 6, (int)window.Y + 50, false);
                    }
                }


            // Rendering system controls
            foreach (Control control in ShellCore.AllSystemControls)
            {
                if (control is Label)
                {
                    Label label = control as Label;

                    GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", label.ForegroundColor.ToArgb(), label.Content, label.X + 3, label.Y + 17, false);
                }
                else if (control is Button)
                {
                    Button button = control as Button;

                    if (control.Pressed && PS2Mouse.MouseStatus == MouseStatus.Left)
                    {
                        Draw3DBorder(control.X + Window.BorderWeight, control.Y + Window.TitleBarHeight + Window.BorderWeight, control.Width, control.Height, true);
                    }
                    else
                    {
                        Draw3DBorder(control.X + Window.BorderWeight, control.Y + Window.TitleBarHeight + Window.BorderWeight, control.Width, control.Height);
                    }

                    if (!string.IsNullOrEmpty(control.Content)) GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.Black.ToArgb(), button.Content, button.X + 3, button.Y + 17, false);
                }
            }

            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Current Driver is " + GraphicsDriver.Graphics.CurrentDriver, 10, 10);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "FPS is " + FPSMeter.FPS, 10, 26);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Available Memory is " + Memory.GetAvailableMemory() / 1048576 + " MB out of " + PageFrameAllocator.TotalPages * PageFrameAllocator.PageSize / 1048576 + " MB", 10, 42);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Mouse X: " + PS2Mouse.X, 10, 58);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Mouse Y: " + PS2Mouse.Y, 10, 74);

            GraphicsDriver.Graphics.DrawImage(IconGallery.BMP_Sys_Acorn, 100, 400, true);
            RenderTaskbar();

            #region Draw Cursor

            switch (CurrentCursor)
            {
                case Cursor.Arrow:
                    CursorWidth = 12;
                    CursorOffsetX = 0;
                    CursorOffsetY = 0;
                    DrawCursor(PS2Mouse.X, PS2Mouse.Y, CursorArrow);
                    break;
                case Cursor.VerticalResize:
                    CursorWidth = 7;
                    CursorOffsetX = -CursorWidth / 2;
                    CursorOffsetY = -(CursorVerticalResize.Length / CursorWidth) / 2;
                    DrawCursor(PS2Mouse.X, PS2Mouse.Y, CursorVerticalResize); 
                    break;
                case Cursor.HorizontalResize:
                    CursorWidth = 22;
                    CursorOffsetX = -CursorWidth / 2;
                    CursorOffsetY = -(CursorVerticalResize.Length / CursorWidth) / 2;
                    DrawCursor(PS2Mouse.X, PS2Mouse.Y, CursorHorizontalResize);
                    break;
                case Cursor.DiagonalResize:
                    CursorWidth = 15;
                    CursorOffsetX = -CursorWidth / 2;
                    CursorOffsetY = -(CursorVerticalResize.Length / CursorWidth) / 2;
                    DrawCursor(PS2Mouse.X, PS2Mouse.Y, CursorDiagonalResize);
                    break;
            }

            #endregion

            GraphicsDriver.Graphics.Update();
            FPSMeter.Update();

            LeftMousePressed = PS2Mouse.MouseStatus == MouseStatus.Left;
        }

        private void DrawCursor(int X, int Y, int[] Cursor)
        {
            for (int h = 0; h < Cursor.Length / CursorWidth; h++)
                for (int w = 0; w < CursorWidth; w++)
                {
                    if (Cursor[h * CursorWidth + w] == 1)
                        GraphicsDriver.Graphics.DrawPoint(Color.Black.ToArgb(), w + X + CursorOffsetX, h + Y + CursorOffsetY);

                    if (Cursor[h * CursorWidth + w] == 2)
                        GraphicsDriver.Graphics.DrawPoint(Color.White.ToArgb(), w + X + CursorOffsetX, h + Y + CursorOffsetY);
                }
        }

        private void Draw3DBorder(int X, int Y, int Width, int Height, bool SwitchShadow = false)
        {
            if (SwitchShadow)
            {
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.Black.ToArgb(), Width - 1, X, Y); // Black horizontal
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(128, 128, 128).ToArgb(), Width - 3, X + 1, Y + 1); // Dark gray horizontal
                GraphicsDriver.Graphics.DrawVerticalLine(Color.Black.ToArgb(), Height - 2, X, Y + 1); // Black vertical
                GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(128, 128, 128).ToArgb(), Height - 4, X + 1, Y + 2); // Dark gray horizontal
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.White.ToArgb(), Width, X, Y + Height - 1); // White horizontal
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(223, 223, 223).ToArgb(), Width - 2, X + 1, Y + Height - 2); // Gray inside vertical
                GraphicsDriver.Graphics.DrawVerticalLine(Color.White.ToArgb(), Height - 1, X + Width - 1, Y); // White vertical
                GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(223, 223, 223).ToArgb(), Height - 3, X + Width - 2, Y + 1); // Gray inside vertical
                GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(180, 180, 180).ToArgb(), X + 2, Y + 2, Width - 4, Height - 4); // Fill
            }
            else
            {
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.White.ToArgb(), Width - 1, X, Y); // White horizontal
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(223, 223, 223).ToArgb(), Width - 3, X + 1, Y + 1); // Gray inside horizontal
                GraphicsDriver.Graphics.DrawVerticalLine(Color.White.ToArgb(), Height - 2, X, Y + 1); // White vertical
                GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(223, 223, 223).ToArgb(), Height - 4, X + 1, Y + 2); // Gray inside vertical
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.Black.ToArgb(), Width, X, Y + Height - 1); // Black horizontal
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(128, 128, 128).ToArgb(), Width - 2, X + 1, Y + Height - 2); // Dark gray horizontal
                GraphicsDriver.Graphics.DrawVerticalLine(Color.Black.ToArgb(), Height - 1, X + Width - 1, Y); // Black vertical
                GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(128, 128, 128).ToArgb(), Height - 3, X + Width - 2, Y + 1); // Dark gray vertical
                GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(180, 180, 180).ToArgb(), X + 2, Y + 2, Width - 4, Height - 4); // Fill
            }
        }

        private void RenderTaskbar()
        {
            GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(180, 180, 180).ToArgb(), GraphicsDriver.Width, 0, GraphicsDriver.Height - ShellCore.TaskBarHeight);
            GraphicsDriver.Graphics.DrawHorizontalLine(Color.White.ToArgb(), GraphicsDriver.Width, 0, GraphicsDriver.Height - ShellCore.TaskBarHeight + 1);
            GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(180, 180, 180).ToArgb(), 0, GraphicsDriver.Height - ShellCore.TaskBarHeight + 2, GraphicsDriver.Width, ShellCore.TaskBarHeight - 2);
        }
    }
}
