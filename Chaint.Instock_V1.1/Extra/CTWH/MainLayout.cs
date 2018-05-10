using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.StockOut;
using CTWH.StockIn;
using CTWH.Common;
using CTWH.StockManage;
using System.Diagnostics;

namespace CTWH
{
    public partial class MainLayout : DevExpress.XtraEditors.XtraForm
    {

        public event MenuClickEventHandle OnMenuClickEvent;

        /// <summary>
        /// 主panel里面现有的控件名字
        /// </summary>
        string _PanelCtrlName = "";
        public MainLayout()
        {
            InitializeComponent();
        }
        internal void OpenOutStockAdd(string voucherno,string sourceno)
        {
            if (_PanelCtrlName == "添加销售出库单")
                return;
            U_StockOutAdd uvpa = new U_StockOutAdd(this);
            uvpa._VourcherNO = voucherno;
            uvpa._SourceNO = sourceno;

            //  //uc = new U_VoucherPlanAdd();


            //AddUserControl(uvpa, "U_VoucherPlanAdd");

            //uc = new U_VoucherPlanAdd();


            AddUserControl(uvpa, "添加销售出库单");

            this.DisplayMenus(true, true, false, false, false, false, true, false, false, true, true,false,false);
        }
        public void OpenVoucherPlanAdd(string VourcherNO)
        {
            string name = "添加发货通知单";
            if (VourcherNO != "")
                name = "修改发货通知单";
            if (_PanelCtrlName == name)
                return;
            U_VoucherPlanAdd uvpa = new U_VoucherPlanAdd(this);
            uvpa._VourcherNO = VourcherNO;
            //  //uc = new U_VoucherPlanAdd();


            //AddUserControl(uvpa, "U_VoucherPlanAdd");

            //uc = new U_VoucherPlanAdd();


            AddUserControl(uvpa, name);

            this.DisplayMenus(false, true, false, false, false, false, true, false, false, true, true, false, false);
        }
        public void OpenOutStockAdd(string VourcherNO)
        {
            string name = "添加销售出库单";
            if (VourcherNO != "")
                name = "修改销售出库单";
            if (_PanelCtrlName == name)
                return;
            U_StockOutAdd uvpa = new U_StockOutAdd(this);
            uvpa._VourcherNO = VourcherNO;
            //  //uc = new U_VoucherPlanAdd();


            //AddUserControl(uvpa, "U_VoucherPlanAdd");

            //uc = new U_VoucherPlanAdd();


            AddUserControl(uvpa, name);

            this.DisplayMenus(false, true, false, false, false, false, true, false, false, true, true, false, false);
        }

        internal void OpenOutStockPrint(string ss)
        {
            if (_PanelCtrlName == "打印出库码单")
                return;
            U_StockOutPrint uvpa = new U_StockOutPrint(this);
            uvpa._VourcherNO = ss;
            //  //uc = new U_VoucherPlanAdd();


            //AddUserControl(uvpa, "U_VoucherPlanAdd");

            //uc = new U_VoucherPlanAdd();


            AddUserControl(uvpa, "打印出库码单");

            this.DisplayMenus(false, false, false, false, false, false, true, false, false, false, false, false, false);
        }


        //internal void OpenOutStockPrint( string voucherno)
        //{
        //    if (_PanelCtrlName == "查看销售出库单")
        //        return;
        //    U_StockOutPrint uvpa = new U_StockOutPrint(this);
        //    uvpa._VourcherNO = voucherno;
        //    //  //uc = new U_VoucherPlanAdd();


        //    //AddUserControl(uvpa, "U_VoucherPlanAdd");

        //    //uc = new U_VoucherPlanAdd();


        //    AddUserControl(uvpa, "查看销售出库单");

        //    this.DisplayMenus(true, true, false, false, false, false, true, false, false, false, false);
        //}
        public void OpenInStockAdd(string VourcherNO, string voucherid,string openType)
        {

            if (openType == "edit"&&_PanelCtrlName == "修改入库单")
                return;
            if (openType == "view" && _PanelCtrlName == "查看入库单")
                return;
            if (openType == "add" && _PanelCtrlName == "新建入库单")
                return;
            U_StockInAdd uvpa = new U_StockInAdd(this);
            uvpa._VourcherNO = VourcherNO;
            uvpa._VourcherID = voucherid;
            uvpa._OpenType = openType;
            //  //uc = new U_VoucherPlanAdd();


            //AddUserControl(uvpa, "U_VoucherPlanAdd");

            //uc = new U_VoucherPlanAdd();

            string type ="";
            if (openType == "edit" )
                type = "修改入库单";
            if (openType == "view" )
                type = "查看入库单";
            if (openType == "add")
                type = "新建入库单";
            AddUserControl(uvpa,type );

            this.DisplayMenus(false, false, false, false, false, false, true, false, false, false, false, false, false);
        }
        public void OpenOutStockAdd()
        {
            if (_PanelCtrlName == "新建销售出库单")
                return;
            U_StockOutAdd usod = new U_StockOutAdd(this);
            AddUserControl(usod, "新建销售出库单");
            this.DisplayMenus(true, true, false, false, false, false, true, false, false, true, true, false, false);
        }

