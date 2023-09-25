using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;

using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.LineClearances.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.LineClearances
{
    [PMMSAuthorize]
    public class LineClearanceAppService : ApplicationService, ILineClearanceAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<LineClearanceCheckpoint> _lineClearanceCheckPointRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<LineClearanceTransaction> _lineClearanceTransactionRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly string startedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Started).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Verified).ToLower();
        private readonly string approvedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Approved).ToLower();
        private readonly string rejectedStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Rejected).ToLower();
        private readonly string cancelledStatus = nameof(PMMSEnums.LineClearanceHeaderStatus.Cancelled).ToLower();
        #endregion fields

        #region constructor

        public LineClearanceAppService(
            IRepository<CheckpointMaster> checkpointRepository,
            IDispensingAppService dispensingAppService, IRepository<LineClearanceTransaction> lineClearanceTransactionRepository,
            IRepository<LineClearanceCheckpoint> lineClearanceCheckPointRepository, IRepository<User, long> userRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _checkpointRepository = checkpointRepository;
            _dispensingAppService = dispensingAppService;
            _lineClearanceTransactionRepository = lineClearanceTransactionRepository;
            _lineClearanceCheckPointRepository = lineClearanceCheckPointRepository;
            _userRepository = userRepository;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.LineClearance_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<LineClearanceTransactionDto> CreateAsync(CreateLineClearanceTransactionDto input)
        {
            return await CreateLineClearanceTransaction(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingLineClearance_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<LineClearanceTransactionDto> CreateSamplingAsync(CreateLineClearanceTransactionDto input)
        {
            return await CreateLineClearanceTransaction(input, true);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.LineClearance_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UpdateAsync(LineClearanceTransactionDto input)
        {
            await UpdateLineClearanceTransaction(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingLineClearance_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UpdateSamplingAsync(LineClearanceTransactionDto input)
        {
            await UpdateLineClearanceTransaction(input);
        }

        public async Task<LineClearanceTransactionDto> ValidateLineClearanceAsync(int cubicleId, int groupId, bool isSampling)
        {
            var startedStatusMasterId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, startedStatus);
            var approvedStatusMasterId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, approvedStatus);
            var startedAndApprovedStatusList = new List<int> { startedStatusMasterId, approvedStatusMasterId };
            var lineClearanceTransactionData = await _lineClearanceTransactionRepository.FirstOrDefaultAsync(x => x.CubicleId == cubicleId && x.IsSampling == isSampling && startedAndApprovedStatusList.Contains(x.StatusId));
            if (lineClearanceTransactionData != null)
            {
                var allowedStatusList = new List<string> { verifiedStatus, rejectedStatus, cancelledStatus };
                var lineClearanceStatusList = await _dispensingAppService.GetStatusListByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule);
                var lineClearanceNotInProgressStatusList = lineClearanceStatusList.Where(x => allowedStatusList.Contains(x.Status.ToLower())).Select(x => x.Id);
                var lineClearanceTransaction = await _lineClearanceTransactionRepository.FirstOrDefaultAsync(x => x.CubicleId == cubicleId && x.GroupId == groupId && x.IsSampling == isSampling && !lineClearanceNotInProgressStatusList.Contains(x.StatusId));
                if (lineClearanceTransaction != null)
                {
                    var data = ObjectMapper.Map<LineClearanceTransactionDto>(lineClearanceTransaction);
                    data.CreatorName = _userRepository.FirstOrDefault(x => x.Id == lineClearanceTransaction.CreatorUserId).FullName;
                    if (data.StatusId == startedStatusMasterId)
                    {
                        data.CanApproved = lineClearanceTransaction.CreatorUserId != (int)AbpSession.UserId;
                    }
                    else if (data.StatusId == approvedStatusMasterId)
                    {
                        data.ApprovedByName = _userRepository.FirstOrDefault(x => x.Id == lineClearanceTransaction.ApprovedBy).FullName;
                        data.CanVerified = data.ApprovedBy != (int)AbpSession.UserId && lineClearanceTransaction.CreatorUserId != (int)AbpSession.UserId;
                    }
                    GetLineClearanceCheckPoints(data);
                    return data;
                }
                else
                {
                    return new LineClearanceTransactionDto
                    {
                        IsInValidTransaction = true,
                        LineClearanceCheckpoints = new List<CheckpointDto>()
                    };
                }
            }
            var lineClearanceTransactionDto = new LineClearanceTransactionDto
            {
                LineClearanceCheckpoints = new List<CheckpointDto>()
            };
            return lineClearanceTransactionDto;
        }

        #endregion public

        #region private

        private async Task<LineClearanceTransactionDto> CreateLineClearanceTransaction(CreateLineClearanceTransactionDto input, bool isSampling)
        {
            var startedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, startedStatus);
            var lineClearanceTransaction = ObjectMapper.Map<LineClearanceTransaction>(input);
            lineClearanceTransaction.TenantId = AbpSession.TenantId;
            lineClearanceTransaction.ClearanceDate = DateTime.Now;
            lineClearanceTransaction.StartTime = DateTime.Now;
            lineClearanceTransaction.StatusId = startedStatusId;
            lineClearanceTransaction.IsSampling = isSampling;
            await _lineClearanceTransactionRepository.InsertAsync(lineClearanceTransaction);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<LineClearanceTransactionDto>(lineClearanceTransaction);
        }

        private async Task UpdateLineClearanceTransaction(LineClearanceTransactionDto input)
        {
            var lineClearanceTransaction = await _lineClearanceTransactionRepository.GetAsync(input.Id);
            if (input.IsVerified)
            {
                var verfiedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, verifiedStatus);
                input.StatusId = verfiedStatusId;
                input.VerifiedBy = (int)AbpSession.UserId;
                input.StopTime = DateTime.Now;
            }
            else if (input.IsApproved)
            {
                var approvedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, approvedStatus);
                input.ApprovedBy = (int)AbpSession.UserId;
                input.StatusId = approvedStatusId;
                input.ApprovedTime = DateTime.Now;
            }
            else
            {
                var rejectStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.LineClearanceSubModule, rejectedStatus);
                input.StatusId = rejectStatusId;
                input.StopTime = DateTime.Now;
            }
            ObjectMapper.Map(input, lineClearanceTransaction);
            await _lineClearanceTransactionRepository.UpdateAsync(lineClearanceTransaction);
        }

        private void GetLineClearanceCheckPoints(LineClearanceTransactionDto lineclearanceCheckpoint)
        {
            lineclearanceCheckpoint.LineClearanceCheckpoints = (from detail in _lineClearanceCheckPointRepository.GetAll()
                                                                join checkpoint in _checkpointRepository.GetAll()
                                                                on detail.CheckPointId equals checkpoint.Id
                                                                where detail.LineClearanceTransactionId == lineclearanceCheckpoint.Id
                                                                select new CheckpointDto
                                                                {
                                                                    Id = detail.Id,
                                                                    CheckPointId = checkpoint.Id,
                                                                    CheckpointName = checkpoint.CheckpointName,
                                                                    ValueTag = checkpoint.ValueTag,
                                                                    AcceptanceValue = checkpoint.AcceptanceValue,
                                                                    CheckpointTypeId = checkpoint.CheckpointTypeId,
                                                                    ModeId = checkpoint.ModeId,
                                                                    Observation = detail.Observation,
                                                                    DiscrepancyRemark = detail.Remark,
                                                                }).ToList() ?? default;
        }

        #endregion private
    }
}