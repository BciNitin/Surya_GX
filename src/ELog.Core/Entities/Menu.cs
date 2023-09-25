using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELog.Core.Entities
{
    [Table("Menu")]
    public class Menu : Entity<int>
    {
        public string Name { get; set; }
    }
}
