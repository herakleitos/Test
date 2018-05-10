
using Chaint.Common.Core;

namespace Chaint.Common.Interface.Business
{
    public interface IStockInAutoScan
    {
        OperateResult GetDefaultDisplay(Context ctx, string dispType);

        OperateResult GetFactoryOrg(Context ctx, bool isChoose, bool isLocal);

        OperateResult GetBusinessType(Context ctx, string businessCode);

        OperateResult GetShiftInfo(Context ctx, string shiftName);

    }
}
