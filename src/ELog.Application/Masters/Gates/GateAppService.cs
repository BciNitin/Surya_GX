using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.Gates.Dto;
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

namespace ELog.Application.Masters.Gates
{
    [PMMSAuthorize]
    public class GateAppService : ApplicationService, IGateAppService
    {
        private readonly IRepository<GateMaster> _gateRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<PlantMaster> _plantRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GateAppService(IRepository<GateMaster> gateRepository,
           IAbpSession abpSession, IRepository<PlantMaster> plantRepository,
           IMasterCommonRepository masterCommonRepository,
          IRepository<ApprovalStatusMaster> approvalStatusRepository,
          IHttpContextAccessor httpContextAccessor)

        {
            _gateRepository = gateRepository;
            _abpSession = abpSession;
            _plantRepository = plantRepository;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Gate_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<GateDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _gateRepository.GetAsync(input.Id);
            var gate = ObjectMapper.Map<GateDto>(entity);
            gate.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.Gate_SubModule);
            gate.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return gate;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Gate_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<GateListDto>> GetAllAsync(PagedGateResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<GateListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Gate_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<GateDto> CreateAsync(CreateGateDto input)
        {
            if (await _gateRepository.GetAll().AnyAsync(x => x.GateCode == input.GateCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GateCodeAlreadyExist);
            }
            var gate = ObjectMapper.Map<GateMaster>(input);
            gate.TenantId = AbpSession.TenantId;
            gate.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.Gate_SubModule);
            await _gateRepository.InsertAsync(gate);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<GateDto>(gate);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Gate_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<GateDto> UpdateAsync(GateDto input)
        {
            if (await _gateRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.GateCode == input.GateCode))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.GateCodeAlreadyExist);
            }
            var gate = await _gateRepository.GetAsync(input.Id);
            gate.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.Gate_SubModule, gate.ApprovalStatusId);
            ObjectMapper.Map(input, gate);

            await _gateRepository.UpdateAsync(gate);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Gate_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var gate = await _gateRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _gateRepository.DeleteAsync(gate).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Gate_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectGateAsync(ApprovalStatusDto input)
        {
            var gate = await _gateRepository.GetAsync(input.Id);

            gate.ApprovalStatusId = input.ApprovalStatusId;
            gate.ApprovalStatusDescription = input.Description;
            await _gateRepository.UpdateAsync(gate);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<GateListDto> ApplySorting(IQueryable<GateListDto> query, PagedGateResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null && !sortInput.Sorting.IsNullOrWhiteSpace())
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
        protected IQueryable<GateListDto> ApplyPaging(IQueryable<GateListDto> query, PagedGateResultRequestDto input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected IQueryable<GateListDto> CreateUserListFilteredQuery(PagedGateResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var gateQuery = from gate in _gateRepository.GetAll()
                            join plant in _plantRepository.GetAll()
                             on gate.PlantId equals plant.Id into ps
                            from plant in ps.DefaultIfEmpty()
                            join approvalStatus in _approvalStatusRepository.GetAll()
                         on gate.ApprovalStatusId equals approvalStatus.Id into paStatus
                            from approvalStatus in paStatus.DefaultIfEmpty()
                            select new GateListDto
                            {
                                Id = gate.Id,
                                IsActive = gate.IsActive,
                                PlantName = plant.PlantName,
                                PlantId = gate.PlantId,
                                GateCode = gate.GateCode,
                                UserEnteredPlantId = plant.PlantId,
                                ApprovalStatusId = gate.ApprovalStatusId,
                                UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                            };

            if (!(string.IsNullOrEmpty(input.GateCode) || string.IsNullOrWhiteSpace(input.GateCode)))
            {
                gateQuery = gateQuery.Where(x => x.GateCode.Contains(input.GateCode));
            }
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                gateQuery = gateQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (input.PlantId != null)
            {
                gateQuery = gateQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    gateQuery = gateQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    gateQuery = gateQuery.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                gateQuery = gateQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }

            return gateQuery;
        }
    }
}