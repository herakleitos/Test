
namespace Chaint.Instock.Core
{
    public class Const_StockAreaDataQuery
    {
        public const string Base_Form = "form";
        #region head
        public const string Head_Menu_Query = "btQuery";

        public const string Head_Menu_Export = "btExport";

        public const string Head_Field_FBeginDate = "FBeginDate";
        public const string Head_Field_FEndDate = "FEndDate";
        public const string Head_Field_FStock = "FHStock";
        public const string Head_Field_FProduct = "FHProduct";
        public const string Head_Field_FStockArea = "FHStockArea";
        public const string Head_Field_FPaperType = "FHPaperType";
        public const string Head_Field_FSpecification = "FHSpecification";
        public const string Head_Field_FCoreDiameterOrReam = "FHCoreDiameterOrReam";
        public const string Head_Field_FLength = "FHLength";
        public const string Head_Field_FDiameterOrSlides = "FHDiameterOrSlides";
        public const string Head_Field_FTransportType = "FHTransportType";
        public const string Head_Field_FWeightMode = "FHWeightMode";
        public const string Head_Field_FMemo = "FHMemo";
        public const string Head_Field_FPoNumber = "FHPoNumber";
        public const string Head_Field_FCertification = "FHCertification";
        public const string Head_Field_FTrademarkStyle = "FHTrademarkStyle";
        public const string Head_Field_FPackType = "FHPackType";
        public const string Head_Field_FSpecCustName = "FHSpecCustName";
        public const string Head_Field_FPaperGrade = "FHPaperGrade";
        public const string Head_Field_FSpecProdName = "FHSpecProdName";
        public const string Head_Field_FDeliverDate = "FHDeliverDate";
        public const string Head_Field_FColor = "FHColor";
        public const string Head_Field_FSpCustomer = "FHSpCustomer";

        public const string Head_Container_FSpCustomer = "";
        #endregion
        #region entry
        public const string Entry_Name = "StockAreaDataEntry";
        public const string Entry_TableName = "T_AutoScan_StockAreaDataEntry";

        public const string Head_Container = "lc_Head";
        public const string Entry_Control = "gc_Data";
        public const string Entry_Entry = "gv_Entry";
        public const string Entry_Control_FStockAreaPlan = "slu_StockAreaPlan";
        public const string Entry_Control_FProduct = "slu_Product";
        public const string Entry_Control_Status = "slu_Status";
        public const string Entry_Control_Amount = "cab_Amount";
        public const string Entry_Control_PlanAmount = "cab_PlanAmount";

        public const string Entry_Control_Stock= "slu_Stock";
        public const string Entry_Control_StockArea = "slu_StockArea";

        public const string Entry_Field_FName = "FName";
        public const string Entry_Field_FEntryId = "FEntryID";
        public const string Entry_Field_FProduct = "FProduct";
        public const string Entry_Field_FStock = "FStock";
        public const string Entry_Field_FStockArea = "FStockArea";
        public const string Entry_Field_FStockAreaPlan= "FStockAreaPlan";
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

        public const string Entry_Field_FWeight = "FWeight";
        public const string Entry_Field_FOutWeight = "FOutWeight";


        public const string Entry_Field_FTotalCapacity = "FTotalCapacity";
        public const string Entry_Field_FUsedCapacity = "FUsedCapacity";


        public const string Entry_Column_FName = "FNAME";
        public const string Entry_Column_FEntryId = "FENTRYID";
        public const string Entry_Column_FProduct = "FPRODUCT";
        public const string Entry_Column_FStock = "FSTOCK";
        public const string Entry_Column_FStockArea = "FSTOCKAREA";
        public const string Entry_Column_FStockAreaPlan = "FSTOCKAREAPLAN";
        public const string Entry_Column_FAmount = "FAMOUNT";
        public const string Entry_Column_FPlanAmount = "FPLANAMOUNT";
        public const string Entry_Column_FStatus = "FSTATUS";
        public const string Entry_Column_FDate = "FDATE";

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
        public const string Entry_Column_FOutAmount = "FOUTAMOUNT";
        public const string Entry_Column_FSpCustomer = "FSPCUSTOMER";

        public const string Entry_Column_FWeight = "FWEIGHT";
        public const string Entry_Column_FOutWeight = "FOUTWEIGHT";

        public const string Entry_Column_FTotalCapacity = "FTOTALCAPACITY";
        public const string Entry_Column_FUsedCapacity = "FUSEDCAPACITY";



        public const string Entry_Container_FProduct = "lo_FProduct";
        public const string Entry_Container_FStock = "lo_FStock";
        public const string Entry_Container_FStockArea = "lo_FStockArea";
        public const string Entry_Container_FPaperType = "lo_FPaperType";
        public const string Entry_Container_FPaperGrade = "lo_FPaperGrade";
        public const string Entry_Container_FBeginDate = "lo_FBeginDate";
        public const string Entry_Container_FEndDate = "lo_FEndDate";
        public const string Entry_Container_FMemo = "lo_FMemo";
        public const string Entry_Container_FTransportType = "lo_FTransportType";
        public const string Entry_Container_FCertification = "lo_FCertification";
        public const string Entry_Container_FSpecification = "lo_FSpecification";
        public const string Entry_Container_FCoreDiameterOrReam = "lo_FCoreDiameterOrReam";
        public const string Entry_Container_FDiameterOrSlides = "lo_FDiameterOrSlides";
        public const string Entry_Container_FLength = "lo_FLength";
        public const string Entry_Container_FTrademarkStyle = "lo_FTrademarkStyle";
        public const string Entry_Container_FSpecCustName = "lo_FSpecCustName";
        public const string Entry_Container_FSpecProdName = "lo_FSpecProdName";
        public const string Entry_Container_FPackType = "lo_FPackType";
        public const string Entry_Container_FDeliverDate = "lo_FDeliverDate";
        public const string Entry_Container_FWeightMode = "lo_FWeightMode";
        public const string Entry_Container_FColor = "lo_FColor";
        public const string Entry_Container_FPoNumber = "lo_FPoNumber";
        public const string Entry_Container_FSpCustomer = "lo_FSpCustomer";
        #endregion
    }
}
