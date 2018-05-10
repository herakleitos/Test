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
 * (1) �����ӽӿ�bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams,DataTable dtPrintData);
 * (2) ɾ���������ӿ�;
 * 
 * ===============================Changed 2010.10.24===================================================
 * (1) ȥ����TCP������(CTCPConnector)��ԭ����Imaje4020������������ݹ��������ӳ�,��Ϊֱ��ͬ�����ͣ�
 * (2) ȥ����Domino\Marsh\Imaje5200������ʵ�ֲ���,ԭ��:δ���ԣ�
 * 
 * Functions:
 * (1) �������ռ��а�����9����,��: 
 *      ��Ϣ������(MessageEventHandler),
 *      ��ӡ����������(JetParamsCollect),
 *      TCP����������(CTCPConnector)
 *      ������ӿ�(CPrintBrandOperator),
 *      �๤��(CJetPrinterFactory),
 *      Marsh�������(CMarshOperation)
 *      Domino�������(CDominoOperation)
 *      Imaje4020�������(CImaje4020Operation)
 *      Imaje5200�������(CImaje5200Operation)
 *      ���У������߼̳г�����ӿڣ��������๤�����ݴ���Ĵ�ӡ�����Լ����������Զ�ʶ�𴴽���Ӧ�������
 *	
 * (2) ������ӿ��г������ķ���������Ϣ��SendJetInform)�⣬��������һ����Ϣ�¼�����Ҫ����ĳһ����������û��ؼ�֮���ͨ��
 * 
 * (3) ʹ��ĳһ�������ʱ�����ȵ��ô�����˿�(OpenJetPrinter),Ȼ��������˿�(SendJetInform),���ر�����˿�(CloseJetPrinter)
 * 
 * (4) >>>>Imaje5200�������SendJetInform��δ���>>>>>>>>
 * 
 * (5) ���ʷ�ʽ��
 *  using Chaint.Jet
 *  JetParamsCollect jetParams=new JetParamsCollect();//�˴���ҪΪ�ำ��ֵ���磺MobanName,JetPrintID,DBAccessor
 *  CJetOperation imaje4020=CJetPrinterFactory.GetJetPrinter(jetParams,InkJetType.Imaje4020)
 * imaje4020.OpenJetPrinter();
 * imaje4020.SendJetInform();
 * imaje4020.CloseJetPrinter();
 */

namespace Chaint.RHS.CommLibs.Devices
{
    public delegate void MessageEventHandler(string strMsg);
    public enum InkJetType { Marsh, Domino, Imaje4020, Imaje5200,SK3000 };

    #region �����������Ҫ����ͳ�ʼ���Ĳ���
    public class JetParamsCollect
    {
        #region ���캯��
        public JetParamsCollect()
        { }
        #endregion

        #region �Զ��庯������
        /// ��ȡ������Ϣ��
        /// </summary>
        /// <param name="dtJetPrintRollData">��Ҫ��ӡ����</param>
        /// <param name="intLineCount">����</param>
        /// <param name="JetLines">���ص���ӡ����</param>
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
                // SendMessage(OnProcessMessage, "��ȡ��ӡ������Ϣ����ԭ��:"+ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// ������Ϣ
        /// </summary>
        /// <param name="msgEventHandler">�¼�������</param>
        /// <param name="sender">������</param>
        /// <param name="message">�����͵���Ϣ</param>
        public void SendMessage(MessageEventHandler msgEventHandler, string message, bool blnError)
        {
            if (msgEventHandler != null && message.Trim().Length>0)
            {
                //if (blnError)
                //    msgEventHandler(string.Format("��--{0}  ʱ��: {1}", message, DateTime.Now));
                //else
                //    msgEventHandler(string.Format("��--{0}  ʱ��: {1}", message, DateTime.Now));
                msgEventHandler(message);
            }
        }

