using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaint.Common.Core
{
    public class OperationResult
    {
        public List<OperateResult> ResultFun { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public OperationResult()
        {
            ResultFun = new List<OperateResult>();
        }
    }
}
