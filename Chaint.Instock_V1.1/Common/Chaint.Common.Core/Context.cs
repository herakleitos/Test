using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chaint.Common.Core.Const;
namespace Chaint.Common.Core
{
    [Serializable]
    public class Context
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string AppConfigFilePath { get; set; }
        public string DevicesConfigFilePath { get; set; }
        public string Section { get; set; }

        public string CompanyCode { get; set; }

        private Dictionary<string, object> _options;
        public Context()
        {
            _options = new Dictionary<string, object>();
        }
        public Context(string userId, string passWord, string appConfigFilePath, string section)
        {
            UserID = userId;
            PassWord = passWord;
            AppConfigFilePath = appConfigFilePath;
            Section = section;
            _options = new Dictionary<string, object>();
        }
        public object GetOption(string name)
        {
            object value;
            if (_options.TryGetValue(name, out value))
            {
                return value;
            }
            return null;
        }
        public void AddOption(string name,object value)
        {
            if (_options.Keys.Contains(name)) return;
            _options.Add(name, value);
        }
        public void RemoveOption(string name)
        {
            if (_options.Keys.Contains(name))
            {
                _options.Remove(name);
            }
        }
    }
}
