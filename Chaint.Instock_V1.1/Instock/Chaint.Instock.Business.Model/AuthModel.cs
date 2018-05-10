using Chaint.Instock.Core;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Entity;
using Chaint.Common.Core;
using Chaint.Common.Entity;

namespace Chaint.Instock.Business.Model
{
    public class AuthModel : AbstractBillModel
    {
        public AuthModel(Context ctx) : base(ctx)
        {
            BillEntry entry = new BillEntry();
            entry.Name = Const_Auth.Main_Form;
            entry.TableName = Const_Auth.Entry_TableName;
            entry.PrimaryKey = Const_Auth.Entry_Field_FEntryId;

            entry.AddField(new Field(Const_Auth.Entry_Field_FCheck,
                Const_Auth.Entry_Column_FCheck, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_Auth.Entry_Field_FEntryId,
                Const_Auth.Entry_Column_FEntryId, Enums_FieldType.String));
            entry.AddField(new Field(Const_Auth.Entry_Field_FUserID,
                Const_Auth.Entry_Column_FUserID, Enums_FieldType.String));
            entry.AddField(new Field(Const_Auth.Entry_Field_FFormId,
                Const_Auth.Entry_Column_FFormId, Enums_FieldType.String));
            entry.AddField(new Field(Const_Auth.Entry_Field_FFormName,
                Const_Auth.Entry_Column_FFormName, Enums_FieldType.String));
            this.Add(entry);
        }
    }
}
