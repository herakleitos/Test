using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaint.Instock.Business.PlugIns
{
    public class PlcSignalName
    {
        public static string Plc_StartScanning = "StartScanning";       //开始扫描 86
        public static string Plc_EndScanning = "EndScanning";       //结束扫描  86
        public static string Plc_ProductCount = "ProductCount";     //产品数量

        public static string Plc_GroupOID = "GroupOID";
        public static string Plc_ScanResult = "ScanResult";         //99:扫描完成，86：扫描成功，85：扫描失败

        public static string Plc_Position1 = "Position1";
        public static string Plc_Position2 = "Position2";
        public static string Plc_Position3 = "Position3";
        public static string Plc_Position4 = "Position4";
        public static string Plc_Position5 = "Position5";

        public static string Plc_Spare1 = "Spare1";
        public static string Plc_Spare2 = "Spare2";
        public static string Plc_Spare3 = "Spare3";
        public static string Plc_Spare4 = "Spare4";
        public static string Plc_Spare5 = "Spare5";

        public static string Plc_MotorStart = "MotorStart";             //人工启动扫描仪下降  86
        public static string Plc_IsUseScanner = "IsUseScanner";         //扫描仪是否投入   86:不投入

        public static string Plc_ScanDes = "ScanDes";         //去向： 100立体仓库，200人工仓库
    }
}
