using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.DomainService.Inward;
using ELog.Application.Inward.PutAways.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.Inward.PutAways
{
    [PMMSAuthorize]
    public class PutAwaysAppService : ApplicationService, IPutAwaysAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayBinToBinTrasferRepository;

        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IPutAwayManager _putAwayManager;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<GRNMaterialLabelPrintingHeader> _grnMaterialLabelPrintingHeaderRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnMaterialLabelPrintingContainerBarcodeRepository;
        private const int putAwayPalletToBin = (int)PMMSEnums.MaterialTransferType.PutAwayPalletToBin;
        private const int putAwayMaterialToBin = (int)PMMSEnums.MaterialTransferType.PutAwayMaterialToBin;

        public PutAwaysAppService(
            IHttpContextAccessor httpContextAccessor, IRepository<HandlingUnitMaster> handlingUnitRepository,
            IRepository<Material> materialRepository, IRepository<GRNDetail> grnDetailRepository,
            IRepository<GRNMaterialLabelPrintingHeader> grnMaterialLabelPrintingHeaderRepository,
            IRepository<GRNMaterialLabelPrintingContainerBarcode> grnMaterialLabelPrintingContainerBarcodeRepository,
            IRepository<Palletization> palletizationRepository, IPutAwayManager putAwayManager,
            IRepository<LocationMaster> locationRepository, IRepository<PutAwayBinToBinTransfer> putAwayBinToBinTrasferRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _handlingUnitRepository = handlingUnitRepository;
            _materialRepository = materialRepository;
            _grnDetailRepository = grnDetailRepository;
            _grnMaterialLabelPrintingContainerBarcodeRepository = grnMaterialLabelPrintingContainerBarcodeRepository;
            _grnMaterialLabelPrintingHeaderRepository = grnMaterialLabelPrintingHeaderRepository;
            _httpContextAccessor = httpContextAccessor;
            _palletizationRepository = palletizationRepository;
            _locationRepository = locationRepository;
            _putAwayManager = putAwayManager;
            _putAwayBinToBinTrasferRepository = putAwayBinToBinTrasferRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.PutAway_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<PutAwayBinToBinTransferDto>> GetAsync(Guid? materialToBinId, int palletToBinId)
        {
            return await _putAwayManager.GetPutAwayAsync(materialToBinId, putAwayPalletToBin);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.PutAway_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<PutAwayBinToBinTransferListDto>> GetAllAsync(PagedPutAwayBinToBinTransferResultRequestDto input)
        {
            var query = _putAwayManager.CreateUserListFilteredQuery(input, putAwayPalletToBin, putAwayMaterialToBin);
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

        [PMMSAuthorize(Permissions = PMMSPermissionConst.PutAway_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<PutAwayBinToBinTransferDto> CreateAsync(CreatePutAwayBinToBinTransferDto input)
        {
            var putAway = ObjectMapper.Map<PutAwayBinToBinTransfer>(input);

            if (input.TransactionId == null || input.TransactionId == Guid.Empty)
            {
                putAway.TransactionId = Guid.NewGuid();
            }
            putAway.TenantId = AbpSession.TenantId;
            await _putAwayBinToBinTrasferRepository.InsertAsync(putAway);
            CurrentUnitOfWork.SaveChanges();
            var result = ObjectMapper.Map<PutAwayBinToBinTransferDto>(putAway);
            await _putAwayManager.MapPalletAndMaterialDetails(input, result, input.MaterialTransferTypeId);
            return result;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.PutAway_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UnloadBinAsync(int putAwayId, Guid? transationId)
        {
            await _putAwayManager.UnloadBinAsync(putAwayId, transationId);
        }
    }
}