using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Elog.Application.ElogControls.Dto;
//using PMMS.Application.Masters.HandlingUnits.Dto;
using Elog.Core.Entities;
using ELog.Core.Authorization;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Elog.Application.ElogControls
{
    [PMMSAuthorize]
    public class ElogControlsService : ApplicationService, IElogControlsService
    {
        private readonly IRepository<ElogControl> _elogControlRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IMasterCommonRepository _masterCommonRepository;

        public ElogControlsService(IRepository<ElogControl> elogControlRepository, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _elogControlRepository = elogControlRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
        }

        //        [PMMSAuthorize(Permissions = "ClientForms.Add")]
        public async Task<ElogControlsDto> CreateAsync(ElogControlsDto input)
        {

            var elogControlCreate = ObjectMapper.Map<ElogControl>(input);

            await _elogControlRepository.InsertAsync(elogControlCreate);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<ElogControlsDto>(elogControlCreate);
        }

        public async Task<ElogControlsDto> UpdateAsync(ElogControlsDto input)
        {
            var elogControlUpdate = await _elogControlRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, elogControlUpdate);

            await _elogControlRepository.UpdateAsync(elogControlUpdate);

            return await GetAsync(input);
        }



        //  [PMMSAuthorize]
        //  [PMMSAuthorize(Permissions = "ClientForms.View")]
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.ClientForms_SubModule + "." + PMMSPermissionConst.View)]

        public async Task<ElogControlsDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _elogControlRepository.GetAsync(input.Id);
            return ObjectMapper.Map<ElogControlsDto>(entity);
        }

        public async Task<PagedResultDto<ElogControlsDto>> GetAllAsync(PagedElogControlsResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<ElogControlsDto>(
                totalCount,
                entities
            );
        }
        protected IQueryable<ElogControlsDto> CreateUserListFilteredQuery(PagedElogControlsResultRequestDto input)
        {
            var elogControlQuery =
                from elogControl in _elogControlRepository.GetAll()

                select new ElogControlsDto
                {
                    Id = elogControl.Id,
                    ELogId = elogControl.ELogId
                };

            if (input.Id != null)
            {
                elogControlQuery = elogControlQuery.Where(x => x.Id == input.Id);
            }
            if (input.ELogId != null)
            {
                elogControlQuery = elogControlQuery.Where(x => x.ELogId == input.ELogId);
            }
            //if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            //{
            //    gateEntryQuery = gateEntryQuery.Where(x => x.GatePassNumber.Contains(input.Keyword) || x.GatePassNumber.Contains(input.Keyword));
            //}
            //if (input.ActiveInactiveStatusId != null)
            //{
            //    if (input.ActiveInactiveStatusId == (int)Status.In_Active)
            //    {
            //        gateEntryQuery = gateEntryQuery.Where(x => !x.IsActive);
            //    }
            //    else if (input.ActiveInactiveStatusId == (int)Status.Active)
            //    {
            //        gateEntryQuery = gateEntryQuery.Where(x => x.IsActive);
            //    }
            //}
            return elogControlQuery;
        }



        protected IQueryable<ElogControlsDto> ApplySorting(IQueryable<ElogControlsDto> query, PagedElogControlsResultRequestDto input)
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
        protected IQueryable<ElogControlsDto> ApplyPaging(IQueryable<ElogControlsDto> query, PagedElogControlsResultRequestDto input)
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
