using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using DataModel;
using System.IO;
using System.Transactions;
namespace CTWH.Common.MSSQL
{
    public partial class MSSQLAccess
    {

        /// <summary>
        /// 工厂对象
        /// </summary>
     //   private DBAccess.IADOFactory factory;
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

        /// <summary>
        /// Initializes a new instance of the SQLAccess class.
        /// </summary>
        public MSSQLAccess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnctionString = Utils.SQLConnectionString; 
        }

        public MSSQLAccess(string connectionstr)
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
      /// <summary>
      /// 查询所有的用户
      /// CTWMS
      /// </summary>
      /// <returns></returns>
        public DataSet GetAllUser()
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.Text,
                        "select Onlyid,UserName,UserCode,Password,UserTypeID,ShiftId from T_User where ischoose ='1'",
                        ds,
                        new string[] { "T_User" }
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "GetAllUsers查询成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }


        //Paper_Grade
        public MainDS Paper_GradeQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from Paper_Grade where IsChoose=1;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Grade" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_GradeQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_GradeQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        //Paper_Inspector
        public MainDS Paper_InspectorQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from Paper_Inspector where IsChoose=1;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Inspector" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_InspectorQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_InspectorQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public bool Paper_InspectorInsertByValue(String Inspector, String Inspector_Desc, String Inspector_Print, Boolean IsChoose)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO Paper_Inspector( Inspector,Inspector_Desc,Inspector_Print,IsChoose)VALUES(@Inspector,@Inspector_Desc,@Inspector_Print,@IsChoose);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@Inspector", Inspector),
                new SqlParameter("@Inspector_Desc", Inspector_Desc),
                new SqlParameter("@Inspector_Print", Inspector_Print),
                new SqlParameter("@IsChoose", IsChoose)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_InspectorInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_InspectorInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        //Paper_Machine
        public MainDS Paper_MachineQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from Paper_Machine where IsChoose=1;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Machine" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_MachineQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_MachineQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        //Paper_Rewinder
        public MainDS Paper_RewinderQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from Paper_Rewinder where IsChoose=1;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Rewinder" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_RewinderQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_RewinderQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        //Paper_Shift
        public MainDS Paper_ShiftQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from Paper_Shift where IsChoose=1;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Shift" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_ShiftQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_ShiftQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        //Paper_Standard
        public MainDS Paper_StandardQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from Paper_Standard where IsChoose=1;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Standard" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_StandardQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_StandardQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        //Paper_Type
        public MainDS Paper_TypeQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from Paper_Type where IsChoose=1;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Type" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_TypeQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Paper_TypeQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }


        //Paper_Grade
        //Paper_Inspector
        //Paper_Machine
        //Paper_Rewinder 
        //Paper_Shift 
        //Paper_Standard
        //Paper_Type
        //Paper_DegradeCause
        public MainDS PageInitDSQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr =
                    "select * from Paper_Grade where IsChoose=1;" +
                    "select * from Paper_Inspector where IsChoose=1;" +
                    "select * from Paper_Machine where IsChoose=1;" +
                    "select * from Paper_Rewinder where IsChoose=1;" +
                    "select * from Paper_Shift where IsChoose=1;" +
                    "select * from Paper_Standard where IsChoose=1;" +
                    "select * from Paper_Type where IsChoose=1;"+
                     "select * from Paper_DegradeCause where IsChoose=1;"+
                     "select * from Paper_Size where IsChoose=1;";   
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Grade", "Paper_Inspector", "Paper_Machine", "Paper_Rewinder", "Paper_Shift", "Paper_Standard", "Paper_Type", "Paper_DegradeCause" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "PageInitDSQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PageInitDSQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }


        //ERPInit Data
        public ERPDS ERPDSInitSQueryAll()
        {
            
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr =
                    "select top " + Utils.SelectERPDataRowsMax + " * from TOCT_Material order by CreateTime desc;" +
                    "select top " + Utils.SelectERPDataRowsMax + " * from TOCT_ORDER order by SO_DATE desc;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_Material", "TOCT_ORDER" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "ERPDSInitSQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "ERPDSInitSQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// ERPInit Data   ProductType 1=卷筒，2=平板
        /// </summary>
        /// <param name="type"></param>
        /// <param name="basisweight"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        /// <param name="ProductType"></param>
        /// <returns></returns>
        public ERPDS ERPDSInitSQuery(string type,string grade,int basisweight,int width,int core,  int length,int ProductType)
        {

            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string where = " where MaterialName like '%"+type+"%' and Grade ='"+grade+"' and basisweight ="+basisweight+" and width ="+width;
                if (ProductType == 1)
                {
                    where += " and PaperType='卷筒' and Core=" + core.ToString();
                }
                else if (ProductType == 2)
                {

                    where += " and PaperType='平板' and palletlength=" + length;
                }
                string sqlstr =
                    "select top " + Utils.SelectERPDataRowsMax + " * from TOCT_Material "+where+" order by CreateTime desc;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "TOCT_Material" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "ERPDSInitSQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "ERPDSInitSQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        
        
        //App_Parameter
        public MainDS App_ParameterQueryByType(string Type)
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from App_Parameter where Type = @Type;";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@Type", Type)          
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "App_Parameter" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "App_ParameterQueryByType OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "App_ParameterQueryByType Error:" + ex.ToString()));
                }
                return ds;
            }
        }


        //App_InkJetFormat
        public MainDS App_InkJetFormatQueryByFormatID(string FormatID)
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from App_InkJetFormat where FormatID = @FormatID;";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@FormatID", FormatID)          
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "App_InkJetFormat" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "App_InkJetFormatQueryByFormatID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "App_InkJetFormatQueryByFormatID Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public MainDS App_InkJetFormatQueryByID(Int16 FormatID, Int16 OrderID)
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from App_InkJetFormat where FormatID = @FormatID and  OrderID=@OrderID;";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@FormatID", FormatID),
                new SqlParameter("@OrderID", OrderID)               
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "App_InkJetFormat" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "App_InkJetFormatQueryByID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "App_InkJetFormatQueryByID Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        //PaperUser
        public MainDS PaperUserQueryByNameAndPWD(String UserName, String Password)
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from PaperUser where UserName = @UserName and  Password=@Password;";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName),
                new SqlParameter("@Password", Password)               
                };
                try
                {
                    connection.Open(); 
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "PaperUser" },
                          parameter);                    
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserQueryByNameAndPWD OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserQueryByNameAndPWD Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public MainDS PaperUserQueryByName(String UserName)
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from PaperUser where UserName = @UserName;";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName)              
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "PaperUser" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserQueryByName OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserQueryByName Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 通过姓名和密码登陆
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public MainDS T_UserQueryByNameAndPassword(String UserName,String Password)
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from T_User where UserName = @UserName and password =@Password;";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName)   ,
                new SqlParameter("@Password", Password) 
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_User" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_UserQueryByNameAndPassword OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_UserQueryByNameAndPassword Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 通过代码和密码登陆
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public WMSDS T_UserQueryByCodeAndPassword(String UserCode, String Password)
        {
            WMSDS ds = new WMSDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from T_User where UserCode = @UserCode and password =@Password;";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserCode", UserCode)   ,
                new SqlParameter("@Password", Password) 
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_User" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_UserQueryByCodeAndPassword OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_UserQueryByCodeAndPassword Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool PaperUserInsertByValue(String UserName, String Password, String Usertype, String UserShift, String Privilige, String Remark1, String Remark2, String Remark3)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO PaperUser( UserName,Password,Usertype,UserShift,Privilige,Remark1,Remark2,Remark3)VALUES(@UserName,@Password,@Usertype,@UserShift,@Privilige,@Remark1,@Remark2,@Remark3);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName),
                new SqlParameter("@Password", Password),
                new SqlParameter("@Usertype", Usertype),
                new SqlParameter("@UserShift", UserShift),
                new SqlParameter("@Privilige", Privilige),
                new SqlParameter("@Remark1", Remark1),
                new SqlParameter("@Remark2", Remark2),
                new SqlParameter("@Remark3", Remark3)                
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserInsertByValue OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserInsertByValue Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool PaperUserInsertByRow(MainDS.PaperUserRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "INSERT INTO PaperUser( UserName,Password,Usertype,UserShift,Privilige,Remark1,Remark2,Remark3)VALUES(@UserName,@Password,@Usertype,@UserShift,@Privilige,@Remark1,@Remark2,@Remark3);";

                    SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", row.UserName),
                new SqlParameter("@Password",row.IsPasswordNull()?DBNull.Value:(object)row.Password),
                new SqlParameter("@Usertype",row.IsUsertypeNull()?DBNull.Value:(object)row.Usertype),
                new SqlParameter("@UserShift",row.IsUserShiftNull()?DBNull.Value:(object)row.UserShift),
                new SqlParameter("@Privilige",row.IsPriviligeNull()?DBNull.Value:(object)row.Privilige),
                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3)                
                };

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserInsertByRow OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserInsertByRow Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool PaperUserDeleteByName(String UserName)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "delete from PaperUser where UserName = @UserName;";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName)              
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,                   
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserDeleteByName OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserDeleteByName Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public MainDS PaperUserQueryAllUserName()
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select UserName from PaperUser;"; 
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string []{"PaperUser"}
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserQueryAllUserName OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserQueryAllUserName Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public bool PaperUserUpdateAllByPK(MainDS.PaperUserRow row, bool IsUpdateDBNull)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "UPDATE PaperUser SET ";

                if (IsUpdateDBNull || !row.IsPasswordNull())
                    sqlstr = sqlstr + " Password = @Password,";
                if (IsUpdateDBNull || !row.IsUsertypeNull())
                    sqlstr = sqlstr + " Usertype = @Usertype,";
                if (IsUpdateDBNull || !row.IsUserShiftNull())
                    sqlstr = sqlstr + " UserShift = @UserShift,";
                if (IsUpdateDBNull || !row.IsPriviligeNull())
                    sqlstr = sqlstr + " Privilige = @Privilige,";
                if (IsUpdateDBNull || !row.IsRemark1Null())
                    sqlstr = sqlstr + " Remark1 = @Remark1,";
                if (IsUpdateDBNull || !row.IsRemark2Null())
                    sqlstr = sqlstr + " Remark2 = @Remark2,";
                if (IsUpdateDBNull || !row.IsRemark3Null())
                    sqlstr = sqlstr + " Remark3 = @Remark3,";
                sqlstr = sqlstr.TrimEnd(new char[] { ' ', ',' });
                sqlstr = sqlstr + " WHERE UserName = '" + row.UserName.ToString() + "'";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@UserName", row.UserName),
                                new SqlParameter("@Password",row.IsPasswordNull()?DBNull.Value:(object)row.Password),
                                new SqlParameter("@Usertype",row.IsUsertypeNull()?DBNull.Value:(object)row.Usertype),
                                new SqlParameter("@UserShift",row.IsUserShiftNull()?DBNull.Value:(object)row.UserShift),
                                new SqlParameter("@Privilige",row.IsPriviligeNull()?DBNull.Value:(object)row.Privilige),
                                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3)                                
                                };
                #endregion

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserUpdateAllByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="IsUpdateDBNull"></param>
        /// <returns></returns>
        public bool T_UserUpdateAllByPK(WMSDS.T_UserRow row, bool IsUpdateDBNull)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "UPDATE T_User SET ";

                if (IsUpdateDBNull || !row.IsPasswordNull())
                    sqlstr = sqlstr + " Password = @Password,";
                if (IsUpdateDBNull || !row.IsUserTypeIDNull())
                    sqlstr = sqlstr + " Usertype = @Usertype,";
                if (IsUpdateDBNull || !row.IsShiftIDNull())
                    sqlstr = sqlstr + " UserShift = @UserShift,";
                //if (IsUpdateDBNull || !row.IsPriviligeNull())
                //    sqlstr = sqlstr + " Privilige = @Privilige,";
                //if (IsUpdateDBNull || !row.IsRemark1Null())
                //    sqlstr = sqlstr + " Remark1 = @Remark1,";
                //if (IsUpdateDBNull || !row.IsRemark2Null())
                //    sqlstr = sqlstr + " Remark2 = @Remark2,";
                //if (IsUpdateDBNull || !row.IsRemark3Null())
                //    sqlstr = sqlstr + " Remark3 = @Remark3,";
                sqlstr = sqlstr.TrimEnd(new char[] { ' ', ',' });
                sqlstr = sqlstr + " WHERE UserCode = '" + row.UserCode.ToString() + "'";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@UserName", row.UserName),
                                new SqlParameter("@Password",row.IsPasswordNull()?DBNull.Value:(object)row.Password),
                                new SqlParameter("@Usertype",row.IsUserTypeIDNull()?DBNull.Value:(object)row.UserTypeID),
                                new SqlParameter("@UserShift",row.IsShiftIDNull()?DBNull.Value:(object)row.ShiftID),
                                //new SqlParameter("@Privilige",row.IsPriviligeNull()?DBNull.Value:(object)row.Privilige),
                                //new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                                //new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                                //new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3)                                
                                };
                #endregion

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_UserUpdateAllByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }
        }
        //PaperUserPrivilige
        public MainDS PaperUserPriviligeQueryByUserName(string UserName)
        {
            MainDS ds = new MainDS();

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from PaperUserPrivilige where UserName = @UserName;";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "PaperUserPrivilige" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserPriviligeQueryByUserName OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserPriviligeQueryByUserName Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool PaperUserPriviligeInsertByValue(string UserName, string PriviligeName, bool IsOK)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO PaperUserPrivilige( UserName,PriviligeName,IsOK) values(@UserName,@PriviligeName,@IsOK);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName),
                new SqlParameter("@PriviligeName", PriviligeName),
                new SqlParameter("@IsOK", IsOK)         
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserPriviligeInsertByValue OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserPriviligeInsertByValue Error:" + ex.ToString()));
                }
                return ret;
            }

        }
        public bool PaperUserPriviligeDeleteByName(string UserName)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "delete from PaperUserPrivilige where UserName = @UserName;";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@UserName", UserName)              
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserPriviligeDeleteByName OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserPriviligeDeleteByName Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public bool PaperUserPriviligeDeleteByNameAndPriviligeName(string UserName,string PriviligeName)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "delete from PaperUserPrivilige where UserName = @UserName and PriviligeName = @PriviligeName;";
                SqlParameter[] parameter = new SqlParameter[]
                {
                 new SqlParameter("@UserName", UserName) ,
                 new SqlParameter("@PriviligeName", PriviligeName) 
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserPriviligeDeleteByName OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PaperUserPriviligeDeleteByName Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        //PriviligeType
        public MainDS PriviligeTypeQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select * from PriviligeType;";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "PriviligeType" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "PriviligeTypeQueryAll OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "PriviligeTypeQueryAll Error:" + ex.ToString()));
                }
                return ds;
            }
        }


        //AlarmTitle
        public DefineDS AlarmTitleQueryALL(string PLCAddress)
        {
            DefineDS ds = new DefineDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.Text,
                        "select * from AlarmTitle where PLCAddress ='" + PLCAddress + "'",
                        ds,
                        new string[] { "AlarmTitle" }
                        );

                    OnSqlStateChange(new SqlStateEventArgs(true, "AlarmTitle读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;
        }


        //AlarmLog
        public int AlarmLogInsertByAlarmID(int AlarmID, DateTime BeginTime)
        {
            int ret = 0;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "insert into AlarmLog(AlarmID,BeginTime) values( @AlarmID,@BeginTime );SELECT @@IDENTITY";
                SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@AlarmID", AlarmID),
                    new SqlParameter("@BeginTime", BeginTime)
                };

                try
                {
                    object obj = MSSqlHelper.ExecuteScalar(connection,
                         CommandType.Text,
                         sqlstr,
                         parameter
                         );
                    ret = Convert.ToInt32(obj);

                    OnSqlStateChange(new SqlStateEventArgs(true, "AlarmLogInsertByAlarmID新增成功"));
                }
                catch (Exception ex)
                {
                    ret = -1;
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

                return ret;
            }
        }

        public void AlarmLogUpdateEndTimeByOnlyID(int OnlyID, DateTime EndTime)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "update AlarmLog set EndTime=@EndTime where OnlyID=@OnlyID";

                SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@OnlyID", OnlyID),
                    new SqlParameter("@EndTime", EndTime)
                };

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.Text,
                        sqlstr,
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "AlarmLogUpdateEndTimeByOnlyID更新成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
        }

        public DefineDS AlarmLogQueryByTime(DateTime BeginTime, DateTime EndTime, string PLCAddress)
        {
            DefineDS ds = new DefineDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select BeginTime,EndTime,Comment from AlarmLog  inner join AlarmTitle on AlarmLog.AlarmID = AlarmTitle.OnlyID where BeginTime between @BeginTime and @EndTime and AlarmTitle.PLCAddress = @PLCAddress";

                SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@BeginTime", BeginTime),
                    new SqlParameter("@EndTime", EndTime),
                    new SqlParameter("@PLCAddress", PLCAddress)
                };

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.Text,
                         sqlstr,
                         ds,
                         new string[] { "AlarmLogShow" },
                         parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "AlarmLogQueryByTime查询成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;

        }

        public bool AlarmLogDeleteBeforeMonth()
        {
            return AlarmLogDeleteBeforeDays(30);
        }

        public bool AlarmLogDeleteBeforeDays(int days)
        {
            return AlarmLogDeleteBeforeDays(days, 3000);
        }

        public bool AlarmLogDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();

                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from AlarmLog where BeginTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                             CommandType.Text,
                            "set rowcount " + setrowcount.ToString() + " delete from AlarmLog where BeginTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                            );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "AlarmLogDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }

        }

        //AutoUpdateInfo
        public DataSet AutoUpdateInfoQueryAllVersions()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "SELECT VersionID,CreateTime,IsForced FROM AutoUpdateInfo group by VersionID,CreateTime,IsForced order by VersionID desc",
                          ds,
                          new string[] { "AutoUpdateInfo" }
                            );
                    OnSqlStateChange(new SqlStateEventArgs(true, "AutoUpdateInfoQueryAllVersions OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "AutoUpdateInfoQueryAllVersions error:" + ex.ToString()));
                }
            }
            return ds;
        }        

        //AppConfig
        public bool AppConfigInsertByValue(String Position, String IPAddress, DateTime LastEditTime, string ElementSection, string ElementKey, string ElementValue)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO AppConfig( Position,IPAddress,LastEditTime,ElementSection,ElementKey,ElementValue)VALUES(@Position,@IPAddress,@LastEditTime,@ElementSection,@ElementKey,@ElementValue);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@Position", Position),
                new SqlParameter("@IPAddress", IPAddress),
                new SqlParameter("@LastEditTime", LastEditTime),
                new SqlParameter("@ElementSection", ElementSection),
                new SqlParameter("@ElementKey", ElementKey),
                new SqlParameter("@ElementValue", ElementValue)
                };
                try
                {
                    connection.Open();                  

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "AppConfigInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "AppConfigInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool AppConfigDeleteByPosition(string position)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "delete  from AppConfig where Position='" + position + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "AppConfigDeleteByPosition OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "AppConfigDeleteByPosition error:"+ex.ToString()));
                    return false;
                }
                return true;
            }

        }

        public MainDS AppConfigQueryByPosition(string position)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from AppConfig where Position='" + position + "'",
                          ds,
                          new string[] { "AppConfig" }
                            );
                    OnSqlStateChange(new SqlStateEventArgs(true, "AppConfigQueryByPosition OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "AppConfigQueryByPosition error:" + ex.ToString()));
                }
            }
            return ds;
        }        

        //ConveyLog
        public int ConveyLogInsert(string RollID, string InfoType, string Position,string IPAddress, string DeviceType, DateTime LogTime, string Description)
        {
            int retID = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "INSERT INTO [ConveyLog]([RollID],[InfoType],[Position],[IPAddress],[DeviceType],[LogTime],[Description])VALUES(@RollID ,@InfoType,@Position,@IPAddress,@DeviceType,@LogTime ,@Description);select @@IDENTITY";
                    SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@RollID", RollID),
                    new SqlParameter("@InfoType", InfoType),
                    new SqlParameter("@Position", Position),
                    new SqlParameter("@IPAddress", IPAddress),
                    new SqlParameter("@DeviceType", DeviceType),
                    new SqlParameter("@LogTime", LogTime),
                    new SqlParameter("@Description", Description)
                };                      
                    object reto = MSSqlHelper.ExecuteScalar(connection,//tran,
                          CommandType.Text,
                         sqlstr,
                         parameter);
                    retID = Convert.ToInt32(reto);
                    OnSqlStateChange(new SqlStateEventArgs(true, "ConveyLogInsert执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false,"ConveyLogInsert:"+ ex.ToString()));
                }
                return retID;
            }

        }

        public bool ConveyLogDeleteBeforeMonth()
        {
            return ConveyLogDeleteBeforeDays(60);
        }

        public bool ConveyLogDeleteBeforeDays(int days)
        {
            return ConveyLogDeleteBeforeDays(days, 3000);
        }

        public bool ConveyLogDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from ConveyLog where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                             CommandType.Text,
                            "set rowcount " + setrowcount.ToString() + " delete from ConveyLog where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                            );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "ConveyLogDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    return false;
                }
            }
            return true;
        }
         
        public MainDS ConveyLogQueryByPosition(string position)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select top " + Utils.SelectRowsMax + " * from ConveyLog where Position ='" + position + "'",
                          ds,
                          new string[] { "ConveyLog" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "ConveyLogQueryByPosition读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        public MainDS ConveyLogQueryAllByFK(string Position, DateTime begintime, DateTime endtime)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select top " + Utils.SelectRowsMax + " * from ConveyLog where 1=1  ";
                if (Position != "")
                    sqlstr = sqlstr + " and Position = @Position";


                sqlstr = sqlstr + " and LogTime >= @BeginTime";
                sqlstr = sqlstr + " and LogTime <= @EndTime";


                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@Position", Position),
                                new SqlParameter("@BeginTime",begintime),
                                new SqlParameter("@EndTime",endtime),
                                 };
                #endregion

                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "ConveyLog" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "ConveyLogQueryAllByFK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                     
                }
                return ds;
            }
        }
 
        //Paper_DS
        public MainDS Paper_DestinationQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from Paper_Destination where isnull(DS_Des,'') <> ''",
                          ds,
                          new string[] { "Paper_Destination" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_DestinationQueryAll读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        //SheetBufferLock
        public bool SheetBufferLockInsertByValue(int SheetID, Int16 SheetBufferID, Boolean IsLock)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SheetBufferLock( SheetID,SheetBufferID,IsLock)VALUES(@SheetID,@SheetBufferID,@IsLock);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@SheetID", SheetID),
                new SqlParameter("@SheetBufferID", SheetBufferID),
                new SqlParameter("@IsLock", IsLock)                
                };

                try
                {
                    connection.Open();                    
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SheetBufferLockInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SheetBufferLockInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public MainDS SheetBufferLockQueryBySheetID(int SheetID)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SheetBufferLock where SheetID=" + SheetID,
                          ds,
                          new string[] { "SheetBufferLock" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SheetBufferLockQueryBySheetID读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }
        public MainDS SheetBufferLockQueryBySheetIDAndLock(int SheetID,bool IsLock)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SheetBufferLock where IsLock=" + (IsLock ? "1" : "0") + " and SheetID=" + SheetID,
                          ds,
                          new string[] { "SheetBufferLock" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SheetBufferLockQueryBySheetIDAndLock读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }
        public MainDS SheetBufferLockQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SheetBufferLock",
                          ds,
                          new string[] { "SheetBufferLock" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SheetBufferLockQueryAll读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        public bool SheetBufferLockUpdateLockByPK(int SheetID, Int16 SheetBufferID, Boolean IsLock)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "UPDATE SheetBufferLock SET IsLock = @IsLock WHERE  SheetID = @SheetID and SheetBufferID = @SheetBufferID";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@SheetID", SheetID),
                                new SqlParameter("@SheetBufferID", SheetBufferID),
                                new SqlParameter("@IsLock",IsLock)                                
                                };
                #endregion

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "SheetBufferLockUpdateLockByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }
        }

        //SheetBufferDesc
        public DefineDS SheetBufferDescQueryAll()
        {
            DefineDS ds = new DefineDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SheetBufferLock",
                          ds,
                          new string[] { "SheetBufferDesc" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SheetBufferDescQueryAll读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }
        
        // RecordDiameterVerify
        public bool RecordDiameterVerifyInsert(int diameter_plc, int diameter_actual, string rollid)
        {

            bool ret = false;

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "INSERT INTO RecordDiameterVerify(Diameter_PLC,Diameter_Actual ,Position,LogTime,RollID)VALUES (" + diameter_plc + "," + diameter_actual + ",'" + Utils.Position + "', getdate(),'" + rollid + "')";

                try
                {
                    connection.Open();


                    MSSqlHelper.ExecuteNonQuery(connection,
                           CommandType.Text,
                          sqlstr);

                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordDiameterVerifyInsert执行成功"));
                    ret = true;

                }
                catch (Exception ex)
                {

                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    ret = false;
                }



                return ret;
            }

        }

        public MainDS RecordDiameterVerifyQueryByPosition(string position)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from RecordDiameterVerify where Position='" + position + "'",
                          ds,
                          new string[] { "RecordDiameterVerify" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordDiameterVerifyQueryByPosition读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        public bool RecordDiameterVerifyDeleteBeforeMonth()
        {
            return RecordDiameterVerifyDeleteBeforeDays(60);
        }

        public bool RecordDiameterVerifyDeleteBeforeDays(int days)
        {
            return RecordDiameterVerifyDeleteBeforeDays(days, 3000);
        }

        public bool RecordDiameterVerifyDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {

                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from RecordDiameterVerify where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from RecordDiameterVerify where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordDiameterVerifyDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    return false;
                }
            }
            return true;
        }

        //RecordWidthVerify
        public bool RecordWidthVerifyInsert(int widthleft, int widthright, int widthactual, string rollid)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO RecordWidthVerify(WidthLeft,WidthRight,WidthActual,Position,LogTime,RollID)VALUES (" + widthleft + "," + widthright + "," + widthactual + ",'" + Utils.Position + "', getdate(),'" + rollid + "')";

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                           CommandType.Text,
                          sqlstr);
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordWidthVerify Insert执行成功"));
                    ret = true;
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    ret = false;
                }
                return ret;
            }

        }

        public bool RecordWidthVerifyInsert(int widthleft, int widthright, int widthactual, string rollid,string position)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO RecordWidthVerify(WidthLeft,WidthRight,WidthActual,Position,LogTime,RollID)VALUES (" + widthleft + "," + widthright + "," + widthactual + ",'" + position + "', getdate(),'" + rollid + "')";

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                           CommandType.Text,
                          sqlstr);
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordWidthVerify Insert执行成功"));
                    ret = true;
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    ret = false;
                }
                return ret;
            }

        }

        public MainDS RecordWidthVerifyQueryByPosition(string position)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from RecordWidthVerify where Position='" + position + "'",
                          ds,
                          new string[] { "RecordWidthVerify" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordWidthVerifyQueryByPosition读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        public bool RecordWidthVerifyDeleteBeforeMonth()
        {
            return RecordWidthVerifyDeleteBeforeDays(60);
        }

        public bool RecordWidthVerifyDeleteBeforeDays(int days)
        {
            return RecordWidthVerifyDeleteBeforeDays(days, 3000);
        }

        public bool RecordWidthVerifyDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {

                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from RecordWidthVerify where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from RecordWidthVerify where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordWidthVerifyDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    return false;
                }
            }
            return true;
        }

        //RecordScan
        public bool RecordScanInsertByRow(MainDS.RecordScanRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "INSERT INTO RecordScan( RollID,Position,ScanTime,IsAuto)VALUES(@RollID,@Position,ISNULL(@ScanTime,getdate()),@IsAuto);";
                    
                    SqlParameter par = new SqlParameter();
                    par.Value = row.IsScanTimeNull() ? DBNull.Value : (object)row.ScanTime;
                    par.DbType = DbType.DateTime;
                    par.ParameterName = "@ScanTime";

                    SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@RollID", row.RollID),
                new SqlParameter("@Position",row.IsPositionNull()?DBNull.Value:(object)row.Position),
                par,
               // new SqlParameter("@ScanTime", SqlDbType.DateTime, row.IsScanTimeNull()?DBNull.Value: (object) row.ScanTime),
                new SqlParameter("@IsAuto",row.IsIsAutoNull()?DBNull.Value:(object)row.IsAuto)                
                };

                 
                   
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordScanInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "RecordScanInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }
        
        public int RecordScanCountByTime(string RollID, System.DateTime EndTime)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                int Count = 0;
                try
                {
                    Count = (int)MSSqlHelper.ExecuteScalar(connection,
                      CommandType.Text,
                      "select Count(*) from RecordScan where RollID='" + RollID + "' and ScanTime <='" + EndTime.ToString() + "'"
                      );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordScan读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

                return Count;
            }

        }
        
        public int RecordScanCountByRollIDAndPosition(DateTime ScanTime ,string RollID, string Position)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select Count(*) from RecordScan where ScanTime<@ScanTime and RollID=@RollID and Position = @Position";
               
                #endregion
                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@ScanTime", ScanTime),
                                new SqlParameter("@RollID",RollID),
                                new SqlParameter("@Position",Position),
                                 };
                #endregion


                int Count = 0;
                try
                {
                    Count = (int)MSSqlHelper.ExecuteScalar(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter
                      );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordScan读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

                return Count;
            }

        }

        public MainDS RecordScanQueryByRollID(string rollid)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from RecordScan where RollID='" + rollid + "'",
                          ds,
                          new string[] { "RecordScan" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordScan读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        public bool RecordScanDeleteBeforeMonth()
        {
            return RecordScanDeleteBeforeDays(60);
        }

        public bool RecordScanDeleteBeforeDays(int days)
        {
            return RecordScanDeleteBeforeDays(days, 3000);
        }

        public bool RecordScanDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {

                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from RecordScan where ScanTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from RecordScan where ScanTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordScanDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    return false;
                }
            }
            return true;
        }

        //RecordWeight
        public bool RecordWeightInsertByRow(MainDS.RecordWeightRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "INSERT INTO RecordWeight( RollID,Position,Weight,WeightTime,IsAuto)VALUES(@RollID,@Position,@Weight,ISNULL(@WeightTime,getdate()),@IsAuto);";

                    SqlParameter par = new SqlParameter();
                    par.Value = row.IsWeightTimeNull() ? DBNull.Value : (object)row.WeightTime;
                    par.DbType = DbType.DateTime;
                    par.ParameterName = "@WeightTime";

                    SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@RollID", row.RollID),
                new SqlParameter("@Position",row.IsPositionNull()?DBNull.Value:(object)row.Position),
                new SqlParameter("@Weight",row.IsWeightNull()?DBNull.Value:(object)row.Weight),
                par,
                new SqlParameter("@IsAuto",row.IsIsAutoNull()?DBNull.Value:(object)row.IsAuto)                
                };

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordWeightInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "RecordWeightInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool RecordWeightDeleteBeforeMonth()
        {
            return RecordWeightDeleteBeforeDays(60);
        }

        public bool RecordWeightDeleteBeforeDays(int days)
        {
            return RecordWeightDeleteBeforeDays(days, 3000);
        }

        public bool RecordWeightDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();

                    //SqlHelper.ExecuteNonQuery(connection,//tran,
                    //     CommandType.Text,
                    //     "delete from RecordWeight where  WeightTime<'" + Utils.DateTimeNow.AddMonths(-2).ToString() + "'"
                    //    );

                    //OnSqlStateChange(new SqlStateEventArgs(true, "RecordWeight删除成功"));

                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from RecordWeight where WeightTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from RecordWeight where WeightTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "RecordWeightDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));




                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }

        }
        


        //GripperJoin
        public bool GripperJoinInsertByRow(int JoinOnlyID, int ProductOnlyID,string JoinType, string ProductID, string SetID,int Amount, int SortIndex, string Position)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "INSERT INTO GripperJoin( JoinOnlyID,ProductOnlyID,JoinType,ProductID,SetID,Amount,SortIndex,Position,LogTime)VALUES(@JoinOnlyID,@ProductOnlyID,@JoinType,@ProductID,@SetID,@Amount,@SortIndex,@Position,getdate());";

                    SqlParameter[] parameter = new SqlParameter[]
                    {
                    new SqlParameter("@JoinOnlyID", JoinOnlyID),
                    new SqlParameter("@ProductOnlyID", ProductOnlyID),
                    new SqlParameter("@JoinType", JoinType),
                    new SqlParameter("@ProductID",ProductID),
                    new SqlParameter("@SetID",SetID),
                    new SqlParameter("@Amount",Amount),
                    new SqlParameter("@SortIndex",SortIndex),
                    new SqlParameter("@Position",Position) 
                    // new SqlParameter("@LogTime",LogTime)                
                    };

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "GripperJoinInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "GripperJoinInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }
        //public bool GripperJoinDeleteByPK(int JoinOnlyID, int ProductOnlyID,string JoinType)
        public bool GripperJoinDeleteByPK(int ProductOnlyID, string JoinType)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "delete from GripperJoin where ProductOnlyID=@ProductOnlyID and JoinType=@JoinType;";

                    SqlParameter[] parameter = new SqlParameter[]
                    {
                       //new SqlParameter("@JoinOnlyID", JoinOnlyID),
                       new SqlParameter("@ProductOnlyID", ProductOnlyID),
                        new SqlParameter("@JoinType", JoinType)
                    };

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "GripperJoinInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "GripperJoinInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        //GripperGroup


        public int GripperGroupInsertByRow(string Grade, int Basisweight, int Width, int Diameter, int CoreSize, string PaperType, string Customer)
        {
            int ret = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "INSERT INTO GripperGroup(Grade,Basisweight,Width,Diameter,CoreSize,PaperType,Customer,IsLock,QueryCount,QueryTime)VALUES(@Grade,@Basisweight,@Width,@Diameter,@CoreSize,@PaperType,@Customer,0,1,getdate());SELECT @@IDENTITY;";

                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        //new SqlParameter("@GroupID", row.GroupID),
                        new SqlParameter("@Grade", Grade),
                        new SqlParameter("@Basisweight", Basisweight),
                        new SqlParameter("@Width", Width),
                        new SqlParameter("@Diameter", Diameter),
                        new SqlParameter("@CoreSize", CoreSize),
                        new SqlParameter("@PaperType", PaperType),
                        new SqlParameter("@Customer", Customer)                
                    };

                  object obj =  MSSqlHelper.ExecuteScalar(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = Convert.ToInt32(obj);
                    OnSqlStateChange(new SqlStateEventArgs(true, "GripperGroupInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "GripperGroupInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public int  GripperGroupQueryByFK(string Grade, int Basisweight, int Width, int Diameter, int CoreSize, string PaperType, string Customer)
        {
            int ret = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlwhere =   "where Grade=@Grade " +
                                "and Basisweight=@Basisweight " +
                                "and Width=@Width " +
                                "and Diameter=@Diameter " +
                                "and CoreSize=@CoreSize " +
                                "and PaperType=@PaperType " +
                                "and Customer=@Customer";

                string sqlstr = "select GroupID from GripperGroup " + 
                    sqlwhere +
                    ";UPDATE GripperGroup SET QueryCount=ISNULL(QueryCount,0)+1,QueryTime= getdate() " + 
                    sqlwhere;
                              



                try
                {
                    connection.Open(); 

                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        //new SqlParameter("@GroupID", row.GroupID),
                        new SqlParameter("@Grade", Grade),
                        new SqlParameter("@Basisweight", Basisweight),
                        new SqlParameter("@Width", Width),
                        new SqlParameter("@Diameter", Diameter),
                        new SqlParameter("@CoreSize", CoreSize),
                        new SqlParameter("@PaperType", PaperType),
                        new SqlParameter("@Customer", Customer)                
                    };

                  object obj =  MSSqlHelper.ExecuteScalar(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = Convert.ToInt32(obj);
                    OnSqlStateChange(new SqlStateEventArgs(true, "GripperGroupQueryByFK OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "GripperGroupQueryByFK Error:" + ex.ToString()));
                }
                return ret;
            }

        }




        //SocketTempMES
        public int SocketTempMESInsertByRow(MainDS.SocketTempMESRow row)
        {
            int ret = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "INSERT INTO SocketTempMES( RollID,FunctionCode,Position,SessionID,ToIP,SendTime,SendCount,Data)VALUES( @RollID,@FunctionCode,@Position,@SessionID,@ToIP,ISNULL(@SendTime,getdate()),@SendCount,@Data); SELECT @@IDENTITY;";

                SqlParameter par = new SqlParameter();
                par.Value = row.IsSendTimeNull() ? DBNull.Value : (object)row.SendTime;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@SendTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@RollID",row.IsRollIDNull()?DBNull.Value:(object)row.RollID),
                new SqlParameter("@FunctionCode",row.IsFunctionCodeNull()?DBNull.Value:(object)row.FunctionCode),
                new SqlParameter("@Position",row.IsPositionNull()?DBNull.Value:(object)row.Position),
                new SqlParameter("@SessionID",row.IsSessionIDNull()?DBNull.Value:(object)row.SessionID),
                new SqlParameter("@ToIP",row.IsToIPNull()?DBNull.Value:(object)row.ToIP),
                par,//new SqlParameter("@SendTime",row.IsSendTimeNull()?DBNull.Value:(object)row.SendTime),
                new SqlParameter("@SendCount",row.IsSendCountNull()?DBNull.Value:(object)row.SendCount),
                new SqlParameter("@Data", row.Data)                
                };

                try
                {
                    connection.Open(); 

                   object retobj = MSSqlHelper.ExecuteScalar(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                   ret = Convert.ToInt32(retobj);
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempMESInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketTempMESInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public MainDS SocketTempMESQueryByOnlyID(string OnlyID)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SocketTempMES where OnlyID='" + OnlyID + "'",
                          ds,
                          new string[] { "SocketTempMES" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempMES读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;
        }

        public MainDS SocketTempMESQueryALL()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SocketTempMES",
                          ds,
                          new string[] { "SocketTempMES" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempMES读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;
        }

        public MainDS SocketTempMESQueryALLTop1000()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select top 1000 * from SocketTempMES  order by SendTime desc",
                          ds,
                          new string[] { "SocketTempMES" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempMES读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;
        }

        public bool SocketTempMESUpdateCount(string OnlyStr, int count)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "UPDATE SocketTempMES SET SendCount='" + count + "' where OnlyID ='" + OnlyStr + "'"
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempMES更新成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }

                return true;
            }
        }

        public bool SocketTempMESDeleteByOnlyID(string OnlyID)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "delete from SocketTempMES where OnlyID ='" + OnlyID + "'"
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempMES删除成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }

                return true;
            }
        }

        //SocketTempKONE
        public bool SocketTempKONEInsertByRow(MainDS.SocketTempKONERow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SocketTempKONE( DataType,SendData,SendTime,SendCount,Station,RollID)VALUES(@DataType,@SendData,ISNULL(@SendTime,getdate()),@SendCount,@Station,@RollID);";

                SqlParameter par = new SqlParameter();
                par.Value = row.IsSendTimeNull() ? DBNull.Value : (object)row.SendTime;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@SendTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@DataType", row.DataType),
                new SqlParameter("@SendData",row.IsSendDataNull()?DBNull.Value:(object)row.SendData),
                par,//new SqlParameter("@SendTime",row.IsSendTimeNull()?DBNull.Value:(object)row.SendTime),
                new SqlParameter("@SendCount",row.IsSendCountNull()?DBNull.Value:(object)row.SendCount),
                new SqlParameter("@Station", row.Station),
                new SqlParameter("@RollID",row.IsRollIDNull()?DBNull.Value:(object)row.RollID)                
                };

                try
                {
                    connection.Open(); 

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONEInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketTempKONEInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }        

        public int SocketTempKONECountByPK(string DataType, string station)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                int Count = 0;
                try
                {
                    Count = (int)MSSqlHelper.ExecuteScalar(connection,
                      CommandType.Text,
                      "select Count(*) from SocketTempKONE where  DataType='" + DataType + "' and Station ='" + station + "'"
                      );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONE读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    Count = -1;
                }

                return Count;
            }

        }

        public MainDS SocketTempKONEQueryByPK(string DataType, string station)
        {

            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                         "select * from SocketTempKONE where  DataType='" + DataType + "' and Station ='" + station + "'",
                          ds,
                          new string[] { "SocketTempKONE" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONE读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;
        }

        public bool SocketTempKONEDeleteByPK(string DataType, string station)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "delete  from SocketTempKONE where DataType='" + DataType + "' and Station ='" + station + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONE读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }

        }

        public void SocketTempKONEDeleteByDelayTime(int delaySeconds)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "delete  from SocketTempKONE where DATEDIFF(second,SendTime,getdate()) >'" + delaySeconds + "'"
                            );
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONE删除成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
        }

        public MainDS SocketTempKONEQueryAll()
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SocketTempKONE",
                          ds,
                          new string[] { "SocketTempKONE" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONE读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;
        }

        public MainDS SocketTempKONEQueryALLByMinTime(int seconds)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select * from SocketTempKONE where DATEDIFF(second,SendTime,getdate()) >'" + seconds + "'",
                          ds,
                          new string[] { "SocketTempKONE" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONE读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;
        }

        public bool SocketTempKONEUpdateCount(string DataType, string station, int count)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "UPDATE SocketTempKONE SET Repeat='" + count + "' where DataType ='" + DataType + "' and Station ='" + station + "'"
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketTempKONE更新成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }

                return true;
            }
        }

        //SocketRecordMES
        public bool SocketRecordMESInsertByRow(MainDS.SocketRecordMESRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr =
                   "if(exists(select OnlyID from SocketRecordMES where OnlyID = @OnlyID and IsSend = @IsSend)) " +
                   "update SocketRecordMES set [Repeat] = ([Repeat]  +1) where  OnlyID = @OnlyID and IsSend = @IsSend;" +
                   " else INSERT INTO SocketRecordMES( OnlyID,CT_Data,CT_Time,DataType,IsSend,MES_Data,MES_Time,Repeat,Description)VALUES(@OnlyID,@CT_Data,ISNULL(@CT_Time,getdate()),@DataType,@IsSend,@MES_Data,ISNULL(@MES_Time,getdate()),@Repeat,@Description);";

                SqlParameter par1 = new SqlParameter();
                par1.Value = row.IsCT_TimeNull() ? DBNull.Value : (object)row.CT_Time;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@CT_Time";

                SqlParameter par2 = new SqlParameter();
                par2.Value = row.IsMES_TimeNull() ? DBNull.Value : (object)row.MES_Time;
                par2.DbType = DbType.DateTime;
                par2.ParameterName = "@MES_Time";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@OnlyID", row.OnlyID),
                new SqlParameter("@CT_Data",row.IsCT_DataNull()?DBNull.Value:(object)row.CT_Data),
               par1, //new SqlParameter("@CT_Time",row.IsCT_TimeNull()?DBNull.Value:(object)row.CT_Time),
                new SqlParameter("@DataType",row.IsDataTypeNull()?DBNull.Value:(object)row.DataType),
                new SqlParameter("@IsSend", row.IsSend),
                new SqlParameter("@MES_Data",row.IsMES_DataNull()?DBNull.Value:(object)row.MES_Data),
                par2,// new SqlParameter("@MES_Time",row.IsMES_TimeNull()?DBNull.Value:(object)row.MES_Time),
                new SqlParameter("@Repeat",row.IsRepeatNull()?DBNull.Value:(object)row.Repeat),
                new SqlParameter("@Description",row.IsDescriptionNull()?DBNull.Value:(object)row.Description)                
                };

                try
                {
                    connection.Open();
                   

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMESInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketRecordMESInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }
        
        public bool SocketRecordMESDeleteBeforeMonth()
        {
            return SocketRecordMESDeleteBeforeDays(60);
        }

        public bool SocketRecordMESDeleteBeforeDays(int days)
        {
            return SocketRecordMESDeleteBeforeDays(days, 3000);
        }

        public bool SocketRecordMESDeleteBeforeDays(int days, int setrowcount)//setrowcount 默认200
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();

                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from SocketRecordMES where CT_Time<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from SocketRecordMES where CT_Time<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMESDeleteBeforeMonth执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }

            }
            return true;
        }

        public MainDS SocketRecordMESQueryAllByFK(string datatype, DateTime begintime, DateTime endtime)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select top " + Utils.SelectRowsMax + " * from SocketRecordMES where 1=1  ";
                if (datatype != "")
                    sqlstr = sqlstr + " and DataType = @DataType";

                sqlstr = sqlstr + " and CT_Time >= @BeginTime";
                sqlstr = sqlstr + " and CT_Time <= @EndTime";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@DataType", datatype),
                                new SqlParameter("@BeginTime",begintime),
                                new SqlParameter("@EndTime",endtime),
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "SocketRecordMES" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMESQueryAllByFK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                }
                return ds;
            }
        }

        //SocketRecordKONE
        public bool SocketRecordKONEInsertByRow(MainDS.SocketRecordKONERow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SocketRecordKONE(DataType,IsSend,Description,Message,MseeageType,LogTime,RollID)VALUES(@DataType,@IsSend,@Description,@Message,@MseeageType,@LogTime,@RollID);";
               
                SqlParameter par1 = new SqlParameter();
                par1.Value = row.LogTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@LogTime"; 

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@DataType", row.DataType),
                new SqlParameter("@IsSend", row.IsSend),
                new SqlParameter("@Description",row.IsDescriptionNull()?DBNull.Value:(object)row.Description),
                new SqlParameter("@Message", row.Message),
                new SqlParameter("@MseeageType", row.MseeageType),
                par1,//new SqlParameter("@SendTime", row.SendTime),
                new SqlParameter("@RollID",row.IsRollIDNull()?DBNull.Value:(object)row.RollID)  
                };
                try
                {
                    connection.Open(); 
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordKONEInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketRecordKONEInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool SocketRecordKONEInsertByValue(String DataType, Boolean IsSend, String Description, String Message, string MseeageType, DateTime LogTime, String RollID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SocketRecordKONE( DataType,IsSend,Description,Message,MseeageType,LogTime,RollID)VALUES(@DataType,@IsSend,@Description,@Message,@MseeageType,@LogTime,@RollID);";
                SqlParameter par1 = new SqlParameter();
                par1.Value = LogTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@LogTime";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@DataType", DataType),
                new SqlParameter("@IsSend", IsSend),
                new SqlParameter("@Description", Description),
                new SqlParameter("@Message", Message),
                new SqlParameter("@MseeageType", MseeageType),
                par1,//new SqlParameter("@SendTime", SendTime),
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordKONEInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketRecordKONEInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool SocketRecordKONEDeleteBeforeMonth()
        {
            return SocketRecordKONEDeleteBeforeDays(60);
        }

        public bool SocketRecordKONEDeleteBeforeDays(int days)
        {
            return SocketRecordKONEDeleteBeforeDays(days, 3000);
        }

        public bool SocketRecordKONEDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from SocketRecordKONE where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from SocketRecordKONE where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordKONEDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    return false;
                }
            }
            return true;
        }

        public MainDS SocketRecordKONEQueryAllByFK(string Description, string rollid, DateTime begintime, DateTime endtime)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select top " + Utils.SelectRowsMax + " * from SocketRecordKONE where Description=@Description ";
                if (rollid != "")
                    sqlstr = sqlstr + " and RollID like @RollID";

                sqlstr = sqlstr + " and LogTime >= @BeginTime";
                sqlstr = sqlstr + " and LogTime <= @EndTime";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                     new SqlParameter("@Description", Description),
                                     new SqlParameter("@RollID",  "%"+rollid+"%"),
                                     new SqlParameter("@BeginTime",begintime),
                                     new SqlParameter("@EndTime",endtime)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "SocketRecordKONE" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordKONEQueryAllByFK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                }
                return ds;
            }
        }

        //SocketRecordMetsoWrap
        public bool SocketRecordMetsoWrapInsertByRow(MainDS.SocketRecordMetsoWrapRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SocketRecordMetsoWrap(DataType,IsSend,Description,Message,MseeageType,LogTime,RollID)VALUES(@DataType,@IsSend,@Description,@Message,@MseeageType,@LogTime,@RollID);";

                SqlParameter par1 = new SqlParameter();
                par1.Value = row.LogTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@LogTime";
                SqlParameter[] parameter = new SqlParameter[]
                {
                 new SqlParameter("@DataType", row.DataType),
                new SqlParameter("@IsSend", row.IsSend),
                new SqlParameter("@Description",row.IsDescriptionNull()?DBNull.Value:(object)row.Description),
                new SqlParameter("@Message", row.Message),
                new SqlParameter("@MseeageType", row.MseeageType),
                par1,//new SqlParameter("@SendTime", row.SendTime),
                new SqlParameter("@RollID",row.IsRollIDNull()?DBNull.Value:(object)row.RollID)  
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMetsoWrapInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketRecordMetsoWrapInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool SocketRecordMetsoWrapInsertByValue(String DataType, Boolean IsSend, String Description, String Message, string MseeageType, DateTime SendTime, String RollID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SocketRecordMetsoWrap( DataType,IsSend,Description,Message,MseeageType,LogTime,RollID)VALUES(@DataType,@IsSend,@Description,@Message,@MseeageType,@LogTime,@RollID);";
                SqlParameter par1 = new SqlParameter();
                par1.Value = SendTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@LogTime";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@DataType", DataType),
                new SqlParameter("@IsSend", IsSend),
                new SqlParameter("@Description", Description),
                new SqlParameter("@Message", Message),
                new SqlParameter("@MseeageType", MseeageType),
                par1,//new SqlParameter("@SendTime", SendTime),
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMetsoWrapInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketRecordMetsoWrapInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool SocketRecordMetsoWrapDeleteBeforeMonth()
        {
            return SocketRecordMetsoWrapDeleteBeforeDays(60);
        }

        public bool SocketRecordMetsoWrapDeleteBeforeDays(int days)
        {
            return SocketRecordMetsoWrapDeleteBeforeDays(days, 3000);
        }

        public bool SocketRecordMetsoWrapDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from SocketRecordMetsoWrap where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from SocketRecordMetsoWrap where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMetsoWrapDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    return false;
                }
            }
            return true;
        }

        public MainDS SocketRecordMetsoWrapQueryAllByFK(string rollid, DateTime begintime, DateTime endtime)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select top " + Utils.SelectRowsMax + " * from SocketRecordMetsoWrap where 1=1  ";
                if (rollid != "")
                    sqlstr = sqlstr + " and RollID = @RollID";

                sqlstr = sqlstr + " and LogTime >= @BeginTime";
                sqlstr = sqlstr + " and LogTime <= @EndTime";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@RollID", rollid),
                                new SqlParameter("@BeginTime",begintime),
                                new SqlParameter("@EndTime",endtime),
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "SocketRecordMetsoWrap" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMetsoWrapQueryAllByFK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                }
                return ds;
            }
        }

        //SocketRecordMSKWrap
        public bool SocketRecordMSKWrapInsertByRow(MainDS.SocketRecordMSKWrapRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SocketRecordMSKWrap(DataType,IsSend,Description,Message,MseeageType,LogTime,PalletID)VALUES(@DataType,@IsSend,@Description,@Message,@MseeageType,@LogTime,@PalletID);";

                SqlParameter par1 = new SqlParameter();
                par1.Value = row.LogTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@LogTime";
                SqlParameter[] parameter = new SqlParameter[]
                {
                 new SqlParameter("@DataType", row.DataType),
                new SqlParameter("@IsSend", row.IsSend),
                new SqlParameter("@Description",row.IsDescriptionNull()?DBNull.Value:(object)row.Description),
                new SqlParameter("@Message", row.Message),
                new SqlParameter("@MseeageType", row.MseeageType),
                par1,//new SqlParameter("@SendTime", row.SendTime),
                new SqlParameter("@PalletID",row.IsPalletIDNull()?DBNull.Value:(object)row.PalletID)  
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMSKWrapInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketRecordMSKWrapInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool SocketRecordMSKWrapInsertByValue(String DataType, Boolean IsSend, String Description, String Message, string MseeageType, DateTime SendTime, String PalletID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO SocketRecordMSKWrap( DataType,IsSend,Description,Message,MseeageType,LogTime,PalletID)VALUES(@DataType,@IsSend,@Description,@Message,@MseeageType,@LogTime,@PalletID);";
                SqlParameter par1 = new SqlParameter();
                par1.Value = SendTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@LogTime";
                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@DataType", DataType),
                new SqlParameter("@IsSend", IsSend),
                new SqlParameter("@Description", Description),
                new SqlParameter("@Message", Message),
                new SqlParameter("@MseeageType", MseeageType),
                par1,//new SqlParameter("@SendTime", SendTime),
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMSKWrapInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SocketRecordMSKWrapInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public bool SocketRecordMSKWrapDeleteBeforeMonth()
        {
            return SocketRecordMSKWrapDeleteBeforeDays(60);
        }

        public bool SocketRecordMSKWrapDeleteBeforeDays(int days)
        {
            return SocketRecordMSKWrapDeleteBeforeDays(days, 3000);
        }

        public bool SocketRecordMSKWrapDeleteBeforeDays(int days, int setrowcount)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    int count = 0;
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                        "select count(*) from SocketRecordMSKWrap where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                        );

                    int imax = (int)((count / setrowcount) + 0.5);
                    for (int i = 0; i < imax; i++)
                    {
                        MSSqlHelper.ExecuteNonQuery(connection,//tran,
                               CommandType.Text,
                              "set rowcount " + setrowcount.ToString() + " delete from SocketRecordMSKWrap where LogTime<'" + Utils.DateTimeNow.AddDays(0 - days).ToString() + "'"
                              );
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMSKWrapDeleteBeforeDays执行成功,set rowcount " + setrowcount.ToString() + " 执行次数为:" + imax.ToString()));


                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    return false;
                }
            }
            return true;
        }

        public MainDS SocketRecordMSKWrapQueryAllByFK(string palletid, DateTime begintime, DateTime endtime)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select top " + Utils.SelectRowsMax + " * from SocketRecordMSKWrap where 1=1  ";
                if (palletid != "")
                    sqlstr = sqlstr + " and PalletID = @PalletID";

                sqlstr = sqlstr + " and LogTime >= @BeginTime";
                sqlstr = sqlstr + " and LogTime <= @EndTime";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@PalletID", palletid),
                                new SqlParameter("@BeginTime",begintime),
                                new SqlParameter("@EndTime",endtime),
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "SocketRecordMSKWrap" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "SocketRecordMSKWrapQueryAllByFK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                }
                return ds;
            }
        }
        
        //Roll_Product 
        public MainDS Roll_ProductQueryByOnlyID(string OnlyID)
        {
            MainDS ds = new MainDS();
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
                          "select * from Roll_Product where OnlyID=@OnlyID",
                          ds,
                          new string[] { "Roll_Product" },
                          parameter
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
        
        public MainDS Roll_ProductQueryByProductID(string ProductID)
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
                          "select * from Roll_Product where ProductID=@ProductID",
                          ds,
                          new string[] { "Roll_Product" },
                          parameter
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

        public MainDS Roll_ProductQueryByBarcode(string Barcode)
        {
            if (Barcode.Length > 10)
            {
                return Roll_ProductQueryByProductID(Barcode);
            }
            else
            {
                return Roll_ProductQueryByOnlyID(Barcode);
            }
        }
        
        public MainDS Roll_ProductQueryAllByMESTime(DateTime begin, DateTime end)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //WINDER_DATE   MESTime

                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          "select top "+Utils.SelectRowsMax+"  * from Roll_Product where MESTime>='" + begin.ToString() + "' and MESTime<='" + end.ToString() + "'",
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

        public bool Roll_ProductUpdateAllByOnlyID(MainDS.Roll_ProductRow row, bool IsUpdateDBNull)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    #region sqlstr
                    string sqlstr = "UPDATE Roll_Product SET IsUploadERP=0,"; //修改数据后，设置重新传递
                    //if (IsUpdateDBNull || !row.IsOnlyIDNull())
                    //    sqlstr = sqlstr + " OnlyID = @OnlyID,";
                    if (IsUpdateDBNull || !row.IsProductTypeNull())
                        sqlstr = sqlstr + " ProductType = @ProductType,";
                    //if (IsUpdateDBNull || !row.IsProductIDNull())
                    //    sqlstr = sqlstr + " ProductID = @ProductID,";
                    if (IsUpdateDBNull || !row.IsBarcodeNull())
                        sqlstr = sqlstr + " Barcode = @Barcode,";
                    if (IsUpdateDBNull || !row.IsMachineIDNull())
                        sqlstr = sqlstr + " MachineID = @MachineID,";
                    if (IsUpdateDBNull || !row.IsJumboShiftNull())
                        sqlstr = sqlstr + " JumboShift = @JumboShift,";
                    if (IsUpdateDBNull || !row.IsJumboDateNull())
                        sqlstr = sqlstr + " JumboDate = @JumboDate,";
                    if (IsUpdateDBNull || !row.IsJumboNONull())
                        sqlstr = sqlstr + " JumboNO = @JumboNO,";
                    if (IsUpdateDBNull || !row.IsRewinderSetNull())
                        sqlstr = sqlstr + " RewinderSet = @RewinderSet,";
                    if (IsUpdateDBNull || !row.IsRewinderSetIndexNull())
                        sqlstr = sqlstr + " RewinderSetIndex = @RewinderSetIndex,";
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
                    if (IsUpdateDBNull || !row.IsDirectionNull())
                        sqlstr = sqlstr + " Direction = @Direction,";
                    if (IsUpdateDBNull || !row.IsBasisweightNull())
                        sqlstr = sqlstr + " Basisweight = @Basisweight,";
                    if (IsUpdateDBNull || !row.IsWidth_MNull())
                        sqlstr = sqlstr + " Width_M = @Width_M,";
                    if (IsUpdateDBNull || !row.IsWidth_MetsoNull())
                        sqlstr = sqlstr + " Width_Metso = @Width_Metso,";
                    if (IsUpdateDBNull || !row.IsWidthNull())
                        sqlstr = sqlstr + " Width = @Width,";
                    if (IsUpdateDBNull || !row.IsPalletLengthNull())
                        sqlstr = sqlstr + " PalletLength = @PalletLength,";
                    if (IsUpdateDBNull || !row.IsWidthModeNull())
                        sqlstr = sqlstr + " WidthMode = @WidthMode,";
                    if (IsUpdateDBNull || !row.IsRollLengthNull())
                        sqlstr = sqlstr + " RollLength = @RollLength,";
                    if (IsUpdateDBNull || !row.IsWeight_CalcNull())
                        sqlstr = sqlstr + " Weight_Calc = @Weight_Calc,";
                    if (IsUpdateDBNull || !row.IsInspector_DescNull())
                        sqlstr = sqlstr + " Inspector_Desc = @Inspector_Desc,";
                    if (IsUpdateDBNull || !row.IsWinderIDNull())
                        sqlstr = sqlstr + " WinderID = @WinderID,";
                    if (IsUpdateDBNull || !row.IsDiameter_MNull())
                        sqlstr = sqlstr + " Diameter_M = @Diameter_M,";
                    if (IsUpdateDBNull || !row.IsDiameter_MetsoNull())
                        sqlstr = sqlstr + " Diameter_Metso = @Diameter_Metso,";
                    if (IsUpdateDBNull || !row.IsDiameterNull())
                        sqlstr = sqlstr + " Diameter = @Diameter,";
                    if (IsUpdateDBNull || !row.IsCoreNull())
                        sqlstr = sqlstr + " Core = @Core,";
                    if (IsUpdateDBNull || !row.IsCoreWeightNull())
                        sqlstr = sqlstr + " CoreWeight = @CoreWeight,";
                    if (IsUpdateDBNull || !row.IsSpliceNull())
                        sqlstr = sqlstr + " Splice = @Splice,";
                    if (IsUpdateDBNull || !row.IsHolesNull())
                        sqlstr = sqlstr + " Holes = @Holes,";
                    if (IsUpdateDBNull || !row.IsReWinderShiftNull())
                        sqlstr = sqlstr + " ReWinderShift = @ReWinderShift,";
                    if (IsUpdateDBNull || !row.IsReWinderDateNull())
                        sqlstr = sqlstr + " ReWinderDate = @ReWinderDate,";
                    if (IsUpdateDBNull || !row.IsDestinationNull())
                        sqlstr = sqlstr + " Destination = @Destination,";
                    if (IsUpdateDBNull || !row.IsDegradeCauseNull())
                        sqlstr = sqlstr + " DegradeCause = @DegradeCause,";
                    if (IsUpdateDBNull || !row.IsCustomerNull())
                        sqlstr = sqlstr + " Customer = @Customer,";
                    if (IsUpdateDBNull || !row.IsRollWrapIDNull())
                        sqlstr = sqlstr + " RollWrapID = @RollWrapID,";
                    if (IsUpdateDBNull || !row.IsRollWrapShiftNull())
                        sqlstr = sqlstr + " RollWrapShift = @RollWrapShift,";
                    if (IsUpdateDBNull || !row.IsRollWrapDateNull())
                        sqlstr = sqlstr + " RollWrapDate = @RollWrapDate,";
                    if (IsUpdateDBNull || !row.IsWeight_GrossNull())
                        sqlstr = sqlstr + " Weight_Gross = @Weight_Gross,";
                    if (IsUpdateDBNull || !row.IsWeight_WeiNull())
                        sqlstr = sqlstr + " Weight_Wei = @Weight_Wei,";
                    if (IsUpdateDBNull || !row.IsWeight_NetNull())
                        sqlstr = sqlstr + " Weight_Net = @Weight_Net,";
                    if (IsUpdateDBNull || !row.IsWeightModeNull())
                        sqlstr = sqlstr + " WeightMode = @WeightMode,";
                    if (IsUpdateDBNull || !row.IsColor_DescNull())
                        sqlstr = sqlstr + " Color_Desc = @Color_Desc,";
                    if (IsUpdateDBNull || !row.IsFactoryNull())
                        sqlstr = sqlstr + " Factory = @Factory,";
                    if (IsUpdateDBNull || !row.IsSheeterIDNull())
                        sqlstr = sqlstr + " SheeterID = @SheeterID,";
                    if (IsUpdateDBNull || !row.IsSheetShiftNull())
                        sqlstr = sqlstr + " SheetShift = @SheetShift,";
                    if (IsUpdateDBNull || !row.IsSheetDateNull())
                        sqlstr = sqlstr + " SheetDate = @SheetDate,";
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
                    if (IsUpdateDBNull || !row.IsCONVEYOR_NBRNull())
                        sqlstr = sqlstr + " CONVEYOR_NBR = @CONVEYOR_NBR,";
                    if (IsUpdateDBNull || !row.IsIsNBRNull())
                        sqlstr = sqlstr + " IsNBR = @IsNBR,";
                    if (IsUpdateDBNull || !row.IsIsWrapOKNull())
                        sqlstr = sqlstr + " IsWrapOK = @IsWrapOK,";
                    if (IsUpdateDBNull || !row.IsIsJetOKNull())
                        sqlstr = sqlstr + " IsJetOK = @IsJetOK,";
                    if (IsUpdateDBNull || !row.IsIsMeasureNull())
                        sqlstr = sqlstr + " IsMeasure = @IsMeasure,";
                    if (IsUpdateDBNull || !row.IsIsWeightScaleNull())
                        sqlstr = sqlstr + " IsWeightScale = @IsWeightScale,";
                    if (IsUpdateDBNull || !row.IsMeasureTimeNull())
                        sqlstr = sqlstr + " MeasureTime = @MeasureTime,";
                    if (IsUpdateDBNull || !row.IsWeightScaleTimeNull())
                        sqlstr = sqlstr + " WeightScaleTime = @WeightScaleTime,";
                    if (IsUpdateDBNull || !row.IsMESTimeNull())
                        sqlstr = sqlstr + " MESTime = @MESTime,";
                    if (IsUpdateDBNull || !row.IsLastEditTimeNull())
                        sqlstr = sqlstr + " LastEditTime = @LastEditTime,";
                    sqlstr = sqlstr.TrimEnd(new char[] { ' ', ',' });
                    sqlstr = sqlstr + " WHERE OnlyID = @OnlyID";
                    #endregion

                    #region parameter
                    SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@OnlyID", row.OnlyID),
                                new SqlParameter("@ProductType",row.IsProductTypeNull()?DBNull.Value:(object)row.ProductType),
                                //new SqlParameter("@ProductID",row.IsProductIDNull()?DBNull.Value:(object)row.ProductID),
                               new SqlParameter("@Barcode",row.IsBarcodeNull()?DBNull.Value:(object)row.Barcode),
                                new SqlParameter("@MachineID",row.IsMachineIDNull()?DBNull.Value:(object)row.MachineID),
                                new SqlParameter("@JumboShift",row.IsJumboShiftNull()?DBNull.Value:(object)row.JumboShift),
                                new SqlParameter("@JumboDate",row.IsJumboDateNull()?DBNull.Value:(object)row.JumboDate),
                                new SqlParameter("@JumboNO",row.IsJumboNONull()?DBNull.Value:(object)row.JumboNO),
                                new SqlParameter("@RewinderSet",row.IsRewinderSetNull()?DBNull.Value:(object)row.RewinderSet),
                                new SqlParameter("@RewinderSetIndex",row.IsRewinderSetIndexNull()?DBNull.Value:(object)row.RewinderSetIndex),
                                new SqlParameter("@Type",row.IsTypeNull()?DBNull.Value:(object)row.Type),
                                new SqlParameter("@Grade",row.IsGradeNull()?DBNull.Value:(object)row.Grade),
                                new SqlParameter("@Stand_Desc",row.IsStand_DescNull()?DBNull.Value:(object)row.Stand_Desc),
                                new SqlParameter("@OrderNO",row.IsOrderNONull()?DBNull.Value:(object)row.OrderNO),
                                new SqlParameter("@MaterialCode",row.IsMaterialCodeNull()?DBNull.Value:(object)row.MaterialCode),
                                new SqlParameter("@PartNO",row.IsPartNONull()?DBNull.Value:(object)row.PartNO),
                                new SqlParameter("@Direction",row.IsDirectionNull()?DBNull.Value:(object)row.Direction),
                                new SqlParameter("@Basisweight",row.IsBasisweightNull()?DBNull.Value:(object)row.Basisweight),
                                new SqlParameter("@Width_M",row.IsWidth_MNull()?DBNull.Value:(object)row.Width_M),
                                new SqlParameter("@Width_Metso",row.IsWidth_MetsoNull()?DBNull.Value:(object)row.Width_Metso),
                                new SqlParameter("@Width",row.IsWidthNull()?DBNull.Value:(object)row.Width),
                                new SqlParameter("@PalletLength",row.IsPalletLengthNull()?DBNull.Value:(object)row.PalletLength),
                                new SqlParameter("@WidthMode",row.IsWidthModeNull()?DBNull.Value:(object)row.WidthMode),
                                new SqlParameter("@RollLength",row.IsRollLengthNull()?DBNull.Value:(object)row.RollLength),
                                new SqlParameter("@Weight_Calc",row.IsWeight_CalcNull()?DBNull.Value:(object)row.Weight_Calc),
                                new SqlParameter("@Inspector_Desc",row.IsInspector_DescNull()?DBNull.Value:(object)row.Inspector_Desc),
                                new SqlParameter("@WinderID",row.IsWinderIDNull()?DBNull.Value:(object)row.WinderID),
                                new SqlParameter("@Diameter_M",row.IsDiameter_MNull()?DBNull.Value:(object)row.Diameter_M),
                                new SqlParameter("@Diameter_Metso",row.IsDiameter_MetsoNull()?DBNull.Value:(object)row.Diameter_Metso),
                                new SqlParameter("@Diameter",row.IsDiameterNull()?DBNull.Value:(object)row.Diameter),
                                new SqlParameter("@Core",row.IsCoreNull()?DBNull.Value:(object)row.Core),
                                new SqlParameter("@CoreWeight",row.IsCoreWeightNull()?DBNull.Value:(object)row.CoreWeight),
                                new SqlParameter("@Splice",row.IsSpliceNull()?DBNull.Value:(object)row.Splice),
                                new SqlParameter("@Holes",row.IsHolesNull()?DBNull.Value:(object)row.Holes),
                                new SqlParameter("@ReWinderShift",row.IsReWinderShiftNull()?DBNull.Value:(object)row.ReWinderShift),
                                new SqlParameter("@ReWinderDate",row.IsReWinderDateNull()?DBNull.Value:(object)row.ReWinderDate),
                                new SqlParameter("@Destination",row.IsDestinationNull()?DBNull.Value:(object)row.Destination),
                                new SqlParameter("@DegradeCause",row.IsDegradeCauseNull()?DBNull.Value:(object)row.DegradeCause),
                                new SqlParameter("@Customer",row.IsCustomerNull()?DBNull.Value:(object)row.Customer),
                                new SqlParameter("@RollWrapID",row.IsRollWrapIDNull()?DBNull.Value:(object)row.RollWrapID),
                                new SqlParameter("@RollWrapShift",row.IsRollWrapShiftNull()?DBNull.Value:(object)row.RollWrapShift),
                                new SqlParameter("@RollWrapDate",row.IsRollWrapDateNull()?DBNull.Value:(object)row.RollWrapDate),
                                new SqlParameter("@Weight_Gross",row.IsWeight_GrossNull()?DBNull.Value:(object)row.Weight_Gross),
                                new SqlParameter("@Weight_Wei",row.IsWeight_WeiNull()?DBNull.Value:(object)row.Weight_Wei),
                                new SqlParameter("@Weight_Net",row.IsWeight_NetNull()?DBNull.Value:(object)row.Weight_Net),
                                new SqlParameter("@WeightMode",row.IsWeightModeNull()?DBNull.Value:(object)row.WeightMode),
                                new SqlParameter("@Color_Desc",row.IsColor_DescNull()?DBNull.Value:(object)row.Color_Desc),
                                new SqlParameter("@Factory",row.IsFactoryNull()?DBNull.Value:(object)row.Factory),
                                new SqlParameter("@SheeterID",row.IsSheeterIDNull()?DBNull.Value:(object)row.SheeterID),
                                new SqlParameter("@SheetShift",row.IsSheetShiftNull()?DBNull.Value:(object)row.SheetShift),
                                new SqlParameter("@SheetDate",row.IsSheetDateNull()?DBNull.Value:(object)row.SheetDate),
                                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3),
                                new SqlParameter("@Remark4",row.IsRemark4Null()?DBNull.Value:(object)row.Remark4),
                                new SqlParameter("@Remark5",row.IsRemark5Null()?DBNull.Value:(object)row.Remark5),
                                new SqlParameter("@IsDelete",row.IsIsDeleteNull()?DBNull.Value:(object)row.IsDelete),
                                new SqlParameter("@CONVEYOR_NBR",row.IsCONVEYOR_NBRNull()?DBNull.Value:(object)row.CONVEYOR_NBR),
                                new SqlParameter("@IsNBR",row.IsIsNBRNull()?DBNull.Value:(object)row.IsNBR),
                                new SqlParameter("@IsWrapOK",row.IsIsWrapOKNull()?DBNull.Value:(object)row.IsWrapOK),
                                new SqlParameter("@IsJetOK",row.IsIsJetOKNull()?DBNull.Value:(object)row.IsJetOK),
                                new SqlParameter("@IsMeasure",row.IsIsMeasureNull()?DBNull.Value:(object)row.IsMeasure),
                                new SqlParameter("@IsWeightScale",row.IsIsWeightScaleNull()?DBNull.Value:(object)row.IsWeightScale),
                                new SqlParameter("@MeasureTime",row.IsMeasureTimeNull()?DBNull.Value:(object)row.MeasureTime),
                                new SqlParameter("@WeightScaleTime",row.IsWeightScaleTimeNull()?DBNull.Value:(object)row.WeightScaleTime),
                                new SqlParameter("@MESTime",row.IsMESTimeNull()?DBNull.Value:(object)row.MESTime),
                                new SqlParameter("@LastEditTime",row.IsLastEditTimeNull()?DBNull.Value:(object)row.LastEditTime)
                                
                                };
                    #endregion

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductUpdateAllByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }
        }

        public bool Roll_ProductUpdateAllByProductID(MainDS.Roll_ProductRow row, bool IsUpdateDBNull)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    #region sqlstr
                    string sqlstr = "UPDATE Roll_Product SET IsUploadERP=0,"; //修改数据后，设置重新传递
                    //if (IsUpdateDBNull || !row.IsOnlyIDNull())
                    //    sqlstr = sqlstr + " OnlyID = @OnlyID,";
                    if (IsUpdateDBNull || !row.IsProductTypeNull())
                        sqlstr = sqlstr + " ProductType = @ProductType,";
                    //if (IsUpdateDBNull || !row.IsProductIDNull())
                    //    sqlstr = sqlstr + " ProductID = @ProductID,";
                    if (IsUpdateDBNull || !row.IsBarcodeNull())
                        sqlstr = sqlstr + " Barcode = @Barcode,";
                    if (IsUpdateDBNull || !row.IsMachineIDNull())
                        sqlstr = sqlstr + " MachineID = @MachineID,";
                    if (IsUpdateDBNull || !row.IsJumboShiftNull())
                        sqlstr = sqlstr + " JumboShift = @JumboShift,";
                    if (IsUpdateDBNull || !row.IsJumboDateNull())
                        sqlstr = sqlstr + " JumboDate = @JumboDate,";
                    if (IsUpdateDBNull || !row.IsJumboNONull())
                        sqlstr = sqlstr + " JumboNO = @JumboNO,";
                    if (IsUpdateDBNull || !row.IsRewinderSetNull())
                        sqlstr = sqlstr + " RewinderSet = @RewinderSet,";
                    if (IsUpdateDBNull || !row.IsRewinderSetIndexNull())
                        sqlstr = sqlstr + " RewinderSetIndex = @RewinderSetIndex,";
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
                    if (IsUpdateDBNull || !row.IsDirectionNull())
                        sqlstr = sqlstr + " Direction = @Direction,";
                    if (IsUpdateDBNull || !row.IsBasisweightNull())
                        sqlstr = sqlstr + " Basisweight = @Basisweight,";
                    if (IsUpdateDBNull || !row.IsWidth_MNull())
                        sqlstr = sqlstr + " Width_M = @Width_M,";
                    if (IsUpdateDBNull || !row.IsWidth_MetsoNull())
                        sqlstr = sqlstr + " Width_Metso = @Width_Metso,";
                    if (IsUpdateDBNull || !row.IsWidthNull())
                        sqlstr = sqlstr + " Width = @Width,";
                    if (IsUpdateDBNull || !row.IsPalletLengthNull())
                        sqlstr = sqlstr + " PalletLength = @PalletLength,";
                    if (IsUpdateDBNull || !row.IsWidthModeNull())
                        sqlstr = sqlstr + " WidthMode = @WidthMode,";
                    if (IsUpdateDBNull || !row.IsRollLengthNull())
                        sqlstr = sqlstr + " RollLength = @RollLength,";
                    if (IsUpdateDBNull || !row.IsWeight_CalcNull())
                        sqlstr = sqlstr + " Weight_Calc = @Weight_Calc,";
                    if (IsUpdateDBNull || !row.IsInspector_DescNull())
                        sqlstr = sqlstr + " Inspector_Desc = @Inspector_Desc,";
                    if (IsUpdateDBNull || !row.IsWinderIDNull())
                        sqlstr = sqlstr + " WinderID = @WinderID,";
                    if (IsUpdateDBNull || !row.IsDiameter_MNull())
                        sqlstr = sqlstr + " Diameter_M = @Diameter_M,";
                    if (IsUpdateDBNull || !row.IsDiameter_MetsoNull())
                        sqlstr = sqlstr + " Diameter_Metso = @Diameter_Metso,";
                    if (IsUpdateDBNull || !row.IsDiameterNull())
                        sqlstr = sqlstr + " Diameter = @Diameter,";
                    if (IsUpdateDBNull || !row.IsCoreNull())
                        sqlstr = sqlstr + " Core = @Core,";
                    if (IsUpdateDBNull || !row.IsCoreWeightNull())
                        sqlstr = sqlstr + " CoreWeight = @CoreWeight,";
                    if (IsUpdateDBNull || !row.IsSpliceNull())
                        sqlstr = sqlstr + " Splice = @Splice,";
                    if (IsUpdateDBNull || !row.IsHolesNull())
                        sqlstr = sqlstr + " Holes = @Holes,";
                    if (IsUpdateDBNull || !row.IsReWinderShiftNull())
                        sqlstr = sqlstr + " ReWinderShift = @ReWinderShift,";
                    if (IsUpdateDBNull || !row.IsReWinderDateNull())
                        sqlstr = sqlstr + " ReWinderDate = @ReWinderDate,";
                    if (IsUpdateDBNull || !row.IsDestinationNull())
                        sqlstr = sqlstr + " Destination = @Destination,";
                    if (IsUpdateDBNull || !row.IsDegradeCauseNull())
                        sqlstr = sqlstr + " DegradeCause = @DegradeCause,";
                    if (IsUpdateDBNull || !row.IsCustomerNull())
                        sqlstr = sqlstr + " Customer = @Customer,";
                    if (IsUpdateDBNull || !row.IsRollWrapIDNull())
                        sqlstr = sqlstr + " RollWrapID = @RollWrapID,";
                    if (IsUpdateDBNull || !row.IsRollWrapShiftNull())
                        sqlstr = sqlstr + " RollWrapShift = @RollWrapShift,";
                    if (IsUpdateDBNull || !row.IsRollWrapDateNull())
                        sqlstr = sqlstr + " RollWrapDate = @RollWrapDate,";
                    if (IsUpdateDBNull || !row.IsWeight_GrossNull())
                        sqlstr = sqlstr + " Weight_Gross = @Weight_Gross,";
                    if (IsUpdateDBNull || !row.IsWeight_WeiNull())
                        sqlstr = sqlstr + " Weight_Wei = @Weight_Wei,";
                    if (IsUpdateDBNull || !row.IsWeight_NetNull())
                        sqlstr = sqlstr + " Weight_Net = @Weight_Net,";
                    if (IsUpdateDBNull || !row.IsWeightModeNull())
                        sqlstr = sqlstr + " WeightMode = @WeightMode,";
                    if (IsUpdateDBNull || !row.IsColor_DescNull())
                        sqlstr = sqlstr + " Color_Desc = @Color_Desc,";
                    if (IsUpdateDBNull || !row.IsFactoryNull())
                        sqlstr = sqlstr + " Factory = @Factory,";
                    if (IsUpdateDBNull || !row.IsSheeterIDNull())
                        sqlstr = sqlstr + " SheeterID = @SheeterID,";
                    if (IsUpdateDBNull || !row.IsSheetShiftNull())
                        sqlstr = sqlstr + " SheetShift = @SheetShift,";
                    if (IsUpdateDBNull || !row.IsSheetDateNull())
                        sqlstr = sqlstr + " SheetDate = @SheetDate,";
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
                    if (IsUpdateDBNull || !row.IsCONVEYOR_NBRNull())
                        sqlstr = sqlstr + " CONVEYOR_NBR = @CONVEYOR_NBR,";
                    if (IsUpdateDBNull || !row.IsIsNBRNull())
                        sqlstr = sqlstr + " IsNBR = @IsNBR,";
                    if (IsUpdateDBNull || !row.IsIsWrapOKNull())
                        sqlstr = sqlstr + " IsWrapOK = @IsWrapOK,";
                    if (IsUpdateDBNull || !row.IsIsJetOKNull())
                        sqlstr = sqlstr + " IsJetOK = @IsJetOK,";
                    if (IsUpdateDBNull || !row.IsIsMeasureNull())
                        sqlstr = sqlstr + " IsMeasure = @IsMeasure,";
                    if (IsUpdateDBNull || !row.IsIsWeightScaleNull())
                        sqlstr = sqlstr + " IsWeightScale = @IsWeightScale,";
                    if (IsUpdateDBNull || !row.IsMeasureTimeNull())
                        sqlstr = sqlstr + " MeasureTime = @MeasureTime,";
                    if (IsUpdateDBNull || !row.IsWeightScaleTimeNull())
                        sqlstr = sqlstr + " WeightScaleTime = @WeightScaleTime,";
                    if (IsUpdateDBNull || !row.IsMESTimeNull())
                        sqlstr = sqlstr + " MESTime = @MESTime,";
                    if (IsUpdateDBNull || !row.IsLastEditTimeNull())
                        sqlstr = sqlstr + " LastEditTime = @LastEditTime,";
                    sqlstr = sqlstr.TrimEnd(new char[] { ' ', ',' });
                    sqlstr = sqlstr + " WHERE ProductID = @ProductID";
                    #endregion

                    #region parameter
                    SqlParameter[] parameter = new SqlParameter[]
                                {
                                //new SqlParameter("@OnlyID", row.OnlyID),
                                new SqlParameter("@ProductType",row.IsProductTypeNull()?DBNull.Value:(object)row.ProductType),
                                new SqlParameter("@ProductID",(object)row.ProductID),
                                 new SqlParameter("@Barcode",row.IsBarcodeNull()?DBNull.Value:(object)row.Barcode),
                                new SqlParameter("@MachineID",row.IsMachineIDNull()?DBNull.Value:(object)row.MachineID),
                                new SqlParameter("@JumboShift",row.IsJumboShiftNull()?DBNull.Value:(object)row.JumboShift),
                                new SqlParameter("@JumboDate",row.IsJumboDateNull()?DBNull.Value:(object)row.JumboDate),
                                new SqlParameter("@JumboNO",row.IsJumboNONull()?DBNull.Value:(object)row.JumboNO),
                                new SqlParameter("@RewinderSet",row.IsRewinderSetNull()?DBNull.Value:(object)row.RewinderSet),
                                new SqlParameter("@RewinderSetIndex",row.IsRewinderSetIndexNull()?DBNull.Value:(object)row.RewinderSetIndex),
                                new SqlParameter("@Type",row.IsTypeNull()?DBNull.Value:(object)row.Type),
                                new SqlParameter("@Grade",row.IsGradeNull()?DBNull.Value:(object)row.Grade),
                                new SqlParameter("@Stand_Desc",row.IsStand_DescNull()?DBNull.Value:(object)row.Stand_Desc),
                                new SqlParameter("@OrderNO",row.IsOrderNONull()?DBNull.Value:(object)row.OrderNO),
                                new SqlParameter("@MaterialCode",row.IsMaterialCodeNull()?DBNull.Value:(object)row.MaterialCode),
                                new SqlParameter("@PartNO",row.IsPartNONull()?DBNull.Value:(object)row.PartNO),
                                new SqlParameter("@Direction",row.IsDirectionNull()?DBNull.Value:(object)row.Direction),
                                new SqlParameter("@Basisweight",row.IsBasisweightNull()?DBNull.Value:(object)row.Basisweight),
                                new SqlParameter("@Width_M",row.IsWidth_MNull()?DBNull.Value:(object)row.Width_M),
                                new SqlParameter("@Width_Metso",row.IsWidth_MetsoNull()?DBNull.Value:(object)row.Width_Metso),
                                new SqlParameter("@Width",row.IsWidthNull()?DBNull.Value:(object)row.Width),
                                new SqlParameter("@PalletLength",row.IsPalletLengthNull()?DBNull.Value:(object)row.PalletLength),
                                new SqlParameter("@WidthMode",row.IsWidthModeNull()?DBNull.Value:(object)row.WidthMode),
                                new SqlParameter("@RollLength",row.IsRollLengthNull()?DBNull.Value:(object)row.RollLength),
                                new SqlParameter("@Weight_Calc",row.IsWeight_CalcNull()?DBNull.Value:(object)row.Weight_Calc),
                                new SqlParameter("@Inspector_Desc",row.IsInspector_DescNull()?DBNull.Value:(object)row.Inspector_Desc),
                                new SqlParameter("@WinderID",row.IsWinderIDNull()?DBNull.Value:(object)row.WinderID),
                                new SqlParameter("@Diameter_M",row.IsDiameter_MNull()?DBNull.Value:(object)row.Diameter_M),
                                new SqlParameter("@Diameter_Metso",row.IsDiameter_MetsoNull()?DBNull.Value:(object)row.Diameter_Metso),
                                new SqlParameter("@Diameter",row.IsDiameterNull()?DBNull.Value:(object)row.Diameter),
                                new SqlParameter("@Core",row.IsCoreNull()?DBNull.Value:(object)row.Core),
                                new SqlParameter("@CoreWeight",row.IsCoreWeightNull()?DBNull.Value:(object)row.CoreWeight),
                                new SqlParameter("@Splice",row.IsSpliceNull()?DBNull.Value:(object)row.Splice),
                                new SqlParameter("@Holes",row.IsHolesNull()?DBNull.Value:(object)row.Holes),
                                new SqlParameter("@ReWinderShift",row.IsReWinderShiftNull()?DBNull.Value:(object)row.ReWinderShift),
                                new SqlParameter("@ReWinderDate",row.IsReWinderDateNull()?DBNull.Value:(object)row.ReWinderDate),
                                new SqlParameter("@Destination",row.IsDestinationNull()?DBNull.Value:(object)row.Destination),
                                new SqlParameter("@DegradeCause",row.IsDegradeCauseNull()?DBNull.Value:(object)row.DegradeCause),
                                new SqlParameter("@Customer",row.IsCustomerNull()?DBNull.Value:(object)row.Customer),
                                new SqlParameter("@RollWrapID",row.IsRollWrapIDNull()?DBNull.Value:(object)row.RollWrapID),
                                new SqlParameter("@RollWrapShift",row.IsRollWrapShiftNull()?DBNull.Value:(object)row.RollWrapShift),
                                new SqlParameter("@RollWrapDate",row.IsRollWrapDateNull()?DBNull.Value:(object)row.RollWrapDate),
                                new SqlParameter("@Weight_Gross",row.IsWeight_GrossNull()?DBNull.Value:(object)row.Weight_Gross),
                                new SqlParameter("@Weight_Wei",row.IsWeight_WeiNull()?DBNull.Value:(object)row.Weight_Wei),
                                new SqlParameter("@Weight_Net",row.IsWeight_NetNull()?DBNull.Value:(object)row.Weight_Net),
                                new SqlParameter("@WeightMode",row.IsWeightModeNull()?DBNull.Value:(object)row.WeightMode),
                                new SqlParameter("@Color_Desc",row.IsColor_DescNull()?DBNull.Value:(object)row.Color_Desc),
                                new SqlParameter("@Factory",row.IsFactoryNull()?DBNull.Value:(object)row.Factory),
                                new SqlParameter("@SheeterID",row.IsSheeterIDNull()?DBNull.Value:(object)row.SheeterID),
                                new SqlParameter("@SheetShift",row.IsSheetShiftNull()?DBNull.Value:(object)row.SheetShift),
                                new SqlParameter("@SheetDate",row.IsSheetDateNull()?DBNull.Value:(object)row.SheetDate),
                                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3),
                                new SqlParameter("@Remark4",row.IsRemark4Null()?DBNull.Value:(object)row.Remark4),
                                new SqlParameter("@Remark5",row.IsRemark5Null()?DBNull.Value:(object)row.Remark5),
                                new SqlParameter("@IsDelete",row.IsIsDeleteNull()?DBNull.Value:(object)row.IsDelete),
                                new SqlParameter("@CONVEYOR_NBR",row.IsCONVEYOR_NBRNull()?DBNull.Value:(object)row.CONVEYOR_NBR),
                                new SqlParameter("@IsNBR",row.IsIsNBRNull()?DBNull.Value:(object)row.IsNBR),
                                new SqlParameter("@IsWrapOK",row.IsIsWrapOKNull()?DBNull.Value:(object)row.IsWrapOK),
                                new SqlParameter("@IsJetOK",row.IsIsJetOKNull()?DBNull.Value:(object)row.IsJetOK),
                                new SqlParameter("@IsMeasure",row.IsIsMeasureNull()?DBNull.Value:(object)row.IsMeasure),
                                new SqlParameter("@IsWeightScale",row.IsIsWeightScaleNull()?DBNull.Value:(object)row.IsWeightScale),
                                new SqlParameter("@MeasureTime",row.IsMeasureTimeNull()?DBNull.Value:(object)row.MeasureTime),
                                new SqlParameter("@WeightScaleTime",row.IsWeightScaleTimeNull()?DBNull.Value:(object)row.WeightScaleTime),
                                new SqlParameter("@MESTime",row.IsMESTimeNull()?DBNull.Value:(object)row.MESTime),
                                new SqlParameter("@LastEditTime",row.IsLastEditTimeNull()?DBNull.Value:(object)row.LastEditTime)
                                
                                };
                    #endregion

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductUpdateAllByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }
        }

        public bool Roll_ProductInsertByRow(MainDS.Roll_ProductRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //string sqlstr = "INSERT INTO Roll_Product( OnlyID,ProductType,ProductID,Barcode,MachineID,JumboShift,JumboDate,JumboNO,RewinderSet,RewinderSetIndex,Type,Grade,Stand_Desc,OrderNO,MaterialCode,PartNO,Direction,Basisweight,Width_M,Width_Metso,Width,PalletLength,WidthMode,RollLength,Weight_Calc,Inspector_Desc,WinderID,Diameter_M,Diameter_Metso,Diameter,Core,CoreWeight,Splice,Holes,ReWinderShift,ReWinderDate,Destination,DegradeCause,Customer,RollWrapID,RollWrapShift,RollWrapDate,Weight_Gross,Weight_Wei,Weight_Net,WeightMode,Color_Desc,Factory,SheeterID,SheetShift,SheetDate,Remark1,Remark2,Remark3,Remark4,Remark5,IsDelete,CONVEYOR_NBR,IsNBR,IsWrapOK,IsJetOK,IsMeasure,IsWeightScale,MeasureTime,WeightScaleTime,MESTime,LastEditTime)VALUES(@OnlyID,@ProductType,@ProductID,@Barcode,@MachineID,@JumboShift,@JumboDate,@JumboNO,@RewinderSet,@RewinderSetIndex,@Type,@Grade,@Stand_Desc,@OrderNO,@MaterialCode,@PartNO,@Direction,@Basisweight,@Width_M,@Width_Metso,@Width,@PalletLength,@WidthMode,@RollLength,@Weight_Calc,@Inspector_Desc,@WinderID,@Diameter_M,@Diameter_Metso,@Diameter,@Core,@CoreWeight,@Splice,@Holes,@ReWinderShift,@ReWinderDate,@Destination,@DegradeCause,@Customer,@RollWrapID,@RollWrapShift,@RollWrapDate,@Weight_Gross,@Weight_Wei,@Weight_Net,@WeightMode,@Color_Desc,@Factory,@SheeterID,@SheetShift,@SheetDate,@Remark1,@Remark2,@Remark3,@Remark4,@Remark5,@IsDelete,@CONVEYOR_NBR,@IsNBR,@IsWrapOK,@IsJetOK,@IsMeasure,@IsWeightScale,@MeasureTime,@WeightScaleTime,@MESTime,@LastEditTime);";

                string sqlstr = "INSERT INTO Roll_Product(ProductType,ProductID,Barcode,MachineID,JumboShift,JumboDate,JumboNO,RewinderSet,RewinderSetIndex,Type,Grade,Stand_Desc,OrderNO,MaterialCode,PartNO,Direction,Basisweight,Width_M,Width_Metso,Width,PalletLength,WidthMode,RollLength,Weight_Calc,Inspector_Desc,WinderID,Diameter_M,Diameter_Metso,Diameter,Core,CoreWeight,Splice,Holes,ReWinderShift,ReWinderDate,Destination,DegradeCause,Customer,RollWrapID,RollWrapShift,RollWrapDate,Weight_Gross,Weight_Wei,Weight_Net,WeightMode,Color_Desc,Factory,SheeterID,SheetShift,SheetDate,Remark1,Remark2,Remark3,Remark4,Remark5,IsDelete,CONVEYOR_NBR,IsNBR,IsWrapOK,IsJetOK,IsMeasure,IsWeightScale,MeasureTime,WeightScaleTime,MESTime,LastEditTime)"
                + " VALUES(@ProductType,@ProductID,@Barcode,@MachineID,@JumboShift,@JumboDate,@JumboNO,@RewinderSet,@RewinderSetIndex,@Type,@Grade,@Stand_Desc,@OrderNO,@MaterialCode,@PartNO,@Direction,@Basisweight,@Width_M,@Width_Metso,@Width,@PalletLength,@WidthMode,@RollLength,@Weight_Calc,@Inspector_Desc,@WinderID,@Diameter_M,@Diameter_Metso,@Diameter,@Core,@CoreWeight,@Splice,@Holes,@ReWinderShift,@ReWinderDate,@Destination,@DegradeCause,@Customer,@RollWrapID,@RollWrapShift,@RollWrapDate,@Weight_Gross,@Weight_Wei,@Weight_Net,@WeightMode,@Color_Desc,@Factory,@SheeterID,@SheetShift,@SheetDate,@Remark1,@Remark2,@Remark3,@Remark4,@Remark5,@IsDelete,@CONVEYOR_NBR,@IsNBR,@IsWrapOK,@IsJetOK,@IsMeasure,@IsWeightScale,@MeasureTime,@WeightScaleTime,ISNULL(@MESTime,getdate()),ISNULL(@LastEditTime,getdate()));";
       

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
                //new SqlParameter("@OnlyID", row.OnlyID),
                new SqlParameter("@ProductType",row.IsProductTypeNull()?DBNull.Value:(object)row.ProductType),
                new SqlParameter("@ProductID", (object)row.ProductID),
               new SqlParameter("@Barcode",row.IsBarcodeNull()?DBNull.Value:(object)row.Barcode),
                new SqlParameter("@MachineID",row.IsMachineIDNull()?DBNull.Value:(object)row.MachineID),
                new SqlParameter("@JumboShift",row.IsJumboShiftNull()?DBNull.Value:(object)row.JumboShift),
                new SqlParameter("@JumboDate",row.IsJumboDateNull()?DBNull.Value:(object)row.JumboDate),
                new SqlParameter("@JumboNO",row.IsJumboNONull()?DBNull.Value:(object)row.JumboNO),
                new SqlParameter("@RewinderSet",row.IsRewinderSetNull()?DBNull.Value:(object)row.RewinderSet),
                new SqlParameter("@RewinderSetIndex",row.IsRewinderSetIndexNull()?DBNull.Value:(object)row.RewinderSetIndex),
                new SqlParameter("@Type",row.IsTypeNull()?DBNull.Value:(object)row.Type),
                new SqlParameter("@Grade",row.IsGradeNull()?DBNull.Value:(object)row.Grade),
                new SqlParameter("@Stand_Desc",row.IsStand_DescNull()?DBNull.Value:(object)row.Stand_Desc),
                new SqlParameter("@OrderNO",row.IsOrderNONull()?DBNull.Value:(object)row.OrderNO),
                new SqlParameter("@MaterialCode",row.IsMaterialCodeNull()?DBNull.Value:(object)row.MaterialCode),
                new SqlParameter("@PartNO",row.IsPartNONull()?DBNull.Value:(object)row.PartNO),
                new SqlParameter("@Direction",row.IsDirectionNull()?DBNull.Value:(object)row.Direction),
                new SqlParameter("@Basisweight",row.IsBasisweightNull()?DBNull.Value:(object)row.Basisweight),
                new SqlParameter("@Width_M",row.IsWidth_MNull()?DBNull.Value:(object)row.Width_M),
                new SqlParameter("@Width_Metso",row.IsWidth_MetsoNull()?DBNull.Value:(object)row.Width_Metso),
                new SqlParameter("@Width",row.IsWidthNull()?DBNull.Value:(object)row.Width),
                new SqlParameter("@PalletLength",row.IsPalletLengthNull()?DBNull.Value:(object)row.PalletLength),
                new SqlParameter("@WidthMode",row.IsWidthModeNull()?DBNull.Value:(object)row.WidthMode),
                new SqlParameter("@RollLength",row.IsRollLengthNull()?DBNull.Value:(object)row.RollLength),
                new SqlParameter("@Weight_Calc",row.IsWeight_CalcNull()?DBNull.Value:(object)row.Weight_Calc),
                new SqlParameter("@Inspector_Desc",row.IsInspector_DescNull()?DBNull.Value:(object)row.Inspector_Desc),
                new SqlParameter("@WinderID",row.IsWinderIDNull()?DBNull.Value:(object)row.WinderID),
                new SqlParameter("@Diameter_M",row.IsDiameter_MNull()?DBNull.Value:(object)row.Diameter_M),
                new SqlParameter("@Diameter_Metso",row.IsDiameter_MetsoNull()?DBNull.Value:(object)row.Diameter_Metso),
                new SqlParameter("@Diameter",row.IsDiameterNull()?DBNull.Value:(object)row.Diameter),
                new SqlParameter("@Core",row.IsCoreNull()?DBNull.Value:(object)row.Core),
                new SqlParameter("@CoreWeight",row.IsCoreWeightNull()?DBNull.Value:(object)row.CoreWeight),
                new SqlParameter("@Splice",row.IsSpliceNull()?DBNull.Value:(object)row.Splice),
                new SqlParameter("@Holes",row.IsHolesNull()?DBNull.Value:(object)row.Holes),
                new SqlParameter("@ReWinderShift",row.IsReWinderShiftNull()?DBNull.Value:(object)row.ReWinderShift),
                new SqlParameter("@ReWinderDate",row.IsReWinderDateNull()?DBNull.Value:(object)row.ReWinderDate),
                new SqlParameter("@Destination",row.IsDestinationNull()?DBNull.Value:(object)row.Destination),
                new SqlParameter("@DegradeCause",row.IsDegradeCauseNull()?DBNull.Value:(object)row.DegradeCause),
                new SqlParameter("@Customer",row.IsCustomerNull()?DBNull.Value:(object)row.Customer),
                new SqlParameter("@RollWrapID",row.IsRollWrapIDNull()?DBNull.Value:(object)row.RollWrapID),
                new SqlParameter("@RollWrapShift",row.IsRollWrapShiftNull()?DBNull.Value:(object)row.RollWrapShift),
                new SqlParameter("@RollWrapDate",row.IsRollWrapDateNull()?DBNull.Value:(object)row.RollWrapDate),
                new SqlParameter("@Weight_Gross",row.IsWeight_GrossNull()?DBNull.Value:(object)row.Weight_Gross),
                new SqlParameter("@Weight_Wei",row.IsWeight_WeiNull()?DBNull.Value:(object)row.Weight_Wei),
                new SqlParameter("@Weight_Net",row.IsWeight_NetNull()?DBNull.Value:(object)row.Weight_Net),
                new SqlParameter("@WeightMode",row.IsWeightModeNull()?DBNull.Value:(object)row.WeightMode),
                new SqlParameter("@Color_Desc",row.IsColor_DescNull()?DBNull.Value:(object)row.Color_Desc),
                new SqlParameter("@Factory",row.IsFactoryNull()?DBNull.Value:(object)row.Factory),
                new SqlParameter("@SheeterID",row.IsSheeterIDNull()?DBNull.Value:(object)row.SheeterID),
                new SqlParameter("@SheetShift",row.IsSheetShiftNull()?DBNull.Value:(object)row.SheetShift),
                new SqlParameter("@SheetDate",row.IsSheetDateNull()?DBNull.Value:(object)row.SheetDate),
                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3),
                new SqlParameter("@Remark4",row.IsRemark4Null()?DBNull.Value:(object)row.Remark4),
                new SqlParameter("@Remark5",row.IsRemark5Null()?DBNull.Value:(object)row.Remark5),
                new SqlParameter("@IsDelete",row.IsIsDeleteNull()?DBNull.Value:(object)row.IsDelete),
                new SqlParameter("@CONVEYOR_NBR",row.IsCONVEYOR_NBRNull()?DBNull.Value:(object)row.CONVEYOR_NBR),
                new SqlParameter("@IsNBR",row.IsIsNBRNull()?DBNull.Value:(object)row.IsNBR),
                new SqlParameter("@IsWrapOK",row.IsIsWrapOKNull()?DBNull.Value:(object)row.IsWrapOK),
                new SqlParameter("@IsJetOK",row.IsIsJetOKNull()?DBNull.Value:(object)row.IsJetOK),
                new SqlParameter("@IsMeasure",row.IsIsMeasureNull()?DBNull.Value:(object)row.IsMeasure),
                new SqlParameter("@IsWeightScale",row.IsIsWeightScaleNull()?DBNull.Value:(object)row.IsWeightScale),
                new SqlParameter("@MeasureTime",row.IsMeasureTimeNull()?DBNull.Value:(object)row.MeasureTime),
                new SqlParameter("@WeightScaleTime",row.IsWeightScaleTimeNull()?DBNull.Value:(object)row.WeightScaleTime),
                par1,//new SqlParameter("@MESTime",row.IsMESTimeNull()?DBNull.Value:(object)row.MESTime),
                par2//new SqlParameter("@LastEditTime",row.IsLastEditTimeNull()?DBNull.Value:(object)row.LastEditTime)                
                };
                try
                {
                    connection.Open(); 

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Roll_ProductInsert Error:" + ex.ToString()));
                }
                return ret;
            }

        }

        public Int64 Roll_ProductInsertByRowAutoProductID(string InitProductID,  MainDS.Roll_ProductRow row)
        {          
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //string sqlstr = "INSERT INTO Roll_Product( OnlyID,ROLL_CODE,JUMBOROLL_CODE,WRAPPING_ID,STENCILLER_ID,CUSTOMER_REMARK_TEXT,QUALITY_REMARK_TEXT,DS_ID,COLOR_TEXT,CORE_SIZE,CORE_WEIGHT,CORE_TYPE,PAPER_TYPE,BASIC_WEIGHT,BRAND_TEXT,LENGTH,WEIGHT,WEIGHT_M,WIDTH,WIDTH_M,DIAMETER,DIAMETER_M,GRADE,SPLICE_NUM,PATCH_NUM,SET_NBR,REEL_NBR,EXCEPTION_POSITION_TEXT,EXCEPTION_TYPE,DEFECT,WINDER_DATE,PARENT_ROLL_ID,SOURCE_PAPERTYPE,SOURCE_BASIC_WEIGHT,SOURCE_GRADE,IGT,STRAP_ID,CONVEYOR_NBR,IsNBR,IsWrapOK,IsJetOK,IsMeasure,MESTime,LastEditTime)VALUES(@OnlyID,@ROLL_CODE,@JUMBOROLL_CODE,@WRAPPING_ID,@STENCILLER_ID,@CUSTOMER_REMARK_TEXT,@QUALITY_REMARK_TEXT,@DS_ID,@COLOR_TEXT,@CORE_SIZE,@CORE_WEIGHT,@CORE_TYPE,@PAPER_TYPE,@BASIC_WEIGHT,@BRAND_TEXT,@LENGTH,@WEIGHT,@WEIGHT_M,@WIDTH,@WIDTH_M,@DIAMETER,@DIAMETER_M,@GRADE,@SPLICE_NUM,@PATCH_NUM,@SET_NBR,@REEL_NBR,@EXCEPTION_POSITION_TEXT,@EXCEPTION_TYPE,@DEFECT,@WINDER_DATE,@PARENT_ROLL_ID,@SOURCE_PAPERTYPE,@SOURCE_BASIC_WEIGHT,@SOURCE_GRADE,@IGT,@STRAP_ID,@CONVEYOR_NBR,@IsNBR,@IsWrapOK,@IsJetOK,@IsMeasure,ISNULL(@MESTime,getdate()),ISNULL(@LastEditTime,getdate()));";
                string sqlstr ="";

                sqlstr = sqlstr + "set @OutProductID =(select ISNULL(max(ProductID), @OutProductID) from ROll_Product where WinderID=@WinderID and datediff(DAY,ReWinderDate,@ReWinderDate)=0);";
                sqlstr = sqlstr + "set @OutProductID = @OutProductID +1;";
                sqlstr = sqlstr +"INSERT INTO Roll_Product(ProductType,ProductID,Barcode,MachineID,JumboShift,JumboDate,JumboNO,RewinderSet,RewinderSetIndex,Type,Grade,Stand_Desc,OrderNO,MaterialCode,PartNO,Direction,Basisweight,Width_M,Width_Metso,Width,PalletLength,WidthMode,RollLength,Weight_Calc,Inspector_Desc,WinderID,Diameter_M,Diameter_Metso,Diameter,Core,CoreWeight,Splice,Holes,ReWinderShift,ReWinderDate,Destination,DegradeCause,Customer,RollWrapID,RollWrapShift,RollWrapDate,Weight_Gross,Weight_Wei,Weight_Net,WeightMode,Color_Desc,Factory,SheeterID,SheetShift,SheetDate,Remark1,Remark2,Remark3,Remark4,Remark5,IsDelete,CONVEYOR_NBR,IsNBR,IsWrapOK,IsJetOK,IsMeasure,IsWeightScale,MeasureTime,WeightScaleTime,MESTime,LastEditTime)"
                              + " VALUES(@ProductType,@OutProductID,@OutProductID,@MachineID,@JumboShift,@JumboDate,@JumboNO,@RewinderSet,@RewinderSetIndex,@Type,@Grade,@Stand_Desc,@OrderNO,@MaterialCode,@PartNO,@Direction,@Basisweight,@Width_M,@Width_Metso,@Width,@PalletLength,@WidthMode,@RollLength,@Weight_Calc,@Inspector_Desc,@WinderID,@Diameter_M,@Diameter_Metso,@Diameter,@Core,@CoreWeight,@Splice,@Holes,@ReWinderShift,@ReWinderDate,@Destination,@DegradeCause,@Customer,@RollWrapID,@RollWrapShift,@RollWrapDate,@Weight_Gross,@Weight_Wei,@Weight_Net,@WeightMode,@Color_Desc,@Factory,@SheeterID,@SheetShift,@SheetDate,@Remark1,@Remark2,@Remark3,@Remark4,@Remark5,@IsDelete,@CONVEYOR_NBR,@IsNBR,@IsWrapOK,@IsJetOK,@IsMeasure,@IsWeightScale,@MeasureTime,@WeightScaleTime,ISNULL(@MESTime,getdate()),ISNULL(@LastEditTime,getdate()));";

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
                //new SqlParameter("@OnlyID", row.OnlyID),
                new SqlParameter("@ProductType",row.IsProductTypeNull()?DBNull.Value:(object)row.ProductType),
                //new SqlParameter("@ProductID", (object)row.ProductID),
                //new SqlParameter("@Barcode",row.IsBarcodeNull()?DBNull.Value:(object)row.Barcode),
                 new SqlParameter("@MachineID",row.IsMachineIDNull()?DBNull.Value:(object)row.MachineID),
                new SqlParameter("@JumboShift",row.IsJumboShiftNull()?DBNull.Value:(object)row.JumboShift),
                new SqlParameter("@JumboDate",row.IsJumboDateNull()?DBNull.Value:(object)row.JumboDate),
                new SqlParameter("@JumboNO",row.IsJumboNONull()?DBNull.Value:(object)row.JumboNO),
                new SqlParameter("@RewinderSet",row.IsRewinderSetNull()?DBNull.Value:(object)row.RewinderSet),
                new SqlParameter("@RewinderSetIndex",row.IsRewinderSetIndexNull()?DBNull.Value:(object)row.RewinderSetIndex),
                new SqlParameter("@Type",row.IsTypeNull()?DBNull.Value:(object)row.Type),
                new SqlParameter("@Grade",row.IsGradeNull()?DBNull.Value:(object)row.Grade),
                new SqlParameter("@Stand_Desc",row.IsStand_DescNull()?DBNull.Value:(object)row.Stand_Desc),
                new SqlParameter("@OrderNO",row.IsOrderNONull()?DBNull.Value:(object)row.OrderNO),
                new SqlParameter("@MaterialCode",row.IsMaterialCodeNull()?DBNull.Value:(object)row.MaterialCode),
                new SqlParameter("@PartNO",row.IsPartNONull()?DBNull.Value:(object)row.PartNO),
                new SqlParameter("@Direction",row.IsDirectionNull()?DBNull.Value:(object)row.Direction),
                new SqlParameter("@Basisweight",row.IsBasisweightNull()?DBNull.Value:(object)row.Basisweight),
                new SqlParameter("@Width_M",row.IsWidth_MNull()?DBNull.Value:(object)row.Width_M),
                new SqlParameter("@Width_Metso",row.IsWidth_MetsoNull()?DBNull.Value:(object)row.Width_Metso),
                new SqlParameter("@Width",row.IsWidthNull()?DBNull.Value:(object)row.Width),
                new SqlParameter("@PalletLength",row.IsPalletLengthNull()?DBNull.Value:(object)row.PalletLength),
                new SqlParameter("@WidthMode",row.IsWidthModeNull()?DBNull.Value:(object)row.WidthMode),
                new SqlParameter("@RollLength",row.IsRollLengthNull()?DBNull.Value:(object)row.RollLength),
                new SqlParameter("@Weight_Calc",row.IsWeight_CalcNull()?DBNull.Value:(object)row.Weight_Calc),
                new SqlParameter("@Inspector_Desc",row.IsInspector_DescNull()?DBNull.Value:(object)row.Inspector_Desc),
                new SqlParameter("@WinderID",row.IsWinderIDNull()?DBNull.Value:(object)row.WinderID),
                new SqlParameter("@Diameter_M",row.IsDiameter_MNull()?DBNull.Value:(object)row.Diameter_M),
                new SqlParameter("@Diameter_Metso",row.IsDiameter_MetsoNull()?DBNull.Value:(object)row.Diameter_Metso),
                new SqlParameter("@Diameter",row.IsDiameterNull()?DBNull.Value:(object)row.Diameter),
                new SqlParameter("@Core",row.IsCoreNull()?DBNull.Value:(object)row.Core),
                new SqlParameter("@CoreWeight",row.IsCoreWeightNull()?DBNull.Value:(object)row.CoreWeight),
                new SqlParameter("@Splice",row.IsSpliceNull()?DBNull.Value:(object)row.Splice),
                new SqlParameter("@Holes",row.IsHolesNull()?DBNull.Value:(object)row.Holes),
                new SqlParameter("@ReWinderShift",row.IsReWinderShiftNull()?DBNull.Value:(object)row.ReWinderShift),
                new SqlParameter("@ReWinderDate",row.IsReWinderDateNull()?DBNull.Value:(object)row.ReWinderDate),
                new SqlParameter("@Destination",row.IsDestinationNull()?DBNull.Value:(object)row.Destination),
                new SqlParameter("@DegradeCause",row.IsDegradeCauseNull()?DBNull.Value:(object)row.DegradeCause),
                new SqlParameter("@Customer",row.IsCustomerNull()?DBNull.Value:(object)row.Customer),
                new SqlParameter("@RollWrapID",row.IsRollWrapIDNull()?DBNull.Value:(object)row.RollWrapID),
                new SqlParameter("@RollWrapShift",row.IsRollWrapShiftNull()?DBNull.Value:(object)row.RollWrapShift),
                new SqlParameter("@RollWrapDate",row.IsRollWrapDateNull()?DBNull.Value:(object)row.RollWrapDate),
                new SqlParameter("@Weight_Gross",row.IsWeight_GrossNull()?DBNull.Value:(object)row.Weight_Gross),
                new SqlParameter("@Weight_Wei",row.IsWeight_WeiNull()?DBNull.Value:(object)row.Weight_Wei),
                new SqlParameter("@Weight_Net",row.IsWeight_NetNull()?DBNull.Value:(object)row.Weight_Net),
                new SqlParameter("@WeightMode",row.IsWeightModeNull()?DBNull.Value:(object)row.WeightMode),
                new SqlParameter("@Color_Desc",row.IsColor_DescNull()?DBNull.Value:(object)row.Color_Desc),
                new SqlParameter("@Factory",row.IsFactoryNull()?DBNull.Value:(object)row.Factory),
                new SqlParameter("@SheeterID",row.IsSheeterIDNull()?DBNull.Value:(object)row.SheeterID),
                new SqlParameter("@SheetShift",row.IsSheetShiftNull()?DBNull.Value:(object)row.SheetShift),
                new SqlParameter("@SheetDate",row.IsSheetDateNull()?DBNull.Value:(object)row.SheetDate),
                new SqlParameter("@Remark1",row.IsRemark1Null()?DBNull.Value:(object)row.Remark1),
                new SqlParameter("@Remark2",row.IsRemark2Null()?DBNull.Value:(object)row.Remark2),
                new SqlParameter("@Remark3",row.IsRemark3Null()?DBNull.Value:(object)row.Remark3),
                new SqlParameter("@Remark4",row.IsRemark4Null()?DBNull.Value:(object)row.Remark4),
                new SqlParameter("@Remark5",row.IsRemark5Null()?DBNull.Value:(object)row.Remark5),
                new SqlParameter("@IsDelete",row.IsIsDeleteNull()?DBNull.Value:(object)row.IsDelete),
                new SqlParameter("@CONVEYOR_NBR",row.IsCONVEYOR_NBRNull()?DBNull.Value:(object)row.CONVEYOR_NBR),
                new SqlParameter("@IsNBR",row.IsIsNBRNull()?DBNull.Value:(object)row.IsNBR),
                new SqlParameter("@IsWrapOK",row.IsIsWrapOKNull()?DBNull.Value:(object)row.IsWrapOK),
                new SqlParameter("@IsJetOK",row.IsIsJetOKNull()?DBNull.Value:(object)row.IsJetOK),
                new SqlParameter("@IsMeasure",row.IsIsMeasureNull()?DBNull.Value:(object)row.IsMeasure),
                new SqlParameter("@IsWeightScale",row.IsIsWeightScaleNull()?DBNull.Value:(object)row.IsWeightScale),
                new SqlParameter("@MeasureTime",row.IsMeasureTimeNull()?DBNull.Value:(object)row.MeasureTime),
                new SqlParameter("@WeightScaleTime",row.IsWeightScaleTimeNull()?DBNull.Value:(object)row.WeightScaleTime),
                par1,//new SqlParameter("@MESTime",row.IsMESTimeNull()?DBNull.Value:(object)row.MESTime),
                par2//new SqlParameter("@LastEditTime",row.IsLastEditTimeNull()?DBNull.Value:(object)row.LastEditTime)                
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                   
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Roll_ProductInsert Error:" + ex.ToString()));

                    return -1;
                }
                return Convert.ToInt64(par0.Value);
            }

        }
        
        public bool Roll_ProductDeleteByOnlyID(int OnlyID)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "delete  from Roll_Product where OnlyID='" + OnlyID.ToString() + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product删除成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }

        }

        public bool Roll_ProductDeleteByProductID(string ProductID)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "delete  from Roll_Product where ProductID='" + ProductID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductDeleteByProductID 成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }

        }

        public int Roll_ProductCountByOnlyID(int OnlyID)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                int Count = 0;
                try
                {
                    Count = (int)MSSqlHelper.ExecuteScalar(connection,
                      CommandType.Text,
                      "select Count(*) from Roll_Product where OnlyID='" + OnlyID + "'"
                      );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

                return Count;
            }

        }

        public int Roll_ProductCountByOnlyID(string OnlyID)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                int Count = 0;
                try
                {
                    Count = (int)MSSqlHelper.ExecuteScalar(connection,
                      CommandType.Text,
                      "select Count(*) from Roll_Product where OnlyID='" + OnlyID + "'"
                      );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

                return Count;
            }

        }

        public bool Roll_ProductDestinationUpdateByOnlyID(string OnlyID, string Destination)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product set Destination = '" + Destination + "' where OnlyID='" + OnlyID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductDestinationUpdateByOnlyID 执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductDestinationUpdateByProductID(string ProductID, string Destination)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product set Destination = '" + Destination + "' where ProductID='" + ProductID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true,  "Roll_ProductDestinationUpdateByProductID 执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ProductID + "/" + Destination + ":" + ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductDestinationUpdateByProductIDs(System.Collections.ArrayList ProductIDs, string Destination)
        {
            string sqltxt = "update  Roll_Product set Destination = '" + Destination + "' where ProductID in(" + String.Join(",", ProductIDs.ToArray()) + ")";

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          sqltxt
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductDestinationUpdateByProductIDs 执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false,
                        "sqlscript:" 
                        + sqltxt
                        +"-Roll_ProductDestinationUpdateByProductIDs" 
                        + ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductIsNBRUpdateByRollID(string RollID, bool IsNBR)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqltext = "update  Roll_Product  set IsNBR=" + (IsNBR ? "1" : "0") + " where ProductID='" + RollID + "'";


                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          sqltext
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product修改成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductCONVEYOR_NBRUpdateByProductID(string ProductID, string CONVEYOR_NBR, string DS_ID)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set CONVEYOR_NBR = '" + CONVEYOR_NBR + "',Destination='" + DS_ID + "',IsNBR=1 where ProductID='" + ProductID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductCONVEYOR_NBRUpdateByProductID修改成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductWRAPPING_IDUpdateByRollID(string RollID, string Wrap_ID)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set WRAPPING_ID = '" + Wrap_ID + "' where OnlyID='" + RollID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product修改成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductSTENCILLER_IDUpdateByRollID(string RollID, string STENCILLER_ID)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set STENCILLER_ID = '" + STENCILLER_ID + "' where OnlyID='" + RollID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product修改成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductWidth_MUpdateByOnlyID(string OnlyID, int width_m)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set Width_M='" + width_m.ToString() + "',IsMeasure=1,MeasureTime = getdate() where OnlyID='" + OnlyID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductWidth_MUpdateByOnlyID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false,"Roll_ProductWidth_MUpdateByOnlyID error:"+ ex.ToString()));
                    return false;
                }
                return true;
            }
        }
        public bool Roll_ProductDiameter_MUpdateByOnlyID(string OnlyID, int diameter_m)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set DIAMETER_M ='" + diameter_m.ToString() + "' where OnlyID='" + OnlyID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductDiameter_MUpdateByOnlyID:OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false,"Roll_ProductDiameter_MUpdateByOnlyID:" + ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        /// <summary>
        /// 保存测量重量，同时设置是否测量为真
        /// </summary>
        /// <param name="RollID"></param>
        /// <param name="weight_m"></param>
        /// <returns></returns>
        public bool Roll_ProductWeight_MUpdateByRollID(string RollID, int weight_m)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set WEIGHT_M='" + weight_m.ToString() + "', IsWeightScale=1,WeightScaleTime = getdate() where OnlyID='" + RollID + "'"
                            );


                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product修改成功"));
                }
                catch (Exception ex)
                {


                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public bool Roll_ProductMetsoMValueUpdateByProductID(string ProductID, int width_m,int diameter_m,int weight_m)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set  Width_Metso='" + width_m.ToString() + "',Diameter_Metso ='" + diameter_m.ToString() + "', Weight_Wei='" + weight_m.ToString() + "', IsWeightScale=1,WeightScaleTime = getdate() where ProductID='" + ProductID + "'"
                            );


                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductMetsoMValueUpdateByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Roll_ProductMetsoMValueUpdateByProductID:" + ex.ToString()));
                    return false;
                }
                return true;
            }


        }
        
        public bool Roll_ProductIsJetOKUpdateByRollID(string RollID, bool isok)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product set IsJetOK=" + (isok ? "1" : "0") + " where OnlyID='" + RollID + "'"
                            );


                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product修改成功"));
                }
                catch (Exception ex)
                {


                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        public MainDS Roll_ProductQueryAllByMeasureTimeInHours(string position,int hours)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@Position", position),
                new SqlParameter("@BeforeMeasureTime",Utils.DateTimeNow.AddHours(hours) )
                };

              DateTime time = Utils.DateTimeNow.AddHours(hours);
             // string sqltext = "select top 1000 * from Roll_Product where MeasureTime>=@BeforeMeasureTime order by MeasureTime desc;";
              //string sqltext = "select top 1000 Roll_Product.* from  Roll_Product inner join RecordWeight on RecordWeight.RollID = Roll_Product.OnlyID  where  WeightTime>='2012-08-17 08:13:37.247' ";

              string sqltext = " select  Roll_Product.* from Roll_Product inner join (select RecordWeight.RollID from RecordWeight where RecordWeight.Position=@Position group by RecordWeight.RollID) as tt on tt.RollID =Roll_Product.OnlyID where Roll_Product.MeasureTime>=@BeforeMeasureTime;" +
                  "select RecordWeight.* from RecordWeight inner join Roll_Product on RecordWeight.RollID = Roll_Product.OnlyID where RecordWeight.Position=@Position and Roll_Product.MeasureTime>@BeforeMeasureTime;";
 
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          sqltext,
                          ds,
                          new string[] { "Roll_Product", "RecordWeight" },
                          parameter
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
        
        public DataSet Roll_Product_TotalQueryAllByMeasureTimeInHours(int hours)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //WINDER_DATE   MESTime
                 DateTime time = Utils.DateTimeNow.AddHours(hours);
  
                string sqltext = "select top 8 COUNT(*) as Amount, SUBSTRING(convert(varchar, MeasureTime ,120),0,14) as HourInterval from Roll_Product where MeasureTime>='" + time.ToString() + "' group by SUBSTRING(convert(varchar, MeasureTime ,120),0,14) order by SUBSTRING(convert(varchar, MeasureTime ,120),0,14);";
                 
                try
                {
                    MSSqlHelper.FillDataset(connection,
                          CommandType.Text,
                          sqltext,
                          ds,
                          new string[] {   "Roll_Product_Total" }
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product_Total读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }

            }
            return ds;

        }
        
        public MainDS Roll_ProductQueryAllByFK(string onlyid, DateTime begintime, DateTime endtime)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select top " + Utils.SelectRowsMax + " * from Roll_Product where 1=1  ";
                if (onlyid != "")
                    sqlstr = sqlstr + " and OnlyID = @OnlyID";

                sqlstr = sqlstr + " and MESTime >= @BeginTime";
                sqlstr = sqlstr + " and MESTime <= @EndTime";
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@OnlyID", onlyid),
                                new SqlParameter("@BeginTime",begintime),
                                new SqlParameter("@EndTime",endtime),
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "Roll_Product" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductQueryAllByFK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                }
                return ds;
            }
        }

        public int Roll_ProductQueryIncreaseNO(string WinderID, DateTime ReWinderDate)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                int ret = -1;

                #region sqlstr
                string sqlstr = " select ISNULL(max(ProductID),'0000') from ROll_Product where WinderID=@WinderID and datediff(DAY,ReWinderDate,@ReWinderDate)=0";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                      new SqlParameter("@WinderID", WinderID),
                                     new SqlParameter("@ReWinderDate", ReWinderDate)
                                 };
                #endregion
                try
                {
                    connection.Open();

                   object obj=  MSSqlHelper.ExecuteScalar(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);

                   string rollid = obj.ToString();

                   if (rollid.Length > 4)
                   {
                       ret = Convert.ToInt32(rollid.Substring(9,4));
                   } 
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductQueryIncreaseNO执行成功"));
                }
                catch (Exception ex)
                {
                    ret = -1;
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
                return ret;
             
            }
        }

        //public MainDS Roll_ProductQueryAllForUploadERP()
        //{
        //    MainDS ds = new MainDS();
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        string sqltext = "select top 500 * from roll_product where isnull(isuploaderp,0) =0 order by mestime desc;";

        //        try
        //        {
        //            MSSqlHelper.FillDataset(connection,
        //                  CommandType.Text,
        //                  sqltext,
        //                  ds,
        //                  new string[] { "Roll_Product"}
        //                  );
        //            OnSqlStateChange(new SqlStateEventArgs(true, "Roll_Product读取成功"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
        //        }
        //    }
        //    return ds;
        //}

        public bool Roll_ProductIsUploadERPUpdateByRollID(string RollID, bool IsUploadERP)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                          "update  Roll_Product  set IsUploadERP='" + (IsUploadERP?"1":"0") + "' where ProductID='" + RollID + "'"
                            );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductIsUploadERPUpdateByRollID修改成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Roll_ProductIsUploadERPUpdateByRollID Error:"+ex.ToString()));
                    return false;
                }
                return true;
            }


        }

        //Roll_Product && KONEJOIN
        //public MainDS Roll_ProductQueryByOnlyIDsForKONEJoin(System.Collections.ArrayList OnlyIDs, string JoinType)
        //{
        //    MainDS ds = new MainDS();
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {

        //        string strsql = "select Roll_Product.* from GripperJoin " +
        //                        "inner join Roll_Product on GripperJoin.ProductOnlyID= Roll_Product.OnlyID " +
        //                        "where Roll_Product.OnlyID in(" + String.Join(",", OnlyIDs.ToArray()) + ") and GripperJoin.JoinType='" + JoinType + "'" +
        //                        "order by GripperJoin.SortIndex";


        //        //SqlParameter[] parameter = new SqlParameter[]
        //        //{
        //        //new SqlParameter("@OnlyID", OnlyID)              
        //        //};

        //        try
        //        {
        //            MSSqlHelper.FillDataset(connection,
        //                  CommandType.Text,
        //                  strsql,
        //                  ds,
        //                  new string[] { "Roll_Product" }
        //                //parameter
        //                  );

        //            OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductQueryByOnlyIDs 成功"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "Roll_ProductQueryByOnlyIDs 失败:" + ex.ToString()));
        //        }
        //    }
        //    return ds;
        //}     


       

        //LadingNumToProducts
        public bool LadingNumToProductsInsertByValue(string LadingNum, string ProductID, string ProductTye, DateTime LogTime, string ScanShift)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO LadingNumToProducts(LadingNum,ProductID,ProductTye,LogTime,ScanShift)VALUES(@LadingNum,@ProductID,@ProductTye,@LogTime,@ScanShift);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@LadingNum", LadingNum),
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@ProductTye", ProductTye),
                new SqlParameter("@LogTime", LogTime),
                new SqlParameter("@ScanShift", ScanShift)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumToProductsInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumToProductsInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public MainDS LadingNumToProductsQueryAllByPK(string LadingNum, string ProductID)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select * from  LadingNumToProducts where LadingNum=@LadingNum and ProductID=@ProductID;";
                
                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@LadingNum", LadingNum),
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "LadingNumToProducts" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumToProductsQueryAllByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumToProductsQueryAllByPK Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public MainDS LadingNumToProductsQueryAllByProductID(string ProductID)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select * from  LadingNumToProducts where ProductID=@ProductID;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {                             
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "LadingNumToProducts" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumToProductsQueryAllByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumToProductsQueryAllByProductID Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        
        public bool LadingNumToProductsDeleteByPK(string LadingNum, string ProductID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "delete from  LadingNumToProducts where LadingNum=@LadingNum and ProductID=@ProductID;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@LadingNum", LadingNum),
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr, 
                     parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumToProductsDeleteByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumToProductsDeleteByPK Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public bool LadingNumToProductsDeleteByProductID(string ProductID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "delete from  LadingNumToProducts where ProductID=@ProductID;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {                              
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumToProductsDeleteByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumToProductsDeleteByProductID Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public DataSet LadingNumQueryRollInfoByNum(string LadingNum)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count( Roll_Product.ProductID) as Amount,cast(sum(Roll_Product.Weight_Calc) as float) as Weight_Total from LadingNumToProducts " +
                  "inner join Roll_Product on LadingNumToProducts.ProductID= Roll_Product.ProductID " +
                  "where LadingNumToProducts.LadingNum =@LadingNum;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@LadingNum", LadingNum)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "LadingNumInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumQueryRollInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumQueryRollInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet LadingNumQueryPalletInfoByNum(string LadingNum)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count(Pallet_Product.ProductID) as Amount,sum(Pallet_Product.Weight_Calc) as Weight_Total from LadingNumToProducts " +
                  "inner join Pallet_Product on LadingNumToProducts.ProductID= Pallet_Product.ProductID " +
                  "where LadingNumToProducts.LadingNum =@LadingNum;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@LadingNum", LadingNum)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "LadingNumInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumQueryPalletInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumQueryPalletInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }
      
        public DataSet LadingNumQueryRollTotalInfoByShift(string ScanShift, DateTime LogTime)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count( Roll_Product.ProductID) as Amount,cast(sum(Roll_Product.Weight_Calc) as float) as Weight_Total from LadingNumToProducts " +
                  "inner join Roll_Product on LadingNumToProducts.ProductID= Roll_Product.ProductID " +
                   "where ScanShift=@ScanShift and DATEDIFF(day, LogTime, @LogTime)=0 ;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@ScanShift", ScanShift),
                                  new SqlParameter("@LogTime", LogTime)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "LadingNumInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumQueryRollInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumQueryRollInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet LadingNumQueryPalletTotalInfoByShift(string ScanShift, DateTime LogTime)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count(Pallet_Product.ProductID) as Amount,sum(Pallet_Product.Weight_Calc) as Weight_Total from LadingNumToProducts " +
                  "inner join Pallet_Product on LadingNumToProducts.ProductID= Pallet_Product.ProductID " +
                   "where ScanShift=@ScanShift and DATEDIFF(day, LogTime, @LogTime)=0 ;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@ScanShift", ScanShift),
                                  new SqlParameter("@LogTime", LogTime)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "LadingNumInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "LadingNumQueryPalletInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "LadingNumQueryPalletInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }


        //OrderNOToProducts
        public bool OrderNOToProductsInsertByValue(string OrderNO, string ProductID, string ProductTye, DateTime LogTime,string ScanShift)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO OrderNOToProducts( OrderNO,ProductID,ProductTye,LogTime,ScanShift)VALUES(@OrderNO,@ProductID,@ProductTye,@LogTime,@ScanShift);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@OrderNO", OrderNO),
                    new SqlParameter("@ProductID", ProductID),
                    new SqlParameter("@ProductTye", ProductTye),
                    new SqlParameter("@LogTime", LogTime),
                    new SqlParameter("@ScanShift", ScanShift)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOToProductsInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOToProductsInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public MainDS OrderNOToProductsQueryAllByPK(string OrderNO, string ProductID)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select * from  OrderNOToProducts where OrderNO=@OrderNO and ProductID=@ProductID;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@OrderNO", OrderNO),
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "OrderNOToProducts" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOToProductsQueryAllByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOToProductsQueryAllByPK Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public MainDS OrderNOToProductsQueryAllByProductID(string ProductID)
        {
            MainDS ds = new MainDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select * from  OrderNOToProducts where ProductID=@ProductID;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {                             
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "OrderNOToProducts" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOToProductsQueryAllByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOToProductsQueryAllByProductID Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public bool OrderNOToProductsDeleteByPK(string OrderNO, string ProductID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "delete from  OrderNOToProducts where OrderNO=@OrderNO and ProductID=@ProductID;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@OrderNO", OrderNO),
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOToProductsDeleteByPK执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOToProductsDeleteByPK Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public bool OrderNOToProductsDeleteByProductID(string ProductID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "delete from  OrderNOToProducts where ProductID=@ProductID;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {                              
                                new SqlParameter("@ProductID",ProductID)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,
                      CommandType.Text,
                     sqlstr,
                     parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOToProductsDeleteByProductID执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOToProductsDeleteByProductID Error:" + ex.ToString()));
                }
                return ret;
            }
        }

        public DataSet OrderNOQueryRollInfoByNum(string OrderNO)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count( Roll_Product.ProductID) as Amount,cast(sum(Roll_Product.Weight_Calc) as float) as Weight_Total from OrderNOToProducts " +
                  "inner join Roll_Product on OrderNOToProducts.ProductID= Roll_Product.ProductID " +
                  "where OrderNOToProducts.OrderNO =@OrderNO;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@OrderNO", OrderNO)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "OrderNOInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOQueryRollInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOQueryRollInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet OrderNOQueryPalletInfoByNum(string OrderNO)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count(Pallet_Product.ProductID) as Amount,sum(Pallet_Product.Weight_Calc) as Weight_Total from OrderNOToProducts " +
                  "inner join Pallet_Product on OrderNOToProducts.ProductID= Pallet_Product.ProductID " +
                  "where OrderNOToProducts.OrderNO =@OrderNO;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                new SqlParameter("@OrderNO", OrderNO)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "OrderNOInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOQueryPalletInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOQueryPalletInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet OrderNOQueryRollTotalInfoByShift(string ScanShift, DateTime LogTime)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count( Roll_Product.ProductID) as Amount,cast(sum(Roll_Product.Weight_Calc) as float) as Weight_Total from OrderNOToProducts " +
                  "inner join Roll_Product on OrderNOToProducts.ProductID= Roll_Product.ProductID " +
                  "where ScanShift=@ScanShift and DATEDIFF(day, LogTime, @LogTime)=0 ;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                  new SqlParameter("@ScanShift", ScanShift),
                                  new SqlParameter("@LogTime", LogTime)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "OrderNOInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOQueryRollInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOQueryRollInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet OrderNOQueryPalletTotalInfoByShift(string ScanShift, DateTime LogTime)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                #region sqlstr
                string sqlstr = "select count(Pallet_Product.ProductID) as Amount,sum(Pallet_Product.Weight_Calc) as Weight_Total from OrderNOToProducts " +
                  "inner join Pallet_Product on OrderNOToProducts.ProductID= Pallet_Product.ProductID " +
                   "where ScanShift=@ScanShift and DATEDIFF(day, LogTime, @LogTime)=0 ;";

                #endregion

                #region parameter
                SqlParameter[] parameter = new SqlParameter[]
                                {
                                    new SqlParameter("@ScanShift", ScanShift),
                                    new SqlParameter("@LogTime", LogTime)
                                 };
                #endregion
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,
                      CommandType.Text,
                     sqlstr,
                     ds,
                     new string[] { "OrderNOInfo" },
                     parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "OrderNOQueryPalletInfoByNum执行成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "OrderNOQueryPalletInfoByNum Error:" + ex.ToString()));
                }
                return ds;
            }
        }



        //Pallet




        //中间表

        //TOCT_Material
        public bool TOCT_MaterialInsertByRow(ERPDS.TOCT_MaterialRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO TOCT_Material( MaterialCode,MaterialName,Spec,ProductType,PaperType,Basisweight,Grade,Width,Diameter,Core,PalletLength,CreateTime,Flag,Note)VALUES(@MaterialCode,@MaterialName,@Spec,@ProductType,@PaperType,@Basisweight,@Grade,@Width,@Diameter,@Core,@PalletLength,@CreateTime,@Flag,@Note);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@MaterialCode", row.MaterialCode),
                new SqlParameter("@MaterialName", row.MaterialName),
                new SqlParameter("@Spec",row.IsSpecNull()?DBNull.Value:(object)row.Spec),
                new SqlParameter("@ProductType", row.ProductType),
                new SqlParameter("@PaperType", row.PaperType),
                new SqlParameter("@Basisweight",row.IsBasisweightNull()?DBNull.Value:(object)row.Basisweight),
                new SqlParameter("@Grade", row.Grade),
                new SqlParameter("@Width", row.Width),
                new SqlParameter("@Diameter", row.Diameter),
                new SqlParameter("@Core", row.Core),
                new SqlParameter("@PalletLength", row.PalletLength),
                new SqlParameter("@CreateTime", row.CreateTime),
                new SqlParameter("@Flag", row.Flag),
                new SqlParameter("@Note",row.IsNoteNull()?DBNull.Value:(object)row.Note)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_MaterialInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_MaterialInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS TOCT_MaterialQueryByMaterialCode(string MaterialCode)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
             
                string sqlstr = "select top 500 * from TOCT_Material where MaterialCode=@MaterialCode;";
                             
                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@MaterialCode", MaterialCode)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_MaterialQueryByMaterialCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_MaterialQueryByMaterialCode Error:" + ex.ToString()));
                }
                return ds;
            }
        }        
        public bool TOCT_MaterialDeleteByMaterialCode(string MaterialCode)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
            
                string sqlstr = "delete from TOCT_Material where MaterialCode=@MaterialCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@MaterialCode", MaterialCode)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,      
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_MaterialDeleteByMaterialCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_MaterialDeleteByMaterialCode Error:" + ex.ToString()));
                }              
            }
            return ret;
        }


        //TOCT_Department
        public bool TOCT_DepartmentInsertByRow(ERPDS.TOCT_DepartmentRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO TOCT_Department( DeptCode,DeptName,CreateTime,FLAG,Note)VALUES(@DeptCode,@DeptName,@CreateTime,@FLAG,@Note);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@DeptCode", row.DeptCode),
                new SqlParameter("@DeptName", row.DeptName),
                new SqlParameter("@CreateTime", row.CreateTime),
                new SqlParameter("@FLAG", row.FLAG),
                new SqlParameter("@Note",row.IsNoteNull()?DBNull.Value:(object)row.Note)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_DepartmentInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_DepartmentInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS TOCT_DepartmentQueryByDeptCode(string DeptCode)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select top 500 * from TOCT_Department where DeptCode=@DeptCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@DeptCode", DeptCode)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_DepartmentQueryByDeptCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_DepartmentQueryByDeptCode Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_DepartmentDeleteByDeptCode(string DeptCode)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "delete from TOCT_Department where DeptCode=@DeptCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@DeptCode", DeptCode)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_DepartmentDeleteByDeptCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_DepartmentDeleteByDeptCode Error:" + ex.ToString()));
                }
            }
            return ret;
        }

        //TOCT_Warehouse
        public bool TOCT_WarehouseInsertByRow(ERPDS.TOCT_WarehouseRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO TOCT_Warehouse( WareHouseCode,WarehouseName,CreateTime,FLAG,Note)VALUES(@WareHouseCode,@WarehouseName,@CreateTime,@FLAG,@Note);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@WareHouseCode", row.WareHouseCode),
                new SqlParameter("@WarehouseName", row.WarehouseName),
                new SqlParameter("@CreateTime", row.CreateTime),
                new SqlParameter("@FLAG", row.FLAG),
                new SqlParameter("@Note",row.IsNoteNull()?DBNull.Value:(object)row.Note)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_WarehouseInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_WarehouseInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS TOCT_WarehouseQueryByWareHouseCode(string WareHouseCode)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select top 500 * from TOCT_Warehouse where WareHouseCode=@WareHouseCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@WareHouseCode", WareHouseCode)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_WarehouseQueryByWareHouseCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_WarehouseQueryByWareHouseCode Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_WarehouseDeleteByWareHouseCode(string WareHouseCode)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "delete from TOCT_Warehouse where WareHouseCode=@WareHouseCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@WareHouseCode", WareHouseCode)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_WarehouseDeleteByWareHouseCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_WarehouseDeleteByWareHouseCode Error:" + ex.ToString()));
                }
            }
            return ret;
        }

        //TOCT_Location
        public bool TOCT_LocationInsertByRow(ERPDS.TOCT_LocationRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO TOCT_Location( LocationCode,LocationName,WarehouseCode,CreateTime,FLAG,Note)VALUES(@LocationCode,@LocationName,@WarehouseCode,@CreateTime,@FLAG,@Note);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@LocationCode", row.LocationCode),
                new SqlParameter("@LocationName", row.LocationName),
                new SqlParameter("@WarehouseCode", row.WarehouseCode),
                new SqlParameter("@CreateTime", row.CreateTime),
                new SqlParameter("@FLAG", row.FLAG),
                new SqlParameter("@Note",row.IsNoteNull()?DBNull.Value:(object)row.Note)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LocationInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LocationInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS TOCT_LocationQueryByLocationCode(string LocationCode)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "select top 500 * from TOCT_Location where LocationCode=@LocationCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LocationCode", LocationCode)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LocationQueryByLocationCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LocationQueryByLocationCode Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_LocationDeleteByLocationCode(string LocationCode)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "delete from TOCT_Location where LocationCode=@LocationCode;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LocationCode", LocationCode)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LocationDeleteByLocationCode OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LocationDeleteByLocationCode Error:" + ex.ToString()));
                }
            }
            return ret;
        }


        //TOCT_ORDER
        public bool TOCT_ORDERInsertByRow(ERPDS.TOCT_ORDERRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO TOCT_ORDER( SO_ID,SO_NUM,SO_FLBH,SO_CUSTNUM,SO_CUSTNAME,SO_DATE,SO_MATERIALCODE,SO_MATERIAL,SO_SPEC,SO_UNITCODE,SO_UNITNAME,SO_QTY,SO_FLAG,Note)VALUES(@SO_ID,@SO_NUM,@SO_FLBH,@SO_CUSTNUM,@SO_CUSTNAME,@SO_DATE,@SO_MATERIALCODE,@SO_MATERIAL,@SO_SPEC,@SO_UNITCODE,@SO_UNITNAME,@SO_QTY,@SO_FLAG,@Note);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@SO_ID", row.SO_ID),
                new SqlParameter("@SO_NUM", row.SO_NUM),
                new SqlParameter("@SO_FLBH", row.SO_FLBH),
                new SqlParameter("@SO_CUSTNUM", row.SO_CUSTNUM),
                new SqlParameter("@SO_CUSTNAME", row.SO_CUSTNAME),
                new SqlParameter("@SO_DATE", row.SO_DATE),
                new SqlParameter("@SO_MATERIALCODE", row.SO_MATERIALCODE),
                new SqlParameter("@SO_MATERIAL", row.SO_MATERIAL),
                new SqlParameter("@SO_SPEC",row.IsSO_SPECNull()?DBNull.Value:(object)row.SO_SPEC),
                new SqlParameter("@SO_UNITCODE",row.IsSO_UNITCODENull()?DBNull.Value:(object)row.SO_UNITCODE),
                new SqlParameter("@SO_UNITNAME",row.IsSO_UNITNAMENull()?DBNull.Value:(object)row.SO_UNITNAME),
                new SqlParameter("@SO_QTY", row.SO_QTY),
                new SqlParameter("@SO_FLAG", row.SO_FLAG),
                new SqlParameter("@Note",row.IsNoteNull()?"": row.Note)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_ORDERInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_ORDERInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS TOCT_ORDERQueryBySO_ID(string SO_ID)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select top 500 * from TOCT_ORDER where SO_ID=@SO_ID;";
                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@SO_ID", SO_ID)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_ORDERQueryBySO_ID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_ORDERQueryBySO_ID Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_ORDERDeleteBySO_ID(string SO_ID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "delete from TOCT_ORDER where SO_ID=@SO_ID;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@SO_ID", SO_ID)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_ORDERDeleteBySO_ID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_ORDERDeleteBySO_ID Error:" + ex.ToString()));
                }
            }
            return ret;
        }

        public ERPDS TOCT_ORDERQueryBySO_NUM(string SO_NUM)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "select top 500 * from TOCT_ORDER where SO_NUM=@SO_NUM;";
                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@SO_NUM", SO_NUM)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_ORDERQueryBySO_NUM OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_ORDERQueryBySO_NUM Error:" + ex.ToString()));
                }
                return ds;
            }
        }



        //TOCT_LADING
        public bool TOCT_LADINGInsertByRow(ERPDS.TOCT_LADINGRow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO TOCT_LADING( LADINGTYPE,LADINGNUM,LADINGDATE,CUSTNUM,CUSTNAME,MATERIALCODE,MATERIAL,UNITCODE,UNITNAME,LADINGQTY,LADINGQTY2,SUMMARYQTY,SUMMARYQTY2,Createtime,FLAG,NOTE)VALUES(@LADINGTYPE,@LADINGNUM,@LADINGDATE,@CUSTNUM,@CUSTNAME,@MATERIALCODE,@MATERIAL,@UNITCODE,@UNITNAME,@LADINGQTY,@LADINGQTY2,@SUMMARYQTY,@SUMMARYQTY2,@Createtime,@FLAG,@NOTE);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@LADINGTYPE", row.LADINGTYPE),
                new SqlParameter("@LADINGNUM", row.LADINGNUM),
                new SqlParameter("@LADINGDATE", row.LADINGDATE),
                new SqlParameter("@CUSTNUM", row.CUSTNUM),
                new SqlParameter("@CUSTNAME", row.CUSTNAME),
                new SqlParameter("@MATERIALCODE", row.MATERIALCODE),
                new SqlParameter("@MATERIAL", row.MATERIAL),
                new SqlParameter("@UNITCODE", row.UNITCODE),
                new SqlParameter("@UNITNAME", row.UNITNAME),
                new SqlParameter("@LADINGQTY", row.LADINGQTY),
                new SqlParameter("@LADINGQTY2", row.LADINGQTY2),
                new SqlParameter("@SUMMARYQTY", row.SUMMARYQTY),
                new SqlParameter("@SUMMARYQTY2", row.SUMMARYQTY2),
                new SqlParameter("@Createtime", row.Createtime),
                new SqlParameter("@FLAG", row.FLAG),
                new SqlParameter("@NOTE",row.IsNOTENull()?DBNull.Value:(object)row.NOTE)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LADINGInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LADINGInsert Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        public ERPDS TOCT_LADINGQueryByLADINGNUM(string LADINGNUM)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "select top 500 * from TOCT_LADING where LADINGNUM=@LADINGNUM;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LADINGNUM", LADINGNUM)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LADINGQueryByLADINGNUM OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LADINGQueryByLADINGNUM Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool TOCT_LADINGDeleteByLADINGNUM(string LADINGNUM)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "delete from TOCT_LADING where LADINGNUM=@LADINGNUM;";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@LADINGNUM", LADINGNUM)
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "TOCT_LADINGDeleteByLADINGNUM OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "TOCT_LADINGDeleteByLADINGNUM Error:" + ex.ToString()));
                }
            }
            return ret;
        }



        //CT_CODEDATA 
        public ERPDS CT_CODEDATAQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "select top 500 * from CT_CODEDATA where CreateTime > @CreateTime and Flag=@Flag;";

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
                          new string[] { "CT_CODEDATA" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_CODEDATAQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_CODEDATAQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool CT_CODEDATAUpdateForERPLoad(string BARCODE, FlagType Flag)
        {

            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "update CT_CODEDATA set Flag=@Flag where BARCODE= @BARCODE;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@BARCODE", BARCODE),
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_CODEDATAUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_CODEDATAUpdateForERPLoad Error:" + ex.ToString()));
                }

            }

            return ret;
        }
        public bool CT_CODEDATAInsertByRow(ERPDS.CT_CODEDATARow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO CT_CODEDATA( BARCODE,ProdDate,DEPARTNUM,ClassNum,BarCodeType,OrderNo,MATERIALCODE,MATERIALNAME,Weight,OPERATEDATE,CUSTOMERBATCH,OPERATORNAME,Createtime,FLAG,NOTE,RollLength,SheetCount)VALUES(@BARCODE,@ProdDate,@DEPARTNUM,@ClassNum,@BarCodeType,@OrderNo,@MATERIALCODE,@MATERIALNAME,@Weight,@OPERATEDATE,@CUSTOMERBATCH,@OPERATORNAME,@Createtime,@FLAG,@NOTE,@RollLength,@SheetCount);";

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

                string sqlstr = "select top 500 * from CT_CODEDATA where BARCODE=@BARCODE;";

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

                string sqlstr = "delete from CT_CODEDATA where BARCODE=@BARCODE;";

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
        public ERPDS CT_INSTOCKDATAQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "select top 500 * from CT_INSTOCKDATA where OPERATEDATE > @OPERATEDATE and FLAG=@FLAG;";

                //日期为字符型，比较字符型大小
                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime.ToString("yyyyMMdd");
                par1.DbType = DbType.String;
                par1.ParameterName = "@OPERATEDATE";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@FLAG", (int)Flag)
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_INSTOCKDATAQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_INSTOCKDATAQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool CT_INSTOCKDATAUpdateForERPLoad(string LSBH, FlagType Flag)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "update CT_INSTOCKDATA set FLAG=@FLAG where LSBH= @LSBH;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@LSBH", LSBH),
                  new SqlParameter("@FLAG", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_INSTOCKDATAUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_INSTOCKDATAUpdateForERPLoad Error:" + ex.ToString()));
                }
            }
            return ret;
        }
        public bool CT_INSTOCKDATAInsertByRow(ERPDS.CT_INSTOCKDATARow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO CT_INSTOCKDATA( BusinessType,RESTOCK,MATERIALCODE,MATERIALNAME,DEPARTNUM,STOCKNUM,STOCKNUM2,BARCODE,OPERATEDATE,OPERATORNAME,FLAG,NOTE,Shift,LogTime)VALUES(@BusinessType,@RESTOCK,@MATERIALCODE,@MATERIALNAME,@DEPARTNUM,@STOCKNUM,@STOCKNUM2,@BARCODE,@OPERATEDATE,@OPERATORNAME,@FLAG,@NOTE,@Shift,getdate());";

                SqlParameter[] parameter = new SqlParameter[]
                {
                ///new SqlParameter("@LSBH", row.LSBH),
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

                string sqlstr = "select top 500 * from CT_INSTOCKDATA where LSBH=@LSBH;";

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
        public void testTran() { 
        
            using(TransactionScope tran =new TransactionScope()){
            //CT_INSTOCKDATADeleteByLSBH（）；
            //CT_OUTSTOCKDATAQueryForERPLoad（）；
                
            tran.Complete();
            }
        }



        public bool CT_INSTOCKDATADeleteByLSBH(string LSBH)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "delete from CT_INSTOCKDATA where LSBH=@LSBH;";

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
        public ERPDS CT_OUTSTOCKDATAQueryForERPLoad(DateTime CreateTime, FlagType Flag)
        {
            ERPDS ds = new ERPDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "select top 500 * from CT_OUTSTOCKDATA where CreateTime > @CreateTime and FLAG=@FLAG;";

                //日期为字符型，比较字符型大小
                SqlParameter par1 = new SqlParameter();
                par1.Value = CreateTime;
                par1.DbType = DbType.DateTime;
                par1.ParameterName = "@CreateTime";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  new SqlParameter("@FLAG", (int)Flag)
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

                    


                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_OUTSTOCKDATAQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_OUTSTOCKDATAQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public bool CT_OUTSTOCKDATAUpdateForERPLoad(string LSBH, FlagType Flag)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //Flag :0-初始 1-成功 2-失败 9-完成

                string sqlstr = "update CT_OUTSTOCKDATA set FLAG=@FLAG where LSBH= @LSBH;";

                SqlParameter[] parameter = new SqlParameter[]
                {           
                  new SqlParameter("@LSBH", LSBH),
                  new SqlParameter("@FLAG", (int)Flag)
                };

                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_OUTSTOCKDATAUpdateForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_OUTSTOCKDATAUpdateForERPLoad Error:" + ex.ToString()));
                }
            }
            return ret;
        }        
        public bool CT_OUTSTOCKDATAInsertByRow(ERPDS.CT_OUTSTOCKDATARow row)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO CT_OUTSTOCKDATA( BusinessType,RESTOCK,LADINGNUM,STOCKOUTDATE,STOCKNUM,CUSTNUM,CUSTNAME,MATERIALCODE,MATERIAL,UNITCODE,UNITNAME,BARCODE,OPERATORNAME,Createtime,FLAG,NOTE,Shift,LogTime)VALUES(@BusinessType,@RESTOCK,@LADINGNUM,@STOCKOUTDATE,@STOCKNUM,@CUSTNUM,@CUSTNAME,@MATERIALCODE,@MATERIAL,@UNITCODE,@UNITNAME,@BARCODE,@OPERATORNAME,@Createtime,@FLAG,@NOTE,@Shift,getdate());";

                SqlParameter[] parameter = new SqlParameter[]
                {
                //new SqlParameter("@LSBH", row.LSBH),
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
                new SqlParameter("@Shift",row.IsShiftNull()?"0":row.Shift),

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
                string sqlstr = "select top 500 * from CT_OUTSTOCKDATA where LSBH=@LSBH;";

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

                string sqlstr = "delete from CT_OUTSTOCKDATA where LSBH=@LSBH;";

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



        public WMSDS Select_T_Factory(bool choose, bool local)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "[Select_T_Factory]";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@IsChoose", SqlDbType.Bit),
                  new SqlParameter("@IsLocal", SqlDbType.Bit)
                };
                parameter[0].Value = choose;
                parameter[1].Value = local;
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          sqlstr,
                          ds,
                          new string[] { "T_Factory" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_Factory OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_Factory Error:" + ex.ToString()));
                }
                return ds;
            }
        }
    }

}
