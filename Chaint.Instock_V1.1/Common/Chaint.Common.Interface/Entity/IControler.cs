
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Interface.PlugIn;
namespace Chaint.Common.Interface.Controler
{
    public interface IControler
    {
        void AddPluIn(IPlugIn plugIn);
        void Invoke(string methodName, BaseEventArgs args);
    }
}
