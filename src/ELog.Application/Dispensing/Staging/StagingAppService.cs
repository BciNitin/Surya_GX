using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.Picking.Dto;
using ELog.Application.Dispensing.Stage.Dto;
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

namespace ELog.Application.Dispensing.Stage
{
    [PMMSAuthorize]
    public class StageAppService : ApplicationService, IStagingAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<MaterialBatchDispensingHeader> _materialBatchDispensingHeaderRepository;
        private readonly IRepository<MaterialBatchDispensingContainerDetail> _materialBatchDispensingContainerDetailRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;

        private readonly string StagingInProgressStatus = nameof(PMMSEnums.StageHeaderStatus.InProgress).ToLower();
        private readonly string StagingCompletedStatus = nameof(PMMSEnums.StageHeaderStatus.Completed).ToLower();

        private readonly int StageMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.Staging;
        private readonly int PreStageMaterialBatchDispensingHeaderType = (int)MaterialBatchdispensingHeaderType.PreStaging;
        private readonly string PreStageCompletedStatus = nameof(PMMSEnums.PreStageHeaderStatus.Completed).ToLower();

        #endregion fields

        #region constructor

        public StageAppService(IHttpContextAccessor httpContextAccessor,
            IRepository<MaterialBatchDispensingHeader> materialBatchDispensingHeaderRepository,
            IRepository<MaterialBatchDispensingContainerDetail> materialBatchDispensingContainerDetailRepository, IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
            IRepository<CubicleMaster> cubicleRepository, IDispensingAppService dispensingAppService, IMasterCommonRepository masterCommonRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _materialBatchDispensingHeaderRepository = materialBatchDispensingHeaderRepository;
            _materialBatchDispensingContainerDetailRepository = materialBatchDispensingContainerDetailRepository;
            _cubicleRepository = cubicleRepository;
            _dispensingAppService = dispensingAppService;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _masterCommonRepository = masterCommonRepository;
        }

        #endregion constructor

        #region public

        public async Task<HTTPResponseDto> GetCubicleByBarcode(string input, bool isSampling)
        {
            return await GetCubicleByBarcodeInternalAsync(input, isSampling);
        }

        /// <summary>
        /// Used to complete staging of required materials from dispensing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.Staging_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.Staging_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> CompleteStagingAsync(StagingDto input)
        {
            return await CompleteStagingInternalAsync(input, false);
        }

        /// <summary>
        /// Used to complete staging of required materials from sampling
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingStaging_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingStaging_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> CompleteStagingForSamplingAsync(StagingDto input)
        {
            return await CompleteStagingInternalAsync(input, true);
        }

