using System;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.EventArgs;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data;
using Chaint.Common.Core.Utils;
using Chaint.Instock.Core;
using Chaint.Instock.Business.Controler;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business
{
    public partial class StockAreaDataQuery : XtraForm
    {
        private Context context;
        private StockAreaDataQueryControler controler;
        private StockAreaDataQueryView view;
        public StockAreaDataQuery(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
        }
        private void InitControler()
        {
            controler = new StockAreaDataQueryControler(view);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void InitView()
        {
            this.view = new StockAreaDataQueryView(context);
            //控件
            this.view.AddControl(Const_StockAreaDataQuery.Base_Form, this);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Menu_Query, btQuery);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Container, this.lc_Head);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Control, gc_Data);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Entry, gv_Entry);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FBeginDate, FBeginDate);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FEndDate, FEndDate);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FProduct, this.FHProduct);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FStock, this.FHStock);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FStockArea, this.FHStockArea);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FPaperType, this.FHPaperType);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FSpecification, this.FHSpecification);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FCoreDiameterOrReam, this.FHCoreDiameterOrReam);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FLength, this.FHLength);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FDiameterOrSlides, this.FHDiameterOrSlides);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FTransportType, this.FHTransportType);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FWeightMode, this.FHWeightMode);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FMemo, this.FHMemo);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FPoNumber, this.FHPoNumber);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FCertification, this.FHCertification);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FTrademarkStyle, this.FHTrademarkStyle);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FPackType, this.FHPackType);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FSpecCustName, this.FHSpecCustName);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FPaperGrade, this.FHPaperGrade);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FSpecProdName, this.FHSpecProdName);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FDeliverDate, this.FHDeliverDate);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FColor, this.FHColor);
            this.view.AddControl(Const_StockAreaDataQuery.Head_Field_FSpCustomer, this.FHSpCustomer);

            //container
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FProduct, this.lo_FProduct);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FStock, this.lo_FStock);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FStockArea, this.lo_FStockArea);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FPaperType, this.lo_FPaperType);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FPaperGrade,this.lo_FPaperGrade);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FBeginDate, this.lo_FBeginDate);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FEndDate, this.lo_FEndDate);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FMemo, this.lo_FMemo);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FTransportType, this.lo_FTransportType);

            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FCertification, this.lo_FCertification);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FSpecification, this.lo_FSpecification);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FCoreDiameterOrReam, this.lo_FCoreDiameterOrReam);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FDiameterOrSlides, this.lo_FDiameterOrSlides);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FLength, this.lo_FLength);

            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FTrademarkStyle, this.lo_FTrademarkStyle);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FSpecCustName, this.lo_FSpecCustName);

            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FSpecCustName, this.lo_FSpecCustName);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FSpecProdName, this.lo_FSpecProdName);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FPackType, this.lo_FPackType);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FDeliverDate, this.lo_FDeliverDate);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FWeightMode, this.lo_FWeightMode);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FColor, this.lo_FColor);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FPoNumber, this.lo_FPoNumber);
            this.view.AddControl(Const_StockAreaDataQuery.Entry_Container_FSpCustomer, this.lo_FSpCustomer);
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.Load+=new EventHandler(this.OnLoad);
            this.gv_Entry.CustomRowCellEdit += new CustomRowCellEditEventHandler(gv_View_CustomRowCellEdit);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(this.OnLoad);
            this.gv_Entry.CustomRowCellEdit -= new CustomRowCellEditEventHandler(gv_View_CustomRowCellEdit);
            this.view.Dispose();
        }

        private void gv_View_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            CustomEntryRowCellEventArgs args = new CustomEntryRowCellEventArgs();
            args.Sender = e.Column.Name;
            args.Row = e.RowHandle;
            args.Item = e.RepositoryItem;
            controler.Invoke(Const_Event.CustomEntryRowCell, args);
            e.RepositoryItem = args.Item;
        }
        #endregion
        private void btQuery_Click(object sender, EventArgs e)
        {
            string oldText = this.btQuery.Text;
            this.btQuery.Text = "请稍后";
            this.btQuery.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaDataQuery.Head_Menu_Query;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btQuery.Text = oldText;
            this.btQuery.Enabled = true;
        }

        private void FHPaperType_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockAreaDataQuery.Head_Field_FPackType;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void FHStock_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockAreaDataQuery.Head_Field_FStock;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void btExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btExport.Caption;
            this.btExport.Caption = "请稍后";
            this.btExport.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaDataQuery.Head_Menu_Export;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btExport.Caption = oldText;
            this.btExport.Enabled = true;
        }
    }
}
