using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    [DataContract]
    public class InventoryWIPInfo
    {
        [DataMember]
        public long InwardOrderId { get; set; }

        [DataMember]
        public long InwardOrderItemId { get; set; }

        [DataMember]
        public string Barcode { get; set; }

        [DataMember]
        public long PalletId { get; set; }

        [DataMember]
        public string VendorBatch { get; set; }

        [DataMember]
        public decimal Qty { get; set; }

        public long PlantId { get; set; }
    }

    [DataContract]
    public class QualityStatusInfo
    {
        [DataMember]
        public long InwardOrderId { get; set; }

        [DataMember]
        public long InwardOrderItemId { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public long PlantId { get; set; }

        [DataMember]
        public int CompletedOrderQC { get; set; }

        [DataMember]
        public string PalletCode { get; set; }


        [DataMember]
        public decimal PartialQty { get; set; }

        [DataMember]
        public long inventoryId { get; set; }


    }

    [DataContract]
    public class PutawayInfo
    {
        [DataMember]
        public string PalletCode { get; set; }

        [DataMember]
        public string LocationCode { get; set; }

        [DataMember]
        public long PlantId { get; set; }
    }


    [DataContract]
    public class PutawayPackingInfo
    {
        [DataMember]
        public string PalletCode { get; set; }

        [DataMember]
        public string MaterialCode { get; set; }

        [DataMember]
        public long PlantId { get; set; }
    }
}