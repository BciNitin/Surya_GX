using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Atesttable.Dto
{
    [AutoMapFrom(typeof(AtesttableMaster))]
    public class AtesttableMasterDto : EntityDto<int>
    {
        public string testfield1 { get; set; }
        public string testfield2 { get; set; }
        public string testfield3 { get; set; }
        public string testfield4 { get; set; }
    }
}
