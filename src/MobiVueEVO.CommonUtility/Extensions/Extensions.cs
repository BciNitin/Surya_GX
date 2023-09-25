using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MobiVUE
{
    public delegate TReturn EventHandler<TEventArgs, TReturn>(object sender, TEventArgs argument);

    public static class Extensions
    {
        #region Conversions
        public static Int32 ToInt32<TException>(this string toConvertInInteger, string errMessage)
           where TException : Exception, new()
        {
            int value;
            if (Int32.TryParse(toConvertInInteger, out value))
            {
                return value;
            }
            throw (TException)Activator.CreateInstance(typeof(TException), errMessage);
        }

        public static Int64 ToInt64<TException>(this string toConvertInInteger, string errMessage)
          where TException : Exception, new()
        {
            Int64 value;
            if (Int64.TryParse(toConvertInInteger, out value))
            {
                return value;
            }
            throw (TException)Activator.CreateInstance(typeof(TException), errMessage);
        }
        public static Int32 ToInt32(this string toConvertInInteger, string errMessage)
        {
            int value;
            if (int.TryParse(toConvertInInteger, out value))
            {
                return value;
            }
            throw new InvalidCastException(errMessage);
        }

        public static Int32 ToInt32(this object toConvertInInteger, string errMessage)
        {
            CodeContract.Required<InvalidCastException>(toConvertInInteger.IsNotNull(), errMessage);
            return toConvertInInteger.ToString().ToInt32(errMessage);
        }

        public static Int64 ToInt64(this string toConvertInBigInteger, string errMessage)
        {
            Int64 value;
            if (Int64.TryParse(toConvertInBigInteger, out value))
            {
                return value;
            }
            throw new InvalidCastException(errMessage);
        }

        public static Int64 ToInt64(this object toConvertInBigInteger, string errMessage)
        {
            CodeContract.Required<InvalidCastException>(toConvertInBigInteger.IsNotNull(), errMessage);
            return toConvertInBigInteger.ToString().ToInt64(errMessage);
        }

        public static decimal ToDecimal(this string toConvertInDecimal, string errMessage)
        {
            decimal value;
            if (decimal.TryParse(toConvertInDecimal, out value))
            {
                return value;
            }
            throw new InvalidCastException(errMessage);
        }

        public static decimal ToDecimal(this object toConvertInDecimal, string errMessage)
        {
            CodeContract.Required<InvalidCastException>(toConvertInDecimal.IsNotNull(), errMessage);
            return toConvertInDecimal.ToString().ToDecimal(errMessage);
        }

        public static Int32 ToInt32(this string toConvertInInteger, int defaultValueForNullOrWhiteSpace, string errMessage)
        {
            if (toConvertInInteger.IsNullOrWhiteSpace()) return defaultValueForNullOrWhiteSpace;
            return toConvertInInteger.ToInt32(errMessage);
        }

        public static Int32 ToInt32(this object toConvertInInteger, int defaultValueForNull, string errMessage)
        {
            if (toConvertInInteger.IsNull()) return defaultValueForNull;
            return toConvertInInteger.ToString().ToInt32(defaultValueForNull, errMessage);
        }

        public static Int64 ToInt64(this string toConvertInBigInteger, Int64 defaultValueForNullOrWhiteSpace, string errMessage)
        {
            if (toConvertInBigInteger.IsNullOrWhiteSpace()) return defaultValueForNullOrWhiteSpace;
            return toConvertInBigInteger.ToInt64(errMessage);
        }

        public static long ToInt64(this object toConvertInBigInteger, long defaultValueForNull, string errMessage)
        {
            if (toConvertInBigInteger.IsNull()) return defaultValueForNull;
            return toConvertInBigInteger.ToString().ToInt64(defaultValueForNull, errMessage);
        }

        public static decimal ToDecimal(this string toConvertInDecimal, decimal defaultValueForNullOrWhiteSpace, string errMessage)
        {
            if (toConvertInDecimal.IsNullOrWhiteSpace()) return defaultValueForNullOrWhiteSpace;
            return toConvertInDecimal.ToInt64(errMessage);
        }

        public static decimal ToDecimal(this object toConvertInDecimal, decimal defaultValueForNull, string errMessage)
        {
            if (toConvertInDecimal.IsNull()) return defaultValueForNull;
            return toConvertInDecimal.ToString().ToDecimal(defaultValueForNull, errMessage);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private static List<T> ToList<T>(this DataTable dataTable)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static List<T> Split<T>(this string value, string separator) where T : struct
        {
            var splitedData = value.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return splitedData.Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static string ToTitleCase(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return value;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(value.ToLower());
        }

        public static int ToYearMonthInt(this DateTime value)
        {
            return Convert.ToInt32(value.ToString("yyyyMM"));
        }

        public static string IfIsNotNullOrWhiteSpace(this string value, string returnValue)
        {
            return value.IsNullOrWhiteSpace() ? returnValue : value;
        }

        #endregion Conversions

        #region Calculate Percents

        public static int GetPercentValue(this int value, decimal percentage)
        {
            return (int)(value * percentage / 100);
        }

        public static long GetPercentValue(this long value, decimal percentage)
        {
            return (long)(value * percentage / 100);
        }

        public static decimal GetPercentValue(this decimal value, decimal percentage)
        {
            return value * percentage / 100;
        }

        public static int PercentOf(this int value, decimal total)
        {
            return (int)((value * 100) / total);
        }

        public static long PercentOf(this long value, decimal total)
        {
            return (long)((value * 100) / total);
        }

        public static decimal PercentOf(this decimal value, decimal total)
        {
            return (value * 100) / total;
        }

        public static float PercentOf(this float value, float total)
        {
            return (value * 100) / total;
        }

        #endregion Calculate Percents

        #region Checks

        public static bool HaveItems<TSource>(this ICollection<TSource> source)
        {
            return source != null && source.Count > 0;
        }

        public static bool DontHaveItems<TSource>(this ICollection<TSource> source)
        {
            return source == null || source.Count == 0;
        }

        public static void AddUnique<TSource>(this ICollection<TSource> source, TSource item)
        {
            CodeContract.Required<MobiVUEException>(source.IsNotNull(), "Collection is not initialized");
            if (!source.Contains(item))
            {
                source.Add(item);
            }
        }

        public static bool InvariantEquals(this string source, string to)
        {
            if (source.IsNull() || to.IsNull()) return false;
            return source.ToUpperInvariant() == to.ToUpperInvariant();
        }

        public static bool IsNull(this object value)
        {
            return value == null;
        }

        public static bool IsNotNull(this object value)
        {
            return value != null;
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNumeric(this string value)
        {
            return Microsoft.VisualBasic.Information.IsNumeric(value);
        }

        #endregion Checks

        #region Enun's Utilities

        public static Dictionary<string, string> EnumToDictionary(this Enum @enum)
        {
            var type = @enum.GetType();
            return Enum.GetValues(type).Cast<string>().ToDictionary(e => e, e => Enum.GetName(type, e));
        }

        public static string DisplayName(this Enum @enum)
        {
            Type mytype = @enum.GetType();
            var field = mytype.GetField(@enum.ToString());
            var valueAttribute = field.GetCustomAttribute(typeof(DescriptionAttribute));
            if (valueAttribute != null)
                return ((DescriptionAttribute)valueAttribute).Description;
            else
                return @enum.ToString();
        }

        public static string GetAttributeValue<TAttribute>(this Enum @enum, Expression<Func<TAttribute, string>> controlPropertyAccessor) where TAttribute : Attribute
        {
            Type mytype = @enum.GetType();
            var field = mytype.GetField(@enum.ToString());
            var valueAttribute = field.GetCustomAttribute(typeof(TAttribute));

            if (valueAttribute != null)
            {
                TAttribute attrObj = (TAttribute)valueAttribute;

                var a = controlPropertyAccessor.Compile();
                return a((TAttribute)valueAttribute);
            }

            throw new MobiVUEException("Attribute not found");
        }

        #endregion Enun's Utilities

        #region Object Pivot

        public static DataTable ToPivotTable<T, TColumn, TRow, TData>(this IEnumerable<T> source,
            Func<T, TColumn> columnSelector, Expression<Func<T, TRow>> rowSelector, Func<IEnumerable<T>, TData> dataSelector)
        {
            DataTable table = new DataTable();
            var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            table.Columns.Add(new DataColumn(rowName));
            var columns = source.Select(columnSelector).Distinct();

            foreach (var column in columns)
                table.Columns.Add(new DataColumn(column.ToString()));

            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             });

            foreach (var row in rows)
            {
                var dataRow = table.NewRow();
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                dataRow.ItemArray = items.ToArray();
                table.Rows.Add(dataRow);
            }
            return table;
        }

        public static dynamic[] ToPivotArray<T, TColumn, TRow, TData>(this IEnumerable<T> source,
        Func<T, TColumn> columnSelector, Expression<Func<T, TRow>> rowSelector, Func<IEnumerable<T>, TData> dataSelector)
        {
            var arr = new List<object>();
            var cols = new List<string>();
            string rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            var columns = source.Select(columnSelector).Distinct();

            cols = (new[] { rowName }).Concat(columns.Select(x => x.ToString())).ToList();

            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             }).ToArray();

            foreach (var row in rows)
            {
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                var obj = GetAnonymousObject(cols, items);
                arr.Add(obj);
            }
            return arr.ToArray();
        }

        private static dynamic GetAnonymousObject(IEnumerable<string> columns, IEnumerable<object> values)
        {
            IDictionary<string, object> eo = new ExpandoObject() as IDictionary<string, object>;
            int i;
            for (i = 0; i < columns.Count(); i++)
            {
                eo.Add(columns.ElementAt(i), values.ElementAt(i));
            }
            return eo;
        }

        #endregion Object Pivot

        #region Miscellaneous

        public static string Ordinal(this int number)
        {
            string suffix = String.Empty;

            int ones = number % 10;
            int tens = (int)Math.Floor(number / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return String.Format("{0}{1}", number, suffix);
        }

        public static string Join<TSource>(this IEnumerable<TSource> source, string separator, Expression<Func<TSource, object>> dataSourceMemberAccess)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TSource item in source)
            {
                var val = dataSourceMemberAccess.Compile()(item);
                if (val != null)
                {
                    if (sb.Length > 0)
                        sb.AppendFormat("{0}{1}", separator, val);
                    else
                        sb.Append(val);
                }
            }
            return sb.ToString();
        }

        public static string ToNumberString(this bool[] array)
        {
            return array.Join("", x => Convert.ToInt32(x).ToString());
        }

        public static bool[] ToBoolArray(this string zeroOnes)
        {
            return zeroOnes.Select(x => Convert.ToInt32(x.ToString()) == 1).ToArray();
        }

        public static string JoinWithSingleQuoteWrap<TSource>(this IEnumerable<TSource> source, string separator, Expression<Func<TSource, object>> dataSourceMemberAccess)
        {
            StringBuilder sb = new StringBuilder("");

            foreach (TSource item in source)
            {
                var val = dataSourceMemberAccess.Compile()(item);
                if (val != null)
                {
                    if (sb.Length > 0)
                        sb.AppendFormat("{0}'{1}'", separator, val);
                    else
                        sb.AppendFormat("'{0}'", val);
                }
            }
            return sb.ToString();
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataColumn Add<T>(this DataColumnCollection columns, string name)
        {
            return columns.Add(name, typeof(T));
        }

        public static IEnumerable<TResult> SortBy<TResult, TKey>(this IEnumerable<TResult> itemsToSort, IEnumerable<TKey> sortKeys, Func<TResult, TKey> matchFunc)
        {
            return sortKeys.Join(itemsToSort,
                key => key,
                matchFunc,
                (key, iitem) => iitem);
        }

        #endregion Miscellaneous
    }
}