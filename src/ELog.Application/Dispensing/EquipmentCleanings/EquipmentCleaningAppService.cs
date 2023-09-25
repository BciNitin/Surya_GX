using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;

using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.EquipmentCleanings.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.EquipmentCleanings
{
    [PMMSAuthorize]
    public class EquipmentCleaningAppService : ApplicationService, IEquipmentCleaningAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<EquipmentCleaningTransaction> _equipmentCleaningTransactionRepository;
        private readonly IRepository<EquipmentCleaningStatus> _equipmentCleaningStatusRepository;
        private readonly IRepository<EquipmentCleaningCheckpoint> _equipmentCleaningCheckpointRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly string startedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Started).ToLower();
        private readonly string cleanedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Cleaned).ToLower();
        private readonly string verifiedStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Verified).ToLower();
        private readonly string uncleanStatus = nameof(PMMSEnums.CubicleCleaningHeaderStatus.Uncleaned).ToLower();
        #endregion fields

        #region constructor

        public EquipmentCleaningAppService(IRepository<EquipmentCleaningTransaction> equipmentCleaningTransactionRepository,
            IRepository<EquipmentCleaningStatus> equipmentCleaningDailyStatusRepository,
             IRepository<EquipmentCleaningCheckpoint> equipmentCleaningCheckpointRepository,
             IRepository<CheckpointMaster> checkpointRepository, IRepository<User, long> userRepository,
             IDispensingAppService dispensingAppService)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _equipmentCleaningTransactionRepository = equipmentCleaningTransactionRepository;
            _equipmentCleaningStatusRepository = equipmentCleaningDailyStatusRepository;
            _equipmentCleaningCheckpointRepository = equipmentCleaningCheckpointRepository;
            _checkpointRepository = checkpointRepository;
            _dispensingAppService = dispensingAppService;
            _userRepository = userRepository;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentCleaning_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<EquipmentCleaningTransactionDto> CreateAsync(CreateEquipmentCleaningTransactionDto input)
        {
            return await CreateEquipmentCleaningTransaction(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingEquipmentCleaning_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<EquipmentCleaningTransactionDto> CreateSamplingAsync(CreateEquipmentCleaningTransactionDto input)
        {
            return await CreateEquipmentCleaningTransaction(input, true);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentCleaning_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UpdateAsync(EquipmentCleaningTransactionDto input)
        {
            await UpdateEquipmentCleaningTransaction(input, false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.SamplingEquipmentCleaning_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UpdateSamplingAsync(EquipmentCleaningTransactionDto input)
        {
            await UpdateEquipmentCleaningTransaction(input, true);
        }
        private async Task InsertOrUpdateStatus(EquipmentCleaningTransaction cubicleCleaningTransaction, bool isSampling)
        {
            var cleaningDate = cubicleCleaningTransaction.CleaningDate;
            var equipmentCleaningStatus = await _equipmentCleaningStatusRepository.FirstOrDefaultAsync(x => x.EquipmentId == cubicleCleaningTransaction.EquipmentId
             && x.CleaningDate.Day == cleaningDate.Day && x.CleaningDate.Month == cleaningDate.Month && x.CleaningDate.Year == cleaningDate.Year && x.IsSampling == isSampling);
            if (equipmentCleaningStatus != null)
            {
                equipmentCleaningStatus.StatusId = cubicleCleaningTransaction.StatusId;
                equipmentCleaningStatus.TenantId = AbpSession.TenantId;
                await _equipmentCleaningStatusRepository.UpdateAsync(equipmentCleaningStatus);
            }
            else
            {
                var equipmentStatus = new EquipmentCleaningStatus
                {
                    StatusId = cubicleCleaningTransaction.StatusId,
                    EquipmentId = cubicleCleaningTransaction.EquipmentId,
                    CleaningDate = cubicleCleaningTransaction.CleaningDate,
                    TenantId = AbpSession.TenantId,
                    IsSampling = isSampling
                };
                await _equipmentCleaningStatusRepository.InsertAsync(equipmentStatus);
            }
        }

        public async Task<EquipmentCleaningTransactionDto> ValidateEquipmentStatusAsync(int equipmentId, int type, bool isSampling)
        {
            var localDate = DateTime.Now;
            var verfiedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, verifiedStatus);
            var cleanedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, cleanedStatus);
            var unCleanedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, uncleanStatus);
            var equipmentCleaningStatus = await _equipmentCleaningStatusRepository.FirstOrDefaultAsync(x => x.EquipmentId == equipmentId && x.CleaningDate.Day == localDate.Day && x.CleaningDate.Month == localDate.Month && x.CleaningDate.Year == localDate.Year && x.StatusId != verfiedStatusId && x.StatusId != unCleanedStatusId && x.IsSampling == isSampling);
            if (equipmentCleaningStatus != null)
            {
                var equipmentCleaningTransaction = await _equipmentCleaningTransactionRepository.FirstOrDefaultAsync(x => x.EquipmentId == equipmentId && x.CleaningDate.Day == localDate.Day && x.CleaningDate.Month == localDate.Month && x.CleaningDate.Year == localDate.Year && x.CleaningTypeId == type && x.StatusId != verfiedStatusId && x.IsSampling == isSampling);
                if (equipmentCleaningTransaction != null)
                {
                    var data = ObjectMapper.Map<EquipmentCleaningTransactionDto>(equipmentCleaningTransaction);
                    data.IsUncleaned = false;
                    data.CreatorName = _userRepository.FirstOrDefault(x => x.Id == equipmentCleaningTransaction.CreatorUserId).FullName;
                    if (data.StatusId == cleanedStatusId)
                    {
                        GetEquipmentCleaningCheckPoints(data);
                        data.CanApproved = data.CleanerId != (int)AbpSession.UserId;
                        data.CleanerName = _userRepository.FirstOrDefault(x => x.Id == equipmentCleaningTransaction.CleanerId).FullName;
                    }
                    return data;
                }
                else
                {
                    return new EquipmentCleaningTransactionDto
                    {
                        IsInValidTransaction = true,
                        IsUncleaned = true,
                        EquipmentCleaningCheckpoints = new List<CheckpointDto>()
                    };
                }
            }
            return new EquipmentCleaningTransactionDto
            {
                IsUncleaned = true,
                EquipmentCleaningCheckpoints = new List<CheckpointDto>()
            };
        }

        #endregion public

        #region private

        private async Task<EquipmentCleaningTransactionDto> CreateEquipmentCleaningTransaction(CreateEquipmentCleaningTransactionDto input, bool isSampling)
        {
            DateTime localDate = DateTime.Now;
            var equipmentCleaningStartedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, startedStatus);
            var equipmentCleaningTransaction = ObjectMapper.Map<EquipmentCleaningTransaction>(input);
            equipmentCleaningTransaction.TenantId = AbpSession.TenantId;
            equipmentCleaningTransaction.CleaningDate = localDate;
            equipmentCleaningTransaction.StartTime = localDate;
            equipmentCleaningTransaction.StatusId = equipmentCleaningStartedStatusId;
            equipmentCleaningTransaction.CleanerId = (int)AbpSession.UserId;
            equipmentCleaningTransaction.IsSampling = isSampling;
            await _equipmentCleaningTransactionRepository.InsertAsync(equipmentCleaningTransaction);
            await InsertOrUpdateStatus(equipmentCleaningTransaction, isSampling);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<EquipmentCleaningTransactionDto>(equipmentCleaningTransaction);
        }

        private async Task UpdateEquipmentCleaningTransaction(EquipmentCleaningTransactionDto input, bool isSampling)
        {
            var equipmentCleaningTransaction = await _equipmentCleaningTransactionRepository.GetAsync(input.Id);
            if (input.IsVerified)
            {
                var equipmentCleaningVerfiedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, verifiedStatus);
                input.StatusId = equipmentCleaningVerfiedStatusId;
                input.VerifierId = (int)AbpSession.UserId;
                input.VerifiedTime = DateTime.Now;
                input.StopTime = equipmentCleaningTransaction.StopTime;
            }
            else if (input.IsRejected)
            {
                var equipmentCleaningRejectedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, uncleanStatus);
                input.StatusId = equipmentCleaningRejectedStatusId;
                input.VerifierId = (int)AbpSession.UserId;
                input.VerifiedTime = DateTime.Now;
            }
            else
            {
                var equipmentCleaningCleanedStatusId = await _dispensingAppService.GetStatusByModuleSubModuleName(PMMSConsts.DispensingSubModule, PMMSConsts.EquipmentCleaningSubModule, cleanedStatus);
                input.StatusId = equipmentCleaningCleanedStatusId;
                input.StopTime = DateTime.Now;
                input.CleanerId = (int)AbpSession.UserId;
            }
            input.StartTime = equipmentCleaningTransaction.StartTime;
            input.CleaningDate = equipmentCleaningTransaction.CleaningDate;
            ObjectMapper.Map(input, equipmentCleaningTransaction);
            await InsertOrUpdateStatus(equipmentCleaningTransaction, isSampling);
            await _equipmentCleaningTransactionRepository.UpdateAsync(equipmentCleaningTransaction);
        }
        private void GetEquipmentCleaningCheckPoints(EquipmentCleaningTransactionDto equipmentCleaningTransactionDto)
        {
            equipmentCleaningTransactionDto.EquipmentCleaningCheckpoints = (from detail in _equipmentCleaningCheckpointRepository.GetAll()
                                                                            join checkpoint in _checkpointRepository.GetAll()
                                                                            on detail.CheckPointId equals checkpoint.Id
                                                                            where detail.EquipmentCleaningTransactionId == equipmentCleaningTransactionDto.Id
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