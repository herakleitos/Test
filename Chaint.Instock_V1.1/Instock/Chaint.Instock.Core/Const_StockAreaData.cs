
namespace Chaint.Instock.Core
{
    public class Const_StockAreaData
    {
        public const string Base_Form = "form";
        #region head
        public const string Head_Name = "StockAreaData";
        public const string Head_TableName = "T_AutoScan_StockAreaData";
        public const string Head_Menu_Save = "btSave";
        public const string Head_Menu_Update = "btUpdate";
        public const string Head_Menu_New = "btNew";
        public const string Head_Menu_Refresh = "btRefresh";
        public const string Head_Menu_Query = "btQuery";
        public const string Head_Menu_PrintGroup = "bsPrint";
        public const string Head_Menu_Print = "btPrint";
        public const string Head_Menu_Export = "btExport";
        public const string Head_Menu_PrintPreview = "btPrintPreview";
        public const string Head_Menu_PrintTemplet = "btPrintTemplet";

        public const string Head_Field_FBeginDate = "FBeginDate";
        public const string Head_Field_FEndDate = "FEndDate";
        public const string Head_Field_FHStock = "FHStock";
        public const string Head_Field_FHStockArea = "FHStockArea";
        public const string Head_Field_FHProduct = "FHProduct";
        public const string Head_Field_FStatus = "FStatus";
        public const string Head_Field_FHSpecification = "FHSpecification";
        public const string Head_Field_FHPapertype = "FHPapertype";
        #endregion
        #region entry
        public const string Entry_Name = "StockAreaDataEntry";
        public const string Entry_TableName = "T_AutoScan_StockAreaDataEntry";

        public const string Entry_Control_FCheckAll = "FCheckAll";

        public const string Entry_Menu_DeleteEntry = "bt_DeleteEntry";
        public const string Entry_Menu_NewEntry = "bt_NewEntry";
        public const string Entry_Menu_CopyEntry = "bt_CopyEntry";
        public const string Entry_Menu_Complete = "bt_Complete";
        public const string Head_Container = "lc_Head";
        public const string Entry_Control = "gc_Data";
        public const string Entry_Entry = "gv_Entry";
        public const string Entry_Control_FStockAreaPlan = "slu_StockAreaPlan";
        public const string Entry_Control_FProduct = "slu_Product";
        public const string Entry_Control_Status = "slu_Status";
        public const string Entry_Control_Amount = "cab_Amount";
        public const string Entry_Control_PlanAmount = "cab_PlanAmount";

        public const string Entry_Control_Stock = "slu_Stock";
        public const string Entry_Control_StockArea = "slu_StockArea";

        public const string Entry_Field_FName = "FName";
        public const string Entry_Field_FEntryId = "FEntryID";
        public const string Entry_Field_FProduct = "FProduct";
        public const string Entry_Field_FStock = "FStock";
        public const string Entry_Field_FStockArea = "FStockArea";
        public const string Entry_Field_FStockAreaPlan = "FStockAreaPlan";
        public const string Entry_Field_FAmount = "FAmount";
        public const string Entry_Field_FPlanAmount = "FPlanAmount";
        public const string Entry_Field_FStatus = "FStatus";
        public const string Entry_Field_FDate = "FDate";

        public const string Entry_Field_FPaperType = "FPaperType";
        public const string Entry_Field_FSpecification = "FSpecification";
        public const string Entry_Field_FCoreDiameterOrReam = "FCoreDiameterOrReam";
        public const string Entry_Field_FLength = "FLength";
        public const string Entry_Field_FDiameterOrSlides = "FDiameterOrSlides";
        public const string Entry_Field_FTransportType = "FTransportType";
        public const string Entry_Field_FWeightMode = "FWeightMode";
        public const string Entry_Field_FMemo = "FMemo";
        public const string Entry_Field_FPoNumber = "FPoNumber";
        public const string Entry_Field_FCertification = "FCertification";
        public const string Entry_Field_FTrademarkStyle = "FTrademarkStyle";
        public const string Entry_Field_FPackType = "FPackType";
        public const string Entry_Field_FSpecCustName = "FSpecCustName";
        public const string Entry_Field_FPaperGrade = "FPaperGrade";
        public const string Entry_Field_FSpecProdName = "FSpecProdName";
        public const string Entry_Field_FDeliverDate = "FDeliverDate";
        public const string Entry_Field_FColor = "FColor";
        public const string Entry_Field_FCustomer = "FCustomer";
        public const string Entry_Field_FStockDate = "FStockDate";
        public const string Entry_Field_FSpCustomer = "FSpCustomer";

