
using Chaint.Common.Core.EventArgs;
namespace Chaint.Common.Interface.PlugIn
{
    public interface IPlugIn
    {
        void ButtonClick(ButtonClickEventArgs e);

        void OnLoad();

        void OnClose();

        void OnDispose();
        void DataReceived(DataReceivedEventArgs e);

        void DataChanged(DataChangedEventArgs e);

        void BeforeUpdateValue(DataChangingEventArgs e);

        void AfterCreateNewEntryRow(AfterCreateNewEntryRowEventArgs e);

        void AfterDeleteEntryRow(AfterDeleteEntryRowEventArgs e);
        void BeforeDeleteEntryRow(BeforeDeleteEntryRowEventArgs e);

        void EntryRowClick(EntryRowClickEventArgs e);

        /// <summary>
        /// 自定义表体的单元格
        /// </summary>
        void CustomEntryRowCell(CustomEntryRowCellEventArgs e);
    }
}
