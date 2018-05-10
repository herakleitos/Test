using System;
using System.Collections.Generic;
using System.Text;


using System.Diagnostics;
using System.IO;

/*-----------------------------------------------------------------------------------
 * ����: Chaint.IT
 * 
 * ����ʱ��: 2015-09-28
 * 
 * ��������: 
 *      ��ִ���ļ����µİ����ļ�����(CHM)�ĵ��ò�����
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.IO
{
    public class HelpService
    {
        private static System.Windows.Forms.HelpProvider m_helpProvider = null;


        /// <summary>
        /// ���ý��̵ķ�����CHM�ļ�������ÿ�ζ�λ���ļ�����ҳ
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
                opendlg.Filter = "CHM�����ļ�|*.chm";
                opendlg.Multiselect = false;
                System.Windows.Forms.DialogResult res= opendlg.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    Process.Start(opendlg.FileName);
                }
            }
        }

        /// <summary>
        /// ����.net �Դ������HelpProvider ��CHM�ļ�
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
