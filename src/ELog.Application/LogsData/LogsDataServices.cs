
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.LogsData;
using ELog.Application.LogsData.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Elog.Application.LogsData
{
    [PMMSAuthorize]

    public class LogsDataServices : ApplicationService, ILogsDataServices
    {
        private readonly IRepository<Logs> _logsRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IMasterCommonRepository _masterCommonRepository;

        public LogsDataServices(IRepository<Logs> logsRepository, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logsRepository = logsRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
        }



        [PMMSAuthorize]
        public async Task<LogsDataDto> CreateAsync(LogsDataDto input)
        {

            var clientCreate = ObjectMapper.Map<Logs>(input);

            await _logsRepository.InsertAsync(clientCreate);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<LogsDataDto>(clientCreate);
        }



        //public async Task<LogsDataDto> GetAsync(EntityDto<int> input)
        //{
        //    var entity = await _logsRepository.GetAsync(input.Id);
        //    return ObjectMapper.Map<LogsDataDto>(entity);
        //}


        [PMMSAuthorize]
        public async Task<PagedResultDto<LogsDataDto>> GetAllAsync(PagedLogsDataResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);


            return new PagedResultDto<LogsDataDto>(
                totalCount,
                entities
            );
        }
        protected IQueryable<LogsDataDto> CreateUserListFilteredQuery(PagedLogsDataResultRequestDto input)
        {
            var clientQuery =
                from client in _logsRepository.GetAll()

                select new LogsDataDto
                {
                    Id = client.Id,
                    Action = client.Action,
                    Data = client.Data,

                };


            //if (input.Id != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.Id == input.Id);
            //}
            //if (input.log_Id != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.log_Id == input.log_Id);
            //}
            //if (input.notification_type != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.notification_type == input.notification_type);
            //}
            //if (input.assign_roles != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.assign_roles == input.assign_roles);
            // }
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



        protected IQueryable<LogsDataDto> ApplySorting(IQueryable<LogsDataDto> query, PagedLogsDataResultRequestDto input)
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
        protected IQueryable<LogsDataDto> ApplyPaging(IQueryable<LogsDataDto> query, PagedLogsDataResultRequestDto input)
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
