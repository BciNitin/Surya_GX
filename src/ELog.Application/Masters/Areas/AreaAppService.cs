//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.Masters.Areas.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Entities;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using MobiVueEvo.Application.Masters.Areas;
//using System;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Masters.Areas
//{
//    [PMMSAuthorize]
//    public class AreaAppService : ApplicationService, IAreaAppService
//    {
//        private readonly IRepository<AreaMaster> _areaRepository;
//        private readonly IRepository<DepartmentMaster> _departmentRepository;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
//        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
//        private readonly IRepository<LocationMaster> _locationRepository;
//        private readonly IRepository<CubicleMaster> _cubicleRepository;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IMasterCommonRepository _masterCommonRepository;

//        public AreaAppService(IRepository<AreaMaster> areaRepository, IRepository<DepartmentMaster> departmentRepository, IRepository<PlantMaster> plantRepository,
//            IRepository<StandardWeightMaster> standardWeightRepository, IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
//            IRepository<LocationMaster> locationRepository, IRepository<CubicleMaster> cubicleRepository,
//            IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor, IRepository<ApprovalStatusMaster> approvalStatusRepository)

//        {
//            _areaRepository = areaRepository;
//            _plantRepository = plantRepository;
//            _standardWeightRepository = standardWeightRepository;
//            _standardWeightBoxRepository = standardWeightBoxRepository;
//            _cubicleRepository = cubicleRepository;
//            _locationRepository = locationRepository;
//            _departmentRepository = departmentRepository;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _masterCommonRepository = masterCommonRepository;
//            _httpContextAccessor = httpContextAccessor;
//            _approvalStatusRepository = approvalStatusRepository;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Area_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<AreaDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _areaRepository.GetAsync(input.Id);
//            var area = ObjectMapper.Map<AreaDto>(entity);
//            area.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Area_SubModule);
//            area.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
//            return area;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Area_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<PagedResultDto<AreaListDto>> GetAllAsync(PagedAreaResultRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<AreaListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Area_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<AreaDto> CreateAsync(CreateAreaDto input)
//        {
//            var currentDate = DateTime.UtcNow;
//            var area = ObjectMapper.Map<AreaMaster>(input);
//            area.TenantId = AbpSession.TenantId;
//            area.AreaCode = $"A{currentDate.Month:D2}{currentDate:yy}{_masterCommonRepository.GetNextUOMSequence():D4}";
//            area.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Area_SubModule);
//            await _areaRepository.InsertAsync(area);

//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<AreaDto>(area);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Area_SubModule + "." + PMMSPermissionConst.Edit)]
//        public async Task<AreaDto> UpdateAsync(AreaDto input)
//        {
//            if (!await ValidateAreaIsNotAssignedForInActive(input))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.AreaCannotDeactivated);
//            }
//            var area = await _areaRepository.GetAsync(input.Id);
//            area.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Area_SubModule, area.ApprovalStatusId);

//            ObjectMapper.Map(input, area);

//            await _areaRepository.UpdateAsync(area);

//            return await GetAsync(input);
//        }

//        private async Task<bool> ValidateAreaIsNotAssignedForInActive(AreaDto input)
//        {
//            return input.IsActive || !await IsAreaAssociatedWithAnyMaster(input.Id);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Area_SubModule + "." + PMMSPermissionConst.Delete)]
//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            if (await IsAreaAssociatedWithAnyMaster(input.Id))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.AreaDelete);
//            }

//            var area = await _areaRepository.GetAsync(input.Id).ConfigureAwait(false);
//            await _areaRepository.DeleteAsync(area).ConfigureAwait(false);
//        }

//        private async Task<bool> IsAreaAssociatedWithAnyMaster(int id)
//        {
//            return await (from area in _areaRepository.GetAll().Where(x => x.IsActive)

//                          join stdWtBox in _standardWeightBoxRepository.GetAll().Where(x => x.IsActive)
//                          on area.Id equals stdWtBox.AreaId into stdWtBoxps
//                          from stdWtBox in stdWtBoxps.DefaultIfEmpty()

//                          join stdWt in _standardWeightRepository.GetAll().Where(x => x.IsActive)
//                          on area.Id equals stdWt.AreaId into stdWtps
//                          from stdWt in stdWtps.DefaultIfEmpty()

