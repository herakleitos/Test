using Chaint.Common.Core;

namespace Chaint.Common.Interface
{
    public interface IEmployeeService
    {
        OperateResult GetUserInfo(Context ctx, string usercode);

        bool ValidateUser(Context ctx, string usercode, string password);
    }
}
