using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Inward.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.CubicleAssignments.Dto;
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

namespace ELog.Application.WIP.CubicleAssignments
{

    [PMMSAuthorize]
    public class CubicleAssignmentsAppService : ApplicationService, ICubicleAssignmentsAppService
    {
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrderMaterialRepository;
        private readonly IRepository<CubicleAssignmentWIP> _cubicleAssignmentsRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<AreaUsageLog> _areaUsageLogRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<ELog.Core.Entities.EquipmentUsageLog> _equipmentUsageLogRepository;
        public CubicleAssignmentsAppService(IRepository<ProcessOrderAfterRelease> processOrderRepository,
            IRepository<ProcessOrderMaterialAfterRelease> processOrderMaterialRepository,
        IHttpContextAccessor httpContextAccessor,
        IRepository<CubicleAssignmentWIP> cubicleAssignmentsRepository,
        IRepository<CubicleMaster> cubicleRepository,
         IRepository<EquipmentMaster> equipmentRepository, IRepository<AreaUsageLog> areaUsageLogRepository,
         IRepository<ELog.Core.Entities.EquipmentUsageLog> equipmentUsageLogRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _processOrderRepository = processOrderRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _httpContextAccessor = httpContextAccessor;
            _cubicleAssignmentsRepository = cubicleAssignmentsRepository;
            _cubicleRepository = cubicleRepository;
            _equipmentRepository = equipmentRepository;
            _areaUsageLogRepository = areaUsageLogRepository;
            _equipmentUsageLogRepository = equipmentUsageLogRepository;

        }

        [PMMSAuthorize(Permissions = "CubicleAssignments.Add")]
        public async Task<CubicleAssignmentsDto> CreateAsync(CreateCubicleAssignmentsDto input)
        {
            var CubicleAssignment = ObjectMapper.Map<CubicleAssignmentWIP>(input);
            var currentDate = DateTime.UtcNow;
            CubicleAssignment.Status = true;
            await _cubicleAssignmentsRepository.InsertAsync(CubicleAssignment);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<CubicleAssignmentsDto>(CubicleAssignment);
        }

        [PMMSAuthorize(Permissions = "CubicleAssignments.View")]
        public async Task<CubicleAssignmentsDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _cubicleAssignmentsRepository.GetAsync(input.Id);
            var cubicleCode = "";
            var equipmentBarcode = "";
            if (entity.CubicleBarcodeId != 0)
            {
                var cubiclebarcode = await _cubicleRepository.GetAsync(entity.CubicleBarcodeId);
                cubicleCode = cubiclebarcode.CubicleCode;
            }
            if (entity.EquipmentBarcodeId != 0)
            {
                var equipmentcode = await _equipmentRepository.GetAsync(entity.EquipmentBarcodeId);
                equipmentBarcode = equipmentcode.EquipmentCode;
            }
            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrderMaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from material in areaps.DefaultIfEmpty()
                           where po.Id == entity.ProcessOrderId
                           select new CubicleAssignmentsDto
                           {
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = po.ProductCode,
                               BatchNo = material.SAPBatchNo,
                               LotNo = material.LotNo,

                           };
            var data = batchNos.FirstOrDefault();
            CubicleAssignmentsDto cubicleAssignmentsDto = new CubicleAssignmentsDto();
            cubicleAssignmentsDto.ProcessOrderNo = data.ProcessOrderNo;
            cubicleAssignmentsDto.ProductCode = data.ProductCode;
            cubicleAssignmentsDto.BatchNo = data.BatchNo;
            cubicleAssignmentsDto.LotNo = data.LotNo;
            cubicleAssignmentsDto.ProcessOrderId = entity.ProcessOrderId;
            cubicleAssignmentsDto.CubicleBarcode = cubicleCode;
            cubicleAssignmentsDto.EquipmentBarcode = equipmentBarcode;
            cubicleAssignmentsDto.Id = entity.Id;
            cubicleAssignmentsDto.EquipmentBarcodeId = entity.EquipmentBarcodeId;
            cubicleAssignmentsDto.CubicleBarcodeId = entity.CubicleBarcodeId;
            return ObjectMapper.Map<CubicleAssignmentsDto>(cubicleAssignmentsDto);

        }

