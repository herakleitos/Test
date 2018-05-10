using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DataModel;
using CTWH.Common;

namespace CTWH.StockOut
{
    public partial class U_StockOutUploadList : DevExpress.XtraEditors.XtraUserControl
    {

        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        MainLayout _MainLayout = null;
        public string _VourcherNO = "";
        WMSDS _WMS_MAIN;
        public U_StockOutUploadList()
        {
            InitializeComponent();
        }
        public U_StockOutUploadList(MainLayout tl)
        {
            InitializeComponent();
            this._MainLayout = tl;
            this._MainLayout.OnMenuClickEvent += new MenuClickEventHandle(OnMenuClickEventProcess);
            this.Disposed += new EventHandler(U_VoucherPlanAdd_Disposed);

        }
        void U_VoucherPlanAdd_Disposed(object sender, EventArgs e)
        {
            this._MainLayout.OnMenuClickEvent -= new MenuClickEventHandle(OnMenuClickEventProcess);
        }
        private void DisposeData()
        {
            _WMSAccess.SqlStateChange -= new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMSAccess = null;
        }
        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }
        private void btn_Query_Click(object sender, EventArgs e)
        {
            //从out plan 查询单号和发货量
            //string voucherno =this.chk_BillNO.Checked? this.cmb_VoucherNO.Text.Trim():"";
            //string dateStart =this.chk_BillNO.Checked?"": this.dt_Bill.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //string dateEnd = this.chk_BillNO.Checked ? "" : this.dt_end.DateTime.ToString("yyyy-MM-dd HH:mm:ss");

            string voucherno =this.cmb_VoucherNO.Text.Trim() ;
            if (voucherno == "")
            {
                MessageBox.Show("请填写出库单号");
                return;
            }
            string dateStart = "";// this.chk_BillNO.Checked ? "" : this.dt_Bill.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string dateEnd = "";// this.chk_BillNO.Checked ? "" : this.dt_end.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            InterfaceDS planDS = this._WMSAccess.Select_T_OutStockAndEntry_RelationForUpload(voucherno, dateStart, dateEnd);
            this.gridControl1.DataSource = planDS.CT_StockOut;
            this.gridView1.BestFitColumns();
            //string a ="";
            //if (planDS.T_OutStock_Plan_Entry.Rows.Count > 0)
            //    a = this._WMSAccess.Tran_Insert_CT_StockOut(planDS);
            //else
            //    a = "单据"+voucherno+"没有分录";
            //if(a!="")
            //    MessageBox.Show("上传失败："+a);
            //else
            //    MessageBox.Show("上传成功");

        }
        //WH_PDA_Service.PDAForm pdaform = null;

