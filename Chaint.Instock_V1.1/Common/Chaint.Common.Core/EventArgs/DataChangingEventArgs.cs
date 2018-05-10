
namespace Chaint.Common.Core.EventArgs
{
    public class DataChangingEventArgs:BaseEventArgs
    {
        public object NewValue { get; set; }
        public object OldValue { get; set; }
    }
}
