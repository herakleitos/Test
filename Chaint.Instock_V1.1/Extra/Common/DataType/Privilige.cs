using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.DataType
{
    //public struct PaperUserPrivilige
    //{
    //    // public List<Privilige> LoginUserPrivilige;
  
    //    public Dictionary<string, Privilige> LoginUserPrivilige;
    //    public PaperUserType UserType;
    //    public PaperUserPrivilige(string strPrivilige, PaperUserType _UserType)
    //    {
    //        UserType = _UserType;
    //        LoginUserPrivilige = new Dictionary<string, Privilige>();
    //        AddUserPrivilige("RollProductQuery", "纸卷查询", strPrivilige[0].Equals('1'));
    //        AddUserPrivilige("SheetBufferLock", "切纸缓存区锁定", strPrivilige[1].Equals('1'));
    //        AddUserPrivilige("AppConfig", "配置文件备份", strPrivilige[2].Equals('1'));             
    //    }

    //    public PaperUserPrivilige()
    //    {
    //        UserType = PaperUserType.Normal;
    //        LoginUserPrivilige = new Dictionary<string, Privilige>();
    //        AddUserPrivilige("RollProductQuery", "纸卷查询", false );
    //        AddUserPrivilige("SheetBufferLock", "切纸缓存区锁定", false );
    //        AddUserPrivilige("AppConfig", "配置文件备份", false );
    //    }


    //    private void AddUserPrivilige(string PriviligeName, string PriviligeDesc, bool IsOK)
    //    {
    //        LoginUserPrivilige.Add(PriviligeName, new Privilige(PriviligeName, PriviligeDesc, IsOK));
    //    }

     

    //    public string GetParameterString()
    //    { 
    //        return "";
    //    }
    //}

    //public struct Privilige
    //{
    //    public string PriviligeName;
    //    public string PriviligeDesc;

    //    public bool IsOK;
    //    public Privilige(string _PriviligeName, string _PriviligeDesc, bool _IsOK)
    //    {
    //        PriviligeName = _PriviligeName;
    //        PriviligeDesc = _PriviligeDesc;

    //        IsOK = _IsOK;
    //    }
    //}


    public enum PriviligeName
    {
        RollProductQuery = 0x0,
        SheetBufferLock = 0x1,
        AppConfigBackUp = 0x2,
        Settings = 0x3,
        UserEdit = 0x4,
        
        //RollProductQuery = 0x3,
    }

    public enum PaperUserType
    {
        Normal = 0x0,
        Admin = 0x1,
        SuperAdmin = 0x2
    }
}
