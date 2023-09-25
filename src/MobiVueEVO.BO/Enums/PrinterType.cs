using System.ComponentModel;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    [DataContract]
    public enum PrinterType
    {
        [EnumMember]
        [Description("LAN")]
        LAN = 0,

        [EnumMember]
        [Description("USB")]
        USB = 1
    }

    [DataContract]
    public enum PrintingModule
    {
        [EnumMember]
        [Description("Location")]
        Location = 0,

        [EnumMember]
        [Description("Pallet")]
        Pallet = 1,

        [EnumMember]
        [Description("Machine")]
        Machine = 2,

        [EnumMember]
        [Description("Spool")]
        Spool = 3
    }
}