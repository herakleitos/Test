using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace Chaint.Common.Devices.Utils
{
    public static class EnumHelper
    {
        /// <summary>
        /// 调用方式:  int i = GetNumberByEnum<barcodetype>(barcodetype.CODE128C);
        /// </summary>
        /// <typeparam name="TEnum"> 对应枚举类型</typeparam>
        /// <param name="e">枚举值</param>
        /// <returns></returns>
        public static int GetNumberByEnum<TEnum>(TEnum e)
        {
            return Convert.ToInt32(Enum.Parse(typeof(TEnum), e.ToString()));
        }
        /// <summary>
        /// 根据文本信息获取对应的枚举值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="number"></param>
        /// <returns></returns>
        public static TEnum GetEnumbyNumber<TEnum>(string number)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), number);
        }

        /// <summary>
        /// 根据文本信息获取对应的枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// 调用方式:EnumHelper.GetDescription(UserColours.BrightPink);
        /// </summary>
        /// <param name="en">获取当前枚举值对应的描述信息</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        /// <summary>
        /// 根据某一枚举类型，获取枚举集合
        /// 调用方式: IList<barcodetype> abc = Chaint.CH.TypeUtil.CEnumHelper.GetValues<barcodetype>();
        /// </summary>
        /// <returns></returns>
        public static IList<T> GetValues<T>()
        {
            IList<T> list = new List<T>();
            foreach (object value in Enum.GetValues(typeof(T)))
            {
                list.Add((T)value);
            }
            return list;
        }


        /// <summary>
        /// 将枚举转换成Dictionary类型
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumParseDictionary(Type type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string[] names = Enum.GetNames(type);
            int[] values = (int[])Enum.GetValues(type);
            for (int i = 0; i < names.Length; i++)
            {
                dic.Add(values[i], names[i]);
            }
            return dic;
        }



    }
}

