//using Abp.Application.Services;
//using Abp.Domain.Repositories;
//using Abp.Linq;
//using ELog.Application.CommonDto;
//using ELog.Application.CommonService.Dispensing;
//using ELog.Application.Dispensing.MaterialDispensing.Dto;
//using ELog.Application.Dispensing.MaterialSampleDispensing.Dto;
//using ELog.Application.SelectLists.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Authorization.Users;
//using ELog.Core.Entities;
//using ELog.Core.Printer;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using ELog.HardwareConnectorFactory;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Dispensing.MaterialSampleDispensing
//{
//    [PMMSAuthorize]
//    public class MaterialSampleDispensingAppService : ApplicationService, IMaterialSampleDispensingAppService
//    {
//        #region fields
//        private const string formatNumber = "FDSTG/F/013,Version:01";
//        private const string sopNo = "FDSOP/GHT/ST/018";
//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IRepository<InspectionLot> _inspectionLotRepository;
//        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignmentHeaderRepository;
//        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignmentDetailRepository;
//        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
//        private readonly IRepository<EquipmentMaster> _equipmentRepository;
//        private readonly IRepository<EquipmentAssignment> _equipmentAssignmentRepository;
//        private readonly IDispensingAppService _dispensingAppService;
//        private readonly IRepository<MaterialBatchDispensingHeader> _materialBatchDispensingHeaderRepository;
//        private readonly IRepository<MaterialBatchDispensingContainerDetail> _materialBatchDispensingContainerDetailRepository;
//        private readonly IRepository<MaterialMaster> _materialMasterRepository;
//        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
//        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementMasterRepository;
//        private readonly IRepository<WMCalibratedLatestMachineDetail> _wmcalibratedMachineRepository;
//        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
//        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
//        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnLabelPrintingBarcodeRepository;
//        private readonly IRepository<PlantMaster> _plantRepository;
//        private readonly MasterCommonRepository _masterCommonRepository;
//        private readonly IWebHostEnvironment _environment;
//        private readonly IConfiguration _configuration;
//        private readonly PrinterFactory _printerFactory;
//        private readonly IRepository<DeviceMaster> _deviceRepository;
//        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
//        private readonly int StagingMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.Staging;
//        private readonly string StagingCompletedStatus = nameof(StageHeaderStatus.Completed).ToLower();
//        private readonly string SamplingInProgressStatus = nameof(SamplingHeaderStatus.InProgress).ToLower();
//        private readonly string SamplingCompletedStatus = nameof(SamplingHeaderStatus.Completed).ToLower();
//        private readonly string PackUOMType = nameof(UOMType.Pack).ToLower();
//        private readonly IRepository<SamplingTypeMaster> _samplingTypeRepository;
//        private readonly IRepository<DispensingPrintDetail> _dispensingPrintDetailAppservice;
//        private readonly IRepository<GRNDetail> _grnDetail;
//        private readonly IRepository<GRNHeader> _grnHeader;
//        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnMaterialLabelPrintingContainerBarcode;
//        private readonly IRepository<User, long> _userRepository;
//        private readonly IRepository<MaterialConsignmentDetail> _materialConsignmentDetail;
//        private readonly IRepository<GRNQtyDetail> _grnQtyDetailRepository;


//        #endregion fields

//        #region constructor

//        public MaterialSampleDispensingAppService(IHttpContextAccessor httpContextAccessor,
//            IRepository<CubicleAssignmentHeader> cubicleAssignmentHeaderRepository,
//            IRepository<ProcessOrderMaterial> processOrderMaterialRepository, IRepository<EquipmentMaster> equipmentRepository, IDispensingAppService dispensingAppService,
//            IRepository<EquipmentAssignment> equipmentAssignmentRepository, IRepository<CubicleAssignmentDetail> cubicleAssignmentDetailRepository,
//             IRepository<MaterialBatchDispensingHeader> materialBatchDispensingHeaderRepository, IRepository<MaterialBatchDispensingContainerDetail> materialBatchDispensingContainerDetailRepository,
//             IRepository<MaterialMaster> materialMasterRepository, IRepository<WeighingMachineMaster> weighingMachineRepository, IRepository<UnitOfMeasurementMaster> unitOfMeasurementMasterRepository,
//             IRepository<WMCalibratedLatestMachineDetail> wmcalibratedMachineRepository, IRepository<DispensingHeader> dispensingHeaderRepository, IRepository<DispensingDetail> dispensingDetailRepository,
//             IRepository<GRNMaterialLabelPrintingContainerBarcode> grnLabelPrintingContainerBarcodeRepository,
//             MasterCommonRepository masterCommonRepository, IWebHostEnvironment environment, IRepository<PlantMaster> plantRepository, PrinterFactory printerFactory,
//             IRepository<InspectionLot> inspectionLotRepository, IRepository<SamplingTypeMaster> samplingTypeRepository, IRepository<DispensingPrintDetail> dispensingPrintDetailAppservice,
//              IRepository<DeviceMaster> deviceRepository, IConfiguration configuration, IRepository<GRNDetail> grnDetail, IRepository<GRNHeader> grnHeader,
//              IRepository<GRNMaterialLabelPrintingContainerBarcode> grnMaterialLabelPrintingContainerBarcode,
//              IRepository<User, long> userRepository, IRepository<MaterialConsignmentDetail> materialConsignmentDetail,
//              IRepository<GRNQtyDetail> grnQtyDetailRepository)
//        {
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _httpContextAccessor = httpContextAccessor;
//            _cubicleAssignmentHeaderRepository = cubicleAssignmentHeaderRepository;
//            _processOrderMaterialRepository = processOrderMaterialRepository;
//            _equipmentRepository = equipmentRepository;
//            _dispensingAppService = dispensingAppService;
//            _equipmentAssignmentRepository = equipmentAssignmentRepository;
//            _cubicleAssignmentDetailRepository = cubicleAssignmentDetailRepository;
//            _materialBatchDispensingHeaderRepository = materialBatchDispensingHeaderRepository;
//            _materialBatchDispensingContainerDetailRepository = materialBatchDispensingContainerDetailRepository;
//            _materialMasterRepository = materialMasterRepository;
//            _weighingMachineRepository = weighingMachineRepository;
//            _unitOfMeasurementMasterRepository = unitOfMeasurementMasterRepository;
//            _wmcalibratedMachineRepository = wmcalibratedMachineRepository;
//            _dispensingHeaderRepository = dispensingHeaderRepository;
//            _dispensingDetailRepository = dispensingDetailRepository;
//            _grnLabelPrintingBarcodeRepository = grnLabelPrintingContainerBarcodeRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _environment = environment;
//            _plantRepository = plantRepository;
//            _printerFactory = printerFactory;
//            _inspectionLotRepository = inspectionLotRepository;
//            _samplingTypeRepository = samplingTypeRepository;
//            _dispensingPrintDetailAppservice = dispensingPrintDetailAppservice;
//            _deviceRepository = deviceRepository;
//            _configuration = configuration;
//            _grnDetail = grnDetail;
//            _grnHeader = grnHeader;
//            _grnMaterialLabelPrintingContainerBarcode = grnMaterialLabelPrintingContainerBarcode;
//            _userRepository = userRepository;
//            _materialConsignmentDetail = materialConsignmentDetail;
//            _grnQtyDetailRepository = grnQtyDetailRepository;
//        }

