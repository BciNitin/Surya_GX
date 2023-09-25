using Abp.Application.Services.Dto;

using ELog.Application.SelectLists.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.MaterialSampleDispensing.Dto
{
    public class MaterialSampleDispensingDto : EntityDto<int>
    {
        public int RLAFId { get; set; }
        public string RLAFCode { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? InspectionLotId { get; set; }
        public int? SamplingTypeId { get; set; }

        public string MaterialCode { get; set; }
        public string SAPBatchNo { get; set; }
        public float? RequiredQty { get; set; }
        public float? BalanceQty { get; set; }
        public string BaseUnitOfMeasurement { get; set; }
        public int? BaseUnitOfMeasurementId { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public string DispensingBarcode { get; set; }
        public bool IsAnySAPBatchNoExistForHeader { get; set; }
        public int? MaterialContainerId { get; set; }
        public int? MaterialBatchDispensingHeaderId { get; set; }

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
        public int? DeviceId { get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? StatusId { get; set; }
        public List<SelectListDto> SapbBatchNumberSelectList { get; set; }
        public bool IsPackUOM { get; set; }
        public int? NoofPacks { get; set; }
        public bool IsController { get; set; }
        public float? MaterialContainerBalanceQuantity { get; set; }
        public int MaterialSamplingDetailIdToReprint { get; set; }
        public int? ReprintDeviceId { get; set; }
        public float? ConvertedNoOfPack { get; set; }
        public float? ConvertedNetWeight { get; set; }
        public bool IsSampling { get; set; }
        public float NoOfQuantity { get; set; }
        public float SelectedContainer { get; set; }


    }

    public class MaterialSampleDispensingLabel
    {
        public DateTime? ExpiryDate { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public string BatchSize { get; set; }
        public string FormatNo { get; set; }
        public string SOPNo { get; set; }
        public string GrossWeight { get; set; }
        public string NetWeight { get; set; }
        public string TareWeight { get; set; }
        public string PlantId { get; set; }

        public string ArNo { get; set; }
        public string GrnNo { get; set; }
        public string SampledBy { get; set; }
        public string SampledQty { get; set; }
        public string ReceviedQty { get; set; }
        public string MfgName { get; set; }
        public string PlantName { get; set; }
        public DateTime? MfgDate { get; set; }
    }
}