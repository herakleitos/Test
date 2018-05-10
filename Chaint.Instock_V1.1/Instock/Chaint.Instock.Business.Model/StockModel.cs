using Chaint.Instock.Core;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Entity;
using Chaint.Common.Core;
using Chaint.Common.Entity;

namespace Chaint.Instock.Business.Model
{
    public class StockModel : AbstractBillModel
    {
        public StockModel(Context ctx) : base(ctx)
        {
            BillHead head = new BillHead();
            head.TableName = Const_Stock.Head_TableName;
            head.PrimaryKey = Const_Stock.Head_Field_FID;
            head.Name = Const_Stock.Head_Name;
            head.AddField(new Field(Const_Stock.Head_Field_FID,
                Const_Stock.Head_Column_FID, Enums_FieldType.String));

            head.AddField(new Field(Const_Stock.Head_Field_FNumber,
            Const_Stock.Head_Column_FNumber, Enums_FieldType.String));

            head.AddField(new Field(Const_Stock.Head_Field_FName,
            Const_Stock.Head_Column_FName, Enums_FieldType.String));

            head.AddField(new Field(Const_Stock.Head_Field_FLocation,
            Const_Stock.Head_Column_FLocation, Enums_FieldType.String));

            head.AddField(new Field(Const_Stock.Head_Field_FMemo,
            Const_Stock.Head_Column_FMemo, Enums_FieldType.String));

            head.AddField(new Field(Const_Stock.Head_Field_FCreateDate,
            Const_Stock.Head_Column_FCreateDate, Enums_FieldType.DateTime));

            head.AddField(new Field(Const_Stock.Head_Field_FModifyDate,
            Const_Stock.Head_Column_FModifyDate, Enums_FieldType.DateTime));

            this.Add(head);
        }
    }
}
