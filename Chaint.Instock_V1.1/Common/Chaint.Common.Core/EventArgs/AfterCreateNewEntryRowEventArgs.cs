
namespace Chaint.Common.Core.EventArgs
{
    public class AfterCreateNewEntryRowEventArgs:BaseEventArgs
    {
        public int Row { get; set; }
        public string ParentKey { get; set; }
        public string ParentContainer { get; set; }
        public string PrimaryKey { get; set; }
        public string SeqKey { get; set; }
    }
}