        public const string Entry_Field_FInStockArea = "FInStockArea";
        public const string Entry_Field_FOutAmount = "FOutAmount";
        public const string Entry_Control_FInStockArea = "slu_InStockArea";
        public const string Entry_Field_FWeight = "FWeight";
        public const string Entry_Field_FOutWeight = "FOutWeight";


        public const string Entry_Field_FTotalCapacity = "FTotalCapacity";
        public const string Entry_Field_FUsedCapacity = "FUsedCapacity";


        public const string Entry_Column_FName = "FNAME";
        public const string Entry_Column_FProduct = "FPRODUCT";
        public const string Entry_Column_FStock = "FSTOCK";
        public const string Entry_Column_FStockArea = "FSTOCKAREA";
        public const string Entry_Column_FPlanAmount = "FPLANAMOUNT";

        public const string Entry_Column_FCheck = "FCHECK";
        public const string Entry_Column_FEntryId = "FENTRYID";
        public const string Entry_Column_FAmount = "FAMOUNT";
        public const string Entry_Column_FStockAreaPlan = "FSTOCKAREAPLAN";
        public const string Entry_Column_FStatus = "FSTATUS";
        public const string Entry_Column_FDate = "FDATE";
        public const string Entry_Column_FOutAmount = "FOUTAMOUNT";
        public const string Entry_Column_FWeight = "FWEIGHT";
        public const string Entry_Column_FOutWeight = "FOUTWEIGHT";
        public const string Entry_Column_FTotalCapacity = "FTOTALCAPACITY";
        public const string Entry_Column_FUsedCapacity = "FUSEDCAPACITY";

        public const string Entry_Column_FPaperType = "FPAPERTYPE";
        public const string Entry_Column_FCoreDiameterOrReam = "FCOREDIAMETERORREAM";
        public const string Entry_Column_FDiameterOrSlides = "FDIAMETERORSLIDES";
        public const string Entry_Column_FSpecification = "FSPECIFICATION";
        public const string Entry_Column_FLength = "FLENGTH";
        public const string Entry_Column_FTransportType = "FTRANSPORTTYPE";
        public const string Entry_Column_FWeightMode = "FWEIGHTMODE";
        public const string Entry_Column_FMemo = "FMEMO";
        public const string Entry_Column_FPoNumber = "FPONUMBER";
        public const string Entry_Column_FCertification = "FCERTIFICATION";
        public const string Entry_Column_FTrademarkStyle = "FTRADEMARKSTYLE";
        public const string Entry_Column_FPackType = "FPACKTYPE";
        public const string Entry_Column_FSpecCustName = "FSPECCUSTNAME";
        public const string Entry_Column_FPaperGrade = "FPAPERGRADE";
        public const string Entry_Column_FSpecProdName = "FSPECPRODNAME";
        public const string Entry_Column_FDeliverDate = "FDELIVERDATE";
        public const string Entry_Column_FColor = "FCOLOR";
        public const string Entry_Column_FCustomer = "FCUSTOMER";
        public const string Entry_Column_FStockDate = "FSTOCKDATE";
        public const string Entry_Column_FInStockArea = "FINSTOCKAREA";
        public const string Entry_Column_FSpCustomer = "FSPCUSTOMER";

        public const string Entry_Control_FPaperType = "slu_PaperType";
        public const string Entry_Control_FCoreDiameterOrReam = "cb_CoreDiameterOrReam";
        public const string Entry_Control_FDiameterOrSlides = "cb_DiameterOrSlides";
        public const string Entry_Control_FSpecification = "cb_Specification";
        public const string Entry_Control_FLength = "cb_Length";
        public const string Entry_Control_FTransportType = "slu_TransportType";
        public const string Entry_Control_FWeightMode = "slu_WeightMode";
        public const string Entry_Control_FCertification = "slu_Certification";
        public const string Entry_Control_FTrademarkStyle = "slu_TrademarkStyle";
        public const string Entry_Control_FPackType = "slu_PackType";
        public const string Entry_Control_FSpecCustName = "slu_SpecCustName";
        public const string Entry_Control_FPaperGrade = "slu_PaperGrade";
        public const string Entry_Control_FSpecProdName = "slu_SpecProdName";
        public const string Entry_Control_FDeliverDate = "date_DeliverDate";
        public const string Entry_Control_FColor = "slu_Color";
        public const string Entry_Control_SpCustomer = "slu_SpCustomer";
        #endregion
    }
}