//        #endregion constructor

//        #region public

//        /// <summary>
//        /// Used for getting staging completed process order and equipemntId
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public async Task<HTTPResponseDto> GetRLAFByBarcode(string input)
//        {
//            var responseDto = new HTTPResponseDto();
//            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
//            {
//                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
//                //Get all Equipment by input
//                var allEquipment = from equipment in _equipmentRepository.GetAll()
//                                   where equipment.IsActive && equipment.ApprovalStatusId == approvedApprovalStatusId &&
//                                         equipment.EquipmentCode.ToLower() == input.ToLower()
//                                   select new
//                                   {
//                                       Id = equipment.Id,
//                                       Value = equipment.EquipmentCode,
//                                       PlantId = equipment.PlantId,
//                                   };
//                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
//                {
//                    allEquipment = allEquipment.Where(x => x.PlantId == Convert.ToInt32(plantId));
//                }
//                var scannedEquipment = await allEquipment.FirstOrDefaultAsync();
//                if (scannedEquipment == null)
//                {
//                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentNotFoundValidation);
//                }
//                else
//                {
//                    var inspectionLotSelectList = await (from equipment in _equipmentRepository.GetAll()
//                                                         join equipmentAssignement in _equipmentAssignmentRepository.GetAll()
//                                                         on equipment.Id equals equipmentAssignement.EquipmentId
//                                                         join cubicleAssignment in _cubicleAssignmentHeaderRepository.GetAll()
//                                                         on equipmentAssignement.CubicleAssignmentHeaderId equals cubicleAssignment.Id
//                                                         join cubicleAssignmentDetail in _cubicleAssignmentDetailRepository.GetAll()
//                                                         on equipmentAssignement.CubicleAssignmentHeaderId equals cubicleAssignmentDetail.CubicleAssignmentHeaderId
//                                                         join inspectionLot in _inspectionLotRepository.GetAll()
//                                                         on cubicleAssignmentDetail.InspectionLotId equals inspectionLot.Id
//                                                         where equipment.Id == scannedEquipment.Id && equipmentAssignement.IsSampling && cubicleAssignment.IsSampling
//                                                         select new
//                                                         {
//                                                             Id = inspectionLot.Id,
//                                                             Value = inspectionLot.InspectionLotNumber,
//                                                             GroupCode = cubicleAssignment.GroupId,
//                                                         }).ToListAsync() ?? default;
//                    if (inspectionLotSelectList.Any())
//                    {
//                        var stagingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
//                        var stagingCompletedInspectionLots = (from inspectionLot in inspectionLotSelectList
//                                                              join materialDispBatch in _materialBatchDispensingHeaderRepository.GetAll()
//                                                              on inspectionLot.GroupCode equals materialDispBatch.GroupCode
//                                                              where materialDispBatch.MaterialBatchDispensingHeaderType == StagingMaterialBatchDispensingHeaderType && materialDispBatch.StatusId == stagingCompletedStatusId
//                                                              && materialDispBatch.IsSampling
//                                                              select new SelectListDto
//                                                              {
//                                                                  Id = inspectionLot.Id,
//                                                                  Value = inspectionLot.Value
//                                                              }).ToList() ?? default;
//                        var groupedResult = stagingCompletedInspectionLots.GroupBy(x => x.Id).Select(x => x.First());
//                        if (groupedResult?.Count() == 0)
//                        {
//                            return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoStagingCompletedInspectionLot);
//                        }
//                        responseDto.ResultObject = new { EquipementId = scannedEquipment.Id, EquipmentBarcode = scannedEquipment.Value, SelectList = groupedResult };
//                    }
//                    else
//                    {
//                        return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoInspectionLotAvailableUnderEquipment);
//                    }
//                    return responseDto;
//                }
//            }
//            return responseDto;
//        }

//        /// <summary>
//        /// Used for getting all material codes under selected inspection lot
//        /// </summary>
//        /// <param name="inspectionLotId"></param>
//        /// <returns></returns>
//        public async Task<HTTPResponseDto> GetMaterialByInspectionLotId(int inspectionLotId)
//        {
//            var today = DateTime.Now;
//            var responseDto = new HTTPResponseDto();
//            var stagingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
//            //todo: material should be approved.
//            //Get staging completed process order
//            var stagingCompletedMaterials = await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
//                                                   join materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
//                                                   on processOrderMaterial.ItemCode equals materialBatchDispensingHeader.MaterialCode
//                                                   where processOrderMaterial.InspectionLotId == inspectionLotId
//                                                    && materialBatchDispensingHeader.StatusId == stagingCompletedStatusId
//                                                         && materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == StagingMaterialBatchDispensingHeaderType
//                                                         && materialBatchDispensingHeader.IsSampling
//                                                   orderby processOrderMaterial.ExpiryDate ascending
//                                                   select new
//                                                   {
//                                                       Id = processOrderMaterial.ItemCode,
//                                                       Value = processOrderMaterial.ItemCode,
//                                                       RetestDate = processOrderMaterial.RetestDate,
//                                                       ExpiryDate = processOrderMaterial.ExpiryDate,
//                                                   })?.ToListAsync() ?? default;
//            var expiredMaterial = stagingCompletedMaterials.Where(x => x.RetestDate <= today || x.ExpiryDate <= today);
//            if (expiredMaterial.Any())
//            {
//                return _dispensingAppService.UpdateErrorResponse(responseDto, $"Material Code:{string.Join(",", expiredMaterial.Select(x => x.Value))} has been expired please update expiry date and retest date");
//            }
//            var materialSelectList = stagingCompletedMaterials.Where(x => x.ExpiryDate > today && x.RetestDate > today).GroupBy(x => x.Id).Select(x => new SelectListDto { Id = x.First().Id, Value = x.First().Value }).ToList();
//            materialSelectList = materialSelectList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
//            if (materialSelectList?.Count == 0)
//            {
//                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderInspectionLotNo);
//            }
//            // var processOrderHaveIssueIndicator = _inspectionLotRepository.GetAll().Where(x => x.Id == inspectionLotId).Select(x => x.IssueIndicator).FirstOrDefault();
//            responseDto.ResultObject = new { MaterialSelectList = materialSelectList };
//            return responseDto;
//        }

