//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.Masters.Departments.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Entities;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Masters.Departments
//{
//    [PMMSAuthorize]
//    public class DepartmentAppService : ApplicationService, IDepartmentAppService
//    {
//        private readonly IRepository<DepartmentMaster> _departmentRepository;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
//        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IRepository<AreaMaster> _areaRepository;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

//        public DepartmentAppService(IRepository<DepartmentMaster> departmentRepository, IRepository<PlantMaster> plantRepository,
//            IRepository<StandardWeightBoxMaster> standardWeightBoxRepository, IRepository<StandardWeightMaster> standardWeightrRepository,
//           IRepository<AreaMaster> areaRepository, IMasterCommonRepository masterCommonRepository,
//           IHttpContextAccessor httpContextAccessor, IRepository<ApprovalStatusMaster> approvalStatusRepository)

//        {
//            _plantRepository = plantRepository;
//            _departmentRepository = departmentRepository;
//            _standardWeightBoxRepository = standardWeightBoxRepository;
//            _standardWeightRepository = standardWeightrRepository;
//            _areaRepository = areaRepository;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _masterCommonRepository = masterCommonRepository;
//            _httpContextAccessor = httpContextAccessor;
//            _approvalStatusRepository = approvalStatusRepository;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Department_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<DepartmentDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _departmentRepository.GetAsync(input.Id);
//            var department = ObjectMapper.Map<DepartmentDto>(entity);
//            department.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Department_SubModule);
//            department.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
//            return department;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Department_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<PagedResultDto<DepartmentListDto>> GetAllAsync(PagedDepartmentResultRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<DepartmentListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Department_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto input)
//        {
//            var department = ObjectMapper.Map<DepartmentMaster>(input);
//            department.TenantId = AbpSession.TenantId;
//            department.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Department_SubModule);
//            var currentDate = DateTime.UtcNow;
//            department.DepartmentCode = $"D{currentDate.Month:D2}{currentDate:yy}{_masterCommonRepository.GetNextUOMSequence():D4}";
//            await _departmentRepository.InsertAsync(department);

//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<DepartmentDto>(department);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Department_SubModule + "." + PMMSPermissionConst.Edit)]
//        public async Task<DepartmentDto> UpdateAsync(DepartmentDto input)
//        {
//            if (!input.IsActive)
//            {
//                if (!await ValidateDepartmentIsNotAssignedForInActive(input))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.DepartmentCannotDeactivated);
//                }
//            }
//            var department = await _departmentRepository.GetAsync(input.Id);
//            department.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Department_SubModule, department.ApprovalStatusId);

//            ObjectMapper.Map(input, department);

//            await _departmentRepository.UpdateAsync(department);

//            return await GetAsync(input);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Department_SubModule + "." + PMMSPermissionConst.Delete)]
//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            if (await IsDepartmentAssociatedWithAnyMaster(input.Id))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.DepartmentDelete);
//            }
//            var department = await _departmentRepository.GetAsync(input.Id).ConfigureAwait(false);
//            await _departmentRepository.DeleteAsync(department).ConfigureAwait(false);
//        }

//        private async Task<bool> ValidateDepartmentIsNotAssignedForInActive(DepartmentDto input)
//        {
//            return input.IsActive || !await IsDepartmentAssociatedWithAnyMaster(input.Id);
//        }

//        private async Task<bool> IsDepartmentAssociatedWithAnyMaster(int id)
//        {
//            return await (from department in _departmentRepository.GetAll().Where(x => x.IsActive)

//                          join stdWtBox in _standardWeightBoxRepository.GetAll().Where(x => x.IsActive)
//                          on department.Id equals stdWtBox.DepartmentId into stdWtBoxps
//                          from stdWtBox in stdWtBoxps.DefaultIfEmpty()

//                          join stdWt in _standardWeightRepository.GetAll().Where(x => x.IsActive)
//                          on department.Id equals stdWt.DepartmentId into stdWtps
//                          from stdWt in stdWtps.DefaultIfEmpty()

//                          join area in _areaRepository.GetAll().Where(x => x.IsActive)
//                          on department.Id equals area.DepartmentId into areaps
//                          from area in areaps.DefaultIfEmpty()

//                          where department.Id == id
//                          select new
//                          {
//                              stdWtBoxDepartmentId = stdWtBox.DepartmentId,
//                              stdWtDepartmentId = stdWt.DepartmentId,
//                              areaDepartmentId = area.DepartmentId
//                          })
//                           .AnyAsync(x => x.stdWtBoxDepartmentId > 0 || x.areaDepartmentId > 0 ||
//                                             x.stdWtDepartmentId > 0);
//        }

