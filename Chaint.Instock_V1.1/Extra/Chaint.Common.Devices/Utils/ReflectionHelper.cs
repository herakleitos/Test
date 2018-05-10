using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Chaint.Common.Devices.Utils
{
    /// <summary>
    /// Help functions for Reflection
    /// </summary>
    public static class ReflectionHelper
    {

        #region GetDescription
        /// <overloads>
        ///		Get The Member Description using Description Attribute.
        /// </overloads>
        /// <summary>
        /// Get The Enum Field Description using Description Attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>return description or value.ToString()</returns>
        public static string GetDescription(Enum value)
        {
            return GetDescription(value, null);
        }

        /// <summary>
        /// Get The Enum Field Description using Description Attribute and 
        /// objects to format the Description.
        /// </summary>
        /// <param name="value">Enum For Which description is required.</param>
        /// <param name="args">An Object array containing zero or more objects to format.</param>
        /// <returns>return null if DescriptionAttribute is not found or return type description</returns>
        public static string GetDescription(Enum value, params object[] args)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string text1;

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            text1 = (attributes.Length > 0) ? attributes[0].Description : value.ToString();

            if ((args != null) && (args.Length > 0))
            {
                return string.Format(null, text1, args);
            }
            return text1;
        }

        /// <summary>
        ///	Get The Type Description using Description Attribute.
        /// </summary>
        /// <param name="member">Specified Member for which Info is Required</param>
        /// <returns>return null if DescriptionAttribute is not found or return type description</returns>
        public static string GetDescription(MemberInfo member)
        {
            return GetDescription(member, null);
        }

        /// <summary>
        /// Get The Type Description using Description Attribute and 
        /// objects to format the Description.
        /// </summary>
        /// <param name="member"> Specified Member for which Info is Required</param>
        /// <param name="args">An Object array containing zero or more objects to format.</param>
        /// <returns>return <see cref="String.Empty"/> if DescriptionAttribute is 
        /// not found or return type description</returns>
        public static string GetDescription(MemberInfo member, params object[] args)
        {
            string text1;

            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (member.IsDefined(typeof(DescriptionAttribute), false))
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])member.GetCustomAttributes(typeof(DescriptionAttribute), false);
                text1 = attributes[0].Description;
            }
            else
            {
                return String.Empty;
            }

            if ((args != null) && (args.Length > 0))
            {
                return String.Format(null, text1, args);
            }
            return text1;
        }

        #endregion

        #region CallMethod

        /// <overloads>
        /// Calls the method on an object using reflection.
        /// </overloads>
        /// <summary>
        /// Calls the method on an object using reflection with method name to call.
        /// </summary>
        /// <param name="obj">The object on which method is called.</param>
        /// <param name="methodName">Name of the method to execute.</param>
        /// <returns>Return Method Value</returns>
        public static object CallMethod(Object obj, string methodName)
        {
            return CallMethod(obj, methodName, null);
        } // CallMethod

        /// <summary>
        /// Calls the method on an object using reflection with method name to call and arguments to pass.
        /// </summary>
        /// <param name="obj">The object on which method is called.</param>
        /// <param name="methodName">Name of the method to execute.</param>
        /// <param name="args">An Object array containing zero or more objects to format.</param>
        /// <returns>Return Method Value</returns>
        public static object CallMethod(Object obj, string methodName, params object[] args)
        {
            if (obj == null)
            {
                return null;
            }

            if (String.IsNullOrEmpty(methodName))
            {
                return null;
            }

            Type[] argumentList;
            MethodInfo method;

            if (args != null)
            {
                int length = args.Length;
                argumentList = new Type[length];

                for (int index = 0; index < length; index++)
                {
                    argumentList[index] = args[index].GetType();
                }

                method = obj.GetType().GetMethod(methodName, argumentList);
            }
            else
            {
                method = obj.GetType().GetMethod(methodName);
            } // else


            if (method != null)
            {
                return method.Invoke(obj, args);
            }

            return null;
        } // CallMethod

        #endregion

        #region GetAttribute

        /// <overloads>
        /// Gets the specified object attributes
        /// </overloads>
        /// <summary>
        /// Gets the specified object attributes for assembly as specified by type
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="assembly">the assembly in which the specified attribute is defined</param>
        /// <returns>Attribute as Object or null if not found.</returns>
        public static object GetAttribute(Type attributeType, Assembly assembly)
        {
            if (attributeType == null)
            {
                throw new ArgumentNullException("attributeType");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }


            if (assembly.IsDefined(attributeType, false))
            {
                object[] attributes = assembly.GetCustomAttributes(attributeType, false);

                return attributes[0];
            }

            return null;
        }


        /// <summary>
        /// Gets the specified object attributes for type as specified by type
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <returns>Attribute as Object or null if not found.</returns>
        public static object GetAttribute(Type attributeType, MemberInfo type)
        {
            return GetAttribute(attributeType, type, false);
        }


        /// <summary>
        /// Gets the specified object attributes for type as specified by type with option to serach parent
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <param name="searchParent">if set to <see langword="true"/> [search parent].</param>
        /// <returns>
        /// Attribute as Object or null if not found.
        /// </returns>
        public static object GetAttribute(Type attributeType, MemberInfo type, bool searchParent)
        {
            if (attributeType == null)
            {
                return null;
            }

            if (type == null)
            {
                return null;
            }

            if (!(attributeType.IsSubclassOf(typeof(Attribute))))
            {
                return null;
            }


            if (type.IsDefined(attributeType, searchParent))
            {
                object[] attributes = type.GetCustomAttributes(attributeType, searchParent);

                if (attributes.Length > 0)
                {
                    return attributes[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the collection of all specified object attributes for type as specified by type
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <returns>Attribute as Object or null if not found.</returns>
        public static object[] GetAttributes(Type attributeType, MemberInfo type)
        {
            return GetAttributes(attributeType, type, false);
        }


        /// <summary>
        /// Gets the collection of all specified object attributes for type as specified by type with option to serach parent
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <param name="searchParent">The attribute Type for which the custom attribute is to be returned.</param>
        /// <returns>
        /// Attribute as Object or null if not found.
        /// </returns>
        public static object[] GetAttributes(Type attributeType, MemberInfo type, bool searchParent)
        {
            if (type == null)
            {
                return null;
            }

            if (attributeType == null)
            {
                return null;
            }

            if (!(attributeType.IsSubclassOf(typeof(Attribute))))
            {
                return null;
            }


            if (type.IsDefined(attributeType, false))
            {
                return type.GetCustomAttributes(attributeType, searchParent);
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Converts a named enum value to its enum equivalent.
        /// </summary>
        /// <param name="enumType">A System.Enum descendant (your enumeration)</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object StringToEnum(Type enumType, string value)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            return StringToEnum(enumType, value, false);
        }

        /// <summary>
        /// Converts a named enum value to its enum equivalent.
        /// </summary>
        /// <param name="enumType">A System.Enum descendant (your enumeration)</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <see langword="true"/> [ignore case].</param>
        /// <returns></returns>
        public static object StringToEnum(Type enumType, string value, bool ignoreCase)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            if (enumType.IsEnum)
            {
                try
                {
                    return Enum.Parse(enumType, value, ignoreCase);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
            throw new ArgumentException("not enumType");
        }
    }
}
