using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {//凑数求和的动态规划算法-- 最关键的一步是 在每层运算时  去掉重复算式和不可能得到目标值的算式，从而降低每层运算后输出的结果数量
            // 精简每层的运算结果，不同的问题有不同的方式。
            int[] a = new int[] { 1, 2, 3, 4, 5};
            int numbers = a.Length * (a.Length - 1) / 2;
            List<int[]> temp = new List<int[]>();
            int s = 3;
            List<string> resultPath = new List<string>();
            Q1(a, s, a.Length - 1, a.Length - 1, ref resultPath);
            List<string> fResult = new List<string>();
            foreach (var li in resultPath)
            {
                List<string> itemLi = li.TrimStart('+').TrimStart('-').Split(',').ToList();
                int sum = 0;
                foreach (var ii in itemLi)
                {
                    if (ii.StartsWith("+"))
                    {
                        sum += Convert.ToInt32(ii.TrimStart('+'));
                    }
                    else if (ii.StartsWith("-"))
                    {
                        sum -= Convert.ToInt32(ii.TrimStart('-'));
                    }
                    else
                    {
                        sum += Convert.ToInt32(ii);
                    }
                }
                var newLi = string.Join("", itemLi);
                Console.WriteLine($"{newLi} ---- {sum}");
                fResult.Add(newLi);
            }
            var result = string.Join(",", fResult);
            Console.Read();
        }

        public static Tuple<List<int>, List<string>> Q1(int[] a, int s, int n, int longN, ref List<string> resultPath)
        {
            if (n == 1)
            {
                List<int> ss = new List<int>();
                List<string> path = new List<string>();

                int temp1 = a[0] + a[1];
                int temp2 = a[0] - a[1];
                int temp3 = a[1] - a[0];
                var tt1 = new List<string>() { $"+{a[0]}", $"+{a[1]}" };
                string path1 = string.Join(",", tt1.OrderBy(o => o));

                var tt2 = new List<string>() { $"+{a[0]}", $"-{a[1]}" };
                string path2 = string.Join(",", tt2.OrderBy(o => o));

                var tt3 = new List<string>() { $"+{a[1]}", $"-{a[0]}" };
                string path3 = string.Join(",", tt3.OrderBy(o => o));

                if (temp1 == s)
                {
                    resultPath.Add(path1);
                }
                ss.Add(temp1);
                path.Add(path1);
                if (temp2 == s)
                {
                    resultPath.Add(path2);
                }
                ss.Add(temp2);
                path.Add(path2);
                if (temp3 == s)
                {
                    resultPath.Add(path3);
                }
                ss.Add(temp3);
                path.Add(path3);
                return new Tuple<List<int>, List<string>>(ss, path);
            }
            else
            {
                var temp = Q1(a, s, n - 1, longN, ref resultPath);
                if (temp == null) return null;
                List<int> ss = new List<int>();
                List<string> path = new List<string>();
                if (temp.Item1.Count <= 0) return null;
                var maxLeft = 0;
                if (n < longN)
                {//计算剩下的数的最大值
                    for (int j = n + 1; j <= longN; j++)
                    {
                        maxLeft += a[j];
                    }
                }
                for (int i = 0; i < temp.Item1.Count; i++)
                {
                    int temp1 = temp.Item1[i] + a[n];
                    int temp2 = temp.Item1[i] - a[n];
                    int temp3 = a[n] - temp.Item1[i];

                    List<string> aPath1 = new List<string>();
                    var prePath1 = temp.Item2[i].Split(',');
                    foreach (var tt in prePath1)
                    {
                        List<string> ttPath1 = new List<string>();
                        ttPath1.Add($"+{tt.Trim('-').Trim('+')}");
                        ttPath1.Add($"+{a[n]}");
                        int intTT1 = Convert.ToInt32(tt.Trim('-').Trim('+'));
                        int tempTT1 = intTT1 + a[n];
                        string ttPathStr1 = string.Join(",", ttPath1.OrderBy(o => o));
                        if (tempTT1 == s && !resultPath.Any(all => all == ttPathStr1))
                        {
                            resultPath.Add(ttPathStr1);
                        }
                        path.Add(ttPathStr1);
                        ss.Add(tempTT1);

                        List<string> ttPath2 = new List<string>();
                        ttPath2.Add($"+{tt.Trim('-').Trim('+')}");
                        ttPath2.Add($"-{a[n]}");
                        int intTT2 = Convert.ToInt32(tt.Trim('-').Trim('+'));
                        int tempTT2 = intTT2 - a[n];
                        string ttPathStr2 = string.Join(",", ttPath2.OrderBy(o => o));
                        if (tempTT2 == s && !resultPath.Any(all => all == ttPathStr2))
                        {
                            resultPath.Add(ttPathStr2);
                        }
                        path.Add(ttPathStr2);
                        ss.Add(tempTT2);

                        List<string> ttPath3 = new List<string>();
                        ttPath3.Add($"+{a[n]}");
                        ttPath3.Add($"-{tt.Trim('-').Trim('+')}");
                        int intTT3 = Convert.ToInt32(tt.Trim('-').Trim('+'));
                        int tempTT3 = a[n] - intTT3;
                        string ttPathStr3 = string.Join(",", ttPath3.OrderBy(o => o));
                        if (tempTT3 == s && !resultPath.Any(all => all == ttPathStr3))
                        {
                            resultPath.Add(ttPathStr3);
                        }
                        path.Add(ttPathStr3);
                        ss.Add(tempTT3);
                    }
                    aPath1.AddRange(prePath1);
                    aPath1.Add($"+{a[n]}");
                    string path1 = string.Join(",", aPath1.OrderBy(o => o));

                    List<string> aPath2 = new List<string>();
                    aPath2.AddRange(prePath1);
                    aPath2.Add($"-{a[n]}");
                    string path2 = string.Join(",", aPath2.OrderBy(o => o));

                    List<string> aPath3 = new List<string>();
                    aPath3.Add($"+{a[n]}");
                    foreach (var li in temp.Item2[i].Split(','))
                    {
                        if (li.StartsWith("+"))
                        {
                            aPath3.Add($"-{li.Trim('+')}");
                        }
                        if (li.StartsWith("-"))
                        {
                            aPath3.Add($"+{li.Trim('-')}");
                        }
                    }
                    string path3 = string.Join(",", aPath3.OrderBy(o => o));
                    if (temp1 == s && !resultPath.Any(all => all == path1))
                    {
                        resultPath.Add(path1);
                    }
                    if (temp1 + maxLeft >= s && temp1 - maxLeft <= s)
                    {//加上剩余的最大数，需要大于等于目标值S，减去剩余的最大数，需要小于等于目标值S， 否则该计算式应被舍弃
                        bool add = true;
                        if (path.Any(aa => aa == path1))
                        {//去掉重复的计算式
                            add = false;
                        }
                        if (add)
                        {
                            path.Add(path1);
                            ss.Add(temp1);
                        }
                    }
                    if (temp2 == s && !resultPath.Any(all => all == path2))
                    {
                        resultPath.Add(path2);
                    }
                    if (temp2 + maxLeft >= s && temp2 - maxLeft <= s)
                    {//加上剩余的最大数，需要大于等于目标值S，减去剩余的最大数，需要小于等于目标值S， 否则该计算式应被舍弃
                        //这种计算式不会和前面的计算式重复
                        bool add = true;
                        if (path.Any(aa => aa == path2))
                        {//去掉重复的计算式
                            add = false;
                        }
                        if (add)
                        {
                            path.Add(path2);
                            ss.Add(temp2);
                        }
                    }
                    if (temp3 == s && !resultPath.Any(all => all == path3))
                    {
                        resultPath.Add(path3);
                    }
                    if (temp3 + maxLeft >= s && temp3 - maxLeft <= s)
                    {//加上剩余的最大数，需要大于等于目标值S，减去剩余的最大数，需要小于等于目标值S， 否则该计算式应被舍弃
                        bool add = true;
                        if (path.Any(aa => aa == path3))
                        {//去掉重复的计算式
                            add = false;
                        }
                        if (add)
                        {
                            path.Add(path3);
                            ss.Add(temp3);
                        }
                    }
                }
                return new Tuple<List<int>, List<string>>(ss, path);
            }
        }
    }

}
