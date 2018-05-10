using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Chaint.CH.Printing;
using Chaint.CH.Global;

/* Author: Hychong
 * Time: 2010.9.08
 * ===============================Changed 2011.10.24===================================================
 * 
 * (1) 新增加接口bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams,DataTable dtPrintData);
 * (2) 删除了其他接口;
 * 
 * ===============================Changed 2010.10.24===================================================
 * (1) 去掉了TCP连接类(CTCPConnector)，原因：在Imaje4020喷码机发送数据过程中有延迟,改为直接同步发送！
 * (2) 去掉了Domino\Marsh\Imaje5200的网络实现部分,原因:未测试！
 * 
 * Functions:
 * (1) 此命名空间中包含有9个类,即: 
 *      消息处理类(MessageEventHandler),
 *      打印参数集合类(JetParamsCollect),
 *      TCP网络连接类(CTCPConnector)
 *      抽象类接口(CPrintBrandOperator),
 *      类工厂(CJetPrinterFactory),
 *      Marsh喷码机类(CMarshOperation)
 *      Domino喷码机类(CDominoOperation)
 *      Imaje4020喷码机类(CImaje4020Operation)
 *      Imaje5200喷码机类(CImaje5200Operation)
 *      其中，后四者继承抽象类接口，并且由类工厂根据传入的打印参数以及喷码机类别自动识别创建相应喷码机类
 *	
 * (2) 抽象类接口中除正常的发送喷码信息（SendJetInform)外，还包含有一个消息事件，主要用于某一喷码机类与用户控件之间的通信
 * 
 * (3) 使用某一喷码机类时，首先调用打开喷码端口(OpenJetPrinter),然后发送喷码端口(SendJetInform),最后关闭喷码端口(CloseJetPrinter)
 * 
 * (4) >>>>Imaje5200喷码机的SendJetInform还未完成>>>>>>>>
 * 
 * (5) 访问方式：
 *  using Chaint.Jet
 *  JetParamsCollect jetParams=new JetParamsCollect();//此处需要为类赋初值，如：MobanName,JetPrintID,DBAccessor
 *  CJetOperation imaje4020=CJetPrinterFactory.GetJetPrinter(jetParams,InkJetType.Imaje4020)
 * imaje4020.OpenJetPrinter();
 * imaje4020.SendJetInform();
 * imaje4020.CloseJetPrinter();
 */

namespace Chaint.RHS.CommLibs.Devices
{
    public delegate void MessageEventHandler(string strMsg);
    public enum InkJetType { Marsh, Domino, Imaje4020, Imaje5200,SK3000 };

    #region 喷码机事先需要定义和初始化的参数
    public class JetParamsCollect
    {
        #region 构造函数
        public JetParamsCollect()
        { }
        #endregion

