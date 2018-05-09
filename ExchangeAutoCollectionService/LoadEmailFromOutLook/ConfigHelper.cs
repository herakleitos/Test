using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace LoadEmailFromOutLook
{
    public class Account
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public ExchangeVersion Version { get { return ExchangeVersion.Exchange2010_SP2; } }
    }

    public interface IUserData
    {
        Uri AutodiscoverUrl { get; set; }
        IList<Account> Accounts { get; }
    }

    public class ConfigHelper : IUserData
    {
        public Uri AutodiscoverUrl { get; set; }

        public IList<Account> Accounts
        {
            get
            {
                var accounts = new List<Account>();
                var EmailList = ConfigurationManager.AppSettings["EmailList"];
                var emailArray = EmailList.Split(';');
                foreach (var email in emailArray)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var entity = email.Split(':');
                        accounts.Add(new Account { EmailAddress = entity[0], Password = entity[1] });
                    }
                }

                return accounts;
            }
        }
    }

}
