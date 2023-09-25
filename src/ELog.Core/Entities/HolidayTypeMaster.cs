using Abp.AutoMapper;

using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [AutoMapFrom(typeof(HolidayTypeMaster))]
    public class HolidayTypeMaster : PMMSFullAudit
    {
        public string HolidayType { get; set; }

        [ForeignKey("HolidayTypeId")]
        public ICollection<CalenderMaster> CalenderMasters { get; set; }
    }
}