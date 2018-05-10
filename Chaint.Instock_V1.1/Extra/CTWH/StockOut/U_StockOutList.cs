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
    public partial class U_StockOutList : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        MainLayout _MainLayout = null;
        public string _SourceVoucherNO = "";
        WMSDS _WMS_MAIN;
        public U_StockOutList()
        {
            InitializeComponent();
        }
        public U_StockOutList(MainLayout tl,string SourceVoucherNO)
        {
            InitializeComponent();
            this._MainLayout = tl;
            this._MainLayout.OnMenuClickEvent += new MenuClickEventHandle(OnMenuClickEventProcess);
            this.Disposed += new EventHandler(U_VoucherPlanAdd_Disposed);
            this._SourceVoucherNO = SourceVoucherNO;

            //是否启用立体库
            if (Utils.Stereo)
            {
                this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
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

        private void U_StockOutList_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMS_MAIN = new WMSDS();
            this.date_End.DateTime = DateTime.Now.AddMinutes(1);
            this.date_Start.DateTime = DateTime.Now.AddDays(-1);
            if (this._SourceVoucherNO != "")
            {
                this.chk_Source.Checked = true;
                this.cmb_SourceVoucherNO.Text = this._SourceVoucherNO;
            }
            this.btn_Search_Click(null, null);

        }
        public void OnMenuClickEventProcess(object sender, MenuClickEventArgs e)
        {

            switch (e.MenuName)
            {
                case Utils.WMSMenu._New:
                    this._MainLayout.OpenOutStockAdd();

                    string Des = string.Format("{0}新建了销售出库单{1}", Utils.LoginUserName, "");
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", "");

                    break;
                case Utils.WMSMenu._Save:
                    break;
                case Utils.WMSMenu._Delete:
                    string vno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    if (DialogResult.No == MessageBox.Show("确定要删除出库单" + vno + "吗？数据将不可恢复。", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        return;
                    //如果已开始发货就 不能删除
                    DataSet ds = this._WMSAccess.Select_T_OutStock_Product(vno, "");

                        
                    
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("该出库单已发货" + ds.Tables[0].Rows.Count + "件，不允许删除。");
                        return;
                    }
                    else
                    {
                        //可以删除
                        string ret = this._WMSAccess.Delete_T_OutStockAndEntry(vno);
                        if (ret == "")
                        {
                            MessageBox.Show("删除成功。");
                            btn_Search_Click(null, null);
                            _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}删除了销售出库单{1}", Utils.LoginUserName, vno), "", "", vno);
                        }
                        else
                        {
                            MessageBox.Show("删除失败：" + ret);
                        }
                    }
                    break;
                case Utils.WMSMenu._NewLine:
                    break;
                case Utils.WMSMenu._DelLine:
                    break;
                case Utils.WMSMenu._ReFresh:
                    break;
                case Utils.WMSMenu._Edit:
                    //修改一个销售出库单
                    string voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    string check = this.gridView1.GetFocusedRowCellDisplayText("IsUpload");
                    string close = this.gridView1.GetFocusedRowCellDisplayText("IsClose");
                    if (voucherno == "")
                    {
                        MessageBox.Show("请选择出库单，再进行修改");
                        return;
                    }
                    if (check == "已上传" & Utils.LoginUserType != "5")
                    {
                        MessageBox.Show("出库单已上传，如需修改请联系管理员!");
                        return;
                    }
                    //if (close == "已关闭")
                    //{
                    //    MessageBox.Show("出库单已关闭，不能修改");
                    //    return;
                    //}
                    if (DialogResult.Yes == MessageBox.Show("确定要修改" + voucherno + "吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        //U_VoucherPlanAdd_Disposed(null,null);
                        this._MainLayout.OpenOutStockAdd(voucherno);

                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}选择修改了销售出库单{1}", Utils.LoginUserName, voucherno), "", "", voucherno);

                    }
                    break;
                case Utils.WMSMenu._Preview:
                     voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO").ToString();
                    if (voucherno == "")
                    {
                        return;
                    }

                    this._MainLayout.OpenStockOut_PlanList(voucherno);

                       break;

                case Utils.WMSMenu._View:

                        voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    if (voucherno != "")
                        if (DialogResult.Yes == MessageBox.Show("确定要查看" + voucherno + "吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        this._MainLayout.OpenOutStockPrint(voucherno);


                    }
                    break;
                case Utils.WMSMenu._Exit:
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭页面吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        this._MainLayout.AddUserControl(null, "");
                    break;
                case Utils.WMSMenu._Lock:
                    voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
                    string islock = this.gridView1.GetFocusedRowCellDisplayText("IsClose");
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭出库单" + voucherno + "吗？单据将无法更改", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        if (islock == "")
                        {
                            this.LockOutStockBill(voucherno);
                            _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}关闭了销售出库单{1}", Utils.LoginUserName, voucherno), "", "", voucherno);

                            //if (billtype == "蓝")
                            //    this.CheckInStockBill(voucherno, true, "B");
                            //else if (billtype == "红")
                            //    this.CheckInStockBill(voucherno, true, "R");
                        }
                        else if (islock == "已关闭")
                        {
                            MessageBox.Show("单据已关闭，不能再次关闭。");
                        }
                    }

                    break;
            }

        }
        /// <summary>
        /// 关闭入库单据
        /// </summary>
        /// <param name="voucherno">入库单号</param>
        private void LockOutStockBill(string voucherno)
        {

            WMSDS.T_OutStockRow osp = (new WMSDS()).T_OutStock.NewT_OutStockRow();
            osp.VoucherNO = voucherno;
            osp.IsClose = "1";
            string ret = this._WMSAccess.Update_T_OutStock(osp);
            if (ret == "")
            {
                MessageBox.Show("单据" + voucherno + "关闭成功。");
                this.btn_Search_Click(null, null);
            }
            else
            {
                MessageBox.Show("单据" + voucherno + "关闭失败。" + ret);
            }
        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            string dateS = this.date_Start.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string dateE = this.date_End.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string factory =  "" ;
            string sourceVoucher = this.cmb_SourceVoucherNO.Text.Trim();
        //DataSet outListDS=    this._WMSAccess.Select_T_OutStock_List(factory,dateS,dateE);
        //    this.gridControl1.DataSource=outListDS.Tables["T_OutStock"];
        //    this.gridView1.BestFitColumns();
            if (this.chk_Source.Checked) {
                DataSet relationDS = this._WMSAccess.Select_T_OutStockAndEntry_RelationBySourceVoucher(sourceVoucher);
                //DataRelation dr = new DataRelation("通知分录",  relationDS.T_OutStock_Plan.OnlyIDColumn,  relationDS.T_OutStock_Plan_Entry.VoucherIDColumn,true);
                if (relationDS.Tables.Count > 0)
                {
                    int a = relationDS.Tables["T_OutStock_Entry"].Rows.Count;
                    DataRelation dr = new DataRelation("通知分录", relationDS.Tables["T_OutStock"].Columns["OnlyID"], relationDS.Tables["T_OutStock_Entry"].Columns["VoucherID"], false);

                    relationDS.Relations.Add(dr);　　　　//添加表关系到dataset

                    this.grid_OutStock.DataSource = relationDS.Tables["T_OutStock"];
                    //this.gridView1.BestFitColumns();
                    //this.gridView2.BestFitColumns();
                }
            
            }
            else
            {

                DataSet relationDS = this._WMSAccess.Select_T_OutStockAndEntry_Relation(dateS, dateE);
                //DataRelation dr = new DataRelation("通知分录",  relationDS.T_OutStock_Plan.OnlyIDColumn,  relationDS.T_OutStock_Plan_Entry.VoucherIDColumn,true);

                int a = relationDS.Tables["T_OutStock"].Rows.Count;
                DataRelation dr = new DataRelation("通知分录", relationDS.Tables["T_OutStock"].Columns["OnlyID"], relationDS.Tables["T_OutStock_Entry"].Columns["VoucherID"], false);

                relationDS.Relations.Add(dr);　　　　//添加表关系到dataset

                this.grid_OutStock.DataSource = relationDS.Tables["T_OutStock"];
                //this.gridView1.BestFitColumns();
                //this.gridView2.BestFitColumns();
            }
        }

        //发送移库入库单
        private void btn_Look_Click(object sender, EventArgs e)
        {
            string vno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO").ToString();
            string stat = this.gridView1.GetFocusedRowCellDisplayText("IsClose").ToString();
            string send = this.gridView1.GetFocusedRowCellDisplayText("IsCancel").ToString();

            string business = this.gridView1.GetFocusedRowCellDisplayText("BusinessType").ToString();
            string btype = this.gridView1.GetFocusedRowCellDisplayText("BillType").ToString();


            if (vno == "")
            {
                MessageBox.Show("单号不正确。");
            }
            else if (send == "已传输")
            {
                MessageBox.Show("单据已传输，不能再次传输。");
            }
            else if (business != "移库出库")
            {
                MessageBox.Show("只有移库出库单才能发送入库单。");
            }

            else if (btype == "红")
            {
                MessageBox.Show("红单不能发送入库单。");

            }
            else if (stat == "")
            {
                MessageBox.Show("移库出库单未关闭不能发送入库单。");

            }
            else
            {
                if (DialogResult.No == MessageBox.Show("确定要发送移库入库单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;

                string localorg = "";
                string org = this.gridView1.GetFocusedRowCellDisplayText("FactoryCode").ToString();//组织代码
                string ckbh = this.gridView1.GetFocusedRowCellDisplayText("WHToBH").ToString();//仓库代码

                WMSDS localorgDS = this._WMSAccess.Select_T_Factory(false, true);
                WMSDS oDS = this._WMSAccess.Select_T_FactoryByCode(true, org);
                WMSDS ckDS = this._WMSAccess.Select_T_Factory_WarehouseByCode(true, ckbh);

                string connstr = "";
                string localconnstr = "";

                string whname = this.gridView1.GetFocusedRowCellDisplayText("WHCode").ToString();
                string whtoname = this.gridView1.GetFocusedRowCellDisplayText("WHToCode").ToString();

                if (localorgDS.T_Factory.Rows.Count == 0)
                {
                    MessageBox.Show("请先配置本地库存组织");
                    return;
                }
                else
                {
                    if (oDS.T_Factory.Rows.Count == 0)
                    {
                        MessageBox.Show("请先配置接收库存组织");
                        return;
                    }
                    else
                    {

                        localorg = localorgDS.T_Factory.Rows[0][localorgDS.T_Factory.FactoryAbbrColumn].ToString();
                        localconnstr = localorgDS.T_Factory.Rows[0][localorgDS.T_Factory.FactoryPhoneColumn].ToString();
                        //org = oDS.T_Factory.Rows[0][oDS.T_Factory.FactoryAbbrColumn].ToString();
                        //connstr = oDS.T_Factory.Rows[0][oDS.T_Factory.FactoryPhoneColumn].ToString();//zfz

                        org = ckDS.T_Factory_Warehouse.Rows[0][ckDS.T_Factory_Warehouse.FactoryAddrColumn].ToString();
                        connstr = ckDS.T_Factory_Warehouse.Rows[0][ckDS.T_Factory_Warehouse.FactoryPhoneColumn].ToString();

                        if (localorg == "")
                        {
                            MessageBox.Show("请先配置本地库存组织");
                            return;
                        }
                        if (org == "")
                        {
                            MessageBox.Show("请先配置接收库存组织");
                            return;
                        }
                        if (localconnstr == "")
                        {
                            MessageBox.Show("请先配置本地库存组织连接串");
                            return;
                        }
                        if (connstr == "")
                        {
                            MessageBox.Show("请先配置接收库存组织连接串");
                            return;
                        }
                        //if (org == localorg)
                        //{
                        //    MessageBox.Show("不能往本地库存组织发送入库明细");
                        //    return;
                        //}

                        //存在不同组织拥有相同仓库的情况
                        if (org == localorg && whname == whtoname)
                        {
                            MessageBox.Show("不能往相同的仓库做移库");
                            return;
                        }

                        if (localconnstr == connstr)
                        {
                            MessageBox.Show("本地库存组织和接收库存组织连接串相同，不能发送入库明细，请检查库存组织映射");
                            return;
                        }

                    }

                }
                //可以发送
                string ret = this.TransferInStockBill(vno, connstr);
                if (ret == "")
                {

                    string invno = this.NewInStockVoucherNO();
                    _WMSAccess.Insert_LocalInStock(invno);//本地占用单号
                    _WMSAccess.Update_IsTransfer(vno);//更新传输标识

                    MessageBox.Show("发送成功");

                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}发送移库入库单{1}成功", Utils.LoginUserName, vno), "", "", vno);
                }
                else
                {
                    MessageBox.Show(ret);
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}发送移库入库单{1}失败", Utils.LoginUserName, vno), ret, "", vno);
                }

                this.btn_Search_Click(null, null);
            }
        }
        /// <summary>
        /// 发送调拨移库自动生成的入库单
        /// </summary>
        /// <param name="dbvno">调拨出库单号</param>
        /// <returns></returns>
        private string TransferInStockBill(string dbvno, string connStr)
        {
            //查询调拨单信息
            InterfaceDS iDS = this._WMSAccess.Select_T_OutStockAndEntry_RelationForUpload(dbvno, "", "");
            if (iDS.CT_StockOut.Rows.Count > 0)
            {
                //生成本地入库单抬头，并在发送成功后占用这个单号
                string invno = this.NewInStockVoucherNO();

                if (invno == "")
                {
                    return "生成异地入库单号失败，无法传输数据";
                }
                string org = iDS.CT_StockOut.Rows[0][iDS.CT_StockOut.Cdefine1Column].ToString();
                //生成分录，直接以入库单的方式查询出来instock,instock_entry,和product in
                WMSDS wmsDS = this._WMSAccess.Select_T_InStockBillForTransfer(dbvno, "0", "YKRK");
                int a = wmsDS.T_InStock.Rows.Count;
                if (a == 0)
                    return "未查询到对应入库单抬头，无法传输数据。";
                int b = wmsDS.T_InStock_Entry.Rows.Count;
                if (b == 0)
                    return "未查询到对应入库单分录，无法传输数据。";
                int c = wmsDS.T_InStock_Product.Rows.Count;
                if (c == 0)
                    return "未查询到对应入库单条码，无法传输数据。";
                int d = wmsDS.T_ProductLife.Rows.Count;
                if (d == 0)
                    return "未查询到入库记录，无法传输数据。";
                int e = wmsDS.T_Product_In.Rows.Count;
                if (e == 0)
                    return "未查询到入库条码，无法传输数据。";
                //开始用事务写入远程服务器
                DataSet ds = this._WMSAccess.Select_OutStockInfo(dbvno);
                string ret = this._WMSAccess.Tran_TransferInStockBill(dbvno, invno, wmsDS, ds, connStr);
                return ret;
            }
            else
            {
                return "移库出库单不存在。";
            }
        }
        /// <summary>
        /// 新建一个入库单号
        /// </summary>
        private string NewInStockVoucherNO()
        {
            //刷新单号
           WMSDS orgDS = this._WMSAccess.Select_T_Factory(false, true);
           if (orgDS.T_Factory.Rows.Count > 0)
           {
               string type = Utils.WMSVoucherType._BillIn;
               string machineid = orgDS.T_Factory.Rows[0][orgDS.T_Factory.MachineIDColumn].ToString();// Utils.LoginMachineID;
               string planNO = this._WMSAccess.Get_T_StockIn_NewFlow(type, machineid);
               return planNO;
           }
           else
           {
               MessageBox.Show("请先配置本地库存组织");
               return "";
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

        private void chk_Source_CheckedChanged(object sender, EventArgs e)
        {
            this.cmb_SourceVoucherNO.Enabled = this.chk_Source.Checked;
        }

        private void btn_SendToZZ_Click(object sender, EventArgs e)
        {
            string vno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO").ToString();
            string stat = this.gridView1.GetFocusedRowCellDisplayText("IsClose").ToString(); //关单
            string send = this.gridView1.GetFocusedRowCellDisplayText("IsCancel").ToString();//上传
            string sendToZZ = this.gridView1.GetFocusedRowCellDisplayText("IsSendToZZ").ToString();//发送至立库
            string business = this.gridView1.GetFocusedRowCellDisplayText("BusinessType").ToString();//业务类型
            string btype = this.gridView1.GetFocusedRowCellDisplayText("BillType").ToString();//红单蓝单

            if (vno == "") {
                MessageBox.Show("出库单号不正确。");
            }
            else if (sendToZZ == "已发送")
            {
                MessageBox.Show("单据已发送至立体库，不能再次发送。");
            }
            else if (sendToZZ == "已删除")
            {
                MessageBox.Show("单据已被删除，不能再次发送。");
            }
            else if (btype == "红") {
                MessageBox.Show("红单不能提交至立体库。");

            }
            else
            {
                if (DialogResult.No == MessageBox.Show(string.Format("确定要发送销售出库单【{0}】至立体库吗？",vno), "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;

                //往立体库发送单据内容
                string ret = this.Insert_StockOut_Entry_ToZZ(vno);
                if (ret == "")
                {
                    _WMSAccess.Update_IsSendToZZ(vno,"1");//更新传输标识
                    MessageBox.Show("成功发送至立体库!");
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}发送销售出库单{1}至立体库成功", Utils.LoginUserName, vno), "", "", vno);
                }
                else
                {
                    MessageBox.Show(ret);
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}发送销售出库单{1}至立体库失败", Utils.LoginUserName, vno), ret, "", vno);
                }
                this.btn_Search_Click(null, null);
            }
        }

        private string Insert_StockOut_Entry_ToZZ(string voucherNo)
        {
            InterfaceDS planDS = this._WMSAccess.Select_T_OutStockAndEntry_RelationForZZ(voucherNo, "", "");

            string a = "";
            if (planDS.CT_StockOut.Rows.Count > 0)
                a = this._WMSAccess.Tran_Insert_ProductGroup(planDS);
            else
                a = "单据" + voucherNo + "没有分录,无法发送至立体库!!";
            if (a != "")
            {
                return a;
            }
            else
            {
                return "";
            }

        }

        private void btn_EditToZZ_Click(object sender, EventArgs e)
        {
            string vno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO").ToString();
            string stat = this.gridView1.GetFocusedRowCellDisplayText("IsClose").ToString(); //关单
            string send = this.gridView1.GetFocusedRowCellDisplayText("IsCancel").ToString();//上传
            string sendToZZ = this.gridView1.GetFocusedRowCellDisplayText("IsSendToZZ").ToString();//发送至立库
            string business = this.gridView1.GetFocusedRowCellDisplayText("BusinessType").ToString();//业务类型
            string btype = this.gridView1.GetFocusedRowCellDisplayText("BillType").ToString();//红单蓝单

            if (vno == "")
            {
                MessageBox.Show("出库单号不正确。");
                return;
            }
            else if (sendToZZ != "已发送")
            {
                MessageBox.Show("单据未发送至立体库，不需要修改!");
                return;
            }
            else if (btype == "红") {
                MessageBox.Show("红单不能提交至立体库。");
                return;
            }

            if (DialogResult.No == MessageBox.Show(string.Format("确定要修改销售出库单【{0}】吗？",vno), "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;

            //锁定，则不可修改

            DataSet ds = this._WMSAccess.Select_IsVoucherOperateLockedZZ(vno);

            if (ds.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("该销售出库单已被立体库锁定，无法修改!");
                return;
            }
            else
            {
                this._WMSAccess.Delete_VoucherZZ(vno);
                string ret = this.Insert_StockOut_Entry_ToZZ(vno);
                if (ret == "")
                {
                    MessageBox.Show("成功修改立体库销售出库单!");
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}修改立体库销售出库单{1}成功", Utils.LoginUserName, vno), "", "", vno);
                }
                else
                {
                    MessageBox.Show(ret);
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}修改立体库销售出库单{1}失败", Utils.LoginUserName, vno), ret, "", vno);
                }
            }

        }

        private void btn_DeleteToZZ_Click(object sender, EventArgs e)
        {
            string vno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO").ToString();
            string stat = this.gridView1.GetFocusedRowCellDisplayText("IsClose").ToString(); //关单
            string send = this.gridView1.GetFocusedRowCellDisplayText("IsCancel").ToString();//上传
            string sendToZZ = this.gridView1.GetFocusedRowCellDisplayText("IsSendToZZ").ToString();//发送至立库
            string business = this.gridView1.GetFocusedRowCellDisplayText("BusinessType").ToString();//业务类型
            string btype = this.gridView1.GetFocusedRowCellDisplayText("BillType").ToString();//红单蓝单

            if (vno == "")
            {
                MessageBox.Show("出库单号不正确。");
                return;
            }
            else if (sendToZZ != "已发送")
            {
                MessageBox.Show("单据未发送至立体库，不需要删除!","提示");
                return;
            }
            else if (btype == "红")
            {
                MessageBox.Show("红单不能提交至立体库。");
                return;
            }

            if (DialogResult.No == MessageBox.Show(string.Format("确定要删除销售出库单【{0}】吗？", vno), "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;
            //锁定，则不可删除

            DataSet ds = this._WMSAccess.Select_IsVoucherOperateLockedZZ(vno);

            if (ds.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("该销售出库单已被立体库锁定，无法删除!");
                return;
            }
            else
            {
                this._WMSAccess.Delete_VoucherZZ(vno);
                _WMSAccess.Update_IsSendToZZ(vno,"2");//更新传输标识
                _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}删除了立体库销售出库单{1}", Utils.LoginUserName, vno), "", "", vno);
                MessageBox.Show(string.Format("已删除单据{0}",vno));
                this.btn_Search_Click(null, null);
                return;
            }

            

        }

    }
}
