using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chaint.Common.Devices.Devices.LED
{
    public class LED_TY
    {
        public event RunMessageEventHandler OnRunMessage;
        private LED_FontStyle m_FontSytle;
       // private LED_TextStyle m_TextStyle;
        private LED_ActionSytle m_ActionStyle;
        private Param_Base m_ParamConnect;
        private LEDDEV m_LEDHandler;

        private bool m_IsConnected = false;

        /// <summary>
        /// 当仅只有连接参数时，其他文本形式均采用默认形式
        /// </summary>
        /// <param name="paramConnect"></param>
        public LED_TY(Param_Base paramConnect)
        {
             m_ParamConnect = paramConnect;
        }

        /// <summary>
        /// 当有连接参数、文本定义以及文本进出方式定义相同时，可调用 WriteData(LED_TextStyle txtStyle)
        /// </summary>
        /// <param name="paramConnect"></param>
        /// <param name="fontSytle"></param>
        /// <param name="actionStyle"></param>
        public LED_TY(Param_Base paramConnect, LED_FontStyle fontSytle,  LED_ActionSytle actionStyle)
        {
            m_ParamConnect = paramConnect;
            m_FontSytle = fontSytle;
            m_ActionStyle = actionStyle;
        }

        private bool ConnectDevice()
        {
            m_IsConnected = false;
            int iRet=-1;
            if(m_ParamConnect is Param_LED_SerialPort)
            {
                Param_LED_SerialPort param=(Param_LED_SerialPort)m_ParamConnect;
                iRet = TYLedDll.ConnectLED_RS232(param.ScreenNO, param.PortNO, (uint)param.BaudRate, ref m_LEDHandler);
            }
            else if (m_ParamConnect is Param_LED_Ethernet)
            {
                Param_LED_Ethernet param = (Param_LED_Ethernet)m_ParamConnect;
                byte[] cIP = new byte[4];
                int ret=GetIPBytes(out cIP,param.IPAddr);
                if(ret==-1)
                {
                    SendMessage("IP地址定义格式错误");
                    return false;
                }

                iRet = TYLedDll.ConnectLED_TCPIP(ref cIP[0], param.IPPort, param.ConnTimeOut, param.RevTimeOut, ref m_LEDHandler);
            }

            if (iRet == (int)RetVal.ERR_OK)	//连接OK. 将获得的参数显示在对话框
            {
                SendMessage(string.Format("LED 连接成功,屏宽<{0}>,屏高<{1}>",m_LEDHandler.iWidth,m_LEDHandler.iHeight));
                m_IsConnected = true;
                return true;
            }
            else
            {
                SendMessage(string.Format("LED 连接失败,错误代码:<{0}>",iRet));
                m_IsConnected = false;
                return false;
            }
        }

        private bool CloseDevice()
        {
            m_IsConnected = false;
            return TYLedDll.CloseLEDConnect(m_LEDHandler.iID);  
        }

        /// <summary>
        /// 只需要定义文本的位置，内容，字体和进出方式相同
        /// </summary>
        /// <param name="txtStyle"></param>
        /// <returns></returns>
        public bool WriteData(LED_TextStyle txtStyle)
        {
            return WriteData(m_FontSytle, txtStyle, m_ActionStyle);
        }

        public bool WriteData(string strDisplayText, int posX,int posY,int charSpace)
        {
            return WriteData(m_FontSytle, new LED_TextStyle(strDisplayText,posX,posY,charSpace),m_ActionStyle);
        }

        public bool WriteData(string strDisplayText, int posX,int posY)
        {
            return WriteData(strDisplayText, posX,posY,0);
        }

        public bool WriteData(LED_FontStyle fontSytle,LED_TextStyle txtStyle,LED_ActionSytle actionStyle)
        {
            bool retVal = false;
            int iRet = 0;

            try
            {
                bool blnSucc = ConnectDevice();
                if (blnSucc)
                {
                    //清内存映象内容
                    TYLedDll.Image_Clear(m_LEDHandler.iID);

                    //汉字24x24点,黑体,未加粗,非斜体 
                    TYLedDll.Image_SetFontW(m_LEDHandler.iID,
                                            fontSytle.FontName,
                                            fontSytle.FontWidth,
                                            fontSytle.FontHeight,
                                            fontSytle.IsBold,
                                            fontSytle.IsItalic);

                    //红色,字距0,覆盖方式,左到右书写
                    TYLedDll.Image_WriteTextW(m_LEDHandler.iID,
                                              txtStyle.DisplayText,
                                              txtStyle.PosX,
                                              txtStyle.PosY,
                                              (int)txtStyle.TextColor,
                                              txtStyle.CharSpace,
                                              txtStyle.IsTransParent,
                                              txtStyle.IsRightToLeft);


                    //送画面到LED屏. 进入:立即显示,退出:无动作,中速。函数等待发送结束。
                    iRet = TYLedDll.SendImageToLED(m_LEDHandler.iID,
                                                        (byte)actionStyle.InMode,
                                                        (byte)actionStyle.InSpeed,
                                                        actionStyle.StayTime,
                                                        (byte)actionStyle.OutMode,
                                                        (byte)actionStyle.OutSpeed,
                                                        (byte)actionStyle.PlayControl,
                                                        actionStyle.TimeOrLoops,
                                                        actionStyle.IsWaitSend);

                    if (iRet != (int)RetVal.ERR_OK)
                    {
                        SendMessage(string.Format("发送数据失败,错误代码<{0}>", iRet));
                        retVal = false;
                    }
                    else
                    {
                        SendMessage("发送至LED数据成功");
                        retVal = true;
                    }
                }
                else
                {
                    SendMessage("当前LED未连接或者连接失败,不能发送数据!");
                    retVal = false;
                }
            }
            catch (Exception ex)
            {
                SendMessage(string.Format("发送数据出错,原因:{0}", ex.Message));
                retVal = false;
            }
            
            CloseDevice();      //关闭连接
            return retVal;
        }

        /// <summary>
        /// 发送多个字符串列表,每个字符串对应的字体以及发送模式的索引号与字符串列表一致
        /// </summary>
        /// <param name="fontStyles">字体队列</param>
        /// <param name="txtSytles">发送文本队列</param>
        /// <param name="actionStyles">发送模式队列</param>
        /// <returns></returns>
        public bool WriteData(IList<LED_FontStyle> fontStyles,IList<LED_TextStyle> txtStyles,IList<LED_ActionSytle> actionStyles)
        {
            bool retVal = false;
            int iRet = 0;

            //各列表内对象数量必须一致
            if (fontStyles.Count != txtStyles.Count && fontStyles.Count != actionStyles.Count) return retVal;

            try
            {
                bool blnSucc = ConnectDevice();
                if (blnSucc)
                {
                    //清内存映象内容
                    TYLedDll.Image_Clear(m_LEDHandler.iID);

                    for (int i = 0; i < txtStyles.Count;i++ )
                    {
                        //汉字24x24点,黑体,未加粗,非斜体 
                        TYLedDll.Image_SetFontW(m_LEDHandler.iID,
                                                fontStyles[i].FontName,
                                                fontStyles[i].FontWidth,
                                                fontStyles[i].FontHeight,
                                                fontStyles[i].IsBold,
                                                fontStyles[i].IsItalic);

                        //红色,字距0,覆盖方式,左到右书写
                        TYLedDll.Image_WriteTextW(m_LEDHandler.iID,
                                                  txtStyles[i].DisplayText,
                                                  txtStyles[i].PosX,
                                                  txtStyles[i].PosY,
                                                  (int)txtStyles[i].TextColor,
                                                  txtStyles[i].CharSpace,
                                                  txtStyles[i].IsTransParent,
                                                  txtStyles[i].IsRightToLeft);


                        //送画面到LED屏. 进入:立即显示,退出:无动作,中速。函数等待发送结束。
                        iRet = TYLedDll.SendImageToLED(m_LEDHandler.iID,
                                                            (byte)actionStyles[i].InMode,
                                                            (byte)actionStyles[i].InSpeed,
                                                            actionStyles[i].StayTime,
                                                            (byte)actionStyles[i].OutMode,
                                                            (byte)actionStyles[i].OutSpeed,
                                                            (byte)actionStyles[i].PlayControl,
                                                            actionStyles[i].TimeOrLoops,
                                                            actionStyles[i].IsWaitSend);

                        if (iRet != (int)RetVal.ERR_OK)
                        {
                          //  SendMessage(string.Format("发送数据失败,错误代码<{0}>", iRet));
                            retVal = false;
                            break;
                        }
                        else
                        {
                            retVal = true;
                        }
                    }

                    if(retVal)
                    {
                        SendMessage("发送至LED数据成功");
                    }
                    else
                    {
                        SendMessage(string.Format("发送数据失败,错误代码<{0}>", iRet));
                    }
                }
                else
                {
                    SendMessage("当前LED未连接或者连接失败,不能发送数据!");
                    retVal = false;
                }
            }
            catch (Exception ex)
            {
                SendMessage(string.Format("发送数据出错,原因:{0}", ex.Message));
                retVal = false;
            }

            CloseDevice();      //关闭连接
            return retVal;
        }

        /// <summary>
        /// 当各行字体和发送方式相同，仅只有文本和对应的位置不同时，可使用此方法
        /// </summary>
        /// <param name="fontStyle">字体样式</param>
        /// <param name="txtStyles">文本样式</param>
        /// <param name="actionStyle">发送模式</param>
        /// <returns></returns>
        public bool WriteData(LED_FontStyle fontStyle, IList<LED_TextStyle> txtStyles, LED_ActionSytle actionStyle)
        {
            bool retVal = false;
            int iRet = 0;

            try
            {
                bool blnSucc = ConnectDevice();
                if (blnSucc)
                {
                    //清内存映象内容
                    TYLedDll.Image_Clear(m_LEDHandler.iID);

                    for (int i = 0; i < txtStyles.Count; i++)
                    {
                        //汉字24x24点,黑体,未加粗,非斜体 
                        TYLedDll.Image_SetFontW(m_LEDHandler.iID,
                                                fontStyle.FontName,
                                                fontStyle.FontWidth,
                                                fontStyle.FontHeight,
                                                fontStyle.IsBold,
                                                fontStyle.IsItalic);

                        //红色,字距0,覆盖方式,左到右书写
                        TYLedDll.Image_WriteTextW(m_LEDHandler.iID,
                                                  txtStyles[i].DisplayText,
                                                  txtStyles[i].PosX,
                                                  txtStyles[i].PosY,
                                                  (int)txtStyles[i].TextColor,
                                                  txtStyles[i].CharSpace,
                                                  txtStyles[i].IsTransParent,
                                                  txtStyles[i].IsRightToLeft);


                        //送画面到LED屏. 进入:立即显示,退出:无动作,中速。函数等待发送结束。
                        iRet = TYLedDll.SendImageToLED(m_LEDHandler.iID,
                                                            (byte)actionStyle.InMode,
                                                            (byte)actionStyle.InSpeed,
                                                            actionStyle.StayTime,
                                                            (byte)actionStyle.OutMode,
                                                            (byte)actionStyle.OutSpeed,
                                                            (byte)actionStyle.PlayControl,
                                                            actionStyle.TimeOrLoops,
                                                            actionStyle.IsWaitSend);

                        if (iRet != (int)RetVal.ERR_OK)
                        {
                            //  SendMessage(string.Format("发送数据失败,错误代码<{0}>", iRet));
                            retVal = false;
                            break;
                        }
                        else
                        {
                            retVal = true;
                        }
                    }

                    if (retVal)
                    {
                        SendMessage("发送至LED数据成功");
                    }
                    else
                    {
                        SendMessage(string.Format("发送数据失败,错误代码<{0}>", iRet));
                    }
                }
                else
                {
                    SendMessage("当前LED未连接或者连接失败,不能发送数据!");
                    retVal = false;
                }
            }
            catch (Exception ex)
            {
                SendMessage(string.Format("发送数据出错,原因:{0}", ex.Message));
                retVal = false;
            }

            CloseDevice();      //关闭连接
            return retVal;
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        /// <returns></returns>
        public bool ClearData()
        {
            bool blnSucc = ConnectDevice();
            if (blnSucc)
            {
                //清内存映象内容
                return TYLedDll.Image_Clear(m_LEDHandler.iID);
            }
            return blnSucc;
        }

        private int GetIPBytes(out byte[] cOutByte, string strIPAddr)
        {
            cOutByte = new byte[4];
            string[] strIP = strIPAddr.Split(new char[] { '.' },StringSplitOptions.RemoveEmptyEntries);
            if (strIP.Length != 4) return -1;
            for (int i = 0; i < cOutByte.Length; i++)
            {
                cOutByte[i] = byte.Parse(strIP[i]);
            }
            return 0;
        }

        private void SendMessage(string strMsg)
        {
            if (OnRunMessage != null && strMsg.Trim().Length>0) OnRunMessage(this, strMsg);
        }

    }
}
