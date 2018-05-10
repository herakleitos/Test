using Chaint.Instock.Core;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Entity;
using Chaint.Common.Core;
using Chaint.Common.Entity;
namespace Chaint.Instock.Business.Model
{
    public class StockAreaPlanModel:AbstractBillModel
    {
        public StockAreaPlanModel(Context ctx):base(ctx)
        {
            BillHead head = new BillHead();
            head.TableName = Const_StockAreaPlan.Head_TableName;
            head.PrimaryKey = Const_StockAreaPlan.Head_Field_FID;
            head.Name = Const_StockAreaPlan.Head_Name;
            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FID,
                Const_StockAreaPlan.Head_Column_FID, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FNumber,
            Const_StockAreaPlan.Head_Column_FNumber, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FName,
            Const_StockAreaPlan.Head_Column_FName, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FPaperType,
            Const_StockAreaPlan.Head_Column_FPaperType, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FPaperGrade,
            Const_StockAreaPlan.Head_Column_FPaperGrade, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FCertification,
            Const_StockAreaPlan.Head_Column_FCertification, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FPackType,
            Const_StockAreaPlan.Head_Column_FPackType, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FTransportType,
            Const_StockAreaPlan.Head_Column_FTransportType, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FColor,
            Const_StockAreaPlan.Head_Field_FColor, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FLength,
            Const_StockAreaPlan.Head_Column_FLength, Enums_FieldType.Decimal));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FTrademarkStyle,
            Const_StockAreaPlan.Head_Column_FTrademarkStyle, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FSpecCustName,
            Const_StockAreaPlan.Head_Column_FSpecCustName, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FSpecProdName,
            Const_StockAreaPlan.Head_Column_FSpecProdName, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FWeightMode,
            Const_StockAreaPlan.Head_Column_FWeightMode, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FCreateDate,
            Const_StockAreaPlan.Head_Column_FCreateDate, Enums_FieldType.DateTime));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FModifyDate,
            Const_StockAreaPlan.Head_Column_FModifyDate, Enums_FieldType.DateTime));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FSpecification,
            Const_StockAreaPlan.Head_Column_FSpecification, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FPoNumber,
            Const_StockAreaPlan.Head_Column_FPoNumber, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FMemo,
            Const_StockAreaPlan.Head_Column_FMemo, Enums_FieldType.String));


            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FSpCustomer,
            Const_StockAreaPlan.Head_Column_FSpCustomer, Enums_FieldType.String));

            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FDeliverDate,
            Const_StockAreaPlan.Head_Field_FDeliverDate, Enums_FieldType.DateTime));
            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam,
                    Const_StockAreaPlan.Head_Column_FCoreDiameterOrReam, Enums_FieldType.Decimal));
            head.AddField(new Field(Const_StockAreaPlan.Head_Field_FDiameterOrSlides,
                Const_StockAreaPlan.Head_Column_FDiameterOrSlides, Enums_FieldType.Decimal));
            this.Add(head);
        }
    }
}
