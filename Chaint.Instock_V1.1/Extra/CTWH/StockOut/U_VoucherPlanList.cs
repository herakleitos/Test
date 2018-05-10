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
    public partial class U_VoucherPlanList : DevExpress.XtraEditors.XtraUserControl
    {
        public U_VoucherPlanList()
        {
            InitializeComponent();
        }
        MainLayout _MainLayout = null;
        string _VoucherNO;
        void U_VoucherPlanAdd_Disposed(object sender, EventArgs e)
        {
            this._MainLayout.OnMenuClickEvent -= new MenuClickEventHandle(OnMenuClickEventProcess);
        }
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        private void DisposeData()
        {
            _WMSAccess.SqlStateChange -= new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMSAccess = null;
        }
        public U_VoucherPlanList(MainLayout tl,string voucherNO)
        {
            InitializeComponent();
            this._MainLayout = tl;
            this._MainLayout.OnMenuClickEvent += new MenuClickEventHandle(OnMenuClickEventProcess);
            this.Disposed += new EventHandler(U_VoucherPlanAdd_Disposed);
            this._VoucherNO = voucherNO;
           
        } 
        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }
        private void U_VoucherPlanList_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            //加载机台号
            LoadOrg();

            this.date_End.DateTime = DateTime.Now.AddMinutes(1);
            this.date_Start.DateTime = DateTime.Now.AddDays(-1);
            //this.date_Start.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00");

            if (this._VoucherNO != "")
            {
                this.chk_Out.Checked = true;
                this.cmb_OutBill.Text = this._VoucherNO;
            }
            this.btn_Search_Click(null, null);

        }
        private void LoadOrg()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_OrgInfo("");

            this.cmb_Factory.Properties.DataSource = iDS.CT_OrgInfo;
            this.cmb_Factory.Properties.ValueMember = "ORGCode";
            this.cmb_Factory.Properties.DisplayMember = "ORGName";
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string dateS = this.date_Start.DateTime.ToString("yyyy-MM-dd HH:mm:00");
            string dateE = this.date_End.DateTime.ToString("yyyy-MM-dd HH:mm:00");
            if (this.chk_Out.Checked)
                this._VoucherNO = this.cmb_OutBill.Text.Trim();
            if (this._VoucherNO != "") {
                DataSet relationDS = this._WMSAccess.Select_T_OutPlanAndEntry_RelationByOutStock(this._VoucherNO);
                DataRelation dr = new DataRelation("通知分录", relationDS.Tables["T_OutStock_Plan"].Columns["OnlyID"], relationDS.Tables["T_OutStock_Plan_Entry"].Columns["VoucherID"], false);
                relationDS.Relations.Add(dr);　　　　//添加表关系到dataset
                this.grid_VoucherPlanList.DataSource = relationDS.Tables["T_OutStock_Plan"];
                this.gridView1.BestFitColumns();
                this.gridView2.BestFitColumns();
            }
            else
            {
                DataSet relationDS = this._WMSAccess.Select_T_OutPlanAndEntry_Relation(dateS, dateE);
                DataRelation dr = new DataRelation("通知分录", relationDS.Tables["T_OutStock_Plan"].Columns["OnlyID"], relationDS.Tables["T_OutStock_Plan_Entry"].Columns["VoucherID"], false);
                relationDS.Relations.Add(dr);　　　　//添加表关系到dataset
                this.grid_VoucherPlanList.DataSource = relationDS.Tables["T_OutStock_Plan"];
                //this.gridView1.BestFitColumns();
                //this.gridView2.BestFitColumns();
            }
        }

       

        private void gridView1_MasterRowExpanded(object sender, DevExpress.XtraGrid.Views.Grid.CustomMasterRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView aView = gridView1.GetDetailView(e.RowHandle, e.RelationIndex) as DevExpress.XtraGrid.Views.Grid.GridView;

            if (aView != null)
            {
                aView.Columns["VoucherID"].Visible = false;　　　　//设置UID 列为隐藏
                aView.BestFitColumns();
            }
        }


        public void OnMenuClickEventProcess(object sender, MenuClickEventArgs e)
        {

            switch (e.MenuName)
            {
                case Utils.WMSMenu._New:

                    //新建一个发货通知单
                    if (DialogResult.Yes == MessageBox.Show("确定要新建发货通知单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        this._MainLayout.OpenVoucherPlanAdd("");

                        string Des = string.Format("{0}点击了新建发货通知单{1}", Utils.LoginUserName, "");
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", "");

                    }
                    break;
                case Utils.WMSMenu._Edit:
                    //修改一个发货通知单
                    string voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    if (voucherno == "")
                    {
                        return;
                    }
                     string chk = this.gridView1.GetFocusedRowCellDisplayText("IsCheck");
                     if (chk == "已审核")
                    {
                        MessageBox.Show("单据已审核，不能修改");
                        return;
                    }

                  

                    if (DialogResult.Yes == MessageBox.Show("确定要修改" + voucherno + "吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        //U_VoucherPlanAdd_Disposed(null,null);
                        this._MainLayout.OpenVoucherPlanAdd(voucherno);

                        string Des = string.Format("{0}点击了修改发货通知单{1}", Utils.LoginUserName, voucherno);
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);

                    }
                    break;
                case Utils.WMSMenu._Next:
                      voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    if (voucherno == "")
                    {
                        return;
                    }
                    this._MainLayout.OpenStockOutList(voucherno);
                    break;
                case Utils.WMSMenu._Delete:
                      voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                     string check  = this.gridView1.GetFocusedRowCellDisplayText("IsCheck");
                     string close = this.gridView1.GetFocusedRowCellDisplayText("IsClose");

                     if (voucherno != "")
                     {
                         this.DeleteVoucherPlan(voucherno, check, close);
                         string Des = string.Format("{0}删除了出库通知单{1}", Utils.LoginUserName, voucherno);
                         _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
                     }
                    break;
                case Utils.WMSMenu._Check:
                    voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    if (voucherno != "")
                    {
                        this.CheckVoucherPlan(voucherno, true);
                        string Des = string.Format("{0}审核了出库通知单{1}", Utils.LoginUserName, voucherno);
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
                    }

                    break;
                case Utils.WMSMenu._Uncheck:
                    voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    if (voucherno != "")
                    {
                        this.CheckVoucherPlan(voucherno, false);
                        string Des = string.Format("{0}反审核了出库通知单{1}", Utils.LoginUserName, voucherno);
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
                    }

                    break;

                case Utils.WMSMenu._Exit:
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭页面吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        this._MainLayout.AddUserControl(null, "");
                    break;
            }

        }
        //删除一个发货通知单
        private void DeleteVoucherPlan(string voucherno,string IsCheck,string IsClose)
        {
            //先判断是不是审核
            if (IsCheck == "已审核") {
                MessageBox.Show("删除失败，请先反审核该单据.");
                return;
            }
            if (IsCheck == "已关闭")
            {
                MessageBox.Show("删除失败，请先反关闭该单据.");
                return;
            }
            ////1.下面不能有单子，
            //WMSDS eDs = this._WMSAccess.Select_T_OutStock_EntryBySource(voucherno);
            //if (eDs.T_OutStock_Entry.Rows.Count > 0)
            //{
            //    MessageBox.Show("删除失败，单据已经下推销售出库单");

            //}
            //else
            //{
            //    WMSDS.T_OutStock_PlanRow osp = new WMSDS().T_OutStock_Plan.NewT_OutStock_PlanRow();
            //    osp.VoucherNO = voucherno;
            //    osp.IsCheck = "0";
                string ret = this._WMSAccess.Delete_T_OutStock_PlanAndEntry(voucherno);
                if (ret == "")
                {
                    MessageBox.Show("删除成功。");
                    btn_Search_Click(null, null);

                }
                else
                {
                    MessageBox.Show("删除失败：" + ret);
                }
            
        }
        /// <summary>
        /// 审核或反审核这个发货通知单
        /// </summary>
        /// <param name="voucherno"></param>
        /// <param name="check">审核还是反审核</param>
        /// 
        private void CheckVoucherPlan(string voucherno, bool check)
        {

            if (check)
            {
                WMSDS.T_OutStock_PlanRow osp = (new WMSDS()).T_OutStock_Plan.NewT_OutStock_PlanRow();
                osp.VoucherNO = voucherno;
                osp.IsCheck = "1";
                string ret = this._WMSAccess.Update_T_OutStock_PlanByRow(osp);
                if (ret == "")
                {
                    MessageBox.Show("审核成功。");
                    btn_Search_Click(null, null);
                }
                else
                {
                    MessageBox.Show("审核失败：" + ret);
                }
            }
            else
            {
                //先判断能不能反审核

                //1.下面不能有单子，
               WMSDS eDs= this._WMSAccess.Select_T_OutStock_EntryBySource(voucherno);
               if (eDs.T_OutStock_Entry.Rows.Count > 0)
               {
                   MessageBox.Show("反审核失败，单据已经下推销售出库单");

               }
               else {
                   WMSDS.T_OutStock_PlanRow osp = (new WMSDS()).T_OutStock_Plan.NewT_OutStock_PlanRow();
                   osp.VoucherNO = voucherno;
                   osp.IsCheck = "0";
                   string ret = this._WMSAccess.Update_T_OutStock_PlanByRow(osp);
                   if (ret == "")
                   {
                       MessageBox.Show("反审核成功。");
                       btn_Search_Click(null, null);

                   }
                   else
                   {
                       MessageBox.Show("反审核失败：" + ret);
                   }
               }
            }
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            //下推一个销售出库单
            string voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
            string ischeck = this.gridView1.GetFocusedRowCellDisplayText("IsCheck");

            if (voucherno == "")
            {
                return;
            }
            if (ischeck != "已审核")
            {
                MessageBox.Show("请先审核该发货通知单再下推销售出库单。");
                return;
            }

            WMSDS eDs = this._WMSAccess.Select_T_OutStock_EntryBySource(voucherno);
            if (eDs.T_OutStock_Entry.Rows.Count > 0)
            {
                MessageBox.Show("单据已经下推销售出库单！");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("确定要下推" + voucherno + "的销售出库单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                //U_VoucherPlanAdd_Disposed(null,null);
                this._MainLayout.OpenOutStockAdd("",voucherno);

                string Des = string.Format("{0}下推了出库单{1}", Utils.LoginUserName, voucherno);
                _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
            }
        }

        private void chk_Out_CheckedChanged(object sender, EventArgs e)
        {
            this.cmb_OutBill.Enabled = this.chk_Out.Checked;
        }

    }
}
