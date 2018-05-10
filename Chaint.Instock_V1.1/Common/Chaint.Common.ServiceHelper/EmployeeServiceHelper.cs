using System;
using Chaint.Common.Core;
using Chaint.Common.App;
using Chaint.Common.Interface;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.Log;
namespace Chaint.Common.ServiceHelper
{
    public class EmployeeServiceHelper
    {
        public static OperateResult GetUserInfo(Context ctx, string usercode)
        {
            OperateResult result = new OperateResult();
            IEmployeeService service
                = AppServiceContext.GetService<IEmployeeService>(ctx);
            try
            {
                ctx.Section = Const_ConfigSection.WMSSection;
                result = service.GetUserInfo(ctx, usercode);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
            }
            finally
            {
                ctx.Section = Const_ConfigSection.MainSection;
            }
            return result;
        }
        public static bool ValidateUser(Context ctx, string usercode, string password)
        {
            bool result = false;
            IEmployeeService service
                = AppServiceContext.GetService<IEmployeeService>(ctx);
            try
            {
                ctx.Section = Const_ConfigSection.WMSSection;
                result = service.ValidateUser(ctx, usercode, password);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result = false;
            }
            finally
            {
                ctx.Section = Const_ConfigSection.MainSection;
            }
            return result;
        }
    }
}
