using System;
using System.Collections.Generic;
using System.Text;


using System.Diagnostics;
using System.IO;

/*-----------------------------------------------------------------------------------
 * 作者: Chaint.IT
 * 
 * 创建时间: 2015-09-28
 * 
 * 功能描述: 
 *      对执行文件夹下的帮助文件类型(CHM)的调用操作类
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.IO
{
    public class HelpService
    {
        private static System.Windows.Forms.HelpProvider m_helpProvider = null;


        /// <summary>
        /// 利用进程的方法打开CHM文件，并且每次定位至文件的首页
        /// </summary>
        /// <param name="strFilePath"></param>
        public static void OpenChm_By_Process(string strFilePath)
        {
            if(File.Exists(strFilePath))
            {
                Process.Start(strFilePath);
            }
            else
            {
                System.Windows.Forms.OpenFileDialog opendlg = new System.Windows.Forms.OpenFileDialog();
                opendlg.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                opendlg.Filter = "CHM帮助文件|*.chm";
                opendlg.Multiselect = false;
                System.Windows.Forms.DialogResult res= opendlg.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    Process.Start(opendlg.FileName);
                }
            }
        }

        /// <summary>
        /// 利用.net 自带的组件HelpProvider 打开CHM文件
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public static void OpenChm_By_HelpProvider(System.Windows.Forms.Control sender, string strFilePath,string strKeyWord)
        {
            if (m_helpProvider == null)
            {
                m_helpProvider = new System.Windows.Forms.HelpProvider();
            }

            if (strKeyWord != "") m_helpProvider.SetHelpKeyword(sender, strKeyWord);

            m_helpProvider.HelpNamespace = strFilePath;
            m_helpProvider.SetShowHelp(sender, true);
            m_helpProvider.SetHelpNavigator(sender, System.Windows.Forms.HelpNavigator.Topic);
            

            
        }

    }
}
