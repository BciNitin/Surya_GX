using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.Password.Dto;
using ELog.Application.Users.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Password
{

    public class ChangePswdAppService : ApplicationService, IChangePswdAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<User, long> _repository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<WMSPasswordManager> _wmsPasswordRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusMasterRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public ChangePswdAppService(UserManager userManager, IRepository<User, long> repository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<ApprovalStatusMaster> approvalStatusMasterRepository,
             IRepository<Role> roleRepository,
             IRepository<WMSPasswordManager> wmsPasswordRepository)
        {
            _userRoleRepository = userRoleRepository;
            _approvalStatusMasterRepository = approvalStatusMasterRepository;
            _roleRepository = roleRepository;
            _repository = repository;
            _userManager = userManager;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _wmsPasswordRepository = wmsPasswordRepository;
        }

        [PMMSAuthorize]
        public async Task<ChangePasswordDto> ChangePasswordAsync(ChangePasswordDto input)
        {
            return await ChangePassword(input).ConfigureAwait(false);
        }

        public async Task<ChangePasswordSuperAdminOutputDto> ChangePasswordForSuperAdminAsync(ChangePasswordSuperAdminDto input)
        {
            return await ChangePasswordForSuperAdmin(input).ConfigureAwait(false);
        }

        // [PMMSAuthorize(Permissions = PMMSPermissionConst.Password_SubModule + "." + PMMSPermissionConst.Edit)]
        [PMMSAuthorize]
        public async Task<UserDto> GetAsync(EntityDto<long> input)
        {
            return await GetUserById(input);
        }

        private async Task<UserDto> GetUserById(EntityDto<long> input)
        {
            var entity = await _repository.GetAsync(input.Id);
            var userDto = ObjectMapper.Map<UserDto>(entity);
            userDto.FirstName = entity.Name;
            userDto.LastName = entity.Surname;
            userDto.Email = entity.EmailAddress;
            userDto.UserName = entity.UserName;
            userDto.PasswordStatus = entity.PasswordStatus;
            var rolesList = await _userManager.GetRolesAsync(entity);
            userDto.RoleNames = rolesList.ToArray();
            return userDto;
        }

        private async Task<ChangePasswordSuperAdminOutputDto> ChangePasswordForSuperAdmin(ChangePasswordSuperAdminDto input)
        {
            try
            {
                var user = await _userManager.FindByNameOrEmailAsync(input.EmployeeCode).ConfigureAwait(false);
                if (user != null)
                {
                    var rolesList = await _userManager.GetRolesAsync(user);
                    if (rolesList.Contains(RoleCategories.SuperAdmin.ToString()))
                    {
                        user.PasswordStatus = (int)ResetPasswordStatus.SelfReset;
                        user.PasswordResetTime = DateTime.Now;
                        user.IsLockout = false;
                        CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword).ConfigureAwait(false));
                        return new ChangePasswordSuperAdminOutputDto
                        {
                            Status = true,
                            Message = "Password Updated Successfully"
                        };
                    }
                    else
                    {
                        return new ChangePasswordSuperAdminOutputDto
                        {
                            Status = false,
                            Message = "User is not a SuperAdmin"
                        };

                    }
                }
                else
                {
                    return new ChangePasswordSuperAdminOutputDto
                    {
                        Status = false,
                        Message = "User not exists"
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Message.Contains("User name"))
                {
                    string newMessage = ex.Message.Replace("User name", "Username");
                    throw new UserFriendlyException(422, newMessage);
                }
            }
            return new ChangePasswordSuperAdminOutputDto
            {
                Status = true
            };
        }

        public async Task<WMSPasswordManagerDto> CreatePasswordManagerAsync(WMSPasswordManagerDto input)
        {

            var passwordmanager = ObjectMapper.Map<WMSPasswordManager>(input);
            passwordmanager.Password = await EncryptPassword(input.Password);
            await _wmsPasswordRepository.InsertAsync(passwordmanager);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<WMSPasswordManagerDto>(passwordmanager);
        }

        public async Task<List<WMSPasswordManagerDetailsDto>> GetPasswordManagerAsync(string input)
        {
            //List<WMSPasswordManagerDetailsDto> entities = new List<WMSPasswordManagerDetailsDto>();

            var query = from wms in _wmsPasswordRepository.GetAll()
                        where wms.UserName == input
                        select new WMSPasswordManagerDetailsDto
                        {
                            Id = wms.Id,
                            UserName = wms.UserName,
                            Password = wms.Password,
                            IsActive = wms.IsActive,
                            CreatedOn = wms.CreationTime,
                            ModifiedOn = wms.LastModificationTime
                        };


            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            entities = entities.OrderByDescending(x => x.CreatedOn).Take(3).ToList();
            return entities;
        }


        public Task<string> EncryptPassword(string input)
        {

            byte[] clearBytes = Encoding.Unicode.GetBytes(input);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    input = Convert.ToBase64String(ms.ToArray());
                }
            }
            return Task.FromResult(input);
        }

        public Task<string> DecryptPassword(string input)
        {

            input = input.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(input);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    input = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return Task.FromResult(input);
        }


        private async Task<ChangePasswordDto> ChangePassword(ChangePasswordDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId).ConfigureAwait(false);
            try
            {
                //Check user password last 3 attempts from WMSPasswordMAnager table 
                var lastThreePasswords = GetPasswordManagerAsync(input.UserName);


                for (int i = 0; i < lastThreePasswords.Result.Count; i++)
                {
                    if (input.NewPassword == DecryptPassword(lastThreePasswords.Result[i].Password).Result)
                    {

                        throw new UserFriendlyException(422, "Password Should be different from the last three passwords.");


                    }
                }

                if (input.PasswordStatus == (int)ResetPasswordStatus.ResetPending || input.PasswordStatus == (int)ResetPasswordStatus.Submitted)
                {
                    if (input.UserId != input.CurrentUser)
                    {
                        user.PasswordStatus = (int)ResetPasswordStatus.Submitted;
                    }
                    else
                    {
                        user.PasswordStatus = (int)ResetPasswordStatus.SelfResetAfterRequest;
                    }
                }
                else
                {
                    if (input.UserId != input.CurrentUser)
                    {
                        user.PasswordStatus = (int)ResetPasswordStatus.Submitted;
                    }
                    else
                    {
                        user.PasswordStatus = (int)ResetPasswordStatus.SelfReset;
                    }
                }
                DateTime localdate = DateTime.Now;
                user.PasswordResetTime = localdate;
                user.IsLockout = false;
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword).ConfigureAwait(false));

                //Add entry to WMS Password manager
                WMSPasswordManagerDto userPw = new WMSPasswordManagerDto();
                userPw.UserName = input.UserName;
                userPw.Password = input.NewPassword;
                userPw.IsActive = true;

                WMSPasswordManagerDto result = await CreatePasswordManagerAsync(userPw);

                return new ChangePasswordDto
                {
                    Status = true
                };

            }
            catch (UserFriendlyException ex)
            {
                if (ex.Message.Contains("User name"))
                {
                    string newMessage = ex.Message.Replace("User name", "Username");
                    throw new UserFriendlyException(422, newMessage);
                }
                else if (ex.Message.Contains("last three passwords"))
                {
                    throw new UserFriendlyException(422, ex.Message);
                }
            }

            return new ChangePasswordDto
            {
                Status = true
            };
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }



        [PMMSAuthorize(Permissions = PMMSPermissionConst.Password_SubModule + "." + PMMSPermissionConst.View)]

        public async Task<PagedResultDto<RequestedUsersListDto>> GetAllAsync(PagedChangePasswordResultRequestDto input)
        {
            string userRole = string.Empty;
            var entity = await _repository.GetAsync(input.CurrentUser);
            var rolesList = await _userManager.GetRolesAsync(entity);

            if (rolesList.Contains(RoleCategories.SuperAdmin.ToString()) && rolesList.Contains(RoleCategories.Admin.ToString()))
            {
                userRole = RoleCategories.SuperAdminAndAdmin.ToString();
            }
            else if (rolesList.Contains(RoleCategories.SuperAdmin.ToString()))
            {
                userRole = RoleCategories.SuperAdmin.ToString();
            }
            else if (rolesList.Contains(RoleCategories.Admin.ToString()))
            {
                userRole = RoleCategories.Admin.ToString();
            }

            var query = CreateChangePswdListFilteredQuery(input, userRole);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<RequestedUsersListDto>(
                totalCount,
                entities);
        }

        protected IQueryable<RequestedUsersListDto> CreateChangePswdListFilteredQuery(PagedChangePasswordResultRequestDto input, string currentUserRole)
        {

            const int approvalStatusValue = (int)(PMMSEnums.ApprovalStatus.Approved);
            var approvedApprovalStatus = Enum.GetName(typeof(PMMSEnums.ApprovalStatus), approvalStatusValue);
            var query = from users in _repository.GetAllIncluding(a => a.Users).Where(x => (x.PasswordStatus == (int)ResetPasswordStatus.ResetPending || x.PasswordStatus == (int)ResetPasswordStatus.Submitted || x.PasswordStatus == (int)ResetPasswordStatus.SelfResetAfterRequest)
                        && x.IsActive == true && x.ApprovalStatusId == (int)ApprovalStatus.Approved)
                        join userRole in _userRoleRepository.GetAll() on users.Id equals userRole.UserId
                        join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id


                        join approvalStatus in _approvalStatusMasterRepository.GetAll() on role.ApprovalStatusId equals approvalStatus.Id
                        where role.IsActive && approvalStatus.ApprovalStatus.ToLower() == approvedApprovalStatus.ToLower()

                        select new RequestedUsersListDto
                        {
                            FirstName = users.Name,
                            LastName = users.Surname,
                            UserName = users.UserName,
                            UserId = users.Id,
                            Request = "Forgot Password",
                            Status = (users.PasswordStatus == 1) ? "Pending" : (users.PasswordStatus == 2) ? "Submitted" : "Completed",
                            RoleNames = role.Name

                        };




            if (currentUserRole == RoleCategories.SuperAdmin.ToString())
            {
                var result = query.Where(x => x.RoleNames == RoleCategories.SuperAdmin.ToString())
                      .Select(i => new { i.UserId }).Distinct();
                query = query.Where(x => x.RoleNames == RoleCategories.Admin.ToString());
                foreach (var item in result)
                {
                    query = query.Where(x => x.UserId != item.UserId);
                }
            }
            else if (currentUserRole == RoleCategories.Admin.ToString())
            {
                var result = query.Where(x => x.RoleNames == RoleCategories.Admin.ToString() || x.RoleNames == RoleCategories.SuperAdmin.ToString())
                             .Select(i => new { i.UserId }).Distinct();


                query = query.Where(x => x.RoleNames != RoleCategories.Admin.ToString() && x.RoleNames != RoleCategories.SuperAdmin.ToString()).Distinct()
                        .Select(i => new { i.UserId, i.FirstName, i.LastName, i.UserName, i.Status, i.Request }).Distinct()
                         .Select(x => new RequestedUsersListDto
                         {
                             UserId = x.UserId,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             UserName = x.UserName,
                             Status = x.Status,
                             Request = x.Request,
                         });
                foreach (var item in result)
                {
                    query = query.Where(x => x.UserId != item.UserId);
                }
            }
            else if (currentUserRole == RoleCategories.SuperAdminAndAdmin.ToString())
            {
                var result = query.Where(x => x.RoleNames == RoleCategories.SuperAdmin.ToString())
                      .Select(i => new { i.UserId }).Distinct();

                query = query.Where(x => x.RoleNames != RoleCategories.SuperAdmin.ToString())
                        .Select(i => new { i.UserId, i.FirstName, i.LastName, i.UserName, i.Status, i.Request }).Distinct()
                        .Select(x => new RequestedUsersListDto
                        {
                            UserId = x.UserId,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            UserName = x.UserName,
                            Status = x.Status,
                            Request = x.Request,
                        });

                foreach (var item in result)
                {
                    query = query.Where(x => x.UserId != item.UserId);
                }
            }

            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                query = query.Where(x => x.UserName.Contains(input.Keyword)
                                            || x.LastName.Contains(input.Keyword)
                                            || x.FirstName.Contains(input.Keyword));
            }

            if (input.Username != null)
            {
                query = query.Where(x => x.UserId.ToString() == input.Username);
            }
            if (input.FirstName != null)
            {
                query = query.Where(x => x.FirstName == input.FirstName);
            }
            if (input.LastName != null)
            {
                query = query.Where(x => x.LastName == input.LastName);
            }

            return query;
        }

        protected IQueryable<RequestedUsersListDto> ApplySorting(IQueryable<RequestedUsersListDto> query, PagedChangePasswordResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;


            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
            {

                return query.OrderBy(sortInput.Sorting);
            }
            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.UserId);
            }

            //No sorting
            return query;
        }

        protected IQueryable<RequestedUsersListDto> ApplyPaging(IQueryable<RequestedUsersListDto> query, PagedChangePasswordResultRequestDto input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

    }
}
