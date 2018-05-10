
namespace Chaint.Common.Core.Enums
{
    //App 配置类型
    public enum Enums_AppConfigType { INI, SQLITE, ACCESS, XML, REGISTER };
    //数据库类型
    public enum Enums_DBType { ACCESS, SQLITE, MSSQL, MYSQL, ORACLE, DB2, SYBASE }
    //数据库类型
    public enum Enums_FieldType { String = 0, Int32 = 1,Int64=2, DateTime =3, Decimal = 4, Bool = 5}

    public enum Enums_ErrorLevel { Normal = 0, Waring = 1, Error = 2}

    public enum Enums_PLC_GroupID { NoBarCode = 1, InstockFailed = 2, BarCodeRepeat = 3,OneBarCode=4,
    TwoBarCode = 5}
}
