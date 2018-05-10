using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using DataModel;
using System.IO;


namespace CTWH.Common.MSSQL
{
    public partial class MSSQLAccess
    {
        #region PalletDS_Pallet_ProductSelect 
        private static string PalletDS_Pallet_ProductSelect =
            @" Pallet_Product.*,
TypeTable.Type_Desc as Type_Desc,
GradeTable.Grade_Desc as Grade_Desc,
SheetShiftTable.Shift_Desc as SheetShift_Desc,
PalletShiftTable.Shift_Desc as PalletShift_Desc
from Pallet_Product
left join Paper_Type TypeTable on TypeTable.Type = Pallet_Product.Type
left join Paper_Grade GradeTable on GradeTable.Grade = Pallet_Product.Grade
left join Paper_Shift SheetShiftTable on SheetShiftTable.Shift = Pallet_Product.SheetShift
left join Paper_Shift PalletShiftTable on PalletShiftTable.Shift = Pallet_Product.PalletShift";

        #endregion


        #region RollDS_Roll_ProductSelect
        private static string PalletDS_Rolls_ProductSelect =
            @" PalletToRolls.PalletID as PalletID,Roll_Product.*
,JumboShiftTable.Shift_Desc as JumboShift_Desc
,TypeTable.Type_Desc as Type_Desc
,GradeTable.Grade_Desc as Grade_Desc
,ReWinderShiftTable.Shift_Desc as ReWinderShift_Desc
,RollWrapShiftTable.Shift_Desc as RollWrapShift_Desc
,SheetShiftTable.Shift_Desc as SheetShift_Desc
FROM PalletToRolls
inner join Pallet_Product on Pallet_Product.ProductID =PalletToRolls.PalletID 
inner join Roll_Product on Roll_Product.ProductID= PalletToRolls.RollID 
left join Paper_Shift JumboShiftTable on JumboShiftTable.Shift = Roll_Product.JumboShift
left join Paper_Type TypeTable on TypeTable.Type = Roll_Product.Type
left join Paper_Grade GradeTable on GradeTable.Grade = Roll_Product.Grade
left join Paper_Shift ReWinderShiftTable on ReWinderShiftTable.Shift = Roll_Product.ReWinderShift 
left join Paper_Shift RollWrapShiftTable on RollWrapShiftTable.Shift = Roll_Product.RollWrapShift 
left join Paper_Shift SheetShiftTable on SheetShiftTable.Shift = Roll_Product.SheetShift";

        #endregion

        //Pallet_Product  用于打印，显示等，包括纸卷的各种详细信息
        public PalletDS PalletDS_Pallet_ProductQueryByOnlyID(string OnlyID)
        {
            PalletDS ds = new PalletDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@OnlyID", OnlyID)              
                };

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                         "select " + PalletDS_Pallet_ProductSelect + " where OnlyID=@OnlyID",
                          ds,
                          new string[] { "Pallet_Product" },
                          parameter
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "PalletDS_Pallet_ProductQueryByOnlyID读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PalletDS_Pallet_ProductQueryByOnlyID" + ex.ToString()));
                }

            }
            return ds;

        }

        public PalletDS PalletDS_Pallet_ProductQueryByProductID(string ProductID)
        {
            PalletDS ds = new PalletDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@ProductID", ProductID)              
                };

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                         "select " + PalletDS_Pallet_ProductSelect + " where ProductID=@ProductID",
                          ds,
                          new string[] { "Pallet_Product" },
                          parameter
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "PalletDS_Pallet_ProductQueryByProductID读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PalletDS_Pallet_ProductQueryByProductID" + ex.ToString()));
                }

            }
            return ds;

        }

        public PalletDS PalletDS_Pallet_ProductQueryByBarcode(string Barcode)
        {
            if (Barcode.Length > 10)
            {
                return PalletDS_Pallet_ProductQueryByProductID(Barcode);
            }
            else
            {
                return PalletDS_Pallet_ProductQueryByOnlyID(Barcode);
            }
        }


        public PalletDS PalletDS_Pallet_ProductQueryAllByMESTime(DateTime begin, DateTime end)
        {
            PalletDS ds = new PalletDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //WINDER_DATE   MESTime

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select top " + Utils.SelectRowsMax + PalletDS_Pallet_ProductSelect + " where MESTime>='" + begin.ToString() + "' and MESTime<='" + end.ToString() + "'",
                          ds,
                          new string[] { "Pallet_Product" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "PalletDS_Pallet_ProductQueryAllByMESTime读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PalletDS_Pallet_ProductQueryAllByMESTime" + ex.ToString()));
                }

            }
            return ds;
        }


        public PalletDS Pallet_ProductQueryAllForUploadERP()
        {
            PalletDS ds = new PalletDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqltext = "select top 500 " + PalletDS_Pallet_ProductSelect + " where isnull(isuploaderp,0) =0 order by mestime desc;";

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          sqltext,
                          ds,
                          new string[] { "Pallet_Product" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductQueryAllForUploadERP读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductQueryAllForUploadERP Error:" + ex.ToString()));
                }
            }
            return ds;
        }



       
        //Pallet to  roll
        public DataSet PalletToRolls_ProductQueryAllByMESTime(DateTime begin, DateTime end)
        {
            DataSet ds = new DataSet();

        
                
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //WINDER_DATE   MESTime
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select top " + Utils.SelectRowsMax + PalletDS_Pallet_ProductSelect + " where MESTime>='" + begin.ToString() + "' and MESTime<='" + end.ToString() + "';"
                          + "select top " + Utils.SelectRowsMax + PalletDS_Rolls_ProductSelect + " where Pallet_Product.MESTime>='" + begin.ToString() + "' and Pallet_Product.MESTime<='" + end.ToString() + "'",
                          ds,
                          new string[] { "Pallet_Product" ,"Roll_Product"}
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "PalletToRolls_ProductQueryAllByMESTime读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PalletToRolls_ProductQueryAllByMESTime" + ex.ToString()));
                }

            }
            return ds;
        }

        public DataSet PalletToRolls_ProductQueryAllByPalletID(string PalletID)
        {
            DataSet ds = new DataSet();

 

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //WINDER_DATE   MESTime
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select top " + Utils.SelectRowsMax + PalletDS_Pallet_ProductSelect +   " where Pallet_Product.ProductID='" +PalletID+ "';"
                          + "select top " + Utils.SelectRowsMax + PalletDS_Rolls_ProductSelect + " where Pallet_Product.ProductID='" + PalletID + "'",
                          ds,
                          new string[] { "Pallet_Product", "Roll_Product" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "PalletToRolls_ProductQueryAllByMESTime读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PalletToRolls_ProductQueryAllByMESTime" + ex.ToString()));
                }

            }
            return ds;
        }

    }
}
