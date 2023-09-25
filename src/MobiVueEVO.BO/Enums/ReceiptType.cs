using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    [DataContract]
    public enum PrintReceiptType
    {
        [EnumMember]
        Article = 0,

        [EnumMember]
        Pallet = 1,

        [EnumMember]
        Bin = 2,

        [EnumMember]
        GateEntry = 3,

        [EnumMember]
        Location = 4,

        [EnumMember]
        WMSGRNPrint = 5
    }
}