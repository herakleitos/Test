
namespace Chaint.Common.Core.EventArgs
{
    public class AfterDeleteEntryRowEventArgs:BaseEventArgs
    {
        public int Row { get; set; }
        public string SeqName { get; set; }
    }
}
