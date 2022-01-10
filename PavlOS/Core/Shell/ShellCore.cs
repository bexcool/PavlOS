using PavlOS.Core.Shell.Controls.Base;
using PavlOS_Dev.Core.Shell.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace PavlOS.Core.Shell
{
    public static class ShellCore
    {
        // Window elements
        public static List<Control> AllControls = new List<Control>();
        // Windows
        public static List<Window> AllWindows = new List<Window>();

        public static void AddControl(Control Control) => AllControls.Add(Control);
        public static void RemoveControl(Control Control) => AllControls.Remove(Control);
        public static void AddWindow(Window Window) => AllWindows.Add(Window);
        public static void RemoveWindow(Window Window) => AllWindows.Remove(Window);
    }
}