        /// <summary>
        /// ��ʾ������Ϣ
        /// </summary>
        /// <param name="msgEventHandler"></param>
        /// <param name="messages"></param>
        public void DisJetInform(MessageEventHandler msgEventHandler, string[] messages)
        {
            StringBuilder sb = new StringBuilder();
            if (msgEventHandler != null)
            {
                msgEventHandler("****************������Ϣ��Begin��****************");
                for (int i = 0; i < messages.Length; i++)
                {
                    sb.AppendFormat("����{0}{1}{2}", messages[i], (char)13, (char)10);
                    //msgEventHandler(string.Format("����{0}", messages[i]));
                }
                msgEventHandler(sb.ToString());
                msgEventHandler("****************������Ϣ��End��****************");
            }
        }
        #endregion
    }
    #endregion

    #region ������ӿ�
    public abstract class CJetOperation
    {
        public abstract event MessageEventHandler OnSysMsg;
        public abstract bool SendJetInform(DataTable dtMobanParams, DataTable dtFormatParams, DataTable dtPrintData);
    }
    #endregion

    #region �����๤��
    public class CJetPrinterFactory
    {
        public static CJetOperation GetJetPrinter(JetParamsCollect CJetPrinterParams, InkJetType enumJetType)
        {
            switch (enumJetType)
            {
                case InkJetType.Marsh:   //Marsh��ӡ��
                    return new CMarshOperation(CJetPrinterParams);

                case InkJetType.Domino:   //Domino��ӡ��
                    return new CDominoOperation(CJetPrinterParams);

                case InkJetType.Imaje4020:   //Imaje4020��ӡ��
                    return new CImaje4020Operation(CJetPrinterParams);

                case InkJetType.Imaje5200:   //Imaje5200��ӡ��,�˴�����������Ϣδ���!!!!!!!
                    return new CImaje520Operation(CJetPrinterParams);

                default:   //Ĭ��Ϊ Domino��ӡ��
                    return new CDominoOperation(CJetPrinterParams);
            }
        }
    }
    #endregion

    #region Marsh�������

    internal class CMarshOperation : CJetOperation
    {
        #region ��������
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //ģ�����
        private DataTable m_dtJetFormat = null;             //�����ʽ
        private DataTable m_dtRollData = null;              //��Ҫ��ӡ��ֽ������
        private DataTable m_dtJetRollData = null;           //���������ʽ������ӡ�ֶ��滻����Ӧֽ�����ݺ��γɵĿ���ֱ����ӡ��ֽ�����ݱ�
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        private StringBuilder m_sbJetResponesMsg = new StringBuilder();
        #endregion

        #region ���캯��
        public CMarshOperation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region �Զ��庯��
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

