using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;

namespace Chaint.Common.Core
{
/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2015-05-21
 * 
 * 功能描述: 
 *       报表设计器在打开或者新建报表时可以自动绑定相应的数据源
 * 
 ------------------------------------------------------------------------------------*/
    public class ReportDesigner
    {
        private DevExpress.XtraReports.Parameters.Parameter[] m_CustomParams = null;
        private DataTable m_dtSource = null;
        private string m_FormCaption = "";
        private string m_RptFileName = "";
        /// <summary>
        /// 报表设计器定义
        /// </summary>
        /// <param name="dtSource">报表数据源(数据表)</param>
        public ReportDesigner(DataTable dtSource):this(dtSource,"",null,"")
        {

        }

        /// <summary>
        /// 报表设计器定义
        /// </summary>
        /// <param name="dtSource">报表数据源(数据表)</param>
        /// <param name="strCaption">报表设计器的标题</param>
        public ReportDesigner(DataTable dtSource, string strCaption):this(dtSource,strCaption,null,"")
        {

        }
        /// <summary>
        /// 显示报表设计器
        /// </summary>
        /// <param name="dtSource">报表数据源(数据表)</param>
        /// <param name="strCaption">报表设计器的标题</param>
        /// <param name="customParam">报表设计器对应的的自定义参数</param>
        /// <param name="strFileName">报表设计器初始打开的报表文件名称,如为空，则打开一个空报表</param>
        public ReportDesigner(DataTable dtSource, string strCaption, DevExpress.XtraReports.Parameters.Parameter[] customParam, string strFileName)
        {
            m_dtSource = dtSource;
            m_FormCaption = strCaption;
            m_CustomParams = customParam;
            m_RptFileName = strFileName;
        }

        /// <summary>
        /// 显示报表设计器
        /// </summary>
        public void ShowDesigner()
        {
            if (m_dtSource == null) return;

            XRDesignForm form = new XRDesignForm();
            if (m_FormCaption.Trim().Length > 0) form.Text = m_FormCaption;
            XRDesignMdiController mdiController = form.DesignMdiController;

            mdiController.DesignPanelLoaded += mdiController_DesignPanelLoaded;

            if(File.Exists(m_RptFileName))
                mdiController.OpenReport(m_RptFileName);
            else
                mdiController.OpenReport(new XtraReport());
            form.Show();
        }

        public void ShowDesigner(System.Windows.Forms.IWin32Window owner)
        {
            if (m_dtSource == null) return;

            XRDesignForm form = new XRDesignForm();
            if (m_FormCaption.Trim().Length > 0) form.Text = m_FormCaption;
            XRDesignMdiController mdiController = form.DesignMdiController;

            mdiController.DesignPanelLoaded += mdiController_DesignPanelLoaded;
            if (File.Exists(m_RptFileName))
                mdiController.OpenReport(m_RptFileName);
            else
                mdiController.OpenReport(new XtraReport());
            form.Show(owner);
        }
        private void mdiController_DesignPanelLoaded(object sender, DesignerLoadedEventArgs e)
        {
            XRDesignPanel pnl = sender as XRDesignPanel;
            pnl.Report.DataSource = m_dtSource;
            if (m_CustomParams != null)
            {
                pnl.Report.Parameters.Clear();
                pnl.Report.Parameters.AddRange(m_CustomParams);
            }
        }
    }
}
