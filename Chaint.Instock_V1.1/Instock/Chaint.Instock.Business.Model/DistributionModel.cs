using Chaint.Instock.Core;
using Chaint.Common.Core;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Entity;
using Chaint.Common.Entity;

namespace Chaint.Instock.Business.Model
{
    public class DistributionModel : AbstractBillModel
    {
        public DistributionModel(Context ctx) : base(ctx)
        {
            BillEntry entry = new BillEntry();
            entry.TableName = Const_Distribution.Entry_TableName;
            entry.Name = Const_Distribution.Entry_Name;
            entry.PrimaryKey = Const_Distribution.Entry_Field_FID;
            entry.CheckField = Const_Distribution.Entry_Column_FCheck;

            entry.AddField(new Field(Const_Distribution.Entry_Field_FID,
              Const_Distribution.Entry_Column_FID, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FBarCode,
  Const_Distribution.Entry_Column_FBarCode, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FProduct,
              Const_Distribution.Entry_Column_FProduct, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FStockAreaPlan,
              Const_Distribution.Entry_Column_FStockAreaPlan, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FStock,
          Const_Distribution.Entry_Column_FStock, Enums_FieldType.String));
                    entry.AddField(new Field(Const_Distribution.Entry_Field_FStockArea,
          Const_Distribution.Entry_Column_FStockArea, Enums_FieldType.String));
                    entry.AddField(new Field(Const_Distribution.Entry_Field_FAmount,
          Const_Distribution.Entry_Column_FAmount, Enums_FieldType.Int32));
                    entry.AddField(new Field(Const_Distribution.Entry_Field_FStatus,
         Const_Distribution.Entry_Column_FStatus, Enums_FieldType.Int32));
                    entry.AddField(new Field(Const_Distribution.Entry_Field_FDate,
         Const_Distribution.Entry_Column_FDate, Enums_FieldType.DateTime));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FOperator,
Const_Distribution.Entry_Column_FOperator, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FPaperType,
Const_Distribution.Entry_Column_FPaperType, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FSpecification,
    Const_Distribution.Entry_Column_FSpecification, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FPaperGrade,
    Const_Distribution.Entry_Column_FPaperGrade, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FCertification,
    Const_Distribution.Entry_Column_FCertification, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FTransportType,
    Const_Distribution.Entry_Column_FTransportType, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FCoreDiameterOrReam,
    Const_Distribution.Entry_Column_FCoreDiameterOrReam, Enums_FieldType.Decimal));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FDiameterOrSlides,
    Const_Distribution.Entry_Column_FDiameterOrSlides, Enums_FieldType.Decimal));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FLength,
    Const_Distribution.Entry_Column_FLength, Enums_FieldType.Decimal));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FTrademarkStyle,
    Const_Distribution.Entry_Column_FTrademarkStyle, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FSpecCustName,
    Const_Distribution.Entry_Column_FSpecCustName, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FSpecProdName,
    Const_Distribution.Entry_Column_FSpecProdName, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FPackType,
    Const_Distribution.Entry_Column_FPackType, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FDeliverDate,
    Const_Distribution.Entry_Column_FDeliverDate, Enums_FieldType.DateTime));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FWeightMode,
    Const_Distribution.Entry_Column_FWeightMode, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FColor,
    Const_Distribution.Entry_Column_FColor, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FPoNumber,
    Const_Distribution.Entry_Column_FPoNumber, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FMemo,
    Const_Distribution.Entry_Column_FMemo, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FCustomer,
Const_Distribution.Entry_Column_FCustomer, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FStockDate,
Const_Distribution.Entry_Column_FStockDate, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FWeight,
Const_Distribution.Entry_Column_FWeight, Enums_FieldType.String));
            entry.AddField(new Field(Const_Distribution.Entry_Field_FProductInfo,
Const_Distribution.Entry_Column_FProductInfo, Enums_FieldType.String));
            this.Add(entry);
        }
    }
}
