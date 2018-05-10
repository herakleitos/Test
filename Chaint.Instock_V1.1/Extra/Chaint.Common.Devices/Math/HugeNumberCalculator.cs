using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Chaint.Common.Devices.Math
{
    /// <summary>
    /// 算法：超大数据值 加，减，乘，除
    /// </summary>
    public class HugeNumberOperator
    {
        public HugeNumberOperator()
        {
        }

        #region  利用数组进行大数运算

        /// <summary>
        /// 多个大数的和    (运用数组)
        /// </summary>
        /// <param name="array">多个超大数的集合</param>
        /// <returns></returns>
        public string GetAdd(ArrayList array)
        {
            int count = 0, num = 0, jin = 0;
            string s = "";
            SortedList<int, int> st = new SortedList<int, int>();
            foreach (string str in array)
            {
                if (str.Length > count)
                    count = str.Length;
            }

            for (int i = 1; i <= count; i++)
            {
                num = jin; jin = 0;
                foreach (string str in array)
                {
                    if (str.Length < i)
                        continue;
                    num += Int32.Parse(str[str.Length - i].ToString());
                }
                st.Add(i, num % 10);
                jin = num / 10;
                if (i == count & jin != 0)
                {
                    st.Add(i + 1, jin);
                }
            }

            for (int k = st.Count; k > 0; k--)
            {
                s += st[k].ToString();
            }
            return s;
        }

        /// <summary>
        /// 大数减法   (运用数组)
        /// </summary>
        /// <param name="x">减数</param>
        /// <param name="y">被减法</param>
        /// <returns>结果</returns>
        public string GetSubtract(string x, string y)
        {
            ArrayList arr = new ArrayList();
            string str = "";
            string str1 = "";
            string str2 = "";
            int int1 = 0;
            int int2 = 0;
            int jie = 0;
            bool flag = CompareStr(x, y);
            if (flag) { str1 = x; str2 = y; }
            else { str1 = y; str2 = x; }
            int index = str2.Length - 1;

            for (int i = str1.Length - 1; i >= 0; i--)
            {
                int1 = Int32.Parse(str1[i].ToString());
                if (index >= 0)
                    int2 = Int32.Parse(str2[index].ToString());
                else
                    int2 = 0;       //没有的以 0 代替

                int1 = int1 - jie;  //减去被借的的，0/1 
                if (int1 < 0)
                    int1 = 9;       //当0位被借时，用9替代

                if (int1 >= int2)
                {
                    if (int1 == 9 & jie == 1)   //当该位置为9，且被借过 1，则该位置应该向前借 1
                        jie = 1;
                    else
                        jie = 0;
                    arr.Add(int1 - int2);
                }
                else
                {
                    jie = 1;
                    arr.Add(Int32.Parse(("1" + int1).ToString()) - int2);
                }
                index--;
            }

            for (int i = arr.Count - 1; i >= 0; i--)
            {
                if (i == arr.Count - 1 & arr[i].ToString() == "0")
                    continue;
                str = str + arr[i].ToString();
            }
            if (!flag) str = "-" + str;
            return str;
        }

        /// <summary>
        /// 大数除法  (运用数组),string[1]为余数,string[0]为商
        /// </summary>
        /// <param name="x">被除数</param>
        /// <param name="y">除数</param>
        /// <returns>结果</returns>
        public string[] GetDivision(string x, string y)
        {
            string[] s = new string[2];
            string str = "";        //商 
            string temp = "";
            string sh = "";
            string sh_Top = "";
            int starIndex = 0;
            int intCompareRet = 0;

            switch (CompareString(x, y))
            {
                case -1:
                    s[0] = "0";
                    s[1] = x;
                    break;
                case 0:
                    s[0] = "1";
                    s[1] = "0";
                    break;
                case 1:
                    temp = x;
                    if (x.Length == y.Length)
                    {
                        for (int j = 1; j <= 9; j++)
                        {
                            sh = (new Number(y) * new Number(j.ToString())).ToString();
                            sh_Top = (new Number(y) * new Number((j + 1).ToString())).ToString();

                            intCompareRet = CompareString(x, sh);
                            if (intCompareRet < 0)
                            {
                                str = (j - 1).ToString();
                                sh = (new Number(y) * new Number((j - 1).ToString())).ToString();
                                sh = Subtract(x, sh);                  //余数
                                break;
                            }
                            if (intCompareRet == 0)
                            {
                                str = j.ToString();
                                sh = "0";
                                break;
                            }
                            if (intCompareRet > 0 && CompareString(x, sh_Top) < 0)
                            {
                                str = j.ToString();
                                sh = Subtract(x, sh);
                                break;
                            }
                        }
                        s[0] = str;
                        s[1] = sh;
                    }
                    else
                    {
                        //X位数＞Y位数
                        temp = x.Substring(0, y.Length);
                        starIndex = y.Length;
                        if (CompareString(temp, y) < 0 && x.Length > y.Length)
                        {
                            temp = x.Substring(0, y.Length + 1);
                            starIndex += 1;
                        }
                        for (int i = starIndex; i <= x.Length; i++)
                        {
                            for (int j = 1; j <= 9; j++)
                            {
                                sh = (new Number(y) * new Number(j.ToString())).ToString();//用除数的倍数尝试
                                sh_Top = (new Number(y) * new Number((j + 1).ToString())).ToString();

                                intCompareRet = CompareString(temp, sh);
                                if (intCompareRet < 0)
                                {
                                    str = str + (j - 1).ToString();
                                    sh = (new Number(y) * new Number((j - 1).ToString())).ToString();
                                    sh = Subtract(temp, sh);                  //余数
                                    if (i < x.Length)
                                        temp = sh + x.Substring(i, 1);
                                    break;
                                }
                                if (intCompareRet == 0)
                                {
                                    str = str + j.ToString();
                                    temp = "0";
                                    break;
                                }
                                if (intCompareRet > 0 && CompareString(temp, sh_Top) < 0)
                                {
                                    str = str + j.ToString();
                                    temp = Subtract(temp, sh);
                                    if (i < x.Length)
                                        temp = temp + x.Substring(i, 1);
                                    break;
                                }
                            }
                        }
                        s[0] = str;//商值
                        s[1] = temp == "" ? "0" : temp;//余数
                    }
                    break;

                default:
                    s[0] = "0";
                    s[1] = "0";
                    break;
            }
            return s;
        }

        /// <summary>
        /// 大数乘法  (运用数组)
        /// </summary>
        /// <param name="x">乘数</param>
        /// <param name="y">被乘数</param>
        /// <returns>乘积</returns>
        public string GetMultiply(string x, string y)
        {
            int num = 0, jin = 0;
            string s = "";
            SortedList<int, int> st = new SortedList<int, int>();
            ArrayList arrMul = new ArrayList();

            for (int j = 1; j <= x.Length; j++)                //乘数
            {
                st.Clear(); jin = 0;
                for (int k = 1; k <= y.Length; k++)            //被乘数
                {
                    num = 0;
                    num = jin;
                    num = num + Int32.Parse(x[x.Length - j].ToString()) * Int32.Parse(y[y.Length - k].ToString());
                    st.Add(k, num % 10);
                    jin = num / 10;

                    if (k == y.Length && jin != 0)
                    {
                        st.Add(k + 1, jin);
                    }
                }

                s = "";
                for (int key = st.Count; key > 0; key--)
                {
                    s += st[key].ToString();
                }
                arrMul.Add(s + AddZero(j));
            }

            //多个超大数相加
            return GetAdd(arrMul);
        }

        /// <summary>
        /// 求x的n次方   (运用数组)
        /// </summary>
        /// <param name="x">底数</param>
        /// <param name="n">乘方数</param>
        /// <returns>结果</returns>
        public string GetSquare(int x, int n)
        {
            if (n == 0)
                return "1";

            SortedList<int, int> st = new SortedList<int, int>();
            for (int i = 1; i <= x.ToString().Length; i++)
            {
                st.Add(i, Int32.Parse(x.ToString()[x.ToString().Length - i].ToString()));
            }

            /**************************************************************************/
            /** 数组最大长度 4294967296（int类型）
            /**************************************************************************************/
            int num = 0;
            int jin = 0;        //进位数
            string str = "";
            ArrayList arr = new ArrayList();
            for (int i = 1; i < n; i++)
            {
                arr.Clear();
                for (int key = 1; key <= st.Count; key++)
                {
                    num = st[key] * x;
                    //如果是十位以上（包含十位），则要补0
                    arr.Add(num.ToString() + AddZero(key));
                }

                //将乘的结果相加
                num = 0; jin = 0;
                st.Clear();
                for (int j = 1; j <= arr[arr.Count - 1].ToString().Length; j++)
                {
                    num = 0;
                    num = jin;
                    jin = 0;
                    foreach (string s in arr)
                    {
                        if (s.Length < j)
                            continue;
                        num += Int32.Parse(s[s.Length - j].ToString());
                    }
                    jin = num / 10;

                    //将新的结果导入    SortedList
                    st.Add(j, num % 10);
                    if (j == arr[arr.Count - 1].ToString().Length && jin != 0)
                    {
                        st.Add(j + 1, jin);
                    }
                }
            }

            /**************************************************************************************/
            for (int key = st.Count; key > 0; key--)
            {
                str = str + st[key].ToString();
            }
            Console.WriteLine(str.Length);
            return str;
        }

        /// <summary>
        /// 数字补0
        /// </summary>
        /// <param name="key">进位</param>
        /// <returns></returns>
        private string AddZero(int key)
        {
            string s = "";
            if (key == 1)
            {
                return s;
            }
            for (int i = 2; i <= key; i++)
            {
                s = s + "0";
            }
            return s;
        }

        /// <summary>
        /// 比较两个数字字符串的大小:x>y return true;
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool CompareStr(string x, string y)
        {
            bool flag = true;
            if (x.Length > y.Length)
                return true;
            if (x.Length < y.Length)
                return false;
            for (int i = 0; i < x.Length; i++)
            {
                if (Int32.Parse(x[i].ToString()) == Int32.Parse(y[i].ToString()))
                    continue;
                if (Int32.Parse(x[i].ToString()) > Int32.Parse(y[i].ToString()))
                {
                    flag = true;
                    break;
                }
                if (Int32.Parse(x[i].ToString()) < Int32.Parse(y[i].ToString()))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 比较两个数字型字符串的大小，相等：0, 大于：1，小于：-1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CompareString(string x, string y)
        {
            int flag = 1;
            if (x.Length > y.Length)
                return 1;
            if (x.Length < y.Length)
                return -1;
            for (int i = 0; i < x.Length; i++)
            {
                if (Int32.Parse(x[i].ToString()) == Int32.Parse(y[i].ToString()))
                {
                    flag = 0;
                    continue;
                }
                if (Int32.Parse(x[i].ToString()) > Int32.Parse(y[i].ToString()))
                {
                    flag = 1;
                    break;
                }
                if (Int32.Parse(x[i].ToString()) < Int32.Parse(y[i].ToString()))
                {
                    flag = -1;
                    break;
                }
            }
            return flag;
        }

        #endregion


        //*********************************************加，减，乘运算,运用 Number 类******************************************************
        public string Add(string x, string y)
        {
            return ((new Number(x) + new Number(y)).ToString());
        }
        public string Multiply(string x, string y)
        {
            return ((new Number(x) * new Number(y)).ToString());
        }
        public string Subtract(string x, string y)
        {
            return ((new Number(x) - new Number(y)).ToString());
        }
        /// <summary>
        /// 求x的n次方,运用 Number类
        /// </summary>
        /// <param name="x">底数</param>
        /// <param name="n">乘方数</param>
        /// <returns>结果</returns>
        public string Square(string x, int n)
        {
            string rs = x.ToString();
            for (int i = 0; i < n - 1; i++)
            {
                rs = (new Number(x.ToString()) * new Number(rs)).ToString();
            }
            return rs;
        }

        //*************************************************二进制运算,暂未完善******************************************************
        public string BAdd(ArrayList arr)
        {
            string str = "";

            return str;
        }
        public string BMultiply(string x, string y)
        {
            ArrayList arr = new ArrayList();

            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] != '0')
                {
                    arr.Add(x + AddZero(i + 1));
                }
            }
            return BAdd(arr);
        }
        public string BSubtract(string x, string y)
        {
            return "";
        }


        /// <summary>
        /// 将数字型字符串的值存储至字节数据组中
        /// (按倒序方式存储:如:260存储至B[1]B[0]中时为:B[1]=4,B[0]=1  1*256+4=260)
        /// </summary>
        /// <param name="strnum">数字型字符串</param>
        /// <param name="w">数组长度</param>
        /// <returns></returns>
        public byte[] NumberToByte(string strnum, int length)
        {
            byte[] buffer = new byte[length];
            string[] strDivision = new string[2];

            for (int i = length - 1; i > 0; i--)
            {
                if (i == 2)
                {
                    Console.WriteLine(i);
                }
                strDivision = GetDivision(strnum, GetSquare(256, i));
                if (strDivision[0] == "")
                    strDivision[0] = "0";

                buffer[length - 1 - i] = Convert.ToByte(strDivision[0]);//商值
                strnum = strDivision[1];//余数
            }
            buffer[length - 1] = Convert.ToByte(strDivision[1]);
            return buffer;
        }


        /// <summary>
        /// 将字节数组的值转换为字符串的值
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="begin">实际开始位置，从0开始</param>
        /// <param name="end">结束位置,最大为length-1</param>
        /// <returns></returns>
        public string ByteToNumber(byte[] buffer, int begin, int end)
        {
            string retValue = "0";
            ArrayList arAddNumber = new ArrayList();
            if (begin < 0 || end > buffer.Length - 1)
                return "0";
            for (int i = begin; i < end + 1; i++)
            {
                if (buffer[i] == 0)
                    continue;
                else
                {

                    string x = buffer[i].ToString();
                    string y = GetSquare(256, buffer.Length - 1 - i);
                    retValue = GetMultiply(x, y);
                    arAddNumber.Add(retValue);
                }
            }
            return GetAdd(arAddNumber);
        }

        public string ByteToNumber(byte[] buffer)
        {
            string retValue = "0";
            ArrayList arAddNumber = new ArrayList();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == (byte)0)
                    continue;
                else
                {
                    string x = buffer[i].ToString();
                    string y = GetSquare(256, buffer.Length - 1 - i);
                    retValue = GetMultiply(x, y);
                    arAddNumber.Add(retValue);
                }
            }
            return GetAdd(arAddNumber);
        }

        public byte[] LongToByte(ulong num, int w)
        {
            byte[] buffer = new byte[w];
            if (w > 4 || w <= 0)
                return buffer;
            for (int i = w - 1; i > 0; i--)
            {
                buffer[w - 1 - i] = (byte)(num / Convert.ToUInt64(System.Math.Pow(256, i)));
                num = num - (ulong)(buffer[w - 1 - i] * (int)Convert.ToUInt64(System.Math.Pow(256, i)));
            }
            buffer[w - 1] = (byte)(num);
            return buffer;
        }

        /// <summary>
        ///		<remark>将byte数组转化为int</remark> 	
        ///</summary>  
        ///     <param name="buffer">PLC数据缓存</param>
        ///     <param name="Begin">开始位置</param>
        ///     <param name="Begin">结束位置</param>
        ///     <retvalue>返回数据</retvalue>		  
        public ulong ByteToLong(byte[] buffer, int Begin, int End)
        {
            try
            {
                if (Begin < 0 || End > 3)
                    return 0;
                ulong retValue = 0;
                for (int i = End; i >= Begin; i--)
                {
                    retValue += buffer[i] * Convert.ToUInt64(System.Math.Pow(256, End - i));
                }
                return retValue;
            }
            catch
            {
                return 0;
            }
        }

    }


    /// <summary>
    /// 自定义数据类型： Number
    /// </summary>
    public sealed class Number
    {
        public Number()
            : this(false, string.Empty)
        { }

        /**/
        /// <summary>
        /// 构造一个数的Number实例
        /// </summary>
        /// <param name="num">数值</param>
        public Number(long num)
            : this(false, num.ToString())
        { }

        /**/
        /// <summary>
        /// 构造一个数字串的Number实例
        /// </summary>
        /// <param name="numStr"></param>
        public Number(string numStr)
        {
            if (numStr.StartsWith("-"))
            {
                this.isNegative = true;
                numStr = numStr.Substring(1);
            }
            else
            {
                this.isNegative = false;
            }

            this.Val = numStr;
        }

        /**/
        /// <summary>
        /// 构造一个数的Number实例
        /// </summary>
        /// <param name="negative">是否为负数</param>
        /// <param name="num">数字串</param>
        public Number(bool negative, string num)
        {
            this.isNegative = negative;
            this.Val = num;
        }

        /**/
        /// <summary>
        /// 计算数的被拆的最小长度，必须为2的整次幂，且小于等于8
        /// </summary>
        private static readonly int part = 4;

        /**/
        /// <summary>
        /// 计算两数之差，要求两数必须为正。且大数在前，小数在后
        /// </summary>
        /// <param name="n1">被减数</param>
        /// <param name="n2">减数</param>
        /// <returns></returns>
        private static Number Sub(Number n1, Number n2)
        {
            if (n1.isNegative || n2.isNegative)
                throw new ArgumentException("Number cant't be negative.");

            string a = Fill(n1.Val, System.Math.Max(n1.Length, n2.Length));
            string b = Fill(n2.Val, System.Math.Max(n1.Length, n2.Length));

            switch (CompareStr(a, b))
            {
                case 0:
                    return new Number(0);
                case 1:
                    {
                        Number reNum = new Number();
                        reNum.isNegative = false;
                        string result = string.Empty;
                        bool sf = false;
                        int i = 1;
                        while (i <= a.Length)
                        {
                            int r = Convert.ToInt32(a.Substring(a.Length - i, 1)) -
                                Convert.ToInt32(b.Substring(a.Length - i, 1)) - (sf ? 1 : 0);
                            result = Convert.ToString(r < 0 ? r + 10 : r) + result;
                            sf = r < 0;
                            i++;
                        }
                        reNum.Val = result;
                        return reNum;
                    }
                case -1:
                    {
                        Number reNum = Sub(new Number(false, b), new Number(false, a));
                        reNum.isNegative = true;
                        return reNum;
                    }
                default:
                    throw new ArgumentException("Inerrorct compareStr value.");
            }
        }

        /**/
        /// <summary>
        /// 计算两数之和，要求两数必须为正。
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        private static Number Add(Number n1, Number n2)
        {
            if (n1.isNegative || n2.isNegative)
                throw new ArgumentException("Number cant't be negative.");

            string a = Fill(n1.Val, System.Math.Max(n1.Length, n2.Length));
            string b = Fill(n2.Val, System.Math.Max(n1.Length, n2.Length));

            string result = string.Empty;
            bool sf = false;
            int i = 1;
            while (i <= a.Length)
            {
                int r = Convert.ToInt32(a.Substring(a.Length - i, 1)) +
                    Convert.ToInt32(b.Substring(a.Length - i, 1)) + (sf ? 1 : 0);
                result = Convert.ToString(r >= 10 ? r - 10 : r) + result;
                sf = r >= 10;
                i++;
            }
            if (sf) result = "1" + result;
            return new Number(false, result);
        }

        /**/
        /// <summary>
        /// 把一个数拆成两个数，被拆数长度必须为偶数
        /// </summary>
        /// <param name="sou">被拆数</param>
        /// <param name="a">高位</param>
        /// <param name="b">低位</param>
        private static void DevideTo(Number sou, out Number a, out Number b)
        {
            if (sou.Length == 0 || sou.Length % 2 == 1)
                throw new ArgumentException("Devided num not a valid value.");
            string value = sou.val;
            a = new Number(sou.IsNegative, value.Substring(0, value.Length / 2));
            b = new Number(sou.IsNegative, value.Substring(value.Length / 2));
        }

        /**/
        /// <summary>
        /// 按2的整次幂，用0进行填充。
        /// </summary>
        /// <param name="nm">被填充数</param>
        /// <param name="maxLeng">希望被填充的大小，若不为2的整次幂，则长度为大小此数的下一个2的整次幂数</param>
        /// <returns></returns>
        private static Number PowerFill(Number nm, int maxLeng)
        {
            int p = 1;
            while (p < maxLeng)
            {
                p *= 2;
            }
            Number re = new Number();
            re.IsNegative = nm.IsNegative;
            re.val = Fill(nm.val, p);
            return re;
        }

        /**/
        /// <summary>
        /// 计算两数相乘
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Number Multiply(Number x, Number y, int n)
        {
            if ((x.Val.Length + y.Val.Length) > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("The length of the result exceeds maxinum value:2147483647}");

            if (x == Number.Zero || y == Number.Zero)
                return Number.Zero;

            Number a, b, c, d;
            if (x.Length > part) //x.length = y.length
            {
                DevideTo(x, out a, out b);
                DevideTo(y, out c, out d);

                Number ac = Multiply(a, c, n / 2);
                Number bd = Multiply(b, d, n / 2);
                //Number a_b_d_c = (a - b) * (d - c);
                //Number n1 = PowTen(ac, n);
                //Number n2 = PowTen(a_b_d_c + ac + bd, n / 2);
                //return n1 + n2 + bd;
                return PowTen(ac, n) + PowTen((a - b) * (d - c) + ac + bd, n / 2) + bd;
            }
            else
            {
                Number result = new Number(Convert.ToInt32(x.val) * Convert.ToInt32(y.val));
                return result;
            }
        }

        /// <summary>
        /// 计算一个数的与10的l次幂的积
        /// </summary>
        /// <param name="num">被乘数</param>
        /// <param name="l">10的L次方</param>
        /// <returns></returns>
        private static Number PowTen(Number num, int l)
        {
            if (num == Number.Zero) return Number.Zero;
            Number re = new Number();
            re.IsNegative = num.IsNegative;
            re.val = num.val;
            while (l > 0)
            {
                re.val += "0";
                l--;
            }
            return re;
        }

        #region operator override
        public static Number operator *(Number x, Number y)
        {
            if (x == Number.Zero || y == Number.Zero)
                return Number.Zero;
            Number a = PowerFill(x, System.Math.Max(x.Length, y.Length));
            Number b = PowerFill(y, System.Math.Max(x.Length, y.Length));
            a.IsNegative = b.IsNegative = false;

            Number nm = Multiply(a, b, a.Length);
            nm.IsNegative = x.IsNegative == y.IsPasstive;
            return nm;
        }

        public static bool operator ==(Number a, Number b)
        {
            if (a.isNegative != b.isNegative)
                return false;
            if (CompareStr(a.Val, b.Val) == 0)
                return true;
            return false;
        }

        public static bool operator !=(Number a, Number b)
        {
            return !(a == b);
        }

        public static bool operator >(Number a, Number b)
        {
            if (a.isNegative && b.IsPasstive)
                return false;
            if (a.IsPasstive && b.isNegative)
                return true;

            int cs = CompareStr(Trim(a.val), Trim(b.val));
            switch (cs)
            {
                case 0:
                    return true;
                case 1:
                    return !a.isNegative;
                case -1:
                    return a.isNegative;
                default:
                    throw new ArgumentException("Inerrorct compareStr value.");
            }
        }

        public static bool operator >=(Number a, Number b)
        {
            return (a == b) || (a > b);
        }

        public static bool operator <(Number a, Number b)
        {
            return !(a > b);
        }

        public static bool operator <=(Number a, Number b)
        {
            return (a == b) || (a < b);
        }

        public static Number operator +(Number a, Number b)
        {
            Number re;
            a = a.Clone();
            b = b.Clone();
            if (a.IsNegative && b.IsNegative)
            {
                a.isNegative = b.isNegative = false;
                re = Add(a, b);
                re.isNegative = true;
            }
            else if (a.IsPasstive && b.IsPasstive)
            {
                re = Add(a, b);
            }
            else if (a.IsNegative && b.IsPasstive)
            {
                a.IsPasstive = true;
                re = Sub(b, a);
            }
            else// if(a.IsPasstive && b.IsNegative)
            {
                b.IsPasstive = true;
                re = Sub(a, b);
            }
            return re;
        }

        public static Number operator -(Number a, Number b)
        {
            b = b.Clone();
            b.isNegative = !b.isNegative;
            return a + b;
        }
        #endregion

        /**/
        /// <summary>
        /// 把一个数字串，按leng长度用0进行填充
        /// </summary>
        /// <param name="val">被埴数字串</param>
        /// <param name="leng">最大长度</param>
        /// <returns></returns>
        private static string Fill(string val, int leng)
        {
            if (val.Length == leng)
                return val;
            else if (val.Length > leng)
                throw new OverflowException("The length of val can't above leng.");
            else
                while (val.Length != leng)
                    val = "0" + val;
            return val;
        }

        /**/
        /// <summary>
        /// 检查字符串是否为合法的数字串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool Check(string val)
        {
            val = val.Trim();
            for (int i = 0; i < val.Length; i++)
            {
                if (!char.IsNumber(val[i]))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            if (IsNegative) return "-" + this.Val;
            return Val;
        }

        /**/
        /// <summary>
        /// 删除数字串前多余的0填充
        /// </summary>
        /// <param name="valNum"></param>
        /// <returns></returns>
        private static string Trim(string valNum)
        {
            int i = 0;
            while (i < valNum.Length && (char.GetNumericValue(valNum, i) == 0)) i++;
            valNum = valNum.Substring(i);
            if (valNum.Length == 0 || (valNum.Length == 1 && valNum == "0"))
                return "0";
            return valNum;
        }

        /**/
        /// <summary>
        /// 比较两数串表示的数的大小
        /// </summary>
        /// <param name="des"></param>
        /// <param name="sour"></param>
        /// <returns>1 大于，0 相等， -1 小于</returns>
        private static int CompareStr(string des, string sour)
        {
            string a = Trim(des);
            string b = Trim(sour);
            if (a.Length == b.Length)
            {
                if (a.Length == 0) return 0;
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] > b[i]) return 1;
                    else if (a[i] < b[i]) return -1;
                    else continue;
                }
                return 0;
            }
            if (a.Length > b.Length)
                return 1;
            else
                return -1;
        }

        /**/
        /// <summary>
        /// 返回该数的一个副本
        /// </summary>
        /// <returns></returns>
        public Number Clone()
        {
            return new Number(this.isNegative, this.Val);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        private bool isNegative;
        private string val;
        public bool IsPasstive
        {
            get { return !this.IsNegative; }
            set { this.IsNegative = !value; }
        }
        public bool IsNegative
        {
            get { return isNegative; }
            set { isNegative = value; }
        }

        /**/
        /// <summary>
        /// 该数的数字
        /// </summary>
        public string Val
        {
            get
            {
                return Trim(val);
            }
            set
            {
                if (Check(value))
                    val = value;
                else
                    throw new ArgumentException("Num is not a valid value.");
            }
        }

        /**/
        /// <summary>
        /// 该数的位数
        /// </summary>
        public int Length
        {
            get { return this.val.Length; }
        }

        /**/
        /// <summary>
        /// 数字0的Number实例
        /// </summary>
        public static Number Zero
        {
            get { return new Number(0); }
        }
    }


    /// <summary>
    /// 自定义数据类型保存大整数，此方法暂未完善
    /// </summary>
    public sealed class BigInteger
    {

        private const int maxLength = 80; //最大32位无符号整数长度，80*32 = 2560位

        private uint[] data;              //用于保存大数组      

        public int dataLength;            //实际使用32位无符号整数长度


        //用无符号整形数构造

        public BigInteger(uint[] inData)
        {

            dataLength = inData.Length;

            if (dataLength > maxLength)

                throw (new ArithmeticException("用无符号整形数构造溢出。"));

            data = new uint[maxLength];

            for (int i = dataLength - 1, j = 0; i >= 0; i--, j++) data[j] = inData[i];

        }
        //重载 + 运算 

        public static BigInteger operator +(BigInteger bi1, BigInteger bi2)
        {
            //BigInteger result = new BigInteger();
            //result.dataLength = (bi1.dataLength > bi2.dataLength) ? bi1.dataLength : bi2.dataLength;
            BigInteger result;
            if (bi1.dataLength > bi2.dataLength)
            {
                result = new BigInteger(new uint[bi1.dataLength]);
                result.dataLength = bi1.dataLength;
            }
            else
            {
                result = new BigInteger(new uint[bi2.dataLength]);
                result.dataLength = bi2.dataLength;
            }

            long carry = 0;

            for (int i = 0; i < result.dataLength; i++)
            {

                long sum = (long)bi1.data[i] + (long)bi2.data[i] + carry;

                carry = sum >> 32;

                result.data[i] = (uint)(sum & 0xFFFFFFFF);
            }

            return result;

        }

        public static BigInteger operator -(BigInteger bi1, BigInteger bi2)
        {

            //BigInteger result = new BigInteger();
            //result.dataLength = (bi1.dataLength > bi2.dataLength) ? bi1.dataLength : bi2.dataLength;
            BigInteger result;
            if (bi1.dataLength > bi2.dataLength)
            {
                result = new BigInteger(new uint[bi1.dataLength]);
                result.dataLength = bi1.dataLength;
            }
            else
            {
                result = new BigInteger(new uint[bi2.dataLength]);
                result.dataLength = bi2.dataLength;
            }

            long carryIn = 0;

            for (int i = 0; i < result.dataLength; i++)
            {

                long diff = (long)bi1.data[i] - (long)bi2.data[i] - carryIn;

                result.data[i] = (uint)(diff & 0xFFFFFFFF);

                carryIn = (diff < 0) ? 1 : 0;

            }

            // 若进位位〈0高位部分设置为负数补码形式

            if (carryIn != 0)
            {

                for (int i = result.dataLength; i < maxLength; i++)
                    result.data[i] = 0xFFFFFFFF;

                result.dataLength = maxLength;

            }

            while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0) result.dataLength--;

            return result;

        }

        public static BigInteger operator *(BigInteger bi1, BigInteger bi2)
        {

            //int lastPos = maxLength - 1;

            //bool bi1Neg = false, bi2Neg = false;

            // 取绝对值

            //if ((bi1.data[lastPos] & 0x80000000) != 0)
            //{ // bi1 是负数

            //    bi1Neg = true; bi1 = -bi1;
            //}

            //if ((bi2.data[lastPos] & 0x80000000) != 0)
            //{     // bi2 是负数

            //    bi2Neg = true; bi2 = -bi2;
            //}
            BigInteger result = new BigInteger(new uint[bi1.dataLength + bi2.dataLength]);

            //BigInteger result = new BigInteger();

            // 绝对值乘法

            for (int i = 0; i < bi1.dataLength; i++)
            {

                if (bi1.data[i] == 0) continue;

                ulong mcarry = 0;

                for (int j = 0, k = i; j < bi2.dataLength; j++, k++)
                {

                    ulong val = (bi1.data[i] * bi2.data[j]) + result.data[k] + mcarry;

                    result.data[k] = (uint)(val & 0xFFFFFFFF);

                    mcarry = (val >> 32);

                }

                if (mcarry != 0) result.data[i + bi2.dataLength] = (uint)mcarry;

            }

            result.dataLength = bi1.dataLength + bi2.dataLength;

            if (result.dataLength > maxLength) result.dataLength = maxLength;

            while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0) result.dataLength--;

            // 输入的两个数符号相反，则为 -ve

            //if (bi1Neg != bi2Neg) return -result;

            return result;

        }

        public static BigInteger operator /(BigInteger bi1, BigInteger bi2)
        {
            BigInteger result = new BigInteger(new uint[bi1.dataLength]);
            return result;
        }

        public static BigInteger operator %(BigInteger bi1, BigInteger bi2)
        {
            BigInteger result = new BigInteger(new uint[bi1.dataLength]);
            return result;
        }
    }

    /*
    /// <summary>
    /// 运用位移运算，方法暂时未完善
    /// </summary>
    public struct BigInteger
    {
        private short _sign;
        private uint[] _data;

        public static BigInteger Pow(BigInteger baseValue, BigInteger exponent)
        {
            if (exponent < 0)
                throw new ArgumentOutOfRangeException("exponent", "NonNegative");
            if (exponent == 0)
            {
                return One;
            }
            BigInteger integer = baseValue;
            BigInteger result = One;
            while (exponent > 0)
            {
                if ((exponent._data[0] & 1) != 0)
                {
                    result *= integer;
                }
                if (exponent == 1)
                {
                    return result;
                }
                integer = integer.Square();
                exponent = RightShift(exponent, 1);
            }
            return result;
        }
        public static BigInteger RightShift(BigInteger exponent, int n)
        { }

        public static BigInteger Square()
        { }
        // other members, implement *, Square, RightShift, One (value is 1)
    }*/
}

