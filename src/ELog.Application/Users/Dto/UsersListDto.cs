using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Authorization.Users;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UsersListDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public DateTime CreationTime { get; set; }
        public int ApprovalStatusId { get; set; }
        public bool IsActive { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? PswdResetDate { get; set; }

    }

    public class UserListInternalDto
    {
        public UsersListDto UserListDto { get; set; }
        public ICollection<UserPlants> UserPlants { get; set; }
        public int? DesignationId { get; set; }
        public int? ModeId { get; set; }
    }
}