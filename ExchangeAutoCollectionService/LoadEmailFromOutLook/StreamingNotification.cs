using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Exchange.WebServices.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace LoadEmailFromOutLook
{
    public class StreamingNotification
    {
        private ExchangeService exchangeService { get; }
        int index = 0;
        private BlockingCollection<int> blockCol = new BlockingCollection<int>();

        public StreamingNotification(Account account)
        {
            this.exchangeService = new ExchangeService(account.Version);
            this.exchangeService.Credentials =
                new WebCredentials(account.EmailAddress, account.Password);
            this.exchangeService.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            this.exchangeService.TraceEnabled = false;
            this.exchangeService.KeepAlive = true;
            //System.Threading.Tasks.Task.Factory.StartNew(()=>
            //{
            //    foreach (int i in blockCol.GetConsumingEnumerable())
            //    {
            //        ReceiveEmailFromServer(account);
            //    }
            //});
        }

        public void SetStreamingNotifications()
        {
            StreamingSubscriptionConnection connection = new StreamingSubscriptionConnection(exchangeService, 1);
            SubscribeNotification(exchangeService, connection);
            connection.Open();
        }

        public void OnDisconnect(object sender, SubscriptionErrorEventArgs args)
        {
            // Cast the sender as a StreamingSubscriptionConnection object.           
            StreamingSubscriptionConnection connection = (StreamingSubscriptionConnection)sender;
            // Ask the user if they want to reconnect or close the subscription. 
            if (!connection.IsOpen)
            {
                try
                {
                    //if (connection.CurrentSubscriptions.ToArray().Length < 3)
                    //{
                    //    SubscribeNotification(exchangeService, connection);
                    //}
                    connection.Open();
                }
                catch (Exception ex)
                {
   
                }
            }
        }

        public void OnEvent(object sender, NotificationEventArgs args)
        {
            StreamingSubscription subscription = args.Subscription;
            index++;
            blockCol.Add(index);
            //// Loop through all item-related events. 
            //foreach (NotificationEvent notification in args.Events)
            //{

            //    switch (notification.EventType)
            //    {
            //        case EventType.NewMail:
            //            LoggerHelper.Logger.Info("\n-------------Mail created:-------------");
            //            break;
            //        case EventType.Created:
            //            LoggerHelper.Logger.Info("\n-------------Item or folder created:-------------");
            //            break;
            //        case EventType.Deleted:
            //            LoggerHelper.Logger.Info("\n-------------Item or folder deleted:-------------");
            //            break;
            //    }
            //    // Display the notification identifier. 
            //    if (notification is ItemEvent)
            //    {
            //        // The NotificationEvent for an email message is an ItemEvent. 
            //        ItemEvent itemEvent = (ItemEvent)notification;
            //        LoggerHelper.Logger.Info("\nItemId: " + itemEvent.ItemId.UniqueId);
            //    }
            //    else
            //    {
            //        // The NotificationEvent for a folder is a FolderEvent. 
            //        FolderEvent folderEvent = (FolderEvent)notification;
            //        LoggerHelper.Logger.Info("\nFolderId: " + folderEvent.FolderId.UniqueId);
            //    }
            //}
            //System.Timers.Timer timer = new System.Timers.Timer(5 * 1000 * new Random().Next(1, 10));
            //timer.Elapsed += delegate (object send, System.Timers.ElapsedEventArgs e)
            //{
            //    ReceiveEmailFromServer(exchangeService);
            //};
            //timer.Enabled = true;
            //ReceiveEmailFromServer(exchangeService);
           // AsyncReceiveEmailFromServer();
        }

        public void OnError(object sender, SubscriptionErrorEventArgs args)
        {
            // Handle error conditions. 
            Exception e = args.Exception;
        }

        private void SubscribeNotification(ExchangeService exchangeService, StreamingSubscriptionConnection connection)
        {
            StreamingSubscription streamingsubscription = exchangeService.SubscribeToStreamingNotifications(
                new FolderId[] { WellKnownFolderName.Inbox },
                EventType.NewMail,
                EventType.Created,
                EventType.Deleted);
            connection.AddSubscription(streamingsubscription);
            // Delegate event handlers. 
            connection.OnNotificationEvent +=
                new StreamingSubscriptionConnection.NotificationEventDelegate(OnEvent);
            connection.OnSubscriptionError +=
                new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnError);
            connection.OnDisconnect +=
                new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnDisconnect);
        }

        private void ReceiveEmailFromServer(Account account)
        {
            ExchangeService service = new ExchangeService(account.Version);
            service.Credentials =
                new WebCredentials(account.EmailAddress, account.Password);
            service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            service.TraceEnabled = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("New Email has received at {0}\n\r", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                LogInfo(sb.ToString());
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
        }

        private void DownLoadEmail(string emailSubject, string emailContent, string account)
        {
            try
            {
                var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailOutput");
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

        private void LogInfo(string record)
        {
            try
            {
                var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogInfo");
                if (!Directory.Exists(basePath))
                { 
                    Directory.CreateDirectory(basePath);
                }
                using (FileStream fs = new FileStream($"{basePath}\\{Regex.Replace("Log", "[\\/:*\\?\\”“\\<>|,]", "")}.txt",
                    FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    fs.Position = fs.Length;
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(record);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
            catch
            {
            }
        }
    }
}
