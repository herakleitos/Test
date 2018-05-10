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

        //测量直径增补偏差数
        private static bool IsFirstLoadOffsetParaDiameter = true;
        private static DataType.OffsetParameter _OffsetParaDiameter;
        public static DataType.OffsetParameter OffsetParaDiameter
        {
            get
            {
                if (IsFirstLoadOffsetParaDiameter)
                {
                    try
                    {
                        _OffsetParaDiameter = new DataType.OffsetParameter(GetAppSettings("OffsetParaDiameter"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaDiameter = false;
                }
                return _OffsetParaDiameter;
            }

            set
            {
                _OffsetParaDiameter = value;
                try
                {
                    SetAppSettings("OffsetParaDiameter", _OffsetParaDiameter.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaDiameter_F = true;
        private static DataType.OffsetParameter _OffsetParaDiameter_F;
        public static DataType.OffsetParameter OffsetParaDiameter_F
        {
            get
            {
                if (IsFirstLoadOffsetParaDiameter_F)
                {
                    try
                    {
                        _OffsetParaDiameter_F = new DataType.OffsetParameter(GetAppSettings("OffsetParaDiameter_F"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaDiameter_F = false;
                }
                return _OffsetParaDiameter_F;
            }

            set
            {
                _OffsetParaDiameter_F = value;
                try
                {
                    SetAppSettings("OffsetParaDiameter_F", _OffsetParaDiameter_F.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaDiameter_S = true;
        private static DataType.OffsetParameter _OffsetParaDiameter_S;
        public static DataType.OffsetParameter OffsetParaDiameter_S
        {
            get
            {
                if (IsFirstLoadOffsetParaDiameter_S)
                {
                    try
                    {
                        _OffsetParaDiameter_S = new DataType.OffsetParameter(GetAppSettings("OffsetParaDiameter_S"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaDiameter_S = false;
                }
                return _OffsetParaDiameter_S;
            }

            set
            {
                _OffsetParaDiameter_S = value;
                try
                {
                    SetAppSettings("OffsetParaDiameter_S", _OffsetParaDiameter_S.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaDiameter_R = true;
        private static DataType.OffsetParameter _OffsetParaDiameter_R;
        public static DataType.OffsetParameter OffsetParaDiameter_R
        {
            get
            {
                if (IsFirstLoadOffsetParaDiameter_R)
                {
                    try
                    {
                        _OffsetParaDiameter_R = new DataType.OffsetParameter(GetAppSettings("OffsetParaDiameter_R"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaDiameter_R = false;
                }
                return _OffsetParaDiameter_R;
            }

            set
            {
                _OffsetParaDiameter_R = value;
                try
                {
                    SetAppSettings("OffsetParaDiameter_R", _OffsetParaDiameter_R.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadOffsetParaWidth = true;
        private static DataType.OffsetParameter _OffsetParaWidth;
        public static DataType.OffsetParameter OffsetParaWidth
        {
            get
            {
                if (IsFirstLoadOffsetParaWidth)
                {
                    try
                    {
                        _OffsetParaWidth = new DataType.OffsetParameter(GetAppSettings("OffsetParaWidth"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWidth = false;
                }
                return _OffsetParaWidth;
            }

            set
            {
                _OffsetParaWidth = value;
                try
                {
                    SetAppSettings("OffsetParaWidth", _OffsetParaWidth.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWidth_F = true;
        private static DataType.OffsetParameter _OffsetParaWidth_F;
        public static DataType.OffsetParameter OffsetParaWidth_F
        {
            get
            {
                if (IsFirstLoadOffsetParaWidth_F)
                {
                    try
                    {
                        _OffsetParaWidth_F = new DataType.OffsetParameter(GetAppSettings("OffsetParaWidth_F"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWidth_F = false;
                }
                return _OffsetParaWidth_F;
            }

            set
            {
                _OffsetParaWidth_F = value;
                try
                {
                    SetAppSettings("OffsetParaWidth_F", _OffsetParaWidth_F.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWidth_S = true;
        private static DataType.OffsetParameter _OffsetParaWidth_S;
        public static DataType.OffsetParameter OffsetParaWidth_S
        {
            get
            {
                if (IsFirstLoadOffsetParaWidth_S)
                {
                    try
                    {
                        _OffsetParaWidth_S = new DataType.OffsetParameter(GetAppSettings("OffsetParaWidth_S"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWidth_S = false;
                }
                return _OffsetParaWidth_S;
            }

            set
            {
                _OffsetParaWidth_S = value;
                try
                {
                    SetAppSettings("OffsetParaWidth_S", _OffsetParaWidth_S.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWidth_R = true;
        private static DataType.OffsetParameter _OffsetParaWidth_R;
        public static DataType.OffsetParameter OffsetParaWidth_R
        {
            get
            {
                if (IsFirstLoadOffsetParaWidth_R)
                {
                    try
                    {
                        _OffsetParaWidth_R = new DataType.OffsetParameter(GetAppSettings("OffsetParaWidth_R"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWidth_R = false;
                }
                return _OffsetParaWidth_R;
            }

            set
            {
                _OffsetParaWidth_R = value;
                try
                {
                    SetAppSettings("OffsetParaWidth_R", _OffsetParaWidth_R.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadOffsetParaWeightPercent = true;
        private static DataType.OffsetParameter _OffsetParaWeightPercent;
        public static DataType.OffsetParameter OffsetParaWeightPercent
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightPercent)
                {
                    try
                    {
                        _OffsetParaWeightPercent = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightPercent"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightPercent = false;
                }
                return _OffsetParaWeightPercent;
            }

            set
            {
                _OffsetParaWeightPercent = value;
                try
                {
                    SetAppSettings("OffsetParaWeightPercent", _OffsetParaWeightPercent.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWeightPercent_F = true;
        private static DataType.OffsetParameter _OffsetParaWeightPercent_F;
        public static DataType.OffsetParameter OffsetParaWeightPercent_F
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightPercent_F)
                {
                    try
                    {
                        _OffsetParaWeightPercent_F = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightPercent_F"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightPercent_F = false;
                }
                return _OffsetParaWeightPercent_F;
            }

            set
            {
                _OffsetParaWeightPercent_F = value;
                try
                {
                    SetAppSettings("OffsetParaWeightPercent_F", _OffsetParaWeightPercent_F.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWeightPercent_S = true;
        private static DataType.OffsetParameter _OffsetParaWeightPercent_S;
        public static DataType.OffsetParameter OffsetParaWeightPercent_S
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightPercent_S)
                {
                    try
                    {
                        _OffsetParaWeightPercent_S = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightPercent_S"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightPercent_S = false;
                }
                return _OffsetParaWeightPercent_S;
            }

            set
            {
                _OffsetParaWeightPercent_S = value;
                try
                {
                    SetAppSettings("OffsetParaWeightPercent_S", _OffsetParaWeightPercent_S.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWeightPercent_R = true;
        private static DataType.OffsetParameter _OffsetParaWeightPercent_R;
        public static DataType.OffsetParameter OffsetParaWeightPercent_R
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightPercent_R)
                {
                    try
                    {
                        _OffsetParaWeightPercent_R = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightPercent_R"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightPercent_R = false;
                }
                return _OffsetParaWeightPercent_R;
            }

            set
            {
                _OffsetParaWeightPercent_R = value;
                try
                {
                    SetAppSettings("OffsetParaWeightPercent_R", _OffsetParaWeightPercent_R.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadOffsetParaWeightStatic = true;
        private static DataType.OffsetParameter _OffsetParaWeightStatic;
        public static DataType.OffsetParameter OffsetParaWeightStatic
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightStatic)
                {
                    try
                    {
                        _OffsetParaWeightStatic = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightStatic"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightStatic = false;
                }
                return _OffsetParaWeightStatic;
            }

            set
            {
                _OffsetParaWeightStatic = value;
                try
                {
                    SetAppSettings("OffsetParaWeightStatic", _OffsetParaWeightStatic.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWeightStatic_F = true;
        private static DataType.OffsetParameter _OffsetParaWeightStatic_F;
        public static DataType.OffsetParameter OffsetParaWeightStatic_F
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightStatic_F)
                {
                    try
                    {
                        _OffsetParaWeightStatic_F = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightStatic_F"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightStatic_F = false;
                }
                return _OffsetParaWeightStatic_F;
            }

            set
            {
                _OffsetParaWeightStatic_F = value;
                try
                {
                    SetAppSettings("OffsetParaWeightStatic_F", _OffsetParaWeightStatic_F.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWeightStatic_S = true;
        private static DataType.OffsetParameter _OffsetParaWeightStatic_S;
        public static DataType.OffsetParameter OffsetParaWeightStatic_S
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightStatic_S)
                {
                    try
                    {
                        _OffsetParaWeightStatic_S = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightStatic_S"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightStatic_S = false;
                }
                return _OffsetParaWeightStatic_S;
            }

            set
            {
                _OffsetParaWeightStatic_S = value;
                try
                {
                    SetAppSettings("OffsetParaWeightStatic_S", _OffsetParaWeightStatic_S.GetParameterString());

                }
                catch { }
            }
        }
        private static bool IsFirstLoadOffsetParaWeightStatic_R = true;
        private static DataType.OffsetParameter _OffsetParaWeightStatic_R;
        public static DataType.OffsetParameter OffsetParaWeightStatic_R
        {
            get
            {
                if (IsFirstLoadOffsetParaWeightStatic_R)
                {
                    try
                    {
                        _OffsetParaWeightStatic_R = new DataType.OffsetParameter(GetAppSettings("OffsetParaWeightStatic_R"));
                    }
                    catch { }
                    IsFirstLoadOffsetParaWeightStatic_R = false;
                }
                return _OffsetParaWeightStatic_R;
            }

            set
            {
                _OffsetParaWeightStatic_R = value;
                try
                {
                    SetAppSettings("OffsetParaWeightStatic_R", _OffsetParaWeightStatic_R.GetParameterString());

                }
                catch { }
            }
        }       
      
        
       
    } 
}
