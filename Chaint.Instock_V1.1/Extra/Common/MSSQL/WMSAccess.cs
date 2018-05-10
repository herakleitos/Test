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
    public partial class WMSAccess
    {

        /// <summary>
        /// 工厂对象
        /// </summary>
        //   private DBAccess.IADOFactory factory;
        private string ConnctionString = "";
        private string MESConnectString = "";
        private string ZZConnctionString = "";

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
        public WMSAccess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnctionString = Utils.SQLConnectionString;
            MESConnectString = Utils.MESSQLConnectString;
            ZZConnctionString = Utils.ZZSQLConnctionString;
        }

        public WMSAccess(string connectionstr)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnctionString = connectionstr;
        }


        public bool _IsConect = false;
        public bool IsConnect
        {
            get { return _IsConect; }
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
                    "select * from Paper_Type where IsChoose=1;" +
                     "select * from Paper_DegradeCause where IsChoose=1;" +
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
        public MainDS T_UserQueryByNameAndPassword(String UserName, String Password)
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
                          new string[] { "PaperUser" }
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
        public bool PaperUserPriviligeDeleteByNameAndPriviligeName(string UserName, string PriviligeName)
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
                    OnSqlStateChange(new SqlStateEventArgs(false, "AppConfigDeleteByPosition error:" + ex.ToString()));
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
        public int ConveyLogInsert(string RollID, string InfoType, string Position, string IPAddress, string DeviceType, DateTime LogTime, string Description)
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
                    OnSqlStateChange(new SqlStateEventArgs(false, "ConveyLogInsert:" + ex.ToString()));
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
        public MainDS SheetBufferLockQueryBySheetIDAndLock(int SheetID, bool IsLock)
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

        public bool RecordWidthVerifyInsert(int widthleft, int widthright, int widthactual, string rollid, string position)
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

        public int RecordScanCountByRollIDAndPosition(DateTime ScanTime, string RollID, string Position)
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
        public bool GripperJoinInsertByRow(int JoinOnlyID, int ProductOnlyID, string JoinType, string ProductID, string SetID, int Amount, int SortIndex, string Position)
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

                    object obj = MSSqlHelper.ExecuteScalar(connection,//tran,
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

        public int GripperGroupQueryByFK(string Grade, int Basisweight, int Width, int Diameter, int CoreSize, string PaperType, string Customer)
        {
            int ret = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlwhere = "where Grade=@Grade " +
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

                    object obj = MSSqlHelper.ExecuteScalar(connection,//tran,
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
                          "select top " + Utils.SelectRowsMax + "  * from Roll_Product where MESTime>='" + begin.ToString() + "' and MESTime<='" + end.ToString() + "'",
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

       


        #region WMS 部分函数
        /// <summary>
        ///查询组织记录
        /// </summary>
        /// <param name="OrgLevel">组织级别</param>
        /// <param name="OrgParent">组织上级</param>
        /// <param name="isChoose">是否启用</param>
        /// <returns></returns>
        public WMSDS T_OrganizationQuery(int OrgLevel, int OrgParent, bool isChoose)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "select OnlyID,OrganizationCode,OrganizationName,OrganizationParent,OrganizationLevel from T_Organization where  OrganizationLevel = @OrganizationLevel and OrganizationParent =@OrganizationParent and IsChoose =@IsChoose";

                SqlParameter par1 = new SqlParameter();
                par1.Value = OrgLevel;
                par1.DbType = DbType.Int32;
                par1.ParameterName = "@OrgLevel";

                SqlParameter par2 = new SqlParameter();
                par2.Value = OrgParent;
                par2.DbType = DbType.Int32;
                par2.ParameterName = "@OrgParent";

                SqlParameter par3 = new SqlParameter();
                par3.Value = isChoose;
                par3.DbType = DbType.Boolean;
                par3.ParameterName = "@IsChoose";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                 par2,
                 par3
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Organization" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_OrganizationQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_OrganizationQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        /// <summary>
        /// 组织和业务和班组一起查询
        /// </summary>
        /// <param name="OrgLevel"></param>
        /// <param name="OrgParent"></param>
        /// <param name="busiType"></param>
        /// <param name="isChoose"></param>
        /// <returns></returns>
        public WMSDS Org_Factory_ShiftQuery(int OrgLevel, int OrgParent, string busiType, bool isChoose)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "select OnlyID,OrganizationCode,OrganizationName,OrganizationParent,OrganizationLevel from T_Organization where  OrganizationLevel = @OrganizationLevel and OrganizationParent =@OrganizationParent and IsChoose =@IsChoose";
                sqlstr += " select OnlyID, BusinessCode,BusinessName from T_Business_Type where BusinessType = @BusinessType and isChoose =@IsChoose";
                sqlstr += " select OnlyID, ShiftCode ,ShiftName From T_Shift where isChoose = @IsChoose";
                SqlParameter par1 = new SqlParameter();
                par1.Value = OrgLevel;
                par1.DbType = DbType.Int32;
                par1.ParameterName = "@OrgLevel";

                SqlParameter par2 = new SqlParameter();
                par2.Value = OrgParent;
                par2.DbType = DbType.Int32;
                par2.ParameterName = "@OrgParent";


                SqlParameter par3 = new SqlParameter();
                par3.Value = busiType;
                par3.DbType = DbType.String;
                par3.ParameterName = "@BusinessType";

                SqlParameter par4 = new SqlParameter();
                par4.Value = isChoose;
                par4.DbType = DbType.Boolean;
                par4.ParameterName = "@IsChoose";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                 par2,
                 par3,
                 par4
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Organization", "T_Business_Type", "T_Shift" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_OrganizationQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_OrganizationQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 用户和仓库一起查询
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="usertype"></param>
        /// <param name="whParent"></param>
        /// <param name="whLevel"></param>
        /// <param name="isChoose"></param>
        /// <returns></returns>
        public WMSDS User_WarehouseQuery(string factory, int usertype, string whParent, int whLevel, bool isChoose)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = "select OnlyID,UserCode,UserName from T_User where  userTypeID = @UserTypeID and factoryID =@FactoryID and IsChoose =@IsChoose";
                sqlstr += " select OnlyID, WHCode,WHName from T_Warehouse where whParent = @WHParent and whLevel =@WHLevel and whorgcode=@FactoryID isChoose =@IsChoose";
                SqlParameter par1 = new SqlParameter();
                par1.Value = factory;
                par1.DbType = DbType.String;
                par1.ParameterName = "@FactoryID";

                SqlParameter par2 = new SqlParameter();
                par2.Value = usertype;
                par2.DbType = DbType.Int32;
                par2.ParameterName = "@UserTypeID";


                SqlParameter par3 = new SqlParameter();
                par3.Value = whParent;
                par3.DbType = DbType.String;
                par3.ParameterName = "@WHParent";


                SqlParameter par4 = new SqlParameter();
                par4.Value = whLevel;
                par4.DbType = DbType.Int32;
                par4.ParameterName = "@WHLevel";

                SqlParameter par5 = new SqlParameter();
                par5.Value = isChoose;
                par5.DbType = DbType.Boolean;
                par5.ParameterName = "@IsChoose";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                 par2,
                 par3,
                 par4,
                 par5
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_User", "T_Warehouse" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_OrganizationQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_OrganizationQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }
        }


        /// <summary>
        /// 仓库查询
        /// </summary>
        /// <param name="factory">车间</param>
        /// <param name="parent">上级</param>
        /// <param name="level">级别</param>
        /// <param name="ischoose"></param>
        /// <returns></returns>
        public WMSDS T_WarehouseQuery(string factory, string parent, int level, bool ischoose)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = " select OnlyID, WHCode,WHName from T_Warehouse where whParent = @WHParent and whLevel =@WHLevel and whorgcode=@FactoryID isChoose =@IsChoose";
                SqlParameter par1 = new SqlParameter();
                par1.Value = factory;
                par1.DbType = DbType.String;
                par1.ParameterName = "@FactoryID";


                SqlParameter par2 = new SqlParameter();
                par2.Value = parent;
                par2.DbType = DbType.String;
                par2.ParameterName = "@WHParent";


                SqlParameter par3 = new SqlParameter();
                par3.Value = level;
                par3.DbType = DbType.Int32;
                par3.ParameterName = "@WHLevel";

                SqlParameter par4 = new SqlParameter();
                par4.Value = ischoose;
                par4.DbType = DbType.Boolean;
                par4.ParameterName = "@IsChoose";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                 par2,
                 par3,
                 par4
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Warehouse" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_WarehouseQueryForERPLoad OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_WarehouseQueryForERPLoad Error:" + ex.ToString()));
                }
                return ds;
            }

        }


        /// <summary>
        /// 查询仓库的入库记录
        /// </summary>
        /// <param name="productid">小于10就是查onlyid</param>
        /// <returns></returns>
        public WMSDS Select_T_Product_InByProductID(string productid)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = " select  * from T_Product_In ";
                
                if(productid.Length<10)
                    sqlstr += "where onlyid = @ProductID";

                else
              sqlstr+=  "where productid = @ProductID";
                SqlParameter par1 = new SqlParameter();
                par1.Value = productid;
                par1.DbType = DbType.String;
                par1.ParameterName = "@ProductID";




                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Product_In" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InByProductID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InByProductID Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        /// <summary>
        /// 查询仓库的库存记录
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public WMSDS T_Product_WarehouseQuery(string productid)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = " select  OnlyID,ProductID from T_Product_Warehouse where productid = @ProductID";
                SqlParameter par1 = new SqlParameter();
                par1.Value = productid;
                par1.DbType = DbType.String;
                par1.ParameterName = "@ProductID";




                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Product_Warehouse" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_Product_WarehouseQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_Product_Warehouse Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 查询楼上的卷筒生产数据
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public ProduceDS Roll_ProductQueryAllByFK(string productid,string machineID)
        {
            ProduceDS ds = new ProduceDS();
            //先查询机台的连接
            string machineStr = "";

      WMSDS wmsds=      this.Select_T_Factory(true,machineID);
      if (wmsds.T_Factory.Rows.Count > 0)
          machineStr = wmsds.T_Factory.Rows[0][wmsds.T_Factory.FactoryAddrColumn].ToString();
      else
          return ds;

            using (SqlConnection connection = new SqlConnection(machineStr))
            {

                //string sqlstr = " select * from Roll_Product where rollid = @ProductID";
                string sqlstr = "Roll_ProductQueryByProductIDNew";

                SqlParameter par1 = new SqlParameter();
                par1.Value = productid;
                par1.DbType = DbType.String;
                par1.ParameterName = "@ProductID";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          sqlstr,
                          ds,
                          new string[] { "Roll_Product" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Roll_ProductQueryByProductID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Roll_ProductQueryByProductID Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        //public int Insert_T_Product_In(WMSDS.T_Product_InRow row,string connString)
        //{
        //    int retid = -1;
        //    using (SqlConnection connection = new SqlConnection(connString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Insert_T_Product_In");
        //        #region 传参数
        //        //parameter[0].Value = row.OnlyID;
        //        if (row.IsProductIDNull())
        //        {
        //            parameter[0].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[0].Value = row.ProductID;
        //        }
        //        if (row.IsProductTypeCodeNull())
        //        {
        //            parameter[1].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[1].Value = row.ProductTypeCode;
        //        }
        //        if (row.IsBatchNONull())
        //        {
        //            parameter[2].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[2].Value = row.BatchNO;
        //        }
        //        if (row.IsFactoryNull())
        //        {
        //            parameter[3].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[3].Value = row.Factory;
        //        }
        //        if (row.IsMachineIDNull())
        //        {
        //            parameter[4].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[4].Value = row.MachineID;
        //        }
        //        if (row.IsRollCountNull())
        //        {
        //            parameter[5].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[5].Value = row.RollCount;
        //        }
        //        if (row.IsMaterialCodeNull())
        //        {
        //            parameter[6].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[6].Value = row.MaterialCode;
        //        }
        //        if (row.IsMaterialNameNull())
        //        {
        //            parameter[7].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[7].Value = row.MaterialName;
        //        }
        //        if (row.IsStandardNull())
        //        {
        //            parameter[8].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[8].Value = row.Standard;
        //        }
        //        if (row.IsProductNameNull())
        //        {
        //            parameter[9].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[9].Value = row.ProductName;
        //        }
        //        if (row.IsProductTypeNull())
        //        {
        //            parameter[10].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[10].Value = row.ProductType;
        //        }
        //        if (row.IsTrademarkNull())
        //        {
        //            parameter[11].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[11].Value = row.Trademark;
        //        }
        //        if (row.IsGradeNull())
        //        {
        //            parameter[12].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[12].Value = row.Grade;
        //        }
        //        if (row.IsBasisweightLabelNull())
        //        {
        //            parameter[13].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[13].Value = row.BasisweightLabel;
        //        }
        //        if (row.IsWidthLabelNull())
        //        {
        //            parameter[14].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[14].Value = row.WidthLabel;
        //        }
        //        if (row.IsDiameterLabelNull())
        //        {
        //            parameter[15].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[15].Value = row.DiameterLabel;
        //        }
        //        if (row.IsLengthLabelNull())
        //        {
        //            parameter[16].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[16].Value = row.LengthLabel;
        //        }
        //        if (row.IsWhiteDegreeNull())
        //        {
        //            parameter[17].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[17].Value = row.WhiteDegree;
        //        }
        //        if (row.IsColorNull())
        //        {
        //            parameter[18].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[18].Value = row.Color;
        //        }
        //        if (row.IsWeightModeNull())
        //        {
        //            parameter[19].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[19].Value = row.WeightMode;
        //        }
        //        if (row.IsWeightLabelNull())
        //        {
        //            parameter[20].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[20].Value = row.WeightLabel;
        //        }
        //        if (row.IsCoreDiameterNull())
        //        {
        //            parameter[21].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[21].Value = row.CoreDiameter;
        //        }
        //        if (row.IsSpliceNull())
        //        {
        //            parameter[22].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[22].Value = row.Splice;
        //        }
        //        if (row.IsCustCodeNull())
        //        {
        //            parameter[23].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[23].Value = row.CustCode;
        //        }
        //        if (row.IsOrderNONull())
        //        {
        //            parameter[24].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[24].Value = row.OrderNO;
        //        }
        //        if (row.IsCustTrademarkNull())
        //        {
        //            parameter[25].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[25].Value = row.CustTrademark;
        //        }
        //        if (row.IsPaperCertCodeNull())
        //        {
        //            parameter[26].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[26].Value = row.PaperCertCode;
        //        }
        //        if (row.IsSpecProdNameNull())
        //        {
        //            parameter[27].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[27].Value = row.SpecProdName;
        //        }
        //        if (row.IsSpecCustNameNull())
        //        {
        //            parameter[28].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[28].Value = row.SpecCustName;
        //        }
        //        if (row.IsDirectionNull())
        //        {
        //            parameter[29].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[29].Value = row.Direction;
        //        }
        //        if (row.IsIsWhiteFlagNull())
        //        {
        //            parameter[30].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[30].Value = row.IsWhiteFlag;
        //        }
        //        if (row.IsLayersNull())
        //        {
        //            parameter[31].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[31].Value = row.Layers;
        //        }
        //        if (row.IsSKUNull())
        //        {
        //            parameter[32].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[32].Value = row.SKU;
        //        }
        //        if (row.IsFinishNull())
        //        {
        //            parameter[33].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[33].Value = row.Finish;
        //        }
        //        if (row.IsPKGNull())
        //        {
        //            parameter[34].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[34].Value = row.PKG;
        //        }
        //        if (row.IsMWeightNull())
        //        {
        //            parameter[35].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[35].Value = row.MWeight;
        //        }
        //        if (row.IsSheetWidthLabelNull())
        //        {
        //            parameter[36].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[36].Value = row.SheetWidthLabel;
        //        }
        //        if (row.IsSheetLengthLabelNull())
        //        {
        //            parameter[37].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[37].Value = row.SheetLengthLabel;
        //        }
        //        if (row.IsPalletReamsNull())
        //        {
        //            parameter[38].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[38].Value = row.PalletReams;
        //        }
        //        if (row.IsSlidesOfReamNull())
        //        {
        //            parameter[39].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[39].Value = row.SlidesOfReam;
        //        }
        //        if (row.IsSlidesOfSheetNull())
        //        {
        //            parameter[40].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[40].Value = row.SlidesOfSheet;
        //        }
        //        if (row.IsTransportTypeNull())
        //        {
        //            parameter[41].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[41].Value = row.TransportType;
        //        }
        //        if (row.IsReamPackTypeNull())
        //        {
        //            parameter[42].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[42].Value = row.ReamPackType;
        //        }
        //        if (row.IsFiberDirectNull())
        //        {
        //            parameter[43].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[43].Value = row.FiberDirect;
        //        }
        //        if (row.IsPalletHeightNull())
        //        {
        //            parameter[44].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[44].Value = row.PalletHeight;
        //        }
        //        if (row.IsTrademarkStyleNull())
        //        {
        //            parameter[45].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[45].Value = row.TrademarkStyle;
        //        }
        //        if (row.IsPalletRemarkNull())
        //        {
        //            parameter[46].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[46].Value = row.PalletRemark;
        //        }
        //        if (row.IsRemarkNull())
        //        {
        //            parameter[47].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[47].Value = row.Remark;
        //        }
        //        if (row.IsRollWrap_RemarkNull())
        //        {
        //            parameter[48].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[48].Value = row.RollWrap_Remark;
        //        }
        //        if (row.IsWHRemarkNull())
        //        {
        //            parameter[49].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[49].Value = row.WHRemark;
        //        }
        //        if (row.IsBusinessTypeNull())
        //        {
        //            parameter[50].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[50].Value = row.BusinessType;
        //        }
        //        if (row.IsSourceVoucherNull())
        //        {
        //            parameter[51].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[51].Value = row.SourceVoucher;
        //        }
        //        if (row.IsWarehouseNull())
        //        {
        //            parameter[52].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[52].Value = row.Warehouse;
        //        }
        //        if (row.IsWHPositionNull())
        //        {
        //            parameter[53].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[53].Value = row.WHPosition;
        //        }
        //        if (row.IsStatusInNull())
        //        {
        //            parameter[54].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[54].Value = row.StatusIn;
        //        }
        //        if (row.IsInDateNull())
        //        {
        //            parameter[55].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[55].Value = row.InDate;
        //        }
        //        if (row.IsInShiftNull())
        //        {
        //            parameter[56].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[56].Value = row.InShift;
        //        }
        //        if (row.IsInShiftTimeNull())
        //        {
        //            parameter[57].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[57].Value = row.InShiftTime;
        //        }
        //        if (row.IsInUserNull())
        //        {
        //            parameter[58].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[58].Value = row.InUser;
        //        }
        //        if (row.IsStatusOutNull())
        //        {
        //            parameter[59].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[59].Value = row.StatusOut;
        //        }
        //        if (row.IsOutDateNull())
        //        {
        //            parameter[60].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[60].Value = row.OutDate;
        //        }
        //        if (row.IsOutShiftNull())
        //        {
        //            parameter[61].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[61].Value = row.OutShift;
        //        }
        //        if (row.IsOutUserNull())
        //        {
        //            parameter[62].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[62].Value = row.OutUser;
        //        }
        //        if (row.IsCdefine1Null())
        //        {
        //            parameter[63].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[63].Value = row.Cdefine1;
        //        }
        //        if (row.IsCdefine2Null())
        //        {
        //            parameter[64].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[64].Value = row.Cdefine2;
        //        }
        //        if (row.IsCdefine3Null())
        //        {
        //            parameter[65].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[65].Value = row.Cdefine3;
        //        }
        //        if (row.IsUdefine1Null())
        //        {
        //            parameter[66].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[66].Value = row.Udefine1;
        //        }
        //        if (row.IsUdefine2Null())
        //        {
        //            parameter[67].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[67].Value = row.Udefine2;
        //        }
        //        if (row.IsUdefine3Null())
        //        {
        //            parameter[68].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[68].Value = row.Udefine3;
        //        }
        //        if (row.IsIsDeletedNull())
        //        {
        //            parameter[69].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[69].Value = row.IsDeleted;
        //        }
        //        if (row.IsReadDateNull())
        //        {
        //            parameter[70].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[70].Value = row.ReadDate;
        //        }
        //        if (row.IsTradeModeNull())
        //        {
        //            parameter[71].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[71].Value = row.TradeMode;
        //        }
        //        if (row.IsSpecificationNull())
        //        {
        //            parameter[72].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[72].Value = row.Specification;
        //        }
        //        if (row.IsVoucherInIDNull())
        //        {
        //            parameter[73].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[73].Value = row.VoucherInID;
        //        }
        //        if (row.IsVoucherOutIDNull())
        //        {
        //            parameter[74].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[74].Value = row.VoucherOutID;
        //        }
        //        if (row.IsVoucherRetrieveIDNull())
        //        {
        //            parameter[75].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[75].Value = row.VoucherRetrieveID;
        //        }
        //        if (row.IsIsPolyHookNull())
        //        {
        //            parameter[76].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[76].Value = row.IsPolyHook;
        //        }
        //        if (row.IsSourcePIDNull())
        //        {
        //            parameter[77].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[77].Value = row.SourcePID;
        //        }
        //        #endregion
        //        parameter[78].Value = row.OnlyID;

        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "Insert_T_Product_In",
        //                parameter);

        //            retid = Convert.ToInt32(parameter[78].Value);

        //            OnSqlStateChange(new SqlStateEventArgs(true, "Insert_T_Product_In OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "Insert_T_Product_In Error:" + ex.ToString()));
        //        }
        //    }
        //    return retid;
        //}

        public int Insert_T_Product_In(WMSDS.T_Product_InRow row, string connStr)
        {
            int retid = -1;
            if (connStr == "")
                connStr = ConnctionString;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Insert_T_Product_In");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsProductIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.ProductID;
                }
                if (row.IsProductTypeCodeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductTypeCode;
                }
                if (row.IsBatchNONull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BatchNO;
                }
                if (row.IsFactoryNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Factory;
                }
                if (row.IsMachineIDNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.MachineID;
                }
                if (row.IsRollCountNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.RollCount;
                }
                if (row.IsMaterialCodeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.MaterialCode;
                }
                if (row.IsMaterialNameNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.MaterialName;
                }
                if (row.IsStandardNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Standard;
                }
                if (row.IsProductNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.ProductName;
                }
                if (row.IsProductTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.ProductType;
                }
                if (row.IsTrademarkNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Trademark;
                }
                if (row.IsGradeNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.Grade;
                }
                if (row.IsBasisweightLabelNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BasisweightLabel;
                }
                if (row.IsWidthLabelNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.WidthLabel;
                }
                if (row.IsDiameterLabelNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.DiameterLabel;
                }
                if (row.IsLengthLabelNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.LengthLabel;
                }
                if (row.IsWhiteDegreeNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.WhiteDegree;
                }
                if (row.IsColorNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.Color;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.WeightMode;
                }
                if (row.IsWeightLabelNull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.WeightLabel;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.CoreDiameter;
                }
                if (row.IsSpliceNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.Splice;
                }
                if (row.IsCustCodeNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.CustCode;
                }
                if (row.IsOrderNONull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.OrderNO;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.CustTrademark;
                }
                if (row.IsPaperCertCodeNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.PaperCertCode;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.SpecCustName;
                }
                if (row.IsDirectionNull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.Direction;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.IsWhiteFlag;
                }
                if (row.IsLayersNull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.Layers;
                }
                if (row.IsSKUNull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.SKU;
                }
                if (row.IsFinishNull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Finish;
                }
                if (row.IsPKGNull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.PKG;
                }
                if (row.IsMWeightNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.MWeight;
                }
                if (row.IsSheetWidthLabelNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.SheetWidthLabel;
                }
                if (row.IsSheetLengthLabelNull())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.SheetLengthLabel;
                }
                if (row.IsPalletReamsNull())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.PalletReams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[40].Value = DBNull.Value;
                }
                else
                {
                    parameter[40].Value = row.SlidesOfSheet;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[41].Value = DBNull.Value;
                }
                else
                {
                    parameter[41].Value = row.TransportType;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[42].Value = DBNull.Value;
                }
                else
                {
                    parameter[42].Value = row.ReamPackType;
                }
                if (row.IsFiberDirectNull())
                {
                    parameter[43].Value = DBNull.Value;
                }
                else
                {
                    parameter[43].Value = row.FiberDirect;
                }
                if (row.IsPalletHeightNull())
                {
                    parameter[44].Value = DBNull.Value;
                }
                else
                {
                    parameter[44].Value = row.PalletHeight;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[45].Value = DBNull.Value;
                }
                else
                {
                    parameter[45].Value = row.TrademarkStyle;
                }
                if (row.IsPalletRemarkNull())
                {
                    parameter[46].Value = DBNull.Value;
                }
                else
                {
                    parameter[46].Value = row.PalletRemark;
                }
                if (row.IsRemarkNull())
                {
                    parameter[47].Value = DBNull.Value;
                }
                else
                {
                    parameter[47].Value = row.Remark;
                }
                if (row.IsRollWrap_RemarkNull())
                {
                    parameter[48].Value = DBNull.Value;
                }
                else
                {
                    parameter[48].Value = row.RollWrap_Remark;
                }
                if (row.IsWHRemarkNull())
                {
                    parameter[49].Value = DBNull.Value;
                }
                else
                {
                    parameter[49].Value = row.WHRemark;
                }
                if (row.IsBusinessTypeNull())
                {
                    parameter[50].Value = DBNull.Value;
                }
                else
                {
                    parameter[50].Value = row.BusinessType;
                }
                if (row.IsSourceVoucherNull())
                {
                    parameter[51].Value = DBNull.Value;
                }
                else
                {
                    parameter[51].Value = row.SourceVoucher;
                }
                if (row.IsWarehouseNull())
                {
                    parameter[52].Value = DBNull.Value;
                }
                else
                {
                    parameter[52].Value = row.Warehouse;
                }
                if (row.IsWHPositionNull())
                {
                    parameter[53].Value = DBNull.Value;
                }
                else
                {
                    parameter[53].Value = row.WHPosition;
                }
                if (row.IsStatusInNull())
                {
                    parameter[54].Value = DBNull.Value;
                }
                else
                {
                    parameter[54].Value = row.StatusIn;
                }
                if (row.IsInDateNull())
                {
                    parameter[55].Value = DBNull.Value;
                }
                else
                {
                    parameter[55].Value = row.InDate;
                }
                if (row.IsInShiftNull())
                {
                    parameter[56].Value = DBNull.Value;
                }
                else
                {
                    parameter[56].Value = row.InShift;
                }
                if (row.IsInShiftTimeNull())
                {
                    parameter[57].Value = DBNull.Value;
                }
                else
                {
                    parameter[57].Value = row.InShiftTime;
                }
                if (row.IsInUserNull())
                {
                    parameter[58].Value = DBNull.Value;
                }
                else
                {
                    parameter[58].Value = row.InUser;
                }
                if (row.IsStatusOutNull())
                {
                    parameter[59].Value = DBNull.Value;
                }
                else
                {
                    parameter[59].Value = row.StatusOut;
                }
                if (row.IsOutDateNull())
                {
                    parameter[60].Value = DBNull.Value;
                }
                else
                {
                    parameter[60].Value = row.OutDate;
                }
                if (row.IsOutShiftNull())
                {
                    parameter[61].Value = DBNull.Value;
                }
                else
                {
                    parameter[61].Value = row.OutShift;
                }
                if (row.IsOutUserNull())
                {
                    parameter[62].Value = DBNull.Value;
                }
                else
                {
                    parameter[62].Value = row.OutUser;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[63].Value = DBNull.Value;
                }
                else
                {
                    parameter[63].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[64].Value = DBNull.Value;
                }
                else
                {
                    parameter[64].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[65].Value = DBNull.Value;
                }
                else
                {
                    parameter[65].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[66].Value = DBNull.Value;
                }
                else
                {
                    parameter[66].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[67].Value = DBNull.Value;
                }
                else
                {
                    parameter[67].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[68].Value = DBNull.Value;
                }
                else
                {
                    parameter[68].Value = row.Udefine3;
                }
                if (row.IsIsDeletedNull())
                {
                    parameter[69].Value = DBNull.Value;
                }
                else
                {
                    parameter[69].Value = row.IsDeleted;
                }
                if (row.IsReadDateNull())
                {
                    parameter[70].Value = DBNull.Value;
                }
                else
                {
                    parameter[70].Value = row.ReadDate;
                }
                if (row.IsTradeModeNull())
                {
                    parameter[71].Value = DBNull.Value;
                }
                else
                {
                    parameter[71].Value = row.TradeMode;
                }
                if (row.IsSpecificationNull())
                {
                    parameter[72].Value = DBNull.Value;
                }
                else
                {
                    parameter[72].Value = row.Specification;
                }
                if (row.IsVoucherInIDNull())
                {
                    parameter[73].Value = DBNull.Value;
                }
                else
                {
                    parameter[73].Value = row.VoucherInID;
                }
                if (row.IsVoucherOutIDNull())
                {
                    parameter[74].Value = DBNull.Value;
                }
                else
                {
                    parameter[74].Value = row.VoucherOutID;
                }
                if (row.IsVoucherRetrieveIDNull())
                {
                    parameter[75].Value = DBNull.Value;
                }
                else
                {
                    parameter[75].Value = row.VoucherRetrieveID;
                }
                if (row.IsIsPolyHookNull())
                {
                    parameter[76].Value = DBNull.Value;
                }
                else
                {
                    parameter[76].Value = row.IsPolyHook;
                }
                if (row.IsSourcePIDNull())
                {
                    parameter[77].Value = DBNull.Value;
                }
                else
                {
                    parameter[77].Value = row.SourcePID;
                }

                if (row.IsOnlyIDNull())
                {
                    parameter[78].Value = DBNull.Value;
                }
                else
                {
                    parameter[78].Value = row.OnlyID;
                }

                if (row.IsDiameterLabelPrintNull())
                {
                    parameter[80].Value = DBNull.Value;
                }
                else
                {
                    parameter[80].Value = row.DiameterLabelPrint;
                }
                if (row.IsLengthLabelPrintNull())
                {
                    parameter[79].Value = DBNull.Value;
                }
                else
                {
                    parameter[79].Value = row.LengthLabelPrint;
                }
                if (row.IsSlidesOfReamPrintNull())
                {
                    parameter[81].Value = DBNull.Value;
                }
                else
                {
                    parameter[81].Value = row.SlidesOfReamPrint;
                }
                if (row.IsSlidesOfSheetPrintNull())
                {
                    parameter[82].Value = DBNull.Value;
                }
                else
                {
                    parameter[82].Value = row.SlidesOfSheetPrint;
                }
                //2017-9-14
                if (row.IsFTransOutBillNoNull())
                {
                    parameter[83].Value = DBNull.Value;
                }
                else
                {
                    parameter[83].Value = row.FTransOutBillNo;
                }
                if (row.IsFTotalAmountNull())
                {
                    parameter[84].Value = 0;
                }
                else
                {
                    parameter[84].Value = row.FTotalAmount;
                }
                if (row.IsFTotalWeightNull())
                {
                    parameter[85].Value = 0;
                }
                else
                {
                    parameter[85].Value = row.FTotalWeight;
                }
                if (row.IsFIsInstockConfirmNull())
                {
                    parameter[86].Value = DBNull.Value;
                }
                else
                {
                    parameter[86].Value = row.FIsInstockConfirm;
                }
                #endregion
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_Product_In",
                        parameter);

                    retid = Convert.ToInt32(parameter[78].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "Insert_T_Product_In OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Insert_T_Product_In Error:" + ex.ToString()));
                }
            }
            return retid;
        }

        //插入数据时，把状态改成in = 1  out = 0
        public int Insert_T_Product_In_ForYK(WMSDS.T_Product_InRow row, string connStr)
        {
            int retid = -1;
            if (connStr == "")
                connStr = ConnctionString;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Insert_T_Product_In");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsProductIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.ProductID;
                }
                if (row.IsProductTypeCodeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductTypeCode;
                }
                if (row.IsBatchNONull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BatchNO;
                }
                if (row.IsFactoryNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Factory;
                }
                if (row.IsMachineIDNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.MachineID;
                }
                if (row.IsRollCountNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.RollCount;
                }
                if (row.IsMaterialCodeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.MaterialCode;
                }
                if (row.IsMaterialNameNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.MaterialName;
                }
                if (row.IsStandardNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Standard;
                }
                if (row.IsProductNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.ProductName;
                }
                if (row.IsProductTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.ProductType;
                }
                if (row.IsTrademarkNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Trademark;
                }
                if (row.IsGradeNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.Grade;
                }
                if (row.IsBasisweightLabelNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BasisweightLabel;
                }
                if (row.IsWidthLabelNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.WidthLabel;
                }
                if (row.IsDiameterLabelNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.DiameterLabel;
                }
                if (row.IsLengthLabelNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.LengthLabel;
                }
                if (row.IsWhiteDegreeNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.WhiteDegree;
                }
                if (row.IsColorNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.Color;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.WeightMode;
                }
                if (row.IsWeightLabelNull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.WeightLabel;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.CoreDiameter;
                }
                if (row.IsSpliceNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.Splice;
                }
                if (row.IsCustCodeNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.CustCode;
                }
                if (row.IsOrderNONull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.OrderNO;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.CustTrademark;
                }
                if (row.IsPaperCertCodeNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.PaperCertCode;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.SpecCustName;
                }
                if (row.IsDirectionNull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.Direction;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.IsWhiteFlag;
                }
                if (row.IsLayersNull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.Layers;
                }
                if (row.IsSKUNull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.SKU;
                }
                if (row.IsFinishNull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Finish;
                }
                if (row.IsPKGNull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.PKG;
                }
                if (row.IsMWeightNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.MWeight;
                }
                if (row.IsSheetWidthLabelNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.SheetWidthLabel;
                }
                if (row.IsSheetLengthLabelNull())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.SheetLengthLabel;
                }
                if (row.IsPalletReamsNull())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.PalletReams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[40].Value = DBNull.Value;
                }
                else
                {
                    parameter[40].Value = row.SlidesOfSheet;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[41].Value = DBNull.Value;
                }
                else
                {
                    parameter[41].Value = row.TransportType;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[42].Value = DBNull.Value;
                }
                else
                {
                    parameter[42].Value = row.ReamPackType;
                }
                if (row.IsFiberDirectNull())
                {
                    parameter[43].Value = DBNull.Value;
                }
                else
                {
                    parameter[43].Value = row.FiberDirect;
                }
                if (row.IsPalletHeightNull())
                {
                    parameter[44].Value = DBNull.Value;
                }
                else
                {
                    parameter[44].Value = row.PalletHeight;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[45].Value = DBNull.Value;
                }
                else
                {
                    parameter[45].Value = row.TrademarkStyle;
                }
                if (row.IsPalletRemarkNull())
                {
                    parameter[46].Value = DBNull.Value;
                }
                else
                {
                    parameter[46].Value = row.PalletRemark;
                }
                if (row.IsRemarkNull())
                {
                    parameter[47].Value = DBNull.Value;
                }
                else
                {
                    parameter[47].Value = row.Remark;
                }
                if (row.IsRollWrap_RemarkNull())
                {
                    parameter[48].Value = DBNull.Value;
                }
                else
                {
                    parameter[48].Value = row.RollWrap_Remark;
                }
                if (row.IsWHRemarkNull())
                {
                    parameter[49].Value = DBNull.Value;
                }
                else
                {
                    parameter[49].Value = row.WHRemark;
                }
                if (row.IsBusinessTypeNull())
                {
                    parameter[50].Value = DBNull.Value;
                }
                else
                {
                    parameter[50].Value = row.BusinessType;
                }
                if (row.IsSourceVoucherNull())
                {
                    parameter[51].Value = DBNull.Value;
                }
                else
                {
                    parameter[51].Value = row.SourceVoucher;
                }
                if (row.IsWarehouseNull())
                {
                    parameter[52].Value = DBNull.Value;
                }
                else
                {
                    parameter[52].Value = row.Warehouse;
                }
                if (row.IsWHPositionNull())
                {
                    parameter[53].Value = DBNull.Value;
                }
                else
                {
                    parameter[53].Value = row.WHPosition;
                }
                if (row.IsStatusInNull())
                {
                    parameter[54].Value = DBNull.Value;
                }
                else
                {
                    parameter[54].Value = "1";//入库状态
                }
                if (row.IsInDateNull())
                {
                    parameter[55].Value = DBNull.Value;
                }
                else
                {
                    parameter[55].Value = row.InDate;
                }
                if (row.IsInShiftNull())
                {
                    parameter[56].Value = DBNull.Value;
                }
                else
                {
                    parameter[56].Value = row.InShift;
                }
                if (row.IsInShiftTimeNull())
                {
                    parameter[57].Value = DBNull.Value;
                }
                else
                {
                    parameter[57].Value = row.InShiftTime;
                }
                if (row.IsInUserNull())
                {
                    parameter[58].Value = DBNull.Value;
                }
                else
                {
                    parameter[58].Value = row.InUser;
                }
                if (row.IsStatusOutNull())
                {
                    parameter[59].Value = DBNull.Value;
                }
                else
                {
                    parameter[59].Value = "0";//出库状态
                }
                if (row.IsOutDateNull())
                {
                    parameter[60].Value = DBNull.Value;
                }
                else
                {
                    parameter[60].Value = row.OutDate;
                }
                if (row.IsOutShiftNull())
                {
                    parameter[61].Value = DBNull.Value;
                }
                else
                {
                    parameter[61].Value = row.OutShift;
                }
                if (row.IsOutUserNull())
                {
                    parameter[62].Value = DBNull.Value;
                }
                else
                {
                    parameter[62].Value = row.OutUser;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[63].Value = DBNull.Value;
                }
                else
                {
                    parameter[63].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[64].Value = DBNull.Value;
                }
                else
                {
                    parameter[64].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[65].Value = DBNull.Value;
                }
                else
                {
                    parameter[65].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[66].Value = DBNull.Value;
                }
                else
                {
                    parameter[66].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[67].Value = DBNull.Value;
                }
                else
                {
                    parameter[67].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[68].Value = DBNull.Value;
                }
                else
                {
                    parameter[68].Value = row.Udefine3;
                }
                if (row.IsIsDeletedNull())
                {
                    parameter[69].Value = DBNull.Value;
                }
                else
                {
                    parameter[69].Value = row.IsDeleted;
                }
                if (row.IsReadDateNull())
                {
                    parameter[70].Value = DBNull.Value;
                }
                else
                {
                    parameter[70].Value = row.ReadDate;
                }
                if (row.IsTradeModeNull())
                {
                    parameter[71].Value = DBNull.Value;
                }
                else
                {
                    parameter[71].Value = row.TradeMode;
                }
                if (row.IsSpecificationNull())
                {
                    parameter[72].Value = DBNull.Value;
                }
                else
                {
                    parameter[72].Value = row.Specification;
                }
                if (row.IsVoucherInIDNull())
                {
                    parameter[73].Value = DBNull.Value;
                }
                else
                {
                    parameter[73].Value = row.VoucherInID;
                }
                if (row.IsVoucherOutIDNull())
                {
                    parameter[74].Value = DBNull.Value;
                }
                else
                {
                    parameter[74].Value = row.VoucherOutID;
                }
                if (row.IsVoucherRetrieveIDNull())
                {
                    parameter[75].Value = DBNull.Value;
                }
                else
                {
                    parameter[75].Value = row.VoucherRetrieveID;
                }
                if (row.IsIsPolyHookNull())
                {
                    parameter[76].Value = DBNull.Value;
                }
                else
                {
                    parameter[76].Value = row.IsPolyHook;
                }
                if (row.IsSourcePIDNull())
                {
                    parameter[77].Value = DBNull.Value;
                }
                else
                {
                    parameter[77].Value = row.SourcePID;
                }

                if (row.IsOnlyIDNull())
                {
                    parameter[78].Value = DBNull.Value;
                }
                else
                {
                    parameter[78].Value = row.OnlyID;
                }

                if (row.IsDiameterLabelPrintNull())
                {
                    parameter[80].Value = DBNull.Value;
                }
                else
                {
                    parameter[80].Value = row.DiameterLabelPrint;
                }
                if (row.IsLengthLabelPrintNull())
                {
                    parameter[79].Value = DBNull.Value;
                }
                else
                {
                    parameter[79].Value = row.LengthLabelPrint;
                }
                if (row.IsSlidesOfReamPrintNull())
                {
                    parameter[81].Value = DBNull.Value;
                }
                else
                {
                    parameter[81].Value = row.SlidesOfReamPrint;
                }
                if (row.IsSlidesOfSheetPrintNull())
                {
                    parameter[82].Value = DBNull.Value;
                }
                else
                {
                    parameter[82].Value = row.SlidesOfSheetPrint;
                }

                #endregion
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_Product_In",
                        parameter);

                    retid = Convert.ToInt32(parameter[78].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "Insert_T_Product_In OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Insert_T_Product_In Error:" + ex.ToString()));
                }
            }
            return retid;
        }

        //public int Insert_T_Product_Warehouse(WMSDS.T_Product_WarehouseRow row)
        //{
        //    int retid = -1;
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "SP_Insert_Product_Warehouse");

        //        #region 传参数
        //        if (row.IsProductIDNull())
        //        {
        //            parameter[0].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[0].Value = row.ProductID;
        //        }
        //        if (row.IsBarCodeNull())
        //        {
        //            parameter[1].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[1].Value = row.BarCode;
        //        }
        //        if (row.IsProductInIDNull())
        //        {
        //            parameter[2].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[2].Value = row.ProductInID;
        //        }
        //        if (row.IsFactoryIDNull())
        //        {
        //            parameter[3].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[3].Value = row.FactoryID;
        //        }
        //        if (row.IsProductTypeNull())
        //        {
        //            parameter[4].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[4].Value = row.ProductType;
        //        }
        //        if (row.IsBusinessTypeNull())
        //        {
        //            parameter[5].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[5].Value = row.BusinessType;
        //        }
        //        if (row.IsSourceVoucherNull())
        //        {
        //            parameter[6].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[6].Value = row.SourceVoucher;
        //        }
        //        if (row.IsWarehouseNull())
        //        {
        //            parameter[7].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[7].Value = row.Warehouse;
        //        }
        //        if (row.IsWHPositionNull())
        //        {
        //            parameter[8].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[8].Value = row.WHPosition;
        //        }
        //        if (row.IsInTimeNull())
        //        {
        //            parameter[9].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[9].Value = row.InTime;
        //        }
        //        if (row.IsInShiftNull())
        //        {
        //            parameter[10].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[10].Value = row.InShift;
        //        }
        //        if (row.IsInShiftTimeNull())
        //        {
        //            parameter[11].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[11].Value = row.InShiftTime;
        //        }
        //        if (row.IsInUserNull())
        //        {
        //            parameter[12].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[12].Value = row.InUser;
        //        }
        //        if (row.IsRemarkNull())
        //        {
        //            parameter[13].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[13].Value = row.Remark;
        //        }
        //        if (row.IsUDefineNull())
        //        {
        //            parameter[14].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[14].Value = row.UDefine;
        //        }
        //        if (row.IsCDefineNull())
        //        {
        //            parameter[15].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[15].Value = row.CDefine;
        //        }
        //        parameter[16].Value = row.OnlyID;
        //        #endregion

        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "SP_Insert_Product_Warehouse",
        //                parameter);

        //            retid = Convert.ToInt32(parameter[16].Value);

        //            OnSqlStateChange(new SqlStateEventArgs(true, "InsertT_Product_Warehouse OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "InsertT_Product_Warehouse Error:" + ex.ToString()));
        //        }
        //    }
        //    return retid;
        //}

        /// <summary>
        /// 删除入库产品信息
        /// </summary>
        /// <param name="tpiID"></param>
        /// <returns></returns>
        public bool Delete_T_Product_In(int tpiID)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    string sqlstr = "delete from T_Product_In where onlyid=@OnlyID;";

                    SqlParameter[] parameter = new SqlParameter[]
                    {
                       new SqlParameter("@OnlyID", tpiID),
                    };

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_Product_In OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_Product_In Error:" + ex.ToString()));
                }
                return ret;
            }
        }


        //public WMSDS Product_In_SumaryByShift(string inDate, string biller, string shift, string shiftTime, string productid)
        //{
        //    WMSDS sumDs = new WMSDS();
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "SP_Product_In_SumaryByShift");

        //        #region 传参数

        //        parameter[0].Value = Convert.ToDateTime(inDate);
        //        parameter[1].Value = biller;
        //        parameter[2].Value = shift;
        //        parameter[3].Value = shiftTime;
        //        parameter[4].Value = productid;

        //        #endregion

        //        try
        //        {
        //            MSSqlHelper.FillDataset(connection,
        //                CommandType.StoredProcedure,
        //                "SP_Product_In_SumaryByShift",
        //                sumDs,
        //                new string[] { "T_Product_In" },
        //                parameter
        //                );
        //            OnSqlStateChange(new SqlStateEventArgs(true, "InsertT_Product_Warehouse OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "InsertT_Product_Warehouse Error:" + ex.ToString()));
        //        }
        //    }
        //    return sumDs;
        //}
        /// <summary>
        /// 用来刷新入库后显示的同一属性的数量
        /// </summary>
        /// <param name="inDateS"></param>
        /// <param name="inDateE"></param>
        /// <param name="biller"></param>
        /// <param name="shift"></param>
        /// <param name="shiftTime"></param>
        /// <param name="MaterialCode"></param>
        /// <param name="MachineID"></param>
        /// <param name="Grade"></param>
        /// <param name="Width"></param>
        /// <param name="CoreDiameter"></param>
        /// <param name="IsWhitedFlag"></param>
        /// <param name="Layers"></param>
        /// <param name="RollCount"></param>
        /// <param name="SKU"></param>
        /// <param name="WeightMode"></param>
        /// <returns></returns>
        public DataSet Product_In_SumaryByFK(string inDateS, string inDateE, string biller, string shift, string shiftTime, string MaterialCode, string MachineID, string Grade, string Width, string CoreDiameter, string IsWhitedFlag, string Layers, string RollCount, string SKU, string WeightMode, string orderno, string whremark, string trademode)
        {
            DataSet sumDs = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Product_In_SumaryByFK");

                #region 传参数

                parameter[0].Value = inDateS;
                parameter[1].Value = inDateE;
                parameter[2].Value = biller;
                parameter[3].Value = shift;
                parameter[4].Value = shiftTime;
                parameter[5].Value = MaterialCode;
                parameter[6].Value = MachineID;
                parameter[7].Value = Grade;
                parameter[8].Value = Width;
                parameter[9].Value = CoreDiameter;
                parameter[10].Value = IsWhitedFlag;
                parameter[11].Value = Layers;
             
                parameter[12].Value = RollCount;
                
                parameter[13].Value = SKU;
            
                parameter[14].Value = WeightMode;
               
                parameter[15].Value = orderno;
                
                parameter[16].Value = whremark;
                parameter[17].Value = trademode;
                parameter[18].Direction = ParameterDirection.Output;
                parameter[18].Value = "";

                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Product_In_SumaryByFK",
                        sumDs,
                        new string[] { "T_Product_In" },
                        parameter
                        );
                    string ss = parameter[18].Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "Product_In_SumaryByFK OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Product_In_SumaryByFK Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }

        public DataSet Product_In_SumaryByBatchNO(string inDateS, string inDateE, string biller, string shift, string shiftTime,string batchNo,string status)
        {
            DataSet sumDs = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Product_In_SumaryByBatchNO");

                #region 传参数

                parameter[0].Value = inDateS;
                parameter[1].Value = inDateE;
                parameter[2].Value = biller;
                parameter[3].Value = shift;
                parameter[4].Value = shiftTime;
                parameter[5].Value = batchNo;
                parameter[6].Value = status;

                parameter[7].Direction = ParameterDirection.Output;
                parameter[7].Value = "";

                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Product_In_SumaryByBatchNO",
                        sumDs,
                        new string[] { "T_Product_In" },
                        parameter
                        );
                    string ss = parameter[7].Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "Product_In_SumaryByBatchNO OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Product_In_SumaryByBatchNO Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }

        public bool Delete_T_Product_Warehouse(string productid)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();

                    string sqlstr = " delete from T_Product_In where onlyid in(select productinid from T_Product_Warehouse where productid=@ProductID)    Delete from T_Product_Warehouse where productid =@ProductID;";

                    SqlParameter[] parameter = new SqlParameter[]
                    {
                       new SqlParameter("@ProductID", productid),
                    };

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_Product_Warehouse OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_Product_Warehouse Error:" + ex.ToString()));
                }
                return ret;
            }
        }



        /// <summary>
        /// 倒排序查询产品最后的生涯状态
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public DataSet Select_T_ProductLifeByProductID(string productid)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select pl.OnlyID as  OnlyID,pl.ProductOnlyID as ProductOnlyID, pl.Status as Status ,pl.Operate as Operate ,pl.OperUser as OperUser,pl.OperDate as OperDate,pi.VoucherInID as VoucherInID,pi.VoucherOutID as VoucherOutID ,pi.Cdefine2 as SourceOnlyID ,pi.SourcePID as SourcePID from T_ProductLife pl";
                sqlstr+=" left join t_product_in pi on  pi.onlyid =pl.productonlyid ";
               sqlstr+=" where pl.productid = @ProductID order by pl.onlyid desc ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = productid;
                par1.DbType = DbType.String;
                par1.ParameterName = "@ProductID";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_ProductLife" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_ProductLifeQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_ProductLifeQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        //public int Insert_T_ProductLife(WMSDS.T_ProductLifeRow row,string connString)
        //{
        //    WMSDS sumDs = new WMSDS();
        //    int retid = 0;

        //    using (SqlConnection connection = new SqlConnection(connString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Insert_T_ProductLife");

        //        #region 传参数
        //        //parameter[0].Value = row.OnlyID;
        //        if (row.IsProductOnlyIDNull())
        //        {
        //            parameter[0].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[0].Value = row.ProductOnlyID;
        //        }
        //        if (row.IsProductIDNull())
        //        {
        //            parameter[1].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[1].Value = row.ProductID;
        //        }
        //        if (row.IsStatusNull())
        //        {
        //            parameter[2].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[2].Value = row.Status;
        //        }
        //        if (row.IsOperateNull())
        //        {
        //            parameter[3].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[3].Value = row.Operate;
        //        }
        //        if (row.IsOperUserNull())
        //        {
        //            parameter[4].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[4].Value = row.OperUser;
        //        }
        //        if (row.IsOperDateNull())
        //        {
        //            parameter[5].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[5].Value = row.OperDate;
        //        }

        //        parameter[6].Value = row.OnlyId;
        //        #endregion

        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "Insert_T_ProductLife",
        //                parameter
        //                );
        //            retid = Convert.ToInt32(parameter[6].Value);
        //            OnSqlStateChange(new SqlStateEventArgs(true, "insert Insert_T_ProductLife OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, " insert Insert_T_ProductLife Error:" + ex.ToString()));
        //        }
        //    }
        //    return retid;
        //}

        public int Insert_T_ProductLife(WMSDS.T_ProductLifeRow row,string connStr)
        {
            WMSDS sumDs = new WMSDS();
            int retid = 0;
            if (connStr == "")
                connStr = ConnctionString;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Insert_T_ProductLife");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsProductOnlyIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.ProductOnlyID;
                }
                if (row.IsProductIDNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductID;
                }
                if (row.IsStatusNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.Status;
                }
                if (row.IsOperateNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Operate;
                }
                if (row.IsOperUserNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.OperUser;
                }
                if (row.IsOperDateNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.OperDate;
                }

                parameter[6].Value = row.OnlyId;
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_ProductLife",
                        parameter
                        );
                    retid = Convert.ToInt32(parameter[6].Value);
                    OnSqlStateChange(new SqlStateEventArgs(true, "insert Insert_T_ProductLife OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, " insert Insert_T_ProductLife Error:" + ex.ToString()));
                }
            }
            return retid;
        }

        public string Update_T_Product_In(WMSDS.T_Product_InRow row)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Update_T_Product_In");

                #region 传参数
                if (row.IsProductIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.ProductID;
                }
                if (row.IsProductTypeCodeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductTypeCode;
                }
                if (row.IsBatchNONull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BatchNO;
                }
                if (row.IsFactoryNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Factory;
                }
                if (row.IsMachineIDNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.MachineID;
                }
                if (row.IsRollCountNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.RollCount;
                }
                if (row.IsMaterialCodeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.MaterialCode;
                }
                if (row.IsMaterialNameNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.MaterialName;
                }
                if (row.IsStandardNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Standard;
                }
                if (row.IsProductNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.ProductName;
                }
                if (row.IsProductTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.ProductType;
                }
                if (row.IsTrademarkNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Trademark;
                }
                if (row.IsGradeNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.Grade;
                }
                if (row.IsBasisweightLabelNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BasisweightLabel;
                }
                if (row.IsWidthLabelNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.WidthLabel;
                }
                if (row.IsDiameterLabelNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.DiameterLabel;
                }
                if (row.IsLengthLabelNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.LengthLabel;
                }
                if (row.IsWhiteDegreeNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.WhiteDegree;
                }
                if (row.IsColorNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.Color;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.WeightMode;
                }
                if (row.IsWeightLabelNull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.WeightLabel;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.CoreDiameter;
                }
                if (row.IsSpliceNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.Splice;
                }
                if (row.IsCustCodeNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.CustCode;
                }
                if (row.IsOrderNONull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.OrderNO;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.CustTrademark;
                }
                if (row.IsPaperCertCodeNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.PaperCertCode;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.SpecCustName;
                }
                if (row.IsDirectionNull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.Direction;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.IsWhiteFlag;
                }
                if (row.IsLayersNull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.Layers;
                }
                if (row.IsSKUNull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.SKU;
                }
                if (row.IsFinishNull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Finish;
                }
                if (row.IsPKGNull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.PKG;
                }
                if (row.IsMWeightNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.MWeight;
                }
                if (row.IsSheetWidthLabelNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.SheetWidthLabel;
                }
                if (row.IsSheetLengthLabelNull())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.SheetLengthLabel;
                }
                if (row.IsPalletReamsNull())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.PalletReams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[40].Value = DBNull.Value;
                }
                else
                {
                    parameter[40].Value = row.SlidesOfSheet;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[41].Value = DBNull.Value;
                }
                else
                {
                    parameter[41].Value = row.TransportType;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[42].Value = DBNull.Value;
                }
                else
                {
                    parameter[42].Value = row.ReamPackType;
                }
                if (row.IsFiberDirectNull())
                {
                    parameter[43].Value = DBNull.Value;
                }
                else
                {
                    parameter[43].Value = row.FiberDirect;
                }
                if (row.IsPalletHeightNull())
                {
                    parameter[44].Value = DBNull.Value;
                }
                else
                {
                    parameter[44].Value = row.PalletHeight;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[45].Value = DBNull.Value;
                }
                else
                {
                    parameter[45].Value = row.TrademarkStyle;
                }
                if (row.IsPalletRemarkNull())
                {
                    parameter[46].Value = DBNull.Value;
                }
                else
                {
                    parameter[46].Value = row.PalletRemark;
                }
                if (row.IsRemarkNull())
                {
                    parameter[47].Value = DBNull.Value;
                }
                else
                {
                    parameter[47].Value = row.Remark;
                }
                if (row.IsRollWrap_RemarkNull())
                {
                    parameter[48].Value = DBNull.Value;
                }
                else
                {
                    parameter[48].Value = row.RollWrap_Remark;
                }
                if (row.IsWHRemarkNull())
                {
                    parameter[49].Value = DBNull.Value;
                }
                else
                {
                    parameter[49].Value = row.WHRemark;
                }
                if (row.IsBusinessTypeNull())
                {
                    parameter[50].Value = DBNull.Value;
                }
                else
                {
                    parameter[50].Value = row.BusinessType;
                }
                if (row.IsSourceVoucherNull())
                {
                    parameter[51].Value = DBNull.Value;
                }
                else
                {
                    parameter[51].Value = row.SourceVoucher;
                }
                if (row.IsWarehouseNull())
                {
                    parameter[52].Value = DBNull.Value;
                }
                else
                {
                    parameter[52].Value = row.Warehouse;
                }
                if (row.IsWHPositionNull())
                {
                    parameter[53].Value = DBNull.Value;
                }
                else
                {
                    parameter[53].Value = row.WHPosition;
                }
                if (row.IsStatusInNull())
                {
                    parameter[54].Value = DBNull.Value;
                }
                else
                {
                    parameter[54].Value = row.StatusIn;
                }
                if (row.IsInDateNull())
                {
                    parameter[55].Value = DBNull.Value;
                }
                else
                {
                    parameter[55].Value = row.InDate;
                }
                if (row.IsInShiftNull())
                {
                    parameter[56].Value = DBNull.Value;
                }
                else
                {
                    parameter[56].Value = row.InShift;
                }
                if (row.IsInShiftTimeNull())
                {
                    parameter[57].Value = DBNull.Value;
                }
                else
                {
                    parameter[57].Value = row.InShiftTime;
                }
                if (row.IsInUserNull())
                {
                    parameter[58].Value = DBNull.Value;
                }
                else
                {
                    parameter[58].Value = row.InUser;
                }
                if (row.IsStatusOutNull())
                {
                    parameter[59].Value = DBNull.Value;
                }
                else
                {
                    parameter[59].Value = row.StatusOut;
                }
                if (row.IsOutDateNull())
                {
                    parameter[60].Value = DBNull.Value;
                }
                else
                {
                    parameter[60].Value = row.OutDate;
                }
                if (row.IsOutShiftNull())
                {
                    parameter[61].Value = DBNull.Value;
                }
                else
                {
                    parameter[61].Value = row.OutShift;
                }
                if (row.IsOutUserNull())
                {
                    parameter[62].Value = DBNull.Value;
                }
                else
                {
                    parameter[62].Value = row.OutUser;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[63].Value = DBNull.Value;
                }
                else
                {
                    parameter[63].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[64].Value = DBNull.Value;
                }
                else
                {
                    parameter[64].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[65].Value = DBNull.Value;
                }
                else
                {
                    parameter[65].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[66].Value = DBNull.Value;
                }
                else
                {
                    parameter[66].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[67].Value = DBNull.Value;
                }
                else
                {
                    parameter[67].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[68].Value = DBNull.Value;
                }
                else
                {
                    parameter[68].Value = row.Udefine3;
                }
                if (row.IsIsDeletedNull())
                {
                    parameter[69].Value = DBNull.Value;
                }
                else
                {
                    parameter[69].Value = row.IsDeleted;
                }
                if (row.IsReadDateNull())
                {
                    parameter[70].Value = DBNull.Value;
                }
                else
                {
                    parameter[70].Value = row.ReadDate;
                }
                if (row.IsTradeModeNull())
                {
                    parameter[71].Value = DBNull.Value;
                }
                else
                {
                    parameter[71].Value = row.TradeMode;
                }
                if (row.IsSpecificationNull())
                {
                    parameter[72].Value = DBNull.Value;
                }
                else
                {
                    parameter[72].Value = row.Specification;
                }
                if (row.IsVoucherInIDNull())
                {
                    parameter[73].Value = DBNull.Value;
                }
                else
                {
                    parameter[73].Value = row.VoucherInID;
                }
                if (row.IsVoucherOutIDNull())
                {
                    parameter[74].Value = DBNull.Value;
                }
                else
                {
                    parameter[74].Value = row.VoucherOutID;
                }
                if (row.IsVoucherRetrieveIDNull())
                {
                    parameter[75].Value = DBNull.Value;
                }
                else
                {
                    parameter[75].Value = row.VoucherRetrieveID;
                }
                if (row.IsIsPolyHookNull())
                {
                    parameter[76].Value = DBNull.Value;
                }
                else
                {
                    parameter[76].Value = row.IsPolyHook;
                }
                if (row.IsSourcePIDNull())
                {
                    parameter[77].Value = DBNull.Value;
                }
                else
                {
                    parameter[77].Value = row.SourcePID;
                }
                parameter[78].Value = row.OnlyID;

                if (row.IsLengthLabelPrintNull())
                {
                    parameter[79].Value = DBNull.Value;
                }
                else
                {
                    parameter[79].Value = row.LengthLabelPrint;
                }
                if (row.IsDiameterLabelPrintNull())
                {
                    parameter[80].Value = DBNull.Value;
                }
                else
                {
                    parameter[80].Value = row.DiameterLabelPrint;
                }
                if (row.IsSlidesOfReamPrintNull())
                {
                    parameter[81].Value = DBNull.Value;
                }
                else
                {
                    parameter[81].Value = row.SlidesOfReamPrint;
                }
                if (row.IsSlidesOfSheetPrintNull())
                {
                    parameter[82].Value = DBNull.Value;
                }
                else
                {
                    parameter[82].Value = row.SlidesOfSheetPrint;
                }
                #endregion
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Update_T_Product_In",
                        parameter);

                    //retid = Convert.ToInt32(parameter[72].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_T_Product_In OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_T_Product_In Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }
        /// <summary>
        /// 通过事务来出库
        /// </summary>
        /// <param name="tpiRow">入库row</param>
        /// <param name="tpiRow">出库row</param>
        /// <param name="tpfRow">生涯row</param>
        /// <param name="ospprow">出库分录和产品关联表</param>
        /// <returns></returns>
        public string Tran_ProductScanOut(WMSDS.T_Product_InRow tpiRow, WMSDS.T_Product_InRow outRow, WMSDS.T_ProductLifeRow tpfRow, WMSDS.T_OutStock_ProductRow osppRow, WMSDS.T_OutStock_EntryRow ospeRow, WMSDS.T_OutStock_Plan_EntryRow ospRow)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    int outid = this.Insert_T_Product_In(outRow,"");
                    if(outid<0||outid==0)
                        return "Tran_ProductScanOut出库产品插入失败" ;
                    string error = this.Update_T_Product_In(tpiRow);
                    if (error != "")
                        return "Tran_ProductScanOut产品更新失败" + error;
                    tpfRow.ProductOnlyID = outid;
                    int lifeID = this.Insert_T_ProductLife(tpfRow, "");
                    if (lifeID < 0||lifeID==0)
                        return "Tran_ProductScanOut生涯保存失败";
                    osppRow.ProductOnlyID = outid;
                    int outID = this.Insert_T_OutStock_Product(osppRow);
                    if (outID < 0||outID==0)
                        return "Tran_ProductScanOut出库分录关联保存失败";
                    
                    string ospeerr = this.Update_T_OutStock_Entry(ospeRow);
                    if (ospeerr != "")
                        return "Tran_ProductScanOut出库分录数量保存失败";
                    if (ospRow.VoucherID != 0)
                    {
                        string osperr = this.Update_T_OutStock_Plan_Entry(ospRow);
                        if (osperr != "")
                            return "Tran_ProductScanOut通知分录数量保存失败";
                    }
                    ts.Complete();

                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        private string Update_T_OutStock_Entry(WMSDS.T_OutStock_EntryRow row)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_OutStock_Entry]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsOnlyIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.OnlyID;
                }
                if (row.IsProductTypeIDNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductTypeID;
                }
                if (row.IsBrandNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.Brand;
                }
                if (row.IsTypeNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Type;
                }
                if (row.IsGradeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.Grade;
                }
                if (row.IsBasisweightNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.Basisweight;
                }
                if (row.IsSpecificationNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.Specification;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.CoreDiameter;
                }
                if (row.IsWidth_RNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Width_R;
                }
                if (row.IsWidth_PNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.Width_P;
                }
                if (row.IsLength_PNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.Length_P;
                }
                if (row.IsReamsNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Reams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.SlidesOfSheet;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.ReamPackType;
                }
                if (row.IsRemarkNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.Remark;
                }
                if (row.IsMaterialCodeNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.MaterialCode;
                }
                if (row.IsSKUNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.SKU;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.TrademarkStyle;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.IsWhiteFlag;
                }
                if (row.IsOrderNONull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.OrderNO;
                }
                if (row.IsPriceNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.Price;
                }
                if (row.IsPaperCertNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.PaperCert;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.SpecCustName;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.CustTrademark;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.WeightMode;
                }
                if (row.IsPlanQtyNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.PlanQty;
                }
                if (row.IsPlanAuxQty1Null())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.PlanAuxQty1;
                }
                if (row.IsPlanAuxQty2Null())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.PlanAuxQty2;
                }
                if (row.IsPlanCommitQtyNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.PlanCommitQty;
                }
                if (row.IsPlanCommitAuxQty1Null())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.PlanCommitAuxQty1;
                }
                if (row.IsPlanCommitAuxQty2Null())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.PlanCommitAuxQty2;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.Udefine3;
                }
                if (row.IsMaterialNameNull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.MaterialName;
                }
                if (row.IsBatchNONull())
                {
                    parameter[40].Value = DBNull.Value;
                }
                else
                {
                    parameter[40].Value = row.BatchNO;
                } if (row.IsSourceEntryIDNull())
                {
                    parameter[41].Value = DBNull.Value;
                }
                else
                {
                    parameter[41].Value = row.SourceEntryID;
                }
                if (row.IsSourceVoucherIDNull())
                {
                    parameter[42].Value = DBNull.Value;
                }
                else
                {
                    parameter[42].Value = row.SourceVoucherID;
                } if (row.IsRemainQtyNull())
                {
                    parameter[43].Value = DBNull.Value;
                }
                else
                {
                    parameter[43].Value = row.RemainQty;
                } if (row.IsRemainAuxQtyNull())
                {
                    parameter[44].Value = DBNull.Value;
                }
                else
                {
                    parameter[44].Value = row.RemainAuxQty;
                }


                parameter[45].Value = row.EntryID;

                //zfz
                //if (row.IsEntryIDNull())
                //{
                //    parameter[45].Value = DBNull.Value;
                //}
                //else
                //{
                //    parameter[45].Value = row.EntryID;
                //}



                parameter[46].Value = row.VoucherID;
                
                //zfz
                //if (row.IsVoucherIDNull())
                //{
                //    parameter[46].Value = DBNull.Value;
                //}
                //else
                //{
                //    parameter[46].Value = row.VoucherID;
                //}
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Update_T_OutStock_Entry",
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_T_OutStock_Entry OK"));
                    return ret = "";

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_T_OutStock_Entry Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }

        public string Update_T_OutStock(WMSDS.T_OutStockRow row)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_OutStock]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsOnlyIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.OnlyID;
                }
                if (row.IsSourceVoucherNONull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.SourceVoucherNO;
                }
                if (row.IsBusinessTypeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BusinessType;
                }
                if (row.IsSourceVoucherTypeNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.SourceVoucherType;
                }
                if (row.IsBillTypeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.BillType;
                }
                if (row.IsFactoryIDNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.FactoryID;
                }
                if (row.IsWHCodeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.WHCode;
                }
                if (row.IsWHToCodeNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.WHToCode;
                }
                if (row.IsTradeTypeNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.TradeType;
                }
                if (row.IsDeptNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.DeptName;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.TransportType;
                }
                if (row.IsKeeperNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Keeper;
                }
                if (row.IsBillerNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.Biller;
                }
                if (row.IsBillDeptNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BillDept;
                }
                if (row.IsBillDateNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.BillDate;
                }
                if (row.IsBillRemarkNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.BillRemark;
                }
                if (row.IsCheckerNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.Checker;
                }
                if (row.IsCheckDateNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.CheckDate;
                }
                if (row.IsForklifterNONull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.ForklifterNO;
                }
                if (row.IsCarrierNONull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.CarrierNO;
                }
                if (row.IsContainerNONull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.ContainerNO;
                }
                if (row.IsVehicleNONull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.VehicleNO;
                }
                if (row.IsSealNONull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.SealNO;
                }
                if (row.IsPortNONull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.PortNO;
                }
                if (row.IsShipNONull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.ShipNO;
                }
                if (row.IsPickNONull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.PickNO;
                }
               
                if (row.IsOrderNONull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.OrderNO;
                }
                if (row.IsIsCheckNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.IsCheck;
                }
                if (row.IsIsCancelNull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.IsCancel;
                }
                if (row.IsIsCloseNull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.IsClose;
                }
                if (row.IsCustomerNameNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.CustomerName;
                }
              
                if (row.IsQtyNull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.Qty;
                }
                if (row.IsAuxQtyNull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.AuxQty;
                }
               
               
                if (row.IsFinisherNull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Finisher;
                }
                if (row.IsFinishDateNull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.FinishDate;
                }
                if (row.IsOrgToCodeNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.OrgToCode;
                }
                if (row.IsIsUploadNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.IsUpload;
                } if (row.IsUploadDateNull())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.UploadDate;
                } if (row.IsVoucherNONull())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.VoucherNO;
                }
                #endregion



                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Update_T_OutStock",
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_T_OutStock OK"));
                    return ret = "";

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_T_OutStock Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }

        public string Update_T_OutStock(WMSDS.T_OutStockRow row,string conStr)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_OutStock]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsOnlyIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.OnlyID;
                }
                if (row.IsSourceVoucherNONull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.SourceVoucherNO;
                }
                if (row.IsBusinessTypeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BusinessType;
                }
                if (row.IsSourceVoucherTypeNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.SourceVoucherType;
                }
                if (row.IsBillTypeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.BillType;
                }
                if (row.IsFactoryIDNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.FactoryID;
                }
                if (row.IsWHCodeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.WHCode;
                }
                if (row.IsWHToCodeNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.WHToCode;
                }
                if (row.IsTradeTypeNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.TradeType;
                }
                if (row.IsDeptNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.DeptName;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.TransportType;
                }
                if (row.IsKeeperNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Keeper;
                }
                if (row.IsBillerNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.Biller;
                }
                if (row.IsBillDeptNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BillDept;
                }
                if (row.IsBillDateNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.BillDate;
                }
                if (row.IsBillRemarkNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.BillRemark;
                }
                if (row.IsCheckerNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.Checker;
                }
                if (row.IsCheckDateNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.CheckDate;
                }
                if (row.IsForklifterNONull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.ForklifterNO;
                }
                if (row.IsCarrierNONull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.CarrierNO;
                }
                if (row.IsContainerNONull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.ContainerNO;
                }
                if (row.IsVehicleNONull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.VehicleNO;
                }
                if (row.IsSealNONull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.SealNO;
                }
                if (row.IsPortNONull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.PortNO;
                }
                if (row.IsShipNONull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.ShipNO;
                }
                if (row.IsPickNONull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.PickNO;
                }

                if (row.IsOrderNONull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.OrderNO;
                }
                if (row.IsIsCheckNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.IsCheck;
                }
                if (row.IsIsCancelNull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.IsCancel;
                }
                if (row.IsIsCloseNull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.IsClose;
                }
                if (row.IsCustomerNameNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.CustomerName;
                }

                if (row.IsQtyNull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.Qty;
                }
                if (row.IsAuxQtyNull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.AuxQty;
                }


                if (row.IsFinisherNull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Finisher;
                }
                if (row.IsFinishDateNull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.FinishDate;
                }
                if (row.IsOrgToCodeNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.OrgToCode;
                }
                if (row.IsIsUploadNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.IsUpload;
                } if (row.IsUploadDateNull())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.UploadDate;
                } if (row.IsVoucherNONull())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.VoucherNO;
                }
                #endregion



                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Update_T_OutStock",
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_T_OutStock OK"));
                    return ret = "";

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_T_OutStock Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }

        private int Insert_T_OutStock_Product(WMSDS.T_OutStock_ProductRow row)
        {
            WMSDS sumDs = new WMSDS();
            int retid = 0;

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Insert_T_OutStock_Product");


                #region 传参数

                parameter[0].Value = row.VoucherID;
                parameter[1].Value = row.EntryID;


                parameter[2].Value = row.ProductOnlyID;

                parameter[3].Value = row.ProductID;

                if (row.IsScanTimeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.ScanTime;
                }
                parameter[5].Value = row.OnlyID;
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_OutStock_Product",
                        parameter
                        );
                    retid = Convert.ToInt32(parameter[5].Value);
                    OnSqlStateChange(new SqlStateEventArgs(true, " Insert_T_OutStock_Product OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "  Insert_T_OutStock_Product Error:" + ex.ToString()));
                }
            }
            return retid;
        }

        private string Update_T_OutStock_Plan_Entry(WMSDS.T_OutStock_Plan_EntryRow row)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_OutStock_Plan_Entry]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsOnlyIDNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.OnlyID;
                }
                if (row.IsProductTypeIDNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductTypeID;
                }
                if (row.IsBrandNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.Brand;
                }
                if (row.IsTypeNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Type;
                }
                if (row.IsGradeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.Grade;
                }
                if (row.IsBasisweightNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.Basisweight;
                }
                if (row.IsSpecificationNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.Specification;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.CoreDiameter;
                }
                if (row.IsWidth_RNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Width_R;
                }
                if (row.IsWidth_PNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.Width_P;
                }
                if (row.IsLength_PNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.Length_P;
                }
                if (row.IsReamsNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Reams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.SlidesOfSheet;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.ReamPackType;
                }
                if (row.IsRemarkNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.Remark;
                }
                if (row.IsMaterialCodeNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.MaterialCode;
                }
                if (row.IsSKUNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.SKU;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.TrademarkStyle;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.IsWhiteFlag;
                }
                if (row.IsOrderNONull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.OrderNO;
                }
                if (row.IsPriceNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.Price;
                }
                if (row.IsPaperCertNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.PaperCert;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.SpecCustName;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.CustTrademark;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.WeightMode;
                }
                if (row.IsPlanQtyNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.PlanQty;
                }
                if (row.IsPlanAuxQty1Null())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.PlanAuxQty1;
                }
                if (row.IsPlanAuxQty2Null())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.PlanAuxQty2;
                }
                if (row.IsPlanCommitQtyNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.PlanCommitQty;
                }
                if (row.IsPlanCommitAuxQty1Null())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.PlanCommitAuxQty1;
                }
                if (row.IsPlanCommitAuxQty2Null())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.PlanCommitAuxQty2;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.Udefine3;
                }
                if (row.IsMaterialNameNull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.MaterialName;
                }
                if (row.IsBatchNONull())
                {
                    parameter[40].Value = DBNull.Value;
                }
                else
                {
                    parameter[40].Value = row.BatchNO;
                } if (row.IsSourceEntryIDNull())
                {
                    parameter[41].Value = DBNull.Value;
                }
                else
                {
                    parameter[41].Value = row.SourceEntryID;
                }
                if (row.IsEntryIDNull())
                {
                    parameter[42].Value = DBNull.Value;
                }
                else
                {
                    parameter[42].Value = row.EntryID;
                } if (row.IsVoucherIDNull())
                {
                    parameter[43].Value = DBNull.Value;
                }
                else
                {
                    parameter[43].Value = row.VoucherID;
                }
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Update_T_OutStock_Plan_Entry",
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_T_OutStock_Plan_Entry OK"));
                    return ret = "";

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_T_OutStock_Plan_Entry Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }
        ///// <summary>
        ///// 插入出库分录与条码的关联
        ///// </summary>
        ///// <param name="row"></param>
        ///// <returns></returns>
        //private int Insert_T_OutStock_Plan_Product(WMSDS.T_OutStock_Plan_ProductRow row)
        //{
        //    WMSDS sumDs = new WMSDS();
        //    int retid = 0;

        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "SP_Insert_OutStock_Plan_Product");


        //        #region 传参数

        //        parameter[0].Value = row.VoucherID;
        //        parameter[1].Value = row.EntryID;


        //        parameter[2].Value = row.ProductOnlyID;

        //        parameter[3].Value = row.ProductID;

        //        if (row.IsScanTimeNull())
        //        {
        //            parameter[4].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[4].Value = row.ScanTime;
        //        }
        //        parameter[5].Value = row.OnlyID;
        //        #endregion

        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "SP_Insert_OutStock_Plan_Product",
        //                parameter
        //                );
        //            retid = Convert.ToInt32(parameter[5].Value);
        //            OnSqlStateChange(new SqlStateEventArgs(true, " SP_Insert_OutStock_Plan_Product OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "  SP_Insert_OutStock_Plan_Product Error:" + ex.ToString()));
        //        }
        //    }
        //    return retid;
        //}
        /// <summary>
        /// 通过事务来同时更新入库表和生涯表和instock_product表
        /// </summary>
        /// <param name="tpiRow">入库row</param>
        /// <param name="tpfRow">生涯row</param>
        /// <returns></returns>
        public string Tran_Update_ProductScanInForCancel(WMSDS.T_Product_InRow tpiRow, WMSDS.T_ProductLifeRow tpfRow)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                  string error=  this.Update_T_Product_In(tpiRow);
                  if (error !="")
                      return "Tran_Update_ProductScanIn产品更新失败" + error;
                   int lifeID= this.Insert_T_ProductLife(tpfRow,"");
                     if (lifeID < 0)
                         return "Tran_Update_ProductScanIn生涯保存失败";
                    ts.Complete();
                   
                        return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        //private void Delete_T_InStock_Product()
        //{
        //    bool ret = false;
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            int voucherid = osppRow.VoucherID;
        //            int planid = osppRow.EntryID;
        //            int productonlyid = osppRow.ProductOnlyID;
        //            string sqlstr = "delete from T_InStock_Product where VoucherID =@VoucherID  and EntryID=@EntryID and ProductOnlyID=@ProductOnlyID ;";

        //            SqlParameter[] parameter = new SqlParameter[]
        //            {
        //               new SqlParameter("@VoucherID", voucherid),
        //                new SqlParameter("@EntryID", planid), new SqlParameter("@ProductOnlyID", productonlyid),
        //            };

        //            MSSqlHelper.ExecuteNonQuery(connection,//tran,
        //                   CommandType.Text,
        //                  sqlstr,
        //                  parameter);
        //            ret = true;
        //            OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_OutStock_Product OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_OutStock_Product Error:" + ex.ToString()));
        //        }
        //        return ret;
        //    }
        //}
        /// <summary>
        /// 通过事务来同时插入入库表和生涯表
        /// </summary>
        /// <param name="tpiRow">入库row</param>
        /// <param name="tpfRow">生涯row</param>
        /// <returns></returns>
        public string Tran_InsertProductScanIn(WMSDS.T_Product_InRow tpirow, WMSDS.T_ProductLifeRow tplrow)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    int id = this.Insert_T_Product_In(tpirow,"");
                    if (id < 0)
                        return "Tran_ProductScanIn产品插入失败";

                    tplrow.ProductOnlyID = id;
                    int lifeID = this.Insert_T_ProductLife(tplrow, "");
                    if (lifeID < 0)
                        return "Tran_ProductScanIn保存失败";
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }
        /// <summary>
        /// 通过事务来同时插入入库表和生涯表，同时更新源ID的状态
        /// </summary>
        /// <param name="tpiRow">入库row</param>
        /// <param name="tpfRow">生涯row</param>
        /// <returns></returns>
        public string Tran_InsertUpdateProductScanInForRedIn(WMSDS.T_Product_InRow tpirow, WMSDS.T_ProductLifeRow tplrow, WMSDS.T_Product_InRow sourcetpirow)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                  string ret=  this.Update_T_Product_In(sourcetpirow);
                    if(ret!="")
                        return "Tran_InsertUpdateProductScanInForRedIn源产品更新状态失败"+ret;
                    int id = this.Insert_T_Product_In(tpirow,"");
                    if (id < 0)
                        return "Tran_InsertUpdateProductScanInForRedIn产品插入失败";
                    tplrow.ProductOnlyID = id;
                    int lifeID = this.Insert_T_ProductLife(tplrow, "");
                    if (lifeID < 0)
                        return "Tran_InsertUpdateProductScanInForRedIn保存失败";
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        /// <summary>
        /// 通过事务来同时插入入库表和生涯表，同时更新源ID的状态
        /// </summary>
        /// <param name="tpiRow">入库row</param>
        /// <param name="tpfRow">生涯row</param>
        /// <returns></returns>
        public string Tran_SetProductCancelRedInCaseRedIn(WMSDS.T_Product_InRow tpirow, WMSDS.T_ProductLifeRow tplrow, WMSDS.T_Product_InRow sourcetpirow)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    string ret = this.Update_T_Product_In(sourcetpirow);
                    if (ret != "")
                        return "Tran_SetProductCancelRedInCaseRedIn源产品更新状态失败" + ret;
                    string id = this.Update_T_Product_In(tpirow);
                    if (id!="")
                        return "Tran_SetProductCancelRedInCaseRedIn产品更新失败";
                    tplrow.ProductOnlyID = tpirow.OnlyID;
                    int lifeID = this.Insert_T_ProductLife(tplrow, "");
                    if (lifeID < 0)
                        return "Tran_SetProductCancelRedInCaseRedIn保存失败";
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }
        #endregion
        /// <summary>
        /// 通过用户类型查询用户
        /// </summary>
        /// <param name="usertype"></param>
        /// <returns></returns>
        public WMSDS Select_T_UserByType(string usertype)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select UserCode,UserName ,password from T_user ";
                if (usertype != "")
                    sqlstr += " where usertypeid = @UserType ";
                sqlstr += " order by usercode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = usertype;
                par1.DbType = DbType.String;
                par1.ParameterName = "@UserType";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_UserQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_UserQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public WMSDS Select_T_User(string usercode)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select UserCode,UserName ,password from T_user ";
                if(usercode!="")
                    sqlstr+=" where usercode = @UserCode ";
                sqlstr+=" order by usercode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = usercode;
                par1.DbType = DbType.String;
                par1.ParameterName = "@UserCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_UserQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_UserQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        public WMSDS Select_T_UserForList(string usercode)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select tu.UserCode,tu.UserName ,tu.password,ts.ShiftName ,tu.ShiftID ,tut.UserTypeName ,tu.UserTypeID from T_user tu ";
              sqlstr +="  left join t_shift ts on ts.ShiftCode = tu.ShiftID ";
              sqlstr +="  left join t_User_Type tut on tut.UserTypeCode = tu.UserTypeID ";
              sqlstr += " where tu.ischoose ='1' ";
                
                if (usercode != "")
                    sqlstr += " and tu.usercode = @UserCode ";
                sqlstr += " order by tu.usercode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = usercode;
                par1.DbType = DbType.String;
                par1.ParameterName = "@UserCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
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
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_UserQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_UserQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 查询工厂组织
        /// </summary>
        /// <param name="choose">是否可用</param>
        /// <param name="local">是否本地</param>
        /// <returns></returns>
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
        /// <summary>
        /// 查询工厂组织
        /// </summary>
        /// <param name="choose">是否可用</param>
        /// <param name="machineID">机台号</param>
        /// <returns></returns>
        public WMSDS Select_T_Factory(bool choose, string machineID)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "[Select_T_FactoryByID]";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@IsChoose", SqlDbType.Bit),
                  new SqlParameter("@IsLocal", SqlDbType.VarChar,20)
                };
                parameter[0].Value = choose;
                parameter[1].Value = machineID;
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
        /// <summary>
        /// 查询工厂组织
        /// </summary>
        /// <param name="choose">是否可用</param>
        /// <param name="machineID">组织代码</param>
        /// <returns></returns>
        public WMSDS Select_T_FactoryByCode(bool choose, string code)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "[Select_T_FactoryByCode]";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@IsChoose", SqlDbType.Bit),
                  new SqlParameter("@IsLocal", SqlDbType.VarChar,20)
                };
                parameter[0].Value = choose;
                parameter[1].Value = code;
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

        /// <summary>
        /// 查询仓库信息
        /// </summary>
        /// <param name="choose">是否可用</param>
        /// <param name="machineID">仓库代码</param>
        /// <returns></returns>
        public WMSDS Select_T_Factory_WarehouseByCode(bool choose, string code)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "[Select_T_Factory_WarehouseByCode]";

                SqlParameter[] parameter = new SqlParameter[]
                {    
                  new SqlParameter("@IsChoose", SqlDbType.Bit),
                  new SqlParameter("@IsLocal", SqlDbType.VarChar,20)
                };
                parameter[0].Value = choose;
                parameter[1].Value = code;
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          sqlstr,
                          ds,
                          new string[] { "T_Factory_Warehouse" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_Factory_Warehouse OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_Factory_Warehouse Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        /// <summary>
        /// 查询工厂组织
        /// </summary>
        /// <param name="factory">组织名称，为空时全部查询</param>
        /// <returns></returns>
        public WMSDS Select_T_Factory(string factory)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select OnlyID,FactoryName,MachineID,FactoryAbbr,FactoryAddr,FactoryPhone,ct_orginfo.OrgName,IsChoose,IsLocal  from T_Factory left join  ct_orginfo on ct_orginfo.orgcode =t_factory.factoryabbr";
                if (factory != "")
                    sqlstr += " where factory = @FactoryName ";
                sqlstr += " order by MachineID  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = factory;
                par1.DbType = DbType.String;
                par1.ParameterName = "@FactoryName";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Factory" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_FactoryQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_FactoryQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public WMSDS Select_T_Product_InForDetail(string factory, string user, string dateS, string dateE,
           string confirmDateS, string confirmDateE,bool isConfirm, string pType)
        {
            WMSDS sumDs = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Select_T_Product_InForDetail");

                #region 传参数
                if (DateTime.Parse(dateS) == DateTime.MinValue)
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = dateS;
                }
                if (DateTime.Parse(dateE) == DateTime.MinValue)
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = dateE;
                }
                if (DateTime.Parse(confirmDateS) == DateTime.MinValue)
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = confirmDateS;
                }
                if (DateTime.Parse(confirmDateE) == DateTime.MinValue)
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = confirmDateS;
                }
                if (user == "")
                    parameter[4].Value = DBNull.Value;
                else
                    parameter[4].Value = user;

                if (factory == "")
                    parameter[5].Value = DBNull.Value;
                else
                    parameter[5].Value = factory;

                if (pType == "")
                    parameter[6].Value = DBNull.Value;
                else
                    parameter[6].Value = pType;

                    parameter[7].Value = isConfirm?"Y":"N";

                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_Product_InForDetail",
                        sumDs,
                        new string[] { "T_Product_In" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InForDetail OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InForDetail Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }

        public WMSQueryDS Select_T_Product_InForStat(string factory, string user, string dateS, string dateE, string pType)
        {
            WMSQueryDS sumDs = new WMSQueryDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Select_T_Product_InForStat");

                #region 传参数
                parameter[0].Value = dateS;
                parameter[1].Value = dateE;
                if(user=="")
                    parameter[2].Value = DBNull.Value;

                else
                parameter[2].Value = user;
                if (factory == "")
                    parameter[3].Value = DBNull.Value;

                else
                parameter[3].Value = factory;
                if (pType == "")
                    parameter[4].Value = DBNull.Value;

                else
                parameter[4].Value = pType;


                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_Product_InForStat",
                        sumDs,
                        new string[] { "T_Product_In_Stat" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InForStat OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InForStat Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }

        public WMSDS Select_T_Customer(string customercode)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select OnlyID,CustomerCode,CustomerName  from T_Customer ";
                if (customercode != "")
                    sqlstr += " where customercode = @CustomerCode ";
                sqlstr += " order by customercode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = customercode;
                par1.DbType = DbType.String;
                par1.ParameterName = "@CustomerCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Customer" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_CustomerQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_CustomerQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public WMSDS select_T_Transport()
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " SELECT OnlyID,TransportCode,TransportName,TransportDesc,IsChoose FROM T_Transport ";
                
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Transport" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_TransportQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_CustomerQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        /// <summary>
        /// 业务类型查询
        /// </summary>
        /// <param name="businessCode">业务类型代码</param>
        /// <param name="businessdesc">业务类型类型</param>
        /// <returns></returns>
        public WMSDS Select_T_BusinessType(string businessCode,string businessdesc)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select OnlyID,BusinessCode,BusinessName,BusinessType  from T_Business_Type where 1=1 ";
                if (businessCode != "")
                    sqlstr += " and BusinessType = @BusinessType ";
                if (businessdesc != "")
                    sqlstr += " and BusinessDesc = @BusinessDesc ";
                sqlstr += " order by BusinessCode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = businessCode;
                par1.DbType = DbType.String;
                par1.ParameterName = "@BusinessType";
                SqlParameter par2 = new SqlParameter();
                par2.Value = businessdesc;
                par2.DbType = DbType.String;
                par2.ParameterName = "@BusinessDesc";
                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1,
                  par2

                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Business_Type" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_BusinessQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_BusinessQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 查询接口组织字典
        /// </summary>
        /// <param name="orgInfo">组织代码，为空的话查询全部</param>
        /// <returns></returns>
        public InterfaceDS Select_CT_OrgInfo(string orgInfo)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select LSBH,OrgCode,OrgName,OrgType,WriteDate  from CT_OrgInfo ";
                if (orgInfo != "")
                    sqlstr += " where orgtype='3' and OrgCode = @OrgCode ";
                sqlstr += " order by OrgCode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = orgInfo;
                par1.DbType = DbType.String;
                par1.ParameterName = "@OrgCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_OrgInfo" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_OrgInfoQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_OrgInfoQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public InterfaceDS Select_CT_BMZD(string org,string bm)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select bm.LSBH,bm.DeptCode,bm.DeptName,oi.ORGCode,oi.ORGName,bm.WriteDate  from CT_BMZD bm ";
                 sqlstr += " left join ct_orginfo oi on oi.ORGCode = bm.ORGCode ";

                if (org != "")
                    sqlstr += " where bm.OrgCode = @OrgCode ";
                sqlstr += " order by bm.DeptCode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = org;
                par1.DbType = DbType.String;
                par1.ParameterName = "@OrgCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_BMZD" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_BMZDQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_BMZDQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public InterfaceDS Select_T_Factory_Warehouse()
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select *  from T_Factory_Warehouse ";
                sqlstr += " order by LSBH  ";
                //SqlParameter par1 = new SqlParameter();
                //par1.Value = orgInfo;
                //par1.DbType = DbType.String;
                //par1.ParameterName = "@OrgCode";

                //SqlParameter[] parameter = new SqlParameter[]
                //{
                //  par1
                //};
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Factory_Warehouse" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_Factory_Warehouse OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_Factory_Warehouse Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public InterfaceDS Select_CT_CKZD(string org)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select LSBH,CKBH,CKMC ,WriteDate from CT_CKZD ";
                if (org != "")
                    sqlstr += " where OrgCode = @OrgCode ";
                sqlstr += " order by CKBH  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = org;
                par1.DbType = DbType.String;
                par1.ParameterName = "@OrgCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_CKZD" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_CKZDQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_CKZDQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public InterfaceDS Select_CT_RYZD(string ck, string emp)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select LSBH,EmpCode,EmpName,CKCode,CKName,WriteDate  from CT_RYZD ";
                if (ck != "")
                    sqlstr += " where CKCode = @CKCode ";
                sqlstr += " order by EmpCode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = ck;
                par1.DbType = DbType.String;
                par1.ParameterName = "@CKCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_RYZD" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_RYZDQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_RYZDQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public string Get_T_OutStock_Plan_NewFlow(string type, string machineid)
        {
            string voucherNO = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Get_T_OutStock_Plan_NewFlow]");

                #region 传参数

                parameter[0].Value = type+machineid;
                parameter[1].Value = "";
                parameter[1].Direction = ParameterDirection.Output;


                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Get_T_OutStock_Plan_NewFlow",
                        parameter
                        );
                    voucherNO = parameter[1].Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "Get_T_OutStock_Plan_NewFlow OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Get_T_OutStock_Plan_NewFlow Error:" + ex.ToString()));
                }
            }
            return voucherNO;
        }
        /// <summary>
        /// 查询发货通知单
        /// </summary>
        /// <param name="VoucherNO"></param>
        /// <param name="minDate_Invo"></param>
        /// <param name="maxDate_Invo"></param>
        /// <param name="minDate_Exec"></param>
        /// <param name="maxDate_Exec"></param>
        /// <param name="User_Invo"></param>
        /// <param name="User_Exec"></param>
        /// <param name="IsCheck"></param>
        /// <param name="IsExecute"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public WMSDS Select_T_OutStock_PlanByFK(
       string VoucherNO, string minDate_Invo, string maxDate_Invo, string minDate_Exec, string maxDate_Exec,
          string User_Invo, string User_Exec,
          int IsCheck, int IsExecute, int IsDelete)
        {

            WMSDS ds = new WMSDS();

            SqlParameter par1 = new SqlParameter("@VoucherNO", SqlDbType.VarChar, 20);
            if (VoucherNO == "")
                par1.Value = DBNull.Value;
            else
                par1.Value = VoucherNO;

            SqlParameter par2 = new SqlParameter("@minDate_Invo", SqlDbType.DateTime);
            if (minDate_Invo == "")
                par2.Value = DBNull.Value;
            else
                par2.Value = minDate_Invo;

            SqlParameter par3 = new SqlParameter("@maxDate_Invo", SqlDbType.DateTime);
            if (maxDate_Invo == "")
                par3.Value = DBNull.Value;
            else
                par3.Value = maxDate_Invo;

            SqlParameter par4 = new SqlParameter("@minDate_Exec", SqlDbType.DateTime);
            if (minDate_Exec == "")
                par4.Value = DBNull.Value;
            else
                par4.Value = minDate_Exec;

            SqlParameter par5 = new SqlParameter("@maxDate_Exec", SqlDbType.DateTime);
            if (maxDate_Exec == "")
                par5.Value = DBNull.Value;
            else
                par5.Value = maxDate_Exec;

            SqlParameter par6 = new SqlParameter("@User_Invo", SqlDbType.VarChar, 20);
            if (User_Invo == "")
                par6.Value = DBNull.Value;
            else
                par6.Value = User_Invo;

            SqlParameter par7 = new SqlParameter("@User_Exec", SqlDbType.VarChar, 20);
            if (User_Exec == "")
                par7.Value = DBNull.Value;
            else
                par7.Value = User_Exec;


            SqlParameter par8 = new SqlParameter("@IsCheck", SqlDbType.Bit);
            if (IsCheck == -1)
                par8.Value = DBNull.Value;
            else
                par8.Value = IsCheck;

            SqlParameter par9 = new SqlParameter("@IsExecute", SqlDbType.Bit);
            if (IsExecute == -1)
                par9.Value = DBNull.Value;
            else
                par9.Value = IsExecute;

            SqlParameter par10 = new SqlParameter("@IsDelete", SqlDbType.Bit);
            if (IsDelete == -1)
                par10.Value = DBNull.Value;
            else
                par10.Value = IsDelete;

            SqlParameter par11 = new SqlParameter("@Sql", SqlDbType.VarChar, 1000);
            par11.Direction = ParameterDirection.Output;


            SqlParameter[] pars = new SqlParameter[] { par1, par2, par3, par4, par5, par6, par7, par8, par9, par10, par11 };


            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_OutStock_PlanByFK",
                        ds,
                        new string[] { "T_OutStock_Plan" },
                        pars
                        );
                    string par = par11.Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "selectT_outstockplan OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "selectT_outstockplan Error:" + ex.ToString()));
                }
            }
            return ds;

        }
        /// <summary>
        /// 查询销售出库单
        /// </summary>
        /// <param name="VoucherNO"></param>
        /// <param name="minDate_Invo"></param>
        /// <param name="maxDate_Invo"></param>
        /// <param name="minDate_Exec"></param>
        /// <param name="maxDate_Exec"></param>
        /// <param name="User_Invo"></param>
        /// <param name="User_Exec"></param>
        /// <param name="IsCheck"></param>
        /// <param name="IsExecute"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public WMSDS Select_T_OutStockByFK(
         string VoucherNO, string minDate_Invo, string maxDate_Invo, string minDate_Exec, string maxDate_Exec,
            string User_Invo, string User_Exec,
            int IsCheck, int IsExecute, int IsDelete)
        {

            WMSDS ds = new WMSDS();

            SqlParameter par1 = new SqlParameter("@VoucherNO", SqlDbType.VarChar, 20);
            if (VoucherNO == "")
                par1.Value = DBNull.Value;
            else
                par1.Value = VoucherNO;

            SqlParameter par2 = new SqlParameter("@minDate_Invo", SqlDbType.DateTime);
            if (minDate_Invo == "")
                par2.Value = DBNull.Value;
            else
                par2.Value = minDate_Invo;

            SqlParameter par3 = new SqlParameter("@maxDate_Invo", SqlDbType.DateTime);
            if (maxDate_Invo == "")
                par3.Value = DBNull.Value;
            else
                par3.Value = maxDate_Invo;

            SqlParameter par4 = new SqlParameter("@minDate_Exec", SqlDbType.DateTime);
            if (minDate_Exec == "")
                par4.Value = DBNull.Value;
            else
                par4.Value = minDate_Exec;

            SqlParameter par5 = new SqlParameter("@maxDate_Exec", SqlDbType.DateTime);
            if (maxDate_Exec == "")
                par5.Value = DBNull.Value;
            else
                par5.Value = maxDate_Exec;

            SqlParameter par6 = new SqlParameter("@User_Invo", SqlDbType.VarChar, 20);
            if (User_Invo == "")
                par6.Value = DBNull.Value;
            else
                par6.Value = User_Invo;

            SqlParameter par7 = new SqlParameter("@User_Exec", SqlDbType.VarChar, 20);
            if (User_Exec == "")
                par7.Value = DBNull.Value;
            else
                par7.Value = User_Exec;


            SqlParameter par8 = new SqlParameter("@IsCheck", SqlDbType.Bit);
            if (IsCheck == -1)
                par8.Value = DBNull.Value;
            else
                par8.Value = IsCheck;

            SqlParameter par9 = new SqlParameter("@IsExecute", SqlDbType.Bit);
            if (IsExecute == -1)
                par9.Value = DBNull.Value;
            else
                par9.Value = IsExecute;

            SqlParameter par10 = new SqlParameter("@IsDelete", SqlDbType.Bit);
            if (IsDelete == -1)
                par10.Value = DBNull.Value;
            else
                par10.Value = IsDelete;

            SqlParameter par11 = new SqlParameter("@Sql", SqlDbType.VarChar,1000);
            par11.Direction = ParameterDirection.Output;


            SqlParameter[] pars = new SqlParameter[] { par1, par2, par3, par4, par5, par6, par7, par8, par9, par10,par11 };


            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_OutStockByFK",
                        ds,
                        new string[] { "T_OutStock" },
                        pars
                        );
                    string par = par11.Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "selectT_outstock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "selectT_outstock Error:" + ex.ToString()));
                }
            }
            return ds;

        }
        //public DataSet Select_T_OutStock_PlanAndEntryByVoucherNO(      string VoucherNO)
        //{

        //    DataSet ds = new DataSet();

        //    SqlParameter par1 = new SqlParameter("@varcharNO", SqlDbType.VarChar, 20);
        //    if (VoucherNO == "")
        //        par1.Value = DBNull.Value;
        //    else
        //        par1.Value = VoucherNO;
        //    SqlParameter[] pars = new SqlParameter[] { par1};


        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {

        //        try
        //        {
        //            MSSqlHelper.FillDataset(connection,
        //                CommandType.StoredProcedure,
        //                "SP_Select_T_OutStock_PlanAndEntryByVoucherNO",
        //                ds,
        //                new string[] { "T_OutStock_PlanAndEntry" },
        //                pars
        //                );
        //            OnSqlStateChange(new SqlStateEventArgs(true, "SP_Select_T_OutStock_PlanAndEntryByVoucherNO OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "SP_Select_T_OutStock_PlanAndEntryByVoucherNO Error:" + ex.ToString()));
        //        }
        //    }
        //    return ds;
//        ALTER PROCEDURE [dbo].[SP_Select_T_OutStock_PlanAndEntryByVoucherNO]
//    @varcharNO varchar(20)
//AS
//BEGIN
	
//    SELECT osp.VoucherNO,osp.Qty,osp.AuxQty,osp.CommitQty,osp.CommitAuxQty,osp.BusinessType,osp.IsClose,osp.IsCheck,osp.OnlyID,
//    ospe.MaterialCode,ospe.MaterialName,ospe.CoreDiameter,ospe.CustTrademark,ospe.EntryID,ospe.Grade,
//    ospe.IsWhiteFlag,ospe.Length_P,ospe.OrderNO,ospe.PaperCert,ospe.PlanAuxQty1,ospe.PlanQty,ospe.ReamPackType,
//    ospe.Reams,ospe.SKU,ospe.SlidesOfReam,ospe.SlidesOfSheet,ospe.SpecCustName,ospe.Specification,ospe.SpecProdName,
//    ospe.TrademarkStyle,ospe.VoucherID,ospe.WeightMode,ospe.Width_P,ospe.Width_R
//     from T_OutStock_Plan osp left join T_OutStock_Plan_Entry ospe on osp.OnlyID=ospe.VoucherID
//    where osp.VoucherNO=@varcharNO
//END
        //}
        public WMSDS Select_T_OutStock_Plan_Entry(string VoucherNO, int entryID)
        {

            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStock_Plan_Entry]");

                #region 传参数

                parameter[0].Value = VoucherNO;
                parameter[1].Value = entryID;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_OutStock_Plan_Entry",
                        ds,
                      new string[]{  "T_OutStock_Plan_Entry"},
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "selectT_OutStock_Plan_Entry OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "selectT_OutStock_Plan_Entry Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        //public string UpdateT_OutStock_PlanHeadByRow(WMSDS.T_OutStock_PlanRow tospRow)
        //{
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "SP_UpdateT_OutStock_Plan");

           
        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "SP_UpdateT_OutStock_Plan",
        //                parameter);

        //            //retid = Convert.ToInt32(parameter[72].Value);

        //            OnSqlStateChange(new SqlStateEventArgs(true, "SP_UpdateT_OutStock_Plan OK"));
        //            return "";
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "SP_UpdateT_OutStock_Plan Error:" + ex.ToString()));
        //            return ex.Message;
        //        }
        //    }
        //}
        /// <summary>
        /// 用事务插入发货通知的表头和循环插入发货通知的分录
        /// </summary>
        /// <param name="tospRow">通知的表头</param>
        /// <param name="rowCollect">分录</param>
        /// <returns></returns>
        public string Tran_SaveNewOutStockPlan(WMSDS.T_OutStock_PlanRow tospRow,DataRowCollection rowCollect)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //插入抬头
                    int result = this.Insert_T_OutStock_PlanByRow(tospRow);
                    if(result<0)
                        return "Tran_SaveNewOutStockPlan抬头插入失败";
                    //插入分录
                    int entryresult = -1;
                    for (int i = 0; i < rowCollect.Count; i++)
                    {
                        WMSDS.T_OutStock_Plan_EntryRow ospeRow = rowCollect[i] as WMSDS.T_OutStock_Plan_EntryRow;
                        ospeRow.VoucherID = result;
                        entryresult = this.Insert_T_OutStock_Plan_EntryByRow(ospeRow);
                        if (entryresult < 0)
                            return "Tran_SaveNewOutStockPlan第"+i+"个分录插入失败";
                    }
                    if (entryresult > 0)
                    {
                       int ret = this.Update_CT_RequestInfoStat(tospRow.VoucherNO,"已参照");
                        if (ret <0)
                            return "Tran_SaveNewOutStockPlan更新源单状态失败";
                    }
                    ts.Complete();
                        return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }
        /// <summary>
        /// 更新通知单源单状态
        /// </summary>
        /// <param name="voucherno">单号</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        private int Update_CT_RequestInfoStat(string voucherno, string state)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sql = "update  CT_RequestInfo set cdefine3 ='" + state + "' where voucherno ='" + voucherno + "'";

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.Text,
                       sql
                        );
                    retid = 0;
                    OnSqlStateChange(new SqlStateEventArgs(true, "[[Update_CT_RequestInfoStat]] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[[Update_CT_RequestInfoStat]] Error:" + ex.ToString()));
                }
            }
            return retid;
        }/// <summary>
        /// 用事务更新发货通知的表头和循环插入发货通知的分录（主键voucherno）
        /// </summary>
        /// <param name="tospRow">通知的表头</param>
        /// <param name="rowCollect">分录</param>
        /// <returns></returns>
        public string Tran_SaveUpdateOutStockPlan(WMSDS.T_OutStock_PlanRow tospRow, DataRowCollection rowCollect)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //更新抬头
                    string result = this.Update_T_OutStock_PlanByRow(tospRow);
                    if (result != "")
                        return "Tran_SaveNewOutStockPlan抬头更新失败" + result;
                    ////找出抬头的onlyid
                    //WMSDS wms = this.Select_T_OutStock_PlanByFK(tospRow.VoucherNO, "", "", "", "", "", "", 0, 0, 0);
                    //string onlyid = wms.T_OutStock_Plan.Rows[0][wms.T_OutStock_Plan.OnlyIDColumn].ToString();
                    //删除单据表体，再插入
                    string ss = this.Delete_T_OutStock_Plan_EntryByVoucherNO(tospRow.VoucherNO);
                    if (ss != "")
                        return "Tran_SaveNewOutStockPlan分录删除失败" + ss;
                    //插入分录
                    for (int i = 0; i < rowCollect.Count; i++)
                    {
                        WMSDS.T_OutStock_Plan_EntryRow ospeRow = rowCollect[i] as WMSDS.T_OutStock_Plan_EntryRow;
                        ospeRow.VoucherID = tospRow.OnlyID;
                        int entryresult = this.Insert_T_OutStock_Plan_EntryByRow(ospeRow);
                        if (entryresult < 0)
                            return "Tran_SaveNewOutStockPlan分录" + (i + 1) + "插入失败";
                    }
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }
        public int Insert_T_OutStock_PlanByRow(WMSDS.T_OutStock_PlanRow row)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_OutStock_Plan]");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                parameter[0].Value = row.OnlyID;

                if (row.IsBusinessTypeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.BusinessType;
                }
                if (row.IsBillTypeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BillType;
                }
                if (row.IsFactoryIDNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.FactoryID;
                }
                if (row.IsWHCodeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.WHCode;
                }
                if (row.IsTradeTypeNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.TradeType;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.TransportType;
                }
                if (row.IsSourceVoucherNONull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.SourceVoucherNO;
                }
                if (row.IsSourceVoucherTypeNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.SourceVoucherType;
                }
                if (row.IsDeptNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.DeptName;
                }
                if (row.IsBillerNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.Biller;
                }
                if (row.IsBillDeptNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.BillDept;
                }
                if (row.IsBillDateNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.BillDate;
                }
                if (row.IsBillRemarkNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BillRemark;
                }
                if (row.IsCustomerNameNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.CustomerName;
                }
                if (row.IsCustomerAddrNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.CustomerAddr;
                }
                if (row.IsVehicleNONull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.VehicleNO;
                }
                if (row.IsRetrieveDateNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.RetrieveDate;
                }
                if (row.IsCheckerNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.Checker;
                }
                if (row.IsCheckDateNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.CheckDate;
                }
                if (row.IsFinisherNull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.Finisher;
                }
                if (row.IsFinishDateNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.FinishDate;
                }
                if (row.IsIsCheckNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.IsCheck;
                }
                if (row.IsIsCancelNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.IsCancel;
                }
                if (row.IsIsCloseNull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.IsClose;
                }
                if (row.IsQtyNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.Qty;
                }
                if (row.IsAuxQtyNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.AuxQty;
                }
                if (row.IsBoxNONull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.BoxNO;
                }
                if (row.IsOrderNONull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.OrderNO;
                }
                if (row.IsPickNONull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.PickNO;
                }
                if (row.IsCarrierNONull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.CarrierNO;
                }
                if (row.IsForklifterNONull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.ForklifterNO;
                }
                if (row.IsPortNONull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.PortNO;
                }
                if (row.IsBoatNONull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.BoatNO;
                }
                if (row.IsSealNONull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.SealNO;
                }
                if (row.IsCommitQtyNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.CommitQty;
                } if (row.IsCommitAuxQtyNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.CommitAuxQty;
                }
                if (row.IsWHToCodeNull())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.WHToCode;
                }
                if (row.IsOrgToCodeNull())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.OrgToCode;
                }
                    parameter[39].Value = row.VoucherNO;
                #endregion



                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_OutStock_Plan",
                        parameter);

                    retid = Convert.ToInt32(parameter[0].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "Insert_T_OutStock_Planbyrow OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Insert_T_OutStock_Planbyrow Error:" + ex.ToString()));
                }
            }
            return retid;
        }

        public InterfaceDS Select_CT_WLZD(string wldm)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select LSBH,WLBH,WLMC,PZBM,PZMC,PZEN,LBBM,LBMC,LBEN,PPBM,PPMC,PPEN,PSKU,Cdefine1,WriteDate  from CT_WLZD ";
                if (wldm != "")
                    sqlstr += " where WLBH = @WLBH ";
                sqlstr += " order by WLBH  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = wldm;
                par1.DbType = DbType.String;
                par1.ParameterName = "@WLBH";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_WLZD" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_WLZDQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_WLZDQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public WMSDS Select_T_Grade(string gradeCode)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select OnlyID,GradeCode,GradeName  from T_Grade ";
                if (gradeCode != "")
                    sqlstr += " where gradeCode = @gradeCode ";
                sqlstr += " order by gradeCode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = gradeCode;
                par1.DbType = DbType.String;
                par1.ParameterName = "@gradeCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Grade" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_GradeQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_GradeQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public int Insert_T_OutStock_Plan_EntryByRow(WMSDS.T_OutStock_Plan_EntryRow row)
        {
              int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_OutStock_Plan_Entry]");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                parameter[0].Value = row.IsOnlyIDNull()?0:row.OnlyID;

                if (row.IsProductTypeIDNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductTypeID;
                }
                if (row.IsBrandNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.Brand;
                }
                if (row.IsTypeNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Type;
                }
                if (row.IsGradeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.Grade;
                }
                if (row.IsBasisweightNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.Basisweight;
                }
                if (row.IsSpecificationNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.Specification;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.CoreDiameter;
                }
                if (row.IsWidth_RNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Width_R;
                }
                if (row.IsWidth_PNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.Width_P;
                }
                if (row.IsLength_PNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.Length_P;
                }
                if (row.IsReamsNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Reams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.SlidesOfSheet;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.ReamPackType;
                }
                if (row.IsRemarkNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.Remark;
                }
                if (row.IsMaterialCodeNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.MaterialCode;
                }
                if (row.IsSKUNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.SKU;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.TrademarkStyle;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.IsWhiteFlag;
                }
                if (row.IsOrderNONull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.OrderNO;
                }
                if (row.IsPriceNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.Price;
                }
                if (row.IsPaperCertNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.PaperCert;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.SpecCustName;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.CustTrademark;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.WeightMode;
                }
                if (row.IsPlanQtyNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.PlanQty;
                }
                if (row.IsPlanAuxQty1Null())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.PlanAuxQty1;
                }
                if (row.IsPlanAuxQty2Null())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.PlanAuxQty2;
                }
                if (row.IsPlanCommitQtyNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.PlanCommitQty;
                }
                if (row.IsPlanCommitAuxQty1Null())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.PlanCommitAuxQty1;
                }
                if (row.IsPlanCommitAuxQty2Null())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.PlanCommitAuxQty2;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.Udefine3;
                }
                if (row.IsMaterialNameNull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.MaterialName;
                }
                if (row.IsBatchNONull())
                {
                    parameter[40].Value = DBNull.Value;
                }
                else
                {
                    parameter[40].Value = row.BatchNO;
                }
                if (row.IsSourceEntryIDNull())
                {
                    parameter[41].Value = DBNull.Value;
                }
                else
                {
                    parameter[41].Value = row.SourceEntryID;
                }
                if (row.IsDiameterNull())
                {
                    parameter[42].Value = DBNull.Value;
                }
                else
                {
                    parameter[42].Value = row.Diameter;
                }
                if (row.IsRollLengthNull())
                {
                    parameter[43].Value = DBNull.Value;
                }
                else
                {
                    parameter[43].Value = row.RollLength;
                }

                    parameter[44].Value = row.EntryID;
                
                    parameter[45].Value = row.VoucherID;
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_OutStock_Plan_Entry",
                        parameter);

                    retid = Convert.ToInt32(parameter[0].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[Insert_T_OutStock_Plan_Entrybyrow] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Insert_T_OutStock_Plan_Entrybyrow] Error:" + ex.ToString()));
                }
            }
            return retid;
        }

        public InterfaceDS Select_CT_ZDYZD(string zdlx)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select LSBH,ZDLX,Code,Name,WriteDate  from CT_ZDYZD ";
                if (zdlx != "")
                    sqlstr += " where ZDLX = @ZDLX ";
                sqlstr += " order by Name  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = zdlx;
                par1.DbType = DbType.String;
                par1.ParameterName = "@ZDLX";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_ZDYZD" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_ZDYZDQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_ZDYZDQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 查询某个发货通知单的表体和分录用来显示字母表格
        /// </summary>
        /// <param name="dateS"></param>
        /// <param name="dateE"></param>
        /// <returns></returns>
        public DataSet Select_T_OutPlanAndEntry_Relation(string dateS, string dateE)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutPlanAndEntry_Relation]");

                #region 传参数

                parameter[0].Value = dateS;
                parameter[1].Value = dateE;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutPlanAndEntry_Relation]",
                        ds,
                      new string[] {  "T_OutStock_Plan","T_OutStock_Plan_Entry" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Select_T_OutPlanAndEntry_Relation] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Select_T_OutPlanAndEntry_Relation] Error:" + ex.ToString()));
                }
            }
            return ds;
        }
        /// <summary>
        /// 查询某个发货通知单的表体和分录用来显示字母表格
        /// </summary>
        /// <param name="dateS"></param>
        /// <param name="dateE"></param>
        /// <returns></returns>
        public DataSet Select_T_OutPlanAndEntry_RelationByOutStock(string voucherno)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutPlanAndEntry_RelationByOutStock]");

                #region 传参数

                parameter[0].Value = voucherno;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutPlanAndEntry_RelationByOutStock]",
                        ds,
                      new string[] { "T_OutStock_Plan", "T_OutStock_Plan_Entry" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutPlanAndEntry_RelationByOutStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutPlanAndEntry_RelationByOutStock Error:" + ex.ToString()));
                }
            }
            return ds;
        }
        /// <summary>
        /// 查询某个发货通知单的表体和分录用来加载历史发货通知单
        /// </summary>
        /// <param name="voucherno"></param>
        /// <returns></returns>
        public WMSDS Select_T_OutPlanAndEntry_RelationByVoucherNO(string voucherno)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutPlanAndEntry_RelationByVoucherNO]");

                #region 传参数

                parameter[0].Value = voucherno;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutPlanAndEntry_RelationByVoucherNO]",
                        ds,
                      new string[] { "T_OutStock_Plan", "T_OutStock_Plan_Entry", "T_OutStock_Entry" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutPlanAndEntry_RelationByVoucherNO OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutPlanAndEntry_RelationByVoucherNO Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        //public WMSDS Select_OutPlanAndEntry_RelationForUpload(string voucherno)
        //{
        //    WMSDS ds = new WMSDS();
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_OutPlanAndEntry_RelationForUpload]");

        //        #region 传参数

        //        parameter[0].Value = voucherno;
        //        #endregion
        //        try
        //        {
        //            MSSqlHelper.FillDataset(connection,
        //                CommandType.StoredProcedure,
        //                "[Select_OutPlanAndEntry_RelationForUpload]",
        //                ds,
        //              new string[] { "T_OutStock_Plan", "T_OutStock_Plan_Entry" },
        //                parameter

        //                );
        //            OnSqlStateChange(new SqlStateEventArgs(true, "Select_OutPlanAndEntry_RelationForUpload OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "Select_OutPlanAndEntry_RelationForUpload Error:" + ex.ToString()));
        //        }
        //    }
        //    return ds;
        //}
        /// <summary>
        /// 删除某个发货通知单的全部分录
        /// </summary>
        /// <param name="VoucherNO">可以是单号也可以是id</param>
        /// <returns></returns>
        public string Delete_T_OutStock_Plan_EntryByVoucherNO(string VoucherNO)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Delete_T_OutStock_Plan_EntryByVoucherNO]");

                #region 传参数

                parameter[0].Value = VoucherNO;
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Delete_T_OutStock_Plan_EntryByVoucherNO",
                          parameter);
                    return ret = "";
                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_OutStock_Plan_EntryByVoucherNO OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_OutStock_Plan_EntryByVoucherNO Error:" + ex.ToString()));
                    return ret+ex.Message;

                }
            }
        }
        /// <summary>
        /// 更新发货通知单表头
        /// </summary>
        /// <param name="tospRow"></param>
        /// <returns></returns>
        public string Update_T_OutStock_PlanByRow(WMSDS.T_OutStock_PlanRow row)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_OutStock_Plan]");


                #region 传参数
                parameter[0].Value = row.OnlyID;
               // parameter[0].Value = 0;

                if (row.IsBusinessTypeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.BusinessType;
                }
                if (row.IsBillTypeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BillType;
                }
                if (row.IsFactoryIDNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.FactoryID;
                }
                if (row.IsWHCodeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.WHCode;
                }
                if (row.IsTradeTypeNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.TradeType;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.TransportType;
                }
                if (row.IsSourceVoucherNONull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.SourceVoucherNO;
                }
                if (row.IsSourceVoucherTypeNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.SourceVoucherType;
                }
                if (row.IsDeptNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.DeptName;
                }
                if (row.IsBillerNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.Biller;
                }
                if (row.IsBillDeptNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.BillDept;
                }
                if (row.IsBillDateNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.BillDate;
                }
                if (row.IsBillRemarkNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BillRemark;
                }
                if (row.IsCustomerNameNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.CustomerName;
                }
                if (row.IsCustomerAddrNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.CustomerAddr;
                }
                if (row.IsVehicleNONull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.VehicleNO;
                }
                if (row.IsRetrieveDateNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.RetrieveDate;
                }
                if (row.IsCheckerNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.Checker;
                }
                if (row.IsCheckDateNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.CheckDate;
                }
                if (row.IsFinisherNull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.Finisher;
                }
                if (row.IsFinishDateNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.FinishDate;
                }
                if (row.IsIsCheckNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.IsCheck;
                }
                if (row.IsIsCancelNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.IsCancel;
                }
                if (row.IsIsCloseNull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.IsClose;
                }
                if (row.IsQtyNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.Qty;
                }
                if (row.IsAuxQtyNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.AuxQty;
                }
                if (row.IsBoxNONull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.BoxNO;
                }
                if (row.IsOrderNONull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.OrderNO;
                }
                if (row.IsPickNONull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.PickNO;
                }
                if (row.IsCarrierNONull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.CarrierNO;
                }
                if (row.IsForklifterNONull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.ForklifterNO;
                }
                if (row.IsPortNONull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.PortNO;
                }
                if (row.IsBoatNONull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.BoatNO;
                }
                if (row.IsSealNONull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.SealNO;
                }
                if (row.IsCommitQtyNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.CommitQty;
                }
                if (row.IsCommitAuxQtyNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.CommitAuxQty;
                }
                if (row.IsWHToCodeNull())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.WHToCode;
                }
                if (row.IsOrgToCodeNull())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.OrgToCode;
                }
                if (row.IsVoucherNONull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.VoucherNO;
                }
                #endregion


                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Update_T_OutStock_Plan",
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_T_OutStock_Plan OK"));
                    return ret = "";

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_T_OutStock_Plan Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }

        public DataSet Select_OutPlan_Situation(string voucherId, string planId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_OutPlan_Situation]");

                #region 传参数

                parameter[0].Value = voucherId;
                parameter[1].Value = planId;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_OutPlan_Situation]",
                        ds,
                      new string[] { "T_OutStock_Plan",  },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_OutPlan_Situation OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_OutPlan_Situation Error:" + ex.ToString()));
                }
            }
            return ds;
        }
        public DataSet Select_OutStock_Situation(string voucherId, string planId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_OutStock_Situation]");

                #region 传参数

                parameter[0].Value = voucherId;
                parameter[1].Value = planId;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_OutStock_Situation]",
                        ds,
                      new string[] { "T_OutStock", },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_OutStock_Situation OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_OutStock_Situation Error:" + ex.ToString()));
                }
            }
            return ds;
        }
       
        /// <summary>
        /// 生成批次号
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="date"></param>
        /// <param name="batchstr"></param>
        /// <returns></returns>
        public string Select_T_BatchNO(string factory, string date, string batchstr)
        {
            string batchno = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_BatchNO]");



                parameter[0].Value = factory;

                parameter[1].Value = date;

                parameter[2].Value = batchstr;
           

                parameter[3].Value = "";
                parameter[3].Direction = ParameterDirection.Output;




                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Select_T_BatchNO",
                        parameter
                        );
                    batchno = parameter[3].Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_BatchNO OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_BatchNO Error:" + ex.ToString()));
                }
            }
            return batchno;
        }

        public InterfaceDS Select_T_Product_InDetailAndStat(string factory, string user, string dateS, string dateE, string pType, string batchno, string voucherid, string rb, string specific, string businesstype, string material, string grade, string order, string wFlag, string tskh, string whremark, string barcodenotin,string papercert)
        {
            InterfaceDS sumDs = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_Product_InDetailAndStat]");

                #region 传参数
                if(dateS=="")
                    parameter[0].Value = DBNull.Value;
                else
                parameter[0].Value = dateS;
                if (dateE == "")
                    parameter[1].Value = DBNull.Value;
                else
                parameter[1].Value = dateE;
                if (user == "")
                    parameter[2].Value = DBNull.Value;
                else
                parameter[2].Value = user;
                if (factory == "")
                    parameter[3].Value = DBNull.Value;
                else
                parameter[3].Value = factory;
                if (pType == "")
                    parameter[4].Value = DBNull.Value;
                else
                parameter[4].Value = pType;
                if (batchno == "")
                    parameter[5].Value = DBNull.Value;
                else
                parameter[5].Value = batchno;

                if (voucherid == "")
                    parameter[6].Value = DBNull.Value;
                else
                    parameter[6].Value = voucherid;
                if (rb == "")
                    parameter[7].Value = DBNull.Value;
                else
                    parameter[7].Value = rb;
                if (specific == "")
                    parameter[8].Value = DBNull.Value;
                else
                    parameter[8].Value = specific;
                if (businesstype == "")
                    parameter[9].Value = DBNull.Value;
                else
                    parameter[9].Value = businesstype;
                //
                 if (material == "")
                    parameter[10].Value = DBNull.Value;
                else
                    parameter[10].Value = material;
                if (grade == "")
                    parameter[11].Value = DBNull.Value;
                else
                    parameter[11].Value = grade;
                if (order == "")
                    parameter[12].Value = DBNull.Value;
                else
                    parameter[12].Value = order;
                //zjg modify
                if (wFlag == "")
                    parameter[13].Value = DBNull.Value;
                else
                    parameter[13].Value = wFlag;
                 if (tskh == "")
                     parameter[14].Value = DBNull.Value;
                 else
                     parameter[14].Value = tskh;
                 if (whremark == "")
                     parameter[15].Value = DBNull.Value;
                 else
                     parameter[15].Value = whremark;
                 if (barcodenotin == "")
                     parameter[16].Value = DBNull.Value;
                 else
                     parameter[16].Value = barcodenotin;
                 if (papercert == "")
                     parameter[17].Value = DBNull.Value;
                 else
                     parameter[17].Value = papercert;

                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_Product_InDetailAndStat]",
                        sumDs,
                        new string[] { "CT_StockInDetail", "CT_StockIn" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InDetailAndStat OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InDetailAndStat Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }
        //public int Insert_T_InStock(WMSDS.T_InStockRow row,string connectString)
        //{
        //    int retid = -1;
        //    using (SqlConnection connection = new SqlConnection(connectString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_InStock]");


        //        #region 传参数
        //        //parameter[0].Value = row.OnlyID;
        //        parameter[0].Value = row.OnlyID;

        //        if (row.IsBusinessTypeNull())
        //        {
        //            parameter[1].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[1].Value = row.BusinessType;
        //        }
        //        if (row.IsBillTypeNull())
        //        {
        //            parameter[2].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[2].Value = row.BillType;
        //        }
        //        if (row.IsFactoryIDNull())
        //        {
        //            parameter[3].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[3].Value = row.FactoryID;
        //        }
        //        if (row.IsWHCodeNull())
        //        {
        //            parameter[4].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[4].Value = row.WHCode;
        //        }
        //        if (row.IsKeeperNull())
        //        {
        //            parameter[5].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[5].Value = row.Keeper;
        //        }
        //        if (row.IsSourceVoucherNONull())
        //        {
        //            parameter[6].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[6].Value = row.SourceVoucherNO;
        //        }
        //        if (row.IsBillRemarkNull())
        //        {
        //            parameter[7].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[7].Value = row.BillRemark;
        //        }
        //        if (row.IsBillerNull())
        //        {
        //            parameter[8].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[8].Value = row.Biller;
        //        }
        //        if (row.IsCheckerNull())
        //        {
        //            parameter[9].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[9].Value = row.Checker;
        //        }
        //        if (row.IsIsCheckNull())
        //        {
        //            parameter[10].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[10].Value = row.IsCheck;
        //        }
        //        if (row.IsIsCancelNull())
        //        {
        //            parameter[11].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[11].Value = row.IsCancel;
        //        }
        //        if (row.IsIsCloseNull())
        //        {
        //            parameter[12].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[12].Value = row.IsClose;
        //        }
        //        if (row.IsBillDateNull())
        //        {
        //            parameter[13].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[13].Value = row.BillDate;
        //        }
        //        if (row.IsUploadDateNull())
        //        {
        //            parameter[14].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[14].Value = row.UploadDate;
        //        }
        //        if (row.IsIsUploadNull())
        //        {
        //            parameter[15].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[15].Value = row.IsUpload;
        //        }
        //        parameter[16].Value = row.VoucherNO;

        //        #endregion






        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "[Insert_T_InStock]",
        //                parameter);

        //            retid = Convert.ToInt32(parameter[0].Value);

        //            OnSqlStateChange(new SqlStateEventArgs(true, "[Insert_T_InStock] OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "[Insert_T_InStock] Error:" + ex.ToString()));
        //        }
        //    }
        //    return retid;
        //}

        public int Insert_T_InStock(WMSDS.T_InStockRow row,string connStr)
        {
            int retid = -1;
            if (connStr == "")//异地数据库
                connStr = ConnctionString;//Factory
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_InStock]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;
                parameter[0].Value = row.OnlyID;

                if (row.IsBusinessTypeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.BusinessType;
                }
                if (row.IsBillTypeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BillType;
                }
                if (row.IsFactoryIDNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.FactoryID;
                }
                if (row.IsWHCodeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.WHCode;
                }
                if (row.IsKeeperNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.Keeper;
                }
                if (row.IsSourceVoucherNONull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.SourceVoucherNO;
                }
                if (row.IsBillRemarkNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.BillRemark;
                }
                if (row.IsBillerNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Biller;
                }
                if (row.IsCheckerNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.Checker;
                }
                if (row.IsIsCheckNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.IsCheck;
                }
                if (row.IsIsCancelNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.IsCancel;
                }
                if (row.IsIsCloseNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.IsClose;
                }
                if (row.IsBillDateNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BillDate;
                }
                if (row.IsUploadDateNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.UploadDate;
                }
                if (row.IsIsUploadNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.IsUpload;
                }
                parameter[16].Value = row.VoucherNO;

                #endregion


	



                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Insert_T_InStock]",
                        parameter);

                    retid = Convert.ToInt32(parameter[0].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[Insert_T_InStock] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Insert_T_InStock] Error:" + ex.ToString()));
                }
            }
            return retid; 
        }

        public int Insert_T_InStock_Entry(WMSDS.T_InStock_EntryRow row,string connStr)
        {
            int retid = -1;
            if (connStr == "")
                connStr = ConnctionString;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_InStock_Entry]");
                #region 传参数
                //parameter[0].Value = row.OnlyID;
                parameter[0].Value = row.OnlyID;

                if (row.IsMaterialCodeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.MaterialCode;
                }
                if (row.IsGradeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.Grade;
                }
                if (row.IsWidth_RNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Width_R;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.WeightMode;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.CoreDiameter;
                }
                if (row.IsLength_PNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.Length_P;
                }
                if (row.IsWidth_PNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.Width_P;
                }
                if (row.IsReamsNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Reams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.SlidesOfSheet;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.TransportType;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.ReamPackType;
                }
                if (row.IsFiberDirectNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.FiberDirect;
                }
                if (row.IsLayersNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.Layers;
                }
                if (row.IsSKUNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.SKU;
                }
                if (row.IsCommitQtyNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.CommitQty;
                }
                if (row.IsAuxCommitQty1Null())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.AuxCommitQty1;
                }
                if (row.IsAuxCommitQty2Null())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.AuxCommitQty2;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.CustTrademark;
                }
                if (row.IsPaperCertNull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.PaperCert;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.SpecCustName;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.TrademarkStyle;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.IsWhiteFlag;
                }
                if (row.IsOrderNONull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.OrderNO;
                }
                if (row.IsStockInDateNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.StockInDate;
                }
                if (row.IsInspurVoucherNONull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.InspurVoucherNO;
                }
                if (row.IsBatchNONull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.BatchNO;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.Udefine3;
                }

                if (row.IsDiameterNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.Diameter;
                } if (row.IsRollLengthNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.RollLength;
                }
                parameter[37].Value = row.EntryID;
                parameter[38].Value = row.VoucherID;

                #endregion
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Insert_T_InStock_Entry]",
                        parameter);

                    retid = Convert.ToInt32(parameter[0].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[[Insert_T_InStock_Entry]] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[[Insert_T_InStock_Entry]] Error:" + ex.ToString()));
                }
            }
            return retid;
        }
  
        //public int Insert_T_InStock_Entry(WMSDS.T_InStock_EntryRow row,string connectString)
        //{
        //    int retid = -1;
        //    using (SqlConnection connection = new SqlConnection(connectString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_InStock_Entry]");
        //        #region 传参数
        //        //parameter[0].Value = row.OnlyID;
        //        parameter[0].Value = row.OnlyID;

        //        if (row.IsMaterialCodeNull())
        //        {
        //            parameter[1].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[1].Value = row.MaterialCode;
        //        }
        //        if (row.IsGradeNull())
        //        {
        //            parameter[2].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[2].Value = row.Grade;
        //        }
        //        if (row.IsWidth_RNull())
        //        {
        //            parameter[3].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[3].Value = row.Width_R;
        //        }
        //        if (row.IsWeightModeNull())
        //        {
        //            parameter[4].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[4].Value = row.WeightMode;
        //        }
        //        if (row.IsCoreDiameterNull())
        //        {
        //            parameter[5].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[5].Value = row.CoreDiameter;
        //        }
        //        if (row.IsLength_PNull())
        //        {
        //            parameter[6].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[6].Value = row.Length_P;
        //        }
        //        if (row.IsWidth_PNull())
        //        {
        //            parameter[7].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[7].Value = row.Width_P;
        //        }
        //        if (row.IsReamsNull())
        //        {
        //            parameter[8].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[8].Value = row.Reams;
        //        }
        //        if (row.IsSlidesOfReamNull())
        //        {
        //            parameter[9].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[9].Value = row.SlidesOfReam;
        //        }
        //        if (row.IsSlidesOfSheetNull())
        //        {
        //            parameter[10].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[10].Value = row.SlidesOfSheet;
        //        }
        //        if (row.IsTransportTypeNull())
        //        {
        //            parameter[11].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[11].Value = row.TransportType;
        //        }
        //        if (row.IsReamPackTypeNull())
        //        {
        //            parameter[12].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[12].Value = row.ReamPackType;
        //        }
        //        if (row.IsFiberDirectNull())
        //        {
        //            parameter[13].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[13].Value = row.FiberDirect;
        //        }
        //        if (row.IsLayersNull())
        //        {
        //            parameter[14].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[14].Value = row.Layers;
        //        }
        //        if (row.IsSKUNull())
        //        {
        //            parameter[15].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[15].Value = row.SKU;
        //        }
        //        if (row.IsCommitQtyNull())
        //        {
        //            parameter[16].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[16].Value = row.CommitQty;
        //        }
        //        if (row.IsAuxCommitQty1Null())
        //        {
        //            parameter[17].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[17].Value = row.AuxCommitQty1;
        //        }
        //        if (row.IsAuxCommitQty2Null())
        //        {
        //            parameter[18].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[18].Value = row.AuxCommitQty2;
        //        }
        //        if (row.IsCustTrademarkNull())
        //        {
        //            parameter[19].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[19].Value = row.CustTrademark;
        //        }
        //        if (row.IsPaperCertNull())
        //        {
        //            parameter[20].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[20].Value = row.PaperCert;
        //        }
        //        if (row.IsSpecProdNameNull())
        //        {
        //            parameter[21].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[21].Value = row.SpecProdName;
        //        }
        //        if (row.IsSpecCustNameNull())
        //        {
        //            parameter[22].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[22].Value = row.SpecCustName;
        //        }
        //        if (row.IsTrademarkStyleNull())
        //        {
        //            parameter[23].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[23].Value = row.TrademarkStyle;
        //        }
        //        if (row.IsIsWhiteFlagNull())
        //        {
        //            parameter[24].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[24].Value = row.IsWhiteFlag;
        //        }
        //        if (row.IsOrderNONull())
        //        {
        //            parameter[25].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[25].Value = row.OrderNO;
        //        }
        //        if (row.IsStockInDateNull())
        //        {
        //            parameter[26].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[26].Value = row.StockInDate;
        //        }
        //        if (row.IsInspurVoucherNONull())
        //        {
        //            parameter[27].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[27].Value = row.InspurVoucherNO;
        //        }
        //        if (row.IsBatchNONull())
        //        {
        //            parameter[28].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[28].Value = row.BatchNO;
        //        }
        //        if (row.IsCdefine1Null())
        //        {
        //            parameter[29].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[29].Value = row.Cdefine1;
        //        }
        //        if (row.IsCdefine2Null())
        //        {
        //            parameter[30].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[30].Value = row.Cdefine2;
        //        }
        //        if (row.IsCdefine3Null())
        //        {
        //            parameter[31].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[31].Value = row.Cdefine3;
        //        }
        //        if (row.IsUdefine1Null())
        //        {
        //            parameter[32].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[32].Value = row.Udefine1;
        //        }
        //        if (row.IsUdefine2Null())
        //        {
        //            parameter[33].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[33].Value = row.Udefine2;
        //        }
        //        if (row.IsUdefine3Null())
        //        {
        //            parameter[34].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[34].Value = row.Udefine3;
        //        }

        //        if (row.IsDiameterNull())
        //        {
        //            parameter[35].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[35].Value = row.Diameter;
        //        } if (row.IsRollLengthNull())
        //        {
        //            parameter[36].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[36].Value = row.RollLength;
        //        }
        //        parameter[37].Value = row.EntryID;
        //        parameter[38].Value = row.VoucherID;

        //        #endregion
        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "[Insert_T_InStock_Entry]",
        //                parameter);

        //            retid = Convert.ToInt32(parameter[0].Value);

        //            OnSqlStateChange(new SqlStateEventArgs(true, "[[Insert_T_InStock_Entry]] OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "[[Insert_T_InStock_Entry]] Error:" + ex.ToString()));
        //        }
        //    }
        //    return retid; 
        //}
        //public int Insert_T_InStock_Product(WMSDS.T_InStock_ProductRow row,string connString)
        //{
        //    int retid = -1;
        //    using (SqlConnection connection = new SqlConnection(connString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_InStock_ProductAndUpdate_T_Product_In]");


        //        #region 传参数
        //        //parameter[0].Value = row.OnlyID;
        //        parameter[0].Value = row.OnlyID;

        //        if (row.IsProductIDNull())
        //        {
        //            parameter[1].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[1].Value = row.ProductID;
        //        }
        //        if (row.IsInTimeNull())
        //        {
        //            parameter[2].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parameter[2].Value = row.InTime;
        //        }
        //        parameter[3].Value = row.VoucherID;
        //        parameter[4].Value = row.ProductOnlyID;

        //        #endregion


        //        try
        //        {
        //            MSSqlHelper.ExecuteNonQuery(connection,
        //                CommandType.StoredProcedure,
        //                "[Insert_T_InStock_ProductAndUpdate_T_Product_In]",
        //                parameter);

        //            retid = Convert.ToInt32(parameter[0].Value);

        //            OnSqlStateChange(new SqlStateEventArgs(true, "[[[Insert_T_InStock_ProductAndUpdate_T_Product_In]]] OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "[[[Insert_T_InStock_ProductAndUpdate_T_Product_In]]] Error:" + ex.ToString()));
        //        }
        //    }
        //    return retid;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="connStr">连接串为空时是默认的本地连接，有值时为指定连接</param>
        /// <returns></returns>
        public int Insert_T_InStock_Product(WMSDS.T_InStock_ProductRow row,string connStr)
        {
            int retid = -1;
            if (connStr == "")
                connStr = ConnctionString;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_InStock_ProductAndUpdate_T_Product_In]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;



                parameter[0].Value = -1;

                if (row.IsProductIDNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductID;
                }
                if (row.IsInTimeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.InTime;
                }
                parameter[3].Value = row.VoucherID;
                parameter[4].Value = row.ProductOnlyID;

                #endregion


                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Insert_T_InStock_ProductAndUpdate_T_Product_In]",
                        parameter);

                    retid = Convert.ToInt32(parameter[0].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[[[Insert_T_InStock_ProductAndUpdate_T_Product_In]]] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[[[Insert_T_InStock_ProductAndUpdate_T_Product_In]]] Error:" + ex.ToString()));
                }
            }
            return retid; 
        }
        /// <summary>
        /// 用事务保存本地入库单
        /// </summary>
        /// <param name="isRow"></param>
        /// <param name="siDS"></param>
        /// <returns></returns>
        public string Tran_Insert_StockIn(WMSDS.T_InStockRow isRow, InterfaceDS siDS)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    int inID = this.Insert_T_InStock(isRow,"");
                    //得到voucherid
                    if (inID < 0 || inID == 0)
                        return "入库单表头插入失败";
                    //2.保存分录
                    for (int i = 0; i < siDS.CT_StockIn.Rows.Count; i++)
                    {
                        WMSDS.T_InStock_EntryRow iseRow = new WMSDS().T_InStock_Entry.NewT_InStock_EntryRow();
                        iseRow.EntryID = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.EntryIDColumn].ToString());
                        iseRow.AuxCommitQty1 = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.AuxCommitQty1Column].ToString());
                        iseRow.BatchNO = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.BatchNOColumn].ToString();
                        iseRow.CommitQty = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CommitQtyColumn].ToString());
                        iseRow.CoreDiameter = Convert.ToInt16(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CoreDiameterColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CoreDiameterColumn].ToString());
                        iseRow.CustTrademark = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CustTrademarkColumn].ToString();
                        iseRow.FiberDirect = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.FiberDirectColumn].ToString();
                        iseRow.Grade = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.GradeColumn].ToString();
                        iseRow.IsWhiteFlag = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.IsWhiteFlagColumn].ToString();
                        iseRow.Layers = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.LayersColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.LayersColumn].ToString());
                        iseRow.Length_P = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Length_PColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Length_PColumn].ToString());
                        iseRow.MaterialCode = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.MaterialCodeColumn].ToString();
                        iseRow.OrderNO = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.OrderNOColumn].ToString();
                        iseRow.PaperCert = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.PaperCertColumn].ToString();
                        iseRow.ReamPackType = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.ReamPackTypeColumn].ToString();
                        iseRow.Reams = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.ReamsColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.ReamsColumn].ToString());
                        iseRow.SKU = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SKUColumn].ToString();
                        iseRow.SlidesOfReam = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfReamColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfReamColumn].ToString());
                        iseRow.SlidesOfSheet = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfSheetColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfSheetColumn].ToString());
                        iseRow.SpecCustName = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SpecCustNameColumn].ToString();
                        iseRow.SpecProdName = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SpecProdNameColumn].ToString();
                        iseRow.StockInDate = Convert.ToDateTime(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.StockInDateColumn].ToString());
                        iseRow.TrademarkStyle = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.TrademarkStyleColumn].ToString();
                        iseRow.TransportType = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.TransportTypeColumn].ToString();
                        iseRow.VoucherID = inID;
                        iseRow.WeightMode = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.WeightModeColumn].ToString();
                        iseRow.Width_P = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_PColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_PColumn].ToString());
                        iseRow.Width_R = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_RColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_RColumn].ToString());
                        iseRow.Diameter=Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.DiameterColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.DiameterColumn].ToString());
                        iseRow.RollLength = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.RollLengthColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.RollLengthColumn].ToString());
                        iseRow.Cdefine1 = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Cdefine1Column].ToString();
                        iseRow.Cdefine3 = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Cdefine3Column].ToString();
                        int ret = Insert_T_InStock_Entry(iseRow,"");
                        if (ret < 0 || ret == 0)
                            return "入库单"+iseRow.VoucherID+"分录"+iseRow.EntryID+"插入失败。";
                    }
                    //3.保存明细,同时更新productid关联
                    for (int j = 0; j < siDS.CT_StockInDetail.Rows.Count; j++)
                    {
                        WMSDS.T_InStock_ProductRow ispRow = new WMSDS().T_InStock_Product.NewT_InStock_ProductRow();
                        ispRow.InTime = Convert.ToDateTime(siDS.CT_StockInDetail.Rows[j][siDS.CT_StockInDetail.StockInDateColumn].ToString());
                        ispRow.ProductID = siDS.CT_StockInDetail.Rows[j][siDS.CT_StockInDetail.ProductIDColumn].ToString();
                        ispRow.ProductOnlyID = Convert.ToInt32(siDS.CT_StockInDetail.Rows[j][siDS.CT_StockInDetail.OnlyIDColumn].ToString());
                        ispRow.VoucherID = inID;
                        int ret = Insert_T_InStock_Product(ispRow,"");
                        if(ret<0||ret==0)
                            return "入库单" + ispRow.VoucherID + "明细" + ispRow.ProductOnlyID + "插入失败。";

                    }
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }

        }

        public string Get_T_StockIn_NewFlow(string type, string machineid)
        {
            string voucherNO = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Get_T_StockIn_NewFlow]");

                #region 传参数

                parameter[0].Value = type + machineid;
                parameter[1].Value = "";
                parameter[1].Direction = ParameterDirection.Output;


                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Get_T_StockIn_NewFlow",
                        parameter
                        );
                    voucherNO = parameter[1].Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "Get_T_StockIn_NewFlow OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Get_T_StockIn_NewFlow Error:" + ex.ToString()));
                }
            }
            return voucherNO;
        }
        /// <summary>
        /// 通过单号和ID一起查入库单的抬头和明细
        /// </summary>
        /// <param name="VoucherNO">单号或者id</param>
        /// <returns>抬头表和明细表</returns>
        public WMSDS Select_T_InStockByVoucherNO(string VoucherNO)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_InStockByVoucherNO]");

                #region 传参数

                parameter[0].Value = VoucherNO;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_InStockByVoucherNO]",
                        ds,
                      new string[] { "T_InStock","T_InStock_Entry" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Select_T_InStockByVoucherNO] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Select_T_InStockByVoucherNO] Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        /// <summary>
        /// 通过单号和ID一起查入库单的抬头和明细
        /// </summary>
        /// <param name="VoucherNO">单号或者id</param>
        /// <returns>抬头表和明细表</returns>
        public WMSDS Select_T_InStockByVoucherNO(string VoucherNO, string conStr)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_InStockByVoucherNO]");

                #region 传参数

                parameter[0].Value = VoucherNO;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_InStockByVoucherNO]",
                        ds,
                      new string[] { "T_InStock", "T_InStock_Entry" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Select_T_InStockByVoucherNO] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Select_T_InStockByVoucherNO] Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public string Tran_Insert_CT_StockIn(WMSDS.T_InStockDataTable t_InStockDataTable, WMSDS.T_InStock_EntryDataTable t_InStock_EntryDataTable)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    InterfaceDS.CT_StockInRow siRow = new InterfaceDS().CT_StockIn.NewCT_StockInRow();

                    for (int i = 0; i < t_InStock_EntryDataTable.Rows.Count; i++)
                    {
                        siRow.VoucherNO = t_InStockDataTable.Rows[0][t_InStockDataTable.VoucherNOColumn].ToString();
                        siRow.BusinessType = t_InStockDataTable.Rows[0][t_InStockDataTable.BusinessTypeColumn].ToString();
                        siRow.BillType = t_InStockDataTable.Rows[0][t_InStockDataTable.BillTypeColumn].ToString();
                        siRow.FactoryID = t_InStockDataTable.Rows[0][t_InStockDataTable.FactoryIDColumn].ToString();
                        siRow.WHCode = t_InStockDataTable.Rows[0][t_InStockDataTable.WHCodeColumn].ToString();
                        siRow.Keeper = t_InStockDataTable.Rows[0][t_InStockDataTable.KeeperColumn].ToString();
                        siRow.SourceVoucherNO = t_InStockDataTable.Rows[0][t_InStockDataTable.SourceVoucherNOColumn].ToString();
                        siRow.BillRemark = t_InStockDataTable.Rows[0][t_InStockDataTable.BillRemarkColumn].ToString();
                        //
                        siRow.AuxCommitQty1 = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.AuxCommitQty1Column].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.AuxCommitQty1Column].ToString());
                        siRow.AuxCommitQty2 = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.AuxCommitQty2Column].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.AuxCommitQty2Column].ToString());
                        siRow.BatchNO = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.BatchNOColumn].ToString();
                        siRow.Cdefine1 = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Cdefine1Column].ToString();
                        siRow.Cdefine2 = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Cdefine2Column].ToString();
                        siRow.Cdefine3 = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Cdefine3Column].ToString();
                        siRow.CommitQty = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.CommitQtyColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.CommitQtyColumn].ToString());
                        siRow.CoreDiameter = Convert.ToInt16(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.CoreDiameterColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.CoreDiameterColumn].ToString());
                        siRow.CustTrademark = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.CustTrademarkColumn].ToString();
                        siRow.Diameter = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.DiameterColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.DiameterColumn].ToString());
                        siRow.EntryID = Convert.ToInt32(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.EntryIDColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.EntryIDColumn].ToString());
                        siRow.FiberDirect = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.FiberDirectColumn].ToString();
                        siRow.Grade = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.GradeColumn].ToString();
                        //siRow.InspurVoucherNO=t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.].ToString();
                        siRow.IsWhiteFlag = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.IsWhiteFlagColumn].ToString();
                        siRow.Layers = Convert.ToInt32(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.LayersColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.LayersColumn].ToString());
                        siRow.Length_P = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Length_PColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Length_PColumn].ToString());
                        siRow.MaterialCode = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.MaterialCodeColumn].ToString();
                        siRow.OrderNO = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.OrderNOColumn].ToString();
                        siRow.PaperCert = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.PaperCertColumn].ToString();
                        siRow.ReamPackType = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.ReamPackTypeColumn].ToString();
                        siRow.Reams = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.ReamsColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.ReamsColumn].ToString());
                        siRow.RollLength = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.RollLengthColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.RollLengthColumn].ToString());
                        siRow.SKU = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.SKUColumn].ToString();
                        siRow.SlidesOfReam = Convert.ToInt32(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.SlidesOfReamColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.SlidesOfReamColumn].ToString());
                        siRow.SlidesOfSheet = Convert.ToInt32(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.SlidesOfSheetColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.SlidesOfSheetColumn].ToString());
                        siRow.SpecCustName = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.SpecCustNameColumn].ToString();
                        siRow.SpecProdName = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.SpecProdNameColumn].ToString();
                        siRow.StockInDate = Convert.ToDateTime(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.StockInDateColumn].ToString());
                        siRow.TrademarkStyle = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.TrademarkStyleColumn].ToString();
                        siRow.TransportType = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.TransportTypeColumn].ToString();
                        siRow.Udefine1 = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Udefine1Column].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Udefine1Column].ToString());
                        siRow.Udefine2 = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Udefine2Column].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Udefine2Column].ToString());
                        siRow.Udefine3 = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Udefine3Column].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Udefine3Column].ToString());
                        siRow.WeightMode = t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.WeightModeColumn].ToString();
                        //计重模式为空，则判断为平板（二楼会不会不选计重模式呢？）
                        //if (siRow.WeightMode == ""&& siRow.CoreDiameter == 0)
                        //{
                        //    siRow.WeightMode = siRow.FiberDirect == "" ? "正丝" : "反丝";
                        //}
                        siRow.Width_P = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Width_PColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Width_PColumn].ToString());
                        siRow.Width_R = Convert.ToDecimal(t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Width_RColumn].ToString() == "" ? "0" : t_InStock_EntryDataTable.Rows[i][t_InStock_EntryDataTable.Width_RColumn].ToString());

                        siRow.Flag = "0";

                        int retid = -1;
                        using (SqlConnection connection = new SqlConnection(ConnctionString))
                        {
                            SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Insert_CT_StockIn");


                            #region 传参数
                            //parameter[0].Value = row.OnlyID;
                            if (siRow.IsVoucherNONull())
                            {
                                parameter[0].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[0].Value = siRow.VoucherNO;
                            }
                            if (siRow.IsBusinessTypeNull())
                            {
                                parameter[1].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[1].Value = siRow.BusinessType;
                            }
                            if (siRow.IsBillTypeNull())
                            {
                                parameter[2].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[2].Value = siRow.BillType;
                            }
                            if (siRow.IsFactoryIDNull())
                            {
                                parameter[3].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[3].Value = siRow.FactoryID;
                            }
                            if (siRow.IsWHCodeNull())
                            {
                                parameter[4].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[4].Value = siRow.WHCode;
                            }
                            if (siRow.IsKeeperNull())
                            {
                                parameter[5].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[5].Value = siRow.Keeper;
                            }
                            if (siRow.IsSourceVoucherNONull())
                            {
                                parameter[6].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[6].Value = siRow.SourceVoucherNO;
                            }
                            if (siRow.IsBillRemarkNull())
                            {
                                parameter[7].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[7].Value = siRow.BillRemark;
                            }
                            if (siRow.IsEntryIDNull())
                            {
                                parameter[8].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[8].Value = siRow.EntryID;
                            }
                            if (siRow.IsMaterialCodeNull())
                            {
                                parameter[9].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[9].Value = siRow.MaterialCode;
                            }
                            if (siRow.IsGradeNull())
                            {
                                parameter[10].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[10].Value = siRow.Grade;
                            }
                            if (siRow.IsWidth_RNull())
                            {
                                parameter[11].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[11].Value = siRow.Width_R;
                            }
                            if (siRow.IsWeightModeNull())
                            {
                                parameter[12].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[12].Value = siRow.WeightMode;
                            }
                            if (siRow.IsCoreDiameterNull())
                            {
                                parameter[13].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[13].Value = siRow.CoreDiameter;
                            }
                            if (siRow.IsLength_PNull())
                            {
                                parameter[14].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[14].Value = siRow.Length_P;
                            }
                            if (siRow.IsWidth_PNull())
                            {
                                parameter[15].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[15].Value = siRow.Width_P;
                            }
                            if (siRow.IsReamsNull())
                            {
                                parameter[16].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[16].Value = siRow.Reams;
                            }
                            if (siRow.IsSlidesOfReamNull())
                            {
                                parameter[17].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[17].Value = siRow.SlidesOfReam;
                            }
                            if (siRow.IsSlidesOfSheetNull())
                            {
                                parameter[18].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[18].Value = siRow.SlidesOfSheet;
                            }
                            if (siRow.IsTransportTypeNull())
                            {
                                parameter[19].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[19].Value = siRow.TransportType;
                            }
                            if (siRow.IsReamPackTypeNull())
                            {
                                parameter[20].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[20].Value = siRow.ReamPackType;
                            }
                            if (siRow.IsFiberDirectNull())
                            {
                                parameter[21].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[21].Value = siRow.FiberDirect;
                            }
                            if (siRow.IsDiameterNull())
                            {
                                parameter[22].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[22].Value = siRow.Diameter;
                            }
                            if (siRow.IsRollLengthNull())
                            {
                                parameter[23].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[23].Value = siRow.RollLength;
                            }
                            if (siRow.IsLayersNull())
                            {
                                parameter[24].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[24].Value = siRow.Layers;
                            }
                            if (siRow.IsSKUNull())
                            {
                                parameter[25].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[25].Value = siRow.SKU;
                            }
                            if (siRow.IsCommitQtyNull())
                            {
                                parameter[26].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[26].Value = siRow.CommitQty;
                            }
                            if (siRow.IsAuxCommitQty1Null())
                            {
                                parameter[27].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[27].Value = siRow.AuxCommitQty1;
                            }
                            if (siRow.IsAuxCommitQty2Null())
                            {
                                parameter[28].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[28].Value = siRow.AuxCommitQty2;
                            }
                            if (siRow.IsCustTrademarkNull())
                            {
                                parameter[29].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[29].Value = siRow.CustTrademark;
                            }
                            if (siRow.IsPaperCertNull())
                            {
                                parameter[30].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[30].Value = siRow.PaperCert;
                            }
                            if (siRow.IsSpecProdNameNull())
                            {
                                parameter[31].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[31].Value = siRow.SpecProdName;
                            }
                            if (siRow.IsSpecCustNameNull())
                            {
                                parameter[32].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[32].Value = siRow.SpecCustName;
                            }
                            if (siRow.IsTrademarkStyleNull())
                            {
                                parameter[33].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[33].Value = siRow.TrademarkStyle;
                            }
                            if (siRow.IsIsWhiteFlagNull())
                            {
                                parameter[34].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[34].Value = siRow.IsWhiteFlag;
                            }
                            if (siRow.IsOrderNONull())
                            {
                                parameter[35].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[35].Value = siRow.OrderNO;
                            }
                            if (siRow.IsStockInDateNull())
                            {
                                parameter[36].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[36].Value = siRow.StockInDate;
                            }
                            if (siRow.IsCdefine1Null())
                            {
                                parameter[37].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[37].Value = siRow.Cdefine1;
                            }
                            if (siRow.IsCdefine2Null())
                            {
                                parameter[38].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[38].Value = siRow.Cdefine2;
                            }
                            if (siRow.IsCdefine3Null())
                            {
                                parameter[39].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[39].Value = siRow.Cdefine3;
                            }
                            if (siRow.IsUdefine1Null())
                            {
                                parameter[40].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[40].Value = siRow.Udefine1;
                            }
                            if (siRow.IsUdefine2Null())
                            {
                                parameter[41].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[41].Value = siRow.Udefine2;
                            }
                            if (siRow.IsUdefine3Null())
                            {
                                parameter[42].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[42].Value = siRow.Udefine3;
                            }
                            if (siRow.IsWriteDateNull())
                            {
                                parameter[43].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[43].Value = siRow.WriteDate;
                            }
                            if (siRow.IsReadDateNull())
                            {
                                parameter[44].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[44].Value = siRow.ReadDate;
                            }
                            if (siRow.IsInspurVoucherNONull())
                            {
                                parameter[45].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[45].Value = siRow.InspurVoucherNO;
                            }
                            if (siRow.IsBatchNONull())
                            {
                                parameter[46].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[46].Value = siRow.BatchNO;
                            }
                            if (siRow.IsFlagNull())
                            {
                                parameter[47].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[47].Value = siRow.Flag;
                            }
                            if (siRow.IsErrorMsgNull())
                            {
                                parameter[48].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[48].Value = siRow.ErrorMsg;
                            }
                            if (siRow.IsIsReadNull())
                            {
                                parameter[49].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[49].Value = siRow.IsRead;
                            }
                            if (siRow.IsIsWriteNull())
                            {
                                parameter[50].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[50].Value = siRow.IsWrite;
                            }
                            if (siRow.IsLastActionTimeNull())
                            {
                                parameter[51].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[51].Value = siRow.LastActionTime;
                            }
                            if (siRow.IsConnectionIDNull())
                            {
                                parameter[52].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[52].Value = siRow.ConnectionID;
                            }
                            if (siRow.IsOptimisticLockFieldNull())
                            {
                                parameter[53].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[53].Value = siRow.OptimisticLockField;
                            }

                            #endregion

                            parameter[54].Value = siRow.IsOnlyIDNull()?0:siRow.OnlyID ;

                            try
                            {
                                MSSqlHelper.ExecuteNonQuery(connection,
                                    CommandType.StoredProcedure,
                                    "[Insert_CT_StockIn]",
                                    parameter);

                                retid = Convert.ToInt32(parameter[54].Value);

                                OnSqlStateChange(new SqlStateEventArgs(true, "[Insert_CT_StockIn] OK"));
                            }
                            catch (Exception ex)
                            {
                                OnSqlStateChange(new SqlStateEventArgs(false, "[Insert_CT_StockIn] Error:" + ex.ToString()));
                                retid = -1;
                            }
                        }
                        if (retid < 0 || retid == 0)
                            return "插入入库接口失败";
                    }
                    //更新本地单据的isupload
                    WMSDS.T_InStockRow isRow = new WMSDS().T_InStock.NewT_InStockRow();
                    isRow.VoucherNO = t_InStockDataTable.Rows[0][t_InStockDataTable.VoucherNOColumn].ToString();
                    isRow.OnlyID = Convert.ToInt32(t_InStockDataTable.Rows[0][t_InStockDataTable.OnlyIDColumn].ToString());

                    isRow.UploadDate = DateTime.Now;
                    isRow.IsUpload = "1";
                    isRow.IsClose = "1";
                   string ss= this.Update_T_InStock(isRow);
                   if (ss != "")
                       return "更新上传标记失败："+ss;
                        ts.Complete();
                        return "";
                    
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        public string Update_T_InStock(WMSDS.T_InStockRow row)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_InStock]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;
                parameter[0].Value = row.OnlyID;

                if (row.IsBusinessTypeNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.BusinessType;
                }
                if (row.IsBillTypeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BillType;
                }
                if (row.IsFactoryIDNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.FactoryID;
                }
                if (row.IsWHCodeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.WHCode;
                }
                if (row.IsKeeperNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.Keeper;
                }
                if (row.IsSourceVoucherNONull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.SourceVoucherNO;
                }
                if (row.IsBillRemarkNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.BillRemark;
                }
                if (row.IsBillerNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Biller;
                }
                if (row.IsCheckerNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.Checker;
                }
                if (row.IsIsCheckNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.IsCheck;
                }
                if (row.IsIsCancelNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.IsCancel;
                }
                if (row.IsIsCloseNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.IsClose;
                }
                if (row.IsBillDateNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BillDate;
                }
                if (row.IsUploadDateNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.UploadDate;
                }
                if (row.IsIsUploadNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.IsUpload;
                }
                parameter[16].Value = row.VoucherNO;

                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Update_T_InStock]",
                        parameter);

                    //retid = Convert.ToInt32(parameter[72].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[[Update_T_InStock]] OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[[Update_T_InStock]] Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }
        /// <summary>
        /// 事务插入出库接口
        /// </summary>
        /// <param name="planDS"></param>
        /// <returns></returns>
        public string Tran_Insert_CT_StockOut(InterfaceDS planDS)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //InterfaceDS.CT_StockInRow siRow = new InterfaceDS().CT_StockIn.NewCT_StockInRow();


                    for (int i = 0; i < planDS.CT_StockOut.Rows.Count; i++)
                    {

                        InterfaceDS.CT_StockOutRow soRow = new InterfaceDS().CT_StockOut.NewCT_StockOutRow();
                        soRow.VoucherNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.VoucherNOColumn].ToString();
                        soRow.SourceVoucherNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SourceVoucherNOColumn].ToString();
                        soRow.BusinessType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.BusinessTypeColumn].ToString();
                        soRow.BillType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.BillTypeColumn].ToString();
                        soRow.FactoryID = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.FactoryIDColumn].ToString();
                        soRow.WHCodeFrom = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.WHCodeFromColumn].ToString();
                        soRow.WHCodeTo = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.WHCodeToColumn].ToString();
                        soRow.TradeType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TradeTypeColumn].ToString();
                        soRow.DeptName = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.DeptNameColumn].ToString();
                        soRow.TransportType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TransportTypeColumn].ToString();
                        soRow.Keeper = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.KeeperColumn].ToString();
                        soRow.ContainerNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerNOColumn].ToString();
                        soRow.VehicleNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.VehicleNOColumn].ToString();
                        //
                        soRow.EntryID = Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.EntryIDColumn].ToString());
                        soRow.SourceEntryID =planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SourceEntryIDColumn].ToString()==""?0: Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SourceEntryIDColumn].ToString());
                        soRow.MaterialCode = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.MaterialCodeColumn].ToString();
                        soRow.Grade = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.GradeColumn].ToString();
                        soRow.Width_R = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_RColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_RColumn].ToString());
                        soRow.WeightMode = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.WeightModeColumn].ToString();
                        soRow.CoreDiameter = Convert.ToInt16(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CoreDiameterColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CoreDiameterColumn].ToString());

                        soRow.Length_P = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Length_PColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Length_PColumn].ToString());
                        soRow.Width_P = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_PColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_PColumn].ToString());
                        
                        soRow.Reams = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ReamsColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ReamsColumn].ToString());
                        soRow.SlidesOfReam = Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfReamColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfReamColumn].ToString());
                        soRow.SlidesOfSheet = Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfSheetColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfSheetColumn].ToString());
                        soRow.ReamPackType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ReamPackTypeColumn].ToString();
                        soRow.SKU = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SKUColumn].ToString();
                        soRow.CustTrademark = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CustTrademarkColumn].ToString();
                        soRow.PaperCert = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.PaperCertColumn].ToString();
                        soRow.SpecProdName = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SpecProdNameColumn].ToString();
                        soRow.SpecCustName = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SpecCustNameColumn].ToString();
                        soRow.TrademarkStyle = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TrademarkStyleColumn].ToString();
                        soRow.IsWhiteFlag = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.IsWhiteFlagColumn].ToString();
                        soRow.OrderNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.OrderNOColumn].ToString();
                        //if (planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TradeTypeColumn].ToString() == "内贸")//zjg add
                        //{
                        //    soRow.ContainerBulk = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString());
                        //}
                        soRow.ContainerBulk = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString());
                        soRow.SpecialCustomer = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SpecialCustomerColumn].ToString();
                        soRow.Diameter = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.DiameterColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.DiameterColumn].ToString());
                        soRow.RollLength = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.RollLengthColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.RollLengthColumn].ToString());
                        soRow.BatchNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.BatchNOColumn].ToString();
                        soRow.AuxCommitQty1 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty1Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty1Column].ToString());
                        soRow.CommitQty = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CommitQtyColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CommitQtyColumn].ToString());
                        soRow.AuxCommitQty2 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty2Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty2Column].ToString());
                        soRow.Cdefine1 = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Cdefine1Column].ToString();
                        soRow.Cdefine2 = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Cdefine2Column].ToString();
                        soRow.Cdefine3 = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Cdefine3Column].ToString();
                        soRow.Udefine1 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine1Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine1Column].ToString());
                        soRow.Udefine2 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine2Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine2Column].ToString());
                        soRow.Udefine3 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine3Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine3Column].ToString());
                        soRow.WriteDate = DateTime.Now;

                        //内贸时，毛重和体积上传为0
                        if (planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TradeTypeColumn].ToString() == "内贸")//zjg add
                        {
                            soRow.ContainerBulk = 0;//体积
                            soRow.AuxCommitQty2 = 0;//毛重
                        
                        }

                        soRow.Flag = "0";
                        int retid = -1;
                        using (SqlConnection connection = new SqlConnection(ConnctionString))
                        {
                            SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_CT_StockOut]");


                            #region 传参数
                            //parameter[0].Value = row.OnlyID;
                            if (soRow.IsVoucherNONull())
                            {
                                parameter[0].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[0].Value = soRow.VoucherNO;
                            }
                            if (soRow.IsSourceVoucherNONull())
                            {
                                parameter[1].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[1].Value = soRow.SourceVoucherNO;
                            }
                            if (soRow.IsBusinessTypeNull())
                            {
                                parameter[2].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[2].Value = soRow.BusinessType;
                            }
                            if (soRow.IsBillTypeNull())
                            {
                                parameter[3].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[3].Value = soRow.BillType;
                            }
                            if (soRow.IsFactoryIDNull())
                            {
                                parameter[4].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[4].Value = soRow.FactoryID;
                            }
                            if (soRow.IsWHCodeFromNull())
                            {
                                parameter[5].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[5].Value = soRow.WHCodeFrom;
                            }
                            if (soRow.IsWHCodeToNull())
                            {
                                parameter[6].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[6].Value = soRow.WHCodeTo;
                            }
                            if (soRow.IsTradeTypeNull())
                            {
                                parameter[7].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[7].Value = soRow.TradeType;
                            }
                            if (soRow.IsDeptNameNull())
                            {
                                parameter[8].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[8].Value = soRow.DeptName;
                            }
                            if (soRow.IsTransportTypeNull())
                            {
                                parameter[9].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[9].Value = soRow.TransportType;
                            }
                            if (soRow.IsKeeperNull())
                            {
                                parameter[10].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[10].Value = soRow.Keeper;
                            }
                            if (soRow.IsContainerNONull())
                            {
                                parameter[11].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[11].Value = soRow.ContainerNO;
                            }
                            if (soRow.IsVehicleNONull())
                            {
                                parameter[12].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[12].Value = soRow.VehicleNO;
                            }
                            if (soRow.IsEntryIDNull())
                            {
                                parameter[13].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[13].Value = soRow.EntryID;
                            }
                            if (soRow.IsSourceEntryIDNull())
                            {
                                parameter[14].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[14].Value = soRow.SourceEntryID;
                            }
                            if (soRow.IsMaterialCodeNull())
                            {
                                parameter[15].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[15].Value = soRow.MaterialCode;
                            }
                            if (soRow.IsGradeNull())
                            {
                                parameter[16].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[16].Value = soRow.Grade;
                            }
                            if (soRow.IsWidth_RNull())
                            {
                                parameter[17].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[17].Value = soRow.Width_R;
                            }
                            if (soRow.IsWeightModeNull())
                            {
                                parameter[18].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[18].Value = soRow.WeightMode;
                            }
                            if (soRow.IsCoreDiameterNull())
                            {
                                parameter[19].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[19].Value = soRow.CoreDiameter;
                            }
                            if (soRow.IsLength_PNull())
                            {
                                parameter[20].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[20].Value = soRow.Length_P;
                            }
                            if (soRow.IsWidth_PNull())
                            {
                                parameter[21].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[21].Value = soRow.Width_P;
                            }
                            if (soRow.IsReamsNull())
                            {
                                parameter[22].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[22].Value = soRow.Reams;
                            }
                            if (soRow.IsSlidesOfReamNull())
                            {
                                parameter[23].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[23].Value = soRow.SlidesOfReam;
                            }
                            if (soRow.IsSlidesOfSheetNull())
                            {
                                parameter[24].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[24].Value = soRow.SlidesOfSheet;
                            }
                            if (soRow.IsReamPackTypeNull())
                            {
                                parameter[25].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[25].Value = soRow.ReamPackType;
                            }
                            if (soRow.IsSKUNull())
                            {
                                parameter[26].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[26].Value = soRow.SKU;
                            }
                            if (soRow.IsCustTrademarkNull())
                            {
                                parameter[27].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[27].Value = soRow.CustTrademark;
                            }
                            if (soRow.IsPaperCertNull())
                            {
                                parameter[28].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[28].Value = soRow.PaperCert;
                            }
                            if (soRow.IsSpecProdNameNull())
                            {
                                parameter[29].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[29].Value = soRow.SpecProdName;
                            }
                            if (soRow.IsSpecCustNameNull())
                            {
                                parameter[30].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[30].Value = soRow.SpecCustName;
                            }
                            if (soRow.IsTrademarkStyleNull())
                            {
                                parameter[31].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[31].Value = soRow.TrademarkStyle;
                            }
                            if (soRow.IsIsWhiteFlagNull())
                            {
                                parameter[32].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[32].Value = soRow.IsWhiteFlag;
                            }
                            if (soRow.IsOrderNONull())
                            {
                                parameter[33].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[33].Value = soRow.OrderNO;
                            }
                            if (soRow.IsContainerBulkNull())
                            {
                                parameter[34].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[34].Value = soRow.ContainerBulk;
                            }
                            if (soRow.IsSpecialCustomerNull())
                            {
                                parameter[35].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[35].Value = soRow.SpecialCustomer;
                            }
                            if (soRow.IsDiameterNull())
                            {
                                parameter[36].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[36].Value = soRow.Diameter;
                            }
                            if (soRow.IsRollLengthNull())
                            {
                                parameter[37].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[37].Value = soRow.RollLength;
                            }
                            if (soRow.IsBatchNONull())
                            {
                                parameter[38].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[38].Value = soRow.BatchNO;
                            }
                            if (soRow.IsCommitQtyNull())
                            {
                                parameter[39].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[39].Value = soRow.CommitQty;
                            }
                            if (soRow.IsAuxCommitQty1Null())
                            {
                                parameter[40].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[40].Value = soRow.AuxCommitQty1;
                            }
                            if (soRow.IsAuxCommitQty2Null())
                            {
                                parameter[41].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[41].Value = soRow.AuxCommitQty2;
                            }
                            if (soRow.IsCdefine1Null())
                            {
                                parameter[42].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[42].Value = soRow.Cdefine1;
                            }
                            if (soRow.IsCdefine2Null())
                            {
                                parameter[43].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[43].Value = soRow.Cdefine2;
                            }
                            if (soRow.IsCdefine3Null())
                            {
                                parameter[44].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[44].Value = soRow.Cdefine3;
                            }
                            if (soRow.IsUdefine1Null())
                            {
                                parameter[45].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[45].Value = soRow.Udefine1;
                            }
                            if (soRow.IsUdefine2Null())
                            {
                                parameter[46].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[46].Value = soRow.Udefine2;
                            }
                            if (soRow.IsUdefine3Null())
                            {
                                parameter[47].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[47].Value = soRow.Udefine3;
                            }
                            if (soRow.IsWriteDateNull())
                            {
                                parameter[48].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[48].Value = soRow.WriteDate;
                            }
                            if (soRow.IsReadDateNull())
                            {
                                parameter[49].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[49].Value = soRow.ReadDate;
                            }
                            if (soRow.IsFlagNull())
                            {
                                parameter[50].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[50].Value = soRow.Flag;
                            }

                            if (soRow.IsInspurVoucherNONull())
                            {
                                parameter[51].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[51].Value = soRow.InspurVoucherNO;
                            }
                            if (soRow.IsErrorMsgNull())
                            {
                                parameter[52].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[52].Value = soRow.ErrorMsg;
                            }
                            if (soRow.IsIsReadNull())
                            {
                                parameter[53].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[53].Value = soRow.IsRead;
                            }
                            if (soRow.IsIsWriteNull())
                            {
                                parameter[54].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[54].Value = soRow.IsWrite;
                            }
                            if (soRow.IsLastActionTimeNull())
                            {
                                parameter[55].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[55].Value = soRow.LastActionTime;
                            }
                            if (soRow.IsConnectionIDNull())
                            {
                                parameter[56].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[56].Value = soRow.ConnectionID;
                            }
                            if (soRow.IsOptimisticLockFieldNull())
                            {
                                parameter[57].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[57].Value = soRow.OptimisticLockField;
                            }
                            parameter[58].Value = soRow.IsOnlyIDNull() ? 0 : soRow.OnlyID;

                            #endregion

                            try
                            {
                                MSSqlHelper.ExecuteNonQuery(connection,
                                    CommandType.StoredProcedure,
                                    "[Insert_CT_StockOut]",
                                    parameter);

                                retid = Convert.ToInt32(parameter[58].Value);

                                OnSqlStateChange(new SqlStateEventArgs(true, "[[Insert_CT_StockOut]] OK"));
                            }
                            catch (Exception ex)
                            {
                                OnSqlStateChange(new SqlStateEventArgs(false, "[[Insert_CT_StockOut]] Error:" + ex.ToString()));
                                retid = -1;
                            }
                        }
                        if (retid < 0 || retid == 0)
                            return "插入出库接口失败";
                    }


                    //更新本地单据的isupload
                    //WMSDS.T_InStockRow isRow = new WMSDS().T_InStock.NewT_InStockRow();
                    //isRow.VoucherNO = t_InStockDataTable.Rows[0][t_InStockDataTable.VoucherNOColumn].ToString();
                    //isRow.OnlyID = Convert.ToInt32(t_InStockDataTable.Rows[0][t_InStockDataTable.OnlyIDColumn].ToString());

                    //isRow.UploadDate = DateTime.Now;
                    //isRow.IsUpload = "1";
                    //string ss = this.Update_T_InStock(isRow);
                    //if (ss != "")
                    //    return "更新上传标记失败：" + ss;




                    ts.Complete();
                    return "";

                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        public string Tran_Insert_ProductGroup(InterfaceDS planDS)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    for (int i = 0; i < planDS.CT_StockOut.Rows.Count; i++)
                    {
                        InterfaceDS.CT_StockOutRow soRow = new InterfaceDS().CT_StockOut.NewCT_StockOutRow();
                        soRow.VoucherNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.VoucherNOColumn].ToString();
                        //soRow.SourceVoucherNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SourceVoucherNOColumn].ToString();
                        //soRow.BusinessType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.BusinessTypeColumn].ToString();
                        //soRow.BillType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.BillTypeColumn].ToString();
                        //soRow.FactoryID = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.FactoryIDColumn].ToString();
                        //soRow.WHCodeFrom = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.WHCodeFromColumn].ToString();
                        //soRow.WHCodeTo = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.WHCodeToColumn].ToString();
                        //soRow.TradeType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TradeTypeColumn].ToString();
                        //soRow.DeptName = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.DeptNameColumn].ToString();
                        //soRow.TransportType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TransportTypeColumn].ToString();
                        //soRow.Keeper = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.KeeperColumn].ToString();
                        //soRow.ContainerNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerNOColumn].ToString();
                        soRow.VehicleNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.VehicleNOColumn].ToString();//车号
                        //
                        soRow.EntryID = Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.EntryIDColumn].ToString());
                        //soRow.SourceEntryID = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SourceEntryIDColumn].ToString() == "" ? 0 : Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SourceEntryIDColumn].ToString());
                        soRow.MaterialCode = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.MaterialCodeColumn].ToString();
                        soRow.Grade = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.GradeColumn].ToString();
                        soRow.Width_R = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_RColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_RColumn].ToString());
                        soRow.WeightMode = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.WeightModeColumn].ToString();
                        soRow.CoreDiameter = Convert.ToInt16(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CoreDiameterColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CoreDiameterColumn].ToString());

                        soRow.Length_P = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Length_PColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Length_PColumn].ToString());
                        soRow.Width_P = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_PColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Width_PColumn].ToString());

                        soRow.Reams = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ReamsColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ReamsColumn].ToString());
                        soRow.SlidesOfReam = Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfReamColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfReamColumn].ToString());
                        soRow.SlidesOfSheet = Convert.ToInt32(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfSheetColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SlidesOfSheetColumn].ToString());
                        soRow.ReamPackType = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ReamPackTypeColumn].ToString();
                        //soRow.SKU = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SKUColumn].ToString();
                        soRow.CustTrademark = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CustTrademarkColumn].ToString();
                        soRow.PaperCert = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.PaperCertColumn].ToString();
                        soRow.SpecProdName = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SpecProdNameColumn].ToString();
                        soRow.SpecCustName = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SpecCustNameColumn].ToString();
                        soRow.TrademarkStyle = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TrademarkStyleColumn].ToString();
                        soRow.IsWhiteFlag = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.IsWhiteFlagColumn].ToString();
                        soRow.OrderNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.OrderNOColumn].ToString();
                        //if (planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.TradeTypeColumn].ToString() == "内贸")//zjg add
                        //{
                        //    soRow.ContainerBulk = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString());
                        //}
                        //soRow.ContainerBulk = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.ContainerBulkColumn].ToString());
                        soRow.SpecialCustomer = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.SpecialCustomerColumn].ToString();
                        soRow.Diameter = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.DiameterColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.DiameterColumn].ToString());
                        soRow.RollLength = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.RollLengthColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.RollLengthColumn].ToString());
                        //soRow.BatchNO = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.BatchNOColumn].ToString();
                        //计划重量
                        soRow.AuxCommitQty1 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i]["PlanAuxQty1"].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i]["PlanAuxQty1"].ToString());
                        //soRow.AuxCommitQty1 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty1Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty1Column].ToString());
                        //计划件数
                        soRow.CommitQty = Convert.ToDecimal(planDS.CT_StockOut.Rows[i]["PlanQty"].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i]["PlanQty"].ToString());
                        //soRow.CommitQty = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CommitQtyColumn].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.CommitQtyColumn].ToString());
                        //soRow.AuxCommitQty2 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty2Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.AuxCommitQty2Column].ToString());
                        soRow.Cdefine1 = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Cdefine1Column].ToString();
                        soRow.Cdefine2 = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Cdefine2Column].ToString();
                        soRow.Cdefine3 = planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Cdefine3Column].ToString();
                        soRow.Udefine1 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine1Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine1Column].ToString());
                        soRow.Udefine2 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine2Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine2Column].ToString());
                        soRow.Udefine3 = Convert.ToDecimal(planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine3Column].ToString() == "" ? "0" : planDS.CT_StockOut.Rows[i][planDS.CT_StockOut.Udefine3Column].ToString());
                        soRow.WriteDate = DateTime.Now;


                        soRow.Flag = "0";
                        int retid = -1;
                        using (SqlConnection connection = new SqlConnection(ConnctionString))
                        {
                            SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_ProductGroup]");


                            #region 传参数
                            //parameter[0].Value = row.OnlyID;

                            if (soRow.IsMaterialCodeNull())
                            {
                                parameter[0].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[0].Value = soRow.MaterialCode;
                            }
                            if (soRow.IsWidth_RNull())
                            {
                                parameter[1].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[1].Value = soRow.Width_R;
                            }
                            if (soRow.IsGradeNull())
                            {
                                parameter[2].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[2].Value = soRow.Grade;
                            }
                            if (soRow.IsPaperCertNull())
                            {
                                parameter[3].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[3].Value = soRow.PaperCert;
                            }
                            if (soRow.IsReamPackTypeNull())
                            {
                                parameter[4].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[4].Value = soRow.ReamPackType;
                            }
                            if (soRow.IsTrademarkStyleNull())
                            {
                                parameter[5].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[5].Value = soRow.TrademarkStyle;
                            }
                            if (soRow.IsCoreDiameterNull())
                            {
                                parameter[6].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[6].Value = soRow.CoreDiameter;
                            }
                            if (soRow.IsWeightModeNull())
                            {
                                parameter[7].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[7].Value = soRow.WeightMode;
                            }
                            if (soRow.IsSpecCustNameNull())
                            {
                                parameter[8].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[8].Value = soRow.SpecCustName;
                            }
                            if (soRow.IsSpecProdNameNull())
                            {
                                parameter[9].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[9].Value = soRow.SpecProdName;
                            }
                            if (soRow.IsDiameterNull())
                            {
                                parameter[10].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[10].Value = soRow.Diameter;
                            }
                            if (soRow.IsRollLengthNull())
                            {
                                parameter[11].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[11].Value = soRow.RollLength;
                            }
                            if (soRow.IsIsWhiteFlagNull())
                            {
                                parameter[12].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[12].Value = soRow.IsWhiteFlag;
                            }
                            if (soRow.IsOrderNONull())
                            {
                                parameter[13].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[13].Value = soRow.OrderNO;
                            }
                            if (soRow.IsSpecCustNameNull())
                            {
                                parameter[14].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[14].Value = soRow.SpecialCustomer;
                            }
                            if (soRow.IsLastActionTimeNull())
                            {
                                parameter[15].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[15].Value = soRow.LastActionTime;
                            }
                            if (soRow.IsFlagNull())
                            {
                                parameter[16].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[16].Value = soRow.Flag;
                            }
                            if (soRow.IsVoucherNONull())
                            {
                                parameter[17].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[17].Value = soRow.VoucherNO;
                            }

                            if (soRow.IsEntryIDNull())
                            {
                                parameter[18].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[18].Value = soRow.EntryID;
                            }
                            if (soRow.IsCommitQtyNull())
                            {
                                parameter[19].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[19].Value = soRow.CommitQty;
                            }
                            if (soRow.IsAuxCommitQty1Null())
                            {
                                parameter[20].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[20].Value = soRow.AuxCommitQty1;
                            }
                           
                            if (soRow.IsCdefine1Null())
                            {
                                parameter[21].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[21].Value = soRow.Cdefine1;
                            }
                            if (soRow.IsCdefine2Null())
                            {
                                parameter[22].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[22].Value = soRow.Cdefine2;
                            }
                            if (soRow.IsCdefine3Null())
                            {
                                parameter[23].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[23].Value = soRow.Cdefine3;
                            }
                            if (soRow.IsUdefine1Null())
                            {
                                parameter[24].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[24].Value = soRow.Udefine1;
                            }
                            if (soRow.IsUdefine2Null())
                            {
                                parameter[25].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[25].Value = soRow.Udefine2;
                            }
                            if (soRow.IsUdefine3Null())
                            {
                                parameter[26].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[26].Value = soRow.Udefine3;
                            }
                            parameter[27].Value = soRow.IsOnlyIDNull() ? 0 : soRow.OnlyID;

                            if (soRow.IsVehicleNONull())
                            {
                                parameter[28].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[28].Value = soRow.VehicleNO;
                            }
                            //新增加-平板属性
                            if (soRow.IsWidth_PNull())
                            {
                                parameter[29].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[29].Value = soRow.Width_P;
                            }
                            if (soRow.IsLength_PNull())
                            {
                                parameter[30].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[30].Value = soRow.Length_P;
                            }
                            if (soRow.IsCdefine3Null())
                            {
                                parameter[31].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[31].Value = soRow.Cdefine3;
                            }
                            if (soRow.IsReamsNull())
                            {
                                parameter[32].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[32].Value = soRow.Reams;
                            }
                            if (soRow.IsSlidesOfReamNull())
                            {
                                parameter[33].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[33].Value = soRow.SlidesOfReam;
                            }
                            if (soRow.IsWeightModeNull())
                            {
                                parameter[34].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[34].Value = soRow.WeightMode;
                            }
                            if (soRow.IsCustTrademarkNull())
                            {
                                parameter[35].Value = DBNull.Value;
                            }
                            else
                            {
                                parameter[35].Value = soRow.CustTrademark;
                            }
                            #endregion

                            try
                            {
                                MSSqlHelper.ExecuteNonQuery(connection,
                                    CommandType.StoredProcedure,
                                    "[Insert_ProductGroup]",
                                    parameter);

                                retid = Convert.ToInt32(parameter[27].Value);

                                OnSqlStateChange(new SqlStateEventArgs(true, "[[Insert_ProductGroup]] OK"));
                            }
                            catch (Exception ex)
                            {
                                OnSqlStateChange(new SqlStateEventArgs(false, "[[Insert_ProductGroup]] Error:" + ex.ToString()));
                                retid = -1;
                            }
                        }
                        if (retid < 0 || retid == 0)
                            return "插入立库接口失败";
                    }

                    ts.Complete();
                    return "";

                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }
        //public string Tran_Insert_CT_StockOutOld(WMSDS planDS)
        //{
        //    try
        //    {
        //        using (TransactionScope ts = new TransactionScope())
        //        {
        //            InterfaceDS.CT_StockInRow siRow = new InterfaceDS().CT_StockIn.NewCT_StockInRow();

        //            string VoucherNO = planDS.T_OutStock.Rows[0][planDS.T_OutStock.VoucherNOColumn].ToString();
        //            string SourceVoucherNO = planDS.T_OutStock.Rows[0][planDS.T_OutStock.SourceVoucherNOColumn].ToString(); //planDS.T_OutStock.Rows[0][planDS.T_OutStock.SourceVoucherNOColumn].ToString();
        //            string BusinessType = planDS.T_OutStock.Rows[0][planDS.T_OutStock.BusinessTypeColumn].ToString();
        //            string BillType = planDS.T_OutStock.Rows[0][planDS.T_OutStock.BillTypeColumn].ToString();
        //            string FactoryID = planDS.T_OutStock.Rows[0][planDS.T_OutStock.FactoryIDColumn].ToString();
        //            string WHCodeFrom = planDS.T_OutStock.Rows[0][planDS.T_OutStock.WHCodeColumn].ToString();
        //            string WHCodeTo = planDS.T_OutStock.Rows[0][planDS.T_OutStock.WHToCodeColumn].ToString();
        //            string TradeType = planDS.T_OutStock.Rows[0][planDS.T_OutStock.TradeTypeColumn].ToString();
        //            string DeptName = planDS.T_OutStock.Rows[0][planDS.T_OutStock.BillDeptColumn].ToString();
        //            string TransportType = planDS.T_OutStock.Rows[0][planDS.T_OutStock.TransportTypeColumn].ToString();
        //            string Keeper = planDS.T_OutStock.Rows[0][planDS.T_OutStock.BillerColumn].ToString();
        //            string ContainerNO = planDS.T_OutStock.Rows[0][planDS.T_OutStock.BoxNOColumn].ToString();
        //            string VehicleNO = planDS.T_OutStock.Rows[0][planDS.T_OutStock.VehicleNOColumn].ToString();
        //            for (int i = 0; i < planDS.T_OutStock_Entry.Rows.Count; i++)
        //            {
                       
        //                    InterfaceDS.CT_StockOutRow soRow = new InterfaceDS().CT_StockOut.NewCT_StockOutRow();
        //                    soRow.VoucherNO = VoucherNO;
        //                    soRow.SourceVoucherNO = SourceVoucherNO;
        //                    soRow.BusinessType = BusinessType;
        //                    soRow.BillType = BillType;
        //                    soRow.FactoryID = FactoryID;
        //                    soRow.WHCodeFrom = WHCodeFrom;
        //                    soRow.WHCodeTo = WHCodeTo;
        //                    soRow.TradeType = TradeType;
        //                    soRow.DeptName = DeptName;
        //                    soRow.TransportType = TransportType;
        //                    soRow.Keeper = Keeper;
        //                    soRow.ContainerNO = ContainerNO;
        //                    soRow.VehicleNO = VehicleNO;

        //                    //
        //                    soRow.EntryID = Convert.ToInt32(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.EntryIDColumn].ToString());
        //                    soRow.SourceEntryID = Convert.ToInt32(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.SourceEntryIDColumn].ToString());
        //                    soRow.MaterialCode = planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.MaterialCodeColumn].ToString();
        //                    soRow.BatchNO = planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.BatchNOColumn].ToString();
        //                    soRow.AuxCommitQty1 = Convert.ToDecimal(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.PlanCommitQtyColumn].ToString());
        //                    soRow.CommitQty = Convert.ToDecimal(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.PlanCommitAuxQty1Column].ToString() == "" ? "0" : planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.PlanCommitAuxQty1Column].ToString());
        //                    soRow.AuxCommitQty2 = Convert.ToDecimal(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.PlanCommitAuxQty2Column].ToString() == "" ? "0" : planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.PlanCommitAuxQty2Column].ToString());
        //                    soRow.Cdefine1 = planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Cdefine1Column].ToString();
        //                    soRow.Cdefine2 = planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Cdefine2Column].ToString();
        //                    soRow.Cdefine3 = planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Cdefine3Column].ToString();
        //                    soRow.Udefine1 = Convert.ToDecimal(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Udefine1Column].ToString() == "" ? "0" : planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Udefine1Column].ToString());
        //                    soRow.Udefine2 = Convert.ToDecimal(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Udefine2Column].ToString() == "" ? "0" : planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Udefine2Column].ToString());
        //                    soRow.Udefine3 = Convert.ToDecimal(planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Udefine3Column].ToString() == "" ? "0" : planDS.T_OutStock_Entry.Rows[i][planDS.T_OutStock_Entry.Udefine3Column].ToString());
        //                    soRow.WriteDate = DateTime.Now;
        //                    soRow.Flag = "0";
        //                int retid = -1;
        //                using (SqlConnection connection = new SqlConnection(ConnctionString))
        //                {
        //                    SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[SP_Insert_CT_StockOut]");


        //                    #region 传参数
        //                    //parameter[0].Value = row.OnlyID;
        //                    parameter[0].Value = soRow.IsOnlyIDNull()?0:soRow.OnlyID;

        //                    if (soRow.IsVoucherNONull())
        //                    {
        //                        parameter[1].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[1].Value = soRow.VoucherNO;
        //                    }
        //                    if (soRow.IsSourceVoucherNONull())
        //                    {
        //                        parameter[2].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[2].Value = soRow.SourceVoucherNO;
        //                    }
        //                    if (soRow.IsBusinessTypeNull())
        //                    {
        //                        parameter[3].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[3].Value = soRow.BusinessType;
        //                    }
        //                    if (soRow.IsBillTypeNull())
        //                    {
        //                        parameter[4].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[4].Value = soRow.BillType;
        //                    }
        //                    if (soRow.IsFactoryIDNull())
        //                    {
        //                        parameter[5].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[5].Value = soRow.FactoryID;
        //                    }
        //                    if (soRow.IsWHCodeFromNull())
        //                    {
        //                        parameter[6].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[6].Value = soRow.WHCodeFrom;
        //                    }
        //                    if (soRow.IsWHCodeToNull())
        //                    {
        //                        parameter[7].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[7].Value = soRow.WHCodeTo;
        //                    }
        //                    if (soRow.IsTradeTypeNull())
        //                    {
        //                        parameter[8].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[8].Value = soRow.TradeType;
        //                    }
        //                    if (soRow.IsDeptNameNull())
        //                    {
        //                        parameter[9].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[9].Value = soRow.DeptName;
        //                    }
        //                    if (soRow.IsTransportTypeNull())
        //                    {
        //                        parameter[10].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[10].Value = soRow.TransportType;
        //                    }
        //                    if (soRow.IsKeeperNull())
        //                    {
        //                        parameter[11].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[11].Value = soRow.Keeper;
        //                    }
        //                    if (soRow.IsContainerNONull())
        //                    {
        //                        parameter[12].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[12].Value = soRow.ContainerNO;
        //                    }
        //                    if (soRow.IsVehicleNONull())
        //                    {
        //                        parameter[13].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[13].Value = soRow.VehicleNO;
        //                    }
        //                    if (soRow.IsEntryIDNull())
        //                    {
        //                        parameter[14].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[14].Value = soRow.EntryID;
        //                    }
        //                    if (soRow.IsSourceEntryIDNull())
        //                    {
        //                        parameter[15].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[15].Value = soRow.SourceEntryID;
        //                    }
        //                    if (soRow.IsMaterialCodeNull())
        //                    {
        //                        parameter[16].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[16].Value = soRow.MaterialCode;
        //                    }
        //                    if (soRow.IsBatchNONull())
        //                    {
        //                        parameter[17].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[17].Value = soRow.BatchNO;
        //                    }
        //                    if (soRow.IsCommitQtyNull())
        //                    {
        //                        parameter[18].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[18].Value = soRow.CommitQty;
        //                    }
        //                    if (soRow.IsAuxCommitQty1Null())
        //                    {
        //                        parameter[19].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[19].Value = soRow.AuxCommitQty1;
        //                    }
        //                    if (soRow.IsAuxCommitQty2Null())
        //                    {
        //                        parameter[20].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[20].Value = soRow.AuxCommitQty2;
        //                    }
        //                    if (soRow.IsCdefine1Null())
        //                    {
        //                        parameter[21].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[21].Value = soRow.Cdefine1;
        //                    }
        //                    if (soRow.IsCdefine2Null())
        //                    {
        //                        parameter[22].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[22].Value = soRow.Cdefine2;
        //                    }
        //                    if (soRow.IsCdefine3Null())
        //                    {
        //                        parameter[23].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[23].Value = soRow.Cdefine3;
        //                    }
        //                    if (soRow.IsUdefine1Null())
        //                    {
        //                        parameter[24].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[24].Value = soRow.Udefine1;
        //                    }
        //                    if (soRow.IsUdefine2Null())
        //                    {
        //                        parameter[25].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[25].Value = soRow.Udefine2;
        //                    }
        //                    if (soRow.IsUdefine3Null())
        //                    {
        //                        parameter[26].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[26].Value = soRow.Udefine3;
        //                    }
        //                    if (soRow.IsWriteDateNull())
        //                    {
        //                        parameter[27].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[27].Value = soRow.WriteDate;
        //                    }
        //                    if (soRow.IsReadDateNull())
        //                    {
        //                        parameter[28].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[28].Value = soRow.ReadDate;
        //                    }
        //                    if (soRow.IsFlagNull())
        //                    {
        //                        parameter[29].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[29].Value = soRow.Flag;
        //                    }
        //                    if (soRow.IsInspurVoucherNONull())
        //                    {
        //                        parameter[30].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[30].Value = soRow.InspurVoucherNO;
        //                    }
        //                    if (soRow.IsErrorMsgNull())
        //                    {
        //                        parameter[31].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[31].Value = soRow.ErrorMsg;
        //                    }
        //                    if (soRow.IsIsReadNull())
        //                    {
        //                        parameter[32].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[32].Value = soRow.IsRead;
        //                    }
        //                    if (soRow.IsIsWriteNull())
        //                    {
        //                        parameter[33].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[33].Value = soRow.IsWrite;
        //                    }
        //                    if (soRow.IsLastActionTimeNull())
        //                    {
        //                        parameter[34].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[34].Value = soRow.LastActionTime;
        //                    }
        //                    if (soRow.IsConnectionIDNull())
        //                    {
        //                        parameter[35].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[35].Value = soRow.ConnectionID;
        //                    }
        //                    if (soRow.IsOptimisticLockFieldNull())
        //                    {
        //                        parameter[36].Value = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        parameter[36].Value = soRow.OptimisticLockField;
        //                    }

        //                    #endregion

        //                    try
        //                    {
        //                        MSSqlHelper.ExecuteNonQuery(connection,
        //                            CommandType.StoredProcedure,
        //                            "[SP_Insert_CT_StockOut]",
        //                            parameter);

        //                        retid = Convert.ToInt32(parameter[0].Value);

        //                        OnSqlStateChange(new SqlStateEventArgs(true, "[[SP_Insert_CT_StockOut]] OK"));
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        OnSqlStateChange(new SqlStateEventArgs(false, "[[SP_Insert_CT_StockOut]] Error:" + ex.ToString()));
        //                        retid = -1;
        //                    }
        //                }
        //                if (retid < 0 || retid == 0)
        //                    return "插入出库接口失败";
        //            }
                    
                    
        //            //更新本地单据的isupload
        //            //WMSDS.T_InStockRow isRow = new WMSDS().T_InStock.NewT_InStockRow();
        //            //isRow.VoucherNO = t_InStockDataTable.Rows[0][t_InStockDataTable.VoucherNOColumn].ToString();
        //            //isRow.OnlyID = Convert.ToInt32(t_InStockDataTable.Rows[0][t_InStockDataTable.OnlyIDColumn].ToString());

        //            //isRow.UploadDate = DateTime.Now;
        //            //isRow.IsUpload = "1";
        //            //string ss = this.Update_T_InStock(isRow);
        //            //if (ss != "")
        //            //    return "更新上传标记失败：" + ss;
                        
                    

                        
        //            ts.Complete();
        //            return "";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //写入日志
        //        return ex.Message;
        //    }
        //}
        /// <summary>
        /// 查询接口中所有的出库通知
        /// </summary>
        /// <param name="voucher"></param>
        /// <param name="dates"></param>
        /// <param name="dateE"></param>
        /// <returns></returns>
        public InterfaceDS Select_CT_RequestInfo(string voucher, string dates, string dateE)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select *  from CT_RequestInfo where 1=1 ";
                if (voucher != "")
                    sqlstr += "  and VoucherNO = '" + voucher+"'";
                if(dates!="")
                    sqlstr += "  and RetrieveDate >= '" + dates + "'" ;
                if (dateE != "")
                    sqlstr += "  and RetrieveDate <= '" + dateE + "'";
                sqlstr += " order by VoucherNO  ";
                
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "CT_RequestInfo" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "CT_RequestInfoQuery OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "CT_RequestInfoQuery Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet Select_T_InStockAndEntry_Relation(string factory, string dateS, string dateE)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_InStockAndEntry_Relation]");

                #region 传参数

                parameter[0].Value = dateS;
                parameter[1].Value = dateE;
                parameter[2].Value = factory;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_InStockAndEntry_Relation]",
                        ds,
                      new string[] { "T_InStock", "T_InStock_Entry" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_InStockAndEntry_Relation OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_InStockAndEntry_Relation Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public string Get_T_OutStock_NewFlow(string type, string machineid)
        {
            string voucherNO = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Get_T_OutStock_NewFlow]");

                #region 传参数

                parameter[0].Value = type + machineid;
                parameter[1].Value = "";
                parameter[1].Direction = ParameterDirection.Output;


                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Get_T_OutStock_NewFlow",
                        parameter
                        );
                    voucherNO = parameter[1].Value.ToString();
                    OnSqlStateChange(new SqlStateEventArgs(true, "Get_T_OutStock_NewFlow OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Get_T_OutStock_NewFlow Error:" + ex.ToString()));
                }
            }
            return voucherNO;
        }

        public string Tran_SaveNewOutStock(WMSDS.T_OutStockRow tospRow, DataRowCollection rowCollect)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //插入抬头
                    int result = this.Insert_T_OutStock(tospRow);
                    if (result < 0||result==0)
                        return "Tran_SaveNewOutStock抬头插入失败";
                    //插入分录
                    for (int i = 0; i < rowCollect.Count; i++)
                    {
                        WMSDS.T_OutStock_EntryRow ospeRow = rowCollect[i] as WMSDS.T_OutStock_EntryRow;
                        ospeRow.VoucherID = result;
                        int entryresult = this.Insert_T_OutStock_EntryByRow(ospeRow);
                        if (entryresult < 0||entryresult==0)
                            return "Tran_SaveNewOutStock表体插入失败";
                    }
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        private int Insert_T_OutStock_EntryByRow(WMSDS.T_OutStock_EntryRow row)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_OutStock_Entry]");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                parameter[0].Value =row.IsOnlyIDNull()?0: row.OnlyID;

                if (row.IsProductTypeIDNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ProductTypeID;
                }
                if (row.IsBrandNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.Brand;
                }
                if (row.IsTypeNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.Type;
                }
                if (row.IsGradeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.Grade;
                }
                if (row.IsBasisweightNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.Basisweight;
                }
                if (row.IsSpecificationNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.Specification;
                }
                if (row.IsCoreDiameterNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.CoreDiameter;
                }
                if (row.IsWidth_RNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.Width_R;
                }
                if (row.IsWidth_PNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.Width_P;
                }
                if (row.IsLength_PNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.Length_P;
                }
                if (row.IsReamsNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Reams;
                }
                if (row.IsSlidesOfReamNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.SlidesOfReam;
                }
                if (row.IsSlidesOfSheetNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.SlidesOfSheet;
                }
                if (row.IsReamPackTypeNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.ReamPackType;
                }
                if (row.IsRemarkNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.Remark;
                }
                if (row.IsMaterialCodeNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.MaterialCode;
                }
                if (row.IsSKUNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.SKU;
                }
                if (row.IsTrademarkStyleNull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.TrademarkStyle;
                }
                if (row.IsIsWhiteFlagNull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.IsWhiteFlag;
                }
                if (row.IsOrderNONull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.OrderNO;
                }
                if (row.IsPriceNull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.Price;
                }
                if (row.IsPaperCertNull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.PaperCert;
                }
                if (row.IsSpecProdNameNull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.SpecProdName;
                }
                if (row.IsSpecCustNameNull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.SpecCustName;
                }
                if (row.IsCustTrademarkNull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.CustTrademark;
                }
                if (row.IsWeightModeNull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.WeightMode;
                }
                if (row.IsPlanQtyNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.PlanQty;
                }
                if (row.IsPlanAuxQty1Null())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.PlanAuxQty1;
                }
                if (row.IsPlanAuxQty2Null())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.PlanAuxQty2;
                }
                if (row.IsPlanCommitQtyNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.PlanCommitQty;
                }
                if (row.IsPlanCommitAuxQty1Null())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.PlanCommitAuxQty1;
                }
                if (row.IsPlanCommitAuxQty2Null())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.PlanCommitAuxQty2;
                }
                if (row.IsCdefine1Null())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Cdefine1;
                }
                if (row.IsCdefine2Null())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.Cdefine2;
                }
                if (row.IsCdefine3Null())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.Cdefine3;
                }
                if (row.IsUdefine1Null())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.Udefine1;
                }
                if (row.IsUdefine2Null())
                {
                    parameter[37].Value = DBNull.Value;
                }
                else
                {
                    parameter[37].Value = row.Udefine2;
                }
                if (row.IsUdefine3Null())
                {
                    parameter[38].Value = DBNull.Value;
                }
                else
                {
                    parameter[38].Value = row.Udefine3;
                }
                if (row.IsMaterialNameNull())
                {
                    parameter[39].Value = DBNull.Value;
                }
                else
                {
                    parameter[39].Value = row.MaterialName;
                }
                if (row.IsBatchNONull())
                {
                    parameter[40].Value = DBNull.Value;
                }
                else
                {
                    parameter[40].Value = row.BatchNO;
                }
                if (row.IsSourceEntryIDNull())
                {
                    parameter[41].Value = DBNull.Value;
                }
                else
                {
                    parameter[41].Value = row.SourceEntryID;
                }
                if (row.IsSourceVoucherIDNull())
                {
                    parameter[42].Value = DBNull.Value;
                }
                else
                {
                    parameter[42].Value = row.SourceVoucherID;
                }
                if (row.IsRemainQtyNull())
                {
                    parameter[43].Value = DBNull.Value;
                }
                else
                {
                    parameter[43].Value = row.RemainQty;
                }
                if (row.IsRemainAuxQtyNull())
                {
                    parameter[44].Value = DBNull.Value;
                }
                else
                {
                    parameter[44].Value = row.RemainAuxQty;
                }

                parameter[45].Value = row.EntryID;

                //zfz
                //if (row.IsEntryIDNull())
                //{
                //    parameter[45].Value = DBNull.Value;
                //}
                //else
                //{
                //    parameter[45].Value = row.EntryID;
                //}

                parameter[46].Value = row.VoucherID;

                //zfz
                //if (row.IsVoucherIDNull())
                //{
                //    parameter[46].Value = DBNull.Value;
                //}
                //else
                //{
                //    parameter[46].Value = row.VoucherID;
                //}
                if (row.IsDiameterNull())
                {
                    parameter[47].Value = DBNull.Value;
                }
                else
                {
                    parameter[47].Value = row.Diameter;
                }
                if (row.IsRollLengthNull())
                {
                    parameter[48].Value = DBNull.Value;
                }
                else
                {
                    parameter[48].Value = row.RollLength;
                }
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_OutStock_Entry",
                        parameter);

                    retid = Convert.ToInt32(parameter[0].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[Insert_T_OutStock_Entry] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Insert_T_OutStock_Entry] Error:" + ex.ToString()));
                }
            }
            return retid;
        }


        private int Insert_T_OutStock(WMSDS.T_OutStockRow row)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_OutStock]");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                parameter[0].Value = row.IsOnlyIDNull()?0:row.OnlyID;

                if (row.IsSourceVoucherNONull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.SourceVoucherNO;
                }
                if (row.IsBusinessTypeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.BusinessType;
                }
                if (row.IsSourceVoucherTypeNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.SourceVoucherType;
                }
                if (row.IsBillTypeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.BillType;
                }
                if (row.IsFactoryIDNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.FactoryID;
                }
                if (row.IsWHCodeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.WHCode;
                }
                if (row.IsWHToCodeNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.WHToCode;
                }
                if (row.IsTradeTypeNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.TradeType;
                }
                if (row.IsDeptNameNull())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.DeptName;
                }
                if (row.IsTransportTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.TransportType;
                }
                if (row.IsKeeperNull())
                {
                    parameter[11].Value = DBNull.Value;
                }
                else
                {
                    parameter[11].Value = row.Keeper;
                }
                if (row.IsBillerNull())
                {
                    parameter[12].Value = DBNull.Value;
                }
                else
                {
                    parameter[12].Value = row.Biller;
                }
                if (row.IsBillDeptNull())
                {
                    parameter[13].Value = DBNull.Value;
                }
                else
                {
                    parameter[13].Value = row.BillDept;
                }
                if (row.IsBillDateNull())
                {
                    parameter[14].Value = DBNull.Value;
                }
                else
                {
                    parameter[14].Value = row.BillDate;
                }
                if (row.IsBillRemarkNull())
                {
                    parameter[15].Value = DBNull.Value;
                }
                else
                {
                    parameter[15].Value = row.BillRemark;
                }
                if (row.IsCheckerNull())
                {
                    parameter[16].Value = DBNull.Value;
                }
                else
                {
                    parameter[16].Value = row.Checker;
                }
                if (row.IsCheckDateNull())
                {
                    parameter[17].Value = DBNull.Value;
                }
                else
                {
                    parameter[17].Value = row.CheckDate;
                }
                if (row.IsForklifterNONull())
                {
                    parameter[18].Value = DBNull.Value;
                }
                else
                {
                    parameter[18].Value = row.ForklifterNO;
                }
                if (row.IsCarrierNONull())
                {
                    parameter[19].Value = DBNull.Value;
                }
                else
                {
                    parameter[19].Value = row.CarrierNO;
                }
                if (row.IsContainerNONull())
                {
                    parameter[20].Value = DBNull.Value;
                }
                else
                {
                    parameter[20].Value = row.ContainerNO;
                }
                if (row.IsVehicleNONull())
                {
                    parameter[21].Value = DBNull.Value;
                }
                else
                {
                    parameter[21].Value = row.VehicleNO;
                }
                if (row.IsSealNONull())
                {
                    parameter[22].Value = DBNull.Value;
                }
                else
                {
                    parameter[22].Value = row.SealNO;
                }
                if (row.IsPortNONull())
                {
                    parameter[23].Value = DBNull.Value;
                }
                else
                {
                    parameter[23].Value = row.PortNO;
                }
                if (row.IsShipNONull())
                {
                    parameter[24].Value = DBNull.Value;
                }
                else
                {
                    parameter[24].Value = row.ShipNO;
                }
                if (row.IsPickNONull())
                {
                    parameter[25].Value = DBNull.Value;
                }
                else
                {
                    parameter[25].Value = row.PickNO;
                }
           
                if (row.IsOrderNONull())
                {
                    parameter[26].Value = DBNull.Value;
                }
                else
                {
                    parameter[26].Value = row.OrderNO;
                }
                if (row.IsIsCheckNull())
                {
                    parameter[27].Value = DBNull.Value;
                }
                else
                {
                    parameter[27].Value = row.IsCheck;
                }
                if (row.IsIsCancelNull())
                {
                    parameter[28].Value = DBNull.Value;
                }
                else
                {
                    parameter[28].Value = row.IsCancel;
                }
                if (row.IsIsCloseNull())
                {
                    parameter[29].Value = DBNull.Value;
                }
                else
                {
                    parameter[29].Value = row.IsClose;
                }
                if (row.IsCustomerNameNull())
                {
                    parameter[30].Value = DBNull.Value;
                }
                else
                {
                    parameter[30].Value = row.CustomerName;
                }
               
                if (row.IsQtyNull())
                {
                    parameter[31].Value = DBNull.Value;
                }
                else
                {
                    parameter[31].Value = row.Qty;
                }
                if (row.IsAuxQtyNull())
                {
                    parameter[32].Value = DBNull.Value;
                }
                else
                {
                    parameter[32].Value = row.AuxQty;
                }
              
                if (row.IsFinisherNull())
                {
                    parameter[33].Value = DBNull.Value;
                }
                else
                {
                    parameter[33].Value = row.Finisher;
                }
                if (row.IsFinishDateNull())
                {
                    parameter[34].Value = DBNull.Value;
                }
                else
                {
                    parameter[34].Value = row.FinishDate;
                }
                if (row.IsOrgToCodeNull())
                {
                    parameter[35].Value = DBNull.Value;
                }
                else
                {
                    parameter[35].Value = row.OrgToCode;
                }
               if (row.IsIsUploadNull())
                {
                    parameter[36].Value = DBNull.Value;
                }
                else
                {
                    parameter[36].Value = row.IsUpload;
                } 
                if (row.IsUploadDateNull())
               {
                   parameter[37].Value = DBNull.Value;
               }
               else
               {
                   parameter[37].Value = row.UploadDate;
               }
                parameter[38].Value = row.VoucherNO;

                #endregion


                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "Insert_T_OutStock",
                        parameter);

                    retid = Convert.ToInt32(parameter[0].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "Insert_T_OutStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Insert_T_OutStock Error:" + ex.ToString()));
                }
            }
            return retid;
        }

        public DataSet Select_T_OutStock_List(string factory, string dateS, string dateE)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStock_List]");

                #region 传参数

                parameter[0].Value = dateS;
                parameter[1].Value = dateE;
                parameter[2].Value = factory;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutStock_List]",
                        ds,
                      new string[] { "T_OutStock"},
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStock_List OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStock_List Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public WMSReportDS Select_OutStockAndEntry_ForPrintByVoucherNO(string voucherno)
        {
            WMSReportDS ds = new WMSReportDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStockAndEntry_ForPrintByVoucherNO]");

                #region 传参数

                parameter[0].Value = voucherno;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutStockAndEntry_ForPrintByVoucherNO]",
                        ds,
                      new string[] { "T_OutStock", "T_OutStockDetail_Domestic_Rep", "T_OutStockDetail_International_Rep", "T_OutStockTitle_Domestic_Rep", "T_OutStockTitle_International_Rep", "T_OutStockLink_International_Rep" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_OutStockAndEntry_ForPrintByVoucherNO OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_OutStockAndEntry_ForPrintByVoucherNO Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public WMSDS Select_T_OutStock_PrintPre(string voucherno)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStock_PrintPre]");

                #region 传参数

                parameter[0].Value = voucherno;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutStock_PrintPre]",
                        ds,
                      new string[] { "T_OutStock_PrintPre" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStock_PrintPre OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStock_PrintPre Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public int Insert_T_OutStock_PrintPreByRow(WMSDS.T_OutStock_PrintPreRow row)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_OutStock_PrintPre]");



                #region 传参数
                parameter[0].Value =row.IsOnlyIDNull()?0: row.OnlyID;

                if (row.IsControlNONull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.ControlNO;
                }
                if (row.IsRollPlusNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.RollPlus;
                }
                if (row.IsPalletPlusNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.PalletPlus;
                }
                if (row.IsContainerSizeNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.ContainerSize;
                }
                if (row.IsContainerBulkNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.ContainerBulk;
                }
                if (row.IsPrintTypeNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.PrintType;
                }
                if (row.IsPrintDateNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.PrintDate;
                }
                parameter[8].Value = row.VoucherID;

                #endregion


	




                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Insert_T_OutStock_PrintPre]",
                        parameter);
                    if (row.IsOnlyIDNull())

                        retid = Convert.ToInt32(parameter[0].Value);
                    else
                        retid = row.OnlyID;
                    OnSqlStateChange(new SqlStateEventArgs(true, "[[[Insert_T_OutStock_PrintPre]]] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[[[Insert_T_OutStock_PrintPre]]] Error:" + ex.ToString()));
                }
            }
            return retid; 
        }

        public WMSDS Select_T_Shift(string shift)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select ShiftCode,ShiftName from T_Shift ";
                if (shift != "")
                    sqlstr += " where ShiftName = @ShiftName ";
                sqlstr += " order by shiftcode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = shift;
                par1.DbType = DbType.String;
                par1.ParameterName = "@ShiftName";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_Shift" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_Shift OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_Shift Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public ProduceDS Sheet_ProductQueryAllByFK(string productid,string machineID)
        {
            ProduceDS ds = new ProduceDS();

            //先查询机台的连接
            string machineStr = "";

            WMSDS wmsds = this.Select_T_Factory(true, machineID);
            if (wmsds.T_Factory.Rows.Count > 0)
                machineStr = wmsds.T_Factory.Rows[0][wmsds.T_Factory.FactoryAddrColumn].ToString();
            else
                return ds;
            using (SqlConnection connection = new SqlConnection(machineStr))
            {

                //string sqlstr = " select * from Roll_Product where rollid = @ProductID";
                string sqlstr = "Sheet_ProductQueryByProductIDNew";

                SqlParameter par1 = new SqlParameter();
                par1.Value = productid;
                par1.DbType = DbType.String;
                par1.ParameterName = "@ProductID";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          sqlstr,
                          ds,
                          new string[] { "Sheet_Product" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Sheet_ProductQueryByProductID OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Sheet_ProductQueryByProductID Error:" + ex.ToString()));
                }
                return ds;
            }
        }
        /// <summary>
        /// 使用存储过程事务删除入库单据
        /// </summary>
        /// <param name="delno"></param>
        /// <returns></returns>
        public string Tran_Delete_T_InStock(string delno)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Tran_Delete_T_InStock]");
                parameter[0].Value = delno;
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Tran_Delete_T_InStock]",
                        parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Tran_Delete_T_InStock] OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Tran_Delete_T_InStock] Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }
        /// <summary>
        /// 删除入库单据
        /// </summary>
        /// <param name="delno"></param>
        /// <returns></returns>
        public string Delete_T_InStock(string delno)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Delete_T_InStock]");
                parameter[0].Value = delno;
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Delete_T_InStock]",
                        parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Delete_T_InStock] OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Delete_T_InStock] Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }
        public DataSet Select_T_OutStockAndEntry_RelationByVoucherNO(string voucherno)
        {
            {

                DataSet ds = new DataSet();

                SqlParameter par1 = new SqlParameter("@voucherno", SqlDbType.VarChar,20);
               
                    par1.Value = voucherno;
             
                SqlParameter[] pars = new SqlParameter[] { par1 };


                using (SqlConnection connection = new SqlConnection(ConnctionString))
                {

                    try
                    {
                        MSSqlHelper.FillDataset(connection,
                            CommandType.StoredProcedure,
                            "Select_T_OutStockAndEntry_RelationByVoucherNO",
                            ds,
                            new string[] { "T_OutStock", "T_OutStock_Entry" },
                            pars
                            );
                        OnSqlStateChange(new SqlStateEventArgs(true, "[Select_T_OutStockAndEntry_RelationByVoucherNO] OK"));
                    }
                    catch (Exception ex)
                    {
                        OnSqlStateChange(new SqlStateEventArgs(false, "[Select_T_OutStockAndEntry_RelationByVoucherNO] Error:" + ex.ToString()));
                    }
                }
                return ds;

            }
        }
        /// <summary>
        /// 扫描出库匹配分录的时候用
        /// </summary>
        /// <param name="VoucherNO"></param>
        /// <returns></returns>
        public DataSet Select_T_OutStockAndEntryByVoucherNOForMatch(string VoucherNO)
        {

                DataSet ds = new DataSet();

                SqlParameter par1 = new SqlParameter("@varcharNO", SqlDbType.VarChar, 20);
                if (VoucherNO == "")
                    par1.Value = DBNull.Value;
                else
                    par1.Value = VoucherNO;
                SqlParameter[] pars = new SqlParameter[] { par1 };


                using (SqlConnection connection = new SqlConnection(ConnctionString))
                {

                    try
                    {
                        MSSqlHelper.FillDataset(connection,
                            CommandType.StoredProcedure,
                            "Select_T_OutStockAndEntryByVoucherNOForMatch",
                            ds,
                            new string[] { "T_OutStockAndEntry" },
                            pars
                            );
                        OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStockAndEntryByVoucherNOForMatch OK"));
                    }
                    catch (Exception ex)
                    {
                        OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStockAndEntryByVoucherNOForMatch Error:" + ex.ToString()));
                    }
                }
                return ds;

        }
        /// <summary>
        /// 查询上传的出库单
        /// </summary>
        /// <param name="voucherno">出库单号</param>
        /// <returns></returns>
        public InterfaceDS Select_T_OutStockAndEntry_RelationForUpload(string voucherno,string dateStart,string dateEnd)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStockAndEntry_RelationForUpload]");

                #region 传参数
                if(voucherno=="")
                    parameter[0].Value = DBNull.Value;
                else
                    parameter[0].Value = voucherno;
                if (dateStart == "")
                    parameter[1].Value = DBNull.Value;
                else
                parameter[1].Value = dateStart;
                if (dateEnd == "")
                    parameter[2].Value = DBNull.Value;
                else
                parameter[2].Value = dateEnd;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutStockAndEntry_RelationForUpload]",
                        ds,
                      //new string[] { "T_OutStock", "T_OutStock_Entry","CT_StockOut" },
                      new string[] { "CT_StockOut" },

                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStockAndEntry_RelationForUpload OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStockAndEntry_RelationForUpload Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public InterfaceDS Select_T_OutStockAndEntry_RelationForZZ(string voucherno, string dateStart, string dateEnd)
        {
            InterfaceDS ds = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStockAndEntry_RelationForZZ]");

                #region 传参数
                if (voucherno == "")
                    parameter[0].Value = DBNull.Value;
                else
                    parameter[0].Value = voucherno;
                if (dateStart == "")
                    parameter[1].Value = DBNull.Value;
                else
                    parameter[1].Value = dateStart;
                if (dateEnd == "")
                    parameter[2].Value = DBNull.Value;
                else
                    parameter[2].Value = dateEnd;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutStockAndEntry_RelationForZZ]",
                        ds,
                        //new string[] { "T_OutStock", "T_OutStock_Entry","CT_StockOut" },
                      new string[] { "CT_StockOut" },

                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStockAndEntry_RelationForZZ OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStockAndEntry_RelationForZZ Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public string Tran_SaveUpdateOutStock(WMSDS.T_OutStockRow tospRow, DataRowCollection rowCollect)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //更新抬头
                    string result = this.Update_T_OutStock(tospRow);
                    if (result != "")
                        return "Tran_SaveNewOutStockPlan抬头更新失败" + result;
                    ////找出抬头的onlyid
                    //WMSDS wms = this.Select_T_OutStock_PlanByFK(tospRow.VoucherNO, "", "", "", "", "", "", 0, 0, 0);
                    //string onlyid = wms.T_OutStock_Plan.Rows[0][wms.T_OutStock_Plan.OnlyIDColumn].ToString();
                    //删除单据表体，再插入
                    string ss = this.Delete_T_OutStock_EntryByVoucherNO(tospRow.VoucherNO);
                    if (ss != "")
                        return "Tran_SaveNewOutStockPlan分录删除失败" + ss;
                    //插入分录
                    for (int i = 0; i < rowCollect.Count; i++)
                    {
                        WMSDS.T_OutStock_EntryRow ospeRow = rowCollect[i] as WMSDS.T_OutStock_EntryRow;
                        ospeRow.VoucherID = tospRow.OnlyID;
                        int entryresult = this.Insert_T_OutStock_EntryByRow(ospeRow);
                        if (entryresult < 0)
                            return "Tran_SaveNewOutStockPlan分录" + (i + 1) + "插入失败";
                    }
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        private string Delete_T_OutStock_EntryByVoucherNO(string VoucherNO)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Delete_T_OutStock_EntryByVoucherNO]");

                #region 传参数

                parameter[0].Value = VoucherNO;
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Delete_T_OutStock_EntryByVoucherNO",
                          parameter);
                    return ret = "";
                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_OutStock_EntryByVoucherNO OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_OutStock_EntryByVoucherNO Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }

        public DataSet Select_T_OutStock_Product(string voucherno, string productid)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStock_Product]");

                #region 传参数
                if(voucherno=="")
                parameter[0].Value = DBNull.Value;
                else
                    parameter[0].Value = voucherno;
                if (productid == "")
                    parameter[1].Value = DBNull.Value;
                else
                    parameter[1].Value = productid;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_OutStock_Product]",
                        ds,
                      new string[] {  "T_OutStock_Product" },
                        parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStock_Product OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStock_Product Error:" + ex.ToString()));
                }
            }
            return ds;
        }
        /// <summary>
        /// 事务处理取消出库
        /// </summary>
        /// <param name="outRow"></param>
        /// <param name="lifeRow"></param>
        /// <param name="osppRow"></param>
        /// <param name="ospeRow"></param>
        /// <param name="ospRow"></param>
        /// <returns></returns>
        public string Tran_ProductScanOutCancel(WMSDS.T_Product_InRow inRow, WMSDS.T_Product_InRow outRow, WMSDS.T_ProductLifeRow lifeRow, WMSDS.T_OutStock_ProductRow osppRow, WMSDS.T_OutStock_EntryRow ospeRow, WMSDS.T_OutStock_Plan_EntryRow ospRow)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    string error1 = this.Update_T_Product_In(inRow);
                    if (error1 != "")
                        return "Tran_ProductScanOutCancel原入库产品更新失败" + error1;

                    string error = this.Update_T_Product_In(outRow);
                    if (error != "")
                        return "Tran_ProductScanOutCancel出库产品更新失败" + error;
                    int lifeID = this.Insert_T_ProductLife(lifeRow, "");
                    if (lifeID < 0||lifeID==0)
                        return "Tran_ProductScanOutCancel生涯保存失败";
                    bool outID = this.Delete_T_OutStock_Product(osppRow);
                    if (!outID)
                        return "Tran_ProductScanOutCancel出库分录关联保存失败";
                    
                    string ospeerr = this.Update_T_OutStock_Entry(ospeRow);
                    if (ospeerr != "")
                        return "Tran_ProductScanOutCancel出库分录数量保存失败";
                    string osperr = this.Update_T_OutStock_Plan_Entry(ospRow);
                    if (osperr != "")
                        return "Tran_ProductScanOutCancel通知分录数量保存失败";
                    ts.Complete();

                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        private bool Delete_T_OutStock_Product(WMSDS.T_OutStock_ProductRow osppRow)
        {
            bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                try
                {
                    connection.Open();
                    int voucherid = osppRow.VoucherID;
                    int planid = osppRow.EntryID;
                    int productonlyid = osppRow.ProductOnlyID;
                    string sqlstr = "delete from T_OutStock_Product where VoucherID =@VoucherID  and EntryID=@EntryID and ProductOnlyID=@ProductOnlyID ;";

                    SqlParameter[] parameter = new SqlParameter[]
                    {
                       new SqlParameter("@VoucherID", voucherid),
                        new SqlParameter("@EntryID", planid), new SqlParameter("@ProductOnlyID", productonlyid),
                    };

                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    ret = true;
                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_OutStock_Product OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_OutStock_Product Error:" + ex.ToString()));
                }
                return ret;
            }
        }
        /// <summary>
        /// 按照时间段查询出库单和分录的关系，用于出库单列表
        /// </summary>
        /// <param name="dateS"></param>
        /// <param name="dateE"></param>
        /// <returns></returns>
        public DataSet Select_T_OutStockAndEntry_Relation(string dateS, string dateE)
        {
            {

                DataSet ds = new DataSet();

                SqlParameter par1 = new SqlParameter("@dateS", SqlDbType.DateTime);
                if (dateS == "")
                    par1.Value = DBNull.Value;
                else
                    par1.Value = Convert.ToDateTime(dateS);
                SqlParameter par2 = new SqlParameter("@dateE", SqlDbType.DateTime);
                if (dateE == "")
                    par2.Value = DBNull.Value;
                else
                    par2.Value = Convert.ToDateTime(dateE);
                SqlParameter[] pars = new SqlParameter[] { par1,par2 };


                using (SqlConnection connection = new SqlConnection(ConnctionString))
                {

                    try
                    {
                        MSSqlHelper.FillDataset(connection,
                            CommandType.StoredProcedure,
                            "Select_T_OutStockAndEntry_Relation",
                            ds,
                            new string[] { "T_OutStock", "T_OutStock_Entry" },
                            pars
                            );
                        OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStockAndEntry_Relation OK"));
                    }
                    catch (Exception ex)
                    {
                        OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStockAndEntry_Relation Error:" + ex.ToString()));
                    }
                }
                return ds;

            }
        }

        public WMSDS Select_T_UserType(string usercode)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select UserTypeCode,UserTypeName  from T_User_Type where ischoose ='1' ";
                if (usercode != "")
                    sqlstr += " and UserTypeCode = @UserCode ";
                sqlstr += " order by UserTypeCode  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = usercode;
                par1.DbType = DbType.String;
                par1.ParameterName = "@UserCode";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_User_Type" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_User_Type OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_User_Type Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public int Insert_Update_T_User(string usercode, string username, string password, string shift, string usertype)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_Update_T_User]");



                #region 传参数
                parameter[0].Value = usercode;

                if (username=="")
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = username;
                }
                if (password == "")
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = password;
                } if (shift == "")
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = shift;
                } if (usertype == "")
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value =Convert.ToInt32( usertype);
                }

                #endregion







                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Insert_Update_T_User]",
                        parameter);
                    retid = 0;
                    OnSqlStateChange(new SqlStateEventArgs(true, "[[Insert_Update_T_User]] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[[Insert_Update_T_User]] Error:" + ex.ToString()));
                }
            }
            return retid; 
        }

        public int DeleteT_User(string usercode)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sql = "update  t_user set ischoose ='0' where usercode ='"+usercode+"'";

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.Text,
                       sql
                        );
                    retid = 0;
                    OnSqlStateChange(new SqlStateEventArgs(true, "[[DeleteT_User]] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[[DeleteT_User]] Error:" + ex.ToString()));
                }
            }
            return retid; 
        }

        public DataSet Select_StockDetail(string dateS,string dateE)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_StockDetail]");



                    #region 传参数

                    parameter[0].Value =Convert.ToDateTime( dateE);
                    parameter[1].Value = Convert.ToDateTime(dateS);

                   

                    #endregion

                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          "Select_StockDetail",
                          ds,
                          new string[] { "T_StockDetail" },
                         parameter );
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_StockDetail OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_StockDetail Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet Select_StockStat(string dateS,string dateE)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_StockStat]");



                    #region 传参数

                    parameter[0].Value = Convert.ToDateTime(dateE);
                    parameter[1].Value = Convert.ToDateTime(dateS);



                    #endregion

                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          "Select_StockStat",
                          ds,
                          new string[] { "T_StockStat" },
                         parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_StockStat OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_StockStat Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet Select_StockQuery(string productids)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_StockQuery]");



                    #region 传参数

                    parameter[0].Value = (productids);



                    #endregion

                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          "Select_StockQuery",
                          ds,
                          new string[] { "T_StockDetail" },
                         parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_StockDetail OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_StockDetail Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public DataSet Select_T_Product_In(string productids)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_PaperLife]");



                    #region 传参数

                    parameter[0].Value = (productids);



                    #endregion

                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          "Select_PaperLife",
                          ds,
                          new string[] { "T_Product_In" },
                         parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_Product_In OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_Product_In Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        /// <summary>
        /// 查询出库单和分录，用户加载出库单
        /// </summary>
        /// <param name="voucherno"></param>
        /// <returns></returns>
        public WMSDS Select_T_OutStockAndEntryByVoucherNOForLoad(string voucherno)
        {

            WMSDS ds = new WMSDS();

            SqlParameter par1 = new SqlParameter("@voucherno", SqlDbType.VarChar, 20);

            par1.Value = voucherno;

            SqlParameter[] pars = new SqlParameter[] { par1 };


            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_OutStockAndEntryByVoucherNOForLoad",
                        ds,
                        new string[] { "T_OutStock", "T_OutStock_Entry" },
                        pars
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Select_T_OutStockAndEntryByVoucherNOForLoad] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Select_T_OutStockAndEntryByVoucherNOForLoad] Error:" + ex.ToString()));
                }
            }
            return ds;

        }
        /// <summary>
        /// 查询销售出库单分录，看看是否有源单下推的分录
        /// </summary>
        /// <param name="voucherno"></param>
        /// <returns></returns>
        public WMSDS Select_T_OutStock_EntryBySource(string voucherno)
        {
            WMSDS ds = new WMSDS();

            SqlParameter par1 = new SqlParameter("@voucherno", SqlDbType.VarChar, 20);

            par1.Value = voucherno;

            SqlParameter[] pars = new SqlParameter[] { par1 };


            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_OutStock_EntryBySource",
                        ds,
                        new string[] { "T_OutStock_Entry" },
                        pars
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Select_T_OutStock_EntryBySource] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Select_T_OutStock_EntryBySource] Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public string Delete_T_OutStock_PlanAndEntry(string voucherno)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Delete_T_OutStock_PlanAndEntry]");

                #region 传参数

                parameter[0].Value = voucherno;
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Delete_T_OutStock_PlanAndEntry",
                          parameter);
                    return ret = "";
                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_OutStock_PlanAndEntry OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_OutStock_PlanAndEntry Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }

        public DataSet Select_T_OutStockAndEntry_RelationBySourceVoucher(string sourceVoucher)
        {

            DataSet ds = new DataSet();

            SqlParameter par1 = new SqlParameter("@sourceVoucher", SqlDbType.VarChar, 20);
            if (sourceVoucher == "")
                par1.Value = DBNull.Value;
            else
                par1.Value = sourceVoucher;
       
            SqlParameter[] pars = new SqlParameter[] { par1 };


            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_OutStockAndEntry_RelationBySourceVoucher",
                        ds,
                        new string[] { "T_OutStock", "T_OutStock_Entry" },
                        pars
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "[Select_T_OutStockAndEntry_RelationBySourceVoucher] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Select_T_OutStockAndEntry_RelationBySourceVoucher] Error:" + ex.ToString()));
                }
            }
            return ds;

        }
        /// <summary>
        /// 查询入库单的抬头，明细，统计
        /// </summary>
        /// <param name="vno"></param>
        /// <returns></returns>
        public InterfaceDS Select_T_InStockBill(string vno)
        {
            InterfaceDS sumDs = new InterfaceDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_InStockBill]");

                #region 传参数
                if (vno == "")
                    parameter[0].Value = DBNull.Value;
                else
                    parameter[0].Value = vno;
              

                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                      "Select_T_InStockBill",//  "[Select_T_Product_InDetailAndStat]",
                        sumDs,
                        new string[] { "T_InStock","CT_StockInDetail", "CT_StockIn" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_InStockBill OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_InStockBill Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }
        /// <summary>
        /// 通过事务更新入库单，先删除，在插入
        /// </summary>
        /// <param name="isRow"></param>
        /// <param name="siDS"></param>
        /// <returns></returns>
        public string Tran_Update_T_InStock(WMSDS.T_InStockRow isRow, InterfaceDS siDS)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                 string ss=   this.Delete_T_InStock(isRow.VoucherNO);

                 if (ss != "")
                     return "原单删除失败："+ss;
    

                    int inID = this.Insert_T_InStock(isRow,"");
                    //得到voucherid
                    if (inID < 0 || inID == 0)
                        return "入库单表头插入失败";
                    //2.保存分录
                    for (int i = 0; i < siDS.CT_StockIn.Rows.Count; i++)
                    {
                        WMSDS.T_InStock_EntryRow iseRow = new WMSDS().T_InStock_Entry.NewT_InStock_EntryRow();
                        iseRow.EntryID = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.EntryIDColumn].ToString());
                        iseRow.AuxCommitQty1 = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.AuxCommitQty1Column].ToString());
                        iseRow.BatchNO = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.BatchNOColumn].ToString();
                        iseRow.CommitQty = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CommitQtyColumn].ToString());
                        iseRow.CoreDiameter = Convert.ToInt16(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CoreDiameterColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CoreDiameterColumn].ToString());
                        iseRow.CustTrademark = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.CustTrademarkColumn].ToString();
                        iseRow.FiberDirect = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.FiberDirectColumn].ToString();
                        iseRow.Grade = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.GradeColumn].ToString();
                        iseRow.IsWhiteFlag = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.IsWhiteFlagColumn].ToString();
                        iseRow.Layers = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.LayersColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.LayersColumn].ToString());
                        iseRow.Length_P = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Length_PColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Length_PColumn].ToString());
                        iseRow.MaterialCode = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.MaterialCodeColumn].ToString();
                        iseRow.OrderNO = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.OrderNOColumn].ToString();
                        iseRow.PaperCert = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.PaperCertColumn].ToString();
                        iseRow.ReamPackType = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.ReamPackTypeColumn].ToString();
                        iseRow.Reams = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.ReamsColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.ReamsColumn].ToString());
                        iseRow.SKU = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SKUColumn].ToString();
                        iseRow.SlidesOfReam = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfReamColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfReamColumn].ToString());
                        iseRow.SlidesOfSheet = Convert.ToInt32(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfSheetColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SlidesOfSheetColumn].ToString());
                        iseRow.SpecCustName = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SpecCustNameColumn].ToString();
                        iseRow.SpecProdName = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.SpecProdNameColumn].ToString();
                        iseRow.StockInDate = Convert.ToDateTime(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.StockInDateColumn].ToString());
                        iseRow.TrademarkStyle = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.TrademarkStyleColumn].ToString();
                        iseRow.TransportType = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.TransportTypeColumn].ToString();
                        iseRow.VoucherID = inID;
                        iseRow.WeightMode = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.WeightModeColumn].ToString();
                        iseRow.Width_P = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_PColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_PColumn].ToString());
                        iseRow.Width_R = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_RColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Width_RColumn].ToString());
                        iseRow.Diameter = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.DiameterColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.DiameterColumn].ToString());
                        iseRow.RollLength = Convert.ToDecimal(siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.RollLengthColumn].ToString() == "" ? "0" : siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.RollLengthColumn].ToString());
                        iseRow.Cdefine1 = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Cdefine1Column].ToString();
                        iseRow.Cdefine3 = siDS.CT_StockIn.Rows[i][siDS.CT_StockIn.Cdefine3Column].ToString();
                        int ret = Insert_T_InStock_Entry(iseRow,"");
                        if (ret < 0 || ret == 0)
                            return "入库单" + iseRow.VoucherID + "分录" + iseRow.EntryID + "插入失败。";
                    }
                    //3.保存明细,同时更新productid关联
                    for (int j = 0; j < siDS.CT_StockInDetail.Rows.Count; j++)
                    {
                        WMSDS.T_InStock_ProductRow ispRow = new WMSDS().T_InStock_Product.NewT_InStock_ProductRow();
                        ispRow.InTime = Convert.ToDateTime(siDS.CT_StockInDetail.Rows[j][siDS.CT_StockInDetail.StockInDateColumn].ToString());
                        ispRow.ProductID = siDS.CT_StockInDetail.Rows[j][siDS.CT_StockInDetail.ProductIDColumn].ToString();
                        ispRow.ProductOnlyID = Convert.ToInt32(siDS.CT_StockInDetail.Rows[j][siDS.CT_StockInDetail.OnlyIDColumn].ToString());
                        ispRow.VoucherID = inID;
                        int ret = Insert_T_InStock_Product(ispRow,"");
                        if (ret < 0 || ret == 0)
                            return "入库单" + ispRow.VoucherID + "明细" + ispRow.ProductOnlyID + "插入失败。";

                    }
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }

        }
        /// <summary>
        /// 批量修改产品属性
        /// </summary>
        /// <param name="diameter"></param>
        /// <param name="length"></param>
        /// <param name="package"></param>
        /// <param name="customer"></param>
        /// <param name="remark"></param>
        /// <param name="idstr"></param>
        /// <returns></returns>
        public string Update_T_Product_InByIDs(string diameter, string length, string package, string customer, string remark, string idstr,string slidesofReam,string slidesofSheet,string color)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "update T_Product_In set ";

                if(diameter!="-1")
                    sqlstr += " DiameterLabel = '"+diameter+"' ,";
                if (length != "-1")
                    sqlstr += " LengthLabel = '" + length + "' ,";
                if (package != "-1")
                    sqlstr += " IsPolyHook = '" + package + "' ,";
                if (customer != "-1")
                    sqlstr += " CustTrademark = '" + customer + "' ,";
                if (remark != "-1")
                    sqlstr += " WHRemark = '" + remark + "' ,";

                if (slidesofReam != "-1")
                    sqlstr += " SlidesOfReam = '" + slidesofReam + "' ,";

                if (slidesofSheet != "-1")
                    sqlstr += " SlidesOfSheet = '" + slidesofSheet + "' ,";
                if (color != "-1")
                    sqlstr += " Cdefine3 = '" + color + "' ,";

                sqlstr = sqlstr.TrimEnd(',');
                sqlstr += " where onlyid in ( " + idstr + " )and (VoucherInID is null or voucherinid =0)";
                //Paper_Inspector( Inspector,Inspector_Desc,Inspector_Print,IsChoose)VALUES(@Inspector,@Inspector_Desc,@Inspector_Print,@IsChoose);";

              
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_T_Product_InByIDs OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_T_Product_InByIDs Error:" + ex.ToString()));
                    ret = "批量修改产品属性失败："+ex.Message;
                }
                return ret;
            }
        }

        public string Update_T_Factory(WMSDS.T_FactoryRow row)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_Factory]");


                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.IsFactoryNameNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.FactoryName;
                }
                if (row.IsFactoryAbbrNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.FactoryAbbr;
                }
                if (row.IsFactoryAddrNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.FactoryAddr;
                }
                if (row.IsFactoryPhoneNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.FactoryPhone;
                }
                if (row.IsMachineIDNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.MachineID;
                }
                if (row.IsIsChooseNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.IsChoose;
                }
                if (row.IsIsLocalNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.IsLocal;
                }
                parameter[7].Value = row.OnlyID;

                #endregion


	






                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Update_T_Factory]",
                        parameter);

                    //retid = Convert.ToInt32(parameter[72].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[Update_T_Factory] OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Update_T_Factory] Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }

        public string Update_T_Factory_Warehouse(WMSDS.T_Factory_WarehouseRow row)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_Factory_Warehouse]");


                #region 传参数
                
                if (row.IsCKCodeNull())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.CKCode;
                }
                if (row.IsCKNameNull())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.CKName;
                }
                if (row.IsOrgCodeNull())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.OrgCode;
                }
                if (row.IsOrgNameNull())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.OrgName;
                }
                if (row.IsFactoryAddrNull())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.FactoryAddr;
                }
                if (row.IsFactoryPhoneNull())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.FactoryPhone;
                }
                if (row.IsMachineIDNull())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.MachineID;
                }

                if (row.IsIsChooseNull())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.IsChoose;
                }
                if (row.IsIsLocalNull())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.IsLocal;
                }
                
                parameter[9].Value = row.LSBH;

                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Update_T_Factory_Warehouse]",
                        parameter);

                    //retid = Convert.ToInt32(parameter[72].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[Update_T_Factory_Warehouse] OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Update_T_Factory_Warehouse] Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }

        //public WMSDS Select_CT_StockOutForUpload(string voucherno)
        //{
        //    WMSDS ds = new WMSDS();
        //    using (SqlConnection connection = new SqlConnection(ConnctionString))
        //    {
        //        SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_OutStockAndEntry_RelationForUpload]");

        //        #region 传参数

        //        parameter[0].Value = voucherno;
        //        #endregion
        //        try
        //        {
        //            MSSqlHelper.FillDataset(connection,
        //                CommandType.StoredProcedure,
        //                "[Select_T_OutStockAndEntry_RelationForUpload]",
        //                ds,
        //              new string[] { "T_OutStock", "T_OutStock_Entry", "CT_StockOut" },
        //                parameter

        //                );
        //            OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_OutStockAndEntry_RelationForUpload OK"));
        //        }
        //        catch (Exception ex)
        //        {
        //            OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_OutStockAndEntry_RelationForUpload Error:" + ex.ToString()));
        //        }
        //    }
        //    return ds;
        //}

        public WMSQueryDS Select_T_Product_InForOutDetail(string voucherno, string dateS, string dateE)
        {
            WMSQueryDS sumDs = new WMSQueryDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Select_T_Product_InForOutDetail");

                #region 传参数
                if (voucherno == "")
                    parameter[0].Value = DBNull.Value;

                else
                    parameter[0].Value = voucherno;
                parameter[1].Value = dateS;
                parameter[2].Value = dateE;
              
              


                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_Product_InForOutDetail",
                        sumDs,
                        new string[] { "T_Product_In_Detail" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InForOutDetail OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InForOutDetail Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }

        public WMSQueryDS Select_T_Product_InForOutStat(string dateS, string dateE, string voucherNO)
        {
            WMSQueryDS sumDs = new WMSQueryDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Select_T_Product_InForOutStat");

                #region 传参数
             

                if (voucherNO == "")
                    parameter[0].Value = DBNull.Value;

                else
                    parameter[0].Value = voucherNO;
                parameter[1].Value = dateS;
                parameter[2].Value = dateE;

                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_Product_InForOutStat",
                        sumDs,
                        new string[] { "T_Product_In_Stat" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InForStat OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InForStat Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }

        public string Delete_T_OutStockAndEntry(string voucherno)
        {
            string ret = "";
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Delete_T_OutStockAndEntry]");

                #region 传参数

                parameter[0].Value = voucherno;
                #endregion

                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.StoredProcedure,
                          "Delete_T_OutStockAndEntry",
                          parameter);
                    return ret = "";
                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_T_OutStockAndEntry OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_T_OutStockAndEntry Error:" + ex.ToString()));
                    return ret + ex.Message;

                }
            }
        }
        /// <summary>
        /// 查询默认值
        /// </summary>
        /// <param name="dispType"></param>
        /// <returns></returns>
        public WMSDS Select_T_DefaultDisplay(string dispType)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = " select * from T_DefaultDisplay where 1=1 ";
                if (dispType != "")
                    sqlstr += " and dispType = @dispType ";
                sqlstr += " order by dispType  ";
                SqlParameter par1 = new SqlParameter();
                par1.Value = dispType;
                par1.DbType = DbType.String;
                par1.ParameterName = "@dispType";

                SqlParameter[] parameter = new SqlParameter[]
                {
                  par1
                };
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "T_DefaultDisplay" },
                          parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "T_DefaultDisplay OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_DefaultDisplay Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public int Insert_T_DefaultDisplay(WMSDS.T_DefaultDisplayRow row)
        {
            int retid = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Insert_T_DefaultDisplay]");

                #region 传参数
                //parameter[0].Value = row.OnlyID;
                if (row.Isdefine1Null())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.define1;
                }
                if (row.Isdefine2Null())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.define2;
                }
                if (row.Isdefine3Null())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.define3;
                }
                if (row.Isdefine4Null())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.define4;
                }
                if (row.Isdefine5Null())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.define5;
                }
                if (row.Isdefine6Null())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.define6;
                }
                if (row.Isdefine7Null())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.define7;
                }
                if (row.Isdefine8Null())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.define8;
                }
                if (row.Isdefine9Null())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.define9;
                }
                if (row.Isdefine10Null())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.define10;
                }
                if (row.IsdispTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.dispType;
                }
                parameter[11].Value =-1;

                #endregion







                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Insert_T_DefaultDisplay]",
                        parameter);

                    retid = Convert.ToInt32(parameter[11].Value);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[Insert_T_DefaultDisplay] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Insert_T_DefaultDisplay] Error:" + ex.ToString()));
                }
            }
            return retid; 
        }

        public string Update_T_DefaultDisplay(WMSDS.T_DefaultDisplayRow row)
        {
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Update_T_DefaultDisplay]");


                #region 传参数
                if (row.Isdefine1Null())
                {
                    parameter[0].Value = DBNull.Value;
                }
                else
                {
                    parameter[0].Value = row.define1;
                }
                if (row.Isdefine2Null())
                {
                    parameter[1].Value = DBNull.Value;
                }
                else
                {
                    parameter[1].Value = row.define2;
                }
                if (row.Isdefine3Null())
                {
                    parameter[2].Value = DBNull.Value;
                }
                else
                {
                    parameter[2].Value = row.define3;
                }
                if (row.Isdefine4Null())
                {
                    parameter[3].Value = DBNull.Value;
                }
                else
                {
                    parameter[3].Value = row.define4;
                }
                if (row.Isdefine5Null())
                {
                    parameter[4].Value = DBNull.Value;
                }
                else
                {
                    parameter[4].Value = row.define5;
                }
                if (row.Isdefine6Null())
                {
                    parameter[5].Value = DBNull.Value;
                }
                else
                {
                    parameter[5].Value = row.define6;
                }
                if (row.Isdefine7Null())
                {
                    parameter[6].Value = DBNull.Value;
                }
                else
                {
                    parameter[6].Value = row.define7;
                }
                if (row.Isdefine8Null())
                {
                    parameter[7].Value = DBNull.Value;
                }
                else
                {
                    parameter[7].Value = row.define8;
                }
                if (row.Isdefine9Null())
                {
                    parameter[8].Value = DBNull.Value;
                }
                else
                {
                    parameter[8].Value = row.define9;
                }
                if (row.Isdefine10Null())
                {
                    parameter[9].Value = DBNull.Value;
                }
                else
                {
                    parameter[9].Value = row.define10;
                }
                if (row.IsdispTypeNull())
                {
                    parameter[10].Value = DBNull.Value;
                }
                else
                {
                    parameter[10].Value = row.dispType;
                }
                parameter[11].Value =DBNull.Value;

                #endregion
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[Update_T_DefaultDisplay]",
                        parameter);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[Update_T_DefaultDisplay] OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[Update_T_DefaultDisplay] Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }
        /// <summary>
        /// 为红单入库查到life表中最后一个条码
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public WMSDS Select_T_Product_InForRedIn(string productid)
        {
            WMSDS sumDs = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Select_T_Product_InForRedIn");

                #region 传参数

                    parameter[0].Value = productid;

                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_Product_InForRedIn",
                        sumDs,
                        new string[] { "T_Product_In" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InForRedIn OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InForRedIn Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }
        /// <summary>
        /// 审核，反审核入库红单
        /// </summary>
        /// <param name="voucherno">红单号</param>
        /// <param name="statusin">原蓝单条码入库状态</param>
        /// <param name="ischeck">红单审核状态</param>
        /// <returns></returns>
        public string CheckInStockBillRed(string voucherno, int statusin, int ischeck)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[CheckInStockBillRed]");


                #region 传参数
                parameter[0].Value = voucherno;
                parameter[1].Value = statusin;
                parameter[2].Value = ischeck;

                #endregion
                try
                {
                    MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[CheckInStockBillRed]",
                        parameter);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[CheckInStockBillRed] OK"));
                    return "";
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[CheckInStockBillRed] Error:" + ex.ToString()));
                    return ex.Message;
                }
            }
        }

        public WMSDS Select_T_InStockBillForTransfer(string sourcevno,string vno,string business)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_InStockBillForTransfer]");

                #region 传参数

                parameter[0].Value = sourcevno;
                parameter[1].Value = vno;
                parameter[2].Value = business;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_InStockBillForTransfer]",
                        ds,
                        //new string[] { "T_InStock", "T_InStock_Entry","T_InStock_Product","T_ProductLife","T_Product_In" },
                      new string[] { "T_InStock", "T_InStock_Entry", "T_InStock_Product", "T_ProductLife", "T_Product_In" },
                      
                      parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_InStockBillForTransfer OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_InStockBillForTransfer Error:" + ex.ToString()));
                }
            }
            return ds;
        }
        /// <summary>
        /// 用事务远程发送移库出库产生的入库单，
        /// </summary>
        /// <param name="invno">本地调拨单号</param>
        /// <param name="invno">本地入库单号</param>
        /// <param name="wmsDS">入库单数据</param>
        /// <param name="conStr">远程服务器地址</param>
        /// <returns></returns>
        public string Tran_TransferInStockBill(string dbvno, string invno, WMSDS wmsDS, DataSet outStockBillInfo, string conStr)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //插入异地抬头
                    wmsDS.T_InStock.Rows[0]["VoucherNO"] = invno;
                    wmsDS.T_InStock.Rows[0]["UploadDate"] = DateTime.Now;
                    wmsDS.T_InStock.Rows[0]["IsUpload"] = "1";
                    wmsDS.T_InStock.Rows[0]["IsClose"] = "1";

                    int vid = this.Insert_T_InStock(wmsDS.T_InStock.Rows[0] as WMSDS.T_InStockRow, conStr);//得到插入的voucherid


                    if (vid < 0 || vid == 0)
                        return "Tran_TransferInStockBill抬头插入失败";


                    //插入分录
                    for (int i = 0; i < wmsDS.T_InStock_Entry.Rows.Count; i++)
                    {
                        WMSDS.T_InStock_EntryRow ospeRow = wmsDS.T_InStock_Entry.Rows[i] as WMSDS.T_InStock_EntryRow;
                        ospeRow.VoucherID = vid;
                        int entryresult = this.Insert_T_InStock_Entry(ospeRow, conStr);
                        if (entryresult < 0 || entryresult == 0)
                            return "Tran_TransferInStockBill表体插入失败";
                    }
                    //插入条码明细和明细对应
                    string TransOutBillNo = string.Empty;
                    decimal TotalAmount = 0;
                    decimal TotalWeight = 0;
                    if (outStockBillInfo.Tables.Count > 0 && outStockBillInfo.Tables[0].Rows.Count > 0)
                    {
                        TransOutBillNo = Convert.ToString(outStockBillInfo.Tables[0].Rows[0]["FTRANSOUTBILLNO"]);
                        TotalAmount = Convert.ToDecimal(outStockBillInfo.Tables[0].Rows[0]["FTOTALAMOUNT"]);
                        TotalWeight = Convert.ToDecimal(outStockBillInfo.Tables[0].Rows[0]["FTOTALWEIGHT"]);
                    }
                    for (int i = 0; i < wmsDS.T_Product_In.Rows.Count; i++)
                    {
                        //插入产品明细
                        WMSDS.T_Product_InRow inRow = wmsDS.T_Product_In.Rows[i] as WMSDS.T_Product_InRow;
                        inRow.VoucherInID = vid;
                        inRow.OnlyID = -1;
                        inRow.StatusIn = 1;
                        inRow.StatusOut = 0;
                        inRow.FTransOutBillNo = TransOutBillNo;
                        inRow.FTotalAmount = TotalAmount;
                        inRow.FTotalWeight = TotalWeight;
                        inRow.FIsInstockConfirm = "N";
                        int prodid = this.Insert_T_Product_In(inRow, conStr);//得到新插入库存条码的id
                        if (prodid < 0 || prodid == 0)
                            return "Tran_TransferInStockBill表体in插入失败";
                        //插入产品life
                        WMSDS.T_ProductLifeRow lifeRow = wmsDS.T_ProductLife.Rows[i] as WMSDS.T_ProductLifeRow;
                        lifeRow.ProductOnlyID = prodid;  //记录新插入条码的ID
                        int lifeid = this.Insert_T_ProductLife(lifeRow, conStr);
                        if (lifeid < 0 || lifeid == 0)
                            return "Tran_TransferInStockBill表体life插入失败";
                        //插入产品入库对应
                        WMSDS.T_InStock_ProductRow pRow = wmsDS.T_InStock_Product.Rows[i] as WMSDS.T_InStock_ProductRow;
                        pRow.VoucherID = vid;
                        pRow.ProductOnlyID = prodid;
                        pRow.OnlyID = -1;
                        int billPid = this.Insert_T_InStock_Product(pRow, conStr);
                        if (billPid < 0 || billPid == 0)
                            return "Tran_TransferInStockBill表体插入产品入库对应失败";
                    }
                    //更新本地已上传标记
                    //WMSDS.T_OutStockRow osRow = wmsDS.T_OutStock.NewT_OutStockRow();
                    //osRow.VoucherNO = dbvno;
                    //osRow.IsCancel = "1";//使用iscancel作为已传递调拨入库单的标记
                    //string osresault = this.Update_T_OutStock(osRow);

                    //if (osresault != "")
                    //    return "Tran_TransferInStockBill表体更新本地上传记录失败";
                    //查询本地是否有这个入库单号，主要是为了本地组织A仓移库到本地组织B仓的情况，异地组织不用查询也可以
                    //WMSDS wmsDS2 = this.Select_T_InStockByVoucherNO(invno);
                    //WMSDS wmsDS2 = this.Select_T_InStockByVoucherNO(invno, conStr);
                    //if (wmsDS2.T_InStock.Rows.Count > 0)
                    //{
                    //占用本地入库单号
                    //Insert_LocalInStock(invno);
                    //WMSDS.T_InStockRow localinRow = wmsDS.T_InStock.NewT_InStockRow();
                    //localinRow.VoucherNO = invno;
                    //int localvid = this.Insert_T_InStock(localinRow,"");//得到插入的voucherid
                    ////int localvid = this.Insert_T_InStock(localinRow, conStr);//得到插入的voucherid
                    //if (localvid < 0 || localvid == 0)
                    //    return "Tran_TransferInStockBill占用本地入库单号失败";
                    //}
                    ts.Complete();

                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }

        //写入本地入库单号
        public void Insert_LocalInStock(string invno)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                //string sqlstr = "insert into T_InStock (VoucherNO) values ('CTI270000135')";
                string sqlstr = string.Format("insert into T_InStock (VoucherNO) values ('{0}')", invno);
                try
                {
                    
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );
                    
                    OnSqlStateChange(new SqlStateEventArgs(true, "Paper_InspectorInsert OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_System_Connections Error:" + ex.ToString()));
                }
                
            }
    }

        //更新本地移库出库成功标识
        public void Update_IsTransfer(string outvno)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                
                string sqlstr = string.Format("update T_OutStock set IsCancel = '1' where VoucherNO = '{0}'", outvno);
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "T_OutStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_OutStock Error:" + ex.ToString()));
                }

            }
        }

        public void Update_IsSendToZZ(string outvno,string Flag)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = string.Format("update T_OutStock set IsSendToZZ = '{0}',SendToZZDate = '{1}' where VoucherNO = '{2}'",Flag,DateTime.Now.ToString(), outvno);
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "T_OutStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_OutStock Error:" + ex.ToString()));
                }

            }
        }

        //更新本地入库明细发送成功标识
        public void Update_IsTransfer_ForStockIn(string invno)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = string.Format("update T_InStock set IsCancel = '1' where VoucherNO = '{0}'", invno);
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "T_InStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "T_InStock Error:" + ex.ToString()));
                }

            }
        }
        public DataSet Select_OutStockInfo(string vno)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                SqlParameter[] parameter = new SqlParameter[] { new SqlParameter("@VOUCHERNO", DbType.String) };
                #region 传参数
                parameter[0].Value = vno;
                #endregion
                try
                {
                    string sql = @"SELECT OS.VOUCHERNO AS FTransOutBillNo,SUM(OSE.PLANCOMMITQTY) AS FTOTALAMOUNT,
                        SUM(OSE.PLANCOMMITAUXQTY1) AS FTOTALWEIGHT
                        FROM T_OUTSTOCK OS LEFT JOIN T_OUTSTOCK_ENTRY OSE ON OS.ONLYID=OSE.VOUCHERID
                        WHERE OS.VOUCHERNO=@VOUCHERNO
                        GROUP BY OS.VOUCHERNO";
                    ds = MSSqlHelper.ExecuteDataset(connection, CommandType.Text, sql, parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_OutStockInfo OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_OutStockInfo Error:" + ex.ToString()));
                }
            }
            return ds;
        }
        public DataSet Select_StockInConfirmInfo(string barcode)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                SqlParameter[] parameter = new SqlParameter[] { new SqlParameter("@PRODUCTID", DbType.String) };
                #region 传参数
                parameter[0].Value = barcode;
                #endregion
                try
                {
                    string sql = @"SELECT PDI0.PRODUCTID,PDI0.WEIGHTLABEL,PDI0.FTRANSOUTBILLNO,PDI0.FTOTALAMOUNT,
                    PDI0.FTOTALWEIGHT,PDI0.FISINSTOCKCONFIRM 
                    FROM T_PRODUCT_IN PDI0 INNER JOIN 
                    (SELECT PDI1.FTRANSOUTBILLNO FROM T_PRODUCT_IN PDI1  
                    WHERE PDI1.PRODUCTID=@PRODUCTID AND PDI1.FTRANSOUTBILLNO IS NOT NULL) TEMP
                    ON PDI0.FTRANSOUTBILLNO= TEMP.FTRANSOUTBILLNO";
                    ds = MSSqlHelper.ExecuteDataset(connection, CommandType.Text, sql, parameter);
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_StockInConfirmIfo OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_StockInConfirmIfo Error:" + ex.ToString()));
                }
            }
            return ds;
        }
        //0代表没有更新到数据，1代表更新成功，-1代表发生错误
        public int StockInConfirm(string barcode)
        {
            int result = -1;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                SqlParameter[] parameter = new SqlParameter[] { new SqlParameter("@PRODUCTID", DbType.String) };
                #region 传参数
                parameter[0].Value = barcode;
                #endregion
                try
                {
                    string sql =
                        @" UPDATE TPI SET TPI.FISINSTOCKCONFIRM='Y',TPI.FCONFIRMDATE =GETDATE()
                    FROM T_PRODUCT_IN TPI INNER JOIN
                    (SELECT MAX(ONLYID) AS ONLYID ,PRODUCTONLYID FROM T_PRODUCTLIFE
                    WHERE OPERATE='移库入库'  GROUP BY PRODUCTONLYID) TEMP
                    ON TPI.ONLYID = TEMP.PRODUCTONLYID AND  TPI.PRODUCTID=@PRODUCTID AND TPI.FISINSTOCKCONFIRM='N'; ";
                    int i = MSSqlHelper.ExecuteNonQuery(connection, CommandType.Text, sql, parameter);
                    if (i <= 0)
                    {
                        result = 0;
                    }
                    else
                    {
                        result = 1;
                    }
                    OnSqlStateChange(new SqlStateEventArgs(true, "StockInConfirm OK"));
                }
                catch (Exception ex)
                {
                    result = -1;
                    OnSqlStateChange(new SqlStateEventArgs(false, "StockInConfirm Error:" + ex.ToString()));
                }
            }
            return result;
        }

        public void Delete_VoucherZZ(string invno)
        {

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {

                string sqlstr = string.Format("update A_StockOutVoucher set IsDelete = '1' where VocherNO = '{0}'", invno);
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "A_StockOutVoucher OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "A_StockOutVoucher Error:" + ex.ToString()));
                }

            }
        }

        public DataSet Select_IsVoucherOperateLockedZZ(string invno)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = string.Format("select * from A_StockOutVoucher where IsDelete = '0' and IsOperateLocked = '1' and VocherNO = '{0}'", invno);
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.Text,
                        sqlstr,
                        ds,
                        new string[] { "A_StockOutVoucher" }
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "A_StockOutVoucher读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

        /// <summary>
        /// 用事务远程发送异地入库单，
        /// </summary>
        /// <param name="invno">本地调拨单号</param>
        /// <param name="wmsDS">入库单数据</param>
        /// <param name="conStr">远程服务器地址</param>
        /// <returns></returns>
        public string Tran_TransferInStockBillByInStock(string dbvno, WMSDS wmsDS, string conStr)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    //插入异地抬头
                    int vid = this.Insert_T_InStock(wmsDS.T_InStock.Rows[0] as WMSDS.T_InStockRow, conStr);//得到插入的voucherid
                    if (vid < 0 || vid == 0)
                        return "Tran_TransferInStockBill抬头插入失败";


                    //插入分录
                    for (int i = 0; i < wmsDS.T_InStock_Entry.Rows.Count; i++)
                    {
                        WMSDS.T_InStock_EntryRow ospeRow = wmsDS.T_InStock_Entry.Rows[i] as WMSDS.T_InStock_EntryRow;
                        ospeRow.VoucherID = vid;
                        int entryresult = this.Insert_T_InStock_Entry(ospeRow, conStr);
                        if (entryresult < 0 || entryresult == 0)
                            return "Tran_TransferInStockBill表体插入失败";
                    }
                    //插入条码明细和明细对应
                    for (int i = 0; i < wmsDS.T_Product_In.Rows.Count; i++)
                    {
                        //插入产品明细
                        WMSDS.T_Product_InRow inRow = wmsDS.T_Product_In.Rows[i] as WMSDS.T_Product_InRow;
                        inRow.VoucherInID = vid;
                        inRow.SourcePID = 0;
                        inRow.OnlyID = -1;
                        int prodid = this.Insert_T_Product_In(inRow, conStr);//得到新插入库存条码的id
                        if (prodid < 0 || prodid == 0)
                            return "Tran_TransferInStockBill表体in插入失败";
                        //插入产品life
                        WMSDS.T_ProductLifeRow lifeRow = wmsDS.T_ProductLife.Rows[i] as WMSDS.T_ProductLifeRow;
                        lifeRow.ProductOnlyID = prodid;  //记录新插入条码的ID
                        int lifeid = this.Insert_T_ProductLife(lifeRow, conStr);
                        if (lifeid < 0 || lifeid == 0)
                            return "Tran_TransferInStockBill表体life插入失败";
                        //插入产品入库对应
                        WMSDS.T_InStock_ProductRow pRow = wmsDS.T_InStock_Product.Rows[i] as WMSDS.T_InStock_ProductRow;
                        pRow.VoucherID = vid;
                        pRow.ProductOnlyID = prodid;
                        int billPid = this.Insert_T_InStock_Product(pRow, conStr);
                        if (billPid < 0 || billPid == 0)
                            return "Tran_TransferInStockBill表体插入产品入库对应失败";
                    }
                    //更新本地已上传标记
                    //WMSDS.T_InStockRow osRow = wmsDS.T_InStock.NewT_InStockRow();
                    //osRow.VoucherNO = dbvno;
                    //osRow.IsCancel = "1";//使用iscancel作为已传递入库单的标记
                    //string osresault = this.Update_T_InStock(osRow);
                    //if (osresault != "")
                    //    return "Tran_TransferInStockBill表体更新本地上传记录失败";
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }
        /// <summary>
        /// 查询生产数据库中的生产服务器连接串
        /// </summary>
        /// <param name="factory">为空查询全部</param>
        /// <returns></returns>
        public ProduceDS Select_System_Connections(string factory)
        {
            ProduceDS ds = new ProduceDS();
            using (SqlConnection connection = new SqlConnection(MESConnectString))
            {
                string sqlstr = "select * from System_Connections where isuse =1";
                if(factory!="")
                    sqlstr+=" and machineid ='"+factory+"'";
               sqlstr+=" order by machineid";
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "System_Connections" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_System_Connections OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_System_Connections Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public ProduceDS Select_Paper_Destination(string factoryStr)
        {
            ProduceDS ds = new ProduceDS();
            using (SqlConnection connection = new SqlConnection(factoryStr))
            {
                string sqlstr = "select * from Paper_Destination ";
               
                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          ds,
                          new string[] { "Paper_Destination" }
                          );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_Paper_Destination OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_Paper_Destination Error:" + ex.ToString()));
                }
                return ds;
            }
        }

        public string Insert_System_CrossFactory_TransCommands(string barcode, string source, string accept, string dest, string operate, string ip, string datetime)
        {

            string retid = "";
            using (SqlConnection connection = new SqlConnection(MESConnectString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[CrossFactory_TransCommands_Insert]");

                #region 传参数
                parameter[0].Value = barcode;
                parameter[1].Value = source;
                parameter[2].Value = accept;
                parameter[3].Value = dest;
                parameter[4].Value = operate;
                parameter[5].Value = ip;
                parameter[6].Value = "N";
                parameter[7].Value = DBNull.Value;
                parameter[8].Value = DBNull.Value;
                parameter[9].Value = DBNull.Value;

                #endregion

                try
                {
                     MSSqlHelper.ExecuteNonQuery(connection,
                        CommandType.StoredProcedure,
                        "[CrossFactory_TransCommands_Insert]",
                        parameter);

                    OnSqlStateChange(new SqlStateEventArgs(true, "[CrossFactory_TransCommands_Insert] OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "[CrossFactory_TransCommands_Insert] Error:" + ex.ToString()));
                    return "保存失败："+ex.Message;
                }
            }
            return retid; 
        }

        public WMSDS Select_T_InStockDetailForTransfer(string vno)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_InStockDetailForTransfer]");

                #region 传参数

                parameter[0].Value = vno;
                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_InStockDetailForTransfer]",
                        ds,
                        //new string[] { "T_InStock", "T_InStock_Entry","T_InStock_Product","T_ProductLife","T_Product_In" },
                      new string[] {  "T_ProductLife", "T_Product_In" },

                      parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_InStockDetailForTransfer OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_InStockDetailForTransfer Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        public string Tran_TransferInStockDetail(string voucherno,WMSDS wmsDS, string conStr)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    ////插入异地抬头
                    //wmsDS.T_InStock.Rows[0]["VoucherNO"] = invno;

                    //int vid = this.Insert_T_InStock(wmsDS.T_InStock.Rows[0] as WMSDS.T_InStockRow, conStr);//得到插入的voucherid
                    //if (vid < 0 || vid == 0)
                    //    return "Tran_SaveInStockBill抬头插入失败";


                    //插入分录
                    //for (int i = 0; i < wmsDS.T_InStock_Entry.Rows.Count; i++)
                    //{
                    //    WMSDS.T_InStock_EntryRow ospeRow = wmsDS.T_InStock_Entry.Rows[i] as WMSDS.T_InStock_EntryRow;
                    //    ospeRow.VoucherID = vid;
                    //    int entryresult = this.Insert_T_InStock_Entry(ospeRow, conStr);
                    //    if (entryresult < 0 || entryresult == 0)
                    //        return "Tran_SaveNewOutStock表体插入失败";
                    //}
                    //插入条码明细和明细对应
                    for (int i = 0; i < wmsDS.T_Product_In.Rows.Count; i++)
                    {
                        //插入产品明细
                        WMSDS.T_Product_InRow inRow = wmsDS.T_Product_In.Rows[i] as WMSDS.T_Product_InRow;
                        //inRow.VoucherInID = vid.ToString();
                        int prodid = this.Insert_T_Product_In(inRow, conStr);//得到新插入库存条码的id
                        if (prodid < 0 || prodid == 0)
                            return "Tran_TransferInStockDetail表体in插入失败";
                        //插入产品life
                        WMSDS.T_ProductLifeRow lifeRow = wmsDS.T_ProductLife.Rows[i] as WMSDS.T_ProductLifeRow;
                        lifeRow.ProductOnlyID = prodid;  //记录新插入条码的ID
                        int lifeid = this.Insert_T_ProductLife(lifeRow, conStr);
                        if (lifeid < 0 || lifeid == 0)
                            return "Tran_TransferInStockDetail表体life插入失败";
                        ////插入产品入库对应
                        //WMSDS.T_InStock_ProductRow pRow = wmsDS.T_InStock_Product.Rows[i] as WMSDS.T_InStock_ProductRow;
                        //pRow.VoucherID = vid;
                        //pRow.ProductOnlyID = prodid;
                        //int billPid = this.Insert_T_InStock_Product(pRow, conStr);
                        //if (billPid < 0 || billPid == 0)
                        //    return "Tran_SaveNewOutStock表体插入产品入库对应失败";
                    }
                    //更新本地已传输标记
                    WMSDS.T_InStockRow osRow = wmsDS.T_InStock.NewT_InStockRow();
                    osRow.VoucherNO = voucherno;
                    osRow.IsCancel = "1";//使用iscancel作为已传递调拨入库单的标记
                    string osresault = this.Update_T_InStock(osRow);
                    if (osresault != "")
                        return "Tran_TransferInStockDetail表体更新本地上传记录失败";
                    ////查询本地是否有这个入库单号，主要是为了本地组织A仓移库到本地组织B仓的情况，异地组织不用查询也可以
                    //WMSDS wmsDS2 = this.Select_T_InStockByVoucherNO(invno);
                    //if (wmsDS2.T_InStock.Rows.Count == 0)
                    //{
                    //    //占用本地入库单号
                    //    WMSDS.T_InStockRow localinRow = wmsDS.T_InStock.NewT_InStockRow();
                    //    localinRow.VoucherNO = invno;
                    //    int localvid = this.Insert_T_InStock(localinRow);//得到插入的voucherid
                    //    if (localvid < 0 || localvid == 0)
                    //        return "Tran_SaveInStockBill占用本地入库单号失败";
                    //}
                    ts.Complete();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //写入日志
                return ex.Message;
            }
        }
        /// <summary>
        /// 通过产品的onlyid，查找对应入库单的抬头
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public WMSDS Select_T_Product_InAndT_InStock(string productid)
        {
            WMSDS sumDs = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Select_T_Product_InAndT_InStock");

                #region 传参数

                parameter[0].Value = productid;


                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_Product_InAndT_InStock",
                        sumDs,
                        new string[] { "T_Product_In","T_InStock" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InAndT_InStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InAndT_InStock Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }
        /// <summary>
        /// 通过产品的onlyid，查找对应出库单的抬头
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public WMSDS Select_T_Product_InAndT_OutStock(string productid)
        {
            WMSDS sumDs = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "Select_T_Product_InAndT_OutStock");

                #region 传参数

                parameter[0].Value = productid;


                #endregion

                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "Select_T_Product_InAndT_OutStock",
                        sumDs,
                        new string[] { "T_Product_In", "T_OutStock" },
                        parameter
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_Product_InAndT_OutStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_Product_InAndT_OutStock Error:" + ex.ToString()));
                }
            }
            return sumDs;
        }

        public WMSDS Select_T_InStockBillForTransferInStock(string vno)
        {
            WMSDS ds = new WMSDS();
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                SqlParameter[] parameter = SqlHelperParameterCache.GetSpParameterSet(connection, "[Select_T_InStockBillForTransferInStock]");

                #region 传参数

                parameter[0].Value = vno;

                #endregion
                try
                {
                    MSSqlHelper.FillDataset(connection,
                        CommandType.StoredProcedure,
                        "[Select_T_InStockBillForTransferInStock]",
                        ds,
                        //new string[] { "T_InStock", "T_InStock_Entry","T_InStock_Product","T_ProductLife","T_Product_In" },
                      new string[] { "T_InStock", "T_InStock_Entry", "T_InStock_Product", "T_ProductLife", "T_Product_In" },

                      parameter

                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "Select_T_InStockBillForTransferInStock OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_T_InStockBillForTransferInStock Error:" + ex.ToString()));
                }
            }
            return ds;
        }

        //保存用户操作记录
        public void SaveOperateLog(string Operator, DateTime OperateTime, string Description,string Remark1,string Remark2,string VoucherID)
        {
            //bool ret = false;
            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sqlstr = "INSERT INTO T_OperationLog( Operator,OperateTime,Description,Remark1,Remark2,VoucherID)VALUES(@Operator,@OperateTime,@Description,@Remark1,@Remark2,@VoucherID);";

                SqlParameter[] parameter = new SqlParameter[]
                {
                new SqlParameter("@Operator", Operator),
                new SqlParameter("@OperateTime", OperateTime),
                new SqlParameter("@Description", Description),
                new SqlParameter("@Remark1", Remark1),
                new SqlParameter("@Remark2", Remark2),
                new SqlParameter("@VoucherID", VoucherID),

                };
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr,
                          parameter);
                    //ret = true;
                   //OnSqlStateChange(new SqlStateEventArgs(true, "SaveOperateLog OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "SaveOperateLog Error:" + ex.ToString()));
                }
                //return ret;
            }
        }
        public void UpdateSelectedRowInfo(string VoucherID, int EntryID, WMSDS.T_OutStock_EntryRow ospeRow)
        {
            int count = 0;
            ospeRow.VoucherID = int.Parse(VoucherID);//其实是OnlyID

            using (SqlConnection connection = new SqlConnection(ConnctionString))
            {
                string sql = "select count(*) from T_OutStock_Entry " + " WHERE EntryID = '" + EntryID.ToString() + "'" + "AND VoucherID = '" + VoucherID.ToString() + "'";
                try
                {
                    connection.Open();
                    count = (int)MSSqlHelper.ExecuteScalar(connection,//tran,
                         CommandType.Text,
                         sql);
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Select_Paper_Destination Error:" + ex.ToString()));
                }

            //WMSDS.T_OutStock_EntryRow ospeRow = dr.Find(findTheseVals) as WMSDS.T_OutStock_EntryRow;

           
                if (count == 0)//判断T_OutStock_Entry表中是否有这条分录信息//没有则插入
                {
                    int entryresult = this.Insert_T_OutStock_EntryByRow(ospeRow);
                    if (entryresult < 0)
                        MessageBox.Show("Tran_SaveNewOutStockPlan分录" + EntryID + "插入失败");
                    else
                    {
                        MessageBox.Show("成功新增一条分录!");
                    }
                }

                else  //存在则更新
                {
                    #region sqlstr
                    string sqlstr = "UPDATE T_OutStock_Entry SET ";

                    if (!ospeRow.IsMaterialCodeNull())
                        sqlstr = sqlstr + " MaterialCode = @MaterialCode,";
                    if (!ospeRow.IsMaterialNameNull())
                        sqlstr = sqlstr + " MaterialName = @MaterialName,";
                    if (!ospeRow.IsSpecificationNull())
                        sqlstr = sqlstr + " Specification = @Specification,";
                    if (!ospeRow.IsGradeNull())
                        sqlstr = sqlstr + " Grade = @Grade,";
                    if (!ospeRow.IsWeightModeNull())
                        sqlstr = sqlstr + " WeightMode = @WeightMode,";
                    if (!ospeRow.IsCoreDiameterNull())
                        sqlstr = sqlstr + " CoreDiameter = @CoreDiameter,";
                    if (!ospeRow.IsDiameterNull())
                        sqlstr = sqlstr + " Diameter = @Diameter,";
                    else
                        sqlstr = sqlstr + " Diameter = NULL,";
                    if (!ospeRow.IsRollLengthNull())
                        sqlstr = sqlstr + " RollLength = @RollLength,";
                    if (!ospeRow.IsCdefine3Null())
                        sqlstr = sqlstr + " Cdefine3 = @Cdefine3,";
                    if (!ospeRow.IsReamPackTypeNull())
                        sqlstr = sqlstr + " ReamPackType = @ReamPackType,";
                    if (!ospeRow.IsSKUNull())
                        sqlstr = sqlstr + " SKU = @SKU,";
                    if (!ospeRow.IsPaperCertNull())
                        sqlstr = sqlstr + " PaperCert = @PaperCert,";
                    if (!ospeRow.IsSpecProdNameNull())
                        sqlstr = sqlstr + " SpecProdName = @SpecProdName,";
                    if (!ospeRow.IsSpecCustNameNull())
                        sqlstr = sqlstr + " SpecCustName = @SpecCustName,";
                    if (!ospeRow.IsTrademarkStyleNull())
                        sqlstr = sqlstr + " TrademarkStyle = @TrademarkStyle,";
                    if (!ospeRow.IsIsWhiteFlagNull())
                        sqlstr = sqlstr + " IsWhiteFlag = @IsWhiteFlag,";
                    if (!ospeRow.IsOrderNONull())
                        sqlstr = sqlstr + " OrderNO = @OrderNO,";
                    if (!ospeRow.IsRemarkNull())
                        sqlstr = sqlstr + " Remark = @Remark,";
                    if (!ospeRow.IsSourceVoucherIDNull())
                        sqlstr = sqlstr + " SourceVoucherID = @SourceVoucherID,";
                    if (!ospeRow.IsPlanQtyNull())
                        sqlstr = sqlstr + " PlanQty = @PlanQty,";
                    if (!ospeRow.IsPlanAuxQty1Null())
                        sqlstr = sqlstr + " PlanAuxQty1 = @PlanAuxQty1,";
                    if (!ospeRow.IsPlanCommitQtyNull())
                        sqlstr = sqlstr + " PlanCommitQty = @PlanCommitQty,";
                    if (!ospeRow.IsPlanCommitAuxQty1Null())
                        sqlstr = sqlstr + " PlanCommitAuxQty1 = @PlanCommitAuxQty1,";
                    sqlstr = sqlstr.TrimEnd(new char[] { ' ', ',' });
                    sqlstr = sqlstr + " WHERE EntryID = '" + EntryID.ToString() + "'" + "AND VoucherID = '" + VoucherID.ToString() + "'";
                    #endregion

                    #region parameter
                    SqlParameter[] parameter = new SqlParameter[]
                                {
                                
                                new SqlParameter("@MaterialCode",ospeRow.IsMaterialCodeNull()?DBNull.Value:(object)ospeRow.MaterialCode),
                                new SqlParameter("@MaterialName",ospeRow.IsMaterialNameNull()?DBNull.Value:(object)ospeRow.MaterialName),
                                new SqlParameter("@Specification",ospeRow.IsSpecificationNull()?DBNull.Value:(object)ospeRow.Specification),
                                new SqlParameter("@Grade",ospeRow.IsGradeNull()?DBNull.Value:(object)ospeRow.Grade),
                                new SqlParameter("@WeightMode",ospeRow.IsWeightModeNull()?DBNull.Value:(object)ospeRow.WeightMode),
                                new SqlParameter("@CoreDiameter",ospeRow.IsCoreDiameterNull()?DBNull.Value:(object)ospeRow.CoreDiameter),
                                new SqlParameter("@Diameter",ospeRow.IsDiameterNull()?DBNull.Value:(object)ospeRow.Diameter),
                                new SqlParameter("@RollLength",ospeRow.IsRollLengthNull()?DBNull.Value:(object)ospeRow.RollLength),  
 
                                new SqlParameter("@Cdefine3",ospeRow.IsCdefine3Null()?DBNull.Value:(object)ospeRow.Cdefine3),
                                new SqlParameter("@ReamPackType",ospeRow.IsReamPackTypeNull()?DBNull.Value:(object)ospeRow.ReamPackType),
                                new SqlParameter("@SKU",ospeRow.IsSKUNull()?DBNull.Value:(object)ospeRow.SKU),
                                new SqlParameter("@PaperCert",ospeRow.IsPaperCertNull()?DBNull.Value:(object)ospeRow.PaperCert),
                                new SqlParameter("@SpecProdName",ospeRow.IsSpecProdNameNull()?DBNull.Value:(object)ospeRow.SpecProdName),
                                new SqlParameter("@SpecCustName",ospeRow.IsSpecCustNameNull()?DBNull.Value:(object)ospeRow.SpecCustName),
                                new SqlParameter("@TrademarkStyle",ospeRow.IsTrademarkStyleNull()?DBNull.Value:(object)ospeRow.TrademarkStyle),
                                new SqlParameter("@IsWhiteFlag",ospeRow.IsIsWhiteFlagNull()?DBNull.Value:(object)ospeRow.IsWhiteFlag),
                                new SqlParameter("@OrderNO",ospeRow.IsOrderNONull()?DBNull.Value:(object)ospeRow.OrderNO),   

                                new SqlParameter("@SourceVoucherID",ospeRow.IsSourceVoucherIDNull()?DBNull.Value:(object)ospeRow.SourceVoucherID),
                                new SqlParameter("@Remark",ospeRow.IsRemarkNull()?DBNull.Value:(object)ospeRow.Remark),  
                                new SqlParameter("@PlanQty",ospeRow.IsPlanQtyNull()?DBNull.Value:(object)ospeRow.PlanQty),
                                new SqlParameter("@PlanAuxQty1",ospeRow.IsPlanAuxQty1Null()?DBNull.Value:(object)ospeRow.PlanAuxQty1),
                                new SqlParameter("@PlanCommitQty",ospeRow.IsPlanCommitQtyNull()?DBNull.Value:(object)ospeRow.PlanCommitQty),
                                new SqlParameter("@PlanCommitAuxQty1",ospeRow.IsPlanCommitAuxQty1Null()?DBNull.Value:(object)ospeRow.PlanCommitAuxQty1)
                                
                                };
                    #endregion

                    try
                    {
                        
                        //connection.Open();//已经打开过了
                        MSSqlHelper.ExecuteNonQuery(connection,
                          CommandType.Text,
                         sqlstr,
                         parameter);
                        OnSqlStateChange(new SqlStateEventArgs(true, "PaperUserUpdateAllByPK执行成功"));
                        MessageBox.Show("分录更新成功!");
                    }
                    catch (Exception ex)
                    {
                        OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));

                    }
                }
            }
        }

        public DataSet ReadWHDataFromZZ()
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(ZZConnctionString))
            {

                try
                {
                    connection.Open();
                    MSSqlHelper.FillDataset(connection,//tran,
                           CommandType.StoredProcedure,
                          "[FindProductInfoForERPLoad]",
                          ds,
                          new string[] { "Roll_Product" }
                          );

                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }


        public void Insert_IsUpdatedToZZ(string rollid)
        {

            using (SqlConnection connection = new SqlConnection(ZZConnctionString))
            {
                string sqlstr = string.Format("INSERT INTO [ERP_ProductUpload]([ProductID],[IsUpload],[UploadTime],[CreateTime]) VALUES('{0}','1','{1}','{2}')", rollid,DateTime.Now.ToString(),DateTime.Now.ToString());
                try
                {

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Insert_IsUpdatedToZZ OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Insert_IsUpdatedToZZ Error:" + ex.ToString()));
                }

            }
        }

        public void Update_IsUpdatedToZZ(string rollid)
        {

            using (SqlConnection connection = new SqlConnection(ZZConnctionString))
            {
                string sqlstr = string.Format("Update [RackWMS].[dbo].[ERP_ProductUpload] set IsUpload = '1' where  ProductID = '{0}' ", rollid);
                try
                {
                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Update_IsUpdatedToZZ OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Update_IsUpdatedToZZ Error:" + ex.ToString()));
                }

            }
        }

        public void Delete_IsUpdatedToZZ(string rollid)
        {

            using (SqlConnection connection = new SqlConnection(ZZConnctionString))
            {
                string sqlstr = string.Format("Delete From [RackWMS].[dbo].[ERP_ProductUpload] where ProductID = '{0}'", rollid);
                try
                {

                    connection.Open();
                    MSSqlHelper.ExecuteNonQuery(connection,//tran,
                           CommandType.Text,
                          sqlstr
                          );

                    OnSqlStateChange(new SqlStateEventArgs(true, "Delete_IsUpdatedToZZ OK"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, "Delete_IsUpdatedToZZ Error:" + ex.ToString()));
                }

            }
        }


    }

}

