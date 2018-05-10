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

        //PalletToRolls
        public bool PalletToRollsInsertByValue(string PalletID, string RollID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO PalletToRolls( PalletID,RollID)VALUES(@PalletID,@RollID);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@PalletID", PalletID),
                new SqlParameter("@RollID", RollID)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PalletToRollsInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PalletToRollsInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public bool PalletToRollsDeleteByPalletID(string PalletID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "delete from PalletToRolls where PalletID=@PalletID;";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@PalletID", PalletID) 
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PalletToRollsDeleteByPalletID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PalletToRollsDeleteByPalletID Error:" + ex.ToString()));
                }
                return ret;
            }
        }


        //Pallet_Prouct   
        public Int64 Pallet_ProductInsertByRowAutoProductID(string InitProductID, MainDS.Pallet_ProductRow row)
        {           
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "";
                //sqlstr = sqlstr + "set @OutProductID =(select ISNULL(max(ProductID), @OutProductID) from Pallet_Product where datediff(DAY,SheetDate,@SheetDate)=0);";

                sqlstr = sqlstr + "set @OutProductID =(select ISNULL(max(ProductID), @OutProductID) from Pallet_Product where datediff(DAY,SheetDate,@SheetDate)=0);";
 
                sqlstr = sqlstr + "set @OutProductID =  (@OutProductID +1);";

                sqlstr = sqlstr + "INSERT INTO Pallet_Product(ProductType,ProductID,Barcode,MachineID,ProduceDate,Type,Grade,Stand_Desc,OrderNO,MaterialCode,PartNO,Basisweight,PalletSize,Width,PalletLength,WidthMode,Weight_Calc,Inspector_Desc,Destination,DegradeCause,Customer,Weight_Gross,Weight_Wei,Weight_Net,WeightMode,Factory,SheeterID,SheetShift,SheetDate,SheetGrade,PalletType,SheetReam,ReamPallet,PalletWrapperID,PalletShift,PalletDate,Height,Remark1,Remark2,Remark3,Remark4,Remark5,IsDelete,IsWrapOK,IsJetOK,IsMeasure,MeasureTime,MESTime,LastEditTime)VALUES(@ProductType,@OutProductID,@OutProductID,@MachineID,@ProduceDate,@Type,@Grade,@Stand_Desc,@OrderNO,@MaterialCode,@PartNO,@Basisweight,@PalletSize,@Width,@PalletLength,@WidthMode,@Weight_Calc,@Inspector_Desc,@Destination,@DegradeCause,@Customer,@Weight_Gross,@Weight_Wei,@Weight_Net,@WeightMode,@Factory,@SheeterID,@SheetShift,@SheetDate,@SheetGrade,@PalletType,@SheetReam,@ReamPallet,@PalletWrapperID,@PalletShift,@PalletDate,@Height,@Remark1,@Remark2,@Remark3,@Remark4,@Remark5,@IsDelete,@IsWrapOK,@IsJetOK,@IsMeasure,@MeasureTime,ISNULL(@MESTime,getdate()),ISNULL(@LastEditTime,getdate()));";


                SqlParameter par0 = new SqlParameter();
                par0.Value = InitProductID;
                par0.DbType = DbType.Int64;
                par0.Direction = ParameterDirection.InputOutput;
                par0.ParameterName = "@OutProductID";

                SqlParameter par1 = new SqlParameter();
                par1.Value = row.IsMESTimeNull() ? DBNull.Value : (object)row.MESTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@MESTime";

                SqlParameter par2 = new SqlParameter();
                par2.Value = row.IsLastEditTimeNull() ? DBNull.Value : (object)row.LastEditTime;
                par2.DbType = DbType.DateTime;
                par2.ParameterName = "@LastEditTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                    par0,

               // new SqlParameter("@OnlyID", row.OnlyID),
                new SqlParameter("@ProductType",row.IsProductTypeNull()?DBNull.Value:(object)row.ProductType),
                //new SqlParameter("@ProductID",row.IsProductIDNull()?DBNull.Value:(object)row.ProductID),
                //new SqlParameter("@Barcode",row.IsBarcodeNull()?DBNull.Value:(object)row.Barcode),
                new SqlParameter("@MachineID",row.IsMachineIDNull()?DBNull.Value:(object)row.MachineID),
                new SqlParameter("@ProduceDate",row.IsProduceDateNull()?DBNull.Value:(object)row.ProduceDate),
                new SqlParameter("@Type",row.IsTypeNull()?DBNull.Value:(object)row.Type),
                new SqlParameter("@Grade",row.IsGradeNull()?DBNull.Value:(object)row.Grade),
                new SqlParameter("@Stand_Desc",row.IsStand_DescNull()?DBNull.Value:(object)row.Stand_Desc),
                new SqlParameter("@OrderNO",row.IsOrderNONull()?DBNull.Value:(object)row.OrderNO),
                new SqlParameter("@MaterialCode",row.IsMaterialCodeNull()?DBNull.Value:(object)row.MaterialCode),
                new SqlParameter("@PartNO",row.IsPartNONull()?DBNull.Value:(object)row.PartNO),
                new SqlParameter("@Basisweight",row.IsBasisweightNull()?DBNull.Value:(object)row.Basisweight),
                new SqlParameter("@PalletSize",row.IsPalletSizeNull()?DBNull.Value:(object)row.PalletSize),
                new SqlParameter("@Width",row.IsWidthNull()?DBNull.Value:(object)row.Width),
                new SqlParameter("@PalletLength",row.IsPalletLengthNull()?DBNull.Value:(object)row.PalletLength),
                new SqlParameter("@WidthMode",row.IsWidthModeNull()?DBNull.Value:(object)row.WidthMode),
                new SqlParameter("@Weight_Calc",row.IsWeight_CalcNull()?DBNull.Value:(object)row.Weight_Calc),
                new SqlParameter("@Inspector_Desc",row.IsInspector_DescNull()?DBNull.Value:(object)row.Inspector_Desc),
                new SqlParameter("@Destination",row.IsDestinationNull()?DBNull.Value:(object)row.Destination),
                new SqlParameter("@DegradeCause",row.IsDegradeCauseNull()?DBNull.Value:(object)row.DegradeCause),
                new SqlParameter("@Customer",row.IsCustomerNull()?DBNull.Value:(object)row.Customer),
                new SqlParameter("@Weight_Gross",row.IsWeight_GrossNull()?DBNull.Value:(object)row.Weight_Gross),
                new SqlParameter("@Weight_Wei",row.IsWeight_WeiNull()?DBNull.Value:(object)row.Weight_Wei),
                new SqlParameter("@Weight_Net",row.IsWeight_NetNull()?DBNull.Value:(object)row.Weight_Net),
                new SqlParameter("@WeightMode",row.IsWeightModeNull()?DBNull.Value:(object)row.WeightMode),
                new SqlParameter("@Factory",row.IsFactoryNull()?DBNull.Value:(object)row.Factory),
                new SqlParameter("@SheeterID",row.IsSheeterIDNull()?DBNull.Value:(object)row.SheeterID),
                new SqlParameter("@SheetShift",row.IsSheetShiftNull()?DBNull.Value:(object)row.SheetShift),
                new SqlParameter("@SheetDate",row.IsSheetDateNull()?DBNull.Value:(object)row.SheetDate),
                new SqlParameter("@SheetGrade",row.IsSheetGradeNull()?DBNull.Value:(object)row.SheetGrade),
                new SqlParameter("@PalletType",row.IsPalletTypeNull()?DBNull.Value:(object)row.PalletType),
                new SqlParameter("@SheetReam",row.IsSheetReamNull()?DBNull.Value:(object)row.SheetReam),
                new SqlParameter("@ReamPallet",row.IsReamPalletNull()?DBNull.Value:(object)row.ReamPallet),
                new SqlParameter("@PalletWrapperID",row.IsPalletWrapperIDNull()?DBNull.Value:(object)row.PalletWrapperID),
                new SqlParameter("@PalletShift",row.IsPalletShiftNull()?DBNull.Value:(object)row.PalletShift),
                new SqlParameter("@PalletDate",row.IsPalletDateNull()?DBNull.Value:(object)row.PalletDate),
                new SqlParameter("@Height",row.IsHeightNull()?DBNull.Value:(object)row.Height),
                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3),
                new SqlParameter("@Remark4",row.IsRemark4Null()?DBNull.Value:(object)row.Remark4),
                new SqlParameter("@Remark5",row.IsRemark5Null()?DBNull.Value:(object)row.Remark5),
                new SqlParameter("@IsDelete",row.IsIsDeleteNull()?DBNull.Value:(object)row.IsDelete),
                new SqlParameter("@IsWrapOK",row.IsIsWrapOKNull()?DBNull.Value:(object)row.IsWrapOK),
                new SqlParameter("@IsJetOK",row.IsIsJetOKNull()?DBNull.Value:(object)row.IsJetOK),
                new SqlParameter("@IsMeasure",row.IsIsMeasureNull()?DBNull.Value:(object)row.IsMeasure),
                new SqlParameter("@MeasureTime",row.IsMeasureTimeNull()?DBNull.Value:(object)row.MeasureTime),
                //new SqlParameter("@MESTime",row.IsMESTimeNull()?DBNull.Value:(object)row.MESTime),
                //new SqlParameter("@LastEditTime",row.IsLastEditTimeNull()?DBNull.Value:(object)row.LastEditTime)
                par1,
                par2                
                };
                try
                {
                    connection.Open();                    

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                 
                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductInsert Error:" + ex.ToString()));

                    return -1;
                }
                return Convert.ToInt64(par0.Value);
            }

        }
        
        public bool Pallet_ProductUpdateMSKScanDataByProductID(string ProdutID, string PalletWrapperID, string PalletShift, DateTime PalletDate)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    #region sqlstr
                    string sqlstr = "UPDATE Pallet_Product SET PalletWrapperID = @PalletWrapperID,PalletShift=@PalletShift,PalletDate=@PalletDate  where ProductID = @ProductID";
                   
                    #endregion

                    #region parameter
                    SqlParameter[] parameter = new SqlParameter[]
                                { 
                                new SqlParameter("@ProductID",ProdutID),
                                new SqlParameter("@PalletWrapperID",PalletWrapperID),
                                new SqlParameter("@PalletShift",PalletShift),
                                new SqlParameter("@PalletDate",PalletDate)                                
                                };
                    #endregion

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductUpdateMSKScanDataByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductUpdateMSKScanDataByProductID:"+ex.ToString()));
                    return false;
                }
                return true;
            }
        }

        public bool Pallet_ProductUpdateMSKMeasureDataByProductID(string ProductID, Int16 Height, Int16 Width, Int16 PalletLength, Int16 Weight_Wei,  DateTime MeasureTime)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    #region sqlstr
                    string sqlstr = "UPDATE Pallet_Product SET Height = @Height,Width=@Width,PalletLength=@PalletLength,Weight_Wei=@Weight_Wei,MeasureTime=@MeasureTime where ProductID = @ProductID";

                    #endregion

                    #region parameter
                    SqlParameter[] parameter = new SqlParameter[]
                                { 
                                new SqlParameter("@ProductID",ProductID),
                                new SqlParameter("@Height",Height),
                                new SqlParameter("@Width",Width),
                                new SqlParameter("@PalletLength",PalletLength),
                                new SqlParameter("@Weight_Wei",Weight_Wei),
                                new SqlParameter("@MeasureTime",MeasureTime)                                   
                                };
                    #endregion

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductUpdateMSKMeasureDataByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductUpdateMSKMeasureDataByProductID:" + ex.ToString()));
                    return false;
                }
                return true;
            }
        }
        
        public MainDS Pallet_ProductQueryByProductID(string ProductID)
        {
            MainDS ds = new MainDS();
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
                          "select * from Pallet_Product where ProductID=@ProductID",
                          ds,
                          new string[] { "Pallet_Product" },
                          parameter
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductQueryByProductID读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductQueryByProductID Error:"+ ex.ToString()));
                }
            }
            return ds;
        }
        
        public bool Pallet_ProductUpdateAllByPK(MainDS.Pallet_ProductRow row, bool IsUpdateDBNull)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {

                    #region sqlstr
                    string sqlstr = "UPDATE Pallet_Product SET IsUploadERP=0,";
                    //if (IsUpdateDBNull || !row.IsOnlyIDNull())
                    //    sqlstr = sqlstr + " OnlyID = @OnlyID,";
                    if (IsUpdateDBNull || !row.IsProductTypeNull())
                        sqlstr = sqlstr + " ProductType = @ProductType,";
                    if (IsUpdateDBNull || !row.IsProductIDNull())
                        sqlstr = sqlstr + " ProductID = @ProductID,";
                    if (IsUpdateDBNull || !row.IsBarcodeNull())
                        sqlstr = sqlstr + " Barcode = @Barcode,";
                    if (IsUpdateDBNull || !row.IsMachineIDNull())
                        sqlstr = sqlstr + " MachineID = @MachineID,";
                    if (IsUpdateDBNull || !row.IsProduceDateNull())
                        sqlstr = sqlstr + " ProduceDate = @ProduceDate,";
                    if (IsUpdateDBNull || !row.IsTypeNull())
                        sqlstr = sqlstr + " Type = @Type,";
                    if (IsUpdateDBNull || !row.IsGradeNull())
                        sqlstr = sqlstr + " Grade = @Grade,";
                    if (IsUpdateDBNull || !row.IsStand_DescNull())
                        sqlstr = sqlstr + " Stand_Desc = @Stand_Desc,";
                    if (IsUpdateDBNull || !row.IsOrderNONull())
                        sqlstr = sqlstr + " OrderNO = @OrderNO,";
                    if (IsUpdateDBNull || !row.IsMaterialCodeNull())
                        sqlstr = sqlstr + " MaterialCode = @MaterialCode,";
                    if (IsUpdateDBNull || !row.IsPartNONull())
                        sqlstr = sqlstr + " PartNO = @PartNO,";
                    if (IsUpdateDBNull || !row.IsBasisweightNull())
                        sqlstr = sqlstr + " Basisweight = @Basisweight,";
                    if (IsUpdateDBNull || !row.IsPalletSizeNull())
                        sqlstr = sqlstr + " PalletSize = @PalletSize,";
                    if (IsUpdateDBNull || !row.IsWidthNull())
                        sqlstr = sqlstr + " Width = @Width,";
                    if (IsUpdateDBNull || !row.IsPalletLengthNull())
                        sqlstr = sqlstr + " PalletLength = @PalletLength,";
                    if (IsUpdateDBNull || !row.IsWidthModeNull())
                        sqlstr = sqlstr + " WidthMode = @WidthMode,";
                    if (IsUpdateDBNull || !row.IsWeight_CalcNull())
                        sqlstr = sqlstr + " Weight_Calc = @Weight_Calc,";
                    if (IsUpdateDBNull || !row.IsInspector_DescNull())
                        sqlstr = sqlstr + " Inspector_Desc = @Inspector_Desc,";
                    if (IsUpdateDBNull || !row.IsDestinationNull())
                        sqlstr = sqlstr + " Destination = @Destination,";
                    if (IsUpdateDBNull || !row.IsDegradeCauseNull())
                        sqlstr = sqlstr + " DegradeCause = @DegradeCause,";
                    if (IsUpdateDBNull || !row.IsCustomerNull())
                        sqlstr = sqlstr + " Customer = @Customer,";
                    if (IsUpdateDBNull || !row.IsWeight_GrossNull())
                        sqlstr = sqlstr + " Weight_Gross = @Weight_Gross,";
                    if (IsUpdateDBNull || !row.IsWeight_WeiNull())
                        sqlstr = sqlstr + " Weight_Wei = @Weight_Wei,";
                    if (IsUpdateDBNull || !row.IsWeight_NetNull())
                        sqlstr = sqlstr + " Weight_Net = @Weight_Net,";
                    if (IsUpdateDBNull || !row.IsWeightModeNull())
                        sqlstr = sqlstr + " WeightMode = @WeightMode,";
                    if (IsUpdateDBNull || !row.IsFactoryNull())
                        sqlstr = sqlstr + " Factory = @Factory,";
                    if (IsUpdateDBNull || !row.IsSheeterIDNull())
                        sqlstr = sqlstr + " SheeterID = @SheeterID,";
                    if (IsUpdateDBNull || !row.IsSheetShiftNull())
                        sqlstr = sqlstr + " SheetShift = @SheetShift,";
                    if (IsUpdateDBNull || !row.IsSheetDateNull())
                        sqlstr = sqlstr + " SheetDate = @SheetDate,";
                    if (IsUpdateDBNull || !row.IsSheetGradeNull())
                        sqlstr = sqlstr + " SheetGrade = @SheetGrade,";
                    if (IsUpdateDBNull || !row.IsPalletTypeNull())
                        sqlstr = sqlstr + " PalletType = @PalletType,";
                    if (IsUpdateDBNull || !row.IsSheetReamNull())
                        sqlstr = sqlstr + " SheetReam = @SheetReam,";
                    if (IsUpdateDBNull || !row.IsReamPalletNull())
                        sqlstr = sqlstr + " ReamPallet = @ReamPallet,";
                    if (IsUpdateDBNull || !row.IsPalletWrapperIDNull())
                        sqlstr = sqlstr + " PalletWrapperID = @PalletWrapperID,";
                    if (IsUpdateDBNull || !row.IsPalletShiftNull())
                        sqlstr = sqlstr + " PalletShift = @PalletShift,";
                    if (IsUpdateDBNull || !row.IsPalletDateNull())
                        sqlstr = sqlstr + " PalletDate = @PalletDate,";
                    if (IsUpdateDBNull || !row.IsHeightNull())
                        sqlstr = sqlstr + " Height = @Height,";
                    if (IsUpdateDBNull || !row.IsRemark1Null())
                        sqlstr = sqlstr + " Remark1 = @Remark1,";
                    if (IsUpdateDBNull || !row.IsRemark2Null())
                        sqlstr = sqlstr + " Remark2 = @Remark2,";
                    if (IsUpdateDBNull || !row.IsRemark3Null())
                        sqlstr = sqlstr + " Remark3 = @Remark3,";
                    if (IsUpdateDBNull || !row.IsRemark4Null())
                        sqlstr = sqlstr + " Remark4 = @Remark4,";
                    if (IsUpdateDBNull || !row.IsRemark5Null())
                        sqlstr = sqlstr + " Remark5 = @Remark5,";
                    if (IsUpdateDBNull || !row.IsIsDeleteNull())
                        sqlstr = sqlstr + " IsDelete = @IsDelete,";
                    if (IsUpdateDBNull || !row.IsIsWrapOKNull())
                        sqlstr = sqlstr + " IsWrapOK = @IsWrapOK,";
                    if (IsUpdateDBNull || !row.IsIsJetOKNull())
                        sqlstr = sqlstr + " IsJetOK = @IsJetOK,";
                    if (IsUpdateDBNull || !row.IsIsMeasureNull())
                        sqlstr = sqlstr + " IsMeasure = @IsMeasure,";
                    if (IsUpdateDBNull || !row.IsMeasureTimeNull())
                        sqlstr = sqlstr + " MeasureTime = @MeasureTime,";
                    //if (IsUpdateDBNull || !row.IsMESTimeNull())
                    //    sqlstr = sqlstr + " MESTime = @MESTime,";
                   // if (IsUpdateDBNull || !row.IsLastEditTimeNull())
                    sqlstr = sqlstr + " LastEditTime = getdate(),";
                    sqlstr = sqlstr.TrimEnd(new char[] { ' ', ',' });
                    sqlstr = sqlstr + " WHERE ProductID = @ProductID";
                    #endregion

                    #region parameter
                    SqlParameter[] parameter = new SqlParameter[]
                                {
                                //new SqlParameter("@OnlyID", row.OnlyID),
                                new SqlParameter("@ProductType",row.IsProductTypeNull()?DBNull.Value:(object)row.ProductType),
                                new SqlParameter("@ProductID", row.ProductID),
                                new SqlParameter("@Barcode",row.IsBarcodeNull()?DBNull.Value:(object)row.Barcode),
                                new SqlParameter("@MachineID",row.IsMachineIDNull()?DBNull.Value:(object)row.MachineID),
                                new SqlParameter("@ProduceDate",row.IsProduceDateNull()?DBNull.Value:(object)row.ProduceDate),
                                new SqlParameter("@Type",row.IsTypeNull()?DBNull.Value:(object)row.Type),
                                new SqlParameter("@Grade",row.IsGradeNull()?DBNull.Value:(object)row.Grade),
                                new SqlParameter("@Stand_Desc",row.IsStand_DescNull()?DBNull.Value:(object)row.Stand_Desc),
                                new SqlParameter("@OrderNO",row.IsOrderNONull()?DBNull.Value:(object)row.OrderNO),
                                new SqlParameter("@MaterialCode",row.IsMaterialCodeNull()?DBNull.Value:(object)row.MaterialCode),
                                new SqlParameter("@PartNO",row.IsPartNONull()?DBNull.Value:(object)row.PartNO),
                                new SqlParameter("@Basisweight",row.IsBasisweightNull()?DBNull.Value:(object)row.Basisweight),
                                new SqlParameter("@PalletSize",row.IsPalletSizeNull()?DBNull.Value:(object)row.PalletSize),
                                new SqlParameter("@Width",row.IsWidthNull()?DBNull.Value:(object)row.Width),
                                new SqlParameter("@PalletLength",row.IsPalletLengthNull()?DBNull.Value:(object)row.PalletLength),
                                new SqlParameter("@WidthMode",row.IsWidthModeNull()?DBNull.Value:(object)row.WidthMode),
                                new SqlParameter("@Weight_Calc",row.IsWeight_CalcNull()?DBNull.Value:(object)row.Weight_Calc),
                                new SqlParameter("@Inspector_Desc",row.IsInspector_DescNull()?DBNull.Value:(object)row.Inspector_Desc),
                                new SqlParameter("@Destination",row.IsDestinationNull()?DBNull.Value:(object)row.Destination),
                                new SqlParameter("@DegradeCause",row.IsDegradeCauseNull()?DBNull.Value:(object)row.DegradeCause),
                                new SqlParameter("@Customer",row.IsCustomerNull()?DBNull.Value:(object)row.Customer),
                                new SqlParameter("@Weight_Gross",row.IsWeight_GrossNull()?DBNull.Value:(object)row.Weight_Gross),
                                new SqlParameter("@Weight_Wei",row.IsWeight_WeiNull()?DBNull.Value:(object)row.Weight_Wei),
                                new SqlParameter("@Weight_Net",row.IsWeight_NetNull()?DBNull.Value:(object)row.Weight_Net),
                                new SqlParameter("@WeightMode",row.IsWeightModeNull()?DBNull.Value:(object)row.WeightMode),
                                new SqlParameter("@Factory",row.IsFactoryNull()?DBNull.Value:(object)row.Factory),
                                new SqlParameter("@SheeterID",row.IsSheeterIDNull()?DBNull.Value:(object)row.SheeterID),
                                new SqlParameter("@SheetShift",row.IsSheetShiftNull()?DBNull.Value:(object)row.SheetShift),
                                new SqlParameter("@SheetDate",row.IsSheetDateNull()?DBNull.Value:(object)row.SheetDate),
                                new SqlParameter("@SheetGrade",row.IsSheetGradeNull()?DBNull.Value:(object)row.SheetGrade),
                                new SqlParameter("@PalletType",row.IsPalletTypeNull()?DBNull.Value:(object)row.PalletType),
                                new SqlParameter("@SheetReam",row.IsSheetReamNull()?DBNull.Value:(object)row.SheetReam),
                                new SqlParameter("@ReamPallet",row.IsReamPalletNull()?DBNull.Value:(object)row.ReamPallet),
                                new SqlParameter("@PalletWrapperID",row.IsPalletWrapperIDNull()?DBNull.Value:(object)row.PalletWrapperID),
                                new SqlParameter("@PalletShift",row.IsPalletShiftNull()?DBNull.Value:(object)row.PalletShift),
                                new SqlParameter("@PalletDate",row.IsPalletDateNull()?DBNull.Value:(object)row.PalletDate),
                                new SqlParameter("@Height",row.IsHeightNull()?DBNull.Value:(object)row.Height),
                                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3),
                                new SqlParameter("@Remark4",row.IsRemark4Null()?DBNull.Value:(object)row.Remark4),
                                new SqlParameter("@Remark5",row.IsRemark5Null()?DBNull.Value:(object)row.Remark5),
                                new SqlParameter("@IsDelete",row.IsIsDeleteNull()?DBNull.Value:(object)row.IsDelete),
                                new SqlParameter("@IsWrapOK",row.IsIsWrapOKNull()?DBNull.Value:(object)row.IsWrapOK),
                                new SqlParameter("@IsJetOK",row.IsIsJetOKNull()?DBNull.Value:(object)row.IsJetOK),
                                new SqlParameter("@IsMeasure",row.IsIsMeasureNull()?DBNull.Value:(object)row.IsMeasure),
                                new SqlParameter("@MeasureTime",row.IsMeasureTimeNull()?DBNull.Value:(object)row.MeasureTime),
                                //new SqlParameter("@MESTime",row.IsMESTimeNull()?DBNull.Value:(object)row.MESTime),
                                //new SqlParameter("@LastEditTime",row.IsLastEditTimeNull()?DBNull.Value:(object)row.LastEditTime)
                                
                                };
                    #endregion

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductUpdateAllByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductUpdateAllByPK Error:" + ex.ToString()));
                    return false;
                }
                return true;
            }
        }
        
        public bool Pallet_ProductUpdateIsDeleteByProductID(string productID, bool isDelete)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    #region sqlstr
                    string sqlstr = "UPDATE Pallet_Product SET IsDelete = @IsDelete where ProductID = @ProductID";
                 
                    #endregion
                    #region parameter
                    SqlParameter[] parameter = new SqlParameter[]
                                { 
                                new SqlParameter("@ProductID",productID),
                                new SqlParameter("@IsDelete",isDelete)
                               
                                };
                    #endregion

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductUpdateIsDeleteByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductUpdateIsDeleteByProductID:" + ex.ToString()));
                    return false;
                }
                return true;
            }
        }

        //public MainDS Pallet_ProductQueryAllForUploadERP()
        //{
        //    MainDS ds = new MainDS();
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        string sqltext = "select top 500 * from Pallet_product where isnull(isuploaderp,0) =0 order by mestime desc;";

        //        try
        //        {
        //            MSSqlHelper.FillDataset(connection,
        //                  CommandType.Text,
        //                  sqltext,
        //                  ds,
        //                  new string[] { "Pallet_Product" }
        //                  );
        //            OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductQueryAllForUploadERP读取成功"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false,"Pallet_ProductQueryAllForUploadERP Error:" + ex.ToString()));
        //        }
        //    }
        //    return ds;
        //}

        public bool Pallet_ProductIsUploadERPUpdateByRollID(string RollID, bool IsUploadERP)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Pallet_Product set IsUploadERP='" + (IsUploadERP ? "1" : "0") + "' where ProductID='" + RollID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Pallet_ProductIsUploadERPUpdateByRollID修改成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Pallet_ProductIsUploadERPUpdateByRollID Error:" + ex.ToString()));
                    return false;
                }
                return true;
            }


        }
    }
}
