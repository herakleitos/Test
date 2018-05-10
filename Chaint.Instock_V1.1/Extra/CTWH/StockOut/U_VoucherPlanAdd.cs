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
using DevExpress.XtraEditors.Repository;


namespace CTWH.StockOut
{
    public partial class U_VoucherPlanAdd : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        MainLayout _MainLayout = null;
      public  string _VourcherNO = "";
      WMSDS _WMS_MAIN;

        public U_VoucherPlanAdd()
        {
            InitializeComponent();
            //this.Disposed += new EventHandler(U_VoucherPlanAdd_Disposed);
        }

        void U_VoucherPlanAdd_Disposed(object sender, EventArgs e)
        {
            this._MainLayout.OnMenuClickEvent -= new MenuClickEventHandle(OnMenuClickEventProcess);
        }

        public U_VoucherPlanAdd(MainLayout tl)
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
        public void SaveForm(){

            MessageBox.Show("save ok");
        }
   

        private void U_VoucherPlanAdd_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMS_MAIN = new WMSDS();
            //加载业务类型
            LoadBussinessType();
            //加载机台号
            LoadOrg();

            //加载仓库
            //LoadWarehouse();

            //加载客户
            LoadCustomer();
            //加载等级
            LoadGrade();
            //加载物料
            LoadMeterialCodeToGridItem();
            //加载规格
            LoadSpecific();
            //加载包装方式
            LoadPackType();
            //加载夹板方式
            this.LoadTrademarkstyle();
            //加载客户专用
            LoadSpecCust();
            //加载产品专用
            LoadSpecProd();
            //加载认证
            this.LoadPaperCert();

            //加载色相
            LoadColor();

            //加载特殊客户
            LoadTSKH();

            LoadDefaultValue();

            this.grid_Plans.DataSource = _WMS_MAIN.T_OutStock_Plan_Entry;

