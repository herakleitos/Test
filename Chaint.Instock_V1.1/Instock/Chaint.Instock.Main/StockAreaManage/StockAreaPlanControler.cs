using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
namespace Chaint.Instock.Business
{
    public class StockAreaPlanControler:AbstractControler
    {
        public StockAreaPlanControler(StockAreaPlanView view) : base(view)
        {
            this.AddPluIn(new StockAreaPlanPlugIn(view));
        }
    }
}