        private void U_StockOutStat_Load(object sender, EventArgs e)
        {
            //pdaform = new WH_PDA_Service.PDAForm(false);

            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMS_MAIN = new WMSDS();
            this.dt_Bill.DateTime = DateTime.Now.AddDays(-1);
            this.dt_end.DateTime = DateTime.Now;
        }
        public void OnMenuClickEventProcess(object sender, MenuClickEventArgs e)
        {

            switch (e.MenuName)
            {
                case Utils.WMSMenu._New:

                    //新建一个发货通知单
                    //if (DialogResult.Yes == MessageBox.Show("确定要新建发货通知单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    //{
                    //    this.NewVoucherPlan();

                    //}
                    break;
                case Utils.WMSMenu._Save:
                    //判断是新建还是更新
                    //WMSDS wmsDS = this._WMSAccess.Select_T_OutStock_PlanByFK(this.cmb_VoucherNO.Text, "", "", "", "", "", "", 0, 0, 0);
                    //if (wmsDS.T_OutStock_Plan.Rows.Count == 0)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("确定要保存发货通知单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    //        this.SaveNewVoucherPlan();
                    //}
                    //else
                    //{
                    //    string date = wmsDS.T_OutStock_Plan.Rows[0]["Biller"].ToString();
                    //    string user = wmsDS.T_OutStock_Plan.Rows[0]["BillDate"].ToString();
                    //    if (DialogResult.Yes == MessageBox.Show(this.cmb_VoucherNO.Text + "已于" + date + "由" + user + "创建，要覆盖吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    //        this.SaveUpdateVoucherPlan(wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.OnlyIDColumn].ToString());
                    //}

                    break;
                case Utils.WMSMenu._Delete:
                    MessageBox.Show("_Delete");
                    break;
                case Utils.WMSMenu._NewLine:
                    //this.AddNewLineToPlan();
                    break;
                case Utils.WMSMenu._DelLine:
                    //this.DeleteSelectedLine();
                    break;
                case Utils.WMSMenu._ReFresh:
                    //if (this.cmb_VoucherNO.Text != "")
                    //{
                    //    LoadEditValue(this.cmb_VoucherNO.Text);
                    //}
                    break;
                case Utils.WMSMenu._Exit:
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭页面吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        this._MainLayout.AddUserControl(null, "");
                    break;
            }

        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
            if(this.gridView1.RowCount>0)
            {
                string inspur = this.gridView1.GetFocusedRowCellDisplayText("InspurVoucherNO").ToString();//this.cmb_VoucherNO.Text;
                string upload = this.gridView1.GetFocusedRowCellDisplayText("IsUpload").ToString();//this.cmb_VoucherNO.Text;

            //从out plan 查询单号和发货量
                string voucherno = this.cmb_VoucherNO.Text;//this.gridView1.GetFocusedRowCellValue("VoucherNO").ToString() ;//

            if (voucherno == "") { MessageBox.Show("单号不存在"); return; }
            if (inspur != "") { MessageBox.Show("单据已上传，不能再次上传"); return; }
            if (upload == "已上传") { MessageBox.Show("单据已上传，不能再次上传"); return; }

            if (!CheckSourceID()) //检查源单分录号
            {
                return;
            }

            if (DialogResult.No == MessageBox.Show("确定要上传出库单" + voucherno + "吗？，单据将自动关闭", "提示信息",MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;
            InterfaceDS planDS = this._WMSAccess.Select_T_OutStockAndEntry_RelationForUpload(voucherno, "", "");
            string a = "";
            if (planDS.CT_StockOut.Rows.Count > 0)
                a = this._WMSAccess.Tran_Insert_CT_StockOut(planDS);
            else
                a = "单据" + voucherno + "没有分录";
            if (a != "")
            {
                
                string Des = string.Format("{0}上传销售出库单{1}失败,分录数目为：{2}", Utils.LoginUserName, cmb_VoucherNO.Text.ToString(), this.gridView1.RowCount.ToString());
                _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", cmb_VoucherNO.Text.ToString());
                

                MessageBox.Show("上传失败：" + a);
            }
            else
            {
                
                string Des = string.Format("{0}上传销售出库单{1}成功,分录数目为：{2}", Utils.LoginUserName, cmb_VoucherNO.Text.ToString(), this.gridView1.RowCount.ToString());
                _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", cmb_VoucherNO.Text.ToString());
                

                MessageBox.Show("上传成功");
            }
            }
            this.btn_Query_Click(null,null);
        }

        private bool CheckSourceID()
        {
            for (int i = 0; i < this.gridView1.RowCount; i++)
            {
                if (gridView1.GetRowCellValue(i, this.gridView1.Columns["BusinessType"]).Equals("LYCK")) continue;
                if (gridView1.GetRowCellValue(i, this.gridView1.Columns["SourceEntryID"]).ToString() == "" || gridView1.GetRowCellValue(i, this.gridView1.Columns["SourceEntryID"]).ToString() == "0")
                {
                    MessageBox.Show("源单分录号存在空或者0,请检查!!");
                    return false;
                }

            }

            return true;
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            //string dt = this.dt_Bill.Text.Trim();
            //string[] ao01 = new string[] { "O10", dt };
            //string amsg = Utils.WMSMessage.MakeWMSSocketMsg(ao01);

            //string[] astrs = amsg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                //string aresult = pdaform.Process_Q07(dt);
            //this.txtMemo.Text = aresult;

            //string[] astrs2 = aresult.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
          //MessageBox.Show( astrs2[3]);

            //if (astrs2[2] == "0")
            //{
            //    this.cmb_VoucherNO.Properties.Items.Clear();
            //    string[] users = astrs2[4].Split(Utils.WMSMessage._ForeachChar);
            //    foreach (string user in users)
            //    {
            //        this.cmb_VoucherNO.Properties.Items.Add(user);
            //    }

            //}
        }

        private void cmb_VoucherNO_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            F_VoucherPlanShow vps = new F_VoucherPlanShow();
            //MainDS ds = access.Warehouse_VoucherQueryByDate_Invo(System.DateTime.Now.AddDays(-100).ToString(), System.DateTime.Now.AddDays(100).ToString());

            //MainDS ds = access.Warehouse_VoucherQueryByFK("", System.DateTime.Now.AddDays(-50).ToString(), "", "", "", "", "", -1, 0, 0, -1);

            //vps.InitData(ds.Warehouse_Voucher);
            if (vps.ShowDialog() == DialogResult.OK)
            {
                // string VoucherNO = vps.GetSelectVoucherNO();
                WMSDS.T_OutStockRow wvrow = vps.GetWarehouse_VoucherRow();
                if (wvrow != null)
                {
                    //   this.voucherHead1.SetVoucherRow(wvrow);

                    this.cmb_VoucherNO.Text = wvrow.VoucherNO;
                    this.btn_Query_Click(null,null);
                    //if (wvrow.IsTradeTypeNull())
                    //{ this.cmb_TradeType.Text = ""; }
                    //else
                    //{ this.cmb_TradeType.Text = wvrow.TradeType; }

                    //if (wvrow.IsCustomerNameNull())
                    //{
                    //    this.cmb_CustomerName.Text = "";
                    //}
                    //else { this.cmb_CustomerName.Text = wvrow.CustomerName; }

                    //if (wvrow.IsFactoryIDNull())
                    //{
                    //    this.cmb_Factory.Text = "";
                    //}
                    //else { this.cmb_Factory.Text = wvrow.FactoryID; }

                    //if (wvrow.IsVehicleNONull())
                    //{
                    //    this.cmb_VehicleNO.Text = "";
                    //}
                    //else { this.cmb_VehicleNO.Text = wvrow.VehicleNO; }

                    //if (wvrow.IsBoxNONull())
                    //{
                    //    this.cmb_BoxNO.Text = "";
                    //}
                    //else { this.cmb_BoxNO.Text = wvrow.BoxNO; }

                    ////if (wvrow.IsSealNONull())
                    ////{
                    ////    this.cmb_SealNO.Text = "";
                    ////}
                    ////else { this.cmb_SealNO.Text = wvrow.SealNO; }

                    //if (wvrow.IsBoatNONull())
                    //{
                    //    this.cmb_Boat.Text = "";
                    //}
                    //else { this.cmb_Boat.Text = wvrow.BoatNO; }

                    //if (wvrow.IsPortNONull())
                    //{
                    //    this.cmb_Port.Text = "";
                    //}
                    //else { this.cmb_Port.Text = wvrow.PortNO; }

                    //if (wvrow.IsForklifterNONull())
                    //{
                    //    this.cmb_Lifter.Text = "";
                    //}
                    //else { this.cmb_Lifter.Text = wvrow.ForklifterNO; }

                    ////装卸工
                    //if (wvrow.IsCarrierNONull())
                    //{
                    //    this.cmb_Loader.Text = "";
                    //}
                    //else { this.cmb_Loader.Text = wvrow.CarrierNO; }

                    ////提单号
                    //if (wvrow.IsPickNONull())
                    //{
                    //    this.cmb_PickNo.Text = "";
                    //}
                    //else { this.cmb_PickNo.Text = wvrow.PickNO; }


                    //if (wvrow.IsOrderNONull())
                    //{
                    //    this.cmb_Contract.Text = "";
                    //}
                    //else { this.cmb_Contract.Text = wvrow.OrderNO; }


                    //if (wvrow.IsQtyNull())
                    //{
                    //    this.cmb_Amount.Text = "0";
                    //}
                    //else
                    //{
                    //    this.cmb_Amount.Text = wvrow.Qty.ToString();
                    //}

                    //if (wvrow.IsAuxQtyNull())
                    //{
                    //    this.cmb_Weight.Text = "0";
                    //}
                    //else { this.cmb_Weight.Text = this.TrimEndZero(wvrow.AuxQty.ToString()); }


                }

                //RefreshDataByVoucherNO(wvrow.VoucherNO);

            }
        }

    }
}
