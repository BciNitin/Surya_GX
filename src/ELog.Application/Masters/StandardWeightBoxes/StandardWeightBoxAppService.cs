using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.StandardWeightBoxes.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Masters.StandardWeightBoxes
{
    [PMMSAuthorize]
    public class StandardWeightBoxAppService : ApplicationService, IStandardWeightBoxAppService
    {
        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<StandardWeightMaster> _standradWeightRepository;

        public StandardWeightBoxAppService(IRepository<StandardWeightBoxMaster> standardWeightBoxRepository, IRepository<DepartmentMaster> departmentrepository,
            IRepository<PlantMaster> plantRepository, IRepository<AreaMaster> areaRepository, IRepository<StandardWeightMaster> standradWeightRepository,
            IHttpContextAccessor httpContextAccessor, IMasterCommonRepository masterCommonRepository,
        IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _standardWeightBoxRepository = standardWeightBoxRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _departmentRepository = departmentrepository;
            _plantRepository = plantRepository;
            _areaRepository = areaRepository;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            _standradWeightRepository = standradWeightRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeightBox_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<StandardWeightBoxDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _standardWeightBoxRepository.GetAsync(input.Id);
            var standardWeightBox = ObjectMapper.Map<StandardWeightBoxDto>(entity);
            standardWeightBox.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.StandardWeightBox_SubModule);
            standardWeightBox.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return standardWeightBox;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeightBox_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<StandardWeightBoxListDto>> GetAllAsync(PagedStandardWeightBoxResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<StandardWeightBoxListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeightBox_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<StandardWeightBoxDto> CreateAsync(CreateStandardWeightBoxDto input)
        {
            if (await _standardWeightBoxRepository.GetAll().AnyAsync(x => x.StandardWeightBoxId == input.StandardWeightBoxId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.StandardWeightBoxIdAlreadyExist);
            }
            var standardWeightBox = ObjectMapper.Map<StandardWeightBoxMaster>(input);
            standardWeightBox.TenantId = AbpSession.TenantId;
            standardWeightBox.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.StandardWeightBox_SubModule);
            await _standardWeightBoxRepository.InsertAsync(standardWeightBox);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<StandardWeightBoxDto>(standardWeightBox);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeightBox_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<StandardWeightBoxDto> UpdateAsync(StandardWeightBoxDto input)
        {
            if (!input.IsActive)
            {
                if (await IsStandardWeightBoxAssociatedWithAnyMaster(input.Id))
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.StdWeightBoxInactive);
                }
            }
            if (await _standardWeightBoxRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.StandardWeightBoxId == input.StandardWeightBoxId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.StandardWeightBoxIdAlreadyExist);
            }
            var standardWeightBox = await _standardWeightBoxRepository.GetAsync(input.Id);
            standardWeightBox.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.StandardWeightBox_SubModule, standardWeightBox.ApprovalStatusId);
            ObjectMapper.Map(input, standardWeightBox);

            await _standardWeightBoxRepository.UpdateAsync(standardWeightBox);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeightBox_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            if (await IsStandardWeightBoxAssociatedWithAnyMaster(input.Id))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.StdWeightBoxDelete);
            }
            var standardWeightBox = await _standardWeightBoxRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _standardWeightBoxRepository.DeleteAsync(standardWeightBox).ConfigureAwait(false);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<StandardWeightBoxListDto> ApplySorting(IQueryable<StandardWeightBoxListDto> query, PagedStandardWeightBoxResultRequestDto input)
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
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<StandardWeightBoxListDto> ApplyPaging(IQueryable<StandardWeightBoxListDto> query, PagedStandardWeightBoxResultRequestDto input)
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

        protected StandardWeightBoxListDto MapToEntityDto(StandardWeightBoxMaster entity)
        {
            return ObjectMapper.Map<StandardWeightBoxListDto>(entity);
        }

        protected IQueryable<StandardWeightBoxListDto> CreateUserListFilteredQuery(PagedStandardWeightBoxResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var standardWtBoxquery = from wtBox in _standardWeightBoxRepository.GetAll()
                                     join subPlant in _plantRepository.GetAll()
                                     on wtBox.SubPlantId equals subPlant.Id into ps
                                     from subPlant in ps.DefaultIfEmpty()
                                     join departMent in _departmentRepository.GetAll()
                                     on wtBox.DepartmentId equals departMent.Id into ds
                                     from departMent in ds.DefaultIfEmpty()
                                     join area in _areaRepository.GetAll()
                                     on wtBox.AreaId equals area.Id into areaps
                                     from area in areaps.DefaultIfEmpty()
                                     join approvalStatus in _approvalStatusRepository.GetAll()
                                                  on wtBox.ApprovalStatusId equals approvalStatus.Id into paStatus
                                     from approvalStatus in paStatus.DefaultIfEmpty()
                                     select new StandardWeightBoxListDto
                                     {
                                         Id = wtBox.Id,
                                         StandardWeightBoxId = wtBox.StandardWeightBoxId,
                                         AreaId = wtBox.AreaId,
                                         UserEnteredAreaId = area.AreaCode,
                                         SubPlantId = wtBox.SubPlantId,
                                         UserEnteredSubPlantId = subPlant.PlantId,
                                         DepartmentId = wtBox.DepartmentId,
                                         UserEnteredDepartmentId = departMent.DepartmentCode,
                                         IsActive = wtBox.IsActive,
                                         ApprovalStatusId = wtBox.ApprovalStatusId,
                                         UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                                     };
            if (input.SubPlantId != null)
            {
                standardWtBoxquery = standardWtBoxquery.Where(x => x.SubPlantId == input.SubPlantId);
            }
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                standardWtBoxquery = standardWtBoxquery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }
            if (input.DepartmentId != null)
            {
                standardWtBoxquery = standardWtBoxquery.Where(x => x.DepartmentId == input.DepartmentId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                standardWtBoxquery = standardWtBoxquery.Where(x => x.StandardWeightBoxId.Contains(input.Keyword));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    standardWtBoxquery = standardWtBoxquery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    standardWtBoxquery.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                standardWtBoxquery = standardWtBoxquery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }

            return standardWtBoxquery;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.StandardWeightBox_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectStandardWeightBoxAsync(ApprovalStatusDto input)
        {
            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
            {
                if (await IsStandardWeightBoxAssociatedWithAnyMaster(input.Id))
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.StdWeightBoxReject);
                }
            }
            var standardWeightBox = await _standardWeightBoxRepository.GetAsync(input.Id);

            standardWeightBox.ApprovalStatusId = input.ApprovalStatusId;
            standardWeightBox.ApprovalStatusDescription = input.Description;
            await _standardWeightBoxRepository.UpdateAsync(standardWeightBox);
        }
        private async Task<bool> IsStandardWeightBoxAssociatedWithAnyMaster(int id)
        {
            return await (from standardWeightBox in _standardWeightBoxRepository.GetAll()
                          join standardWeight in _standradWeightRepository.GetAll()
                          on standardWeightBox.Id equals standardWeight.StandardWeightBoxMasterId into standardWeightBoxs
                          from standardWeight in standardWeightBoxs.DefaultIfEmpty()
                          where standardWeightBox.Id == id
                          select new
                          {
                              standardWeightBoxId = standardWeightBox.Id
                          })
                          .AnyAsync(x => x.standardWeightBoxId > 0);
        }
    }
}