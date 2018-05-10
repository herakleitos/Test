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
using DevExpress.XtraGrid.Views.Base;

namespace Chaint.Instock.Business
{
    public partial class Auth : XtraForm
    {
        private Context context;
        private AuthControler controler;
        public AuthView view;
        public Auth()
        {
        }
        public Auth(Context ctx)
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
            controler = new AuthControler(view);
        }
        private void InitView()
        {
            this.view = new AuthView(context);
            this.view.AddControl(Const_Auth.Main_Form, this);
            this.view.AddControl(Const_Auth.Main_ButtonSave, btSave);
            this.view.AddControl(Const_Auth.Head_Field_FUser, FUser);
            this.view.AddControl(Const_Auth.Main_Entry, gv_Entry);
            this.view.AddControl(Const_Auth.Main_EntryControl, gc_Entry);
            this.view.AddControl(Const_Auth.Entry_Field_FUserID, FUser);
            this.view.AddControl(Const_Auth.Entry_Field_FFormId, FFormId);
            this.view.AddControl(Const_Auth.Entry_Field_FFormName, FFormName);
            this.view.AddControl(Const_Auth.Entry_Field_FCheck, FCheck);
            this.view.AddControl(Const_Auth.Entry_Control_FCheck, cb_Check);
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.Load += new EventHandler(this.OnLoad);
            this.btSave.ItemClick += new ItemClickEventHandler(this.bt_Save_ItemClick);
            this.gv_Entry.CellValueChanged +=
                 new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(this.OnLoad);
            this.btSave.ItemClick -= new ItemClickEventHandler(this.bt_Save_ItemClick);
            this.gv_Entry.CellValueChanged -=
                 new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
            this.view.Dispose();
        }
        #endregion
        private void bt_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btSave.Caption;
            this.btSave.Caption = "请稍后";
            this.btSave.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Auth.Main_ButtonSave;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btSave.Caption = oldText;
            this.btSave.Enabled = true;
        }
        private void gv_Entry_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //即时更新数据源
            if (!gv_Entry.IsNewItemRow(gv_Entry.FocusedRowHandle))
            {
                gv_Entry.CloseEditor();
                gv_Entry.UpdateCurrentRow();
            }
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Row = e.RowHandle;
            args.Parent = Const_Auth.Main_Entry;
            args.Sender = e.Column.Name;
            args.Value = e.Value;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void FUser_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_Auth.Head_Field_FUser;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
    }
}
