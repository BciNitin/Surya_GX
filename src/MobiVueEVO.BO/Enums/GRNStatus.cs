using System.ComponentModel;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    public enum GRNStatus
    {
        [EnumMember]
        [Description("Pending")]
        Pending = 0,

        [EnumMember]
        [Description("Completed")]
        Completed = 1
    }

    public enum GRNItemStatus
    {
        [EnumMember]
        [Description("Pending")]
        Pending = 0,

        [EnumMember]
        [Description("Completed")]
        Completed = 1
    }
}