using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Runtime.Validation;

using AutoMapper;

using ELog.Core;
using ELog.Core.Authorization.Users;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
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

        [StringLength(PMMSConsts.Small)]
        public string PhoneNumber { get; set; }


        [EmailAddress]
        //[StringLength(PMMSConsts.Medium)]
        public string Email { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool? IsDeleted { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        public List<int?> Plants { get; set; }

        public long? ReportingManagerId { get; set; }

        [Required]
        public int? ModeId { get; set; }

        public int? DesignationId { get; set; }

        public string EmployeeCode { get; set; }

        public string[] RoleNames { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        [DisableAuditing]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [IgnoreMap]
        [Required]
        [StringLength(PMMSConsts.Small)]
        [DisableAuditing]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}