        [PMMSAuthorize(Permissions = "CubicleAssignments.View")]
        public async Task<PagedResultDto<CubicleAssignmentsListDto>> GetAllAsync(PagedCubicleAssignmenstResultRequestDto input)
        {
            var query = CreateCubicleAssignmentsListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<CubicleAssignmentsListDto>(
                totalCount,
                entities
            );
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<CubicleAssignmentsListDto> ApplySorting(IQueryable<CubicleAssignmentsListDto> query, PagedCubicleAssignmenstResultRequestDto input)
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
        protected IQueryable<CubicleAssignmentsListDto> ApplyPaging(IQueryable<CubicleAssignmentsListDto> query, PagedCubicleAssignmenstResultRequestDto input)
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

        [PMMSAuthorize(Permissions = "CubicleAssignments.Edit")]
        public async Task<CubicleAssignmentsDto> UpdateAsync(CubicleAssignmentsDto input)
        {

            var CubicleAssignment = await _cubicleAssignmentsRepository.GetAsync(input.Id);
            CubicleAssignment.Status = true;
            input.Status = true;
            ObjectMapper.Map(input, CubicleAssignment);

            await _cubicleAssignmentsRepository.UpdateAsync(CubicleAssignment);

            return await GetAsync(input);
        }
        protected IQueryable<CubicleAssignmentsListDto> CreateCubicleAssignmentsListFilteredQuery(PagedCubicleAssignmenstResultRequestDto input)
        {
            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

            var cubicleAssignmentQuery = from detail in _cubicleAssignmentsRepository.GetAll()
                                         join po in _processOrderRepository.GetAll()
                                             on detail.ProcessOrderId equals po.Id into pos
                                         from po in pos.DefaultIfEmpty()
                                             //join poMaterial in _processOrderMaterialRepository.GetAll()
                                             //on po.Id equals poMaterial.ProcessOrderId into pomaterials
                                             //from poMaterial in pomaterials.DefaultIfEmpty()
                                         join cubicle in _cubicleRepository.GetAll()
                                         on detail.CubicleBarcodeId equals cubicle.Id into poms
                                         from cubicle in poms.DefaultIfEmpty()
                                         join equipment in _equipmentRepository.GetAll()
                                         on detail.EquipmentBarcodeId equals equipment.Id into pomss
                                         from equipment in pomss.DefaultIfEmpty()
                                         from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                         .Where(x => x.ProcessOrderId == po.Id).Take(1)
                                         select new CubicleAssignmentsListDto
                                         {
                                             Id = detail.Id,
                                             ProductId = po.ProductCodeId,
                                             ProductCode = po.ProductCode,
                                             ProcessOrderId = po.Id,
                                             ProcessOrderNo = po.ProcessOrderNo,
                                             CubicleBarcodeId = cubicle.Id,
                                             CubicleCode = cubicle.CubicleCode,
                                             EquipmentBarcodeId = equipment.Id,
                                             EquipmentCode = equipment.EquipmentCode,
                                             BatchNo = processOrderMaterial.ProductBatchNo,
                                             LotNo = processOrderMaterial.LotNo,
                                             Status = detail.Status

                                         };
            cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.Status == true);
            if (input.ProductId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.ProductId == input.ProductId);
            }
            if (input.ProcessOrderId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.ProcessOrderId == input.ProcessOrderId);
            }
            if (input.CubicleBarcodeId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.CubicleBarcodeId == input.CubicleBarcodeId);
            }
            if (input.EquipmentBarcodeId != null)
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.EquipmentBarcodeId == input.EquipmentBarcodeId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                cubicleAssignmentQuery = cubicleAssignmentQuery.Where(x => x.ProductCode.Contains(input.Keyword) || x.ProcessOrderNo.Contains(input.Keyword) || x.CubicleCode.Contains(input.Keyword) || x.EquipmentCode.Contains(input.Keyword));
            }
            return cubicleAssignmentQuery;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllProductCodeAsync(string input)
        {
            // var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var productCodes = from po in _processOrderRepository.GetAll()
                               select new SelectListDtoWithPlantId
                               {
                                   Id = po.Id,
                                   // PlantId = po.PlantId,
                                   Value = po.ProductCode,
                               };
            //if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            //{
            //    productCodes = productCodes.Where(x => x.PlantId == Convert.ToInt32(plantId));
            //}
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

        public async Task<CubicleAssignmentsDto> GetProcessMaterialDetailsbyProcessOrderAsync(int input)
        {


            //var batchNos =  _processAfterrepository.GetAll().Where(x=> x.Id == input).FirstOrDefault();

            var batchNos = from po in _processOrderRepository.GetAll()
                           join material in _processOrderMaterialRepository.GetAll()
                           on po.Id equals material.ProcessOrderId into areaps
                           from material in areaps.DefaultIfEmpty()
                           where po.Id == input
                           select new CubicleAssignmentsDto
                           {
                               ProcessOrderNo = po.ProcessOrderNo,
                               ProductCode = po.ProductCode,
                               BatchNo = material.SAPBatchNo,
                               LotNo = material.LotNo,
                           };
            var data = batchNos.FirstOrDefault();
            CubicleAssignmentsDto cubicleAssignmentsDto = new CubicleAssignmentsDto();
            cubicleAssignmentsDto.ProcessOrderNo = data.ProcessOrderNo;
            cubicleAssignmentsDto.ProductCode = data.ProductCode;
            cubicleAssignmentsDto.BatchNo = data.BatchNo;
            cubicleAssignmentsDto.LotNo = data.LotNo;

            return cubicleAssignmentsDto;
        }

        public async Task<BarcodeValidationDto> GetCubicleIsCleanedAsync(int cubicleId)
        {
            if (cubicleId != 0)
            {
                var barcodeValidationDto = new BarcodeValidationDto();
                var cubiclePresent = await _areaUsageLogRepository.GetAllListAsync(x => x.CubicalId == cubicleId);
                if (cubiclePresent.Any())
                {
                    var cubicleCleaned = await _areaUsageLogRepository.GetAllListAsync(x => x.CubicalId == cubicleId && x.StopTime != null && x.Status == true);
                    if (cubicleCleaned.Any())
                    {
                        barcodeValidationDto.IsValid = true;
                        return barcodeValidationDto;

                    }
                    else
                    {
                        barcodeValidationDto.IsValid = false;
                        return barcodeValidationDto;
                    }
                }
                else
                {
                    barcodeValidationDto.IsValid = true;
                    return barcodeValidationDto;
                }

            }
            return default;
        }

        public async Task<BarcodeValidationDto> GetEquipmentIsCleanedAsync(int equipmentId)
        {
            if (equipmentId != 0)
            {
                var barcodeValidationDto = new BarcodeValidationDto();
                var equipmentPresent = await _equipmentUsageLogRepository.GetAllListAsync(x => x.EquipmentBracodeId == equipmentId);
                if (equipmentPresent.Any())
                {
                    var equipmentCleaned = await _equipmentUsageLogRepository.GetAllListAsync(x => x.EquipmentBracodeId == equipmentId && x.EndTime != null && x.Status == true);
                    if (equipmentCleaned.Any())
                    {
                        barcodeValidationDto.IsValid = true;
                        return barcodeValidationDto;

                    }
                    else
                    {
                        barcodeValidationDto.IsValid = false;
                        return barcodeValidationDto;
                    }
                }
                else
                {
                    barcodeValidationDto.IsValid = true;
                    return barcodeValidationDto;
                }
            }
            return default;
        }
        public async Task<BarcodeValidationDto> CheckValidationForProcessOrder(CreateCubicleAssignmentsDto input)
        {

            var CubicleAssignment = ObjectMapper.Map<CubicleAssignmentWIP>(input);
            var currentDate = DateTime.UtcNow;
            var processorderAssignedwithCubicleAndEquipment = await _cubicleAssignmentsRepository.GetAllListAsync(x => x.ProcessOrderId == input.ProcessOrderId && x.CubicleBarcodeId == input.CubicleBarcodeId && x.EquipmentBarcodeId == input.EquipmentBarcodeId && x.Status == true);
            var processorderAssignedWithDiffCubicleFixedEquip = await (from cubicleAssignments in _cubicleAssignmentsRepository.GetAll().Where(x => x.ProcessOrderId == input.ProcessOrderId && x.EquipmentBarcodeId == input.EquipmentBarcodeId)
                                                                       join equipment in _equipmentRepository.GetAll()
                                                                       on cubicleAssignments.EquipmentBarcodeId equals equipment.Id
                                                                       select new CubicleAssignmentsDto
                                                                       {
                                                                           EquipmentBarcodeId = cubicleAssignments.EquipmentBarcodeId,
                                                                           CubicleBarcodeId = cubicleAssignments.CubicleBarcodeId,
                                                                           EquipmentType = equipment.IsPortable == true ? "Portable" : "Fixed",
                                                                           ProcessOrderId = cubicleAssignments.ProcessOrderId,
                                                                           Status = cubicleAssignments.Status
                                                                       }).ToListAsync() ?? default;

            var processorderWithFixedEquipment = processorderAssignedWithDiffCubicleFixedEquip.Where(x => x.EquipmentType == "Fixed" && x.Status == true);
            var result = processorderWithFixedEquipment.Select(x => x.CubicleBarcodeId != input.CubicleBarcodeId).ToList();

            var barcodeValidationDto = new BarcodeValidationDto();
            if (processorderAssignedwithCubicleAndEquipment.Any())
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.ProcessOrderAlreadyAssigned;
                return barcodeValidationDto;
            }
            else if (result.Any())
            {
                barcodeValidationDto.IsValid = false;
                barcodeValidationDto.ValidationMessage = PMMSValidationConst.ProcessOrderAssignedWithFixedEquipment;
                return barcodeValidationDto;
            }
            else
            {
                barcodeValidationDto.IsValid = true;
                return barcodeValidationDto;
            }
        }

    }
}
