using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataModel;
using Npgsql;
using System.IO;

namespace Common.NPG
{
    public class NpgSQLAccess
    {
        private string ConnctionString = "";
         
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

        public bool _IsConect = false;
        public bool IsConnect
        {
            get { return _IsConect; }
        }




        public NpgSQLAccess()
        {
            //"User ID=root; Password=pwd; Host=localhost; Port=5432; Database=testdb;Pooling=true; Min Pool Size=0; Max Pool Size=100; Connection Lifetime=0"
            ConnctionString = "Server=127.0.0.1;Port=5432;Database=testdb;User Id=icemile;Password=100200;";
        }


        public bool TestConnection()
        {
            if (ConnctionString == "")
                return false;


            NpgsqlConnection conn = new NpgsqlConnection();
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



        public DataSet LogQueryAll()
        {
            DataSet ds = new DataSet();
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnctionString))
            {

                try
                {
                    NpgSqlHelper.FillDataset(connection,
                        CommandType.Text,
                        "select * from log",
                        ds,
                        new string[] { "log" }
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "log读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }


        public DataSet ServerInfoQuery()
        {
            DataSet ds = new DataSet();
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnctionString))
            {
                try
                {
                    NpgSqlHelper.FillDataset(connection,
                        CommandType.Text,
                        "select  now() as ServerDate",
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
        }



        //Roll_Product
        public MainDS Roll_ProductQueryByPK(string MANDT, string CHARG)
        {
            MainDS ds = new MainDS();
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnctionString))
            {
                string sqltext = String.Format( "select * from Roll_Product where mandt='{0}' and charg = '{1}'",new string[]{MANDT,CHARG});

                try
                {
                    NpgSqlHelper.FillDataset(connection,
                        CommandType.Text,
                        sqltext,
                        ds,
                         new string[] { ds.Roll_Product.TableName }
                        );
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        public bool Roll_ProductInsertByRow(MainDS.Roll_ProductRow row)
        {
            string sqltext = "insert into zz.roll_product("
            + "MANDT,"
            + "CHARG,"
            + "WERKS,"
            + "PROD_DATE,"
            + "MACH,"
            + "JUMBO_CLASS,"
            + "COM,"
            + "MATNR,"
            + "MAKTX,"
            + "B_WET,"
            + "S_R,"
            + "S_SIZE,"
            + "REAL_LENGTH,"
            + "P_UNIT,"
            + "STD_WET,"
            + "REAL_WET_MARK,"
            + "JUMBO_NO,"
            + "ORDR_CUST,"
            + "CUST_NAME,"
            + "ORDR_NO,"
            + "ORDR_CUST_EX,"
            + "ORDR_PROD_SEQ,"
            + "CUTPOS,"
            + "CORLOR,"
            + "BONED,"
            + "PRINT_MARK_1,"
            + "PRINT_MARK_2,"
            + "PRINT_MARK_3,"
            + "PRINT_MARK_4,"
            + "PRINT_MARK_5,"
            + "PRINT_MARK_6,"
            + "OEM_MACH,"
            + "MIS_WRITE_DATE,"
            + "MIS_WRITE_TIME,"
            + "IPC_WRITE_DATE,"
            + "IPC_WRITE_TIME,"
            + "IPC_WRITE_DATE2,"
            + "IPC_WRITE_TIME2,"
            + "IPC_WRITE_MARK,"
            + "Prdnr,"
            + "Bottom,"
            + "Reason,"
            + "C_CORE_TYPE,"
            + "Print_type"
            + ") values("
            + "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43}"
            + ")";

   

            //sqltext = String.Format(sqltext, new string[] { 
            //"'"+row.MANDT.ToString()+"'",
            //"'"+row.CHARG.ToString()+"'",
            //row.IsWERKSNull()?"null":"'"+row.WERKS.ToString()+"'",
            //row.IsPROD_DATENull()?"null":"'"+row.PROD_DATE.ToString()+"'",
            //row.IsMACHNull()?"null":"'"+row.MACH.ToString()+"'",
            //row.IsJUMBO_CLASSNull()?"null":"'"+row.JUMBO_CLASS.ToString()+"'",
            //row.IsCOMNull()?"null":"'"+row.COM.ToString()+"'",
            //row.IsMATNRNull()?"null":"'"+row.MATNR.ToString()+"'",
            //row.IsMAKTXNull()?"null":"'"+row.MAKTX.ToString()+"'",
            //row.IsB_WETNull()?"null":row.B_WET.ToString(),
            //row.IsS_RNull()?"null":row.S_R.ToString(),
            //row.IsS_SIZENull()?"null":row.S_SIZE.ToString(),
            //row.IsREAL_LENGTHNull()?"null":row.REAL_LENGTH.ToString(),
            //row.IsP_UNITNull()?"null":"'"+row.P_UNIT.ToString()+"'",
            //row.IsSTD_WETNull()?"null":row.STD_WET.ToString(),
            //row.IsREAL_WET_MARKNull()?"null":"'"+row.REAL_WET_MARK.ToString()+"'",
            //row.IsJUMBO_NONull()?"null":"'"+row.JUMBO_NO.ToString()+"'",
            //row.IsORDR_CUSTNull()?"null":"'"+row.ORDR_CUST.ToString()+"'",
            //row.IsCUST_NAMENull()?"null":"'"+row.CUST_NAME.ToString()+"'",
            //row.IsORDR_NONull()?"null":"'"+row.ORDR_NO.ToString()+"'",
            //row.IsORDR_CUST_EXNull()?"null":"'"+row.ORDR_CUST_EX.ToString()+"'",
            //row.IsORDR_PROD_SEQNull()?"null":"'"+row.ORDR_PROD_SEQ.ToString()+"'",
            //row.IsCUTPOSNull()?"null":"'"+row.CUTPOS.ToString()+"'",
            //row.IsCORLORNull()?"null":"'"+row.CORLOR.ToString()+"'",
            //row.IsBONEDNull()?"null":"'"+row.BONED.ToString()+"'",
            //row.IsPRINT_MARK_1Null()?"null":"'"+row.PRINT_MARK_1.ToString()+"'",
            //row.IsPRINT_MARK_2Null()?"null":"'"+row.PRINT_MARK_2.ToString()+"'",
            //row.IsPRINT_MARK_3Null()?"null":"'"+row.PRINT_MARK_3.ToString()+"'",
            //row.IsPRINT_MARK_4Null()?"null":"'"+row.PRINT_MARK_4.ToString()+"'",
            //row.IsPRINT_MARK_5Null()?"null":"'"+row.PRINT_MARK_5.ToString()+"'",
            //row.IsPRINT_MARK_6Null()?"null":"'"+row.PRINT_MARK_6.ToString()+"'",
            //row.IsOEM_MACHNull()?"null":"'"+row.OEM_MACH.ToString()+"'",
            //row.IsMIS_WRITE_DATENull()?"null":"'"+row.MIS_WRITE_DATE.ToString()+"'",
            //row.IsMIS_WRITE_TIMENull()?"null":"'"+row.MIS_WRITE_TIME.ToString()+"'",
            //row.IsIPC_WRITE_DATENull()?"null":"'"+row.IPC_WRITE_DATE.ToString()+"'",
            //row.IsIPC_WRITE_TIMENull()?"null":"'"+row.IPC_WRITE_TIME.ToString()+"'",
            //row.IsIPC_WRITE_DATE2Null()?"null":"'"+row.IPC_WRITE_DATE2.ToString()+"'",
            //row.IsIPC_WRITE_TIME2Null()?"null":"'"+row.IPC_WRITE_TIME2.ToString()+"'",
            //row.IsIPC_WRITE_MARKNull()?"null":"'"+row.IPC_WRITE_MARK.ToString()+"'",
            //row.IsPrdnrNull()?"null":"'"+row.Prdnr.ToString()+"'",
            //row.IsBottomNull()?"null":"'"+row.Bottom.ToString()+"'",
            //row.IsReasonNull()?"null":"'"+row.Reason.ToString()+"'",
            //row.IsC_CORE_TYPENull()?"null":"'"+row.C_CORE_TYPE.ToString()+"'",
            //row.IsPrint_typeNull()?"null":"'"+row.Print_type.ToString()+"'"});



             
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnctionString))
            {
                try
                {
                    NpgSqlHelper.ExecuteNonQuery(connection,
                        CommandType.Text,
                        sqltext

                        );
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return true;
        }




    }



    ////数据库连接 断开 事件
    //public class SqlStateEventArgs : EventArgs
    //{
    //    private bool _IsConnect;
    //    private string _Info;
    //    public SqlStateEventArgs(bool isConnect, string msg)
    //    {
    //        this._IsConnect = isConnect;
    //        this._Info = msg;

    //    }
    //    public bool IsConnect
    //    {
    //        get
    //        {
    //            return _IsConnect;
    //        }
    //    }
    //    public string Info
    //    {
    //        get
    //        {
    //            return _Info;
    //        }
    //    }
    //}
}
