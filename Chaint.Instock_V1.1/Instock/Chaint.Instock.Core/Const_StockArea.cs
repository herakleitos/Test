
namespace Chaint.Instock.Core
{
    public class Const_StockArea
    {
        public const string Base_Form = "form";
        #region head
        public const string Head_Name = "StockArea";
        public const string Head_TableName = "T_AUTOSCAN_STOCKAREA";
        public const string Head_Menu_Save = "btSave";
        public const string Head_Menu_Delete = "btDelete";
        public const string Head_Menu_New = "btNew";
        public const string Head_Menu_List = "btList";

        public const string Head_Field_FID = "FID";
        public const string Head_Field_FStock = "FStock";
        public const string Head_Field_FMemo = "FMemo";
        public const string Head_Field_FCreateDate = "FCreateDate";
        public const string Head_Field_FModifyDate = "FModifyDate";

        public const string Head_Column_FID = "FID";
        public const string Head_Column_FStock = "FSTOCK";
        public const string Head_Column_FMemo = "FMEMO";
        public const string Head_Column_FCreateDate = "FCREATEDATE";
        public const string Head_Column_FModifyDate = "FMODIFYDATE";
        #endregion
        #region entry
        public const string Entry_Name = "StockAreaEntry";
        public const string Entry_TableName = "T_AUTOSCAN_STOCKAREAENTRY";

        public const string Entry_Menu_DeleteEntry = "bt_DeleteEntry";
        public const string Entry_Menu_NewEntry = "bt_NewEntry";
        public const string Entry_Control = "gc_Data";
        public const string Entry_Entry = "gv_Entry";

        public const string Entry_Field_FID = "FHeadID";
        public const string Entry_Field_FSeq = "FSeq";
        public const string Entry_Field_FEntryId = "FEntryID";
        public const string Entry_Field_FStockAreaNumber = "FStockAreaNumber";
        public const string Entry_Field_FStockAreaName = "FStockAreaName";
        public const string Entry_Field_FLocation = "FLocation";
        public const string Entry_Field_FTotalCapacity = "FTotalCapacity";
        public const string Entry_Field_FMemo = "FMemo";

        public const string Entry_Column_FID = "FHEADID";
        public const string Entry_Column_FSeq = "FSEQ";
        public const string Entry_Column_FEntryId = "FENTRYID";
        public const string Entry_Column_FStockAreaNumber = "FSTOCKAREANUMBER";
        public const string Entry_Column_FStockAreaName = "FSTOCKAREANAME";
        public const string Entry_Column_FTotalCapacity = "FTOTALCAPACITY";
        public const string Entry_Column_FLocation = "FLOCATION";
        public const string Entry_Column_FMemo = "FMEMO";
        #endregion
    }
}
