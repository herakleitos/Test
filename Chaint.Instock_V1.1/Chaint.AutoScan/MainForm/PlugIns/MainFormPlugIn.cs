using System;
using System.Windows.Forms;
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using Chaint.Common.BasePlugIn;
using Chaint.Instock.Business;
using Chaint.Common.Core;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.Utils;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Chaint.Common.ServiceHelper;
using Chaint.Instock.Core;
namespace Chaint.Instock.Main.PlugIns
{
    public class MainFormPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        public MainFormPlugIn(MainFormView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>("form");
        }
        public override void AfterBind()
        {
            base.AfterBind();
            if (this.form == null) return;
            HomePage firstPage = new HomePage();
            OpenForm(firstPage);
            XtraTabbedMdiManager mdiManager =
                                this.View.GetControl<XtraTabbedMdiManager>("xtraTabbedMdiManager");
            mdiManager.Pages[firstPage].Image = Main.Properties.Resources.Home_16x16;
            if (this.Context.CompanyCode == "2")
            {
                this.View.GetControl<BarButtonItem>("btStockAreaPropertity").Visibility = BarItemVisibility.Never;
            }
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            switch (e.Sender)
            {
                case "btStock":
                case "btStockArea":
                case "btStockAreaPropertity":

                case "btStockAreaInit":
                case "btInitDataQuery":

                case "btStockAreaDataPlan":
                case "btStockAreaPlanManage":
                case "btStockAreaPlanQuery":

                case "btStockAreaDataManage":
                case "btStockAreaDataTransfer":
                case "btStockAreaDataQuery":

                case "btInstockConfirm":
                case "btAutoScan":

                case "btAuthManage":

                    if (this.Context.UserID.IsNullOrEmptyOrWhiteSpace())
                    {
                        ChaintMessageBox.Show("请登录!");
                        return;
                    }
                    break;
                default:
                    break;
            }
            XtraTabbedMdiManager mdiManager =
                                  this.View.GetControl<XtraTabbedMdiManager>("xtraTabbedMdiManager");
            if (mdiManager == null) return;
            var children = this.form.MdiChildren;
            switch (e.Sender)
            {
                case "btStock":
                    if (this.form == null) return;
                    if (Active<Stock>(children)) return;
                    Stock stock = new Stock(this.Context);
                    stock.view.Open += OpenListEvent;
                    OpenForm(stock);
                    break;
                case "btStockArea":
                    if (this.form == null) return;
                    if (Active<StockArea>(children)) return;
                    StockArea stockArea = new StockArea(this.Context);
                    stockArea.view.Open += OpenListEvent;
                    OpenForm(stockArea);
                    break;
                case "btStockAreaPropertity":
                    if (this.form == null) return;
                    if (Active<StockAreaPlan>(children)) return;
                    StockAreaPlan stockAreaPlan = new StockAreaPlan(this.Context);
                    stockAreaPlan.view.Open += OpenListEvent;
                    OpenForm(stockAreaPlan);
                    break;
                case "btAutoScan":
                    if (this.form == null) return;
                    if (Active<StockInAutoScan>(children)) return;
                    StockInAutoScan stockInAutoScan = new StockInAutoScan(this.Context);
                    OpenForm(stockInAutoScan);
                    break;
                case "btInitDataQuery":
                    if (this.form == null) return;
                    if (Active<StockAreaData>(children, "初始化数据查询")) return;
                    this.Context.RemoveOption("Type");
                    this.Context.AddOption("Type", -1);
                    StockAreaData initDataQuery = new StockAreaData(this.Context);
                    OpenForm(initDataQuery);
                    break;
                case "btStockAreaInit":
                    if (this.form == null) return;
                    if (Active<StockAreaData>(children, "库区数据初始化")) return;
                    this.Context.RemoveOption("Type");
                    this.Context.AddOption("Type", 0);
                    StockAreaData stockAreaData = new StockAreaData(this.Context);
                    OpenForm(stockAreaData);
                    break;
                case "btStockAreaDataPlan":
                    if (this.form == null) return;
                    if (Active<StockAreaData>(children, "数据计划")) return;
                    this.Context.RemoveOption("Type");
                    this.Context.AddOption("Type", 1);
                    StockAreaData stockAreaDataPlan = new StockAreaData(this.Context);
                    OpenForm(stockAreaDataPlan);
                    break;
                case "btStockAreaPlanManage":
                    if (this.form == null) return;
                    if (Active<StockAreaData>(children, "计划管理")) return;
                    this.Context.RemoveOption("Type");
                    this.Context.AddOption("Type", 2);
                    StockAreaData stockAreaPlanManage = new StockAreaData(this.Context);
                    OpenForm(stockAreaPlanManage);
                    break;
                case "btStockAreaDataTransfer":
                    if (this.form == null) return;
                    if (Active<StockAreaData>(children,"移库")) return;
                    this.Context.RemoveOption("Type");
                    this.Context.AddOption("Type", 4);
                    StockAreaData stockAreaDataTrans = new StockAreaData(this.Context);
                    OpenForm(stockAreaDataTrans);
                    break;
                case "btStockAreaDataManage":
                    if (this.form == null) return;
                    if (Active<StockAreaData>(children,"库区数据管理")) return;
                    this.Context.RemoveOption("Type");
                    this.Context.AddOption("Type", 3);
                    StockAreaData stockAreaDataManage = new StockAreaData(this.Context);
                    OpenForm(stockAreaDataManage);
                    break;
                case "btStockAreaPlanQuery":
                    if (this.form == null) return;
                    if (Active<StockAreaPlanQuery>(children)) return;
                    StockAreaPlanQuery stockAreaPlanQuery = new StockAreaPlanQuery(this.Context);
                    OpenForm(stockAreaPlanQuery);
                    break;
                case "btStockAreaDataQuery":
                    if (this.form == null) return;
                    if (Active<StockAreaDataQuery>(children)) return;
                    StockAreaDataQuery stockAreaDataQuery = new StockAreaDataQuery(this.Context);
                    OpenForm(stockAreaDataQuery);
                    break;
                case "btInstockConfirm":
                    if (this.form == null) return;
                    if (Active<DistributionList>(children)) return;
                    DistributionList disList = new DistributionList(this.Context);
                    OpenForm(disList);
                    break;
                case "btAuthManage":
                    if (this.form == null) return;
                    if (Active<Auth>(children)) return;
                    Auth auth = new Auth(this.Context);
                    OpenForm(auth);
                    break;
                case "btClose":
                    if (this.form == null) return;
                    form.Close();
                    break;
                case "btLogIn":
                    if (!this.Context.UserID.IsNullOrEmptyOrWhiteSpace()) return;
                    if (this.form == null) return;
                    if (Active<LogIn>(children)) return;
                    LogIn logIn = new LogIn(this.Context);
                    logIn.GetUserInfoEvent += GetUserInfo;
                    logIn.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    logIn.ShowDialog();
                    break;
                case "btLogOut":
                    ClearUserInfo();
                    break;
            }
        }

