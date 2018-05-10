using System;
using System.Text;
using System.IO.Ports;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Core.Utils;
using Chaint.Instock.Core;
using Chaint.Common.Core.Log;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Const;

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockInAutoScanPlugIn
    {
        private StringBuilder sbBarCode = new StringBuilder();
        private delegate void deleSetText(string text, string color);
        private delegate void deleSetMessage(string key, string message);
        /// <summary>
        /// 扫描后处理入库
        /// </summary>
        public string[] ProcessInstock()
        {
            string[] result;
            //判断该填的东西是否都全了
            if (!this.CheckWHInfo())
                return new string[] { "9","仓库信息未录入"};
            //发送条形码
            string barcode = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FBarcode);
            string emp = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FOperator).Split('.')[0];
            string trademode = "";
            string iswhite = "";
            string remark = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FWHRemark);
            string org = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FFactory).Split('.')[0];
            string businesstype = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FBusinessType).Split('.')[0];
            string whcode = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FWareHouse).Split('.')[0];
            string shift = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FShift).Split('.')[0];
            string shiftTime = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FShiftTime);
            DateTime date = this.View.GetValue<DateTime>(Const_StockInAutoScan.Head_Field_FDateS);
            string strDate = date == null ? string.Empty : date.ToString("yyyy-MM-dd HH:mm:ss");
            bool isCancel = false;
            ListBoxControl msgField =
                this.View.GetControl<ListBoxControl>(Const_StockInAutoScan.Head_Field_FMessage);
            switch (businesstype)
            {
                case "HDRK": //红单的情况
                    if (isCancel)
                    {
                        //取消红单入库
                        result = RedCancel(barcode, emp, msgField);
                    }
                    else
                    {
                        //红单入库
                        result = RedIn(barcode, date, businesstype, org, whcode, emp,
                            shift, shiftTime, remark, iswhite, trademode, msgField);
                    }
                    break;
                default: //蓝单的情况
                    if (isCancel)
                    {
                        //取消蓝单入库
                        result = BlueCancel(barcode, emp, msgField);
                    }
                    else
                    {
                        //蓝单入库
                        result = BlueIn(barcode, date, businesstype, org, whcode, emp,
                             shift, shiftTime, remark, iswhite, trademode, msgField);
                    }
                    break;
            }

            if (result[0] == "0" && !isCancel)//如果执行成功并且不是取消操作
            {
                LoadStockAreaPlan();
                MatchStockArea(barcode);
            }
            return result;
        }
        private string[] ProcessInstockWanGuo(string paperType)
        {
            string barcode = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FBarcode);
            string materialCode = GetProductInfo(barcode, paperType);
            if (materialCode.IsNullOrEmptyOrWhiteSpace())
                return new string[] {"9","无产品信息" };
            LoadStockAreaPlan();
            string stockArea = MatchStockArea(barcode);
            InsertObject InsertObj = new InsertObject();
            InsertObj.TableName = "CT_AutoScan_Info";
            InsertObj.AddInsertItem(new InsertItem("[LineNO]",
                "@LineNO", factoryId, Enums_FieldType.String));
            InsertObj.AddInsertItem(new InsertItem("ProductType",
              "@ProductType", paperType, Enums_FieldType.String));
            InsertObj.AddInsertItem(new InsertItem("ProductID",
                "@ProductID", barcode, Enums_FieldType.String));
            InsertObj.AddInsertItem(new InsertItem("AreaCode",
                "@AreaCode", stockArea, Enums_FieldType.String));
            InsertObj.AddInsertItem(new InsertItem("Date_Write",
                "@Date_Write", DateTime.Now, Enums_FieldType.DateTime));
            InsertObj.AddInsertItem(new InsertItem("Flag",
                "@Flag","N", Enums_FieldType.String));
            string insertSql = InsertObj.ToSqlString();
            this.Context.Section = Const_ConfigSection.MingRenSection;
            OperateResult excuteResult =
                DBAccessServiceHelper.ExcuteNonQuery(this.Context, insertSql, InsertObj.QueryParameters.ToArray());
            this.Context.Section = Const_ConfigSection.MainSection;
            if (!excuteResult.IsSuccess)
            {
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FResult,"数据插入鸣人系统失败");
                return new string[] { "9", "入库失败" };
            }
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FResult, "数据插入鸣人系统成功");
            return new string[] { "0", "操作成功" };
        }

        private string GetProductInfo(string barCode,string paperType)
        {
            bool isRoll = true;
            if (paperType == "1")
            {
                isRoll = true;
            }
            else
            {
                isRoll = false;
            }

            string sql = string.Empty;
            if (isRoll)
            {
                sql = @"SELECT ROLLID AS BARCODE,SC.SPECCUSTNAME,RP.MATERIALCODE,WL.WLMC AS MATERIALNAME,
                        RP.REMARK AS REMARK FROM ROLL_PRODUCT RP
                        LEFT JOIN CT_WLZD WL ON RP.MATERIALCODE = WL.WLBH
                        LEFT JOIN PAPER_SPECCUSTNAME SC ON RP.SPECCUSTNAME = SC.ONLYID
                        WHERE RP.ROLLID = @BARCODE";
            }
            else
            {
                sql = @"SELECT SHEETID AS BARCODE,SC.SPECCUSTNAME,SP.MATERIALCODE,WL.WLMC AS MATERIALNAME,
                        SP.SHEETREMARK AS REMARK FROM SHEET_PRODUCT SP
                        LEFT JOIN CT_WLZD WL ON SP.MATERIALCODE = WL.WLBH
                        LEFT JOIN PAPER_SPECCUSTNAME SC ON SP.SPECCUSTNAME = SC.ONLYID
                        WHERE SP.SHEETID = @BARCODE";
            }
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter para = new SqlParameter("@BARCODE", DbType.String);
            para.Value = barCode;
            parameters.Add(para);
            string[] tableName = new string[] { "T1" };
            OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
            if (result.IsSuccess && result.ResultData.Tables["T1"].Rows.Count > 0)
            {
                string materialCode = Convert.ToString(result.ResultData.Tables["T1"].Rows[0]["MATERIALCODE"]);
                string materialName = Convert.ToString(result.ResultData.Tables["T1"].Rows[0]["MATERIALNAME"]);
                string customer = Convert.ToString(result.ResultData.Tables["T1"].Rows[0]["SPECCUSTNAME"]);
                string remark = Convert.ToString(result.ResultData.Tables["T1"].Rows[0]["REMARK"]);
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FMaterial, materialName);
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FSpecCustName, customer);
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FRemark, remark);
                return materialCode;
            }
            else
            {
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FResult,"未找到产品信息");
                return string.Empty;
            }
        }
        /// <summary>
        /// 检查入库的仓库信息是否全了
        /// </summary>
        /// <returns></returns>
        private bool CheckWHInfo()
        {
            string user = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FOperator);
            if (user.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请选择入库人员。");
                return false;
            }
            string shift = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FShift);
            if (shift.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请选择入库班组。");
                return false;
            }
            string shifttime = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FShiftTime);
            if (shifttime.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请选择入库班次。");
                return false;
            }
            DateTime time = this.View.GetValue<DateTime>(Const_StockInAutoScan.Head_Field_FDateS);
            if (time.CompareTo(DateTime.Now) > 0)
            {
                ChaintMessageBox.Show("交班时间不能晚于系统时间。");
                return false;
            }
            string business = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FBusinessType);
            if (business.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请选择业务类型。");
                return false;
            }
            return true;
        }
        private void PortDataReceived(DataReceivedEventArgs e)
        {
            var spScan = this.View.GetControl<SerialPort>(Const_StockInAutoScan.Base_Port);
            int Length = spScan.BytesToRead;
            byte[] receive = new byte[Length];
            spScan.Read(receive, 0, Length);
            foreach (byte b in receive)
            {
                if (b <= '9' && b >= '0' || b <= 'z' && b >= 'a' || b <= 'Z' && b >= 'A')
                {
                    sbBarCode.Append((char)b);
                }
                if (b == 13) //表示字符串接受结束
                {
                    if (sbBarCode.Length >= 10)  //10 11
                    {
                        SetText(sbBarCode.ToString(), "");
                    }
                    sbBarCode = new StringBuilder();
                }
            }
        }
        private void SetText(string text, string color)
        {
            var barCodeField = this.View.GetControl<ButtonEdit>(Const_StockInAutoScan.Head_Field_FBarcode);
            if (barCodeField.InvokeRequired)
            {
                deleSetText setText = new deleSetText(SetText);
                form.Invoke(setText, new object[] { text, "" });
            }
            else
            {
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FBarcode, string.Empty);
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FBarcode, text);
                DataChangedEventArgs args = new DataChangedEventArgs();
                args.Sender = Const_StockInAutoScan.Head_Field_FBarcode;
                this.DataChanged(args);
                ButtonClickEventArgs clickArgs = new ButtonClickEventArgs();
                clickArgs.Sender = Const_StockInAutoScan.Head_Field_FBarcode;
                this.ButtonClick(clickArgs);
            }
        }
        private void SetMessage(string key, string message)
        {
            if (key == Const_StockInAutoScan.Head_Field_FMonitor)
            {
                var field = this.View.GetControl<ListBoxControl>(key);
                if (field.InvokeRequired)
                {
                    deleSetMessage setMsg = new deleSetMessage(SetMessage);
                    form.Invoke(setMsg, new object[] { key, message });
                }
                else
                {
                    Logger.Log(message);
                    if (field.Items.Count >= 10)
                    {
                        field.Items.Clear();
                    }
                    field.Items.Insert(0, message);
                }
            }
            else if (key == Const_StockInAutoScan.Head_Control_ButtonUseScanner)
            {
                var field = this.View.GetControl<SimpleButton>(key);
                if (field.InvokeRequired)
                {
                    deleSetMessage setMsg = new deleSetMessage(SetMessage);
                    form.Invoke(setMsg, new object[] { key, message });
                }
                else
                {
                    this.View.SetValue(key, message);
                }
            }
            else if (key == Const_StockInAutoScan.Head_Field_FBarcode)
            {
                var field = this.View.GetControl<ButtonEdit>(key);
                if (field.InvokeRequired)
                {
                    deleSetMessage setMsg = new deleSetMessage(SetMessage);
                    form.Invoke(setMsg, new object[] { key, message });
                }
                else
                {
                    this.View.SetValue(key, message);
                }
            }
            else if (key == Const_StockInAutoScan.Head_Field_FResult)
            {
                var field = this.View.GetControl<TextEdit>(key);
                if (field.InvokeRequired)
                {
                    deleSetMessage setMsg = new deleSetMessage(SetMessage);
                    form.Invoke(setMsg, new object[] { key, message });
                }
                else
                {
                    this.View.SetValue(key, message);
                }
            }
            else
            {
                var field = this.View.GetControl<LabelControl>(key);
                if (field.InvokeRequired)
                {
                    deleSetMessage setMsg = new deleSetMessage(SetMessage);
                    form.Invoke(setMsg, new object[] { key, message });
                }
                else
                {
                    this.View.SetValue(key, message);
                }
            }
        }
    }
}
