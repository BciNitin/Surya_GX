using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Authorization.Users;

using System.Collections.Generic;

namespace ELog.Application.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public IList<string> RoleNames { get; set; }
        public IList<string> Permissions { get; set; }
        public IList<string> TransactionActiveSubModules { get; set; }

        public string PlantCode { get; set; }
        public int ModeId { get; set; }
        public bool IsControllerMode { get; set; }

        public bool IsGateEntrySubModuleActive { get; set; }
        public bool IsMaterialInspectionModuleSelected { get; set; }
        public int ResetPasswordDaysLeft { get; set; }
        public int? ApprovalLevelId { get; set; }
    }
}