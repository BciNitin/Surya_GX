using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Cubicles.Dto
{
    [AutoMapFrom(typeof(CubicleMaster))]
    public class CubicleListDto : EntityDto<int>
    {
        public string UserEnteredPlantId { get; set; }
        public string UserEnteredAreaId { get; set; }

        public int PlantId { get; set; }
        public string CubicleCode { get; set; }
        public int? Area { get; set; }
        public int? SLOCId { get; set; }
        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}