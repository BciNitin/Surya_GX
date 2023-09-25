using Abp.Application.Services.Dto;

namespace ELog.Application.SelectLists.Dto
{
    public class SelectListDto : EntityDto<object>
    {
        public string Value { get; set; }
    }
}