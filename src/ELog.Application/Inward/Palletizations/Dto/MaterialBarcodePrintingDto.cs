using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.Palletizations.Dto
{
    public class MaterialBarcodePrintingDto : EntityDto<int?>
    {
        public string MaterialBarcode { get; set; }
        public int ContainerNumber { get; set; }
        public string SAPBatchNumber { get; set; }
        public int? GRNDetailId { get; set; }
        public int? ContainerId { get; set; }
        public int? PlantId { get; set; }
        public string PalletBarcode { get; set; }
        public string MaterialDescription { get; set; }

    }
}