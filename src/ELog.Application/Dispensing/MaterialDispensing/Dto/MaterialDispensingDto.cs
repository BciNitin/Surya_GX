using Abp.Application.Services.Dto;

using ELog.Application.SelectLists.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.MaterialDispensing.Dto
{
    public class MaterialDispensingDto : EntityDto<int>
    {
        public int RLAFId { get; set; }
        public string RLAFCode { get; set; }
        public int ProcessOrderId { get; set; }
        public string MaterialCode { get; set; }
        public string SAPBatchNo { get; set; }
        public float? RequiredQty { get; set; }
        public float? BalanceQty { get; set; }
        public string BaseUnitOfMeasurement { get; set; }
        public int? BaseUnitOfMeasurementId { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public string DispensingBarcode { get; set; }
        public string CheckedBy { get; set; }
        public int? MaterialContainerId { get; set; }

        public int? IsVerified { get; set; }
        public bool verified { get; set; }

        public string verifiedBy { get; set; }
        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }
        public bool Printed { get; set; }

        public int? MaterialBatchDispensingHeaderId { get; set; }
        public int? MaterialBatchDispensingContainerDetailsId { get; set; }

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
        public int? ReprintDeviceId { get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? StatusId { get; set; }
        public List<SelectListDto> SapbBatchNumberSelectList { get; set; }
        public bool IsPackUOM { get; set; }
        public bool IsController { get; set; }
        public float? MaterialContainerBalanceQuantity { get; set; }
        public int MaterialDispensingDetailIdToReprint { get; set; }
        public bool IsAnySAPBatchNoExistForHeader { get; set; }
        public float? ConvertedNoOfPack { get; set; }
        public float? ConvertedNetWeight { get; set; }
        public bool IssueIndicator { get; set; }
        public bool IsSampling { get; set; }

        public string RequiredQuantityWithUOM
        {
            get
            {
                return $"{RequiredQty} {BaseUnitOfMeasurement}";
            }
        }

        public string BalanceQuantityWithUOM
        {
            get
            {
                return $"{BalanceQty} {BaseUnitOfMeasurement}";
            }
        }

        public int NoOfContainers { get; set; }

    }

    public class GetDespensingDetailsStatus
    {
        public int? DispensingHeaderId { get; set; }
        public string SAPBatchNo { get; set; }
        public string ContainerMaterialBarcode { get; set; }
        public string DispenseBarcode { get; set; }

        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }
        public int? Id { get; set; }
        public bool Printed { get; set; }
        public string BalanceCode { get; set; }
        public int? BalanceId { get; set; }
        public bool IsGrossWeight { get; set; }
        public float? GrossWeight { get; set; }

        public int? NoOfPacks { get; set; }
        public float? TareWeight { get; set; }
        public float? NetWeight { get; set; }
        public int NoOfContainers { get; set; }

    }

    public class MaterialDispensingLabel
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
        public string PlantName { get; set; }
        public string ARNo { get; set; }
        public float? OrderQuantity { get; set; }

        public bool Printed { get; set; }
    }

    public class DispensingNetWeightModel
    {
        public int? NoOfPacks { get; set; }
        public float? NetWeight { get; set; }
        public int? UnitOfMeasurementId { get; set; }
        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }
        public bool Printed { get; set; }
    }

    public class DispensingUnitOfMeasurementDto
    {
        public int Id { get; set; }
        public string UnitOfMeasurement { get; set; }
    }
}