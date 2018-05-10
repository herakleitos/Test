using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Drawing;
using System.Reflection;
using System.Net;
using System.Windows.Forms;
using DataModel;
using CTSocket;

namespace CTWH.Common
{
    public partial class Utils
    {

        //SerialPort
        private static bool IsFirstLoadSerialParaScan1 = true;
        private static DataType.SerialPortParameter _SerialParaScan1;
        public static DataType.SerialPortParameter SerialParaScan1
        {
            get
            {
                if (IsFirstLoadSerialParaScan1)
                {
                    try
                    {

                        _SerialParaScan1 = new DataType.SerialPortParameter(GetAppSettings("SerialParaScan1"));
                    }
                    catch { }

                    IsFirstLoadSerialParaScan1 = false;
                }

                return _SerialParaScan1;

            }

            set
            {
                _SerialParaScan1 = value;
                try
                {
                    SetAppSettings("SerialParaScan1", _SerialParaScan1.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaScan2 = true;
        private static DataType.SerialPortParameter _SerialParaScan2;
        public static DataType.SerialPortParameter SerialParaScan2
        {
            get
            {
                if (IsFirstLoadSerialParaScan2)
                {
                    try
                    {

                        _SerialParaScan2 = new DataType.SerialPortParameter(GetAppSettings("SerialParaScan2"));
                    }
                    catch { }

                    IsFirstLoadSerialParaScan2 = false;
                }

                return _SerialParaScan2;

            }

            set
            {
                _SerialParaScan2 = value;
                try
                {
                    SetAppSettings("SerialParaScan2", _SerialParaScan2.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaScan3 = true;
        private static DataType.SerialPortParameter _SerialParaScan3;
        public static DataType.SerialPortParameter SerialParaScan3
        {
            get
            {
                if (IsFirstLoadSerialParaScan3)
                {
                    try
                    {

                        _SerialParaScan3 = new DataType.SerialPortParameter(GetAppSettings("SerialParaScan3"));
                    }
                    catch { }

                    IsFirstLoadSerialParaScan3 = false;
                }

                return _SerialParaScan3;

            }

            set
            {
                _SerialParaScan3 = value;
                try
                {
                    SetAppSettings("SerialParaScan3", _SerialParaScan3.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaWeight1 = true;
        private static DataType.SerialPortParameter _SerialParaWeight1;
        public static DataType.SerialPortParameter SerialParaWeight1
        {
            get
            {
                if (IsFirstLoadSerialParaWeight1)
                {
                    try
                    {

                        _SerialParaWeight1 = new DataType.SerialPortParameter(GetAppSettings("SerialParaWeight1"));
                    }
                    catch { }

                    IsFirstLoadSerialParaWeight1 = false;
                }

                return _SerialParaWeight1;

            }

            set
            {
                _SerialParaWeight1 = value;
                try
                {
                    SetAppSettings("SerialParaWeight1", _SerialParaWeight1.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaInkJet1 = true;
        private static DataType.SerialPortParameter _SerialParaInkJet1;
        public static DataType.SerialPortParameter SerialParaInkJet1
        {
            get
            {
                if (IsFirstLoadSerialParaInkJet1)
                {
                    try
                    {

                        _SerialParaInkJet1 = new DataType.SerialPortParameter(GetAppSettings("SerialParaInkJet1"));
                    }
                    catch { }

                    IsFirstLoadSerialParaInkJet1 = false;
                }

                return _SerialParaInkJet1;

            }

            set
            {
                _SerialParaInkJet1 = value;
                try
                {
                    SetAppSettings("SerialParaInkJet1", _SerialParaInkJet1.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaWidthLeft1 = true;
        private static DataType.SerialPortParameter _SerialParaWidthLeft1;
        public static DataType.SerialPortParameter SerialParaWidthLeft1
        {
            get
            {
                if (IsFirstLoadSerialParaWidthLeft1)
                {
                    try
                    {

                        _SerialParaWidthLeft1 = new DataType.SerialPortParameter(GetAppSettings("SerialParaWidthLeft1"));
                    }
                    catch { }

                    IsFirstLoadSerialParaWidthLeft1 = false;
                }

                return _SerialParaWidthLeft1;

            }

            set
            {
                _SerialParaWidthLeft1 = value;
                try
                {
                    SetAppSettings("SerialParaWidthLeft1", _SerialParaWidthLeft1.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaWidthRight1 = true;
        private static DataType.SerialPortParameter _SerialParaWidthRight1;
        public static DataType.SerialPortParameter SerialParaWidthRight1
        {
            get
            {
                if (IsFirstLoadSerialParaWidthRight1)
                {
                    try
                    {

                        _SerialParaWidthRight1 = new DataType.SerialPortParameter(GetAppSettings("SerialParaWidthRight1"));
                    }
                    catch { }

                    IsFirstLoadSerialParaWidthRight1 = false;
                }

                return _SerialParaWidthRight1;

            }

            set
            {
                _SerialParaWidthRight1 = value;
                try
                {
                    SetAppSettings("SerialParaWidthRight1", _SerialParaWidthRight1.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaWidthLeft2 = true;
        private static DataType.SerialPortParameter _SerialParaWidthLeft2;
        public static DataType.SerialPortParameter SerialParaWidthLeft2
        {
            get
            {
                if (IsFirstLoadSerialParaWidthLeft2)
                {
                    try
                    {

                        _SerialParaWidthLeft2 = new DataType.SerialPortParameter(GetAppSettings("SerialParaWidthLeft2"));
                    }
                    catch { }

                    IsFirstLoadSerialParaWidthLeft2 = false;
                }

                return _SerialParaWidthLeft2;

            }

            set
            {
                _SerialParaWidthLeft2 = value;
                try
                {
                    SetAppSettings("SerialParaWidthLeft2", _SerialParaWidthLeft2.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadSerialParaWidthRight2 = true;
        private static DataType.SerialPortParameter _SerialParaWidthRight2;
        public static DataType.SerialPortParameter SerialParaWidthRight2
        {
            get
            {
                if (IsFirstLoadSerialParaWidthRight2)
                {
                    try
                    {

                        _SerialParaWidthRight2 = new DataType.SerialPortParameter(GetAppSettings("SerialParaWidthRight2"));
                    }
                    catch { }

                    IsFirstLoadSerialParaWidthRight2 = false;
                }

                return _SerialParaWidthRight2;

            }

            set
            {
                _SerialParaWidthRight2 = value;
                try
                {
                    SetAppSettings("SerialParaWidthRight2", _SerialParaWidthRight2.GetParameterString());

                }
                catch { }
            }
        }
        
       
    } 
}
