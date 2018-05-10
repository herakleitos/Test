using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.LED
{
    public class LED_FontStyle
    {
        
        private string m_FontName="黑体";
        private int m_FontWidth = 8;
        private int m_FontHeight = 16;
        private bool m_IsBold = false;
        private bool m_IsItalic = false;

        public LED_FontStyle()
        { }

        public LED_FontStyle(int fontWidth,int fontHeight):
            this("黑体",fontWidth,fontHeight,false,false)
        { }

        public LED_FontStyle(string fontName,int fontWidth, int fontHeight) :
            this(fontName, fontWidth, fontHeight, false, false)
        { }

        public LED_FontStyle(string fontName,int fontWidth,int fontHeight,bool isBold,bool isItalic)
        {
            m_FontName = fontName;
            m_FontWidth = fontWidth;
            m_FontHeight = fontHeight;
            m_IsBold = isBold;
            m_IsItalic = isItalic;
        }

        public string FontName
        {
            get { return m_FontName; }
            set { m_FontName = value; }
        }
        

        public int FontWidth
        {
            get { return m_FontWidth; }
            set { m_FontWidth = value; }
        }
      

        public int FontHeight
        {
            get { return m_FontHeight; }
            set { m_FontHeight = value; }
        }

        

        public bool IsBold
        {
            get { return m_IsBold; }
            set { m_IsBold = value; }
        }

        
        public bool IsItalic
        {
            get { return m_IsItalic; }
            set { m_IsItalic = value; }
        }
    }
}
