using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.MaterialVerification.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.MaterialVerification
{
    [PMMSAuthorize]
    public class MaterialVerificationService : ApplicationService, IMaterialVerificationService
    {
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrderMaterialRepository;
        private readonly IRepository<WIPMaterialVerification> _materialverificationRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<CageLabelPrinting> _cageLabelPrintingRepository;
        private readonly IRepository<MaterialMaster> _materialMasterRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MaterialVerificationService(IRepository<ProcessOrderAfterRelease> processOrderRepository,
          IRepository<ProcessOrderMaterialAfterRelease> processOrderMaterialRepository,
      IHttpContextAccessor httpContextAccessor,
      IRepository<WIPMaterialVerification> materialverificationRepository,
      IRepository<CubicleMaster> cubicleRepository,
       IRepository<CageLabelPrinting> cageLabelPrintingRepository, IRepository<MaterialMaster> materialMasterRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _processOrderRepository = processOrderRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _httpContextAccessor = httpContextAccessor;
            _materialverificationRepository = materialverificationRepository;
            _cubicleRepository = cubicleRepository;
            _cageLabelPrintingRepository = cageLabelPrintingRepository;
            _materialMasterRepository = materialMasterRepository;

        }

        [PMMSAuthorize(Permissions = "WIPMaterialVerification.Add")]
        public async Task<MaterialVerificationDto> CreateAsync(CreateMaterialVerificationDto input)
        {
            if (await IsMaterialVerificationPresent(input.ProductID, input.CageBarcodeId))
            {
                throw new Abp.UI.UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.MaterialVerificationAlreadyExist, input.ProductCode));
            }
            var materialVerification = ObjectMapper.Map<WIPMaterialVerification>(input);
            var currentDate = DateTime.UtcNow;
            await _materialverificationRepository.InsertAsync(materialVerification);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<MaterialVerificationDto>(materialVerification);
        }

        public async Task<bool> IsMaterialVerificationPresent(int? ProductID, int? CageBarcodeId)
        {

            if (ProductID != null)
            {
                return await _materialverificationRepository.GetAll().AnyAsync(x => x.ProductId == ProductID);
            }

            return false;
        }

        [PMMSAuthorize(Permissions = "WIPMaterialVerification.View")]
        public async Task<MaterialVerificationDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _materialverificationRepository.GetAsync(input.Id);
            var cubiclebarcode = await _cubicleRepository.GetAsync(entity.CubicleId);
            var cageBarcode = await _cageLabelPrintingRepository.GetAsync(entity.CageBarcodeId);

            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrderMaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from material in areaps.DefaultIfEmpty()
                           where po.Id == entity.ProcessOrderId
                           join materialmom in _materialMasterRepository.GetAll()
                           on po.ProductCodeId equals materialmom.Id into materialmoms
                           from materialmom in materialmoms.DefaultIfEmpty()
                           select new MaterialVerificationDto
                           {
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = po.ProductCode,
                               BatchNo = material.SAPBatchNo,
                               MaterialCode = material.MaterialCode,
                               ProductName = materialmom.MaterialDescription,
                               ARNo = material.ARNO,
                               UOM = material.UOM,

                           };
            var data = batchNos.FirstOrDefault();
            MaterialVerificationDto materialVerificationDto = new MaterialVerificationDto();
            materialVerificationDto.ProcessOrderNo = data.ProcessOrderNo;
            materialVerificationDto.ProductCode = data.ProductCode;
            materialVerificationDto.ProductName = data.ProductCode;
            materialVerificationDto.BatchNo = data.BatchNo;
            materialVerificationDto.ARNo = data.ARNo;
            materialVerificationDto.ProcessOrderId = entity.ProcessOrderId;
            materialVerificationDto.CubicleBarcode = cubiclebarcode.CubicleCode;
            materialVerificationDto.CageBarcode = cageBarcode.CageLabelBarcode;
            materialVerificationDto.Id = entity.Id;
            materialVerificationDto.CageBarcodeId = entity.CageBarcodeId;
            materialVerificationDto.CubicleBarcodeId = entity.CubicleId;
            return ObjectMapper.Map<MaterialVerificationDto>(materialVerificationDto);
            //return ObjectMapper.Map<CubicleAssignmentsDto>(entity);
        }
        [PMMSAuthorize(Permissions = "WIPMaterialVerification.View")]
        public async Task<PagedResultDto<MaterialVerificationListDto>> GetAllAsync(PagedMaterialVerificationResultRequestDto input)
        {
            var query = CreateMaterialVerificationListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<MaterialVerificationListDto>(
                totalCount,
                entities
            );
        }
        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<MaterialVerificationListDto> ApplySorting(IQueryable<MaterialVerificationListDto> query, PagedMaterialVerificationResultRequestDto input)
        {
            //Try to sort query if available
            ISortedResultRequest sortInput = input;
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
        protected IQueryable<MaterialVerificationListDto> ApplyPaging(IQueryable<MaterialVerificationListDto> query, PagedMaterialVerificationResultRequestDto input)
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
        [PMMSAuthorize(Permissions = "WIPMaterialVerification.Edit")]
        public async Task<MaterialVerificationDto> UpdateAsync(MaterialVerificationDto input)
        {

            var materialVerification = await _materialverificationRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, materialVerification);

            await _materialverificationRepository.UpdateAsync(materialVerification);

            return await GetAsync(input);
        }

        protected IQueryable<MaterialVerificationListDto> CreateMaterialVerificationListFilteredQuery(PagedMaterialVerificationResultRequestDto input)
        {
            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

            var materialverificationQuery = from detail in _materialverificationRepository.GetAll()
                                            join po in _processOrderRepository.GetAll()
                                                on detail.ProcessOrderId equals po.Id into pos
                                            from po in pos.DefaultIfEmpty()
                                            join poMaterial in _processOrderMaterialRepository.GetAll()
                                            on po.Id equals poMaterial.ProcessOrderId into pomaterials
                                            from poMaterial in pomaterials.DefaultIfEmpty()
                                            join cubicle in _cubicleRepository.GetAll()
                                            on detail.CubicleId equals cubicle.Id into poms
                                            from cubicle in poms.DefaultIfEmpty()
                                            join cage in _cageLabelPrintingRepository.GetAll()
                                            on detail.CageBarcode equals cage.CageLabelBarcode into cags
                                            from cage in cags.DefaultIfEmpty()
                                            select new MaterialVerificationListDto
                                            {
                                                Id = detail.Id,
                                                ProductID = po.ProductCodeId,
                                                ProductCode = po.ProductCode,
                                                ProcessOrderId = po.Id,
                                                ProcessOrderNo = po.ProcessOrderNo,
                                                CubicleBarcodeId = cubicle.Id,
                                                CubicleBarcode = cubicle.CubicleCode,
                                                CageBarcodeId = cage.Id,
                                                CageBarcode = cage.CageLabelBarcode,
                                                NoOfCage = Convert.ToInt32(cage.NoOfContainer),
                                                BatchNo = poMaterial.SAPBatchNo,
                                                UOM = poMaterial.UOM,
                                                MaterialCode = poMaterial.MaterialCode,
                                                ARNo = poMaterial.ARNO,
                                                CubicleId = detail.CubicleId

                                            };
            if (input.ProductID != null)
            {
                materialverificationQuery = materialverificationQuery.Where(x => x.ProductID == input.ProductID);
            }
            if (input.ProcessOrderId != null)
            {
                materialverificationQuery = materialverificationQuery.Where(x => x.ProcessOrderId == input.ProcessOrderId);
            }
            if (input.CubicleBarcodeId != null)
            {
                materialverificationQuery = materialverificationQuery.Where(x => x.CubicleBarcodeId == input.CubicleBarcodeId);
            }
            //if (input.EquipmentBarcodeId != null)
            //{
            //    cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.EquipmentBarcodeId == input.EquipmentBarcodeId);
            //}
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                materialverificationQuery = materialverificationQuery.Where(x => x.ProductCode.Contains(input.Keyword)
                || x.ProcessOrderNo.Contains(input.Keyword)
                || x.CubicleBarcode.Contains(input.Keyword)
                || x.MaterialCode.Contains(input.Keyword)
                || x.CageBarcode.Contains(input.Keyword)
                || x.BatchNo.Contains(input.Keyword)
                || x.UOM.Contains(input.Keyword)
                 || x.ARNo.Contains(input.Keyword)
                );
            }
            return materialverificationQuery;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {
            var productCodes = from po in _processOrderRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   Id = po.ProductCodeId,
                                   PlantId = po.PlantId,
                                   Value = po.ProductCode,
                               };

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
                var processOrders = await _processOrderRepository.GetAll().Select(po => new SelectListDto
                {
                    Id = po.Id,
                    Value = po.ProcessOrderNo,
                }).ToListAsync();
                return processOrders;
            }

            return default;
        }

        public async Task<List<ProcessOrderMaterialAfterRelease>> GetMaterialDetailsAsync(int input)
        {
            var processOrders = await _processOrderMaterialRepository.GetAll().Select(po => new ProcessOrderMaterialAfterRelease
            {
                Id = po.Id,
                ProcessOrderId = po.ProcessOrderId,
                ARNO = po.ARNO,
                SAPBatchNo = po.SAPBatchNo,
                MaterialCode = po.MaterialCode,
                UOM = po.UOM
            }).Where(x => x.ProcessOrderId == input).ToListAsync();
            return processOrders;
        }

        public async Task<MaterialVerificationDto> GetProcessMaterialDetailsbyProcessOrderAsync(int input)
        {


            //var batchNos =  _processAfterrepository.GetAll().Where(x=> x.Id == input).FirstOrDefault();

            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrderMaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from material in areaps.DefaultIfEmpty()
                           where po.Id == input
                           select new MaterialVerificationDto
                           {
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = po.ProductCode,
                               BatchNo = material.SAPBatchNo,
                               ARNo = material.ARNO,
                           };
            var data = batchNos.FirstOrDefault();
            MaterialVerificationDto materialVerificationDto = new MaterialVerificationDto();
            materialVerificationDto.ProcessOrderNo = data.ProcessOrderNo;
            materialVerificationDto.ProductCode = data.ProductCode;
            materialVerificationDto.BatchNo = data.BatchNo;
            materialVerificationDto.ARNo = data.ARNo;

            return materialVerificationDto;
        }

        [PMMSAuthorize(Permissions = "WIPMaterialVerification.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var activity = await _materialverificationRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _materialverificationRepository.DeleteAsync(activity).ConfigureAwait(false);
        }

        public async Task<List<MaterialVerificationDto>> GetAllCageBarcodeAsync(string input)
        {
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {

                var cageLabelQuery = from cageLabel in _cageLabelPrintingRepository.GetAll()
                                     where cageLabel.CageLabelBarcode.ToLower() == input.ToLower()
                                     select new MaterialVerificationDto
                                     {
                                         CageBarcodeId = cageLabel.Id,
                                         CageBarcode = cageLabel.CageLabelBarcode,
                                         NoOfCage = Convert.ToInt32(cageLabel.NoOfContainer)
                                     };

                return await cageLabelQuery.ToListAsync() ?? default;
            }
            return default;
        }
    }
}
