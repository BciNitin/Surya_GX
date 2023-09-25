using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.Consumption.Dto
{
    public class PagedConsumptionResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Keyword { get; set; }
        public int? ProcessOrderId { get; set; }
        public string ProductId { get; set; }
        public int? processBarcodeId { get; set; }
        //  public string? CubicalCode { get; set; }
        public int? equipmentBracodeId { get; set; }
        // public string? EquipmentNo { get; set; }
        //  public string? ProductCode { get; set; }
    }
}
