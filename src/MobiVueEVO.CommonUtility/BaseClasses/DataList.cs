using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MobiVUE.Utility
{
    [DataContract]
    public class DataList<TItemType, RHeaderDataType>
    {
        public DataList()
        {
            ItemCollection = new List<TItemType>();
        }

        [DataMember]
        public List<TItemType> ItemCollection { get; set; }

        [DataMember]
        public RHeaderDataType HeaderData { get; set; }
    }
}