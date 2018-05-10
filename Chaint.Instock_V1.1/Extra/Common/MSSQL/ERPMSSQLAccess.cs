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
    public partial class ERPMSSQLAccess
    {

        /// <summary>
        /// 工厂对象
        /// </summary>
     //   private DBAccess.IADOFactory factory;
        private string ConnctionString = "";

        private string ERPSchemeName = "dbo.";
         
        public delegate void SqlStateEventHandler(object sender,
                SqlStateEventArgs e);

        public event SqlStateEventHandler SqlStateChange;

        protected virtual void OnSqlStateChange(SqlStateEventArgs e)
        {
            if (SqlStateChange != null)
            {

                _IsConect = e.IsConnect;
                SqlStateChange(this, e);//Raise the event
            }
        }

        /// <summary>
        /// Initializes a new instance of the SQLAccess class.
        /// </summary>
        public ERPMSSQLAccess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnctionString = Utils.ERPSQLConnectionString;
            ERPSchemeName = Utils.ERPSchemeName;
        }

        public ERPMSSQLAccess(string connectionstr)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnctionString = connectionstr;
        }
        
        public bool _IsConect =false;
        public bool IsConnect{
            get { return  _IsConect; }
        }
        
        public bool TestConnection()
        {
            if (ConnctionString == "")
                return false;
           
            DbConnection conn = new SqlConnection();
            conn.ConnectionString = ConnctionString;
            try
            {
                conn.Open();
                _IsConect = true;
            }

            catch
            {
                _IsConect = false;
             
            }
            finally
            {
                conn.Close(); 
            }

            return _IsConect;
        
        }     

        // select  getdate() as ServerDate         
        public DataSet ServerInfoQuery()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.Text,
                        "select  getdate() as ServerDate",
                        ds,
                        new string[] { "ServerInfo" }
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "ServerInfo读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
             

            //DataSet ds = new DataSet();
            //using (SqlConnection connection = new SqlConnection(ConnctionString))
            //{            
            //    try
            //    {
            //        connection.Open();

            //        MSSqlHelper.FillDataset(
            //        connection,
            //        CommandType.StoredProcedure,
            //        ServerInfoQuery_PRO,
            //        ds,
            //        new string[] { "ServerInfo" }
            //       );


            //        OnSqlStateChange(new SqlStateEventArgs(true,"ServerInfo读取成功"));
            //    }
            //    catch (Exception ex)
            //    {
            //        OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
            //    }
 
            //}
            //return ds;
        }
        
        //TOCT_Material
        public ERPDS TOCT_MaterialQueryForERPLoad(DateTime CreateTime,FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成
                string sqlstr = "select top 500 * from "+ERPSchemeName+"TOCT_Material where CreateTime > @CreateTime and Flag=@Flag;";

                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@CreateTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@Flag", (int)Flag)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_Material" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_MaterialQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_MaterialQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_MaterialUpdateForERPLoad(string MaterialCode, FlagType Flag)
        {
          
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "update " + ERPSchemeName + "TOCT_Material set Flag=@Flag where MaterialCode= @MaterialCode;";
                                
                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@MaterialCode", MaterialCode),
                  new SqlParameter("@Flag", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr, 
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_MaterialUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_MaterialUpdateForERPLoad Error:" + ex.ToString()));
                }             
                
            }

            return ret;
        }
        
        //TOCT_Department
        public ERPDS TOCT_DepartmentQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成
                string sqlstr = "select top 500 * from " + ERPSchemeName + "TOCT_Department where CreateTime > @CreateTime and Flag=@Flag;";

                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@CreateTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@Flag", (int)Flag)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_Department" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_DepartmentQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_DepartmentQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_DepartmentUpdateForERPLoad(string DeptCode, FlagType Flag)
        {           
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成
                string sqlstr = "update " + ERPSchemeName + "TOCT_Department set Flag=@Flag where DeptCode= @DeptCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@DeptCode", DeptCode),
                  new SqlParameter("@Flag", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_DepartmentUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_DepartmentUpdateForERPLoad Error:" + ex.ToString()));
                }
            }
            return ret;
        }

        //TOCT_Warehouse
        public ERPDS TOCT_WarehouseQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "select top 500 * from " + ERPSchemeName + "TOCT_Warehouse where CreateTime > @CreateTime and Flag=@Flag;";

                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@CreateTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@Flag", (int)Flag)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_Warehouse" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_WarehouseQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_WarehouseQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_WarehouseUpdateForERPLoad(string WareHouseCode, FlagType Flag)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成
                string sqlstr = "update " + ERPSchemeName + "TOCT_Warehouse set Flag=@Flag where WareHouseCode= @WareHouseCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@WareHouseCode", WareHouseCode),
                  new SqlParameter("@Flag", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_WarehouseUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_WarehouseUpdateForERPLoad Error:" + ex.ToString()));
                }
            }            
            return ret;
        }

        //TOCT_Location
        public ERPDS TOCT_LocationQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "select top 500 * from " + ERPSchemeName + "TOCT_Location where CreateTime > @CreateTime and Flag=@Flag;";

                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@CreateTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@Flag", (int)Flag)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_Location" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LocationQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LocationQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_LocationUpdateForERPLoad(string LocationCode, FlagType Flag)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成
                string sqlstr = "update " + ERPSchemeName + "TOCT_Location set Flag=@Flag where LocationCode= @LocationCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@LocationCode", LocationCode),
                  new SqlParameter("@Flag", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LocationUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LocationUpdateForERPLoad Error:" + ex.ToString()));
                }
            }
            return ret;
        }

        //TOCT_ORDER   //SO_ DATE YYYYMMDD  
        public ERPDS TOCT_ORDERQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "select top 500 * from " + ERPSchemeName + "TOCT_ORDER where SO_DATE > @SO_DATE and SO_FLAG=@SO_FLAG;";

                //日期为字符型，比较字符型大小
                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime.ToString("yyyyMMdd");
                par1.DbType = DbType.String;
                par1.ParameterName = "@SO_DATE";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@SO_FLAG", (int)Flag)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_ORDER" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_ORDERQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_ORDERQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_ORDERUpdateForERPLoad(string SO_ID, FlagType Flag)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "update " + ERPSchemeName + "TOCT_ORDER set SO_FLAG=@SO_FLAG where SO_ID= @SO_ID;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@SO_ID", SO_ID),
                  new SqlParameter("@SO_FLAG", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_ORDERUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_ORDERUpdateForERPLoad Error:" + ex.ToString()));
                }
            }
            return ret;
        }
        
        //TOCT_LADING
        public ERPDS TOCT_LADINGQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "select top 500 * from " + ERPSchemeName + "TOCT_LADING where CreateTime > @CreateTime and Flag=@Flag;";

                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@CreateTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@Flag", (int)Flag)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_LADING" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LADINGQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LADINGQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_LADINGUpdateForERPLoad(string LADINGNUM, FlagType Flag)
        {

            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成
                string sqlstr = "update " + ERPSchemeName + "TOCT_LADING set Flag=@Flag where LADINGNUM= @LADINGNUM;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@LADINGNUM", LADINGNUM),
                  new SqlParameter("@Flag", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LADINGUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LADINGUpdateForERPLoad Error:" + ex.ToString()));
                }

            }

            return ret;
        }



        //CT_CODEDATA
        public bool CT_CODEDATAInsertByRow(ERPDS.CT_CODEDATARow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO " + ERPSchemeName + "CT_CODEDATA( BARCODE,ProdDate,DEPARTNUM,ClassNum,BarCodeType,OrderNo,MATERIALCODE,MATERIALNAME,Weight,OPERATEDATE,CUSTOMERBATCH,OPERATORNAME,Createtime,FLAG,NOTE,RollLength,SheetCount)VALUES(@BARCODE,@ProdDate,@DEPARTNUM,@ClassNum,@BarCodeType,@OrderNo,@MATERIALCODE,@MATERIALNAME,@Weight,@OPERATEDATE,@CUSTOMERBATCH,@OPERATORNAME,@Createtime,@FLAG,@NOTE,@RollLength,@SheetCount);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@BARCODE", row.BARCODE),
                new SqlParameter("@ProdDate", row.ProdDate),
                new SqlParameter("@DEPARTNUM", row.DEPARTNUM),
                new SqlParameter("@ClassNum",row.IsClassNumNull()?DBNull.Value:(object)row.ClassNum),
                new SqlParameter("@BarCodeType", row.BarCodeType),
                new SqlParameter("@OrderNo", row.OrderNo),
                new SqlParameter("@MATERIALCODE", row.MATERIALCODE),
                new SqlParameter("@MATERIALNAME", row.MATERIALNAME),
                new SqlParameter("@Weight", row.Weight),
                new SqlParameter("@OPERATEDATE", row.OPERATEDATE),
                new SqlParameter("@CUSTOMERBATCH",row.IsCUSTOMERBATCHNull()?DBNull.Value:(object)row.CUSTOMERBATCH),
                new SqlParameter("@OPERATORNAME", row.OPERATORNAME),
                new SqlParameter("@Createtime", row.Createtime),
                new SqlParameter("@FLAG", row.FLAG),
                new SqlParameter("@NOTE",row.IsNOTENull()?DBNull.Value:(object)row.NOTE),
                new SqlParameter("@RollLength",row.IsRollLengthNull()?0: row.RollLength),

                new SqlParameter("@SheetCount",row.IsSheetCountNull()?0: row.SheetCount)

                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_CODEDATAInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_CODEDATAInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS CT_CODEDATAQueryByBARCODE(string BARCODE)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "select top 500 * from " + ERPSchemeName + "CT_CODEDATA where BARCODE=@BARCODE;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@BARCODE", BARCODE)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_CODEDATA" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_CODEDATAQueryByBARCODE OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_CODEDATAQueryByBARCODE Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool CT_CODEDATADeleteByBARCODE(string BARCODE)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "delete from " + ERPSchemeName + "CT_CODEDATA where BARCODE=@BARCODE;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@BARCODE", BARCODE)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_CODEDATADeleteByBARCODE OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_CODEDATADeleteByBARCODE Error:" + ex.ToString()));
                }
            }
            return ret;
        }

        //CT_INSTOCKDATA
        public bool CT_INSTOCKDATAInsertByRow(ERPDS.CT_INSTOCKDATARow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO " + ERPSchemeName + "CT_INSTOCKDATA( LSBH,BusinessType,RESTOCK,MATERIALCODE,MATERIALNAME,DEPARTNUM,STOCKNUM,STOCKNUM2,BARCODE,OPERATEDATE,OPERATORNAME,FLAG,NOTE,Shift)VALUES(@LSBH,@BusinessType,@RESTOCK,@MATERIALCODE,@MATERIALNAME,@DEPARTNUM,@STOCKNUM,@STOCKNUM2,@BARCODE,@OPERATEDATE,@OPERATORNAME,@FLAG,@NOTE,@Shift);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@LSBH", row.LSBH),
                new SqlParameter("@BusinessType", row.BusinessType),
                new SqlParameter("@RESTOCK", row.RESTOCK),
                new SqlParameter("@MATERIALCODE", row.MATERIALCODE),
                new SqlParameter("@MATERIALNAME", row.MATERIALNAME),
                new SqlParameter("@DEPARTNUM", row.DEPARTNUM),
                new SqlParameter("@STOCKNUM", row.STOCKNUM),
                new SqlParameter("@STOCKNUM2",row.IsSTOCKNUM2Null()?DBNull.Value:(object)row.STOCKNUM2),
                new SqlParameter("@BARCODE", row.BARCODE),
                new SqlParameter("@OPERATEDATE", row.OPERATEDATE),
                new SqlParameter("@OPERATORNAME", row.OPERATORNAME),
                new SqlParameter("@FLAG", row.FLAG),
                new SqlParameter("@NOTE",row.IsNOTENull()?DBNull.Value:(object)row.NOTE),
                new SqlParameter("@Shift",row.IsShiftNull()?"0": row.Shift),

                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_INSTOCKDATAInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_INSTOCKDATAInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS CT_INSTOCKDATAQueryByLSBH(string LSBH)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "select top 500 * from " + ERPSchemeName + "CT_INSTOCKDATA where LSBH=@LSBH;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LSBH", LSBH)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_INSTOCKDATA" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_INSTOCKDATAQueryByLSBH OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_INSTOCKDATAQueryByLSBH Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool CT_INSTOCKDATADeleteByLSBH(string LSBH)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "delete from " + ERPSchemeName + "CT_INSTOCKDATA where LSBH=@LSBH;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LSBH", LSBH)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_INSTOCKDATADeleteByLSBH OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_INSTOCKDATADeleteByLSBH Error:" + ex.ToString()));
                }
            }
            return ret;
        }


        //CT_OUTSTOCKDATA
        public bool CT_OUTSTOCKDATAInsertByRow(ERPDS.CT_OUTSTOCKDATARow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO " + ERPSchemeName + "CT_OUTSTOCKDATA( LSBH,BusinessType,RESTOCK,LADINGNUM,STOCKOUTDATE,STOCKNUM,CUSTNUM,CUSTNAME,MATERIALCODE,MATERIAL,UNITCODE,UNITNAME,BARCODE,OPERATORNAME,Createtime,FLAG,NOTE,Shift)VALUES(@LSBH,@BusinessType,@RESTOCK,@LADINGNUM,@STOCKOUTDATE,@STOCKNUM,@CUSTNUM,@CUSTNAME,@MATERIALCODE,@MATERIAL,@UNITCODE,@UNITNAME,@BARCODE,@OPERATORNAME,@Createtime,@FLAG,@NOTE,@Shift);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@LSBH", row.LSBH),
                new SqlParameter("@BusinessType", row.BusinessType),
                new SqlParameter("@RESTOCK", row.RESTOCK),
                new SqlParameter("@LADINGNUM", row.LADINGNUM),
                new SqlParameter("@STOCKOUTDATE", row.STOCKOUTDATE),
                new SqlParameter("@STOCKNUM", row.STOCKNUM),
                new SqlParameter("@CUSTNUM", row.CUSTNUM),
                new SqlParameter("@CUSTNAME", row.CUSTNAME),
                new SqlParameter("@MATERIALCODE", row.MATERIALCODE),
                new SqlParameter("@MATERIAL", row.MATERIAL),
                new SqlParameter("@UNITCODE", row.UNITCODE),
                new SqlParameter("@UNITNAME", row.UNITNAME),
                new SqlParameter("@BARCODE", row.BARCODE),
                new SqlParameter("@OPERATORNAME", row.OPERATORNAME),
                new SqlParameter("@Createtime", row.Createtime),
                new SqlParameter("@FLAG", row.FLAG),
                new SqlParameter("@NOTE",row.IsNOTENull()?DBNull.Value:(object)row.NOTE),
                new SqlParameter("@Shift",row.IsShiftNull()?"0": row.Shift),

                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_OUTSTOCKDATAInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_OUTSTOCKDATAInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS CT_OUTSTOCKDATAQueryByLSBH(string LSBH)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select top 500 * from " + ERPSchemeName + "CT_OUTSTOCKDATA where LSBH=@LSBH;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LSBH", LSBH)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_OUTSTOCKDATA" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_OUTSTOCKDATAQueryByLSBH OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_OUTSTOCKDATAQueryByLSBH Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool CT_OUTSTOCKDATADeleteByLSBH(string LSBH)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "delete from " + ERPSchemeName + "CT_OUTSTOCKDATA where LSBH=@LSBH;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LSBH", LSBH)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_OUTSTOCKDATADeleteByLSBH OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_OUTSTOCKDATADeleteByLSBH Error:" + ex.ToString()));
                }
            }
            return ret;
        }

    }


    public enum FlagType
    {
        Init = 0x0,
        OK = 0x1,
        Error = 0x2,
        Complete = 0x9
    }
     
}
