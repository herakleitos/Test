using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Net;

using CTSocket;
using CTWH.Common;
using DataModel;

namespace WH_PDA_Service
{
    partial class PDAService : ServiceBase
    {
        private string ServiceDesc = "Metso RollWrap";
        CTSocket.SocketServer server_rollwrap;
        CTWH.Common.SocketService.ServerSocketService socketservice = new CTWH.Common.SocketService.ServerSocketService();

        MainDS MDS = new MainDS();
        CTWH.Common.MSSQL.MSSQLAccess access;

        //private System.Threading.Thread bgSocketTempMES;
        ////  清空日志信息
        //private System.Threading.Thread bgRecordClearMES;

        //server 通信
        int SocketTempTS = 10000;
        bool IsServiceClosing = false;
        public PDAService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            InitData();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            DisposeData();
        }

        private void InitData()
        {
            access = Utils.MSSqlAccess;

            //通信初始化            
            Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Service start");

            access.SqlStateChange += new CTWH.Common.MSSQL.MSSQLAccess.SqlStateEventHandler(access_SqlStateChange);

            //接受到满足要求的包 触发
            socketservice.DataReceived += new CTWH.Common.SocketService.ServerSocketService.DataReceivedDelegate(socketservice_DataReceived);
            socketservice.ClientConnected += new CTWH.Common.SocketService.ServerSocketService.ClientConnectedDelegate(socketservice_ClientConnected);
            socketservice.ClientDisconnected += new CTWH.Common.SocketService.ServerSocketService.ClientDisconnectedDelegate(socketservice_ClientDisconnected);

            //bgSocketTempMES = new System.Threading.Thread(new System.Threading.ThreadStart(bgSocketTempMES_DoWork));
            //bgSocketTempMES.Start();

            //bgRecordClearMES = new System.Threading.Thread(new System.Threading.ThreadStart(bgRecordClearMES_DoWork));
            //bgRecordClearMES.Start();

            server_rollwrap = new SocketServer(CallbackThreadType.ctWorkerThread,
              socketservice,
              DelimiterType.dtMessageTailExcludeOnReceive,
              Utils.SocketParaMetsoRollWrap.SocketEncoding.GetBytes(new char[] { (char)3 }),
               1024, 2048, 10000, 360000
              );

            if (!server_rollwrap.Active)
            {
                //启动服务               
                server_rollwrap.AddListener(ServiceDesc + " Listener", new IPEndPoint(IPAddress.Any, Utils.SocketParaMetsoRollWrap.SocketPort));
                server_rollwrap.Start();
                Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Listener start");
            }
        }

