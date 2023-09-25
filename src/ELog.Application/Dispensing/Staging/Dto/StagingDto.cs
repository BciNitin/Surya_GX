using Abp.Application.Services.Dto;

namespace ELog.Application.Dispensing.Stage.Dto
{
    public class StagingDto : EntityDto<int>
    {
        public string CubicleCode { get; set; }
        public int? CubicleId { get; set; }
        public string GroupId { get; set; }
        public int ProcessOrderMaterialId { get; set; }
        public int PurchaseOrderMaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public int MaterialContainerId { get; set; }
        public string SAPBatchNo { get; set; }
        public int? CubicleAssignmentHeaderId { get; set; }
        public int ContainerCount { get; set; }
        public float? Quantity { get; set; }
        public float? RequiredQty { get; set; }
        public bool IsCompleteStagingAllowed { get; set; }

        public int? MaterialBatchDispensingHeaderId { get; set; }
    }
}