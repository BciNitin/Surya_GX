using Abp.Application.Services.Dto;

namespace ELog.Application.LogFormsHistoryApi.Dto
{
    public class PagedLogFormHistoryResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int Id { get; set; }

        public int FormId { get; set; }


        public string Remarks { get; set; }


        public int Status { get; set; }

    }
}