//        /// <summary>
//        /// Used for getting all staging completed sap batch number under selected material code.
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public async Task<HTTPResponseDto> UpdateSAPBatchByMaterialCode(MaterialSampleDispensingDto input)
//        {
//            var responseDto = new HTTPResponseDto();
//            var stagingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
//            var sapBatchNumberSelectList = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
//                                                  join processOrderMaterial in _processOrderMaterialRepository.GetAll()
//                                                  on new { MaterialCode = materialBatchDispensingHeader.MaterialCode, SAPBatchNumber = materialBatchDispensingHeader.SAPBatchNumber } equals
//                                                           new { MaterialCode = processOrderMaterial.ItemCode, SAPBatchNumber = processOrderMaterial.SAPBatchNo }
//                                                  where processOrderMaterial.InspectionLotId == input.InspectionLotId
//                                                  && materialBatchDispensingHeader.StatusId == stagingCompletedStatusId && materialBatchDispensingHeader.MaterialCode == input.MaterialCode
//                                                  && materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == StagingMaterialBatchDispensingHeaderType
//                                                  && materialBatchDispensingHeader.IsSampling
//                                                  select new
//                                                  {
//                                                      id = materialBatchDispensingHeader.Id,
//                                                      creationTime = processOrderMaterial.CreationTime,
//                                                      sapBatchNumber = processOrderMaterial.SAPBatchNo,
//                                                  }).ToListAsync() ?? default;
//            input.SapbBatchNumberSelectList = new List<SelectListDto>();
//            input.SapbBatchNumberSelectList = sapBatchNumberSelectList.GroupBy(x => x.sapBatchNumber).Select(x => new SelectListDto { Id = x.First().id, Value = x.First().sapBatchNumber }).ToList();
//            if (!input.SapbBatchNumberSelectList.Any())
//            {
//                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoSAPAvailableUnderGroup);
//            }
//            //sampling header already exist
//            var dispensingHeader = await _dispensingHeaderRepository.GetAll().Where(x => x.InspectionLotId == input.InspectionLotId
//            && x.MaterialCodeId == input.MaterialCode && x.RLAFId == input.RLAFId && x.IsSampling).FirstOrDefaultAsync();
//            var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule, PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
//            var statusId = 0;
//            if (dispensingHeader != null)
//            {
//                input.Id = dispensingHeader.Id;
//                statusId = dispensingHeader.StatusId;
//            }
//            else
//            {
//                input.Id = 0;
//            }
//            responseDto.ResultObject = new { Status = statusId != completedStatusId ? "In Progress" : "Completed", materialDispensingDto = input };
//            return responseDto;
//        }

//        /// <summary>
//        /// Used for Update required qty and balance qty.
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public async Task<MaterialSampleDispensingDto> UpdateRequiredQuantityToSampleDto(MaterialSampleDispensingDto input)
//        {
//            input.RequiredQty = await _processOrderMaterialRepository.GetAll().Where(x => x.InspectionLotId == input.InspectionLotId
//            && x.ItemCode.ToLower() == input.MaterialCode.ToLower()
//                                                         && x.SAPBatchNo.ToLower() == input.SAPBatchNo.ToLower()).Select(x => x.OrderQuantity).SumAsync();
//            var baseUnitOfMeasurement = await _dispensingAppService.GetSamplingBaseUOMAsync(input.MaterialCode, input.InspectionLotId, input.SAPBatchNo);
//            input.BaseUnitOfMeasurementId = baseUnitOfMeasurement.Id;
//            input.BaseUnitOfMeasurement = baseUnitOfMeasurement.UnitOfMeasurement;
//            var lstDetails = await _dispensingDetailRepository.GetAll().Where(x => x.DispensingHeaderId == input.Id && x.SAPBatchNumber == input.SAPBatchNo)
//                 .Select(x => new DispensingNetWeightModel { NoOfPacks = x.NoOfPacks, NetWeight = x.NetWeight, UnitOfMeasurementId = x.UnitOfMeasurementId })
//                 .ToListAsync();
//            if (input.Id != 0 && lstDetails.Any())
//            {
//                input.UnitOfMeasurementId = lstDetails.First().UnitOfMeasurementId;
//                input.BalanceQty = Math.Max(0, (input.RequiredQty - await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value,
//                                                                    input.BaseUnitOfMeasurementId.Value, GetBalanceQuantity(lstDetails).Value)).Value);

//                input.IsAnySAPBatchNoExistForHeader = true;
//            }
//            else
//            {
//                input.BalanceQty = input.RequiredQty;
//            }

//            return input;
//        }

//        /// <summary>
//        /// Used for validating material container barcode.
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="uomId"></param>
//        /// <returns></returns>
//        public async Task<HTTPResponseDto> UpdateMaterialContainerByBarcode(MaterialSampleDispensingDto input)
//        {
//            var responseDto = new HTTPResponseDto();

//            var grnMaterialLabelPrintingContainerBarcode = await _grnLabelPrintingBarcodeRepository.GetAll().Where(x =>
//            x.MaterialLabelContainerBarCode.ToLower() == input.MaterialContainerBarCode
//            && x.BalanceQuantity > 0).FirstOrDefaultAsync();

