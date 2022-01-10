﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PavlOS.Core.Shell.Utility
{
    public class Thickness
    {
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int LeftRight
        {
            get { return Left + Right; }
        }
        public int TopBottom
        {
            get { return Top + Bottom; }
        }

        public Thickness()
        {
            Top = 0;
            Right = 0;
            Bottom = 0;
            Left = 0;
        }

        public Thickness(int Top, int Right, int Bottom, int Left)
        {
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
            this.Left = Left;
        }

        public Thickness(int TopBottom, int LeftRight)
        {
            Top = TopBottom;
            Right = LeftRight;
            Bottom = TopBottom;
            Left = LeftRight;
        }

        public Thickness(int Value)
        {
            Top = Value;
            Right = Value;
            Bottom = Value;
            Left = Value;
        }
    }
}
