using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Chaint.Common.Core;
using Chaint.Common.Core.EventArgs;

namespace Chaint.Instock.Business
{
    public partial class StockArea:XtraForm
    {
        private Context context;
        public StockArea(Context ctx)
        {
            context = ctx;
            InitializeComponent();
        }
    }
}
