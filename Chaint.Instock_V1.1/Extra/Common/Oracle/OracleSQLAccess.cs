using System;
using System.Data;
using System.Collections.Generic;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;

using System.Data.OleDb;
using System.Text;
using DataModel;
namespace Common.Oracle
{ 

    public class OracleSQLAccess
    {
        private string ConnctionString = "";

        //Oracle SchemeName
        //private string SchemaName = "MILL";
        private string SchemaName = "APG";


        public DateTime LastAccessTime = Utils.DateTimeNow;
        public Int32 ExecuteCout = 0;

        /// <summary>
        /// Initializes a new instance of the SQLAccess class.
        /// </summary>
        public OracleSQLAccess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            //ConnctionString = Utils.OracleConnectionString;
            //Provider=MSDAORA.1;
            //ConnctionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.56.101)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=system;Password=icemile;";

            //ConnctionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.1.0.237)(PORT=1522)))(CONNECT_DATA=(SERVICE_NAME=milab)));Persist Security Info=True;User Id=CHAINT;Password=tniach;";
           
            
            //memo OK
           // ConnctionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.214.0.65)(PORT=1522)))(CONNECT_DATA=(SERVICE_NAME=milab)));Persist Security Info=True;User Id=CHAINT;Password=tniahc;";
           ConnctionString = Utils.OracleConnectionString  ;

            //ConnctionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.56.102)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));Persist Security Info=True;User Id=MILL;Password=mill;";



            //ConnctionString = "Provider =OraOLEDB.Oracle;DataSource= MILAB.WINDSORLOCKS.AHLSTROM.COM; User_ID=APG_LK; Password=x;";            

            //ConnctionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.56.102)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));Persist Security Info=True;User Id=sape68;Password=sape68;";
            //ConnctionString = "Provider =OraOLEDB.Oracle;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.56.102)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));Persist Security Info=True;User Id=sape68;Password=sape68;";

            

        }

        public bool _IsConect = false;
        public bool IsConnect
        {
            get { return _IsConect; }
        }

        public delegate void SqlStateEventHandler(object sender,
               SqlStateEventArgs e);


        public event SqlStateEventHandler SqlStateChange;


        protected virtual void OnSqlStateChange(SqlStateEventArgs e)
        {
            if (SqlStateChange != null)
            {
                ExecuteCout++;
                LastAccessTime = Utils.DateTimeNow;

                _IsConect = e.IsConnect;
                SqlStateChange(this, e);//Raise the event
            }
        }