//            var noofcontainers = await _grnDetail.GetAll().Where(x =>
//            x.Id == grnMaterialLabelPrintingContainerBarcode.GRNDetailId
//           ).FirstOrDefaultAsync();
//            if (grnMaterialLabelPrintingContainerBarcode == null)
//            {
//                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFound);
//            }
//            else
//            {
//                var materialBatchDispensingContainerDetails = await (
//                              from materialBatchContainerDetails in _materialBatchDispensingContainerDetailRepository.GetAll()
//                              where materialBatchContainerDetails.MaterialBatchDispensingHeaderId == input.MaterialBatchDispensingHeaderId
//                              && materialBatchContainerDetails.ContainerBarCode.ToLower() == grnMaterialLabelPrintingContainerBarcode.MaterialLabelContainerBarCode.ToLower()
//                              select new
//                              {
//                                  containerBarcode = materialBatchContainerDetails.ContainerBarCode,
//                              }).FirstOrDefaultAsync();
//                if (materialBatchDispensingContainerDetails == null)
//                {
//                    return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NotStagedContainer);
//                }
//                input.MaterialContainerId = grnMaterialLabelPrintingContainerBarcode.Id;
//                input.MaterialContainerBarCode = materialBatchDispensingContainerDetails.containerBarcode;
//                input.MaterialContainerBalanceQuantity = grnMaterialLabelPrintingContainerBarcode.BalanceQuantity;
//                input.NoOfQuantity = noofcontainers.NoOfContainer;

//            }
//            responseDto.ResultObject = input;
//            return responseDto;
//        }

//        /// <summary>
//        /// Used for getting suggested weighing balance under selected UOM.
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public async Task<List<SelectListDto>> GetAllSuggestedBalancesAsync(int uomId)
//        {
//            return await _dispensingAppService.GetAllSuggestedBalancesAsync(uomId);
//        }
//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Sampling_SubModule + "." + PMMSPermissionConst.Add
//            + "," + PMMSPermissionConst.Sampling_SubModule + "." + PMMSPermissionConst.Edit
//             + "," + PMMSPermissionConst.Sampling_SubModule + "." + PMMSPermissionConst.Print)]
//        public async Task<HTTPResponseDto> PrintSamplingBarcodeAsync(MaterialSampleDispensingDto input)
//        {
//            HTTPResponseDto responseDto = new HTTPResponseDto();
//            if (!await ValidateQuantity(input, responseDto))
//            {
//                return responseDto;
//            }
//            //input.IsSampling = true;
//            //Print dispensing material label
//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                MaterialSampleDispensingLabel labelExtraDetails = await GetExtraLabelData(input.InspectionLotId);

//                var user = await _userRepository.GetAsync(Convert.ToInt32(labelExtraDetails.SampledBy));
//              //  var containerNo = _masterCommonRepository.GetNextSamplingLabelBarcodeSequenceValue();
//                var serialNo = $"{containerNo:D10}";
//                var currentDate = DateTime.UtcNow;
//                input.DispensingBarcode = $"S-{currentDate:yy}-{serialNo}";
//                labelExtraDetails.TareWeight = input.TareWeight.ToString();
//                labelExtraDetails.GrossWeight = Convert.ToString(input.GrossWeight);
//                labelExtraDetails.NetWeight = Convert.ToString(input.NetWeight);
//                labelExtraDetails.SampledBy = user.UserName;
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.DeviceId));
//                await PrintSamplingContainerLabel(input, labelExtraDetails, device);
//            }
//            //Insert/Update into materialdispensingheader/Insert into materialdispensingdetail
//            DispensingDetail dispensingDetail = new DispensingDetail();
//            dispensingDetail.ContainerMaterialBarcode = input.MaterialContainerBarCode;
//            dispensingDetail.DispenseBarcode = input.DispensingBarcode;
//            dispensingDetail.SamplingTypeId = input.SamplingTypeId;
//            if (input.IsPackUOM)
//            {
//                dispensingDetail.NoOfPacks = input.NoOfPacks;
//            }
//            else
//            {
//                dispensingDetail.GrossWeight = input.GrossWeight;
//                dispensingDetail.NetWeight = input.NetWeight;
//                dispensingDetail.TareWeight = input.TareWeight;
//            }

//            dispensingDetail.SAPBatchNumber = input.SAPBatchNo;
//            dispensingDetail.UnitOfMeasurementId = input.UnitOfMeasurementId;
//            dispensingDetail.WeighingMachineId = input.BalanceId;
//            //  dispensingDetail.IsGrossWeight = false;

//            DispensingPrintDetail dispensingPrintDetail = new DispensingPrintDetail();
//            dispensingPrintDetail.DeviceId = input.DeviceId;
//            dispensingPrintDetail.IsController = input.IsController;
//            dispensingDetail.DispensingPrintDetails = new List<DispensingPrintDetail>();
//            dispensingDetail.DispensingPrintDetails.Add(dispensingPrintDetail);
//            dispensingDetail.ContainerNo = Convert.ToInt32("00" + input.SelectedContainer);
//            if (input.Id == 0)
//            {
//                DispensingHeader header = new DispensingHeader();
//                header.MaterialCodeId = input.MaterialCode;
//                header.InspectionLotId = input.InspectionLotId;
//                header.RLAFId = input.RLAFId;
//                header.StartTime = DateTime.Now;
//                header.TenantId = AbpSession.TenantId;
//                header.IsSampling = true;
//                header.StatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule,
//                                                                PMMSConsts.DispensingSubModule, SamplingInProgressStatus);
//                header.DispensingDetails = new List<DispensingDetail>();
//                header.DispensingDetails.Add(dispensingDetail);
//                input.Id = await _dispensingHeaderRepository.InsertAndGetIdAsync(header);
//            }
//            else
//            {
//                dispensingDetail.DispensingHeaderId = input.Id;
//                await _dispensingDetailRepository.InsertAsync(dispensingDetail);
//            }

//            //Update balance quantity from containerbarcodetable
//            var containerForBalanceQuantity = await _grnLabelPrintingBarcodeRepository.GetAsync(input.MaterialContainerId.Value);
//            if (input.IsPackUOM)
//            {
//                containerForBalanceQuantity.BalanceQuantity -= input.ConvertedNoOfPack;
//            }
//            else
//            {
//                containerForBalanceQuantity.BalanceQuantity -= input.ConvertedNetWeight;
//            }
//            await _grnLabelPrintingBarcodeRepository.UpdateAsync(containerForBalanceQuantity);
//            await UnitOfWorkManager.Current.SaveChangesAsync();

