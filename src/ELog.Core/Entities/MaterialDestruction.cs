using ELog.Core.Authorization;

namespace ELog.Core.Entities
{
    public class MaterialDestruction : PMMSFullAudit
    {
        public string MovementType { get; set; }
        public int ContainerId { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public string MaterialCode { get; set; }
        public string SAPBatchNo { get; set; }
        public string ARNo { get; set; }
        public float? Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }
        public bool IsPostedToSAP { get; set; }
    }
}