using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class MaterialEntityDto : EntityDto<int>
    {
        public int MaterialId { get; set; }
    }
}