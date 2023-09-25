using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace MobiVUE.Utility
{
    [DataContract]
    public struct SmartDateTime : IComparable
    {
        public static DateTime MinDate { get { return SqlDateTime.MinValue.Value; } }

        public static DateTime MaxDate { get { return SqlDateTime.MaxValue.Value.Date.AddYears(-1); } }

        public SmartDateTime(DateTime dateTime) : this()
        {
            DateTime = dateTime;
        }

        [DataMember(Name = "Year")]
        public int Year { get; private set; }

        [DataMember(Name = "Month")]
        public int Month { get; private set; }

        [DataMember(Name = "Day")]
        public int Day { get; private set; }

        [DataMember(Name = "Hour")]
        public int Hour { get; private set; }

        [DataMember(Name = "Minute")]
        public int Minute { get; private set; }

        [DataMember(Name = "Second")]
        public int Second { get; private set; }

        public DateTime DateTime
        {
            get
            {
                if (Year == 0 && Month == 0 && Day == 0 && Hour == 0 && Minute == 0 && Second == 0)
                {
                    Year = SqlDateTime.MinValue.Value.Year;
                    Month = SqlDateTime.MinValue.Value.Month;
                    Day = SqlDateTime.MinValue.Value.Day;
                    Hour = SqlDateTime.MinValue.Value.Hour;
                    Minute = SqlDateTime.MinValue.Value.Minute;
                    Second = SqlDateTime.MinValue.Value.Second;
                }
                return new DateTime(Year, Month, Day, Hour, Minute, Second);
            }
            set
            {
                Year = value.Year;
                Month = value.Month;
                Day = value.Day;
                Hour = value.Hour;
                Minute = value.Minute;
                Second = value.Second;
            }
        }

        public override string ToString()
        {
            return (SqlDateTime.MinValue == DateTime) ? "" : DateTime.ToString();
        }

        public string ToString(string format)
        {
            return (SqlDateTime.MinValue == DateTime) ? "" : DateTime.ToString(format);
        }

        #region Equality Checks

        public override int GetHashCode()
        {
            return DateTime.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            SmartDateTime objectToCompare = (SmartDateTime)obj;

            return DateTime.Equals(objectToCompare.DateTime);
        }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) throw new InvalidCastException("Invalid object");

            SmartDateTime objectToCompare = (SmartDateTime)obj;
            return DateTime.CompareTo(objectToCompare);
        }

        public static bool operator ==(SmartDateTime object1, SmartDateTime object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.DateTime.CompareTo(object2.DateTime) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator >(SmartDateTime object1, SmartDateTime object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.DateTime.CompareTo(object2.DateTime) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator <(SmartDateTime object1, SmartDateTime object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.DateTime.CompareTo(object2.DateTime) == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(SmartDateTime object1, SmartDateTime object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.DateTime.CompareTo(object2.DateTime) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static implicit operator SmartDateTime(DateTime value)
        {
            return new SmartDateTime(value);
        }

        public static implicit operator SmartDateTime(string value)
        {
            DateTime dateTime = new DateTime();
            if (DateTime.TryParse(value, out dateTime))
                return new SmartDateTime(dateTime);
            else
                throw new ArgumentException("Invalid datetime string");
        }

        public static implicit operator DateTime(SmartDateTime value)
        {
            return value.DateTime;
        }

        #endregion Equality Checks
    }
}