        public bool TestConnection()
        {
            if (ConnctionString == "")
                return false;

            OleDbConnection conn = new OleDbConnection();
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

        //Get Oracle Systemtime, for keepalive
        public string SYSDATEQuery()
        {
            string sysdatestr = "";
            string tempsqltext = "select sysdate from dual";
            using (OracleConnection connection = new OracleConnection(ConnctionString))
            {
                try
                {
                   object reto = Oracle.OracleDataDbHelper.ExecuteScalar(connection,
                        CommandType.Text,
                        tempsqltext
                        );
                   sysdatestr = Convert.ToString(reto);

                    OnSqlStateChange(new SqlStateEventArgs(true, "SYSDATEQuery执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SYSDATEQuery"+ex.ToString()));
                }
            }
            return sysdatestr;
        }


        //LO_CHTOMILLUI
        public bool LO_CHTOMILLUIInsert(string rollid,  DateTime Idate)
        {
            //LO_CHTOMILLUI
            bool ret = false;
            //UNITITEMCODE  rollid 
            string tempsqltext = "insert into " + SchemaName + ".LO_CHTOMILLUI(UNITITEMCODE,OID,IDATE,IBY) values("
            + "'{0}','{1}',{2},'{3}')";           

            using (OracleConnection connection = new OracleConnection(ConnctionString))
            {                 
                int maxindex = 0;
                //获取序列号
                string maxoid = "";
                try
                {
                    maxoid = Convert.ToString(Oracle.OracleDataDbHelper.ExecuteScalar(connection,
                    CommandType.Text,
                        //OID : CHAINT-20120914-1 
                    "select max(to_number(substr(OID,17))) from " + SchemaName + ".LO_CHTOMILLUI where to_char( IDATE ,'yyyyMMdd')= '" + Idate.ToString("yyyyMMdd") + "'"
                    ));

                    OnSqlStateChange(new SqlStateEventArgs(true, "SAPLO_CHTOMILLUIInsert->select max:成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SAPLO_CHTOMILLUIInsert->select max:" + ex.ToString()));
                }


                if (maxoid == "")
                {
                    maxindex = 1;
                }
                else
                {
                    try
                    {
                        maxindex = Convert.ToInt32(maxoid) + 1;
                    }
                    catch { }
                }


                string sqltext = String.Format(tempsqltext, new string[] { 
                rollid,
                "CHAINT-"+Idate.ToString("yyyyMMdd")+"-"+maxindex.ToString(),
                "to_date('"+Idate+"','yyyy/mm/dd hh24:mi:ss')",
                "CHAINT"
             });


                try
                {                 
                    int i = -1;
                    i = Oracle.OracleDataDbHelper.ExecuteNonQuery(connection,
                  CommandType.Text,
                  sqltext
                  );

                    if (i == 1)
                        ret = true;
                    else
                        ret = false;

                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SAPLO_CHTOMILLUIInsert成功"));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SAPLO_CHTOMILLUIInsert:" + ex.ToString()));
                }
            }
            return ret;
        }



        //LO_MILLTOCHUI
        public DataSet LO_MILLTOCHUIQueryByUNITITEMCODE(string UNITITEMCODE)
        {
            DataSet ds = new DataSet();
            string tempsqltext = "select * from " + SchemaName + ".LO_MILLTOCHUI where UNITITEMCODE ='" + UNITITEMCODE + "'";

            using (OracleConnection connection = new OracleConnection(ConnctionString))
            {
                try
                {
                    Oracle.OracleDataDbHelper.FillDataset(connection,
                        CommandType.Text,                   
                        tempsqltext,
                        ds,
                         new string[] { "LO_MILLTOCHUI" }
                        );

                    OnSqlStateChange(new SqlStateEventArgs(true, "LO_MILLTOCHUIQueryByUNITITEMCODE执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        public bool LO_MILLTOCHUIDeleteByUNITITEMCODE(string UNITITEMCODE)
        {
            bool ret = false;
            string tempsqltext = "delete from " + SchemaName + ".LO_MILLTOCHUI where UNITITEMCODE ='" + UNITITEMCODE + "'";

            using (OracleConnection connection = new OracleConnection(ConnctionString))
            {
                try
                {
                    Oracle.OracleDataDbHelper.ExecuteNonQuery(connection,
                        CommandType.Text,
                        tempsqltext
                        );
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "LO_MILLTOCHUIDeleteByUNITITEMCODE执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ret;
        }
     
        //LO_CHTOMILLUIWEIG
        public bool LO_CHTOMILLUIWEIGInsert(string rollid, float weight, DateTime Idate)
        {
            //LO_CHTOMILLUI
            bool ret = false;

            //UNITITEMCODE  rollid

            string tempsqltext = "insert into " + SchemaName + ".LO_CHTOMILLUIWEIG(UNITITEMCODE,WEIG,OID,IDATE,IBY) values("
            + "'{0}',{1},'{2}',{3},'{4}')";

            

            using (OracleConnection connection = new OracleConnection(ConnctionString))
            {

                int maxindex = 0;
                //获取序列号
                string maxoid = "";

                try
                {
                    maxoid = Convert.ToString(Oracle.OracleDataDbHelper.ExecuteScalar(connection,
                  CommandType.Text,
                        //OID : CHAINT-20120914-1 
                  "select max(to_number(substr(OID,17))) from " + SchemaName + ".LO_CHTOMILLUIWEIG where to_char( IDATE ,'yyyyMMdd')= '" + Idate.ToString("yyyyMMdd") + "'"
                  ));
                    OnSqlStateChange(new SqlStateEventArgs(true, "LO_CHTOMILLUIWEIGInsert->select max:成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LO_CHTOMILLUIWEIGInsert->select max:" + ex.ToString()));
                }



                if (maxoid == "")
                {
                    maxindex = 1;
                }
                else
                {
                    try
                    {
                        maxindex = Convert.ToInt32(maxoid) + 1;
                    }
                    catch { }
                }
                string sqltext = String.Format(tempsqltext, new string[] { 
                rollid,
                weight.ToString(),
                "CHAINT-"+Idate.ToString("yyyyMMdd")+"-"+maxindex.ToString(),
                "to_date('"+Idate+"','yyyy/mm/dd hh24:mi:ss')",
                "CHAINT"
             });
                try
                {
                    int i = -1;
                    i = Oracle.OracleDataDbHelper.ExecuteNonQuery(connection,
                  CommandType.Text,
                  sqltext
                  );

                    if (i == 1)
                        ret = true;
                    else
                        ret = false;

                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SAPLO_CHTOMILLUIWEIGInsert成功"));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SAPLO_CHTOMILLUIWEIGInsert:" + ex.ToString()));
                }
            }
            return ret;
        }


        //LO_CHTOMILLUNIT
        public bool LO_CHTOMILLUNITInsert(string rollids, DateTime Idate)
        {
            bool ret = false;
            string tempsqltext = "insert into " + SchemaName + ".LO_CHTOMILLUNIT(ROLLSINUNIT,OID,IDATE,IBY) values("
            + "'{0}','{1}',{2},'{3}')";

            using (OracleConnection connection = new OracleConnection(ConnctionString))
            {
                int maxindex = 0;
                //获取序列号
                string maxoid = "";
                try
                {
                    maxoid = Convert.ToString(Oracle.OracleDataDbHelper.ExecuteScalar(connection,
                  CommandType.Text,
                        //OID : CHAINT-20120914-1 
                  "select max(to_number(substr(OID,17))) from " + SchemaName + ".LO_CHTOMILLUNIT where to_char(IDATE ,'yyyyMMdd')= '" + Idate.ToString("yyyyMMdd") + "'"
                  ));
                    OnSqlStateChange(new SqlStateEventArgs(true, "LO_CHTOMILLUNITInsert->select max:成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LO_CHTOMILLUNITInsert->select max:" + ex.ToString()));
                }


                if (maxoid == "")
                {
                    maxindex = 1;
                }
                else
                {
                    try
                    {
                        maxindex = Convert.ToInt32(maxoid) + 1;
                    }
                    catch { }
                }

                string sqltext = String.Format(tempsqltext, new string[] { 
                rollids,
                "CHAINT-"+Idate.ToString("yyyyMMdd")+"-"+maxindex.ToString(),
                "to_date('"+Idate+"','yyyy/mm/dd hh24:mi:ss')",
                "CHAINT"
             });

                try
                {
                    int i = -1;
                    i = Oracle.OracleDataDbHelper.ExecuteNonQuery(connection,
                  CommandType.Text,
                  sqltext
                  );

                    if (i == 1)
                        ret = true;
                    else
                        ret = false;

                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "LO_CHTOMILLUNITInsert成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LO_CHTOMILLUNITInsert:" + ex.ToString()));
                }
            }
            return ret;
        }


        //GENERRLOG
        //public bool GENERRLOGInsert(string MILLCODE,string MACHCODE,string WORKSTATION,
        //    DateTime LOGTIME,string LEGEND,string OID,string UCNT,string IBY ,DateTime IDATE,
        //    string UBY,DateTime UDATE)

        public bool GENERRLOGInsert( string LEGEND, DateTime IDATE)
        {       
            bool ret = false; 
            string tempsqltext = "insert into " + SchemaName + ".GENERRLOG(MILLCODE,MACHCODE,WORKSTATION,LOGTIME,LEGEND,OID,UCNT,IBY,IDATE,UBY,UDATE) values("
            + "'{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}',{8},'{9}',{10})";

            using (OracleConnection connection = new OracleConnection(ConnctionString))
            {

                int maxindex = 0;
                //获取序列号
                string maxoid = "";
                try
                {
                    maxoid = Convert.ToString(Oracle.OracleDataDbHelper.ExecuteScalar(connection,
                  CommandType.Text,
                        //OID : CHAINT-20120914-1 
                  "select max(to_number(substr(OID,17))) from " + SchemaName + ".GENERRLOG where to_char( IDATE ,'yyyyMMdd')= '" + IDATE.ToString("yyyyMMdd") + "'"
                  ));
                    OnSqlStateChange(new SqlStateEventArgs(true, "GENERRLOGInsert->select max:成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "GENERRLOGInsert->select max:" + ex.ToString()));
                }

                if (maxoid == "")
                {
                    maxindex = 1;
                }
                else
                {
                    try
                    {
                        maxindex = Convert.ToInt32(maxoid) + 1;
                    }
                    catch { }
                }

                string sqltext = String.Format(tempsqltext, new string[] {
                    "LONGKOU",//MILLCODE,
                    "LOWR01",//MACHCODE,
                    "LOWR01",//WORKSTATION,
                    "to_date('"+Utils.DateTimeNow+"','yyyy/mm/dd hh24:mi:ss')",//"to_date('"+LOGTIME+"','yyyy/mm/dd hh24:mi:ss')",
                    LEGEND,
                     "CHAINT-"+IDATE.ToString("yyyyMMdd")+"-"+maxindex.ToString(),//OID,
                    "0",//UCNT,
                    "CHAINT",//IBY,
                    "to_date('"+Utils.DateTimeNow+"','yyyy/mm/dd hh24:mi:ss')",//"to_date('"+IDATE+"','yyyy/mm/dd hh24:mi:ss')",
                    "CHAINT",//UBY,
                    "to_date('"+Utils.DateTimeNow+"','yyyy/mm/dd hh24:mi:ss')"//"to_date('"+UDATE+"','yyyy/mm/dd hh24:mi:ss')"              
             });


                try
                {
                    int i = -1;
                    i = Oracle.OracleDataDbHelper.ExecuteNonQuery(connection,
                  CommandType.Text,
                  sqltext
                  );

                    if (i == 1)
                        ret = true;
                    else
                        ret = false;

                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "GENERRLOGInsert成功"));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "GENERRLOGInsert:" + ex.ToString()));
                }
            }
            return ret;
        }





        //Roll
        //public bool SAPRollInsertByRow(MainDS.Roll_TableRow row)
        //{
        //    //SDMS.ROLL
        //    bool ret = false;

        //    string tempsqltext = "insert into SDMS.ROLL(Company_id,Roll_no,Paper_type,Paper_size,Uom,Weight,Length,Diameter,Broll_no,Production_date,Stock_in_date,Section,Machine_id,Warehouse,Location,Remark,Previous_roll_no,Status,Create_datetime,Create_by,Factory_id) values("
        //    + "'{0}','{1}','{2}',{3},'{4}',{5},{6},{7},'{8}',{9},{10},'{11}',{12},'{13}','{14}','{15}','{16}','{17}',{18},'{19}','{20}')";

        //    string sqltext = String.Format(tempsqltext, new string[] { 
        //        row.Company_id,
        //        row.Rollid,
        //        row.Paper_type,
        //        row.Paper_size.ToString(),
        //        row.Uom,
        //        row.Weight.ToString(),
        //        row.Length.ToString(),
        //        row.Diameter.ToString(),
        //        row.BigRoll,
        //        "to_date('"+row.Production_date.ToString()+"','yyyy/mm/dd hh24:mi:ss')",
        //        "to_date('"+row.Stock_in_date.ToString()+"','yyyy/mm/dd hh24:mi:ss')",
        //        row.Shift_s,
        //        row.Machineid,
        //        row.Warehouse,
        //        row.Location,				
        //        Utils.SimTOTra( row.Remark),
        //        row.IsPrevious_roll_noNull()?"":row.Previous_roll_no,
        //        row.Status,
        //        "to_date('"+Utils.DateTimeNow.ToString()+"','yyyy/mm/dd hh24:mi:ss')",
        //        row.Create_by,
        //        row.Factory_id            
        //     });
          
        //    using (OleDbConnection connection = new OleDbConnection(ConnctionString))
        //    {
        //        try
        //        {
        //            //object k = Oracle.OracleOleDbHelper.ExecuteScalar(connection,
        //            //   CommandType.Text,
        //            //   "select count(*) from SAPE68.ZPH02 WHERE CHARG='" + charg + "'"
        //            //   );

        //            //if (Convert.ToInt32(k) > 0)  //如果数据库存在则返回true
        //            //    return true;

        //            int i = -1;
        //            i = Oracle.OracleOleDbHelper.ExecuteNonQuery(connection,
        //          CommandType.Text,
        //          sqltext
        //          );

        //            if (i == 1)
        //                ret = true;
        //            else
        //                ret = false;



        //            ret = true;
        //            OnSqlStateChange(new SqlStateEventArgs(true, "SAPRollInsertByRow成功"));


        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "SAPRollInsertByRow:"+ex.ToString()));
        //        }
        //    }
        //    return ret;
        //}

        public bool SAPRollIsExistByRollid(string rollid)
        {
            //SDMS.ROLL
            bool ret = false;


            string sqltext = "select count(*) from SDMS.ROLL where ROLL_NO='"+rollid+"'";

            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    object k = Oracle.OracleOleDbHelper.ExecuteScalar(connection,
                       CommandType.Text,
                       sqltext
                       );

                    if (Convert.ToInt32(k) > 0)  //如果数据库存在则返回true
                        ret = true;


                    OnSqlStateChange(new SqlStateEventArgs(true, "SAPRollIsExistByRollid成功"));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false,"SAPRollIsExistByRollid:"+ ex.ToString()));
                }
            }
            return ret;
        }





        //ZPH01 查询
        public DataSet ZPH01QueryForAutoSAPLoad(string mach)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    Oracle.OracleOleDbHelper.FillDataset(connection,
                        CommandType.Text,
                         //"select * from SAPE68.ZPH01 where  ROWNUM<2000 and   MACH ='" + mach + "'",
                        "select * from SAPE68.ZPH01 where  ROWNUM<2000 and IPC_WRITE_DATE =0 and MACH ='" + mach + "'",
                        ds,
                         new string[] { "ZPH01" }
                        );

                    OnSqlStateChange(new SqlStateEventArgs(true, "ZPH01QueryForAutoSAPLoad成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }
          
        public bool ZPH01UpdateWriteTimeByPK(DateTime ipc_write_datetime, string MANDT, string CHARG)
        {

            string date = ipc_write_datetime.ToString(Utils.FormatDate);
            string time = ipc_write_datetime.ToString(Utils.FormatTime);

            string sqltext = "update SAPE68.ZPH01 set IPC_WRITE_DATE ='" + date + "',IPC_WRITE_TIME='" + time + "' where MANDT='" + MANDT + "' and CHARG='" + CHARG + "'";

            bool ret = false;
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    Oracle.OracleOleDbHelper.ExecuteNonQuery(connection,
                        CommandType.Text,
                        sqltext

                        );

                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "ZPH01UpdateWriteTimeByPK成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ret;
        }

        public DataSet ZPH01QueryByCHARG(string CHARG)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    Oracle.OracleOleDbHelper.FillDataset(connection,
                        CommandType.Text,                        
                        "select * from SAPE68.ZPH01 where CHARG ='" + CHARG + "'",
                        ds,
                         new string[] { "ZPH01" }
                        );

                    OnSqlStateChange(new SqlStateEventArgs(true, "ZPH01QueryByCHARG成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }


        public DataSet ZPH01QueryByMISTIME(DateTime begintime,DateTime endtime)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                string begindatestr =  begintime.ToString(Utils.FormatDate);
                string begintimestr = begintime.ToString(Utils.FormatTime);

                string enddatestr = endtime.ToString(Utils.FormatDate);
                string endtimestr = endtime.ToString(Utils.FormatTime);


                string mach = Utils.Machine;

                //string sqltext = "select * from SAPE68.ZPH01 where MACH ='" + mach + "' and MIS_WRITE_DATE>'" + begindatestr + "' and MIS_WRITE_DATE<'" + enddatestr + "' and MIS_WRITE_TIME>'" + begintimestr + "' and MIS_WRITE_TIME<'" + endtimestr + "'";
                string sqltext = "select * from SAPE68.ZPH01 where MACH ='" + mach + "' and MIS_WRITE_DATE>'" + begindatestr + "' and MIS_WRITE_DATE<'" + enddatestr + "'";
                try
                {
                    Oracle.OracleOleDbHelper.FillDataset(connection,
                        CommandType.Text,
                        sqltext,
                        ds,
                         new string[] { "ZPH01" }
                        );

                    OnSqlStateChange(new SqlStateEventArgs(true, "ZPH01QueryByMISTIME成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }


        public int ZPH01QueryCountByMISTIME(DateTime begintime, DateTime endtime)
        {
            int ret = -1;
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                string begindatestr = begintime.ToString(Utils.FormatDate);
                string begintimestr = begintime.ToString(Utils.FormatTime);

                string enddatestr = endtime.ToString(Utils.FormatDate);
                string endtimestr = endtime.ToString(Utils.FormatTime);
                string mach = Utils.Machine;

                //string sqltext = "select count(*) from SAPE68.ZPH01 where MACH ='"+mach+"' and MIS_WRITE_DATE>'" + begindatestr + "' and MIS_WRITE_DATE<'" + enddatestr + "' and MIS_WRITE_TIME>'" + begintimestr + "' and MIS_WRITE_TIME<'" + endtimestr + "'";
                string sqltext = "select count(*) from SAPE68.ZPH01 where MACH ='" + mach + "' and MIS_WRITE_DATE>'" + begindatestr + "' and MIS_WRITE_DATE<'" + enddatestr +  "'";

                try
                {
                   object obj = Oracle.OracleOleDbHelper.ExecuteScalar(connection,
                        CommandType.Text,
                        sqltext
                        );

                   ret = Convert.ToInt32(obj);

                   OnSqlStateChange(new SqlStateEventArgs(true, "ZPH01QueryCountByMISTIME成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }

            return ret;
          
        }



        //ZPH02
        public bool ZPH02InsertJetInfo(string charg, int real_wet,DateTime budatetime,DateTime audatetime,DateTime ipc_write_datetime,string jumbo_no,string jumbo_class,int real_lenth,int width_m)
        {

            string mandt = "168";
            string print_ok = "Y";

            string budat = budatetime.ToString(Utils.FormatDate);
            string audat = audatetime.ToString(Utils.FormatDate);

            string ipc_write_date = ipc_write_datetime.ToString(Utils.FormatDate);
            string ipc_write_time = ipc_write_datetime.ToString(Utils.FormatTime);

            string mis_write_date = "00000000";
            string mis_write_time = "000000";

            string tempsqltext = "insert into SAPE68.ZPH02(MANDT,CHARG,PRINT_OK,REAL_WET,BUDAT,AUDAT,IPC_WRITE_DATE,IPC_WRITE_TIME,MIS_WRITE_DATE,MIS_WRITE_TIME,JUMBO_NO,JUMBO_CLASS,REAL_LENGTH,S_SIZE_ACT) values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13}) ";

            string sqltext = String.Format(tempsqltext, new string[] { mandt, charg, print_ok, real_wet.ToString(), budat, audat, ipc_write_date, ipc_write_time, mis_write_date, mis_write_time, jumbo_no, jumbo_class, real_lenth.ToString(), width_m.ToString() });
            
            
            bool ret = false;
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    object k = Oracle.OracleOleDbHelper.ExecuteScalar(connection,
                       CommandType.Text,
                       "select count(*) from SAPE68.ZPH02 WHERE CHARG='" + charg + "'"
                       );

                    if (Convert.ToInt32(k) > 0)  //如果数据库存在则返回true
                        return true;

                    int i = -1;
                    i = Oracle.OracleOleDbHelper.ExecuteNonQuery(connection,
                  CommandType.Text,
                  sqltext
                  );

                    if (i == 1)
                        ret = true;
                    else
                        ret = false;

                   

                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "ZPH02InsertJetInfo成功"));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ret;
        }

        public bool ZPH02UpdateData_M(string charg, int real_wet,int width_m)
        {

            string sqltext = "update zph02 set S_SIZE_ACT =" + width_m.ToString() + ",REAL_WET = " + real_wet.ToString()+ "  where CHARG = '" + charg + "'";

            

            bool ret = false;
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    

                   Oracle.OracleOleDbHelper.ExecuteNonQuery(connection,
                  CommandType.Text,
                  sqltext
                  ); 


                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "ZPH02UpdateData_M成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ret;
        }




        //ZPH02B
        public bool ZPH02InsertJetInfo(int seq_no, string charg, int real_wet, DateTime budatetime, DateTime audatetime, DateTime ipc_write_datetime, string jumbo_no, string jumbo_class, int real_lenth)
        {

            string mandt = "168";
            string print_ok = "Y";

            string budat = budatetime.ToString(Utils.FormatDate);
            string audat = audatetime.ToString(Utils.FormatDate);

            string ipc_write_date = ipc_write_datetime.ToString(Utils.FormatDate);
            string ipc_write_time = ipc_write_datetime.ToString(Utils.FormatTime);

            string mis_write_date = "00000000";
            string mis_write_time = "000000";

            string tempsqltext = "insert into SAPE68.ZPH02B(SEQ_NO,MANDT,CHARG,PRINT_OK,REAL_WET,BUDAT,AUDAT,IPC_WRITE_DATE,IPC_WRITE_TIME,MIS_WRITE_DATE,MIS_WRITE_TIME,JUMBO_NO,JUMBO_CLASS,REAL_LENTH) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}) ";

            string sqltext = String.Format(tempsqltext, new string[] {seq_no.ToString(), mandt, charg, print_ok, real_wet.ToString(), budat, audat, ipc_write_date, ipc_write_time, mis_write_date, mis_write_time, jumbo_no, jumbo_class, real_lenth.ToString() });


            bool ret = false;
            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    Oracle.OracleOleDbHelper.ExecuteNonQuery(connection,
                        CommandType.Text,
                        sqltext

                        );

                    ret = true;

                    OnSqlStateChange(new SqlStateEventArgs(true, "ZPH02InsertJetInfo成功"));

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ret;
        }


        //Roll_Product
        public DataSet Roll_ProductQueryByPK(string MANDT, string CHARG)
        {
            DataSet ds = new DataSet();
            OleDbConnection connection = new OleDbConnection(ConnctionString);
           // using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    Oracle.OracleOleDbHelper.FillDataset(connection,
                        CommandType.Text,
                        "select * from SAPE68.Roll_Product",
                        ds,
                         new string[] { "Roll_Product" }
                        );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductQueryByPK成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        } 

        //TEST big5
        //ZPH02
        public string TestBIG5()
        {
            string ret = "";

            Encoding sapencode = Encoding.GetEncoding("utf-7");
            Encoding demagencode = Encoding.UTF8;
            DataSet ds = new DataSet();
        
            //System.Data.OleDb.OleDbConnection 

            using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            //using (OleDbConnection connection = new OleDbConnection(ConnctionString))
            {
                try
                {
                    //Oracle.OracleOleDbHelper.FillDataset(connection,
                    //  CommandType.Text,
                    //    //"select  *  from zph01 where charg = '1AR9036877'",
                    //  "select CHARG,MAKTX,CUST_NAME,REASON from zph01  where MACH='A'",
                    //  ds,
                    //  new string[] { "tb1" }
                    //  );

                    System.Data.OleDb.OleDbCommand cmd = new OleDbCommand(
                        "select   REASON from zph01 where charg = '1AR9036616'",
                        connection);
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    connection.Close();


                     //ret = Convert.ToString(obj);  //如果数据库存在则返回true


                   // string sapstr = demagencode.GetString(sapencode.GetBytes(ret));

                    byte[] srcbs = Encoding.Default.GetBytes(obj.ToString());
                    //byte[] tobs = Encoding.Convert(Encoding.Default, Encoding.UTF8, srcbs);    //Encoding.GetEncoding(i2.CodePage).GetString(Encoding.GetEncoding(i1.CodePage).GetBytes(ret));

                    string aa = Encoding.GetEncoding("big5").GetString(srcbs);
                    //string dd = Encoding.GetEncoding("big5").GetString(tobs);


                     //foreach (EncodingInfo i1 in Encoding.GetEncodings())
                     //{
                         
                    //    foreach (EncodingInfo i2 in Encoding.GetEncodings())
                    //    {

                    //       byte[] tobs = Encoding.Convert(Encoding.GetEncoding(i1.CodePage), Encoding.GetEncoding(i2.CodePage), srcbs);   
                            
                    //        //string dd = Encoding.
                    //        //string dd = Encoding.Convert(Encoding.GetEncoding(i1.CodePage), Encoding.GetEncoding(i2.CodePage), srcbs);    //Encoding.GetEncoding(i2.CodePage).GetString(Encoding.GetEncoding(i1.CodePage).GetBytes(ret));
                           

              

                    //        // Utils.WriteTxtLog(@"F:\sasas.txt", i1.DisplayName + "--->" + i2.DisplayName + "==" + dd);
 


                    //       // Utils.WriteTxtLog(@"F:\sasas.txt", i1.DisplayName + "--->" + i2.DisplayName + "==" + dd);

                    //    }
                    //}

                  

             //  string dd =  demagencode.GetString(  sapencode.GetBytes(ret));
                   

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ret;
        }

        
    }
}
