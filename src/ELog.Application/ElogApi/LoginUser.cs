using Abp.Application.Services;
using ELog.Application.Sessions;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using ELog.Application.Masters.Areas;
using System.Collections.Generic;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Abp.Zero.Configuration;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.Core.Identity;
using ELog.Core.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System.Transactions;
using static ELog.Core.PMMSEnums;
using ELog.Core.Authorization;
using static ELog.Core.Authorization.Roles.StaticRoleNames;
using Microsoft.PowerBI.Api.Models;
using Microsoft.AspNetCore.Http;

namespace ELog.Application.ElogApi
{
    //[PMMSAuthorize]
    public class LoginUser : ApplicationService
    {
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusMasterRepository;
        private readonly UserManager _userManager;
        private readonly PMMSLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly SignInManager _signInManager;
        private readonly IUserManagementConfig _userManagementConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginUser(
            IRepository<ApprovalStatusMaster> approvalStatusMasterRepository,
            UserManager userManager,
            PMMSLoginResultTypeHelper abpLoginResultTypeHelper,
            // IRepository<User, long> userRepository,
            SignInManager signInManager,
            IUserManagementConfig userManagementConfig,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _approvalStatusMasterRepository = approvalStatusMasterRepository;
            _userManager = userManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
          //  _userRepository = userRepository;
            _signInManager = signInManager;
            _userManagementConfig = userManagementConfig;
            _httpContextAccessor = httpContextAccessor;
        }

        

        public async Task<Object> Login(string userNameOrEmailAddress, string plainPassword,string plantCode)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
                
            }

            var user = await _userManager.FindByNameOrEmailAsync(userNameOrEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException("Please check userId or password.");
            }
            var resetPasswordDaysLeft = await _userManager.GetResetPasswordDaysLeft(user.Id);
            if (resetPasswordDaysLeft <= 0)
            {
                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(null, resetPasswordDaysLeft);
            }
            //var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.Password, plainPassword);
            var IsCheck = await _userManager.CheckPasswordAsync(user, plainPassword);
            List<string> User = new List<string>();
            var result = new
            {
                UserName = user.FullName
                ,
            };
            if (!IsCheck)
            {
                throw new UserFriendlyException("Please check user password.");
            }
            if(user != null && IsCheck)
            {
                _httpContextAccessor.HttpContext.Session.Clear();
                _httpContextAccessor.HttpContext.Session.SetString("PlantCode", plantCode);
                return result;
            }
            else
            {
                throw new UserFriendlyException("Please check user credential.");
            }
        }
    }
}
