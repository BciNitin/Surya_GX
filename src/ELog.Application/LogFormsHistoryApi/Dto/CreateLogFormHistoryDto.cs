using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.LogFormsHistoryApi.Dto
{
    [AutoMapTo(typeof(LogFormHistory))]
    public class CreateLogFormHistoryDto
    {
        public int FormId { get; set; }


        public string Remarks { get; set; }


        public int Status { get; set; }

    }
}
