using System.ComponentModel;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    [DataContract]
    public enum LocationAttributeGroup
    {
        [EnumMember]
        [Description("Attribute 1")]
        Attribute1 = 1,

        [EnumMember]
        [Description("Attribute 2")]
        Attribute2 = 2,
    }
}