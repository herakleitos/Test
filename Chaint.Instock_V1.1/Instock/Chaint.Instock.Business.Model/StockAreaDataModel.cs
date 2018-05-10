
using Chaint.Instock.Core;
using Chaint.Common.Core;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Entity;
using Chaint.Common.Entity;
namespace Chaint.Instock.Business.Model
{
    public class StockAreaDataModel : AbstractBillModel
    {
        public StockAreaDataModel(Context ctx) : base(ctx)
        {
            BillEntry entry = new BillEntry();
            entry.Name = Const_StockAreaData.Entry_Name;
            entry.TableName = Const_StockAreaData.Entry_TableName;
            entry.PrimaryKey = Const_StockAreaData.Entry_Field_FEntryId;
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FEntryId,
                Const_StockAreaData.Entry_Column_FEntryId, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FDate,
                Const_StockAreaData.Entry_Column_FDate, Enums_FieldType.DateTime));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FProduct,
                Const_StockAreaData.Entry_Column_FProduct, Enums_FieldType.String));

            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FStock,
               Const_StockAreaData.Entry_Column_FStock, Enums_FieldType.String));

            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FStockArea,
               Const_StockAreaData.Entry_Column_FStockArea, Enums_FieldType.String));

            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FStockAreaPlan,
                Const_StockAreaData.Entry_Column_FStockAreaPlan, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FAmount,
                Const_StockAreaData.Entry_Column_FAmount, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FPlanAmount,
                Const_StockAreaData.Entry_Column_FPlanAmount, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FStatus,
                Const_StockAreaData.Entry_Column_FStatus, Enums_FieldType.Int32));

            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FPaperType,
    Const_StockAreaData.Entry_Column_FPaperType, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FSpecification,
    Const_StockAreaData.Entry_Column_FSpecification, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FPaperGrade,
    Const_StockAreaData.Entry_Column_FPaperGrade, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FCertification,
    Const_StockAreaData.Entry_Column_FCertification, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FTransportType,
    Const_StockAreaData.Entry_Column_FTransportType, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FCoreDiameterOrReam,
    Const_StockAreaData.Entry_Column_FCoreDiameterOrReam, Enums_FieldType.Decimal));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FDiameterOrSlides,
    Const_StockAreaData.Entry_Column_FDiameterOrSlides, Enums_FieldType.Decimal));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FLength,
    Const_StockAreaData.Entry_Column_FLength, Enums_FieldType.Decimal));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FTrademarkStyle,
    Const_StockAreaData.Entry_Column_FTrademarkStyle, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FSpecCustName,
    Const_StockAreaData.Entry_Column_FSpecCustName, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FSpecProdName,
    Const_StockAreaData.Entry_Column_FSpecProdName, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FPackType,
    Const_StockAreaData.Entry_Column_FPackType, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FDeliverDate,
    Const_StockAreaData.Entry_Column_FDeliverDate, Enums_FieldType.DateTime));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FWeightMode,
    Const_StockAreaData.Entry_Column_FWeightMode, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FColor,
    Const_StockAreaData.Entry_Column_FColor, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FPoNumber,
    Const_StockAreaData.Entry_Column_FPoNumber, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FMemo,
    Const_StockAreaData.Entry_Column_FMemo, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FSpCustomer,
Const_StockAreaData.Entry_Column_FSpCustomer, Enums_FieldType.String));

            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FName,
Const_StockAreaData.Entry_Column_FName, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FCustomer,
Const_StockAreaData.Entry_Column_FCustomer, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FStockDate,
Const_StockAreaData.Entry_Column_FStockDate, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FInStockArea,
Const_StockAreaData.Entry_Column_FInStockArea, Enums_FieldType.String));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FOutAmount,
Const_StockAreaData.Entry_Column_FOutAmount, Enums_FieldType.Int32));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FOutWeight,
Const_StockAreaData.Entry_Column_FOutWeight, Enums_FieldType.Decimal));
            entry.AddField(new Field(Const_StockAreaData.Entry_Field_FWeight,
Const_StockAreaData.Entry_Column_FWeight, Enums_FieldType.Decimal));
            this.Add(entry);
        }
    }
}
