using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Threading;
using CTWH.Common;
using DataModel;
namespace CTWH
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        CTWH.Common.MSSQL.MSSQLAccess access= null;
        //服务器时间同步
        System.Windows.Forms.Timer servertimer = new System.Windows.Forms.Timer();

        public Login()
        {
            InitializeComponent();
            this.Text = "登陆";
            access = Utils.MSSqlAccess;
            this.tabbedControlGroup1.SelectedTabPageIndex = 0;

            this.txtUserPassword.KeyUp += new KeyEventHandler(txtUserPassword_KeyUp);

            this.txtTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtTime_ButtonClick);
            SetServerTime();
            servertimer.Interval = 10000;
            servertimer.Tick += new EventHandler(servertimer_Tick);
        }
        string[] _args;
        public Login(string[] args)
        {
            InitializeComponent();
            this.Text = "登陆";
            access = Utils.MSSqlAccess;

            //设置图片



            this.tabbedControlGroup1.SelectedTabPageIndex = 0;


            this.txtUserPassword.KeyUp += new KeyEventHandler(txtUserPassword_KeyUp);

            this.txtTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtTime_ButtonClick);
            SetServerTime();
            servertimer.Interval = 10000;
            servertimer.Tick += new EventHandler(servertimer_Tick);
            this._args = args;
            if (args.Length > 2)
            {
                this.txtUserName.EditValue = args[0];
                this.txtUserPassword.EditValue = args[1];
            }

        }
        void txtTime_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SetLocalTime(SetServerTime());

            this.labelControl1.Text = "与服务器时间同步完成！";
        }

        void servertimer_Tick(object sender, EventArgs e)
        {

            //Time  设置服务器时间
            SetServerTime();
        }

        /// <summary>
        /// 显示并返回服务器时间
        /// </summary>
        /// <returns>服务器当前时间</returns>
        private DateTime SetServerTime()
        {
            DateTime servertime = System.DateTime.Now;
            DataSet ds = access.ServerInfoQuery();
            if (ds.Tables.Count > 0)
            {
                servertime = Convert.ToDateTime(ds.Tables[0].Rows[0]["ServerDate"]);
            }
            this.txtTime.Text = servertime.ToString("f");

            return servertime;
        }

        void txtUserPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                this.btnLogin.Focus();
        }



       

        Boolean IsConnect = false;
        private void InitTest()
        {
            DbConnection conn = new SqlConnection();
            conn.ConnectionString = Utils.SQLConnectionString;
            try
            {
                conn.Open();
                IsConnect = true;

            }
            catch
            {
                IsConnect = false;
                if (this.InvokeRequired)
                    this.Invoke(new ShowMessageDelegate(ShowMessage), "不能建立数据库连接,请配置");
                return;
            }
            finally
            {
                conn.Close();
                if (this.InvokeRequired)
                    this.Invoke(new SetControlsDelegate(RefreshControls));
                if (IsConnect)
                {
                    //this.DialogResult = DialogResult.OK;
                    if (this.InvokeRequired)
                        this.Invoke(new SetControlsDelegate(InitPage));


                }
            }
        }

        private void InitPage()
        {
            //Load Data
            if (IsConnect)
            {
                this.tabbedControlGroup1.SelectedTabPageIndex = 0;

                servertimer.Enabled = true;

                //匹配时间
                DateTime sercertime = SetServerTime();
                TimeSpan tspan = sercertime.Subtract(DateTime.Now);
                double minus = tspan.Duration().TotalMinutes;
                if (minus > 1)
                {
                    this.labelControl1.Text = "本地时间和服务器时间相差" + minus.ToString("f0") + "分,请同步！";
                }

                if (this._args != null && this._args.Length > 2)
                {
                    this.LoginFunc();

                }
            }
            else
            {
                this.tabbedControlGroup1.SelectedTabPageIndex = 1;

                servertimer.Enabled = false;
            }

        }

        Thread Testth;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Testth = new Thread(new ThreadStart(thdo));
            SetControlsEnable(false);
            Testth.Start();
        }


        private void SetControlsEnable(bool IsEnable)
        {
            //登陆
            this.btnCCOK.Enabled = IsEnable;

            this.txtUserName.Enabled = IsEnable;
            this.txtUserPassword.Enabled = IsEnable;
            this.btnLogin.Enabled = IsEnable;


            this.simpleButton1.Enabled = IsEnable;
            this.simpleButton2.Enabled = IsEnable;

            this.txtServerName.Enabled = IsEnable;
            this.txtDBName.Enabled = IsEnable;
            this.txtLoginName.Enabled = IsEnable;
            this.txtPassWord.Enabled = IsEnable;

            this.marqueeProgressBarControl1.Properties.Stopped = IsEnable;

            if (IsEnable)
                this.simpleButton3.Text = "退出";
            else
                this.simpleButton3.Text = "取消";

            this.simpleButton2.Enabled = IsConnect;
        }


        private delegate void SetControlsDelegate();
        private delegate void ShowMessageDelegate(object obj);
        private void thdo()
        {
            DbConnection conn = new SqlConnection();
            //设置输入参数
            //////////Utils.SQLServer = this.txtServerName.Text.Trim();
            //////////Utils.SQLDataBase = this.txtDBName.Text.Trim();
            //////////Utils.SQLUserID = this.txtLoginName.Text.Trim();
            //////////Utils.SQLPassword = this.txtPassWord.Text.Trim();

            conn.ConnectionString = Utils.SQLConnectionString;
            try
            {
                conn.Open();
                IsConnect = true;
                if (this.InvokeRequired)
                    this.Invoke(new ShowMessageDelegate(ShowMessage), "连接成功");
            }
            catch
            {
                IsConnect = false;
                if (this.InvokeRequired)
                    this.Invoke(new ShowMessageDelegate(ShowMessage), "不能建立数据库连接");
                return;
            }
            finally
            {
                conn.Close();
                if (this.InvokeRequired)
                    this.Invoke(new SetControlsDelegate(RefreshControls));


            }
        }
        private void ShowMessage(object msg)
        {
            MessageBox.Show(this, msg.ToString(), "提示");
        }


        private void RefreshControls()
        {
            SetControlsEnable(true);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (Testth != null && Testth.IsAlive)
                Testth.Abort();
            else
                this.DialogResult = DialogResult.Cancel;

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            // iniOperate.WriteValue("SQL", "ConnectionString", getConnectionString());

            //this.txtServerName.EditValue = Utils.SQLServer;
            //this.txtDBName.EditValue = Utils.SQLDataBase;
            //this.txtLoginName.EditValue = Utils.SQLUserID;
            //this.txtPassWord.EditValue = Utils.SQLPassword;

            //保存数据库信息 
            Utils.MyAppConfig.SetValue("SQL", "Server", this.txtServerName.Text.Trim());
            Utils.MyAppConfig.SetValue("SQL", "DataBase", this.txtDBName.Text.Trim());
            Utils.MyAppConfig.SetValue("SQL", "UserID", this.txtLoginName.Text.Trim());

            //加密
            //MySecurity ms = new MySecurity();
            //string enstring = ms.RijndaelEncrypt(this.txtPassWord.Text.Trim());
            string enstring = this.txtPassWord.Text.Trim();
            Utils.MyAppConfig.SetValue("SQL", "Password", enstring);

            // 

            this.DialogResult = DialogResult.Yes;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.LoginFunc();


        }
        private void LoginFunc()
        {

            string usercode = this.cmb_Usercode.EditValue == null ? "" : this.cmb_Usercode.EditValue.ToString();
            string password = this.txtUserPassword.EditValue == null ? "" : this.txtUserPassword.EditValue.ToString();
            usercode = usercode.Trim().Trim().ToLower();
            password = password.Trim().ToLower();
            if (usercode == "" || password == "")
            {
                XtraMessageBox.Show("用户名或密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtUserName.Focus();
                return;

            }
            //加密
            // MySecurity ms = new MySecurity();
            // password = ms.RijndaelEncrypt(password);

            WMSDS.T_UserDataTable t_UserDataTable = access.T_UserQueryByCodeAndPassword(usercode, password).T_User;
            //WMSDS.T_FactoryDataTable t_Factory = access.Select_T_Factory(false,true).T_Factory;
            //if (t_Factory.Rows.Count == 0) {
            //    MessageBox.Show("请设置本地机台号");
            //    return;
            //}
            if (t_UserDataTable.Rows.Count > 0)
            {
                WMSDS.T_UserRow pprow = t_UserDataTable.Rows[0] as WMSDS.T_UserRow;
                //保存用户信息
                Utils.LoginUserName =pprow.UserCode+"."+ pprow.UserName;
                Utils.LoginUserPassword = pprow.Password;
                Utils.LoginUserType = pprow.IsUserTypeIDNull() ? "" : pprow.UserTypeID.ToString();
                ///HM
                Utils.LoginUserShift = pprow.IsShiftIDNull() ? "" : pprow.ShiftID.ToString();
                ///
                //Utils.LoginMachineID =t_Factory.Rows
                //Utils.LoginUserpermission = pprow.IsPermissionNull() ? Utils.LoginUserpermission : pprow.Permission;
                ////////////if (!pprow.IsPermissionNull())
                ////////////    Utils.LoginUserPermission = pprow.Permission;
                ////////////else
                ////////////{
                ////////////    Utils.LoginUserpermission = "";
                ////////////}
                //MessageBox.Show("a");
                this.DialogResult = DialogResult.OK;
                //MessageBox.Show("b");

            }
            else
            {
                XtraMessageBox.Show("用户名或密码错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtPassWord.Focus();

                return;

            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMiliseconds;
        }


        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SystemTime sysTime);


        public static bool SetLocalTime(DateTime time)
        {
            bool flag = false;
            SystemTime sysTime = new SystemTime();

            sysTime.wYear = Convert.ToUInt16(time.Year);//Convert.ToUInt16(SysTime.Substring(0,4));
            sysTime.wMonth = Convert.ToUInt16(time.Month);// Convert.ToUInt16(SysTime.Substring(4, 2));
            sysTime.wDay = Convert.ToUInt16(time.Day);// Convert.ToUInt16(SysTime.Substring(6, 2));
            sysTime.wHour = Convert.ToUInt16(time.Hour);// Convert.ToUInt16(SysTime.Substring(8, 2));
            sysTime.wMinute = Convert.ToUInt16(time.Minute);// Convert.ToUInt16(SysTime.Substring(10, 2));
            sysTime.wSecond = Convert.ToUInt16(time.Second);// Convert.ToUInt16(SysTime.Substring(12, 2));
            //注意：
            //结构体的wDayOfWeek属性一般不用赋值，函数会自动计算，写了如果不对应反而会出错
            //wMiliseconds属性默认值为一，可以赋值
            try
            {
                flag = SetLocalTime(ref sysTime);
            }
            //由于不是C#本身的函数，很多异常无法捕获
            //函数执行成功则返回true，函数执行失败返回false
            //经常不返回异常，不提示错误，但是函数返回false，给查找错误带来了一定的困难
            catch (Exception ex1)
            {
                MessageBox.Show("SetLocalTime函数执行异常" + ex1.Message);
            }

            return flag;
        }



        //修改密码部分
        private void btnCCOK_Click(object sender, EventArgs e)
        {
            string usercode = this.txtCUserName.Text.Trim().ToLower();
            string opwd = this.txtCOPWD.Text.Trim().ToLower();
            string npwd = this.txtCNPWD.Text.Trim().ToLower();
            string cpwd = this.txtCCPWD.Text.Trim().ToLower();

            if (usercode == "" || opwd == "" || npwd == "")
            {
                XtraMessageBox.Show("用户名或密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtCUserName.Focus();
                return;

            }


            if (npwd != cpwd)
            {
                XtraMessageBox.Show("新密码和确认密码不相同，请重新输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtCNPWD.Focus();
                return;

            }

            //加密
            //MySecurity ms = new MySecurity();

            //opwd = ms.RijndaelEncrypt(opwd);
            //npwd = ms.RijndaelEncrypt(npwd);
            //cpwd = ms.RijndaelEncrypt(cpwd);

            WMSDS.T_UserDataTable t_UserDataTable = access.T_UserQueryByCodeAndPassword(usercode, opwd).T_User;


            if (t_UserDataTable.Rows.Count > 0)
            {

                WMSDS.T_UserRow userrow = t_UserDataTable.NewT_UserRow();
                userrow.UserCode = usercode;
                userrow.Password = npwd;

                //允许修改
                if (access.T_UserUpdateAllByPK(userrow,false))
                {
                    XtraMessageBox.Show("密码修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //清空
                    this.txtCCPWD.EditValue = "";
                    this.txtCNPWD.EditValue = "";
                    this.txtCOPWD.EditValue = "";
                    this.txtCUserName.EditValue = "";
                    this.txtUserName.EditValue = usercode;
                    this.tabbedControlGroup1.SelectedTabPageIndex = 0;
                }

            }
            else
            {
                XtraMessageBox.Show("原用户名或密码错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtPassWord.Focus();

                return;

            }

        }

        private void btnChangePWD_Click(object sender, EventArgs e)
        {
            //this.tabbedControlGroup1.SelectedTabPageIndex = 1;
            if (Testth != null && Testth.IsAlive)
                Testth.Abort();
            else
                this.DialogResult = DialogResult.Cancel;
        }

        private void txtServerName_EditValueChanged(object sender, EventArgs e)
        {
            this.simpleButton2.Enabled = false;
        }

       

        private void Login_Load(object sender, EventArgs e)
        {
            this.txtServerName.EditValue = Utils.SQLServer;
            this.txtDBName.EditValue = Utils.SQLDataBase;
            this.txtLoginName.EditValue = Utils.SQLUserID;
            this.txtPassWord.EditValue = Utils.SQLPassword;


            //尝试连接一下
            Testth = new Thread(new ThreadStart(InitTest));
            SetControlsEnable(false);
            Testth.Start();

        }
        DataSet _UserDS = null;
        private void cmb_UserCode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //加载系统用户
            if (IsConnect)
            {
                _UserDS = access.GetAllUser();
                if (_UserDS.Tables.Count > 0 && _UserDS.Tables["T_User"].Rows.Count > 0)
                {
                    this.cmb_Usercode.Properties.Items.Clear();
                    foreach (DataRow userRow in _UserDS.Tables["T_User"].Rows)
                    {
                        this.cmb_Usercode.Properties.Items.Add(userRow["UserCode"]);
                    }
                }
                this.cmb_Usercode.ShowPopup();
            }
        }

        private void cmb_Usercode_TextChanged(object sender, EventArgs e)
        {
            //加载系统用户
            if (IsConnect&&this._UserDS !=null&&this.cmb_Usercode.Text != "")
            {
             DataRow[] userRows=   _UserDS.Tables["T_User"].Select("UserCode = '"+this.cmb_Usercode.Text.Trim()+"'");
                    if (userRows.Length > 0)
                    {
                        this.txtUserName.Properties.Items.Clear();
                        this.txtUserPassword.Text = "";
                            this.txtUserName.Properties.Items.Add(userRows[0]["UserName"]);
                            this.txtUserName.SelectedIndex = 0;
                    }
               
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Testth != null && Testth.IsAlive)
            {
                Testth.Abort();
                e.Cancel = true;
            }
        }

       

       
    }
}