using System;
using System.Runtime.Serialization;

namespace MobiVUE.Utility
{
    [DataContract]
    public class KeyValue<TKey, TValue> where TKey : IComparable<TKey>
    {
        public KeyValue()
        {
        }

        public KeyValue(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        [DataMember]
        public TKey Key { get; set; }

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

            if (Key.IsNull()) return false;

            KeyValue<TKey, TValue> objectToCompare = (KeyValue<TKey, TValue>)obj;

            return Key.Equals(objectToCompare.Key);
        }

        public override int GetHashCode()
        {
            if (Value == null)
                return Key.GetHashCode();
            else
                return Key.GetHashCode() ^ Value.GetHashCode();
        }

        public static bool operator ==(KeyValue<TKey, TValue> object1, KeyValue<TKey, TValue> object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.Key.CompareTo(object2.Key) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(KeyValue<TKey, TValue> object1, KeyValue<TKey, TValue> object2)
        {
            if (object1 == null || object2 == null || object1.GetType() != object2.GetType()) return false;

            if (object1.Key.CompareTo(object2.Key) != 0)
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