//                          join location in _locationRepository.GetAll().Where(x => x.IsActive)
//                          on area.Id equals location.AreaId into locationps
//                          from location in locationps.DefaultIfEmpty()

//                          join cubicle in _cubicleRepository.GetAll()
//                          on area.Id equals cubicle.AreaId into cubicles
//                          from cubicle in cubicles.DefaultIfEmpty()
//                          where area.Id == id
//                          select new
//                          {
//                              stdWtBoxAreaId = stdWtBox.AreaId,
//                              stdWtAreaId = stdWt.AreaId,
//                              locationAreaId = location.AreaId,
//                              cubicleAreatId = cubicle.AreaId
//                          })
//                           .AnyAsync(x => x.stdWtBoxAreaId > 0 || x.stdWtAreaId > 0 || x.locationAreaId > 0 ||
//                                             x.cubicleAreatId > 0);
//        }

//        /// <summary>
//        /// Should apply sorting if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<AreaListDto> ApplySorting(IQueryable<AreaListDto> query, PagedAreaResultRequestDto input)
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
//        protected IQueryable<AreaListDto> ApplyPaging(IQueryable<AreaListDto> query, PagedAreaResultRequestDto input)
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

//        protected AreaListDto MapToEntityDto(AreaMaster entity)
//        {
//            return ObjectMapper.Map<AreaListDto>(entity);
//        }

//        protected IQueryable<AreaListDto> CreateUserListFilteredQuery(PagedAreaResultRequestDto input)
//        {
//            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//            var areaQuery = from area in _areaRepository.GetAll()
//                            join subPlant in _plantRepository.GetAll()
//                            on area.SubPlantId equals subPlant.Id into areaps
//                            from subPlant in areaps.DefaultIfEmpty()
//                            join department in _departmentRepository.GetAll()
//                            on area.DepartmentId equals department.Id into ps
//                            from department in ps.DefaultIfEmpty()
//                            join approvalStatus in _approvalStatusRepository.GetAll()
//                            on area.ApprovalStatusId equals approvalStatus.Id into paStatus
//                            from approvalStatus in paStatus.DefaultIfEmpty()
//                            select new AreaListDto
//                            {
//                                Id = area.Id,
//                                DepartmentId = area.DepartmentId,
//                                AreaCode = area.AreaCode,
//                                AreaName = area.AreaName,
//                                SubPlantId = area.SubPlantId,
//                                UserEnteredSubPlantId = subPlant.PlantId,
//                                UserEnteredDepartmentId = department.DepartmentCode,
//                                IsActive = area.IsActive,
//                                ApprovalStatusId = area.ApprovalStatusId,
//                                UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
//                            };
//            if (input.SubPlantId != null)
//            {
//                areaQuery = areaQuery.Where(x => x.SubPlantId == input.SubPlantId);
//            }
//            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//            {
//                areaQuery = areaQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
//            }
//            if (input.DepartmentId != null)
//            {
//                areaQuery = areaQuery.Where(x => x.DepartmentId == input.DepartmentId);
//            }
//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                areaQuery = areaQuery.Where(x => x.AreaCode.Contains(input.Keyword));
//            }
//            if (input.ActiveInactiveStatusId != null)
//            {
//                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
//                {
//                    areaQuery = areaQuery.Where(x => !x.IsActive);
//                }
//                else if (input.ActiveInactiveStatusId == (int)Status.Active)
//                {
//                    areaQuery = areaQuery.Where(x => x.IsActive);
//                }
//            }
//            if (input.ApprovalStatusId != null)
//            {
//                areaQuery = areaQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
//            }
//            return areaQuery;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Area_SubModule + "." + PMMSPermissionConst.Approver)]
//        public async Task ApproveOrRejectAreaAsync(ApprovalStatusDto input)
//        {
//            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
//            {
//                if (await IsAreaAssociatedWithAnyMaster(input.Id))
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.AreaRejected);
//                }
//            }
//            var area = await _areaRepository.GetAsync(input.Id);
//            area.ApprovalStatusId = input.ApprovalStatusId;
//            area.ApprovalStatusDescription = input.Description;
//            await _areaRepository.UpdateAsync(area);
//        }
//    }
//}