using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.LogFormsHistoryApi.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
//using PMMS.Application.Masters.HandlingUnits.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.LogFormsHistoryApi
{
    [PMMSAuthorize]
    public class LogFormsHistoryService : ApplicationService, ILogFormHistoryService
    {
        private readonly IRepository<LogFormHistory> _logFormHistoryRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IMasterCommonRepository _masterCommonRepository;

        public LogFormsHistoryService(IRepository<LogFormHistory> logFormHistoryRepository, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logFormHistoryRepository = logFormHistoryRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
        }

        //        [PMMSAuthorize(Permissions = "ClientForms.Add")]


        [PMMSAuthorize]
        public async Task<LogFormHistoryDto> CreateAsync(LogFormHistoryDto input)
        {
            var clientCreate = ObjectMapper.Map<LogFormHistory>(input);

            await _logFormHistoryRepository.InsertAsync(clientCreate);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<LogFormHistoryDto>(clientCreate);
        }


        [PMMSAuthorize]
        //  [PMMSAuthorize(Permissions = "ClientForms.View")]
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.ClientForms_SubModule + "." + PMMSPermissionConst.View)]

        public async Task<LogFormHistoryDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _logFormHistoryRepository.GetAsync(input.Id);
            return ObjectMapper.Map<LogFormHistoryDto>(entity);
        }




        //[PMMSAuthorize]
        //public async Task<LogFormHistoryDto> UpdateAsync(LogFormHistoryDto input)
        //{
        //    var clientUpdate = await _logFormHistoryRepository.GetAsync(input.Id);
        //    ObjectMapper.Map(input, input);
        //   // var model = await _logFormHistoryRepository.UpdateAsync(clientUpdate);


        //    await _logFormHistoryRepository.UpdateAsync(clientUpdate);

        //    return await GetAsync(input);
        //}





        [PMMSAuthorize]
        public async Task<PagedResultDto<LogFormHistoryDto>> GetAllAsync(PagedLogFormHistoryResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<LogFormHistoryDto>(
                totalCount,
                entities
            );
        }
        protected IQueryable<LogFormHistoryDto> CreateUserListFilteredQuery(PagedLogFormHistoryResultRequestDto input)
        {
            var clientQuery =
                from client in _logFormHistoryRepository.GetAll()

                select new LogFormHistoryDto
                {
                    Id = client.Id,
                    FormId = client.FormId,
                    Remarks = client.Remarks,
                    Status = client.Status,
                    //FormStartDate = client.FormStartDate,
                    //FormJson = client.FormJson,
                    //IsActive = client.IsActive,
                    //CreationDate = client.CreationDate,
                    //ModifiedDate = client.ModifiedDate,
                    //FormStatus = client.FormStatus,
                    //UpdatedBy = client.UpdatedBy,
                    //ApprovedBy = client.ApprovedBy,
                    //CheckedBy = client.CheckedBy,
                    //CreatedBy = client.CreatedBy,
                    //ApproveDateTime = client.ApproveDateTime
                };

            if (input.Id != null)
            {
                clientQuery = clientQuery.Where(x => x.Id == input.Id);
            }
            if (input.FormId != null)
            {
                clientQuery = clientQuery.Where(x => x.FormId == input.FormId);
            }
            if (input.Status != null)
            {
                clientQuery = clientQuery.Where(x => x.Status == input.Status);
            }
            //if (input.IsActive != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.IsActive == input.IsActive);
            //}
            //if (input.FormStatus != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.FormStatus == input.FormStatus);
            //}
            //if (input.ApproveDateTime != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.ApproveDateTime == input.ApproveDateTime);
            //}
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


        protected IQueryable<LogFormHistoryDto> ApplySorting(IQueryable<LogFormHistoryDto> query, PagedLogFormHistoryResultRequestDto input)
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
        protected IQueryable<LogFormHistoryDto> ApplyPaging(IQueryable<LogFormHistoryDto> query, PagedLogFormHistoryResultRequestDto input)
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
