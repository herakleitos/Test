using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.Core.AppConfig;
using Chaint.Instock.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Devices.PLC;
using Chaint.Common.Devices.PLC.Utils;
using Chaint.Common.Devices.Devices;
using Chaint.Common.Core.Enums;
using Chaint.Common.Devices.LED;
using System.Windows.Forms;
using System.Threading;

namespace Chaint.Instock.Business.PlugIns
{

    public delegate void ScannBarcodeCollectHandler(IList<string> Barcodes);

    public class AutoScanProcessor
    {
        public Context context;
        public event ScannBarcodeCollectHandler OnScanningResult;
        public delegate void RecordScanResultDelegate(int PaperCount, IList<string> Barcodes);
        public event RecordScanResultDelegate RecordScanResultHandler;
        public delegate void SendMessageDelegate(string key, string msg);
        public event SendMessageDelegate SendMessageHandler;
        public BarcodeCollection BarcodeCollectionInstance;
        private string ScanStartSignal_Last = "";     //上一次开始信号
        private string ScanEndSignal_Last = "";        //上一次结束信号

        private const string OpenCmd = "TRIGGER ON";
        private const string CloseCmd = "TRIGGER OFF";

        private IList<string> m_lstBarcodes = new List<string>();
        private LED_TY ledHelper;
        private Scanner m_Scanner = null;
        private string m_ScannerIP = "";
        private int m_ScannerPort = 23;
        private AppConfig_INI appConfiger;
        private PLCAccessor m_PlcService = null;
        private string m_Stereo = "0";
        private string paperType = "2";
        private string m_Plc_StationName = "CPU1";      //PLC工位名称
        private string m_Plc_TemplateName = "COM";      //PLC模板名称
        private byte[] m_PlcBuffer = null;

        private BackgroundWorker bgWorker = null;       //处理逻辑
        private BackgroundWorker ledWorker = null;

        public IList<string> ScanBarcodes
        {
            get { return m_lstBarcodes; }
            set { m_lstBarcodes = value; }
        }

        public string ScannerIPAddr
        {
            set
            {
                m_ScannerIP = value;
            }
            get
            {
                return m_ScannerIP;
            }
        }
        public AutoScanProcessor(Context ctx)
        {
            context = ctx;
            appConfiger = new AppConfig_INI(ctx.DevicesConfigFilePath);
            m_Stereo = appConfiger.GetValue("STEREO", "Value", "0");
            m_ScannerIP = appConfiger.GetValue("SCANNER", "IPAddr", "192.168.106.220");
            string port = appConfiger.GetValue("SCANNER", "Port", "23");
            m_ScannerPort = Convert.ToInt32(port);
            m_Plc_StationName = appConfiger.GetValue("PLCConnection", "GongWeiType", "CPU1");
            m_Plc_TemplateName = appConfiger.GetValue("PLCConnection", "MoBanNo", "COM");
            paperType = appConfiger.GetValue("PAPERTYPE", "Type", "2");
            BarcodeCollectionInstance = new BarcodeCollection(ctx);
        }

        public void Dispose()
        {
            m_PlcService.StopPLCAccess();
            ledHelper.CloseDevice();
            CloseScanner();
            if (bgWorker != null)
            {
                bgWorker.DoWork -= BgWorker_DoWork;
                bgWorker.CancelAsync();
                bgWorker.Dispose();
                bgWorker = null;
            }
            if (ledWorker != null)
            {
                ledWorker.DoWork -= ledWorker_DoWork;
                ledWorker.CancelAsync();
                ledWorker.Dispose();
                ledWorker = null;
            }
        }

