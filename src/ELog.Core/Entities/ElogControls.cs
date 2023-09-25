using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elog.Core.Entities
{
    [Table("Elog_ElogControls")]
    public class ElogControl : Entity<int>
    {
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
