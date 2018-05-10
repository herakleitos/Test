using System;
using System.Collections.Generic;
using System.Text;

using Chaint.Common.Devices.Data;
using Chaint.Common.Devices.PLC.Utils;

namespace Chaint.Common.Devices.PLC
{
    /*-----------------------------------------------------------------------------------
     * 作者: Automation&IT Dept. 
     * 
     * 创建时间: 2015-05-21
     * 
     * 功能描述: 
     *      主要用于测试用,要借助DBAccessor
     * 
     ------------------------------------------------------------------------------------*/
    public class PLCOperationTest:IPLCOperation,IDisposable
    {
        public event PLCMessageEventHandler OnPLCMessageChanged;
        private bool m_blnConnected = false;

        private DBAccessFunc m_dbAccessor = null;
        private string m_StationName = "";
        private string m_TemplateName = "";

        #region 属性定义
        public bool Connected
        {
            get { return m_blnConnected; }
        }

        public PLCType CurrPLCType
        {
            get { return PLCType.PLC_Test; }
        }
        #endregion


        public PLCOperationTest(DBAccessFunc dbaccessor,string strStationName,string strTemplateName)
        {
            m_dbAccessor = dbaccessor;
            m_StationName = strStationName;
            m_TemplateName = strTemplateName;
        }

        public bool ConnectPLC()
        {
            if (m_dbAccessor == null) return false;
            string retMsg = "";
            m_dbAccessor.Open(ref retMsg);
            if(retMsg=="success")
            {
                m_blnConnected = true;
                m_dbAccessor.Close(ref retMsg);
                SendPLCMessage(true, "模拟PLC连接成功!");
                return true;
            }
            else
            {
                //Console.WriteLine(retMsg);
                SendPLCMessage(false, "模拟连接PLC出错,原因:" + retMsg);
                return false;
            }
        }

        public bool CloseConnectPLC()
        {
            if (m_dbAccessor == null) return false;
            string retMsg = "";
            m_dbAccessor.Close(ref retMsg);
            if (retMsg == "success")
            {
                m_blnConnected = false;
                SendPLCMessage(false, "模拟PLC已断开连接");
                return true;
            }
            else
            {
                Console.WriteLine(retMsg);
                SendPLCMessage(false, "关闭模拟PLC出错,原因:" + retMsg);
                return false;
            }
        }

        public bool ReConnectPLC()
        {
            CloseConnectPLC();
            System.Threading.Thread.Sleep(300);
            ConnectPLC();
            return true;
        }

        #region 模拟读PLC数据
        public bool ReadData(int dbno, string strAddr, ref string retBoolValue)
        {
            retBoolValue = "0";

            if (strAddr.Split('.').Length < 2) return false;
            int intX = Convert.ToInt32(strAddr.Split('.')[0].Trim());
            int intY = Convert.ToInt32(strAddr.Split('.')[1].Trim());

            ushort dwno = (ushort)intX;
            byte[] bytBuffer = new byte[1];

            bool blnSucc = ReadData(dbno, dwno, 1, ref bytBuffer);
            if (!blnSucc) return false;

            bool isOne = DataConverter.IsOneInByte(bytBuffer[0], intY);
            retBoolValue = isOne ? "1" : "0";

            return true;
        }
        public bool ReadData(int dbno, int dwno, int amount, ref string retValue)
        {
            bool blnSuccess = true;
            if (amount > 4) amount = 4;
            byte[] bytBuffer = new byte[amount];
            retValue = "0";
            bool blnResult = ReadData(dbno, dwno, amount, ref bytBuffer);
            if (blnResult)
            {
                retValue = DataConverter.ByteToLong(bytBuffer).ToString();
                blnSuccess = true;
            }
            else
            {
                blnSuccess = false;
            }
            return blnSuccess;
        }

