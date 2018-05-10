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
        #region RollDS_Roll_ProductSelect 
        private static string RollDS_Roll_ProductSelect =
            @" Roll_Product.*
,JumboShiftTable.Shift_Desc as JumboShift_Desc
,TypeTable.Type_Desc as Type_Desc
,GradeTable.Grade_Desc as Grade_Desc
,ReWinderShiftTable.Shift_Desc as ReWinderShift_Desc
,RollWrapShiftTable.Shift_Desc as RollWrapShift_Desc
,SheetShiftTable.Shift_Desc as SheetShift_Desc
FROM Roll_Product
left join Paper_Shift JumboShiftTable on JumboShiftTable.Shift = Roll_Product.JumboShift
left join Paper_Type TypeTable on TypeTable.Type = Roll_Product.Type
left join Paper_Grade GradeTable on GradeTable.Grade = Roll_Product.Grade
left join Paper_Shift ReWinderShiftTable on ReWinderShiftTable.Shift = Roll_Product.ReWinderShift 
left join Paper_Shift RollWrapShiftTable on RollWrapShiftTable.Shift = Roll_Product.RollWrapShift 
left join Paper_Shift SheetShiftTable on SheetShiftTable.Shift = Roll_Product.SheetShift";

        #endregion

        //Roll_Product  用于打印，显示等，包括纸卷的各种详细信息
        public RollDS RollDS_Roll_ProductQueryByOnlyID(string OnlyID)
        {
            RollDS ds = new RollDS();
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
                         "select " + RollDS_Roll_ProductSelect + " where OnlyID=@OnlyID",
                          ds,
                          new string[] { "Roll_Product" },
                          parameter
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RollDS_Roll_ProductQueryByOnlyID读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "RollDS_Roll_ProductQueryByOnlyID"+ex.ToString()));
                }

            }
            return ds;

        }

        public RollDS RollDS_Roll_ProductQueryByProductID(string ProductID)
        {
            RollDS ds = new RollDS();

            //DataSet ds1 = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@ProductID", ProductID)              
                };

                string strsql = "select " + RollDS_Roll_ProductSelect + " where ProductID=@ProductID";

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          strsql,
                          ds,
                          new string[] { "Roll_Product" },
                          parameter
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RollDS_Roll_ProductQueryByProductID读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "RollDS_Roll_ProductQueryByProductID,Par:" + ProductID + strsql + ex.ToString()));
                }

            }
            return ds;

        }

        public RollDS RollDS_Roll_ProductQueryByBarcode(string Barcode)
        {
            if (Barcode.Length > 10)
            {
                return RollDS_Roll_ProductQueryByProductID(Barcode);
            }
            else
            { 
              return RollDS_Roll_ProductQueryByOnlyID(Barcode);
            }
        
        }
        
        public RollDS RollDS_Roll_ProductQueryByOnlyIDsForKONEJoin(System.Collections.ArrayList OnlyIDs, string JoinType)
        {
            RollDS ds = new RollDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string strsql = "select " +RollDS_Roll_ProductSelect+
                                " inner join GripperJoin on GripperJoin.ProductOnlyID= Roll_Product.OnlyID " +
                                "where Roll_Product.OnlyID in(" + String.Join(",", OnlyIDs.ToArray()) + ") and GripperJoin.JoinType='" + JoinType + "'" +
                                "order by GripperJoin.SortIndex";
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          strsql,
                          ds,
                          new string[] { "Roll_Product" }
                        //parameter
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RollDS_Roll_ProductQueryByOnlyIDsForKONEJoin 成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "RollDS_Roll_ProductQueryByOnlyIDsForKONEJoin 失败:" + ex.ToString()));
                }
            }
            return ds;
        }     
        
        public RollDS RollDS_Roll_ProductQueryAllByMESTime(DateTime begin, DateTime end)
        {
            RollDS ds = new RollDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //WINDER_DATE   MESTime

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select top " + Utils.SelectRowsMax + RollDS_Roll_ProductSelect + " where MESTime>='" + begin.ToString() + "' and MESTime<='" + end.ToString() + "'",
                          ds,
                          new string[] { "Roll_Product" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RollDS_Roll_ProductQueryAllByMESTime读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "RollDS_Roll_ProductQueryAllByMESTime"+ex.ToString()));
                }

            }
            return ds;

        }
        
        public RollDS RollDS_Roll_ProductQueryAllForUploadERP()
        {
            RollDS ds = new RollDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqltext = "select top 500 " + RollDS_Roll_ProductSelect + " where isnull(isuploaderp,0) =0 order by mestime desc;";

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          sqltext,
                          ds,
                          new string[] { "Roll_Product" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }
        
    }
}
