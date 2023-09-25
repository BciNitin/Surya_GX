using Abp.AutoMapper;
using Elog.Core.Entities;

namespace Elog.Application.ElogControls.Dto
{
    [AutoMapTo(typeof(ElogControl))]
    public class CreateElogControlsDto
    {

        //[Key]
        //public int Id { get; set; }
        public int ELogId { get; set; }
        public int ControlID { get; set; }
        public string ControlLabel { get; set; }
        public string ControlType { get; set; }
        public string ControlDefaults { get; set; }
        public int Sequence { get; set; }
        public bool FlagIsDefaultSql { get; set; }
        public string DBFieldName { get; set; }
        public string DBDataType { get; set; }
        public bool FlagIsMandatory { get; set; }
        public bool IsActive { get; set; }

    }
}
