using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace ELog.Core.Authorization
{
    public class PMMSAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter, ITransientDependency
    {
        private static bool isTesting = false;
        private readonly IRepository<RolePermissions, int> _rolePermissionsRepository;
        private readonly IRepository<UserRole, long> _userRolesRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<ApprovalStatusMaster, int> _approvalStatusRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession AbpSession;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public static void EnableTesting(bool flag)
        {
            isTesting = flag;
        }

        public PMMSAuthorizeAttribute()
        {
            if (!isTesting)
            {
                _rolePermissionsRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RolePermissions, int>>().Object;
                _userRolesRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UserRole, long>>().Object;
                _userRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>().Object;
                _roleRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Role>>().Object;
                _approvalStatusRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ApprovalStatusMaster, int>>().Object;
                AbpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>().Object;
                _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>().Object;
                _httpContextAccessor = IocManager.Instance.ResolveAsDisposable<IHttpContextAccessor>().Object;
            }
        }

        public string Permissions { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!AbpSession.UserId.HasValue)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var refreshToken = _httpContextAccessor.HttpContext.Request.Headers["RefreshToken"].FirstOrDefault();
            if (refreshToken != null)
            {
                var decodedRefreshToken = EncryptDecryptHelper.Decrypt(refreshToken);
                var user = _userRepository.FirstOrDefault(AbpSession.UserId.Value);
                if (user.ConcurrencyStamp != decodedRefreshToken)
                {
                    //   throw new AccessViolationException("Another session is active for user.Please login again to proceed.");
                }
            }
            using var unitOfWork = _unitOfWorkManager.Begin();
            if (!(string.IsNullOrEmpty(Permissions) || string.IsNullOrWhiteSpace(Permissions)))
            {
                bool isAuthorized = false;
                const int approvalStatusValue = (int)(PMMSEnums.ApprovalStatus.Approved);
                var approvedApprovalStatus = Enum.GetName(typeof(PMMSEnums.ApprovalStatus), approvalStatusValue);

               var _user = _userRolesRepository.GetAll().ToList();
               var _user_roles = _roleRepository.GetAll().ToList();
               var _user_a = _approvalStatusRepository.GetAll().ToList();
                var r_pr = _rolePermissionsRepository.GetAll().ToList();

                var currentUserPermissions = (from _userRoles in _userRolesRepository.GetAll()
                                              join role in _roleRepository.GetAll() on _userRoles.RoleId equals role.Id
                                              join approvalStatus in _approvalStatusRepository.GetAll() on role.ApprovalStatusId equals approvalStatus.Id
                                              join _rolesPermission in _rolePermissionsRepository.GetAll()
                                              on _userRoles.RoleId equals _rolesPermission.RoleId
                                              where _userRoles.UserId == AbpSession.UserId.Value
                                              && role.IsActive && approvalStatus.ApprovalStatus.ToLower() == approvedApprovalStatus.ToLower()
                                              select new { _rolesPermission.PermissionName }).Distinct()?.ToList() ?? default;

                if (currentUserPermissions?.Count > 0)
                {
                    var requiredPermissions = Permissions.Split(",");
                    foreach (var permission in requiredPermissions)
                    {
                        var currnetModulePermission = currentUserPermissions.Find(a => string.Equals(a.PermissionName, permission, System.StringComparison.OrdinalIgnoreCase));
                        if (currnetModulePermission != null)
                        {
                            isAuthorized = true;
                        }
                    }
                }
                if (!isAuthorized)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}