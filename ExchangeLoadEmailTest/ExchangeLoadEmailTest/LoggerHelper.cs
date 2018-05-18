using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ExchangeAutoCollectionService
{
    public static class LoggerHelper
    {
        public static Logger Logger { get; }

        static LoggerHelper()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }
    }
}
