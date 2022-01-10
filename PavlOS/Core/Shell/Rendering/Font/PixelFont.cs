using System;
using System.Collections.Generic;
using System.Text;

namespace PavlOS.Core.Shell.Rendering.FontRendering
{
    public class PixelFont
    {
        public FontCharLibrary FontCharLibrary { get; set; }
        public int Size { get; set; }

        public PixelFont(FontCharLibrary FontCharLibrary)
        {
            this.FontCharLibrary = FontCharLibrary;
            Size = 1;
        }
        public PixelFont(FontCharLibrary FontCharLibrary, int Size)
        {
            this.FontCharLibrary = FontCharLibrary;
            this.Size = Size;
        }
    }
}
