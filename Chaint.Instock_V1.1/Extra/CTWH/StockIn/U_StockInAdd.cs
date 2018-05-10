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

namespace CTWH.StockIn
{
    public partial class U_StockInAdd : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        MainLayout _MainLayout = null;
        /// <summary>
        /// 全局的单据号
        /// </summary>
        public string _VourcherNO = "";
        /// <summary>
        /// 全局的单据ID
        /// </summary>
        public string _VourcherID = "";
        /// <summary>
        /// 打开方式
        /// </summary>
        public string _OpenType = "";
        private bool _IsLoad = false;
        WMSDS _WMS_MAIN;
        public U_StockInAdd()
        {
            InitializeComponent();
        }
        public U_StockInAdd(MainLayout tl)
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
        private void U_StockInAdd_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMS_MAIN = new WMSDS();
            //加载业务类型
            LoadBussinessType();
            //加载组织
            LoadOrg();
            LoadT_User();
            LoadT_Factory();
            this.LoadGrade();
            this.LoadWhiteFlag();
            this.LoadSpecialCustomer();
            //加载仓库
            //LoadWarehouse();
            LoadDefaultValue();
            LoadPaperCert();

            //if(this._OpenType=="add")
            //    NewVoucherPlan();

             if (this._OpenType == "edit")

