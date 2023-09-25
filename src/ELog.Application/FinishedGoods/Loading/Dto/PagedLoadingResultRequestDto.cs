using Abp.Application.Services.Dto;

namespace ELog.Application.FinishedGoods.Picking.Dto
{

    public class PagedLoadingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string OBD { get; set; }
        public int? ProductId { get; set; }
        public string ProductBatchNo { get; set; }


        public string Batch { get; set; }


        public bool? isActive { get; set; }
    }
}
