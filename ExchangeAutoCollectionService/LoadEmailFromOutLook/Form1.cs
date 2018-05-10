using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using System.IO;
using System.Text.RegularExpressions;
namespace LoadEmailFromOutLook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            IList<Account> accounts = new ConfigHelper().Accounts;
            try
            {
                foreach (var account in accounts)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        new StreamingNotification(account).SetStreamingNotifications();
                    });

                    ExchangeService service = new ExchangeService(account.Version);
                        service.Credentials =
                            new WebCredentials(account.EmailAddress, account.Password);
                        service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
                        service.TraceEnabled = false;
                        //创建过滤器, 条件为邮件未读.  
                        ItemView view = new ItemView(999);
                        view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, EmailMessageSchema.IsRead);
                        SearchFilter filter = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false);
                        FindItemsResults<Item> findResults = service.FindItems(WellKnownFolderName.Inbox,
                            filter,
                            view);
                        foreach (Item item in findResults.Items)
                        {
                            //每次循环花费时间2到3分钟，时间消耗在EmailMessage.Bind和email.Update上。
                            //需要更快速的方法获取邮件。
                            EmailMessage email = EmailMessage.Bind(service, item.Id);

                            if (!email.IsRead)
                            {
                                //标记为已读  
                                email.IsRead = true;
                                //将对邮件的改动提交到服务器  
                                email.Update(ConflictResolutionMode.AlwaysOverwrite);
                                Object _lock = new Object();
                                lock (_lock)
                                {
                                    DownLoadEmail(email.Subject, email.Body, ((System.Net.NetworkCredential)((Microsoft.Exchange.WebServices.Data.WebCredentials)service.Credentials).Credentials).UserName);
                                this.lbMessage.Items.Insert(0,
                                    string.Format("{0}-下载成功-{1}",
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), email.Subject));
                            }
                            }
                        }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void DownLoadEmail(string emailSubject, string emailContent, string account)
        {
            try
            {
                var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailOutputManual");
                emailSubject = emailSubject + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                System.IO.File.WriteAllText($"{basePath}\\{Regex.Replace(emailSubject.Trim(), "[\\/:*\\?\\”“\\<>|,]", "")}-[{account}].html", emailContent);
            }
            catch (IOException ex)
            {
            }
        }
    }
}
