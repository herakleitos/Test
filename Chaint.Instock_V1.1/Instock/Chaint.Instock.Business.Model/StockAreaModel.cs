
using Chaint.Instock.Core;
using Chaint.Common.Core;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Entity;
using Chaint.Common.Entity;
namespace Chaint.Instock.Business.Model
{
    public class StockAreaModel:AbstractBillModel
    {
        public StockAreaModel(Context ctx) : base(ctx)
        {
            BillHead head = new BillHead();
            head.TableName = Const_StockArea.Head_TableName;
            head.PrimaryKey = Const_StockArea.Head_Field_FID;
            head.Name = Const_StockArea.Head_Name;
            head.AddField(new Field(Const_StockArea.Head_Field_FID,
                Const_StockArea.Head_Column_FID, Enums_FieldType.String));
            head.AddField(new Field(Const_StockArea.Head_Field_FStock,
                Const_StockArea.Head_Column_FStock, Enums_FieldType.String));
            head.AddField(new Field(Const_StockArea.Head_Field_FMemo,
                Const_StockArea.Head_Column_FMemo, Enums_FieldType.String));
            head.AddField(new Field(Const_StockArea.Head_Field_FCreateDate,
                Const_StockArea.Head_Column_FCreateDate, Enums_FieldType.DateTime));
            head.AddField(new Field(Const_StockArea.Head_Field_FModifyDate,
                Const_StockArea.Head_Column_FModifyDate, Enums_FieldType.DateTime));
            this.Add(head);
            BillEntry entry = new BillEntry();
            
            entry.Name = Const_StockArea.Entry_Name;
            entry.TableName = Const_StockArea.Entry_TableName;
            entry.ParentTable = head.TableName;
            entry.ParentPrimaryKey = Const_StockArea.Entry_Field_FID;
            entry.PrimaryKey = Const_StockArea.Entry_Field_FEntryId;
            entry.AddField(new Field(Const_StockArea.Entry_Field_FID,
                Const_StockArea.Entry_Column_FID, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockArea.Entry_Field_FEntryId,
                Const_StockArea.Entry_Column_FEntryId, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockArea.Entry_Field_FSeq,
                Const_StockArea.Entry_Column_FSeq, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_StockArea.Entry_Field_FStockAreaNumber,
                Const_StockArea.Entry_Column_FStockAreaNumber, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockArea.Entry_Field_FStockAreaName,
                Const_StockArea.Entry_Column_FStockAreaName, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockArea.Entry_Field_FLocation,
                Const_StockArea.Entry_Column_FLocation, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockArea.Entry_Field_FTotalCapacity,
                Const_StockArea.Entry_Column_FTotalCapacity, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_StockArea.Entry_Field_FMemo,
                Const_StockArea.Entry_Column_FMemo, Enums_FieldType.String));
            this.Add(entry);
        }
    }
}