        /// <summary>
        /// Used for getting pre stage completed materials.
        /// </summary>
        /// <param name="cubicleCode"></param>
        /// <param name="groupCode"></param>
        /// <param name="isSampling"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetPreStageMaterialCode(string cubicleCode, string groupCode, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            var preStageCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, PreStageCompletedStatus);
            var preStageCompletdMaterialCodes = _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.CubicleCode == cubicleCode && x.GroupCode == groupCode && x.IsSampling == isSampling && x.StatusId == preStageCompletedStatusId).Select(x => x.MaterialCode);
            //Get all material code for which picking completed.
            var pickingCompletedMaterials = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                                   join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                   on materialBatchDispensingHeader.MaterialCode equals processOrderMaterial.ItemCode
                                                   where materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType
                                                   && materialBatchDispensingHeader.CubicleCode.ToLower() == cubicleCode.ToLower() &&
                                                   materialBatchDispensingHeader.GroupCode.ToLower() == groupCode.ToLower() &&
                                                   materialBatchDispensingHeader.StatusId == preStageCompletedStatusId && materialBatchDispensingHeader.IsSampling == isSampling
                                                   select new SelectListDto { Id = processOrderMaterial.Id, Value = processOrderMaterial.ItemCode })?.ToListAsync() ?? default;
            pickingCompletedMaterials = pickingCompletedMaterials.GroupBy(x => x.Value).Select(x => x.First()).ToList();
            if (pickingCompletedMaterials?.Count == 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.NoMaterialAvailableUnderGroup);
            }
            responseDto.ResultObject = pickingCompletedMaterials;
            return responseDto;
        }

        /// <summary>
        /// Used for getting pre stage completed materials sap batch nos.
        /// </summary>
        /// <param name="cubicleCode"></param>
        /// <param name="groupCode"></param>
        /// <param name="materialCode"></param>
        /// <param name="isSampling"></param>
        /// <returns></returns>
        public async Task<HTTPResponseDto> GetPreStageSAPBatchNos(string cubicleCode, string groupCode, string materialCode, bool isSampling)
        {
            var responseDto = new HTTPResponseDto();
            var preStageCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, PreStageCompletedStatus);
            //Get all material code for which picking completed.
            var pickingCompletedSapBatchNos = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                                     where materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType
                                                     && materialBatchDispensingHeader.CubicleCode.ToLower() == cubicleCode.ToLower() &&
                                                     materialBatchDispensingHeader.GroupCode.ToLower() == groupCode.ToLower() &&
                                                     materialBatchDispensingHeader.MaterialCode.ToLower() == materialCode.ToLower() && materialBatchDispensingHeader.StatusId == preStageCompletedStatusId &&
                                                     materialBatchDispensingHeader.IsSampling == isSampling
                                                     select new SelectListDto { Id = materialBatchDispensingHeader.Id, Value = materialBatchDispensingHeader.SAPBatchNumber })?.ToListAsync() ?? default;
            pickingCompletedSapBatchNos = pickingCompletedSapBatchNos.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            if (pickingCompletedSapBatchNos?.Count == 0)
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.NoSapBatchNoAvailableUnderMaterial);
            }
            var completedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
            var materialStatus = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.CubicleCode.ToLower() == cubicleCode.ToLower()
                                         && x.MaterialCode.ToLower() == materialCode.ToLower()
                                         && x.GroupCode.ToLower() == groupCode.ToLower()
                                         && x.IsSampling == isSampling
                                         && x.MaterialBatchDispensingHeaderType == StageMaterialBatchDispensingHeaderType)
                                             .Select(x => new { x.StatusId, x.Id }).FirstOrDefaultAsync();
            var StatusId = 0;
            var StagingMaterialId = 0;
            if (materialStatus != null)
            {
                StatusId = materialStatus.StatusId;
                StagingMaterialId = materialStatus.Id;
            }
            responseDto.ResultObject = new { Status = StatusId != completedStatusId ? "In Progress" : "Completed", StagingMaterialId, Selectlist = pickingCompletedSapBatchNos };
            return responseDto;
        }

        /// <summary>
        /// Used when selecting material code and sap batch no to get stage details on UI
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isSampling"></param>
        /// <returns></returns>
        public async Task<StagingDto> UpdateStagingDetailAsync(StagingDto input, bool isSampling)
        {
            var preStageCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, StagingCompletedStatus);
            var cubicleAssignMaterials = await (from materialBatchDispensingHeader in _materialBatchDispensingHeaderRepository.GetAll()
                                                join materialBatchContainerDetail in _materialBatchDispensingContainerDetailRepository.GetAll()
                                                on materialBatchDispensingHeader.Id equals materialBatchContainerDetail.MaterialBatchDispensingHeaderId
                                                where materialBatchDispensingHeader.Id == input.MaterialBatchDispensingHeaderId && materialBatchDispensingHeader.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType
                                                && materialBatchDispensingHeader.IsSampling == isSampling
                                                && materialBatchDispensingHeader.StatusId == preStageCompletedStatusId
                                                select new PickingValidationDto
                                                {
                                                    Id = materialBatchDispensingHeader.Id,
                                                    Value = materialBatchDispensingHeader.MaterialCode,
                                                    RequiredQty = materialBatchContainerDetail.Quantity,
                                                    CreationTime = materialBatchContainerDetail.CreationTime
                                                }).Distinct().ToListAsync() ?? default;

            var cubicleAssignMaterialsWithSapBatchNosAndRequiredQty = cubicleAssignMaterials
                                                                    .OrderBy(a => a.CreationTime)
                                                                    .GroupBy(d => new { d.Value })
                                                                    .Select(x => new PickingValidationDto
                                                                    {
                                                                        Id = x.First().Id,
                                                                        Value = x.First().Value,
                                                                        RequiredQty = x.Sum(s => s.RequiredQty),
                                                                    }).FirstOrDefault() ?? default;
            //Stage header already exist
            var stagingHeader = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
            && x.CubicleCode == input.CubicleCode && x.SAPBatchNumber == input.SAPBatchNo
            && x.MaterialCode == input.MaterialCode && x.MaterialBatchDispensingHeaderType == StageMaterialBatchDispensingHeaderType && x.IsSampling == isSampling).FirstOrDefaultAsync();
            if (stagingHeader != null)
            {
                input.Id = stagingHeader.Id;
                var stagingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
                input.IsCompleteStagingAllowed = stagingHeader.StatusId != stagingCompletedStatus;
                input.ContainerCount = await _materialBatchDispensingContainerDetailRepository.GetAll()
               .Where(x => x.MaterialBatchDispensingHeaderId == stagingHeader.Id).CountAsync();
                input.Quantity = await _materialBatchDispensingContainerDetailRepository.GetAll()
                    .Where(x => x.MaterialBatchDispensingHeaderId == stagingHeader.Id).SumAsync(x => x.Quantity);
            }
            else
            {
                input.ContainerCount = 0;
                input.Quantity = 0; input.IsCompleteStagingAllowed = true;
            }
            if (cubicleAssignMaterialsWithSapBatchNosAndRequiredQty != null)
            {
                input.RequiredQty = cubicleAssignMaterialsWithSapBatchNosAndRequiredQty.RequiredQty;
            }
            return input;
        }

        /// <summary>
        /// Used to save scan materials details from dispensing
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isSampling"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.Staging_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.Staging_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> SaveStagingMaterialContainerDetailsAsync(StagingDto input)
        {
            return await SaveStagingMaterialContainerDetailsInternalAsync(input, false);
        }

        /// <summary>
        /// Used to save scan materials details from sampling
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isSampling"></param>
        /// <returns></returns>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingStaging_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingStaging_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> SaveStagingMaterialContainerDetailsForSamplingAsync(StagingDto input)
        {
            return await SaveStagingMaterialContainerDetailsInternalAsync(input, true);
        }

        #endregion public

        #region private

        private async Task<HTTPResponseDto> CompleteStagingInternalAsync(StagingDto input, bool IsSampling)
        {
            var responseDto = new HTTPResponseDto();
            //get Staging Batch Completed records
            var batchStagingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
            var batchStagingCompletedMaterialHeaderList = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
              && x.CubicleCode == input.CubicleCode && x.BatchPickingStatusId == batchStagingCompletedStatus
              && x.MaterialCode == input.MaterialCode && x.MaterialBatchDispensingHeaderType == StageMaterialBatchDispensingHeaderType && x.IsSampling == IsSampling).ToListAsync();

            //get pre stage Completed Material Records count.
            var preStagingCompletedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, StagingCompletedStatus);
            var preStageCompletedMaterialCount = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode == input.GroupId
                                 && x.CubicleCode == input.CubicleCode && x.StatusId == preStagingCompletedStatusId
                                 && x.MaterialCode == input.MaterialCode && x.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType && x.IsSampling == IsSampling).CountAsync();
            //Check All pre stge material count and Batch StagingCompletedMaterial.

            if (preStageCompletedMaterialCount != batchStagingCompletedMaterialHeaderList.Count())
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.StagingCompletionNotAllowed);
            }
            else
            {
                var preStageCompletedId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
                batchStagingCompletedMaterialHeaderList.ForEach(x => x.StatusId = preStageCompletedId);
                await _masterCommonRepository.BulkUpdateMaterialBatchDispensingHeader(batchStagingCompletedMaterialHeaderList);
                responseDto.ResultObject = batchStagingCompletedMaterialHeaderList;
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
                    var preStageCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.PreStageSubModule, StagingCompletedStatus);
                    var preStageCompletedGroups = await (from materialBatchDispensingHeaders in _materialBatchDispensingHeaderRepository.GetAll()
                                                         where materialBatchDispensingHeaders.CubicleCode.ToLower() == scannedCubicle.Value.ToLower() && materialBatchDispensingHeaders.StatusId == preStageCompletedStatus &&
                                                         materialBatchDispensingHeaders.IsSampling == isSampling && materialBatchDispensingHeaders.MaterialBatchDispensingHeaderType == PreStageMaterialBatchDispensingHeaderType
                                                         select materialBatchDispensingHeaders).ToListAsync() ?? default;
                    var preStageCompleteGroups = new List<SelectListDto>();
                    if (preStageCompletedGroups.Any())
                    {
                        preStageCompleteGroups = preStageCompletedGroups.GroupBy(x => x.GroupCode).Select(a => new SelectListDto { Value = a.Key }).ToList();
                        scannedCubicle.AssignedGroups = preStageCompleteGroups.Distinct().ToList();
                    }
                    else
                    {
                        return UpdateErrorResponse(responseDto, PMMSValidationConst.CubicleAssignGroupPreStageNotDone);
                    }
                    responseDto.ResultObject = scannedCubicle;
                    return responseDto;
                }
            }
            return responseDto;
        }

        private async Task<HTTPResponseDto> SaveStagingMaterialContainerDetailsInternalAsync(StagingDto input, bool isSampling)
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
            int preStageHeaderId = await GetStagingHeaderId(input, isSampling);

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
            input.Id = preStageHeaderId;
            //update batch staging status
            if (input.Quantity == input.RequiredQty)
            {
                var materialBatchHeader = await _materialBatchDispensingHeaderRepository.GetAsync(preStageHeaderId);
                ObjectMapper.Map(input, materialBatchHeader);
                var batchPickingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingCompletedStatus);
                materialBatchHeader.BatchPickingStatusId = batchPickingCompletedStatus;
                await _materialBatchDispensingHeaderRepository.UpdateAsync(materialBatchHeader);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            input.MaterialBatchDispensingHeaderId = input.MaterialBatchDispensingHeaderId;
            responseDto.ResultObject = input;
            return responseDto;
        }

        private async Task<int> GetStagingHeaderId(StagingDto input, bool isSampling)
        {
            //Pre stage header already exist
            var stagingHeaderId = await _materialBatchDispensingHeaderRepository.GetAll().Where(x => x.GroupCode.ToLower() == input.GroupId.ToLower()
            && x.CubicleCode.ToLower() == input.CubicleCode.ToLower() && x.SAPBatchNumber.ToLower() == input.SAPBatchNo.ToLower()
            && x.MaterialCode == input.MaterialCode && x.MaterialBatchDispensingHeaderType == StageMaterialBatchDispensingHeaderType && x.IsSampling == isSampling).Select(x => x.Id).FirstOrDefaultAsync();
            if (stagingHeaderId == 0)
            {
                var stagingHeader = new MaterialBatchDispensingHeader();
                stagingHeader.CubicleCode = input.CubicleCode;
                stagingHeader.GroupCode = input.GroupId;
                stagingHeader.MaterialCode = input.MaterialCode;
                stagingHeader.SAPBatchNumber = input.SAPBatchNo;
                stagingHeader.PickingTime = DateTime.Now;
                stagingHeader.MaterialBatchDispensingHeaderType = StageMaterialBatchDispensingHeaderType;
                stagingHeader.IsSampling = isSampling;
                var inProgressStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.StagingSubModule, StagingInProgressStatus);
                stagingHeader.BatchPickingStatusId = inProgressStatusId;
                stagingHeader.StatusId = inProgressStatusId;
                stagingHeader.TenantId = AbpSession.TenantId;
                stagingHeaderId = await _materialBatchDispensingHeaderRepository.InsertAndGetIdAsync(stagingHeader);
            }
            return stagingHeaderId;
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