using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.Utils
{
    //随机数生成器
    public class Randomizer
    {
        private static Random r;

        public static int RandInt(int min, int max)
        {
            CheckForRandom();
            return r.Next(min, max);
        }

        public static float RandFloat(float min, float max)
        {
            CheckForRandom();
            return Convert.ToSingle(min + r.NextDouble() * (max - min));
        }

        public static double RandDouble(double min, double max)
        {
            CheckForRandom();
            return min + r.NextDouble() * (max - min);
        }

        public static bool PercentChance(int percent)
        {
            CheckForRandom();
            return (r.Next(1, 100) <= percent);
        }

        public static void CheckForRandom()
        {
            if (r == null)
            {
                r = new Random(GetSeed());
            }
        }

        public static int GetSeed()
        {
            DateTime now = DateTime.Now;
            return (now.Day + now.DayOfYear + now.Hour + now.Millisecond +
              now.Minute + now.Month + now.Second + now.Year);
        }

        public static void Reset()
        {
            r = null;
        }

    }

}
