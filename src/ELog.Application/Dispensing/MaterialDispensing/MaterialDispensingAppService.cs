using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.MaterialDispensing.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Sessions;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.Core.Printer;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using ELog.HardwareConnectorFactory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Dispensing.MaterialDispensing
{
    [PMMSAuthorize]
    public class MaterialDispensingAppService : ApplicationService, IMaterialDispensingAppService
    {
        #region fields
        private const string formatNumber = "FDSTG/F/013,Version:01";
        private const string sopNo = "FDSOP/GHT/ST/018";
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<ProcessOrder> _processOrderRepository;
        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignmentHeaderRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignmentDetailRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<EquipmentAssignment> _equipmentAssignmentRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<MaterialBatchDispensingHeader> _materialBatchDispensingHeaderRepository;
        private readonly IRepository<MaterialBatchDispensingContainerDetail> _materialBatchDispensingContainerDetailRepository;
        private readonly IRepository<MaterialMaster> _materialMasterRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementMasterRepository;
        private readonly IRepository<WMCalibratedLatestMachineDetail> _wmcalibratedMachineRepository;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
        private readonly IRepository<UnitOfMeasurementTypeMaster> _unitOfMeasurementTypeMasterRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnLabelPrintingBarcodeRepository;
        private readonly IRepository<DispensingPrintDetail> _dispensingPrintDetailAppservice;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly MasterCommonRepository _masterCommonRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IRepository<User, long> _userRepository;
        private readonly PrinterFactory _printerFactory;
        private readonly IRepository<DeviceMaster> _deviceRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly int StagingMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.Staging;
        private readonly string StagingCompletedStatus = nameof(StageHeaderStatus.Completed).ToLower();
        private readonly string DispensingInProgressStatus = nameof(DispensingHeaderStatus.InProgress).ToLower();
        private readonly string DispensingCompletedStatus = nameof(DispensingHeaderStatus.Completed).ToLower();
        private readonly string PackUOMType = nameof(UOMType.Pack).ToLower();

        #endregion fields

        #region constructor

        public MaterialDispensingAppService(IHttpContextAccessor httpContextAccessor, ISessionAppService sessionAppService,
            IRepository<ProcessOrder> processOrderRepository, IRepository<CubicleAssignmentHeader> cubicleAssignmentHeaderRepository,
            IRepository<ProcessOrderMaterial> processOrderMaterialRepository, IRepository<EquipmentMaster> equipmentRepository, IDispensingAppService dispensingAppService,
            IRepository<EquipmentAssignment> equipmentAssignmentRepository, IRepository<CubicleAssignmentDetail> cubicleAssignmentDetailRepository,
             IRepository<MaterialBatchDispensingHeader> materialBatchDispensingHeaderRepository, IRepository<MaterialBatchDispensingContainerDetail> materialBatchDispensingContainerDetailRepository,
             IRepository<MaterialMaster> materialMasterRepository, IRepository<WeighingMachineMaster> weighingMachineRepository, IRepository<UnitOfMeasurementMaster> unitOfMeasurementMasterRepository,
             IRepository<WMCalibratedLatestMachineDetail> wmcalibratedMachineRepository, IRepository<DispensingHeader> dispensingHeaderRepository, IRepository<DispensingDetail> dispensingDetailRepository,
             IRepository<UnitOfMeasurementTypeMaster> unitOfMeasurementTypeMasterRepository, IRepository<GRNMaterialLabelPrintingContainerBarcode> grnLabelPrintingContainerBarcodeRepository,
             MasterCommonRepository masterCommonRepository, IWebHostEnvironment environment, IRepository<PlantMaster> plantRepository, IRepository<DispensingPrintDetail> dispensingPrintDetailAppservice,
             IConfiguration configuration, PrinterFactory printerFactory, IRepository<DeviceMaster> deviceRepository, IRepository<User, long> userRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _processOrderRepository = processOrderRepository;
            _cubicleAssignmentHeaderRepository = cubicleAssignmentHeaderRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _equipmentRepository = equipmentRepository;
            _dispensingAppService = dispensingAppService;
            _equipmentAssignmentRepository = equipmentAssignmentRepository;
            _cubicleAssignmentDetailRepository = cubicleAssignmentDetailRepository;
            _materialBatchDispensingHeaderRepository = materialBatchDispensingHeaderRepository;
            _materialBatchDispensingContainerDetailRepository = materialBatchDispensingContainerDetailRepository;
            _materialMasterRepository = materialMasterRepository;
            _weighingMachineRepository = weighingMachineRepository;
            _unitOfMeasurementMasterRepository = unitOfMeasurementMasterRepository;
            _wmcalibratedMachineRepository = wmcalibratedMachineRepository;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _dispensingDetailRepository = dispensingDetailRepository;
            _unitOfMeasurementTypeMasterRepository = unitOfMeasurementTypeMasterRepository;
            _grnLabelPrintingBarcodeRepository = grnLabelPrintingContainerBarcodeRepository;
            _masterCommonRepository = masterCommonRepository;
            _userRepository = userRepository;
            _environment = environment;
            _plantRepository = plantRepository;
            _dispensingPrintDetailAppservice = dispensingPrintDetailAppservice;
            _configuration = configuration;
            _printerFactory = printerFactory;
            _deviceRepository = deviceRepository;
        }

        #endregion constructor

        #region public

        /// <summary>
        /// Used for getting staging completed process order and equipemntId
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetRLAFByBarcode(string input)
        {
            var responseDto = new HTTPResponseDto();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
                //Get all Equipment by input
                var allEquipment = from equipment in _equipmentRepository.GetAll()
                                   where equipment.IsActive && equipment.ApprovalStatusId == approvedApprovalStatusId &&
                                         equipment.EquipmentCode.ToLower() == input.ToLower()
                                   select new
                                   {
                                       Id = equipment.Id,
                                       Value = equipment.EquipmentCode,
                                       PlantId = equipment.PlantId,
                                   };
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    allEquipment = allEquipment.Where(x => x.PlantId == Convert.ToInt32(plantId));
                }
                var scannedEquipment = await allEquipment.FirstOrDefaultAsync();
                if (scannedEquipment == null)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentNotFoundValidation);
                }
                else
                {
                    var processOrderSelectList = await (from equipment in _equipmentRepository.GetAll()
                                                        join equipmentAssignement in _equipmentAssignmentRepository.GetAll()
                                                        on equipment.Id equals equipmentAssignement.EquipmentId
                                                        join cubicleAssignment in _cubicleAssignmentHeaderRepository.GetAll()
                                                        on equipmentAssignement.CubicleAssignmentHeaderId equals cubicleAssignment.Id
                                                        join cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
                                                        on equipmentAssignement.CubicleAssignmentHeaderId equals cubicleAssignmentDetail.CubicleAssignmentHeaderId
                                                        join processOrder in _processOrderRepository.GetAll()
                                                        on cubicleAssignmentDetail.ProcessOrderId equals processOrder.Id
                                                        where equipment.Id == scannedEquipment.Id && !equipmentAssignement.IsSampling
                                                        && !cubicleAssignment.IsSampling
                                                        && !equipmentAssignement.IsSampling
                                                        select new
                                                        {
                                                            Id = processOrder.Id,
                                                            Value = processOrder.ProcessOrderNo,
                                                            GroupCode = cubicleAssignment.GroupId,
                                                        }).ToListAsync() ?? default;
                    if (processOrderSelectList.Any())
                    {
                        var stagingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
                        var stagingCompletedPoList = (from processOrder in processOrderSelectList
                                                      join materialDispBatch in _materialBatchDispensingHeaderRepository.GetAll()
                                                      on processOrder.GroupCode equals materialDispBatch.GroupCode
                                                      where materialDispBatch.MaterialBatchDispensingHeaderType == StagingMaterialBatchDispensingHeaderType
                                                      && materialDispBatch.StatusId == stagingCompletedStatusId && !materialDispBatch.IsSampling
                                                      select new SelectListDto
                                                      {
                                                          Id = processOrder.Id,
                                                          Value = processOrder.Value
                                                      }).ToList() ?? default;
                        var groupedResult = stagingCompletedPoList.GroupBy(x => x.Id).Select(x => x.First());
                        if (groupedResult?.Count() == 0)
                        {
                            return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoStagingCompletedPo);
                        }
                        responseDto.ResultObject = new { EquipementId = scannedEquipment.Id, EquipmentBarcode = scannedEquipment.Value, SelectList = groupedResult };
                    }
                    else
                    {
                        return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoProcessorderAvailableUnderEquipment);
                    }
                    return responseDto;
                }
            }
            return responseDto;
        }

        /// <summary>
        /// Used for getting all material codes under selected process order
        /// </summary>
        /// <param name="processOrderId"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetMaterialByProcessOrderId(int processOrderId)
        {
            var today = DateTime.Now;
            var responseDto = new HTTPResponseDto();
            var stagingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
            //todo: material should be approved.
            //Get staging completed process order
            var materialSelectList = await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                            join materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                            on processOrderMaterial.ItemCode equals materialBatchDispensingHeader.MaterialCode
                                            where processOrderMaterial.ProcessOrderId == processOrderId && processOrderMaterial.ExpiryDate > today
                                            && processOrderMaterial.RetestDate > today && materialBatchDispensingHeader.StatusId == stagingCompletedStatusId
                                                  && materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == StagingMaterialBatchDispensingHeaderType
                                                  && !materialBatchDispensingHeader.IsSampling
                                            orderby processOrderMaterial.ExpiryDate ascending
                                            select new SelectListDto
                                            {
                                                Id = processOrderMaterial.ItemCode,
                                                Value = processOrderMaterial.ItemCode
                                            })?.ToListAsync() ?? default;
            materialSelectList = materialSelectList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            if (materialSelectList?.Count == 0)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderPO);
            }
            var processOrderHaveIssueIndicator = _processOrderRepository.GetAll().Where(x => x.Id == processOrderId && x.IssueIndicator.ToLower() == PMMSConsts.issueIndicator).Select(x => x.IssueIndicator).FirstOrDefault();
            responseDto.ResultObject = new { MaterialSelectList = materialSelectList, IssueIndicator = !(string.IsNullOrEmpty(processOrderHaveIssueIndicator) && string.IsNullOrWhiteSpace(processOrderHaveIssueIndicator)) };
            return responseDto;
        }

        /// <summary>
        /// Used for getting all staging completed sap batch number under selected material code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> UpdateSAPBatchByMaterialCode(MaterialDispensingDto input)
        {
            var responseDto = new HTTPResponseDto();
            var stagingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
            var sapBatchNumberSelectList = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                                  join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                                                                   on new { MaterialCode = materialBatchDispensingHeader.MaterialCode, SAPBatchNumber = materialBatchDispensingHeader.SAPBatchNumber } equals
                                                           new { MaterialCode = processOrderMaterial.ItemCode, SAPBatchNumber = processOrderMaterial.SAPBatchNo }
                                                  where processOrderMaterial.ProcessOrderId == input.ProcessOrderId
                                                  && materialBatchDispensingHeader.StatusId == stagingCompletedStatusId && materialBatchDispensingHeader.MaterialCode == input.MaterialCode
                                                  && materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == StagingMaterialBatchDispensingHeaderType
                                                  && !materialBatchDispensingHeader.IsSampling
                                                  join mm in _materialMasterRepository.GetAll()
                                                  on materialBatchDispensingHeader.MaterialCode equals mm.MaterialCode
                                                  select new
                                                  {
                                                      materialBatchDispensingHeader.Id,
                                                      creationTime = processOrderMaterial.CreationTime,
                                                      sapBatchNumber = processOrderMaterial.SAPBatchNo,
                                                      materialdesc = mm.MaterialDescription,
                                                  }).ToListAsync() ?? default;
            input.SapbBatchNumberSelectList = new List<SelectListDto>();
            input.SapbBatchNumberSelectList = sapBatchNumberSelectList.GroupBy(x => x.sapBatchNumber).Select(x => new SelectListDto { Id = x.First().Id, Value = x.First().sapBatchNumber }).ToList();
            if (!input.SapbBatchNumberSelectList.Any())
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoSAPAvailableUnderGroup);
            }
            //dispensing header already exist
            //To Do : Apply !Sampling
            var dispensingHeader = await _dispensingHeaderRepository.GetAll().Where(x => x.ProcessOrderId == input.ProcessOrderId
            && x.MaterialCodeId == input.MaterialCode && x.RLAFId == input.RLAFId && !x.IsSampling).FirstOrDefaultAsync();
            var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.DispensingSubModule, DispensingCompletedStatus);
            var statusId = 0;
            if (dispensingHeader != null)
            {
                input.Id = dispensingHeader.Id;
                statusId = dispensingHeader.StatusId;
                input.CheckedBy = dispensingHeader.CheckedBy;
                input.DoneBy = dispensingHeader.DoneBy;
            }
            else
            {
                input.Id = 0;
            }

            responseDto.ResultObject = new { Status = statusId != completedStatusId ? "In Progress" : "Completed", materialDispensingDto = input };
            return responseDto;
        }

        /// <summary>
        /// Used for Update required qty and balance qty.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MaterialDispensingDto> UpdateRequiredQuantityToDispensingDto(MaterialDispensingDto input)
        {
            input.RequiredQty = await _processOrderMaterialRepository.GetAll().Where(x => x.ProcessOrderId == input.ProcessOrderId
            && x.ItemCode.ToLower() == input.MaterialCode.ToLower()
                                                         && x.SAPBatchNo.ToLower() == input.SAPBatchNo.ToLower()).Select(x => x.OrderQuantity).SumAsync();
            var baseUnitOfMeasurement = await GetBaseUOMByMaterialCode(input.MaterialCode, input.ProcessOrderId, input.SAPBatchNo);
            input.BaseUnitOfMeasurementId = baseUnitOfMeasurement.Id;
            input.BaseUnitOfMeasurement = baseUnitOfMeasurement.UnitOfMeasurement;
            var lstDetails = await _dispensingDetailRepository.GetAll().Where(x => x.DispensingHeaderId == input.Id && x.SAPBatchNumber == input.SAPBatchNo)
                   .Select(x => new DispensingNetWeightModel { NoOfPacks = x.NoOfPacks, NetWeight = x.NetWeight, UnitOfMeasurementId = x.UnitOfMeasurementId, DoneBy = x.DoneBy, CheckedById = x.CheckedById })
                   .ToListAsync();
            if (input.Id != 0 && lstDetails.Any())
            {
                input.UnitOfMeasurementId = lstDetails.First().UnitOfMeasurementId;
                input.BalanceQty = Math.Max(0, (input.RequiredQty - await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value,
                                                                                       input.BaseUnitOfMeasurementId.Value, GetBalanceQuantity(lstDetails).Value)).Value);

                input.IsAnySAPBatchNoExistForHeader = true;
                /*  input.DoneBy = lstDetails.Last().DoneBy;
                   input.CheckedById = lstDetails.Last().CheckedById;
                   input.Printed = lstDetails.Last().Printed;*/

            }
            else
            {
                input.BalanceQty = input.RequiredQty;
            }

            return input;
        }

        public async Task<GetDespensingDetailsStatus> GetDespensingDetailsStatus(int dispensingHeaderId, string SAPBatchNo, string ContainerMaterialBarcode)
        {
            var input = new GetDespensingDetailsStatus();
            var lstDetails = await _dispensingDetailRepository.GetAll().Where(x => x.DispensingHeaderId == dispensingHeaderId && x.SAPBatchNumber == SAPBatchNo && x.ContainerMaterialBarcode == ContainerMaterialBarcode)
                   .ToListAsync();

            if (lstDetails.Any())
            {
                input.DoneBy = lstDetails.First().DoneBy;
                input.CheckedById = lstDetails.Last().CheckedById;
                input.Printed = lstDetails.Last().Printed;
                input.IsGrossWeight = lstDetails.Last().IsGrossWeight;
                input.GrossWeight = lstDetails.Last().GrossWeight;
                input.NetWeight = lstDetails.Last().NetWeight;
                input.NoOfPacks = lstDetails.Last().NoOfPacks;
                input.TareWeight = lstDetails.Last().TareWeight;
                input.Id = lstDetails.Last().Id;
                input.NoOfContainers = lstDetails.Last().NoOfContainers;



            }
            else
            {
                input.DoneBy = null;
                input.CheckedById = null;
                input.Printed = false;
                input.IsGrossWeight = false;
                input.NetWeight = 0;
                input.NoOfPacks = 0;
                input.TareWeight = 0;
                input.Id = 0;
                input.DispenseBarcode = null;
            }

            return input;
        }

        private float? GetBalanceQuantity(List<DispensingNetWeightModel> lstDispenseDetails)
        {
            var netWeightSum = lstDispenseDetails.Sum(x => x.NetWeight);
            if (netWeightSum > 0)
            {
                return netWeightSum;
            }
            else
            {
                return lstDispenseDetails.Sum(x => x.NoOfPacks);
            }
        }

        /// <summary>
        /// Used for getting Uom code under selected material code..
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<MaterialDispensingInternalDto>> GetAllUOMByMaterialCodeAsync(string materialCode, string sapBatchNumber, int processOrderId)
        {
            var baseUnitOfMeasurement = await _processOrderMaterialRepository.GetAll().Where(x => x.ItemCode == materialCode.ToLower() &&
              x.SAPBatchNo == sapBatchNumber.ToLower() && x.ProcessOrderId == processOrderId).Select(x => x.UnitOfMeasurement)
             .FirstOrDefaultAsync();

            if (baseUnitOfMeasurement != null)
            {
                var materialUomQuery = from materialMaster in _materialMasterRepository.GetAll()
                                       join uomMaster in _unitOfMeasurementMasterRepository.GetAll()
                                       on materialMaster.ConversionUOM.ToLower() equals uomMaster.UnitOfMeasurement.ToLower()
                                       join uomTypemaster in _unitOfMeasurementTypeMasterRepository.GetAll()
                                       on uomMaster.UnitOfMeasurementTypeId equals uomTypemaster.Id
                                       where materialMaster.MaterialCode.ToLower() == materialCode.ToLower()
                                      && materialMaster.BaseUOM.ToLower() == baseUnitOfMeasurement.ToLower()
                                       select new MaterialDispensingInternalDto
                                       {
                                           ConversionUOMName = materialMaster.ConversionUOM,
                                           Denominator = materialMaster.Denominator,
                                           Numerator = materialMaster.Numerator,
                                           UomId = uomMaster.Id,
                                           UnitOfMeasurementTypeId = uomTypemaster.Id,
                                           UOMType = uomTypemaster.UnitOfMeasurementTypeName,
                                           IsPackUOM = uomTypemaster.UnitOfMeasurementTypeName == PackUOMType,
                                       };
                return await materialUomQuery.ToListAsync() ?? default;
            }

            return default;
        }

        /// <summary>
        /// Used for getting calibrated weighing balance.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uomId"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetBalanceByBarcode(string input, int uomId)
        {
            var responseDto = new HTTPResponseDto();
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var weighingMachineQuery = _weighingMachineRepository.GetAll().Where(x => x.ApprovalStatusId == approvedApprovalStatusId
            && x.WeighingMachineCode.ToLower() == input.ToLower()).OrderBy(x => x.WeighingMachineCode)
                    .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.WeighingMachineCode, PlantId = x.SubPlantId });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                weighingMachineQuery = weighingMachineQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            var scanBalanceResult = await weighingMachineQuery.FirstOrDefaultAsync();
            if (scanBalanceResult != null && uomId != 0)
            {
                int scanBalanceId = Convert.ToInt32(scanBalanceResult.Id);
                //Get all suggested scan balance
                var suggestedBalance = await GetAllSuggestedCalibratedBalancesInternalAsync(uomId);
                if (suggestedBalance == null)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.WMNotCalibrated);
                }
                //Check valid balance scanned
                var validBalanceFromSuggestedBalance = suggestedBalance.Where(a => (int)a.Id == scanBalanceId)
                    .Select(x => new SelectListDto { Id = (int)x.Id, Value = x.Value }).ToList();
                if (validBalanceFromSuggestedBalance.Any())
                {
                    responseDto.ResultObject = validBalanceFromSuggestedBalance;
                    return responseDto;
                }
                else
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.BalanceIsNotFromSuggestedBalanceValidation);
                }
            }
            else
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.BalanceIdNotFound);
            }
        }

        /// <summary>
        /// Used for validating material container barcode.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uomId"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> UpdateMaterialContainerByBarcode(MaterialDispensingDto input)
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
                var materialBatchDispensingContainerDetails = await (
                              from materialBatchContainerDetails in _materialBatchDispensingContainerDetailRepository.GetAll()
                              where materialBatchContainerDetails.MaterialBatchDispensingHeaderId == input.MaterialBatchDispensingHeaderId
                              && materialBatchContainerDetails.ContainerBarCode.ToLower() == grnMaterialLabelPrintingContainerBarcode.MaterialLabelContainerBarCode.ToLower()
                              select new
                              {
                                  containerBarcode = materialBatchContainerDetails.ContainerBarCode,
                                  IsVerified = materialBatchContainerDetails.IsVerified,
                                  Id = materialBatchContainerDetails.Id
                              }).FirstOrDefaultAsync();
                if (materialBatchDispensingContainerDetails == null)
                {
                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NotStagedContainer);
                }
                input.MaterialContainerId = grnMaterialLabelPrintingContainerBarcode.Id;
                input.MaterialContainerBarCode = materialBatchDispensingContainerDetails.containerBarcode;
                input.MaterialContainerBalanceQuantity = grnMaterialLabelPrintingContainerBarcode.BalanceQuantity;
                input.IsVerified = materialBatchDispensingContainerDetails.IsVerified;
                input.MaterialBatchDispensingContainerDetailsId = materialBatchDispensingContainerDetails.Id;
            }
            responseDto.ResultObject = input;
            return responseDto;
        }

        public async Task<HTTPResponseDto> UpdateMaterialBatchDispensingVerify(GetDespensingDetailsStatus input)
        {

            var IdUser = AbpSession.UserId;
            //  var printByName = _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId).Select(x => x.).FirstOrDefault();
            var responseDto = new HTTPResponseDto();
            var dispensingDetailverify = await _dispensingDetailRepository.GetAsync((int)input.Id);
            input.DispenseBarcode = dispensingDetailverify.DispenseBarcode;
            if (input.CheckedById == null || input.CheckedById == 0)
            {
                dispensingDetailverify.CheckedById = (int)IdUser;
            }

            dispensingDetailverify.Printed = input.Printed;
            var r = await _dispensingDetailRepository.UpdateAsync(dispensingDetailverify);
            responseDto.ResultObject = null;
            return responseDto;
        }

        /// <summary>
        /// Used for getting suggested weighing balance under selected UOM.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SelectListDto>> GetAllSuggestedCalibratedBalancesAsync(int uomId)
        {
            return await GetAllSuggestedCalibratedBalancesInternalAsync(uomId);
        }

        public async Task<double> GetWeightByWeighingMachineCodeAsync(int weighingMachineId)
        {
            var ipAddressPort = await _weighingMachineRepository.GetAll().Where(x => x.Id == weighingMachineId)
                .Select(x => new { x.IPAddress, x.PortNumber }).FirstOrDefaultAsync();
            if (ipAddressPort != null)
            {
                //TO DO : Get value from Weighing Machine
                return 0.2787878676;
            }
            return default;
        }

        public async Task<bool> ReprintDispensingDetailAsync(MaterialDispensingDto input)
        {
            //Print dispensing material label
            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
            {
                MaterialDispensingLabel labelExtraDetails = await GetExtraLabelData(input.ProcessOrderId);
                var dispensingDetailToReprint = await _dispensingDetailRepository.GetAsync(input.MaterialDispensingDetailIdToReprint);
                input.DispensingBarcode = dispensingDetailToReprint.DispenseBarcode;

                labelExtraDetails.TareWeight = Convert.ToString(dispensingDetailToReprint.TareWeight);
                labelExtraDetails.GrossWeight = Convert.ToString(dispensingDetailToReprint.GrossWeight);
                labelExtraDetails.NetWeight = Convert.ToString(dispensingDetailToReprint.NetWeight);
                var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.ReprintDeviceId));
                await PrintDispensingContainerLabel(input, labelExtraDetails, device);
            }
            DispensingPrintDetail printDetail = new DispensingPrintDetail();
            printDetail.IsController = input.IsController;
            printDetail.DispensingDetailId = input.MaterialDispensingDetailIdToReprint;
            printDetail.DeviceId = input.ReprintDeviceId;

            await _dispensingPrintDetailAppservice.InsertAsync(printDetail);
            return true;
        }

        private async Task<bool> ValidateQuantity(MaterialDispensingDto input, HTTPResponseDto responseDto)
        {
            if (input.IsPackUOM)
            {
                var convertedPackQuantity = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, input.NoOfPacks.Value);
                input.ConvertedNoOfPack = convertedPackQuantity;
                if ((!input.IssueIndicator) && convertedPackQuantity > input.BalanceQty)
                {
                    //Not valid
                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoOfPackGreaterThanBalanceQuantity);
                    return false;
                }
                if ((!input.IssueIndicator) && convertedPackQuantity > input.MaterialContainerBalanceQuantity)
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
                if ((!input.IssueIndicator) && convertedNetQuantity > input.BalanceQty)
                {
                    //Not valid
                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NetWeightGreaterThanBalanceQuantity);
                    return false;
                }
                if (convertedNetQuantity > input.MaterialContainerBalanceQuantity)
                {
                    //Not valid
                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NetWeightGreaterThanContainerBalanceQuantity);
                    return false;
                }
            }

            return true;
        }


        public async Task<HTTPResponseDto> PrintDispensingBarcodeAsync(MaterialDispensingDto input)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            Dictionary<string, float?> dictSAPBatchRequiredQuantity = await GetSAPBatchandRequiredQuantity(input);

            var allSAPBatchList = await _dispensingDetailRepository.GetAll().Where(x => x.DispensingHeaderId == input.Id)
                .Select(x => new { x.SAPBatchNumber, x.NetWeight, x.NoOfPacks, x.UnitOfMeasurementId }).ToListAsync();
            if (allSAPBatchList == null || allSAPBatchList?.Count() == 0)
            {
                //Not all SAP Batches are completed
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.AllSAPBatchDispensingNotCompleted);
            }
            var lstDispenseSAPBatchNetQuantity = allSAPBatchList.GroupBy(x => x.SAPBatchNumber).Select(x =>
            new
            {
                SAPBatchNumber = x.First().SAPBatchNumber,
                NetWeight = x.Sum(x => x.NetWeight),
                NoOfPacks = x.Sum(x => x.NoOfPacks),
                UnitOfMeasurementId = x.First().UnitOfMeasurementId
            });
            if (dictSAPBatchRequiredQuantity.Keys.Except(lstDispenseSAPBatchNetQuantity.Select(x => x.SAPBatchNumber)).Any())
            {
                //Not all SAP Batches are completed
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.AllSAPBatchDispensingNotCompleted);
            }
            var IsAnySAPBalanceQuantity = false;
            foreach (var dispensedBatch in lstDispenseSAPBatchNetQuantity)
            {
                if (dispensedBatch.NoOfPacks != null && dispensedBatch.NoOfPacks != 0)
                {
                    var convertedNoOfPack = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, dispensedBatch.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, dispensedBatch.NoOfPacks.Value);
                    if (convertedNoOfPack < dictSAPBatchRequiredQuantity[dispensedBatch.SAPBatchNumber])
                    {
                        IsAnySAPBalanceQuantity = true;
                    }
                }
                else
                {
                    var convertedNetWeight = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, dispensedBatch.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, dispensedBatch.NetWeight.Value);
                    if (convertedNetWeight < dictSAPBatchRequiredQuantity[dispensedBatch.SAPBatchNumber])
                    {
                        IsAnySAPBalanceQuantity = true;
                    }
                }
            }
            if (IsAnySAPBalanceQuantity)
            {
                //Not all quantity is dispensed
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.AllSAPBatchDispensingNotCompleted);
            }
            var dsDetails = new GetDespensingDetailsStatus();
            dsDetails.CheckedById = input.CheckedById;
            dsDetails.Printed = true;
            dsDetails.Id = input.MaterialDispensingDetailIdToReprint;

            var sts = await UpdateMaterialBatchDispensingVerify(dsDetails);


            if (!await ValidateQuantity(input, responseDto))
            {
                return responseDto;
            }

            //if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
            //{
            //    MaterialDispensingLabel labelExtraDetails = await GetExtraLabelData(input.ProcessOrderId);
            //    var containerNo = _masterCommonRepository.GetNextDispensingLabelBarcodeSequenceValue();
            //    var serialNo = $"{containerNo:D10}";
            //    var currentDate = DateTime.UtcNow;
            //    //   input.DispensingBarcode = $"D-{currentDate:yy}-{serialNo}";

            //    input.DispensingBarcode = dsDetails.DispenseBarcode;

            //    if (input.CheckedById != null && input.CheckedById != 0 && labelExtraDetails != null)
            //    {
            //        labelExtraDetails.TareWeight = Convert.ToString(input.TareWeight);
            //        labelExtraDetails.GrossWeight = Convert.ToString(input.GrossWeight);
            //        labelExtraDetails.NetWeight = Convert.ToString(input.NetWeight);
            //        var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.DeviceId));
            //        await PrintDispensingContainerLabel(input, labelExtraDetails, device);
            //        return responseDto;
            //    }
            //}

            return responseDto;
        }
        public async Task<HTTPResponseDto> varifyDispensingBarcodeAsync(MaterialDispensingDto input)
        {
            var IdUser = AbpSession.UserId;
            HTTPResponseDto responseDto = new HTTPResponseDto();
            if (!await ValidateQuantity(input, responseDto))
            {
                return responseDto;
            }

            //if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
            //{
            //    MaterialDispensingLabel labelExtraDetails = await GetExtraLabelData(input.ProcessOrderId);
            //    var containerNo = _masterCommonRepository.GetNextDispensingLabelBarcodeSequenceValue();
            //    var serialNo = $"{containerNo:D10}";
            //    var currentDate = DateTime.UtcNow;
            //    input.DispensingBarcode = $"D-{currentDate:yy}-{serialNo}";



            //    if (input.CheckedById != null && input.CheckedById != 0)
            //    {
            //        labelExtraDetails.TareWeight = Convert.ToString(input.TareWeight);
            //        labelExtraDetails.GrossWeight = Convert.ToString(input.GrossWeight);
            //        labelExtraDetails.NetWeight = Convert.ToString(input.NetWeight);
            //        var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.DeviceId));
            //        await PrintDispensingContainerLabel(input, labelExtraDetails, device);
            //        return null;
            //    }
            //}

            //Insert/Update into materialdispensingheader/Insert into materialdispensingdetail
            DispensingDetail dispensingDetail = new DispensingDetail();
            dispensingDetail.ContainerMaterialBarcode = input.MaterialContainerBarCode;
            dispensingDetail.DispenseBarcode = input.DispensingBarcode;
            if (input.IsPackUOM)
            {
                dispensingDetail.NoOfPacks = input.NoOfPacks;
            }
            else
            {
                dispensingDetail.GrossWeight = input.GrossWeight;
                dispensingDetail.NetWeight = input.NetWeight;
                dispensingDetail.TareWeight = input.TareWeight;
            }

            dispensingDetail.SAPBatchNumber = input.SAPBatchNo;
            dispensingDetail.UnitOfMeasurementId = input.UnitOfMeasurementId;
            dispensingDetail.WeighingMachineId = input.BalanceId;
            dispensingDetail.IsGrossWeight = input.IsGrossWeight;
            dispensingDetail.DoneBy = (int)IdUser;
            dispensingDetail.CheckedById = input.CheckedById;
            dispensingDetail.NoOfContainers = input.NoOfContainers;



            DispensingPrintDetail dispensingPrintDetail = new DispensingPrintDetail();
            dispensingPrintDetail.DeviceId = input.DeviceId;
            dispensingPrintDetail.IsController = input.IsController;
            dispensingDetail.DispensingPrintDetails = new List<DispensingPrintDetail>();
            dispensingDetail.DispensingPrintDetails.Add(dispensingPrintDetail);

            if (input.Id == 0)
            {
                DispensingHeader header = new DispensingHeader();
                header.MaterialCodeId = input.MaterialCode;
                header.ProcessOrderId = input.ProcessOrderId;
                header.RLAFId = input.RLAFId;
                header.StartTime = DateTime.Now;
                header.IsSampling = false;
                header.CheckedBy = input.CheckedBy;
                header.DoneBy = (int)IdUser;
                header.StatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule,
                                                                PMMSConsts.DispensingSubModule, DispensingInProgressStatus);
                header.DispensingDetails = new List<DispensingDetail>();
                header.DispensingDetails.Add(dispensingDetail);
                input.Id = await _dispensingHeaderRepository.InsertAndGetIdAsync(header);
            }
            else
            {
                dispensingDetail.DispensingHeaderId = input.Id;
                await _dispensingDetailRepository.InsertAsync(dispensingDetail);
            }

            //Update balance quantity from containerbarcodetable
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
            await UnitOfWorkManager.Current.SaveChangesAsync();

            var lstDetails = await _dispensingDetailRepository.GetAll().Where(x => x.DispensingHeaderId == input.Id && x.SAPBatchNumber == input.SAPBatchNo)
                   .Select(x => new DispensingNetWeightModel { NoOfPacks = x.NoOfPacks, NetWeight = x.NetWeight, UnitOfMeasurementId = x.UnitOfMeasurementId })
                   .ToListAsync();
            input.BalanceQty = Math.Max(0, (input.RequiredQty - await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value,
                                                                                       input.BaseUnitOfMeasurementId.Value, GetBalanceQuantity(lstDetails).Value)).Value);
            input.MaterialContainerBalanceQuantity = containerForBalanceQuantity.BalanceQuantity;
            input.IsAnySAPBatchNoExistForHeader = true;
            responseDto.ResultObject = new { Status = "In Progress", materialDispensingDto = input };
            return responseDto;
        }

        private async Task<MaterialDispensingLabel> GetExtraLabelData(int ProcessOrderId)
        {
            return await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                          join processOrder in _processOrderRepository.GetAll()
                          on processOrderMaterial.ProcessOrderId equals processOrder.Id
                          join plantMaster in _plantRepository.GetAll()
                          on processOrder.PlantId equals plantMaster.Id
                          where processOrder.Id == ProcessOrderId
                          select new MaterialDispensingLabel
                          {
                              PlantId = plantMaster.PlantId,
                              ExpiryDate = processOrderMaterial.ExpiryDate,
                              BatchNo = processOrderMaterial.BatchNo,
                              ProductName = processOrder.ProductCode,
                              ArNo = processOrderMaterial.ARNo,
                              PlantName = plantMaster.PlantName,
                              ARNo = processOrderMaterial.ARNo,
                              OrderQuantity = processOrderMaterial.OrderQuantity,
                          }).FirstOrDefaultAsync();
        }

        public async Task<HTTPResponseDto> CompleteDispensingAsync(MaterialDispensingDto input)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            Dictionary<string, float?> dictSAPBatchRequiredQuantity = await GetSAPBatchandRequiredQuantity(input);

            var allSAPBatchList = await _dispensingDetailRepository.GetAll().Where(x => x.DispensingHeaderId == input.Id)
                .Select(x => new { x.SAPBatchNumber, x.NetWeight, x.NoOfPacks, x.UnitOfMeasurementId }).ToListAsync();
            if (allSAPBatchList == null || allSAPBatchList?.Count() == 0)
            {
                //Not all SAP Batches are completed
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.AllSAPBatchDispensingNotCompleted);
            }
            var lstDispenseSAPBatchNetQuantity = allSAPBatchList.GroupBy(x => x.SAPBatchNumber).Select(x =>
            new
            {
                SAPBatchNumber = x.First().SAPBatchNumber,
                NetWeight = x.Sum(x => x.NetWeight),
                NoOfPacks = x.Sum(x => x.NoOfPacks),
                UnitOfMeasurementId = x.First().UnitOfMeasurementId
            });
            if (dictSAPBatchRequiredQuantity.Keys.Except(lstDispenseSAPBatchNetQuantity.Select(x => x.SAPBatchNumber)).Any())
            {
                //Not all SAP Batches are completed
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.AllSAPBatchDispensingNotCompleted);
            }
            var IsAnySAPBalanceQuantity = false;
            foreach (var dispensedBatch in lstDispenseSAPBatchNetQuantity)
            {
                if (dispensedBatch.NoOfPacks != null && dispensedBatch.NoOfPacks != 0)
                {
                    var convertedNoOfPack = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, dispensedBatch.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, dispensedBatch.NoOfPacks.Value);
                    if (convertedNoOfPack < dictSAPBatchRequiredQuantity[dispensedBatch.SAPBatchNumber])
                    {
                        IsAnySAPBalanceQuantity = true;
                    }
                }
                else
                {
                    var convertedNetWeight = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, dispensedBatch.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, dispensedBatch.NetWeight.Value);
                    if (convertedNetWeight < dictSAPBatchRequiredQuantity[dispensedBatch.SAPBatchNumber])
                    {
                        IsAnySAPBalanceQuantity = true;
                    }
                }
            }
            if (IsAnySAPBalanceQuantity)
            {
                //Not all quantity is dispensed
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.AllSAPBatchDispensingNotCompleted);
            }
            var dispensingHeader = await _dispensingHeaderRepository.GetAsync(input.Id);
            dispensingHeader.EndTime = DateTime.Now;
            dispensingHeader.StatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule,
                PMMSConsts.DispensingSubModule, DispensingCompletedStatus);
            dispensingHeader.DoneBy = input.DoneBy;
            dispensingHeader.CheckedById = input.CheckedById;
            dispensingHeader.CheckedBy = input.CheckedBy;
            if (input.StatusId == 0)
            {
                dispensingHeader.StatusId = 0;
            }
            await _dispensingHeaderRepository.UpdateAsync(dispensingHeader);
            responseDto.ResultObject = new { Status = "Completed", materialDispensingDto = input };
            return responseDto;
        }

        public async Task<List<MaterialDispensingDetailDto>> UpdateAllMaterialDispensingDetailBySAPBatch(MaterialDispensingDto input)
        {
            return await (from dispensingDetail in _dispensingDetailRepository.GetAll()
                          join uomMaster in _unitOfMeasurementMasterRepository.GetAll()
                          on dispensingDetail.UnitOfMeasurementId equals uomMaster.Id
                          where dispensingDetail.DispensingHeaderId == input.Id
                          && dispensingDetail.SAPBatchNumber == input.SAPBatchNo
                          select new MaterialDispensingDetailDto
                          {
                              Id = dispensingDetail.Id,
                              NoOfPacks = dispensingDetail.NoOfPacks,
                              NetWeight = dispensingDetail.NetWeight,
                              GrossWeight = dispensingDetail.GrossWeight,
                              TareWeight = dispensingDetail.TareWeight,
                              DispensingContainerBarcode = dispensingDetail.DispenseBarcode,
                              UnitOfMeasurement = uomMaster.UnitOfMeasurement
                          }).ToListAsync();
        }

        #endregion public

        #region private

        private async Task<Dictionary<string, float?>> GetSAPBatchandRequiredQuantity(MaterialDispensingDto input)
        {
            var lstSAPBatchRequiredQuantity = await _processOrderMaterialRepository.GetAll().Where(x => x.ProcessOrderId == input.ProcessOrderId
            && x.ItemCode.ToLower() == input.MaterialCode.ToLower())
                .Select(x => new { x.SAPBatchNo, x.OrderQuantity }).ToListAsync();
            return lstSAPBatchRequiredQuantity.GroupBy(x => x.SAPBatchNo).Select(x =>
              new { SAPBatchNumber = x.First().SAPBatchNo, RequiredQuantity = x.Sum(x => x.OrderQuantity) }).ToDictionary(x => x.SAPBatchNumber,
              x => x.RequiredQuantity);
        }

        private async Task PrintDispensingContainerLabel(MaterialDispensingDto input, MaterialDispensingLabel labelExtraDetails, DeviceMaster device)
        {
            var printInput = new PrinterInput
            {
                IPAddress = device.IpAddress,
                Port = Convert.ToInt32(device.Port),
                PrintBody = GetMaterialDispensingPrintBody(input, labelExtraDetails),
            };
            var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
            await prnPrinter.Print(printInput);

        }

        private string GetMaterialDispensingPrintBody(MaterialDispensingDto input, MaterialDispensingLabel labelExtraDetails)
        {
            var printByName = _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId).Select(x => x.Name).FirstOrDefault();
            var serialNo = input.DispensingBarcode.Split("-")[2];
            var materialLabelPRNFilePath = $"{_environment.WebRootPath}\\label_print_prn\\dispensing_container_label.prn";
            var materilLabelPRNFile = File.ReadAllText(materialLabelPRNFilePath);

            var strContainers = "";
            for (int i = 0; i < input.NoOfContainers; i++)
            {
                strContainers += (i + 1) + " of " + input.NoOfContainers;
                strContainers += (i == (input.NoOfContainers - 1)) ? "" : ", ";
            }


            var args = new Dictionary<string, string>(
        StringComparer.OrdinalIgnoreCase) {
            {"{Matcode}", input.MaterialCode},
            {"{SapBatch}", input.SAPBatchNo},
            {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
            {"{ProductName}", labelExtraDetails.ProductName},
            {"{BatchNo}", labelExtraDetails.BatchNo},
            {"{BatchSize}", labelExtraDetails.BatchSize},
            {"{GrossWeight}", labelExtraDetails.GrossWeight},
            {"{NetWeight}", labelExtraDetails.NetWeight},
            {"{TareWeight}", labelExtraDetails.TareWeight},
            { "{ContainerNo}", strContainers },
            {"{FormatNo}",formatNumber},
            {"{SOPNo}", sopNo},
            {"{Printby}",printByName},
            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
            {"{PlantCode}",labelExtraDetails.PlantId },
            {"{ARNo}",labelExtraDetails.ArNo },
            {"{SerialNo}",serialNo },
            {"{Barcode}",$"{input.DispensingBarcode}"},
            {"{PlantName}",$"{labelExtraDetails.PlantName}"}};

            var newstr = args.Aggregate(materilLabelPRNFile, (current, value) => current.Replace(value.Key, value.Value));
            return newstr;
        }

        private async Task<DispensingUnitOfMeasurementDto> GetBaseUOMByMaterialCode(string materialCode, int processOrderId, string SAPBatchNo)
        {
            return await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
                          join uomMaster in _unitOfMeasurementMasterRepository.GetAll()
                          on processOrderMaterial.UnitOfMeasurement equals uomMaster.UnitOfMeasurement
                          where processOrderMaterial.ItemCode.ToLower() == materialCode.ToLower()
                          && processOrderMaterial.ProcessOrderId == processOrderId
                          && processOrderMaterial.SAPBatchNo.ToLower() == SAPBatchNo.ToLower()
                          select new DispensingUnitOfMeasurementDto { Id = uomMaster.Id, UnitOfMeasurement = uomMaster.UnitOfMeasurement })
                          .FirstOrDefaultAsync();
        }

        private async Task<List<SelectListDto>> GetAllSuggestedCalibratedBalancesInternalAsync(int uomId)
        {
            var currentDateTime = DateTime.Now;
            var weighingMachineBalanceList = await (from weighingMachineMaster in _weighingMachineRepository.GetAll()
                                                    join wMCalibratedLatestMachineDetail in _wmcalibratedMachineRepository.GetAll() on
                                                    weighingMachineMaster.Id equals wMCalibratedLatestMachineDetail.WeighingMachineId
                                                    where weighingMachineMaster.UnitOfMeasurementId == uomId && wMCalibratedLatestMachineDetail.LastCalibrationTestDate.Date.Day == currentDateTime.Day &&
                                                    wMCalibratedLatestMachineDetail.LastCalibrationTestDate.Date.Month == currentDateTime.Month && wMCalibratedLatestMachineDetail.LastCalibrationTestDate.Date.Year == currentDateTime.Year
                                                    select new SelectListDto
                                                    {
                                                        Id = weighingMachineMaster.Id,
                                                        Value = weighingMachineMaster.WeighingMachineCode
                                                    }).ToListAsync();
            if (weighingMachineBalanceList.Any())
            {
                return weighingMachineBalanceList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            }
            return default;
        }

        #endregion private
    }
}