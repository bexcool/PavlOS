using PavlOS.Core.Shell.Controls;
using PavlOS_Dev.Core.Shell.Controls.Base;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PavlOS.Core.System32.AppExe.TestApplication
{
    public class TestWindow : Window
    {
        public TestWindow()
        {
            int test = 0;

            X = 200;
            Y = 300;
            Width = 200;
            Height = 100;

            Label label = new Label();
            label.X = 75;
            label.Y = 20;
            label.ForegroundColor = Color.Black;
            label.Content = "I am a label.";
            AddControl(label);

            Button button = new Button();
            button.X = 10;
            button.Y = 10;
            button.Width = 60;
            button.Height = 30;
            button.Content = "Click me!";
            button.OnClick += (o) =>
            {
                test = test + 1;
                label.Content = test.ToString();
                TestWindow d = new TestWindow();
            };
            AddControl(button);

            Button minus = new Button();
            minus.X = 10;
            minus.Y = 45;
            minus.Width = 60;
            minus.Height = 30;
            minus.Content = "Minus";
            minus.OnClick += (o) =>
            {
                test = test - 1;
                label.Content = test.ToString();
                Center();
            };
            AddControl(minus);
        }
    }
}
