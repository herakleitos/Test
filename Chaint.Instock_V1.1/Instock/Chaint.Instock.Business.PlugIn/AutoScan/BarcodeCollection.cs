using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaint.Common.Core.AppConfig;
using Chaint.Common.Core;

namespace Chaint.Instock.Business.PlugIns
{
    public delegate void ProcessBarcodeHandler(IList<string> Barcodes);
    public class BarcodeCollection
    {
        public event ProcessBarcodeHandler PorcessBarcode;
        private AppConfig_INI deviceConfiger;
        private IList<string> m_lstBarcodes = new List<string>();

        public IList<string> LstBarcodes
        {
            get { return m_lstBarcodes; }
            set { m_lstBarcodes = value; }
        }
        public BarcodeCollection(Context ctx)
        {
            deviceConfiger = new AppConfig_INI(ctx.DevicesConfigFilePath);
        }
        /// <summary>
        /// 条码数量
        /// </summary>
        public int BarcodeCount
        {
            get
            {
                lock(this)
                {
                    if (m_lstBarcodes == null) return 0;
                    return m_lstBarcodes.Count;
                }
            }
        }

        public void AddBarcode(string strBarcode)
        {
            if (strBarcode.Trim().Length < 16) return;
            System.Text.RegularExpressions.Regex rex =
                new System.Text.RegularExpressions.Regex(@"^\d+$");
            if(!rex.IsMatch(strBarcode))
            {
                return;
            }
            string barCode = strBarcode.Trim();
            lock (this)
            {
                if (!this.LstBarcodes.Contains(barCode))
                {
                    this.LstBarcodes.Add(barCode);
                    string paperType = deviceConfiger.GetValue("PAPERTYPE", "Type", "2");
                    if (paperType == "2")
                    {
                        PorcessBarcode(this.LstBarcodes);
                    }
                }
            }
        }


        public void ClearBarcodes()
        {
            lock(this)
            {
                this.LstBarcodes.Clear();
            }
        }

      




    }
}
