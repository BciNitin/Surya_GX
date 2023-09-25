using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;

using AutoMapper;

using ELog.Core;
using ELog.Core.Authorization.Users;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(PMMSConsts.Large)]
        public string UserName { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(PMMSConsts.Medium)]
        public string Email { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool? IsDeleted { get; set; }

        public List<int?> Plants { get; set; }
        public int? DesignationId { get; set; }

        public long? ReportingManagerId { get; set; }

        [Required]
        public int? ModeId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string EmployeeCode { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        public string[] RoleNames { get; set; }

        [IgnoreMap]
        [StringLength(PMMSConsts.Small)]
        [DisableAuditing]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [IgnoreMap]
        [StringLength(PMMSConsts.Small)]
        [DisableAuditing]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
        public bool ActiveInactiveStatusOfUser { get; set; }
        public int PasswordStatus { get; set; }
        public DateTime? PasswordResetTime { get; set; }
    }
}