                #region ��ʼ��������Ϣ�������
                if (dtMobanParams == null || dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���������(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtFormatParams == null || dtFormatParams.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���Ӧ����ӡ��ʽ��������(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "��ȡ��ӡֽ�����ݳ���(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "��ʽ����ӡֽ�����ӡ���ݳ���(����:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

                //�˿�����
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:���ڣ�C������
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��������ģ���ȡ��ӡ�������ô���!", true);
                    return false;
                }

                if (!m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��ȡ��ӡ������Ϣ����!", true);
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
                            m_spJetPrinter.Write(string.Format("{0}", (char)2)); //<Ctrl-B> ��ջ�����
                            m_spJetPrinter.Write(string.Format("{0}", (char)20)); //<Ctrl-T> ָ����Ϣ����
                            m_spJetPrinter.Write("CHAINT");                     //��Ϣ����
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)22)); //<Ctrl-V> //������Ϣ
                            m_spJetPrinter.Write(sbJetContent.ToString());
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)1)); //<Ctrl-A> ���ʹ�ӡ����  (�����뻺��������message����ӡ��������׼����ӡ)
                            m_spJetPrinter.Close();
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "������Ϣ����ʧ�ܣ�ԭ��: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    case "N":
                        /*
                        string[] strAddrInform = strPortValue.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strAddrInform.Length != 2)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, string.Format("���������˿ڡ�{0}����ʽ����,��ȷ��ʽ��:127.0.0.1:23", strPortValue), true);
                            blnRetValue = false;
                        }
                        */
                        sndMsg = "���������֧������˿ڻ�δʵ�����緢�ͳ���!";
                        m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                        blnRetValue = false;
                        break;
                    default:    //Ĭ�����
                        try
                        {
                            m_spJetPrinter.PortName = strPortValue;
                            m_spJetPrinter.StopBits = System.IO.Ports.StopBits.Two;
                            m_spJetPrinter.DataBits = 8;
                            m_spJetPrinter.Parity = System.IO.Ports.Parity.None;
                            m_spJetPrinter.ReceivedBytesThreshold = 1;
                            m_spJetPrinter.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(m_spJetPrinter_DataReceived);
                            if (!m_spJetPrinter.IsOpen) m_spJetPrinter.Open();
                            m_spJetPrinter.Write(string.Format("{0}", (char)2)); //<Ctrl-B> ��ջ�����
                            m_spJetPrinter.Write(string.Format("{0}", (char)20)); //<Ctrl-T> ָ����Ϣ����
                            m_spJetPrinter.Write("CHAINT");                     //��Ϣ����
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)22)); //<Ctrl-V> //������Ϣ
                            m_spJetPrinter.Write(sbJetContent.ToString());
                            m_spJetPrinter.Write(string.Format("{0}", (char)13)); //<Ctrl-M> Enter
                            m_spJetPrinter.Write(string.Format("{0}", (char)1)); //<Ctrl-A> ���ʹ�ӡ����  (�����뻺��������message����ӡ��������׼����ӡ)
                            m_spJetPrinter.Close();
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "������Ϣ����ʧ�ܣ�ԭ��: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                }
                m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //��ʾ������Ϣ
                blnRetValue = true;
            }
            catch (System.Exception ex)
            {
                sndMsg = "��ӡֽ�����ԭ��:" + ex.Message;
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
                        string strDispReceive = ">>>�����������Ϣ:" + m_sbJetResponesMsg.ToString();
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

    #region Domino�������
    internal class CDominoOperation : CJetOperation
    {
        #region ��������
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //ģ�����
        private DataTable m_dtJetFormat = null;             //�����ʽ
        private DataTable m_dtRollData = null;              //��Ҫ��ӡ��ֽ������
        private DataTable m_dtJetRollData = null;           //���������ʽ������ӡ�ֶ��滻����Ӧֽ�����ݺ��γɵĿ���ֱ����ӡ��ֽ�����ݱ�
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        private StringBuilder m_sbJetResponesMsg = new StringBuilder();
        #endregion

        #region ���캯��
        public CDominoOperation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region �Զ��庯��
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

                #region ��ʼ��������Ϣ�������
                if (m_dtMobanParams == null || m_dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���������(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (m_dtJetFormat == null || m_dtJetFormat.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���Ӧ����ӡ��ʽ��������(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "��ȡ��ӡֽ�����ݳ���(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "��ʽ����ӡֽ�����ӡ���ݳ���(����:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

              
                //�˿�����
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:���ڣ�C������
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                //��ȡ��ӡ����
                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��������ģ���ȡ��ӡ�������ô���!", true);
                    return false;
                }
                if (m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��ȡ��ӡ������Ϣ����!", true);
                    return false;
                }

                #region �ֶ˿ڿ�ʼ����������Ϣ
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
                                    //��1-2��
                                    //mscJet.Output = Chr(27) & "S001" & strLine(0) & Chr(27) & "r" & strLine(1) & Chr(4)
                                    //��3-4��
                                    //mscJet.Output = Chr(27) & "S002" & strLine(2) & Chr(27) & "r" & strLine(3) & Chr(4)
                                    m_spJetPrinter.Write(string.Format("{0}S001{1}{2}r{3}{4}", (char)27, strLines[0], (char)27, strLines[1], (char)4));
                                    m_spJetPrinter.Write(string.Format("{0}S002{1}{2}r{3}{4}", (char)27, strLines[2], (char)27, strLines[3], (char)4));
                                    break;
                                case 2:
                                    //��1��
                                    //mscJet.Output = Chr(27) & "S001" & Chr(27) & "u2" & strLine(0) & Chr(27) & "u1" & Chr(4)
                                    //��2��
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
                            m_JetParamsCollect.SendMessage(OnSysMsg, "������Ϣ����ʧ�ܣ�ԭ��: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    case "N":
                        /*
                        string[] strAddrInform = strPortValue.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strAddrInform.Length != 2)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, string.Format("���������˿ڡ�{0}����ʽ����,��ȷ��ʽ��:127.0.0.1:23", strPortValue), true);
                            blnRetValue = false;
                        }
                        */
                        sndMsg = "���������֧������˿ڻ�δʵ�����緢�ͳ���!";
                        m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                        blnRetValue = false;
                        break;
                    default:    //Ĭ�����
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
                                    //��1-2��
                                    //mscJet.Output = Chr(27) & "S001" & strLine(0) & Chr(27) & "r" & strLine(1) & Chr(4)
                                    //��3-4��
                                    //mscJet.Output = Chr(27) & "S002" & strLine(2) & Chr(27) & "r" & strLine(3) & Chr(4)
                                    m_spJetPrinter.Write(string.Format("{0}S001{1}{2}r{3}{4}", (char)27, strLines[0], (char)27, strLines[1], (char)4));
                                    m_spJetPrinter.Write(string.Format("{0}S002{1}{2}r{3}{4}", (char)27, strLines[2], (char)27, strLines[3], (char)4));
                                    break;
                                case 2:
                                    //��1��
                                    //mscJet.Output = Chr(27) & "S001" & Chr(27) & "u2" & strLine(0) & Chr(27) & "u1" & Chr(4)
                                    //��2��
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
                            m_JetParamsCollect.SendMessage(OnSysMsg, "������Ϣ����ʧ�ܣ�ԭ��: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                }
                m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //��ʾ������Ϣ
                #endregion
            }
            catch (System.Exception ex)
            {
                sndMsg = "��ӡֽ�����ԭ��:" + ex.Message;
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
                        string strDispReceive = ">>>�����������Ϣ:" + m_sbJetResponesMsg.ToString();
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

    #region Imaje4020�������
    internal class CImaje4020Operation : CJetOperation
    {
        #region ��������
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //ģ�����
        private DataTable m_dtJetFormat = null;             //�����ʽ
        private DataTable m_dtRollData = null;              //��Ҫ��ӡ��ֽ������
        private DataTable m_dtJetRollData = null;           //���������ʽ������ӡ�ֶ��滻����Ӧֽ�����ݺ��γɵĿ���ֱ����ӡ��ֽ�����ݱ�
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        #endregion

        #region ���캯��
        public CImaje4020Operation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region �Զ��庯��
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
                    m_JetParamsCollect.SendMessage(OnSysMsg, string.Format("���������˿ڡ�{0}����ʽ����,��ȷ��ʽ��:127.0.0.1:23", strPortValue), true);
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
                    sndMsg = string.Format("IP��ַ��{0}���������������ӡ��IP�����Ƿ���ȷ!", strAddrInform[0]);
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                string sndMsg = "Imaje��ӡ��δ���ӳɹ�,ԭ��:" + ex.Message;
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

                #region ��ʼ��������Ϣ�������
                if (dtMobanParams == null || dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���������(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtFormatParams == null || dtFormatParams.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���Ӧ����ӡ��ʽ��������(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "��ȡ��ӡֽ�����ݳ���(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "��ʽ����ӡֽ�����ӡ���ݳ���(����:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

                
                //�˿�����
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:���ڣ�C������
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                //��ȡ��ӡ����
                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��������ģ���ȡ��ӡ�������ô���!", true);
                    return false;
                }
                if (!m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��ȡ��ӡ������Ϣ����!", true);
                    return false;
                }

                #region �ֶ˿ڿ�ʼ����������Ϣ
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
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //��ʾ������Ϣ
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
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //��ʾ������Ϣ
                            blnRetValue = true;
                            //Chaint.CH.Global.CGlobal.DelayTime(200);
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "������Ϣ����ʧ�ܣ�ԭ��: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    default:    //Ĭ������˿�
                        for (int i = 0; i < strLines.Length; i++)
                        {
                            //str += "!!" + "v!5" + (char)34 + row[PaperRollData.ROLL_ID_FIELD].ToString() + " " + (char)34 + (char)13;
                            sbJetContent.AppendFormat("!!v!{0}{1}{2}{3}{4}", (i + 1), (char)34, strLines[i], (char)34, (char)13);
                        }
                        if (SendPrintContentBySck(sbJetContent.ToString()))
                        {
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //��ʾ������Ϣ
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
                sndMsg = "��ӡֽ�����ԭ��:" + ex.Message;
                m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                blnRetValue = false;
            }
            return blnRetValue;
        }

        #endregion
    }
    #endregion

    #region Imaje5200�������
    internal class CImaje520Operation : CJetOperation
    {
        #region ��������
        public override event MessageEventHandler OnSysMsg;
        private JetParamsCollect m_JetParamsCollect = null;
        private CFormatPrintParams m_PrintFunc = new CFormatPrintParams();

        private DataTable m_dtMobanParams = null;           //ģ�����
        private DataTable m_dtJetFormat = null;             //�����ʽ
        private DataTable m_dtRollData = null;              //��Ҫ��ӡ��ֽ������
        private DataTable m_dtJetRollData = null;           //���������ʽ������ӡ�ֶ��滻����Ӧֽ�����ݺ��γɵĿ���ֱ����ӡ��ֽ�����ݱ�
        private System.IO.Ports.SerialPort m_spJetPrinter = null;
        #endregion

        #region ���캯��
        public CImaje520Operation(JetParamsCollect jetParamsCollect)
        {
            m_JetParamsCollect = jetParamsCollect;
        }
        #endregion

        #region �Զ��庯��
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

                #region ��ʼ��������Ϣ�������
                if (dtMobanParams == null || dtMobanParams.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���������(Jet_SelectJetMobanParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtFormatParams == null || dtFormatParams.Rows.Count < 1)
                {
                    sndMsg = "��ȡ����ģ���Ӧ����ӡ��ʽ��������(Jet_SelectJetFormatParams)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (dtPrintData == null || dtPrintData.Rows.Count < 1)
                {
                    sndMsg = "��ȡ��ӡֽ�����ݳ���(Jet_SelectRollPrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }
                if (!m_PrintFunc.CombineJetPrintData(m_dtRollData, m_dtJetFormat, ref m_dtJetRollData))
                {
                    sndMsg = "��ʽ����ӡֽ�����ӡ���ݳ���(����:CombinePrintData)!";
                    m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                    return false;
                }

                #endregion

              
                //�˿�����
                strPortType = string.Format("{0}", m_dtMobanParams.Rows[0]["PortType"]).Trim().ToUpper();    //N:���ڣ�C������
                strPortValue = string.Format("{0}", m_dtMobanParams.Rows[0]["PortValue"]).Trim();

                //��ȡ��ӡ����
                if (!int.TryParse(string.Format("{0}", m_dtMobanParams.Rows[0]["LineCount"]), out intLineCount))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��������ģ���ȡ��ӡ�������ô���!", true);
                    return false;
                }
                if (!m_JetParamsCollect.GetJetInformLines(m_dtJetRollData, intLineCount, ref strLines))
                {
                    m_JetParamsCollect.SendMessage(OnSysMsg, "��ȡ��ӡ������Ϣ����!", true);
                    return false;
                }

                #region �ֶ˿ڿ�ʼ����������Ϣ
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
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //��ʾ������Ϣ
                            blnRetValue = true;
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                            blnRetValue = true;
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "������Ϣ����ʧ�ܣ�ԭ��: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                    case "N":
                        sndMsg = "���������֧������˿ڻ�δʵ�����緢�ͳ���!";
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
                            m_JetParamsCollect.DisJetInform(OnSysMsg, strLines);    //��ʾ������Ϣ
                            blnRetValue = true;
                            System.Threading.Thread.Sleep(200);
                            m_spJetPrinter.Close();
                            blnRetValue = true;
                        }
                        catch (System.Exception ex)
                        {
                            m_JetParamsCollect.SendMessage(OnSysMsg, "������Ϣ����ʧ�ܣ�ԭ��: " + ex.Message, true);
                            blnRetValue = false;
                        }
                        break;
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                sndMsg = "��ӡֽ�����ԭ��:" + ex.Message;
                m_JetParamsCollect.SendMessage(OnSysMsg, sndMsg, true);
                blnRetValue = false;
            }
            return blnRetValue;
        }
        #endregion
    }
    #endregion
}
