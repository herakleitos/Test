using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;

using CTSocket;
using CTWH.Common;
using DataModel;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace WH_PDA_Service
{
    public partial class PDAForm : Form
    {
        private string ServiceDesc = "WH_PDA";

        CTSocket.SocketServer server_WH_PDA;
        CTWH.Common.SocketService.ServerSocketService socketservice = new CTWH.Common.SocketService.ServerSocketService();

        BackgroundWorker bgReadData = new BackgroundWorker();
        private BlockingCollection<Tuple<MessageEventArgs, string>> blockingCollection =
            new BlockingCollection<Tuple<MessageEventArgs, string>>();
        MainDS MDS = new MainDS();
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        bool IsServiceClosing = false;
        bool IsWorking = false;//标记一把枪是否在运行
        public PDAForm()
        {
            InitializeComponent();
            this.ControlBox = false;
            InitData(false);
            this.Disposed += new EventHandler(PDAForm_Disposed);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams para = base.CreateParams;
                para.ClassStyle = para.ClassStyle | CP_NOCLOSE_BUTTON;
                return para;
            }
        }
        public PDAForm(bool isSocket)
        {
            InitializeComponent();
            InitData(isSocket);
            if (isSocket)//false 则不启用后台
            {
                if (Utils.Stereo)
                {
                    InitReadDataFromZZ();
                }
            }
            this.Disposed += new EventHandler(PDAForm_Disposed);
        }
        void PDAForm_Disposed(object sender, EventArgs e)
        {
            if (bgReadData != null)
            {
                bgReadData.CancelAsync();
                bgReadData.DoWork -= new DoWorkEventHandler(bgReadData_DoWork);
                bgReadData = null;
            }

            DisposeData();

        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="isSocket">是否打开socket server</param>
        private void InitData(bool isSocket)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            //通信初始化            
            Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Service start");

            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);

            if (isSocket)
            {
                //接受到满足要求的包 触发
                socketservice.DataReceived += new CTWH.Common.SocketService.ServerSocketService.DataReceivedDelegate(WMS_Socketservice_DataReceived);
                socketservice.ClientConnected += new CTWH.Common.SocketService.ServerSocketService.ClientConnectedDelegate(socketservice_ClientConnected);
                socketservice.ClientDisconnected += new CTWH.Common.SocketService.ServerSocketService.ClientDisconnectedDelegate(socketservice_ClientDisconnected);

                //bgSocketTempMES = new System.Threading.Thread(new System.Threading.ThreadStart(bgSocketTempMES_DoWork));
                //bgSocketTempMES.Start();

                //bgRecordClearMES = new System.Threading.Thread(new System.Threading.ThreadStart(bgRecordClearMES_DoWork));
                //bgRecordClearMES.Start();

                server_WH_PDA = new SocketServer(CallbackThreadType.ctWorkerThread,
                  socketservice,
                  //DelimiterType.dtMessageTailExcludeOnReceive,
                  DelimiterType.dtNone,
                  Utils.SocketParaPDA.SocketEncoding.GetBytes(new char[] { (char)3 }),
                   //1024, 2048, 10000, 360000
                   1024 * 8, 1024 * 16, 10000, 360000

                  );

                if (!server_WH_PDA.Active)
                {
                    //启动监听               
                    server_WH_PDA.AddListener(ServiceDesc + " Listener", new IPEndPoint(IPAddress.Any, Utils.SocketParaPDA.SocketPort));
                    server_WH_PDA.Start();
                    Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Listener start");
                }
                Task.Factory.StartNew(() =>
                {
                    bool IsSendBuffer = false;
                    foreach (Tuple<MessageEventArgs, string> value in blockingCollection.GetConsumingEnumerable())
                    {
                        try
                        {
                            string replayMessage = ProcessCommand(value.Item2);
                            if (!IsSendBuffer)
                            {
                                Byte[] bt = Utils.SocketParaPDA.SocketEncoding.GetBytes(replayMessage);
                                value.Item1.Connection.BeginSend(bt);
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataReceived Error:" + ex.ToString());
                        }
                    }
                });
            }
        }
        private string ProcessCommand(string datagramText)
        {
            string ReplyMessage = "Do nothing";
            UInt16 CharCount = 0;
            string FunctionCode = "";
            if (datagramText.StartsWith(Utils.WMSMessage._StartChar + "") && datagramText.EndsWith(Utils.WMSMessage._EndChar + ""))
            {
                //不包含了末尾结束符号，strs0 = 长度 strs1 = function
                string[] strs = datagramText.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                if (strs.Length > 1)
                {
                    CharCount = (UInt16)Convert.ToInt16(strs[0]);
                    FunctionCode = strs[1];
                    //验证数据长度  6= length.padeleft （4） +前缀+后缀 
                    if (CharCount != datagramText.Length)
                    {
                        //验算失败
                        ReplyMessage = "Message length wrong";
                    }
                    else
                    {
                        switch (FunctionCode)
                        {
                            case "IR01":
                                ReplyMessage = "M002";

                                break;
                            case "IR02"://卷筒扫描入库
                                if (strs.Length > 2)
                                {

                                    ReplyMessage = this.Process_IR02(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA02", "9", "Message not valid" });

                                }
                                break;

                            case "IR03":
                            case "IP03":
                                if (strs.Length > 2)
                                {

                                    ReplyMessage = this.Process_IR03(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA03", "9", "Message not valid" });

                                }
                                break;
                                if (strs.Length > 2)
                                {

                                    ReplyMessage = this.Process_IR03(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA03", "9", "Message not valid" });

                                }
                                break;

                            case "IP04":
                            case "IR04":
                                if (strs.Length > 2)
                                {

                                    ReplyMessage = this.Process_IR04(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", "Message not valid" });

                                }
                                //ReplyMessage = "M004"; 
                                break;
                            case "IP05":
                            case "IR05":
                                if (strs.Length > 2)
                                {

                                    ReplyMessage = this.Process_IR05(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA05", "9", "Message not valid" });
                                }
                                //ReplyMessage = "M005";
                                break;
                            case "IR06":
                                ReplyMessage = "M006"; break;
                            case "IP02"://平板扫描入库
                                if (strs.Length > 2)
                                {

                                    ReplyMessage = this.Process_IP02(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA02", "9", "Message not valid" });

                                }
                                break;

                            case "Q01":
                                ReplyMessage = "QA01"; break;
                            case "Q02":
                                ReplyMessage = "QA02"; break;
                            case "Q03":
                                ReplyMessage = "QA03"; break;
                            case "Q04":
                                ReplyMessage = "QA04"; break;
                            case "Q05":
                                //请求货位信息
                                if (strs.Length > 2)
                                {
                                    string fac = strs[2].Trim();
                                    string parent = strs[3].Trim();
                                    ReplyMessage = this.Process_Q05(fac, parent, 2);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA05", "9", "Message not valid" });

                                }
                                break;
                            case "Q06":
                                //请求业务信息
                                if (strs.Length > 2)
                                {
                                    string fac = strs[2].Trim();

                                    ReplyMessage = this.Process_Q06(fac);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA06", "9", "Message not valid" });

                                }
                                break;
                            case "Q07":
                                //请求业务信息
                                if (strs.Length > 2)
                                {
                                    string fac = strs[2].Trim();

                                    ReplyMessage = this.Process_Q07(fac);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA07", "9", "Message not valid" });

                                }
                                break;

                            case "Q08":
                                //请求机台信息
                                if (strs.Length > 2)
                                {
                                    string date = strs[2].Trim();

                                    ReplyMessage = this.Process_Q08(date);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA08", "9", "Message not valid" });

                                }
                                break;
                            case "Q09":
                                ReplyMessage = "QA09";
                                break;
                            case "Q10":
                                //请求业务机台班组信息
                                if (strs.Length > 2)
                                {
                                    string date = strs[2].Trim();

                                    ReplyMessage = this.Process_Q10(date);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA10", "9", "Message not valid" });
                                }
                                break;
                            case "Q11":
                                //请求保管和仓库信息
                                if (strs.Length > 2)
                                {
                                    string fac = strs[2].Trim();

                                    ReplyMessage = this.Process_Q11(fac);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA11", "9", "Message not valid" });

                                }
                                break;
                            case "Q12":
                                //请求本地机台信息
                                if (strs.Length > 2)
                                {
                                    string fac = strs[2].Trim();

                                    ReplyMessage = this.Process_Q12(fac);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA12", "9", "Message not valid" });

                                }
                                break;
                            case "Q13":
                                //请求本地保管信息
                                if (strs.Length > 2)
                                {
                                    string fac = strs[2].Trim();

                                    ReplyMessage = this.Process_Q13(fac);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA13", "9", "Message not valid" });

                                }
                                break;
                            case "Q14":
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_Q14(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA14", "9", "Message not valid" });

                                }
                                break;
                            case "Q15":
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_Q15(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA15", "9", "Message not valid" });

                                }
                                break;
                            //入库确认
                            case "Q20":
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_Q20(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "QA20", "9", "Message not valid" });

                                }
                                break;
                            case "O01":
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_O01(strs, false);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "Message not valid" });

                                }
                                break;

                            case "O02": //重量件数都超
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_O02(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA02", "9", "Message not valid" });

                                }
                                break;
                            case "O03"://超重量
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_O03(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA03", "9", "Message not valid" });

                                }
                                break;
                            case "O04"://超件数
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_O04(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA04", "9", "Message not valid" });

                                }
                                break;
                            case "O15"://刷新出库单状态和分录
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_O15(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA15", "9", "Message not valid" });

                                }
                                break;
                            case "O08"://刷新出库单状态和分录
                                if (strs.Length > 2)
                                {
                                    ReplyMessage = this.Process_O08(strs);

                                }
                                else
                                {
                                    ReplyMessage = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "9", "Message not valid" });

                                }
                                break;
                            default:
                                ReplyMessage = "Message ID wrong,ignore";
                                break;
                        }
                    }
                }
                else
                {
                    ReplyMessage = "Wrong message";
                }
            }
            else
            {
                ReplyMessage = "Wrong message";
            }
            return ReplyMessage;
        }
        private void InitReadDataFromZZ()
        {
            bgReadData.DoWork += new DoWorkEventHandler(bgReadData_DoWork);
            bgReadData.RunWorkerAsync();
        }

        private void bgReadData_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    //如果外部取消,将退出循环
                    if (bgReadData == null || bgReadData.CancellationPending)
                    {
                        return;
                    }
                    ReadDataFromZZ();
                }
                catch (System.Exception ex)
                {
                    this.lst_PDAMsg.Items.Add("<bgReadData_DoWork>出错,原因:" + ex.Message);
                }
                System.Threading.Thread.Sleep(1000);
            }

        }

        string strRollID = "";
        string strDateS = "";
        string strShift = "";
        string retMsg = "";

        private void ReadDataFromZZ()
        {
            DataSet ds = this._WMSAccess.ReadWHDataFromZZ();

            if (ds.Tables["Roll_Product"].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables["Roll_Product"].Rows)
                {
                    strRollID = dr["ProductID"].ToString();
                    strDateS = ds.Tables["Roll_Product"].Rows[0]["EnterTime"].ToString();
                    strShift = "戊";// ds.Tables["Roll_Product"].Rows[0]["Shift"].ToString();
                    string[] strRollIDfromZZ = new string[] { "0099", "IR03", strRollID, strDateS, "SCRK", "", "31", "", "999", "", strShift, "", "L", "", "" };

                    retMsg = Process_IR02(strRollIDfromZZ);

                    if (retMsg.Split((char)4)[2] == "0")//入库成功
                    {
                        //更新ZZ的表
                        this._WMSAccess.Insert_IsUpdatedToZZ(strRollID);
                    }

                    else if (retMsg.Split((char)4)[2] == "9" && retMsg.Contains("已入库"))
                    {

                        this._WMSAccess.Insert_IsUpdatedToZZ(strRollID);//插入Upload记录
                        this._WMSAccess.Update_IsUpdatedToZZ(strRollID);//重新更新Upload标识
                    }

                    else if (retMsg.Split((char)4)[2] == "9")//入库失败,则删除ERP_ProductUpload表中行
                    {
                        //this._WMSAccess.Delete_IsUpdatedToZZ(strRollID);
                    }

                    else //入库失败
                    {

                    }
                }
            }
        }

        public void DisposeData()
        {
            IsServiceClosing = true;

            Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Service stop");
            _WMSAccess.SqlStateChange -= new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMSAccess = null;

            if (server_WH_PDA != null)
            {
                socketservice.DataReceived -= new CTWH.Common.SocketService.ServerSocketService.DataReceivedDelegate(WMS_Socketservice_DataReceived);
                socketservice.ClientConnected -= new CTWH.Common.SocketService.ServerSocketService.ClientConnectedDelegate(socketservice_ClientConnected);
                socketservice.ClientDisconnected -= new CTWH.Common.SocketService.ServerSocketService.ClientDisconnectedDelegate(socketservice_ClientDisconnected);

                server_WH_PDA.Stop();
                server_WH_PDA.Dispose();
            }
        }

        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }

        private void socketservice_ClientDisconnected(ConnectionEventArgs e)
        {
            Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Client disconnect, ConnectionId:" + e.Connection.ConnectionId.ToString() + ",IP:" + e.Connection.RemoteEndPoint.Address.ToString());
        }

        private void socketservice_ClientConnected(ConnectionEventArgs e)
        {
            Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Client Connected, ConnectionId:" + e.Connection.ConnectionId.ToString() + ",IP:" + e.Connection.RemoteEndPoint.Address.ToString());
        }
        private void WMS_Socketservice_DataReceived(MessageEventArgs e)
        {
            string datagramText = Utils.SocketParaPDA.SocketEncoding.GetString(e.Buffer);
            blockingCollection.Add(new Tuple<MessageEventArgs, string>(e, datagramText));
        }

        public string Process_O15(string[] strs)
        {
            string voucherno = strs[2];
            string status = "";
            string entry = "";
            decimal currAmountSum = 0;
            decimal currWeightSum = 0;

            DataSet wms = this._WMSAccess.Select_T_OutStockAndEntry_RelationByVoucherNO(voucherno);
            if (wms.Tables.Count > 0 && wms.Tables["T_OutStock"].Rows.Count > 0)
            {
                status = wms.Tables["T_OutStock"].Rows[0]["IsClose"].ToString();

            }
            if (wms.Tables.Count > 0 && wms.Tables["T_OutStock_Entry"].Rows.Count > 0)
            {
                for (int i = 0; i < wms.Tables["T_OutStock_Entry"].Rows.Count; i++)
                {
                    string spec = wms.Tables["T_OutStock_Entry"].Rows[i]["Specification"].ToString();
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["EntryID"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["MaterialName"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["Specification"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["Grade"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["WeightMode"].ToString() + Utils.WMSMessage._ColumnChar;
                    //entry += spec.Contains("-")?wms.Tables["T_OutStock_Entry"].Rows[i]["Reams"].ToString(): wms.Tables["T_OutStock_Entry"].Rows[i]["CoreDiameter"].ToString();
                    //entry += Utils.WMSMessage._ColumnChar;
                    //entry += spec.Contains("-")?wms.Tables["T_OutStock_Entry"].Rows[i]["SlidesOfReam"].ToString(): wms.Tables["T_OutStock_Entry"].Rows[i]["Diameter"].ToString();
                    //entry += Utils.WMSMessage._ColumnChar;
                    //entry += spec.Contains("-") ? wms.Tables["T_OutStock_Entry"].Rows[i]["SlidesOfSheet"].ToString() : wms.Tables["T_OutStock_Entry"].Rows[i]["RollLength"].ToString();
                    //entry += Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["CoreDiameter"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += spec.Contains("-") ? wms.Tables["T_OutStock_Entry"].Rows[i]["SlidesOfReam"].ToString() : wms.Tables["T_OutStock_Entry"].Rows[i]["Diameter"].ToString();
                    entry += Utils.WMSMessage._ColumnChar;
                    entry += spec.Contains("-") ? wms.Tables["T_OutStock_Entry"].Rows[i]["SlidesOfSheet"].ToString() : wms.Tables["T_OutStock_Entry"].Rows[i]["RollLength"].ToString();
                    entry += Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["ReamPackType"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["SKU"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["PaperCert"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["SpecProdName"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["SpecCustName"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["TrademarkStyle"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["IsWhiteFlag"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["OrderNO"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["Remark"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["PlanQty"].ToString() + Utils.WMSMessage._ColumnChar;
                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["PlanAuxQty1"].ToString() + Utils.WMSMessage._ColumnChar;

                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["planCommitQty"].ToString() + Utils.WMSMessage._ColumnChar;

                    if (wms.Tables["T_OutStock_Entry"].Rows[i]["planCommitQty"].ToString() != "")
                    {
                        currAmountSum += Convert.ToDecimal(wms.Tables["T_OutStock_Entry"].Rows[i]["planCommitQty"].ToString());
                    }

                    entry += wms.Tables["T_OutStock_Entry"].Rows[i]["planCommitAuxQty1"].ToString();

                    if (wms.Tables["T_OutStock_Entry"].Rows[i]["planCommitAuxQty1"].ToString() != "")
                    {
                        currWeightSum += Convert.ToDecimal(wms.Tables["T_OutStock_Entry"].Rows[i]["planCommitAuxQty1"].ToString());
                    }
                    entry += Utils.WMSMessage._ForeachChar;
                }

                //添加最后一行，显示当前已发货的总件数和总重量
                entry += "统计";
                for (int i = 0; i < 19; i++)
                {

                    entry += Utils.WMSMessage._ColumnChar;
                }

                entry += currAmountSum.ToString() + Utils.WMSMessage._ColumnChar;
                entry += currWeightSum.ToString();

                if (entry.Length > 0)
                    entry = entry.TrimEnd(Utils.WMSMessage._ForeachChar);
            }
            string msg = "刷新成功", code = "0";

            if (status != "")
            {
                msg = "出库单不存在";
                code = "9";
            }
            if (entry == "")
            {
                msg = "出库单分录不存在";
                code = "9";
            }
            string[] msgs = new string[] { "OA15", code, msg, voucherno, status, entry };

            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }

        public string Process_Q07(string dts)
        {
            string[] msgs = null;

            DateTime date;
            bool a = DateTime.TryParse(dts, out date);
            if (!a)
            {
                msgs = new string[] { "QA07", "9", "出库时间错误，请检查。" };

                return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
            }

            dts = Convert.ToDateTime(dts).ToString("yyyy-MM-dd") + " 00:00:00";
            string dte = Convert.ToDateTime(dts).ToString("yyyy-MM-dd") + " 23:59:59";

            WMSDS wmsDS = this._WMSAccess.Select_T_OutStockByFK("", dts, dte, "", "", "", "", 0, 0, 0);
            string forMsg = "";
            if (wmsDS.T_OutStock.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_OutStock.Rows.Count; i++)
                {
                    string org = wmsDS.T_OutStock.Rows[i][wmsDS.T_OutStock.VoucherNOColumn].ToString().Trim();
                    forMsg += org + Utils.WMSMessage._ForeachChar;
                }
                forMsg = forMsg.TrimEnd(Utils.WMSMessage._ForeachChar);
                msgs = new string[] { "QA07", "0", "出库单记录刷新成功。", forMsg };

            }
            else
            {
                msgs = new string[] { "QA07", "9", "没有找到出库单记录。" };

            }
            //   QA08|0|011.PM11/012.pm12/013.pm13
            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }
        /// <summary>
        /// 终端获取系统时间
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_Q15(string[] strs)
        {
            string[] paperStrs = new string[] { "QA15", "0", "服务器时间刷新成功", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            string msg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            return msg;
        }
        /// <summary>
        /// 查询入库统计
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_Q14(string[] strs)
        {
            //string factory = strs[2];
            string factory = "";
            string user = strs[3].Split('.')[0];
            string shift = strs[4];
            string shifttime = strs[5];

            string dateS = strs[6];
            string dateE = strs[7];
            string pType = "";
            string msg = "";
            WMSQueryDS wms = this._WMSAccess.Select_T_Product_InForStat(factory, user, dateS, dateE, pType);
            string row = "";
            if (wms.T_Product_In_Stat.Rows.Count > 0)
            {
                for (int i = 0; i < wms.T_Product_In_Stat.Rows.Count; i++)
                {
                    row += wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.MaterialNameColumn].ToString() + Utils.WMSMessage._ColumnChar +//物料名称
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.SpecificationColumn].ToString() + Utils.WMSMessage._ColumnChar +//规格
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.GradeColumn].ToString() + Utils.WMSMessage._ColumnChar +//等级
                           wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.PaperCertCodeColumn].ToString() + Utils.WMSMessage._ColumnChar +//纸种认证

                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.ReamPackTypeColumn].ToString() + Utils.WMSMessage._ColumnChar +//包装方式
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.IsPolyHookColumn].ToString() + Utils.WMSMessage._ColumnChar +//夹板方式

                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.CoreReamColumn].ToString() + Utils.WMSMessage._ColumnChar +//纸芯令数
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.WeightModeColumn].ToString() + Utils.WMSMessage._ColumnChar +//重量模式
                              wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.SpecCustNameColumn].ToString() + Utils.WMSMessage._ColumnChar +//客户专用
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.SpecProdNameColumn].ToString() + Utils.WMSMessage._ColumnChar +//产品专用
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.DiameterSheetsColumn].ToString() + Utils.WMSMessage._ColumnChar +//直径令张

                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.LengthSheetColumn].ToString() + Utils.WMSMessage._ColumnChar +//线长张数
                         wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.IsWhiteFlagColumn].ToString() + Utils.WMSMessage._ColumnChar +//商标类型
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.OrderNOColumn].ToString() + Utils.WMSMessage._ColumnChar +//订单号

                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.WHRemarkColumn].ToString() + Utils.WMSMessage._ColumnChar +//仓库备注

                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.BatchNOColumn].ToString() + Utils.WMSMessage._ColumnChar +//批次号
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.AllCountColumn].ToString() + Utils.WMSMessage._ColumnChar +//件数
                        wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat.AllWeightColumn].ToString() +//重量

                        //wms.T_Product_In_Stat.Rows[i][wms.T_Product_In_Stat].ToString() +
                        Utils.WMSMessage._ForeachChar;
                }
                row = row.TrimEnd(Utils.WMSMessage._ForeachChar);
                string[] paperStrs = new string[] { "QA14", "0", "入库统计刷新成功", row };
                msg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);

            }
            else
            {
                string[] paperStrs = new string[] { "QA14", "9", "入库统计刷新失败" };
                msg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);

            }

            return msg;
        }

        public string Process_Q06(string fac)
        {
            string ss = this.LoadWHBusinessType("in");
            return ss;
        }

        public string Process_Q13(string fac)
        {
            string ss = this.LoadWHUser();
            return ss;
        }

        public string Process_Q12(string fac)
        {
            string ss = this.LoadWHMachine();
            return ss;
        }
        public string CheckCanCancelScanIn(string productid)
        {
            string retMsg = "";
            //先判断这个条码是否已经入库。
            DataSet wmsDS = _WMSAccess.Select_T_ProductLifeByProductID(productid);

            //如果已经入库那么就可以删除
            if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
            {

                string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                if (status == Utils.WMSOperate._StatusIn)
                {
                    /////可以取消，还需要判断是否已做入库单,这个ID的最后一次入库记录
                    // StockInBillQueryByProductID();
                    //还要看这个纸是否已做入库单，如果已做入库单还要提示出来，
                    string onlyid = wmsDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();
                    WMSDS piDS = this._WMSAccess.Select_T_Product_InByProductID(onlyid);
                    if (piDS.T_Product_In.Rows.Count > 0)
                    {
                        string voucherinid = piDS.T_Product_In.Rows[0][piDS.T_Product_In.VoucherInIDColumn].ToString();
                        if (voucherinid != "")
                        {
                            //已做入库单
                            piDS = this._WMSAccess.Select_T_InStockByVoucherNO(voucherinid);
                            if (piDS.T_InStock.Rows.Count > 0)
                            {
                                string voucherno = piDS.T_InStock.Rows[0][piDS.T_InStock.VoucherNOColumn].ToString();
                                retMsg = "产品在入库单" + voucherno + "中，不能取消入库！";
                                string[] paperStrs = new string[]{
                            "IRA03","9",retMsg
                             };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }
                            else
                            {  //不可以取消
                                retMsg = "产品对应的入库单" + voucherinid + "不存在，不能取消入库！";
                                string[] paperStrs = new string[]{
                            "IRA03","9",retMsg
                             };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }

                            //////  //判断入库单是否已上传，已审核
                            //////  wmsDS = this._WMSAccess.Select_T_InStockByVoucherNO(voucherinid);
                            //////  if (wmsDS.T_InStock.Rows.Count > 0)
                            //////  {
                            //////      string check = wmsDS.T_InStock.Rows[0][wmsDS.T_InStock.IsCheckColumn].ToString();
                            //////      string upload = wmsDS.T_InStock.Rows[0][wmsDS.T_InStock.IsUploadColumn].ToString();
                            //////      string voucherno = wmsDS.T_InStock.Rows[0][wmsDS.T_InStock.VoucherNOColumn].ToString();
                            //////      if (upload == "1")
                            //////      { //不可以取消
                            //////          retMsg = "产品对应的入库单" + voucherno + "已上传，不能取消入库！";
                            //////          string[] paperStrs = new string[]{
                            //////"IRA03","9",retMsg
                            ////// };
                            //////          retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            //////      }
                            //////      else if (check == "1")
                            //////      { //不可以取消
                            //////          retMsg = "产品对应的入库单" + voucherno + "已审核，不能取消入库！";
                            //////          string[] paperStrs = new string[]{
                            //////"IRA03","9",retMsg
                            ////// };
                            //////          retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            //////      }
                            //////      else
                            //////      {
                            //////          retMsg = "";
                            //////      }
                            //////  }
                            //////  else
                            //////  {  //不可以取消
                            //////      retMsg = "产品对应的入库单" + voucherinid + "不存在，不能取消入库！";
                            //////      string[] paperStrs = new string[]{
                            //////"IRA03","9",retMsg
                            ////// };
                            //////      retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            //////  }
                        }
                        else
                        {
                            retMsg = "";

                        }

                    }
                    else
                    {
                        //不可以取消
                        retMsg = "产品" + productid + "不在库存中，不能取消入库！";
                        string[] paperStrs = new string[]{
                          "IRA03","9",retMsg
                           };
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                    }
                }
                //else if (status == Utils.WMSOperate._StatusHalfOut)
                //{
                //    //不可以取消
                //    retMsg = "产品已于" + time + "扫描出库，不能取消入库！";
                //    string[] paperStrs = new string[]{
                //          "IRA03","9",retMsg
                //           };
                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //}
                else if (status == Utils.WMSOperate._StatusOut)
                {
                    //不可以取消
                    retMsg = "产品已于" + time + "出库，不能取消入库！";
                    string[] paperStrs = new string[]{
                          "IRA03","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                else if (status == Utils.WMSOperate._StatusCancelIn)
                {
                    //不可以取消
                    retMsg = "产品已于" + time + "取消，不能再次取消入库！";
                    string[] paperStrs = new string[]{
                          "IRA03","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                //else if (status == Utils.WMSOperate._StatusHalfReturn)
                //{
                //    //不可以取消
                //    retMsg = "产品已于" + time + "扫描退货，不能取消入库！";
                //    string[] paperStrs = new string[]{
                //          "IRA03","9",retMsg
                //           };
                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //}
                else
                {
                    //不可以取消
                    retMsg = "产品状态未知" + status + "，不能取消入库！";
                    string[] paperStrs = new string[]{
                          "IRA03","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
            }
            else
            {
                //如果没有入库那么就不能删除
                retMsg = "产品没有入库，不能取消入库！";
                string[] paperStrs = new string[]{
                          "IRA03","9",retMsg
                           };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }



            return retMsg;
        }
        /// <summary>
        /// 取消入库
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_IR03(string[] strs)
        {
            try
            {
                string retMsg = "";
                string productid = strs[2];
                string user = strs[3];
                string command = "IRA03";
                //先判断一个纸能否取消入库
                //string canCancel = this.StatusCheckIR03(productid,"IRA03");
                DataSet wmsDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
                if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
                {
                    string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                    string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                    string prodOnlyID = wmsDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();//life执行的那个id
                    string sourceID = wmsDS.Tables["T_ProductLife"].Rows[0]["SourcePID"].ToString();//执行ID对应的源ID
                    switch (status)
                    {
                        case Utils.WMSOperate._StatusCancelRedIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品的入库单已上传，不能取消入库！" });
                            break;
                        case Utils.WMSOperate._StatusIn:
                            //是否已做入库单
                            WMSDS inds = this._WMSAccess.Select_T_Product_InAndT_InStock(prodOnlyID);
                            if (inds.T_InStock.Rows.Count == 0)
                            {
                                //还没有做入库单,可以取消
                                //把它更新为不入库的状态
                                retMsg = this.SetStatusCancelInCaseIn(prodOnlyID, productid, user);
                                if (retMsg == "")
                                {
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "0", "产品取消入库成功" });

                                }
                                else
                                {
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", retMsg });

                                }
                            }
                            else
                            {
                                //做了入库单就看这个入库单有没有上传
                                string close = inds.T_InStock.Rows[0][inds.T_InStock.IsCloseColumn].ToString();
                                string vno = inds.T_InStock.Rows[0][inds.T_InStock.VoucherNOColumn].ToString();
                                if (close == "1")
                                {
                                    //已关单，已上传
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在入库单" + vno + "中，且已上传，不能取消入库！需要做红单入库" });
                                }
                                else
                                {
                                    //未关单，未上传
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已在入库单" + vno + "中，还未上传，不能入库！需要先修改入库单，再取消入库" });
                                }
                            }
                            break;
                        case Utils.WMSOperate._StatusCancelOut:
                            //是否已做入库单
                            //取消出库状态的纸卷，已经在某个入库单了，不允许取消入库操作，需做红单


                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "该纸卷已被执行取消出库操作，不能取消入库！需要做红单入库" });
                            break;

                        //inds = this._WMSAccess.Select_T_Product_InAndT_InStock(prodOnlyID);
                        //if (inds.T_InStock.Rows.Count == 0)
                        //{
                        //    //还没有做入库单,可以取消
                        //    retMsg = this.SetStatusCancelInCaseCancelOut(command, sourceID, productid, user);
                        //    if (retMsg == "")
                        //    {
                        //        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "0", "取消入库成功" });
                        //    }
                        //    else
                        //    {
                        //        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", retMsg });
                        //    }

                        //}
                        //else
                        //{
                        //    //做了入库单就看这个入库单有没有上传
                        //    string close = inds.T_InStock.Rows[0][inds.T_InStock.IsCloseColumn].ToString();
                        //    string vno = inds.T_InStock.Rows[0][inds.T_InStock.VoucherNOColumn].ToString();
                        //    if (close == "1")
                        //    {
                        //        //已关单，已上传
                        //        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在入库单" + vno + "中，且已上传，不能取消入库！需要做红单入库" });
                        //    }
                        //    else
                        //    {
                        //        //未关单，未上传
                        //        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已在入库单" + vno + "中，还未上传，不能入库！需要先修改入库单，再取消入库" });
                        //    }
                        //}


                        //break;

                        case Utils.WMSOperate._StatusRedOut:
                            WMSDS outds = this._WMSAccess.Select_T_Product_InAndT_OutStock(prodOnlyID);
                            if (outds.T_OutStock.Rows.Count == 0)
                            {
                                //还没有做红单出库单,不可以取消
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库退货，但未找到该退货单" });
                            }
                            else
                            {
                                //做了退货单就看这个退货单有没有关闭
                                string close = outds.T_InStock.Rows[0][outds.T_InStock.IsCloseColumn].ToString();
                                string vno = outds.T_InStock.Rows[0][outds.T_InStock.VoucherNOColumn].ToString();
                                if (close == "1")
                                {
                                    //已关单，已上传
                                    //再看suorceid是不是已做入库单
                                    inds = this._WMSAccess.Select_T_Product_InAndT_InStock(sourceID);
                                    if (inds.T_InStock.Rows.Count == 0)
                                    {
                                        //还没有做入库单,可以取消
                                        retMsg = this.SetStatusCancelInCaseRedOut(command, sourceID, productid, user);
                                        if (retMsg == "")
                                        {
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "0", "产品取消入库成功" });

                                        }
                                        else
                                        {
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", retMsg });

                                        }
                                    }
                                    else
                                    {
                                        //已做入库单，再看入库单是否已上传
                                        close = inds.T_InStock.Rows[0][inds.T_InStock.IsCloseColumn].ToString();
                                        vno = inds.T_InStock.Rows[0][inds.T_InStock.VoucherNOColumn].ToString();
                                        if (close == "1")
                                        {
                                            //已关单，已上传
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在入库单" + vno + "中，且已上传，不能取消入库！需要做红单入库" });
                                        }
                                        else
                                        {
                                            //未关单，未上传
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已在入库单" + vno + "中，还未上传，不能入库！需要先修改入库单，再取消入库" });
                                        }
                                    }
                                }
                                else
                                {
                                    //未关单，未上传
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在退货单" + vno + "中，还未关闭，不能取消入库！请先关闭该单" });
                                }
                            }
                            break;
                        case Utils.WMSOperate._StatusCancelIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已取消入库，不能再次取消入库" });
                            break;
                        case Utils.WMSOperate._StatusRedIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已红单入库扫描，不能取消入库！" });
                            break;
                        case Utils.WMSOperate._StatusOut:
                        case Utils.WMSOperate._StatusCancelRedOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，不能取消入库！" });
                            break;

                        default:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未知状态" + status + "不能入库！" });
                            break;
                    }
                }
                else //没有查询到这个条码说明从来没进过系统，不可以取消入库
                {
                    string[] paperStrs = new string[] { command, "9", "产品未入库，不能取消入库" };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                return retMsg;
            }
            catch (Exception ex)
            {
                string[] paperStrs = new string[]{
                          "IRA03","9","取消入库异常："+ex.Message
                           };
                return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
        }

        private string SetStatusCancelInCaseRedOut(string command, string sourcePID, string productid, string user)
        {
            WMSDS.T_ProductLifeRow tpfRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
            tpfRow.ProductOnlyID = Convert.ToInt32(sourcePID);
            tpfRow.ProductID = productid;
            tpfRow.OperUser = user;
            tpfRow.OperDate = DateTime.Now;
            tpfRow.Operate = Utils.WMSOperate._OperScanInCancel;
            tpfRow.Status = Utils.WMSOperate._StatusCancelIn;
            WMSDS.T_Product_InRow tpiRow = new WMSDS().T_Product_In.NewT_Product_InRow();
            tpiRow.OnlyID = tpfRow.ProductOnlyID;
            tpiRow.StatusIn = 0;
            //tpiRow.VoucherInID = 0;
            string result = this._WMSAccess.Tran_Update_ProductScanInForCancel(tpiRow, tpfRow);
            if (result != "")
            //{
            //    string[] paperStrs = new string[] { command, "0", "产品取消入库成功" };
            //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //}
            //else
            {
                result = "产品取消入库失败:" + result;
            }
            return result;
        }
        /// <summary>
        /// 当状态为cancelout的时候，取消入库
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sourcePID"></param>
        /// <param name="productid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private string SetStatusCancelInCaseCancelOut(string command, string sourcePID, string productid, string user)
        {
            WMSDS.T_ProductLifeRow tpfRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
            tpfRow.ProductOnlyID = Convert.ToInt32(sourcePID);
            tpfRow.ProductID = productid;
            tpfRow.OperUser = user;
            tpfRow.OperDate = DateTime.Now;
            tpfRow.Operate = Utils.WMSOperate._OperScanInCancel;
            tpfRow.Status = Utils.WMSOperate._StatusCancelIn;
            WMSDS.T_Product_InRow tpiRow = new WMSDS().T_Product_In.NewT_Product_InRow();
            tpiRow.OnlyID = tpfRow.ProductOnlyID;
            tpiRow.StatusIn = 0;
            //tpiRow.VoucherInID = 0;
            string result = this._WMSAccess.Tran_Update_ProductScanInForCancel(tpiRow, tpfRow);
            if (result != "")
            {
                result = "产品取消入库失败:" + result;

            }
            return result;
        }
        /// <summary>
        /// 当条码为in的状态取消入库
        /// </summary>
        /// <param name="prodOnlyID"></param>
        /// <param name="productid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private string SetStatusCancelInCaseIn(string prodOnlyID, string productid, string user)
        {
            WMSDS.T_ProductLifeRow tpfRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
            tpfRow.ProductOnlyID = Convert.ToInt32(prodOnlyID);
            tpfRow.ProductID = productid;
            tpfRow.OperUser = user;
            tpfRow.OperDate = DateTime.Now;
            tpfRow.Operate = Utils.WMSOperate._OperScanInCancel;
            tpfRow.Status = Utils.WMSOperate._StatusCancelIn;
            WMSDS.T_Product_InRow tpiRow = new WMSDS().T_Product_In.NewT_Product_InRow();
            tpiRow.OnlyID = tpfRow.ProductOnlyID;
            tpiRow.StatusIn = 0;
            //tpiRow.VoucherInID = 0;
            string result = this._WMSAccess.Tran_Update_ProductScanInForCancel(tpiRow, tpfRow);
            if (result != "")
            //{
            //  string []  paperStrs = new string[] { "IRA03", "0", "产品取消入库成功" };
            //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //}
            //else
            {
                result = "产品取消入库失败:" + result;
                //retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            return result;
        }
        /// <summary>
        /// 检查卷筒入库动作cancelin，对应的条码状态，并返回能否cancelin的结果
        /// </summary>
        /// <param name="productid">条形码</param>
        /// <returns>结果=“”可以cancelin，！=“”为错误信息</returns>
        private string StatusCheckIR03(string productid, string command)
        {
            string retMsg = "";
            //先判断这个条码的life状态
            DataSet wmsDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
            if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
            {
                string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                string prodOnlyID = wmsDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();

            }
            else //没有查询到这个条码说明从来没进过系统，不可以取消入库
            {
                string[] paperStrs = new string[] { command, "9", "产品未入库，不能取消入库" };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            return retMsg;
        }
        /// <summary>
        /// 判断一个纸能否入库 ProductLife
        /// </summary>
        /// <param name="productid">卷筒和平板的ID</param>
        /// <returns></returns>
        public string CheckCanScanIn(string productid)
        {
            string retMsg = "";
            //先判断这个条码的life状态
            DataSet wmsDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
            //如果已经入库那么就提示不能入库
            if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
            {
                string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();

                if (status == "null" || status == Utils.WMSOperate._StatusCancelIn)//说明以前入库后取消过，可以再入
                {
                    retMsg = "";

                }
                else if (status == Utils.WMSOperate._StatusIn)//说明在库中不可以再入
                {
                    retMsg = "产品已于" + time + "入库，不能重复入库！";
                    string[] paperStrs = new string[]{
                          "IRA02","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);

                }
                else if (status == Utils.WMSOperate._StatusOut)//说明已出库，不可以入
                {
                    retMsg = "产品已于" + time + "出库，不能重复入库！";
                    string[] paperStrs = new string[]{
                          "IRA02","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                //else if (status == Utils.WMSOperate._StatusHalfOut)//说明半出库，不可以入
                //{
                //    retMsg = "产品已于" + time + "扫描出库，不能重复入库！";
                //    string[] paperStrs = new string[]{
                //          "IRA02","9",retMsg
                //           };
                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //}
                ////else if (status == "return") //说明退货，不可以入(不会有这种状态)
                ////{
                ////    //retMsg = "产品已于" + time + "出库，不能重复入库！";
                ////    //string[] paperStrs = new string[]{
                ////    //  "IRA02","9",retMsg
                ////    //   };
                ////    //retMsg = Utils.MakeWMSSocketMsg(paperStrs);
                ////}
                //else if (status == Utils.WMSOperate._StatusHalfReturn) //说明半退货，不可以入
                //{
                //    retMsg = "产品已于" + time + "扫描退货，不能重复入库！";
                //    string[] paperStrs = new string[]{
                //          "IRA02","9",retMsg
                //           };
                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //}
                else
                {
                    retMsg = "产品未知状态" + status + "不能入库！";
                    string[] paperStrs = new string[]{
                          "IRA02","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);

                }


            }
            else //没有查询到这个条码说明从来没进过系统，可以入库
            {
                retMsg = "";
            }
            return retMsg;
        }
        /// <summary>
        /// 判断一个纸能否红单入库 ProductLife
        /// </summary>
        /// <param name="productid">卷筒和平板的ID</param>
        /// <returns></returns>
        public string CheckCanScanInRed(string productid)
        {
            string retMsg = "";
            //先判断这个条码最后的life状态，
            DataSet wmsDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
            //如果已经入库，而且没有出库那么就可以红单入库
            if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
            {
                string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                string vid = wmsDS.Tables["T_ProductLife"].Rows[0]["VoucherInID"].ToString();
                if (status == Utils.WMSOperate._StatusIn)//说明在库中,或者红单入库取消的状态可以红单
                {

                    if (vid == "")
                    {
                        //没做入库单
                        retMsg = "产品未做入库单，不能红单入库！";
                    }
                    else
                    {
                        //查询入库单，判断入库单的情况
                        WMSDS inDS = this._WMSAccess.Select_T_InStockByVoucherNO(vid);
                        if (inDS.T_InStock.Rows.Count == 0)
                        {
                            //入库单不存在
                            retMsg = "产品入库单id" + vid + "不存在，不能红单入库！";
                        }
                        else
                        {
                            string billstatus = inDS.T_InStock.Rows[0][inDS.T_InStock.IsUploadColumn].ToString();
                            string vno = inDS.T_InStock.Rows[0][inDS.T_InStock.VoucherNOColumn].ToString();

                            if (billstatus != "1")
                            {
                                //没有上传
                                retMsg = "产品入库单" + vno + "未上传，不能红单入库！";

                            }
                            else
                            {
                                //已上传，可以红单
                                return retMsg = "";
                            }

                        }
                    }
                }
                else if (status == Utils.WMSOperate._StatusRedIn)//说明已是红单状态，不可以红单入
                {
                    retMsg = "产品已是红单入库状态！";

                }
                else if (status == Utils.WMSOperate._StatusOut)//说明已出库，不可以红单入
                {
                    retMsg = "产品已于" + time + "出库，不能红单入库！";

                }
                //else if (status == Utils.WMSOperate._StatusHalfOut)//说明半出库，不可以入
                //{
                //    retMsg = "产品已于" + time + "扫描出库，不能红单入库！";

                //}
                ////else if (status == "return") //说明退货，不可以入(不会有这种状态)
                ////{
                ////    //retMsg = "产品已于" + time + "出库，不能重复入库！";
                ////    //string[] paperStrs = new string[]{
                ////    //  "IRA02","9",retMsg
                ////    //   };
                ////    //retMsg = Utils.MakeWMSSocketMsg(paperStrs);
                ////}
                //else if (status == Utils.WMSOperate._StatusHalfReturn) //说明半退货，不可以入
                //{
                //    retMsg = "产品已于" + time + "扫描退货，不能红单入库！";
                //}
                else
                {
                    retMsg = "产品未知状态" + status + "不能红单入库！";
                }


            }
            else //没有查询到这个条码说明从来没进过系统，不可以红单入库
            {
                retMsg = "产品未入库不能红单入库！";
            }
            string[] paperStrs = new string[] { "IRA04", "9", retMsg };
            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            return retMsg;
        }

        /// <summary>
        /// 处理入库请求，并返回入库结果
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_IR02(string[] strs)
        {
            try
            {
                string retMsg = "";
                string productid = strs[2];
                string inDate = strs[3];//交班时间
                string business = strs[4];
                string sourceVoucher = strs[5];
                string factory = strs[6];
                string whcode = strs[7];
                string biller = strs[8];
                string warehouse = strs[9];
                string shift = strs[10];//班组
                string shiftTime = strs[11];//早班晚班
                string remark = strs[12];
                string custName = strs[13];
                string isCanIn = this.StatusCheckIR02(productid, "IRA02");
                string factoryCode = "";
                //通过工厂代码查询ERP的组织
                WMSDS fds = this._WMSAccess.Select_T_Factory(true, factory);
                if (fds.T_Factory.Rows.Count > 0)
                {
                    factoryCode = fds.T_Factory.Rows[0][fds.T_Factory.FactoryAbbrColumn].ToString();
                }
                else
                {
                    string[] factoryStrs = new string[] { "IRA02", "9", " 包装机台不存在" };
                    isCanIn = Utils.WMSMessage.MakeWMSSocketMsg(factoryStrs);
                }
                if (isCanIn == "")
                {
                    //初次入库从指定机台的rollproduct中是否能查的到，
                    ProduceDS sourceDS = _WMSAccess.Roll_ProductQueryAllByFK(productid, factory);
                    if (sourceDS.Roll_Product.Rows.Count > 0)
                    {
                        //查的到就入进来，入进来的时候同时在老数据表和新数据表中插入
                        ProduceDS.Roll_ProductRow rprow = sourceDS.Roll_Product.Rows[0] as ProduceDS.Roll_ProductRow;
                        WMSDS.T_Product_InRow tpirow = (new WMSDS()).T_Product_In.NewT_Product_InRow();
                        #region 卷筒部分
                        tpirow.RollCount = rprow.IsRollCountNull() ? 1 : Convert.ToInt32(rprow.RollCount);
                        tpirow.WidthLabel = rprow.IsWidthLabelNull() ? 0 : Convert.ToDecimal(rprow.WidthLabel);
                        //tpirow.DiameterLabel = 0;//默认为0  传给ERP用的
                        //tpirow.LengthLabel = 0;//默认为0
                        ////实际值 保存打印
                        //tpirow.DiameterLabelPrint = rprow.IsDiameterLabelNull() ? 0 : Convert.ToDecimal(rprow.DiameterLabel.ToString());
                        //tpirow.LengthLabelPrint = rprow.IsLengthLabelNull() ? 0 : Convert.ToDecimal(rprow.LengthLabel.ToString());

                        //以下为新修改，自动扫描入库 DiameterLabel与LengthLabel值均为批量修改属性=============================================
                        tpirow.DiameterLabel = (rprow.IsDiameterLabelNull() || rprow.DiameterLabel == "") ? 0 : Convert.ToDecimal(rprow.DiameterLabel);
                        tpirow.LengthLabel = (rprow.IsLengthLabelNull() || rprow.LengthLabel == "") ? 0 : Convert.ToDecimal(rprow.LengthLabel);

                        //实际线长，实际直径
                        tpirow.DiameterLabelPrint = rprow.IsDiameterLabelPrintNull() ? 0 : Convert.ToDecimal(rprow.DiameterLabelPrint.ToString());
                        tpirow.LengthLabelPrint = rprow.IsLengthLabelPrintNull() ? 0 : Convert.ToDecimal(rprow.LengthLabelPrint.ToString());
                        //==========================================================================================================================
                        //此处已作为色相显示  自动扫描入库
                        tpirow.Color = rprow.IsColorNull() ? null : rprow.Color;
                        tpirow.CustTrademark = rprow.IsCustTrademarkNull() ? null : rprow.CustTrademark;//特殊客户，默认为空

                        tpirow.WeightMode = rprow.IsWeightModeNull() ? null : rprow.WeightMode;
                        tpirow.CoreDiameter = rprow.IsCoreDiameterNull() ? Convert.ToInt32(0) : Convert.ToInt32(rprow.CoreDiameter);
                        tpirow.Splice = rprow.IsSpliceNull() ? Convert.ToInt32(0) : Convert.ToInt32(rprow.Splice);
                        tpirow.Direction = rprow.IsDirectionNull() ? null : rprow.Direction;
                        tpirow.Layers = rprow.IsLayersNull() ? Convert.ToInt32(0) : Convert.ToInt32(rprow.Layers);
                        tpirow.Specification = Utils.WMSMessage.TrimEndZero(tpirow.WidthLabel.ToString());
                        tpirow.ReamPackType = rprow.IsPackTypeNull() ? "" : rprow.PackType;
                        //tpirow.TrademarkStyle = "";//默认为空
                        #endregion
                        #region 公共参数
                        tpirow.ProductID = rprow.ProductID;
                        tpirow.ProductTypeCode = "1";
                        tpirow.BatchNO = rprow.IsBatchNONull() ? null : rprow.BatchNO;
                        tpirow.Factory = rprow.IsFactoryIDNull() ? null : rprow.FactoryID;
                        tpirow.MachineID = rprow.IsMachineIDNull() ? null : rprow.MachineID;
                        tpirow.MaterialCode = rprow.IsMaterialCodeNull() ? null : rprow.MaterialCode;
                        tpirow.MaterialName = rprow.IsMaterialNameNull() ? null : rprow.MaterialName;
                        tpirow.Standard = rprow.IsStandardCodeNull() ? null : rprow.StandardCode;
                        tpirow.ProductName = rprow.IsProductNameNull() ? null : rprow.ProductName;
                        tpirow.ProductType = rprow.IsProductTypeNull() ? null : rprow.ProductType;
                        tpirow.Trademark = rprow.IsTrademarkNull() ? null : rprow.Trademark;
                        tpirow.Grade = rprow.IsGradeNull() ? null : rprow.Grade;
                        tpirow.BasisweightLabel = rprow.IsBasisweightLabelNull() || rprow.BasisweightLabel == "" ? 0 : Convert.ToDecimal(rprow.BasisweightLabel);
                        tpirow.WhiteDegree = rprow.IsWhiteDegreeNull() ? null : rprow.WhiteDegree;
                        tpirow.WeightLabel = rprow.IsWeightLabelNull() ? 0 : Convert.ToDecimal(rprow.WeightLabel.ToString());
                        tpirow.CustCode = rprow.IsCustomerCodeNull() ? null : rprow.CustomerCode;//没用
                        tpirow.OrderNO = rprow.IsOrderNONull() ? null : rprow.OrderNO;
                        tpirow.PaperCertCode = rprow.IsPaperCertNull() ? null : rprow.PaperCert;
                        tpirow.SpecProdName = rprow.IsSpecProdNameNull() ? null : rprow.SpecProdName;
                        tpirow.SpecCustName = rprow.IsSpecCustNameNull() ? null : rprow.SpecCustName;
                        tpirow.SKU = rprow.IsSKUNull() ? null : rprow.SKU;
                        tpirow.Finish = rprow.IsFinishNull() ? null : rprow.Finish;
                        tpirow.PKG = rprow.IsPKGNull() ? null : rprow.PKG;
                        tpirow.MWeight = rprow.IsMWeightNull() ? null : rprow.MWeight;
                        tpirow.Remark = rprow.IsRemarkNull() ? null : rprow.Remark;
                        tpirow.Cdefine1 = rprow.IsCdefine1Null() ? null : rprow.Cdefine1;//复卷工
                        tpirow.Cdefine2 = rprow.IsCdefine2Null() ? null : rprow.Cdefine2;
                        tpirow.Cdefine3 = rprow.IsCdefine3Null() ? null : rprow.Cdefine3;



                        tpirow.Udefine1 = rprow.IsUdefine1Null() ? Convert.ToInt32(0) : rprow.Udefine1;
                        tpirow.Udefine2 = rprow.IsUdefine2Null() ? Convert.ToInt32(0) : rprow.Udefine2;
                        tpirow.Udefine3 = rprow.IsUdefine3Null() ? Convert.ToInt32(0) : rprow.Udefine3;
                        tpirow.SlidesOfReam = 0;
                        tpirow.SlidesOfSheet = 0;
                        tpirow.IsWhiteFlag = rprow.IsWhitedFlag == "普通证" ? "" : rprow.IsWhitedFlag;
                        tpirow.TradeMode = rprow.TradeMode;
                       // tpirow.CustCode = custName;//没用
                        tpirow.IsPolyHook = "";// rprow.IsIsPolyHookNull() ? null : rprow.IsPolyHook;夹板包装默认为空
                        //生成批次号
                        //string batchNO = this.MakeBatchNONew(factoryCode, DateTime.Now.ToString("yyyyMM"));
                        string batchNO = this.MakeBatchNONew(factoryCode, productid);
                        tpirow.BatchNO = batchNO;
                        #endregion
                        #region 仓库部分
                        tpirow.ReadDate = DateTime.Now;
                        tpirow.BusinessType = business;
                        tpirow.SourceVoucher = sourceVoucher;
                        tpirow.Warehouse = whcode;
                        tpirow.WHPosition = factory;// warehouse;
                        tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
                        tpirow.InShift = shift;
                        tpirow.InShiftTime = shiftTime;
                        tpirow.InUser = biller;
                        tpirow.WHRemark = remark;
                        #endregion
                        #region 状态部分
                        tpirow.IsDeleted = false;
                        tpirow.StatusIn = 1;
                        tpirow.StatusOut = 0;
                        tpirow.SourcePID = 0;
                        tpirow.VoucherInID = 0;
                        tpirow.VoucherOutID = 0;
                        #endregion

                        //插入T_Product_In和productlife
                        WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
                        tplrow.Operate = Utils.WMSOperate._OperScanIn;
                        tplrow.OperDate = DateTime.Now;
                        tplrow.OperUser = biller;
                        tplrow.ProductID = productid;
                        tplrow.Status = Utils.WMSOperate._StatusIn;
                        string result = this._WMSAccess.Tran_InsertProductScanIn(tpirow, tplrow);
                        if (result == "")
                        {
                            //retMsg = "入库成功。";
                            //查询这个班组这个班次入库的汇总
                            string inDateS = inDate;// this.ReturnCurrentClassTime(DateTime.Now.ToString("yyyy-MM-dd"), shiftTime);
                            string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");// Convert.ToDateTime(inDateS).AddHours(13).ToString("yyyy-MM-dd HH:mm:ss");
                            //DataSet sumDS = this._WMSAccess.Product_In_SumaryByFk(inDateS, inDateE, biller, shift, shiftTime, tpirow.MaterialCode, tpirow.MachineID, tpirow.Grade, tpirow.WidthLabel.ToString(), tpirow.CoreDiameter.ToString(), tpirow.IsWhiteFlag.ToString(), tpirow.IsLayersNull() ? "" : tpirow.Layers.ToString(), tpirow.IsRollCountNull() ? "" : tpirow.RollCount.ToString(), tpirow.IsSKUNull() ? "" : tpirow.SKU, tpirow.WeightMode, tpirow.IsOrderNONull() ? "" : tpirow.OrderNO, tpirow.IsWHRemarkNull() ? "" : tpirow.WHRemark, tpirow.TradeMode);
                            DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO, "1");

                            if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
                            {
                                //string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
                                // string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();

                                string sum = "0";
                                string count = "1";

                                DataRow currRow = GetGroupRowByProductInRow(sumDS.Tables["T_Product_In"], tpirow);
                                if (currRow == null)
                                {
                                    sum = "0";
                                    count = "1";
                                }
                                else
                                {
                                    sum = currRow["WeightSum"].ToString();
                                    count = currRow["IdCount"].ToString();
                                }

                                string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : "";// Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(rprow.IsLengthLabelPrintNull() ? "0" : (rprow.LengthLabelPrint.ToString())) : "";//Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(rprow.IsDiameterLabelPrintNull() ? "0" : (rprow.DiameterLabelPrint.ToString())) : "";// Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : "";

                                //组合纸卷的信息
                                string[] paperStrs = new string[]{
                         // "IRA02","0","入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, tpirow.CoreDiameter.ToString(), 
                         //Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()),tpirow.Layers.ToString(), tpirow.WeightMode,tpirow.IsPalletReamsNull()?"":tpirow.PalletReams.ToString(),
                         // tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),tpirow.IsSlidesOfSheetNull()?"":tpirow.SlidesOfSheet.ToString(),Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),
                         // count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),tpirow.IsDirectionNull()?"": tpirow.Direction, tpirow.IsSKUNull()?"": tpirow.SKU,
                         //tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName, tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,
                         //tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,
                         //       tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                         //       tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsTransportTypeNull()?"":tpirow.TransportType,tpirow.TradeMode
                          "IRA02","0","入库成功",tpirow.ProductID,tpirow.BatchNO, tpirow.IsMaterialNameNull()?"": tpirow.MaterialName, tpirow.Grade, tpirow.Specification, Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabelPrint.ToString()),
                            coreReam,lengthSheet,direct,tpirow.Layers.ToString(), tpirow.WeightMode, tpirow.IsSlidesOfReamPrintNull()?"":tpirow.SlidesOfReamPrint.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),diameterReamSheet,
                                            tpirow.IsColorNull()?"":tpirow.Color,//色相
                                    tpirow.IsLengthLabelNull()?"":Convert.ToString(tpirow.LengthLabel),//计划线长
                                    tpirow.IsDiameterLabelNull()?"":Convert.ToString(tpirow.DiameterLabel)//计划直径
                           };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }
                        }
                        else
                        {
                            string[] paperStrs = new string[]{
                          "IRA02","9"," 保存入库数据失败:",result
                           };
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        }
                    }

                    else
                    {
                        //查不到就报错
                        retMsg = "生产数据中查不到条码信息！";
                        string[] paperStrs = new string[]{
                          "IRA02","9",retMsg
                           };
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                    }

                }
                else
                {
                    retMsg = isCanIn;
                }

                return retMsg;
            }
            catch (Exception ex)
            {
                string[] paperStrs = new string[] { "IRA02", "9", "入库异常" + ex.Message };
                return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
        }
        /// <summary>
        /// 检查卷筒入库动作in，对应的条码状态，并返回能否in的结果
        /// </summary>
        /// <param name="productid">条形码</param>
        /// <returns>结果=“”可以in，！=“”为错误信息</returns>
        private string StatusCheckIR02(string productid, string command)
        {
            string retMsg = "";
            //先判断这个条码的life状态
            DataSet wmsDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
            if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
            {
                string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                string prodOnlyID = wmsDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();
                switch (status)
                {
                    case Utils.WMSOperate._StatusIn:
                    case Utils.WMSOperate._StatusCancelRedIn:
                    case Utils.WMSOperate._StatusCancelOut:
                    case Utils.WMSOperate._StatusRedOut:
                    case Utils.WMSOperate._StatusCancelRedOut:
                        string[] paperStrs = new string[] { command, "99", "产品已入库，不能重复入库！" };
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        break;
                    case Utils.WMSOperate._StatusCancelIn:
                        retMsg = "";//可以入库
                        break;
                    case Utils.WMSOperate._StatusRedIn:
                        // 看是不是已经做了红单入库单
                        WMSDS ds = this._WMSAccess.Select_T_Product_InAndT_InStock(prodOnlyID);
                        if (ds.T_InStock.Rows.Count == 0)
                        {
                            //还没有做入库红单
                            paperStrs = new string[] { command, "9", "产品已红单入库扫描，不能入库，请取消红单入库扫描！" };
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        }
                        else
                        {
                            //看这个入库红单有没有上传
                            string close = ds.T_InStock.Rows[0][ds.T_InStock.IsCloseColumn].ToString();
                            if (close == "1")
                            {
                                //已关单
                                retMsg = "";
                            }
                            else
                            {
                                //未关单
                                string vno = ds.T_InStock.Rows[0][ds.T_InStock.VoucherNOColumn].ToString();
                                paperStrs = new string[] { command, "9", "产品已在入库红单" + vno + "中，且未上传，不能入库！需要先上传红单入库单使产品从库存中减掉，再入库；或者从红单入库单中取消，回到库存" };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }
                        }
                        break;
                    case Utils.WMSOperate._StatusOut:
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，不能重复入库！" });
                        break;

                    default:
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未知状态" + status + "不能入库！" });
                        break;
                }
            }
            else //没有查询到这个条码说明从来没进过系统，可以入库
            {
                retMsg = "";
            }
            return retMsg;
        }
        // private string MakeBatchNO(WMSDS.T_Product_InRow row)
        // {
        //     string batchNO = "";
        //     string[] batchStrs = new string[]{
        //     row.MachineID,
        //     row.MaterialCode,
        //     row.ProductTypeCode,
        //     row.Grade,
        //     row.IsWidthLabelNull()?"Null":row.WidthLabel.ToString(),
        //     row.IsWeightModeNull()?"Null":row.WeightMode,
        //     row.IsCoreDiameterNull()?"Null":row.CoreDiameter.ToString(),
        //     row.IsSheetLengthLabelNull()?"Null": row.SheetLengthLabel.ToString(),
        //    row.IsSheetWidthLabelNull()?"Null": row.SheetWidthLabel.ToString(),
        //    row.IsPalletReamsNull()?"Null": row.PalletReams.ToString(),
        //    row.IsSlidesOfReamNull()?"Null":row.SlidesOfReam.ToString(),
        //    row.IsSlidesOfSheetNull()?"Null":row.SlidesOfSheet.ToString(),
        //  // row.IsTransportTypeNull()?"Null":  row.TransportType,
        //   row.IsReamPackTypeNull()?"Null":row.ReamPackType,
        //   row.IsIsPolyHookNull()?"Null":row.IsPolyHook,
        //    row.IsFiberDirectNull()?"Null": row.FiberDirect,
        //    row.IsDiameterLabelNull()?"Null":row.DiameterLabel.ToString(),
        //    row.IsLengthLabelNull()?"Null": row.LengthLabel.ToString(), 
        //    row.IsLayersNull()?"Null":row.Layers.ToString(), 
        //    row.IsSKUNull()?"Null":row.SKU, 
        ////   row.IsCustTrademarkNull()?"Null":  row.CustTrademark,
        //   row.IsPaperCertCodeNull()?"Null":row.PaperCertCode,
        //    row.IsSpecProdNameNull()?"Null": row.SpecProdName,
        //    row.IsSpecCustNameNull()?"Null":row.SpecCustName,
        //  //  row.IsTrademarkStyleNull()?"Null":row.TrademarkStyle,
        //    row.IsIsWhiteFlagNull()?"Null":row.IsWhiteFlag,
        //    row.IsOrderNONull()?"Null":row.OrderNO,
        //    DateTime.Now.ToString("yyyyMM"),
        //    row.IsCdefine1Null()?"Null": row.Cdefine1,
        //    row.IsCdefine2Null()?"Null":row.Cdefine2,
        //    row.IsCdefine3Null()?"Null":row.Cdefine3,
        //   row.IsUdefine1Null()?"Null": row.Udefine1.ToString(),
        //   row.IsUdefine2Null()?"Null":row.Udefine2.ToString(),
        //   row.IsUdefine3Null()?"Null":row.Udefine3.ToString()
        //  // row.IsCustCodeNull()?"Null":row.CustCode

        //     };

        //     string batchstr = Utils.WMSMessage.MakeWMSSocketMsg(batchStrs);
        //     batchNO = this._WMSAccess.Select_T_BatchNO(row.MachineID, DateTime.Now.ToString("yyyyMM"), batchstr);//string batchfactory,string batchno,string prodproperties);
        //     return batchNO;
        // }
        /// <summary>
        /// 通过组织年月生成批号
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string MakeBatchNONew(string orgcode, string productid)
        {
            //string batchNO = orgcode + yyyymm;
            //return batchNO;

            // 用产品编号包含的时间生成批次号//2321115010140194"    1611502041620302"
            string batchNO = "";
            if (productid.Substring(2, 1) == "2")//平板
            {
                batchNO = orgcode + "20" + productid.Substring(5, 4);
            }
            else if (productid.Substring(2, 1) == "1")//卷筒
            {
                batchNO = orgcode + "20" + productid.Substring(3, 4);
            }
            return batchNO;
        }

        /// <summary>
        /// 返回当前班次的开始时间，早，中，晚
        /// </summary>
        /// <returns></returns>
        private string ReturnCurrentClassTime(string inDate, string shiftTime)
        {
            string minTime = inDate;
            if (shiftTime == "")
            {
                minTime = inDate;//DateTime.Now.AddHours(-13).ToString();
            }
            if (shiftTime == "早班")
            {
                minTime = minTime + " 5:30:00";
            }
            if (shiftTime == "中班")
            {
                minTime = minTime + " 13:30:00";
            }
            if (shiftTime == "夜班")
            {
                //if (DateTime.Now.CompareTo(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 21:30:59")) == 1)//21点半之后,0点之前，则从当天开始算起
                //    minTime = DateTime.Now.ToString("yyyy-MM-dd ") + " 21:30:59";

                //else if (DateTime.Now.CompareTo(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 21:30:59")) == -1) //在21点半之前，0点之后，则从前一天的21点半算起
                //    minTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd ") + " 21:30:59";
                minTime = (Convert.ToDateTime(minTime).AddDays(-1)).ToString("yyyy-MM-dd") + " 22:00:00";

            }
            return minTime;
        }
        //void socketservice_DataReceived(MessageEventArgs e)
        //{
        //    DateTime receivetime = DateTime.Now;

        //    bool IsSendBuffer = false;

        //    //不包含了末尾结束符号
        //    string datagramText = Utils.SocketParaPDA.SocketEncoding.GetString(e.Buffer);

        //    string ReplyMessage = "Do nothing";

        //    string[] strs = datagramText.TrimStart(this._StartChar).TrimEnd(this._EndChar).Split(this._SpliteChar);

        //    UInt16 CharCount = 0;
        //    string FunctionCode = "";
        //    string RollID = "";

        //    int Diameter_M = 0, Width_M = 0, Weight_M = 0;
        //    string RollStatus = "";

        //    if (strs.Length > 1)
        //    {
        //        CharCount = (UInt16)Convert.ToInt16(strs[0]);
        //        FunctionCode = strs[1];
        //        //验证数据长度
        //        if ((CharCount + 4) != datagramText.Length)
        //        {
        //            //验算失败
        //            ReplyMessage = "Message length wrong";
        //        }
        //        else
        //        {
        //            switch (FunctionCode)
        //            {
        //                case "R01":
        //                    if (strs.Length > 6)
        //                    {
        //                        RollID = strs[2].Trim();

        //                        if (VerifyIsNumber(strs[3]) &&
        //                              VerifyIsNumber(strs[4]) &&
        //                                VerifyIsNumber(strs[5]))
        //                        {
        //                            Diameter_M = Convert.ToInt32(strs[3]);
        //                            Width_M = Convert.ToInt32(strs[4]);
        //                            Weight_M = Convert.ToInt32(strs[5]);
        //                            RollStatus = strs[6].Trim();

        //                            RollDS.Roll_ProductDataTable rolltb;
        //                            rolltb = _WMSAccess.RollDS_Roll_ProductQueryByBarcode(RollID).Roll_Product;

        //                            if (rolltb.Rows.Count > 0)
        //                            {
        //                                RollDS.Roll_ProductRow row = rolltb.Rows[0] as RollDS.Roll_ProductRow;
        //                                //
        //                                IsSendBuffer = true;

        //                                //保存测量值
        //                                _WMSAccess.Roll_ProductMetsoMValueUpdateByProductID(RollID, Width_M, Diameter_M, Weight_M);

        //                                //ReplyMessage = GenerateMessageBufferM001(row, "RollWrap", Width_M, Diameter_M, Weight_M);  
        //                                byte[] sendbs = GenerateMessageBufferM001(row, "RollWrap", Width_M, Diameter_M, Weight_M);
        //                                e.Connection.BeginSend(sendbs);

        //                                //保存记录日志
        //                                _WMSAccess.SocketRecordMetsoWrapInsertByValue(FunctionCode, false, "", datagramText, "SVR GET", Utils.DateTimeNow, RollID);
        //                                _WMSAccess.SocketRecordMetsoWrapInsertByValue(FunctionCode, true, "", Encoding.UTF8.GetString(sendbs), "SVR SEND", Utils.DateTimeNow, RollID);

        //                            }
        //                            else
        //                            {
        //                                //1 = error in ID
        //                                //ReplyMessage = "Roll not exsit";
        //                                ReplyMessage = String.Join("|", new string[]{
        //                               "M001",
        //                               RollID.PadRight(16),
        //                               "1",  //1 = error in ID
        //                               "0000",
        //                               "0000",
        //                               "00000",
        //                               "0",
        //                               " ",
        //                               "0","0","0",
        //                               "".PadRight(20),
        //                               "0000",
        //                               "0000",
        //                               "00000",
        //                               "0000",
        //                               "".PadRight(3),  "0000",  "".PadRight(50),  "00",
        //                               "".PadRight(13), 
        //                               "".PadRight(300),
        //                               "".PadRight(60)
        //                               });
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ReplyMessage = "Message not valid number";
        //                        }
        //                    }
        //                    break;

        //                case "R03":
        //                    if (strs.Length > 3)
        //                    {
        //                        RollID = strs[2].Trim();
        //                        RollStatus = strs[3];
        //                        ReplyMessage = String.Join("|", new string[]{
        //                        "M003",
        //                        RollID.PadRight(16)                             
        //                        });



        //                    }
        //                    else
        //                    {
        //                        ReplyMessage = "Message not valid";
        //                    }
        //                    break;

        //                case "R04":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "C01":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "C02":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "C03":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "C04":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "P01":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "P02":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "P03":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "P04":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "L01":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "L02":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "L03":
        //                    ReplyMessage = "M004";



        //                    break;
        //                case "L04":
        //                    ReplyMessage = "M004";



        //                    break;

        //                default:
        //                    ReplyMessage = "Message ID wrong,ignore";
        //                    break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ReplyMessage = "Wrong message";
        //    }

        //    if (!IsSendBuffer)
        //    {

        //        //保存记录日志
        //        _WMSAccess.SocketRecordMetsoWrapInsertByValue(FunctionCode, false, "", datagramText, "SVR GET", Utils.DateTimeNow, RollID);
        //        _WMSAccess.SocketRecordMetsoWrapInsertByValue(FunctionCode, true, "", ReplyMessage, "SVR SEND", Utils.DateTimeNow, RollID);

        //        //答复信息
        //        e.Connection.BeginSend(Utils.SocketParaMetsoRollWrap.SocketEncoding.GetBytes(GenerateSocketString(ReplyMessage)));
        //    }


        //}

        private byte[] GenerateMessageBufferM001(RollDS.Roll_ProductRow row, string ParaType, int Width_M, int Diameter_M, int Weight_M)
        {
            //Message ID	4	M001
            //Roll ID	16	“1234567890      “
            //Error code 1
            //0= no error
            //1 = error in ID
            //2 = error in width
            //3 = error in diameter
            //4 = error in weight
            //5 = error at MIS (roll status wrapped)
            //Ordered roll diameter (mm)	4	0980 = 980 mm
            //Ordered roll width (mm)	4	1002 = 1002 mm
            //Ordered roll weight(kg)	5	01000 = 1000 kg
            //Roll status	1	0=rejected, 1=OK
            //Wrapper layers	2	25 = 2,5 rounds
            //Spare	1	
            //Wrapping	1	0=no, 1= normal, 2= body
            //Wrapper	1	1=normal, 2= printed
            //Type of wrapping paper (neutral or with logo) 
            //Inner head 	1	1=normal
            //Outer head	1	1=normal
            //Body label	1	0=no, 1=yes
            //End label	1	0=no, 1=yes
            //Inner marking	1	0=no, 1=yes
            //Inner marking text 1	31	1234567890  ==>

            string ErrorCode = "0";
            string RollStatus = "1";
            //string WrapLayers = "25";
            string Wrapping = "1";
            //string Wrapper = "1";
            //string InnerHead = "1";
            //string OuterHead = "1";
            string BodyLabel = "1";
            //string EndLabel = "1";
            string InnerMarking = "1";
            //string InnerMarkingText1 =   "";//GenerateInkjetString(row,1);

            //判断纸卷的误差值
            //Width_M
            //测量宽度不正确
            if (Width_M < row.Width - Utils.OffsetParaWidth.LeftInterval ||
                Width_M > row.Width + Utils.OffsetParaWidth.RightInterval)
                ErrorCode = "2";
            else
            {
                //Diameter_M
                if (Diameter_M < row.Diameter - Utils.OffsetParaDiameter.LeftInterval ||
                    Diameter_M > row.Diameter + Utils.OffsetParaDiameter.RightInterval)
                    ErrorCode = "3";
                else
                {
                    //Weight_M
                    if (Weight_M < row.Weight_Calc - Utils.OffsetParaWeightStatic.LeftInterval ||
                        Weight_M > row.Weight_Calc + Utils.OffsetParaWeightStatic.RightInterval)
                        ErrorCode = "4";
                }
            }

            MainDS.App_ParameterDataTable appParaTB = _WMSAccess.App_ParameterQueryByType(ParaType).App_Parameter;
            foreach (MainDS.App_ParameterRow ParaRow in appParaTB.Rows)
            {
                switch (ParaRow.Name)
                {
                    //case "ErrorCode":
                    //           ErrorCode = ParaRow.Value;
                    //           break;
                    case "RollStatus":
                        RollStatus = ParaRow.Value;
                        break;
                    //case "WrapLayers":
                    //           WrapLayers = ParaRow.Value;
                    //           break;
                    case "Wrapping":
                        Wrapping = ParaRow.Value;
                        break;
                    //case "Wrapper":
                    //           Wrapper = ParaRow.Value;
                    //           break;
                    //case "InnerHead":
                    //           InnerHead = ParaRow.Value;
                    //           break;
                    //case "OuterHead":
                    //           OuterHead = ParaRow.Value;
                    //           break;
                    case "BodyLabel":
                        BodyLabel = ParaRow.Value;
                        break;
                    //case "EndLabel":
                    //           EndLabel = ParaRow.Value;
                    //           break;
                    case "InnerMarking":
                        InnerMarking = ParaRow.Value;
                        break;
                        //case "InnerMarkingTextType":
                        //           InnerMarkingText1 = GenerateInkjetString(row, ParaRow.Value);
                        //           break;
                        //case "InnerMarkingText1":
                        //           InnerMarkingText1 = GenerateInkjetString(row, ParaRow.Value);
                        //           break;
                }
            }

            string message = "";
            try
            {
                message = String.Join("|", new string[] {
            "M001",
           row.ProductID.PadRight(16),
            ErrorCode,
            (row.IsDiameterNull()?0:  (int)row.Diameter).ToString("D4"),
           (row.IsWidthNull()?0: row.Width).ToString("D4"),
           // (row.IsWeight_CalcNull()?0: (int)row.Weight_Calc).ToString("D5"),
            (row.IsWeight_CalcNull()?0: (int)row.Weight_Calc).ToString().PadRight(5,' '),

            RollStatus,
            " ",//Spare
            Wrapping,
            BodyLabel,
            InnerMarking,
            row.ProductID.PadRight(20),//Product number
            (row.IsBasisweightNull()?0: (int)row.Basisweight).ToString("D4"),
            //(row.IsWidthNull()?0:  row.Width).ToString("D4"),//Size
            (row.IsWidthNull()?0:  row.Width).ToString().PadRight(4,' '),//Size

            (row.IsRollLengthNull()?0: row.RollLength).ToString("D5"),
            (row.IsCoreNull()?0: (int)row.Core).ToString("D4"),//Corediam
            (row.IsInspector_DescNull()?"":  row.Inspector_Desc).PadRight(3),//Inspector
            //(row.IsWeight_NetNull()?0: (int)row.Weight_Net).ToString("D4"),
            //(row.IsWeight_CalcNull()?0: (int)row.Weight_Calc).ToString("D4"),
            (row.IsWeight_CalcNull()?0: (int)row.Weight_Calc).ToString().PadRight(4,' '),
            (row.IsOrderNONull()?"":  row.OrderNO).PadRight(50),
            (row.IsSpliceNull()?0:row.Splice).ToString("D2") ,//Linkers  接头
            row.ProductID.PadRight(13),//Bohui_code   30多位的博汇编码，需要再次确认
            //(row.IsType_DescNull()?"": row.Type_Desc).PadRight(300),//Product name
            //(row.IsGradeNull()?"":  row.Grade_Desc).PadRight(60)//Grade
            });
            }
            catch { }


            //CCCC数目表示第一个|到末尾char3  1+ 360+2 +1
            message = (char)2 + (message.Length + 364).ToString("D4") + "|" + message;


            byte[] srcbs = Utils.SocketParaMetsoRollWrap.SocketEncoding.GetBytes(message);

            //  ...|+300+|+60+char2
            //   1+300+1+60+1 = 363
            byte[] SendBS = new byte[srcbs.Length + 362];

            Buffer.BlockCopy(srcbs, 0, SendBS, 0, srcbs.Length);

            //指定中文数据字段用UTF-8编辑 
            //ProductName
            byte[] productbs = Encoding.UTF8.GetBytes("|" + (row.IsType_DescNull() ? "" : row.Type_Desc).PadRight(300));
            //byte[] productbs = Encoding.UTF8.GetBytes("|" + ("test product").PadRight(300));
            ////Grade
            byte[] gradebs = Encoding.UTF8.GetBytes("|" + (row.IsGrade_DescNull() ? "" : row.Grade_Desc).PadRight(60));
            //byte[] gradebs = Encoding.UTF8.GetBytes("|" + ("test grade").PadRight(60));


            Buffer.BlockCopy(productbs, 0, SendBS, srcbs.Length, 301);
            Buffer.BlockCopy(gradebs, 0, SendBS, srcbs.Length + 301, 61);
            // Buffer.SetByte(SendBS, SendBS.Length - 1, 3);


            return SendBS;
        }

        private string GenerateInkjetString(MainDS.Roll_ProductRow rollrow, string FormatID)
        {
            string JetString = "";

            MainDS.App_InkJetFormatDataTable InkjetFormatTB = _WMSAccess.App_InkJetFormatQueryByFormatID(FormatID).App_InkJetFormat;

            foreach (MainDS.App_InkJetFormatRow row in InkjetFormatTB.Rows)
            {
                if (row.IsField)
                {
                    string value = Convert.ToString(rollrow[row.FieldName]).Trim(row.TrimChar[0]);
                    if (row.IsPadLeft)
                    {
                        value = value.PadLeft(row.PadLength, row.PadChar[0]);
                    }
                    else
                    {
                        value = value.PadRight(row.PadLength, row.PadChar[0]);
                    }

                    JetString = JetString + value;
                }
                else
                {
                    JetString = JetString + row.TextValue;
                }


            }


            return JetString;

        }
        //检查字符串是不是纯数字
        private bool VerifyIsNumber(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }

        private string GenerateSocketString(string srcStr)
        {
            //长度包括 首'|'以及末尾分隔符
            return (Char)2 + (srcStr.Length + 2).ToString("D4") + "|" + srcStr;
        }

        //赋值
        private void SendToMetsoIP(string data)
        {
            foreach (ISocketConnectionInfo isocketinfo in server_WH_PDA.GetConnections())
            {
                //if (isocketinfo.RemoteEndPoint.Address.ToString() == Utils.SocketParaMetsoRollWrap.IPAddress)
                //{
                //    (isocketinfo as BaseSocketConnection).BeginSend(Utils.SocketParaMetsoRollWrap.SocketEncoding.GetBytes(data));
                //}

                (isocketinfo as BaseSocketConnection).BeginSend(Utils.SocketParaMetsoRollWrap.SocketEncoding.GetBytes(data));
            }
        }

        private void UpdateList(string text)
        {
            //保存信息
            try
            {
                //litleaccess.ExecuteSql("insert into MESMSG (ComeTime,MESData) values(datetime(),'" + text + "')");
            }
            catch
            {
            }
        }

        private void PrintLabelByProductID(string ProductID)
        {
            //string reportpath = Utils.FilePath_ReportDir + "\\" + "123";
            //XtraReport report = new XtraReport();
            //report.LoadLayout(reportpath);
            //report.DisplayName = "Label Print";

            //MainDS.Roll_ProductDataTable rolltb = access.Roll_ProductQueryByProductID(ProductID).Roll_Product;

            ////print
            //report.DataSource = rolltb;

            //try
            //{
            //    report.Print();
            //}
            //catch { } 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendToMetsoIP(GenerateSocketString(this.textBox1.Text));
        }

        #region  处理客户端消息的部分
        /// <summary>
        /// 查询机台号
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string Process_Q08(string msg)
        {

            WMSDS wmsDS = this._WMSAccess.T_OrganizationQuery(1, 0, true);
            string forMsg = "";
            string[] msgs = null;
            if (wmsDS.T_Organization.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_Organization.Rows.Count; i++)
                {
                    string org = wmsDS.T_Organization.Rows[i]["OrganizationCode"].ToString().Trim() + "." + wmsDS.T_Organization.Rows[i]["OrganizationName"].ToString().Trim();
                    forMsg = org + Utils.WMSMessage._ForeachChar;
                }
                forMsg = forMsg.TrimEnd(Utils.WMSMessage._ForeachChar);
                msgs = new string[] { "QA08", "0", forMsg };

            }
            else
            {
                msgs = new string[] { "QA08", "9", "没有找到组织记录。" };

            }
            //   QA08|0|011.PM11/012.pm12/013.pm13
            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }

        /// <summary>
        /// 查询业务机台班组号
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string Process_Q10(string msg)
        {

            WMSDS wmsDS = this._WMSAccess.Org_Factory_ShiftQuery(1, 0, "in", true);
            string bmsg = "", fmsg = "", smsg = "";
            string[] msgs = null;
            if (wmsDS.T_Business_Type.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_Business_Type.Rows.Count; i++)
                {
                    string btype = wmsDS.T_Business_Type.Rows[i]["BusinessCode"].ToString().Trim() + "." + wmsDS.T_Business_Type.Rows[i]["BusinessName"].ToString().Trim();
                    bmsg = btype + Utils.WMSMessage._ForeachChar;
                }
                bmsg = bmsg.TrimEnd(Utils.WMSMessage._ForeachChar);
            }
            else
            {
                bmsg = "";
            }
            if (wmsDS.T_Organization.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_Organization.Rows.Count; i++)
                {
                    string org = wmsDS.T_Organization.Rows[i]["OrganizationCode"].ToString().Trim() + "." + wmsDS.T_Organization.Rows[i]["OrganizationName"].ToString().Trim();
                    fmsg = org + Utils.WMSMessage._ForeachChar;
                }
                fmsg = fmsg.TrimEnd(Utils.WMSMessage._ForeachChar);
            }
            else
            {
                fmsg = "";
            }
            if (wmsDS.T_Shift.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_Shift.Rows.Count; i++)
                {
                    string shift = wmsDS.T_Shift.Rows[i]["OrganizationCode"].ToString().Trim() + "." + wmsDS.T_Shift.Rows[i]["OrganizationName"].ToString().Trim();
                    smsg = shift + Utils.WMSMessage._ForeachChar;
                }
                smsg = smsg.TrimEnd(Utils.WMSMessage._ForeachChar);
            }
            else
            {
                smsg = "";

            }
            //   QA08|0|011.PM11/012.pm12/013.pm13

            if (bmsg != "" && fmsg != "" && smsg != "")
                msgs = new string[] { "QA10", "0", bmsg, fmsg, smsg };
            else
                msgs = new string[] { "QA10", "9", "业务，组织，班组查询失败" };
            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }


        private string Process_Q11(string factory)
        {

            WMSDS wmsDS = this._WMSAccess.User_WarehouseQuery(factory, 2, "0000", 1, true);
            string umsg = "", wmsg = "";
            string[] msgs = null;
            if (wmsDS.T_User.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_User.Rows.Count; i++)
                {
                    string btype = wmsDS.T_User.Rows[i]["UserCode"].ToString().Trim() + "." + wmsDS.T_User.Rows[i]["UserName"].ToString().Trim();
                    umsg = btype + Utils.WMSMessage._ForeachChar;
                }
                umsg = umsg.TrimEnd(Utils.WMSMessage._ForeachChar);
            }
            else
            {
                umsg = "";
            }
            if (wmsDS.T_Warehouse.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_Warehouse.Rows.Count; i++)
                {
                    string org = wmsDS.T_Warehouse.Rows[i]["WHCode"].ToString().Trim() + "." + wmsDS.T_Warehouse.Rows[i]["WHName"].ToString().Trim();
                    wmsg = org + Utils.WMSMessage._ForeachChar;
                }
                wmsg = wmsg.TrimEnd(Utils.WMSMessage._ForeachChar);
            }
            else
            {
                wmsg = "";
            }

            //   QA08|0|011.PM11/012.pm12/013.pm13

            if (umsg != "" && wmsg != "")
                msgs = new string[] { "QA11", "0", umsg, wmsg };
            else
                msgs = new string[] { "QA11", "9", "用户，仓库查询失败" };
            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }

        private string Process_Q05(string factory, string parent, int level)
        {

            WMSDS wmsDS = this._WMSAccess.T_WarehouseQuery(factory, parent, level, true);
            string umsg = "";
            string[] msgs = null;
            if (wmsDS.T_Warehouse.Rows.Count > 0)
            {
                for (int i = 0; i < wmsDS.T_Warehouse.Rows.Count; i++)
                {
                    string btype = wmsDS.T_Warehouse.Rows[i]["WHCode"].ToString().Trim() + "." + wmsDS.T_Warehouse.Rows[i]["WHName"].ToString().Trim();
                    umsg = btype + Utils.WMSMessage._ForeachChar;
                }
                umsg = umsg.TrimEnd(Utils.WMSMessage._ForeachChar);
            }
            else
            {
                umsg = "";
            }


            //   QA08|0|011.PM11/012.pm12/013.pm13

            if (umsg != "")
                msgs = new string[] { "QA05", "0", umsg };
            else
                msgs = new string[] { "QA05", "9", "库位查询失败" };
            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }
        #endregion

        /// <summary>
        /// 初次出库
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="forceOut">强制出库</param>
        /// 
        /// <returns></returns>
        public string Process_O01(string[] strs, bool forceOut)
        {
            string retMsg = "";
            string productid = strs[2];
            string inDate = strs[4];
            string voucherno = strs[3];
            string command = "OA01";
            //判断发运单的状态,是否已关闭，能否继续出库（发运单的总重量，计划重量）
            DataSet planDS = this._WMSAccess.Select_T_OutStockAndEntryByVoucherNOForMatch(voucherno);

            if (planDS.Tables["T_OutStockAndEntry"].Rows.Count > 0)
            {
                string isclose = planDS.Tables["T_OutStockAndEntry"].Rows[0]["IsClose"].ToString();
                string rb = planDS.Tables["T_OutStockAndEntry"].Rows[0]["BillType"].ToString();

                //判断是否关闭
                //if (isclose == "0" || isclose == "")
                //{
                //看这个出库单据是红单还是蓝单
                if (rb == "R")
                {
                    retMsg = this.Process_O20(strs, forceOut);
                }
                else
                {

                    //开始判断条码是否可以出库,仓库里有没有这个纸的状态
                    DataSet lifeDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);//可以用一条语句查出product in的数据
                    if (lifeDS.Tables.Count > 0 && lifeDS.Tables["T_ProductLife"].Rows.Count > 0)
                    {
                        string status = lifeDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                        string time = lifeDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                        string productonlyid = lifeDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();//life执行的那个id
                        string sourceID = lifeDS.Tables["T_ProductLife"].Rows[0]["SourcePID"].ToString();//执行ID对应的源ID
                        switch (status)
                        {
                            case Utils.WMSOperate._StatusIn:


                                //可以出库
                                //状态为in可以出库,查询纸的信息
                                WMSDS prodDs = this._WMSAccess.Select_T_Product_InByProductID(productonlyid);
                                string machedPlanID = "0";
                                string machedVoucherID = "0";
                                string machedSourcePlanID = "0";
                                string machedSourceVoucherID = "0";
                                string planCountExec = "";
                                string planWeightExec = "";
                                string allCountExec = "";
                                string allWeightExec = "";
                                string result = "";
                                string error = "与计划不符|";
                                if (prodDs.T_Product_In.Rows.Count > 0)
                                {
                                    //开始判断计划
                                    for (int i = 0; i < planDS.Tables["T_OutStockAndEntry"].Rows.Count; i++)
                                    {
                                        result = this.MatchProductAndPlans(prodDs.T_Product_In.Rows[0], planDS.Tables["T_OutStockAndEntry"].Rows[i], forceOut, ref machedPlanID, ref machedVoucherID, ref planCountExec, ref planWeightExec, ref allCountExec, ref allWeightExec, ref machedSourcePlanID, ref machedSourceVoucherID);
                                        error += string.Format("<{0}>{1}|", i, result);
                                        if (result == "" || result.Contains("A:") || result.Contains("B:"))
                                            break;
                                    }
                                    string msg = "", msgCode = "";
                                    //组合纸卷的参数

                                    WMSDS.T_Product_InRow tpirow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                    string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                    string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                    string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                    string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;
                                    string[] paperStrs = new string[]{
                          "OA01",msgCode,msg,tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"": Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(),tpirow.IsWeightModeNull()?"": tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),"count",tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),tpirow.IsCustCodeNull()?"":tpirow.CustCode,tpirow.IsWHRemarkNull()?"":tpirow.WHRemark,
                                planCountExec,planWeightExec,allCountExec,allWeightExec
                           };
                                    if (result == "")//完全配上了
                                    {
                                        msg = "出库成功,已完成" + planCountExec + "件，" + planWeightExec + "吨";
                                        msgCode = "0";
                                        //配上了，就记录productin 表的status out 为1 和生涯life为scanout，
                                        //先插入出库的那条记录
                                        WMSDS.T_Product_InRow outrow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                        #region 状态部分
                                        outrow.StatusIn = 0;
                                        outrow.StatusOut = 1;
                                        outrow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        outrow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                        outrow.VoucherInID = 0;
                                        outrow.SourcePID = tpirow.OnlyID;
                                        #endregion

                                        //把它更新生涯表为出库的状态
                                        WMSDS.T_ProductLifeRow lifeRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                                        lifeRow.ProductOnlyID = 0;//后面会赋值为outrow插入以后的ID
                                        lifeRow.ProductID = productid;
                                        lifeRow.OperUser = "";
                                        lifeRow.OperDate = DateTime.Now;
                                        lifeRow.Operate = Utils.WMSOperate._OperScanOut;
                                        lifeRow.Status = Utils.WMSOperate._StatusOut;
                                        //更新入库记录表
                                        WMSDS.T_Product_InRow sourceRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                        sourceRow.OnlyID = tpirow.OnlyID;
                                        sourceRow.StatusOut = 1;
                                        //sourceRow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        //sourceRow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                        //更新出库与产品关联表
                                        WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                                        osppRow.EntryID = Convert.ToInt32(machedPlanID);
                                        osppRow.ProductID = productid;
                                        osppRow.ProductOnlyID = 0;//后面赋值为outrow插入后的ID
                                        osppRow.ScanTime = DateTime.Now;
                                        osppRow.VoucherID = Convert.ToInt32(machedVoucherID);
                                        //更新出库提交数量
                                        WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                                        oseRow.VoucherID = osppRow.VoucherID;
                                        oseRow.EntryID = osppRow.EntryID;
                                        oseRow.PlanCommitQty = Convert.ToDecimal(planCountExec.Split('/')[0]);
                                        oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(planWeightExec.Split('/')[0]);
                                        //更新发货通知单的提交数量
                                        WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                                        ospeRow.VoucherID = Convert.ToInt32(machedSourceVoucherID == "" ? "0" : machedSourceVoucherID);
                                        ospeRow.EntryID = Convert.ToInt32(machedSourcePlanID == "" ? "0" : machedSourcePlanID);
                                        ospeRow.PlanCommitQty = Convert.ToDecimal(allCountExec);
                                        ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(allWeightExec);


                                        //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                                        //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                                        //ospRow.OnlyID = osppRow.VoucherID;
                                        //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                                        //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                                        string outresult = this._WMSAccess.Tran_ProductScanOut(sourceRow, outrow, lifeRow, osppRow, oseRow, ospeRow);

                                        //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                                        if (outresult == "")
                                        {
                                            paperStrs[1] = msgCode;
                                            paperStrs[2] = msg;
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                        }
                                        else
                                        {
                                            //出库失败 +纸卷信息
                                            //retMsg = "出库失败：" + outresult;
                                            paperStrs[1] = "9";
                                            paperStrs[2] = "出库数据出错：" + outresult;
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                        }

                                    }
                                    else if (result.Contains("A:") && result.Contains("B:"))//配上了但是超重和超件  +纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "1";
                                        paperStrs[2] = result;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                    else if (result.Contains("A:") && !result.Contains("B:"))//配上了但是超重  +纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "2";
                                        paperStrs[2] = result;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                    else if (result.Contains("B:") && !result.Contains("A:"))//配上了但是超件  +纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "3";
                                        paperStrs[2] = result;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                    else//没有配上 报错 +  纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "8";
                                        paperStrs[2] = error;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }

                                }
                                else
                                {
                                    //生涯有查到，实物没查到
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "没有找到条码信息。" });
                                }

                                break;
                            case Utils.WMSOperate._StatusCancelOut:
                            case Utils.WMSOperate._StatusCancelRedIn:

                                //可以出库
                                //状态为in可以出库,查询纸的信息
                                prodDs = this._WMSAccess.Select_T_Product_InByProductID(productonlyid);
                                machedPlanID = "0";
                                machedVoucherID = "0";
                                machedSourcePlanID = "0";
                                machedSourceVoucherID = "0";
                                planCountExec = "";
                                planWeightExec = "";
                                allCountExec = "";
                                allWeightExec = "";
                                result = "";
                                error = "与计划不符|";
                                if (prodDs.T_Product_In.Rows.Count > 0)
                                {
                                    //开始判断计划
                                    for (int i = 0; i < planDS.Tables["T_OutStockAndEntry"].Rows.Count; i++)
                                    {
                                        result = this.MatchProductAndPlans(prodDs.T_Product_In.Rows[0], planDS.Tables["T_OutStockAndEntry"].Rows[i], forceOut, ref machedPlanID, ref machedVoucherID, ref planCountExec, ref planWeightExec, ref allCountExec, ref allWeightExec, ref machedSourcePlanID, ref machedSourceVoucherID);
                                        error += string.Format("<{0}>{1}|", i, result);
                                        if (result == "" || result.Contains("A:") || result.Contains("B:"))
                                            break;
                                    }
                                    string msg = "", msgCode = "";
                                    //组合纸卷的参数

                                    WMSDS.T_Product_InRow tpirow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                    string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                    string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                    string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                    string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;
                                    string[] paperStrs = new string[]{
                          "OA01",msgCode,msg,tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"": Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(),tpirow.IsWeightModeNull()?"": tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),"count",tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle,tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),tpirow.IsCustCodeNull()?"":tpirow.CustCode,tpirow.IsWHRemarkNull()?"":tpirow.WHRemark,
                                planCountExec,planWeightExec,allCountExec,allWeightExec
                           };
                                    if (result == "")//完全配上了
                                    {
                                        msg = "出库成功,已完成" + planCountExec + "件，" + planWeightExec + "吨";
                                        msgCode = "0";
                                        //配上了，就记录productin 表的status out 为1 和生涯life为scanout，
                                        //先插入出库的那条记录
                                        WMSDS.T_Product_InRow outrow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                        #region 状态部分
                                        outrow.StatusIn = 0;
                                        outrow.StatusOut = 1;
                                        outrow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        outrow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                        outrow.VoucherInID = 0;
                                        outrow.SourcePID = tpirow.SourcePID;
                                        #endregion

                                        //把它更新生涯表为出库的状态
                                        WMSDS.T_ProductLifeRow lifeRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                                        lifeRow.ProductOnlyID = 0;//后面会赋值为outrow插入以后的ID
                                        lifeRow.ProductID = productid;
                                        lifeRow.OperUser = "";
                                        lifeRow.OperDate = DateTime.Now;
                                        lifeRow.Operate = Utils.WMSOperate._OperScanOut;
                                        lifeRow.Status = Utils.WMSOperate._StatusOut;
                                        //更新入库记录表
                                        WMSDS.T_Product_InRow sourceRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                        sourceRow.OnlyID = tpirow.SourcePID;
                                        sourceRow.StatusOut = 1;
                                        //sourceRow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        //sourceRow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                        //更新出库与产品关联表
                                        WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                                        osppRow.EntryID = Convert.ToInt32(machedPlanID);
                                        osppRow.ProductID = productid;
                                        osppRow.ProductOnlyID = 0;//后面赋值为outrow插入后的ID
                                        osppRow.ScanTime = DateTime.Now;
                                        osppRow.VoucherID = Convert.ToInt32(machedVoucherID);
                                        //更新出库提交数量
                                        WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                                        oseRow.VoucherID = osppRow.VoucherID;
                                        oseRow.EntryID = osppRow.EntryID;
                                        oseRow.PlanCommitQty = Convert.ToDecimal(planCountExec.Split('/')[0]);
                                        oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(planWeightExec.Split('/')[0]);
                                        //更新发货通知单的提交数量
                                        WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                                        ospeRow.VoucherID = Convert.ToInt32(machedSourceVoucherID == "" ? "0" : machedSourceVoucherID);
                                        ospeRow.EntryID = Convert.ToInt32(machedSourcePlanID == "" ? "0" : machedSourcePlanID);
                                        ospeRow.PlanCommitQty = Convert.ToDecimal(allCountExec);
                                        ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(allWeightExec);


                                        //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                                        //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                                        //ospRow.OnlyID = osppRow.VoucherID;
                                        //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                                        //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                                        string outresult = this._WMSAccess.Tran_ProductScanOut(sourceRow, outrow, lifeRow, osppRow, oseRow, ospeRow);

                                        //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                                        if (outresult == "")
                                        {
                                            paperStrs[1] = msgCode;
                                            paperStrs[2] = msg;
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                        }
                                        else
                                        {
                                            //出库失败 +纸卷信息
                                            //retMsg = "出库失败：" + outresult;
                                            paperStrs[1] = "9";
                                            paperStrs[2] = "出库数据出错：" + outresult;
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                        }

                                    }
                                    else if (result.Contains("A:") && result.Contains("B:"))//配上了但是超重和超件  +纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "1";
                                        paperStrs[2] = result;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                    else if (result.Contains("A:") && !result.Contains("B:"))//配上了但是超重  +纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "2";
                                        paperStrs[2] = result;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                    else if (result.Contains("B:") && !result.Contains("A:"))//配上了但是超件  +纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "3";
                                        paperStrs[2] = result;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                    else//没有配上 报错 +  纸卷信息
                                    {
                                        //需要再次确认
                                        paperStrs[1] = "8";
                                        paperStrs[2] = error;
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }

                                }
                                else
                                {
                                    //生涯有查到，实物没查到
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "没有找到条码信息。" });
                                }
                                break;
                            case Utils.WMSOperate._StatusCancelIn:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未入库不能出库！" });
                                break;
                            case Utils.WMSOperate._StatusRedIn:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已做红单入库，不在库存中，不能出库！" });
                                break;
                            case Utils.WMSOperate._StatusOut:
                                WMSDS ds = this._WMSAccess.Select_T_Product_InAndT_OutStock(productonlyid);
                                if (ds.T_OutStock.Rows.Count > 0)
                                {
                                    //看这个入库单有没有上传
                                    string outvoucherno = ds.T_OutStock.Rows[0][ds.T_OutStock.VoucherNOColumn].ToString();
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在出库单" + outvoucherno + "中，不能出库！" });

                                }
                                else
                                {
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，但未找到出库单，不能出库！" });
                                }
                                break;
                            case Utils.WMSOperate._StatusRedOut:
                                //看这个条码的红单出库单是否已关闭
                                ds = this._WMSAccess.Select_T_Product_InAndT_OutStock(productonlyid);
                                if (ds.T_OutStock.Rows.Count == 0)
                                {
                                    //还没有做入库单
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已做红单出库单，但未找到该单据，不能出库！" });
                                }
                                else
                                {
                                    //看这个入库单有没有上传
                                    string close = ds.T_OutStock.Rows[0][ds.T_OutStock.IsCloseColumn].ToString();
                                    if (close == "1")
                                    {
                                        //如果已关闭就可以出库
                                        //查询纸的信息
                                        prodDs = this._WMSAccess.Select_T_Product_InByProductID(productonlyid);
                                        machedPlanID = "0";
                                        machedVoucherID = "0";
                                        machedSourcePlanID = "0";
                                        machedSourceVoucherID = "0";
                                        planCountExec = "";
                                        planWeightExec = "";
                                        allCountExec = "";
                                        allWeightExec = "";
                                        result = "";
                                        error = "与计划不符|";
                                        if (prodDs.T_Product_In.Rows.Count > 0)
                                        {
                                            //开始判断计划
                                            for (int i = 0; i < planDS.Tables["T_OutStockAndEntry"].Rows.Count; i++)
                                            {
                                                result = this.MatchProductAndPlans(prodDs.T_Product_In.Rows[0], planDS.Tables["T_OutStockAndEntry"].Rows[i], forceOut, ref machedPlanID, ref machedVoucherID, ref planCountExec, ref planWeightExec, ref allCountExec, ref allWeightExec, ref machedSourcePlanID, ref machedSourceVoucherID);
                                                error += string.Format("<{0}>{1}|", i, result);
                                                if (result == "" || result.Contains("A:") || result.Contains("B:"))
                                                    break;
                                            }
                                            string msg = "", msgCode = "";
                                            //组合纸卷的参数

                                            WMSDS.T_Product_InRow tpirow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                            string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                            string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                            string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                            string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;
                                            string[] paperStrs = new string[]{
                          "OA01",msgCode,msg,tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"": Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(),tpirow.IsWeightModeNull()?"": tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),"count",tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),tpirow.IsCustCodeNull()?"":tpirow.CustCode,tpirow.IsWHRemarkNull()?"":tpirow.WHRemark,
                                planCountExec,planWeightExec,allCountExec,allWeightExec
                           };
                                            if (result == "")//完全配上了
                                            {
                                                msg = "出库成功,已完成" + planCountExec + "件，" + planWeightExec + "吨";
                                                msgCode = "0";
                                                //配上了，就记录productin 表的status out 为1 和生涯life为scanout，
                                                //先插入出库的那条记录
                                                WMSDS.T_Product_InRow outrow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                                #region 状态部分
                                                outrow.StatusIn = 0;
                                                outrow.StatusOut = 1;
                                                outrow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                outrow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                                outrow.VoucherInID = 0;
                                                outrow.SourcePID = tpirow.SourcePID;
                                                #endregion

                                                //把它更新生涯表为出库的状态
                                                WMSDS.T_ProductLifeRow lifeRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                                                lifeRow.ProductOnlyID = 0;//后面会赋值为outrow插入以后的ID
                                                lifeRow.ProductID = productid;
                                                lifeRow.OperUser = "";
                                                lifeRow.OperDate = DateTime.Now;
                                                lifeRow.Operate = Utils.WMSOperate._OperScanOut;
                                                lifeRow.Status = Utils.WMSOperate._StatusOut;
                                                //更新入库记录表
                                                WMSDS.T_Product_InRow sourceRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                                sourceRow.OnlyID = tpirow.SourcePID;
                                                sourceRow.StatusOut = 1;
                                                //sourceRow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                //sourceRow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                                //更新出库与产品关联表
                                                WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                                                osppRow.EntryID = Convert.ToInt32(machedPlanID);
                                                osppRow.ProductID = productid;
                                                osppRow.ProductOnlyID = 0;//后面赋值为outrow插入后的ID
                                                osppRow.ScanTime = DateTime.Now;
                                                osppRow.VoucherID = Convert.ToInt32(machedVoucherID);
                                                //更新出库提交数量
                                                WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                                                oseRow.VoucherID = osppRow.VoucherID;
                                                oseRow.EntryID = osppRow.EntryID;
                                                oseRow.PlanCommitQty = Convert.ToDecimal(planCountExec.Split('/')[0]);
                                                oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(planWeightExec.Split('/')[0]);
                                                //更新发货通知单的提交数量
                                                WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                                                ospeRow.VoucherID = Convert.ToInt32(machedSourceVoucherID == "" ? "0" : machedSourceVoucherID);
                                                ospeRow.EntryID = Convert.ToInt32(machedSourcePlanID == "" ? "0" : machedSourcePlanID);
                                                ospeRow.PlanCommitQty = Convert.ToDecimal(allCountExec);
                                                ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(allWeightExec);


                                                //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                                                //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                                                //ospRow.OnlyID = osppRow.VoucherID;
                                                //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                                                //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                                                string outresult = this._WMSAccess.Tran_ProductScanOut(sourceRow, outrow, lifeRow, osppRow, oseRow, ospeRow);

                                                //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                                                if (outresult == "")
                                                {
                                                    paperStrs[1] = msgCode;
                                                    paperStrs[2] = msg;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }
                                                else
                                                {
                                                    //出库失败 +纸卷信息
                                                    //retMsg = "出库失败：" + outresult;
                                                    paperStrs[1] = "9";
                                                    paperStrs[2] = "出库数据出错：" + outresult;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }

                                            }
                                            else if (result.Contains("A:") && result.Contains("B:"))//配上了但是超重和超件  +纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "1";
                                                paperStrs[2] = result;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                            else if (result.Contains("A:") && !result.Contains("B:"))//配上了但是超重  +纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "2";
                                                paperStrs[2] = result;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                            else if (result.Contains("B:") && !result.Contains("A:"))//配上了但是超件  +纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "3";
                                                paperStrs[2] = result;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                            else//没有配上 报错 +  纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "8";
                                                paperStrs[2] = error;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }

                                        }
                                        else
                                        {
                                            //生涯有查到，实物没查到
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "没有找到条码信息。" });
                                        }
                                    }
                                    //如果未关闭就不可以出库
                                    else
                                    {
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在退货单中，但还未关闭，不能出库！请先关闭退货单" });
                                    }
                                }

                                break;
                            case Utils.WMSOperate._StatusCancelRedOut:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，不在库存中，不能出库！" });
                                break;
                            default:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未知状态" + status + "不能入库！" });
                                break;
                        }


                        #region 原来的方法，已注释
                        //判断这个纸的生涯状态
                        //if (status == Utils.WMSOperate._StatusIn)
                        //{
                        //    //状态为in可以出库,查询纸的信息
                        //    WMSDS prodDs = this._WMSAccess.Select_T_Product_InByProductID(productonlyid);
                        //    string machedPlanID = "0";
                        //    string machedVoucherID = "0";
                        //    string machedSourcePlanID = "0";
                        //    string machedSourceVoucherID = "0";
                        //    string planCountExec = "";
                        //    string planWeightExec = "";
                        //    string allCountExec = "";
                        //    string allWeightExec = "";
                        //    string result = "";
                        //    if (prodDs.T_Product_In.Rows.Count > 0)
                        //    {
                        //        //开始判断计划
                        //        for (int i = 0; i < planDS.Tables["T_OutStockAndEntry"].Rows.Count; i++)
                        //        {
                        //            result = this.MatchProductAndPlans(prodDs.T_Product_In.Rows[0], planDS.Tables["T_OutStockAndEntry"].Rows[i], forceOut, ref machedPlanID, ref machedVoucherID, ref planCountExec, ref planWeightExec, ref allCountExec, ref allWeightExec, ref machedSourcePlanID, ref machedSourceVoucherID);
                        //            if (result == "" || result.Contains("A:") || result.Contains("B:"))
                        //                break;
                        //        }
                        //        string msg = "", msgCode = "";
                        //        //组合纸卷的参数

                        //        WMSDS.T_Product_InRow tpirow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                        //           string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                        //        string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                        //        string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                        //        string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;
                        //        string[] paperStrs = new string[]{
                        // // "OA01",msgCode,msg,tpirow.ProductID,tpirow.BatchNO, tpirow.MaterialName, tpirow.Grade, tpirow.Specification,  tpirow.CoreDiameter.ToString(),
                        // // tpirow.PalletReams.ToString(), 
                        // // tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),tpirow.IsSlidesOfSheetNull()?"":tpirow.SlidesOfSheet.ToString(),"",
                        // // "",tpirow.WeightLabel.ToString(),tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                        // //tpirow.IsFiberDirectNull()?"": tpirow.FiberDirect, tpirow.IsSKUNull()?"": tpirow.SKU,
                        // //tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                        // //tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,
                        // //tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,planCountExec,planWeightExec,allCountExec,allWeightExec, 
                        //  "OA01",msgCode,msg,tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"": Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                        //    coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(),tpirow.IsWeightModeNull()?"": tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                        //    Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),"count",tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                        //     tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName, 
                        //     tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                        //     tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                        //        tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.TradeMode,tpirow.IsCustCodeNull()?"":tpirow.CustCode,tpirow.IsWHRemarkNull()?"":tpirow.WHRemark,
                        //        planCountExec,planWeightExec,allCountExec,allWeightExec

                        //   };



                        //        if (result == "")//完全配上了
                        //        {
                        //            msg = "出库成功,已完成"+planCountExec+"件，"+planWeightExec+"吨";
                        //            msgCode = "0";
                        //            //配上了，就记录productin 表的status out 为1 和生涯life为scanout，
                        //            //把它更新生涯表为出库的状态
                        //            WMSDS.T_ProductLifeRow tpfRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                        //            tpfRow.ProductOnlyID = Convert.ToInt32(productonlyid);
                        //            tpfRow.ProductID = productid;
                        //            tpfRow.OperUser = "";
                        //            tpfRow.OperDate = DateTime.Now;
                        //            tpfRow.Operate = Utils.WMSOperate._OperScanOut;
                        //            tpfRow.Status = Utils.WMSOperate._StatusOut;
                        //            //更新入库记录表
                        //            WMSDS.T_Product_InRow tpiRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                        //            tpiRow.OnlyID = tpfRow.ProductOnlyID;
                        //            tpiRow.StatusOut = 1;
                        //            tpirow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //            tpiRow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                        //            //更新出库与产品关联表
                        //            WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                        //            osppRow.EntryID = Convert.ToInt32(machedPlanID);
                        //            osppRow.ProductID = productid;
                        //            osppRow.ProductOnlyID = tpfRow.ProductOnlyID;
                        //            osppRow.ScanTime = DateTime.Now;
                        //            osppRow.VoucherID = Convert.ToInt32(machedVoucherID);
                        //            //更新出库提交数量
                        //            WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                        //            oseRow.VoucherID = osppRow.VoucherID;
                        //            oseRow.EntryID = osppRow.EntryID;
                        //            oseRow.PlanCommitQty = Convert.ToDecimal(planCountExec.Split('/')[0]);
                        //            oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(planWeightExec.Split('/')[0]);
                        //            //更新发货通知单的提交数量
                        //            WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                        //            ospeRow.VoucherID = Convert.ToInt32(machedSourceVoucherID==""?"0":machedSourceVoucherID);
                        //            ospeRow.EntryID = Convert.ToInt32(machedSourcePlanID == "" ? "0" : machedSourcePlanID);
                        //            ospeRow.PlanCommitQty = Convert.ToDecimal(allCountExec);
                        //            ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(allWeightExec);


                        //            //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                        //            //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                        //            //ospRow.OnlyID = osppRow.VoucherID;
                        //            //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                        //            //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                        //            string outresult = this._WMSAccess.Tran_ProductScanOut(tpiRow, tpfRow, osppRow, oseRow, ospeRow);

                        //            //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                        //            if (outresult == "")
                        //            {
                        //                paperStrs[1] = msgCode;
                        //                paperStrs[2] = msg;
                        //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //            }
                        //            else
                        //            {
                        //                //出库失败 +纸卷信息
                        //                //retMsg = "出库失败：" + outresult;
                        //                paperStrs[1] = "9";
                        //                paperStrs[2] = "出库数据出错：" + outresult;
                        //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //            }

                        //        }
                        //        else if (result.Contains("A:") && result.Contains("B:"))//配上了但是超重和超件  +纸卷信息
                        //        {
                        //            //需要再次确认
                        //            paperStrs[1] = "1";
                        //            paperStrs[2] = result;
                        //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //        }
                        //        else if (result.Contains("A:") && !result.Contains("B:"))//配上了但是超重  +纸卷信息
                        //        {
                        //            //需要再次确认
                        //            paperStrs[1] = "2";
                        //            paperStrs[2] = result;
                        //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //        }
                        //        else if (result.Contains("B:") && !result.Contains("A:"))//配上了但是超件  +纸卷信息
                        //        {
                        //            //需要再次确认
                        //            paperStrs[1] = "3";
                        //            paperStrs[2] = result;
                        //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //        }
                        //        else//没有配上 报错 +  纸卷信息
                        //        {
                        //            //需要再次确认
                        //            paperStrs[1] = "8";
                        //            paperStrs[2] = result;
                        //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //        }

                        //    }
                        //    else
                        //    {
                        //        //生涯有查到，实物没查到
                        //        retMsg = "没有找到条码信息。";
                        //        string[] paperStrs = new string[] { "OA01", "9", retMsg };
                        //        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //    }

                        //}
                        //else if (status == Utils.WMSOperate._OperScanOut) {
                        ////看是不是在当前出库单里面
                        //  DataSet pDS=  this._WMSAccess.Select_T_OutStock_Product(voucherno,productid);
                        //  if (pDS.Tables.Count > 0 && pDS.Tables[0].Rows.Count > 0) {
                        //      retMsg = "纸卷已在队列中";
                        //      string[] paperStrs = new string[] { "OA01", "9", retMsg };
                        //      retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //  }
                        //}
                        //else
                        //{
                        //    //纸卷状态不对
                        //    retMsg = "纸卷状态为" + status + "不能出库";
                        //    string[] paperStrs = new string[] { "OA01", "9", retMsg };
                        //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                        //}
                        #endregion
                    }
                    else
                    {
                        //纸卷不存在
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "条码未入库" });
                    }
                }
                //}
                //else
                //{
                //    //发运单已关闭，不能出库。
                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "发运单已关闭，不能出库" });
                //}

            }
            else
            {
                //发运单不存在，不能出库。
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "发运单不存在，不能出库" });
            }

            return retMsg;
        }
        /// <summary>
        /// 红单扫描出库
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="forceOut"></param>
        /// <returns></returns>
        private string Process_O20(string[] strs, bool forceOut)
        {
            string retMsg = "";
            string productid = strs[2];
            string inDate = strs[4];
            string voucherno = strs[3];
            string command = "OA01";
            //判断发运单的状态,是否已关闭，能否继续出库（发运单的总重量，计划重量）
            DataSet planDS = this._WMSAccess.Select_T_OutStockAndEntryByVoucherNOForMatch(voucherno);

            if (planDS.Tables["T_OutStockAndEntry"].Rows.Count > 0)
            {
                string isclose = planDS.Tables["T_OutStockAndEntry"].Rows[0]["IsClose"].ToString();
                string rb = planDS.Tables["T_OutStockAndEntry"].Rows[0]["BillType"].ToString();

                //判断是否关闭
                if (isclose == "0" || isclose == "")
                {
                    //看这个出库单据是红单还是蓝单
                    if (rb == "B")
                    {
                        retMsg = this.Process_O01(strs, forceOut);
                    }
                    else
                    {

                        //开始判断条码是否可以红单出库,仓库里有没有这个纸的状态
                        DataSet lifeDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);//可以用一条语句查出product in的数据
                        if (lifeDS.Tables.Count > 0 && lifeDS.Tables["T_ProductLife"].Rows.Count > 0)
                        {
                            string status = lifeDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                            string time = lifeDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                            string productonlyid = lifeDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();//life执行的那个id
                            string sourceID = lifeDS.Tables["T_ProductLife"].Rows[0]["SourcePID"].ToString();//执行ID对应的源ID
                            switch (status)
                            {
                                case Utils.WMSOperate._StatusIn:
                                case Utils.WMSOperate._StatusRedIn:
                                case Utils.WMSOperate._StatusCancelRedIn:
                                case Utils.WMSOperate._StatusCancelOut:
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "产品未出库不能红单出库。" });
                                    break;
                                case Utils.WMSOperate._StatusCancelIn:
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "产品未入库，不能红单出库。" });
                                    break;
                                case Utils.WMSOperate._StatusRedOut:
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "产品已红单出库，不能再次红单出库。" });
                                    break;
                                case Utils.WMSOperate._StatusCancelRedOut:
                                    {
                                        //可以红单出库
                                        //状态为in可以出库,查询纸的信息
                                        string machedPlanID = "0";
                                        string machedVoucherID = "0";
                                        string machedSourcePlanID = "0";
                                        string machedSourceVoucherID = "0";
                                        string planCountExec = "";
                                        string planWeightExec = "";
                                        string allCountExec = "";
                                        string allWeightExec = "";
                                        string result = "";
                                        string error = "与计划不符|";
                                        WMSDS prodDs = this._WMSAccess.Select_T_Product_InByProductID(productonlyid);
                                        if (prodDs.T_Product_In.Rows.Count > 0)
                                        {
                                            //开始判断计划
                                            for (int i = 0; i < planDS.Tables["T_OutStockAndEntry"].Rows.Count; i++)
                                            {
                                                result = this.MatchProductAndPlans(prodDs.T_Product_In.Rows[0], planDS.Tables["T_OutStockAndEntry"].Rows[i], forceOut, ref machedPlanID, ref machedVoucherID, ref planCountExec, ref planWeightExec, ref allCountExec, ref allWeightExec, ref machedSourcePlanID, ref machedSourceVoucherID);
                                                error += string.Format("<{0}>{1}|", i, result);
                                                if (result == "" || result.Contains("A:") || result.Contains("B:"))
                                                    break;
                                            }
                                            string msg = "", msgCode = "";
                                            //组合纸卷的参数

                                            WMSDS.T_Product_InRow tpirow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                            string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                            string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                            string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                            string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;
                                            string[] paperStrs = new string[]{
                          "OA01",msgCode,msg,tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"": Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(),tpirow.IsWeightModeNull()?"": tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),"count",tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.TradeMode,tpirow.IsCustCodeNull()?"":tpirow.CustCode,tpirow.IsWHRemarkNull()?"":tpirow.WHRemark,
                                planCountExec,planWeightExec,allCountExec,allWeightExec
                           };
                                            if (result == "")//完全配上了
                                            {
                                                msg = "红单出库成功,已完成" + planCountExec + "件，" + planWeightExec + "吨";
                                                msgCode = "0";
                                                //配上了，就记录productin 表的status out 为1 和生涯life为scanout，
                                                //先插入出库的那条记录
                                                WMSDS.T_Product_InRow outrow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                                #region 状态部分
                                                outrow.StatusIn = 0;
                                                outrow.StatusOut = -1;
                                                outrow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                outrow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                                outrow.VoucherInID = 0;
                                                outrow.SourcePID = Convert.ToInt32(sourceID);
                                                #endregion

                                                //把它更新生涯表为出库的状态
                                                WMSDS.T_ProductLifeRow lifeRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                                                lifeRow.ProductOnlyID = 0;//后面会赋值为outrow插入以后的ID
                                                lifeRow.ProductID = productid;
                                                lifeRow.OperUser = "";
                                                lifeRow.OperDate = DateTime.Now;
                                                lifeRow.Operate = Utils.WMSOperate._OperScanOutRed;
                                                lifeRow.Status = Utils.WMSOperate._StatusRedOut;
                                                //更新入库记录表
                                                WMSDS.T_Product_InRow sourceRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                                sourceRow.OnlyID = Convert.ToInt32(sourceID);
                                                sourceRow.StatusOut = 0;
                                                //sourceRow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                //sourceRow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                                //更新出库与产品关联表
                                                WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                                                osppRow.EntryID = Convert.ToInt32(machedPlanID);
                                                osppRow.ProductID = productid;
                                                osppRow.ProductOnlyID = 0;//后面赋值为outrow插入后的ID
                                                osppRow.ScanTime = DateTime.Now;
                                                osppRow.VoucherID = Convert.ToInt32(machedVoucherID);
                                                //更新出库提交数量
                                                WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                                                oseRow.VoucherID = osppRow.VoucherID;
                                                oseRow.EntryID = osppRow.EntryID;
                                                oseRow.PlanCommitQty = Convert.ToDecimal(planCountExec.Split('/')[0]);
                                                oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(planWeightExec.Split('/')[0]);
                                                //更新发货通知单的提交数量
                                                WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                                                ospeRow.VoucherID = Convert.ToInt32(machedSourceVoucherID == "" ? "0" : machedSourceVoucherID);
                                                ospeRow.EntryID = Convert.ToInt32(machedSourcePlanID == "" ? "0" : machedSourcePlanID);
                                                ospeRow.PlanCommitQty = Convert.ToDecimal(allCountExec);
                                                ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(allWeightExec);


                                                //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                                                //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                                                //ospRow.OnlyID = osppRow.VoucherID;
                                                //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                                                //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                                                string outresult = this._WMSAccess.Tran_ProductScanOut(sourceRow, outrow, lifeRow, osppRow, oseRow, ospeRow);

                                                //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                                                if (outresult == "")
                                                {
                                                    paperStrs[1] = msgCode;
                                                    paperStrs[2] = msg;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }
                                                else
                                                {
                                                    //出库失败 +纸卷信息
                                                    //retMsg = "出库失败：" + outresult;
                                                    paperStrs[1] = "9";
                                                    paperStrs[2] = "出库数据出错：" + outresult;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }

                                            }
                                            else if (result.Contains("A:") && result.Contains("B:"))//配上了但是超重和超件  +纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "1";
                                                paperStrs[2] = result;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                            else if (result.Contains("A:") && !result.Contains("B:"))//配上了但是超重  +纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "2";
                                                paperStrs[2] = result;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                            else if (result.Contains("B:") && !result.Contains("A:"))//配上了但是超件  +纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "3";
                                                paperStrs[2] = result;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                            else//没有配上 报错 +  纸卷信息
                                            {
                                                //需要再次确认
                                                paperStrs[1] = "8";
                                                paperStrs[2] = error;
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }

                                        }
                                        else
                                        {
                                            //生涯有查到，实物没查到
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "没有找到条码信息。" });
                                        }
                                    }
                                    break;
                                case Utils.WMSOperate._StatusOut:
                                    //看这个条码的出库单是否已关闭
                                    WMSDS ds = this._WMSAccess.Select_T_Product_InAndT_OutStock(productonlyid);
                                    if (ds.T_OutStock.Rows.Count == 0)
                                    {
                                        //还没有做入库单
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已做出库单，但未找到该单据，不能红单出库！" });
                                    }
                                    else
                                    {
                                        //看这个出库单有没有关闭
                                        string close = ds.T_OutStock.Rows[0][ds.T_OutStock.IsCloseColumn].ToString();
                                        string vno = ds.T_OutStock.Rows[0][ds.T_OutStock.VoucherNOColumn].ToString();

                                        if (close == "1")
                                        {
                                            //可以红单出库
                                            //状态为in可以出库,查询纸的信息
                                            string machedPlanID = "0";
                                            string machedVoucherID = "0";
                                            string machedSourcePlanID = "0";
                                            string machedSourceVoucherID = "0";
                                            string planCountExec = "";
                                            string planWeightExec = "";
                                            string allCountExec = "";
                                            string allWeightExec = "";
                                            string result = "";
                                            string error = "与计划不符|";
                                            WMSDS prodDs = this._WMSAccess.Select_T_Product_InByProductID(productonlyid);
                                            if (prodDs.T_Product_In.Rows.Count > 0)
                                            {
                                                //开始判断计划
                                                for (int i = 0; i < planDS.Tables["T_OutStockAndEntry"].Rows.Count; i++)
                                                {
                                                    result = this.MatchProductAndPlans(prodDs.T_Product_In.Rows[0], planDS.Tables["T_OutStockAndEntry"].Rows[i], forceOut, ref machedPlanID, ref machedVoucherID, ref planCountExec, ref planWeightExec, ref allCountExec, ref allWeightExec, ref machedSourcePlanID, ref machedSourceVoucherID);
                                                    error += string.Format("<{0}>{1}|", i, result);
                                                    if (result == "" || result.Contains("A:") || result.Contains("B:"))
                                                        break;
                                                }
                                                string msg = "", msgCode = "";
                                                //组合纸卷的参数

                                                WMSDS.T_Product_InRow tpirow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                                string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                                string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                                string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                                string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;
                                                string[] paperStrs = new string[]{
                          "OA01",msgCode,msg,tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"": Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(),tpirow.IsWeightModeNull()?"": tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),"count",tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.TradeMode,tpirow.IsCustCodeNull()?"":tpirow.CustCode,tpirow.IsWHRemarkNull()?"":tpirow.WHRemark,
                                planCountExec,planWeightExec,allCountExec,allWeightExec
                           };
                                                if (result == "")//完全配上了
                                                {
                                                    msg = "红单出库成功,已完成" + planCountExec + "件，" + planWeightExec + "吨";
                                                    msgCode = "0";
                                                    //配上了，就记录productin 表的status out 为1 和生涯life为scanout，
                                                    //先插入出库的那条记录
                                                    WMSDS.T_Product_InRow outrow = prodDs.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                                    #region 状态部分
                                                    outrow.StatusIn = 0;
                                                    outrow.StatusOut = -1;
                                                    outrow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    outrow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                                    outrow.VoucherInID = 0;
                                                    outrow.SourcePID = Convert.ToInt32(sourceID);
                                                    #endregion

                                                    //把它更新生涯表为出库的状态
                                                    WMSDS.T_ProductLifeRow lifeRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                                                    lifeRow.ProductOnlyID = 0;//后面会赋值为outrow插入以后的ID
                                                    lifeRow.ProductID = productid;
                                                    lifeRow.OperUser = "";
                                                    lifeRow.OperDate = DateTime.Now;
                                                    lifeRow.Operate = Utils.WMSOperate._OperScanOutRed;
                                                    lifeRow.Status = Utils.WMSOperate._StatusRedOut;
                                                    //更新入库记录表
                                                    WMSDS.T_Product_InRow sourceRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                                    sourceRow.OnlyID = Convert.ToInt32(sourceID);
                                                    sourceRow.StatusOut = 0;
                                                    //sourceRow.OutDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    //sourceRow.VoucherOutID = Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                                    //更新出库与产品关联表
                                                    WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                                                    osppRow.EntryID = Convert.ToInt32(machedPlanID);
                                                    osppRow.ProductID = productid;
                                                    osppRow.ProductOnlyID = 0;//后面赋值为outrow插入后的ID
                                                    osppRow.ScanTime = DateTime.Now;
                                                    osppRow.VoucherID = Convert.ToInt32(machedVoucherID);
                                                    //更新出库提交数量
                                                    WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                                                    oseRow.VoucherID = osppRow.VoucherID;
                                                    oseRow.EntryID = osppRow.EntryID;
                                                    oseRow.PlanCommitQty = Convert.ToDecimal(planCountExec.Split('/')[0]);
                                                    oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(planWeightExec.Split('/')[0]);
                                                    //更新发货通知单的提交数量
                                                    WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                                                    ospeRow.VoucherID = Convert.ToInt32(machedSourceVoucherID == "" ? "0" : machedSourceVoucherID);
                                                    ospeRow.EntryID = Convert.ToInt32(machedSourcePlanID == "" ? "0" : machedSourcePlanID);
                                                    ospeRow.PlanCommitQty = Convert.ToDecimal(allCountExec);
                                                    ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(allWeightExec);


                                                    //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                                                    //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                                                    //ospRow.OnlyID = osppRow.VoucherID;
                                                    //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                                                    //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                                                    string outresult = this._WMSAccess.Tran_ProductScanOut(sourceRow, outrow, lifeRow, osppRow, oseRow, ospeRow);

                                                    //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                                                    if (outresult == "")
                                                    {
                                                        paperStrs[1] = msgCode;
                                                        paperStrs[2] = msg;
                                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                    }
                                                    else
                                                    {
                                                        //出库失败 +纸卷信息
                                                        //retMsg = "出库失败：" + outresult;
                                                        paperStrs[1] = "9";
                                                        paperStrs[2] = "出库数据出错：" + outresult;
                                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                    }

                                                }
                                                else if (result.Contains("A:") && result.Contains("B:"))//配上了但是超重和超件  +纸卷信息
                                                {
                                                    //需要再次确认
                                                    paperStrs[1] = "1";
                                                    paperStrs[2] = result;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }
                                                else if (result.Contains("A:") && !result.Contains("B:"))//配上了但是超重  +纸卷信息
                                                {
                                                    //需要再次确认
                                                    paperStrs[1] = "2";
                                                    paperStrs[2] = result;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }
                                                else if (result.Contains("B:") && !result.Contains("A:"))//配上了但是超件  +纸卷信息
                                                {
                                                    //需要再次确认
                                                    paperStrs[1] = "3";
                                                    paperStrs[2] = result;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }
                                                else//没有配上 报错 +  纸卷信息
                                                {
                                                    //需要再次确认
                                                    paperStrs[1] = "8";
                                                    paperStrs[2] = error;
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                }

                                            }
                                            else
                                            {
                                                //生涯有查到，实物没查到
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "没有找到条码信息。" });
                                            }

                                        }
                                        else
                                        {
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已在出库单" + vno + "中，但未关闭，不能红单出库！请取消扫描出库" });

                                        }
                                    }
                                    break;
                                default:
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未知状态" + status + "不能入库！" });
                                    break;
                            }
                        }
                        else
                        {
                            //纸卷不存在
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "条码未入库" });
                        }
                    }
                }
                else
                {
                    //发运单已关闭，不能出库。
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "发运单已关闭，不能出库" });
                }

            }
            else
            {
                //发运单不存在，不能出库。
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA01", "9", "发运单不存在，不能出库" });
            }

            return retMsg;
        }
        /// <summary>
        ///产品匹配计划
        /// </summary>
        /// <param name="dataRow">产品row</param>
        /// <param name="dataRow_2">计划row</param>
        /// <param name="forceOut">计划row</param>
        /// <param name="planId">planid</param>
        /// <param name="voucherId">voucherId</param>
        /// <param name="planCountExec">计划已出数量</param>
        /// <param name="planWeightExec">计划已出重量</param>
        /// <param name="allCountExec">总出数量</param>
        /// <param name="allOutWeight">总出重量</param>
        /// <returns>匹配结果</returns>
        private string MatchProductAndPlans(DataRow dataRow, DataRow dataRow_2, bool forceOut, ref string planId, ref string voucherId, ref string planCountExec, ref string planWeightExec, ref string allCountExec, ref string allWeightExec, ref string sourceEntryID, ref string sourceVoucherID)
        {
            //，组合纸卷属性；
            string materialcode = dataRow["MaterialCode"].ToString().Trim();
            string grade = dataRow["Grade"].ToString().Trim();
            string specification = dataRow["Specification"].ToString().Trim();
            string widthr = Utils.WMSMessage.TrimEndZero(dataRow["WidthLabel"].ToString()).Trim();
            widthr = widthr == "0" ? "" : widthr;
            string widthp = Utils.WMSMessage.TrimEndZero(dataRow["SheetWidthLabel"].ToString()).Trim();
            widthp = widthp == "0" ? "" : widthp;
            string lengthp = Utils.WMSMessage.TrimEndZero(dataRow["SheetLengthLabel"].ToString()).Trim();
            lengthp = lengthp == "0" ? "" : lengthp;
            string weightmode = dataRow["WeightMode"].ToString().Trim();

            string core = "", diameter = "", length = ""; //(纸芯/令数、直径/令张数、线长/件张数)
            if (dataRow["ProductTypeCode"].ToString() == "1")//卷筒
            {
                core = Utils.WMSMessage.TrimEndZero(dataRow["CoreDiameter"].ToString().Trim());
                diameter = Utils.WMSMessage.TrimEndZero(dataRow["DiameterLabel"].ToString()).Trim();
                length = Utils.WMSMessage.TrimEndZero(dataRow["LengthLabel"].ToString()).Trim();
                core = core == "0" ? "" : core;
                diameter = diameter == "0" ? "" : diameter;
                length = length == "0" ? "" : length;
            }
            if (dataRow["ProductTypeCode"].ToString() == "2")//平板
            {
                core = Utils.WMSMessage.TrimEndZero(dataRow["PalletReams"].ToString().Trim());
                diameter = Utils.WMSMessage.TrimEndZero(dataRow["SlidesOfReam"].ToString()).Trim();
                length = Utils.WMSMessage.TrimEndZero(dataRow["SlidesOfSheet"].ToString()).Trim();
                core = core == "0" ? "" : core;
                diameter = diameter == "0" ? "" : diameter;
                length = length == "0" ? "" : length;
            }
            string ream = "";


            //string slidesream = Utils.WMSMessage.TrimEndZero(dataRow["SlidesOfReam"].ToString().Trim());
            //slidesream = slidesream == "0" ? "" : slidesream;
            //string slidesheet = Utils.WMSMessage.TrimEndZero(dataRow["SlidesOfSheet"].ToString().Trim());
            //slidesheet = slidesheet == "0" ? "" : slidesheet;

            string reampack = dataRow["ReamPackType"].ToString().Trim();//包装方式
            string sku = dataRow["SKU"].ToString().Trim();
            string custtrademark = dataRow["CustTrademark"].ToString().Trim();//特殊客户 来自tpi
            string papercsert = dataRow["PaperCertCode"].ToString().Trim();
            string speccust = dataRow["SpecCustName"].ToString().Trim();
            string specprod = dataRow["SpecProdName"].ToString().Trim();
            string trademarkstyle = dataRow["IsPolyHook"].ToString().Trim();//夹板包装
            string iswhiteflage = dataRow["IsWhiteFlag"].ToString().Trim() == "普通证" ? "" : dataRow["IsWhiteFlag"].ToString().Trim();
            //iswhiteflage = iswhiteflage== "普通证" ? "" : iswhiteflage;
            string orderno = dataRow["OrderNO"].ToString().Trim();
            string color = dataRow["Cdefine3"].ToString().Trim();
            ///下面是计划的
            string pmaterialcode = dataRow_2["MaterialCode"].ToString().Trim();
            string pgrade = dataRow_2["Grade"].ToString().Trim();
            string pspecification = dataRow_2["Specification"].ToString().Trim();
            string pwidthr = Utils.WMSMessage.TrimEndZero(dataRow_2["Width_R"].ToString()).Trim();
            pwidthr = pwidthr == "0" ? "" : pwidthr;
            string pwidthp = Utils.WMSMessage.TrimEndZero(dataRow_2["Width_P"].ToString()).Trim();
            pwidthp = pwidthp == "0" ? "" : pwidthp;
            string plengthp = Utils.WMSMessage.TrimEndZero(dataRow_2["Length_P"].ToString()).Trim();
            plengthp = plengthp == "0" ? "" : plengthp;
            string pweightmode = dataRow_2["WeightMode"].ToString().Trim();
            //(纸芯/令数、直径/令张数、线长/件张数)
            string pcore = "", pdiameter = "", plength = "";

            // 纸芯/令数、直径/令张数、线长/件张数  都保存在分录的以下三个字段，将错就错
            pcore = Utils.WMSMessage.TrimEndZero(dataRow_2["CoreDiameter"].ToString().Trim());
            pcore = pcore == "0" ? "" : pcore;
            pdiameter = Utils.WMSMessage.TrimEndZero(dataRow_2["Diameter"].ToString().Trim());
            pdiameter = pdiameter == "0" ? "" : pdiameter;
            plength = Utils.WMSMessage.TrimEndZero(dataRow_2["RollLength"].ToString().Trim());
            plength = plength == "0" ? "" : plength;


            string preampack = dataRow_2["ReamPackType"].ToString().Trim();
            string psku = dataRow_2["SKU"].ToString().Trim();
            string pcusttrademark = dataRow_2["Remark"].ToString().Trim();//特殊客户 来自tose
            string ppapercsert = dataRow_2["PaperCert"].ToString().Trim();
            string pspeccust = dataRow_2["SpecCustName"].ToString().Trim();
            string pspecprod = dataRow_2["SpecProdName"].ToString().Trim();
            string ptrademarkstyle = dataRow_2["TrademarkStyle"].ToString().Trim();//夹板包装
            string piswhiteflage = dataRow_2["IsWhiteFlag"].ToString().Trim();
            string porderno = dataRow_2["OrderNO"].ToString().Trim();
            string pcolor = dataRow_2["Cdefine3"].ToString().Trim();//色相
            ///匹配
            const string match = "match";
            //不匹配
            const string unmatch = "unmatch";
            //忽略
            const string ignore = "ignore";
            //{widthr,pwidthr,match},{widthp,pwidthp,match},{lengthp,plengthp,match},
            //string[,] properties = new string[17, 3]{
            //{pmaterialcode==""?"":materialcode,pmaterialcode,match},
            //{pgrade==""?"":grade,pgrade,match},
            //{pspecification==""?"":specification,pspecification,match},
            //{pweightmode==""?"":weightmode,pweightmode,match},
            //{pcore==""||pcore=="0"?"":core,pcore,match},
            //{pream==""||pream=="0"?"":ream,pream,match},
            //{pslidesream==""||pslidesream=="0"?"":slidesream,pslidesream,match},
            //{pslidesheet==""||pslidesheet=="0"?"":slidesheet,pslidesheet,match},
            //{preampack==""?"":reampack,preampack,match},
            //{pcusttrademark==""?"":custtrademark,pcusttrademark,match},
            //{ppapercsert==""?"":papercsert,ppapercsert,match},
            //{pspeccust==""?"":speccust,pspeccust,match},
            //{pspecprod==""?"":specprod,pspecprod,match},
            //{ptrademarkstyle==""?"":trademarkstyle,ptrademarkstyle,match},
            //{piswhiteflage==""?"":iswhiteflage,piswhiteflage,match},
            //{porderno==""?"":orderno,porderno,match},
            //{pcolor==""?"":color,pcolor,match}
            //};

            string[,] properties = new string[16, 3]{
            {materialcode,pmaterialcode,match},
            {grade,pgrade,match},
            {specification,pspecification,match},
            {weightmode,pweightmode,match},
            {pcore=="0"||pcore ==""?"":core,pcore,match},
            {diameter=="0"||diameter==""?"":diameter,pdiameter,match},
            {length=="0"||length==""?"":length,plength,match},
            {reampack,preampack,match},
            {custtrademark,pcusttrademark,match},
            {papercsert,ppapercsert,match},
            {speccust,pspeccust,match},
            {specprod,pspecprod,match},
            {trademarkstyle,ptrademarkstyle,match},
            {iswhiteflage,piswhiteflage,match},
            {orderno,porderno,match},
            {color,pcolor,match}
            };

            //
            string result = "";
            for (int i = 0; i < (properties.Length / 3); i++)
            {
                string prod = properties[i, 0];
                string plan = properties[i, 1];
                string type = properties[i, 2];

                if (type == match)
                {
                    if (prod != plan)
                    {
                        result = prod + "≠" + plan;
                        break;
                    }
                }
                else if (type == ignore)
                {


                }
                else if (unmatch == type)
                {

                    if (prod == plan)
                    {
                        result = prod + "=" + plan;
                        break;
                    }
                }

            }
            if (result == "")
            {
                planId = dataRow_2["EntryID"].ToString().Trim();
                voucherId = dataRow_2["VoucherID"].ToString().Trim();
                sourceEntryID = dataRow_2["SourceEntryID"].ToString().Trim();
                sourceVoucherID = dataRow_2["SourceVoucherID"].ToString().Trim();
                //属性匹配通过，判断个数和重量有没有超出计划,,是否超出通知单
                decimal prodWeight = Convert.ToDecimal(dataRow["WeightLabel"].ToString()) / 1000;
                DataSet planDS = this._WMSAccess.Select_OutStock_Situation(voucherId, planId);
                //出库分录已提交数量
                decimal outentrycommitweight = Convert.ToDecimal(planDS.Tables[0].Rows[0]["OutEntryCommitAuxQty"].ToString() == "" ? "0" : planDS.Tables[0].Rows[0]["OutEntryCommitAuxQty"].ToString());
                decimal outentrycommitcount = Convert.ToDecimal(planDS.Tables[0].Rows[0]["OutEntryCommitQty"].ToString() == "" ? "0" : planDS.Tables[0].Rows[0]["OutEntryCommitQty"].ToString());

                //出库分录计划数量
                decimal outentryweight = Convert.ToDecimal(planDS.Tables[0].Rows[0]["OutEntryAuxQty"].ToString() == "" ? "0" : planDS.Tables[0].Rows[0]["OutEntryAuxQty"].ToString());
                decimal outentrycount = Convert.ToDecimal(planDS.Tables[0].Rows[0]["OutEntryQty"].ToString() == "" ? "0" : planDS.Tables[0].Rows[0]["OutEntryQty"].ToString());

                //通知分录已提交数量
                decimal planentryweight = Convert.ToDecimal(planDS.Tables[0].Rows[0]["PlanEntryCommitQty"].ToString() == "" ? "0" : planDS.Tables[0].Rows[0]["PlanEntryCommitQty"].ToString());
                decimal planentrycount = Convert.ToDecimal(planDS.Tables[0].Rows[0]["PlanEntryQty"].ToString() == "" ? "0" : planDS.Tables[0].Rows[0]["PlanEntryQty"].ToString());

                //出库当前
                decimal outc = outentrycommitcount + 1;
                decimal outw = outentrycommitweight + prodWeight;
                //通知当前
                decimal aoutc = planentrycount + 1;
                decimal aoutw = planentryweight + prodWeight;
                if (forceOut)
                {
                    //强制通过就不匹配重量和数量了
                    planCountExec = Utils.WMSMessage.TrimEndZero(outc.ToString()) + "/" + Utils.WMSMessage.TrimEndZero(outentrycount.ToString());
                    planWeightExec = Utils.WMSMessage.TrimEndZero((outw).ToString()) + "/" + Utils.WMSMessage.TrimEndZero(outentryweight.ToString());
                    allCountExec = Utils.WMSMessage.TrimEndZero((aoutc).ToString());// Utils.WMSMessage.TrimEndZero(aoutc.ToString()) + "/" + Utils.WMSMessage.TrimEndZero(pcommitcount.ToString());
                    allWeightExec = Utils.WMSMessage.TrimEndZero((aoutw).ToString());// Utils.WMSMessage.TrimEndZero((aoutw).ToString()) + "/" + Utils.WMSMessage.TrimEndZero(pcommitweight.ToString());

                }
                else
                {
                    //查询计划完成情况，重量   优先判断

                    if (outw > outentryweight)
                    {
                        result += "A:计划重量将会超出" + Utils.WMSMessage.TrimEndZero(outentryweight.ToString()) + "吨；";
                    }
                    if (outc > outentrycount)
                    {
                        result += "B:计划件数将会超出" + Utils.WMSMessage.TrimEndZero(outentrycount.ToString()) + "件；";
                    }
                    //if ( aoutc> pcommitcount)
                    //{
                    //    result += "-总件数将会超出" + Utils.WMSMessage.TrimEndZero(pcommitcount.ToString() ) + "件";
                    //}
                    //if (aoutw > pcommitweight)
                    //{
                    //    result += "-总重量将会超出" + Utils.WMSMessage.TrimEndZero(pcommitweight.ToString()) + "吨";
                    //}
                    if (result != "")
                    {
                        result += "是否继续出库？";
                    }
                    else
                    {
                        //完全通过
                        planCountExec = Utils.WMSMessage.TrimEndZero(outc.ToString()) + "/" + Utils.WMSMessage.TrimEndZero(outentrycount.ToString());
                        planWeightExec = Utils.WMSMessage.TrimEndZero((outw).ToString()) + "/" + Utils.WMSMessage.TrimEndZero(outentryweight.ToString());
                        allCountExec = Utils.WMSMessage.TrimEndZero((aoutc).ToString());// Utils.WMSMessage.TrimEndZero(aoutc.ToString()) + "/" + Utils.WMSMessage.TrimEndZero(pcommitcount.ToString());
                        allWeightExec = Utils.WMSMessage.TrimEndZero((aoutw).ToString());// Utils.WMSMessage.TrimEndZero((aoutw).ToString()) + "/" + Utils.WMSMessage.TrimEndZero(pcommitweight.ToString());
                    }
                }
            }
            else
            {
                //属性匹配不通过
                planId = "";
                voucherId = "";
                //result = "没有找到符合的计划";
            }
            return result;
        }
        /// <summary>
        /// 加载本地用户
        /// </summary>
        /// <returns></returns>
        public string LoadWHUser()
        {
            string retMsg = "";
            WMSDS userDS = this._WMSAccess.Select_T_User("");
            string user = "";
            if (userDS.T_User.Rows.Count > 0)
            {
                for (int i = 0; i < userDS.T_User.Rows.Count; i++)
                {
                    user += userDS.T_User.Rows[i][userDS.T_User.UserCodeColumn].ToString() + "." + userDS.T_User.Rows[i][userDS.T_User.UserNameColumn].ToString() + Utils.WMSMessage._ForeachChar;
                }
                user = user.TrimEnd(Utils.WMSMessage._ForeachChar);
                string[] paperStrs = new string[] { "QA13", "0", "人员刷新成功", user };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            else
            {
                //发运单不存在，不能出库。
                retMsg = "没有人员信息";
                string[] paperStrs = new string[] { "QA13", "9", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }


            return retMsg;
        }

        public string LoadWHShift()
        {
            string retMsg = "";
            WMSDS userDS = this._WMSAccess.Select_T_Shift("");
            string user = "";
            if (userDS.T_Shift.Rows.Count > 0)
            {
                for (int i = 0; i < userDS.T_Shift.Rows.Count; i++)
                {
                    user += userDS.T_Shift.Rows[i][userDS.T_Shift.ShiftNameColumn].ToString() + Utils.WMSMessage._ForeachChar;
                }
                user = user.TrimEnd(Utils.WMSMessage._ForeachChar);
                string[] paperStrs = new string[] { "QA02", "0", "班组刷新成功", user };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            else
            {
                //发运单不存在，不能出库。
                retMsg = "没有班组信息";
                string[] paperStrs = new string[] { "QA02", "9", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }


            return retMsg;
        }
        /// <summary>
        /// 加载业务类型
        /// </summary>
        /// <returns></returns>
        public string LoadWHBusinessType(string type)
        {
            string retMsg = "";
            WMSDS userDS = this._WMSAccess.Select_T_BusinessType(type, "");
            string user = "";
            if (userDS.T_Business_Type.Rows.Count > 0)
            {
                for (int i = 0; i < userDS.T_Business_Type.Rows.Count; i++)
                {
                    user += userDS.T_Business_Type.Rows[i][userDS.T_Business_Type.BusinessCodeColumn].ToString() + "." + userDS.T_Business_Type.Rows[i][userDS.T_Business_Type.BusinessNameColumn].ToString() + Utils.WMSMessage._ForeachChar;
                }
                user = user.TrimEnd(Utils.WMSMessage._ForeachChar);
                string[] paperStrs = new string[] { "QA06", "0", "业务类型刷新成功", user };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            else
            {
                //发运单不存在，不能出库。
                retMsg = "没有业务类型信息";
                string[] paperStrs = new string[] { "QA06", "9", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }


            return retMsg;
        }
        /// <summary>
        /// 加载本地机台
        /// </summary>
        /// <returns></returns>
        public string LoadWHMachine()
        {
            string retMsg = "";
            WMSDS userDS = this._WMSAccess.Select_T_Factory(true, false);
            string user = "";
            if (userDS.T_Factory.Rows.Count > 0)
            {
                for (int i = 0; i < userDS.T_Factory.Rows.Count; i++)
                {
                    user += userDS.T_Factory.Rows[i][userDS.T_Factory.MachineIDColumn].ToString() + Utils.WMSMessage._ForeachChar;
                }
                user = user.TrimEnd(Utils.WMSMessage._ForeachChar);
                string[] paperStrs = new string[] { "QA12", "0", "机台刷新成功", user };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            else
            {
                //发运单不存在，不能出库。
                retMsg = "没有机台信息";
                string[] paperStrs = new string[] { "QA12", "9", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }


            return retMsg;
        }
        /// <summary>
        /// 平板入库函数
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_IP02(string[] strs)
        {
            {
                try
                {
                    string retMsg = "";
                    string productid = strs[2];
                    string inDate = strs[3];//交班时间
                    string business = strs[4];
                    string sourceVoucher = strs[5];
                    string factory = strs[6];
                    string whcode = strs[7];
                    string biller = strs[8];
                    string warehouse = strs[9];
                    string shift = strs[10];
                    string shiftTime = strs[11];
                    string remark = strs[12];
                    string iswhite = strs[13];
                    //string trademode = strs[14];
                    //string factory1 = productid.Substring(0, 2).ToString();//根据条码编号查找组织代码

                    string isCanIn = this.StatusCheckIR02(productid, "IPA02");// this.CheckCanScanIn(productid);
                    string factoryCode = "";
                    //通过工厂代码查询ERP的组织
                    WMSDS fds = this._WMSAccess.Select_T_Factory(true, factory);
                    if (fds.T_Factory.Rows.Count > 0)
                    {
                        factoryCode = fds.T_Factory.Rows[0][fds.T_Factory.FactoryAbbrColumn].ToString();
                    }
                    else
                    {
                        string[] factoryStrs = new string[] { "IPA02", "9", " 条码前两位表示的包装机台不存在!" };
                        isCanIn = Utils.WMSMessage.MakeWMSSocketMsg(factoryStrs);
                    }
                    if (isCanIn == "")
                    {
                        //初次入库从某个机台的rollproduct中是否能查的到，
                        ProduceDS sourceDS = _WMSAccess.Sheet_ProductQueryAllByFK(productid, factory);
                        if (sourceDS.Sheet_Product.Rows.Count > 0)
                        {
                            //查的到就入进来，入进来的时候同时在老数据表和新数据表中插入
                            ProduceDS.Sheet_ProductRow rprow = sourceDS.Sheet_Product.Rows[0] as ProduceDS.Sheet_ProductRow;
                            WMSDS.T_Product_InRow tpirow = (new WMSDS()).T_Product_In.NewT_Product_InRow();
                            #region 平板部分
                            tpirow.SheetWidthLabel = rprow.IsSheetWidthNull() ? 0 : rprow.SheetWidth;
                            tpirow.SheetLengthLabel = rprow.IsSheetLengthNull() ? 0 : rprow.SheetLength;
                            tpirow.PalletReams = rprow.IsPalletReamsNull() ? 0 : rprow.PalletReams;

                            //tpirow.SlidesOfReam = 0;
                            //tpirow.SlidesOfSheet = 0;
                            //tpirow.SlidesOfReamPrint = rprow.IsSlidesOfReamNull() ? 0 : rprow.SlidesOfReam;
                            //tpirow.SlidesOfSheetPrint = rprow.IsSlidesOfSheetNull() ? 0 : rprow.SlidesOfSheet;

                            //修改：自动扫描入库 当为250,500时才取值，其余取0============================================================================================

                            int slidesOfReam = rprow.IsSlidesOfReamNull() ? 0 : rprow.SlidesOfReam;
                            int slidesOfSheet = rprow.IsSlidesOfSheetNull() ? 0 : rprow.SlidesOfSheet;

                            int[] allowValues = new int[] { 250, 500 };
                            if (allowValues.Contains(slidesOfReam))
                            {
                                tpirow.SlidesOfReam = 0;
                        
                            }
                            else
                            {
                                tpirow.SlidesOfReam = slidesOfReam;
                                //tpirow.SlidesOfSheet = slidesOfSheet;
                            }
                            tpirow.SlidesOfSheet = 0;
                            tpirow.SlidesOfReamPrint = slidesOfReam;
                            tpirow.SlidesOfSheetPrint = slidesOfSheet;
                            //=======================================================================================
                            //增加色相
                            tpirow.Color = rprow.IsColorNull() ? null : rprow.Color;
                            tpirow.CustTrademark = rprow.IsCustTrademarkNull() ? null : rprow.CustTrademark;//特殊客户默认为空

                            tpirow.ReamPackType = rprow.IsReamPackTypeNull() ? null : rprow.ReamPackType;
                            tpirow.FiberDirect = rprow.IsFiberDirectNull() ? null : rprow.FiberDirect;
                            tpirow.WeightMode = rprow.IsFiberDirectNull() ? null : rprow.FiberDirect;
                            tpirow.PalletRemark = rprow.IsPalletRemarkNull() ? null : rprow.PalletRemark;
                            tpirow.Specification = Utils.WMSMessage.TrimEndZero(tpirow.SheetWidthLabel.ToString()) + "-" + Utils.WMSMessage.TrimEndZero(tpirow.SheetLengthLabel.ToString());
                            tpirow.Remark = rprow.IsSheetRemarkNull() ? null : rprow.SheetRemark;
                            #endregion
                            #region 卷筒平板公共部分
                            //tpirow.ProductID = rprow.IsPalletIDNull() ? null : rprow.PalletID;
                            tpirow.ProductID = rprow.IsPalletIDNull() ? null : rprow.PalletID; // rprow.IsSheetIDNull() ? null : rprow.SheetID;
                            tpirow.ProductTypeCode = "2";
                            // tpirow.TrademarkStyle = rprow.TrademarkType;
                            // tpirow.TransportType =rprow.IsTransportTypeNull()?null: rprow.TransportType;
                            tpirow.IsPolyHook = rprow.IsPlyWookdPackNull() ? null : rprow.PlyWookdPack;//夹板包装
                            tpirow.Factory = rprow.IsFactoryIDNull() ? null : rprow.FactoryID;
                            tpirow.MachineID = rprow.IsMachineIDNull() ? null : rprow.MachineID;
                            tpirow.MaterialCode = rprow.IsMaterialCodeNull() ? null : rprow.MaterialCode;
                            // tpirow.Standard = rprow.IsStandardCodeNull() ? null : rprow.StandardCode;
                            tpirow.ProductName = rprow.IsProductNameNull() ? null : rprow.ProductName;
                            tpirow.ProductType = rprow.IsProductTypeNull() ? null : rprow.ProductType;
                            tpirow.Trademark = rprow.IsTrademarkNull() ? null : rprow.Trademark;
                            tpirow.Grade = rprow.IsPalletGradeNull() ? null : rprow.PalletGrade;
                            tpirow.BasisweightLabel = rprow.IsBasisweightNull() ? 0 : Convert.ToDecimal(rprow.Basisweight);
                            //tpirow.WhiteDegree = rprow.IsWhiteDegreeNull() ? null : rprow.WhiteDegree;
                            tpirow.WeightLabel = rprow.IsWeightLabelNull() ? 0 : Convert.ToDecimal(rprow.WeightLabel.ToString());
                            tpirow.CustCode = rprow.IsCustCodeNull() ? null : rprow.CustCode;
                            tpirow.OrderNO = rprow.IsOrderNONull() ? null : rprow.OrderNO;
                            tpirow.PaperCertCode = rprow.IsPaperCertNull() ? null : rprow.PaperCert;
                            tpirow.SpecProdName = rprow.IsSpecProdNameNull() ? null : rprow.SpecProdName;
                            tpirow.SpecCustName = rprow.IsSpecCustNameNull() ? null : rprow.SpecCustName;
                            tpirow.SKU = rprow.IsSKUNull() ? null : rprow.SKU;
                            tpirow.IsWhiteFlag = rprow.IsIsWhiteFlagNull() ? null : rprow.IsWhiteFlag;
                            tpirow.TradeMode = rprow.IsTradeModeNull() ? null : rprow.TradeMode;
                            tpirow.Cdefine1 = rprow.IsCdefine1Null() ? null : rprow.Cdefine1;
                            tpirow.Cdefine2 = rprow.IsCdefine2Null() ? null : rprow.Cdefine2;
                            tpirow.Cdefine3 = rprow.IsCdefine3Null() ? null : rprow.Cdefine3;
                            tpirow.Udefine1 = rprow.IsUdefine1Null() ? Convert.ToInt32(0) : rprow.Udefine1;
                            tpirow.Udefine2 = rprow.IsUdefine2Null() ? Convert.ToInt32(0) : rprow.Udefine2;
                            tpirow.Udefine3 = rprow.IsUdefine3Null() ? Convert.ToInt32(0) : rprow.Udefine3;
                            // tpirow.DiameterLabel = 0;
                            tpirow.MaterialName = rprow.MaterialName;
                            //  
                            //生成批次号
                            //string batchNO = this.MakeBatchNONew(factoryCode, DateTime.Now.ToString("yyyyMM"));
                            string batchNO = this.MakeBatchNONew(factoryCode, productid);
                            tpirow.BatchNO = batchNO;
                            #endregion
                            #region 仓库部分
                            tpirow.ReadDate = DateTime.Now;
                            tpirow.BusinessType = business;
                            tpirow.SourceVoucher = sourceVoucher;
                            tpirow.Warehouse = whcode;
                            tpirow.WHPosition = factory;// warehouse;
                            tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
                            tpirow.InShift = shift;
                            tpirow.InShiftTime = shiftTime;
                            tpirow.InUser = biller;
                            tpirow.WHRemark = remark;
                            #endregion
                            #region 状态部分
                            tpirow.IsDeleted = false;
                            tpirow.StatusIn = 1;
                            tpirow.StatusOut = 0;
                            tpirow.SourcePID = 0;
                            tpirow.VoucherInID = 0;
                            tpirow.VoucherOutID = 0;
                            #endregion

                            //插入T_Product_In和productlife
                            WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
                            tplrow.Operate = Utils.WMSOperate._OperScanIn;
                            tplrow.OperDate = DateTime.Now;
                            tplrow.OperUser = biller;
                            tplrow.ProductID = productid;
                            tplrow.Status = Utils.WMSOperate._StatusIn;
                            string result = this._WMSAccess.Tran_InsertProductScanIn(tpirow, tplrow);
                            if (result == "")
                            {
                                retMsg = "入库成功。";


                                //查询这个班组这个班次入库的汇总

                                string inDateS = inDate;// this.ReturnCurrentClassTime(inDate, shiftTime);
                                string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");// Convert.ToDateTime(inDateS).AddHours(13).ToString("yyyy-MM-dd HH:mm:ss");

                                //DataSet sumDS = this._WMSAccess.Product_In_SumaryByFk(inDateS, inDateE, biller, shift, shiftTime, tpirow.MaterialCode, tpirow.MachineID, tpirow.Grade, tpirow.WidthLabel.ToString(), tpirow.CoreDiameter.ToString(), tpirow.IsWhiteFlag.ToString(), tpirow.IsLayersNull() ? "" : tpirow.Layers.ToString(), tpirow.IsRollCountNull() ? "" : tpirow.RollCount.ToString(), tpirow.IsSKUNull() ? "" : tpirow.SKU, tpirow.WeightMode, tpirow.IsOrderNONull() ? "" : tpirow.OrderNO, tpirow.IsWHRemarkNull() ? "" : tpirow.WHRemark, tpirow.TradeMode);
                                DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO, "1");
                                if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
                                {
                                    //string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
                                    //string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();

                                    string sum = "0";
                                    string count = "1";

                                    DataRow currRow = GetGroupRowByProductInRow(sumDS.Tables["T_Product_In"], tpirow);
                                    if (currRow == null)
                                    {
                                        sum = "0";
                                        count = "1";
                                    }
                                    else
                                    {
                                        sum = currRow["WeightSum"].ToString();
                                        count = currRow["IdCount"].ToString();
                                    }


                                    //组合平板的信息
                                    //IPA02|0|msg|materialName|grade|Specification|reams|slidesream|slidesheet|weight|count|fsc|fiberdirect|sku|specprod|speccust|custBrand|orderno|remark
                                    string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                    string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabelPrint.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheetPrint.ToString());
                                    string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabelPrint.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReamPrint.ToString());
                                    string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : "";// tpirow.FiberDirect;
                                    //string strRemark = tpirow.ProductTypeCode == "1" ? (tpirow.IsRemarkNull() ? "" : tpirow.Remark) : (tpirow.IsPalletRemarkNull() ? "" : tpirow.PalletRemark);
                                    string[] paperStrs = new string[]{
                        //  "IPA02","0"," 入库成功", tpirow.ProductID,tpirow.BatchNO, tpirow.MaterialName, tpirow.Grade, tpirow.Specification,Utils.WMSMessage.TrimEndZero( tpirow.PalletReams.ToString()), 
                        //Utils.WMSMessage.TrimEndZero(  tpirow.SlidesOfReam.ToString()),Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString()),Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),
                        //count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(), tpirow.FiberDirect, tpirow.IsSKUNull()?"": tpirow.SKU,
                        // tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName, tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,
                        // tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark
                            "IPA02","0","入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, Utils.WMSMessage.TrimEndZero(tpirow.IsDiameterLabelNull()?"": tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(), tpirow.IsWeightModeNull()?"":tpirow.WeightMode, tpirow.IsSlidesOfReamPrintNull()?"":tpirow.SlidesOfReamPrint.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull() ? "" : tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),diameterReamSheet,
                                    tpirow.IsColorNull()?"":tpirow.Color,"",""//新增色相，计划线长，计划直径

                           };
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                }
                            }
                            else
                            {
                                string[] paperStrs = new string[] { "IPA02", "9", " 保存入库数据失败:", result };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }
                        }

                        else
                        {
                            //查不到就报错
                            string[] paperStrs = new string[] { "IPA02", "9", "生产数据中查不到条码信息！" };
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);


                        }
                    }
                    else
                    {
                        retMsg = isCanIn;
                    }
                    return retMsg;
                }
                catch (Exception ex)
                {
                    string[] paperStrs = new string[] { "IPA02", "9", "入库异常" + ex.Message };
                    return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }

            }
        }

        /// <summary>
        /// 从当前统计行中匹配对应的统计行
        /// </summary>
        /// <param name="dtProductIn">带统计的记录</param>
        /// <param name="currPoductDataRow">当前扫描的纸卷信息</param>
        /// <returns>返回统计行</returns>
        private DataRow GetGroupRowByProductInRow(DataTable dtProductIn, WMSDS.T_Product_InRow currPoductDataRow)
        {
            string strCondition = "1=1 ";

            try
            {
                strCondition += string.Format("AND {0}='{1}' ", "MaterialCode", currPoductDataRow.MaterialCode);


                if (!currPoductDataRow.IsSpecificationNull())
                    strCondition += string.Format("AND {0}='{1}' ", "Specification", currPoductDataRow.Specification);
                if (!currPoductDataRow.IsGradeNull())
                    strCondition += string.Format("AND {0}='{1}' ", "Grade", currPoductDataRow.Grade);
                if (!currPoductDataRow.IsDiameterLabelNull())
                    strCondition += string.Format("AND {0}='{1}' ", "DiameterLabel", currPoductDataRow.DiameterLabel);

                if (!currPoductDataRow.IsLengthLabelNull())
                    strCondition += string.Format("AND {0}={1} ", "LengthLabel", currPoductDataRow.LengthLabel);

                if (!currPoductDataRow.IsWeightModeNull())
                    strCondition += string.Format("AND {0}='{1}' ", "WeightMode", currPoductDataRow.WeightMode);

                if (!currPoductDataRow.IsCoreDiameterNull())
                    strCondition += string.Format("AND {0}={1} ", "CoreDiameter", currPoductDataRow.CoreDiameter);

                if (!currPoductDataRow.IsOrderNONull())
                    strCondition += string.Format("AND {0}='{1}' ", "OrderNO", currPoductDataRow.OrderNO);

                if (!currPoductDataRow.IsCustTrademarkNull())
                    strCondition += string.Format("AND {0}='{1}' ", "CustTrademark", currPoductDataRow.CustTrademark);

                if (!currPoductDataRow.IsPaperCertCodeNull())
                    strCondition += string.Format("AND {0}='{1}' ", "PaperCertCode", currPoductDataRow.PaperCertCode);

                if (!currPoductDataRow.IsSpecProdNameNull())
                    strCondition += string.Format("AND {0}='{1}' ", "SpecProdName", currPoductDataRow.SpecProdName);

                if (!currPoductDataRow.IsSpecCustNameNull())
                    strCondition += string.Format("AND {0}='{1}' ", "SpecCustName", currPoductDataRow.SpecCustName);

                if (!currPoductDataRow.IsIsWhiteFlagNull())
                    strCondition += string.Format("AND {0}='{1}' ", "IsWhiteFlag", currPoductDataRow.IsWhiteFlag);

                if (!currPoductDataRow.IsLayersNull())
                    strCondition += string.Format("AND {0}={1} ", "Layers", currPoductDataRow.Layers);

                if (!currPoductDataRow.IsSKUNull())
                    strCondition += string.Format("AND {0}='{1}' ", "SKU", currPoductDataRow.SKU);

                if (!currPoductDataRow.IsPalletReamsNull())
                    strCondition += string.Format("AND {0}={1} ", "PalletReams", currPoductDataRow.PalletReams);

                if (!currPoductDataRow.IsSlidesOfReamNull())
                    strCondition += string.Format("AND {0}={1} ", "SlidesOfReam", currPoductDataRow.SlidesOfReam);

                if (!currPoductDataRow.IsSlidesOfSheetNull())
                    strCondition += string.Format("AND {0}={1} ", "SlidesOfSheet", currPoductDataRow.SlidesOfSheet);

                if (!currPoductDataRow.IsReamPackTypeNull())
                    strCondition += string.Format("AND {0}='{1}' ", "ReamPackType", currPoductDataRow.ReamPackType);


                if (!currPoductDataRow.IsIsPolyHookNull())
                    strCondition += string.Format("AND {0}='{1}' ", "IsPolyHook", currPoductDataRow.IsPolyHook);

                if (!currPoductDataRow.IsFiberDirectNull())
                    strCondition += string.Format("AND {0}='{1}' ", "FiberDirect", currPoductDataRow.FiberDirect);
                if (!currPoductDataRow.IsBatchNONull())
                    strCondition += string.Format("AND {0}='{1}' ", "BatchNO", currPoductDataRow.BatchNO);



                DataRow[] currRows = dtProductIn.Select(strCondition);
                if (currRows.Length > 0)
                {
                    return currRows[0];
                }
                else
                    return null;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string Process_IP03(string[] strs)
        {
            return this.Process_IR03(strs);
            //try
            //{
            //    string retMsg = "";
            //    string productid = strs[2];
            //    string user = strs[3];
            //    //先判断一个纸能否取消入库
            //    string canCancel = this.CheckCanCancelScanIn(productid);

            //    if (canCancel == "")
            //    {
            //        //先查询出这个这个条码在product in中最后一个的onlyid
            //        //WMSDS tpiDS = this._WMSAccess.T_Product_InQuery(productid);

            //        DataSet tpiDS = _WMSAccess.Select_T_ProductLifeByProductID(productid);

            //        if (tpiDS.Tables.Count>0&&tpiDS.Tables["T_ProductLife"].Rows.Count > 0)
            //        {
            //            int pid = Convert.ToInt32(tpiDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"]);
            //            //把它更新为不入库的状态
            //            WMSDS.T_ProductLifeRow tpfRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
            //            tpfRow.ProductOnlyID = pid;
            //            tpfRow.ProductID = productid;
            //            tpfRow.OperUser = user;
            //            tpfRow.OperDate = DateTime.Now;
            //            tpfRow.Operate = Utils.WMSOperate._OperScanInCancel;
            //            tpfRow.Status = Utils.WMSOperate._StatusCancelIn;
            //            WMSDS.T_Product_InRow tpiRow = new WMSDS().T_Product_In.NewT_Product_InRow();
            //            tpiRow.OnlyID = tpfRow.ProductOnlyID;
            //            tpiRow.StatusIn = 0;
            //            tpiRow.VoucherInID = 0;

            //            string result = this._WMSAccess.Tran_Update_ProductScanInForCancel(tpiRow, tpfRow);
            //            if (result == "")
            //            {
            //                retMsg = "产品取消入库成功";
            //                string[] paperStrs = new string[]{
            //                "IPA03","0",retMsg
            //                 };
            //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //            }
            //            else
            //            {
            //                retMsg = "产品取消入库失败:" + result;
            //                string[] paperStrs = new string[]{
            //                "IPA03","9",retMsg
            //                 };
            //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //            }
            //        }
            //        else
            //        {
            //            retMsg = "产品取消入库失败，没有找到这个条码的记录";
            //            string[] paperStrs = new string[]{
            //                "IPA03","9",retMsg
            //                 };
            //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //        }
            //    }
            //    else
            //    {
            //        retMsg = canCancel;
            //    }
            //    return retMsg;
            //}
            //catch (Exception ex)
            //{
            //    string[] paperStrs = new string[]{
            //              "IPA03","9","取消入库异常："+ex.Message
            //               };
            //    return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //}
        }
        /// <summary>
        /// 取消出库
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_O08(string[] strs)
        {
            string retMsg = "";
            string productid = strs[2];
            string inDate = strs[4];
            string voucherno = strs[3];
            string command = "OA01";
            //检查productlife状态
            DataSet lifeDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
            if (lifeDS.Tables.Count > 0 && lifeDS.Tables["T_ProductLife"].Rows.Count > 0)
            {
                string status = lifeDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = lifeDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                string productonlyid = lifeDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();//life执行的那个id
                string sourceID = lifeDS.Tables["T_ProductLife"].Rows[0]["SourcePID"].ToString();//执行ID对应的源ID


                //看这个条码的出库单是否已关闭
                WMSDS ds = this._WMSAccess.Select_T_Product_InAndT_OutStock(productonlyid);
                if (ds.T_OutStock.Rows.Count == 0)
                {
                    //还没有做出库单
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做出库单，不能取消出库！" });
                }
                else
                {
                    //看这个出库单是红单还是蓝单
                    string rb = ds.T_OutStock.Rows[0][ds.T_OutStock.BillTypeColumn].ToString();
                    if (rb == "R")
                    {
                        //retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，且出库单已关闭，不能取消出库！请做退货单" });
                        retMsg = this.Process_O21(strs);
                    }
                    //如果是蓝单就可以开始判断
                    else
                    {
                        switch (status)
                        {
                            case Utils.WMSOperate._StatusIn:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未出库，不能取消出库！" });
                                break;
                            case Utils.WMSOperate._StatusCancelIn:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未入库，不能取消出库！" });
                                break;
                            case Utils.WMSOperate._StatusRedIn:
                            case Utils.WMSOperate._StatusCancelRedIn:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未出库，不能取消出库！" });
                                break;
                            case Utils.WMSOperate._StatusOut:
                                //看这个条码的出库单是否已关闭
                                //string close = ds.T_OutStock.Rows[0][ds.T_OutStock.IsCloseColumn].ToString();
                                //if (close == "1")
                                //{
                                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，且出库单已关闭，不能取消出库！请做退货单" });

                                //}
                                //如果未关闭就可以取消出库
                                //else
                                //{
                                //看出库单是否存在

                                WMSDS osDS = this._WMSAccess.Select_T_OutStockByFK(voucherno, "", "", "", "", "", "", -1, -1, -1);
                                if (osDS.T_OutStock.Rows.Count > 0)
                                {
                                    {
                                        //看条形码是否在出库单已出库明细里面
                                        DataSet pDS = this._WMSAccess.Select_T_OutStock_Product(voucherno, productid);
                                        if (pDS.Tables["T_OutStock_Product"].Rows.Count > 0)
                                        {
                                            //可以取消
                                            string voucherid = pDS.Tables["T_OutStock_Product"].Rows[0]["VoucherID"].ToString();
                                            string entryid = pDS.Tables["T_OutStock_Product"].Rows[0]["EntryID"].ToString();
                                            string sourcevoucherid = pDS.Tables["T_OutStock_Product"].Rows[0]["SourceVoucherID"].ToString();
                                            string sourceentryid = pDS.Tables["T_OutStock_Product"].Rows[0]["SourceEntryID"].ToString();
                                            productonlyid = pDS.Tables["T_OutStock_Product"].Rows[0]["OnlyID"].ToString();
                                            string prodweight = pDS.Tables["T_OutStock_Product"].Rows[0]["WeightLabel"].ToString();
                                            string outweight = pDS.Tables["T_OutStock_Product"].Rows[0]["PlanCommitAuxQty1"].ToString();
                                            string planweight = pDS.Tables["T_OutStock_Product"].Rows[0]["SourcePlanCommitAuxQty1"].ToString();
                                            string outcount = pDS.Tables["T_OutStock_Product"].Rows[0]["PlanCommitQty"].ToString();
                                            string plancount = pDS.Tables["T_OutStock_Product"].Rows[0]["SourcePlanCommitQty"].ToString();
                                            sourceID = pDS.Tables["T_OutStock_Product"].Rows[0]["SourcePID"].ToString();

                                            //把它更新生涯表为出库的状态
                                            WMSDS.T_ProductLifeRow lifeRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                                            lifeRow.ProductOnlyID = Convert.ToInt32(productonlyid);
                                            lifeRow.ProductID = productid;
                                            lifeRow.OperUser = "";
                                            lifeRow.OperDate = DateTime.Now;
                                            lifeRow.Operate = Utils.WMSOperate._OperScanOutCancel;
                                            lifeRow.Status = Utils.WMSOperate._StatusCancelOut;
                                            //更新入库记录表
                                            WMSDS.T_Product_InRow outRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                            outRow.OnlyID = lifeRow.ProductOnlyID;
                                            outRow.StatusOut = 0;
                                            //tpiRow.VoucherOutID = 0;//; Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                            //更新入库记录表
                                            WMSDS.T_Product_InRow inRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                            inRow.OnlyID = Convert.ToInt32(sourceID);
                                            inRow.StatusOut = 0;
                                            //删除出库与产品关联表
                                            WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                                            osppRow.EntryID = Convert.ToInt32(entryid);
                                            osppRow.ProductOnlyID = Convert.ToInt32(productonlyid);
                                            osppRow.VoucherID = Convert.ToInt32(voucherid);
                                            //更新出库提交数量
                                            WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                                            oseRow.VoucherID = osppRow.VoucherID;
                                            oseRow.EntryID = osppRow.EntryID;
                                            oseRow.PlanCommitQty = Convert.ToDecimal(outcount == "" ? "0" : outcount) - 1;
                                            oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(outweight == "" ? "0" : outweight) - Convert.ToDecimal(prodweight == "" ? "0" : prodweight) / 1000;
                                            //更新发货通知单的提交数量
                                            WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                                            ospeRow.VoucherID = Convert.ToInt32(sourcevoucherid == "" ? "0" : sourcevoucherid);
                                            ospeRow.EntryID = Convert.ToInt32(sourceentryid == "" ? "0" : sourceentryid);
                                            ospeRow.PlanCommitQty = Convert.ToDecimal(plancount == "" ? "0" : plancount) - 1;
                                            ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(planweight == "" ? "0" : planweight) - Convert.ToDecimal(prodweight == "" ? "0" : prodweight) / 1000;


                                            //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                                            //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                                            //ospRow.OnlyID = osppRow.VoucherID;
                                            //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                                            //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                                            string outresult = this._WMSAccess.Tran_ProductScanOutCancel(inRow, outRow, lifeRow, osppRow, oseRow, ospeRow);

                                            //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                                            if (outresult == "")
                                            {
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "0", "取消出库成功" });
                                            }
                                            else
                                            {
                                                //出库失败 +纸卷信息
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "9", "取消出库失败：" + outresult });
                                            }

                                        }
                                        else
                                        {
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "9", "产品不在这个出库单中，不能取消出库。" });
                                        }
                                    }

                                }
                                else
                                {
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "9", "没有找到出库单信息。" });

                                }
                                //}
                                break;
                            case Utils.WMSOperate._StatusCancelOut:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已取消出库，不能再次取消出库！" });
                                break;
                            case Utils.WMSOperate._StatusRedOut:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已做红单出库，不能取消出库！" });
                                break;

                            case Utils.WMSOperate._StatusCancelRedOut:
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已做出库，且出库单已关闭，不能取消出库！" });
                                break;

                        }

                    }
                }
            }
            else
            {
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未入库，不能取消出库！" });

            }
            return retMsg;
        }

        private string Process_O21(string[] strs)
        {
            string retMsg = "";
            string productid = strs[2];
            string inDate = strs[4];
            string voucherno = strs[3];
            string command = "OA01";
            //检查productlife状态
            DataSet lifeDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
            if (lifeDS.Tables.Count > 0 && lifeDS.Tables["T_ProductLife"].Rows.Count > 0)
            {
                string status = lifeDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = lifeDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                string productonlyid = lifeDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();//life执行的那个id
                string sourceID = lifeDS.Tables["T_ProductLife"].Rows[0]["SourcePID"].ToString();//执行ID对应的源ID
                //看这个条码的出库单是否已关闭
                WMSDS ds = this._WMSAccess.Select_T_Product_InAndT_OutStock(productonlyid);
                if (ds.T_OutStock.Rows.Count == 0)
                {
                    //还没有做出库单
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单出库单，不能取消红单出库！" });
                }
                else
                {

                    switch (status)
                    {
                        case Utils.WMSOperate._StatusIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单出库，不能取消红单出库！" });
                            break;
                        case Utils.WMSOperate._StatusCancelIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未入库，不能取消红单出库！" });
                            break;
                        case Utils.WMSOperate._StatusRedIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已做红单入库，不在库存中，不能取消红单出库！" });
                            break;
                        case Utils.WMSOperate._StatusCancelRedIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单出库，不能取消红单出库！" });
                            break;
                        case Utils.WMSOperate._StatusOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，不能取消红单出库！" });

                            break;
                        case Utils.WMSOperate._StatusCancelOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单出库，不能取消红单出库！" });

                            break;
                        case Utils.WMSOperate._StatusCancelRedOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已取消红单出库，不能再次取消红单出库！" });

                            break;
                        case Utils.WMSOperate._StatusRedOut:
                            //keyi 
                            string isclose = ds.T_OutStock.Rows[0][ds.T_OutStock.IsCloseColumn].ToString();
                            string vno = ds.T_OutStock.Rows[0][ds.T_OutStock.VoucherNOColumn].ToString();
                            if (isclose == "1")
                            {
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已在红单出库单" + vno + "中，且已关闭，不能取消红单出库！" });
                            }
                            else
                            {
                                //看出库单是否存在
                                WMSDS osDS = this._WMSAccess.Select_T_OutStockByFK(voucherno, "", "", "", "", "", "", -1, -1, -1);
                                if (osDS.T_OutStock.Rows.Count > 0)
                                {
                                    {
                                        //看条形码是否在出库单已出库明细里面
                                        DataSet pDS = this._WMSAccess.Select_T_OutStock_Product(voucherno, productid);
                                        if (pDS.Tables["T_OutStock_Product"].Rows.Count > 0)
                                        {
                                            //可以取消
                                            string voucherid = pDS.Tables["T_OutStock_Product"].Rows[0]["VoucherID"].ToString();
                                            string entryid = pDS.Tables["T_OutStock_Product"].Rows[0]["EntryID"].ToString();
                                            string sourcevoucherid = pDS.Tables["T_OutStock_Product"].Rows[0]["SourceVoucherID"].ToString();
                                            string sourceentryid = pDS.Tables["T_OutStock_Product"].Rows[0]["SourceEntryID"].ToString();
                                            productonlyid = pDS.Tables["T_OutStock_Product"].Rows[0]["OnlyID"].ToString();
                                            string prodweight = pDS.Tables["T_OutStock_Product"].Rows[0]["WeightLabel"].ToString();
                                            string outweight = pDS.Tables["T_OutStock_Product"].Rows[0]["PlanCommitAuxQty1"].ToString();
                                            string planweight = pDS.Tables["T_OutStock_Product"].Rows[0]["SourcePlanCommitAuxQty1"].ToString();
                                            string outcount = pDS.Tables["T_OutStock_Product"].Rows[0]["PlanCommitQty"].ToString();
                                            string plancount = pDS.Tables["T_OutStock_Product"].Rows[0]["SourcePlanCommitQty"].ToString();
                                            sourceID = pDS.Tables["T_OutStock_Product"].Rows[0]["SourcePID"].ToString();

                                            //把它更新生涯表为出库的状态
                                            WMSDS.T_ProductLifeRow lifeRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
                                            lifeRow.ProductOnlyID = Convert.ToInt32(productonlyid);
                                            lifeRow.ProductID = productid;
                                            lifeRow.OperUser = "";
                                            lifeRow.OperDate = DateTime.Now;
                                            lifeRow.Operate = Utils.WMSOperate._OperScanOutRedCancel;
                                            lifeRow.Status = Utils.WMSOperate._StatusCancelRedOut;
                                            //更新入库记录表
                                            WMSDS.T_Product_InRow outRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                            outRow.OnlyID = lifeRow.ProductOnlyID;
                                            outRow.StatusOut = 0;
                                            //tpiRow.VoucherOutID = 0;//; Convert.ToInt32(machedVoucherID);//tpfRow.OnlyId;
                                            //更新入库记录表
                                            WMSDS.T_Product_InRow inRow = new WMSDS().T_Product_In.NewT_Product_InRow();
                                            inRow.OnlyID = Convert.ToInt32(sourceID);
                                            inRow.StatusOut = 1;
                                            //删除出库与产品关联表
                                            WMSDS.T_OutStock_ProductRow osppRow = new WMSDS().T_OutStock_Product.NewT_OutStock_ProductRow();
                                            osppRow.EntryID = Convert.ToInt32(entryid);
                                            osppRow.ProductOnlyID = Convert.ToInt32(productonlyid);
                                            osppRow.VoucherID = Convert.ToInt32(voucherid);
                                            //更新出库提交数量
                                            WMSDS.T_OutStock_EntryRow oseRow = new WMSDS().T_OutStock_Entry.NewT_OutStock_EntryRow();
                                            oseRow.VoucherID = osppRow.VoucherID;
                                            oseRow.EntryID = osppRow.EntryID;
                                            oseRow.PlanCommitQty = Convert.ToDecimal(outcount) - 1;
                                            oseRow.PlanCommitAuxQty1 = Convert.ToDecimal(outweight) - Convert.ToDecimal(prodweight) / 1000;
                                            //更新发货通知单的提交数量

                                            WMSDS.T_OutStock_Plan_EntryRow ospeRow = new WMSDS().T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                                            ospeRow.VoucherID = Convert.ToInt32(sourcevoucherid == "" ? "0" : sourcevoucherid);
                                            ospeRow.EntryID = Convert.ToInt32(sourceentryid == "" ? "0" : sourceentryid);
                                            ospeRow.PlanCommitQty = Convert.ToDecimal(plancount == "" ? "0" : plancount) - 1;
                                            ospeRow.PlanCommitAuxQty1 = Convert.ToDecimal(planweight == "" ? "0" : planweight) - Convert.ToDecimal(prodweight) / 1000;


                                            //更新单据提交数量,以后不再判断总数，只判断分录数有没有超出
                                            //WMSDS.T_OutStockRow ospRow = new WMSDS().T_OutStock.NewT_OutStockRow();
                                            //ospRow.OnlyID = osppRow.VoucherID;
                                            //ospRow.CommitQty = Convert.ToDecimal(allCountExec.Split('/')[0]);
                                            //ospRow.CommitAuxQty = Convert.ToDecimal(allWeightExec.Split('/')[0]);

                                            string outresult = this._WMSAccess.Tran_ProductScanOutCancel(inRow, outRow, lifeRow, osppRow, oseRow, ospeRow);

                                            //符合计划且重量和数量未超出，都通过就插入product out和product life生涯
                                            if (outresult == "")
                                            {
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "0", "取消红单出库成功" });
                                            }
                                            else
                                            {
                                                //出库失败 +纸卷信息
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "9", "取消红单出库失败：" + outresult });
                                            }

                                        }
                                        else
                                        {
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "9", "产品不在这个红单出库单中，不能取消红单出库。" });
                                        }
                                    }

                                }
                                else
                                {
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "OA08", "9", "没有找到红单出库单信息。" });

                                }
                            }
                            break;
                    }
                }
            }
            return retMsg;
        }

        public string Process_O03(string[] astrs)
        {
            return this.Process_O01(astrs, true);
        }

        public string Process_O04(string[] astrs)
        {
            return this.Process_O01(astrs, true);

        }

        public string Process_O02(string[] astrs)
        {
            return this.Process_O01(astrs, true);

        }
        /// <summary>
        /// 处理红单入库
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_IR04(string[] strs)
        {
            try
            {
                string retMsg = "";
                string productid = strs[2];
                string inDate = strs[3];
                string business = strs[4];
                string sourceVoucher = strs[5];
                string factory = strs[6];
                string whcode = strs[7];
                string biller = strs[8];
                string warehouse = strs[9];
                string shift = strs[10];
                string shiftTime = strs[11];
                string remark = strs[12];
                string custName = strs[13];
                //string isCanInRed = this.CheckCanScanInRed(productid);
                retMsg = this.StatusCheckIR04(productid, "IRA04", business, shift, shiftTime, biller, inDate);

                //if (isCanInRed == "")
                //{
                //    //红单入库从本地product_in中是否能查的到，
                //    WMSDS sourceDS = _WMSAccess.Select_T_Product_InForRedIn(productid);
                //    if (sourceDS.T_Product_In.Rows.Count > 0)
                //    {
                //        //查的到就入进来，入进来的时候同时在老数据表和新数据表中插入
                //        WMSDS.T_Product_InRow tpirow = sourceDS.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                //        int onlyid=tpirow.OnlyID;
                //        #region 仓库部分
                //        tpirow.ReadDate = DateTime.Now;
                //        tpirow.BusinessType = business;
                //        //tpirow.SourceVoucher = sourceVoucher;
                //       // tpirow.Warehouse = whcode;
                //        //tpirow.WHPosition = factory;// warehouse;
                //        tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
                //        tpirow.InShift = shift;
                //        tpirow.InShiftTime = shiftTime;
                //        tpirow.InUser = biller;
                //       // tpirow.WHRemark = remark;
                //        #endregion
                //        #region 状态部分
                //        tpirow.StatusIn = -1;
                //        tpirow.StatusOut = 0;
                //        tpirow.VoucherInID = 0;
                //        tpirow.VoucherOutID = 0;
                //        tpirow.VoucherRetrieveID = 0;
                //        tpirow.SourcePID =onlyid ;//插入红单的条码的时候是以库存中的那个条码为参考，同时保存原条码的onlyid以便对照
                //        #endregion
                //        WMSDS.T_Product_InRow sourcetpirow = sourceDS.T_Product_In.NewT_Product_InRow();
                //        sourcetpirow.OnlyID = onlyid;
                //        sourcetpirow.StatusOut = 1;

                //        //插入T_Product_In和productlife
                //        WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
                //        tplrow.Operate = Utils.WMSOperate._OperScanIn;
                //        tplrow.OperDate = DateTime.Now;
                //        tplrow.OperUser = biller;
                //        tplrow.ProductID = productid;
                //        tplrow.Status = Utils.WMSOperate._StatusRedIn;
                //        string result = this._WMSAccess.Tran_InsertUpdateProductScanInForRedIn(tpirow, tplrow,sourcetpirow);

                //        //int tpiID = this._WMSAccess.InsertT_Product_InByRow(tpirow, tplrow);
                //        if (result == "")
                //        {
                //            //retMsg = "入库成功。";
                //            //查询这个班组这个班次红单入库的汇总
                //            string inDateS = inDate;
                //            string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                //            DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO,"-1");

                //            if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
                //            {
                //                string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
                //                string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();
                //                string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                //                string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsLengthLabelNull() ? "0" : (tpirow.LengthLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                //                string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsDiameterLabelNull() ? "0" : (tpirow.DiameterLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                //                string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;

                //                //组合纸卷的信息
                //                string[] paperStrs = new string[]{
                //         // "IRA02","0","入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, tpirow.CoreDiameter.ToString(), 
                //         //Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()),tpirow.Layers.ToString(), tpirow.WeightMode,tpirow.IsPalletReamsNull()?"":tpirow.PalletReams.ToString(),
                //         // tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),tpirow.IsSlidesOfSheetNull()?"":tpirow.SlidesOfSheet.ToString(),Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),
                //         // count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),tpirow.IsDirectionNull()?"": tpirow.Direction, tpirow.IsSKUNull()?"": tpirow.SKU,
                //         //tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName, tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,
                //         //tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,
                //         //       tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                //         //       tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsTransportTypeNull()?"":tpirow.TransportType,tpirow.TradeMode
                //          "IRA04","0","红单入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                //            coreReam,lengthSheet,direct,tpirow.Layers.ToString(), tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                //            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                //             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName, 
                //             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                //             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                //                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.TradeMode,diameterReamSheet
                //           };
                //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //            }
                //        }
                //        else
                //        {
                //            //删除刚才入的product_In
                //            string[] paperStrs = new string[]{
                //          "IRA04","9"," 保存红单入库数据失败:",result
                //           };
                //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //        }
                //    }

                //    else
                //    {
                //        //查不到就报错

                //        retMsg = "库存数据中查不到条码信息！";
                //        string[] paperStrs = new string[]{
                //          "IRA04","9",retMsg
                //           };
                //        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //    }

                //}
                //else
                //{
                //    retMsg = isCanInRed;
                //}

                return retMsg;
            }
            catch (Exception ex)
            {
                string[] paperStrs = new string[]{
                          "IRA04","9","入库异常"+ex.Message
                           };
                return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }

        }

        private string StatusCheckIR04(string productid, string command, string business, string shift, string shiftTime, string biller, string inDate)
        {
            string retMsg = "";
            //先判断这个条码的life状态
            DataSet wmsDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
            if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
            {
                string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                string prodOnlyID = wmsDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();
                string sourcepid = wmsDS.Tables["T_ProductLife"].Rows[0]["SourcePID"].ToString();

                switch (status)
                {
                    case Utils.WMSOperate._StatusIn:
                        WMSDS ds = this._WMSAccess.Select_T_Product_InAndT_InStock(prodOnlyID);
                        if (ds.T_InStock.Rows.Count == 0)
                        {
                            //还没有做入库单
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做入库单，不能红单入库，请做取消入库扫描！" });
                        }
                        else
                        {
                            //看这个入库单有没有上传
                            string close = ds.T_InStock.Rows[0][ds.T_InStock.IsCloseColumn].ToString();
                            if (close == "1")
                            {
                                //已关单，已上传可以红单入库
                                {
                                    //红单入库从本地product_in中是否能查的到，
                                    WMSDS sourceDS = _WMSAccess.Select_T_Product_InForRedIn(productid);
                                    if (sourceDS.T_Product_In.Rows.Count > 0)
                                    {
                                        //查的到就入进来
                                        WMSDS.T_Product_InRow tpirow = sourceDS.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                        int onlyid = tpirow.OnlyID;
                                        #region 仓库部分
                                        tpirow.ReadDate = DateTime.Now;
                                        tpirow.BusinessType = business;
                                        //tpirow.SourceVoucher = sourceVoucher;
                                        // tpirow.Warehouse = whcode;
                                        //tpirow.WHPosition = factory;// warehouse;
                                        tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
                                        tpirow.InShift = shift;
                                        tpirow.InShiftTime = shiftTime;
                                        tpirow.InUser = biller;
                                        // tpirow.WHRemark = remark;
                                        #endregion
                                        #region 状态部分
                                        tpirow.StatusIn = -1;
                                        tpirow.StatusOut = 0;
                                        tpirow.VoucherInID = 0;
                                        tpirow.VoucherOutID = 0;
                                        tpirow.VoucherRetrieveID = 0;
                                        tpirow.SourcePID = onlyid;//插入红单的条码的时候是以库存中的那个条码为参考，同时保存原条码的onlyid以便对照
                                        #endregion
                                        WMSDS.T_Product_InRow sourcetpirow = sourceDS.T_Product_In.NewT_Product_InRow();
                                        sourcetpirow.OnlyID = onlyid;
                                        sourcetpirow.StatusOut = 1;

                                        //插入T_Product_In和productlife
                                        WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
                                        tplrow.Operate = Utils.WMSOperate._OperScanRedIn;
                                        tplrow.OperDate = DateTime.Now;
                                        tplrow.OperUser = biller;
                                        tplrow.ProductID = productid;
                                        tplrow.Status = Utils.WMSOperate._StatusRedIn;
                                        string result = this._WMSAccess.Tran_InsertUpdateProductScanInForRedIn(tpirow, tplrow, sourcetpirow);

                                        //int tpiID = this._WMSAccess.InsertT_Product_InByRow(tpirow, tplrow);
                                        if (result == "")
                                        {
                                            //retMsg = "入库成功。";
                                            //查询这个班组这个班次红单入库的汇总
                                            string inDateS = inDate;
                                            string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                                            DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO, "-1");

                                            if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
                                            {
                                                string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
                                                string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();
                                                string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                                string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsLengthLabelNull() ? "0" : (tpirow.LengthLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                                string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsDiameterLabelNull() ? "0" : (tpirow.DiameterLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                                string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;

                                                //组合纸卷的信息
                                                string[] paperStrs = new string[]{
                         // "IRA02","0","入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, tpirow.CoreDiameter.ToString(), 
                         //Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()),tpirow.Layers.ToString(), tpirow.WeightMode,tpirow.IsPalletReamsNull()?"":tpirow.PalletReams.ToString(),
                         // tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),tpirow.IsSlidesOfSheetNull()?"":tpirow.SlidesOfSheet.ToString(),Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),
                         // count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),tpirow.IsDirectionNull()?"": tpirow.Direction, tpirow.IsSKUNull()?"": tpirow.SKU,
                         //tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName, tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,
                         //tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,
                         //       tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                         //       tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsTransportTypeNull()?"":tpirow.TransportType,tpirow.TradeMode
                          "IRA04","0","红单入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, tpirow.IsDiameterLabelNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.Layers.ToString()), tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),diameterReamSheet
                           };
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                        }
                                        else
                                        {
                                            //删除刚才入的product_In
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", " 保存红单入库数据失败:" + result });
                                        }
                                    }

                                    else
                                    {
                                        //查不到就报错
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", "库存数据中查不到条码信息！" });
                                    }

                                }
                            }
                            else
                            {
                                //未关单
                                string vno = ds.T_InStock.Rows[0][ds.T_InStock.VoucherNOColumn].ToString();
                                string[] paperStrs = new string[] { command, "9", "产品已在入库单" + vno + "中，且未关闭，不能红单入库！需要先上传入库单，或者修改入库单使产品从库存中减掉，再取消入库" };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }
                        }
                        break;
                    case Utils.WMSOperate._StatusCancelIn:
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未入库，不能红单入库！" });
                        break;
                    case Utils.WMSOperate._StatusRedIn:
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已做红单入库，不能再次红单入库！" });
                        break;
                    case Utils.WMSOperate._StatusCancelRedIn:
                        {
                            //红单入库从本地product_in中是否能查的到，
                            WMSDS sourceDS = _WMSAccess.Select_T_Product_InForRedIn(productid);
                            if (sourceDS.T_Product_In.Rows.Count > 0)
                            {
                                //查的到就入进来
                                WMSDS.T_Product_InRow tpirow = sourceDS.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                int onlyid = tpirow.OnlyID;//取消动作生成的ID
                                int sourcePid = tpirow.SourcePID;//被取消的ID
                                #region 仓库部分
                                tpirow.ReadDate = DateTime.Now;
                                tpirow.BusinessType = business;
                                //tpirow.SourceVoucher = sourceVoucher;
                                // tpirow.Warehouse = whcode;
                                //tpirow.WHPosition = factory;// warehouse;
                                tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
                                tpirow.InShift = shift;
                                tpirow.InShiftTime = shiftTime;
                                tpirow.InUser = biller;
                                // tpirow.WHRemark = remark;
                                #endregion
                                #region 状态部分
                                tpirow.StatusIn = -1;
                                tpirow.StatusOut = 0;
                                tpirow.VoucherInID = 0;
                                tpirow.VoucherOutID = 0;
                                tpirow.VoucherRetrieveID = 0;
                                tpirow.SourcePID = sourcePid;//插入红单的条码的时候是以库存中的那个条码为参考，同时保存原条码的onlyid以便对照
                                #endregion
                                WMSDS.T_Product_InRow sourcetpirow = sourceDS.T_Product_In.NewT_Product_InRow();
                                sourcetpirow.OnlyID = sourcePid;
                                sourcetpirow.StatusOut = 1;
                                //插入T_Product_In和productlife
                                WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
                                tplrow.Operate = Utils.WMSOperate._OperScanRedIn;
                                tplrow.OperDate = DateTime.Now;
                                tplrow.OperUser = biller;
                                tplrow.ProductID = productid;
                                tplrow.Status = Utils.WMSOperate._StatusRedIn;
                                string result = this._WMSAccess.Tran_InsertUpdateProductScanInForRedIn(tpirow, tplrow, sourcetpirow);
                                if (result == "")
                                {
                                    //retMsg = "入库成功。";
                                    //查询这个班组这个班次红单入库的汇总
                                    string inDateS = inDate;
                                    string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                                    DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO, "-1");

                                    if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
                                    {
                                        string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
                                        string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();
                                        string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                        string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsLengthLabelNull() ? "0" : (tpirow.LengthLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                        string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsDiameterLabelNull() ? "0" : (tpirow.DiameterLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                        string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;

                                        //组合纸卷的信息
                                        string[] paperStrs = new string[]{
                          "IRA04","0","红单入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, tpirow.IsDiameterLabelNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.Layers.ToString()), tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),diameterReamSheet
                           };
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                }
                                else
                                {
                                    //删除刚才入的product_In
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", " 保存红单入库数据失败:" + result });
                                }
                            }

                            else
                            {
                                //查不到就报错
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", "库存数据中查不到条码信息！" });
                            }

                        }
                        break;
                    case Utils.WMSOperate._StatusOut:
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，不能红单入库！" });
                        break;
                    case Utils.WMSOperate._StatusCancelOut:
                        ds = this._WMSAccess.Select_T_Product_InAndT_InStock(sourcepid);
                        if (ds.T_InStock.Rows.Count == 0)
                        {
                            //还没有做入库单
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做入库单，不能红单入库，请做取消入库扫描！" });
                        }
                        else
                        {
                            //看这个入库单有没有上传
                            string close = ds.T_InStock.Rows[0][ds.T_InStock.IsCloseColumn].ToString();
                            if (close == "1")
                            {
                                //已关单，已上传可以红单入库
                                {
                                    //红单入库从本地product_in中是否能查的到，
                                    WMSDS sourceDS = _WMSAccess.Select_T_Product_InForRedIn(productid);
                                    if (sourceDS.T_Product_In.Rows.Count > 0)
                                    {
                                        //查的到就入进来
                                        WMSDS.T_Product_InRow tpirow = sourceDS.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                        int onlyid = tpirow.OnlyID;//取消动作生成的ID
                                        int sourcePid = tpirow.SourcePID;//被取消的ID
                                        #region 仓库部分
                                        tpirow.ReadDate = DateTime.Now;
                                        tpirow.BusinessType = business;
                                        //tpirow.SourceVoucher = sourceVoucher;
                                        // tpirow.Warehouse = whcode;
                                        //tpirow.WHPosition = factory;// warehouse;
                                        tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
                                        tpirow.InShift = shift;
                                        tpirow.InShiftTime = shiftTime;
                                        tpirow.InUser = biller;
                                        // tpirow.WHRemark = remark;
                                        #endregion
                                        #region 状态部分
                                        tpirow.StatusIn = -1;
                                        tpirow.StatusOut = 0;
                                        tpirow.VoucherInID = 0;
                                        tpirow.VoucherOutID = 0;
                                        tpirow.VoucherRetrieveID = 0;
                                        tpirow.SourcePID = sourcePid;//插入红单的条码的时候是以库存中的那个条码为参考，同时保存原条码的onlyid以便对照
                                        #endregion
                                        WMSDS.T_Product_InRow sourcetpirow = sourceDS.T_Product_In.NewT_Product_InRow();
                                        sourcetpirow.OnlyID = sourcePid;
                                        sourcetpirow.StatusOut = 1;

                                        //插入T_Product_In和productlife
                                        WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
                                        tplrow.Operate = Utils.WMSOperate._OperScanRedIn;
                                        tplrow.OperDate = DateTime.Now;
                                        tplrow.OperUser = biller;
                                        tplrow.ProductID = productid;
                                        tplrow.Status = Utils.WMSOperate._StatusRedIn;
                                        string result = this._WMSAccess.Tran_InsertUpdateProductScanInForRedIn(tpirow, tplrow, sourcetpirow);

                                        //int tpiID = this._WMSAccess.InsertT_Product_InByRow(tpirow, tplrow);
                                        if (result == "")
                                        {
                                            //retMsg = "入库成功。";
                                            //查询这个班组这个班次红单入库的汇总
                                            string inDateS = inDate;
                                            string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                                            DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO, "-1");

                                            if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
                                            {
                                                string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
                                                string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();
                                                string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                                string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsLengthLabelNull() ? "0" : (tpirow.LengthLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                                string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsDiameterLabelNull() ? "0" : (tpirow.DiameterLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                                string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;

                                                //组合纸卷的信息
                                                string[] paperStrs = new string[]{
                          "IRA04","0","红单入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.Layers.ToString()), tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle,tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),diameterReamSheet
                           };
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                            }
                                        }
                                        else
                                        {
                                            //删除刚才入的product_In
                                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", " 保存红单入库数据失败:" + result });
                                        }
                                    }

                                    else
                                    {
                                        //查不到就报错
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", "库存数据中查不到条码信息！" });
                                    }

                                }
                            }
                            else
                            {
                                //未关单
                                string vno = ds.T_InStock.Rows[0][ds.T_InStock.VoucherNOColumn].ToString();
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已在入库单" + vno + "中，且未上传，不能红单入库！需要先上传入库单，或者修改入库单使产品从库存中减掉，再取消入库" });
                            }
                        }
                        break;
                    case Utils.WMSOperate._StatusRedOut:
                        WMSDS outds = this._WMSAccess.Select_T_Product_InAndT_OutStock(prodOnlyID);
                        if (outds.T_OutStock.Rows.Count == 0)
                        {
                            //还没有做红单出库单,不可以取消
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库退货，但未找到该退货单" });
                        }
                        else
                        {
                            //做了退货单就看这个退货单有没有关闭
                            string close = outds.T_InStock.Rows[0][outds.T_InStock.IsCloseColumn].ToString();
                            string vno = outds.T_InStock.Rows[0][outds.T_InStock.VoucherNOColumn].ToString();
                            if (close == "1")
                            {
                                //已关单，已上传
                                //再看suorceid是不是已做入库单
                                ds = this._WMSAccess.Select_T_Product_InAndT_InStock(sourcepid);
                                if (ds.T_InStock.Rows.Count == 0)
                                {
                                    //还没有做入库单,不可以红单入库
                                    //retMsg = this.SetProductCancelInCaseRedOut(command, sourceID, productid, user);
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做入库单，不能红单入库！需要做取消入库扫描" });

                                }
                                else
                                {
                                    //看这个入库单有没有上传
                                    close = ds.T_InStock.Rows[0][ds.T_InStock.IsCloseColumn].ToString();
                                    if (close == "1")
                                    {
                                        //已关单，已上传可以红单入库
                                        {
                                            //红单入库从本地product_in中是否能查的到，
                                            WMSDS sourceDS = _WMSAccess.Select_T_Product_InForRedIn(productid);
                                            if (sourceDS.T_Product_In.Rows.Count > 0)
                                            {
                                                //查的到就入进来
                                                WMSDS.T_Product_InRow tpirow = sourceDS.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
                                                int onlyid = tpirow.OnlyID;//取消动作生成的ID
                                                int sourcePid = tpirow.SourcePID;//被取消的ID
                                                #region 仓库部分
                                                tpirow.ReadDate = DateTime.Now;
                                                tpirow.BusinessType = business;
                                                //tpirow.SourceVoucher = sourceVoucher;
                                                // tpirow.Warehouse = whcode;
                                                //tpirow.WHPosition = factory;// warehouse;
                                                tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
                                                tpirow.InShift = shift;
                                                tpirow.InShiftTime = shiftTime;
                                                tpirow.InUser = biller;
                                                // tpirow.WHRemark = remark;
                                                #endregion
                                                #region 状态部分
                                                tpirow.StatusIn = -1;
                                                tpirow.StatusOut = 0;
                                                tpirow.VoucherInID = 0;
                                                tpirow.VoucherOutID = 0;
                                                tpirow.VoucherRetrieveID = 0;
                                                tpirow.SourcePID = sourcePid;//插入红单的条码的时候是以库存中的那个条码为参考，同时保存原条码的onlyid以便对照
                                                #endregion
                                                WMSDS.T_Product_InRow sourcetpirow = sourceDS.T_Product_In.NewT_Product_InRow();
                                                sourcetpirow.OnlyID = sourcePid;
                                                sourcetpirow.StatusOut = 1;

                                                //插入T_Product_In和productlife
                                                WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
                                                tplrow.Operate = Utils.WMSOperate._OperScanRedIn;
                                                tplrow.OperDate = DateTime.Now;
                                                tplrow.OperUser = biller;
                                                tplrow.ProductID = productid;
                                                tplrow.Status = Utils.WMSOperate._StatusRedIn;
                                                string result = this._WMSAccess.Tran_InsertUpdateProductScanInForRedIn(tpirow, tplrow, sourcetpirow);

                                                //int tpiID = this._WMSAccess.InsertT_Product_InByRow(tpirow, tplrow);
                                                if (result == "")
                                                {
                                                    //retMsg = "入库成功。";
                                                    //查询这个班组这个班次红单入库的汇总
                                                    string inDateS = inDate;
                                                    string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                                                    DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO, "-1");

                                                    if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
                                                    {
                                                        string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
                                                        string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();
                                                        string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
                                                        string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsLengthLabelNull() ? "0" : (tpirow.LengthLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
                                                        string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.IsDiameterLabelNull() ? "0" : (tpirow.DiameterLabel.ToString())) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
                                                        string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;

                                                        //组合纸卷的信息
                                                        string[] paperStrs = new string[]{
                          "IRA04","0","红单入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification,tpirow.IsDiameterLabelNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.DiameterLabel.ToString()),
                            coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":Utils.WMSMessage.TrimEndZero( tpirow.Layers.ToString()), tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
                            Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
                             tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName,
                             tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
                             tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
                                tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsSpliceNull()?"":tpirow.Splice.ToString(),diameterReamSheet
                           };
                                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                                    }
                                                }
                                                else
                                                {
                                                    //删除刚才入的product_In
                                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", " 保存红单入库数据失败:" + result });
                                                }
                                            }

                                            else
                                            {
                                                //查不到就报错
                                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { "IRA04", "9", "库存数据中查不到条码信息！" });
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //未关单
                                        vno = ds.T_InStock.Rows[0][ds.T_InStock.VoucherNOColumn].ToString();
                                        string[] paperStrs = new string[] { command, "9", "产品已在入库单" + vno + "中，且未关闭，不能红单入库！需要先上传入库单，或者修改入库单使产品从库存中减掉，再取消入库" };
                                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                                    }
                                }
                            }
                            else
                            {
                                //未关单，未上传
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在退货单" + vno + "中，还未关闭，不能红单入库！请先关闭该单" });
                            }
                        }
                        break;
                    case Utils.WMSOperate._StatusCancelRedOut:
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，不能红单入库！" });
                        break;
                    default:
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未知状态" + status + "不能入库！" });
                        break;
                }
            }
            else //没有查询到这个条码说明从来没进过系统，不可以红单入库
            {
                string[] paperStrs = new string[] { command, "9", "产品未入库，不能红单扫描入库！" };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            return retMsg;
        }

        public string Process_IP04(string[] strs)
        {
            return this.Process_IR04(strs);
            //{
            //    try
            //    {
            //        string retMsg = "";
            //        string productid = strs[2];
            //        string inDate = strs[3];
            //        string business = strs[4];
            //        string sourceVoucher = strs[5];
            //        string factory = strs[6];
            //        string whcode = strs[7];
            //        string biller = strs[8];
            //        string warehouse = strs[9];
            //        string shift = strs[10];
            //        string shiftTime = strs[11];
            //        string remark = strs[12];
            //        string iswhite = strs[13];
            //        string trademode = strs[14];
            //       //string isCanInRed = this.CheckCanScanInRed(productid);
            //        retMsg = this.StatusCheckIR04(productid, "IRA04", business, shift, shiftTime, biller, inDate);

            //        //if (isCanInRed == "")
            //        //{
            //        //    WMSDS sourceDS = _WMSAccess.Select_T_Product_InForRedIn(productid);
            //        //    if (sourceDS.T_Product_In.Rows.Count > 0)
            //        //    {
            //        //        //查的到就入进来，入进来的时候同时在老数据表和新数据表中插入
            //        //        WMSDS.T_Product_InRow tpirow = sourceDS.T_Product_In.Rows[0] as WMSDS.T_Product_InRow;
            //        //        int onlyid = tpirow.OnlyID;

            //        //        #region 仓库部分
            //        //        tpirow.BusinessType = business;
            //        //        //tpirow.SourceVoucher = sourceVoucher;
            //        //        tpirow.ReadDate = DateTime.Now;
            //        //        //tpirow.Warehouse = whcode;
            //        //        //tpirow.WHPosition = warehouse;
            //        //        tpirow.InDate = DateTime.Now;// Convert.ToDateTime(inDate);
            //        //        tpirow.InShift = shift;
            //        //        tpirow.InShiftTime = shiftTime;
            //        //        tpirow.InUser = biller;
            //        //        //tpirow.WHRemark = remark;
            //        //        //tpirow.SlidesOfReam = 0;
            //        //        //tpirow.SlidesOfSheet = 0;

            //        //        #endregion
            //        //        #region 状态部分
            //        //        tpirow.StatusIn = -1;
            //        //        tpirow.StatusOut = 0;
            //        //        tpirow.VoucherInID = 0;
            //        //        tpirow.VoucherOutID = 0;
            //        //        tpirow.VoucherRetrieveID = 0;
            //        //        tpirow.SourcePID = onlyid;//插入红单的条码的时候是以库存中的那个条码为参考，同时保存原条码的onlyid以便对照
            //        //        #endregion
            //        //        WMSDS.T_Product_InRow sourcetpirow = sourceDS.T_Product_In.NewT_Product_InRow();
            //        //        sourcetpirow.OnlyID = onlyid;
            //        //        sourcetpirow.StatusOut = 1;
            //        //        //插入T_Product_In和productlife
            //        //        WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
            //        //        tplrow.Operate = Utils.WMSOperate._OperScanIn;
            //        //        tplrow.OperDate = DateTime.Now;
            //        //        tplrow.OperUser = biller;
            //        //        tplrow.ProductID = productid;
            //        //        tplrow.Status = Utils.WMSOperate._StatusRedIn;
            //        //        //string result = this._WMSAccess.Tran_InsertProductScanIn(tpirow, tplrow);
            //        //        string result = this._WMSAccess.Tran_InsertUpdateProductScanInForRedIn(tpirow, tplrow, sourcetpirow);

            //        //        if (result == "")
            //        //        {
            //        //            retMsg = "红单入库成功。";
            //        //            //查询这个班组这个班次入库的汇总
            //        //            string inDateS = inDate;
            //        //            string inDateE = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
            //        //            DataSet sumDS = this._WMSAccess.Product_In_SumaryByBatchNO(inDateS, inDateE, biller, shift, shiftTime, tpirow.BatchNO, "-1");

            //        //            if (sumDS.Tables.Count > 0 && sumDS.Tables["T_Product_In"].Rows.Count > 0)
            //        //            {
            //        //                string sum = sumDS.Tables[0].Rows[0]["WeightSum"].ToString();
            //        //                string count = sumDS.Tables[0].Rows[0]["IdCount"].ToString();
            //        //                //组合平板的信息
            //        //                //IPA02|0|msg|materialName|grade|Specification|reams|slidesream|slidesheet|weight|count|fsc|fiberdirect|sku|specprod|speccust|custBrand|orderno|remark

            //        //                string coreReam = tpirow.ProductTypeCode == "1" ? tpirow.CoreDiameter.ToString() : Utils.WMSMessage.TrimEndZero(tpirow.PalletReams.ToString());
            //        //                string lengthSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.LengthLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfSheet.ToString());
            //        //                string diameterReamSheet = tpirow.ProductTypeCode == "1" ? Utils.WMSMessage.TrimEndZero(tpirow.DiameterLabel.ToString()) : Utils.WMSMessage.TrimEndZero(tpirow.SlidesOfReam.ToString());
            //        //                string direct = tpirow.ProductTypeCode == "1" ? tpirow.Direction : tpirow.FiberDirect;


            //        //                string[] paperStrs = new string[]{
            //        //        "IPA04","0","红单入库成功",tpirow.ProductID,tpirow.BatchNO,  tpirow.MaterialName, tpirow.Grade, tpirow.Specification, Utils.WMSMessage.TrimEndZero(tpirow.IsDiameterLabelNull()?"": tpirow.DiameterLabel.ToString()),
            //        //        coreReam,lengthSheet,direct,tpirow.IsLayersNull()?"":tpirow.Layers.ToString(), tpirow.IsWeightModeNull()?"":tpirow.WeightMode, tpirow.IsSlidesOfReamNull()?"":tpirow.SlidesOfReam.ToString(),
            //        //        Utils.WMSMessage.TrimEndZero(tpirow.WeightLabel.ToString()),count,tpirow.IsPaperCertCodeNull()?"":tpirow.PaperCertCode.ToString(),
            //        //         tpirow.IsSKUNull()?"": tpirow.SKU,tpirow.IsSpecProdNameNull()?"":tpirow.SpecProdName,tpirow.IsSpecCustNameNull()?"":tpirow.SpecCustName, 
            //        //         tpirow.IsCustTrademarkNull()?"":tpirow.CustTrademark,tpirow.IsOrderNONull()?"":tpirow.OrderNO,tpirow.IsRemarkNull()?"":tpirow.Remark,
            //        //         tpirow.IsIsWhiteFlagNull()?"":tpirow.IsWhiteFlag,tpirow.IsReamPackTypeNull()?"":tpirow.ReamPackType,tpirow.IsIsPolyHookNull()?"":tpirow.IsPolyHook,
            //        //            tpirow.IsTrademarkStyleNull()?"":tpirow.TrademarkStyle, tpirow.IsTradeModeNull()?"":tpirow.TradeMode,diameterReamSheet

            //        //       };
            //        //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //        //            }
            //        //        }
            //        //        else
            //        //        {
            //        //            //删除刚才入的product_In
            //        //            string[] paperStrs = new string[]{
            //        //      "IPA04","9"," 保存红单入库数据失败:",result
            //        //       };
            //        //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //        //        }
            //        //    }

            //        //    else
            //        //    {
            //        //        //查不到就报错

            //        //        retMsg = "库存数据中查不到条码信息！";
            //        //        string[] paperStrs = new string[]{
            //        //      "IPA04","9",retMsg
            //        //       };
            //        //        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //        //    }

            //        //}
            //        //else
            //        //{
            //        //    retMsg = isCanInRed;
            //        //}

            //        return retMsg;
            //    }
            //    catch (Exception ex)
            //    {
            //        string[] paperStrs = new string[]{
            //              "IPA04","9","红单入库异常"+ex.Message
            //               };
            //        return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //    }

            //}
        }

        public string Process_IR05(string[] strs)
        {
            try
            {
                string retMsg = "";
                string productid = strs[2];
                string user = strs[3];
                string command = "IRA05";
                //先判断一个纸能否取消红单入库
                //string canCancel = this.CheckCanCancelScanInRed(productid);
                DataSet wmsDS = this._WMSAccess.Select_T_ProductLifeByProductID(productid);
                if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
                {
                    string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                    string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                    string prodOnlyID = wmsDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();//life执行的那个id
                    string sourceID = wmsDS.Tables["T_ProductLife"].Rows[0]["SourcePID"].ToString();//执行ID对应的源ID
                    switch (status)
                    {
                        case Utils.WMSOperate._StatusCancelRedIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品的已取消红单入库，不能再次取消红单入库！" });
                            break;
                        case Utils.WMSOperate._StatusIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单入库，不能取消红单入库！" });
                            break;
                        case Utils.WMSOperate._StatusCancelOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单入库，不能取消红单入库！" });
                            break;
                        case Utils.WMSOperate._StatusRedOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单入库，不能取消红单入库！" });
                            break;
                        case Utils.WMSOperate._StatusCancelIn:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已取消入库，不能取消红单入库" });
                            break;
                        case Utils.WMSOperate._StatusRedIn:
                            //是否已做入库红单
                            WMSDS inds = this._WMSAccess.Select_T_Product_InAndT_InStock(prodOnlyID);
                            if (inds.T_InStock.Rows.Count == 0)
                            {
                                //还没有做入库单,可以取消
                                //把它更新为不入库的状态
                                retMsg = this.SetStatusCancelRedInCaseRedIn(prodOnlyID, sourceID, productid, user);
                                if (retMsg == "")
                                {
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "0", "取消红单入库成功" });

                                }
                                else
                                {
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", retMsg });
                                }
                            }
                            else
                            {
                                //做了入库单就看这个入库单有没有上传
                                string close = inds.T_InStock.Rows[0][inds.T_InStock.IsCloseColumn].ToString();
                                string vno = inds.T_InStock.Rows[0][inds.T_InStock.VoucherNOColumn].ToString();
                                if (close == "1")
                                {
                                    //已关单，已上传
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品在入库红单" + vno + "中，且已上传，不能取消红单入库！需要重新入库" });
                                }
                                else
                                {
                                    //未关单，未上传
                                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已在入库红单" + vno + "中，还未上传，不能取消红单入库！需要先修改入库红单，再取消红单入库" });
                                }
                            }
                            break;
                        case Utils.WMSOperate._StatusOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品已出库，不能取消红单入库！" });
                            break;
                        case Utils.WMSOperate._StatusCancelRedOut:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未做红单入库，不能取消红单入库！" });
                            break;

                        default:
                            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(new string[] { command, "9", "产品未知状态" + status + "不能入库！" });
                            break;
                    }
                }
                else //没有查询到这个条码说明从来没进过系统，不可以取消入库
                {
                    string[] paperStrs = new string[] { command, "9", "产品未入库，不能取消红单入库" };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                return retMsg;
            }
            catch (Exception ex)
            {
                string[] paperStrs = new string[]{
                          "IRA05","9","取消红单入库异常："+ex.Message
                           };
                return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
        }

        private string SetStatusCancelRedInCaseRedIn(string prodOnlyID, string sourceID, string productid, string user)
        {
            WMSDS inds = new WMSDS();
            WMSDS.T_Product_InRow tpirow = inds.T_Product_In.NewT_Product_InRow();
            tpirow.OnlyID = Convert.ToInt32(prodOnlyID);
            tpirow.StatusIn = 0;

            WMSDS.T_Product_InRow sourcetpirow = inds.T_Product_In.NewT_Product_InRow();
            sourcetpirow.OnlyID = Convert.ToInt32(sourceID);
            sourcetpirow.StatusOut = 0;

            //插入T_Product_In和productlife
            WMSDS.T_ProductLifeRow tplrow = (new WMSDS()).T_ProductLife.NewT_ProductLifeRow();
            tplrow.Operate = Utils.WMSOperate._OperScanRedInCancel;
            tplrow.OperDate = DateTime.Now;
            tplrow.OperUser = user;
            tplrow.ProductID = productid;
            tplrow.Status = Utils.WMSOperate._StatusCancelRedIn;

            string result = this._WMSAccess.Tran_SetProductCancelRedInCaseRedIn(tpirow, tplrow, sourcetpirow);
            if (result != "")
            {
                result = "产品取消入库失败:" + result;
            }
            return result;
        }
        /// <summary>
        /// 检查是不是可以取消红单入库
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        private string CheckCanCancelScanInRed(string productid)
        {
            string retMsg = "";
            //先判断这个条码是否已经红单入库。
            DataSet wmsDS = _WMSAccess.Select_T_ProductLifeByProductID(productid);

            //如果已经入库那么就可以删除
            if (wmsDS.Tables.Count > 0 && wmsDS.Tables["T_ProductLife"].Rows.Count > 0)
            {

                string status = wmsDS.Tables["T_ProductLife"].Rows[0]["Status"].ToString();
                string time = wmsDS.Tables["T_ProductLife"].Rows[0]["OperDate"].ToString();
                if (status == Utils.WMSOperate._StatusRedIn)
                {
                    /////可以取消，还需要判断是否已做入库单,这个ID的最后一次入库记录
                    // StockInBillQueryByProductID();
                    //还要看这个纸是否已做入库单，如果已做入库单还要提示出来，
                    string onlyid = wmsDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"].ToString();
                    WMSDS piDS = this._WMSAccess.Select_T_Product_InByProductID(onlyid);
                    if (piDS.T_Product_In.Rows.Count > 0)
                    {
                        string voucherinid = piDS.T_Product_In.Rows[0][piDS.T_Product_In.VoucherInIDColumn].ToString();
                        if (voucherinid != "")
                        {
                            //已做入库单
                            piDS = this._WMSAccess.Select_T_InStockByVoucherNO(voucherinid);
                            if (piDS.T_InStock.Rows.Count > 0)
                            {
                                string voucherno = piDS.T_InStock.Rows[0][piDS.T_InStock.VoucherNOColumn].ToString();
                                retMsg = "产品在入库单红单" + voucherno + "中，不能取消入库！";
                                string[] paperStrs = new string[]{
                            "IRA05","9",retMsg
                             };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }
                            else
                            {  //不可以取消
                                retMsg = "产品对应的入库单红单" + voucherinid + "不存在，不能取消入库！";
                                string[] paperStrs = new string[]{
                            "IRA05","9",retMsg
                             };
                                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            }

                            //////  //判断入库单是否已上传，已审核
                            //////  wmsDS = this._WMSAccess.Select_T_InStockByVoucherNO(voucherinid);
                            //////  if (wmsDS.T_InStock.Rows.Count > 0)
                            //////  {
                            //////      string check = wmsDS.T_InStock.Rows[0][wmsDS.T_InStock.IsCheckColumn].ToString();
                            //////      string upload = wmsDS.T_InStock.Rows[0][wmsDS.T_InStock.IsUploadColumn].ToString();
                            //////      string voucherno = wmsDS.T_InStock.Rows[0][wmsDS.T_InStock.VoucherNOColumn].ToString();
                            //////      if (upload == "1")
                            //////      { //不可以取消
                            //////          retMsg = "产品对应的入库单" + voucherno + "已上传，不能取消入库！";
                            //////          string[] paperStrs = new string[]{
                            //////"IRA03","9",retMsg
                            ////// };
                            //////          retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            //////      }
                            //////      else if (check == "1")
                            //////      { //不可以取消
                            //////          retMsg = "产品对应的入库单" + voucherno + "已审核，不能取消入库！";
                            //////          string[] paperStrs = new string[]{
                            //////"IRA03","9",retMsg
                            ////// };
                            //////          retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            //////      }
                            //////      else
                            //////      {
                            //////          retMsg = "";
                            //////      }
                            //////  }
                            //////  else
                            //////  {  //不可以取消
                            //////      retMsg = "产品对应的入库单" + voucherinid + "不存在，不能取消入库！";
                            //////      string[] paperStrs = new string[]{
                            //////"IRA03","9",retMsg
                            ////// };
                            //////      retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                            //////  }
                        }
                        else
                        {
                            retMsg = "";

                        }

                    }
                    else
                    {
                        //不可以取消
                        retMsg = "产品" + productid + "不在库存中，不能取消入库！";
                        string[] paperStrs = new string[]{
                          "IRA05","9",retMsg
                           };
                        retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                    }
                }
                //else if (status == Utils.WMSOperate._StatusHalfOut)
                //{
                //    //不可以取消
                //    retMsg = "产品已于" + time + "扫描出库，不能取消红单入库！";
                //    string[] paperStrs = new string[]{
                //          "IRA05","9",retMsg
                //           };
                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //}
                else if (status == Utils.WMSOperate._StatusOut)
                {
                    //不可以取消
                    retMsg = "产品已于" + time + "出库，不能取消红单入库！";
                    string[] paperStrs = new string[]{
                          "IRA05","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                else if (status == Utils.WMSOperate._StatusCancelIn)
                {
                    //不可以取消
                    retMsg = "产品已于" + time + "取消入库，不能取消红单入库！";
                    string[] paperStrs = new string[]{
                          "IRA05","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                //else if (status == Utils.WMSOperate._StatusHalfReturn)
                //{
                //    //不可以取消
                //    retMsg = "产品已于" + time + "扫描退货，不能取消红单入库！";
                //    string[] paperStrs = new string[]{
                //          "IRA05","9",retMsg
                //           };
                //    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                //}
                else
                {
                    //不可以取消
                    retMsg = "产品状态未知" + status + "，不能取消红单入库！";
                    string[] paperStrs = new string[]{
                          "IRA05","9",retMsg
                           };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
            }
            else
            {
                //如果没有入库那么就不能删除
                retMsg = "产品没有入库，不能取消红单入库！";
                string[] paperStrs = new string[]{
                          "IRA05","9",retMsg
                           };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            return retMsg;
        }

        public string Process_IP05(string[] strs)
        {
            return this.Process_IR05(strs);
            //try
            //{
            //    string retMsg = "";
            //    string productid = strs[2];
            //    string user = strs[3];
            //    //先判断一个纸能否取消红单入库
            //    string canCancel = this.CheckCanCancelScanInRed(productid);

            //    if (canCancel == "")
            //    {
            //        //先查询出这个这个条码在product in中最后一个的onlyid
            //        //WMSDS tpiDS = this._WMSAccess.T_Product_InQuery(productid);

            //        DataSet tpiDS = _WMSAccess.Select_T_ProductLifeByProductID(productid);

            //        if (tpiDS.Tables.Count>0&&tpiDS.Tables["T_ProductLife"].Rows.Count > 0)
            //        {
            //            int pid = Convert.ToInt32(tpiDS.Tables["T_ProductLife"].Rows[0]["ProductOnlyID"]);
            //            //把它更新为不入库的状态
            //            WMSDS.T_ProductLifeRow tpfRow = new WMSDS().T_ProductLife.NewT_ProductLifeRow();
            //            tpfRow.ProductOnlyID = pid;
            //            tpfRow.ProductID = productid;
            //            tpfRow.OperUser = user;
            //            tpfRow.OperDate = DateTime.Now;
            //            tpfRow.Operate = Utils.WMSOperate._OperScanRedInCancel;
            //            tpfRow.Status = Utils.WMSOperate._StatusIn;
            //            WMSDS.T_Product_InRow tpiRow = new WMSDS().T_Product_In.NewT_Product_InRow();
            //            tpiRow.OnlyID = tpfRow.ProductOnlyID;
            //            tpiRow.StatusIn = 0;
            //            tpiRow.VoucherInID = 0;

            //            string result = this._WMSAccess.Tran_Update_ProductScanInForCancel(tpiRow, tpfRow);
            //            if (result == "")
            //            {
            //                retMsg = "产品取消红单入库成功";
            //                string[] paperStrs = new string[]{
            //                "IPA05","0",retMsg
            //                 };
            //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //            }
            //            else
            //            {
            //                retMsg = "产品取消红单入库失败:" + result;
            //                string[] paperStrs = new string[]{
            //                "IPA05","9",retMsg
            //                 };
            //                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //            }
            //        }
            //        else
            //        {
            //            retMsg = "产品取消红单入库失败，没有找到这个条码的记录";
            //            string[] paperStrs = new string[]{
            //                "IPA05","9",retMsg
            //                 };
            //            retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //        }
            //    }
            //    else
            //    {
            //        retMsg = canCancel;
            //    }
            //    return retMsg;
            //}
            //catch (Exception ex)
            //{
            //    string[] paperStrs = new string[]{
            //              "IPA05","9","取消红单入库异常："+ex.Message
            //               };
            //    return Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            //}
        }

        public string Process_Q16(string[] strs)
        {
            string retMsg = "";
            ProduceDS sysDS = this._WMSAccess.Select_System_Connections("");
            string sys = "";
            if (sysDS.System_Connections.Rows.Count > 0)
            {
                for (int i = 0; i < sysDS.System_Connections.Rows.Count; i++)
                {
                    sys += sysDS.System_Connections.Rows[i][sysDS.System_Connections.MachineIDColumn].ToString() + Utils.WMSMessage._ForeachChar;
                }
                sys = sys.TrimEnd(Utils.WMSMessage._ForeachChar);
                string[] paperStrs = new string[] { "QA16", "0", "机台号刷新成功", sys };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            else
            {
                //发运单不存在，不能出库。
                retMsg = "没有机台信息";
                string[] paperStrs = new string[] { "QA16", "9", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }


            return retMsg;
        }

        public string Process_Q17(string[] strs)
        {
            string retMsg = "";
            string factory = strs[2];

            //先根据接收机台号查询出接收机台的连接地址
            ProduceDS pDS = this._WMSAccess.Select_System_Connections(factory);
            if (pDS.System_Connections.Rows.Count > 0)
            {
                string factoryStr = pDS.System_Connections.Rows[0][pDS.System_Connections.ConnectStringColumn].ToString();
                //再从那个连接地址中找到去向
                ProduceDS sysDS = this._WMSAccess.Select_Paper_Destination(factoryStr);
                string sys = "";
                if (sysDS.Paper_Destination.Rows.Count > 0)
                {
                    for (int i = 0; i < sysDS.Paper_Destination.Rows.Count; i++)
                    {
                        sys += sysDS.Paper_Destination.Rows[i][sysDS.Paper_Destination.DestinationColumn].ToString() + "." + sysDS.Paper_Destination.Rows[i][sysDS.Paper_Destination.DestinationDescColumn].ToString() + Utils.WMSMessage._ForeachChar;
                    }
                    sys = sys.TrimEnd(Utils.WMSMessage._ForeachChar);
                    string[] paperStrs = new string[] { "QA17", "0", "去向刷新成功", sys };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
                else
                {
                    retMsg = "没有去向信息";
                    string[] paperStrs = new string[] { "QA17", "9", retMsg };
                    retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
                }
            }
            else
            {
                retMsg = "没有找到机台" + factory + "信息";
                string[] paperStrs = new string[] { "QA17", "9", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }



            return retMsg;
        }

        public string Process_Q18(string[] strs)
        {
            string retMsg = "";
            string barcode = strs[2];
            string source = strs[3];
            string accept = strs[4];
            string dest = strs[5];
            string operate = strs[6];
            string ip = strs[7];
            string datetime = strs[8];

            //先根据接收机台号查询出接收机台的连接地址
            string ret = this._WMSAccess.Insert_System_CrossFactory_TransCommands(barcode, source, accept, dest, operate, ip, datetime);
            if (ret == "")
            {

                retMsg = "保存成功";
                string[] paperStrs = new string[] { "QA18", "0", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }
            else
            {
                retMsg = "保存失败：" + ret;
                string[] paperStrs = new string[] { "QA18", "9", retMsg };
                retMsg = Utils.WMSMessage.MakeWMSSocketMsg(paperStrs);
            }



            return retMsg;
        }
        /// <summary>
        /// 入库确认，并返回移库单已确认产品件数和重量
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string Process_Q20(string[] strs)
        {
            string productId = strs[2].Trim();
            int result = this._WMSAccess.StockInConfirm(productId);
            string alertmsg = string.Empty;
            string msg = string.Empty;
            string code = "0";
            if (result == -1)
            {
                alertmsg = "入库确认过程中发生错误，请重试!";
                code = "9";
            }
            else
            {
                DataSet transOutBillInfo = _WMSAccess.Select_StockInConfirmInfo(productId);
                if (transOutBillInfo.Tables.Count <= 0 || transOutBillInfo.Tables[0].Rows.Count <= 0)
                {
                    alertmsg = "当前条形码没有移库数据！";
                    code = "9";
                }
                else
                {
                    string transOutBillNo = Convert.ToString(transOutBillInfo.Tables[0].Rows[0]["FTRANSOUTBILLNO"]);
                    decimal totalAmount = Convert.ToDecimal(transOutBillInfo.Tables[0].Rows[0]["FTOTALAMOUNT"]);
                    decimal totalWeight = Convert.ToDecimal(transOutBillInfo.Tables[0].Rows[0]["FTOTALWEIGHT"]);
                    decimal confirmedAmount = 0;
                    decimal confirmedWeight = 0;
                    foreach (DataRow item in transOutBillInfo.Tables[0].Rows)
                    {

                        string isConfirm
                            = Convert.ToString(item["FISINSTOCKCONFIRM"]);
                        if (isConfirm == "Y")
                        {
                            confirmedAmount++;
                            decimal weight = Convert.ToDecimal(item["WEIGHTLABEL"]) / 1000;
                            confirmedWeight += weight;
                        }
                    }
                    if (result == 0)
                    {
                        alertmsg = string.Format("条形码{0}已经入库确认!", productId);
                        code = "9";
                    }
                    else
                    {
                        code = "0";
                    }
                    msg = string.Format("移出单号{0}\n已确认入库\n{1}件/{2}件,\n已确认入库\n{3}吨/{4}吨",
                        transOutBillNo, Utils.WMSMessage.TrimEndZero(confirmedAmount.ToString()),
                        Utils.WMSMessage.TrimEndZero(totalAmount.ToString()),
                        Utils.WMSMessage.TrimEndZero(confirmedWeight.ToString()),
                        Utils.WMSMessage.TrimEndZero(totalWeight.ToString()));
                }
            }
            string[] msgs = new string[] { "QA20", code, msg, alertmsg, productId };
            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }
    }
}
