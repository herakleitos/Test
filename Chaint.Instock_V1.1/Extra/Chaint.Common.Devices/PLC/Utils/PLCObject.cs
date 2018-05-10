using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.PLC.Utils
{
    /// <summary>
    /// PLC 一次性写入的参数对象
    /// </summary>
    //public class PLCObject:IComparable
    //{
    //    private string m_PLCStationName = "";
    //    private string m_PLCTemplateName = "";
    //    private int m_DBUnit = 0;
    //    private int m_DWNO = 0;
    //    private int m_Amount = 0;
    //    private byte[] m_bytsData = null;

    //    public PLCObject(string strStationName, string strTempName, int dbunit, int dwno, int amount, byte[] bytsData)
    //    {
    //        m_PLCStationName = strStationName;
    //        m_PLCTemplateName = strTempName;
    //        m_DBUnit = dbunit;
    //        m_DWNO = dwno;
    //        m_Amount = amount;
    //        m_bytsData = bytsData;
    //    }

    //    public string PLCStationName
    //    {
    //        set { m_PLCStationName = value; }
    //        get { return m_PLCStationName; }
    //    }
    //    public string PLCTemplateName
    //    {
    //        set { m_PLCTemplateName = value; }
    //        get { return m_PLCTemplateName; }
    //    }

    //    public int DBUnit
    //    {
    //        set { m_DBUnit = value; }
    //        get { return m_DBUnit; }
    //    }
    //    public int DWNO
    //    {
    //        set { m_DWNO = value; }
    //        get { return m_DWNO; }
    //    }
    //    public int Amount
    //    {
    //        set { m_Amount = value; }
    //        get { return m_Amount; }
    //    }

    //    /// <summary>
    //    /// PLC地址对应的字节数组
    //    /// </summary>
    //    public byte[] BytsData
    //    {
    //        set { m_bytsData = value; }
    //        get { return m_bytsData; }
    //    }
      //  public int CompareTo(Object obj)
      //   {
      //       PLCObject plcObj = (PLCObject)obj;
      //       return this.DWNO.CompareTo(plcObj.DWNO);
      //   }
     
    //}


    /// <summary>
    /// PLC 一次性写入的参数对象
    /// </summary>
     public class PLCObject:PLCSignalItem,IComparable
     {
         private byte[] m_bytsData = null;

         public PLCObject() { }

         public PLCObject(string strStationName, string strTempName, int dbunit, int dwno, int amount, byte[] bytsData)
         {
             base.PLCStationName = strStationName;
             base.PLCTemplateName = strTempName;
             base.DBUnit = dbunit;
             base.DWNO = dwno;
             base.Amount = amount;
             m_bytsData = bytsData;
         }

 
         /// <summary>
         /// PLC地址对应的字节数组
         /// </summary>
         public byte[] BytsData
         {
             set { m_bytsData = value; }
             get { return m_bytsData; }
         }

         public int CompareTo(Object obj)
         {
             PLCObject plcObj = (PLCObject)obj;
             return this.DWNO.CompareTo(plcObj.DWNO);
         }

     }

}