//            var lstDetails = await _dispensingDetailRepository.GetAll().Where(x => x.DispensingHeaderId == input.Id && x.SAPBatchNumber == input.SAPBatchNo)
//       .Select(x => new DispensingNetWeightModel { NoOfPacks = x.NoOfPacks, NetWeight = x.NetWeight, UnitOfMeasurementId = x.UnitOfMeasurementId })
//       .ToListAsync();
//            input.BalanceQty = Math.Max(0, (input.RequiredQty - await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value,
//                                                                                       input.BaseUnitOfMeasurementId.Value, GetBalanceQuantity(lstDetails).Value)).Value);
//            input.MaterialContainerBalanceQuantity = containerForBalanceQuantity.BalanceQuantity;
//            input.IsAnySAPBatchNoExistForHeader = true;

//            input.MaterialContainerBalanceQuantity = containerForBalanceQuantity.BalanceQuantity;
//            responseDto.ResultObject = new { Status = "In Progress", materialSamplingDto = input };
//            return responseDto;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.Sampling_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.Sampling_SubModule + "." + PMMSPermissionConst.Edit)]
//        public async Task<HTTPResponseDto> CompleteSamplingAsync(MaterialSampleDispensingDto input)
//        {
//            HTTPResponseDto responseDto = new HTTPResponseDto();
//            if (input.Id == 0)
//            {
//                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.InvalidValueMsg);
//            }
//            var samplingHeader = await _dispensingHeaderRepository.GetAsync(input.Id);
//            samplingHeader.EndTime = DateTime.Now;
//            samplingHeader.IsSampling = true;
//            samplingHeader.StatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule,
//                PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
//            await _dispensingHeaderRepository.UpdateAsync(samplingHeader);
//            responseDto.ResultObject = new { Status = "Completed", materialSamplingDto = input };
//            return responseDto;
//        }

//        public async Task<List<MaterialSampleDispensingDetailDto>> UpdateAllMaterialDispensingDetailBySAPBatch(MaterialSampleDispensingDto input)
//        {
//            return await (from dispensingDetail in _dispensingDetailRepository.GetAll()
//                          join uomMaster in _unitOfMeasurementMasterRepository.GetAll()
//                          on dispensingDetail.UnitOfMeasurementId equals uomMaster.Id
//                          where dispensingDetail.DispensingHeaderId == input.Id
//                          && dispensingDetail.SAPBatchNumber == input.SAPBatchNo
//                          select new MaterialSampleDispensingDetailDto
//                          {
//                              Id = dispensingDetail.Id,
//                              NoOfPacks = dispensingDetail.NoOfPacks,
//                              NetWeight = dispensingDetail.NetWeight,
//                              GrossWeight = dispensingDetail.GrossWeight,
//                              TareWeight = dispensingDetail.TareWeight,
//                              DispensingContainerBarcode = dispensingDetail.DispenseBarcode,
//                              UnitOfMeasurement = uomMaster.UnitOfMeasurement
//                          }).ToListAsync();
//        }
//        public async Task<List<SelectListDto>> GetSamplingTypesAsync()
//        {
//            var samplingTypes = _samplingTypeRepository.GetAll().OrderBy(x => x.Type)
//                       .Select(x => new SelectListDto { Id = x.Id, Value = x.Type });

//            return await samplingTypes.ToListAsync() ?? default;
//        }

//        public async Task<bool> ReprintSamplingDetailAsync(MaterialSampleDispensingDto input)
//        {
//            //Print sampling material label
//            if (_configuration.GetValue<bool>(PMMSConsts.IsPrinterEnabledValue))
//            {
//                MaterialSampleDispensingLabel labelExtraDetails = await GetExtraLabelData(input.InspectionLotId);
//                var samplingDetailToReprint = await _dispensingDetailRepository.GetAsync(input.MaterialSamplingDetailIdToReprint);
//                input.DispensingBarcode = samplingDetailToReprint.DispenseBarcode;
//                labelExtraDetails.TareWeight = Convert.ToString(samplingDetailToReprint.TareWeight);
//                labelExtraDetails.GrossWeight = Convert.ToString(samplingDetailToReprint.GrossWeight);
//                labelExtraDetails.NetWeight = Convert.ToString(samplingDetailToReprint.NetWeight);
//                var device = await _deviceRepository.GetAsync(Convert.ToInt32(input.ReprintDeviceId));
//                await PrintSamplingContainerLabel(input, labelExtraDetails, device);
//            }
//            DispensingPrintDetail printDetail = new DispensingPrintDetail();
//            printDetail.IsController = input.IsController;
//            printDetail.DispensingDetailId = input.MaterialSamplingDetailIdToReprint;
//            printDetail.DeviceId = input.ReprintDeviceId;

//            await _dispensingPrintDetailAppservice.InsertAsync(printDetail);
//            return true;
//        }

//        #endregion public

//        #region private

//        private async Task<bool> ValidateQuantity(MaterialSampleDispensingDto input, HTTPResponseDto responseDto)
//        {
//            if (input.IsPackUOM)
//            {
//                var convertedPackQuantity = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, input.NoOfPacks.Value);
//                input.ConvertedNoOfPack = convertedPackQuantity;
//                if (convertedPackQuantity > input.BalanceQty)
//                {
//                    //Not valid
//                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoOfPackGreaterThanBalanceQuantity);
//                    return false;
//                }
//                if (convertedPackQuantity > input.MaterialContainerBalanceQuantity)
//                {
//                    //Not valid
//                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoOfPackGreaterThanContainerBalanceQuantity);
//                    return false;
//                }
//            }
//            else
//            {
//                var convertedNetQuantity = await _dispensingAppService.GetConvertedQuantityByUOM(input.MaterialCode, input.UnitOfMeasurementId.Value, input.BaseUnitOfMeasurementId.Value, input.NetWeight.Value);
//                input.ConvertedNetWeight = convertedNetQuantity;
//                if (convertedNetQuantity > input.BalanceQty)
//                {
//                    //Not valid
//                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NetWeightGreaterThanBalanceQuantity);
//                    return false;
//                }
//                if (convertedNetQuantity > input.MaterialContainerBalanceQuantity)
//                {
//                    //Not valid
//                    _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NetWeightGreaterThanContainerBalanceQuantity);
//                    return false;
//                }
//            }