        private bool Active<T>(Form[] children)
        {
            foreach (var child in children)
            {
                if (child is T)
                {
                    child.Activate();
                    return true;
                }
            }
            return false;
        }
        private bool Active<T>(Form[] children,string title)
        {
            foreach (var child in children)
            {
                if (child is T&&child.Text==title)
                {
                    child.Activate();
                    return true;
                }
            }
            return false;
        }
        private void tmrActive_Tick(object sender, EventArgs e)
        {
            ToolStripLabel lbTime = this.View.GetControl<ToolStripLabel>("lbTime");
            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private bool NotVal()
        {
            XtraTabbedMdiManager mdiManager =
                                   this.View.GetControl<XtraTabbedMdiManager>("xtraTabbedMdiManager");
            if (mdiManager == null) return false;
            var children = this.form.MdiChildren;
            foreach (var child in children)
            {
                if (child is StockInAutoScan)
                {
                    ChaintMessageBox.Show("自动扫描入库只能打开一个实例!");
                    return false;
                }
            }
            return true;
        }
        private void ClearUserInfo()
        {
            if (this.Context.UserID.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("当前用户没有登陆!");
                return;
            }
            this.Context.UserID = string.Empty;
            this.Context.UserName = string.Empty;
            this.Context.PassWord = string.Empty;
            BarButtonItem btLogIn = this.View.GetControl<BarButtonItem>("btLogIn");
            btLogIn.Caption = "登陆";
            XtraTabbedMdiManager mdiManager =
                                  this.View.GetControl<XtraTabbedMdiManager>("xtraTabbedMdiManager");
            if (mdiManager == null) return;
            //关闭所有子窗体
            var children = this.form.MdiChildren;
            foreach (var child in children)
            {
                if (child is HomePage) continue;
                child.Close();
            }
            ChaintMessageBox.Show("注销成功!");
        }
        private void GetUserInfo(string userId, string userName, string passWord)
        {
            this.Context.UserID = userId;
            this.Context.UserName = userName;
            this.Context.PassWord = passWord;
            BarButtonItem btLogIn = this.View.GetControl<BarButtonItem>("btLogIn");
            btLogIn.Caption = string.Format("当前用户：{0}", userName);
            if (this.Context.UserID != "Admin")
            {
                this.View.GetControl<BarStaticItem>("Auth").Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>("btAuthManage").Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>("btPasswordModify").Visibility = BarItemVisibility.Never;
                List<string> formIds = GetAuthInfo();
                bool isShowBase = true;
                if (formIds.Contains(Const_Option.Const_Stock))
                {
                    this.View.GetControl<BarButtonItem>("btStock").Visibility = BarItemVisibility.Always;
                    isShowBase = true;
                }
                else
                {
                    this.View.GetControl<BarButtonItem>("btStock").Visibility = BarItemVisibility.Never;
                    isShowBase = false;
                }
                if (formIds.Contains(Const_Option.Const_StockArea))
                {
                    this.View.GetControl<BarButtonItem>("btStockArea").Visibility = BarItemVisibility.Always;
                    isShowBase = true;
                }
                else
                {
                    this.View.GetControl<BarButtonItem>("btStockArea").Visibility = BarItemVisibility.Never;
                    isShowBase = false;
                }
                if (isShowBase)
                {
                    this.View.GetControl<BarStaticItem>("Base").Visibility = BarItemVisibility.Always;
                }
                else
                {
                    this.View.GetControl<BarStaticItem>("Base").Visibility = BarItemVisibility.Never;
                }
            }
            else
            {
                this.View.GetControl<BarStaticItem>("Auth").Visibility = BarItemVisibility.Always;
                this.View.GetControl<BarButtonItem>("btAuthManage").Visibility = BarItemVisibility.Always;
                this.View.GetControl<BarButtonItem>("btPasswordModify").Visibility = BarItemVisibility.Always;
                this.View.GetControl<BarButtonItem>("btStockArea").Visibility = BarItemVisibility.Always;
                this.View.GetControl<BarButtonItem>("btStock").Visibility = BarItemVisibility.Always;
            }
        }
        private List<string> GetAuthInfo()
        {
            string sql = @"SELECT * FROM T_AUTOSCAN_AUTH WHERE FUSERID =@FUSERID AND FCHECK =1 ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FUSERID", DbType.String));
            param[0].Value = this.Context.UserID;
            string[] tableName = new string[] { "T1" };
            List<string> formIds = new List<string>();
            OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, param.ToArray());
            if (!result.IsSuccess || result.ResultData.Tables.Count <= 0
                 || result.ResultData.Tables["T1"].Rows.Count <= 0)
            {
                return formIds;
            }
            foreach (DataRow row in result.ResultData.Tables["T1"].Rows)
            {
                string formId = row.GetValue<string>(Const_Auth.Entry_Column_FFormId);
                formIds.Add(formId);
            }
            return formIds;
        }
        private void OpenBillEvent(string formName, object data)
        {
            if (data != null)
            {
                this.Context.RemoveOption("FID");
                this.Context.AddOption("FID", data);
            }
            if (formName == Const_Option.Const_StockAreaPlan)
            {
                StockAreaPlan stockAreaPlan = new StockAreaPlan(this.Context);
                stockAreaPlan.view.Open += OpenListEvent;
                OpenForm(stockAreaPlan);
            }
            else if (formName == Const_Option.Const_StockArea)
            {
                StockArea stockArea = new StockArea(this.Context);
                stockArea.view.Open += OpenListEvent;
                OpenForm(stockArea);
            }
            else if (formName == Const_Option.Const_Stock)
            {
                Stock stock = new Stock(this.Context);
                stock.view.Open += OpenListEvent;
                OpenForm(stock);
            }
        }
        private void OpenListEvent(string formName, object data)
        {
            if (data.Equals(Const_Option.Const_StockAreaPlan))
            {
                this.Context.RemoveOption(Const_Option.Const_FormName);
                this.Context.AddOption(Const_Option.Const_FormName, Const_Option.Const_StockAreaPlan);
            }
            else if (data.Equals(Const_Option.Const_StockArea))
            {
                this.Context.RemoveOption(Const_Option.Const_FormName);
                this.Context.AddOption(Const_Option.Const_FormName, Const_Option.Const_StockArea);
            }
            else if (data.Equals(Const_Option.Const_Stock))
            {
                this.Context.RemoveOption(Const_Option.Const_FormName);
                this.Context.AddOption(Const_Option.Const_FormName, Const_Option.Const_Stock);
            }
            CommonList comList = new CommonList(this.Context);
            comList.view.Open += OpenBillEvent;
            OpenForm(comList);
        }
        private void OpenForm(XtraForm frm)
        {
            XtraTabbedMdiManager mdiManager =
                                 this.View.GetControl<XtraTabbedMdiManager>("xtraTabbedMdiManager");
            if (mdiManager == null) return;
            mdiManager.MdiParent = this.form;//设置控件的父表单.
            frm.MdiParent = this.form;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            mdiManager.SelectedPage = mdiManager.Pages[frm];//使得标签的选择为当前新建的窗口
            mdiManager.ClosePageButtonShowMode =
                DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
        }
    }
}
