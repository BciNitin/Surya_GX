using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.Picking.Dto;
using ELog.Application.Dispensing.PreStage.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Dispensing.PreStage
{
    [PMMSAuthorize]
    public class PreStageAppService : ApplicationService, IPreStageAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<MaterialBatchDispensingHeader> _materialBatchDispensingHeaderRepository;
        private readonly IRepository<MaterialBatchDispensingContainerDetail> _materialBatchDispensingContainerDetailRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly string PreStageInProgressStatus = nameof(PMMSEnums.PreStageHeaderStatus.InProgress).ToLower();
        private readonly string CompletedStatus = nameof(PMMSEnums.PreStageHeaderStatus.Completed).ToLower();
        private readonly int PickingMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.Picking;
        private readonly int PreStageMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.PreStaging;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;

        #endregion fields

        #region constructor

        public PreStageAppService(IHttpContextAccessor httpContextAccessor,
            IRepository<MaterialBatchDispensingHeader> materialBatchDispensingHeaderRepository,
            IRepository<MaterialBatchDispensingContainerDetail> materialBatchDispensingContainerDetailRepository, IMasterCommonRepository masterCommonRepository,
            IRepository<CubicleMaster> cubicleRepository, IDispensingAppService dispensingAppService, IRepository<ProcessOrderMaterial> processOrderMaterialRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _masterCommonRepository = masterCommonRepository;
            _materialBatchDispensingHeaderRepository = materialBatchDispensingHeaderRepository;
            _materialBatchDispensingContainerDetailRepository = materialBatchDispensingContainerDetailRepository;
            _cubicleRepository = cubicleRepository;
            _dispensingAppService = dispensingAppService;
            _processOrderMaterialRepository = processOrderMaterialRepository;
        }

        #endregion constructor

        #region public

        /// <summary>
        /// Used for getting cubicle by barcode under dispensing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetCubicleByBarcode(string input, bool isSampling)
        {
            return await GetCubicleByBarcodeInternalAsync(input, isSampling);
        }

        /// <summary>
        /// Used to complete pre stage of required materials from dispensing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.Prestage_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.Prestage_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> CompletePreStageAsync(PreStageDto input)
        {
            return await CompletePreStageInternalAsync(input, false);
        }

        /// <summary>
        /// Used to complete pre stage of required materials from sampling
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingPrestage_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingPrestage_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> CompletePreStageForSamplingAsync(PreStageDto input)
        {
            return await CompletePreStageInternalAsync(input, true);
        }

        /// <summary>
        /// Used for getting picking completed materials.
        /// </summary>
        /// <param name="cubicleCode"></param>
        /// /// <param name="groupCode"></param>
        /// /// <param name="isSampling"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetPickedMaterialCode(string cubicleCode, string groupCode, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            var pickingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
            //Get all material code for which picking completed.
            var pickingCompletedMaterials = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                                   join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                   on materialBatchDispensingHeader.MaterialCode equals processOrderMaterial.ItemCode
                                                   where materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                                   && materialBatchDispensingHeader.CubicleCode.ToLower() == cubicleCode.ToLower() &&
                                                   materialBatchDispensingHeader.GroupCode.ToLower() == groupCode.ToLower() &&
                                                   materialBatchDispensingHeader.StatusId == pickingCompletedStatusId && materialBatchDispensingHeader.IsSampling == isSampling
                                                   select new SelectListDto { Id = processOrderMaterial.Id, Value = processOrderMaterial.ItemCode })?.ToListAsync() ?? default;
            pickingCompletedMaterials = pickingCompletedMaterials.GroupBy(x => x.Value).Select(x => x.First()).ToList();
            if (pickingCompletedMaterials?.Count == 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderGroup);
            }
            responseDto.ResultObject = pickingCompletedMaterials.Distinct();
            return responseDto;
        }

        public async Task<HTTPResponseDto> GetPickedSAPBatchNos(string cubicleCode, string groupCode, string materialCode, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            var pickingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
            //Get all material code for which picking completed.
            var pickingCompletedSapBatchNos = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                                     where materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                                     && materialBatchDispensingHeader.CubicleCode.ToLower() == cubicleCode.ToLower() &&
                                                     materialBatchDispensingHeader.GroupCode.ToLower() == groupCode.ToLower() &&
                                                     materialBatchDispensingHeader.MaterialCode.ToLower() == materialCode.ToLower() && materialBatchDispensingHeader.StatusId == pickingCompletedStatusId &&
                                                     materialBatchDispensingHeader.IsSampling == isSampling
                                                     select new SelectListDto { Id = materialBatchDispensingHeader.Id, Value = materialBatchDispensingHeader.SAPBatchNumber })?.ToListAsync() ?? default;
            pickingCompletedSapBatchNos = pickingCompletedSapBatchNos.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            if (pickingCompletedSapBatchNos?.Count == 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.NoSapBatchNoAvailableUnderMaterial);
            }
            var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, CompletedStatus);
            var materialStatus = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.CubicleCode.ToLower() == cubicleCode.ToLower()
                                         && x.MaterialCode.ToLower() == materialCode.ToLower()
                                         && x.GroupCode.ToLower() == groupCode.ToLower()
                                         && x.IsSampling == isSampling
                                         && x.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType)
                                             .Select(x => new { x.StatusId, x.Id }).FirstOrDefaultAsync();
            var StatusId = 0;
            var PreStageMaterialId = 0;
            if (materialStatus != null)
            {
                StatusId = materialStatus.StatusId;
                PreStageMaterialId = materialStatus.Id;
            }
            responseDto.ResultObject = new { Status = StatusId != completedStatusId ? "In Progress" : "Completed", PreStageMaterialId, Selectlist = pickingCompletedSapBatchNos };
            return responseDto;
        }

        /// <summary>
        /// Used when selecting material code to Get PreStage details on UI
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PreStageDto> UpdatePreStageDetailAsync(PreStageDto input, bool isSampling)
        {
            var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
            var cubicleAssignMaterials = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                                join materialBatchContainerDetail in _materialBatchDispensingContainerDetailRepository.GetAll()
                                                on materialBatchDispensingHeader.Id equals materialBatchContainerDetail.MaterialBatchDispensingHeaderId
                                                where materialBatchDispensingHeader.Id == input.MaterialBatchDispensingHeaderId && materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                                && materialBatchDispensingHeader.IsSampling == isSampling
                                                && materialBatchDispensingHeader.StatusId == completedStatusId
                                                select new PickingValidationDto
                                                {
                                                    Id = materialBatchDispensingHeader.Id,
                                                    Value = materialBatchDispensingHeader.MaterialCode,
                                                    RequiredQty = materialBatchContainerDetail.Quantity,
                                                    CreationTime = materialBatchContainerDetail.CreationTime,
                                                }).Distinct().ToListAsync() ?? default;
            var cubicleAssignMaterialsWithRequiredQty = cubicleAssignMaterials
                                                                     .OrderBy(a => a.CreationTime)
                                                                     .GroupBy(d => new { d.Value })
                                                                     .Select(x => new PickingValidationDto
                                                                     {
                                                                         Id = x.First().Id,
                                                                         Value = x.First().Value,
                                                                         RequiredQty = x.Sum(s => s.RequiredQty),
                                                                     }).FirstOrDefault() ?? default;
            //PreStage header already exist
            var preStagingHeader = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
            && x.CubicleCode == input.CubicleCode
            && x.MaterialCode == input.MaterialCode && x.SAPBatchNumber == input.SAPBatchNo && x.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType && x.IsSampling == isSampling).FirstOrDefaultAsync();
            if (preStagingHeader != null)
            {
                input.Id = preStagingHeader.Id;
                var preStageCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, CompletedStatus);
                input.IsCompletePreStageAllowed = preStagingHeader.StatusId != preStageCompletedStatus;
                input.ContainerCount = await _materialBatchDispensingContainerDetailRepository.GetAll()
               .Where(x => x.MaterialBatchDispensingHeaderId == preStagingHeader.Id).CountAsync();
                input.Quantity = await _materialBatchDispensingContainerDetailRepository.GetAll()
                    .Where(x => x.MaterialBatchDispensingHeaderId == preStagingHeader.Id).SumAsync(x => x.Quantity);
            }
            else
            {
                input.ContainerCount = 0;
                input.Quantity = 0;
                input.IsCompletePreStageAllowed = true;
            }
            if (cubicleAssignMaterialsWithRequiredQty != null)
            {
                input.RequiredQty = cubicleAssignMaterialsWithRequiredQty.RequiredQty;
            }
            return input;
        }

        /// <summary>
        /// Used to save scan materials details from dispensing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.Prestage_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.Prestage_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> SavePickedMaterialContainerDetailsAsync(PreStageDto input)
        {
            return await SavePickedMaterialContainerDetailsInternalAsync(input, false);
        }

        /// <summary>
        /// Used to save scan materials details from sampling
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingPrestage_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingPrestage_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> SavePickedMaterialContainerDetailsForSamplingAsync(PreStageDto input)
        {
            return await SavePickedMaterialContainerDetailsInternalAsync(input, true);
        }

        #endregion public

        #region private

        private async Task<HTTPResponseDto> CompletePreStageInternalAsync(PreStageDto input, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            //Get PreStage Batch Completed records
            var batchPreStageCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, CompletedStatus);
            var batchPreStageCompletedMaterialHeaderList = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
              && x.CubicleCode == input.CubicleCode && x.BatchPickingStatusId == batchPreStageCompletedStatus
              && x.MaterialCode == input.MaterialCode && x.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType && x.IsSampling == isSampling).ToListAsync();

            //Get Picking Completed Material Record's count.
            var pickingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
            var pickingCompletedMaterialCount = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
                                 && x.CubicleCode == input.CubicleCode && x.StatusId == pickingCompletedStatus
                                 && x.MaterialCode == input.MaterialCode && x.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType && x.IsSampling == isSampling).CountAsync();
            //Check All picked material count and Batch PreStageCompletedMaterial.

            if (pickingCompletedMaterialCount != batchPreStageCompletedMaterialHeaderList.Count())
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.PreStageCompletionNotAllowed);
            }
            else
            {
                var preStagecompletedId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, CompletedStatus);
                batchPreStageCompletedMaterialHeaderList.ForEach(x => x.StatusId = preStagecompletedId);
                await _masterCommonRepository.BulkUpdateMaterialBatchDispensingHeader(batchPreStageCompletedMaterialHeaderList);
                responseDto.ResultObject = batchPreStageCompletedMaterialHeaderList;
            }
            return responseDto;
        }

        private async Task<HTTPResponseDto> GetCubicleByBarcodeInternalAsync(string input, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
                //Get all cubicle by barcode
                var allCubicles = from cubicle in _cubicleRepository.GetAll()
                                  where cubicle.IsActive && cubicle.ApprovalStatusId == approvedApprovalStatusId
                                  && cubicle.IsActive && cubicle.CubicleCode.ToLower() == input.ToLower()
                                  select new PickingValidationDto
                                  {
                                      Id = cubicle.Id,
                                      Value = cubicle.CubicleCode,
                                      PlantId = cubicle.PlantId,
                                      AssignedGroups = null,
                                  };
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    allCubicles = allCubicles.Where(x => x.PlantId == Convert.ToInt32(plantId));
                }
                var scannedCubicle = await allCubicles.FirstOrDefaultAsync();
                if (scannedCubicle == null)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleNotFoundValidation);
                }
                else
                {
                    var pickingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PickingSubModule, CompletedStatus);
                    var pickingCompletdGroups = await (from materialBatchDispensingHeaders in _materialBatchDispensingHeaderRepository.GetAll()
                                                       where materialBatchDispensingHeaders.CubicleCode.ToLower() == scannedCubicle.Value.ToLower() && materialBatchDispensingHeaders.StatusId == pickingCompletedStatus &&
                                                       materialBatchDispensingHeaders.IsSampling == isSampling && materialBatchDispensingHeaders.MaterialBatchDispensingHeaderType == PickingMaterialBatchDispensingHeaderType
                                                       select materialBatchDispensingHeaders).ToListAsync() ?? default;
                    var pcikingCompleteGroups = new List<SelectListDto>();
                    if (pickingCompletdGroups.Any())
                    {
                        pcikingCompleteGroups = pickingCompletdGroups.GroupBy(x => x.GroupCode).Select(a => new SelectListDto { Value = a.Key }).ToList();
                        scannedCubicle.AssignedGroups = pcikingCompleteGroups.Distinct().ToList();
                    }
                    else
                    {
                        return UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleAssignGroupPickingNotDone);
                    }
                    responseDto.ResultObject = scannedCubicle;
                    return responseDto;
                }
            }
            return responseDto;
        }

        private async Task<HTTPResponseDto> SavePickedMaterialContainerDetailsInternalAsync(PreStageDto input, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            var materialBatchDispensingContainerDetails = await (
                from materialBatchContainerDetails in _materialBatchDispensingContainerDetailRepository.GetAll()
                where materialBatchContainerDetails.MaterialBatchDispensingHeaderId == input.MaterialBatchDispensingHeaderId
                && materialBatchContainerDetails.ContainerBarCode.ToLower() == input.MaterialContainerBarCode.ToLower()
                select new
                {
                    ContainerLabelId = materialBatchContainerDetails.Id,
                    SAPBatchNo = materialBatchContainerDetails.SAPBatchNumber,
                    Quantity = materialBatchContainerDetails.Quantity,
                }).FirstOrDefaultAsync();
            if (materialBatchDispensingContainerDetails == null)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFound);
            }
            int preStageHeaderId = await GetPreStageHeaderId(input, isSampling);

            var alreadyExistPreStageContainerId = await (from containerDetail in _materialBatchDispensingContainerDetailRepository.GetAll()
                                                         where containerDetail.MaterialBatchDispensingHeaderId == preStageHeaderId
                                                         && containerDetail.ContainerBarCode == input.MaterialContainerBarCode
                                                         select containerDetail.Id).FirstOrDefaultAsync();
            if (alreadyExistPreStageContainerId > 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerAlreadyScanned);
            }
            var materialBatchContainerDetail = new MaterialBatchDispensingContainerDetail();
            materialBatchContainerDetail.MaterialBatchDispensingHeaderId = preStageHeaderId;
            materialBatchContainerDetail.ContainerBarCode = input.MaterialContainerBarCode;
            materialBatchContainerDetail.Quantity = materialBatchDispensingContainerDetails.Quantity;
            materialBatchContainerDetail.ContainerPickingTime = DateTime.Now;
            materialBatchContainerDetail.SAPBatchNumber = materialBatchDispensingContainerDetails.SAPBatchNo;
            await _materialBatchDispensingContainerDetailRepository.InsertAndGetIdAsync(materialBatchContainerDetail);
            input.ContainerCount = await _materialBatchDispensingContainerDetailRepository.GetAll()
                .Where(x => x.MaterialBatchDispensingHeaderId == preStageHeaderId).CountAsync();
            input.Quantity = await _materialBatchDispensingContainerDetailRepository.GetAll()
                .Where(x => x.MaterialBatchDispensingHeaderId == preStageHeaderId).SumAsync(x => x.Quantity);
            //update batch pre-staging status
            input.Id = preStageHeaderId;
            if (input.Quantity == input.RequiredQty)
            {
                var materialBatchHeader = await _materialBatchDispensingHeaderRepository.GetAsync(preStageHeaderId);
                ObjectMapper.Map(input, materialBatchHeader);
                var batchPickingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, CompletedStatus);
                materialBatchHeader.BatchPickingStatusId = batchPickingCompletedStatus;
                await _materialBatchDispensingHeaderRepository.UpdateAsync(materialBatchHeader);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            input.MaterialBatchDispensingHeaderId = input.MaterialBatchDispensingHeaderId;
            responseDto.ResultObject = input;
            return responseDto;
        }

        private async Task<int> GetPreStageHeaderId(PreStageDto input, bool isSampling)
        {
            //Pre stage header already exist
            var preStageHeaderId = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode.ToLower() == input.GroupId.ToLower()
            && x.CubicleCode.ToLower() == input.CubicleCode.ToLower() && x.SAPBatchNumber.ToLower() == input.SAPBatchNo.ToLower()
            && x.MaterialCode == input.MaterialCode && x.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType && x.IsSampling == isSampling).Select(x => x.Id).FirstOrDefaultAsync();
            if (preStageHeaderId == 0)
            {
                var preStageHeader = new MaterialBatchDispensingHeader();
                preStageHeader.CubicleCode = input.CubicleCode;
                preStageHeader.GroupCode = input.GroupId;
                preStageHeader.MaterialCode = input.MaterialCode;
                preStageHeader.SAPBatchNumber = input.SAPBatchNo;
                preStageHeader.PickingTime = DateTime.Now;
                preStageHeader.MaterialBatchDispensingHeaderType = PreStageMaterialBatchDispensingHeaderType;
                preStageHeader.IsSampling = isSampling;
                var inProgressStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, PreStageInProgressStatus);
                preStageHeader.BatchPickingStatusId = inProgressStatusId;
                preStageHeader.StatusId = inProgressStatusId;
                preStageHeader.TenantId = AbpSession.TenantId;
                preStageHeaderId = await _materialBatchDispensingHeaderRepository.InsertAndGetIdAsync(preStageHeader);
            }
            return preStageHeaderId;
        }

        private HTTPResponseDto UpdateErrorResponse(HTTPResponseDto responseDto, String ValidationError)
        {
            responseDto.Result = (int)HttpStatusCode.PreconditionFailed;
            responseDto.Error = ValidationError;
            return responseDto;
        }

        #endregion private
    }
}