            if (this._VourcherNO != "")
            {
                LoadEditValue(this._VourcherNO);
            }
            else {
                //this.NewVoucherPlan();
            }
            date_OutTime.DateTime = DateTime.Now;
            //this.gridView1.BestFitColumns();
        }
        /// <summary>
        /// 通过单号查询出单据信息，并且加载到控件中
        /// </summary>
        /// <param name="voucherno"></param>
        private void LoadEditValue(string voucherno)
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_OutPlanAndEntry_RelationByVoucherNO(voucherno);
            if (wmsDS.T_OutStock_Plan.Rows.Count > 0)
            {
                this.cmb_VoucherNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.VoucherNOColumn].ToString();
                this.cmb_CustomerName.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.CustomerNameColumn].ToString();
                //this.cmb_Amount.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.QtyColumn].ToString();
                this.cmb_BillNo.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.PickNOColumn].ToString();
                this.cmb_BillType.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillTypeColumn].ToString();
                //this.cmb_Boat.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BoatNOColumn].ToString();
                //this.cmb_BoxNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BoxNOColumn].ToString();
                this.cmb_BusinessType.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BusinessTypeColumn].ToString();
                this.cmb_Contract.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.OrderNOColumn].ToString();
                this.cmb_Dept.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.DeptNameColumn].ToString();
                this.cmb_Emp.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillerColumn].ToString(); ;
                this.cmb_Factory.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.FactoryIDColumn].ToString();
                this.cmb_OrgTo.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.OrgToCodeColumn].ToString();

                //this.cmb_Lifter.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.ForklifterNOColumn].ToString(); ;
                //this.cmb_Loader.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.PickNOColumn].ToString();
                //this.cmb_Port.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.PortNOColumn].ToString();
                this.cmb_Remark.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillRemarkColumn].ToString();
                //this.cmb_SealNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.SealNOColumn].ToString();
                this.cmb_TradeType.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.TradeTypeColumn].ToString();
                this.cmb_TransportType.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.TransportTypeColumn].ToString();
                //this.cmb_VehicleNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.VehicleNOColumn].ToString();
                this.cmb_Warehouse.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.WHCodeColumn].ToString();
                this.cmb_WHto.EditValue = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.WHToCodeColumn].ToString();

                //this.cmb_Weight.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.AuxQtyColumn].ToString();
                this.date_OutTime.DateTime = Convert.ToDateTime(wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.BillDateColumn].ToString());

            }
            else {
                //this.cmb_VoucherNO.Text = wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.VoucherNOColumn].ToString();
                this.cmb_CustomerName.Text = null;
                //this.cmb_Amount.Text = "";
                this.cmb_BillNo.Text = "";
                this.cmb_BillType.Text = "";
                //this.cmb_Boat.Text = "";
                //this.cmb_BoxNO.Text = "";
                this.cmb_BusinessType.Text = "";
                this.cmb_Contract.Text = "";
                this.cmb_Dept.EditValue = null;
                this.cmb_Emp.EditValue = null;
                this.cmb_Factory.EditValue = null;
                //this.cmb_Lifter.Text = "";
                //this.cmb_Loader.Text = "";
                //this.cmb_Port.Text = "";
                this.cmb_Remark.Text = "";
                //this.cmb_SealNO.Text = "";
                this.cmb_TradeType.Text = "";
                this.cmb_TransportType.Text = "";
                //this.cmb_VehicleNO.Text = "";
                this.cmb_Warehouse.EditValue = null;
                //this.cmb_Weight.Text = "";
                this.date_OutTime.DateTime = DateTime.Now;
                this.LoadDefaultValue();

            }
            this._WMS_MAIN = wmsDS;
            this.grid_Plans.DataSource = _WMS_MAIN.T_OutStock_Plan_Entry;
            this.gridView1.BestFitColumns();
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
            for(int i=0;i<iDS.T_Grade.Rows.Count;i++)
                this.gridItem_Grade.Items.Add(iDS.T_Grade.Rows[i]["GradeName"].ToString()); 
          
        }
        /// <summary>
        /// 包装方式
        /// </summary>
        private void LoadPackType()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("BZFS");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridCmb_ReamPackType.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }
        private void LoadPaperCert()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("ZZRZ");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridCmb_PaperCert.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }

        //加载色相
        private void LoadColor()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("CPSX");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridCmb_Color.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }

        //特殊客户
        private void LoadTSKH()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("TSKH");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridCmb_Remark.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }

        /// <summary>
        /// 夹板方式
        /// </summary>
        private void LoadTrademarkstyle()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("JBBZ");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.gridCmb_TradeMarkStyle.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }
        private void LoadMeterialCodeToGridItem()
        {
            InterfaceDS wmsDS = this._WMSAccess.Select_CT_WLZD("");

            this.gridItem_Material.DataSource = wmsDS.CT_WLZD;
            this.gridItem_Material.ValueMember = "WLBH";
            this.gridItem_Material.DisplayMember = "WLBH";
        }
        private void LoadBussinessType() {
            WMSDS wmsDS = this._WMSAccess.Select_T_BusinessType("request","A");

            this.cmb_BusinessType.Properties.DataSource = wmsDS.T_Business_Type;
            this.cmb_BusinessType.Properties.ValueMember = "BusinessCode";
            this.cmb_BusinessType.Properties.DisplayMember = "BusinessName";
        }
        private void LoadDept(string org,string dept)
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_BMZD(org,dept);
            iDS.CT_BMZD.Rows.Add(iDS.CT_BMZD.NewCT_BMZDRow());
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
            iDS.CT_OrgInfo.Rows.Add(iDS.CT_OrgInfo.NewCT_OrgInfoRow());

            this.cmb_OrgTo.Properties.DataSource = iDS.CT_OrgInfo;
            this.cmb_OrgTo.Properties.ValueMember = "ORGCode";
            this.cmb_OrgTo.Properties.DisplayMember = "ORGName";
        }
        /// <summary>
        /// 根据组织找仓库
        /// </summary>
        private void LoadWarehouse(string org, DevExpress.XtraEditors.LookUpEdit com)
        {
            //LoadSourceWarehouse(cmb_Factory.EditValue == null ? "" : cmb_Factory.EditValue.ToString());
            //LoadDestWarehouse(cmb_OrgTo.EditValue == null ? "" : cmb_OrgTo.EditValue.ToString());
            InterfaceDS iDS = this._WMSAccess.Select_CT_CKZD(org);
            iDS.CT_CKZD.Rows.Add(iDS.CT_CKZD.NewCT_CKZDRow());

            com.Properties.DataSource = iDS.CT_CKZD;
            com.Properties.ValueMember = "CKBH";
            com.Properties.DisplayMember = "CKMC";
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

          private void cmb_Factory_EditValueChanged(object sender, EventArgs e)
          {
              //加载部门
              LoadDept(this.cmb_Factory.EditValue==null?"-1":this.cmb_Factory.EditValue.ToString(),"");
              string org = this.cmb_Factory.EditValue == null ? "" : this.cmb_Factory.EditValue.ToString();
              LoadWarehouse(org, this.cmb_Warehouse);
              
          }

          private void cmb_Warehouse_EditValueChanged(object sender, EventArgs e)
          {
              //加载制单人
              LoadEmp(this.cmb_Warehouse.EditValue == null ? "-1" : this.cmb_Warehouse.EditValue.ToString(), "");
          }

          public void OnMenuClickEventProcess(object sender, MenuClickEventArgs e)
          {

              switch (e.MenuName)
              {
                  case Utils.WMSMenu._New:

                      //新建一个发货通知单
                      if (DialogResult.Yes == MessageBox.Show("确定要新建发货通知单吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                      {
                          //this.NewVoucherPlan();

                      }
                      break;
                  case Utils.WMSMenu._Save:

                      if (this.gridView1.RowCount == 0)
                      {
                          MessageBox.Show("请添加发运通知单发货计划！");
                          return;
                      }

                      if (!this.CheckBillCanSave())
                      {
                          return;
                      }

                      this.NewVoucherPlan();
                      

                      this.gridView1.PostEditor();

                      //判断是新建还是更新
                      WMSDS wmsDS = this._WMSAccess.Select_T_OutStock_PlanByFK(this.cmb_VoucherNO.Text, "", "", "", "", "", "", 0, 0, 0);
                      if (wmsDS.T_OutStock_Plan.Rows.Count == 0)
                      {
                          if (DialogResult.Yes == MessageBox.Show(string.Format("确定要保存发货通知单【{0}】吗？", cmb_VoucherNO.Text), "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                          {
                              

                              this.SaveNewVoucherPlan();

                              string str = "";
                              for(int i = 0;i<this.gridView1.RowCount;i++)
                              {
                                  for (int j = 0; j < this.gridView1.Columns.Count; j++)
                                      str += this.gridView1.GetRowCellValue(i, this.gridView1.Columns[j]).ToString() + '|';
                              }
                              string Des = string.Format("制单人{0}新建了发货通知单{1},分录数目为：{2}", cmb_Emp.Text.ToString(), cmb_VoucherNO.Text.ToString(), this.gridView1.RowCount.ToString());
                              _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, str, "", cmb_VoucherNO.Text.ToString());
                              str = "";
                              cmb_VoucherNO.Text = "";
                              //grid_Plans.DataSource = null;
                              _WMS_MAIN.T_OutStock_Plan_Entry.Clear();
                          }
                          else {
                              cmb_VoucherNO.Text = ""; 
                              return;
                          }
                      }
                      else
                      {
                          string date = wmsDS.T_OutStock_Plan.Rows[0]["Biller"].ToString();
                          string user = wmsDS.T_OutStock_Plan.Rows[0]["BillDate"].ToString();
                          string s1s = this._WMS_MAIN.T_OutStock_Plan_Entry.Rows[0]["PlanQty"].ToString();
                          string ss = this._WMS_MAIN.T_OutStock_Plan_Entry.Rows[0]["PlanAuxQty1"].ToString();

                          if (DialogResult.Yes == MessageBox.Show(this.cmb_VoucherNO.Text + "已于" + date + "由" + user + "创建，要覆盖吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                          {
                              this.SaveUpdateVoucherPlan(wmsDS.T_OutStock_Plan.Rows[0][wmsDS.T_OutStock_Plan.OnlyIDColumn].ToString());

                              string str = "";
                              for (int i = 0; i < this.gridView1.RowCount; i++)
                              {
                                  for (int j = 0; j < this.gridView1.Columns.Count; j++)
                                      str += this.gridView1.GetRowCellValue(i, this.gridView1.Columns[j]).ToString() + '|';
                              }
                              string Des = string.Format("制单人{0}覆盖了发货通知单{1},分录数目为：{2}", cmb_Emp.Text.ToString(), cmb_VoucherNO.Text.ToString(), this.gridView1.RowCount.ToString());
                              _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, str, "", cmb_VoucherNO.Text.ToString());
                              str = "";
                          } cmb_VoucherNO.Text = "";
                      }
                         
                      break;
                  case Utils.WMSMenu._Delete:
                      MessageBox.Show("_Delete");
                      break;
                  case Utils.WMSMenu._NewLine:

                      this.AddNewLineToPlan();
                      
                     // _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, string.Format("{0}新增了一行发货通知单{1}", Utils.LoginUserName, ""), "", "", "");
                      break;
                  case Utils.WMSMenu._DelLine:
                      
                      this.DeleteSelectedLine();


                      
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
        /// <summary>
        /// 保存新做的发货通知单，不包括更新的方法
        /// </summary>
          private void SaveNewVoucherPlan()
          {
              //先看单号是否存在
              WMSDS.T_OutStock_PlanRow tospRow = new WMSDS().T_OutStock_Plan.NewT_OutStock_PlanRow();
              tospRow.BillDate = this.date_OutTime.DateTime;
              tospRow.BillDept = "";// this.cmb_Dept.EditValue.ToString();
              tospRow.Biller = this.cmb_Emp.EditValue==null?"":this.cmb_Emp.EditValue.ToString();
              tospRow.BillRemark = this.cmb_Remark.Text;
              tospRow.BillType = this.cmb_BillType.Text;
              tospRow.BusinessType = this.cmb_BusinessType.EditValue.ToString();
              tospRow.CustomerName = this.cmb_CustomerName.Text;
              tospRow.DeptName =  this.cmb_Dept.EditValue==null?"" :this.cmb_Dept.EditValue.ToString();
              tospRow.FactoryID = this.cmb_Factory.EditValue == null ? "" : this.cmb_Factory.EditValue.ToString();
              tospRow.OrgToCode = this.cmb_OrgTo.EditValue == null ? "" : this.cmb_OrgTo.EditValue.ToString();
              tospRow.TradeType = this.cmb_TradeType.Text;
              tospRow.TransportType = this.cmb_TransportType.Text;
              //tospRow.VehicleNO = this.cmb_VehicleNO.Text;
              tospRow.VoucherNO = this.cmb_VoucherNO.Text;
              tospRow.WHCode = this.cmb_Warehouse.EditValue == null ? "" : this.cmb_Warehouse.EditValue.ToString();
              tospRow.WHToCode = this.cmb_WHto.EditValue == null ? "" : this.cmb_WHto.EditValue.ToString();

              //tospRow.Qty = Convert.ToInt32(this.cmb_Amount.Text==""?"0":this.cmb_Amount.Text);
              //tospRow.AuxQty = Convert.ToDecimal(this.cmb_Weight.Text == "" ? "0" : this.cmb_Weight.Text);
              //tospRow.BoatNO = this.cmb_Boat.Text;
              //tospRow.BoxNO = this.cmb_BoxNO.Text;
              //tospRow.CarrierNO = this.cmb_Loader.Text;
              //tospRow.ForklifterNO = this.cmb_Lifter.Text;
              tospRow.OrderNO = this.cmb_Contract.Text;
              tospRow.PickNO = this.cmb_BillNo.Text;
              //tospRow.PortNO = this.cmb_Port.Text;
              //tospRow.SealNO = tospRow.BoxNO;// this.cmb_SealNO.Text;
              tospRow.IsCancel = "0";
              tospRow.IsCheck = "0";
              tospRow.IsClose = "0";

                      bool ok = this.CheckBillCanSave();
                      if (ok)
                      {

                        string ss=  this._WMSAccess.Tran_SaveNewOutStockPlan(tospRow, this._WMS_MAIN.T_OutStock_Plan_Entry.Rows);
                        if (ss == "")
                            MessageBox.Show("保存成功");
                        else {
                            MessageBox.Show("保存失败：" + ss);
                        }
                          ////插入抬头
                          //int result = this._WMSAccess.InsertT_OutStock_PlanByRow(tospRow);
                          ////插入分录
                          //for (int i = 0; i < this._WMS_MAIN.T_OutStock_Plan_Entry.Rows.Count; i++)
                          //{
                          //    WMSDS.T_OutStock_Plan_EntryRow ospeRow = this._WMS_MAIN.T_OutStock_Plan_Entry.Rows[i] as WMSDS.T_OutStock_Plan_EntryRow;
                          //    ospeRow.VoucherID = result;
                          //    int entryresult = this._WMSAccess.InsertT_OutStock_Plan_EntryByRow(ospeRow);
                          //}
                      }

                      
          }
          /// <summary>
          /// 保存新做的发货通知单，不包括更新的方法
          /// </summary>
          private void SaveUpdateVoucherPlan(string OnlyID)
          {
              //先看单号是否存在
              WMSDS.T_OutStock_PlanRow tospRow = new WMSDS().T_OutStock_Plan.NewT_OutStock_PlanRow();
              tospRow.OnlyID =Convert.ToInt32( OnlyID);
              tospRow.BillDate = this.date_OutTime.DateTime;
              tospRow.BillDept = this.cmb_Dept.EditValue == null ? "" : this.cmb_Dept.EditValue.ToString();//this.cmb_Dept.EditValue.ToString();
              tospRow.Biller = this.cmb_Emp.EditValue.ToString();
              tospRow.BillRemark = this.cmb_Remark.Text;
              tospRow.BillType = this.cmb_BillType.Text;
              tospRow.BusinessType = this.cmb_BusinessType.EditValue.ToString();
              tospRow.CustomerName = this.cmb_CustomerName.Text;
              tospRow.DeptName = this.cmb_Dept.EditValue == null ? "" : this.cmb_Dept.EditValue.ToString();
              tospRow.FactoryID = this.cmb_Factory.EditValue.ToString();
              tospRow.OrgToCode =this.cmb_OrgTo.EditValue==null?"": this.cmb_OrgTo.EditValue.ToString();
              tospRow.TradeType = this.cmb_TradeType.Text;
              tospRow.TransportType = this.cmb_TransportType.Text;
              //tospRow.VehicleNO = this.cmb_VehicleNO.Text;
              tospRow.VoucherNO = this.cmb_VoucherNO.Text;
              tospRow.WHCode = this.cmb_Warehouse.EditValue.ToString();
              tospRow.WHToCode =this.cmb_WHto.EditValue==null?"": this.cmb_WHto.EditValue.ToString();
              //tospRow.Qty = Convert.ToDecimal(this.cmb_Amount.Text);
              //tospRow.AuxQty = Convert.ToDecimal(this.cmb_Weight.Text);
              //tospRow.BoatNO = this.cmb_Boat.Text;
              //tospRow.BoxNO = this.cmb_BoxNO.Text;
              //tospRow.CarrierNO = this.cmb_Loader.Text;
              //tospRow.ForklifterNO = this.cmb_Lifter.Text;
              tospRow.OrderNO = this.cmb_Contract.Text;
              tospRow.PickNO = this.cmb_BillNo.Text;
              //tospRow.PortNO = this.cmb_Port.Text;
              //tospRow.SealNO = tospRow.BoxNO;// this.cmb_SealNO.Text;
              
              //更新单据表头
              //插入抬头
           string ss=   this._WMS_MAIN.T_OutStock_Plan_Entry.Rows[0]["PlanAuxQty1"].ToString();
              string result = this._WMSAccess.Tran_SaveUpdateOutStockPlan(tospRow, this._WMS_MAIN.T_OutStock_Plan_Entry.Rows);

              if (result == "")
              {
                  MessageBox.Show("保存成功");
              }
              else {
                  MessageBox.Show("保存失败："+result);
              }


          }
          private void AddNewLineToPlan()
          {
              
              WMSDS.T_OutStock_Plan_EntryRow osprow = _WMS_MAIN.T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
              osprow.EntryID = this.gridView1.DataRowCount+1;
              osprow.VoucherID = 0;//this.cmb_VoucherNO.e.Trim();
              osprow.Width_P = 0;
              osprow.Width_R = 0;
              osprow.Length_P = 0;
              osprow.Udefine1 = 0;
              osprow.Udefine2 = 0;
              osprow.PlanQty = 0;
              osprow.PlanAuxQty1 = 0;
              osprow.Reams=0;
              osprow.SlidesOfReam = 0;
              osprow.SlidesOfSheet = 0;
              _WMS_MAIN.T_OutStock_Plan_Entry.Rows.Add(osprow);
              for (int i = 0; i <   _WMS_MAIN.T_OutStock_Plan_Entry.Rows.Count; i++) {
                  _WMS_MAIN.T_OutStock_Plan_Entry.Rows[i]["EntryID"] = i + 1;
              }

              
                  string Des = string.Format("{0}新增了发货通知单一行分录", Utils.LoginUserName);
                  _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", "");
              

          }
          private void DeleteSelectedLine() {
              //string entryId = this.gridView1.GetFocusedRowCellDisplayText(gridColumn1).ToString();

              if (this.gridView1.RowCount == 0)
              {
                  MessageBox.Show("没有可删除的行！");
                  return;
              }

              int sourcehandle = this.gridView1.GetDataSourceRowIndex(this.gridView1.FocusedRowHandle);
              this._WMS_MAIN.T_OutStock_Plan_Entry.Rows.RemoveAt(sourcehandle);
              for (int i = 0; i < _WMS_MAIN.T_OutStock_Plan_Entry.Rows.Count; i++)
              {
                  _WMS_MAIN.T_OutStock_Plan_Entry.Rows[i]["EntryID"] = i + 1;
              }


              string Des = string.Format("{0}删除了发货通知单第{1}行分录", Utils.LoginUserName, ++sourcehandle);
                  _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", "");
              
             
          }
        /// <summary>
        /// 检查数据，bill能否被保存
        /// </summary>
        /// <returns></returns>
          private bool CheckBillCanSave()
          {
              string voucherno = this.cmb_VoucherNO.Text;
              string org = this.cmb_Factory.EditValue == null ? "" : this.cmb_Factory.EditValue.ToString();
              string business = this.cmb_BusinessType.EditValue.ToString();
              string dept = "";// this.cmb_Dept.EditValue.ToString();
              string wh = this.cmb_Warehouse.EditValue == null ? "" : this.cmb_Warehouse.EditValue.ToString();
              string dateout = this.date_OutTime.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
              string cust = this.cmb_CustomerName.Text;
              string biller = this.cmb_Emp.EditValue == null ? "" : this.cmb_Emp.EditValue.ToString();
              string trade = this.cmb_TradeType.Text;
              string transpot = this.cmb_TransportType.Text;

              string  isOk = "";
              //if (voucherno == "")
              //{
              //    if (MessageBox.Show("要自动生成单号吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              //    {
              //        this.NewVoucherPlan();
              //        this.CheckBillCanSave();
              //        return true;
              //    }
              //    else
              //    {
              //        return false;
              //    }
              //}
              if (business == "")
              {
                  MessageBox.Show("请选择业务类型。");
                  return false;
              }
              if (org == "")
              {
                  MessageBox.Show("请选择组织。");
                  return false;
              }
              //if (dept == "")
              //{
              //    MessageBox.Show("请选择部门。");
              //    return false;
              //}
              if (wh == "")
              {
                  MessageBox.Show("请选择仓库。");
                  return false;
              }
              if (dateout == "")
              {
                  MessageBox.Show("请选择日期。");
                  return false;
              }
              //if (cust == "")
              //{
              //    MessageBox.Show("请选择客户。");
              //    return false;
              //}
              if (biller == "")
              {
                  MessageBox.Show("请选择制单人。");
                  return false;
              }
              if (trade == "")
              {
                  MessageBox.Show("请选择贸易方式。");
                  return false;
              }
              if (transpot == "")
              {
                  MessageBox.Show("请选择运输方式。");
                  return false;
              }
              
              return true;
          }
        /// <summary>
        /// 新建一个发货通知单
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
                  string type = Utils.WMSVoucherType._BillPlan;
                  string machineid = orgDS.T_Factory.Rows[0][orgDS.T_Factory.MachineIDColumn].ToString();// Utils.LoginMachineID;
                  string planNO = this._WMSAccess.Get_T_OutStock_Plan_NewFlow(type, machineid);
                  this.cmb_VoucherNO.Text = planNO;
                  this.date_OutTime.DateTime = DateTime.Now;
              }
              else
              {
                  MessageBox.Show("请先配置本地库存组织");
              }
          }

          private void cmb_VoucherNO_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
          {
            //  VoucherPlanShow vps = new VoucherPlanShow();
            ////MainDS ds = access.Warehouse_VoucherQueryByDate_Invo(System.DateTime.Now.AddDays(-100).ToString(), System.DateTime.Now.AddDays(100).ToString());

            ////MainDS ds = access.Warehouse_VoucherQueryByFK("", System.DateTime.Now.AddDays(-50).ToString(), "", "", "", "", "", -1, 0, 0, -1);

            ////vps.InitData(ds.Warehouse_Voucher);
            //  if (vps.ShowDialog() == DialogResult.OK)
            //  {
            //      // string VoucherNO = vps.GetSelectVoucherNO();
            //      WMSDS.T_OutStock_PlanRow wvrow = vps.GetWarehouse_VoucherRow();
            //      if (wvrow != null)
            //      {
            //          //   this.voucherHead1.SetVoucherRow(wvrow);

            //          this.cmb_VoucherNO.Text = wvrow.VoucherNO;

            //          if (wvrow.IsTradeTypeNull())
            //          { this.cmb_TradeType.Text = ""; }
            //          else
            //          { this.cmb_TradeType.Text = wvrow.TradeType; }

            //          if (wvrow.IsCustomerNameNull())
            //          {
            //              this.cmb_CustomerName.Text = "";
            //          }
            //          else { this.cmb_CustomerName.Text = wvrow.CustomerName; }

            //          if (wvrow.IsFactoryIDNull())
            //          {
            //              this.cmb_Factory.Text = "";
            //          }
            //          else { this.cmb_Factory.Text = wvrow.FactoryID; }

            //          //if (wvrow.IsVehicleNONull())
            //          //{
            //          //    this.cmb_VehicleNO.Text = "";
            //          //}
            //          //else { this.cmb_VehicleNO.Text = wvrow.VehicleNO; }

            //          //if (wvrow.IsBoxNONull())
            //          //{
            //          //    this.cmb_BoxNO.Text = "";
            //          //}
            //          //else { this.cmb_BoxNO.Text = wvrow.BoxNO; }

            //          //if (wvrow.IsSealNONull())
            //          //{
            //          //    this.cmb_SealNO.Text = "";
            //          //}
            //          //else { this.cmb_SealNO.Text = wvrow.SealNO; }

            //          //if (wvrow.IsBoatNONull())
            //          //{
            //          //    this.cmb_Boat.Text = "";
            //          //}
            //          //else { this.cmb_Boat.Text = wvrow.BoatNO; }

            //          //if (wvrow.IsPortNONull())
            //          //{
            //          //    this.cmb_Port.Text = "";
            //          //}
            //          //else { this.cmb_Port.Text = wvrow.PortNO; }

            //          //if (wvrow.IsForklifterNONull())
            //          //{
            //          //    this.cmb_Lifter.Text = "";
            //          //}
            //          //else { this.cmb_Lifter.Text = wvrow.ForklifterNO; }

            //          ////装卸工
            //          //if (wvrow.IsCarrierNONull())
            //          //{
            //          //    this.cmb_Loader.Text = "";
            //          //}
            //          //else { this.cmb_Loader.Text = wvrow.CarrierNO; }

            //          //提单号
            //          if (wvrow.IsPickNONull())
            //          {
            //              this.cmb_BillNo.Text = "";
            //          }
            //          else { this.cmb_BillNo.Text = wvrow.PickNO; }


            //          if (wvrow.IsOrderNONull())
            //          {
            //              this.cmb_Contract.Text = "";
            //          }
            //          else { this.cmb_Contract.Text = wvrow.OrderNO; }


            //          //if (wvrow.IsQtyNull())
            //          //{
            //          //    this.cmb_Amount.Text = "0";
            //          //}
            //          //else
            //          //{
            //          //    this.cmb_Amount.Text = wvrow.Qty.ToString();
            //          //}

            //          //if (wvrow.IsAuxQtyNull())
            //          //{
            //          //    this.cmb_Weight.Text = "0";
            //          //}
            //          //else { this.cmb_Weight.Text = this.TrimEndZero(wvrow.AuxQty.ToString()); }


            //      }

            //      RefreshDataByVoucherNO(wvrow.VoucherNO);

            //  }
          }
        /// <summary>
        /// 刷新发货单的分类
        /// </summary>
        /// <param name="VoucherNO"></param>
          private void RefreshDataByVoucherNO(string VoucherNO)
          {

              this.grid_Plans.DataSource = null;
              WMSDS ds = this._WMSAccess.Select_T_OutStock_Plan_Entry(VoucherNO, 0);
              if (ds.T_OutStock_Plan.Rows.Count > 0)
              {

                  this.grid_Plans.DataSource = ds.T_OutStock_Plan;
              }

          }
          private string TrimEndZero(string source)
          {
              if (source.Contains("."))
                  source = source.TrimEnd('0').TrimEnd('.');
              return source;
          }

          //private void simpleButton1_Click(object sender, EventArgs e)
          //{
          //    WMSDS.T_OutStock_Plan_EntryRow osprow = WMS_MAIN.T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
          //    osprow.EntryID = 4;
          //    osprow.VoucherID = 2;
          //    osprow.Grade = "一等品";
          //    osprow.Width_R = 787;
          //    osprow.WeightMode = "0";
          //    osprow.PlanQty = 10;
          //    osprow.PlanAuxQty1 = 21;
          //    osprow.CoreDiameter = 3;
          //    osprow.IsWhiteFlag = "0";
          //    osprow.PaperCert = "FSC";
          //    osprow.OrderNO = "CW12399332";
          //    osprow.Remark = "备注12345";

          //    osprow.MaterialCode = "12352342";

          //    WMS_MAIN.T_OutStock_Plan_Entry.Rows.Add(osprow);

          //    this.gridView1.BestFitColumns();
          //}

          private void gridItem_Material_EditValueChanged(object sender, EventArgs e)
          {
              DevExpress.XtraEditors.BaseEdit edit = gridView1.ActiveEditor;

              //string ss = gridItem_Material.GetKeyValueByDisplayText(edit.EditValue.ToString()).ToString();
              //string sss = gridItem_Material.GetDisplayValueByKeyValue(edit.EditValue.ToString()).ToString();
              string ss = edit.EditValue.ToString();
              object obj = gridItem_Material.GetDataSourceRowByKeyValue(ss);
             DataRowView wr = obj as DataRowView;
             string wlmc = wr.Row["WLMC"].ToString();
             string sku = wr.Row["Cdefine1"].ToString();

              //DataRow sss = gridItem_Material.GetDataSourceRowByKeyValue(ss) as DataRow;//.GetDisplayValueByKeyValue(ss).ToString();

              _WMS_MAIN.T_OutStock_Plan_Entry.Rows[gridView1.GetFocusedDataSourceRowIndex()]["MaterialName"] = wlmc;
              //_WMS_MAIN.T_OutStock_Plan_Entry.Rows[gridView1.GetFocusedDataSourceRowIndex()]["SKU"] = sku;

              //gridView1.SetFocusedRowCellValue("MaterialName", ss);
              //经测试setfocusrowcellvalue以后tb中也自动有值了。但是直接在这个函数里面bestfit的时候显示的东西会清掉？？？？
              //经测试直接给TB赋值，view中也自动显示出来。但是直接在这个函数里面bestfit的时候显示的东西会清掉？？？？
              //只好放在view的cellvaluechange里面bestfit
              //string sss = WMS_MAIN.T_OutStock_Plan_Entry.Rows[gridView1.GetFocusedDataSourceRowIndex()]["MaterialName"].ToString();
          }

          private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
          {
              this.gridView1.BestFitColumns();

          }
        /// <summary>
        /// 刷新总数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          private void gridView1_RowCountChanged(object sender, EventArgs e)
          {
              //decimal allc = 0;
              //decimal allw = 0;
              //if(this._WMS_MAIN.T_OutStock_Plan_Entry.Rows.Count>0&& this.gridView1.RowCount>0)
              //for (int i = 0; i < this._WMS_MAIN.T_OutStock_Plan_Entry.Rows.Count; i++) {
              //string count=   this. _WMS_MAIN.T_OutStock_Plan_Entry.Rows[i]["PlanQty"] .ToString();
              //string weight = this._WMS_MAIN.T_OutStock_Plan_Entry.Rows[i]["PlanAuxQty1"] .ToString();
              //    decimal c = Convert.ToDecimal(count==""?"0":count);
              //    decimal w = Convert.ToDecimal(weight == "" ? "0" : weight);
              //    allc += c;
              //    allw += w;
              //}

              //this.cmb_Amount.Text = allc.ToString();
              //this.cmb_Weight.Text = allw.ToString();
              //this.gridView1.BestFitColumns();
          }

          private void cmb_TradeType_SelectedIndexChanged(object sender, EventArgs e)
          {
              if (cmb_TradeType.Text == "内贸")
              {
                  this.cmb_CustomerName.Properties.ReadOnly = false;
                  //this.cmb_CustomerName.Text = "";

                  //this.cmb_BillNo.Properties.ReadOnly = true;
                  //this.cmb_BillNo.Text = "";
                  //this.cmb_Boat.Properties.ReadOnly = true;
                  //this.cmb_Boat.Text = "";
                  //this.cmb_Contract.Properties.ReadOnly = true;
                  //this.cmb_Contract.Text = "";
              }

              else if (cmb_TradeType.Text == "出口")
              {
                  this.cmb_CustomerName.Properties.ReadOnly = true;
                  //this.cmb_CustomerName.Text = "";
                  //this.cmb_BillNo.Properties.ReadOnly = false;
                  //this.cmb_Boat.Properties.ReadOnly = false;
                  //this.cmb_Contract.Properties.ReadOnly = false;
              }
          }

          private void cmb_BillType_TextChanged(object sender, EventArgs e)
          {
              if (this.cmb_BillType.Text == "R")
                  this.cmb_BillType.ForeColor = Color.Red;
              else
                  this.cmb_BillType.ForeColor = Color.Blue;
          }

          private void cmb_BusinessType_EditValueChanged(object sender, EventArgs e)
          {
              if (this.cmb_BusinessType.EditValue.ToString() == "YKTZ")
              {
                  this.cmb_OrgTo.Enabled = true;
                  this.cmb_WHto.Enabled = true;
              }
              else
              {
                  this.cmb_OrgTo.Enabled = false;
                  this.cmb_WHto.Enabled = false;
                  this.cmb_OrgTo.EditValue = null;
                  this.cmb_WHto.EditValue = null;
              }
          }

          private void cmb_OrgTo_EditValueChanged(object sender, EventArgs e)
          {
              string org = this.cmb_OrgTo.EditValue == null ? "" : this.cmb_OrgTo.EditValue.ToString();
              LoadWarehouse(org, this.cmb_WHto);
          }
    }
}