//            return true;
//        }

//        private float? GetBalanceQuantity(List<DispensingNetWeightModel> lstDispenseDetails)
//        {
//            var netWeightSum = lstDispenseDetails.Sum(x => x.NetWeight);
//            if (netWeightSum > 0)
//            {
//                return netWeightSum;
//            }
//            else
//            {
//                return lstDispenseDetails.Sum(x => x.NoOfPacks);
//            }
//        }

//        private async Task<Dictionary<string, float?>> GetSAPBatchandRequiredQuantity(MaterialSampleDispensingDto input)
//        {
//            var lstSAPBatchRequiredQuantity = await _processOrderMaterialRepository.GetAll().Where(x => x.InspectionLotId == input.InspectionLotId
//            && x.ItemCode.ToLower() == input.MaterialCode.ToLower())
//                .Select(x => new { x.SAPBatchNo, x.OrderQuantity }).ToListAsync();
//            return lstSAPBatchRequiredQuantity.GroupBy(x => x.SAPBatchNo).Select(x =>
//              new { SAPBatchNumber = x.First().SAPBatchNo, RequiredQuantity = x.Sum(x => x.OrderQuantity) }).ToDictionary(x => x.SAPBatchNumber,
//              x => x.RequiredQuantity);
//        }

//        private async Task PrintSamplingContainerLabel(MaterialSampleDispensingDto input, MaterialSampleDispensingLabel labelExtraDetails, DeviceMaster device)
//        {
//            var printInput = new PrinterInput
//            {
//                IPAddress = device.IpAddress,
//                Port = Convert.ToInt32(device.Port),
//                PrintBody = GetSamplingBarcodeBody(input, labelExtraDetails),
//            };
//            var prnPrinter = _printerFactory.GetPrintConnector(PrinterType.PRN);
//            await prnPrinter.Print(printInput);

//        }

//        private string GetSamplingBarcodeBody(MaterialSampleDispensingDto input, MaterialSampleDispensingLabel labelExtraDetails)
//        {
//            var materialdesc = _materialMasterRepository.GetAll().Where(x => x.MaterialCode == input.MaterialCode).Select(x => x.MaterialDescription).FirstOrDefault();

//            var serialNo = input.DispensingBarcode.Split("-")[2];
//            var materialLabelPRNFilePathForSampling = "";// $"{_environment.WebRootPath}\\label_print_prn\\SAMPLE_ LABEL.prn";
//            var materilLabelPRNFileForSampling = "";// File.ReadAllText(materialLabelPRNFilePathForSampling);
//            var packMaterial = materialdesc + " / " + input.MaterialCode;
//            var QTyrecvd = labelExtraDetails.ReceviedQty + " " + input.BaseUnitOfMeasurement;
//            var Sampledqty = (string.IsNullOrEmpty(input.ConvertedNoOfPack.ToString()) ? input.ConvertedNetWeight.ToString() : input.ConvertedNoOfPack.ToString()) + " " + input.BaseUnitOfMeasurement;
//            var containers = input.SelectedContainer + " Nos.";

//            var args = new Dictionary<string, string>();

//            switch (input.SamplingTypeId)
//            {
//                case 1:

//                    #region Control Sample/Reserve Sample

//                    materialLabelPRNFilePathForSampling = $"{_environment.WebRootPath}\\label_print_prn\\Control_Label.prn";
//                    materilLabelPRNFileForSampling = File.ReadAllText(materialLabelPRNFilePathForSampling);

//                    args = new Dictionary<string, string>(
//           StringComparer.OrdinalIgnoreCase) {
//            {"{Rawmaterial}", materialdesc},
//            {"{MFG}",labelExtraDetails.MfgName },
//            {"{SAPBNo}", input.SAPBatchNo},
//            {"{SampledQty}", Sampledqty},
//            {"{GRNNo}",labelExtraDetails.GrnNo },
//            {"{ARNo}",labelExtraDetails.ArNo },
//            {"{Qty}",QTyrecvd },
//            {"{SampledBoxes}",input.MaterialContainerBarCode },
//            {"{SampledBy}",labelExtraDetails.SampledBy },
//            {"{Date}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{ProductName}", labelExtraDetails.ProductName},
//            {"{BatchNo}", labelExtraDetails.BatchNo},
//            {"{BatchSize}", labelExtraDetails.BatchSize},
//            {"{GrossWeight}", labelExtraDetails.GrossWeight},
//            {"{NetWeight}", labelExtraDetails.NetWeight},
//            {"{TareWeight}", labelExtraDetails.TareWeight},
//            { "{ContainerNo}", serialNo },
//            {"{PlantCode}",labelExtraDetails.PlantId },
//            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{SerialNo}",serialNo },
//            {"{Barcode}",$"{input.DispensingBarcode}"},
//            {"{Mfd}",$"{labelExtraDetails.MfgDate.GetValueOrDefault().ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}"},

//            {"{PlantName}",$"{labelExtraDetails.PlantName}"},};


//                    #endregion

//                    break;
//                case 2:

//                    #region Individual Analysis

//                    materialLabelPRNFilePathForSampling = $"{_environment.WebRootPath}\\label_print_prn\\IndividualAnalysis.prn";
//                    materilLabelPRNFileForSampling = File.ReadAllText(materialLabelPRNFilePathForSampling);

//                    args = new Dictionary<string, string>(
//           StringComparer.OrdinalIgnoreCase) {
//            {"{Rawmaterial}", materialdesc},
//            {"{MFG}",labelExtraDetails.MfgName },
//            {"{SAPBNo}", input.SAPBatchNo},
//            {"{SampledQty}", Sampledqty},
//            {"{GRNNo}",labelExtraDetails.GrnNo },
//            {"{ARNo}",labelExtraDetails.ArNo },
//            {"{Qty}",QTyrecvd },
//            {"{SampledBoxes}",input.MaterialContainerBarCode },
//            {"{SampledBy}",labelExtraDetails.SampledBy },
//            {"{Date}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{ProductName}", labelExtraDetails.ProductName},
//            {"{BatchNo}", labelExtraDetails.BatchNo},
//            {"{BatchSize}", labelExtraDetails.BatchSize},
//            {"{GrossWeight}", labelExtraDetails.GrossWeight},
//            {"{NetWeight}", labelExtraDetails.NetWeight},
//            {"{TareWeight}", labelExtraDetails.TareWeight},
//            //{ "{ContainerNo}", serialNo },
//              { "{ContainerNo}", containers },
//            {"{PlantCode}",labelExtraDetails.PlantId },
//            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{SerialNo}",serialNo },
//            {"{Barcode}",$"{input.DispensingBarcode}"},
//            {"{Mfd}",$"{labelExtraDetails.MfgDate.GetValueOrDefault().ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}"},

