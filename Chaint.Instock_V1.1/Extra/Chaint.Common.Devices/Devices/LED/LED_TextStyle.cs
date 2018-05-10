using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices.LED
{
    public class LED_TextStyle
    {
        private string m_DisplayText = "";
        private Color m_TextColor = Color.RED;
        private int m_PosX = 0;
        private int m_PosY = 0;
        private int m_CharSpace = 0;        //字符间距
        private bool m_IsTransParent = false;       //是否覆盖
        private bool m_IsRightToLeft = false;       //文本是否从右至左


        public LED_TextStyle()
        { }
        public LED_TextStyle(string strDispText, int posX, int posY,int charSpace)
            : this(strDispText, Color.RED, posX, posY,charSpace)
        { }

        public LED_TextStyle(string strDispText, int posX, int posY):this(strDispText,Color.RED,posX,posY)
        { }

        public LED_TextStyle(string strDispText, Color textColor, int posX, int posY)
            : this(strDispText, textColor, posX, posY, 0)
        { }

        public LED_TextStyle(string strDispText, Color textColor, int posX, int posY,int charSpace)
            : this(strDispText, textColor, posX, posY, 0, false, false)
        { }

        public LED_TextStyle(string strDispText,Color textColor,int posX,int posY,int charSpace,bool isTransparent,bool isRightToLeft)
        {
            m_DisplayText = strDispText;
            m_TextColor = textColor;
            m_PosX = posX;
            m_PosY = posY;
            m_CharSpace = charSpace;
            m_IsTransParent = isTransparent;
            m_IsRightToLeft = isRightToLeft;
        }


        /// <summary>
        /// 文本是否从右至左
        /// </summary>
        public bool IsRightToLeft
        {
            get { return m_IsRightToLeft; }
            set { m_IsRightToLeft = value; }
        }

        /// <summary>
        /// 是否覆盖文本
        /// </summary>
        public bool IsTransParent
        {
            get { return m_IsTransParent; }
            set { m_IsTransParent = value; }
        }

        /// <summary>
        /// 字符间距
        /// </summary>
        public int CharSpace
        {
            get { return m_CharSpace; }
            set { m_CharSpace = value; }
        }

        /// <summary>
        /// 显示文本初始位置 Y
        /// </summary>
        public int PosY
        {
            get { return m_PosY; }
            set { m_PosY = value; }
        }



        /// <summary>
        /// 显示文本初始位置 X
        /// </summary>
        public int PosX
        {
            get { return m_PosX; }
            set { m_PosX = value; }
        }

        public Color TextColor
        {
            get { return m_TextColor; }
            set { m_TextColor = value; }
        }

        public string DisplayText
        {
            get { return m_DisplayText; }
            set { m_DisplayText = value; }
        }
    }
}