        private void DisposeData()
        {
            IsServiceClosing = true;

            Utils.WriteTxtLog(Utils.FilePath_txtMetsoLog, ServiceDesc + " Service stop");
            access.SqlStateChange -= new CTWH.Common.MSSQL.MSSQLAccess.SqlStateEventHandler(access_SqlStateChange);
            access = null;

            if (server_rollwrap != null)
            {
                socketservice.DataReceived -= new CTWH.Common.SocketService.ServerSocketService.DataReceivedDelegate(socketservice_DataReceived);
                socketservice.ClientConnected -= new CTWH.Common.SocketService.ServerSocketService.ClientConnectedDelegate(socketservice_ClientConnected);
                socketservice.ClientDisconnected -= new CTWH.Common.SocketService.ServerSocketService.ClientDisconnectedDelegate(socketservice_ClientDisconnected);

                server_rollwrap.Stop();
                server_rollwrap.Dispose();
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

        void socketservice_DataReceived(MessageEventArgs e)
        {
            DateTime receivetime = DateTime.Now;

            bool IsSendBuffer = false;

            //不包含了末尾结束符号
            string datagramText = Utils.SocketParaMetsoRollWrap.SocketEncoding.GetString(e.Buffer);

            string ReplyMessage = "Do nothing";

            string[] strs = datagramText.TrimStart((char)2).TrimEnd((char)3).Split('|');

            UInt16 CharCount = 0;
            string FunctionCode = "";
            string RollID = "";

            int Diameter_M = 0, Width_M = 0, Weight_M = 0;
            string RollStatus = "";

            if (strs.Length > 1)
            {
                CharCount = (UInt16)Convert.ToInt16(strs[0]);
                FunctionCode = strs[1];
                //
                if ((CharCount + 4) != datagramText.Length)
                {
                    //验算失败
                    ReplyMessage = "Message length wrong";
                }
                else
                {
                    switch (FunctionCode)
                    {
                        case "R001":
                            if (strs.Length > 6)
                            {
                                RollID = strs[2].Trim();

                                if (VerifyIsNumber(strs[3]) &&
                                      VerifyIsNumber(strs[4]) &&
                                        VerifyIsNumber(strs[5]))
                                {
                                    Diameter_M = Convert.ToInt32(strs[3]);
                                    Width_M = Convert.ToInt32(strs[4]);
                                    Weight_M = Convert.ToInt32(strs[5]);
                                    RollStatus = strs[6].Trim();

                                    RollDS.Roll_ProductDataTable rolltb;
                                    rolltb = access.RollDS_Roll_ProductQueryByBarcode(RollID).Roll_Product;

                                    if (rolltb.Rows.Count > 0)
                                    {
                                        RollDS.Roll_ProductRow row = rolltb.Rows[0] as RollDS.Roll_ProductRow;
                                        //
                                        IsSendBuffer = true;

                                        //保存测量值
                                        access.Roll_ProductMetsoMValueUpdateByProductID(RollID, Width_M, Diameter_M, Weight_M);



                                        //ReplyMessage = GenerateMessageBufferM001(row, "RollWrap", Width_M, Diameter_M, Weight_M);  
                                        byte[] sendbs = GenerateMessageBufferM001(row, "RollWrap", Width_M, Diameter_M, Weight_M);
                                        e.Connection.BeginSend(sendbs);

                                        //保存记录日志
                                        access.SocketRecordMetsoWrapInsertByValue(FunctionCode, false, "", datagramText, "SVR GET", Utils.DateTimeNow, RollID);
                                        access.SocketRecordMetsoWrapInsertByValue(FunctionCode, true, "", Encoding.UTF8.GetString(sendbs), "SVR SEND", Utils.DateTimeNow, RollID);

                                    }
                                    else
                                    {
                                        //1 = error in ID
                                        //ReplyMessage = "Roll not exsit";
                                        ReplyMessage = String.Join("|", new string[]{
                                       "M001",
                                       RollID.PadRight(16),
                                       "1",  //1 = error in ID
                                       "0000",
                                       "0000",
                                       "00000",
                                       "0",
                                       " ",
                                       "0","0","0",
                                       "".PadRight(20),
                                       "0000",
                                       "0000",
                                       "00000",
                                       "0000",
                                       "".PadRight(3),  "0000",  "".PadRight(50),  "00",
                                       "".PadRight(13), 
                                       "".PadRight(300),
                                       "".PadRight(60)
                                       });
                                    }
                                }
                                else
                                {
                                    ReplyMessage = "Message not valid number";
                                }
                            }
                            break;

                        case "R003":
                            if (strs.Length > 3)
                            {
                                RollID = strs[2].Trim();
                                RollStatus = strs[3];
                                ReplyMessage = String.Join("|", new string[]{
                                "M003",
                                RollID.PadRight(16)                             
                                });



                            }
                            else
                            {
                                ReplyMessage = "Message not valid";
                            }
                            break;

                        case "R004":
                            ReplyMessage = "M004";



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

            if (!IsSendBuffer)
            {

                //保存记录日志
                access.SocketRecordMetsoWrapInsertByValue(FunctionCode, false, "", datagramText, "SVR GET", Utils.DateTimeNow, RollID);
                access.SocketRecordMetsoWrapInsertByValue(FunctionCode, true, "", ReplyMessage, "SVR SEND", Utils.DateTimeNow, RollID);

                //答复信息
                e.Connection.BeginSend(Utils.SocketParaMetsoRollWrap.SocketEncoding.GetBytes(GenerateSocketString(ReplyMessage)));
            }


        }

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

            MainDS.App_ParameterDataTable appParaTB = access.App_ParameterQueryByType(ParaType).App_Parameter;
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
         ( row.IsRemark5Null()||  row.Remark5.Trim()=="")?"1":row.Remark5.Trim(),//BodyLabel,    在复卷机那里有选项，0=不打印，1=一号纸盒，2=二号纸盒      
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

            MainDS.App_InkJetFormatDataTable InkjetFormatTB = access.App_InkJetFormatQueryByFormatID(FormatID).App_InkJetFormat;

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
            foreach (ISocketConnectionInfo isocketinfo in server_rollwrap.GetConnections())
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

    }
}
