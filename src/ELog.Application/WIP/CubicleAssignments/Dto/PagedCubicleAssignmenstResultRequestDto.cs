using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.CubicleAssignments.Dto
{
    public class PagedCubicleAssignmenstResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }

        public int? ProductId { get; set; }
        public int? ProcessOrderId { get; set; }

        public int? CubicleBarcodeId { get; set; }

        public int? EquipmentBarcodeId { get; set; }
    }
}
