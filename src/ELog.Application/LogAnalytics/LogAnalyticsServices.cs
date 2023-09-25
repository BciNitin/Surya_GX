using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
//using PMMS.Application.Masters.HandlingUnits.Dto;
using Elog.Core.Entities;
using ELog.Application.LogAnalytics;
using ELog.Application.LogAnalytics.Dto;
using ELog.Core.Authorization;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Elog.Application.LogAnalytics
{
    [PMMSAuthorize]
    public class LogAnalyticsServices : ApplicationService, ILogAnalyticsSecvice
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IRepository<ClientForm> _logAnalyticsRepository { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IMasterCommonRepository _masterCommonRepository;

        public LogAnalyticsServices(IRepository<ClientForm> logAnalyticsRepository, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logAnalyticsRepository = logAnalyticsRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
        }

        [PMMSAuthorize]
        //  [PMMSAuthorize(Permissions = "ClientForms.View")]
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.ClientForms_SubModule + "." + PMMSPermissionConst.View)]

        public async Task<LogAnalyticsDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _logAnalyticsRepository.GetAsync(input.Id);
            return ObjectMapper.Map<LogAnalyticsDto>(entity);
        }

        [PMMSAuthorize]
        public async Task<PagedResultDto<LogAnalyticsDto>> GetAllAsync(PagedLogAnalyticsResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalForms = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            //  var tatalActive = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<LogAnalyticsDto>(
                totalForms,
                entities
              );
        }

        protected IQueryable<LogAnalyticsDto> CreateUserListFilteredQuery(PagedLogAnalyticsResultRequestDto input)
        {
            var clientQuery =
                from client in _logAnalyticsRepository.GetAll()

                select new LogAnalyticsDto
                {
                    Id = client.Id,
                    ClientId = client.ClientId,
                    FormName = client.FormName,
                    FormEndDate = client.FormEndDate,
                    FormStartDate = client.FormStartDate,
                    FormJson = client.FormJson,
                    IsActive = client.IsActive,
                    CreationDate = client.CreationDate,
                    ModifiedDate = client.ModifiedDate,
                    FormStatus = client.FormStatus,
                    UpdatedBy = client.UpdatedBy,
                    ApprovedBy = client.ApprovedBy,
                    CheckedBy = client.CheckedBy,
                    CreatedBy = client.CreatedBy,
                    ApproveDateTime = client.ApproveDateTime
                };

            if (input.Id != null)
            {
                clientQuery = clientQuery.Where(x => x.Id == input.Id);
            }
            if (input.ClientId != null)
            {
                clientQuery = clientQuery.Where(x => x.ClientId == input.ClientId);
            }

            if (input.FormName != null)
            {
                clientQuery = clientQuery.Where(x => x.FormName == input.FormName);
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
            return clientQuery;
        }

        protected IQueryable<LogAnalyticsDto> ApplySorting(IQueryable<LogAnalyticsDto> query, PagedLogAnalyticsResultRequestDto input)
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
        protected IQueryable<LogAnalyticsDto> ApplyPaging(IQueryable<LogAnalyticsDto> query, PagedLogAnalyticsResultRequestDto input)
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

        [PMMSAuthorize]
        public async Task<LogAnalyticsListDto> GetByFilterAsync(int input)
        {
            int Count = await _logAnalyticsRepository.GetAll().CountAsync();

            int totalActiveForm = await _logAnalyticsRepository.GetAll().Where(x => x.IsActive == true).CountAsync();

            int totalInactiveForms = Count - totalActiveForm;
            int totalApprovalCount = await _logAnalyticsRepository.GetAll().Where(x => x.FormStatus == 2).CountAsync();
            int totalDisApproveCount = Count - totalApprovalCount;
            LogAnalyticsListDto c = new LogAnalyticsListDto
            {
                Count = Count,
                totalActiveForm = totalActiveForm,
                totalInactiveForms = totalInactiveForms,
                totalApprovalCount = totalApprovalCount,
                totalDisApproveCount = totalDisApproveCount
            };
            return c;
        }



    }
}
