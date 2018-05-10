using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.Common;
using DataModel;

namespace CTWH.StockIn
{
    public partial class U_StockInList : DevExpress.XtraEditors.XtraUserControl
    {
        //private TestLayout testLayout;
        MainLayout _MainLayout = null;
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
        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }
        public U_StockInList()
        {
            InitializeComponent();
        }

        public U_StockInList(MainLayout testLayout)
        {
            InitializeComponent();
            this._MainLayout = testLayout;
            this._MainLayout.OnMenuClickEvent += new MenuClickEventHandle(OnMenuClickEventProcess);
            this.Disposed += new EventHandler(U_VoucherPlanAdd_Disposed);
        }

        private void U_StockInList_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            //加载机台号
            LoadOrg();

            this.date_End.DateTime = DateTime.Now.AddMinutes(1);
            this.date_Start.DateTime = DateTime.Now.AddDays(-1);
            this.btn_Query_Click(null,null);
            
        }
        private void LoadOrg()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_OrgInfo("");

            this.cmb_Factory.Properties.DataSource = iDS.CT_OrgInfo;
            this.cmb_Factory.Properties.ValueMember = "ORGCode";
            this.cmb_Factory.Properties.DisplayMember = "ORGName";
        }
        public void OnMenuClickEventProcess(object sender, MenuClickEventArgs e)
        {
            string voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
            string voucherid = this.gridView1.GetFocusedRowCellDisplayText("OnlyID");
            string ischk = this.gridView1.GetFocusedRowCellDisplayText("IsCheck");
            string isupload = this.gridView1.GetFocusedRowCellDisplayText("UploadDate");
            string delno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");
            string billtype = this.gridView1.GetFocusedRowCellDisplayText("BillType");
            string islock = this.gridView1.GetFocusedRowCellDisplayText("IsClose");
            switch (e.MenuName)
            {
                case Utils.WMSMenu._New:

                    //新建一个入库单
                    if (DialogResult.Yes == MessageBox.Show("确定要新建入库单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        this._MainLayout.OpenInStockAdd("","","add");

                        string Des = string.Format("{0}点击了新建入库单{1}", Utils.LoginUserName, "");
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", "");
                    }
                    break;
                case Utils.WMSMenu._Edit:
                    //修改一个入库单
                    if (voucherno == "")
                        return;
                    if (ischk == "已审核") {
                        MessageBox.Show("入库单已审核，不能修改。");
                        return;
                    }
                    if (islock == "已关闭")
                    {
                        MessageBox.Show("入库单已关闭，不能修改。");
                        return;
                    }
                    if (isupload == "已上传")
                    {
                        MessageBox.Show("入库单已上传，不能修改。");
                        return;
                    }

                    if (DialogResult.Yes == MessageBox.Show("确定要修改" + voucherno + "吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        this._MainLayout.OpenInStockAdd(voucherno, voucherid,"edit");

                        string Des = string.Format("{0}选择了修改入库单{1}", Utils.LoginUserName, voucherno);
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
                    }
                    break;
                case Utils.WMSMenu._Save:
                    break;
                case Utils.WMSMenu._Delete:
                    if (voucherno == "")
                        return;

                    //先看能不能删除
                    if (ischk == "已审核")
                    {
                        MessageBox.Show("入库单已审核，不能删除。");
                        return;
                    }
                    if (islock == "已关闭")
                    {
                        MessageBox.Show("入库单已关闭，不能删除。");
                        return;
                    }

                    if (isupload == "已上传")
                    {
                        MessageBox.Show("入库单已上传，不能删除。");
                        return;
                    }

                    if (DialogResult.Yes == MessageBox.Show("确定要删除入库单"+delno+"吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        this.DeleteInStockBill(delno);

                        string Des = string.Format("{0}删除了入库单{1}", Utils.LoginUserName, delno);
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", delno);
                    
                    }

                    this.btn_Query_Click(null,null);
                    break;
                case Utils.WMSMenu._Check:
                    if (voucherno == "")
                        return;
                    //先看能不能审核
                    if (ischk == "已审核")
                    {
                        MessageBox.Show("入库单已审核，不能再次审核。");
                        return;
                    }
                    if (islock == "已关闭")
                    {
                        MessageBox.Show("入库单已关闭，不能审核。");
                        return;
                    }

                    if (isupload == "已上传")
                    {
                        MessageBox.Show("入库单已上传，不能审核。");
                        return;
                    }
                    if(DialogResult.Yes==MessageBox.Show("确定要审核"+voucherno+"吗？","提示信息",MessageBoxButtons.YesNo,MessageBoxIcon.Question))
                        if (ischk == "")
                        {
                                if (billtype == "蓝")
                                    this.CheckInStockBill(voucherno, true, "B");
                                else if (billtype == "红")
                                    this.CheckInStockBill(voucherno, true, "R");


                        }
                        else if(ischk=="已审核"){
                            MessageBox.Show("单据已审核，不能再次审核。");
                        }


                    break;
                case Utils.WMSMenu._Uncheck:
                    if (voucherno == "")
                        return;
                    if (ischk == "")
                    {
                        MessageBox.Show("入库单未审核，不能反审核。");
                        return;
                    }
                    if (islock == "已关闭")
                    {
                        MessageBox.Show("入库单已关闭，不能反审核。");
                        return;
                    }

                    if (isupload == "已上传")
                    {
                        MessageBox.Show("入库单已上传，不能反审核。");
                        return;
                    }
                    if (DialogResult.Yes == MessageBox.Show("确定要反审核" + voucherno + "吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        if (ischk == "已审核")
                        {
                            if (billtype == "蓝")
                                this.CheckInStockBill(voucherno, false, "B");
                            else if (billtype == "红")
                                this.CheckInStockBill(voucherno, false, "R");
                        }
                        else if (ischk == "")
                        {
                            MessageBox.Show("单据未审核，不能反审核。");
                        }

                    break;
                case Utils.WMSMenu._Lock:


                    if (ischk == "")
                    {
                        MessageBox.Show("入库单未审核，不能关单。");
                        return;
                    }
                    if (islock == "已关闭")
                    {
                        MessageBox.Show("入库单已关单，不能再次关单。");
                        return;
                    }

                   
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭入库单"+voucherno+"吗？单据将不能再修改！", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        if (islock == "")
                        {
                            this.LockInStockBill(voucherno);

                            string Des = string.Format("入库单关闭成功,关闭人{0}", Utils.LoginUserName);
                            _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);

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
                case Utils.WMSMenu._Exit:
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭页面吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        this._MainLayout.AddUserControl(null, "");
                    break;
                case Utils.WMSMenu._View://查看
                        this._MainLayout.OpenInStockAdd(voucherno, voucherid,"view");
                    break;
            }

        }
        /// <summary>
        /// 审核和反审核入库单据
        /// </summary>
        /// <param name="voucherno">入库单号</param>
        /// <param name="check">审核还是反审核</param>
        /// <param name="rb">红单还是蓝单</param>
        private void CheckInStockBill(string voucherno, bool check,string rb)
        {

            if (check)//如果是审核
            {
                if (rb == "B")
                {
                    WMSDS.T_InStockRow osp = (new WMSDS()).T_InStock.NewT_InStockRow();
                    osp.VoucherNO = voucherno;
                    osp.IsCheck = "1";
                    string ret = this._WMSAccess.Update_T_InStock(osp);
                    if (ret == "")
                    {
                        MessageBox.Show("蓝单审核成功。");
                        this.btn_Query_Click(null, null);

                        string Des = string.Format("蓝单审核成功,审核人{0}", Utils.LoginUserName);
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
                        
                    }
                    else
                    {
                        MessageBox.Show("蓝单审核失败：" + ret);
                    }
                }
                else if (rb == "R") {
                    //红单要把单据包含条码对应的蓝单条码status全部=0
                    //先查出红单所有的条形码ID
                    string ret = this._WMSAccess.CheckInStockBillRed(voucherno,0,1);
                    if (ret == "")
                    {
                        MessageBox.Show("红单审核成功。");
                        this.btn_Query_Click(null, null);

                        string Des = string.Format("红单审核成功,审核人{0}", Utils.LoginUserName);
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
                    }
                    else
                    {
                        MessageBox.Show("红单审核失败：" + ret);
                    }
                }
            }
            else
            {
                //先判断能不能反审核
                WMSDS eDs = this._WMSAccess.Select_T_InStockByVoucherNO(voucherno);
                if (eDs.T_InStock.Rows.Count > 0)
                {
                    if (eDs.T_InStock.Rows[0][eDs.T_InStock.IsUploadColumn].ToString() == "1")
                    {
                        MessageBox.Show("反审核失败，单据已经上传到ERP");
                        return;
                    }

                    if (rb == "B")
                    {
                        WMSDS.T_InStockRow osp = (new WMSDS()).T_InStock.NewT_InStockRow();
                        osp.VoucherNO = voucherno;
                        osp.IsCheck = "0";
                        string ret = this._WMSAccess.Update_T_InStock(osp);
                        if (ret == "")
                        {
                            MessageBox.Show("蓝单反审核成功。");

                            string Des = string.Format("蓝单反审核成功,审核人{0}", Utils.LoginUserName);
                            _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);

                            this.btn_Query_Click(null, null);

                        }
                        else
                        {
                            MessageBox.Show("蓝单反审核失败：" + ret);
                        }
                    }
                    else if(rb=="R") {
                        //红单要把单据包含条码对应的蓝单条码status全部=1
                        //先查出红单所有的条形码ID
                        string ret = this._WMSAccess.CheckInStockBillRed(voucherno, 1, 0);
                        if (ret == "")
                        {
                            MessageBox.Show("红单反审核成功。");

                            string Des = string.Format("红单反审核成功,审核人{0}", Utils.LoginUserName);
                            _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);

                            this.btn_Query_Click(null, null);
                        }
                        else
                        {
                            MessageBox.Show("红单反审核失败：" + ret);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("反审核失败：" + "单据不存在");
                }
            }
        }
        /// <summary>
        /// 关闭入库单据
        /// </summary>
        /// <param name="voucherno">入库单号</param>
        private void LockInStockBill(string voucherno)
        {

                    WMSDS.T_InStockRow osp = (new WMSDS()).T_InStock.NewT_InStockRow();
                    osp.VoucherNO = voucherno;
                    osp.IsClose = "1";
                    string ret = this._WMSAccess.Update_T_InStock(osp);
                    if (ret == "")
                    {
                        MessageBox.Show("单据"+voucherno+"关闭成功。");
                        this.btn_Query_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("单据" + voucherno + "关闭失败。" + ret);
                    }
        }
        private void DeleteInStockBill(string delno)
        {
        


            //再用事务开始删除

            //先删除
          string ss=  this._WMSAccess.Tran_Delete_T_InStock(delno);

          if (ss == "")
              MessageBox.Show("删除成功");
          else
              MessageBox.Show("删除失败："+ss);
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            string dateS = this.date_Start.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string dateE = this.date_End.DateTime.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
            string factory = this.cmb_Factory.EditValue == null ? "" : this.cmb_Factory.EditValue.ToString();
          DataSet inDS=  this._WMSAccess.Select_T_InStockAndEntry_Relation(factory,date_Start.Text,date_End.Text);
          DataRelation dr = new DataRelation("入库分录", inDS.Tables["T_InStock"].Columns["OnlyID"], inDS.Tables["T_InStock_Entry"].Columns["VoucherID"], false);

          inDS.Relations.Add(dr);　　　　//添加表关系到dataset
          this.grid_Data.DataSource = inDS.Tables["T_InStock"];
          this.gridView1.BestFitColumns();
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

        private void btn_Upload_Click(object sender, EventArgs e)
        {

            string voucherno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO");//.Text;
           
                //查询这个voucher的信息，判断是否已经保存，是否已经上传等？
            WMSDS ids = this._WMSAccess.Select_T_InStockByVoucherNO(voucherno);
            string ischeck = ids.T_InStock.Rows[0]["IsCheck"].ToString();
            string isup = ids.T_InStock.Rows[0]["IsUpload"].ToString();
            string update = ids.T_InStock.Rows[0]["UploadDate"].ToString();
            string isclose = ids.T_InStock.Rows[0]["IsClose"].ToString();
            if (isup == "1")
            {
                MessageBox.Show("单据已于" + update + "上传，不能再次上传");
                return;
            }
            if (ischeck == "0")
            {
                MessageBox.Show("单据未审核，不能上传");
                return;
            }
            
            if (DialogResult.No == MessageBox.Show("确定要上传入库单" + voucherno + "吗？单据将自动关闭。", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;
            string business = this.gridView1.GetFocusedRowCellDisplayText("BusinessTypeCode");// this.cmb_BusinessType.EditValue.ToString();
            if (business == "YKRK")
            {
                MessageBox.Show("移库入库单不需要上传到ERP");
                return;
            }
            string redblue = this.gridView1.GetFocusedRowCellDisplayText("BillTypeCode");//this.cmb_RedBlue.Text;
            string org = this.gridView1.GetFocusedRowCellDisplayText("FactoryID");// this.cmb_Org.EditValue.ToString();
            string wh = this.gridView1.GetFocusedRowCellDisplayText("WHCode");//this.cmb_Warehouse.EditValue.ToString();
            string emp = this.gridView1.GetFocusedRowCellDisplayText("Keeper");// this.cmb_Emp.EditValue.ToString();
            string source = this.gridView1.GetFocusedRowCellDisplayText("SourceVoucherNO");// this.cmb_SourceVoucherNO.Text;
            string remark = this.gridView1.GetFocusedRowCellDisplayText("BillRemark");// this.cmb_WHRemark.Text;
            //用循环上传到中间表ct——stockin    //更新本地入库单状态
            if (ids.T_InStock.Rows.Count > 0 && ids.T_InStock_Entry.Rows.Count > 0)
            {
                string a = ids.T_InStock_Entry.Rows[0]["IsWhiteFlag"].ToString();
                string ss = this._WMSAccess.Tran_Insert_CT_StockIn(ids.T_InStock, ids.T_InStock_Entry);

                if (ss == "")
                {
                    MessageBox.Show("上传成功");

                    string Des = string.Format("单据上传成功,上传人{0}", Utils.LoginUserName);
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);
                }
                else
                {
                    MessageBox.Show("上传失败：" + ss);

                    string Des = string.Format("单据上传失败,上传人{0}", Utils.LoginUserName);
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", voucherno);

                }
                this.btn_Query_Click(null,null);
            }
         

              
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string vno = this.gridView1.GetFocusedRowCellDisplayText("VoucherNO").ToString();
            //string stat = this.gridView1.GetFocusedRowCellDisplayText("IsClose").ToString();
            string send = this.gridView1.GetFocusedRowCellDisplayText("IsCancel").ToString();

            string business = this.gridView1.GetFocusedRowCellDisplayText("BusinessTypeCode").ToString();
            string btype = this.gridView1.GetFocusedRowCellDisplayText("BillType").ToString();
            string localorg = "";
            string org = this.gridView1.GetFocusedRowCellDisplayText("FactoryID").ToString();
            WMSDS localorgDS = this._WMSAccess.Select_T_Factory(false, true);
            WMSDS oDS = this._WMSAccess.Select_T_FactoryByCode(true, org);
            string connstr = "";
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
                    string localconnstr = localorgDS.T_Factory.Rows[0][localorgDS.T_Factory.FactoryPhoneColumn].ToString();
                    org = oDS.T_Factory.Rows[0][oDS.T_Factory.FactoryAbbrColumn].ToString();
                    connstr = oDS.T_Factory.Rows[0][oDS.T_Factory.FactoryPhoneColumn].ToString();
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
                    if (org == localorg)
                    {
                        MessageBox.Show("不能往本地库存组织发送入库明细");
                        return;
                    }
                    if (localconnstr == connstr)
                    {
                        MessageBox.Show("本地库存组织和接收库存组织连接串相同，不能发送入库明细，请检查库存组织映射");
                        return;
                    }

                }




            }
            if (vno == "")
            {
                MessageBox.Show("单号不正确。");
            }
            else if (send == "已传输")
            {
                MessageBox.Show("单据已传输，不能再次传输。");
            }
            else if (business == "移库入库")
            {
                MessageBox.Show("移库入库单不能发送明细。");
            }
            else if (btype == "红")
            {
                MessageBox.Show("红单不能发送入库单明细。");

            }
            else
            {
                if (DialogResult.No == MessageBox.Show("确定要发送入库单" + vno + "的明细吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;
                ////可以发送
                //string ret = this.TransferInStockBarcode(vno, org, connstr);
                //if (ret == "")
                //MessageBox.Show("发送成功");
                //this.btn_Query_Click(null,null);

                //可以发送
                string ret = this.TransferInStockBillForInStock(vno, connstr);

                //发送成功之后，更新本地上传标记

                if (ret == "")
                {
                    _WMSAccess.Update_IsTransfer_ForStockIn(vno);
                    MessageBox.Show("发送成功");

                    string Des = string.Format("单据发送成功,发送人{0}", Utils.LoginUserName);
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", vno);

                }
                else
                    MessageBox.Show(ret);
                this.btn_Query_Click(null, null);
            }
        }

        private string TransferInStockBillForInStock(string vno, string connstr)
        {
                //生成分录，直接以入库单的方式查询出来instock,instock_entry,和product in
                WMSDS wmsDS = this._WMSAccess.Select_T_InStockBillForTransferInStock(vno);
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
                string ret = this._WMSAccess.Tran_TransferInStockBillByInStock(vno, wmsDS, connstr);
                return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vno">入库单号</param>
        /// <param name="org">异地组织</param>
        /// <returns></returns>
        private string TransferInStockBarcode(string vno,string org,string orgConnStr)
        {
            //查询调拨单信息
            //InterfaceDS iDS = this._WMSAccess.Select_T_OutStockAndEntry_RelationForUpload(dbvno, "", "");
            //if (iDS.CT_StockOut.Rows.Count > 0)
          
            //{
                //生成本地入库单抬头，并在发送成功后占用这个单号
                //string invno = this.NewInStockVoucherNO();
                //if (invno == "")
                //{
                //    return "生成异地入库单号失败，无法传输数据";
                //}
                //string org = iDS.CT_StockOut.Rows[0][iDS.CT_StockOut.Cdefine1Column].ToString();
                //生成分录，直接以入库单的方式查询出来instock,instock_entry,和product in
                WMSDS wmsDS = this._WMSAccess.Select_T_InStockDetailForTransfer(vno);
              
                int d = wmsDS.T_ProductLife.Rows.Count;
                if (d == 0)
                    return "未查询到入库记录，无法传输数据。";
                int e = wmsDS.T_Product_In.Rows.Count;
                if (e == 0)
                    return "未查询到入库条码，无法传输数据。";
                    //开始用事务写入远程服务器
                    string ret = this._WMSAccess.Tran_TransferInStockDetail(vno, wmsDS, orgConnStr);
                    return ret;
        }
    }
}
