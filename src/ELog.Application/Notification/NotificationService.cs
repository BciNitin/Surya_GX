using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.Notification.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.Notification
{
    [PMMSAuthorize]
    public class NotificationService : ApplicationService, INotificationService
    {
        private IRepository<Notifications> _notificationRepository;
        private NullAsyncQueryableExecuter AsyncQueryableExecuter;
        private IHttpContextAccessor _httpContextAccessor;
        private IMasterCommonRepository _masterCommonRepository;

        public NotificationService(IRepository<Notifications> notificationRepository, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
        }


        [PMMSAuthorize]
        public async Task<NotificationDto> CreateAsync(NotificationDto input)
        {
            var clientCreate = ObjectMapper.Map<Notifications>(input);

            await _notificationRepository.InsertAsync(clientCreate);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<NotificationDto>(clientCreate);
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
        public async Task<PagedResultDto<NotificationDto>> GetAllAsync(PagedNotificationResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);


            return new PagedResultDto<NotificationDto>(
                totalCount,
                entities
            );
        }
        protected IQueryable<NotificationDto> CreateUserListFilteredQuery(PagedNotificationResultRequestDto input)
        {
            var clientQuery =
                from notice in _notificationRepository.GetAll()

                select new NotificationDto
                {
                    Id = notice.Id,
                    notification_type = notice.notification_type,
                    assign_roles = notice.assign_roles,
                    log_Id = notice.log_Id,
                    assign_email = notice.assign_email,
                    assign_mobile = notice.assign_mobile,
                    isActive = notice.isActive,
                    Repeat = notice.Repeat,
                    CreatedOn = notice.CreatedOn,
                    ModifiedOn = notice.ModifiedOn
                };


            //if (input.Id != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.Id == input.Id);
            //}
            //if (input.notification_type != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.notification_type == input.notification_type);
            //}

            //if (input.assign_roles != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.assign_roles == input.assign_roles);
            //}
            //if (input.log_Id != null)
            //{
            //    clientQuery = clientQuery.Where(x => x.log_Id == input.log_Id);
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



        protected IQueryable<NotificationDto> ApplySorting(IQueryable<NotificationDto> query, PagedNotificationResultRequestDto input)
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
        protected IQueryable<NotificationDto> ApplyPaging(IQueryable<NotificationDto> query, PagedNotificationResultRequestDto input)
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
