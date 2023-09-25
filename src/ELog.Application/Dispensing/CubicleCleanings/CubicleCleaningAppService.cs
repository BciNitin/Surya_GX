using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;

using ELog.Application.Dispensing.CubicleCleanings.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Modules;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.CubicleCleanings
{
    [PMMSAuthorize]
    public class CubicleCleaningAppService : ApplicationService, ICubicleCleaningAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<CubicleCleaningTransaction> _cubicleCleaningTransactionRepository;
        private readonly IRepository<CubicleCleaningDailyStatus> _cubicleCleaningDailyStatusRepository;
        private readonly IRepository<CubicleCleaningCheckpoint> _cubicleCleaningCheckpointRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<StatusMaster> _statusMasterRepository;
        private readonly IModuleAppService _moduleAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly string startedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Started).ToLower();
        private readonly string cleanedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Cleaned).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Verified).ToLower();
        private readonly string uncleanStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Uncleaned).ToLower();
        #endregion fields

        #region constructor

        public CubicleCleaningAppService(IRepository<CubicleCleaningTransaction> cubicleCleaningTransactionRepository,
            IRepository<CubicleCleaningDailyStatus> cubicleCleaningDailyStatusRepository,
            IModuleAppService moduleAppService, IRepository<CubicleCleaningCheckpoint> cubicleCleaningCheckpointRepository,
        IRepository<StatusMaster> statusMasterRepository, IRepository<CheckpointMaster> checkpointRepository,
        IRepository<User, long> userRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _cubicleCleaningTransactionRepository = cubicleCleaningTransactionRepository;
            _cubicleCleaningDailyStatusRepository = cubicleCleaningDailyStatusRepository;
            _cubicleCleaningCheckpointRepository = cubicleCleaningCheckpointRepository;
            _moduleAppService = moduleAppService;
            _statusMasterRepository = statusMasterRepository;
            _checkpointRepository = checkpointRepository;
            _userRepository = userRepository;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleCleaning_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<CubicleCleaningTransactionDto> CreateAsync(CreateCubicleCleaningTransactionDto input)
        {
            return await InsertAsync(input, false);
        }
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingCubicleCleaning_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<CubicleCleaningTransactionDto> CreateSamplingAsync(CreateCubicleCleaningTransactionDto input)
        {
            return await InsertAsync(input, true);
        }
        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleCleaning_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UpdateAsync(CubicleCleaningTransactionDto input)
        {
            await UpdateDataAsync(input, false);
        }
        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingCubicleCleaning_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UpdateSamplingAsync(CubicleCleaningTransactionDto input)
        {
            await UpdateDataAsync(input, true);
        }
        private async Task InsertOrUpdateDailyStatus(CubicleCleaningTransaction cubicleCleaningTransaction, bool isSampling)
        {
            var CleaningDate = cubicleCleaningTransaction.CleaningDate;
            var cubicleCleaningDailyStatus = await _cubicleCleaningDailyStatusRepository.FirstOrDefaultAsync(x => x.CubicleId == cubicleCleaningTransaction.CubicleId
             && x.CleaningDate.Day == CleaningDate.Day && x.CleaningDate.Month == CleaningDate.Month && x.CleaningDate.Year == CleaningDate.Year && x.IsSampling == isSampling);
            if (cubicleCleaningDailyStatus != null)
            {
                cubicleCleaningDailyStatus.StatusId = cubicleCleaningTransaction.StatusId;
                cubicleCleaningDailyStatus.TenantId = AbpSession.TenantId;
                await _cubicleCleaningDailyStatusRepository.UpdateAsync(cubicleCleaningDailyStatus);
            }
            else
            {
                var cubicleDailyStatus = new CubicleCleaningDailyStatus
                {
                    StatusId = cubicleCleaningTransaction.StatusId,
                    CubicleId = cubicleCleaningTransaction.CubicleId,
                    CleaningDate = cubicleCleaningTransaction.CleaningDate,
                    TenantId = AbpSession.TenantId,
                    IsSampling = isSampling
                };
                await _cubicleCleaningDailyStatusRepository.InsertAsync(cubicleDailyStatus);
            }
        }

        public async Task<CubicleCleaningTransactionDto> ValidateCubicleStatusAsync(int cubicleId, int type, bool isSampling)
        {
            var cubicleCleaningTransactionData = new CubicleCleaningTransactionDto
            {
                IsUncleaned = true,
                IsInValidTransaction = false,
                CubicleCleaningCheckpoints = new List<CheckpointDto>()
            };
            var localDate = DateTime.Now;
            var verfiedStatusId = await GetCubicleCleaningStatus(verifiedStatus);
            var cleanedStatusId = await GetCubicleCleaningStatus(cleanedStatus);
            var uncleanStatus = await GetCubicleCleaningStatus(this.uncleanStatus);
            var cubicleCleaningDailystatus = await _cubicleCleaningDailyStatusRepository.FirstOrDefaultAsync(x => x.CubicleId == cubicleId && x.CleaningDate.Day == localDate.Day && x.CleaningDate.Month == localDate.Month && x.CleaningDate.Year == localDate.Year && x.StatusId != verfiedStatusId.Id && x.StatusId != uncleanStatus.Id && x.IsSampling == isSampling);
            if (cubicleCleaningDailystatus != null)
            {
                var cubicleCleaningTransaction = await _cubicleCleaningTransactionRepository.FirstOrDefaultAsync(x => x.CubicleId == cubicleId && x.CleaningDate.Day == localDate.Day && x.CleaningDate.Month == localDate.Month && x.CleaningDate.Year == localDate.Year && x.TypeId == type && x.StatusId != verfiedStatusId.Id && x.IsSampling == isSampling);
                if (cubicleCleaningTransaction != null)
                {
                    var data = ObjectMapper.Map<CubicleCleaningTransactionDto>(cubicleCleaningTransaction);
                    data.IsUncleaned = false;
                    data.CreatorName = _userRepository.FirstOrDefault(x => x.Id == cubicleCleaningTransaction.CreatorUserId).FullName;
                    if (data.StatusId == cleanedStatusId.Id)
                    {
                        GetCubicleCleaningCheckPoints(data);
                        data.CanApproved = data.CleanerId != (int)AbpSession.UserId;
                        data.CleanerName = _userRepository.FirstOrDefault(x => x.Id == cubicleCleaningTransaction.CleanerId).FullName;
                    }
                    return data;
                }
                else
                {
                    cubicleCleaningTransactionData.IsInValidTransaction = true;
                    return cubicleCleaningTransactionData;
                }
            }

            return cubicleCleaningTransactionData;
        }

        #endregion public

        #region private

        private void GetCubicleCleaningCheckPoints(CubicleCleaningTransactionDto dailyStatusDto)
        {
            dailyStatusDto.CubicleCleaningCheckpoints = (from detail in _cubicleCleaningCheckpointRepository.GetAll()
                                                         join checkpoint in _checkpointRepository.GetAll()
                                                         on detail.CheckPointId equals checkpoint.Id
                                                         where detail.CubicleCleaningTransactionId == dailyStatusDto.Id
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

        private async Task<StatusMaster> GetCubicleCleaningStatus(string statusValue)
        {
            var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.CubicleCleaningSubModule);
            var statusMaster = await _statusMasterRepository.FirstOrDefaultAsync(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == statusValue);
            return statusMaster;
        }
        private async Task<CubicleCleaningTransactionDto> InsertAsync(CreateCubicleCleaningTransactionDto input, bool isSampling)
        {
            DateTime localDate = DateTime.Now;
            var statusMaster = await GetCubicleCleaningStatus(startedStatus);
            var cubicleCleaningTransaction = ObjectMapper.Map<CubicleCleaningTransaction>(input);
            cubicleCleaningTransaction.TenantId = AbpSession.TenantId;
            cubicleCleaningTransaction.CleaningDate = localDate;
            cubicleCleaningTransaction.StartTime = localDate;
            cubicleCleaningTransaction.StatusId = statusMaster.Id;
            cubicleCleaningTransaction.CleanerId = (int)AbpSession.UserId;
            cubicleCleaningTransaction.IsSampling = isSampling;
            await _cubicleCleaningTransactionRepository.InsertAsync(cubicleCleaningTransaction);
            await InsertOrUpdateDailyStatus(cubicleCleaningTransaction, isSampling);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<CubicleCleaningTransactionDto>(cubicleCleaningTransaction);
        }
        private async Task UpdateDataAsync(CubicleCleaningTransactionDto input, bool isSampling)
        {
            var cubicleTransaction = await _cubicleCleaningTransactionRepository.GetAsync(input.Id);
            if (input.IsVerified)
            {
                var verfiedStatusMaster = await GetCubicleCleaningStatus(verifiedStatus);
                input.StatusId = verfiedStatusMaster.Id;
                input.VerifierId = (int)AbpSession.UserId;
                input.VerifiedTime = DateTime.Now;
                input.StopTime = cubicleTransaction.StopTime;
                //input.DoneBy = cubicleTransaction.DoneBy;
            }
            else if (input.IsRejected)
            {
                var verfiedStatusMaster = await GetCubicleCleaningStatus(uncleanStatus);
                input.VerifierId = (int)AbpSession.UserId;
                input.VerifiedTime = DateTime.Now;
                input.StatusId = verfiedStatusMaster.Id;
            }
            else
            {
                var cleanedStatuMaster = await GetCubicleCleaningStatus(cleanedStatus);
                input.StatusId = cleanedStatuMaster.Id;
                input.CleanerId = (int)AbpSession.UserId;
                input.StopTime = DateTime.Now;
                // input.DoneBy = cleanedStatuMaster.DoneBy;
            }
            input.StartTime = cubicleTransaction.StartTime;
            input.CleaningDate = cubicleTransaction.CleaningDate;
            ObjectMapper.Map(input, cubicleTransaction);
            await InsertOrUpdateDailyStatus(cubicleTransaction, isSampling);
            await _cubicleCleaningTransactionRepository.UpdateAsync(cubicleTransaction);
        }

        #endregion private
    }
}