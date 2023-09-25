using System.Runtime.Serialization;

namespace MobiVueEVO.BO.Common
{
    [DataContract]
    public class Shift
    {
        [DataMember]
        public string ShiftName { get; set; }

        [DataMember]
        public string ShiftStartTime { get; set; }

        [DataMember]
        public string ShiftEndTime { get; set; }
    }
}