//        /// <summary>
//        /// Should apply sorting if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<DepartmentListDto> ApplySorting(IQueryable<DepartmentListDto> query, PagedDepartmentResultRequestDto input)
//        {
//            //Try to sort query if available
//            var sortInput = input as ISortedResultRequest;
//            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
//            {
//                return query.OrderBy(sortInput.Sorting);
//            }

//            //IQueryable.Task requires sorting, so we should sort if Take will be used.
//            if (input is ILimitedResultRequest)
//            {
//                return query.OrderByDescending(e => e.Id);
//            }

//            //No sorting
//            return query;
//        }

//        /// <summary>
//        /// Should apply paging if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<DepartmentListDto> ApplyPaging(IQueryable<DepartmentListDto> query, PagedDepartmentResultRequestDto input)
//        {
//            //Try to use paging if available
//            if (input is IPagedResultRequest pagedInput)
//            {
//                return query.PageBy(pagedInput);
//            }

//            //Try to limit query result if available
//            if (input is ILimitedResultRequest limitedInput)
//            {
//                return query.Take(limitedInput.MaxResultCount);
//            }

//            //No paging
//            return query;
//        }

//        protected DepartmentListDto MapToEntityDto(DepartmentMaster entity)
//        {
//            return ObjectMapper.Map<DepartmentListDto>(entity);
//        }

//        protected IQueryable<DepartmentListDto> CreateUserListFilteredQuery(PagedDepartmentResultRequestDto input)
//        {
//            var subPlantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var departmentQuery = from department in _departmentRepository.GetAll()
//                                  join subPlant in _plantRepository.GetAll()
//                                  on department.SubPlantId equals subPlant.Id into ps
//                                  from subPlant in ps.DefaultIfEmpty()
//                                  join approvalStatus in _approvalStatusRepository.GetAll()
//                                  on department.ApprovalStatusId equals approvalStatus.Id into paStatus
//                                  from approvalStatus in paStatus.DefaultIfEmpty()
//                                  select new DepartmentListDto
//                                  {
//                                      Id = department.Id,
//                                      DepartmentCode = department.DepartmentCode,
//                                      DepartmentName = department.DepartmentName,
//                                      SubPlantId = department.SubPlantId,
//                                      UserEnteredSubPlantId = subPlant.PlantId,
//                                      IsActive = department.IsActive,
//                                      ApprovalStatusId = department.ApprovalStatusId,
//                                      UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
//                                  };
//            if (input.SubPlantId != null)
//            {
//                departmentQuery = departmentQuery.Where(x => x.SubPlantId == input.SubPlantId);
//            }
//            if (!(string.IsNullOrEmpty(subPlantId) || string.IsNullOrWhiteSpace(subPlantId)))
//            {
//                departmentQuery = departmentQuery.Where(x => x.SubPlantId == Convert.ToInt32(subPlantId));
//            }
//            if (!(string.IsNullOrEmpty(input.DepartmentCode) || string.IsNullOrWhiteSpace(input.DepartmentCode)))
//            {
//                departmentQuery = departmentQuery.Where(x => x.DepartmentCode.Contains(input.DepartmentCode));
//            }
//            if (input.ActiveInactiveStatusId != null)
//            {
//                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
//                {
//                    departmentQuery = departmentQuery.Where(x => !x.IsActive);
//                }
//                else if (input.ActiveInactiveStatusId == (int)Status.Active)
//                {
//                    departmentQuery = departmentQuery.Where(x => x.IsActive);
//                }
//            }
//            if (input.ApprovalStatusId != null)
//            {
//                departmentQuery = departmentQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
//            }
//            return departmentQuery;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Department_SubModule + "." + PMMSPermissionConst.Approver)]
//        public async Task ApproveOrRejectDepartmentAsync(ApprovalStatusDto input)
//        {
//            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
//            {
//                if (await IsDepartmentAssociatedWithAnyMaster(input.Id))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.DepartmentRejected);
//                }
//            }
//            var department = await _departmentRepository.GetAsync(input.Id);

//            department.ApprovalStatusId = input.ApprovalStatusId;
//            department.ApprovalStatusDescription = input.Description;
//            await _departmentRepository.UpdateAsync(department);
//        }
//    }
//}