        public bool ReadData(int dbno, int dwno, int amount, ref byte[] buffer)
        {
          
            if (m_dbAccessor == null)
            {
                SendPLCMessage(true, "DB读取器不能为空");
                return false;
            }

            string strSql = string.Format("Select * From PLC_Config Where GongWeiType='{0}' AND MobanNO='{1}' AND DBUnit={2} AND DWNO>={3} AND DWNO<{4}  Order by GongWeiType,MobanNo,DBUnit,DWNO",
                m_StationName, m_TemplateName, dbno, dwno, dwno+amount);
            string retMsg = "";
            string strValue = "";

            buffer = new byte[amount];
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                m_dbAccessor.ExeSql(strSql, ref ds, ref retMsg);
                if (retMsg == "success" && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int bytCount = 0, bytIndex = 0;
                    byte[] tempByt;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        bytCount = int.Parse(ds.Tables[0].Rows[i]["Amount"].ToString());
                        tempByt = new byte[bytCount];
                        strValue = ds.Tables[0].Rows[i]["SignalValue"].ToString().Trim();
                        if (strValue == "") strValue = "0";

                        tempByt = DataConverter.LongToByte(ulong.Parse(strValue), bytCount);
                        tempByt.CopyTo(buffer, bytIndex);
                        bytIndex += bytCount;
                    }
                    return true;
                }
                else
                {
                    if (retMsg != "success")
                    {
                        SendPLCMessage(true, "模拟读取PLC失败,原因:" + retMsg);
                    }
                    else
                    {
                        SendPLCMessage(true, "数据库中不存在定义的PLC信号或者DWNO和DBUnit参数错误");
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                SendPLCMessage(true, "模拟读取PLC出错,原因:" + ex.Message);
                return false;
            }
        }
        #endregion

        #region 模拟写入PLC数据

        /// <summary>
        /// 翻转某个地址的值
        /// </summary>
        /// <param name="dbno">DB单元</param>
        /// <param name="strAddr">地址 格式X.Y   X为地址,Y为位置 如：5.3表示第5个字节第3位</param>
        /// <returns></returns>
        public bool WriteData(int dbno, string strAddr)
        {
            if (strAddr.Split('.').Length < 2) return false;
            int intX = Convert.ToInt32(strAddr.Split('.')[0].Trim());
            int intY = Convert.ToInt32(strAddr.Split('.')[1].Trim());

            ushort dwno = (ushort)intX;
            byte[] bytBuffer = new byte[1];

            bool blnSucc = ReadData(dbno, dwno, 1, ref bytBuffer);
            bytBuffer[0] = (byte)DataConverter.ConvertBit(bytBuffer[0], intY);
            return WriteData(dbno, dwno, 1, bytBuffer);
        }

        /// <summary>
        /// 将当前地址写1或0,此处以字节来处理
        /// </summary>
        /// <param name="dbno">DB单元</param>
        /// <param name="strAddr">地址 格式X.Y   X为地址,Y为位置 如：5.3表示第5个字节第3位</param>
        /// <param name="blnValue">值,非0为真,其他为假</param>
        /// <returns></returns>
        public bool WriteData(int dbno, string strAddr, bool blnValue)
        {
            if (strAddr.Split('.').Length < 2) return false;
            int intX = Convert.ToInt32(strAddr.Split('.')[0].Trim());
            int intY = Convert.ToInt32(strAddr.Split('.')[1].Trim());

            ushort dwno = (ushort)intX;
            byte[] bytBuffer = new byte[1];

            bool blnSucc = ReadData(dbno, dwno, 1, ref bytBuffer);
            bytBuffer[0] = (byte)DataConverter.SetBitBoolValue(bytBuffer[0], intY, blnValue);
            return WriteData(dbno, dwno, 1, bytBuffer);
        }

        public bool WriteData(int dbno, int dwno, int amount, string strData)
        {
            ulong tmpValue = 0;
            string strMsg = "";
            byte[] bytBuffer = new byte[amount];

            if (ulong.TryParse(strData, out tmpValue))
            {
                bytBuffer = DataConverter.LongToByte(tmpValue, amount);
                bool blnSucc = WriteData(dbno, dwno, amount, bytBuffer);
                return blnSucc;
            }
            else
            {
                strMsg = string.Format("写入值<{0}>不是有效的无符号长整型", strData);
                SendPLCMessage(true, strMsg);
                return false;
            }
        }

