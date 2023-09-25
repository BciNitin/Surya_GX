using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Gates.Dto
{
    [AutoMapFrom(typeof(GateMaster))]
    public class GateListDto : EntityDto<int>
    {
        public int PlantId { get; set; }
        public string GateCode { get; set; }

        public string PlantName { get; set; }
        public string UserEnteredPlantId { get; set; }
        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}