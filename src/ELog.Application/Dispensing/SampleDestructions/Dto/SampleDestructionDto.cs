using Abp.Application.Services.Dto;

using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;

namespace ELog.Application.Dispensing.SampleDestructions.Dto
{
    public class SampleDestructionDto : EntityDto<int>
    {
        public int? InspectionLotId { get; set; }
        public string MaterialCode { get; set; }
        public string SAPBatchNo { get; set; }
        public int? BaseUnitOfMeasurementId { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public int? MaterialContainerId { get; set; }
        public int? MaterialSampleHeaderHeaderId { get; set; }

        public int? UnitOfMeasurementId { get; set; }
        public List<SelectListDto> SuggestedBalanceIds { get; set; }
        public string CommsSepratedSuggestedBalanceIds { get; set; }
        public string BalanceCode { get; set; }
        public int? BalanceId { get; set; }
        public bool IsGrossWeight { get; set; }
        public float? GrossWeight { get; set; }

        public int? NoOfPacks { get; set; }
        public float? TareWeight { get; set; }
        public float? NetWeight { get; set; }
        public bool IsPackUOM { get; set; }
        public int? NoofPacks { get; set; }
        public float? MaterialContainerBalanceQuantity { get; set; }
        public float? ConvertedNoOfPack { get; set; }
        public float? ConvertedNetWeight { get; set; }
    }
}