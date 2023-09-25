using System.ComponentModel;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    public enum RMPickOrderStatus
    {
        [EnumMember]
        [Description("Pending")]
        Open = 0,

        [EnumMember]
        [Description("Picklist Generated")]
        PicklistGenerated = 1,

        [EnumMember]
        [Description("Picking Completed")]
        PickingCompleted = 2
    }

    public enum RMItemStatus
    {
        [EnumMember]
        [Description("Pending")]
        Pending = 0,

        [EnumMember]
        [Description("Completed")]
        Completed = 1
    }
}