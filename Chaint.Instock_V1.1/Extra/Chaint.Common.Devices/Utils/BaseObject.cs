using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Chaint.Common.Devices.Utils
{
    #region 所有要放在PropertyGird中的对像的基类.

    public class BaseObject : ICustomTypeDescriptor
    {
        private PropertyDescriptorCollection globalizedProps;

        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);

        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            if (globalizedProps == null)
            {
                PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(this, attributes, true);

                globalizedProps = new PropertyDescriptorCollection(null);

                foreach (PropertyDescriptor oProp in baseProps)
                {
                    //globalizedProps.Add(new GlobalizedPropertyDescriptor(oProp));
                    globalizedProps.Add(new BasePropertyDescriptor(oProp));
                }
            }
            return globalizedProps;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            if (globalizedProps == null)
            {
                PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(this, true);
                globalizedProps = new PropertyDescriptorCollection(null);

                foreach (PropertyDescriptor oProp in baseProps)
                {
                    globalizedProps.Add(new BasePropertyDescriptor(oProp));
                }
            }
            return globalizedProps;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }
    #endregion

    #region 所以要放在PropertyGird中的对像的描绘进行重写

    public class BasePropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor basePropertyDescriptor;

        public BasePropertyDescriptor(PropertyDescriptor basePropertyDescriptor)
            : base(basePropertyDescriptor)
        {
            this.basePropertyDescriptor = basePropertyDescriptor;
        }

        public override bool CanResetValue(object component)
        {
            return basePropertyDescriptor.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return basePropertyDescriptor.ComponentType; }
        }

        public override string DisplayName
        {
            get
            {
                string svalue = "";
                foreach (Attribute attribute in this.basePropertyDescriptor.Attributes)
                {
                    if (attribute is ShowChinese)
                    {
                        svalue = attribute.ToString();
                        break;
                    }
                }
                if (svalue == "") return this.basePropertyDescriptor.Name;
                else return svalue;
            }
        }

        public override string Description
        {
            get
            {
                return this.basePropertyDescriptor.Description;
            }
        }

        public override object GetValue(object component)
        {
            return this.basePropertyDescriptor.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return this.basePropertyDescriptor.IsReadOnly; }
        }

        public override string Name
        {
            get { return this.basePropertyDescriptor.Name; }
        }

        public override Type PropertyType
        {
            get { return this.basePropertyDescriptor.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            this.basePropertyDescriptor.ResetValue(component);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return this.basePropertyDescriptor.ShouldSerializeValue(component);
        }

        public override void SetValue(object component, object value)
        {
            try
            {
                string[] inform = value.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (inform.Length > 1)
                    this.basePropertyDescriptor.SetValue(component, inform[0]);
                else
                    this.basePropertyDescriptor.SetValue(component, value);
            }
            catch
            {

            }

        }
    }
    #endregion

    #region 自定义属性用来显示左边的汉字
    [AttributeUsage(AttributeTargets.Property)]
    public class ShowChinese : System.Attribute
    {
        private string sChineseChar = "";

        public ShowChinese(string sChineseChar)
        {
            this.sChineseChar = sChineseChar;
        }

        public string ChineseChar
        {
            get
            {
                return this.sChineseChar;
            }
        }

        public override string ToString()
        {
            return this.sChineseChar;
        }
    }
    #endregion

}
