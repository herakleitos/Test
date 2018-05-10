
using Chaint.Common.Core;
using Chaint.Common.Core.Entity;

namespace Chaint.Common.Interface
{
    public interface IModel
    {
        void Add(BillHead head);
        void Add(BillEntry entry);
        void Add(string tableName, Field field);
        OperationResult Save();
        void SetValue(string fieldName, object value,int row=-1);
        T GetValue<T>(string name, int row = -1);
        BillHead GetBillHead(string headTableName);
        BillEntry GetBillEntry(string entryTableName);
    }
}
