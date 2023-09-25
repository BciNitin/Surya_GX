using System;
using System.Runtime.Serialization;

namespace MobiVUE.Utility
{
    [Serializable, DataContract]
    public class KeyInfo<TKey, TKeyCode, TValue> where TKey : IComparable<TKey>
    {
        public KeyInfo()
        {
        }

        public KeyInfo(TKey key, TKeyCode keyCode, TValue value)
        {
            this.Key = key;
            this.KeyCode = keyCode;
            this.Value = value;
        }

        [DataMember]
        public TKey Key { get; set; }

        [DataMember]
        public TKeyCode KeyCode { get; set; }

        [DataMember]
        public TValue Value { get; set; }

        public string DisplayText
        {
            get
            {
                if (KeyCode != null && Value != null) return KeyCode + " - " + Value;
                if (KeyCode != null) return KeyCode.ToString();
                if (Value != null) return Value.ToString();
                return "";
            }
        }

        public override string ToString()
        {
            if (KeyCode != null && Value != null) return KeyCode + " - " + Value;
            if (KeyCode != null) return KeyCode.ToString();
            if (Value != null) return Value.ToString();
            return "";
        }
    }
}