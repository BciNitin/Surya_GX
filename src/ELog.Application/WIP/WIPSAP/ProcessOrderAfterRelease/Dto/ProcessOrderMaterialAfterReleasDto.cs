using System;


namespace ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease.Dto
{
    public class ProcessOrderMaterialAfterReleasDto
    {

        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }


        public string LineItemNo { get; set; }


        public string MaterialCode { get; set; }

        public string MaterialDescription { get; set; }


        public string ARNo { get; set; }

        public string LotNo { get; set; }


        public string SAPBatchNo { get; set; }


        public string ProductBatchNo { get; set; }


        public string CurrentStage { get; set; }


        public string NextStage { get; set; }

        public float Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime RetestDate { get; set; }


    }
}
