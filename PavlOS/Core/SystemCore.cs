// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.External.x86;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using Mosa.External.x86.FileSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using PavlOS.Core.Shell;
using PavlOS.Core.Shell.Controls;
using PavlOS.Core.Shell.Controls.Base;
using PavlOS.Core.Shell.Rendering;
using PavlOS.Core.System32.AppExe.TestApplication;
using PavlOS.Core.Utility;
using PavlOS_Dev.Core.Shell.Controls;
using PavlOS_Dev.Core.Shell.Controls.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using static PavlOS.Core.Shell.Utility.Utility;
using Convert = Mosa.External.x86.Convert;

namespace PavlOS.Core
{
    public static class SystemCore
    {
        public static bool Panicked = false;

        public static void Main() { }

        [VBERequire(640, 480, 32)]
        [Plug("Mosa.Runtime.StartUp::KMain")]
        [UnmanagedCallersOnly(EntryPoint = "KMain", CallingConvention = CallingConvention.StdCall)]
        public static void KMain()
        {
            IDT.OnInterrupt += IDT_OnInterrupt;
            Panic.OnPanic += Panic_OnPanic;

            new Thread(MainThread).Start();

            while (true);
        }

        private static void IDT_OnInterrupt(uint irq, uint error)
        {
            switch (irq)
            {
                case 0x21:
                    PS2Keyboard.OnInterrupt();
                    break;
                case 0x2C:
                    PS2Mouse.OnInterrupt();
                    break;
            }
        }

        // VBE Panic, this gets called if a panic happens!
        private static void Panic_OnPanic(string message)
        {
            Panicked = true;
            string CustomCharset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!:";
            byte[] ArialCustomCharset24 = Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAAAAA4AABEAACCAACCAACCAACCAACCAACCAACCAACCAABEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAYAAAoAABIAAAIAAAIAAAIAAAIAAAIAAAIAAAIAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA8AABEAACCAAACAAACAAAEAAAEAAAIAAAQAAAgAABAAAD+AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AABEAACEAAAEAAAIAAA4AAAEAAACAAACAACCAADEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAYAAAoAAAoAABIAABIAACIAAEIAAH+AAAIAAAIAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB+AABAAABAAACAAAD4AACEAAACAAACAAACAACCAABEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AABGAACCAACAAAC4AADEAACCAACCAACCAACCAABEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+AAAEAAAEAAAIAAAIAAAQAAAQAAAQAAAQAAAgAAAgAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AABEAACCAACCAABEAAA4AABEAACCAACCAACCAABEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AABEAACCAACCAACCAACCAABGAAA6AAACAACCAABEAAB4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAUAAAUAAAiAAAiAAAiAABBAAB/AACAgACAgAEAQAEAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+AACBAACAgACAgACBAAD/AACBAACAgACAgACAgACBAAD+AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfAAAggABAQACAAACAAACAAACAAACAAACAAABAQAAggAAfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+AACBAACAgACAQACAQACAQACAQACAQACAQACAgACBAAD+AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/gACAAACAAACAAACAAAD/AACAAACAAACAAACAAACAAAD/gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/AACAAACAAACAAACAAAD+AACAAACAAACAAACAAACAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAeAAAhAABAgACAAACAAACAAACHwACAQACAQABAgAAhAAAeAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgACAgACAgACAgACAgAD/gACAgACAgACAgACAgACAgACAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAABAAABAAABAAABAAABAAABAAABAAABAAABAAABAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAACAAACAAACAAACAAACAACCAACCAABEAAB4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgACBAACCAACEAACIAACYAACkAADCAACCAACBAACBAACAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAACAAACAAACAAACAAACAAACAAACAAACAAAD+AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAIADAYADAYACgoACgoACRIACRIACSIACKIACKIACEIACEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgADAgACggACggACQgACIgACIgACEgACCgACCgACBgACAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAeAAAhAABAgACAQACAQACAQACAQACAQACAQABAgAAhAAAeAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+AACBAACAgACAgACAgACBAAD+AACAAACAAACAAACAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAeAAAhAABAgACAQACAQACAQACAQACAQACAQABGwAAhgAAewAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+AACBAACAgACAgACAgACBAAD+AACEAACCAACCAACBAACAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA+AABBAACAgACAAABAAAA4AAAHAAAAgACAgACAgABBAAA+AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH/AAAQAAAQAAAQAAAQAAAQAAAQAAAQAAAQAAAQAAAQAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgABBAAA+AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgACAgABBAABBAABBAAAiAAAiAAAiAAAUAAAUAAAIAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQCCggCCggCCggBERABERABERAAoKAAoKAAoKAAQEAAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgABBAAAiAAAiAAAUAAAIAAAUAAAiAAAiAABBAACAgAEAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAgABBAABBAAAiAAAUAAAUAAAIAAAIAAAIAAAIAAAIAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/AAACAAAEAAAEAAAIAAAQAAAQAAAgAABAAABAAACAAAH/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA8AABCAACCAAAOAAByAACCAACCAACGAAB6AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAAC4AADEAACCAACCAACCAACCAACCAADEAAC4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AABEAACCAACAAACAAACAAACCAABEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAA6AABGAACCAACCAACCAACCAACCAABGAAA6AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AABEAACCAACCAAD+AACAAACCAABEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwAABAAABAAADwAABAAABAAABAAABAAABAAABAAABAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA6AABGAACCAACCAACCAACCAACCAABGAAA6AAACAACEAAB4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAAC8AADCAACCAACCAACCAACCAACCAACCAACCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAAAAAAAAACAAACAAACAAACAAACAAACAAACAAACAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAAAAAAAAACAAACAAACAAACAAACAAACAAACAAACAAACAAACAAACAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAACCAACEAACIAACQAACwAADIAACEAACEAACCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAACAAACAAACAAACAAACAAACAAACAAACAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC5wADGIACEIACEIACEIACEIACEIACEIACEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC8AADCAACCAACCAACCAACCAACCAACCAACCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AABEAACCAACCAACCAACCAACCAABEAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC4AADEAACCAACCAACCAACCAACCAADEAAC4AACAAACAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA6AABGAACCAACCAACCAACCAACCAABGAAA6AAACAAACAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC4AADAAACAAACAAACAAACAAACAAACAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB4AACEAACAAACAAAB4AAAEAAAEAACEAAB4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAACAAAHgAACAAACAAACAAACAAACAAACAAACAAADgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACCAACCAACCAACCAACCAACCAACCAACGAAB6AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEEAAEEAACIAACIAACIAABQAABQAAAgAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEIQAEIQACUgACUgACigACigACigABBAABBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEEAACIAABQAABQAAAgAABQAABQAACIAAEEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACCAACCAACCAABEAABEAABMAAAoAAAoAAAQAAAQAAAgAADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH8AAAIAAAQAAAQAAAgAABAAABAAACAAAH8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAABAAABAAABAAABAAABAAABAAABAAABAAABAAAAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            BitFont.RegisterBitFont(new BitFontDescriptor("ArialCustomCharset24", CustomCharset, ArialCustomCharset24, 24));
            while (true)
            {
                GraphicsDriver.Graphics.Clear(Color.Blue.ToArgb());

                GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset24", Color.White.ToArgb(), "Kernel Panic!", 10, 10, false);
                GraphicsDriver.Graphics.DrawBitFontString("ArialCustomCharset24", Color.White.ToArgb(), "Message: " + message, 10, 30, false);

                GraphicsDriver.Graphics.Update();
            }
        }

