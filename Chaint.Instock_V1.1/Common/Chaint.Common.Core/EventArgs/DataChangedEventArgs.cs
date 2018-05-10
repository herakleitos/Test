
namespace Chaint.Common.Core.EventArgs
{
    public class DataChangedEventArgs:BaseEventArgs
    {
        public object Value { get; set; }

        public int Row { get; set; }
    }
}
