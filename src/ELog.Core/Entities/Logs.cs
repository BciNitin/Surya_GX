using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("logsData")]
    public class Logs : Entity<int>
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Data { get; set; }
        public string CreatedBy { get; set; }
        public string Submodule { get; set; }
        public string ModuleName { get; set; }


    }
}
