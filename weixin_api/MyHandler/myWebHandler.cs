using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Security;

namespace MyHandler
{
    public class myWebHandler : IHttpHandler
    {
        private string Token = "weixinTest";
        public void ProcessRequest(HttpContext param_context)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["echoStr"]))
            {
                string echoStr = HttpContext.Current.Request.QueryString["echoStr"].ToString();
                if (CheckSignature())
                {
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        HttpContext.Current.Response.Write(echoStr);
                        HttpContext.Current.Response.End();
                    }
                }
            }
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                    Handle(postString);
                }
            }
        }
        /// <summary>
        /// 处理信息并应答
        /// </summary>
        private void Handle(string postStr)
        {
            messageHelp help = new messageHelp();
            string responseContent = help.ReturnMessage(postStr);

            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Write(responseContent);
        }
        private bool CheckSignature()
        {
            string signature = HttpContext.Current.Request.QueryString["signature"].ToString();
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"].ToString();
            string nonce = HttpContext.Current.Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
