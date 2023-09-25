using MobiVUE.Utility;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    [DataContract]
    public class UnitOfMeasurement : BusinessBase
    {
        private short _uomId;

        [DataMember]
        public short UOMId
        {
            get { return _uomId; }
            set { SetValue<short>(ref _uomId, value, () => this.UOMId); }
        }

        private string _code;

        [Required(AllowEmptyStrings = false, ErrorMessage = "UOM Code is mandatory.")]
        [StringLength(25, ErrorMessage = "UOM Code length should not exceed 25 characters.")]
        [DataMember]
        public string Code
        {
            get { return _code; }
            set { SetValue<string>(ref _code, value, () => this.Code); }
        }

        private string _name;

        [Required(AllowEmptyStrings = false, ErrorMessage = "UOM Name is mandatory.")]
        [StringLength(50, ErrorMessage = "UOM Name length should not exceed 50 charaters.")]
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { SetValue<string>(ref _name, value, () => this.Name); }
        }
    }
}