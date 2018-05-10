using System;
using System.Collections.Generic;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Interface.Business;
namespace Chaint.Common.Interface
{
    public static class ServiceFactory
    {
        private static Dictionary<object, string> _map = new Dictionary<object, string>();
        private static Dictionary<object, object> _cache = new Dictionary<object, object>();
        public static T GetService<T>(Context ctx)
        {
            if (_map.IsEmpty())
            {
                InitMap();
            }
            object cachedObj;
            if (_cache.TryGetValue(typeof(T), out cachedObj))
            {
                return (T)cachedObj;
            }
            string path = string.Empty;
            if (!_map.TryGetValue(typeof(T), out path)) return default(T);
            try
            {
                Type type = Type.GetType(path);
                object obj = Activator.CreateInstance(type);
                _cache.Add(typeof(T),obj);
                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }
        private static void InitMap()
        {
            _map.Add(typeof(IEmployeeService), "Chaint.Common.Service.EmployeeService,Chaint.Common.Service");
            _map.Add(typeof(IDBAccessService), "Chaint.Common.Service.DBAccessService,Chaint.Common.Service");
            //business
            _map.Add(typeof(IStockAreaDataService), "Chaint.Instock.Service.StockAreaDataService,Chaint.Instock.Service");
            _map.Add(typeof(IStockAreaPlanService), "Chaint.Instock.Service.StockAreaPlanService,Chaint.Instock.Service");
            _map.Add(typeof(IStockAreaService), "Chaint.Instock.Service.StockAreaService,Chaint.Instock.Service");
            _map.Add(typeof(IStockInAutoScan), "Chaint.Instock.Service.StockInAutoScanService,Chaint.Instock.Service");
        }
    }
}
