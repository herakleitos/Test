using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraEditors;

namespace Chaint.Common.Core.Utils
{
    public class GridViewUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCaption">列标题</param>
        /// <param name="strFieldName">列字段名</param>
        /// <param name="columnWidth">列宽</param>
        /// <returns></returns>
        public static DevExpress.XtraGrid.Columns.GridColumn CreateGridColumn(string strCaption, string strFieldName, int columnWidth)
        {
            return CreateGridColumn(strCaption, string.Join("col", strFieldName), strFieldName, columnWidth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCaption">列标题</param>
        /// <param name="strName">列名</param>
        /// <param name="strFieldName">列字段名</param>
        /// <param name="columnWidth">列宽</param>
        /// <returns></returns>
        public static DevExpress.XtraGrid.Columns.GridColumn CreateGridColumn(string strCaption, string strName, string strFieldName, int columnWidth)
        {
            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = strCaption;
            col.Name = strName;
            col.FieldName = strFieldName;
            col.Width = columnWidth;
            col.Visible = true;
            return col;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCaption">列标题</param>
        /// <param name="strFieldName">绑定列字段</param>
        /// <param name="columnWidth">列宽</param>
        /// <returns></returns>
        public static DevExpress.XtraGrid.Columns.GridColumn CreateGridColumnDateTime(string strCaption, string strFieldName, int columnWidth)
        {
            return CreateGridColumnDateTime(strCaption, string.Join("col", strFieldName), strFieldName, columnWidth);
        }

        /// <summary>
        /// 创建带日期的列
        /// </summary>
        /// <param name="strCaption"列标题></param>
        /// <param name="strName">列名</param>
        /// <param name="strFieldName">绑定列字段</param>
        /// <param name="columnWidth">列宽</param>
        /// <returns></returns>
        public static DevExpress.XtraGrid.Columns.GridColumn CreateGridColumnDateTime(string strCaption, string strName, string strFieldName, int columnWidth)
        {
            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = strCaption;
            col.Name = strName;
            col.FieldName = strFieldName;
            col.Width = columnWidth;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            col.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            col.Visible = true;
            return col;
        }

        /// <summary>
        /// 设置 GridView的常用属性
        /// </summary>
        /// <param name="gvCntrl"></param>
        public static void SetGridViewBasicPrperty(DevExpress.XtraGrid.Views.Grid.GridView gvCntrl)
        {
            //可以手动调节列宽
            gvCntrl.OptionsView.ColumnAutoWidth = false;


            gvCntrl.IndicatorWidth = 50;
            gvCntrl.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

            //可自定义奇偶颜色
            gvCntrl.Appearance.EvenRow.BackColor = Color.FromArgb(150, 237, 243, 254);
            gvCntrl.Appearance.OddRow.BackColor = Color.FromArgb(150, 199, 237, 204);

            gvCntrl.OptionsView.EnableAppearanceEvenRow = true;
            gvCntrl.OptionsView.EnableAppearanceOddRow = true;

            gvCntrl.CustomDrawRowIndicator += GridViewUtils.GridView_CustomDrawRowIndicator;
            gvCntrl.CustomDrawEmptyForeground += GridViewUtils.GridView_CustomDrawEmptyForeground;

        }


        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="toolTip"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void ShowToolTip(DevExpress.Utils.ToolTipController toolTip, string title, string content)
        {
            toolTip.AutoPopDelay = 2000;
            System.Drawing.Point _mousePoint = Control.MousePosition;
            toolTip.ShowHint(content, title, _mousePoint);
        }

        /// <summary>
        /// 可以显示某一行的基本信息
        /// 有时候为了方便数据的显示，需要在GridView的第一列显示该列的行信息以及行号，
        /// 那么需要为GridView控件添加一个ToolTipController控件，然后实现该控件的GetActiveObjectInfo事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ToolTipController_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e, DevExpress.XtraGrid.GridControl gridCntrl)
        {
            DevExpress.Utils.ToolTipControlInfo info = null;

            //Get the view at the current mouse position
            DevExpress.XtraGrid.Views.Grid.GridView view = gridCntrl.GetViewAt(e.ControlMousePosition) as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            //Get the view's element information that resides at the current position
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
            //Display a hint for row indicator cells
            if (hi.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowIndicator)
            {
                //An object that uniquely identifies a row indicator cell
                object o = hi.HitTest.ToString() + hi.RowHandle.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("行数据基本信息：");
                foreach (DevExpress.XtraGrid.Columns.GridColumn gridCol in view.Columns)
                {
                    if (gridCol.Visible)
                    {
                        sb.AppendFormat("    {0}：{1}\r\n", gridCol.Caption, view.GetRowCellDisplayText(hi.RowHandle, gridCol.FieldName));
                    }
                }
                info = new DevExpress.Utils.ToolTipControlInfo(o, sb.ToString());
            }

            //Supply tooltip information if applicable, otherwise preserve default tooltip (if any)
            if (info != null)
            {
                e.Info = info;
            }
        }

        /// <summary>
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void GridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //IndicatorWidth = 40;
            if (e.Info.IsRowIndicator && e.RowHandle >= 0) e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        /// <summary>
        /// 当查询为空时显示提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void GridView_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            //(不GridView没有设置数据源绑定时，使用，一般使用此种方法）  
            if (sender is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                DevExpress.XtraGrid.Views.Grid.GridView currView = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                if (currView.RowCount == 0)
                {
                    string disMsg = "No Data!";
                    Font f = new Font("宋体", 12, FontStyle.Regular);
                    Rectangle r = new Rectangle(e.Bounds.Left + 10, e.Bounds.Top + 20, e.Bounds.Width - 5, e.Bounds.Height - 5);
                    e.Graphics.DrawString(disMsg, f, Brushes.Gray, r);
                }
            }
        }
    }
}