        private void Bar_VoucherPlan_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "发货通知单列表")
                return;
            
            U_VoucherPlanList uvpa = new U_VoucherPlanList(this,"");
             
            AddUserControl(uvpa, "发货通知单列表");
            this.DisplayMenus(true, false, true, true, false, true, false, true, true, false, false, false, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ctrlName"></param>
        public void AddUserControl(UserControl ctrl, string ctrlName)
        {
            if (_PanelCtrlName == "")//如果还没有添加控件就直接加进来
            {
                ctrl.Dock = DockStyle.Fill;
                ctrl.Parent = this.Panel_Main;
                _PanelCtrlName = ctrlName;
                this.bar_CtrlName.Caption = ctrlName;

            }
            else   //如果已添加控件就判断是不是相同的控件
            {
                if (_PanelCtrlName == ctrlName)
                {
                    //相同的控件就 do nothing
                    
                }
                else  //不同的控件就清除原来的添加新的
                {
                    foreach (Control ctr in this.Panel_Main.Controls)
                    {
                        ctr.Dispose();
                    }
                    this.Panel_Main.Controls.Clear();
                    if (ctrl != null)
                    {
                        ctrl.Dock = DockStyle.Fill;
                        ctrl.Parent = this.Panel_Main;
                    }
                    _PanelCtrlName = ctrlName;
                    this.bar_CtrlName.Caption = ctrlName;
                  
                }
            }
        }
   
        private void bar_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
       
            ////触发一个事件通知control
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Save;
                OnMenuClickEvent(null, args);
            }
         
        }

        private void bar_StockInList_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "入库单列表")
                return;
            U_StockInList usil = new U_StockInList(this);

            AddUserControl(usil, "入库单列表");
            this.DisplayMenus(true, false, true, true, false, false, false, true, true, false, false, true, true);


        }
        private void bar_StockInConfirm_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "入库确认")
                return;
            StockIn.U_StockInConfirm sis = new StockIn.U_StockInConfirm(this);
            AddUserControl(sis, "入库确认");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }
        private void bar_StockOutDetail_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "出库明细")
                return;
            U_StockOutDetail usod = new U_StockOutDetail();

            AddUserControl(usod, "出库明细");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void Bar_VoucherList_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "出库单列表")
                return;
            U_StockOutList usol = new U_StockOutList(this,"");

            AddUserControl(usol, "出库单列表");
            this.DisplayMenus(true, false, true, true, true, false, false, false, false, false, false, true, true);

        }

        private void bar_Exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (DialogResult.Yes == MessageBox.Show("确定要退出系统吗？", "提示信息", MessageBoxButtons.YesNo,MessageBoxIcon.Question))
            //this.Dispose();
            
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Exit;
                OnMenuClickEvent(null, args);
            }
        }
        /// <summary>
        /// 根据打开的页面显示菜单的按钮
        /// </summary>
        /// <param name="bnew">new</param>
        /// <param name="bsave">save</param>
        /// <param name="bedit">edit</param>
        /// <param name="bdelete">delete</param>
        /// <param name="bpre">pre</param>
        /// <param name="bnext">next</param>
        /// <param name="bfresh">fresh</param>
        /// <param name="bcheck">check</param>
        /// <param name="buncheck">uncheck</param>   
        /// <param name="bnewline">newline</param>
        /// <param name="bdelline">delline</param>
        /// <param name="bview">view</param>
        /// <param name="block">lock</param>
        private void DisplayMenus(bool bnew, bool bsave, bool bedit, bool bdelete, bool bpre, bool bnext, bool bfresh, bool bcheck, bool buncheck, bool bnewline, bool bdelline, bool bview,bool block)
        {
            if (bnew == false)
                this.bar_New.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_New.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bsave == false)
                this.bar_Save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Save.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bedit == false)
                this.bar_Edit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Edit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bdelete == false)
                this.bar_Delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bpre == false)
                this.bar_Previous.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Previous.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bnext == false)
                this.bar_Next.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Next.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bfresh == false)
                this.bar_Fresh.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Fresh.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bcheck == false)
                this.bar_Check.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Check.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (buncheck == false)
                this.bar_UnCheck.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_UnCheck.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bnewline == false)
                this.bar_NewLine.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_NewLine.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bdelline == false)
                this.bar_DelLine.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_DelLine.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (bview == false)
                this.bar_View.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_View.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (block == false)
                this.bar_Lock.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else
                this.bar_Lock.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

        }
        /// <summary>
        /// 根据登陆的权限显示左侧菜单组
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <param name="h"></param>
        /// <param name="i"></param>
        private void DisplayGroups(bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h, bool i)
        {

            this.bar_New.Enabled = a;
            this.bar_Save.Enabled = b;

            this.bar_Edit.Enabled = c;
            this.bar_Delete.Enabled = d;
            this.bar_Previous.Enabled = e;
            this.bar_Next.Enabled = f;
            this.bar_Fresh.Enabled = g;
            this.bar_Check.Enabled = h;
            this.bar_UnCheck.Enabled = i;


        }
        /// <summary>
        /// 根据登陆的权限显示左侧菜单明细
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <param name="h"></param>
        /// <param name="i"></param>
        private void DisplayGroupItems(bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h, bool i)
        {

            //this.bar_New.Enabled = a;
            //this.bar_Save.Enabled = b;

            //this.bar_Edit.Enabled = c;
            //this.bar_Delete.Enabled = d;
            //this.bar_Previous.Enabled = e;
            //this.bar_Next.Enabled = f;
            //this.bar_Fresh.Enabled = g;
            //this.bar_Check.Enabled = h;
            //this.bar_UnCheck.Enabled = i;


        }

        private void bar_StockInDetail_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "入库明细")
                return;
            StockIn.U_StockInDetail sid = new StockIn.U_StockInDetail();

            AddUserControl(sid, "入库明细");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);

        }

        private void bar_StockInStat_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "入库统计")
                return;
            StockIn.U_StockInStat sis = new StockIn.U_StockInStat();

            AddUserControl(sis, "入库统计");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);

        }

        
        private void bar_StockInScan_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //if (_PanelCtrlName == "扫描入库")
            //    return;
            //StockIn.U_StockInScanRoll sis = new StockIn.U_StockInScanRoll(this);

            //AddUserControl(sis, "扫描入库");
            //this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false);
            if (_PanelCtrlName == "扫描入库")
                return;
            StockIn.U_StockInScanTab sis = new StockIn.U_StockInScanTab(this);

            AddUserControl(sis, "扫描入库");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ////触发一个事件通知control
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._New;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ////触发一个事件通知control
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Delete;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_Edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Edit;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_Fresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._ReFresh;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_NewLine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._NewLine;
                OnMenuClickEvent(null, args);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._DelLine;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_Check_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Check;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_UnCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Uncheck;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_StockOutScan_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //if (_PanelCtrlName == "扫描出库")
            //    return;
            //U_StockOutScan usod = new U_StockOutScan();

            //AddUserControl(usod, "扫描出库");
            //this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false);
            if (_PanelCtrlName == "扫描出库")
                return;
            U_StockOutScanTab usod = new U_StockOutScanTab(this);

            AddUserControl(usod, "扫描出库");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_NewStockIn_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "U_StockInAdd")
                return;
            U_StockInAdd usod = new U_StockInAdd();

            AddUserControl(usod, "U_StockInAdd");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_StockOutStat_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "出库上传")
                return;
            U_StockOutUploadList usod = new U_StockOutUploadList();

            AddUserControl(usod, "出库上传");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_CopyRequest_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "发货通知单参照")
                return;
            U_CT_RequestList usod = new U_CT_RequestList();
            AddUserControl(usod, "发货通知单参照");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_OutStockAdd_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void bar_View_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            ////触发一个事件通知control
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._View;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_ScanInPallet_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "平板扫描入库")
                return;
            StockIn.U_StockInScanPallet sis = new StockIn.U_StockInScanPallet(this);

            AddUserControl(sis, "平板扫描入库");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);

        }

        private void TestLayout_Load(object sender, EventArgs e)
        {
            string info = "用户：";
            info += Utils.LoginUserName+"  ";
            info += "角色："+Utils.LoginUserType+"  ";
            info += "机台：" + Utils.LoginMachineID;
            this.bar_LoginInfo.Caption = info;
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
            if (Utils.LoginUserType == "5" || Utils.LoginUserType == "9")
            {
                navBarGroup5.Visible = true;
                navBarGroup6.Visible = true;
                navBarGroup7.Visible = true;
            }
            else
            {
                navBarGroup5.Visible = false ;
                navBarGroup6.Visible = false ;
                navBarGroup7.Visible = false ;
            }

        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "用户管理")
                return;
            StockManage.U_UserManager sis = new StockManage.U_UserManager();

            AddUserControl(sis, "用户管理");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_WLZD_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "物料字典")
                return;
            StockManage.U_MaterialManager sis = new StockManage.U_MaterialManager();

            AddUserControl(sis, "物料字典");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_Org_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "组织字典")
                return;
            StockManage.U_OrganizationManager sis = new StockManage.U_OrganizationManager();

            AddUserControl(sis, "组织字典");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_BMZD_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "部门字典")
                return;
            StockManage.U_DepartmentManager sis = new StockManage.U_DepartmentManager();

            AddUserControl(sis, "部门字典");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_CKZD_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "仓库字典")
                return;
            StockManage.U_WarehouseManager sis = new StockManage.U_WarehouseManager();

            AddUserControl(sis, "仓库字典");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_RYZD_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "人员字典")
                return;
            StockManage.U_EmployerManager sis = new StockManage.U_EmployerManager();

            AddUserControl(sis, "人员字典");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_Fuzhu_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "辅助字典")
                return;
            StockManage.U_AuxiliaryManager sis = new StockManage.U_AuxiliaryManager();

            AddUserControl(sis, "辅助字典");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_Factory_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "工厂管理")
                return;
            StockManage.U_FactoryManager sis = new StockManage.U_FactoryManager();

            AddUserControl(sis, "工厂管理");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_Shift_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "班组管理")
                return;
            StockManage.U_ShiftManager sis = new StockManage.U_ShiftManager();

            AddUserControl(sis, "班组管理");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_transport_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "运输方式管理")
                return;
            StockManage.U_TransportManager sis = new StockManage.U_TransportManager();

            AddUserControl(sis, "运输方式管理");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_Business_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "业务类型管理")
                return;
            StockManage.U_BusinessManager sis = new StockManage.U_BusinessManager();

            AddUserControl(sis, "业务类型管理");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void TestLayout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("确定要退出本系统吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                e.Cancel =true;
            }
        }

        private void bar_StockDetail_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "库存明细")
                return;
            StockReport.U_StockDetail sis = new StockReport.U_StockDetail();

            AddUserControl(sis, "库存明细");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_StockStat_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "库存统计")
                return;
            StockReport.U_StockStat sis = new StockReport.U_StockStat();

            AddUserControl(sis, "库存统计");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "库存查询")
                return;
            StockReport.U_StockQuery sis = new StockReport.U_StockQuery();

            AddUserControl(sis, "库存查询");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }


        internal void OpenStockOutList(string voucherno)
        {
            if (_PanelCtrlName == "出库单列表")
                return;
            U_StockOutList usol = new U_StockOutList(this,voucherno);

            AddUserControl(usol, "出库单列表");
            this.DisplayMenus(true, false, true, true, true, false, false, false, false, false, false, true, false);

        }

        private void bar_Next_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ////触发一个事件通知control
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Next;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_Previous_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ////触发一个事件通知control
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Preview;
                OnMenuClickEvent(null, args);
            }
        }

        internal void OpenStockOut_PlanList(string voucherno)
        {
            if (_PanelCtrlName == "发货通知单列表")
                return;

            U_VoucherPlanList uvpa = new U_VoucherPlanList(this,voucherno);

            AddUserControl(uvpa, "发货通知单列表");
            this.DisplayMenus(true, false, true, true, false, true, false, true, true, false, false, false, false);
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            //MainTab uvpa = new MainTab();

            //AddUserControl(uvpa, "test");
        }

        private void bar_OrgMapping_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "组织映射")
                return;

            U_OrgMapping uvpa = new U_OrgMapping();

            AddUserControl(uvpa, "组织映射");
            this.DisplayMenus(true, false, true, true, false, true, false, true, true, false, false, false, false);
        }

        private void bar_Scanoutstat_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "出库统计")
                return;
            U_StockOutStat usod = new U_StockOutStat();

            AddUserControl(usod, "出库统计");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_FactoryCross_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "跨机台扫描")
                return;
            U_TransferScan usod = new U_TransferScan();

            AddUserControl(usod, "跨机台扫描");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_Lock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            ////触发一个事件通知control
            if (OnMenuClickEvent != null)
            {
                MenuClickEventArgs args = new MenuClickEventArgs();
                args.MenuName = Utils.WMSMenu._Lock;
                OnMenuClickEvent(null, args);
            }
        }

        private void bar_Warehouse_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "仓库映射")
                return;

            U_WarehouseMapping uvpa = new U_WarehouseMapping();

            AddUserControl(uvpa, "仓库映射");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);
        }

        private void bar_PaperLife_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (_PanelCtrlName == "纸卷状态")
                return;
            StockReport.U_PaperLife sis = new StockReport.U_PaperLife();

            AddUserControl(sis, "纸卷状态");
            this.DisplayMenus(false, false, false, false, false, false, false, false, false, false, false, false, false);

        }
        private void bar_AutoScan_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                Process autoScan = new Process();
                autoScan.StartInfo = new ProcessStartInfo("Chaint.Instock.Main.exe");
                autoScan.Start();
            }
            catch
            {
                MessageBox.Show("库区管理和自动扫描软件打开失败，请检查之后重试!");
            }
        }
    }
}