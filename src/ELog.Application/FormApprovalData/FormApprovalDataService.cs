using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.FormApprovalData;
using ELog.Application.FormApprovalData.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Elog.Application.FormApprovalData
{
    [PMMSAuthorize]
    public class FormApprovalDataService : ApplicationService, IFormApprovalDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<FormApproval> _formApprovalRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }


        private readonly IMasterCommonRepository _masterCommonRepository;

        public FormApprovalDataService(IRepository<FormApproval> formApprovalRepository, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _formApprovalRepository = formApprovalRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
        }


        [PMMSAuthorize]
        public async Task<FormApprovalDataDto> CreateAsync(FormApprovalDataDto input)
        {
            var clientCreate = ObjectMapper.Map<FormApproval>(input);

            await _formApprovalRepository.InsertAsync(clientCreate);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<FormApprovalDataDto>(clientCreate);
        }


        //[PMMSAuthorize]
        ////  [PMMSAuthorize(Permissions = "ClientForms.View")]
        ////[PMMSAuthorize(Permissions = PMMSPermissionConst.ClientForms_SubModule + "." + PMMSPermissionConst.View)]

        //public async Task<FormApprovalDataDto> GetAsync(EntityDto<int> input)
        //{
        //    var entity = await _formApprovalRepository.GetAsync(input.Id);
        //    return ObjectMapper.Map<FormApprovalDataDto>(entity);
        //}


        [PMMSAuthorize]
        public async Task<PagedResultDto<FormApprovalDataDto>> GetAllAsync(PagedFormApprovalDataResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<FormApprovalDataDto>(
                totalCount,
                entities
            );
        }
        protected IQueryable<FormApprovalDataDto> CreateUserListFilteredQuery(PagedFormApprovalDataResultRequestDto input)
        {
            var clientQuery =
                from client in _formApprovalRepository.GetAll()

                select new FormApprovalDataDto
                {
                    Id = client.Id,
                    FormId = client.FormId,
                    Status = client.Status,
                    Remark = client.Remark,
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
            if (input.Remark != null)
            {
                clientQuery = clientQuery.Where(x => x.Remark == input.Remark);
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



        protected IQueryable<FormApprovalDataDto> ApplySorting(IQueryable<FormApprovalDataDto> query, PagedFormApprovalDataResultRequestDto input)
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
        protected IQueryable<FormApprovalDataDto> ApplyPaging(IQueryable<FormApprovalDataDto> query, PagedFormApprovalDataResultRequestDto input)
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
