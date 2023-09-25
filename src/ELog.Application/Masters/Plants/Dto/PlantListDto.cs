using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Plants.Dto
{
    [AutoMapFrom(typeof(PlantMaster))]
    public class PlantListDto : EntityDto<int>
    {
        public string PlantName { get; set; }
        public string PlantId { get; set; }
        public int? PlantTypeId { get; set; }
        public string? License { get; set; }
        public int? CountryId { get; set; }

        public bool IsActive { get; set; }
        public string CountryName { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public int? MasterPlantId { get; set; }

    }
}