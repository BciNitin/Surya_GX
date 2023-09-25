using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("LogFormHistory")]
    public class LogFormHistory : PMMSFullAudit
    {

        [ForeignKey("ClientId")]
        public int FormId { get; set; }


        public string Remarks { get; set; }


        public int Status { get; set; }


    }
}
