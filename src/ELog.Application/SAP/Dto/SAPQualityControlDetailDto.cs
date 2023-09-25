using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.SAP.Dto
{
    [AutoMap(typeof(SAPQualityControlDetail))]
    public class SAPQualityControlDetailDto
    {
        public string ItemCode { get; set; }
        public string InspectionlotNo { get; set; }
        public string SAPBatchNo { get; set; }
        public string BatchStockStatus { get; set; }
        public string RetestDate { get; set; }
        public string ReleasedOn { get; set; }
        public Decimal? ReleasedQty { get; set; }
        public string MovementType { get; set; }
    }
}