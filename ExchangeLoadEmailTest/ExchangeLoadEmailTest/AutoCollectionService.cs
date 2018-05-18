using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeAutoCollectionService
{
    public partial class AutoCollectionService : ServiceBase
    {
        AutoResetEvent stoppingSignal = new AutoResetEvent(false);

        public AutoCollectionService()
        {
            InitializeComponent();
        }

#if DEBUG
        public void DebugRun()
        {
            OnStart(Environment.GetCommandLineArgs());
            stoppingSignal.WaitOne();
        }
#endif

        protected override void OnStart(string[] args)
        {
            LoggerHelper.Logger.Info("start windows service.");
            IList<Account> accounts = new ConfigHelper().Accounts;
            try
            {
                foreach (var account in accounts)
                {
                    //System.Threading.Tasks.Task.Factory.StartNew(() =>
                    //{
                        //ExchangeService service =
                        //Service.ConnectToService(account,
                        //null,
                        //() => LoggerHelper.Logger.Info($"start connecting {account.EmailAddress}..."),
                        //null);
                        //ExchangeService service = new ExchangeService(account.Version);
                        //service.Credentials =
                        //    new WebCredentials(account.EmailAddress, account.Password);
                        //service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
                        //service.TraceEnabled = false;
                        new StreamingNotification(account).SetStreamingNotifications();
                    //});
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected override void OnStop()
        {
            LoggerHelper.Logger.Info("stop windows service.");
#if DEBUG
            stoppingSignal.Set();
#endif
        }
    }
}
