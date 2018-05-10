using System;
using DevExpress.XtraEditors;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.Utils;
using Chaint.Instock.Core;

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockInAutoScanPlugIn
    {
        public string[] RedCancel(string barcode, string emp, ListBoxControl msgField)
        {
            string command = "";
            string[] ir03 = new string[] { command, barcode, emp,
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            string msg = Message.MakeWMSSocketMsg(ir03);
            string[] strs = msg.TrimStart(Const_WMSMessage._StartChar).TrimEnd(Const_WMSMessage._EndChar).Split(Const_WMSMessage._SpliteChar);
            string result = "";
            if (barcode.Substring(2, 1) == "1")
            {
                command = "IR05";
                result = pdaform.Process_IR05(strs);
            }
            else if (barcode.Substring(2, 1) == "2")
            {
                command = "IP05";
                result = pdaform.Process_IP05(strs);
            }
            else
            {
                ShowErrorBarCode();
                return new string[] { "9", "条形码错误" };
            }
            this.ShowNewInformation(result);
            if (msgField.Items.Count > 1000)
            {
                msgField.Items.Clear();
            }
            string message =
                string.Format("{0}|{1}|{2}", DateTime.Now.ToString("HH:mm:ss"), barcode, "取消红单入库");
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FMessage, message);
            string[] formatResult = result.TrimStart(Const_WMSMessage._StartChar)
                .TrimEnd(Const_WMSMessage._EndChar).Split(Const_WMSMessage._SpliteChar);
            return new string[] { formatResult[2], formatResult[3] };
        }
        public string[] RedIn(string barcode, DateTime date, string businesstype, string org,
            string whcode, string emp, string shift, string shiftTime, string remark,
            string iswhite, string trademode, ListBoxControl msgField)
        {
            string command = "";
            string[] ir02 = new string[] { command, barcode,
                            date.ToString("yyyy-MM-dd HH:mm:ss"), businesstype, "",
                            org, whcode, emp, "", shift, shiftTime, remark, iswhite, trademode };
            string msg = Message.MakeWMSSocketMsg(ir02);
            string[] strs =
                msg.TrimStart(Const_WMSMessage._StartChar).TrimEnd(Const_WMSMessage._EndChar).Split(Const_WMSMessage._SpliteChar);
            string result = "";
            if (barcode.Substring(2, 1) == "1")
            {
                command = "IR04";
                result = pdaform.Process_IR04(strs);
            }
            else if (barcode.Substring(2, 1) == "2")
            {
                command = "IP04";
                result = pdaform.Process_IP04(strs);
            }
            else
            {
                ShowErrorBarCode();
                return new string[] { "9", "条形码错误" };
            }
            this.ShowNewInformation(result);
            if (msgField.Items.Count > 1000)
            {
                msgField.Items.Clear();
            }
            string message =
                string.Format("{0}|{1}|{2}", DateTime.Now.ToString("HH:mm:ss"), barcode, "红单扫描入库");
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FMessage, message);
            string[] formatResult = result.TrimStart(Const_WMSMessage._StartChar)
                 .TrimEnd(Const_WMSMessage._EndChar).Split(Const_WMSMessage._SpliteChar);
            return new string[] { formatResult[2], formatResult[3] };
        }

        public string[] BlueCancel(string barcode, string emp, ListBoxControl msgField)
        {
            string command = "";
            if (barcode.Substring(2, 1) == "1")
                command = "IR03";
            else if (barcode.Substring(2, 1) == "2")
                command = "IP03";
            else
            {
                ShowErrorBarCode();
                return new string[] { "9", "条形码错误" };
            }
            string[] ir03 = new string[] { command, barcode, emp,
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            string msg = Message.MakeWMSSocketMsg(ir03);
            string[] strs =
                msg.TrimStart(Const_WMSMessage._StartChar).TrimEnd(Const_WMSMessage._EndChar).
                Split(Const_WMSMessage._SpliteChar);
            string result = "";
            if (command == "IR03")
            {
                result = pdaform.Process_IR03(strs);
            }
            else if (command == "IP03")
            {
                result = pdaform.Process_IP03(strs);
            }
            this.ShowNewInformation(result);
            if (msgField.Items.Count > 1000)
            {
                msgField.Items.Clear();
            }
            string message =
               string.Format("{0}|{1}|{2}", DateTime.Now.ToString("HH:mm:ss"), barcode, "取消入库");
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FMessage, message);
            string[] formatResult = result.TrimStart(Const_WMSMessage._StartChar)
                .TrimEnd(Const_WMSMessage._EndChar).Split(Const_WMSMessage._SpliteChar);
            return new string[] { formatResult[2], formatResult[3] };
        }
        public string[] BlueIn(string barcode, DateTime date, string businesstype, string org,
            string whcode, string emp, string shift, string shiftTime, string remark,
            string iswhite, string trademode, ListBoxControl msgField)
        {
            string command = "";
            if (barcode.Substring(2, 1) == "1")
                command = "IR02";
            else if (barcode.Substring(2, 1) == "2")
                command = "IP02";
            else
            {
                ShowErrorBarCode();
                return new string[] { "9","条形码错误"};
            }
            string[] ir02 = new string[] { command, barcode,
                            date.ToString("yyyy-MM-dd HH:mm:ss"), businesstype, "",
                            org, whcode, emp, "", shift, shiftTime, remark, iswhite, trademode };
            string msg = Message.MakeWMSSocketMsg(ir02);
            string[] strs = msg.TrimStart(Const_WMSMessage._StartChar).TrimEnd(Const_WMSMessage._EndChar).
                Split(Const_WMSMessage._SpliteChar);
            string result = "";
            if (command == "IR02")
                result = pdaform.Process_IR02(strs);
            else if (command == "IP02")
                result = pdaform.Process_IP02(strs);
            this.ShowNewInformation(result);
            if (msgField.Items.Count > 1000)
            {
                msgField.Items.Clear();
            }
            string message =
               string.Format("{0}|{1}|{2}", DateTime.Now.ToString("HH:mm:ss"), barcode, "扫描入库");
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FMessage, message);
            string[] formatResult = result.TrimStart(Const_WMSMessage._StartChar)
                .TrimEnd(Const_WMSMessage._EndChar).Split(Const_WMSMessage._SpliteChar);
            if (formatResult.Length > 17)
            {
                //入库结果： 是否成功代码，描述，规格
                return new string[] { formatResult[2], formatResult[3], formatResult[8], formatResult[16] };
            }
            else
            {
                //入库结果： 是否成功代码，描述，规格
                return new string[] { formatResult[2], formatResult[3],"0", "0"};
            }
        }
        private void ShowErrorBarCode()
        {
            var resultField =
                this.View.GetControl<TextEdit>(Const_StockInAutoScan.Head_Field_FResult);
            resultField.ForeColor = System.Drawing.Color.Red;
            this.View.SetValue(Const_StockInAutoScan.Head_Field_FResult, "条形码错误。");
        }
    }
}
