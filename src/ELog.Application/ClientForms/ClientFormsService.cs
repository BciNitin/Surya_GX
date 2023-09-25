using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Elog.Application.ClientForms.Dto;
//using PMMS.Application.Masters.HandlingUnits.Dto;
using Elog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Elog.Application.ClientForms
{
    /* [PMMSAuthorize]*/
    public class ClientFormsService : ApplicationService, IClientFormsService
    {
        private readonly IRepository<ClientForm> _clientFormRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IMasterCommonRepository _masterCommonRepository;

        public ClientFormsService(IRepository<ClientForm> clientFormRepository, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _clientFormRepository = clientFormRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
        }

        //        [PMMSAuthorize(Permissions = "ClientForms.Add")]


        /*  [PMMSAuthorize]*/
        public async Task<ClientFormsDto> CreateAsync(ClientFormsDto input)
        {

            var clientCreate = ObjectMapper.Map<ClientForm>(input);

            await _clientFormRepository.InsertAsync(clientCreate);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<ClientFormsDto>(clientCreate);
        }


        /*  [PMMSAuthorize]*/
        public async Task<ClientFormsDto> UpdateAsync(ClientFormsDto input)
        {
            var clientUpdate = await _clientFormRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, clientUpdate);

            await _clientFormRepository.UpdateAsync(clientUpdate);

            return await GetAsync(input);
        }


        /*
                [PMMSAuthorize]*/
        //  [PMMSAuthorize(Permissions = "ClientForms.View")]
        //[PMMSAuthorize(Permissions = PMMSPermissionConst.ClientForms_SubModule + "." + PMMSPermissionConst.View)]

        public async Task<ClientFormsDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _clientFormRepository.GetAsync(input.Id);
            return ObjectMapper.Map<ClientFormsDto>(entity);
        }


        /*  [PMMSAuthorize]*/
        public async Task<PagedResultDto<ClientFormsDto>> GetAllAsync(PagedClientFormsResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<ClientFormsDto>(
                totalCount,
                entities
            );
        }
        protected IQueryable<ClientFormsDto> CreateUserListFilteredQuery(PagedClientFormsResultRequestDto input)
        {
            var clientQuery =
                from client in _clientFormRepository.GetAll()

                select new ClientFormsDto
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
                    ApproveDateTime = client.ApproveDateTime,
                    MenuId = client.MenuId,
                    Permissions = client.Permissions
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
            if (input.IsActive != null)
            {
                clientQuery = clientQuery.Where(x => x.IsActive == input.IsActive);
            }
            if (input.ApproveDateTime != null)
            {
                clientQuery = clientQuery.Where(x => x.ApproveDateTime == input.ApproveDateTime);
            }
            if (input.FormStatus != null)
            {
                clientQuery = clientQuery.Where(x => x.FormStatus == input.FormStatus);
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



        protected IQueryable<ClientFormsDto> ApplySorting(IQueryable<ClientFormsDto> query, PagedClientFormsResultRequestDto input)
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
        protected IQueryable<ClientFormsDto> ApplyPaging(IQueryable<ClientFormsDto> query, PagedClientFormsResultRequestDto input)
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
