using DevExpress.XtraEditors.Repository;
namespace Chaint.Common.Core.EventArgs
{
    public class CustomEntryRowCellEventArgs:BaseEventArgs
    {
        public int Row { get; set; }
        public RepositoryItem Item { get; set; }
    }
}
