
namespace Chaint.Common.Core.EventArgs
{
    public class BeforeDeleteEntryRowEventArgs:BaseEventArgs
    {
        public int Row { get; set; }
        public string SeqName { get; set; }
    }
}
