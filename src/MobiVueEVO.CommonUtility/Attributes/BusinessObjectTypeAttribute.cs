using System;

namespace MobiVUE.Common.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BusinessObjectTypeAttribute : Attribute
    {
        private string _type;

        public BusinessObjectTypeAttribute(string type)
        {
            _type = type;
        }

        public Type BoType
        {
            get
            {
                return Type.GetType(_type);
            }
        }
    }
}