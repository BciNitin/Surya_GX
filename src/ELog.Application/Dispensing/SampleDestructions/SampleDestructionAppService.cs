using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.SampleDestructions.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Dispensing.SampleDestructions
{
    [PMMSAuthorize]
    public class SampleDestructionAppService : ApplicationService, ISampleDestructionAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<InspectionLot> _inspectionLotRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnLabelPrintingBarcodeRepository;
        private readonly IRepository<SampleDestruction> _sampleDestructionRepository;
        private readonly string SamplingCompletedStatus = nameof(SamplingHeaderStatus.Completed).ToLower();

        #endregion fields

        #region constructor

        public SampleDestructionAppService(IHttpContextAccessor httpContextAccessor, IRepository<InspectionLot> inspectionLotRepository,
           IRepository<ProcessOrderMaterial> processOrderMaterialRepository, IDispensingAppService dispensingAppService, IRepository<DispensingHeader> dispensingHeaderRepository,
           IRepository<DispensingDetail> dispensingDetailRepository, IRepository<GRNMaterialLabelPrintingContainerBarcode> grnLabelPrintingBarcodeRepository,
           IRepository<SampleDestruction> sampleDestructionRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _inspectionLotRepository = inspectionLotRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _dispensingAppService = dispensingAppService;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _dispensingDetailRepository = dispensingDetailRepository;
            _grnLabelPrintingBarcodeRepository = grnLabelPrintingBarcodeRepository;
            _sampleDestructionRepository = sampleDestructionRepository;
        }

        #endregion constructor

        #region public

        /// <summary>
        /// Used for getting all sampling completed inspection lot numbers
        /// </summary>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetAllSampleInspectionLotNosAsync()
        {
            var responseDto = new HTTPResponseDto();
            var samplingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule,
               PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var inspectionLotQuery = from sampling in _dispensingHeaderRepository.GetAll()
                                     join inspectionLot in _inspectionLotRepository.GetAll()
                                     on sampling.InspectionLotId equals inspectionLot.Id
                                     where sampling.StatusId == samplingCompletedStatusId
                                      && sampling.IsSampling
                                     select new SelectListDtoWithPlantId
                                     {
                                         Id = inspectionLot.Id,
                                         Value = inspectionLot.InspectionLotNumber,
                                         PlantId = inspectionLot.PlantId
                                     };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                inspectionLotQuery = inspectionLotQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var inspectionLotList = await inspectionLotQuery.ToListAsync() ?? default;
            inspectionLotList = inspectionLotList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            if (inspectionLotList?.Count == 0)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoInspectionLotNoAvailable);
            }
            responseDto.ResultObject = inspectionLotList;
            return responseDto;
        }

        /// <summary>
        /// Used for getting all material codes under selected inspection lot
        /// </summary>
        /// <param name="inspectionLotId"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetMaterialByInspectionLotId(int inspectionLotId)
        {
            var today = DateTime.Now;
            var responseDto = new HTTPResponseDto();
            var samplingCompleted = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule, PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
            //todo: material should be approved.
            //Get sampling completed process order
            var materialSelectList = await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                            join dispensingHeader in _dispensingHeaderRepository.GetAll()
                                            on processOrderMaterial.ItemCode equals dispensingHeader.MaterialCodeId
                                            where processOrderMaterial.InspectionLotId == inspectionLotId && processOrderMaterial.ExpiryDate > today
                                            && processOrderMaterial.RetestDate > today && dispensingHeader.StatusId == samplingCompleted
                                            && dispensingHeader.IsSampling
                                            orderby processOrderMaterial.ExpiryDate ascending
                                            select new SelectListDto
                                            {
                                                Id = processOrderMaterial.ItemCode,
                                                Value = processOrderMaterial.ItemCode,
                                            })?.ToListAsync() ?? default;
            materialSelectList = materialSelectList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            if (materialSelectList?.Count == 0)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderInspectionLotNo);
            }
            responseDto.ResultObject = new { MaterialSelectList = materialSelectList };
            return responseDto;
        }

        /// <summary>
        /// Used for getting all sampling completed sap batch number under selected material code && inspection lot id.
        /// </summary>
        /// <param name="inspectionLotId"></param>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetSamplingSapBatchNumbersAsync(int? inspectionLotId, string materialCode)
        {
            var responseDto = new HTTPResponseDto();
            var samplingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule, PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
            var sapBatchNumberSelectList = await (from disepensingHeader in _dispensingHeaderRepository.GetAll()
                                                  join dispensingDetail in _dispensingDetailRepository.GetAll()
                                                  on disepensingHeader.Id equals dispensingDetail.DispensingHeaderId
                                                  where disepensingHeader.InspectionLotId == inspectionLotId
                                                  && disepensingHeader.StatusId == samplingCompletedStatusId && disepensingHeader.MaterialCodeId.ToLower() == materialCode.ToLower()
                                                  && disepensingHeader.IsSampling
                                                  select new
                                                  {
                                                      id = disepensingHeader.Id,
                                                      sapBatchNumber = dispensingDetail.SAPBatchNumber,
                                                  }).ToListAsync() ?? default;
            var sapbBatchNumberSelectList = new List<SelectListDto>();
            sapbBatchNumberSelectList = sapBatchNumberSelectList.GroupBy(x => x.sapBatchNumber).Select(x => new SelectListDto { Id = x.First().id, Value = x.First().sapBatchNumber }).ToList();
            responseDto.ResultObject = sapbBatchNumberSelectList;
            return responseDto;
        }

        /// <summary>
        /// Used for validating material container barcode.
        /// </summary>
        /// <param name="input"></param
        /// <returns></returns>
        public async Task<HTTPResponseDto> UpdateSampleDestructionByContainerBarcode(SampleDestructionDto input)
        {
            var responseDto = new HTTPResponseDto();
            var grnMaterialLabelPrintingContainerBarcode = await _grnLabelPrintingBarcodeRepository.GetAll().Where(x =>
            x.MaterialLabelContainerBarCode.ToLower() == input.MaterialContainerBarCode
            && x.BalanceQuantity > 0).FirstOrDefaultAsync();
            if (grnMaterialLabelPrintingContainerBarcode == null)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFound);
            }
            else
            {
                var alreadyScanDestructionContainerId = await (from sampleDestruction in _sampleDestructionRepository.GetAll()
                                                               where sampleDestruction.ContainerMaterialBarcode == input.MaterialContainerBarCode
                                                               && sampleDestruction.MaterialCode.ToLower() == input.MaterialCode.ToLower() && sampleDestruction.InspectionLotId == input.InspectionLotId
                                                               && sampleDestruction.SAPBatchNumber.ToLower() == input.SAPBatchNo.ToLower()
                                                               select sampleDestruction.Id).FirstOrDefaultAsync();
                if (alreadyScanDestructionContainerId > 0)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerAlreadyScanned);
                }
                var samplingCompletedContainerBarcodes = await (
                              from dispensingHeader in _dispensingHeaderRepository.GetAll()
                              join dispensingDeatil in _dispensingDetailRepository.GetAll()
                              on dispensingHeader.Id equals dispensingDeatil.DispensingHeaderId
                              where dispensingHeader.IsSampling && dispensingHeader.Id == input.MaterialSampleHeaderHeaderId
                              && dispensingDeatil.ContainerMaterialBarcode.ToLower() == grnMaterialLabelPrintingContainerBarcode.MaterialLabelContainerBarCode.ToLower()
                              select new
                              {
                                  containerBarcode = dispensingDeatil.ContainerMaterialBarcode,
                              }).FirstOrDefaultAsync();
                if (samplingCompletedContainerBarcodes == null)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NotSamplingContainer);
                }
                input.MaterialContainerId = grnMaterialLabelPrintingContainerBarcode.Id;
                input.MaterialContainerBalanceQuantity = grnMaterialLabelPrintingContainerBarcode.BalanceQuantity;
                input.MaterialContainerBarCode = samplingCompletedContainerBarcodes.containerBarcode;
            }
            responseDto.ResultObject = input;
            return responseDto;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SampleDestruction_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SampleDestruction_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> CreateAsync(SampleDestructionDto input)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            if (!await ValidateQuantity(input, responseDto))
            {
                return responseDto;
            }
            var sampleDestruction = ObjectMapper.Map<SampleDestruction>(input);
            sampleDestruction.TenantId = AbpSession.TenantId;
            await _sampleDestructionRepository.InsertAsync(sampleDestruction);
            //Update balance quantity from container barcode table
            var containerForBalanceQuantity = await _grnLabelPrintingBarcodeRepository.GetAsync(input.MaterialContainerId.Value);
            if (input.IsPackUOM)
            {
                containerForBalanceQuantity.BalanceQuantity -= input.ConvertedNoOfPack;
            }
            else
            {
                containerForBalanceQuantity.BalanceQuantity -= input.ConvertedNetWeight;
            }
            await _grnLabelPrintingBarcodeRepository.UpdateAsync(containerForBalanceQuantity);
            await CurrentUnitOfWork.SaveChangesAsync();
            responseDto.ResultObject = input;
            return responseDto;
        }

        #endregion public

        #region private

        /// <summary>
        /// Used for validating material container Quantity.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="responseDto"></param
        /// <returns></returns>
        private async Task<bool> ValidateQuantity(SampleDestructionDto input, HTTPResponseDto responseDto)
        {
            if (input.IsPackUOM)
            {
                var convertedPackQuantity = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, input.NoOfPacks.Value);
                input.ConvertedNoOfPack = convertedPackQuantity;
                if (convertedPackQuantity > input.MaterialContainerBalanceQuantity)
                {
                    //Not valid
                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoOfPackGreaterThanContainerBalanceQuantity);
                    return false;
                }
            }
            else
            {
                var convertedNetQuantity = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, input.NetWeight.Value);
                input.ConvertedNetWeight = convertedNetQuantity;
                if (convertedNetQuantity > input.MaterialContainerBalanceQuantity)
                {
                    //Not valid
                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NetWeightGreaterThanContainerBalanceQuantity);
                    return false;
                }
            }

            return true;
        }

        #endregion private
    }
}