        #region 自定义函数定义
        /// 获取喷码信息行
        /// </summary>
        /// <param name="dtJetPrintRollData">需要喷印数据</param>
        /// <param name="intLineCount">行数</param>
        /// <param name="JetLines">返回的喷印行数</param>
        /// <returns></returns>
        public bool GetJetInformLines(DataTable dtJetPrintRollData, int intLineCount, ref string[] JetLines)
        {
            if (dtJetPrintRollData == null || intLineCount == 0) return false;

            DataRow[] drJetRow = null;
            JetLines = new string[intLineCount];
            try
            {
                for (int indexLine = 0; indexLine < intLineCount; indexLine++)
                {
                    drJetRow = dtJetPrintRollData.Select(string.Format("LineCode={0}", indexLine + 1));
                    if (drJetRow.Length == 0)
                    {
                        JetLines[indexLine] = "";
                        continue;
                    }
                    for (int i = 0; i < drJetRow.Length; i++)
                    {
                        JetLines[indexLine] = JetLines[indexLine] + string.Format("{0}", drJetRow[i]["FieldValue"]);
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                // SendMessage(OnProcessMessage, "获取喷印行数信息出错，原因:"+ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// 发送消息
        /// </summary>
        /// <param name="msgEventHandler">事件处理器</param>
        /// <param name="sender">发送者</param>
        /// <param name="message">待发送的消息</param>
        public void SendMessage(MessageEventHandler msgEventHandler, string message, bool blnError)
        {
            if (msgEventHandler != null && message.Trim().Length>0)
            {
                //if (blnError)
                //    msgEventHandler(string.Format("★--{0}  时间: {1}", message, DateTime.Now));
                //else
                //    msgEventHandler(string.Format("☆--{0}  时间: {1}", message, DateTime.Now));
                msgEventHandler(message);
            }
        }

        /// <summary>
        /// 显示喷码信息
        /// </summary>
        /// <param name="msgEventHandler"></param>
        /// <param name="messages"></param>
        public void DisJetInform(MessageEventHandler msgEventHandler, string[] messages)
        {
            StringBuilder sb = new StringBuilder();
            if (msgEventHandler != null)
            {
                msgEventHandler("****************喷码信息【Begin】****************");
                for (int i = 0; i < messages.Length; i++)
                {
                    sb.AppendFormat("☉☉{0}{1}{2}", messages[i], (char)13, (char)10);
                    //msgEventHandler(string.Format("☉☉{0}", messages[i]));
                }
                msgEventHandler(sb.ToString());
                msgEventHandler("****************喷码信息【End】****************");
            }
        }
        #endregion
    }
    #endregion

    #region 抽象类接口
    public abstract class CJetOperation
    {
        public abstract event MessageEventHandler OnSysMsg;
        public abstract bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams, DataTable dtPrintData);
    }
    #endregion

    #region 喷码类工厂
    public class CJetPrinterFactory
    {
        public static CJetOperation GetJetPrinter(JetParamsCollect CJetPrinterParams, InkJetType enumJetType)
        {
            switch (enumJetType)
            {
                case InkJetType.Marsh:   //Marsh打印机
                    return new CMarshOperation(CJetPrinterParams);

                case InkJetType.Domino:   //Domino打印机
                    return new CDominoOperation(CJetPrinterParams);

                case InkJetType.Imaje4020:   //Imaje4020打印机
                    return new CImaje4020Operation(CJetPrinterParams);

                case InkJetType.Imaje5200:   //Imaje5200打印机,此处发送喷码信息未完成!!!!!!!
                    return new CImaje520Operation(CJetPrinterParams);

                default:   //默认为 Domino打印机
                    return new CDominoOperation(CJetPrinterParams);
            }
        }
    }
    #endregion

    #region Marsh喷码机类

    internal class CMarshOperation : CJetOperation
    {
        #region 变量定义
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //模板参数
        private DataTable m_dtJetFormat = null;             //喷码格式
        private DataTable m_dtRollData = null;              //需要喷印的纸卷数据
        private DataTable m_dtJetRollData = null;           //根据喷码格式，把喷印字段替换成相应纸卷数据后形成的可以直接喷印的纸卷数据表
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        private StringBuilder m_sbJetResponesMsg = new StringBuilder();
        #endregion

        #region 构造函数
        public CMarshOperation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region 自定义函数
        public override bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams, DataTable dtPrintData)
        {
            string sndMsg = "";
            string strPortType = "", strPortValue = "";
            int intLineCount = 0; string[] strLines = null;
            bool blnRetValue = true;
            StringBuilder sbJetContent = new StringBuilder();
            try
            {
                m_dtMobanParams = dtMobanParams;
                m_dtJetFormat = dtFormatParams;
                m_dtRollData = dtPrintData;

                #region 初始化喷码信息各类参数
                if (dtMobanParams == null || dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板参数出错(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtFormatParams == null || dtFormatParams.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板对应的喷印格式参数出错(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "获取喷印纸卷数据出错(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "格式化喷印纸卷的喷印数据出错(函数:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

                //端口类型
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:网口，C：串口
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "根据喷码模板获取喷印行数设置错误!", true);
                    return false;
                }

                if (!m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "获取喷印行数信息出错!", true);
                    return false;
                }

                for (int i = 0; i < strLines.Length; i++)
                {
                    sbJetContent.Append(strLines[i]);
                }

                switch (strPortType)
                {
                    case "C":
                        try
                        {
                            m_spJetPrinter = new System.IO.Ports.SerialPort();
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.Two;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            m_spJetPrinter.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(m_spJetPrinter_DataReceived);
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();
                            m_spJetPrinter.Write(string.Format("{0}", (char)2)); //<Ctrl-B> 清空缓冲区
                            m_spJetPrinter.Write(string.Format("{0}", (char)20)); //<Ctrl-T> 指定信息名称
                            m_spJetPrinter.Write("CHAINT");                     //信息名称
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)22)); //<Ctrl-V> //发送消息
                            m_spJetPrinter.Write(sbJetContent.ToString());
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)1)); //<Ctrl-A> 发送打印命令  (从输入缓冲区加载message到打印缓冲区，准备打印)
                            m_spJetPrinter.Close();
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "喷码信息发送失败，原因: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    case "N":
                        /*
                        string[] strAddrInform = strPortValue.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strAddrInform.Length != 2)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, string.Format("喷码机网络端口【{0}】格式错误,正确格式如:127.0.0.1:23", strPortValue), true);
                            blnRetValue = false;
                        }
                        */
                        sndMsg = "此喷码机不支持网络端口或未实现网络发送程序!";
                        m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                        blnRetValue = false;
                        break;
                    default:    //默认情况
                        try
                        {
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.Two;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            m_spJetPrinter.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(m_spJetPrinter_DataReceived);
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();
                            m_spJetPrinter.Write(string.Format("{0}", (char)2)); //<Ctrl-B> 清空缓冲区
                            m_spJetPrinter.Write(string.Format("{0}", (char)20)); //<Ctrl-T> 指定信息名称
                            m_spJetPrinter.Write("CHAINT");                     //信息名称
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)22)); //<Ctrl-V> //发送消息
                            m_spJetPrinter.Write(sbJetContent.ToString());
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)1)); //<Ctrl-A> 发送打印命令  (从输入缓冲区加载message到打印缓冲区，准备打印)
                            m_spJetPrinter.Close();
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "喷码信息发送失败，原因: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                }
                m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //显示喷码信息
                blnRetValue = true;
            }
            catch (System.Exception ex)
            {
                sndMsg = "喷印纸卷出错，原因:" + ex.Message;
                m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                blnRetValue = false;
            }
            return blnRetValue;
        }

        private void m_spJetPrinter_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int length = m_spJetPrinter.BytesToRead;
            byte[] receive = new byte[length];
            m_spJetPrinter.Read(receive, 0, length);
            foreach (byte b in receive)
            {
                if ((char)b == (char)4)
                {
                    if (m_sbJetResponesMsg.ToString().Trim().Length > 0)
                    {
                        string strDispReceive = ">>>喷码机反馈信息:" + m_sbJetResponesMsg.ToString();
                        m_JetParamsCollect.SendMessage(OnSysMsg, strDispReceive, true);
                        m_sbJetResponesMsg = new StringBuilder();
                    }
                }
                else
                {
                    if ((char)b == (char)6) continue;
                    m_sbJetResponesMsg.AppendFormat("{0}", (char)b);
                }
            }
        }
        #endregion
    }
    #endregion

    #region Domino喷码机类
    internal class CDominoOperation : CJetOperation
    {
        #region 变量定义
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //模板参数
        private DataTable m_dtJetFormat = null;             //喷码格式
        private DataTable m_dtRollData = null;              //需要喷印的纸卷数据
        private DataTable m_dtJetRollData = null;           //根据喷码格式，把喷印字段替换成相应纸卷数据后形成的可以直接喷印的纸卷数据表
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        private StringBuilder m_sbJetResponesMsg = new StringBuilder();
        #endregion

        #region 构造函数
        public CDominoOperation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region 自定义函数
        public override bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams, DataTable dtPrintData)
        {
            string sndMsg = "";
            string strPortType = "", strPortValue = "";
            int intLineCount = 0; string[] strLines = null;
            bool blnRetValue = true;
           
            try
            {
                m_dtMobanParams = dtMobanParams;
                m_dtJetFormat = dtFormatParams;
                m_dtRollData = dtPrintData;

                #region 初始化喷码信息各类参数
                if (m_dtMobanParams == null || m_dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板参数出错(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (m_dtJetFormat == null || m_dtJetFormat.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板对应的喷印格式参数出错(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "获取喷印纸卷数据出错(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "格式化喷印纸卷的喷印数据出错(函数:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

              
                //端口类型
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:网口，C：串口
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                //获取喷印数据
                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "根据喷码模板获取喷印行数设置错误!", true);
                    return false;
                }
                if (m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "获取喷印行数信息出错!", true);
                    return false;
                }

                #region 分端口开始发送喷码信息
                switch (strPortType)
                {
                    case "C":
                        try
                        {
                            m_spJetPrinter = new System.IO.Ports.SerialPort();
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.One;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            m_spJetPrinter.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(m_spJetPrinter_DataReceived);
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();

                            m_spJetPrinter.Write(string.Format("{0}R{1}", (char)27, (char)4));
                            m_spJetPrinter.Write(string.Format("{0}P1001{1}", (char)27, (char)4));
                            m_spJetPrinter.Write(string.Format("{0}P2002{1}", (char)27, (char)4));
                            switch (strLines.Length)
                            {
                                case 4:
                                    //喷1-2行
                                    //mscJet.Output = Chr(27) & "S001" & strLine(0) & Chr(27) & "r" & strLine(1) & Chr(4)
                                    //喷3-4行
                                    //mscJet.Output = Chr(27) & "S002" & strLine(2) & Chr(27) & "r" & strLine(3) & Chr(4)
                                    m_spJetPrinter.Write(string.Format("{0}S001{1}{2}r{3}{4}", (char)27, strLines[0], (char)27, strLines[1], (char)4));
                                    m_spJetPrinter.Write(string.Format("{0}S002{1}{2}r{3}{4}", (char)27, strLines[2], (char)27, strLines[3], (char)4));
                                    break;
                                case 2:
                                    //喷1行
                                    //mscJet.Output = Chr(27) & "S001" & Chr(27) & "u2" & strLine(0) & Chr(27) & "u1" & Chr(4)
                                    //喷2行
                                    //mscJet.Output = Chr(27) & "S002" & Chr(27) & "u2" & strLine(1) & Chr(27) & "u1" & Chr(4)
                                    m_spJetPrinter.Write(string.Format("{0}S001{1}u2{2}{3}u1{4}", (char)27, (char)27, strLines[0], (char)27, (char)4));
                                    m_spJetPrinter.Write(string.Format("{0}S002{1}u2{2}{3}u1{4}", (char)27, (char)27, strLines[1], (char)27, (char)4));
                                    break;
                            }
                            //mscJet.Output = Chr(27) & "S001" & "?" & Chr(4)
                            //mscJet.Output = Chr(27) & "S002" & "?" & Chr(4)
                            m_spJetPrinter.Write(string.Format("{0}S001?{1}", (char)27, (char)4));
                            m_spJetPrinter.Write(string.Format("{0}S002?{1}", (char)27, (char)4));
                            //Chaint.CH.Global.CGlobal.DelayTime(200);
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                            blnRetValue = true;
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "喷码信息发送失败，原因: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    case "N":
                        /*
                        string[] strAddrInform = strPortValue.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strAddrInform.Length != 2)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, string.Format("喷码机网络端口【{0}】格式错误,正确格式如:127.0.0.1:23", strPortValue), true);
                            blnRetValue = false;
                        }
                        */
                        sndMsg = "此喷码机不支持网络端口或未实现网络发送程序!";
                        m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                        blnRetValue = false;
                        break;
                    default:    //默认情况
                        try
                        {
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.One;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            m_spJetPrinter.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(m_spJetPrinter_DataReceived);
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();
                            m_spJetPrinter.Write(string.Format("{0}R{1}", (char)27, (char)4));
                            m_spJetPrinter.Write(string.Format("{0}P1001{1}", (char)27, (char)4));
                            m_spJetPrinter.Write(string.Format("{0}P2002{1}", (char)27, (char)4));
                            switch (strLines.Length)
                            {
                                case 4:
                                    //喷1-2行
                                    //mscJet.Output = Chr(27) & "S001" & strLine(0) & Chr(27) & "r" & strLine(1) & Chr(4)
                                    //喷3-4行
                                    //mscJet.Output = Chr(27) & "S002" & strLine(2) & Chr(27) & "r" & strLine(3) & Chr(4)
                                    m_spJetPrinter.Write(string.Format("{0}S001{1}{2}r{3}{4}", (char)27, strLines[0], (char)27, strLines[1], (char)4));
                                    m_spJetPrinter.Write(string.Format("{0}S002{1}{2}r{3}{4}", (char)27, strLines[2], (char)27, strLines[3], (char)4));
                                    break;
                                case 2:
                                    //喷1行
                                    //mscJet.Output = Chr(27) & "S001" & Chr(27) & "u2" & strLine(0) & Chr(27) & "u1" & Chr(4)
                                    //喷2行
                                    //mscJet.Output = Chr(27) & "S002" & Chr(27) & "u2" & strLine(1) & Chr(27) & "u1" & Chr(4)
                                    m_spJetPrinter.Write(string.Format("{0}S001{1}u2{2}{3}u1{4}", (char)27, (char)27, strLines[0], (char)27, (char)4));
                                    m_spJetPrinter.Write(string.Format("{0}S002{1}u2{2}{3}u1{4}", (char)27, (char)27, strLines[1], (char)27, (char)4));
                                    break;
                            }
                            //mscJet.Output = Chr(27) & "S001" & "?" & Chr(4)
                            //mscJet.Output = Chr(27) & "S002" & "?" & Chr(4)
                            m_spJetPrinter.Write(string.Format("{0}S001?{1}", (char)27, (char)4));
                            m_spJetPrinter.Write(string.Format("{0}S002?{1}", (char)27, (char)4));
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                            blnRetValue = true;
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "喷码信息发送失败，原因: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                }
                m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //显示喷码信息
                #endregion
            }
            catch (System.Exception ex)
            {
                sndMsg = "喷印纸卷出错，原因:" + ex.Message;
                m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                blnRetValue = false;
            }
            return blnRetValue;
        }

        private void m_spJetPrinter_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int length = m_spJetPrinter.BytesToRead;
            byte[] receive = new byte[length];
            m_spJetPrinter.Read(receive, 0, length);
            foreach (byte b in receive)
            {
                if ((char)b == (char)4)
                {
                    if (m_sbJetResponesMsg.ToString().Trim().Length > 0)
                    {
                        string strDispReceive = ">>>喷码机反馈信息:" + m_sbJetResponesMsg.ToString();
                        m_JetParamsCollect.SendMessage(OnSysMsg, strDispReceive, true);
                        m_sbJetResponesMsg = new StringBuilder();
                    }
                }
                else
                {
                    if ((char)b == (char)6) continue;
                    m_sbJetResponesMsg.AppendFormat("{0}", (char)b);
                }
            }
        }
        #endregion
    }
    #endregion

    #region Imaje4020喷码机类
    internal class CImaje4020Operation : CJetOperation
    {
        #region 变量定义
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //模板参数
        private DataTable m_dtJetFormat = null;             //喷码格式
        private DataTable m_dtRollData = null;              //需要喷印的纸卷数据
        private DataTable m_dtJetRollData = null;           //根据喷码格式，把喷印字段替换成相应纸卷数据后形成的可以直接喷印的纸卷数据表
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        #endregion

        #region 构造函数
        public CImaje4020Operation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region 自定义函数
        private bool SendPrintContentBySck(string strPrintContent)
        {
            try
            {
                IPAddress printerip;
                Byte[] sendBytes = new Byte[4096];
                Encoding encoder = System.Text.Encoding.GetEncoding("ASCII");
                string sndMsg = "";
                DataRow row = null;
                row = m_dtMobanParams.Rows[0];
                string strPortType = string.Format("{0}", row["PortType"]);
                string strPortValue = string.Format("{0}", row["PortValue"]);

                string[] strAddrInform = strPortValue.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (strAddrInform.Length != 2)
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, string.Format("喷码机网络端口【{0}】格式错误,正确格式如:127.0.0.1:23", strPortValue), true);
                    return false;
                }

                if (IPAddress.TryParse(strAddrInform[0], out printerip))
                {
                    sendBytes = encoder.GetBytes(strPrintContent);
                    IPEndPoint ipPoint = new IPEndPoint(printerip, Convert.ToInt32(strAddrInform[1]));
                    Socket clientSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSck.Connect(ipPoint);
                    clientSck.Send(sendBytes, sendBytes.Length, 0);
                    System.Threading.Thread.Sleep(200);
                    clientSck.Close();
                    clientSck = null;
                    return true;
                }
                else
                {
                    sndMsg = string.Format("IP地址【{0}】输入有误，请检查打印机IP配置是否正确!", strAddrInform[0]);
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                string sndMsg = "Imaje打印机未连接成功,原因:" + ex.Message;
                m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                return false;
            }
        }
        public override bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams, DataTable dtPrintData)
        {
            string sndMsg = "";
            string strPortType = "", strPortValue = "";
            int intLineCount = 0; string[] strLines = null;
            StringBuilder sbJetContent = new StringBuilder();
            bool blnRetValue = true;

            try
            {

                m_dtMobanParams = dtMobanParams;
                m_dtJetFormat = dtFormatParams;
                m_dtRollData = dtPrintData;

                #region 初始化喷码信息各类参数
                if (dtMobanParams == null || dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板参数出错(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtFormatParams == null || dtFormatParams.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板对应的喷印格式参数出错(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "获取喷印纸卷数据出错(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "格式化喷印纸卷的喷印数据出错(函数:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

                
                //端口类型
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:网口，C：串口
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                //获取喷印数据
                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "根据喷码模板获取喷印行数设置错误!", true);
                    return false;
                }
                if (!m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "获取喷印行数信息出错!", true);
                    return false;
                }

                #region 分端口开始发送喷码信息
                switch (strPortType)
                {
                    case "N":
                        for (int i = 0; i < strLines.Length; i++)
                        {
                            //str += "!!" + "v!5" + (char)34 + row[PaperRollData.ROLL_ID_FIELD].ToString() + " " + (char)34 + (char)13;
                            sbJetContent.AppendFormat("!!v!{0}{1}{2}{3}{4}", (i + 1), (char)34, strLines[i], (char)34, (char)13);
                        }
                        if (SendPrintContentBySck(sbJetContent.ToString()))
                        {
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //显示喷码信息
                            blnRetValue = true;
                        }
                        else
                        {
                            blnRetValue = false;
                        }
                        break;
                    case "C":
                        try
                        {
                            m_spJetPrinter = new System.IO.Ports.SerialPort();
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.One;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();
                            for (int i = 0; i < strLines.Length; i++)
                            {
                                //str += "!!" + "v!5" + (char)34 + row[PaperRollData.ROLL_ID_FIELD].ToString() + " " + (char)34 + (char)13;
                                sbJetContent.AppendFormat("!!v!{0}{1}{2}{3}{4}", (i + 1), (char)34, strLines[i], (char)34, (char)13);
                            }
                            m_spJetPrinter.Write(sbJetContent.ToString());
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //显示喷码信息
                            blnRetValue = true;
                            //Chaint.CH.Global.CGlobal.DelayTime(200);
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "喷码信息发送失败，原因: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    default:    //默认网络端口
                        for (int i = 0; i < strLines.Length; i++)
                        {
                            //str += "!!" + "v!5" + (char)34 + row[PaperRollData.ROLL_ID_FIELD].ToString() + " " + (char)34 + (char)13;
                            sbJetContent.AppendFormat("!!v!{0}{1}{2}{3}{4}", (i + 1), (char)34, strLines[i], (char)34, (char)13);
                        }
                        if (SendPrintContentBySck(sbJetContent.ToString()))
                        {
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //显示喷码信息
                            blnRetValue = true;
                        }
                        else
                        {
                            blnRetValue = false;
                        }
                        break;
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                sndMsg = "喷印纸卷出错，原因:" + ex.Message;
                m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                blnRetValue = false;
            }
            return blnRetValue;
        }

        #endregion
    }
    #endregion

    #region Imaje5200喷码机类
    internal class CImaje520Operation : CJetOperation
    {
        #region 变量定义
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //模板参数
        private DataTable m_dtJetFormat = null;             //喷码格式
        private DataTable m_dtRollData = null;              //需要喷印的纸卷数据
        private DataTable m_dtJetRollData = null;           //根据喷码格式，把喷印字段替换成相应纸卷数据后形成的可以直接喷印的纸卷数据表
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        #endregion

        #region 构造函数
        public CImaje520Operation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region 自定义函数
        public override bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams, DataTable dtPrintData)
        {
            string sndMsg = "";
            string strPortType = "", strPortValue = "";
            int intLineCount = 0; string[] strLines = null;
            StringBuilder sbJetContent = new StringBuilder();
            bool blnRetValue = true;
            try
            {
                m_dtMobanParams = dtMobanParams;
                m_dtJetFormat = dtFormatParams;
                m_dtRollData = dtPrintData;

                #region 初始化喷码信息各类参数
                if (dtMobanParams == null || dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板参数出错(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtFormatParams == null || dtFormatParams.Rows.Count < 1)
                {
                    sndMsg = "获取喷码模板对应的喷印格式参数出错(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "获取喷印纸卷数据出错(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "格式化喷印纸卷的喷印数据出错(函数:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

              
                //端口类型
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:网口，C：串口
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                //获取喷印数据
                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "根据喷码模板获取喷印行数设置错误!", true);
                    return false;
                }
                if (!m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "获取喷印行数信息出错!", true);
                    return false;
                }

                #region 分端口开始发送喷码信息
                switch (strPortType)
                {
                    case "C":
                        try
                        {
                            m_spJetPrinter = new System.IO.Ports.SerialPort();
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.One;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();

                            //^UV<Sub Command>|<Number of Prints>|<Number Of Fields> |<Field Number>|<Data 1>|... |<Field Number>|<Data >| 

                            sbJetContent.AppendFormat("{0}{1}UV0|0|{2}|", (char)1, (char)94, strLines.Length);
                            for (int i = 0; i < strLines.Length; i++)
                            {
                                //UV0 | 0 | 1 | 0 | 222222 |
                                sbJetContent.AppendFormat("{0}|{1}|", i, strLines[i]);
                            }
                            sbJetContent.AppendFormat("{0}", (char)26);
                            m_spJetPrinter.Write(sbJetContent.ToString());
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //显示喷码信息
                            blnRetValue = true;
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                            blnRetValue = true;
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "喷码信息发送失败，原因: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    case "N":
                        sndMsg = "此喷码机不支持网络端口或未实现网络发送程序!";
                        m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                        blnRetValue = false;
                        break;

                    default:
                        try
                        {
                            m_spJetPrinter = new System.IO.Ports.SerialPort();
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.One;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();

                            //^UV<Sub Command>|<Number of Prints>|<Number Of Fields> |<Field Number>|<Data 1>|... |<Field Number>|<Data >| 

                            sbJetContent.AppendFormat("{0}{1}UV0|0|{2}|", (char)1, (char)94, strLines.Length);
                            for (int i = 0; i < strLines.Length; i++)
                            {
                                //UV0 | 0 | 1 | 0 | 222222 |
                                sbJetContent.AppendFormat("{0}|{1}|", i, strLines[i]);
                            }
                            sbJetContent.AppendFormat("{0}", (char)26);
                            m_spJetPrinter.Write(sbJetContent.ToString());
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //显示喷码信息
                            blnRetValue = true;
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                            blnRetValue = true;
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "喷码信息发送失败，原因: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                sndMsg = "喷印纸卷出错，原因:" + ex.Message;
                m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                blnRetValue = false;
            }
            return blnRetValue;
        }
        #endregion
    }
    #endregion
}
