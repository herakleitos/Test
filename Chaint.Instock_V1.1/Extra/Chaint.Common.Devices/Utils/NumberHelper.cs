using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Chaint.Common.Devices.Utils
{
    public class NumberHelper
    {
        /// <summary>
        /// Get the LoWord (the least significant 2 bytes) from a 4-byte DoubleWord (aka Integer).
        /// retrieves the low-order word from the specified value
        /// </summary>
        /// <param name="value">Specifies the value to be converted</param>
        /// <returns>The return value is the low-order word of the specified value.</returns>
        public static short GetLOWord(int value)
        {
            return (short)(value & 0xffff);
        }

        /// <summary>
        /// Get he HiWord (the most significant 2 bytes) from a 4-byte DoubleWord (aka Integer).
        /// </summary>
        /// <param name="value">Specifies the value to be converted</param>
        /// <returns>The return value is the high-order word of the specified value.</returns>
        public static short GetHIWord(int value)
        {
            return (short)((value >> 16) & 0xffff);
        }

        /// <summary>
        /// Combines two 2-byte Words (aka Shrort) to one 4-byte DoubleWord (aka Integer).
        /// </summary>
        /// <param name="loValue">Specifies the low-order word of the new value.</param>
        /// <param name="hiValue">Specifies the high-order word of the new value.</param>
        public static int MakeLong(short loValue, short hiValue)
        {
            return (hiValue << 16) | (loValue & 0xffff);
        }

        /// <summary>
        /// Formats the specified number and return a string representation .
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <returns></returns>
        public static string ToString(int number)
        {
            return number.ToString(NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Formats the specified number and return a string representation .
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <returns></returns>
        public static string ToString(long number)
        {
            return number.ToString(NumberFormatInfo.InvariantInfo);
        }
        
    }


}
