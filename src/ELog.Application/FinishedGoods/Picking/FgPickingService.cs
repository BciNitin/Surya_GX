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
    public class FgPickingService : ApplicationService, IFgPickingService
    {
        private readonly IRepository<FgPicking> _pickingrepository;
        private readonly IRepository<LocationMaster> _locationMasterrepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<OBDDetails> _obdDetails;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public FgPickingService(IRepository<FgPicking> pickingrepository,
             IRepository<LocationMaster> locationMasterrepository,
             IMasterCommonRepository masterCommonRepository, IRepository<OBDDetails> obdDetails)
        {
            _obdDetails = obdDetails;
            _pickingrepository = pickingrepository;
            _locationMasterrepository = locationMasterrepository;
            _masterCommonRepository = masterCommonRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
        public async Task<FgPickingDto> CreateAsync(FgPickingDto input)
        {
            var picking = ObjectMapper.Map<FgPicking>(input);
            await _pickingrepository.InsertAsync(picking);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<FgPickingDto>(picking);
        }

        public async Task<List<FgPickingDto>> CreateBulkAsync(List<FgPickingDto> input)
        {
            List<FgPickingDto> fgPickingDtos = new List<FgPickingDto>();
            foreach (var item in input)
            {
                var picking = ObjectMapper.Map<FgPicking>(item);
                await _pickingrepository.InsertAsync(picking);
                CurrentUnitOfWork.SaveChanges();
                fgPickingDtos.Add(ObjectMapper.Map<FgPickingDto>(picking));
            }
            return fgPickingDtos;
        }

        public async Task<int> GetOBDCodeIfAlreadyExist(string input)
        {
            return await _obdDetails.GetAll().Where(x => x.OBD.ToLower() == input.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<FgPickingDto> GetPickingOBDDetailsIfAlreadyExist(string input)
        {
            var entity = await _pickingrepository.GetAll().Where(x => x.OBD.ToLower() == input.ToLower()).ToListAsync();
            var result = ObjectMapper.Map<FgPickingDto>(entity.FirstOrDefault());
            return result;
        }

        public async Task<PagedResultDto<FgPickingListDto>> GetPendingListAsync()
        {

            var query = from pick in _pickingrepository.GetAll()
                        where pick.isActive == false
                        select new FgPickingListDto
                        {
                            Batch = pick.Batch,
                            Description = pick.Description,
                            SuggestedLocationId = pick.SuggestedLocationId,
                            Quantity = pick.Quantity,
                            Id = pick.Id,
                            LineItem = pick.LineItem,
                            LocationId = pick.LocationId,
                            OBD = pick.OBD,
                            PalletBarcode = pick.PalletBarcode,
                            PalletCount = pick.PalletCount,
                            ProductCode = pick.ProductCode,
                            ProductBatchNo = pick.ProductBatchNo,
                            ProductId = pick.ProductId,
                            UOM = pick.UOM,
                            LocationBarcode = pick.LocationBarcode,
                            isPicked = pick.isPicked,
                            isActive = pick.isActive,
                            HUCode = pick.HUCode,
                            NoOfPacks = pick.NoOfPacks,
                            PlantId = pick.PlantId,
                            ProductName = pick.ProductName,
                            ShipperCount = pick.ShipperCount
                        };


            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<FgPickingListDto>(
                totalCount,
                entities
            );
        }


        public async Task DeleteAsync(EntityDto<int> input)
        {
            var approvalUserModule = await _pickingrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _pickingrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<FgPickingListDto>> GetListAsync()
        {

            var query = from pick in _pickingrepository.GetAll()
                        where pick.IsDeleted == false && pick.isActive == true
                        select new FgPickingListDto
                        {
                            Batch = pick.Batch,
                            Description = pick.Description,
                            SuggestedLocationId = pick.SuggestedLocationId,
                            Quantity = pick.Quantity,
                            Id = pick.Id,
                            LineItem = pick.LineItem,
                            LocationId = pick.LocationId,
                            OBD = pick.OBD,
                            PalletBarcode = pick.PalletBarcode,
                            PalletCount = pick.PalletCount,
                            ProductCode = pick.ProductCode,
                            ProductBatchNo = pick.ProductBatchNo,
                            ProductId = pick.ProductId,
                            UOM = pick.UOM,
                            LocationBarcode = pick.LocationBarcode,
                            isPicked = pick.isPicked,
                            isActive = pick.isActive,
                            HUCode = pick.HUCode,
                            NoOfPacks = pick.NoOfPacks,
                            PlantId = pick.PlantId,
                            ProductName = pick.ProductName,
                            ShipperCount = pick.ShipperCount
                        };

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<FgPickingListDto>(
                totalCount,
                entities
            );
        }

        public async Task<FgPickingDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _pickingrepository.GetAsync(input.Id);
            return ObjectMapper.Map<FgPickingDto>(entity);
        }

        public async Task<FgPickingDto> UpdateAsync(FgPickingDto input)
        {
            var putaway = await _pickingrepository.GetAsync(input.Id);
            ObjectMapper.Map(input, putaway);
            await _pickingrepository.UpdateAsync(putaway);
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

        public async Task<PagedResultDto<FgPickingListDto>> GetAllAsync(PagedFgPickingResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<FgPickingListDto>(
                totalCount,
                entities
            );
        }

        protected IQueryable<FgPickingListDto> CreateUserListFilteredQuery(PagedFgPickingResultRequestDto input)
        {
            var query = from pick in _pickingrepository.GetAll()

                        select new FgPickingListDto
                        {
                            Batch = pick.Batch,
                            Description = pick.Description,
                            SuggestedLocationId = pick.SuggestedLocationId,
                            Quantity = pick.Quantity,
                            Id = pick.Id,
                            LineItem = pick.LineItem,
                            LocationId = pick.LocationId,
                            OBD = pick.OBD,
                            PalletBarcode = pick.PalletBarcode,
                            PalletCount = pick.PalletCount,
                            ProductCode = pick.ProductCode,
                            ProductBatchNo = pick.ProductBatchNo,
                            ProductId = pick.ProductId,
                            UOM = pick.UOM,
                            LocationBarcode = pick.LocationBarcode,
                            isPicked = pick.isPicked,
                            isActive = pick.isActive,
                            HUCode = pick.HUCode,
                            NoOfPacks = pick.NoOfPacks,
                            PlantId = pick.PlantId,
                            ProductName = pick.ProductName,
                            ShipperCount = pick.ShipperCount
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

        protected IQueryable<FgPickingListDto> ApplySorting(IQueryable<FgPickingListDto> query, PagedFgPickingResultRequestDto input)
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

        protected IQueryable<FgPickingListDto> ApplyPaging(IQueryable<FgPickingListDto> query, PagedFgPickingResultRequestDto input)
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
