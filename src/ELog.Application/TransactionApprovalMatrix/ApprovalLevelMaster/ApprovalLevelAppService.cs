using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.TransactionApprovalMatrix.ApprovalLevel.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalLevel
{

    public class ApprovalLevelAppService : ApplicationService, IApprovalLevelAppService
    {
        private readonly IRepository<ApprovalLevelMaster> _approvallevelrepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public ApprovalLevelAppService(IRepository<ApprovalLevelMaster> approvallevelrepository)

        {
            _approvallevelrepository = approvallevelrepository;

            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;


        }
        [PMMSAuthorize(Permissions = "ApprovalLevelMaster.View")]
        public async Task<ApprovalLevelDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _approvallevelrepository.GetAsync(input.Id);
            return ObjectMapper.Map<ApprovalLevelDto>(entity);
        }

        [PMMSAuthorize(Permissions = "ApprovalLevelMaster.View")]
        public async Task<PagedResultDto<ApprovalLevelListDto>> GetAllAsync(PagedApprovalLevelResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<ApprovalLevelListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = "ApprovalLevelMaster.Add")]
        public async Task<ApprovalLevelDto> CreateAsync(CreateApprovalLevelDto input)
        {
            if (await IsLevelPresent(input.LevelName))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.LavelAlreadyExist, input.LevelName));
            }

            var aprovellevel = ObjectMapper.Map<ApprovalLevelMaster>(input);
            //var currentDate = System.DateTime.UtcNow;
            await _approvallevelrepository.InsertAsync(aprovellevel);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<ApprovalLevelDto>(aprovellevel);
        }

        public async Task<bool> IsLevelPresent(string LevelName)
        {
            if (LevelName != null)
            {
                return await _approvallevelrepository.GetAll().AnyAsync(x => x.LevelName == LevelName);
            }


            return false;
        }

        [PMMSAuthorize(Permissions = "ApprovalLevelMaster.Edit")]
        public async Task<ApprovalLevelDto> UpdateAsync(ApprovalLevelDto input)
        {

            var approvallevel = await _approvallevelrepository.GetAsync(input.Id);

            ObjectMapper.Map(input, approvallevel);

            await _approvallevelrepository.UpdateAsync(approvallevel);

            return await GetAsync(input);
        }



        [PMMSAuthorize(Permissions = "ApprovalLevelMaster.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {


            var area = await _approvallevelrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _approvallevelrepository.DeleteAsync(area).ConfigureAwait(false);
        }



        protected IQueryable<ApprovalLevelListDto> ApplySorting(IQueryable<ApprovalLevelListDto> query, PagedApprovalLevelResultRequestDto input)
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
        protected IQueryable<ApprovalLevelListDto> ApplyPaging(IQueryable<ApprovalLevelListDto> query, PagedApprovalLevelResultRequestDto input)
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

        protected ApprovalLevelListDto MapToEntityDto(ApprovalLevelMaster entity)
        {
            return ObjectMapper.Map<ApprovalLevelListDto>(entity);
        }
        public async Task<PagedResultDto<ApprovalLevelListDto>> GetListAsync()
        {
            var query = from approvalLevel in _approvallevelrepository.GetAll()
                        where approvalLevel.IsDeleted == false && approvalLevel.IsActive == true
                        select new ApprovalLevelListDto
                        {
                            Id = approvalLevel.Id,
                            LevelCode = approvalLevel.LevelCode,
                            LevelName = approvalLevel.LevelName,
                            Description = approvalLevel.Description,
                            IsActive = approvalLevel.IsActive
                        };

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<ApprovalLevelListDto>(
                totalCount,
                entities
            );
        }

        protected IQueryable<ApprovalLevelListDto> CreateUserListFilteredQuery(PagedApprovalLevelResultRequestDto input)
        {
            var approvallevelQuery = from approvalLevel in _approvallevelrepository.GetAll()

                                     select new ApprovalLevelListDto
                                     {
                                         Id = approvalLevel.Id,
                                         LevelCode = approvalLevel.LevelCode,
                                         LevelName = approvalLevel.LevelName,
                                         Description = approvalLevel.Description,
                                         IsActive = approvalLevel.IsActive
                                     };

            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                approvallevelQuery = approvallevelQuery.Where(x =>
                 x.LevelName.Contains(input.Keyword));
            }
            if (!(input.LevelCode == 0))
            {
                approvallevelQuery = approvallevelQuery.Where(x => x.LevelCode == input.LevelCode);
            }
            if (!(string.IsNullOrEmpty(input.LevelName) || string.IsNullOrWhiteSpace(input.LevelName)))
            {
                approvallevelQuery = approvallevelQuery.Where(x => x.LevelName.Contains(input.LevelName));
            }

            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    approvallevelQuery = approvallevelQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    approvallevelQuery = approvallevelQuery.Where(x => x.IsActive);
                }
            }
            return approvallevelQuery;
        }
    }
}