        public bool WriteData(int dbno, int dwno, int amount, ulong data)
        {
            byte[] bytBuffer = new byte[amount];
            bytBuffer = DataConverter.LongToByte(data, amount);
            bool blnSucc = WriteData(dbno, dwno, amount, bytBuffer);
            return blnSucc;
        }

        /// <summary>
        /// 写入PLC数据, 只能单个信号写
        /// </summary>
        /// <param name="dbno">DB单元</param>
        /// <param name="dwno">初始地址</param>
        /// <param name="amount">字节数量</param>
        /// <param name="buffer">返回的字节数组</param>
        /// <returns>写入成功返回True</returns>
        public bool WriteData(int dbno, int dwno, int amount, byte[] buffer)
        {
            string retMsg = "";
            try
            {
                if (amount != buffer.Length)
                {
                    SendPLCMessage(true, "字节数量与数组长度不一致");
                    return false;
                }

                if (m_dbAccessor == null)
                {
                    SendPLCMessage(true, "DB读取器不能为空");
                    return false;
                }
                string strSql = string.Format("Select * From PLC_Config Where GongWeiType='{0}' AND MobanNO='{1}' AND DBUnit={2} AND DWNO>={3} AND DWNO<{4}  Order by GongWeiType,MobanNo,DBUnit,DWNO",
                                                m_StationName, m_TemplateName, dbno, dwno, dwno + amount);
                System.Data.DataSet ds = new System.Data.DataSet();
                m_dbAccessor.ExeSql(strSql, ref ds, ref retMsg);
                if (retMsg == "success" && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int bytCount = 0, bytIndex = 0;
                    ulong ulngValue = 0;
                    bool blnSucc = false;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dbno = int.Parse(ds.Tables[0].Rows[i]["DBUnit"].ToString());
                        dwno = int.Parse(ds.Tables[0].Rows[i]["DWNO"].ToString());
                        bytCount = int.Parse(ds.Tables[0].Rows[i]["Amount"].ToString());

                        ulngValue = DataConverter.ByteToLong(buffer, bytIndex, bytIndex + bytCount - 1);

                        blnSucc=WriteSinglePLCValue(dbno, dwno, bytCount, ulngValue);
                        if(!blnSucc)
                        {
                            SendPLCMessage(true, string.Format("模拟写PLC数据<{0}:{1}:{2}:{3}失败", dbno, dwno, bytCount, ulngValue));
                            return false;
                        }
                        bytIndex += bytCount;
                    }

                    return true;
                }
                else
                {
                    if (retMsg != "success")
                    {
                        SendPLCMessage(true, "模拟写PLC失败,原因:" + retMsg);
                    }
                    else
                    {
                        SendPLCMessage(true, "数据库中不存在定义的PLC信号或者DWNO和DBUnit参数错误");
                    }

                    return false;
                }
            }
            catch (System.Exception ex)
            {
                retMsg = "模拟写入PLC数据出错,原因:" + ex.Message;
                m_blnConnected = false;
            }
            SendPLCMessage(m_blnConnected, retMsg);
            return m_blnConnected;
        }

        /// <summary>
        /// 模拟写单个PLC信号值
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="bytCount"></param>
        /// <param name="objplcValue"></param>
        /// <returns></returns>
        private bool WriteSinglePLCValue(int dbno, int dwno,int bytCount,object objplcValue)
        {
            string strSql = string.Format("Update PLC_Config Set SignalValue='{0}' Where GongWeiType='{1}' AND MobanNO='{2}' AND DBUnit={3} AND DWNO={4} AND Amount={5}",
                                            objplcValue, m_StationName, m_TemplateName, dbno, dwno, bytCount);
            string retMsg = "";
            m_dbAccessor.ExeSql(strSql, ref retMsg);
            return (retMsg == "success");
        }
        #endregion

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Part of basic design pattern for implementing Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseConnectPLC();
            }
        }
        #endregion

        private void SendPLCMessage(bool blnConnected, string strMsg)
        {
            if (OnPLCMessageChanged != null)
            {
                OnPLCMessageChanged(this, new PLCEventArgument(blnConnected, strMsg));
            }
        }


    }
}