//            {"{PlantName}",$"{labelExtraDetails.PlantName}"},};


//                    #endregion

//                    break;
//                case 3:
//                    #region Micro Analysis

//                    materialLabelPRNFilePathForSampling = $"{_environment.WebRootPath}\\label_print_prn\\MicroAnalysis_Label.prn";
//                    materilLabelPRNFileForSampling = File.ReadAllText(materialLabelPRNFilePathForSampling);

//                    args = new Dictionary<string, string>(
//           StringComparer.OrdinalIgnoreCase) {
//            {"{Rawmaterial}", packMaterial},
//            {"{MFG}",labelExtraDetails.MfgName },
//            {"{SAPBNo}", input.SAPBatchNo},
//            {"{SampledQty}", Sampledqty},
//            {"{GRNNo}",labelExtraDetails.GrnNo },
//            {"{ARNo}",labelExtraDetails.ArNo },
//            {"{Qty}",QTyrecvd },
//            {"{SampledBoxes}",input.MaterialContainerBarCode },
//            {"{SampledBy}",labelExtraDetails.SampledBy },
//            {"{Date}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{ProductName}", labelExtraDetails.ProductName},
//            {"{BatchNo}", labelExtraDetails.BatchNo},
//            {"{BatchSize}", labelExtraDetails.BatchSize},
//            {"{GrossWeight}", labelExtraDetails.GrossWeight},
//            {"{NetWeight}", labelExtraDetails.NetWeight},
//            {"{TareWeight}", labelExtraDetails.TareWeight},
//            { "{ContainerNo}", serialNo },
//            {"{PlantCode}",labelExtraDetails.PlantId },
//            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{SerialNo}",serialNo },
//            {"{Barcode}",$"{input.DispensingBarcode}"},
//            {"{Mfd}",$"{labelExtraDetails.MfgDate.GetValueOrDefault().ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}"},

//            {"{PlantName}",$"{labelExtraDetails.PlantName}"},};


//                    #endregion
//                    break;
//                case 4:

//                    #region Chemical Analysis

//                    materialLabelPRNFilePathForSampling = $"{_environment.WebRootPath}\\label_print_prn\\ChemicalAnalysis_label.prn";
//                    materilLabelPRNFileForSampling = File.ReadAllText(materialLabelPRNFilePathForSampling);

//                    args = new Dictionary<string, string>(
//           StringComparer.OrdinalIgnoreCase) {
//            {"{Rawmaterial}", packMaterial},
//            {"{MFG}",labelExtraDetails.MfgName },
//            {"{SAPBNo}", input.SAPBatchNo},
//            {"{SampledQty}", Sampledqty},
//            {"{GRNNo}",labelExtraDetails.GrnNo },
//            {"{ARNo}",labelExtraDetails.ArNo },
//            {"{Qty}",QTyrecvd },
//            {"{SampledBoxes}",input.MaterialContainerBarCode },
//            {"{SampledBy}",labelExtraDetails.SampledBy },
//            {"{Date}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{ProductName}", labelExtraDetails.ProductName},
//            {"{BatchNo}", labelExtraDetails.BatchNo},
//            {"{BatchSize}", labelExtraDetails.BatchSize},
//            {"{GrossWeight}", labelExtraDetails.GrossWeight},
//            {"{NetWeight}", labelExtraDetails.NetWeight},
//            {"{TareWeight}", labelExtraDetails.TareWeight},
//            { "{ContainerNo}", serialNo },
//            {"{PlantCode}",labelExtraDetails.PlantId },
//            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{SerialNo}",serialNo },
//            {"{Barcode}",$"{input.DispensingBarcode}"},
//            {"{Mfd}",$"{labelExtraDetails.MfgDate.GetValueOrDefault().ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}"},

//            {"{PlantName}",$"{labelExtraDetails.PlantName}"},};


//                    #endregion

//                    break;

//                case 5:

//                    #region Packaging Material Sample for Analysis

//                    materialLabelPRNFilePathForSampling = $"{_environment.WebRootPath}\\label_print_prn\\SAMPLE_ LABEL.prn";
//                    materilLabelPRNFileForSampling = File.ReadAllText(materialLabelPRNFilePathForSampling);


//                    args = new Dictionary<string, string>(
//           StringComparer.OrdinalIgnoreCase) {
//                 {"{Rawmaterial}", packMaterial},
//                 {"{MFG}",labelExtraDetails.MfgName },
//                 {"{SAPBNo}", input.SAPBatchNo},
//                 {"{SampledQty}", string.IsNullOrEmpty(input.ConvertedNoOfPack.ToString())?input.ConvertedNetWeight.ToString():input.ConvertedNoOfPack.ToString()},
//                 {"{GRNNo}",labelExtraDetails.GrnNo },
//                 {"{ARNo}",labelExtraDetails.ArNo },
//                 {"{Qty}",QTyrecvd },
//                 {"{SampledBoxes}",input.MaterialContainerBarCode },
//                 {"{SampledBy}",labelExtraDetails.SampledBy },
//                 {"{Date}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//                 {"{FormatNo}",formatNumber},
//                 {"{SOPNo}",sopNo},
//                 {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//                 {"{ProductName}", labelExtraDetails.ProductName},
//                 {"{BatchNo}", labelExtraDetails.BatchNo},
//                 {"{BatchSize}", labelExtraDetails.BatchSize},
//                 {"{GrossWeight}", labelExtraDetails.GrossWeight},
//                 {"{NetWeight}", labelExtraDetails.NetWeight},
//                 {"{TareWeight}", labelExtraDetails.TareWeight},
//                 { "{ContainerNo}", containers },
//                 {"{PlantCode}",labelExtraDetails.PlantId },
//                 {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//                 {"{SerialNo}",serialNo },
//                // {"{Barcode}",$"{input.DispensingBarcode}"},
//                 {"{PlantName}",$"{labelExtraDetails.PlantName}"},};

