using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace CTWH.Common.DataType
{
    public struct ScannerParameter
    {
        public string ScannerDesc;
        public string Positon;    

        public byte ScanBarcodeLength;
        public int ScanTimeOutMS;

        public byte ScanBarcodePrefix;
        public byte ScanBarcodeSuffix;

        public string ScanOpenString;
        public string ScanCloseString;
        public ScannerParameter(string _ScannerDesc, string _Positon, byte _ScanBarcodeLength,int _ScanTimeOutMS, byte _ScanBarcodePrefix, byte _ScanBarcodeSuffix, string _ScanOpenString, string _ScanCloseString)
        {
            ScannerDesc = _ScannerDesc;
            Positon = _Positon;
            ScanBarcodeLength = _ScanBarcodeLength;
            ScanTimeOutMS = _ScanTimeOutMS;

            ScanBarcodePrefix = _ScanBarcodePrefix;
            ScanBarcodeSuffix = _ScanBarcodeSuffix;
            ScanOpenString = _ScanOpenString;
            ScanCloseString = _ScanCloseString;
        }

      
        public ScannerParameter(string strPara)
        {
            string[] strs = strPara.Split(',');
          
            ScannerDesc = strs[0];
            Positon = strs[1];
            ScanBarcodeLength = (byte)Convert.ToInt16(strs[2]);
            ScanTimeOutMS =  Convert.ToInt32(strs[3]);
            ScanBarcodePrefix = (byte)Convert.ToInt16(strs[4]);
            ScanBarcodeSuffix = (byte)Convert.ToInt16(strs[5]);
            ScanOpenString = strs[6];
            ScanCloseString = strs[7];
        }

        public string GetParameterString()
        {
            return String.Join(",", ScannerDesc, Positon, ScanBarcodeLength,ScanTimeOutMS, ScanBarcodePrefix, ScanBarcodeSuffix, ScanOpenString, ScanCloseString);
        }
    }

 
}
