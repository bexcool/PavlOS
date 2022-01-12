using PavlOS.Core.Shell.Controls.Base;
using PavlOS_Dev.Core.Shell.Controls;
using PavlOS_Dev.Core.Shell.Controls.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PavlOS.Core.Shell
{
    public static class ShellCore
    {
        // Windows
        public static List<Control> AllWindows = new List<Control>();

        public static void AddWindow(Control Control) => AllWindows.Add(Control);
        public static void RemoveWindow(Control Control) => AllWindows.Remove(Control);
    }
}
