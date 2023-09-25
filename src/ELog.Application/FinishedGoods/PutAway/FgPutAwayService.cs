using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.FinishedGoods.PutAway.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.FinishedGoods.PutAway
{
    [PMMSAuthorize]
    public class FgPutAwayService : ApplicationService, IFgPutAwayService
    {
        private readonly IRepository<FgPutAway> _putawayrepository;
        private readonly IRepository<FgPicking> _pickingrepository;
        private readonly IRepository<LocationMaster> _locationMasterrepository;
        private readonly IRepository<PlantMaster> _plantMasterrepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public FgPutAwayService(IRepository<FgPutAway> putawayrepository,
             IRepository<LocationMaster> locationMasterrepository,
              IRepository<FgPicking> pickingrepository,
              IRepository<PlantMaster> plantMasterrepository,
        IMasterCommonRepository masterCommonRepository)
        {
            _plantMasterrepository = plantMasterrepository;
            _putawayrepository = putawayrepository;
            _pickingrepository = pickingrepository;
            _locationMasterrepository = locationMasterrepository;
            _masterCommonRepository = masterCommonRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
        public async Task<FgPutAwayDto> CreateAsync(FgPutAwayDto input)
        {
            var putaway = ObjectMapper.Map<FgPutAway>(input);
            await _putawayrepository.InsertAsync(putaway);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<FgPutAwayDto>(putaway);
        }



        public async Task DeleteAsync(EntityDto<int> input)
        {
            var approvalUserModule = await _putawayrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _putawayrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<FgPutAwayListDto>> GetAllAsync(PagedFgPutAwayResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<FgPutAwayListDto>(
                totalCount,
                entities
            );
        }

        public async Task<FgPutAwayListDto> GetSuggestedLocationAsync(string ProductBatchNo)
        {

            var query = from put in _putawayrepository.GetAll()
                            //join pick in _pickingrepository.GetAll()
                            //on put.LocationBarcode equals pick.LocationBarcode into pc
                            //from picking in pc.DefaultIfEmpty()
                        where put.isActive == true && put.isPicked == false && put.IsDeleted == false && put.ProductBatchNo == ProductBatchNo
                        orderby put.CreationTime ascending
                        select new FgPutAwayListDto
                        {
                            Id = put.Id,
                            LocationId = put.LocationId,
                            PalletId = put.PalletId,
                            LocationBarcode = put.LocationBarcode,
                            PalletBarcode = put.PalletBarcode,
                            PalletCount = put.PalletCount,
                            isActive = put.isActive,
                            isPicked = put.isPicked,
                            HUCode = put.HUCode,
                            PlantId = put.PlantId,
                            ProductBatchNo = put.ProductBatchNo

                        };

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities.FirstOrDefault();
        }

        public async Task<PagedResultDto<FgPutAwayListDto>> GetListAsync()
        {

            var query = from put in _putawayrepository.GetAll()
                        join loc in _locationMasterrepository.GetAll()
                        on put.LocationId equals loc.Id into ps
                        from loc in ps.DefaultIfEmpty()

                        select new FgPutAwayListDto
                        {
                            Id = put.Id,
                            LocationId = put.LocationId,
                            PalletId = put.PalletId,
                            LocationBarcode = put.LocationBarcode,
                            PalletBarcode = put.PalletBarcode,
                            PalletCount = put.PalletCount,
                            isActive = put.isActive,
                            isPicked = put.isPicked,
                            HUCode = put.HUCode,
                            PlantId = put.PlantId,
                            ProductBatchNo = put.ProductBatchNo

                        };

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<FgPutAwayListDto>(
                totalCount,
                entities
            );
        }


        public async Task<PagedResultDto<FgPutAwayListDto>> GetPendingListAsync()
        {

            var query = from put in _putawayrepository.GetAll()
                        join loc in _locationMasterrepository.GetAll()
                        on put.LocationId equals loc.Id into ps
                        from loc in ps.DefaultIfEmpty()
                        where put.isActive == false
                        select new FgPutAwayListDto
                        {
                            Id = put.Id,
                            LocationId = put.LocationId,
                            PalletId = put.PalletId,
                            LocationBarcode = put.LocationBarcode,
                            PalletBarcode = put.PalletBarcode,
                            PalletCount = put.PalletCount,
                            isActive = put.isActive,
                            isPicked = put.isPicked,
                            HUCode = put.HUCode,
                            PlantId = put.PlantId,
                            ProductBatchNo = put.ProductBatchNo

                        };

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<FgPutAwayListDto>(
                totalCount,
                entities
            );
        }



        protected IQueryable<FgPutAwayListDto> CreateUserListFilteredQuery(PagedFgPutAwayResultRequestDto input)
        {
            var query = from put in _putawayrepository.GetAll()
                        join loc in _locationMasterrepository.GetAll()
                        on put.LocationId equals loc.Id into ps
                        from loc in ps.DefaultIfEmpty()
                        select new FgPutAwayListDto
                        {
                            Id = put.Id,
                            LocationId = put.LocationId,
                            PalletId = put.PalletId,
                            LocationBarcode = put.LocationBarcode,
                            PalletBarcode = put.PalletBarcode,
                            PalletCount = put.PalletCount,
                            isActive = put.isActive,
                            isPicked = put.isPicked,
                            HUCode = put.HUCode,
                            PlantId = put.PlantId,
                            ProductBatchNo = put.ProductBatchNo,
                        };


            if (input.LocationId != null)
            {
                query = query.Where(x => x.LocationId == input.LocationId);
            }
            if (input.PalletId != null)
            {
                query = query.Where(x => x.PalletId == input.PalletId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                query = query.Where(x => x.LocationBarcode.Contains(input.Keyword) || x.PalletBarcode.Contains(input.Keyword));
            }

            return query;
        }


        protected IQueryable<FgPutAwayListDto> ApplySorting(IQueryable<FgPutAwayListDto> query, PagedFgPutAwayResultRequestDto input)
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



        protected IQueryable<FgPutAwayListDto> ApplyPaging(IQueryable<FgPutAwayListDto> query, PagedFgPutAwayResultRequestDto input)
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

        public async Task<FgPutAwayDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _putawayrepository.GetAsync(input.Id);
            return ObjectMapper.Map<FgPutAwayDto>(entity);
        }

        public async Task<List<FgPutAwayDto>> CreateBulkAsync(List<FgPutAwayDto> input)
        {
            List<FgPutAwayDto> fgPutAwayDtos = new List<FgPutAwayDto>();
            foreach (var item in input)
            {
                var putaway = ObjectMapper.Map<FgPutAway>(item);
                await _putawayrepository.InsertAsync(putaway);
                CurrentUnitOfWork.SaveChanges();
                fgPutAwayDtos.Add(ObjectMapper.Map<FgPutAwayDto>(putaway));
            }
            return fgPutAwayDtos;
        }

        public async Task<FgPutAwayDto> UpdateAsync(FgPutAwayDto input)
        {
            var putaway = await _putawayrepository.GetAsync(input.Id);
            ObjectMapper.Map(input, putaway);
            await _putawayrepository.UpdateAsync(putaway);
            return await GetAsync(input);
        }




        [PMMSAuthorize(Permissions = PMMSPermissionConst.PutAway_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UnloadBinAsync(int putAwayId, Guid? transactionId)
        {
            var putAway = new List<FgPutAway>();
            if (transactionId != null)
            {
                putAway = await _putawayrepository.GetAllListAsync(x => x.Id.Equals(transactionId)).ConfigureAwait(false);
                putAway.ForEach(x => x.IsDeleted = true);
                await _masterCommonRepository.BulkUpdateFgPutAway(putAway);
            }
        }
    }
}
