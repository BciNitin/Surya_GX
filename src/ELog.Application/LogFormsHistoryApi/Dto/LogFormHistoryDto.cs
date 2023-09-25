using Abp.AutoMapper;
using Abp.Domain.Entities;
using ELog.Core.Entities;

namespace ELog.Application.LogFormsHistoryApi.Dto
{
    [AutoMapFrom(typeof(LogFormHistory))]
    public class LogFormHistoryDto : Entity<int>
    {
        public int FormId { get; set; }


        public string Remarks { get; set; }


        public int Status { get; set; }

    }
}
