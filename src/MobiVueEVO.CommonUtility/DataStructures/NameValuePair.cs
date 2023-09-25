using System;
using System.Runtime.Serialization;

namespace MobiVUE.Utility
{
    [DataContract]
    public class NameValuePair<TValue> where TValue : IComparable<TValue>
    {
        public NameValuePair()
        {
        }

        public NameValuePair(string name, TValue value)
        {
            this.Name = name;
            this.Value = value;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public TValue Value { get; set; }

        public override string ToString()
        {
            if (this.Value == null) return "";
            return this.Value.ToString();
        }

        #region Equality Checks

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            NameValuePair<TValue> objectToCompare = (NameValuePair<TValue>)obj;

            return Name.Equals(objectToCompare.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Value.GetHashCode();
        }

        public static bool operator ==(NameValuePair<TValue> object1, NameValuePair<TValue> object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.Name.CompareTo(object2.Name) == 0 && object2.Value.CompareTo(object2.Value) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(NameValuePair<TValue> object1, NameValuePair<TValue> object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.Name.CompareTo(object2.Name) != 0 || object2.Value.CompareTo(object2.Value) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Equality Checks
    }
}