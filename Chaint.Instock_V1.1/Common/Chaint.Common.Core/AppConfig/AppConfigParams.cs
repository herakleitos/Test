using System.IO;
using Chaint.Common.Core.Const;
namespace Chaint.Common.Core.AppConfig
{
    public class AppConfigParams
    {
        private string m_FilePath = "";
        /// <summary>
        /// 配置参数文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                if (m_FilePath != "")
                {
                    if (!File.Exists(m_FilePath)) File.Create(m_FilePath);
                    return m_FilePath;
                }
                else
                {
                    return Const_AppConfigFilePath.Ini;
                }
            }
            set
            {
                m_FilePath = value;
            }
        }
        public AppConfigParams(string strFilePath)
        {
            m_FilePath = strFilePath;
        }
    }
}
