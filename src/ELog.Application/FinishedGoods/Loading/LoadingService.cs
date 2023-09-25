using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.FinishedGoods.Picking.Dto;
using ELog.Application.FinishedGoods.SAP_FG.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.FinishedGoods.Picking
{
    [PMMSAuthorize]
    public class LoadingService : ApplicationService, ILoadingService
    {
        private readonly IRepository<Loading> _loadingrepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<OBDDetails> _obdDetails;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public LoadingService(IRepository<Loading> loadingrepository,

             IMasterCommonRepository masterCommonRepository, IRepository<OBDDetails> obdDetails)
        {
            _obdDetails = obdDetails;
            _loadingrepository = loadingrepository;

            _masterCommonRepository = masterCommonRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
        public async Task<LoadingDto> CreateAsync(LoadingDto input)
        {
            var loading = ObjectMapper.Map<Loading>(input);
            await _loadingrepository.InsertAsync(loading);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<LoadingDto>(loading);
        }

        public async Task<List<LoadingDto>> CreateBulkAsync(List<LoadingDto> input)
        {
            List<LoadingDto> LoadingDtos = new List<LoadingDto>();
            foreach (var item in input)
            {
                var loading = ObjectMapper.Map<Loading>(item);
                await _loadingrepository.InsertAsync(loading);
                CurrentUnitOfWork.SaveChanges();
                LoadingDtos.Add(ObjectMapper.Map<LoadingDto>(loading));
            }
            return LoadingDtos;
        }

        public async Task<int> GetOBDCodeIfAlreadyExist(string input)
        {
            return await _obdDetails.GetAll().Where(x => x.OBD.ToLower() == input.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<PagedResultDto<LoadingListDto>> GetPendingListAsync()
        {

            var query = from load in _loadingrepository.GetAll()
                        where load.isActive == false
                        select new LoadingListDto
                        {
                            Id = load.Id,
                            Batch = load.Batch,
                            Description = load.Description,
                            LineItem = load.LineItem,
                            UOM = load.UOM,
                            ProductCode = load.ProductCode,
                            NoOfPacks = load.NoOfPacks,
                            OBD = load.OBD,
                            ProductId = load.ProductId,
                            ProductName = load.ProductName,
                            Quantity = load.Quantity,
                            ProductBatchNo = load.ProductBatchNo,
                            PalletBarcode = load.PalletBarcode,
                            PalletCount = load.PalletCount,
                            CustomerAddress = load.CustomerAddress,
                            CustomerName = load.CustomerName,
                            isActive = load.isActive,
                            isPicked = load.isPicked,

                            TransportName = load.TransportName,
                            VehicleNo = load.VehicleNo,
                            PickingId = load.PickingId,
                            PutawayId = load.PutawayId,
                            HUCode = load.HUCode,
                            PlantId = load.PlantId

                        };


            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<LoadingListDto>(
                totalCount,
                entities
            );
        }


        public async Task DeleteAsync(EntityDto<int> input)
        {
            var approvalUserModule = await _loadingrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _loadingrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<LoadingListDto>> GetListAsync()
        {

            var query = from load in _loadingrepository.GetAll()
                        where load.IsDeleted == false && load.isActive == true
                        select new LoadingListDto
                        {
                            Id = load.Id,
                            Batch = load.Batch,
                            Description = load.Description,
                            LineItem = load.LineItem,
                            UOM = load.UOM,
                            ProductCode = load.ProductCode,
                            NoOfPacks = load.NoOfPacks,
                            OBD = load.OBD,
                            ProductId = load.ProductId,
                            ProductName = load.ProductName,
                            Quantity = load.Quantity,
                            ProductBatchNo = load.ProductBatchNo,
                            PalletBarcode = load.PalletBarcode,
                            PalletCount = load.PalletCount,
                            CustomerAddress = load.CustomerAddress,
                            CustomerName = load.CustomerName,
                            isActive = load.isActive,
                            isPicked = load.isPicked,
                            TransportName = load.TransportName,
                            VehicleNo = load.VehicleNo,
                            PickingId = load.PickingId,
                            PutawayId = load.PutawayId,
                            HUCode = load.HUCode,
                            PlantId = load.PlantId
                        };

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<LoadingListDto>(
                totalCount,
                entities
            );
        }

        public async Task<LoadingDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _loadingrepository.GetAsync(input.Id);
            return ObjectMapper.Map<LoadingDto>(entity);
        }

        public async Task<LoadingDto> UpdateAsync(LoadingDto input)
        {
            var putaway = await _loadingrepository.GetAsync(input.Id);
            ObjectMapper.Map(input, putaway);
            await _loadingrepository.UpdateAsync(putaway);
            return await GetAsync(input);
        }

        public async Task<OBDDetailDto> GetOBDDetailsAsync(string input)
        {
            var query = from obdData in _obdDetails.GetAll()
                        where obdData.OBD == input
                        select new OBDDetailDto
                        {

                            OBD = obdData.OBD,
                            CustomerName = obdData.CustomerName,
                            LineItemNo = obdData.LineItemNo,
                            ProductDesc = obdData.ProductDesc,
                            ARNo = obdData.ARNo,
                            CustomerAddress = obdData.CustomerAddress,
                            ProductBatchNo = obdData.ProductBatchNo,
                            ProductCode = obdData.ProductCode,
                            Qty = obdData.Qty,
                            SAPBatchNo = obdData.SAPBatchNo,
                            UOM = obdData.UOM
                        };
            return await query.FirstOrDefaultAsync() ?? default;
        }

        public async Task<List<OBDDetailDto>> GetOBDDetailsListAsync()
        {
            var query = from obdData in _obdDetails.GetAll()
                        select new OBDDetailDto
                        {

                            OBD = obdData.OBD,
                            CustomerName = obdData.CustomerName,
                            LineItemNo = obdData.LineItemNo,
                            ProductDesc = obdData.ProductDesc,
                            ARNo = obdData.ARNo,
                            CustomerAddress = obdData.CustomerAddress,
                            ProductBatchNo = obdData.ProductBatchNo,
                            ProductCode = obdData.ProductCode,
                            Qty = obdData.Qty,
                            SAPBatchNo = obdData.SAPBatchNo,
                            UOM = obdData.UOM
                        };
            return await query.ToListAsync() ?? default;
        }

        public async Task<PagedResultDto<LoadingListDto>> GetAllAsync(PagedLoadingResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<LoadingListDto>(
                totalCount,
                entities
            );
        }

        protected IQueryable<LoadingListDto> CreateUserListFilteredQuery(PagedLoadingResultRequestDto input)
        {
            var query = from load in _loadingrepository.GetAll()

                        select new LoadingListDto
                        {
                            Id = load.Id,
                            Batch = load.Batch,
                            Description = load.Description,
                            LineItem = load.LineItem,
                            UOM = load.UOM,
                            ProductCode = load.ProductCode,
                            NoOfPacks = load.NoOfPacks,
                            OBD = load.OBD,
                            ProductId = load.ProductId,
                            ProductName = load.ProductName,
                            Quantity = load.Quantity,
                            ProductBatchNo = load.ProductBatchNo,
                            PalletBarcode = load.PalletBarcode,
                            PalletCount = load.PalletCount,
                            CustomerAddress = load.CustomerAddress,
                            CustomerName = load.CustomerName,
                            isActive = load.isActive,
                            isPicked = load.isPicked,
                            TransportName = load.TransportName,
                            VehicleNo = load.VehicleNo,
                            PickingId = load.PickingId,
                            PutawayId = load.PutawayId,
                            HUCode = load.HUCode,
                            PlantId = load.PlantId
                        };


            if (input.OBD != null)
            {
                query = query.Where(x => x.OBD == input.OBD);
            }
            if (input.ProductBatchNo != null)
            {
                query = query.Where(x => x.ProductBatchNo == input.ProductBatchNo);
            }
            if (input.Batch != null)
            {
                query = query.Where(x => x.Batch == input.Batch);
            }
            if (input.isActive != null)
            {
                query = query.Where(x => x.isActive == input.isActive);
            }

            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                query = query.Where(x => x.OBD.Contains(input.Keyword) || x.ProductBatchNo.Contains(input.Keyword)
                || x.Batch.Contains(input.Keyword));
            }

            return query;
        }

        protected IQueryable<LoadingListDto> ApplySorting(IQueryable<LoadingListDto> query, PagedLoadingResultRequestDto input)
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

        protected IQueryable<LoadingListDto> ApplyPaging(IQueryable<LoadingListDto> query, PagedLoadingResultRequestDto input)
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
