using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Utils
{
    public class TypeParser
    {
        public static byte ByteParse(object obj)
        {
            byte ret = 0;
            try
            {
                ret = byte.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static Int16 Int16Parse(object obj)
        {
            Int16 ret = 0;
            try
            {
                ret = Int16.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static short ShortParse(object obj)
        {
            Int16 ret = 0;
            try
            {
                ret = short.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static UInt16 UShortParse(object obj)
        {
            return UInt16Parse(obj);
        }

        public static UInt16 UInt16Parse(object obj)
        {
            UInt16 ret = 0;
            try
            {
                ret = UInt16.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static UInt32 UInt32Parse(object obj)
        {
            UInt32 ret = 0;
            try
            {
                ret = UInt32.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static UInt64 UInt64Parse(object obj)
        {
            UInt64 ret = 0;
            try
            {
                ret = UInt64.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static int IntParse(object obj)
        {
            int ret = 0;
            try
            {
                ret = int.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static uint UIntParse(object obj)
        {
            uint ret = 0;
            try
            {
                ret = uint.Parse(obj.ToString());
                return ret;
            }
            catch
            { return 0; }
        }

        public static bool BoolParse(object obj)
        {
            bool ret = true;
            try
            {
                ret = bool.Parse(obj.ToString().ToLower());
                return ret;
            }
            catch
            { return true; }
        }

        public static long Int64Parse(object obj)
        {
            long ret = 0;
            try
            {
                ret = long.Parse(obj.ToString().ToLower());
                return ret;
            }
            catch
            { return 0; }
        }

        public static decimal DecimalParse(object obj)
        {
            decimal ret = 0;
            try
            {
                ret = decimal.Parse(obj.ToString().ToLower());
                return ret;
            }
            catch
            { return 0; }
        }

        public static float FloatParse(object obj)
        {
            float ret = 0.0F;
            try
            {
                ret = float.Parse(obj.ToString().ToLower());
                return ret;
            }
            catch
            { return 0; }
        }

        public static double DoubleParase(object obj)
        {
            double ret = 0;
            try
            {
                ret = double.Parse(obj.ToString().ToLower());
                return ret;
            }
            catch
            { return 0; }
        }


    }
}
