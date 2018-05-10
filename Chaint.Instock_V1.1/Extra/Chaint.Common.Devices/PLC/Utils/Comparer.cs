using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Chaint.Common.Devices.PLC.Utils
{

    public class IntComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x > y)
                return 1;
            else if (x == y)
                return 0;
            else
                return -1;
        }
    }

    public class StringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're equal
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.Length.CompareTo(y.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.CompareTo(y);
                    }
                }
            }
        }
    }

    /// <summary>
    /// PLC 对象比较器 仅比较DWNO值的大小
    /// </summary>
    public class PlcObjectCompare : IComparer<PLCObject>
    {
        public int Compare(PLCObject obj1, PLCObject obj2)
        {
            if (obj1.DWNO > obj2.DWNO)
                return 1;
            else if (obj1.DWNO == obj2.DWNO)
                return 0;
            else
                return -1;
        }
    }

}
