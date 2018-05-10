using System;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.Controler;
using System.Windows.Forms;

namespace Chaint.Instock.Business
{
    public partial class Stock : XtraForm
    {
        private Context context;
        private StockControler controler;
        public StockView view;
        public Stock()
        {
        }
        public Stock(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
        }
        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void InitControler()
        {
            controler = new StockControler(view);
        }
        private void InitView()
        {
            this.view = new StockView(context);
            this.view.AddControl(Const_Stock.Base_Form, this);
            this.view.AddControl(Const_Stock.Head_Menu_Save, btSave);
            this.view.AddControl(Const_Stock.Head_Menu_New, btNew);
            this.view.AddControl(Const_Stock.Head_Menu_List, btList);
            this.view.AddControl(Const_Stock.Head_Menu_Delete, btDelete);

            this.view.AddControl(Const_Stock.Head_Field_FID, this.FID);
            this.view.AddControl(Const_Stock.Head_Field_FNumber, this.FNumber);
            this.view.AddControl(Const_Stock.Head_Field_FName, this.FName);
            this.view.AddControl(Const_Stock.Head_Field_FMemo, this.FMemo);
            this.view.AddControl(Const_Stock.Head_Field_FLocation, this.FLocation);
            this.view.AddControl(Const_Stock.Head_Field_FCreateDate, this.FCreateDate);
            this.view.AddControl(Const_Stock.Head_Field_FModifyDate, this.FModifyDate);
            //初始化view时，给fid赋值。
            view.SetValue(Const_StockAreaPlan.Head_Field_FID, SequenceGuid.NewGuid().ToString());
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);

            this.Load += new EventHandler(this.OnLoad);
            this.btDelete.ItemClick += new ItemClickEventHandler(this.bt_Delete_ItemClick);
            this.btNew.ItemClick += new ItemClickEventHandler(this.bt_New_ItemClick);
            this.btSave.ItemClick += new ItemClickEventHandler(this.bt_Save_ItemClick);
            this.btList.ItemClick += new ItemClickEventHandler(this.btList_ItemClick);

            this.FName.EditValueChanged += new System.EventHandler(this.FName_EditValueChanged);
            this.FNumber.EditValueChanged += new System.EventHandler(this.FNumber_EditValueChanged);
            this.FLocation.EditValueChanged += new System.EventHandler(this.FLocation_EditValueChanged);
            this.FMemo.EditValueChanged += new System.EventHandler(this.FMemo_EditValueChanged);

            this.FCreateDate.EditValueChanged += new System.EventHandler(this.FCreateDate_EditValueChanged);
            this.FModifyDate.EditValueChanged += new System.EventHandler(this.FModifyDate_EditValueChanged);
        }

        private void OnDisposed(object sender, EventArgs e)
        {

            this.Load -= new EventHandler(this.OnLoad);
            this.btDelete.ItemClick -= new ItemClickEventHandler(this.bt_Delete_ItemClick);
            this.btNew.ItemClick -= new ItemClickEventHandler(this.bt_New_ItemClick);
            this.btSave.ItemClick -= new ItemClickEventHandler(this.bt_Save_ItemClick);
            this.btList.ItemClick -= new ItemClickEventHandler(this.btList_ItemClick);

            this.FName.EditValueChanged -= new System.EventHandler(this.FName_EditValueChanged);
            this.FNumber.EditValueChanged -= new System.EventHandler(this.FNumber_EditValueChanged);
            this.FLocation.EditValueChanged -= new System.EventHandler(this.FLocation_EditValueChanged);
            this.FMemo.EditValueChanged -= new System.EventHandler(this.FMemo_EditValueChanged);

            this.FCreateDate.EditValueChanged -= new System.EventHandler(this.FCreateDate_EditValueChanged);
            this.FModifyDate.EditValueChanged -= new System.EventHandler(this.FModifyDate_EditValueChanged);
            this.view.Dispose();
        }
        #endregion
        private void bt_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btSave.Caption;
            this.btSave.Caption = "请稍后";
            this.btSave.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Stock.Head_Menu_Save;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btSave.Caption = oldText;
            this.btSave.Enabled = true;
        }

        private void bt_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btNew.Caption;
            this.btNew.Caption = "请稍后";
            this.btNew.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Stock.Head_Menu_New;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btNew.Caption = oldText;
            this.btNew.Enabled = true;
        }

        private void bt_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btDelete.Caption;
            this.btDelete.Caption = "请稍后";
            this.btDelete.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Stock.Head_Menu_Delete;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btDelete.Caption = oldText;
            this.btDelete.Enabled = true;
        }
        private void FMemo_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_Stock.Head_Field_FMemo);
        }
        private void FNumber_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_Stock.Head_Field_FNumber);
        }
        private void FName_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_Stock.Head_Field_FName);
        }
        private void FLocation_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_Stock.Head_Field_FLocation);
        }
        private void FCreateDate_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_Stock.Head_Field_FCreateDate);
        }

        private void FModifyDate_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_Stock.Head_Field_FModifyDate);
        }
        private void btList_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btList.Caption;
            this.btList.Caption = "请稍后";
            this.btList.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Stock.Head_Menu_List;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btList.Caption = oldText;
            this.btList.Enabled = true;
        }
    }
}
