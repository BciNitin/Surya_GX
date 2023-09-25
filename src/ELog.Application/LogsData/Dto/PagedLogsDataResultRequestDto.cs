using Abp.Application.Services.Dto;

namespace ELog.Application.LogsData.Dto
{
    public class PagedLogsDataResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int Id { get; set; }
        // public string Data { get; set; }

    }
}
