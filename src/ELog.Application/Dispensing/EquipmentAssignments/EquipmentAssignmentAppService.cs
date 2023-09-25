using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.CommonService.Dispensing.Dto;
using ELog.Application.Dispensing.EquipmentAssignments.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Dispensing.EquipmentAssignments
{
    [PMMSAuthorize]
    public class EquipmentAssignmentAppService : ApplicationService, IEquipmentAssignmentAppService
    {
        public const string NotInUse = "Not in Use";
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignmentDetailRepository;
        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignmentHeaderRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<EquipmentCleaningStatus> _equipmentCleaningStatusRepository;
        private readonly IRepository<EquipmentAssignment> _equipmentAssignmentRepository;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<StatusMaster> _statusMasterRepository;
        private readonly IRepository<ProcessOrder> _processOrderRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly string InProgressStatus = nameof(PMMSEnums.CubicleAssignementDetailStatus.InProgress).ToLower();
        private readonly string CleanedStatus = nameof(PMMSEnums.EquipmentCleaningHeaderStatus.Cleaned).ToLower();
        private readonly string GroupOpenStatus = nameof(PMMSEnums.CubicleAssignmentGroupStatus.Open).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.EquipmentCleaningHeaderStatus.Verified).ToLower();
        private readonly string DispensingCompletedStatus = nameof(DispensingHeaderStatus.Completed).ToLower();
        private readonly string SamplingCompletedStatus = nameof(SamplingHeaderStatus.Completed).ToLower();
        public EquipmentAssignmentAppService(
          IHttpContextAccessor httpContextAccessor,
          IRepository<EquipmentAssignment> equipmentAssignmentRepository,
           IRepository<CubicleAssignmentDetail> cubicleAssignmentDetailRepository,
             IRepository<CubicleAssignmentHeader> cubicleAssignmentHeaderRepository,
             IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
             IRepository<CubicleMaster> cubicleRepository,
             IRepository<EquipmentMaster> equipmentRepository,
             IRepository<EquipmentCleaningStatus> equipmentCleaningStatusRepository,
             IDispensingAppService dispensingAppService,
             IRepository<DispensingHeader> dispensingHeaderRepository,
             IRepository<StatusMaster> statusMasterRepository,
              IRepository<ProcessOrder> processOrderRepository
             )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;

            _httpContextAccessor = httpContextAccessor;
            _equipmentAssignmentRepository = equipmentAssignmentRepository;
            _cubicleAssignmentDetailRepository = cubicleAssignmentDetailRepository;
            _cubicleAssignmentHeaderRepository = cubicleAssignmentHeaderRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _cubicleRepository = cubicleRepository;
            _equipmentRepository = equipmentRepository;
            _equipmentCleaningStatusRepository = equipmentCleaningStatusRepository;
            _dispensingAppService = dispensingAppService;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _statusMasterRepository = statusMasterRepository;
            _processOrderRepository = processOrderRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<EquipmentAssignmentDto> GetCubicleAsync(string input)
        {
            return await GetCubicleInternalAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingEquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingEquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<EquipmentAssignmentDto> GetCubicleForSamplingAsync(string input)
        {
            return await GetCubicleInternalAsync(input, true);
        }

        private async Task<EquipmentAssignmentDto> GetCubicleInternalAsync(string input, bool IsSampling)
        {
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
                var cubicleQuery = from cubicles in _cubicleRepository.GetAll()
                                   where cubicles.ApprovalStatusId == approvedApprovalStatusId && cubicles.IsActive && cubicles.CubicleCode.ToLower() == input.ToLower()
                                   select new EquipmentAssignmentDto
                                   {
                                       CubicleId = cubicles.Id,
                                       CubicleCode = cubicles.CubicleCode,
                                       PlantId = cubicles.PlantId,
                                   };
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    cubicleQuery = cubicleQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
                }
                var cubicle = await cubicleQuery.FirstOrDefaultAsync();
                if (cubicle != null)
                {
                    var openGroupStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleAssignmentSubModule, GroupOpenStatus);
                    var inprogressDetailStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.CubicleAssignmentSubModule, InProgressStatus);
                    var lstCubicleItemDetailsQuery = from cubicleHeader in _cubicleAssignmentHeaderRepository.GetAll()
                                                     join cubicleDetail in _cubicleAssignmentDetailRepository.GetAll()
                                                     on cubicleHeader.Id equals cubicleDetail.CubicleAssignmentHeaderId
                                                     join processOrderMaterials in _processOrderMaterialRepository.GetAll()
                                                     on cubicleDetail.ProcessOrderMaterialId equals processOrderMaterials.Id
                                                     where cubicleDetail.CubicleId == cubicle.CubicleId
                                                     && cubicleDetail.StatusId == inprogressDetailStatus
                                                     && cubicleHeader.GroupStatusId == openGroupStatus
                                                     select new
                                                     {
                                                         cubicleHeader.GroupId,
                                                         cubicleHeader.ProductCode,
                                                         processOrderMaterials.BatchNo,
                                                         processOrderMaterials.ItemDescription,
                                                         cubicleDetail.CubicleAssignmentHeaderId,
                                                         cubicleHeader.IsSampling,
                                                         processOrderMaterials.ProcessOrderId,
                                                         processOrderMaterials.SAPBatchNo
                                                     };
                    lstCubicleItemDetailsQuery = lstCubicleItemDetailsQuery.Where(x => x.IsSampling == IsSampling);
                    var lstCubicleItemDetails = await lstCubicleItemDetailsQuery?.ToListAsync() ?? default;
                    var isReservationNumber = await _processOrderRepository.GetAll().Where(x => x.Id == lstCubicleItemDetails.Select(x => x.ProcessOrderId).FirstOrDefault()).Select(x => x.IsReservationNo).FirstOrDefaultAsync();
                    if (lstCubicleItemDetails?.Count > 0)
                    {
                        cubicle.BatchNumber = String.Join(",", lstCubicleItemDetails.Select(x => x.BatchNo).Distinct());
                        cubicle.GroupId = lstCubicleItemDetails.Select(x => x.GroupId).FirstOrDefault();
                        cubicle.ProductCode = lstCubicleItemDetails.Select(x => x.ProductCode).FirstOrDefault();
                        cubicle.CubicleAssignmentHeaderId = lstCubicleItemDetails.Select(x => x.CubicleAssignmentHeaderId).FirstOrDefault();
                        cubicle.SAPBatchNo = lstCubicleItemDetails.Select(x => x.SAPBatchNo).FirstOrDefault();
                        cubicle.IsReservationNo = isReservationNumber;
                        var lstEquipmentQuery = from equipmentAssignment in _equipmentAssignmentRepository.GetAll()
                                                join equipment in _equipmentRepository.GetAll()
                                                on equipmentAssignment.EquipmentId equals equipment.Id
                                                where equipmentAssignment.Cubicleid == cubicle.CubicleId
                                                && equipmentAssignment.CubicleAssignmentHeaderId == cubicle.CubicleAssignmentHeaderId
                                                select new EquipmentNameDto
                                                {
                                                    Id = equipmentAssignment.Id,
                                                    EquipmentBarCode = equipment.EquipmentCode,
                                                    EquipmentId = equipment.Id,
                                                    EquipmentName = equipment.Name,
                                                    IsSampling = equipmentAssignment.IsSampling,
                                                    EquipmentType = equipment.IsPortable == true ? PMMSConsts.Portable : PMMSConsts.Fixed,
                                                };
                        lstEquipmentQuery = lstEquipmentQuery.Where(x => x.IsSampling == IsSampling);
                        cubicle.lstEquipments = await lstEquipmentQuery?.ToListAsync() ?? default;
                        var fixedEquipmentQuery = from cubiclemaster in _cubicleRepository.GetAll()
                                                  join equipmentmaster in _equipmentRepository.GetAll()
                                                  on cubiclemaster.SLOCId equals equipmentmaster.SLOCId
                                                  where cubiclemaster.Id == cubicle.CubicleId
                                                  && (equipmentmaster.IsPortable == false || equipmentmaster.IsPortable == null)
                                                  select new EquipmentNameDto
                                                  {
                                                      Id = equipmentmaster.Id,
                                                      EquipmentBarCode = equipmentmaster.EquipmentCode,
                                                      EquipmentId = equipmentmaster.Id,
                                                      EquipmentName = equipmentmaster.Name,
                                                      EquipmentType = equipmentmaster.IsPortable == true ? PMMSConsts.Portable : PMMSConsts.Fixed,
                                                  };
                        cubicle.FixedEquipments = await fixedEquipmentQuery?.ToListAsync() ?? default;
                        return cubicle;
                    }
                    else
                    {
                        return default;
                    }
                }
            }
            return default;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> GetAllEquipmentAssignmentBarcodeAsync(EquipmentBarcodeRequestDto input)
        {
            return await GetAllEquipmentAssignmentBarCodeInternalAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> GetAllEquipmentAssignmentBarcodeForSamplingAsync(EquipmentBarcodeRequestDto input)
        {
            return await GetAllEquipmentAssignmentBarCodeInternalAsync(input, true);
        }

        private async Task<HTTPResponseDto> GetAllEquipmentAssignmentBarCodeInternalAsync(EquipmentBarcodeRequestDto input, bool IsSampling)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            if (!(string.IsNullOrEmpty(input.EquipmentBarcode) || string.IsNullOrWhiteSpace(input.EquipmentBarcode)))
            {
                var equipmentCleaningVerifiedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, verifiedStatus);
                var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

                var equipmentDetail = await _equipmentRepository.GetAll().Where(x => x.ApprovalStatusId == approvedApprovalStatusId && x.IsActive
                 && x.EquipmentCode.ToLower() == input.EquipmentBarcode.ToLower()
                                      && x.IsPortable == true).Select(x => new { x.Id, x.EquipmentCode, x.Name, x.PlantId, x.CleanHoldTime })
                                     .FirstOrDefaultAsync();

                var equipmentAssigned = (from equipment in _equipmentRepository.GetAll()
                                         join equipmentStatus in _equipmentCleaningStatusRepository.GetAll()
                                         on equipment.Id equals equipmentStatus.EquipmentId
                                         join statusMaster in _statusMasterRepository.GetAll()
                                         on equipmentStatus.StatusId equals statusMaster.Id
                                         where equipmentStatus.IsSampling == IsSampling && equipment.EquipmentCode.ToLower() == input.EquipmentBarcode.ToLower()
                                         select statusMaster.Status == EquipmentCleaningHeaderStatus.Verified.ToString() ? EquipmentCleaningHeaderStatus.Cleaned.ToString() :
                                         statusMaster.Status == EquipmentCleaningHeaderStatus.Cleaned.ToString() ? EquipmentCleaningHeaderStatus.Cleaned.ToString() :
                                         statusMaster.Status == EquipmentCleaningHeaderStatus.Started.ToString() ? EquipmentCleaningHeaderStatus.Uncleaned.ToString() :
                                        NotInUse).ToList();

                if (equipmentDetail == null)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentNotFound, equipmentAssigned.Count == 0 ? NotInUse : equipmentAssigned[0]);
                }
                var isAnyEquipmentExist = await _equipmentAssignmentRepository.GetAll().Where(equipmentAssignment =>
                                                 equipmentAssignment.EquipmentId == equipmentDetail.Id
                                                 && equipmentAssignment.Cubicleid == input.CubicleId
                                                 && equipmentAssignment.GroupId == input.GroupId
                                                 && equipmentAssignment.IsSampling == IsSampling

                                                 ).AnyAsync();

                if (isAnyEquipmentExist)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentAlreadyAssigned, equipmentAssigned.Count == 0 ? NotInUse : equipmentAssigned[0]);
                }

                var equipmentQuery = from cleanedEquipmentStatus in _equipmentCleaningStatusRepository.GetAll()
                                     join equipment in _equipmentRepository.GetAll()
                                     on cleanedEquipmentStatus.EquipmentId equals equipment.Id
                                     where cleanedEquipmentStatus.EquipmentId == equipmentDetail.Id
                                     && cleanedEquipmentStatus.StatusId == equipmentCleaningVerifiedStatusId
                                     select new EquipmentAssignmentBarcodeDto
                                     {
                                         EquipmentId = equipmentDetail.Id,
                                         EquipmentBarcode = equipmentDetail.EquipmentCode,
                                         EquipmentName = equipmentDetail.Name,
                                         PlantId = equipmentDetail.PlantId,
                                         LastCleaningDate = cleanedEquipmentStatus.CleaningDate,
                                         CleanHoldTime = equipmentDetail.CleanHoldTime,
                                         IsSampling = cleanedEquipmentStatus.IsSampling,
                                         EquipmentType = equipment.IsPortable == true ? PMMSConsts.Portable : PMMSConsts.Fixed,
                                         Status = equipmentAssigned.Count == 0 ? NotInUse : equipmentAssigned[0]
                                     };
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    equipmentQuery = equipmentQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
                }
                equipmentQuery = equipmentQuery.Where(x => x.IsSampling == IsSampling);
                var equipmentToAssign = await equipmentQuery.FirstOrDefaultAsync();
                if (equipmentToAssign == null)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentNotCleaned, equipmentAssigned.Count == 0 ? NotInUse : equipmentAssigned[0]);
                }
                if (equipmentToAssign.LastCleaningDate == null)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentNotCleaned, equipmentAssigned.Count == 0 ? NotInUse : equipmentAssigned[0]);
                }
                if (equipmentToAssign.LastCleaningDate.Value.Date.AddDays(equipmentToAssign.CleanHoldTime).CompareTo(DateTime.Now.Date) < 0)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentNotCleaned, equipmentAssigned.Count == 0 ? NotInUse : equipmentAssigned[0]);
                }
                responseDto.ResultObject = equipmentToAssign;
                return responseDto;
            }
            return default;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> AssignEquipmentAsync(EquipmentAssignmentDto input)
        {
            return await AssignEquipmentinternalAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingEquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingEquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> AssignEquipmentForSamplingAsync(EquipmentAssignmentDto input)
        {
            return await AssignEquipmentinternalAsync(input, true);
        }

        private async Task<HTTPResponseDto> AssignEquipmentinternalAsync(EquipmentAssignmentDto input, bool IsSampling)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            var lstEquipmentToAssign = input.lstEquipments.Where(x => x.IsAssignedOrDeAssigned && x.Id > 0);
            if (lstEquipmentToAssign?.Count() > 0)
            {
                var lstEquipments = lstEquipmentToAssign.Select(x => (int?)x.EquipmentId).ToList();
                var isAnyEquipmentExist = await _equipmentAssignmentRepository.GetAll().Where(equipmentAssignment => equipmentAssignment.Cubicleid == input.CubicleId
                                                  && equipmentAssignment.GroupId == input.GroupId
                                                  && equipmentAssignment.IsSampling == IsSampling
                                                  && lstEquipments.Contains(equipmentAssignment.EquipmentId))
                                            .AnyAsync();

                if (isAnyEquipmentExist)
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.OneOrMoreEquipmentAlreadyAssigned, "");
                }
                foreach (var item in input.lstEquipments.Where(x => x.IsAssignedOrDeAssigned && x.Id > 0))
                {
                    EquipmentAssignment assignment = new EquipmentAssignment();
                    assignment.CubicleAssignmentHeaderId = input.CubicleAssignmentHeaderId;
                    assignment.Cubicleid = input.CubicleId;
                    assignment.EquipmentId = item.EquipmentId;
                    assignment.GroupId = input.GroupId;
                    assignment.IsSampling = IsSampling;
                    item.Id = await _equipmentAssignmentRepository.InsertAndGetIdAsync(assignment);
                }
            }
            else if (input.lstEquipments.Any(x => x.IsAssignedOrDeAssigned))
            {
                return UpdateErrorResponse(responseDto, PMMSValidationConst.OneOrMoreEquipmentAlreadyAssigned, "");
            }
            input.lstEquipments.ForEach(item =>
            {
                item.IsAssignedOrDeAssigned = false;
            });
            responseDto.ResultObject = input;
            return responseDto;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.EquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> DeAssignEquipmentAsync(EquipmentAssignmentDto input)
        {
            return await DeAssignEquipmentInternalAsync(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingEquipmentAssignment_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.SamplingEquipmentAssignment_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<HTTPResponseDto> DeAssignEquipmentForSamplingAsync(EquipmentAssignmentDto input)
        {
            return await DeAssignEquipmentInternalAsync(input, true);
        }

        private async Task<HTTPResponseDto> DeAssignEquipmentInternalAsync(EquipmentAssignmentDto input, bool IsSampling)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            var lstEquipmentToDeassign = input.lstEquipments.Where(x => x.IsAssignedOrDeAssigned && x.Id > 0);
            if (lstEquipmentToDeassign?.Count() > 0)
            {
                var completedStatudId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule,
                 PMMSConsts.DispensingSubModule, DispensingCompletedStatus);
                var samplingCompletedStatus = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.SamplingSubModule,
                 PMMSConsts.SamplingSubModule, SamplingCompletedStatus);
                var lstEquipmentId = lstEquipmentToDeassign.Select(x => x.EquipmentId).ToList();
                if (IsSampling && await _dispensingHeaderRepository.GetAll().AnyAsync(x => lstEquipmentId.Contains(x.RLAFId) && x.StatusId != samplingCompletedStatus))
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.SampleEquipmentCannotDeassigned, "");
                }
                else if (await _dispensingHeaderRepository.GetAll().AnyAsync(x => lstEquipmentId.Contains(x.RLAFId) && x.StatusId != completedStatudId))
                {
                    return UpdateErrorResponse(responseDto, PMMSValidationConst.EquipmentCannotDeassigned, "");
                }
            }

            foreach (var item in input.lstEquipments.Where(x => x.IsAssignedOrDeAssigned && x.Id > 0))
            {
                await _equipmentAssignmentRepository.DeleteAsync(item.Id);
            }
            input.lstEquipments = input.lstEquipments.Where(x => !x.IsAssignedOrDeAssigned)?.ToList();
            responseDto.ResultObject = input;
            return responseDto;
        }

        private HTTPResponseDto UpdateErrorResponse(HTTPResponseDto responseDto, string ValidationError, string Status)
        {
            responseDto.Result = (int)HttpStatusCode.PreconditionFailed;
            responseDto.Error = ValidationError;
            responseDto.Status = Status;
            return responseDto;
        }
    }
}