using Abp.Application.Services.Dto;

namespace Elog.Application.ElogControls.Dto
{
    public class PagedElogControlsResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? Id { get; set; }
        public int? ELogId { get; set; }
        public string? ControlLabel { get; set; }

    }
}




