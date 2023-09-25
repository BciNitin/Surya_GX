using System.ComponentModel;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    [DataContract]
    public enum InventoryStatus
    {
        [EnumMember]
        [Description("None")]
        None = 0,

        [EnumMember]
        [Description("Printed")]
        Printed = 1,

        [EnumMember]
        [Description("Quality Reject")]
        QualityReject = 2,

        [EnumMember]
        [Description("Quality Ok")]
        QualityOK = 3,

        [EnumMember]
        [Description("Received")]
        Putaway = 4,

        [EnumMember]
        [Description("Picked")]
        Picked = 5,

        [EnumMember]
        [Description("Dispatched")]
        Dispatched = 6,


        [EnumMember]
        [Description("Hold")]
        Hold = 7
    }
}