        public static void MainThread()
        {
            // Initialize the IDE hard drive
            // MOSA currently only supports FAT12 ( obsolete ) and FAT32
            //IDisk disk = new IDEDisk(IDE.ControllerIndex.One);
            //MBR mBR = new MBR();
            //mBR.Initialize(disk);
            //FAT32 fs = new FAT32(disk, mBR.PartitionInfos[0]);
            // Initialize graphics (default width and height is 640 and 480 respectively)
            // Make sure you've already enabled VMSVGA(in VirtualBox) or VBE(in Run.bat)

            Renderer ShellRenderer = new Renderer();
            InputListener ShellInputListener = new InputListener();
            GraphicsDriver.Init(640, 480);
            ShellRenderer.Init();
            PS2Mouse.Initialize(GraphicsDriver.Width, GraphicsDriver.Height);

            int test = 0;

            TestWindow window = new TestWindow();
            
            window.X = 200;
            window.Y = 300;
            window.Width = 200;
            window.Height = 100;
            
            Label label = new Label();
            label.X = 75;
            label.Y = 20;
            label.ForegroundColor = Color.Black;
            label.Content = "I am a label.";

            window.AddControl(label);
            /*
            Button button = new Button();
            button.X = 300;
            button.Y = 100;
            button.Width = 60;
            button.Height = 30;
            button.Content = "Click me!";
            button.OnClick += (o) =>
            {
                test++;
                label.Content = test.ToString();
            };
            */
            Button button = new Button();
            button.X = 10;
            button.Y = 10;
            button.Width = 60;
            button.Height = 30;
            button.Content = "Click me!";
            button.OnClick += (o) =>
            {
                test++;
                label.Content = test.ToString();
            };

            window.AddControl(button);

            Button minus = new Button();
            minus.X = 10;
            minus.Y = 45;
            minus.Width = 60;
            minus.Height = 30;
            minus.Content = "Minus";
            minus.OnClick += (o) =>
            {
                test--;
                label.Content = test.ToString();
                //window.Center();
            };

            window.AddControl(minus);

            //string CustomCharset = "0123456789A�BC��D�E��FGHI�JKLMN�O�PQR�S�T�U��VWXY�Z�a�bc��d�e��fghi�jklmn�o�pqr�s�t�u��vwxy�z�";
            string CustomCharset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            byte[] ArialCustomCharset16 = Convert.FromBase64String("AAAAAAAAAAAAAAAAHAAiACIAIgAiACIAIgAcAAAAAAAAAAAAAAAAAAAAAAAIABgAKAAIAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAABwAIgACAAIABAAIABAAPgAAAAAAAAAAAAAAAAAAAAAAHAAiAAIADAACAAIAIgAcAAAAAAAAAAAAAAAAAAAAAAAEAAwAFAAUACQAPgAEAAQAAAAAAAAAAAAAAAAAAAAAAB4AEAAgADwAAgACACIAHAAAAAAAAAAAAAAAAAAAAAAAHAAiACAAPAAiACIAIgAcAAAAAAAAAAAAAAAAAAAAAAA+AAQABAAIAAgAEAAQABAAAAAAAAAAAAAAAAAAAAAAABwAIgAiABwAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAHAAiACIAIgAeAAIAIgAcAAAAAAAAAAAAAAAAAAAAAAAEAAoACgAKABEAHwAggCCAAAAAAAAAAAAAAAAAAAAAAD4AIQAhAD8AIQAhACEAPgAAAAAAAAAAAAAAAAAAAAAADgARACAAIAAgACAAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIQAhACEAIQAiADwAAAAAAAAAAAAAAAAAAAAAAD4AIAAgAD4AIAAgACAAPgAAAAAAAAAAAAAAAAAAAAAAPgAgACAAPAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAOABEAIIAgACOAIIARAA4AAAAAAAAAAAAAAAAAAAAAACEAIQAhAD8AIQAhACEAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAEAAQABAAEAAQAJAAkABgAAAAAAAAAAAAAAAAAAAAAACEAIgAkACwANAAiACIAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAA+AAAAAAAAAAAAAAAAAAAAAAAggDGAMYAqgCqAKoAkgCSAAAAAAAAAAAAAAAAAAAAAACEAMQApACkAJQAlACMAIQAAAAAAAAAAAAAAAAAAAAAADgARACCAIIAggCCAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIgAiADwAIAAgACAAAAAAAAAAAAAAAAAAAAAAAA4AEQAggCCAIIAmgBEADoAAAAAAAAAAAAAAAAAAAAAAPgAhACEAPgAkACIAIgAhAAAAAAAAAAAAAAAAAAAAAAAeACEAIAAYAAYAAQAhAB4AAAAAAAAAAAAAAAAAAAAAAD4ACAAIAAgACAAIAAgACAAAAAAAAAAAAAAAAAAAAAAAIQAhACEAIQAhACEAIQAeAAAAAAAAAAAAAAAAAAAAAAAggCCAEQARAAoACgAEAAQAAAAAAAAAAAAAAAAAAAAAAEIQRRAlICUgKKAooBBAEEAAAAAAAAAAAAAAAAAAAAAAIQASABIADAAMABIAEgAhAAAAAAAAAAAAAAAAAAAAAAAggBEAEQAKAAQABAAEAAQAAAAAAAAAAAAAAAAAAAAAAB8AAgAEAAQACAAIABAAPwAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAeACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAyACwAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIAAgACIAHAAAAAAAAAAAAAAAAAAAAAAAAgACABoAJgAiACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAAAAAAHAAiAD4AIAAiABwAAAAAAAAAAAAAAAAAAAAAAAgAEAA4ABAAEAAQABAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAABoAJgAiACIAJgAaAAIAPAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAACAAAAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAIAAAACAAIAAgACAAIAAgACAAQAAAAAAAAAAAAAAAAAAgACAAJAAoADAAKAAoACQAAAAAAAAAAAAAAAAAAAAAACAAIAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAC8ANIAkgCSAJIAkgAAAAAAAAAAAAAAAAAAAAAAAAAAAPAAiACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAAAAAACwAMgAiACIAMgAsACAAIAAAAAAAAAAAAAAAAAAAAAAAGgAmACIAIgAmABoAAgACAAAAAAAAAAAAAAAAAAAAAAAoADAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAYAAQAIgAcAAAAAAAAAAAAAAAAAAAAAAAgACAAcAAgACAAIAAgADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAiACIAIgAiACYAGgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAIiAlIBVAFUAIgAiAAAAAAAAAAAAAAAAAAAAAAAAAAAAiABQACAAIABQAIgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAgAEAAAAAAAAAAAAAAAAAAAAAAAPgAEAAgACAAQAD4AAAAAAA==");
            BitFont.RegisterBitFont(new BitFontDescriptor("ArialCustomCharset16", CustomCharset, ArialCustomCharset16, 16));

            while (true)
            {
                if (!Panicked)
                {
                    ShellInputListener.CheckInput();
                    ShellRenderer.Render();
                    FPSMeter.Update();
                }
            }
        }
    }

    public static class SystemUtilities
    {
        public static string GenerateID(int Size)
        {
            bool Done = true;
            string ID = "";

            while (!Done)
            {
                Done = true;
                while (ID.Length < Size + 1)
                    ID += (char)new Random().Next(48, 90);
                
                foreach (Window window in ShellCore.AllWindows)
                {
                    if (window.Handle == ID)
                    {
                        Done = false;
                        break;
                    }
                }
            }
            return ID;
        }
    }
}
