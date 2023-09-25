using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Locations.Dto
{
    [AutoMapFrom(typeof(LocationMaster))]
    public class LocationListDto : EntityDto<int>
    {
        public string LocationCode { get; set; }

        public string StorageLocationType { get; set; }
        public int Area { get; set; }
        public int PlantId { get; set; }

        public int DepartmentId { get; set; }

        public string UserEnteredPlantId { get; set; }
        public string UserEnteredAreaId { get; set; }

        public string UserEnteredDepartmentId { get; set; }

        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}