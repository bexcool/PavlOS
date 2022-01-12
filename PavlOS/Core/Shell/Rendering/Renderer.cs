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
using static PavlOS.Core.Shell.Utility.Utility;
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
        public int[] Cursor;
        private IconGallery IconGallery = new IconGallery();

        public void Init()
        {
            Cursor = new int[]
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
        }

        public void Render()
        {
            GraphicsDriver.Graphics.Clear(Color.DarkGray.ToArgb());
            
            // Draw controls
            foreach (Window window in ShellCore.AllWindows)
            {
                // Rendering window
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.White.ToArgb(), window.Width - 1, window.X, window.Y); // White horizontal
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(223, 223, 223).ToArgb(), window.Width - 3, window.X + 1, window.Y + 1); // Gray inside horizontal
                GraphicsDriver.Graphics.DrawVerticalLine(Color.White.ToArgb(), window.Height - 2, window.X, window.Y + 1); // White vertical
                GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(223, 223, 223).ToArgb(), window.Height - 4, window.X + 1, window.Y + 2); // Gray inside vertical
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.Black.ToArgb(), window.Width, window.X, window.Y + window.Height - 1); // Black horizontal
                GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(128, 128, 128).ToArgb(), window.Width - 2, window.X + 1, window.Y + window.Height - 2); // Dark gray horizontal
                GraphicsDriver.Graphics.DrawVerticalLine(Color.Black.ToArgb(), window.Height - 1, window.X + window.Width - 1, window.Y); // Black vertical
                GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(128, 128, 128).ToArgb(), window.Height - 3, window.X + window.Width - 2, window.Y + 1); // Dark gray vertical
                GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(180, 180, 180).ToArgb(), window.X + 2, window.Y + 2, window.Width - 4, window.Height - 4); // Fill


                // Rendering window controls
                foreach (Control control in window.Controls)
                {
                    if (control is Label)
                    {
                        Label label = control as Label;

                        GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", label.ForegroundColor.ToArgb(), label.Content, window.X + label.X + 3, window.Y + label.Y + 17, false);

                        label.ValueChanged = false;
                    }
                    else if (control is Button)
                    {
                        Button button = control as Button;

                        if (control.Pressed && PS2Mouse.MouseStatus == MouseStatus.Left)
                        {
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.Black.ToArgb(), control.Width - 1, window.X + control.X + 3, window.Y + control.Y + 17); // Black horizontal
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(128, 128, 128).ToArgb(), control.Width - 3, window.X + control.X + 3 + 1, window.Y + control.Y + 17 + 1); // Dark gray horizontal
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.Black.ToArgb(), control.Height - 2, window.X + control.X + 3, window.Y + control.Y + 17 + 1); // Black vertical
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(128, 128, 128).ToArgb(), control.Height - 4, window.X + control.X + 3 + 1, window.Y + control.Y + 17 + 2); // Dark gray horizontal
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.White.ToArgb(), control.Width, window.X + control.X + 3, window.Y + control.Y + 17 + control.Height - 1); // White horizontal
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(223, 223, 223).ToArgb(), control.Width - 2, window.X + control.X + 3 + 1, window.Y + control.Y + 17 + control.Height - 2); // Gray inside vertical
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.White.ToArgb(), control.Height - 1, window.X + control.X + 3 + control.Width - 1, window.Y + control.Y + 17); // White vertical
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(223, 223, 223).ToArgb(), control.Height - 3, window.X + control.X + 3 + control.Width - 2, window.Y + control.Y + 17 + 1); // Gray inside vertical
                            GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(180, 180, 180).ToArgb(), window.X + control.X + 3 + 2, window.Y + control.Y + 17 + 2, control.Width - 4, control.Height - 4); // Fill
                        }
                        else
                        {
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.White.ToArgb(), control.Width - 1, window.X + control.X + 3, window.Y + control.Y + 17); // White horizontal
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(223, 223, 223).ToArgb(), control.Width - 3, window.X + control.X + 3 + 1, window.Y + control.Y + 17 + 1); // Gray inside horizontal
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.White.ToArgb(), control.Height - 2, window.X + control.X + 3, window.Y + control.Y + 17 + 1); // White vertical
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(223, 223, 223).ToArgb(), control.Height - 4, window.X + control.X + 3 + 1, window.Y + control.Y + 17 + 2); // Gray inside vertical
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.Black.ToArgb(), control.Width, window.X + control.X + 3, window.Y + control.Y + 17 + control.Height - 1); // Black horizontal
                            GraphicsDriver.Graphics.DrawHorizontalLine(Color.FromArgb(128, 128, 128).ToArgb(), control.Width - 2, window.X + control.X + 3 + 1, window.Y + control.Y + 17 + control.Height - 2); // Dark gray horizontal
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.Black.ToArgb(), control.Height - 1, window.X + control.X + 3 + control.Width - 1, window.Y + control.Y + 17); // Black vertical
                            GraphicsDriver.Graphics.DrawVerticalLine(Color.FromArgb(128, 128, 128).ToArgb(), control.Height - 3, window.X + control.X + 3 + control.Width - 2, window.Y + control.Y + 17 + 1); // Dark gray vertical
                            GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(180, 180, 180).ToArgb(), window.X + control.X + 3 + 2, window.Y + control.Y + 17 + 2, control.Width - 4, control.Height - 4); // Fill
                        }

                        if (!string.IsNullOrEmpty(control.Content)) GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.Black.ToArgb(), button.Content, window.X + button.X + 3, window.Y + button.Y + 17, false);
                    }
                }

                // Rendering title
                GraphicsDriver.Graphics.DrawFilledRectangle(Color.FromArgb(16, 131, 207).ToArgb(), window.X + 3, window.Y + 3, window.Width - 6, 15);
                if (!string.IsNullOrEmpty(window.Title)) GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), window.Title, window.X + 6, window.Y + 1, false);

                //GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.Black.ToArgb(), ShellCore.AllWindows.Count.ToString() + " X " + window.X + " I  Y " + window.Y + " I  Width " + window.Width, (int)window.X + 6, (int)window.Y + 50, false);
            }

            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Current Driver is " + GraphicsDriver.Graphics.CurrentDriver, 10, 10);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "FPS is " + FPSMeter.FPS, 10, 26);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Available Memory is " + Memory.GetAvailableMemory() / 1048576 + " MB out of " + PageFrameAllocator.TotalPages * PageFrameAllocator.PageSize / 1048576 + " MB", 10, 42);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Mouse X: " + PS2Mouse.X, 10, 58);
            GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset16", Color.White.ToArgb(), "Mouse Y: " + PS2Mouse.Y, 10, 74);

            GraphicsDriver.Graphics.DrawImage(IconGallery.BMP_Sys_Acorn, 100, 400, true);
            DrawCursor(PS2Mouse.X, PS2Mouse.Y, Cursor);

            GraphicsDriver.Graphics.Update();
            FPSMeter.Update();
        }

        private void DrawCursor(int X, int Y, int[] Cursor)
        {
            for (int h = 0; h < 21; h++)
                for (int w = 0; w < 12; w++)
                {
                    if (Cursor[h * 12 + w] == 1)
                        GraphicsDriver.Graphics.DrawPoint(Color.Black.ToArgb(), w + X, h + Y);

                    if (Cursor[h * 12 + w] == 2)
                        GraphicsDriver.Graphics.DrawPoint(Color.White.ToArgb(), w + X, h + Y);
                }
        }
    }
}
