using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PavlOS.Core.Shell.Rendering.FontRendering.Libraries
{
    public class FontPixel : FontCharLibrary
    {
        public FontPixel()
        {
            // 0 - Zero
            FontChars.Add(new FontChar('0', 5, 8, new Point[] {
                new Point(1, 0), new Point(2, 0), new Point(3, 0),      //  ---
                new Point(0, 1), new Point(4, 1),                       // -   -
                new Point(0, 2), new Point(4, 2),                       // -   -
                new Point(0, 3), new Point(4, 3),                       // -   -
                new Point(0, 4), new Point(4, 4),                       // -   -
                new Point(0, 5), new Point(4, 5),                       // -   -
                new Point(0, 6), new Point(4, 6),                       // -   -
                new Point(1, 7), new Point(2, 7), new Point(3, 7)       //  ---
            }));

            // 1 - One
            FontChars.Add(new FontChar('1', 3, 8, new Point[] {
                new Point(1, 0),                                        //  -
                new Point(0, 1), new Point(1, 1),                       // --
                new Point(1, 2),                                        //  -
                new Point(1, 3),                                        //  -
                new Point(1, 4),                                        //  -
                new Point(1, 5),                                        //  -
                new Point(1, 6),                                        //  -
                new Point(0, 7), new Point(1, 7), new Point(2, 7)       // ---
            }));

            // 2 - Two
            FontChars.Add(new FontChar('2', 5, 8, new Point[] {
                new Point(1, 0), new Point(2, 0), new Point(3, 0),                                        //  ---
                new Point(0, 1), new Point(4, 1),                                                         // -   -
                new Point(4, 2),                                                                          //     -
                new Point(3, 3),                                                                          //    -
                new Point(2, 4),                                                                          //   -
                new Point(1, 5),                                                                          //  -
                new Point(0, 6),                                                                          // -
                new Point(0, 7), new Point(1, 7), new Point(2, 7), new Point(3, 7), new Point(4, 7)       // -----
            }));

            // 3 - Three
            FontChars.Add(new FontChar('3', 5, 8, new Point[] {
                new Point(1, 0), new Point(2, 0), new Point(3, 0),                                        //  ---
                new Point(0, 1), new Point(4, 1),                                                         // -   -
                new Point(4, 2),                                                                          //     -
                new Point(2, 3), new Point(3, 3),                                                         //   --
                new Point(4, 4),                                                                          //     -
                new Point(4, 5),                                                                          //     -
                new Point(0, 6), new Point(4, 6),                                                         // -   -
                new Point(1, 7), new Point(2, 7), new Point(3, 7)                                         //  ---
            }));

            // 4 - Four
            FontChars.Add(new FontChar('4', 5, 8, new Point[] {
                new Point(3, 0),                                                                          //    -
                new Point(2, 1), new Point(3, 1),                                                         //   --
                new Point(1, 2), new Point(3, 2),                                                         //  - -
                new Point(0, 3), new Point(3, 3),                                                         // -  -
                new Point(0, 4), new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4),      // -----
                new Point(3, 5),                                                                          //    -
                new Point(3, 6),                                                                          //    -
                new Point(3, 7)                                                                           //    -
            }));

            // 5 - Five
            FontChars.Add(new FontChar('5', 5, 8, new Point[] {
                new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0),      // -----
                new Point(0, 1),                                                                          // -
                new Point(0, 2),                                                                          // -
                new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3),                       // ----
                new Point(4, 4),                                                                          //     -
                new Point(4, 5),                                                                          //     -
                new Point(0, 6), new Point(4, 6),                                                         // -   -
                new Point(1, 7), new Point(2, 7), new Point(3, 7)                                         //  ---
            }));

            // 6 - Six
            FontChars.Add(new FontChar('6', 5, 8, new Point[] {
                new Point(2, 0), new Point(3, 0),                                                         //   --
                new Point(1, 1),                                                                          //  -
                new Point(0, 2),                                                                          // -
                new Point(0, 3), new Point(1, 3), new Point(2, 3), new Point(3, 3),                       // ----
                new Point(0, 4), new Point(4, 4),                                                         // -   -
                new Point(0, 5), new Point(4, 5),                                                         // -   -
                new Point(0, 6), new Point(4, 6),                                                         // -   -
                new Point(1, 7), new Point(2, 7), new Point(3, 7)                                         //  ---
            }));

            // 7 - Seven
            FontChars.Add(new FontChar('7', 5, 8, new Point[] {
                new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0),      // -----
                new Point(4, 1),                                                                          //     -
                new Point(3, 2),                                                                          //    -
                new Point(3, 3),                                                                          //    -
                new Point(2, 4),                                                                          //   -  
                new Point(2, 5),                                                                          //   -
                new Point(1, 6),                                                                          //  -  
                new Point(1, 7)                                                                           //  -
            }));

            // 8 - Eight
            FontChars.Add(new FontChar('8', 5, 8, new Point[] {
                new Point(1, 0), new Point(2, 0), new Point(3, 0),                                        //  --- 
                new Point(0, 1), new Point(4, 1),                                                         // -   -
                new Point(0, 2), new Point(4, 2),                                                         // -   -
                new Point(1, 3), new Point(2, 3), new Point(3, 3),                                        //  ---
                new Point(0, 4), new Point(4, 4),                                                         // -   -
                new Point(0, 5), new Point(4, 5),                                                         // -   -
                new Point(0, 6), new Point(4, 6),                                                         // -   -
                new Point(1, 7), new Point(2, 7), new Point(3, 7),                                        //  ---
            }));

            // 9 - Nine
            FontChars.Add(new FontChar('9', 5, 8, new Point[] {
                new Point(1, 0), new Point(2, 0), new Point(3, 0),                                        //  --- 
                new Point(0, 1), new Point(4, 1),                                                         // -   -
                new Point(0, 2), new Point(4, 2),                                                         // -   -
                new Point(0, 3), new Point(4, 3),                                                         // -   -
                new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4),                       //  ----
                new Point(4, 5),                                                                          //     -
                new Point(3, 6),                                                                          //    -
                new Point(1, 7), new Point(2, 7),                                                         //  --
            }));
        }
    }
}
