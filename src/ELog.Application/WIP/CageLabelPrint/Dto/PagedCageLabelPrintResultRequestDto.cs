using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.CageLabelPrint.Dto
{
    public class PagedCageLabelPrintResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? ProductID { get; set; }
        public int? ProcessorderID { get; set; }
        public int? CubicleID { get; set; }

        public string Keyword { get; set; }
    }
}
