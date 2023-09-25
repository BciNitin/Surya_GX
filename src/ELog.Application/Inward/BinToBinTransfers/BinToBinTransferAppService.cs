using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.DomainService.Inward;
using ELog.Application.Inward.BinToBinTransfers;
using ELog.Application.Inward.PutAways.Dto;
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
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Inward.BinToBinTransfer
{
    public class BinToBinTransferAppService : ApplicationService, IBinToBinTransferAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IPutAwayManager _putAwayManager;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayBinToBinTransferRepository;

        #endregion fields

        #region constructor

        public BinToBinTransferAppService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<Palletization> palletizationRepository, IRepository<HandlingUnitMaster> handlingUnitRepository,
            IPutAwayManager putAwayManager, IRepository<PutAwayBinToBinTransfer> putAwayBinToBinTransferRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _palletizationRepository = palletizationRepository;
            _handlingUnitRepository = handlingUnitRepository;
            _putAwayManager = putAwayManager;
            _putAwayBinToBinTransferRepository = putAwayBinToBinTransferRepository;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.BinToBinTransfer_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<PutAwayBinToBinTransferDto>> GetAsync(Guid? materialToBinId, int palletToBinId)
        {
            return await _putAwayManager.GetPutAwayAsync(materialToBinId, palletToBinId);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.BinToBinTransfer_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<PutAwayBinToBinTransferListDto>> GetAllAsync(PagedPutAwayBinToBinTransferResultRequestDto input)
        {
            var query = _putAwayManager.CreateUserListFilteredQuery(input, (int)MaterialTransferType.BinToBinTranferPalletToBin,
                (int)MaterialTransferType.BinToBinTranferMaterialToBin);
            query = _putAwayManager.ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var groupEntities = _putAwayManager.ApplyGroupingOnPutAwayList(entities);
            var groupEntitiesPagingResult = _putAwayManager.ApplyPaging(groupEntities, input);
            var totalCount = groupEntitiesPagingResult.Count();

            return new PagedResultDto<PutAwayBinToBinTransferListDto>(
                totalCount,
               groupEntitiesPagingResult.ToList()
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.BinToBinTransfer_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<PutAwayBinToBinTransferDto> CreateAsync(CreatePutAwayBinToBinTransferDto input)
        {
            var putAwayMappedBin = new PutAwayBinToBinTransfer();
            if (input.TransactionId == null || input.TransactionId == Guid.Empty)
            {
                input.TransactionId = Guid.NewGuid();
            }
            if (input.MaterialTransferTypeId == (int)PMMSEnums.MaterialTransferType.BinToBinTranferMaterialToBin)
            {
                putAwayMappedBin = await _putAwayBinToBinTransferRepository.GetAll().Where(a => a.MaterialId == input.MaterialId && a.ContainerId == input.ContainerId && a.IsUnloaded == false).FirstOrDefaultAsync();
            }
            else
            {
                putAwayMappedBin = await _putAwayBinToBinTransferRepository.GetAll().Where(a => a.PalletId == input.PalletId && a.IsUnloaded == false).FirstOrDefaultAsync();
            }
            if (putAwayMappedBin != null)
            {
                putAwayMappedBin.TransactionId = input.TransactionId;
                putAwayMappedBin.MaterialTransferTypeId = input.MaterialTransferTypeId;
                putAwayMappedBin.LocationId = input.LocationId;
                await _putAwayBinToBinTransferRepository.UpdateAsync(putAwayMappedBin);
            }
            var result = ObjectMapper.Map<PutAwayBinToBinTransferDto>(putAwayMappedBin);
            CurrentUnitOfWork.SaveChanges();
            await _putAwayManager.MapPalletAndMaterialDetails(input, result, input.MaterialTransferTypeId);
            return result;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.BinToBinTransfer_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UnloadBinAsync(int putAwayId, Guid? transationId)
        {
            await _putAwayManager.UnloadBinAsync(putAwayId, transationId);
        }

        #endregion public
    }
}