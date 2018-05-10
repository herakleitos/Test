using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
namespace Chaint.Common.Core
{
    public static class ChaintMessageBox
    {
        public static void Show(string msg)
        {
            XtraMessageBox.Show(msg,"长泰提示");
        }
        public static DialogResult ShowConfirmDialog(string msg)
        {
            return XtraMessageBox.Show(msg, "长泰提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult ShowOkDialog(string msg)
        {
            return XtraMessageBox.Show(msg, "长泰提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
    }
}
