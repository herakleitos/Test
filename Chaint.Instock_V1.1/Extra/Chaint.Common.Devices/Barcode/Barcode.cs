using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using System.Collections;


/*
 * 
 * (1) ��ԭ��Barcode��Ļ������޸����û����뵥λ��ʹ������Ŀ�ȿ��Ƹ�Ϊ��ȷ;
 * (2) Magnify Ϊխ����С���,��λΪ�ܶ�(1mil=0.0254mm,1mil=1/1000Inch,1Inch=96dot
 * */

namespace Chaint.Common.Devices.Barcode
{
    public class Barcode
    {
        #region Variables

        private IBarCode iBarCode;
        private BarcodeType _barType;

        /// <summary>
        /// �����ԭʼ�ַ���
        /// </summary>
        private string _RawData;
        /// <summary>
        /// �������0��1�ַ���
        /// </summary>
        private string _EncodedData;

        /// <summary>
        /// ������ɺ��������
        /// </summary>
        public string EncodeData
        {
            get { return _EncodedData; }
        }
        /// <summary>
        /// ��Ҫ�����������
        /// </summary>
        public string RawData
        {
            set { _RawData = value; }
            get { return _RawData; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public BarcodeType EnumBarcodeType
        {
            set { _barType = value; }
            get { return _barType; }
        }

        public enum BarcodeType
        {
            CODE39, // Code39��ͨ��
            CODE39FULLASCII, //Code39��չ��
            CODE128A, //Code128A��
            CODE128B, //COde128B��
            CODE128C, //Code128C��
            EAN128,   //EAN128��
            INTERLEAVED2OF5,//����25��
            CODE128  //���128��
        }
        #endregion

        #region Construct
        public Barcode()
        {
            
        }

        public Barcode(string rawData, BarcodeType barcodeType)
        {
            _RawData = rawData;
            _barType = barcodeType;
            switch (barcodeType)
            {
                case BarcodeType.CODE39:
                    iBarCode = new Code39(rawData, Code39.Code39Model.Code39Normal, true);
                    break;
                case BarcodeType.CODE39FULLASCII:
                    iBarCode = new Code39(rawData, Code39.Code39Model.Code39FullAscII, true);
                    break;
                case BarcodeType.CODE128A:
                    iBarCode = new Code128Class(rawData, Code128Class.Encode.Code128A);
                    break;
                case BarcodeType.CODE128B:
                    iBarCode = new Code128Class(rawData, Code128Class.Encode.Code128B);
                    break;
                case BarcodeType.CODE128C:
                    iBarCode = new Code128Class(rawData, Code128Class.Encode.Code128C);
                    break;
                case BarcodeType.EAN128:
                    iBarCode = new Code128Class(rawData, Code128Class.Encode.EAN128);
                    break;
                case BarcodeType.CODE128:
                    iBarCode = new Code128(rawData);
                    break;
                case BarcodeType.INTERLEAVED2OF5:
                    iBarCode = new Interleaved2of5(rawData);
                    break;
                default:
                    throw new Exception("The BarcodeType is illegal��");
            }
            _EncodedData = iBarCode.EncodedData();
        }
        #endregion

        #region Functions

        /// <summary>
        /// ����Ƿ�ֻ������
        /// </summary>
        /// <param name="Data">������ַ���</param>
        /// <returns>ֻ�����ַ���true�����򷵻�false</returns>
        public static bool CheckNumericOnly(string Data)
        {
            //9223372036854775808 is the largest number a 64bit number(signed) can hold so ... make sure its less than that by one place
            int STRING_LENGTHS = 18;

            string temp = Data;
            string[] strings = new string[(Data.Length / STRING_LENGTHS) + ((Data.Length % STRING_LENGTHS == 0) ? 0 : 1)];

            int i = 0;
            while (i < strings.Length)
                if (temp.Length >= STRING_LENGTHS)
                {
                    strings[i++] = temp.Substring(0, STRING_LENGTHS);
                    temp = temp.Substring(STRING_LENGTHS);
                }//if
                else
                    strings[i++] = temp.Substring(0);

            foreach (string s in strings)
            {
                long value = 0;
                if (!Int64.TryParse(s, out value))
                    return false;
            }//foreach

            return true;
        }//CheckNumericOnly

        #region DrawBarCode
        /*DrawBarCode ���ڻ��������룬������Դ_EncodedData
         * magnifyΪ����ϸ����ʱ����Ŀ�ȣ�Ĭ���������1
         * heightΪ��������ʱ����ĸ߶ȣ�Ĭ�������height����λͼ�Ŀ�ȣ�����������λͼ�������Ρ����Ҫ���ַ�������߶ȼ�20%
         * barWidthRadioΪ��ɫ����԰�ɫ����ı�����Ĭ���������1��1������0�������1�䣬����ע��һ�㣺���magnify*barWidthRadioȡ����Ϊ0����
         * ����ӡ�����Ϊ1
         * fontΪ�����Ӧ���ַ��������font = null�����������·���ӡ�ַ�������������font��ӡ�����Ӧ���ַ���
         * BitmapΪ�������λͼ
        */
        /// <summary>
        /// ��ϸ������Ϊ1�����Ʋ����������ݵ�����������  
        /// </summary>
        /// <returns>���Ƶ�����λͼ</returns>
        public Bitmap DrawBarCode()
        {
            int magnify = 30;
            return DrawBarCode(magnify);
        }
        /// <summary>
        /// ��ϸ������Ϊmagify�������������·����������ݵ�����������
        /// </summary>
        /// <param name="magnify">ÿ��ϸ����Ŀ��</param>
        /// <returns>���Ƶ�����λͼ</returns>
        public Bitmap DrawBarCode(int magnify)
        {
            int height = (int)(_EncodedData.Length * FormatMagnify(magnify));
            return DrawBarCode(magnify, height);
        }
        /// <summary>
        /// ��ϸ������Ϊmagify����������߶�Ϊheight�ߣ��������·��������������ݵ�������λͼ
        /// </summary>
        /// <param name="magnify">ÿ��ϸ����Ŀ��</param>
        /// <param name="height">����ĸ߶�</param>
        /// <returns>���Ƶ�����λͼ</returns>
        public Bitmap DrawBarCode(int magnify, int height)
        {
            Font font = null;
            return DrawBarCode(magnify, height, font);
        }
        /// <summary>
        /// ��ϸ������Ϊmagify�������������·������������ݵ�������������λͼ
        /// </summary>
        /// <param name="magnify">ÿ��ϸ����Ŀ��</param>
        /// <param name="font">�����·���ӡ������</param>
        /// <returns>���Ƶ�����λͼ</returns>
        public Bitmap DrawBarCode(int magnify, Font font)
        {
            int height = (int)(_EncodedData.Length * magnify);
            return DrawBarCode(magnify, height, font);
        }
        /// <summary>
        ///��ϸ������Ϊmagify����������߶�Ϊheight���������·������������ݵ�������λͼ
        /// </summary>
        /// <param name="magnify">ϸ����Ŀ��</param>
        /// <param name="height">����߶�</param>
        /// <param name="font">�����·���ӡ������</param>
        /// <returns>���������λͼ</returns>
        public Bitmap DrawBarCode(int magnify, int height, Font font)
        {
            float barWidthRadio = 1.0f;
            return DrawBarCode(magnify, height, barWidthRadio, font);
        }
        /// <summary>
        /// ��ϸ������Ϊmagify�����������խ��ΪbarWidthRadio���������·��������������ݵ�������������λͼ
        /// </summary>
        /// <param name="magnify">ϸ����Ŀ��</param>
        /// <param name="barWidthRadio">����Ŀ�խ��</param>
        /// <returns>���������λͼ</returns>
        public Bitmap DrawBarCode(int magnify, float barWidthRadio)
        {
            int height = GetBarHeight((int)GetBarWidth(magnify, barWidthRadio) + 1);
            return DrawBarCode(magnify, height, barWidthRadio);
        }
        /// <summary>
        /// ��ϸ������Ϊmagify����������߶�Ϊheight�������խ��ΪbarWidthRadio���������·��������������ݵ�������λͼ
        /// </summary>
        /// <param name="magnify">ϸ����Ŀ��</param>
        /// <param name="height">����ĸ߶�</param>
        /// <param name="barWidthRadio">����Ŀ�խ��</param>
        /// <returns>���������λͼ</returns>
        public Bitmap DrawBarCode(int magnify, int height, float barWidthRadio)
        {
            Font font = null;
            return DrawBarCode(magnify, height, barWidthRadio, font);
        }
        /// <summary>
        /// ��ϸ������Ϊmagify�����������խ��ΪbarWidthRadio��������ײ������������ݵ�������������λͼ    
        /// </summary>
        /// <param name="magnify">ϸ����Ŀ��</param>
        /// <param name="barWidthRadio">����Ŀ�խ��</param>
        /// <param name="font">���������</param>
        /// <returns>���������λͼ</returns>
        public Bitmap DrawBarCode(int magnify, float barWidthRadio, Font font)
        {
            int height = GetBarHeight((int)GetBarWidth(magnify, barWidthRadio) + 1);
            return DrawBarCode(magnify, height, barWidthRadio, font);
        }

        /// <summary>
        /// ��ϸ������Ϊmagify����������߶�Ϊheight�������խ��ΪbarWidthRadio��������ײ��������������ݵ�����λͼ
        /// </summary>
        /// <param name="magnify">ÿ��ϸ����Ŀ��</param>
        /// <param name="height">����ĸ߶�</param>
        /// <param name="font">�����·���ӡ������</param>
        /// <returns>���Ƶ�����λͼ</returns>
        public Bitmap DrawBarCode(int magnify, int mmheight, float barWidthRadio, Font font)
        {
            return DrawBarCode(magnify, mmheight, barWidthRadio, font, 0);

            #region ����
            //if (mmheight < 0)
            //{
            //    throw new Exception("The height of barcode is illegal��");
            //}

            //char[] _Value = _EncodedData.ToCharArray();

            //if (null != font)//��ӡ�ַ�
            //{
            //    mmheight += mmheight / 5; //���߶ȼӳ�20%
            //}
            //float width = GetBarWidth(magnify, barWidthRadio);
            //int height = GetBarHeight(mmheight);
            //Bitmap _CodeImage = new Bitmap((int)width + 1, (int)height + 1);
            //Graphics _Garphics = Graphics.FromImage(_CodeImage);
            //_Garphics.FillRectangle(Brushes.White, new Rectangle(0, 0, _CodeImage.Width, _CodeImage.Height));

            //float _LenEx = 0f;//�����x��ƫ����
            //float _DrawWidth;//��������Ŀ��

            //_DrawWidth = (int)magnify;
            //for (int i = 0; i < _Value.Length; i++)
            //{
            //_DrawWidth = (int)magnify;
            //    if (_Value[i] == '1')
            //    {
            //        _DrawWidth = _DrawWidth * barWidthRadio;
            //       // if (_DrawWidth < 1) _DrawWidth = 1;
            //        _Garphics.FillRectangle(Brushes.Black, new RectangleF(_LenEx, 0, _DrawWidth, height));
            //    }
            //    else
            //    {
            //        _Garphics.FillRectangle(Brushes.White, new RectangleF(_LenEx, 0, _DrawWidth, height));
            //    }
            //    _LenEx += (int)_DrawWidth;
            //}

            //if (null != font)
            //{
            //    GetTextImage(_CodeImage, _RawData, font);
            //}
            //_Garphics.Dispose();
            //return _CodeImage;
            #endregion
        }

        public Bitmap DrawBarCode(int magnify, int mmheight, float barWidthRadio, Font font, int angle)
        {
            if (mmheight < 0)
            {
                throw new Exception("The height of barcode is illegal��");
            }

            if (null != font)//��ӡ�ַ�
            {
                mmheight += mmheight / 5; //Ϊ��ӡ�ı������߶ȼӳ�20% 
            }

            char[] _Value = _EncodedData.ToCharArray();
            float width = GetBarWidth(magnify, barWidthRadio);
            int height = GetBarHeight(mmheight);
            Bitmap _CodeImage = new Bitmap((int)width + 1, (int)height + 1);
            Graphics _Garphics = Graphics.FromImage(_CodeImage);
            _Garphics.FillRectangle(Brushes.White, new Rectangle(0, 0, _CodeImage.Width, _CodeImage.Height));

            float _LenEx = 0;//�����x��ƫ����
            float _DrawWidth = FormatMagnify(magnify);//��������Ŀ��

            for (int i = 0; i < _Value.Length; i++)
            {
                _DrawWidth = FormatMagnify(magnify);
                if (_Value[i] == '1')
                {
                    _DrawWidth = _DrawWidth * barWidthRadio;
                    //if (_DrawWidth < 1) _DrawWidth = 1;
                    _Garphics.FillRectangle(Brushes.Black, new RectangleF(_LenEx, 0, _DrawWidth, height));
                }
                else
                {
                    _Garphics.FillRectangle(Brushes.White, new RectangleF(_LenEx, 0, _DrawWidth, height));
                }
                _LenEx += _DrawWidth;
            }
            if (null != font)
            {
                GetTextImage(_CodeImage, _RawData, font);
            }
            if (angle > 0)
                _CodeImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
            _Garphics.Dispose();
            return _CodeImage;
        }
        #endregion

        /// <summary>
        /// ���������·��ַ�
        /// </summary>
        /// <param name="bitmap">�����λͼ</param>
        /// <param name="viewText">�ַ���</param>
        /// <param name="font">����</param>
        private void GetTextImage(Bitmap bitmap, string viewText, Font font)
        {
            if (font == null)//����Ϊnull����˵������Ҫ��ӡ��������
            {
                return;
            }
            Graphics _Graphics = Graphics.FromImage(bitmap);

            SizeF _FontSize = _Graphics.MeasureString(viewText, font);
            if (_FontSize.Height > bitmap.Height - 20 || _FontSize.Width > bitmap.Width)//����������ٴ�ӡ����ֱ�ӷ���
            {
                _Graphics.Dispose();
                return;
            }

            //�������·�����λ�û����ַ�
            int starHeight = bitmap.Height - (int)_FontSize.Height;
            _Graphics.FillRectangle(Brushes.White, new Rectangle(0, starHeight, bitmap.Width, (int)_FontSize.Height));
            int starWidth = (bitmap.Width - (int)_FontSize.Width) / 2;
            char[] rawTxt = viewText.ToCharArray();
            float distanceSpan = ((bitmap.Width - _FontSize.Width / rawTxt.Length - 5) / (rawTxt.Length - 1));
            if (distanceSpan < 1)
                _Graphics.DrawString(viewText, font, Brushes.Black, starWidth, starHeight);
            else
            {
                int currX = 0;
                for (int i = 0; i < rawTxt.Length; i++)
                {
                    _Graphics.DrawString(rawTxt[i].ToString(), font, Brushes.Black, currX + i * distanceSpan, starHeight);
                }
            }
            _Graphics.Dispose();
        }

        /// <summary>
        /// �õ�����Ŀ��
        /// </summary>
        /// <param name="magnify">��Сխ�����</param>
        /// <param name="barWidthRadio">�����խ��</param>
        /// <returns>������</returns>
        /// 
        private float GetBarWidth(int magnify, float barWidthRadio)
        {
            int oneCharCount = 0;//'1'�ַ��ĸ���
            int zeroCharCount = 0;//'0'�ַ��ĸ���

            //Changed   magnify խ����ȵ�λ(mil=1/1000Ӣ��,1Ӣ��=96point)
            float formatMinWidth = FormatMagnify(magnify);

            foreach (char c in _EncodedData)
            {
                if (c == '1')
                    oneCharCount++;
                else
                    zeroCharCount++;
            }

            if ((formatMinWidth * barWidthRadio) < 1)//���ÿ���������ӡ���С��1��ȡ�������0�����������޷���ӡ������ˣ���ÿ��������Ĵ�ӡ������ó�1
            {
                return (oneCharCount + zeroCharCount * formatMinWidth) + 1;
            }
            if (_barType != BarcodeType.CODE39 &&
                _barType != BarcodeType.CODE39FULLASCII &&
                _barType != BarcodeType.INTERLEAVED2OF5)
            {
                return (formatMinWidth * ((oneCharCount * barWidthRadio) + zeroCharCount)) + 1;
            }
            else
            {
                return (formatMinWidth * ((oneCharCount * barWidthRadio) + zeroCharCount)) + 1;
            }
        }

        /// <summary>
        /// ���ܶ���λת��Ϊ����
        /// </summary>
        /// <param name="magnify"></param>
        /// <returns></returns>
        private float FormatMagnify(int magnify)
        {
            //Changed   magnify խ����ȵ�λ(mil=1/1000Ӣ��,1Ӣ��=96point)
            return magnify * 0.096F;
        }

        /// <summary>
        /// ����߶ȣ���λΪMM,��Ҫת��Ϊ���ص� 1���ס�3.78���� 
        /// </summary>
        /// <param name="barHeight">�Ժ���Ϊ��λ������߶�</param>
        /// <returns></returns>
        private int GetBarHeight(int barHeight)
        {
            return (int)(barHeight * 3.78F + 0.5);
        }
        #endregion
    }

    //�ӿڶ���
    public interface IBarCode
    {
        string EncodedData();
    }

    class Interleaved2of5 : Barcode, IBarCode
    {
        #region Variables
        private string _Raw_Data;//ԭʼ����
        private string[] I25_Code = { "NNWWN", "WNNNW", "NWNNW", "WWNNN", "NNWNW", "WNWNN", "NWWNN", "NNNWW", "WNNWN", "NWNWN" };
        #endregion

        #region Construct
        public Interleaved2of5(string input)
        {
            _Raw_Data = input;
            //_Height = input.Length * 7;
        }
        #endregion

        #region Functions

        /// <summary>
        /// ʹ�ý���25�����
        /// </summary>
        public string EncodedData()
        {
            if (_Raw_Data.Length % 2 != 0)//�����ַ�������������
            {
                _Raw_Data = string.Format("0{0}", _Raw_Data);
                //throw new Exception("����25�볤�ȱ�����ż��.");
            }

            if (!Barcode.CheckNumericOnly(_Raw_Data)) //ԭʼ�����а����˷����ֵ��ַ�
            {
                throw new Exception("The raw data  is must digit for Interleaved2of5 Code!");
            }

            string result = "1010";//��ʼλ

            for (int i = 0; i < _Raw_Data.Length; i += 2)
            {
                bool bars = true;
                string patternbars = I25_Code[Int32.Parse(_Raw_Data[i].ToString())];
                string patternspaces = I25_Code[Int32.Parse(_Raw_Data[i + 1].ToString())];
                string patternmixed = "";

                //interleave
                while (patternbars.Length > 0)
                {
                    patternmixed += patternbars[0].ToString() + patternspaces[0].ToString();
                    patternbars = patternbars.Substring(1);
                    patternspaces = patternspaces.Substring(1);
                }

                foreach (char c1 in patternmixed)
                {
                    if (bars)
                    {
                        if (c1 == 'N')
                            result += "1";
                        else
                            result += "11";
                    }
                    else
                    {
                        if (c1 == 'N')
                            result += "0";
                        else
                            result += "00";
                    }
                    bars = !bars;
                }
            }//for
            result += "1101";           //���ӽ���λ
            return result;
        }

        #endregion
    }//class 

    public class Code39 : Barcode, IBarCode
    {
        #region Variables

        public enum Code39Model
        {
            Code39Normal,               //������� 1234567890ABC
            Code39FullAscII             //ȫASCII��ʽ +A+B ����ʾСд
        }

        private Hashtable _Code39 = new Hashtable();
        private Code39.Code39Model _Model;
        private string _RawData = "";                   //��Ҫ��ӡ���ַ�
        private bool _StarChar = true;                  //Ϊtrue������������ַ����в��ܰ���"*",Ϊfalse���������ַ����п��԰���*��

        #endregion

        #region Construct

        public Code39(string text)
        {
            this._RawData = text;

            _Code39.Add("A", "1101010010110");
            _Code39.Add("B", "1011010010110");
            _Code39.Add("C", "1101101001010");
            _Code39.Add("D", "1010110010110");
            _Code39.Add("E", "1101011001010");
            _Code39.Add("F", "1011011001010");
            _Code39.Add("G", "1010100110110");
            _Code39.Add("H", "1101010011010");
            _Code39.Add("I", "1011010011010");
            _Code39.Add("J", "1010110011010");
            _Code39.Add("K", "1101010100110");
            _Code39.Add("L", "1011010100110");
            _Code39.Add("M", "1101101010010");
            _Code39.Add("N", "1010110100110");
            _Code39.Add("O", "1101011010010");
            _Code39.Add("P", "1011011010010");
            _Code39.Add("Q", "1010101100110");
            _Code39.Add("R", "1101010110010");
            _Code39.Add("S", "1011010110010");
            _Code39.Add("T", "1010110110010");
            _Code39.Add("U", "1100101010110");
            _Code39.Add("V", "1001101010110");
            _Code39.Add("W", "1100110101010");
            _Code39.Add("X", "1001011010110");
            _Code39.Add("Y", "1100101101010");
            _Code39.Add("Z", "1001101101010");
            _Code39.Add("0", "1010011011010");
            _Code39.Add("1", "1101001010110");
            _Code39.Add("2", "1011001010110");
            _Code39.Add("3", "1101100101010");
            _Code39.Add("4", "1010011010110");
            _Code39.Add("5", "1101001101010");
            _Code39.Add("6", "1011001101010");
            _Code39.Add("7", "1010010110110");
            _Code39.Add("8", "1101001011010");
            _Code39.Add("9", "1011001011010");
            _Code39.Add("+", "1001010010010");
            _Code39.Add("-", "1001010110110");
            _Code39.Add("*", "1001011011010");
            _Code39.Add("/", "1001001010010");
            _Code39.Add("%", "1010010010010");
            _Code39.Add("$", "1001001001010");
            _Code39.Add(".", "1100101011010");
            _Code39.Add(" ", "1001101011010");
        }
        public Code39(string text, Code39.Code39Model type)
            : this(text)
        {
            this._Model = type;
        }
        public Code39(string text, Code39.Code39Model type, bool IsAllowStarInput)
            : this(text, type)
        {
            this._StarChar = IsAllowStarInput;
        }

        #endregion

        #region Functions
        /// <summary>
        /// �������ͼ��
        /// </summary>
        /// <param name="p_Text">������Ϣ</param>
        /// <param name="p_Model">���</param>
        /// <param name="p_StarChar">�ַ�����ʾʱ�Ƿ�����ǰ��*��</param>
        /// <returns>ͼ��</returns>
        public string EncodedData()
        {
            string _ValueText = "";
            string _CodeText = "";//0��1�ַ���
            char[] _ValueChar = null;

            switch (_Model)
            {
                //Code39 �������
                case Code39Model.Code39Normal:
                    _ValueText = _RawData.ToUpper();
                    break;
                //ȫASCII��ʽ
                default:
                    _ValueChar = _RawData.ToCharArray();
                    for (int i = 0; i < _ValueChar.Length; i++)
                    {

                        if ((int)_ValueChar[i] >= 97 && (int)_ValueChar[i] <= 122)//��'a'~'z'ת���'A'~'Z',�����ַ���
                        {
                            _ValueText += "+" + _ValueChar[i].ToString().ToUpper();//ȫASCII���ַ�'a'��Ӧ'+A'

                        }
                        else//�����ַ�ֱ�Ӽ����ַ���
                        {
                            _ValueText += _ValueChar[i].ToString();
                        }
                    }
                    break;
            }//switch(_Model)

            _ValueChar = _ValueText.ToCharArray();

            if (_StarChar == true)//����ʼ�ַ�
            {
                _CodeText += _Code39["*"];//�ַ����ײ�����'*'
                string st = _Code39["1"].ToString();
            }
            //���ַ���ת���0��1�ַ���
            for (int i = 0; i < _ValueChar.Length; i++)
            {
                if (_StarChar == true && _ValueChar[i] == '*')
                {
                    throw new Exception("������ʼ���Ų��ܳ���*");
                }
                object _CharCode = _Code39[_ValueChar[i].ToString()];
                if (_CharCode == null)
                {
                    throw new Exception("�����õ��ַ�" + _ValueChar[i].ToString());
                }
                _CodeText += _CharCode.ToString();
            }// for(int i = 0; i < _ValueChar.Length; i++)
            if (_StarChar == true)
            {
                _CodeText += _Code39["*"];
            }
            return _CodeText;
        }
        #endregion

    }//class

    class Code128 : Barcode, IBarCode
    {
        #region Variables

        public enum TYPES : int { DYNAMIC, A, B, C };
        private DataTable _C128_Code = new DataTable("C128");
        private List<string> _FormattedData = new List<string>();
        private List<string> _EncodedData = new List<string>();
        private DataRow _StartCharacter = null;                     //��ʼ�ַ���
        private TYPES _type = TYPES.DYNAMIC;
        private string _Raw_Data;                                   //ԭʼ����

        #endregion

        #region Construct

        /// <summary>
        /// Encodes data in Code128 format.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        public Code128(string input)
        {
            _Raw_Data = input;
        }//Code128

        /// <summary>
        /// Encodes data in Code128 format.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        /// <param name="type">Type of encoding to lock to. (Code 128A, Code 128B, Code 128C)</param>
        public Code128(string input, TYPES type)
        {
            this._type = type;
            _Raw_Data = input;
        }//Code128

        #endregion

        #region Properties
        public string Encoded_Value
        {
            get { return EncodedData(); }
        }

        #endregion

        #region Functions
        public string EncodedData()
        {
            //initialize datastructure to hold encoding information
            this.init_Code128();
            return GetEncoding();       //Encode_Code128
        }

        private void init_Code128()
        {
            //set the table to case sensitive since there are upper and lower case values
            this._C128_Code.CaseSensitive = true;

            //set up columns
            this._C128_Code.Columns.Add("Value", typeof(string));
            this._C128_Code.Columns.Add("A", typeof(string));
            this._C128_Code.Columns.Add("B", typeof(string));
            this._C128_Code.Columns.Add("C", typeof(string));
            this._C128_Code.Columns.Add("Encoding", typeof(string));

            //populate data
            this._C128_Code.Rows.Add(new object[] { "0", " ", " ", "00", "11011001100" });
            this._C128_Code.Rows.Add(new object[] { "1", "!", "!", "01", "11001101100" });
            this._C128_Code.Rows.Add(new object[] { "2", "\"", "\"", "02", "11001100110" });
            this._C128_Code.Rows.Add(new object[] { "3", "#", "#", "03", "10010011000" });
            this._C128_Code.Rows.Add(new object[] { "4", "$", "$", "04", "10010001100" });
            this._C128_Code.Rows.Add(new object[] { "5", "%", "%", "05", "10001001100" });
            this._C128_Code.Rows.Add(new object[] { "6", "&", "&", "06", "10011001000" });
            this._C128_Code.Rows.Add(new object[] { "7", "'", "'", "07", "10011000100" });
            this._C128_Code.Rows.Add(new object[] { "8", "(", "(", "08", "10001100100" });
            this._C128_Code.Rows.Add(new object[] { "9", ")", ")", "09", "11001001000" });
            this._C128_Code.Rows.Add(new object[] { "10", "*", "*", "10", "11001000100" });
            this._C128_Code.Rows.Add(new object[] { "11", "+", "+", "11", "11000100100" });
            this._C128_Code.Rows.Add(new object[] { "12", ",", ",", "12", "10110011100" });
            this._C128_Code.Rows.Add(new object[] { "13", "-", "-", "13", "10011011100" });
            this._C128_Code.Rows.Add(new object[] { "14", ".", ".", "14", "10011001110" });
            this._C128_Code.Rows.Add(new object[] { "15", "/", "/", "15", "10111001100" });
            this._C128_Code.Rows.Add(new object[] { "16", "0", "0", "16", "10011101100" });
            this._C128_Code.Rows.Add(new object[] { "17", "1", "1", "17", "10011100110" });
            this._C128_Code.Rows.Add(new object[] { "18", "2", "2", "18", "11001110010" });
            this._C128_Code.Rows.Add(new object[] { "19", "3", "3", "19", "11001011100" });
            this._C128_Code.Rows.Add(new object[] { "20", "4", "4", "20", "11001001110" });
            this._C128_Code.Rows.Add(new object[] { "21", "5", "5", "21", "11011100100" });
            this._C128_Code.Rows.Add(new object[] { "22", "6", "6", "22", "11001110100" });
            this._C128_Code.Rows.Add(new object[] { "23", "7", "7", "23", "11101101110" });
            this._C128_Code.Rows.Add(new object[] { "24", "8", "8", "24", "11101001100" });
            this._C128_Code.Rows.Add(new object[] { "25", "9", "9", "25", "11100101100" });
            this._C128_Code.Rows.Add(new object[] { "26", ":", ":", "26", "11100100110" });
            this._C128_Code.Rows.Add(new object[] { "27", ";", ";", "27", "11101100100" });
            this._C128_Code.Rows.Add(new object[] { "28", "<", "<", "28", "11100110100" });
            this._C128_Code.Rows.Add(new object[] { "29", "=", "=", "29", "11100110010" });
            this._C128_Code.Rows.Add(new object[] { "30", ">", ">", "30", "11011011000" });
            this._C128_Code.Rows.Add(new object[] { "31", "?", "?", "31", "11011000110" });
            this._C128_Code.Rows.Add(new object[] { "32", "@", "@", "32", "11000110110" });
            this._C128_Code.Rows.Add(new object[] { "33", "A", "A", "33", "10100011000" });
            this._C128_Code.Rows.Add(new object[] { "34", "B", "B", "34", "10001011000" });
            this._C128_Code.Rows.Add(new object[] { "35", "C", "C", "35", "10001000110" });
            this._C128_Code.Rows.Add(new object[] { "36", "D", "D", "36", "10110001000" });
            this._C128_Code.Rows.Add(new object[] { "37", "E", "E", "37", "10001101000" });
            this._C128_Code.Rows.Add(new object[] { "38", "F", "F", "38", "10001100010" });
            this._C128_Code.Rows.Add(new object[] { "39", "G", "G", "39", "11010001000" });
            this._C128_Code.Rows.Add(new object[] { "40", "H", "H", "40", "11000101000" });
            this._C128_Code.Rows.Add(new object[] { "41", "I", "I", "41", "11000100010" });
            this._C128_Code.Rows.Add(new object[] { "42", "J", "J", "42", "10110111000" });
            this._C128_Code.Rows.Add(new object[] { "43", "K", "K", "43", "10110001110" });
            this._C128_Code.Rows.Add(new object[] { "44", "L", "L", "44", "10001101110" });
            this._C128_Code.Rows.Add(new object[] { "45", "M", "M", "45", "10111011000" });
            this._C128_Code.Rows.Add(new object[] { "46", "N", "N", "46", "10111000110" });
            this._C128_Code.Rows.Add(new object[] { "47", "O", "O", "47", "10001110110" });
            this._C128_Code.Rows.Add(new object[] { "48", "P", "P", "48", "11101110110" });
            this._C128_Code.Rows.Add(new object[] { "49", "Q", "Q", "49", "11010001110" });
            this._C128_Code.Rows.Add(new object[] { "50", "R", "R", "50", "11000101110" });
            this._C128_Code.Rows.Add(new object[] { "51", "S", "S", "51", "11011101000" });
            this._C128_Code.Rows.Add(new object[] { "52", "T", "T", "52", "11011100010" });
            this._C128_Code.Rows.Add(new object[] { "53", "U", "U", "53", "11011101110" });
            this._C128_Code.Rows.Add(new object[] { "54", "V", "V", "54", "11101011000" });
            this._C128_Code.Rows.Add(new object[] { "55", "W", "W", "55", "11101000110" });
            this._C128_Code.Rows.Add(new object[] { "56", "X", "X", "56", "11100010110" });
            this._C128_Code.Rows.Add(new object[] { "57", "Y", "Y", "57", "11101101000" });
            this._C128_Code.Rows.Add(new object[] { "58", "Z", "U", "58", "11101100010" });
            this._C128_Code.Rows.Add(new object[] { "59", "[", "[", "59", "11100011010" });
            this._C128_Code.Rows.Add(new object[] { "60", @"\", @"\", "60", "11101111010" });
            this._C128_Code.Rows.Add(new object[] { "61", "]", "]", "61", "11001000010" });
            this._C128_Code.Rows.Add(new object[] { "62", "^", "^", "62", "11110001010" });
            this._C128_Code.Rows.Add(new object[] { "63", "_", "_", "63", "10100110000" });
            this._C128_Code.Rows.Add(new object[] { "64", "\0", "`", "64", "10100001100" });
            this._C128_Code.Rows.Add(new object[] { "65", Convert.ToChar(1).ToString(), "a", "65", "10010110000" });
            this._C128_Code.Rows.Add(new object[] { "66", Convert.ToChar(2).ToString(), "b", "66", "10010000110" });
            this._C128_Code.Rows.Add(new object[] { "67", Convert.ToChar(3).ToString(), "c", "67", "10000101100" });
            this._C128_Code.Rows.Add(new object[] { "68", Convert.ToChar(4).ToString(), "d", "68", "10000100110" });
            this._C128_Code.Rows.Add(new object[] { "69", Convert.ToChar(5).ToString(), "e", "69", "10110010000" });
            this._C128_Code.Rows.Add(new object[] { "70", Convert.ToChar(6).ToString(), "f", "70", "10110000100" });
            this._C128_Code.Rows.Add(new object[] { "71", Convert.ToChar(7).ToString(), "g", "71", "10011010000" });
            this._C128_Code.Rows.Add(new object[] { "72", Convert.ToChar(8).ToString(), "h", "72", "10011000010" });
            this._C128_Code.Rows.Add(new object[] { "73", Convert.ToChar(9).ToString(), "i", "73", "10000110100" });
            this._C128_Code.Rows.Add(new object[] { "74", Convert.ToChar(10).ToString(), "j", "74", "10000110010" });
            this._C128_Code.Rows.Add(new object[] { "75", Convert.ToChar(11).ToString(), "k", "75", "11000010010" });
            this._C128_Code.Rows.Add(new object[] { "76", Convert.ToChar(12).ToString(), "l", "76", "11001010000" });
            this._C128_Code.Rows.Add(new object[] { "77", Convert.ToChar(13).ToString(), "m", "77", "11110111010" });
            this._C128_Code.Rows.Add(new object[] { "78", Convert.ToChar(14).ToString(), "n", "78", "11000010100" });
            this._C128_Code.Rows.Add(new object[] { "79", Convert.ToChar(15).ToString(), "o", "79", "10001111010" });
            this._C128_Code.Rows.Add(new object[] { "80", Convert.ToChar(16).ToString(), "p", "80", "10100111100" });
            this._C128_Code.Rows.Add(new object[] { "81", Convert.ToChar(17).ToString(), "q", "81", "10010111100" });
            this._C128_Code.Rows.Add(new object[] { "82", Convert.ToChar(18).ToString(), "r", "82", "10010011110" });
            this._C128_Code.Rows.Add(new object[] { "83", Convert.ToChar(19).ToString(), "s", "83", "10111100100" });
            this._C128_Code.Rows.Add(new object[] { "84", Convert.ToChar(20).ToString(), "t", "84", "10011110100" });
            this._C128_Code.Rows.Add(new object[] { "85", Convert.ToChar(21).ToString(), "u", "85", "10011110010" });
            this._C128_Code.Rows.Add(new object[] { "86", Convert.ToChar(22).ToString(), "v", "86", "11110100100" });
            this._C128_Code.Rows.Add(new object[] { "87", Convert.ToChar(23).ToString(), "w", "87", "11110010100" });
            this._C128_Code.Rows.Add(new object[] { "88", Convert.ToChar(24).ToString(), "x", "88", "11110010010" });
            this._C128_Code.Rows.Add(new object[] { "89", Convert.ToChar(25).ToString(), "y", "89", "11011011110" });
            this._C128_Code.Rows.Add(new object[] { "90", Convert.ToChar(26).ToString(), "z", "90", "11011110110" });
            this._C128_Code.Rows.Add(new object[] { "91", Convert.ToChar(27).ToString(), "{", "91", "11110110110" });
            this._C128_Code.Rows.Add(new object[] { "92", Convert.ToChar(28).ToString(), "|", "92", "10101111000" });
            this._C128_Code.Rows.Add(new object[] { "93", Convert.ToChar(29).ToString(), "}", "93", "10100011110" });
            this._C128_Code.Rows.Add(new object[] { "94", Convert.ToChar(30).ToString(), "~", "94", "10001011110" });

            this._C128_Code.Rows.Add(new object[] { "95", Convert.ToChar(31).ToString(), Convert.ToChar(127).ToString(), "95", "10111101000" });
            this._C128_Code.Rows.Add(new object[] { "96", Convert.ToChar(202).ToString()/*FNC3*/, Convert.ToChar(202).ToString()/*FNC3*/, "96", "10111100010" });
            this._C128_Code.Rows.Add(new object[] { "97", Convert.ToChar(201).ToString()/*FNC2*/, Convert.ToChar(201).ToString()/*FNC2*/, "97", "11110101000" });
            this._C128_Code.Rows.Add(new object[] { "98", "SHIFT", "SHIFT", "98", "11110100010" });
            this._C128_Code.Rows.Add(new object[] { "99", "CODE_C", "CODE_C", "99", "10111011110" });
            this._C128_Code.Rows.Add(new object[] { "100", "CODE_B", Convert.ToChar(203).ToString()/*FNC4*/, "CODE_B", "10111101110" });
            this._C128_Code.Rows.Add(new object[] { "101", Convert.ToChar(203).ToString()/*FNC4*/, "CODE_A", "CODE_A", "11101011110" });
            this._C128_Code.Rows.Add(new object[] { "102", Convert.ToChar(200).ToString()/*FNC1*/, Convert.ToChar(200).ToString()/*FNC1*/, Convert.ToChar(200).ToString()/*FNC1*/, "11110101110" });
            this._C128_Code.Rows.Add(new object[] { "103", "START_A", "START_A", "START_A", "11010000100" });
            this._C128_Code.Rows.Add(new object[] { "104", "START_B", "START_B", "START_B", "11010010000" });
            this._C128_Code.Rows.Add(new object[] { "105", "START_C", "START_C", "START_C", "11010011100" });
            this._C128_Code.Rows.Add(new object[] { "", "STOP", "STOP", "STOP", "11000111010" });
        }                  //init_Code128

        private List<DataRow> FindStartorCodeCharacter(string s, ref int col)
        {
            List<DataRow> rows = new List<DataRow>();

            //if two chars are numbers then START_C or CODE_C
            //��������ַ������֣�����START_C����CODE_C
            if (s.Length > 1 && Char.IsNumber(s[0]) && Char.IsNumber(s[1]))
            {
                //��ʼ�ַ�Ϊ��
                if (_StartCharacter == null)
                {
                    _StartCharacter = this._C128_Code.Select("A = 'START_C'")[0];
                    rows.Add(_StartCharacter);
                }//if
                else//��ʼ�ַ���Ϊ��
                    rows.Add(this._C128_Code.Select("A = 'CODE_C'")[0]);

                col = 1;
            }//if
            else//��ʼ�ַ�������������
            {
                bool AFound = false;
                bool BFound = false;
                foreach (DataRow row in this._C128_Code.Rows)
                {
                    try
                    {
                        if (!AFound && s == row["A"].ToString())//��д��
                        {
                            AFound = true;
                            col = 2;

                            if (_StartCharacter == null)
                            {
                                _StartCharacter = this._C128_Code.Select("A = 'START_A'")[0];
                                rows.Add(_StartCharacter);
                            }//if
                            else//��ʼ�ַ���Ϊ��
                            {
                                rows.Add(this._C128_Code.Select("B = 'CODE_A'")[0]);//first column is FNC4 so use B
                            }//else
                        }//if
                        else if (!BFound && s == row["B"].ToString())//Сд��
                        {
                            BFound = true;
                            col = 1;

                            if (_StartCharacter == null)
                            {
                                _StartCharacter = this._C128_Code.Select("A = 'START_B'")[0];
                                rows.Add(_StartCharacter);
                            }//if
                            else
                                rows.Add(this._C128_Code.Select("A = 'CODE_B'")[0]);
                        }//else
                        else if (AFound && BFound)
                            break;
                    }//try
                    catch (Exception ex)
                    {
                        throw new Exception("EC128-1: " + ex.Message);
                    }//catch
                }//foreach                

                if (rows.Count <= 0)
                    throw new Exception("EC128-2: Could not determine start character.");
            }//else

            return rows;
        }

        private string CalculateCheckDigit()
        {
            string currentStartChar = _FormattedData[0];
            uint CheckSum = 0;

            for (uint i = 0; i < _FormattedData.Count; i++)
            {
                //replace apostrophes with double apostrophes for escape chars
                string s = _FormattedData[(int)i].Replace("'", "''");

                //try to find value in the A column
                DataRow[] rows = this._C128_Code.Select("A = '" + s + "'");

                //try to find value in the B column
                if (rows.Length <= 0)
                    rows = this._C128_Code.Select("B = '" + s + "'");

                //try to find value in the C column
                if (rows.Length <= 0)
                    rows = this._C128_Code.Select("C = '" + s + "'");

                uint value = UInt32.Parse(rows[0]["Value"].ToString());
                uint addition = value * ((i == 0) ? 1 : i);
                CheckSum += addition;
            }//for

            uint Remainder = (CheckSum % 103);
            DataRow[] RetRows = this._C128_Code.Select("Value = '" + Remainder.ToString() + "'");
            return RetRows[0]["Encoding"].ToString();
        }

        private void BreakUpDataForEncoding()
        {
            string temp = "";
            string tempRawData = _Raw_Data;

            //CODE C: adds a 0 to the front of the Raw_Data if the length is not divisible by 2
            if (this._type == TYPES.C && _Raw_Data.Length % 2 > 0)
                tempRawData = "0" + _Raw_Data;

            foreach (char c in tempRawData)
            {
                if (Char.IsNumber(c))//������
                {
                    if (temp == "")
                    {
                        temp += c;
                    }//if
                    else
                    {
                        temp += c;
                        _FormattedData.Add(temp);
                        temp = "";
                    }//else
                }//if
                else//������
                {
                    if (temp != "")
                    {
                        _FormattedData.Add(temp);
                        temp = "";
                    }//if
                    _FormattedData.Add(c.ToString());
                }//else
            }//foreach

            //if something is still in temp go ahead and push it onto the queue
            if (temp != "")
            {
                _FormattedData.Add(temp);
                temp = "";
            }//if
        }

        private void InsertStartandCodeCharacters()
        {
            DataRow CurrentCodeSet = null;
            string CurrentCodeString = "";

            if (this._type != TYPES.DYNAMIC)
            {
                switch (this._type)
                {
                    case TYPES.A: _FormattedData.Insert(0, "START_A");
                        break;
                    case TYPES.B: _FormattedData.Insert(0, "START_B");
                        break;
                    case TYPES.C: _FormattedData.Insert(0, "START_C");
                        break;
                    default: throw new Exception("EC128-4: Unknown start type in fixed type encoding.");
                }
            }//if
            else
            {
                try
                {
                    for (int i = 0; i < (_FormattedData.Count); i++)
                    {
                        int col = 0;
                        List<DataRow> tempStartChars = FindStartorCodeCharacter(_FormattedData[i], ref col);

                        //check all the start characters and see if we need to stay with the same codeset or if a change of sets is required
                        bool sameCodeSet = false;
                        foreach (DataRow row in tempStartChars)
                        {
                            if (row["A"].ToString().EndsWith(CurrentCodeString) || row["B"].ToString().EndsWith(CurrentCodeString) || row["C"].ToString().EndsWith(CurrentCodeString))
                            {
                                sameCodeSet = true;
                                break;
                            }//if
                        }//foreach

                        //only insert a new code char if starting a new codeset
                        //if (CurrentCodeString == "" || !tempStartChars[0][col].ToString().EndsWith(CurrentCodeString)) /* Removed because of bug */

                        if (CurrentCodeString == "" || !sameCodeSet)
                        {
                            CurrentCodeSet = tempStartChars[0];

                            bool error = true;
                            while (error)
                            {
                                try
                                {
                                    CurrentCodeString = CurrentCodeSet[col].ToString().Split(new char[] { '_' })[1];
                                    error = false;
                                }//try
                                catch
                                {
                                    error = true;

                                    if (col++ > CurrentCodeSet.ItemArray.Length)
                                        throw new Exception("No start character found in CurrentCodeSet.");
                                }//catch
                            }//while

                            _FormattedData.Insert(i++, CurrentCodeSet[col].ToString());
                        }//if

                    }//for
                }//try
                catch (Exception ex)
                {
                    throw new Exception("EC128-3: Could not insert start and code characters.\n Message: " + ex.Message);
                }//catch
            }//else
        }

        private string GetEncoding()
        {
            //break up data for encoding
            BreakUpDataForEncoding();

            //insert the start characters
            InsertStartandCodeCharacters();

            string CheckDigit = CalculateCheckDigit();
            string Encoded_Data = "";
            foreach (string s in _FormattedData)
            {
                //handle exception with apostrophes in select statements
                string s1 = s.Replace("'", "''");

                DataRow[] E_Row = this._C128_Code.Select("A = '" + s1 + "'");

                if (E_Row.Length <= 0)
                {
                    E_Row = this._C128_Code.Select("B = '" + s1 + "'");

                    if (E_Row.Length <= 0)
                    {
                        E_Row = this._C128_Code.Select("C = '" + s1 + "'");
                    }//if
                }//if

                if (E_Row.Length <= 0)
                    throw new Exception("EC128-3: Could not find encoding of a value( " + s1 + " ) in the formatted data.");

                Encoded_Data += E_Row[0]["Encoding"].ToString();
                _EncodedData.Add(E_Row[0]["Encoding"].ToString());
            }//foreach

            //add the check digit
            Encoded_Data += CalculateCheckDigit();
            _EncodedData.Add(CalculateCheckDigit());

            //add the stop character
            Encoded_Data += this._C128_Code.Select("A = 'STOP'")[0]["Encoding"].ToString();
            _EncodedData.Add(this._C128_Code.Select("A = 'STOP'")[0]["Encoding"].ToString());

            //add the termination bars
            Encoded_Data += "11";
            _EncodedData.Add("11");

            return Encoded_Data;
        }
        #endregion
    }//class

    class Code128Class : Barcode, IBarCode
    {
        #region Variables
        public enum Encode
        {
            Code128A,
            Code128B,
            Code128C,
            EAN128
        }
        private readonly string EDNSTRING = "1100011101011";
        private DataTable m_Code128 = new DataTable();
        private string _RawData;                        //��ӡ���ַ�����
        private Encode _EncodedType;                    //���뷽ʽѡ��Code128A�� Code128B,Code128C,EAN128
        #endregion

        #region Construct
        public Code128Class(string p_text)
        {
            _RawData = p_text;
            m_Code128.Columns.Add("ID");
            m_Code128.Columns.Add("Code128A");
            m_Code128.Columns.Add("Code128B");
            m_Code128.Columns.Add("Code128C");
            m_Code128.Columns.Add("BandCode");
            m_Code128.CaseSensitive = true;

            _EncodedType = Encode.Code128A;

            #region ���ݱ�
            m_Code128.Rows.Add("0", " ", " ", "00", "11011001100");
            m_Code128.Rows.Add("1", "!", "!", "01", "11001101100");
            m_Code128.Rows.Add("2", "\"", "\"", "02", "11001100110");
            m_Code128.Rows.Add("3", "#", "#", "03", "10010011000");
            m_Code128.Rows.Add("4", "$", "$", "04", "10010001100");
            m_Code128.Rows.Add("5", "%", "%", "05", "10001001100");
            m_Code128.Rows.Add("6", "&", "&", "06", "10011001000");
            m_Code128.Rows.Add("7", "'", "'", "07", "10011000100");
            m_Code128.Rows.Add("8", "(", "(", "08", "10001100100");
            m_Code128.Rows.Add("9", ")", ")", "09", "11001001000");
            m_Code128.Rows.Add("10", "*", "*", "10", "11001000100");
            m_Code128.Rows.Add("11", "+", "+", "11", "11000100100");
            m_Code128.Rows.Add("12", ",", ",", "12", "10110011100");
            m_Code128.Rows.Add("13", "-", "-", "13", "10011011100");
            m_Code128.Rows.Add("14", ".", ".", "14", "10011001110");
            m_Code128.Rows.Add("15", "/", "/", "15", "10111001100");
            m_Code128.Rows.Add("16", "0", "0", "16", "10011101100");
            m_Code128.Rows.Add("17", "1", "1", "17", "10011100110");
            m_Code128.Rows.Add("18", "2", "2", "18", "11001110010");
            m_Code128.Rows.Add("19", "3", "3", "19", "11001011100");
            m_Code128.Rows.Add("20", "4", "4", "20", "11001001110");
            m_Code128.Rows.Add("21", "5", "5", "21", "11011100100");
            m_Code128.Rows.Add("22", "6", "6", "22", "11001110100");
            m_Code128.Rows.Add("23", "7", "7", "23", "11101101110");
            m_Code128.Rows.Add("24", "8", "8", "24", "11101001100");
            m_Code128.Rows.Add("25", "9", "9", "25", "11100101100");
            m_Code128.Rows.Add("26", ":", ":", "26", "11100100110");
            m_Code128.Rows.Add("27", ";", ";", "27", "11101100100");
            m_Code128.Rows.Add("28", "<", "<", "28", "11100110100");
            m_Code128.Rows.Add("29", "=", "=", "29", "11100110010");
            m_Code128.Rows.Add("30", ">", ">", "30", "11011011000");
            m_Code128.Rows.Add("31", "?", "?", "31", "11011000110");
            m_Code128.Rows.Add("32", "@", "@", "32", "11000110110");
            m_Code128.Rows.Add("33", "A", "A", "33", "10100011000");
            m_Code128.Rows.Add("34", "B", "B", "34", "10001011000");
            m_Code128.Rows.Add("35", "C", "C", "35", "10001000110");
            m_Code128.Rows.Add("36", "D", "D", "36", "10110001000");
            m_Code128.Rows.Add("37", "E", "E", "37", "10001101000");
            m_Code128.Rows.Add("38", "F", "F", "38", "10001100010");
            m_Code128.Rows.Add("39", "G", "G", "39", "11010001000");
            m_Code128.Rows.Add("40", "H", "H", "40", "11000101000");
            m_Code128.Rows.Add("41", "I", "I", "41", "11000100010");
            //m_Code128.Rows.Add("41", "I", "I", "41", "11000100010");
            m_Code128.Rows.Add("42", "J", "J", "42", "10110111000");
            m_Code128.Rows.Add("43", "K", "K", "43", "10110001110");
            m_Code128.Rows.Add("44", "L", "L", "44", "10001101110");
            m_Code128.Rows.Add("45", "M", "M", "45", "10111011000");
            m_Code128.Rows.Add("46", "N", "N", "46", "10111000110");
            m_Code128.Rows.Add("47", "O", "O", "47", "10001110110");
            m_Code128.Rows.Add("48", "P", "P", "48", "11101110110");
            m_Code128.Rows.Add("49", "Q", "Q", "49", "11010001110");
            m_Code128.Rows.Add("50", "R", "R", "50", "11000101110");
            m_Code128.Rows.Add("51", "S", "S", "51", "11011101000");
            m_Code128.Rows.Add("52", "T", "T", "52", "11011100010");
            m_Code128.Rows.Add("53", "U", "U", "53", "11011101110");
            m_Code128.Rows.Add("54", "V", "V", "54", "11101011000");
            m_Code128.Rows.Add("55", "W", "W", "55", "11101000110");
            m_Code128.Rows.Add("56", "X", "X", "56", "11100010110");
            m_Code128.Rows.Add("57", "Y", "Y", "57", "11101101000");
            m_Code128.Rows.Add("58", "Z", "Z", "58", "11101100010");
            m_Code128.Rows.Add("59", "[", "[", "59", "11100011010");
            m_Code128.Rows.Add("60", "\\", "\\", "60", "11101111010");
            m_Code128.Rows.Add("61", "]", "]", "61", "11001000010");
            m_Code128.Rows.Add("62", "^", "^", "62", "11110001010");
            m_Code128.Rows.Add("63", "_", "_", "63", "10100110000");
            m_Code128.Rows.Add("64", "NUL", "`", "64", "10100001100");
            m_Code128.Rows.Add("65", "SOH", "a", "65", "10010110000");
            m_Code128.Rows.Add("66", "STX", "b", "66", "10010000110");
            m_Code128.Rows.Add("67", "ETX", "c", "67", "10000101100");
            m_Code128.Rows.Add("68", "EOT", "d", "68", "10000100110");
            m_Code128.Rows.Add("69", "ENQ", "e", "69", "10110010000");
            m_Code128.Rows.Add("70", "ACK", "f", "70", "10110000100");
            m_Code128.Rows.Add("71", "BEL", "g", "71", "10011010000");
            m_Code128.Rows.Add("72", "BS", "h", "72", "10011000010");
            m_Code128.Rows.Add("73", "HT", "i", "73", "10000110100");
            m_Code128.Rows.Add("74", "LF", "j", "74", "10000110010");
            m_Code128.Rows.Add("75", "VT", "k", "75", "11000010010");
            m_Code128.Rows.Add("76", "FF", "l", "76", "11001010000");
            m_Code128.Rows.Add("77", "CR", "m", "77", "11110111010");
            m_Code128.Rows.Add("78", "SO", "n", "78", "11000010100");
            m_Code128.Rows.Add("79", "SI", "o", "79", "10001111010");
            m_Code128.Rows.Add("80", "DLE", "p", "80", "10100111100");
            m_Code128.Rows.Add("81", "DC1", "q", "81", "10010111100");
            m_Code128.Rows.Add("82", "DC2", "r", "82", "10010011110");
            m_Code128.Rows.Add("83", "DC3", "s", "83", "10111100100");
            m_Code128.Rows.Add("84", "DC4", "t", "84", "10011110100");
            m_Code128.Rows.Add("85", "NAK", "u", "85", "10011110010");
            m_Code128.Rows.Add("86", "SYN", "v", "86", "11110100100");
            m_Code128.Rows.Add("87", "ETB", "w", "87", "11110010100");
            m_Code128.Rows.Add("88", "CAN", "x", "88", "11110010010");
            m_Code128.Rows.Add("89", "EM", "y", "89", "11011011110");
            m_Code128.Rows.Add("90", "SUB", "z", "90", "11011110110");
            m_Code128.Rows.Add("91", "ESC", "{", "91", "11110110110");
            m_Code128.Rows.Add("92", "FS", "|", "92", "10101111000");
            m_Code128.Rows.Add("93", "GS", "}", "93", "10100011110");
            m_Code128.Rows.Add("94", "RS", "~", "94", "10001011110");
            m_Code128.Rows.Add("95", "US", "DEL", "95", "10111101000");
            m_Code128.Rows.Add("96", "FNC3", "FNC3", "96", "10111100010");
            m_Code128.Rows.Add("97", "FNC2", "FNC2", "97", "11110101000");
            m_Code128.Rows.Add("98", "SHIFT", "SHIFT", "98", "11110100010");
            m_Code128.Rows.Add("99", "CODEC", "CODEC", "99", "10111011110");
            m_Code128.Rows.Add("100", "CODEB", "FNC4", "CODEB", "10111101110");
            m_Code128.Rows.Add("101", "FNC4", "CODEA", "CODEA", "11101011110");
            m_Code128.Rows.Add("102", "FNC1", "FNC1", "FNC1", "11110101110");
            m_Code128.Rows.Add("103", "StartA", "StartA", "StartA", "11010000100");
            m_Code128.Rows.Add("104", "StartB", "StartB", "StartB", "11010010000");
            m_Code128.Rows.Add("105", "StartC", "StartC", "StartC", "11010011100");
            m_Code128.Rows.Add("106", "Stop", "Stop", "Stop", "11000111010");
            #endregion
        }
        public Code128Class(string p_Text, Code128Class.Encode type)
            : this(p_Text)
        {
            this._EncodedType = type;
        }
        #endregion

        #region Functions
        public string EncodedData()
        {
            string _ViewText = _RawData;
            string _Text = "";//��Ӧ�����ݱ��е��ַ���
            IList<int> _TextNumb = new List<int>();
            int _Examine = 0;  //��ʼ�룬128AΪ103,128BΪ104��128CΪ105
            const string EAN = "11110101110";
            switch (_EncodedType)
            {
                case Encode.Code128C:
                    _Examine = 105;
                    if (!((_ViewText.Length & 1) == 0))//128Cλ��������ż��,ǰ�油0
                    {
                        _ViewText = string.Format("0{0}", _ViewText);
                        // throw new Exception("128C���ȱ�����ż��");
                    }
                    while (_ViewText.Length != 0)
                    {
                        int _Temp = 0;
                        try
                        {
                            int _CodeNumb128 = Int32.Parse(_ViewText.Substring(0, 2));
                        }
                        catch
                        {
                            throw new Exception("CODE128C�����ȫ��Ϊ���֣�");
                        }
                        _Text += GetValue(_EncodedType, _ViewText.Substring(0, 2), ref _Temp);
                        _TextNumb.Add(_Temp);
                        _ViewText = _ViewText.Remove(0, 2);
                    }
                    break;
                case Encode.EAN128:
                    _Examine = 105;//EAN128��ļ�����CODE128Cһ��
                    if (!((_ViewText.Length & 1) == 0))//EAN128��λ��������ż��
                    {
                        _ViewText = string.Format("0{0}", _ViewText);   //��ǰ��0
                        // throw new Exception("EAN128���ȱ�����ż��.");
                    }
                    _TextNumb.Add(102);
                    _Text += EAN;//EANλ
                    while (_ViewText.Length != 0)
                    {
                        int _Temp = 0;
                        try
                        {
                            int _CodeNumb128 = Int32.Parse(_ViewText.Substring(0, 2));
                        }
                        catch
                        {
                            throw new Exception("EAN128�����ȫ��Ϊ���֣�");
                        }
                        _Text += GetValue(Encode.Code128C, _ViewText.Substring(0, 2), ref _Temp);
                        _TextNumb.Add(_Temp);
                        _ViewText = _ViewText.Remove(0, 2);
                    }
                    break;
                default:
                    if (_EncodedType == Encode.Code128A)
                    {
                        _Examine = 103;
                    }
                    else
                    {
                        _Examine = 104;
                    }

                    while (_ViewText.Length != 0)
                    {
                        int _Temp = 0;
                        string _ValueCode = GetValue(_EncodedType, _ViewText.Substring(0, 1), ref _Temp);
                        if (_ValueCode.Length == 0)
                        {
                            throw new Exception("��Ч���ַ���!" + _ViewText.Substring(0, 1).ToString());
                        }
                        _Text += _ValueCode;
                        _TextNumb.Add(_Temp);
                        _ViewText = _ViewText.Remove(0, 1);
                    }
                    break;
            }
            if (_TextNumb.Count == 0)
            {
                throw new Exception("����ı���,������");
            }
            _Text = _Text.Insert(0, GetValue(_Examine)); //��ȡ��ʼλ

            //���У��λ
            for (int i = 0; i != _TextNumb.Count; i++)
            {
                _Examine += _TextNumb[i] * (i + 1);
            }
            _Examine = _Examine % 103;
            _Text += GetValue(_Examine);  //��ȡУ��λ

            _Text += EDNSTRING; //����λ
            return _Text;
        }

        /// <summary>
        /// ��ȡĿ���Ӧ������
        /// </summary>
        /// <param name="p_Code">����</param>
        /// <param name="p_Value">��ֵ A b  30</param>
        /// <param name="p_SetID">���ر��</param>
        /// <returns>����</returns>
        private string GetValue(Encode p_Code, string p_Value, ref int p_SetID)
        {
            if (m_Code128 == null)
            {
                return "";
            }
            DataRow[] _Row = m_Code128.Select(p_Code.ToString() + "='" + p_Value + "'");
            if (_Row.Length != 1)
            {
                throw new Exception("����ı���" + p_Value.ToString());
            }
            p_SetID = Int32.Parse(_Row[0]["ID"].ToString());
            return _Row[0]["BandCode"].ToString();
        }

        /// <summary>
        /// ���ݱ�Ż������
        /// </summary>
        /// <param name="p_CodeId"></param>
        /// <returns></returns>
        private string GetValue(int p_CodeId)
        {
            DataRow[] _Row = m_Code128.Select("ID='" + p_CodeId.ToString() + "'");
            if (_Row.Length != 1)
            {
                throw new Exception("��Чλ�ı������" + p_CodeId.ToString());
            }
            return _Row[0]["BandCode"].ToString();
        }
        #endregion
    }//class
}
