using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using ELog.Core;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Interfaces;
using ELog.Core.MultiTenancy;
using ELog.Web.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Web.Core.LDAPAuthentication
{
    public class PMMSLDAPAuthenticationSource : DefaultExternalAuthenticationSource<Tenant, User>, ILdapExternalAuthenticationSource
    {
        private readonly IPMMSLdapSettings _settings;
        private readonly IRepository<Role> _roleRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly ILDAPClient _ldapClient;

        public PMMSLDAPAuthenticationSource(IPMMSLdapSettings settings,
                  IRepository<Role> roleRepository,
                  IRepository<UserRole, long> userRoleRepository,
                  IWebHostEnvironment env,
                  ILDAPClient ldapClient
                  )
        {
            _settings = settings;
            _roleRepository = roleRepository;
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
            _userRoleRepository = userRoleRepository;
            _ldapClient = ldapClient;
        }

        public override string Name => "ELog LDAP";

        public async Task CreateLDAPConnection(int? tenantId)
        {
            _ldapClient.CreateLdapConnection(await _settings.GetUserName(tenantId),
                 await _settings.GetPassword(tenantId), await _settings.GetDomain(tenantId));
        }

        public override async Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, Tenant tenant)
        {
            if (!(await _settings.GetIsEnabled(tenant?.Id)))
            {
                return false;
            }

            await CreateLDAPConnection(tenant?.Id);

            if (_ldapClient.validateUserByBind(string.Format(_settings.GetCommonFetchUrl(), userNameOrEmailAddress), plainPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override async Task<User> CreateUserAsync(string userNameOrEmailAddress, Tenant tenant)
        {
            User user = await base.CreateUserAsync(userNameOrEmailAddress, tenant);

            List<Tuple<int, string>> rolesList = _roleRepository.GetAll().Select(role => Tuple.Create(role.Id, role.Name.ToLower())).ToList();

            await CreateLDAPConnection(tenant?.Id).ConfigureAwait(false);
            List<Dictionary<string, string>> searchResult = _ldapClient.search(string.Format(_settings.GetCommonFetchUrl(), userNameOrEmailAddress), "objectClass=*");
            int approvedStatusId = (int)ApprovalStatus.Approved;
            if (searchResult?.Count > 0)
            {
                LdapProperties dictLdapProperties = new LdapProperties(searchResult[0]);
                user.EmailAddress = dictLdapProperties.Mail;
                user.UserName = dictLdapProperties.Uid;
                user.Name = dictLdapProperties.Givenname;
                user.Surname = dictLdapProperties.Sn;
                user.IsActive = true;
                List<int> lstRoleId = new List<int>();
                if (dictLdapProperties.Memberof != null)
                {
                    List<string> roles = dictLdapProperties.GetRoles();
                    if (roles?.Count > 0)
                    {
                        foreach (var roleName in roles)
                        {
                            if (!rolesList.Any(x => x.Item2 == roleName))
                            {
                                var roleToBeInserted = new Role(tenant?.Id, roleName, roleName)
                                {
                                    ApprovalStatusId = approvedStatusId,
                                    IsActive = true
                                };
                                lstRoleId.Add(_roleRepository.InsertAndGetId(roleToBeInserted));
                            }
                            else
                            {
                                lstRoleId.AddRange(rolesList.Where(x => x.Item2 == roleName).Select(x => x.Item1));
                            }
                        }
                    }
                }
                if (user.Roles == null)
                {
                    user.Roles = new List<UserRole>();
                }
                foreach (var roleId in lstRoleId)
                {
                    user.Roles.Add(new UserRole(tenant?.Id, user.Id, roleId));
                }

                user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = Name,
                    ProviderKey = "NO_KEY",
                    TenantId = tenant?.Id
                }
            };
            }
            return user;
        }

        public override async Task UpdateUserAsync(User user, Tenant tenant)
        {
            await CreateLDAPConnection(tenant?.Id);
            var searchResult = _ldapClient.search(string.Format(_settings.GetCommonFetchUrl(), user.UserName), "objectClass=*");
            if (searchResult?.Count > 0)
            {
                LdapProperties dictLdapProperties = new LdapProperties(searchResult[0]);
                user.EmailAddress = dictLdapProperties.Mail;
                user.UserName = dictLdapProperties.Uid;
                user.Name = dictLdapProperties.Givenname;
                user.Surname = dictLdapProperties.Sn;
                List<int> lstRoleId = new List<int>();
                if (dictLdapProperties.Memberof != null)
                {
                    List<string> ldapRoles = dictLdapProperties.GetRoles();

                    if (ldapRoles?.Count > 0)
                    {
                        List<Tuple<int, string>> rolesList = (from userroles in _userRoleRepository.GetAll()
                                                              join roles in _roleRepository.GetAll()
                                                              on userroles.RoleId equals roles.Id
                                                              where userroles.UserId == user.Id
                                                              select Tuple.Create(roles.Id, roles.Name.ToLower()))?.ToList() ?? default;
                        List<int> lstRolesNotPresentnAD = rolesList
                                                .Where(x => !ldapRoles.Contains(x.Item2.ToLower()))
                                                .Select(x => x.Item1)
                                                .ToList();
                        _userRoleRepository.Delete(x => x.UserId == user.Id && lstRolesNotPresentnAD.Contains(x.RoleId));

                        foreach (var roleName in ldapRoles)
                        {
                            if (!rolesList.Any(x => x.Item2 == roleName))
                            {
                                Role roleToBeInserted = new Role(tenant?.Id, roleName, roleName)
                                {
                                    ApprovalStatusId = (int)ApprovalStatus.Approved,
                                    IsActive = true
                                };
                                lstRoleId.Add(_roleRepository.InsertAndGetId(roleToBeInserted));
                            }
                        }
                    }
                }
                if (user.Roles == null)
                {
                    user.Roles = new List<UserRole>();
                }
                foreach (var roleId in lstRoleId)
                {
                    user.Roles.Add(new UserRole(tenant?.Id, user.Id, roleId));
                }
            }
        }
    }
}