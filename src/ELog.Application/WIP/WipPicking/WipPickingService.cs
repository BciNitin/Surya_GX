using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.WipPicking.Dto;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.WIP.WipPicking
{
    public class WipPickingService : ApplicationService, IWipPickingService
    {
        private readonly IRepository<PickingMaster> _pickingMasterrepository;
        private readonly IRepository<ProcessOrderAfterRelease> _processAfterrepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processMaterialAfterrepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WipPickingService(IRepository<PickingMaster> pickingMasterrepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<ProcessOrderAfterRelease> processAfterrepository,
            IRepository<ProcessOrderMaterialAfterRelease> processMaterialAfterrepository
            )
        {
            _pickingMasterrepository = pickingMasterrepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _processAfterrepository = processAfterrepository;
            _processMaterialAfterrepository = processMaterialAfterrepository;
        }
        public async Task<WipPickingDto> CreateAsync(CreateWipPickingDto input)
        {
            int productId = await _processAfterrepository.GetAll().Where(x => x.ProductCode == input.ProductName).Select(x => x.ProductCodeId).FirstOrDefaultAsync();
            input.ProductId = productId;
            var picking = ObjectMapper.Map<PickingMaster>(input);
            await _pickingMasterrepository.InsertAsync(picking);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<WipPickingDto>(picking);
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
            var approvalUserModule = await _pickingMasterrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _pickingMasterrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<WipPickingListDto>> GetAllAsync(PagedWipPickingResultRequestDto input)
        {
            var query = CreatePickingFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<WipPickingListDto>(
                totalCount,
                entities
            );
        }
        protected IQueryable<WipPickingListDto> CreatePickingFilteredQuery(PagedWipPickingResultRequestDto input)
        {
            var PackingQuery = (from pac in _pickingMasterrepository.GetAll()
                                join po in _processAfterrepository.GetAll()
                                on pac.ProcessOrderId equals po.Id into pos
                                from po in pos.DefaultIfEmpty()
                                join poMaterial in _processMaterialAfterrepository.GetAll()
                                on po.Id equals poMaterial.ProcessOrderId into pomaterials
                                from poMaterial in pomaterials.DefaultIfEmpty()

                                select new WipPickingListDto
                                {
                                    Id = pac.Id,
                                    ProductId = pac.ProductId,
                                    ProductCode = po.ProductCode,
                                    ProcessOrderId = pac.ProcessOrderId,
                                    ProcessOrder = po.ProcessOrderNo,
                                    ProductName = poMaterial.MaterialDescription,
                                    Stage = pac.Stage,
                                    SuggestedLocationId = pac.SuggestedLocationId,
                                    ContainerCount = pac.ContainerCount,
                                    ContainerId = pac.ContainerId,
                                    Quantity = pac.Quantity


                                }).Distinct();
            if (input.ProcessOrderId != null)
            {
                PackingQuery = PackingQuery.Where(x => x.ProcessOrderId == input.ProcessOrderId);
            }

            if (input.ProductId != null)
            {
                PackingQuery = PackingQuery.Where(x => x.ProductCode == input.ProductId);
            }

            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                PackingQuery = PackingQuery.Where(x =>
                x.ContainerCode.Contains(input.Keyword));
            }

            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    PackingQuery = PackingQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    PackingQuery = PackingQuery.Where(x => x.IsActive);
                }
            }
            return PackingQuery;
        }

        protected IQueryable<WipPickingListDto> ApplySorting(IQueryable<WipPickingListDto> query, PagedWipPickingResultRequestDto input)
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

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<WipPickingListDto> ApplyPaging(IQueryable<WipPickingListDto> query, PagedWipPickingResultRequestDto input)
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

        public async Task<WipPickingDto> GetAsync(EntityDto<int> input)
        {
            //var entity = await _pickingMasterrepository.GetAsync(input.Id);

            var entity = (from pac in _pickingMasterrepository.GetAll()
                          join po in _processAfterrepository.GetAll()
                          on pac.ProcessOrderId equals po.Id into pos
                          from po in pos.DefaultIfEmpty()
                          where pac.Id == input.Id
                          select new WipPickingDto
                          {
                              Id = pac.Id,
                              ProductId = pac.ProductId,
                              ProductCode = po.ProductCode,
                              ProcessOrderId = pac.ProcessOrderId,
                              ProcessOrder = po.ProcessOrderNo,
                              Stage = pac.Stage,
                              SuggestedLocationId = pac.SuggestedLocationId,
                              ContainerCount = pac.ContainerCount,
                              ContainerId = pac.ContainerId,
                              Quantity = pac.Quantity,



                          });

            //return ObjectMapper.Map<WipPickingDto>(entity);
            var entities = await AsyncQueryableExecuter.FirstOrDefaultAsync(entity);
            return entities;
        }

        public async Task<WipPickingDto> UpdateAsync(WipPickingDto input)
        {
            var putaway = await _pickingMasterrepository.GetAsync(input.Id);
            ObjectMapper.Map(input, putaway);
            await _pickingMasterrepository.UpdateAsync(putaway);
            return await GetAsync(input);
        }


        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = from po in _processAfterrepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   //   Id = po.Id,
                                   PlantId = 2,
                                   Value = po.ProductCode,
                               };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                productCodes = productCodes.Where(x => x.Value.Contains(input)).Distinct();
                return await productCodes?.ToListAsync() ?? default;
            }
            return await productCodes.ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(string input)
        {
            if ((input != null) || (input == null))
            {
                var processOrders = await _processAfterrepository.GetAll().Where(a => a.ProductCode == input).Select(po => new SelectListDto
                {
                    Id = po.Id,
                    Value = po.ProcessOrderNo,
                }).ToListAsync();
                return processOrders;
            }

            return default;
        }

        public async Task<WipPickingDto> GetBatchNosStageAsync(int input)
        {


            //var batchNos =  _processAfterrepository.GetAll().Where(x=> x.Id == input).FirstOrDefault();

            var batchNos = from po in _processAfterrepository.GetAll()
                           join material in _processMaterialAfterrepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from material in areaps.DefaultIfEmpty()
                           where po.Id == input
                           select new WipPickingDto
                           {
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = po.ProductCode,
                               Stage = material.CurrentStage,
                               Batch = material.ProductBatchNo,
                               ProductName = material.MaterialDescription,
                           };
            var data = batchNos.FirstOrDefault();
            WipPickingDto wipPickingDto = new WipPickingDto();
            wipPickingDto.ProcessOrderNo = data.ProcessOrderNo;
            wipPickingDto.ProductCode = data.ProductCode;
            wipPickingDto.Stage = data.Stage;
            wipPickingDto.Batch = data.Batch;
            wipPickingDto.ProductName = data.ProductName;

            return wipPickingDto;
        }

    }
}
