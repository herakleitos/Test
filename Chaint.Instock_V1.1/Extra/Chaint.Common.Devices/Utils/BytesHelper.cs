using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;


namespace Chaint.Common.Devices.Utils
{
    /// Help functions for Bytes.
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public static class ByteHelper
    {
        #region Methods

        /// <summary>
        /// Determine if two byte arrays are equal.
        /// </summary>
        /// <param name="byte1">The first byte array to compare.</param>
        /// <param name="byte2">The byte array to compare to the first.</param>
        /// <returns><see langword="true"/> if the two byte arrays are equal; otherwise <see langword="false"/>.</returns>
        public static bool Compare(byte[] byte1, byte[] byte2)
        {
            if (byte1 == null)
            {
                return false;
            }

            if (byte2 == null)
            {
                return false;
            }

            if (byte1.Length != byte2.Length)
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < byte1.Length; i++)
            {
                if (byte1[i] != byte2[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Combines two byte arrays into one.
        /// </summary>
        /// <param name="byte1">The prefixed bytes.</param>
        /// <param name="byte2">The suffixed bytes.</param>
        /// <returns>The combined byte arrays.</returns>
        public static byte[] Combine(byte[] byte1, byte[] byte2)
        {
            byte[] tmpByte1 = null, tmpByte2 = null;
            
            if (byte1 == null)
                tmpByte1 = new byte[0];
            else
                tmpByte1 = byte1;

            if (byte2 == null)
                tmpByte2 = new byte[0];
            else
                tmpByte2 = byte2;

            byte[] combinedBytes = new byte[tmpByte1.Length + tmpByte2.Length];
            Buffer.BlockCopy(byte1, 0, combinedBytes, 0, tmpByte1.Length);
            Buffer.BlockCopy(byte2, 0, combinedBytes, tmpByte1.Length, tmpByte2.Length);
            return combinedBytes;
        }

        /// <summary>
        /// Copys a byte arrays into new one.
        /// </summary>
        /// <param name="byte1">The byte arrays to which new array is created.</param>
        /// <returns>The cloned byte arrays.</returns>
        public static byte[] Clone(byte[] byte1)
        {
            if (byte1 == null)
            {
                throw new Exception("byte1 is null");
            }

            byte[] copyBytes = new byte[byte1.Length];

            Buffer.BlockCopy(byte1, 0, copyBytes, 0, byte1.Length);

            return copyBytes;


        }

        /// <summary>
        /// <para>Returns a byte array from a string representing a hexidecimal number.</para>
        /// </summary>
        /// <param name="hexadecimalNumber">
        /// <para>The string containing a valid hexidecimal number.</para>
        /// </param>
        /// <returns><para>The byte array representing the hexidecimal.</para></returns>
        public static byte[] GetBytesFromHexString(string hexadecimalNumber)
        {
            if (String.IsNullOrEmpty(hexadecimalNumber))
            {
                throw new ArgumentNullException("hexadecimalNumber");
            }

            StringBuilder sb = new StringBuilder(hexadecimalNumber.ToUpper(CultureInfo.CurrentCulture));

            if (sb[0].Equals('0') && sb[1].Equals('X'))
            {
                sb.Remove(0, 2);
            }

            if (sb.Length % 2 != 0)
            {
                throw new ArgumentException("InvalidHexString");
            }

            byte[] hexBytes = new byte[sb.Length / 2];
            try
            {
                for (int i = 0; i < hexBytes.Length; i++)
                {
                    int stringIndex = i * 2;
                    hexBytes[i] = Convert.ToByte(sb.ToString(stringIndex, 2), 16);
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return hexBytes;
        }

        /// <summary>
        /// <para>Returns a string from a byte array represented as a hexidecimal number (eg: 0F351A).</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to convert to forat as a hexidecimal number.</para>
        /// </param>
        /// <returns>
        /// <para>The formatted representation of the bytes as a hexidecimal number.</para>
        /// </returns>
        public static string GetHexStringFromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException("bytes", new Exception("Byte array value must be greater than zero bytes"));
            }

            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2", CultureInfo.CurrentCulture));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a cryptographically strong random set of bytes.
        /// </summary>
        /// <param name="size">The size of the byte array to generate.</param>
        /// <returns>The computed bytes.</returns>
        public static byte[] GetRandomBytes(int size)
        {
            byte[] randomBytes = new byte[size];
            DateTime now = DateTime.Now;
            int intSeedValue =0;
            Random r;

            for (int i = 0; i < randomBytes.Length; i++)
            {
                now = DateTime.Now;
                intSeedValue = (now.Day + now.DayOfYear + now.Hour + now.Millisecond + now.Minute + now.Month + now.Second + now.Year);
                r = new Random(intSeedValue);
                randomBytes[i]=(Byte)r.Next(1, 255);
                System.Threading.Thread.Sleep(10);
            }
            return randomBytes;
        }

        /// <summary>
        /// <para>Fills <paramref name="bytes"/> zeros.</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to fill.</para>
        /// </param>
        public static void ZeroOutBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }
            Array.Clear(bytes, 0, bytes.Length);
        }

        #endregion
    }

}