                LoadEditValue(this._VourcherNO);
            else if (this._OpenType == "view")
            {
                LoadEditValue(this._VourcherNO);
                this.HideViewControl();
            }
            this.dateEnd.DateTime = DateTime.Now;
            this.dateStart.DateTime = DateTime.Now.AddDays(-1);
            this.cmb_ProductType.SelectedIndex = 0;
            this.dtBill.DateTime = DateTime.Now;

        }
        /// <summary>
        /// 加载特殊客户
        /// </summary>
        private void LoadSpecialCustomer()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("TSKH");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.cmb_TSKH.Properties.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

            //this.look1.Properties.DataSource = iDS.CT_ZDYZD;
            //this.look1.Properties.DisplayMember = "Name";
            //this.look1.Properties.ValueMember = "Name";
        }
        /// <summary>
        /// 加载等级
        /// </summary>
        private void LoadGrade()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("CPDJ");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.cmb_Grade.Properties.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());
        }
        /// <summary>
        /// 加载商标类型
        /// </summary>
        private void LoadWhiteFlag()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("SBLX");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.cmb_WhiteFlag.Properties.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());
        }

        /// <summary>
        /// 加载纸种认证
        /// </summary>
        private void LoadPaperCert()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("ZZRZ");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.cmb_PaperCert.Properties.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());
        }

        private void HideViewControl()
        {
            this.btn_Save.Enabled = false;
            this.btn_Select.Enabled = false;
        }
        /// <summary>
        /// 查询数据库里面的数据，填充到页面
        /// </summary>
        private void LoadEditValue(string vno)
        {
            //throw new NotImplementedException();
            _DetailStatDS  = this._WMSAccess.Select_T_InStockBill(vno);

            //查询抬头
            if (_DetailStatDS.T_InStock.Rows.Count > 0)
            {
                this.cmb_VoucherNO.Text = vno;// _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.BusinessTypeColumn].ToString();

                this.cmb_Emp.EditValue = _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.KeeperColumn].ToString();
                this.cmb_Org.EditValue = _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.FactoryIDColumn].ToString();
                this.cmb_RedBlue.Text = _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.BillTypeColumn].ToString();
                this.cmb_SourceVoucherNO.EditValue = _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.SourceVoucherNOColumn].ToString();
                this.cmb_Warehouse.EditValue = _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.WHCodeColumn].ToString();
                this.cmb_WHRemark.Text = _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.BillRemarkColumn].ToString();
                string up=_DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.IsUploadColumn].ToString();
                this._IsLoad = true;
                this.cmb_BusinessType.EditValue = _DetailStatDS.T_InStock.Rows[0][_DetailStatDS.T_InStock.BusinessTypeColumn].ToString();
                this._IsLoad = false;

                this.txtStat.Text = up == "0" ? "未上传" : "已上传";
                this.txtStat.ForeColor = up == "0" ? Color.Blue :Color.Red;
                this.btn_Save.Enabled = up == "0" ?true : false;
                this.btn_Select.Enabled = this.btn_Save.Enabled;
            }
            //查询明细
                this.grid_Detail.DataSource = _DetailStatDS.CT_StockInDetail;
                this.gridView1.BestFitColumns();
            //查询统计
                this.grid_Stat.DataSource = _DetailStatDS.CT_StockIn;
                this.gridView2.BestFitColumns();

                this.cmb_RedBlue.Properties.ReadOnly = true;

        }
        private void LoadT_Factory()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_Factory(true, false);
            wmsDS.T_Factory.Rows.Add(wmsDS.T_Factory.NewT_FactoryRow());
            this.cmb_Factory.Properties.DataSource = wmsDS.T_Factory;
            this.cmb_Factory.Properties.ValueMember = "OnlyID";
            this.cmb_Factory.Properties.DisplayMember = "MachineID";
        }
        /// <summary>
        /// 新建一个入库单号
        /// </summary>
        private void NewVoucherPlan()
        {
            //如果已经显示单号，再保存则不再生成单号
            if (cmb_VoucherNO.Text != "")
            {
                return;
            }
            //刷新单号
            WMSDS orgDS = this._WMSAccess.Select_T_Factory(false, true);
            if (orgDS.T_Factory.Rows.Count > 0)
            {
                string type = Utils.WMSVoucherType._BillIn;
                string machineid = orgDS.T_Factory.Rows[0][orgDS.T_Factory.MachineIDColumn].ToString();// Utils.LoginMachineID;
                string planNO = this._WMSAccess.Get_T_StockIn_NewFlow(type, machineid);
                this.cmb_VoucherNO.Text = planNO;
                this._VourcherID = "";
            }
            else
            {
                MessageBox.Show("请先配置本地库存组织");
            }
        }
        private void LoadT_User()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_User("");
            wmsDS.T_User.Rows.Add(wmsDS.T_User.NewT_UserRow());

            this.cmb_User.Properties.DataSource = wmsDS.T_User;
            this.cmb_User.Properties.ValueMember = "UserCode";
            this.cmb_User.Properties.DisplayMember = "UserName";
        }
        private void LoadDefaultValue()
        {
            //this.cmb_BillType.SelectedIndex = 0;
            //this.cmb_TradeType.SelectedIndex = 0;
            //this.cmb_TransportType.SelectedIndex = 0;
            this.cmb_RedBlue.SelectedIndex = 0;
            this.cmb_BusinessType.EditValue = "SCRK";
        }

        private void LoadBussinessType()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_BusinessType("in","A");

            this.cmb_BusinessType.Properties.DataSource = wmsDS.T_Business_Type;
            this.cmb_BusinessType.Properties.ValueMember = "BusinessCode";
            this.cmb_BusinessType.Properties.DisplayMember = "BusinessName";
        }
        private void LoadDept(string org, string dept)
        {
            //InterfaceDS iDS = this._WMSAccess.Select_CT_BMZD(org, dept);

            //this.cmb_Dept.Properties.DataSource = iDS.CT_BMZD;
            //this.cmb_Dept.Properties.ValueMember = "DeptCode";
            //this.cmb_Dept.Properties.DisplayMember = "DeptName";
        }
        private void LoadEmp(string org, string emp)
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_RYZD(org, emp);

            this.cmb_Emp.Properties.DataSource = iDS.CT_RYZD;
            this.cmb_Emp.Properties.ValueMember = "EmpCode";
            this.cmb_Emp.Properties.DisplayMember = "EmpName";
        }
        private void LoadOrg()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_OrgInfo("");

            this.cmb_Org.Properties.DataSource = iDS.CT_OrgInfo;
            this.cmb_Org.Properties.ValueMember = "ORGCode";
            this.cmb_Org.Properties.DisplayMember = "ORGName";
        }

        private void LoadWarehouse(string org)
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_CKZD(org);
            iDS.CT_CKZD.Rows.Add(iDS.CT_CKZD.NewCT_CKZDRow());
            this.cmb_Warehouse.Properties.DataSource = iDS.CT_CKZD;
            this.cmb_Warehouse.Properties.ValueMember = "CKBH";
            this.cmb_Warehouse.Properties.DisplayMember = "CKMC";
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
                    if (this.cmb_VoucherNO.Text != "")
                    {
                        LoadEditValue(this.cmb_VoucherNO.Text);
                    }
                    break;
                case Utils.WMSMenu._Exit:
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭页面吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        this._MainLayout.AddUserControl(null, "");
                    break;
            }

        }

        private void cmb_Factory_EditValueChanged(object sender, EventArgs e)
        {
            //加载部门
            //LoadDept(this.cmb_Factory.EditValue == null ? "-1" : this.cmb_Factory.EditValue.ToString(), "");
            string org = this.cmb_Org.EditValue == null ? "" : this.cmb_Org.EditValue.ToString();
            LoadWarehouse(org);
              
        }

        private void cmb_Warehouse_EditValueChanged(object sender, EventArgs e)
        {
            //加载制单人
            LoadEmp(this.cmb_Warehouse.EditValue == null ? "-1" : this.cmb_Warehouse.EditValue.ToString(), "");
        }
        InterfaceDS _DetailStatDS;
        private void btn_Select_Click(object sender, EventArgs e)
        {
            //查询出一段时间入库的纸的明细和统计
            string factory = this.cmb_Factory.Text.Trim()=="全部"?"":this.cmb_Factory.Text.Trim();  //机台号
            string user = this.cmb_User.EditValue == null ? "" : this.cmb_User.EditValue.ToString();//入库人员
            string dateS = this.dateStart.DateTime.ToString("yyyy-MM-dd HH:mm:ss");//入库开始时间
            string dateE = this.dateEnd.DateTime.ToString("yyyy-MM-dd HH:mm:ss");//入库结束时间
            string pType = this.cmb_ProductType.Text.Split('.')[0];//卷筒平板 
            string rb = this.cmb_RedBlue.Text.Trim();  //入库红蓝类型
            string specific = this.cmb_Specific.Text == "" || this.cmb_Specific.Text == "0" ? "" : this.cmb_Specific.Text.Trim();
            if (specific != "")
            {
                specific = string.Format("'{0}'", specific.Replace(",", "','"));
            }
            string batchNO = this.cmb_BatchNO.Text;
            string businesstype = this.cmb_BusinessType.EditValue.ToString();
            //
            string material = this.cmb_Material.Text.TrimEnd(',',' ');
            if (material != "")
            {
                material = string.Format("'{0}'", material.Replace(",", "','"));
            }
            string grade = this.cmb_Grade.Text.Trim();
            string order = this.cmb_Order.Text.Trim();
            string wFlag = this.cmb_WhiteFlag.Text.Trim();
            //
            string tskh = this.cmb_TSKH.Text.Trim();
            string whremark = this.cmb_warehouseRemark.Text.Trim();
            string productids = "";
            string papercert = this.cmb_PaperCert.Text.Trim();
            if (this.txt_Except.Text != "")
            {
                //获得productids
                foreach (string s in this.txt_Except.Lines)
                {
                    if (s.Trim().Length > 0)
                    {
                        productids = productids + "'" + s.Trim() + "',";

                    }

                }

                if (productids.Length > 0)
                {
                    productids = productids.TrimEnd(',');

                }
            }

            if (rb == "R")
                businesstype = "HDRK";
            _DetailStatDS = this._WMSAccess.Select_T_Product_InDetailAndStat(factory, user, dateS, dateE, pType, batchNO, this._VourcherID, rb, specific, businesstype, material, grade, order, wFlag, tskh, whremark, productids,papercert);
            this.grid_Detail.DataSource = _DetailStatDS.CT_StockInDetail;
            //this.gridView1.BestFitColumns();


           
            //WMSDS wms2 = this._WMSAccess.Select_T_Product_InForStat(factory, user, dateS, dateE, pType);
            this.grid_Stat.DataSource = _DetailStatDS.CT_StockIn;
            this.gridView2.BestFitColumns();
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
       


            string voucherno =this.cmb_VoucherNO.Text;
            //查询这个voucher的信息，判断是否已经保存，是否已经上传等？
        WMSDS ids =    this._WMSAccess.Select_T_InStockByVoucherNO(voucherno);

        string isup = ids.T_InStock.Rows[0]["IsUpload"].ToString();
        string update = ids.T_InStock.Rows[0]["UploadDate"].ToString();
        if (isup == "1")
        {
            MessageBox.Show("单据已于" + update + "上传，不能再次上传");
            return;
        }

            string business = this.cmb_BusinessType.EditValue.ToString();
            string redblue = this.cmb_RedBlue.Text;
            string org = this.cmb_Org.EditValue.ToString();
            string wh = this.cmb_Warehouse.EditValue.ToString();
            string emp = this.cmb_Emp.EditValue.ToString();
            string source = this.cmb_SourceVoucherNO.Text;
            string remark = this.cmb_WHRemark.Text;
            //用循环上传到中间表ct——stockin    //更新本地入库单状态
            if (ids.T_InStock.Rows.Count>0 && ids.T_InStock_Entry.Rows.Count > 0) {
                string a = ids.T_InStock_Entry.Rows[0]["IsWhiteFlag"].ToString();
                 string ss = this._WMSAccess.Tran_Insert_CT_StockIn(ids.T_InStock, ids.T_InStock_Entry);

                 if (ss == "")
                     MessageBox.Show("上传成功");
                 else {
                     MessageBox.Show("上传失败："+ss);
                 }
                }
         

              
            
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (!this.CheckCanSave())
                return;
            if (this._OpenType == "add")
                NewVoucherPlan();
            
            if (DialogResult.No == MessageBox.Show(string.Format("是否要保存入库单【{0}】？",cmb_VoucherNO.Text.Trim()), "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;

            //保存这个入库单
            string voucherno = this.cmb_VoucherNO.Text;
            string business = this.cmb_BusinessType.EditValue.ToString();
            string redblue = this.cmb_RedBlue.Text;
            string org = this.cmb_Org.EditValue.ToString();
            string wh = this.cmb_Warehouse.EditValue.ToString();
            string emp = this.cmb_Emp.EditValue.ToString();
            string source = this.cmb_SourceVoucherNO.Text;
            string remark = this.cmb_WHRemark.Text;


            //1.保存表头
            WMSDS.T_InStockRow isRow = new WMSDS().T_InStock.NewT_InStockRow();
            isRow.BillRemark = remark;//this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.BillRemarkColumn].ToString();
            isRow.BillType = redblue;//this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.BillTypeColumn].ToString();
            isRow.BusinessType = business;//this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.BusinessTypeColumn].ToString();
            isRow.FactoryID = org;//this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.FactoryIDColumn].ToString();
            isRow.Keeper = emp;// this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.KeeperColumn].ToString();
            isRow.SourceVoucherNO = source;//this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.SourceVoucherNOColumn].ToString();
            isRow.VoucherNO = voucherno;//this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.VoucherNOColumn].ToString();
            isRow.WHCode = wh;//this._DetailStatDS.CT_StockIn.Rows[0][this._DetailStatDS.CT_StockIn.WHCodeColumn].ToString();
            isRow.BillDate = this.dtBill.DateTime;
            isRow.IsCancel = "0";
            isRow.IsCheck = "0";
            isRow.IsClose = "0";
            isRow.IsUpload = "0";
            //判断是否已有这个单据了

            if (this._WMSAccess.Select_T_InStockByVoucherNO(this.cmb_VoucherNO.Text).T_InStock.Rows.Count > 0)
            
            {
                if (DialogResult.Yes == MessageBox.Show("单据" + this.cmb_VoucherNO.Text + "已存在，是否要覆盖？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            
                {
                    //更新这个单据
                    string ss = this._WMSAccess.Tran_Update_T_InStock(isRow, this._DetailStatDS);

                    //先删除
                    //string ss = this._WMSAccess.Tran_Delete_T_InStock(this.cmb_VoucherNO.Text);

                    if (ss == "")
                    {
                        MessageBox.Show("更新成功");
                      //  this.btn_Select_Click(null, null);
                        if (this.cmb_VoucherNO.Text != "")
                        {
                            LoadEditValue(this.cmb_VoucherNO.Text);
                        }

                        string Des = string.Format("保存操作,{0}覆盖已存在单据{1},入库类型:{2}{3},入库单包含纸卷:{4}个", this.cmb_Emp.Text.ToString(), this.cmb_VoucherNO.Text.ToString(), this.cmb_RedBlue.Text.ToString(), this.cmb_BusinessType.Text.ToString(), this.gridView1.RowCount.ToString());
                        _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", cmb_VoucherNO.Text.ToString());
                    }
                    else
                        MessageBox.Show("更新失败：" + ss);
                }
            }
            else
            {
              
                //string b=    this._DetailStatDS.CT_StockIn.Rows[0]["IsWhiteFlag"].ToString();
                string ss = this._WMSAccess.Tran_Insert_StockIn(isRow, this._DetailStatDS);
                //4.更新T_Product里面的入库单号关联
                if (ss != "")
                    MessageBox.Show("保存失败："+ss);
                else
                {
                    MessageBox.Show("保存成功");

                    string Des = string.Format("保存操作,{0}新建单据{1},入库类型:{2}{3},入库单包含纸卷:{4}个",this.cmb_Emp.Text.ToString(), this.cmb_VoucherNO.Text.ToString(),this.cmb_RedBlue.Text.ToString(), this.cmb_BusinessType.Text.ToString(),this.gridView1.RowCount.ToString());
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", cmb_VoucherNO.Text.ToString());

                    cmb_VoucherNO.Text = "";

                    //保存成功后，清空
                    this._DetailStatDS.CT_StockIn.Clear();
                    this._DetailStatDS.CT_StockInDetail.Clear();

                    //grid_Detail.DataSource = null;
                    //grid_Stat.DataSource = null;
                    //this.btn_Select_Click(null, null);
                    //if (this.cmb_VoucherNO.Text != "")
                    //{
                    //    LoadEditValue(this.cmb_VoucherNO.Text);
                    //}
                }
            }

        }

        private bool CheckCanSave()
        {
            if (this._DetailStatDS == null) { MessageBox.Show("单据没有明细，不允许保存。"); return false; }
            if (this._DetailStatDS.CT_StockIn.Rows.Count == 0) { MessageBox.Show("单据没有查询统计数据，不允许保存。"); return false; }
            if (this._DetailStatDS.CT_StockInDetail.Rows.Count == 0)
            {
                MessageBox.Show("单据没有查询明细数据，不允许保存。"); return false;
            } if (this.cmb_BusinessType.Text == "") { MessageBox.Show("没有业务类型，不允许保存。"); return false; }
            if (this.cmb_Emp.Text == "") { MessageBox.Show("没有制单人，不允许保存。"); return false; }
            if (this.cmb_RedBlue.Text == "") { MessageBox.Show("没有红蓝单，不允许保存。"); return false; }
            if (this.cmb_Org.Text == "") { MessageBox.Show("没有组织，不允许保存。"); return false; }
            //if (this.cmb_VoucherNO.Text == "") { MessageBox.Show("没有单号，不允许保存。"); return false; }
            if (this.cmb_Warehouse.Text == "") { MessageBox.Show("没有仓库，不允许保存。"); return false; }

           
            return true;
        }

        private void cmb_VoucherNO_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //if(this._VourcherID=="")
            //this.NewVoucherPlan();
        }

        private void cmb_RedBlue_TextChanged(object sender, EventArgs e)
        {
            if (this.cmb_RedBlue.Text == "B")
            {
                this.cmb_RedBlue.ForeColor = Color.Blue;
            }
            else
            {
                this.cmb_RedBlue.ForeColor = Color.Red;
            }

            //if (this.cmb_RedBlue.Properties.ReadOnly)
            //{
            //    //  this.btn_Select_Click(null,null);
            //}
            //else
            //{
            //    this.btn_Select_Click(null, null);

            //}
        }

        private void cmb_BusinessType_TextChanged(object sender, EventArgs e)
        {
            if (!this._IsLoad)
            this.btn_Select_Click(null,null);
        }

        private void look1_EditValueChanged(object sender, EventArgs e)
        {
            //this.look1.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            //this.look1.Properties.·
        }

      
    }
}
