using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaint.Common.Core.Log
{
    public class ErrorInfo
    {
        public ErrorInfo()
        { }
        public ErrorInfo(Exception ex)
        {
            Message = ex.Message;
            Source = ex.Source;
            Stack = ex.StackTrace;
        }
        public string Message { get; set; }

        public string Source { get; set; }

        public string Stack { get; set; }

    }
}
