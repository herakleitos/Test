using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.Utils
{
    public class SortedAlgorithm
    {
        /// <summary>
        /// √∞≈›≈≈–Ú
        /// </summary>
        /// <param name="list"></param>
        public static void BubbleSorter(int[] list)
        {
            int i, j, temp;
            bool done = false;
            j = 1;
            while ((j < list.Length) && (!done))
            {
                done = true;
                for (i = 0; i < list.Length - j; i++)
                {
                    if (list[i] > list[i + 1])
                    {
                        done = false;
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }//end if
                }//end for
                j++;
            }//end while

        }

        /// <summary>
        /// —°‘Ò≈≈–Ú
        /// </summary>
        /// <param name="list"></param>
        public static void SelectionSorter(int[] list)
        {
            int min;
            for (int i = 0; i < list.Length - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < list.Length; j++)
                {
                    if (list[j] < list[min])
                        min = j;
                }

                int t = list[min];
                list[min] = list[i];

                list[i] = t;
            }//endfor
        }//end void sort

        /// <summary>
        /// ≤Â»Î≈≈–Ú
        /// </summary>
        /// <param name="list"></param>
        public static void InsertSorter(int[] list)
        {
            for (int i = 1; i < list.Length; i++)
            {
                int t = list[i];
                int j = i;
                while ((j > 0) && (list[j - 1] > t))
                {
                    list[j] = list[j - 1];
                    --j;
                }
                list[j] = t;
            }//end for
        }//end voidsort

        /// <summary>
        /// œ£∂˚≈≈–Ú
        /// </summary>
        /// <param name="list"></param>
        public static void ShellSorter(int[] list)
        {
            int inc;
            for (inc = 1; inc <= list.Length / 9; inc = 3 * inc + 1) ;
            for (; inc > 0; inc /= 3)
            {
                for (int i = inc + 1; i <= list.Length; i += inc)
                {
                    int t = list[i - 1];
                    int j = i;
                    while ((j > inc) && (list[j - inc - 1] > t))
                    {
                        list[j - 1] = list[j - inc - 1];
                        j -= inc;
                    }
                    list[j - 1] = t;
                }
            }
        }//end voidsort

        /// <summary>
        /// øÏÀŸ≈≈–Ú
        /// </summary>
        /// <param name="list"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public static void QuickSort(int[] list,int low,int high)
        {
            int temp = list[(low+high)/2];
            int i=low,j=high;
            do
            {
                while (i < high && list[i] < temp) 
                    i++;
                while (j > low && list[j] > temp)
                    j--;
                if (i <= j)
                {
                    swap(ref list[i], ref list[j]);
                    i++;
                    j--;
                }
            }
            while (i <= j);
            if (j > low)
                QuickSort(list, low, j);
            if (i < high)
                QuickSort(list, i, high);
        }

        public static void swap(ref int l, ref int r)
        {
            int temp = l; l = r; r = temp;
        }

        #region ∂‡πÿº¸◊÷◊÷∑˚¥ÆøÏ≈≈(Robert SedgewickÀ„∑®)
        static char CharOrNull(string s, int pos)
        {
            if (pos >= s.Length)
                return (char)0;
            return s[pos];
        }

        static int medianOf3(string[] x, int a, int b, int c, int depth)
        {
            char va, vb, vc;
            if ((va = CharOrNull(x[a], depth)) == (vb = CharOrNull(x[b], depth)))
                return a;
            if ((vc = CharOrNull(x[c], depth)) == va || vc == vb)
                return c;
            return va < vb ?
                  (vb < vc ? b : (va < vc ? c : a))
                : (vb > vc ? b : (va < vc ? a : c));
        }

        //Pathological case is: strings with long common prefixes will
        //          cause long running times
        public static void InsertionSort(string[] x, int a, int n, int depth)
        {
            int pi;
            int pj;
            for (pi = a + 1; --n > 0; pi++)
            {
                for (pj = pi; pj > a; pj--)
                {
                    string s = x[pj - 1];
                    string t = x[pj];
                    int d = depth;

                    int s_len = s.Length;
                    int t_len = t.Length;
                    while (d < s_len && d < t_len && s[d] == t[d]) { d++; };
                    if (d == s_len || (d < t_len && s[d] <= t[d]))
                        break;
                    int pj1 = pj - 1;
                    string tmp = x[pj]; x[pj] = x[pj1]; x[pj1] = tmp;
                }
            }
        }

        static void vecswap(string[] x, int a, int b, long n)
        {

            while (n-- > 0)
            {
                string t = x[a];
                x[a++] = x[b];
                x[b++] = t;
            }
        }


        public static string[] Sort(string[] input)
        {
            string[] copy = new string[input.Length];
            input.CopyTo(copy, 0);
            InPlaceSort(copy, 0, copy.Length, 0);
            return copy;
        }


        static void InPlaceSort(string[] x, int a, int n, int depth)
        {
            char partval;
            int d, r;
            int pa;
            int pb;
            int pc;
            int pd;
            int pl;
            int pm;
            int pn;
            string t;
            if (n < 10)
            {
                InsertionSort(x, a, n, depth);
                return;
            }
            pl = a;
            pm = a + n / 2;
            pn = a + (n - 1);
            if (n > 30)
            {
                // On big arrays, pseudomedian of 9
                d = (n / 8);
                pl = medianOf3(x, pl, pl + d, pl + 2 * d, depth);
                pm = medianOf3(x, pm - d, pm, pm + d, depth);
                pn = medianOf3(x, pn - 2 * d, pn - d, pn, depth);
            }
            pm = medianOf3(x, pl, pm, pn, depth);

            { t = x[a]; x[a] = x[pm]; x[pm] = t; }

            pa = pb = a + 1;
            pc = pd = a + n - 1;

            partval = CharOrNull(x[a], depth);
            int len = x[a].Length;
            bool empty = (len == depth);

            for (; ; )
            {
                while (pb <= pc && (r = (empty ? (x[pb].Length - depth) : ((depth == x[pb].Length) ? -1 : (x[pb][depth] - partval)))) <= 0)
                {
                    if (r == 0)
                    {
                        //swap(pa, pb);
                        { t = x[pa]; x[pa] = x[pb]; x[pb] = t; }
                        pa++;
                    }
                    pb++;
                }
                while (pb <= pc && (r = (empty ? (x[pc].Length - depth) : ((depth == x[pc].Length) ? -1 : (x[pc][depth] - partval)))) >= 0)
                {
                    if (r == 0)
                    {   //swap(pc, pd); 
                        { t = x[pc]; x[pc] = x[pd]; x[pd] = t; }
                        pd--;
                    }
                    pc--;
                }
                if (pb > pc) break;

                //swap(pb, pc);
                { t = x[pb]; x[pb] = x[pc]; x[pc] = t; }
                pb++;
                pc--;
            }

            pn = a + n;
            if (pa - a < pb - pa)
            {
                r = (pa - a);
            }
            else
            {
                r = (pb - pa);
            }

            //swapping pointers to strings
            vecswap(x, a, pb - r, r);
            if (pd - pc < pn - pd - 1) { r = pd - pc; } else { r = pn - pd - 1; }
            vecswap(x, pb, pn - r, r);
            r = pb - pa;
            if (pa - a + pn - pd - 1 > 1 && x[a + r].Length > depth) //by definition x[a + r] has at least one element
                InPlaceSort(x, a + r, pa - a + pn - pd - 1, depth + 1);
            if (r > 1)
                InPlaceSort(x, a, r, depth);
            if ((r = pd - pc) > 1)
                InPlaceSort(x, a + n - r, r, depth);
        }


        #endregion



    }
}