        private void ledWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string paperType = appConfiger.GetValue("PAPERTYPE", "Type", "2");
                string ipAddr = appConfiger.GetValue("LED", "IPAddr", "");
                string port = appConfiger.GetValue("LED", "Port", "");
                int height = int.Parse(appConfiger.GetValue("LED", "FontHeight", "16"));
                int width = int.Parse(appConfiger.GetValue("LED", "FontWidth", "8"));
                int positionX = int.Parse(appConfiger.GetValue("LED", "PositionX", "0"));
                int positionY = int.Parse(appConfiger.GetValue("LED", "PositionY", "0"));
                string color = appConfiger.GetValue("LED", "Color", "Red");
                LED_FontStyle fontStyle = new LED_FontStyle();
                fontStyle.FontHeight = height;
                fontStyle.FontWidth = width;
                Param_LED_Ethernet param = new Param_LED_Ethernet(ipAddr, ushort.Parse(port));
                ledHelper = new LED_TY(param);
                ledHelper.ConnectDevice();
                string oldIdStr = string.Empty;
                while (true)
                {
                    Thread.Sleep(1000);
                    List<int> groupIds = new List<int>();
                    string newIdStr = string.Empty;
                    bool isVal = true;
                    List<string> positions = new List<string>();
                    positions.Add(GetPlcSignalValue(PlcSignalName.Plc_Position1));
                    //positions.Add(GetPlcSignalValue(PlcSignalName.Plc_Position2));
                    //positions.Add(GetPlcSignalValue(PlcSignalName.Plc_Position3));
                    //positions.Add(GetPlcSignalValue(PlcSignalName.Plc_Position4));
                    //positions.Add(GetPlcSignalValue(PlcSignalName.Plc_Position5));
                    foreach (string position in positions)
                    {
                        if (position.IsNullOrEmptyOrWhiteSpace() && !position.Equals("0"))
                        {
                        }
                        else
                        {
                            int id = Convert.ToInt32(position);
                            if (id > 3)
                            {
                                groupIds.Add(Convert.ToInt32(position));
                            }
                        }
                    }
                    newIdStr = string.Join("|", positions);
                    if (oldIdStr.Equals(newIdStr))
                    {
                        isVal = false;
                    }
                    oldIdStr = newIdStr;
                    if (!isVal) continue;
                    //联合数据库数据和plc数据，发送组合显示信息给led
                    List<Tuple<string, int>> message = CommonHelper.GetProductInfoFromBD(this.context, groupIds, positions, paperType);
                    IList<LED_TextStyle> msgStyles = new List<LED_TextStyle>();
                    if (message == null)
                    {
                        LED_TextStyle style1 = new LED_TextStyle("当前没有产品信息",
                         Common.Devices.LED.Color.RED, positionX, positionY);
                        LED_TextStyle style2 = new LED_TextStyle("",
                         Common.Devices.LED.Color.RED, positionX, positionY + height);
                        msgStyles.Add(style1);
                        msgStyles.Add(style2);
                        msgStyles.Add(style2);
                        msgStyles.Add(style2);
                        msgStyles.Add(style2);
                        msgStyles.Add(style2);
                        msgStyles.Add(style2);
                        msgStyles.Add(style2);
                        if (paperType == "1")
                        {
                            msgStyles.Add(style2);
                        }
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in message)
                        {
                            LED_TextStyle style = new LED_TextStyle(item.Item1,
                                 Common.Devices.LED.Color.RED, positionX, positionY + height * i);
                            msgStyles.Add(style);
                            i++;
                        }
                    }
                    LED_ActionSytle action = new LED_ActionSytle(Common.Devices.LED.InMode.DIRECT,
                        Common.Devices.LED.Speed.S_12);
                    action.StayTime = 0;
                    bool isSuccess = ledHelper.WriteData(fontStyle, msgStyles, action);
                    if (isSuccess)
                    {
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FLedConStatus, "LED已连接");
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FMonitor, "LED数据发送成功");
                    }
                    else
                    {
                        oldIdStr = string.Empty;
                        ledHelper.CloseDevice();
                        Thread.Sleep(100);
                        ledHelper.ConnectDevice();
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FLedConStatus, "LED已断开连接");
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FMonitor, "LED数据发送失败，正在重新发送");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FMonitor, "发生错误，LED进程已关闭");
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitialData(DataTable dtPlcConnect, DataTable dtPlcConfig)
        {
            //PLC模块
            m_PlcService = new PLCAccessor();
            m_PlcService.InitalPLCParams(dtPlcConnect, dtPlcConfig);
            m_PlcService.OnPLCMessageChanged += M_PlcService_OnPLCMessageChanged;
            m_PlcService.StartPLCAccess();
            SendMessageHandler(Const_StockInAutoScan.Head_Field_FPlcConStatus, "PLC已连接");

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += BgWorker_DoWork;

            ledWorker = new BackgroundWorker();
            ledWorker.WorkerSupportsCancellation = true;
            ledWorker.DoWork += ledWorker_DoWork;

            //扫描仪模块
            Chaint.Common.Devices.Devices.Param_Base scannParam = new Param_Ethernet(Param_Ethernet.CommMode.CLISERVICE, this.ScannerIPAddr, m_ScannerPort);
            m_Scanner = ScanFactory.CreateDevice(ScanType.Cognex, scannParam);
            m_Scanner.OnBarcodeValue += M_Scanner_OnBarcodeValue;
            m_Scanner.OnRunMessage += M_Scanner_OnRunMessage;
            m_Scanner.Connect();
            // SetScanParams();


            //tmrWorker.Interval = 800;
            //tmrWorker.Tick += TmrWorker_Tick;
            //tmrWorker.Start();

            bgWorker.RunWorkerAsync();
            SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerConStatus, "扫描仪已连接");
            ledWorker.RunWorkerAsync();
            SendMessageHandler(Const_StockInAutoScan.Head_Field_FLedConStatus, "LED已连接");
        }

        private void TmrWorker_Tick(object sender, EventArgs e)
        {
            try
            {
                ScanningProcess();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (bgWorker == null || bgWorker.CancellationPending) return;


                    ScanningProcess();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }

                System.Threading.Thread.Sleep(400);
            }
        }


        /// <summary>
        /// 扫描处理
        /// </summary>
        private void ScanningProcess()
        {

            if (m_PlcService.PLCConnects[m_Plc_StationName, m_Plc_TemplateName].ByteBuffer == null)
            {
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FMonitor, "PLC未读到数据");
                return;
            }
            m_PlcBuffer = m_PlcService.PLCConnects[m_Plc_StationName, m_Plc_TemplateName].ByteBuffer;

            ShowPlcInfo();

            Process_StartPLCSignal();

            Process_EndPLCSignal();

        }

        /// <summary>
        /// PLC Start Signal
        /// </summary>
        private void Process_StartPLCSignal()
        {
            //开始信号
            string strStartSignal = GetPlcSignalValue(PlcSignalName.Plc_StartScanning);
            if (strStartSignal == ScanStartSignal_Last) return;
            //收到开始信号
            if (strStartSignal == "86" && m_Scanner != null)
            {
                // SendMessageHandler("ResetSendSignal", "复位PLC发送标志");

                ScanStartSignal_Last = strStartSignal;
                SetScanParams();

                SendMessageHandler(Const_StockInAutoScan.Head_Field_FResult, "准备接收条码");
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FBarcode, "");
                BarcodeCollectionInstance.ClearBarcodes();
                OpenScannerLED();
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus, "扫描仪已打开");

            }
            else if (strStartSignal == "0" && m_Scanner != null)
            {
                ScanStartSignal_Last = strStartSignal;
                CloseScannerLED();
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus, "扫描仪已关闭");
            }

        }

        /// <summary>
        /// PLC Start Signal
        /// </summary>
        private void Process_EndPLCSignal()
        {

            //结束信号
            string strEndSignal = GetPlcSignalValue(PlcSignalName.Plc_EndScanning);

            if (strEndSignal == ScanEndSignal_Last) return;

            ScanEndSignal_Last = strEndSignal;
            if (strEndSignal == "86")
            {


                CloseScannerLED();

                //获取条码数量
                int productCount = Convert.ToInt32(GetPlcSignalValue(PlcSignalName.Plc_ProductCount));
                int barcodeCount = BarcodeCollectionInstance.BarcodeCount;

                Logger.Log("BarCodeProcess", string.Format("********************{0}接收到扫描结束信号********************", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                int diffCount = System.Math.Abs(productCount - barcodeCount);
                RecordScanResultHandler(barcodeCount,
                    BarcodeCollectionInstance.LstBarcodes);
                if (paperType == "1")
                {
                    SendMessageHandler(Const_StockInAutoScan.Head_Field_FResult, "开始处理扫描结果");
                }
                if (productCount != barcodeCount || barcodeCount == 0)
                {
                    SendMessageHandler(Const_StockInAutoScan.Head_Field_FResult, string.Format("扫描条码数量不一致[{0}-{1}]", productCount, barcodeCount));
                    if (barcodeCount == 0)
                    {
                        m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_GroupOID),
                            (int)Enums_PLC_GroupID.NoBarCode);
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus,
                            string.Format("发送组合ID-{0}", (int)Enums_PLC_GroupID.NoBarCode));
                        Logger.Log("BarCodeProcess", string.Format("{0}向PLC发送组合ID(NoBarCode-1)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    else if (diffCount == 1)
                    {
                        m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_GroupOID),
                            (int)Enums_PLC_GroupID.OneBarCode);
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus,
                            string.Format("发送组合ID-{0}", (int)Enums_PLC_GroupID.OneBarCode));
                        Logger.Log("BarCodeProcess", string.Format("{0}向PLC发送组合ID(OneBarCode-4)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    else if (diffCount == 2)
                    {
                        m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_GroupOID),
                            (int)Enums_PLC_GroupID.TwoBarCode);
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus,
                            string.Format("发送组合ID-{0}", (int)Enums_PLC_GroupID.TwoBarCode));
                        Logger.Log("BarCodeProcess", string.Format("{0}向PLC发送组合ID(TwoBarCode-5)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    //发送失败信号
                    m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_ScanResult), 85);
                    if (m_Stereo == "1")
                    {
                        //去向：200(人工仓库)
                        m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_ScanDes), 200);
                        Logger.Log("BarCodeProcess", string.Format("{0}向PLC发送扫描失败(85)，去向信号(200-人工仓库)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    else
                    {
                        Logger.Log("BarCodeProcess", string.Format("{0}向PLC发送扫描失败(85)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                }
                else
                {
                    if (paperType == "1")
                    {
                        //发送扫描成功,扫描仪不用再下降扫描
                        m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_ScanResult), 99);
                        SendMessageHandler(Const_StockInAutoScan.Head_Field_FResult, "扫描完成,等待处理结果!");
                        SendScannBarcodes(BarcodeCollectionInstance.LstBarcodes);
                    }

                }
                Logger.Log("BarCodeProcess", string.Format("********************{0}条码处理完成********************", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                Logger.Log("BarCodeProcess", string.Format(" ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                BarcodeCollectionInstance.ClearBarcodes();
            }
        }


        private string oldPlcInfo = string.Empty;
        private void ShowPlcInfo()
        {
            string strStartSignal = GetPlcSignalValue(PlcSignalName.Plc_StartScanning);
            string strEndSignal = GetPlcSignalValue(PlcSignalName.Plc_EndScanning);
            string strScanResult = GetPlcSignalValue(PlcSignalName.Plc_ScanResult);
            string strPrdCount = GetPlcSignalValue(PlcSignalName.Plc_ProductCount);
            string strOId = GetPlcSignalValue(PlcSignalName.Plc_GroupOID);
            string strMotorStart = GetPlcSignalValue(PlcSignalName.Plc_MotorStart);
            string strIsUseScanner = GetPlcSignalValue(PlcSignalName.Plc_IsUseScanner);

            string strPosition1 = GetPlcSignalValue(PlcSignalName.Plc_Position1);
            string strPosition2 = GetPlcSignalValue(PlcSignalName.Plc_Position2);
            string des = GetPlcSignalValue(PlcSignalName.Plc_ScanDes);
            string message =
                string.Format(@"开始信号{0}，结束信号{1}，扫描结果{2}，纸卷数量{3}，组合ID{4},扫描仪下降{5}，扫描仪投入{6},位置信息 1:{7},2:{8},去向{9}",
                strStartSignal, strEndSignal, strScanResult, strPrdCount, strOId, 
                strMotorStart, strIsUseScanner, strPosition1, strPosition2, des);
            if (!oldPlcInfo.EqualIgnorCase(message))
            {
                if (strIsUseScanner == "86")
                {
                    SendMessageHandler(Const_StockInAutoScan.Head_Control_ButtonUseScanner, "点击投入扫描仪");
                }
                else
                {
                    SendMessageHandler(Const_StockInAutoScan.Head_Control_ButtonUseScanner, "点击不投入扫描仪");
                }
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FPlcInfo, message);
                oldPlcInfo = message;
            }

        }

        private PLCSignalItem GetPlcSignalItem(string strSignalName)
        {
            if (m_PlcService.PLCSignals[m_Plc_StationName, m_Plc_TemplateName, strSignalName] == null) return null;
            return m_PlcService.PLCSignals[m_Plc_StationName, m_Plc_TemplateName, strSignalName];

        }

        private string GetPlcSignalValue(string strSignalName)
        {
            //开始信号
            PLCSignalItem item = GetPlcSignalItem(strSignalName);
            if (item == null) return "0";

            return PLCAccessor.ReadPLCSignalValue(item, m_PlcBuffer);
        }


        /// <summary>
        /// 有关PLC返回的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void M_PlcService_OnPLCMessageChanged(object sender, PLCEventArgument args)
        {
            Logger.Log("PLC返回信息" + args.RetMessage);
        }

        /// <summary>
        /// 设置扫描仪参数
        /// </summary>
        private void SetScanParams()
        {
            if (m_Scanner != null)
            {
                m_Scanner.Write("SET DECODER.REREAD-NEVER2X OFF");//不读取重复条码
                m_Scanner.Write("SET TRIGGER.TYPE 4");           //内部
            }
        }


        private void OpenScannerLED()
        {
            if (m_Scanner != null)
            {
                m_Scanner.Write("TRIGGER ON");           //打开

                m_Scanner.Write("SET DECODER.REREAD-NEVER2X ON");//不读取重复条码

                m_Scanner.Write("SET TRIGGER.TYPE 4");           //内部

                SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus, "扫描仪已打开");
            }
        }

        private void CloseScannerLED()
        {
            if (m_Scanner != null)
            {
                m_Scanner.Write("SET DECODER.REREAD-NEVER2X ON");//不读取重复条码
                m_Scanner.Write("SET TRIGGER.TYPE 0");           //内部
                m_Scanner.Write("TRIGGER OFF");
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus, "扫描仪已关闭");
            }
        }

        /// <summary>
        /// 扫描反馈的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="strMsg"></param>
        private void M_Scanner_OnRunMessage(object sender, string strMsg)
        {
        }

        /// <summary>
        /// 扫描的条码
        /// </summary>
        /// <param name="strReadValue">条码</param>
        private void M_Scanner_OnBarcodeValue(string strReadValue)
        {
            BarcodeCollectionInstance.AddBarcode(strReadValue);
        }

        private void CloseScanner()
        {
            if (m_Scanner != null)
            {
                m_Scanner.Disconnect();
                m_Scanner.OnBarcodeValue -=
                    new Chaint.Common.Devices.Devices.ReadStringArrivedHandler(M_Scanner_OnBarcodeValue);
                m_Scanner.OnRunMessage -=
                    new Chaint.Common.Devices.Devices.RunMessageEventHandler(M_Scanner_OnRunMessage);
                m_Scanner = null;
            }
        }

        private void SendScannBarcodes(IList<string> barcodes)
        {
            if (OnScanningResult != null)
            {
                OnScanningResult(barcodes);
            }
        }

        /// <summary>
        /// 当上一级对象处理完成后
        /// </summary>
        /// <param name="result">86:成功,85:失败</param>
        /// <param name="productGroupOID">组合ID,当失败时传0</param>
        public void AfterProcessCompleted(int result, int productGroupOID, int scanDes)
        {
            SendMessageHandler(Const_StockInAutoScan.Head_Field_FScannerStatus,
                string.Format("发送组合ID-{0},去向-{1}", productGroupOID, scanDes, result));

            m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_GroupOID), productGroupOID);
            if (m_Stereo == "1")
            {
                //去向：200(人工仓库),100立体库
                m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_ScanDes), scanDes);
            }
            m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_ScanResult), result);
            if (m_Stereo == "1")
            {
                Logger.Log("BarCodeProcess",
                    string.Format("{0}向PLC发送处理结果，扫描结果({1}),组合ID({2}),去向({3})",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), result, productGroupOID, scanDes));
            }
            else
            {
                Logger.Log("BarCodeProcess",
                    string.Format("{0}向PLC发送处理结果，扫描结果({1}),组合ID({2})",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), result, productGroupOID));
            }
        }

        public void ScannerMotoStart()
        {
            m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_MotorStart), 86);
            SendMessageHandler(Const_StockInAutoScan.Head_Field_FMonitor, "扫描仪人工下降");
        }
        public void UseScanner(bool isUse)
        {
            int signal = isUse ? 86 : 0;
            m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_IsUseScanner), signal);
            if (isUse)
            {
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FMonitor, "扫描仪不投入");
            }
            else
            {
                SendMessageHandler(Const_StockInAutoScan.Head_Field_FMonitor, "扫描仪投入");
            }
        }

        public void TriggerPLCUseScanner()
        {
            string strPlcValue = GetPlcSignalValue(PlcSignalName.Plc_IsUseScanner);
            if (strPlcValue == "86")
            {
                m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_IsUseScanner), 0);
                Logger.Log(string.Format("********************{0}发送扫描仪投入信号(0)********************", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                // SendMessageHandler(Const_StockInAutoScan.Head_Control_ButtonUseScanner, "点击不投入扫描仪");
            }
            else
            {
                m_PlcService.WritePLCSignalValue(GetPlcSignalItem(PlcSignalName.Plc_IsUseScanner), 86);
                Logger.Log(string.Format("********************{0}发送扫描仪投入信号(86)********************", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                //  SendMessageHandler(Const_StockInAutoScan.Head_Control_ButtonUseScanner, "点击投入扫描仪");
            }
        }


        public string GetUseScannerValue()
        {
            return GetPlcSignalValue(PlcSignalName.Plc_IsUseScanner);
        }
        public void OpenScanner(bool isOpen)
        {
            if (isOpen)
            {
                OpenScannerLED();
            }
            else
            {
                CloseScannerLED();
            }
        }

    }



}
