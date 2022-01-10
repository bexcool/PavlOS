using System;
using System.Collections.Generic;
using System.Text;

namespace PavlOS.Core.Shell.Rendering.FontRendering
{
    public class FontCharLibrary
    {
        public static List<FontChar> FontChars = new List<FontChar>();
    }

    public static class FontCharLibraryExtensions
    {
        /// <summary>
        /// Gets FontChar from FontCharLibrary
        /// </summary>
        /// <param name="Char">Character to find.</param>
        /// <returns>Found character. Returns null if character doesn't exist.</returns>
        public static FontChar GetChar(this FontCharLibrary FontCharLib, char Char)
        {
            foreach (FontChar fontChar in FontCharLibrary.FontChars)
            {
                if (fontChar.Char == Char) return fontChar;
            }
            
            return null;
        }

        /// <summary>
        /// Gets if char does exist.
        /// </summary>
        /// <param name="Char">Character to find.</param>
        /// <returns>Boolean.</returns>
        public static bool CharExists(this FontCharLibrary FontCharLib, char Char)
        {
            foreach (FontChar fontChar in FontCharLibrary.FontChars)
            {
                if (fontChar.Char == Char) return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all font charactes.
        /// </summary>
        /// <returns>Found character. Returns null if character doesn't exist.</returns>
        public static FontChar[] GetAllFontChars(this FontCharLibrary FontCharLib)
        {
            return FontCharLibrary.FontChars.ToArray();
        }
    }
}
