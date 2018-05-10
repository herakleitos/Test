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
using CTWH.StockReport;
using DevExpress.XtraReports.UI;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace CTWH.StockOut
{
    public partial class U_StockOutPrint : DevExpress.XtraEditors.XtraUserControl
    {
        Common.MSSQL.WMSAccess _WMSAccess;
        MainLayout _MainLayout = null;
        public string _VourcherNO = "", _SourceNO = "";
        WMSDS _WMS_MAIN;

        public U_StockOutPrint()
        {
            InitializeComponent();
        }
          void U_VoucherPlanAdd_Disposed(object sender, EventArgs e)
        {
            this._MainLayout.OnMenuClickEvent -= new MenuClickEventHandle(OnMenuClickEventProcess);
        }

          public U_StockOutPrint(MainLayout tl)
        {
            InitializeComponent();
            this._MainLayout = tl;
            this._MainLayout.OnMenuClickEvent += new MenuClickEventHandle(OnMenuClickEventProcess);
            this.Disposed += new EventHandler(U_VoucherPlanAdd_Disposed);
           
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
        public void OnMenuClickEventProcess(object sender, MenuClickEventArgs e)
        {

            switch (e.MenuName)
            {
                case Utils.WMSMenu._New:

                    ////新建一个发货通知单
                    //if (DialogResult.Yes == MessageBox.Show("确定要新建发货通知单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    //{
                    //    this.NewVoucherPlan();

                    //}
                    //break;
                case Utils.WMSMenu._Save:
                    ////判断是新建还是更新
                    //WMSDS wmsDS = this._WMSAccess.Select_T_OutStockByFK(this.cmb_VoucherNO.Text, "", "", "", "", "", "", 0, 0, 0);
                    //if (wmsDS.T_OutStock.Rows.Count == 0)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("确定要保存销售出库单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    //        this.SaveNewVoucherPlan();
                    //}
                    //else
                    //{
                    //    string date = wmsDS.T_OutStock.Rows[0]["Biller"].ToString();
                    //    string user = wmsDS.T_OutStock.Rows[0]["BillDate"].ToString();
                    //    if (DialogResult.Yes == MessageBox.Show(this.cmb_VoucherNO.Text + "已于" + date + "由" + user + "创建，要覆盖吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    //        this.SaveUpdateVoucherPlan(wmsDS.T_OutStock.Rows[0][wmsDS.T_OutStock.OnlyIDColumn].ToString());
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
                    if (this.cmb_VoucherNO.Text != "" &&this._VourcherNO!="")
                    {
                        //加载张贴方式
                        LoadDefaultValue();

                        //加载
                        LoadPrintPre();

                        //加载表头，明细，统计
                        LoadVoucherSituation();
                    }
                    break;
                case Utils.WMSMenu._Exit:
                    if (DialogResult.Yes == MessageBox.Show("确定要关闭页面吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        this._MainLayout.AddUserControl(null, "");
                    break;
            }

        }
        private void U_StockOutPrint_Load(object sender, EventArgs e)
        {
            //LoadSourceEditValue();
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMS_MAIN = new WMSDS();


            //加载业务类型
            LoadBussinessType();
            //加载机台号
            LoadOrg();

            //加载仓库
            LoadWarehouse();

            //加载客户
            LoadCustomer();
            //加载等级
            LoadGrade();
            //加载物料
            LoadMeterialCodeToGridItem();
            //加载规格
            LoadSpecific();
            //加载令包类型

            //加载客户品牌

            //加载客户专用
            LoadSpecCust();
            //加载产品专用
            LoadSpecProd();
            //加载张贴方式
            LoadDefaultValue();

            //加载
            LoadPrintPre();

            //加载表头，明细，统计
            LoadVoucherSituation();
        }

        private void LoadPrintPre()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_OutStock_PrintPre(this._VourcherNO);
            if (wmsDS.T_OutStock_PrintPre.Rows.Count > 0)
            {
                this.txt_Box.Text = wmsDS.T_OutStock_PrintPre.Rows[0][wmsDS.T_OutStock_PrintPre.ContainerSizeColumn].ToString();
                this.txt_Bulk.Text = wmsDS.T_OutStock_PrintPre.Rows[0][wmsDS.T_OutStock_PrintPre.ContainerBulkColumn].ToString();
                this.txt_RollWeight.Text = wmsDS.T_OutStock_PrintPre.Rows[0][wmsDS.T_OutStock_PrintPre.RollPlusColumn].ToString();
                this.txtPalletWeight.Text = wmsDS.T_OutStock_PrintPre.Rows[0][wmsDS.T_OutStock_PrintPre.PalletPlusColumn].ToString();
                this.cmb_ControlNO.Text = wmsDS.T_OutStock_PrintPre.Rows[0][wmsDS.T_OutStock_PrintPre.ControlNOColumn].ToString();
                this._PrintOnlyID = Convert.ToInt32(wmsDS.T_OutStock_PrintPre.Rows[0][wmsDS.T_OutStock_PrintPre.OnlyIDColumn].ToString());
            }
            else//还未打印过，则赋予默认值
            {
                if (cmb_TradeType.Text == "内贸")
                {
                    txt_RollWeight.Text = "0";
                    txtPalletWeight.Text = "0";
                    txt_Box.Text = "0";
                    txt_Bulk.Text = "0";
                }

                else if (cmb_TradeType.Text == "出口")
                {
                    txt_RollWeight.Text = "5";
                    txtPalletWeight.Text = "25";
                    txt_Box.Text = "20";
                    txt_Bulk.Text = "23";
                }
            }
        }

        private void LoadSpecProd()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("CPZY");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridCmb_SpecProd.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());
        }

        private void LoadSpecCust()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("KHZY");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridCmb_SpecCust.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());
        }

        private void LoadSpecific()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("GGXH");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridItem_Specific.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());
        }

        private void LoadDefaultValue()
        {
            this.cmb_BusinessType.ItemIndex = 0;
            this.cmb_BillType.SelectedIndex = 0;
            this.cmb_TradeType.SelectedIndex = 0;
            this.cmb_TransportType.SelectedIndex = 0;
        }

        private void LoadGrade()
        {
            WMSDS iDS = this._WMSAccess.Select_T_Grade("");
            for (int i = 0; i < iDS.T_Grade.Rows.Count; i++)
                this.gridItem_Grade.Items.Add(iDS.T_Grade.Rows[i]["GradeName"].ToString());

        }

        private void LoadMeterialCodeToGridItem()
        {
            InterfaceDS wmsDS = this._WMSAccess.Select_CT_WLZD("");

            this.gridItem_Material.DataSource = wmsDS.CT_WLZD;
            this.gridItem_Material.ValueMember = "WLBH";
            this.gridItem_Material.DisplayMember = "WLBH";
        }
        private void LoadBussinessType()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_BusinessType("out","A");

            this.cmb_BusinessType.Properties.DataSource = wmsDS.T_Business_Type;
            this.cmb_BusinessType.Properties.ValueMember = "BusinessCode";
            this.cmb_BusinessType.Properties.DisplayMember = "BusinessName";
        }
        private void LoadDept(string org, string dept)
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_BMZD(org, dept);

            this.cmb_Dept.Properties.DataSource = iDS.CT_BMZD;
            this.cmb_Dept.Properties.ValueMember = "DeptCode";
            this.cmb_Dept.Properties.DisplayMember = "DeptName";
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

            this.cmb_Factory.Properties.DataSource = iDS.CT_OrgInfo;
            this.cmb_Factory.Properties.ValueMember = "ORGCode";
            this.cmb_Factory.Properties.DisplayMember = "ORGName";

            this.cmb_OrgTo.Properties.DataSource = iDS.CT_OrgInfo;
            this.cmb_OrgTo.Properties.ValueMember = "ORGCode";
            this.cmb_OrgTo.Properties.DisplayMember = "ORGName";
        }

        private void LoadWarehouse()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_CKZD("");

            this.cmb_Warehouse.Properties.DataSource = iDS.CT_CKZD;
            this.cmb_Warehouse.Properties.ValueMember = "CKBH";
            this.cmb_Warehouse.Properties.DisplayMember = "CKMC";

            this.cmb_WHto.Properties.DataSource = iDS.CT_CKZD;
            this.cmb_WHto.Properties.ValueMember = "CKBH";
            this.cmb_WHto.Properties.DisplayMember = "CKMC";
        }
        private void LoadCustomer()
        {
            WMSDS iDS = this._WMSAccess.Select_T_Customer("");

            //this.cmb_CustomerName.Properties.DataSource = iDS.T_Customer;
            //this.cmb_CustomerName.Properties.ValueMember = "CustomerCode";
            for (int i = 0; i < iDS.T_Customer.Rows.Count; i++)
            {
                this.cmb_CustomerName.Properties.Items.Add(iDS.T_Customer.Rows[i][iDS.T_Customer.CustomerNameColumn].ToString().Trim());
            }
        }
        WMSReportDS repDS;
        private void LoadVoucherSituation()
        {
            if (this._VourcherNO != "") {
                 repDS = this._WMSAccess.Select_OutStockAndEntry_ForPrintByVoucherNO(this._VourcherNO);

                if (repDS.T_OutStock.Rows.Count > 0)
                {
                    this.cmb_VoucherNO.Text = repDS.T_OutStock.Rows[0][repDS.T_OutStock.VoucherNOColumn].ToString();
                    //this.cmb_Amount.Text=wmsDS.T_OutStock.Rows[0][wmsDS.T_OutStock.QtyColumn].ToString();
                   //this.cmb_BillNo.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.SourceVoucherNOColumn].ToString();
                    this.cmb_BillNo.Text = repDS.T_OutStock.Rows[0][repDS.T_OutStock.PickNOColumn].ToString();
                    this.cmb_BillType.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.BillTypeColumn].ToString();
                    this.cmb_Boat.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.ShipNOColumn].ToString();
                    this.cmb_BoxNO.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.ContainerNOColumn].ToString();
                    this.cmb_BusinessType.EditValue=repDS.T_OutStock.Rows[0][repDS.T_OutStock.BusinessTypeColumn].ToString();
                    this.cmb_Contract.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.OrderNOColumn].ToString();
                    this.cmb_CustomerName.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.CustomerNameColumn].ToString();
                    this.cmb_Dept.EditValue=repDS.T_OutStock.Rows[0][repDS.T_OutStock.DeptNameColumn].ToString();
                    this.cmb_Emp.EditValue=repDS.T_OutStock.Rows[0][repDS.T_OutStock.BillerColumn].ToString();
                    this.cmb_Factory.EditValue=repDS.T_OutStock.Rows[0][repDS.T_OutStock.FactoryIDColumn].ToString();
                    this.cmb_Lifter.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.ForklifterNOColumn].ToString();
                    this.cmb_Loader.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.CarrierNOColumn].ToString();
                    this.cmb_Port.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.PortNOColumn].ToString();
                    this.cmb_Remark.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.BillRemarkColumn].ToString();
                    this.cmb_TradeType.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.TradeTypeColumn].ToString();
                    this.cmb_TransportType.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.TransportTypeColumn].ToString();
                    this.cmb_VehicleNO.Text=repDS.T_OutStock.Rows[0][repDS.T_OutStock.VehicleNOColumn].ToString();
                    this.cmb_Warehouse.EditValue=repDS.T_OutStock.Rows[0][repDS.T_OutStock.WHCodeColumn].ToString();
                    //this.cmb_Weight.Text=wmsDS.T_OutStock.Rows[0][wmsDS.T_OutStock.AuxQtyColumn].ToString();;
                    this.cmb_WHto.EditValue = repDS.T_OutStock.Rows[0][repDS.T_OutStock.WHToCodeColumn].ToString(); 
                    this.cmb_OrgTo.EditValue = repDS.T_OutStock.Rows[0][repDS.T_OutStock.OrgToCodeColumn].ToString(); 
                    this.grid_Detail_International.DataSource = repDS.T_OutStockDetail_International_Rep;
                    this.grid_Detail.DataSource = repDS.T_OutStockDetail_Domestic_Rep;
                }
            
            
            
            }
        }

        ///// <summary>
        ///// 通过源单号查询出单据信息，并且加载到控件中
        ///// </summary>
        ///// <param name="sourceVoucher"></param>
        //private void LoadSourceEditValue(string sourceVoucher)
        //{
        //    WMSDS wmsDS = this._WMSAccess.Select_OutPlanAndEntry_RelationByVoucherNO(sourceVoucher);
        //    if (wmsDS.T_OutStock_Plan.Rows.Count > 0)
        //    {
        //        //this.NewVoucherPlan();
        //        this.cmb_VoucherNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.VoucherNOColumn].ToString();
        //        this.cmb_CustomerName.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.CustomerNameColumn].ToString();
        //        this.cmb_Amount.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.QtyColumn].ToString();
        //        this.cmb_BillNo.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.VoucherNOColumn].ToString(); //wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.PickNOColumn].ToString();
        //        this.cmb_BillType.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillTypeColumn].ToString();
        //        this.cmb_Boat.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BoatNOColumn].ToString();
        //        this.cmb_BoxNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BoxNOColumn].ToString();
        //        this.cmb_BusinessType.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BusinessTypeColumn].ToString();
        //        this.cmb_Contract.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.OrderNOColumn].ToString();
        //        this.cmb_Dept.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.DeptNameColumn].ToString();
        //        this.cmb_Emp.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillerColumn].ToString(); ;
        //        this.cmb_Factory.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.FactoryIDColumn].ToString();
        //        this.cmb_Lifter.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.ForklifterNOColumn].ToString(); ;
        //        this.cmb_Loader.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.PickNOColumn].ToString();
        //        this.cmb_Port.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.PortNOColumn].ToString();
        //        this.cmb_Remark.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillRemarkColumn].ToString();
        //        //this.cmb_SealNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.SealNOColumn].ToString();
        //        this.cmb_TradeType.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.TradeTypeColumn].ToString();
        //        this.cmb_TransportType.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.TransportTypeColumn].ToString();
        //        this.cmb_VehicleNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.VehicleNOColumn].ToString();
        //        this.cmb_Warehouse.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.WHCodeColumn].ToString();
        //        this.cmb_Weight.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.AuxQtyColumn].ToString();
        //        this.date_OutTime.DateTime = Convert.ToDateTime(wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillDateColumn].ToString());

        //    }
        //    else
        //    {
        //        MessageBox.Show("源单不存在，将打开空白单据。");
        //        //this.NewVoucherPlan();
        //        this.cmb_VoucherNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.VoucherNOColumn].ToString();
        //        this.cmb_CustomerName.Text = null;
        //        this.cmb_Amount.Text = "";
        //        this.cmb_BillNo.Text = "";
        //        this.cmb_BillType.Text = "";
        //        this.cmb_Boat.Text = "";
        //        this.cmb_BoxNO.Text = "";
        //        this.cmb_BusinessType.Text = "";
        //        this.cmb_Contract.Text = "";
        //        this.cmb_Dept.EditValue = null;
        //        this.cmb_Emp.EditValue = null;
        //        this.cmb_Factory.EditValue = null;
        //        this.cmb_Lifter.Text = "";
        //        this.cmb_Loader.Text = "";
        //        this.cmb_Port.Text = "";
        //        this.cmb_Remark.Text = "";
        //        //this.cmb_SealNO.Text = "";
        //        this.cmb_TradeType.Text = "";
        //        this.cmb_TransportType.Text = "";
        //        this.cmb_VehicleNO.Text = "";
        //        this.cmb_Warehouse.EditValue = null;
        //        this.cmb_Weight.Text = "";
        //        this.date_OutTime.DateTime = DateTime.Now;
        //        this.LoadDefaultValue();

        //    }
        //    this._WMS_MAIN = wmsDS;
        //    this.grid_Stat.DataSource = _WMS_MAIN.T_OutStock_Entry;
        //    this.grid_Detail.DataSource = _WMS_MAIN.T_OutStock_Product;

        //    this.gv_Stat.BestFitColumns();
        //}

        private void cmb_Factory_EditValueChanged(object sender, EventArgs e)
        {
            //加载部门
            LoadDept(this.cmb_Factory.EditValue == null ? "-1" : this.cmb_Factory.EditValue.ToString(), "");

              
        }

        private void cmb_Warehouse_EditValueChanged(object sender, EventArgs e)
        {
            //加载制单人
            LoadEmp(this.cmb_Warehouse.EditValue == null ? "-1" : this.cmb_Warehouse.EditValue.ToString(), "");
        }

        /// <summary>
        /// 检查打印数据 CH
        /// </summary>
        /// <returns></returns>
        private bool CheckPrintData()
        {
            if (this.txt_Bulk.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入体积!", "提示");
                txt_Bulk.Focus();
                return false;
            }

            if (this.txt_Box.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入箱形!", "提示");
                txt_Bulk.Focus();
                return false;
            }

            if (this.cmb_PrintName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择单据名称!", "提示");
                cmb_PrintName.Focus();
                return false;
            }
            return true;
        }


        int _PrintOnlyID;
        private void btn_Print_Click(object sender, EventArgs e)
        {
            bool blnSucc = CheckPrintData();
            if (!blnSucc) return;


            if (this.txt_Bulk.Text == "") this.txt_Bulk.Text = "0";
            if (this.txt_Box.Text == "") this.txt_Box.Text = "0";
            if (this.txtPalletWeight.Text == "") this.txtPalletWeight.Text = "0";
            if (this.txt_RollWeight.Text == "") this.txt_RollWeight.Text = "0";


            //如果是第一次打印就保存printpre
            WMSDS.T_OutStock_PrintPreRow ppr = new WMSDS().T_OutStock_PrintPre.NewT_OutStock_PrintPreRow();
            ppr.ContainerBulk = Convert.ToInt32(this.txt_Bulk.Text.Trim());
            ppr.ContainerSize = Convert.ToInt32(this.txt_Box.Text.Trim());
            ppr.ControlNO = this.cmb_ControlNO.Text.Trim();
            ppr.PalletPlus = Convert.ToInt32(this.txtPalletWeight.Text.Trim());
            ppr.PrintType = this.cmb_PrintName.Text.Trim();
            ppr.RollPlus = Convert.ToInt32(this.txt_RollWeight.Text.Trim());
            ppr.VoucherID = Convert.ToInt32(repDS.T_OutStock.Rows[0][repDS.T_OutStock.OnlyIDColumn].ToString());
            ppr.PrintDate = DateTime.Now;
            ppr.OnlyID = this._PrintOnlyID;
            int a = this._WMSAccess.Insert_T_OutStock_PrintPreByRow(ppr);
            WMSReportDS wmsDS2 = this._WMSAccess.Select_OutStockAndEntry_ForPrintByVoucherNO(this._VourcherNO);
            string strContainer = "", strSealNO = "";
            if (cmb_BoxNO.Text.Trim().Length > 0)
            {
                string strPattern = "^[A-Za-z0-9]{1,40}/[A-Za-z0-9]{1,40}$";
                bool bln = Regex.IsMatch(cmb_BoxNO.Text, strPattern);
                if (!bln)
                {
                    MessageBox.Show("请输入正确格式的箱/封号(如123/343)", "提示");
                    cmb_BoxNO.Focus();
                    return;
                }
                strContainer = cmb_BoxNO.Text.Split('/')[0];
                strSealNO = cmb_BoxNO.Text.Split('/')[1];
            }


            if (wmsDS2 != null && wmsDS2.T_OutStockTitle_Domestic_Rep.Rows.Count > 0 && wmsDS2.T_OutStockDetail_Domestic_Rep.Rows.Count > 0 && wmsDS2.T_OutStockLink_International_Rep.Rows.Count > 0)
            {
                if (this.cmb_PrintName.Text == "出库磅码单")
                {
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        Rep_StockOutDetail repd = new Rep_StockOutDetail();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        //this.lbCarrier.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString(); ;
                        //this.lbContainerNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString(); ;
                        //this.lbControlNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString(); ;
                        //this.lbCustomerName.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString(); ;
                        //this.lbDate_Exec.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString(); ;
                        //this.lbDriverNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString(); ;
                        //this.lbForklifter.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString(); ;
                        //this.lbMachine.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString(); ;
                        //this.lbOrderNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString(); ;
                        //this.lbSealNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString(); ;
                        //this.lbSourceVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString(); ;
                        ////this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
                        //this.lbVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();
                        //repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.].ToString();
                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    else
                    {
                        Rep_StockOutDetail_International repd = new Rep_StockOutDetail_International();
                        repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                        //repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                        repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                        repd.DataSource = wmsDS2.T_OutStockDetail_International_Rep;// this.grid_Detail_International.DataSource;
                        //repd.RequestParameters = false;

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainer"].Value = strContainer;

                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                }
                else if (this.cmb_PrintName.Text == "出库磅码单(加线长)")
                {
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        Rep_StockOutDetail_WithLength repd = new Rep_StockOutDetail_WithLength();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        //this.lbCarrier.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString(); ;
                        //this.lbContainerNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString(); ;
                        //this.lbControlNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString(); ;
                        //this.lbCustomerName.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString(); ;
                        //this.lbDate_Exec.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString(); ;
                        //this.lbDriverNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString(); ;
                        //this.lbForklifter.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString(); ;
                        //this.lbMachine.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString(); ;
                        //this.lbOrderNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString(); ;
                        //this.lbSealNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString(); ;
                        //this.lbSourceVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString(); ;
                        ////this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
                        //this.lbVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();
                        //repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.].ToString();
                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                }
                else if (this.cmb_PrintName.Text == "出口装货联系单")
                {
                    Rep_StockOutLink_International repd = new Rep_StockOutLink_International();
                    repd.Parameters["parPick"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                    //repd.Parameters["parSeal"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                    // repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                    repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                    repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                    repd.Parameters["parBox"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerSizeColumn].ToString();
                    repd.Parameters["parContract"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.OrderNOColumn].ToString();
                    repd.Parameters["parVehichl"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.VehicleNOColumn].ToString();

                    repd.Parameters["parSeal"].Value = strSealNO;
                    repd.Parameters["parContainer"].Value = strContainer;

                    string bulk = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerBulkColumn].ToString();
                    decimal c = wmsDS2.T_OutStockLink_International_Rep.Rows.Count;
                    int b = Convert.ToInt32(Math.Round(c / 2, 0, MidpointRounding.AwayFromZero));
                    //把抬头里面的体积赋值给联系单里面的体积，并且找到N行中的中间那一行。
                    string ss = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerBulkColumn].ToString();
                    wmsDS2.T_OutStockLink_International_Rep.Rows[b - 1][wmsDS2.T_OutStockLink_International_Rep.BulkColumn] = ss;
                    repd.DataSource = wmsDS2.T_OutStockLink_International_Rep;
                    Rep_StockOutLink_International_Sub repdsub = new Rep_StockOutLink_International_Sub();
                    repdsub.subRep1.ReportSource = repd;
                    repdsub.subRep2.ReportSource = repd;
                    // repdsub.Print();

                    repdsub.ShowPreviewDialog();
                }
                else if (this.cmb_PrintName.Text == "出口装货联系单（生活用纸）")
                {
                    Rep_StockOutLink_InternationalForPM27 repd = new Rep_StockOutLink_InternationalForPM27();
                    repd.Parameters["parPick"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                    //repd.Parameters["parSeal"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                    // repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                    repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                    repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                    repd.Parameters["parBox"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerSizeColumn].ToString();
                    repd.Parameters["parContract"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.OrderNOColumn].ToString();
                    repd.Parameters["parVehichl"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.VehicleNOColumn].ToString();

                    repd.Parameters["parSeal"].Value = strSealNO;
                    repd.Parameters["parContainer"].Value = strContainer;

                    string bulk = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerBulkColumn].ToString();
                    decimal c = wmsDS2.T_OutStockLink_International_Rep.Rows.Count;
                    int b = Convert.ToInt32(Math.Round(c / 2, 0, MidpointRounding.AwayFromZero));
                    //把抬头里面的体积赋值给联系单里面的体积，并且找到N行中的中间那一行。
                    string ss = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerBulkColumn].ToString();
                    wmsDS2.T_OutStockLink_International_Rep.Rows[b - 1][wmsDS2.T_OutStockLink_International_Rep.BulkColumn] = ss;
                    repd.DataSource = wmsDS2.T_OutStockLink_International_Rep;
                    Rep_StockOutLink_International_Sub repdsub = new Rep_StockOutLink_International_Sub();
                    repdsub.subRep1.ReportSource = repd;
                    repdsub.subRep2.ReportSource = repd;
                    // repdsub.Print();

                    repdsub.ShowPreviewDialog();
                }
                else if (this.cmb_PrintName.Text == "出库磅码单（接头）")
                {
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        Rep_StockOutDetailWithSplice repd = new Rep_StockOutDetailWithSplice();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        //this.lbCarrier.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString(); ;
                        //this.lbContainerNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString(); ;
                        //this.lbControlNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString(); ;
                        //this.lbCustomerName.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString(); ;
                        //this.lbDate_Exec.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString(); ;
                        //this.lbDriverNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString(); ;
                        //this.lbForklifter.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString(); ;
                        //this.lbMachine.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString(); ;
                        //this.lbOrderNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString(); ;
                        //this.lbSealNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString(); ;
                        //this.lbSourceVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString(); ;
                        ////this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
                        //this.lbVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    else
                    {
                        Rep_StockOutDetailWithSplice_International repd = new Rep_StockOutDetailWithSplice_International();
                        repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                        //repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                        repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                        repd.DataSource = wmsDS2.T_OutStockDetail_International_Rep;// this.grid_Detail_International.DataSource;
                        //repd.RequestParameters = false;

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainer"].Value = strContainer;

                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                }
                else if (this.cmb_PrintName.Text == "生活用纸出库磅码单")
                {
                    #region 生活用纸出库磅码单
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        PM27Rep_StockOutDetail repd = new PM27Rep_StockOutDetail();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        //this.lbCarrier.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString(); ;
                        //this.lbContainerNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString(); ;
                        //this.lbControlNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString(); ;
                        //this.lbCustomerName.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString(); ;
                        //this.lbDate_Exec.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString(); ;
                        //this.lbDriverNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString(); ;
                        //this.lbForklifter.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString(); ;
                        //this.lbMachine.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString(); ;
                        //this.lbOrderNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString(); ;
                        //this.lbSealNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString(); ;
                        //this.lbSourceVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString(); ;
                        ////this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
                        //this.lbVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    else
                    {
                        PM27Rep_StockOutDetail_International repd = new PM27Rep_StockOutDetail_International();
                        repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                        //repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                        repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                        repd.DataSource = wmsDS2.T_OutStockDetail_International_Rep;// this.grid_Detail_International.DataSource;
                        //repd.RequestParameters = false;

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainer"].Value = strContainer;

                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    #endregion
                }
                else if (this.cmb_PrintName.Text == "生活用纸出口日本")
                {
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        PM27Rep_StockOutDetail repd = new PM27Rep_StockOutDetail();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    else
                    {
                        PM27Rep_StockOutDetail_International_Japan repd = new PM27Rep_StockOutDetail_International_Japan();
                        repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();

                        repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                        repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                        repd.DataSource = wmsDS2.T_OutStockDetail_International_Rep;// this.grid_Detail_International.DataSource;


                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainer"].Value = strContainer;

                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                }
                else if (this.cmb_PrintName.Text == "生活用纸出库磅码单（接头）")
                {
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        PM27Rep_StockOutDetailWithSplice repd = new PM27Rep_StockOutDetailWithSplice();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        //this.lbCarrier.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString(); ;
                        //this.lbContainerNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString(); ;
                        //this.lbControlNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString(); ;
                        //this.lbCustomerName.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString(); ;
                        //this.lbDate_Exec.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString(); ;
                        //this.lbDriverNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString(); ;
                        //this.lbForklifter.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString(); ;
                        //this.lbMachine.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString(); ;
                        //this.lbOrderNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString(); ;
                        //this.lbSealNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString(); ;
                        //this.lbSourceVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString(); ;
                        ////this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
                        //this.lbVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    else
                    {
                        PM27Rep_StockOutDetail_InternationalWithSplice repd = new PM27Rep_StockOutDetail_InternationalWithSplice();
                        repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                        //repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                        repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                        repd.DataSource = wmsDS2.T_OutStockDetail_International_Rep;// this.grid_Detail_International.DataSource;
                        //repd.RequestParameters = false;

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainer"].Value = strContainer;

                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                }
                else if (this.cmb_PrintName.Text == "淋膜纸磅码单")
                {
                    #region 生活用纸淋膜出库磅码单
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        Rep_StockOutDetail_Film repd = new Rep_StockOutDetail_Film();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        //this.lbCarrier.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString(); ;
                        //this.lbContainerNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString(); ;
                        //this.lbControlNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString(); ;
                        //this.lbCustomerName.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString(); ;
                        //this.lbDate_Exec.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString(); ;
                        //this.lbDriverNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString(); ;
                        //this.lbForklifter.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString(); ;
                        //this.lbMachine.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString(); ;
                        //this.lbOrderNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString(); ;
                        //this.lbSealNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString(); ;
                        //this.lbSourceVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString(); ;
                        ////this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
                        //this.lbVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    else
                    {
                        Rep_StockOutDetail_International_Film repd = new Rep_StockOutDetail_International_Film();
                        repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                        //repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                        repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                        repd.DataSource = wmsDS2.T_OutStockDetail_International_Rep;// this.grid_Detail_International.DataSource;
                        //repd.RequestParameters = false;

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainer"].Value = strContainer;

                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    #endregion
                }
                else if (this.cmb_PrintName.Text == "淋膜纸磅码单_万国")
                {
                    //CH 2017-04-23
                    if (this.cmb_TradeType.Text == "内贸")
                    {
                        Rep_StockOutDetail_Film_WG repd = new Rep_StockOutDetail_Film_WG();
                        //repd.SetData(wmsDS2.T_OutStockTitle_Domestic_Rep);
                        repd.Parameters["parCarrierNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.CarrierNOColumn].ToString();
                        //repd.Parameters["parContainerNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parControlNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.ControlNoColumn].ToString();
                        repd.Parameters["parCustomerName"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.CustomerNameColumn].ToString();
                        repd.Parameters["parDate_Exec"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.BillDateColumn].ToString();
                        repd.Parameters["parDriverNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VehicleNOColumn].ToString();
                        repd.Parameters["parForklifterNO"].Value = wmsDS2.T_OutStock.Rows[0][wmsDS2.T_OutStock.ForklifterNOColumn].ToString();
                        repd.Parameters["parMachineNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.MachineIDColumn].ToString();
                        repd.Parameters["parOrderNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.OrderNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SealNOColumn].ToString();
                        repd.Parameters["parSourceVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.SourceVoucherNOColumn].ToString();
                        repd.Parameters["parVoucherNO"].Value = wmsDS2.T_OutStockTitle_Domestic_Rep.Rows[0][wmsDS2.T_OutStockTitle_Domestic_Rep.VoucherNOColumn].ToString();
                        //this.lbCarrier.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString(); ;
                        //this.lbContainerNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString(); ;
                        //this.lbControlNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString(); ;
                        //this.lbCustomerName.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString(); ;
                        //this.lbDate_Exec.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString(); ;
                        //this.lbDriverNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString(); ;
                        //this.lbForklifter.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString(); ;
                        //this.lbMachine.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString(); ;
                        //this.lbOrderNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString(); ;
                        //this.lbSealNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString(); ;
                        //this.lbSourceVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString(); ;
                        ////this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
                        //this.lbVoucherNO.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainerNO"].Value = strContainer;

                        repd.DataSource = wmsDS2.T_OutStockDetail_Domestic_Rep;// this.grid_Detail.DataSource;
                                                                               //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                    else
                    {
                        Rep_StockOutDetail_International_Film repd = new Rep_StockOutDetail_International_Film();
                        repd.Parameters["parPickNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.PickNOColumn].ToString();
                        //repd.Parameters["parSealNO"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.SealNOColumn].ToString();
                        //repd.Parameters["parContainer"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ContainerNOColumn].ToString();
                        repd.Parameters["parShip"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.ShipNOColumn].ToString();
                        repd.Parameters["parDate"].Value = wmsDS2.T_OutStockTitle_International_Rep.Rows[0][wmsDS2.T_OutStockTitle_International_Rep.BillDateColumn].ToString();
                        repd.DataSource = wmsDS2.T_OutStockDetail_International_Rep;// this.grid_Detail_International.DataSource;
                                                                                    //repd.RequestParameters = false;

                        repd.Parameters["parSealNO"].Value = strSealNO;
                        repd.Parameters["parContainer"].Value = strContainer;

                        //repd.Print();
                        repd.ShowPreviewDialog();
                    }
                }


            }
        }

        private void txt_Box_TextChanged(object sender, EventArgs e)
        {
            if (this.txt_Box.Text.Trim() == "20")
            {
                this.txt_Bulk.Text = "23";
            }
            else if (this.txt_Box.Text.Trim() == "40")
            {
                this.txt_Bulk.Text = "33";
            }
        }

        private void cmb_BillType_TextChanged(object sender, EventArgs e)
        {
            if (this.cmb_BillType.Text == "R")
                this.cmb_BillType.ForeColor = Color.Red;
            else
                this.cmb_BillType.ForeColor = Color.Blue;
        }

        private void btnExportDetail_Click(object sender, EventArgs e)
        {
            if (grid_Detail.DataSource == null)
                return;
            if (grid_Detail.DefaultView.RowCount == 0)
                return;
          
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.DefaultExt = "xls";
            saveFileDialog.Filter = "Excel文件|*.xls";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                grid_Detail.ExportToXls(saveFileDialog.FileName);
            }
        }

        public void Export(DataTable datasource)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.DefaultExt = "xls";
            saveFileDialog.Filter = "Excel文件|*.xls";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GridControl grid = new GridControl(); ;
                GridView view = new GridView();
                grid.ViewCollection.Add(view);
                grid.MainView = view;
                view.GridControl = grid;

                foreach (DataColumn dc in datasource.Columns)
                {
                    DevExpress.XtraGrid.Columns.GridColumn gridColumnNumber = view.Columns.AddVisible(dc.ColumnName);
                    gridColumnNumber.FieldName = dc.ColumnName;
                }

                grid.DataSource = datasource;
                this.Controls.Add(grid);//重要  
                grid.ForceInitialize();//重要  
                view.BestFitColumns();
                view.ExportToXls(saveFileDialog.FileName);
            }
        }  

    }
}
