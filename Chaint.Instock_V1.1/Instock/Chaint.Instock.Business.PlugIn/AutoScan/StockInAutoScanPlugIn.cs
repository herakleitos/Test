using System;
using System.Linq;
using System.Threading;
using DevExpress.DataAccess.Sql.DataApi;
using Chaint.Common.BasePlugIn;
using Chaint.Instock.Core;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Const;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors;
using Chaint.Instock.Business;
using System.IO.Ports;
using Chaint.Instock.Business.View;
using System.Text;
using Chaint.Common.Devices.PLC;
using System.Collections.Generic;
using Chaint.Common.Core.AppConfig;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Chaint.Instock.Business.PlugIns
{
    public partial class StockInAutoScanPlugIn : AbstractBillPlugIn
    {
        private XtraForm form;
        private string factoryId = string.Empty;
        private string[] instockResult;
        private AppConfig_INI appConfiger;
        private AppConfig_INI deviceConfiger;
        private decimal m_maxWidth = 0;
        private decimal m_minWidth = 0;
        private decimal m_maxLength = 0;
        private decimal m_minLength = 0;
        private decimal m_maxWeight = 0;
        private string m_Stereo = "0";
        WH_PDA_Service.PDAForm pdaform = new WH_PDA_Service.PDAForm(false);
        bool isOpenScanner = true;
        private DataTable stockAreaPlan;
        private AutoScanProcessor processer;
        public StockInAutoScanPlugIn(StockInAutoScanView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_StockInAutoScan.Base_Form);
            appConfiger = new AppConfig_INI(view.Context.AppConfigFilePath);
            deviceConfiger = new AppConfig_INI(view.Context.DevicesConfigFilePath);
            string maxWidth = deviceConfiger.GetValue("SCANDESRULE", "MaxWdith", "0");
            string minWidth = deviceConfiger.GetValue("SCANDESRULE", "MinWidth", "0");
            string maxLength = deviceConfiger.GetValue("SCANDESRULE", "MaxLength", "0");
            string minLength = deviceConfiger.GetValue("SCANDESRULE", "MinLength", "0");
            string maxWeight = deviceConfiger.GetValue("SCANDESRULE", "MaxWeight", "0");
            m_maxWidth = Convert.ToDecimal(maxWidth);
            m_minWidth = Convert.ToDecimal(minWidth);
            m_maxLength = Convert.ToDecimal(maxLength);
            m_minLength = Convert.ToDecimal(minLength);
            m_maxWeight = Convert.ToDecimal(maxWeight);
            m_Stereo = deviceConfiger.GetValue("STEREO", "Value", "0");
        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            if (this.Context.CompanyCode != "2")
            {
                //加载人员
                BindOperator();
                BindWHMachine(true, false);
                BindBusinessTypes("in");
                BindShift();
            }
            if (m_Stereo == "0")
            {
                this.View.GetControl<LayoutControlItem>(Const_StockInAutoScan.Head_Control_LayOutDirectionConfig).Visibility =
                    DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>("lc_ForceToManual").Visibility =
                 DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }
        public override void OnBind()
        {
            base.OnBind();
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FDateS, DateTime.Now);
            factoryId = appConfiger.GetValue("DefaultFactory", "Factory", "0");
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FFactory, factoryId);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FBusinessType, "SCRK");
            //test
            //List<int> groupIds = new List<int>();
            //groupIds.Add(123);
            //groupIds.Add(0);
            //List<string> positions = new List<string>();
            //positions.Add("123");
            //positions.Add("0");
            //positions.Add("0");
            //positions.Add("0");
            //positions.Add("0");
            //CommonHelper.GetProductInfoFromBD(this.Context, groupIds, positions, "1");

        }
        /// <summary>
        ///绑定界面数据之后，处理其他事情
        /// </summary>
        public override void AfterBind()
        {
            //测试获取去向的方法
            //int des = GetScanDes("0720717050510104");

            base.AfterBind();
            StockInAutoScanFunc autoScanFunc = new StockInAutoScanFunc(this.Context);
            processer = new AutoScanProcessor(this.Context);
            processer.OnScanningResult += new ScannBarcodeCollectHandler(ProcesBarCode);
            processer.BarcodeCollectionInstance.PorcessBarcode += new ProcessBarcodeHandler(ProcesBarCode);
            processer.RecordScanResultHandler += Processer_RecordScanResult;
            processer.SendMessageHandler += AcceptMessage;
            processer.InitialData(autoScanFunc.plcConnection, autoScanFunc.plcConfig);
        }

        private void Processer_RecordScanResult(int paperCount, IList<string> Barcodes)
        {
            CommonHelper.InsertScanResult(this.Context, paperCount, Barcodes);
        }
        public void ProcesBarCode(IList<string> Barcodes)
        {
            ////当扫描仪上升到位后，如果中间已经发送过信号则直接返回不再处理
            string paperType = deviceConfiger.GetValue("PAPERTYPE", "Type", "2");
            if (paperType == "2")
            {
                processer.OpenScanner(false);
            }
            Logger.Log("BarCodeProcess", string.Format("********************{0}开始处理扫描到的条码********************", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            List<string> result = new List<string>();
            bool isRepeat = false;
            decimal width = 0;
            decimal length = 0;
            decimal weight = 0;
            string productCode = string.Empty;
            foreach (string barcode in Barcodes)
            {
                try
                {
                    //然后向主线程发送扫描结果
                    string[] instockResult = AcceptScannerResult(barcode);
                    if (instockResult[0] == "0")
                    {
                        width = Convert.ToDecimal(instockResult[2].Split('-')[0]);
                        length = Convert.ToDecimal(instockResult[2].Split('-')[1]);
                        weight = Convert.ToDecimal(instockResult[3]);
                        productCode = barcode;
                        result.Add(productCode);
                    }
                    else if (instockResult[0] == "99")
                    {
                        isRepeat = true;
                    }
                }
                catch (Exception ex)
                {
                    isRepeat = true;
                }
            }
            Logger.Log("BarCodeProcess", string.Format("{0}条码入库完成", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            if (isRepeat)
            {
                processer.AfterProcessCompleted(85, (int)Enums_PLC_GroupID.BarCodeRepeat, 200);
            }
            else if (result.Count != Barcodes.Count)
            {
                processer.AfterProcessCompleted(85, (int)Enums_PLC_GroupID.InstockFailed, 200);
            }
            else
            {
                int maxGroupId = CommonHelper.GetMaxGroupId(this.Context, Convert.ToInt32(paperType));
                int nextGroupId = maxGroupId + 1;
                bool isSuccess = CommonHelper.InsertGroupIdMapping(this.Context, nextGroupId, result, Convert.ToInt32(paperType));
                if (isSuccess)
                {
                    //int des = GetScanDes(width, length, weight, productCode);
                    int des = GetScanDes(productCode);
                    processer.AfterProcessCompleted(86, nextGroupId, des);
                }
                else
                {
                    processer.AfterProcessCompleted(85, 1, 200);
                }
            }
        }
        private int GetScanDes(decimal width, decimal length, decimal weight, string barCode)
        {
            int des = 200;
            bool isForceToManual = this.View.GetValue<bool>(Const_StockInAutoScan.Head_Control_ForceToManual);
            if (isForceToManual)
                return des;
            if (width <= 0 || length <= 0 || weight <= 0 || barCode.IsNullOrEmptyOrWhiteSpace())
                return des;

            //产品符合规定长，宽，重量以及自定义过滤条件，则进入立体库
            if (m_maxLength > 0 && m_minLength > 0)
            {
                if (m_maxLength >= length && m_minLength <= length)
                {
                    if (m_maxWidth > 0 && m_minWidth > 0)
                    {
                        if (m_maxWidth >= width && m_minWidth <= width)
                        {
                            if (m_maxWeight > 0 && weight <= m_maxWeight)
                            {

                            }
                        }
                    }
                }
            }
            return des;
        }
        private int GetScanDes(string barCode)
        {
            bool isForceToManual = this.View.GetValue<bool>(Const_StockInAutoScan.Head_Control_ForceToManual);
            if (isForceToManual)
                return 200;
            string sql0 = @" select top 1 OnlyID,ISNULL(WidthUnit,'') as WidthUnit,WidthLabel,LenghtLabel,WeightLabel
                                    from [dbo].[Sheet_Product] where SheetID=@SheetID ";
            List<SqlParameter> parameters0 = new List<SqlParameter>();
            SqlParameter para0 = new SqlParameter("@SheetID", DbType.String);
            para0.Value = barCode;
            parameters0.Add(para0);
            string[] tableName0 = new string[] { "T1" };
            OperateResults exeResult0 =
                DBAccessServiceHelper.ExcuteQuery(this.Context, sql0,
                tableName0, parameters0.ToArray());
            if (!exeResult0.IsSuccess || exeResult0.ResultData.Tables["T1"].Rows.Count <= 0)
            {
                return 200;
            }
            decimal weightLabel = Convert.ToDecimal(exeResult0.ResultData.Tables["T1"].Rows[0]["WeightLabel"]);
            if (weightLabel > m_maxWeight)
            {
                return 200;
            }
            decimal widthLabel = Convert.ToDecimal(exeResult0.ResultData.Tables["T1"].Rows[0]["WidthLabel"]);
            decimal lengthLabel = Convert.ToDecimal(exeResult0.ResultData.Tables["T1"].Rows[0]["LenghtLabel"]);
            string unit = Convert.ToString(exeResult0.ResultData.Tables["T1"].Rows[0]["WidthUnit"]);
            GetRealNumber(unit, ref widthLabel, ref lengthLabel);
            if (widthLabel < m_minWidth || widthLabel > m_maxWidth)
            {
                return 200;
            }
            if (lengthLabel < m_minLength || lengthLabel > m_maxLength)
            {
                return 200;
            }
            string customFilter = deviceConfiger.GetValue("SCANDESRULE", "CustomFilter", "");
            if (customFilter.IsNullOrEmptyOrWhiteSpace())
            {
                return 100;
            }
            string sql = @" select OnlyID
                                    from [dbo].[Sheet_Product] where SheetID=@SheetID and {0} ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter para = new SqlParameter("@SheetID", DbType.String);
            para.Value = barCode;
            parameters.Add(para);
            string[] tableName = new string[] { "T1" };
            OperateResults exeResult =
                DBAccessServiceHelper.ExcuteQuery(this.Context, string.Format(sql, customFilter),
                tableName, parameters.ToArray());
            if (!exeResult.IsSuccess || exeResult.ResultData.Tables["T1"].Rows.Count > 0)
            {
                return 200;
            }
            else
            {
                return 100;
            }
        }

        private void GetRealNumber(string unit, ref decimal widthLabel, ref decimal lengthLabel)
        {
            if (unit.IsNullOrEmptyOrWhiteSpace()) return;
            string newUnit = unit.Split('x')[0].Trim();
            switch (newUnit)
            {
                case "cm":
                    widthLabel = widthLabel * 10;
                    lengthLabel = lengthLabel * 10;
                    break;
                case "inch":
                    widthLabel = widthLabel * 25.4m;
                    lengthLabel = lengthLabel * 25.4m;
                    break;
                case "mm":
                default:
                    break;
            }
            GetRealSpecification(ref widthLabel,ref lengthLabel);
        }
        private void GetRealSpecification(ref decimal widthLabel, ref decimal lengthLabel)
        {
            string sql = @" select ActualWidth,ActualLength
                            from [dbo].[BD_SpecialSpecification] 
                            where LabelWdith=@LabelWdith and LabelLength =@LabelLength ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter para1 = new SqlParameter("@LabelWdith", DbType.Int32);
            para1.Value = widthLabel;
            parameters.Add(para1);
            SqlParameter para2 = new SqlParameter("@LabelLength", DbType.Int32);
            para2.Value = lengthLabel;
            parameters.Add(para2);
            string[] tableName = new string[] { "T1" };
            OperateResults exeResult =
                DBAccessServiceHelper.ExcuteQuery(this.Context, sql,
                tableName, parameters.ToArray());
            if (!exeResult.IsSuccess || exeResult.ResultData.Tables["T1"].Rows.Count <= 0)
            {
                return;
            }
            widthLabel = Convert.ToDecimal(exeResult.ResultData.Tables["T1"].Rows[0]["ActualWidth"]);
            lengthLabel = Convert.ToDecimal(exeResult.ResultData.Tables["T1"].Rows[0]["ActualLength"]);
        }
        private void Helper_OnPLCMessageChanged(object sender, PLCEventArgument args)
        {

        }
        //插件的dispose事件在页面的dispose之前执行
        public override void OnDispose()
        {
            base.OnDispose();
            pdaform.DisposeData();
        }
        public override void OnClose()
        {
            base.OnClose();
            processer.Dispose();
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            ButtonEdit scanBox = this.View.GetControl<ButtonEdit>(Const_StockInAutoScan.Head_Field_FBarcode);
            switch (e.Sender)
            {
                case Const_StockInAutoScan.Head_Field_FBarcode:
                    string paperType = deviceConfiger.GetValue("PAPERTYPE", "Type", "2");
                    string barcode = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FBarcode);
                    if (!barcode.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (this.Context.CompanyCode == "1")
                        {
                            instockResult = ProcessInstock();
                        }
                        else if (this.Context.CompanyCode == "2")
                        {
                            instockResult = ProcessInstockWanGuo(paperType);
                        }
                    }
                    break;
                case Const_StockInAutoScan.Head_Control_ButtonMotorStart:
                    DialogResult resultA = ChaintMessageBox.ShowConfirmDialog("是否确认操作？");
                    if (resultA == DialogResult.Yes)
                    {
                        processer.ScannerMotoStart();
                    }
                    break;
                case Const_StockInAutoScan.Head_Control_ButtonUseScanner:
                    DialogResult resultB = ChaintMessageBox.ShowConfirmDialog("是否确认操作？");
                    if (resultB == DialogResult.Yes)
                    {
                        processer.TriggerPLCUseScanner();
                    }
                    break;
                case Const_StockInAutoScan.Head_Control_ButtonDirectionConfig:
                    //int des = GetScanDes("0720717050510104");

                    this.View.Open(Const_Option.Const_DirectionConfig, null);
                    break;
                case Const_StockInAutoScan.Head_Control_ButtonOpenScanner:
                    DialogResult resultC = ChaintMessageBox.ShowConfirmDialog("是否确认操作？");
                    if (resultC == DialogResult.Yes)
                    {
                        processer.OpenScanner(isOpenScanner);
                        if (isOpenScanner)
                        {
                            this.View.SetValue(Const_StockInAutoScan.Head_Control_ButtonOpenScanner,
                                "关闭扫描仪");
                        }
                        else
                        {
                            this.View.SetValue(Const_StockInAutoScan.Head_Control_ButtonOpenScanner,
                                "打开扫描仪");
                        }
                        isOpenScanner = !isOpenScanner;
                    }
                    break;
            }
            scanBox.Focus();
        }

        public override void DataReceived(DataReceivedEventArgs e)
        {
            base.DataReceived(e);
            PortDataReceived(e);
        }
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.Sender)
            {
                case Const_StockInAutoScan.Head_Field_FBarcode:
                    ClearOldInfomation();
                    break;
            }
        }

        #region private fun
        /// <summary>
        /// 接受扫描结果
        /// </summary>
        /// <param name="result"></param>
        private string[] AcceptScannerResult(string result)
        {
            SetText(result, "");
            return instockResult;
        }
        private void AcceptMessage(string key, string message)
        {
            if (key == "SendMessageHandler")
            {
                return;
            }
            string formatMsg = message;
            if (key == Const_StockInAutoScan.Head_Field_FMonitor)
            {
                formatMsg = string.Format("{0}-{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
            }
            SetMessage(key, formatMsg);
        }
        private void ClearOldInfomation()
        {
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FMaterial, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FGrade, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpecification, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FDiameterOrSlides, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FLength, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FDirection, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FLayers, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FWeightMode, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FWeight, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FCount, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FCertification, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FSku, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpecCustName, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpecProdName, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FPoNumber, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FRemark, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FTrademarkStyle, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FTransportType, string.Empty);

            //新增属性
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpCustomer, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FColor, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FPlanDiameter, string.Empty);
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FPlanLength, string.Empty);
        }
        private void ShowNewInformation(string result)
        {
            string[] strs = result.TrimStart(Const_WMSMessage._StartChar)
                .TrimEnd(Const_WMSMessage._EndChar).Split(Const_WMSMessage._SpliteChar);
            var resultField =
                 this.View.GetControl<TextEdit>(Const_StockInAutoScan.Head_Field_FResult);
            if (strs[1] == "IRA02" || strs[1] == "IPA02" || strs[1] == "IRA04" || strs[1] == "IPA04")
            {
                if (strs.Length > 5)
                {
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FMaterial, strs[6]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FGrade, strs[7]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpecification, strs[8]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FDiameterOrSlides, strs[30]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FCoreDiameterOrReam, strs[10]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FLength, strs[11]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FDirection, strs[12]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FLayers, strs[13]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FWeightMode, strs[14]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FWeight, strs[16]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FCount, strs[17]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FCertification, strs[18]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FSku, strs[19]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpecProdName, strs[20]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpecCustName, strs[21]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FPoNumber, strs[23]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FRemark, strs[24]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FTrademarkStyle, strs[25]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FPackType, strs[26]);
                    this.View.SetValue(Const_StockInAutoScan.Head_Field_FTransportType, strs[27]);

                    if (strs.Length >= 34)
                    {
                        //新增属性
                        this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpCustomer, strs[22]);//产品备注-CustTrademark
                        this.View.SetValue(Const_StockInAutoScan.Head_Field_FColor, strs[31]);//色相-Color
                        this.View.SetValue(Const_StockInAutoScan.Head_Field_FPlanDiameter, strs[33]);//计划直径-DiameterLabel
                        this.View.SetValue(Const_StockInAutoScan.Head_Field_FPlanLength, strs[32]);//计划线长-LengthLabel
                    }
                }
                if (strs[2] == "0")
                    resultField.ForeColor = System.Drawing.Color.Blue;
                else
                    resultField.ForeColor = System.Drawing.Color.Red;
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FResult, strs[3]);
            }
            else if (strs[1] == "IRA03" || strs[1] == "IPA03" || strs[1] == "IRA05" || strs[1] == "IPA05")
            {
                if (strs[2] == "0")
                    resultField.ForeColor = System.Drawing.Color.Blue;
                else
                    resultField.ForeColor = System.Drawing.Color.Red;
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FResult, strs[3]);
            }
        }
        #endregion
    }
}
