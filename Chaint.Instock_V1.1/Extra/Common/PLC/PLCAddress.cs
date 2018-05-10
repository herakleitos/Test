using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.PLC
{
    public struct PLCAddress//待连接plc地址属性表
    {
        public byte AdrType; // Type of address: MPI/PB (1), IP (2), MAC (3)
        public byte[] Adr; // connection address
        // MPI/PB station address (2)
        // IP address (192.168.0.1)
        // MAC address (08-00-06-01-AA-BB)       
        public byte SlotNr; // Slot number
        public byte RackNr; // Rack number

        public PLCAddress(string _AdrType, string _Adr, string _SlotNr, string _RackNr)
        {
            AdrType = Convert.ToByte(_AdrType);

            switch (AdrType)
            {
                case 1:
                    Adr = new byte[] { Convert.ToByte(_Adr), 0, 0, 0, 0, 0 };
                    break;
                case 2:
                    string[] str = _Adr.Split('.');
                    Adr = new byte[] { Convert.ToByte(str[0]), Convert.ToByte(str[1]), Convert.ToByte(str[2]), Convert.ToByte(str[3]), 0, 0 };
                    break;

                case 3:
                    string[] hexstr = _Adr.Split('-');
                    Adr = new byte[] { Convert.ToByte(hexstr[0], 16), Convert.ToByte(hexstr[1], 16), Convert.ToByte(hexstr[2], 16), Convert.ToByte(hexstr[3], 16), 0, 0 };
                    break;

                default:
                    Adr = new byte[] { 0, 0, 0, 0, 0, 0 };
                    break;
            }
            //AdrType = Convert.ToByte();
            SlotNr = Convert.ToByte(_SlotNr);
            RackNr = Convert.ToByte(_RackNr);
        }



        public PLCAddress(string strPara)
        {
            string[] strs = strPara.Split(';');
            string _AdrType = strs[0];
            string _Adr = strs[1];
            string _SlotNr = strs[2];
            string _RackNr = strs[3];

            AdrType = Convert.ToByte(_AdrType);

            switch (AdrType)
            {
                case 1:
                    Adr = new byte[] { Convert.ToByte(_Adr), 0, 0, 0, 0, 0 };
                    break;
                case 2:
                    string[] str = _Adr.Split('.');
                    Adr = new byte[] { Convert.ToByte(str[0]), Convert.ToByte(str[1]), Convert.ToByte(str[2]), Convert.ToByte(str[3]), 0, 0 };
                    break;

                case 3:
                    string[] hexstr = _Adr.Split('-');
                    Adr = new byte[] { Convert.ToByte(hexstr[0], 16), Convert.ToByte(hexstr[1], 16), Convert.ToByte(hexstr[2], 16), Convert.ToByte(hexstr[3], 16), 0, 0 };
                    break;

                default:
                    Adr = new byte[] { 0, 0, 0, 0, 0, 0 };
                    break;
            }
            //AdrType = Convert.ToByte();
            SlotNr = Convert.ToByte(_SlotNr);
            RackNr = Convert.ToByte(_RackNr);
        }




        public string GetParameterString()
        {
            string adr = "";
            try
            {
                //string str = _PLCAddress.AdrType.ToString()+

                switch (AdrType)
                {
                    case 1:
                        adr = Adr[0].ToString();
                        break;
                    case 2:

                        adr = String.Join(".", Adr[0].ToString(), Adr[1].ToString(), Adr[2].ToString(), Adr[3].ToString());

                        break;

                    case 3:

                        adr = String.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}", Adr[0], Adr[1], Adr[2], Adr[3], Adr[4], Adr[5]);

                        break;

                    default:
                        adr = "None";
                        break;
                }
            }
           catch{} 

                return String.Join(";", AdrType, adr, SlotNr, RackNr);

            }

        }
    
    public struct PLCReadInfo//PLC读取信息
    {
        public int PLCNO;
        public int DBNO;
        public int ReadBegin;
        public int ReadCount;

        public PLCReadInfo(int _PLCNO, int _DBNO, int _ReadBegin, int _ReadCount)
        {
            PLCNO = _PLCNO;
            DBNO = _DBNO;
            ReadBegin = _ReadBegin;
            ReadCount = _ReadCount;
        }
    }

 }
 