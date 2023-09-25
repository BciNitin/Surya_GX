using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.UnitOfMeasurements.Dto
{
    [AutoMapFrom(typeof(UnitOfMeasurementMaster))]
    public class UnitOfMeasurementListDto : EntityDto<int>
    {
        public string UOMCode { get; set; }
        public string Name { get; set; }
        public int? UnitOfMeasurementTypeId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string UnitOfMeasurement { get; set; }

        public bool IsActive { get; set; }
        public string UserEnteredUOMType { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ActualUom { get; set; }
    }
}