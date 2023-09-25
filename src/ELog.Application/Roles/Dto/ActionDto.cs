using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Roles.Dto
{
    [AutoMapFrom(typeof(PermissionMaster))]
    public class ActionDto : EntityDto<int>
    {
        [Required]
        public new int? Id { get; set; }

        public string Action { get; set; }

        public bool IsGranted { get; set; }

        public string PermissionName { get; set; }
    }
}