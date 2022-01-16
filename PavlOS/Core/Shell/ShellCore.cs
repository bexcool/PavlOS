using PavlOS.Core.Shell.Controls.Base;
using PavlOS_Dev.Core.Shell.Controls;
using PavlOS_Dev.Core.Shell.Controls.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PavlOS.Core.Shell
{
    public static class ShellCore
    {
        // Windows
        public static List<Control> AllWindows = new List<Control>();
        public static Window FocusedWindow;

        public static void AddWindow(Control Control) => AllWindows.Add(Control);
        public static void RemoveWindow(Control Control) => AllWindows.Remove(Control);

        // System controls
        public static List<Control> AllSystemControls = new List<Control>();

        public static void AddSystemControl(Control Control) => AllSystemControls.Add(Control);
        public static void RemoveSystemControl(Control Control) => AllSystemControls.Add(Control);

        // Constants
        public const int TaskBarHeight = 28;
        public const uint DesktopColor = 0x55aaaa;
    }
}