//                    #endregion

//                    break;
//                case 19:

//                    #region Left Over Sample

//                    materialLabelPRNFilePathForSampling = $"{_environment.WebRootPath}\\label_print_prn\\LeftoverSample_label.prn";
//                    materilLabelPRNFileForSampling = File.ReadAllText(materialLabelPRNFilePathForSampling);

//                    args = new Dictionary<string, string>(
//           StringComparer.OrdinalIgnoreCase) {
//            {"{Rawmaterial}", materialdesc},
//            {"{MatCode}",input.MaterialCode },
//            {"{SAPBNo}", input.SAPBatchNo},
//            {"{SampledQty}", Sampledqty},
//            {"{GRNNo}",labelExtraDetails.GrnNo },
//            {"{ARNo}",labelExtraDetails.ArNo },
//            {"{Qty}",QTyrecvd },
//            {"{SampledBoxes}",input.MaterialContainerBarCode },
//            {"{SampledBy}",labelExtraDetails.SampledBy },
//            {"{Date}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{ProductName}", labelExtraDetails.ProductName},
//            {"{BatchNo}", labelExtraDetails.BatchNo},
//            {"{BatchSize}", labelExtraDetails.BatchSize},
//            {"{GrossWeight}", labelExtraDetails.GrossWeight},
//            {"{NetWeight}", labelExtraDetails.NetWeight},
//            {"{TareWeight}", labelExtraDetails.TareWeight},
//            { "{ContainerNo}", serialNo },
//            {"{PlantCode}",labelExtraDetails.PlantId },
//            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{SerialNo}",serialNo },
//            {"{Barcode}",$"{input.DispensingBarcode}"},
//            {"{Mfd}",$"{labelExtraDetails.MfgDate.GetValueOrDefault().ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}"},

//            {"{PlantName}",$"{labelExtraDetails.PlantName}"},};


//                    #endregion

//                    break;

//                default:

//                    serialNo = input.DispensingBarcode.Split("-")[2];
//                    materialLabelPRNFilePathForSampling = $"{_environment.WebRootPath}\\label_print_prn\\SAMPLE_ LABEL.prn";
//                    materilLabelPRNFileForSampling = File.ReadAllText(materialLabelPRNFilePathForSampling);
//                    args = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
//            {"{Rawmaterial}", input.MaterialCode},
//            {"{MFG}",labelExtraDetails.MfgName },
//            {"{SAPBNo}", input.SAPBatchNo},
//            {"{SampledQty}", input.ConvertedNoOfPack.ToString()==null?input.ConvertedNetWeight.ToString():input.ConvertedNoOfPack.ToString()},
//             {"{GRNNo}",labelExtraDetails.GrnNo },
//            {"{ARNo}",labelExtraDetails.ArNo },
//            {"{Qty}",labelExtraDetails.ReceviedQty },
//            {"{SampledBoxes}",input.MaterialContainerBarCode },
//            {"{SampledBy}",labelExtraDetails.SampledBy },
//            {"{Date}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{FormatNo}",formatNumber},
//            {"{SOPNo}",sopNo},
//            {"{ExpiryDate}",labelExtraDetails.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) },
//            {"{ProductName}", labelExtraDetails.ProductName},
//            {"{BatchNo}", labelExtraDetails.BatchNo},
//            {"{BatchSize}", labelExtraDetails.BatchSize},
//            {"{GrossWeight}", labelExtraDetails.GrossWeight},
//            {"{NetWeight}", labelExtraDetails.NetWeight},
//            {"{TareWeight}", labelExtraDetails.TareWeight},
//            { "{ContainerNo}", serialNo },
//            {"{PlantCode}",labelExtraDetails.PlantId },
//            {"{PrintDate}",DateTime.UtcNow.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) },
//            {"{SerialNo}",serialNo },
//            {"{Barcode}",$"{input.DispensingBarcode}"},
//             {"{PlantName}",$"{labelExtraDetails.PlantName}"},};
//                    break;


//            }


//            var newstr = args.Aggregate(materilLabelPRNFileForSampling, (current, value) => current.Replace(value.Key, value.Value));
//            return newstr;
//        }
//        private async Task<MaterialSampleDispensingLabel> GetExtraLabelData(int? inspectionLotId)
//        {
//            return await (from processOrderMaterial in _processOrderMaterialRepository.GetAll()
//                          join inspectionLot in _inspectionLotRepository.GetAll()
//                          on processOrderMaterial.InspectionLotId equals inspectionLot.Id
//                          join plantMaster in _plantRepository.GetAll()
//                          on inspectionLot.PlantId equals plantMaster.Id
//                          join grndetail in _grnDetail.GetAll()
//                          on processOrderMaterial.SAPBatchNo equals grndetail.SAPBatchNumber
//                          join grnheader in _grnHeader.GetAll()
//                          on grndetail.GRNHeaderId equals grnheader.Id
//                          join grnQtyDetail in _grnQtyDetailRepository.GetAll()
//                          on grndetail.Id equals grnQtyDetail.GRNDetailId
//                          join materialconsignemnt in _materialConsignmentDetail.GetAll()
//                          on grndetail.MfgBatchNoId equals materialconsignemnt.Id
//                          where inspectionLot.Id == inspectionLotId
//                          select new MaterialSampleDispensingLabel
//                          {
//                              PlantId = plantMaster.PlantId,
//                              ExpiryDate = processOrderMaterial.ExpiryDate,
//                              BatchNo = processOrderMaterial.BatchNo,
//                              ProductName = inspectionLot.ProductCode,
//                              ArNo = processOrderMaterial.ARNo,
//                              GrnNo = grnheader.GRNNumber,
//                              ReceviedQty = grnQtyDetail.TotalQty.ToString(),
//                              MfgName = materialconsignemnt.ManufacturedBatchNo,
//                              PlantName = plantMaster.PlantName,
//                              SampledBy = grndetail.CreatorUserId.ToString()
//                          }).FirstOrDefaultAsync();
//        }
//        #endregion private
//    }
//}