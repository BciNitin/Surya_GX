using MobiVUE.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MobiVUE
{
    public static class Enum<T> where T : struct, IConvertible
    {
        public static Dictionary<int, string> ToDictionary()
        {
            return GetItemsWitIntKey().ToDictionary(x => x.Key, y => y.Value);
        }

        public static List<KeyValue<int, string>> GetItemsWitIntKey()
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            List<KeyValue<int, string>> list = new List<KeyValue<int, string>>();

            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                int value;
                string display;
                value = (int)field.GetValue(null);
                display = Enum.GetName(enumType, value);
                foreach (Attribute currAttr in field.GetCustomAttributes(true))
                {
                    DescriptionAttribute valueAttribute = currAttr as DescriptionAttribute;
                    if (valueAttribute != null)
                        display = valueAttribute.Description;
                }
                list.Add(new KeyValue<int, string>(value, display));
            }
            return list;
        }

        public static List<KeyValuePair<T, string>> GetItems()
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            List<KeyValuePair<T, string>> list = new List<KeyValuePair<T, string>>();

            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                T value;
                string display;
                value = (T)field.GetValue(null);
                display = Enum.GetName(enumType, value);
                foreach (Attribute currAttr in field.GetCustomAttributes(true))
                {
                    DescriptionAttribute valueAttribute = currAttr as DescriptionAttribute;
                    if (valueAttribute != null)
                        display = valueAttribute.Description;
                }
                list.Add(new KeyValuePair<T, string>(value, display));
            }
            return list;
        }
    }
}