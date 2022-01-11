using Mosa.External.x86;
using Mosa.External.x86.Drawing;
using PavlOS.Core.Shell.Rendering.FontRendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PavlOS.Core.Shell.Utility
{
    public static class Utility
    {
        public enum Visibility { Visible, Hidden, Collapsed };
        public enum Line { Single, Double, SingleRound, DoubleSingle, SingleDouble};
        public enum HorizontalAlignment { Left, Right, Center, Stretch };
        public enum VerticalAlignment { Top, Bottom, Center, Stretch };
        public enum Orientation { Horizontal, Vertical };
        public enum ButtonState { Normal, Hovered, Pressed };
        public enum BorderStyle { Sigle, Rounded, None };
    }

    public static class SyntaxHighlight
    {
        public enum ProgrammingLanguage { CS, None }

        public const string
            KEYWORDS_CS = "abstract,event,new,struct,as,explicit,null,switch,base,extern,this,false,operator,in,throw,break,finally,out,true,fixed,override,try,case,params,typeof,catch,for,private,foreach,protected,checked,goto,public,unchecked,if,readonly,unsafe,implicit,ref,continue,return,using,virtual,default,interface,sealed,volatile,delegate,public,do,is,sizeof,while,double,lock,stackalloc,else,static,namespace",
            DATATYPES = "bool,object,byte,float,class,uint,char,ulong,ushort,const,decimal,int,sbyte,short,void,long,enum,string";

        public static Dictionary<string, ConsoleColor> GetHighlight(ProgrammingLanguage Language)
        {
            Dictionary<string, ConsoleColor> SyntaxKeywords = new Dictionary<string, ConsoleColor>();

            switch (Language)
            {
                case ProgrammingLanguage.CS:
                    {
                        foreach (string Word in KEYWORDS_CS.Split(',')) SyntaxKeywords.Add(Word, ConsoleColor.DarkBlue);
                        foreach (string Type in DATATYPES.Split(',')) SyntaxKeywords.Add(Type, ConsoleColor.Blue);
                        break;
                    }
            }

            return SyntaxKeywords;
        }
    }
}
