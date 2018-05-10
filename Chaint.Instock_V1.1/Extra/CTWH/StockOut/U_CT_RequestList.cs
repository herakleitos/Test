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
    public partial class U_CT_RequestList : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        MainLayout _MainLayout = null;
        public string _VourcherNO = "";
        WMSDS _WMS_MAIN;
        public U_CT_RequestList()
        {
            InitializeComponent();
        }
        public U_CT_RequestList(MainLayout tl)
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

        private void U_CT_RequeseList_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMS_MAIN = new WMSDS();
            this.dateS.DateTime = DateTime.Now.AddDays(-1);
            this.dateE.DateTime = DateTime.Now;
        }
        InterfaceDS _RequestDS;
        private void btn_Query_Click(object sender, EventArgs e)
        {
            string voucher = this.cmb_VoucherNO.Text;
            string dates = this.dateS.DateTime.ToString("yyyy-MM-dd HH:mm:00");
            string dateE = this.dateE.DateTime.ToString("yyyy-MM-dd HH:mm:00");

           _RequestDS=  this._WMSAccess.Select_CT_RequestInfo(voucher,dates,dateE);
          this.grid_Request.DataSource = _RequestDS.CT_RequestInfo;
          //this.gridView1.BestFitColumns();
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            if (this._RequestDS != null && this._RequestDS.CT_RequestInfo.Rows.Count > 0)
            {


                string vno = this.gridView1.GetFocusedRowCellValue("VoucherNO").ToString();

                //判断单据是否可以参照，
                WMSDS planDs = this._WMSAccess.Select_T_OutPlanAndEntry_RelationByVoucherNO(vno);
                if (planDs.T_OutStock_Plan.Rows.Count > 0)
                {
                    MessageBox.Show("该发货通知单已于" + planDs.T_OutStock_Plan.Rows[0][planDs.T_OutStock_Plan.BillDateColumn].ToString() + "参照，不能再次参照。");
                    return;
                }

                //查询接口中的单据信息
                InterfaceDS retDs = this._WMSAccess.Select_CT_RequestInfo(vno, "", "");
                //给本地发货通知抬头赋值
                InterfaceDS.CT_RequestInfoRow focusRow = retDs.CT_RequestInfo.Rows[0] as InterfaceDS.CT_RequestInfoRow;
                WMSDS outPlanDS = new WMSDS();

                WMSDS.T_OutStock_PlanRow ospRow = outPlanDS.T_OutStock_Plan.NewT_OutStock_PlanRow();
                //ospRow.AuxQty=;
                ospRow.BillDate = focusRow.RetrieveDate;//制单日期
                ospRow.BillDept = focusRow.DeptName;//部门名称
                //ospRow.Biller=;
                //ospRow.BillRemark=;
                ospRow.BillType = focusRow.BillType;//红蓝单
                //ospRow.BoatNO=;
                //ospRow.BoxNO=;
                ospRow.BusinessType = focusRow.BusinessType;//业务类型
                //ospRow.CarrierNO=;
                //ospRow.CommitAuxQty=;
                //ospRow.CommitQty=;
                ospRow.CustomerName = focusRow.CustomerName;//客户名称
                ospRow.DeptName = focusRow.DeptName;//部门名称
                ospRow.FactoryID = focusRow.FactoryID;//发出组织

                ospRow.OrgToCode = (focusRow.Cdefine1 == "0" || focusRow.IsCdefine1Null()) ? "" : focusRow.Cdefine1;//移入组织
                //ospRow.OrgToCode = focusRow.Cdefine1;//移入组织

                //ospRow.OrderNO=;
                //ospRow.Qty=;
                //ospRow.SourceVoucherNO=;
                //ospRow.SourceVoucherType=;
                ospRow.TradeType = focusRow.TradeType;//贸易类型
                ospRow.TransportType = focusRow.TransportType;//运输方式
                ospRow.VoucherNO = focusRow.VoucherNO;//通知单号
                ospRow.VehicleNO = focusRow.VehicleNO;//车号
                ospRow.WHCode = focusRow.WHCode;//发出仓库
                ospRow.WHToCode = (focusRow.Cdefine2 == "0" || focusRow.IsCdefine2Null()) ? "" : focusRow.Cdefine2;//移入仓库
                //ospRow.WHToCode = focusRow.Cdefine2;//移入仓库
                ospRow.IsCancel = "0";
                ospRow.IsCheck = "0";
                ospRow.IsClose = "0";

                //循环给本地分录赋值
                DataRowCollection drCollect = outPlanDS.T_OutStock_Plan_Entry.Rows;
                for (int i = 0; i < retDs.CT_RequestInfo.Rows.Count; i++)
                {
                    WMSDS.T_OutStock_Plan_EntryRow ospeRow = outPlanDS.T_OutStock_Plan_Entry.NewT_OutStock_Plan_EntryRow();
                    InterfaceDS.CT_RequestInfoRow requestRow = retDs.CT_RequestInfo.Rows[i] as InterfaceDS.CT_RequestInfoRow;
                    //判断是卷筒还是平板
                    int ProductType = 1;
                    if ((!requestRow.IsLength_PNull()) && requestRow.Length_P > 0)
                    {
                        //平板
                        ProductType = 2;
                    }
                    
                    //ospeRow.EntryID = requestRow.EntryID;//分录号

                    ospeRow.SourceEntryID = requestRow.EntryID;
                    ospeRow.EntryID = i + 1;

                    ospeRow.VoucherID = 0;//后面附上抬头的ID
                    ospeRow.MaterialCode = requestRow.MaterialCode.Trim();//物料代码
                    ospeRow.Grade = requestRow.Grade.Trim();//等级
                    //ospeRow.Width_R = requestRow.Width_R;//幅宽
                    ospeRow.WeightMode = requestRow.WeightMode.Trim();//称重方式
                    //ospeRow.CoreDiameter = ProductType == 1 ? requestRow.CoreDiameter : requestRow.Reams;//纸芯令数
                    //ospeRow.Length_P = requestRow.Length_P;//长度
                    //ospeRow.Width_P = requestRow.Width_P;//宽度
                    //ospeRow.Reams = ProductType == 1 ? requestRow.CoreDiameter : requestRow.Reams;//纸芯令数
                    //ospeRow.Diameter = ProductType == 1 ? requestRow.Udefine1 : requestRow.SlidesOfReam;//直径令张数
                    //ospeRow.RollLength = ProductType == 1 ? requestRow.Udefine2 : requestRow.SlidesOfSheet;//线长件张数
                    //ospeRow.SlidesOfReam = ProductType == 1 ? requestRow.Udefine1 : requestRow.SlidesOfReam;//直径令张数
                    //ospeRow.SlidesOfSheet = ProductType == 1 ? requestRow.Udefine2 : requestRow.SlidesOfSheet;//线长件张数

                    if (ProductType == 1)
                    {
                        ospeRow.CoreDiameter = requestRow.CoreDiameter;
                        ospeRow.Diameter = requestRow.Udefine1;
                        ospeRow.RollLength = requestRow.Udefine2;

                        ospeRow.Reams = 0;
                        ospeRow.SlidesOfReam = 0;
                        ospeRow.SlidesOfSheet = 0;
                    }
                    else
                    {
                        ospeRow.CoreDiameter = 0;
                        ospeRow.Diameter = 0;
                        ospeRow.RollLength = 0;

                        ospeRow.Reams = requestRow.Reams;
                        ospeRow.SlidesOfReam = requestRow.SlidesOfReam;
                        ospeRow.SlidesOfSheet = requestRow.SlidesOfSheet;
                    }

                    ospeRow.ReamPackType = requestRow.ReamPackType.Trim();//包装类型
                    ospeRow.SKU = requestRow.SKU.Trim();//sku

                    //ospeRow.PlanQty = requestRow.CommitQty;//主数量
                    //ospeRow.PlanAuxQty1 = requestRow.AuxCommitQty1;//辅数量1

                    ospeRow.PlanQty = requestRow.AuxCommitQty1;//主数量
                    ospeRow.PlanAuxQty1 = requestRow.CommitQty;//辅数量1

                    ospeRow.PlanAuxQty2 = requestRow.AuxCommitQty2;//辅数量2
                    ospeRow.CustTrademark = requestRow.CustTrademark.Trim();//取消使用
                    ospeRow.PaperCert = requestRow.PaperCert.Trim();//产品认证
                    ospeRow.SpecCustName = requestRow.SpecCustName.Trim();//客户专用
                    ospeRow.SpecProdName = requestRow.SpecProdName.Trim();//产品专用
                    ospeRow.Specification = ProductType==2 ? Utils.WMSMessage.TrimEndZero(requestRow.Width_P.ToString()) + "-" + Utils.WMSMessage.TrimEndZero(requestRow.Length_P.ToString()) : Utils.WMSMessage.TrimEndZero(requestRow.Width_R.ToString());//规格
                    ospeRow.TrademarkStyle = requestRow.TrademarkStyle.Trim();//夹板方式
                    ospeRow.IsWhiteFlag = requestRow.IsWhiteFlag.Trim();//商标类型
                    ospeRow.OrderNO = requestRow.OrderNO.Trim();//订单号

                    //ospeRow.Cdefine1 = requestRow.IsCdefine1Null() ? "" : requestRow.Cdefine1.Trim();//移入组织,在抬头
                    //ospeRow.Cdefine2 = requestRow.IsCdefine2Null() ? "" : requestRow.Cdefine2.Trim();//移入仓库,在抬头
                    ospeRow.Cdefine3 = requestRow.IsCdefine3Null() ? "" : requestRow.Cdefine3.Trim();
                    //ospeRow.Udefine1 = requestRow.IsUdefine1Null() ? 0 : requestRow.Udefine1;//直径
                    //ospeRow.Udefine2 = requestRow.IsUdefine2Null() ? 0 : requestRow.Udefine2;//线长
                    ospeRow.Udefine3 = requestRow.IsUdefine3Null() ? 0 : requestRow.Udefine3;
                    //查询物料名称
                    InterfaceDS wlzd = this._WMSAccess.Select_CT_WLZD(requestRow.MaterialCode);
                    ospeRow.MaterialName = wlzd.CT_WLZD.Rows.Count > 0 ? wlzd.CT_WLZD.Rows[0][wlzd.CT_WLZD.WLMCColumn].ToString() : "";
                    //插入分录集合
                    drCollect.Add(ospeRow);
                }
                //保存本地发货通知单
                string ret = this._WMSAccess.Tran_SaveNewOutStockPlan(ospRow, drCollect);

                if (ret == "")
                {
                    MessageBox.Show("参照成功");

                    string Des = string.Format("{0}参照了发货通知单{1}", Utils.LoginUserName, vno);
                    _WMSAccess.SaveOperateLog(Utils.LoginUserName, DateTime.Now, Des, "", "", vno);
                }
                else
                {
                    MessageBox.Show("参照失败：" + ret);
                }

            }
        }
    }
}
