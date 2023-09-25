using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using ELog.Application.Inward.PutAways.Dto;
using ELog.Core;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.DomainService.Inward
{
    public class PutAwayManager : IPutAwayManager
    {
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<GRNMaterialLabelPrintingHeader> _grnMaterialLabelPrintingHeaderRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnMaterialLabelPrintingContainerBarcodeRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private const int putAwayPalletToBin = (int)PMMSEnums.MaterialTransferType.PutAwayPalletToBin;
        private const int putAwayMaterialToBin = (int)PMMSEnums.MaterialTransferType.PutAwayMaterialToBin;
        private const int binToBinTranferPalletToBin = (int)PMMSEnums.MaterialTransferType.BinToBinTranferPalletToBin;
        private const int binToBinTranferMaterialToBin = (int)PMMSEnums.MaterialTransferType.BinToBinTranferMaterialToBin;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayBinToBinTransferRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;

        public PutAwayManager(
            IRepository<LocationMaster> locationRepository, IRepository<Palletization> palletizationRepository,
            IRepository<HandlingUnitMaster> handlingUnitRepository, IRepository<Material> materialRepository,
            IHttpContextAccessor httpContextAccessor, IRepository<GRNDetail> grnDetailRepository,
            IRepository<GRNMaterialLabelPrintingHeader> grnMaterialLabelPrintingHeaderRepository,
            IRepository<GRNMaterialLabelPrintingContainerBarcode> grnMaterialLabelPrintingContainerBarcodeRepository,
            IRepository<PutAwayBinToBinTransfer> putAwayBinToBinTransferRepository,
            IMasterCommonRepository masterCommonRepository)
        {
            _locationRepository = locationRepository;
            _palletizationRepository = palletizationRepository;
            _handlingUnitRepository = handlingUnitRepository;
            _materialRepository = materialRepository;
            _httpContextAccessor = httpContextAccessor;
            _grnDetailRepository = grnDetailRepository;
            _grnMaterialLabelPrintingHeaderRepository = grnMaterialLabelPrintingHeaderRepository;
            _grnMaterialLabelPrintingContainerBarcodeRepository = grnMaterialLabelPrintingContainerBarcodeRepository;
            _putAwayBinToBinTransferRepository = putAwayBinToBinTransferRepository;
            _masterCommonRepository = masterCommonRepository;
        }

        public async Task<List<PutAwayBinToBinTransferDto>> GetPutAwayAsync(Guid? materialToBinId, int palletToBinId)
        {
            var PutAwayBinToBinTransferDto = (from putAwayDetail in _putAwayBinToBinTransferRepository.GetAll()
                                              join material in _materialRepository.GetAll()
                                              on putAwayDetail.MaterialId equals material.Id into mps
                                              from material in mps.DefaultIfEmpty()
                                              join handlingUnit in _handlingUnitRepository.GetAll()
                                              on putAwayDetail.PalletId equals handlingUnit.Id into hps
                                              from handlinUnit in hps.DefaultIfEmpty()
                                              join location in _locationRepository.GetAll()
                                              on putAwayDetail.LocationId equals location.Id into lps
                                              from location in lps.DefaultIfEmpty()
                                              select new PutAwayBinToBinTransferDto
                                              {
                                                  Id = putAwayDetail.Id,
                                                  LocationId = putAwayDetail.LocationId ?? 0,
                                                  PalletId = putAwayDetail.PalletId ?? 0,
                                                  TransactionId = putAwayDetail.TransactionId,
                                                  MaterialDescription = material.ItemNo + " - " + material.ItemCode,
                                                  SAPBatchNumber = putAwayDetail.SAPBatchNumber,
                                                  ContainerId = putAwayDetail.Id,
                                                  ContainerNo = putAwayDetail.ContainerNo,
                                                  PutAwayHeaderId = putAwayDetail.Id,
                                                  LocationBarcode = location.LocationCode,
                                                  MaterialTransferTypeId = putAwayDetail.MaterialTransferTypeId.Value,
                                                  PalletBarcode = handlinUnit.HUCode + " - " + handlinUnit.Name
                                              });
            if (materialToBinId != null && materialToBinId != Guid.Empty)
            {
                return await PutAwayBinToBinTransferDto.Where(a => a.TransactionId == materialToBinId).ToListAsync() ?? default;
            }
            else
            {
                return await PutAwayBinToBinTransferDto.Where(a => a.Id == palletToBinId).ToListAsync() ?? default;
            }
        }

        public IQueryable<PutAwayBinToBinTransferListDto> CreateUserListFilteredQuery(PagedPutAwayBinToBinTransferResultRequestDto input, int transferPalletToBinId, int materialToBinId)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var putAwayQuery = from putAway in _putAwayBinToBinTransferRepository.GetAll()
                               join material in _materialRepository.GetAll()
                               on putAway.MaterialId equals material.Id into mps
                               from material in mps.DefaultIfEmpty()
                               join handlinUnit in _handlingUnitRepository.GetAll()
                               on putAway.PalletId equals handlinUnit.Id into hps
                               from handlinUnit in hps.DefaultIfEmpty()
                               join location in _locationRepository.GetAll()
                               on putAway.LocationId equals location.Id into lps
                               from location in lps.DefaultIfEmpty()
                               where !putAway.IsUnloaded && (putAway.MaterialTransferTypeId == transferPalletToBinId
                               || putAway.MaterialTransferTypeId == materialToBinId)
                               select new PutAwayBinToBinTransferListDto
                               {
                                   Id = putAway.Id,
                                   PalletId = putAway.PalletId,
                                   MaterialId = putAway.MaterialId,
                                   LocationId = putAway.LocationId,
                                   TransactionId = putAway.TransactionId,
                                   MaterialCode = material.ItemNo + " - " + material.ItemCode,
                                   LocationBarCode = location.LocationCode,
                                   PalletBarcode = handlinUnit.HUCode + " - " + handlinUnit.Name,
                                   PlantId = location.PlantId,
                                   MaterialTransferTypeId = putAway.MaterialTransferTypeId.Value,
                                   ItemDescription = material.ItemDescription,
                                   SAPBatchNumber = putAway.SAPBatchNumber,
                               };

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                putAwayQuery = putAwayQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (input.PalletId != null)
            {
                putAwayQuery = putAwayQuery.Where(x => x.PalletId == input.PalletId);
            }
            if (input.MaterialId != null)
            {
                putAwayQuery = putAwayQuery.Where(x => x.MaterialId == input.MaterialId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                putAwayQuery = putAwayQuery.Where(x => x.LocationBarCode.Contains(input.Keyword));
            }
            return putAwayQuery;
        }

        public async Task UnloadBinAsync(int putAwayId, Guid? transationId)
        {
            var putAwayBinToBinTransfers = new List<PutAwayBinToBinTransfer>();
            if (transationId != null)
            {
                putAwayBinToBinTransfers = await _putAwayBinToBinTransferRepository.GetAllListAsync(x => x.TransactionId.Equals(transationId)).ConfigureAwait(false);
                putAwayBinToBinTransfers.ForEach(x => x.IsUnloaded = true);
                await _masterCommonRepository.BulkUpdatePutAwayBinToBinTransfer(putAwayBinToBinTransfers);
            }
        }

        public List<PutAwayBinToBinTransferListDto> ApplyGroupingOnPutAwayList(List<PutAwayBinToBinTransferListDto> query)
        {
            var result = query.GroupBy(p => new { p.TransactionId }).Select(s => new PutAwayBinToBinTransferListDto()
            {
                Id = s.First().Id,
                PalletId = s.First().PalletId,
                MaterialId = s.First().MaterialId,
                LocationId = s.First().LocationId,
                TransactionId = s.First().TransactionId,
                MaterialCode = s.First().MaterialCode,
                ItemDescription = s.First().ItemDescription,
                PalletBarcode = s.First().PalletBarcode,
                LocationBarCode = s.First().LocationBarCode,
                PlantId = s.First().PlantId,
                MaterialTransferTypeId = s.First().MaterialTransferTypeId.Value,
                SAPBatchNumber = s.First().SAPBatchNumber,
                Count = s.Count()
            }).ToList();
            return result;
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        public IQueryable<PutAwayBinToBinTransferListDto> ApplySorting(IQueryable<PutAwayBinToBinTransferListDto> query, PagedPutAwayBinToBinTransferResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
            {
                return query.OrderBy(sortInput.Sorting);
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        public IEnumerable<PutAwayBinToBinTransferListDto> ApplyPaging(IEnumerable<PutAwayBinToBinTransferListDto> query, PagedPutAwayBinToBinTransferResultRequestDto input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.Skip(pagedInput.SkipCount);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        public async Task MapPalletAndMaterialDetails(CreatePutAwayBinToBinTransferDto input, PutAwayBinToBinTransferDto result, int transferType)
        {
            var putAwayDetails = await (from putAway in _putAwayBinToBinTransferRepository.GetAll()
                                        join material in _materialRepository.GetAll()
                                        on putAway.MaterialId equals material.Id into mps
                                        from material in mps.DefaultIfEmpty()
                                        join handlingUnit in _handlingUnitRepository.GetAll()
                                        on putAway.PalletId equals handlingUnit.Id into hps
                                        from handlinUnit in hps.DefaultIfEmpty()
                                        join location in _locationRepository.GetAll()
                                        on putAway.LocationId equals location.Id into lps
                                        from location in lps.DefaultIfEmpty()
                                        where putAway.TransactionId == result.TransactionId && !result.IsUnloaded
                                        select new PutAwayBinToBinTransferDto
                                        {
                                            Id = putAway.Id,
                                            MaterialDescription = material.ItemNo + " - " + material.ItemCode,
                                            ContainerId = putAway.Id,
                                            ContainerNo = putAway.ContainerNo,
                                            PalletId = putAway.PalletId,
                                            SAPBatchNumber = putAway.SAPBatchNumber,
                                            PutAwayHeaderId = result.Id,
                                            LocationBarcode = location.LocationCode,
                                            MaterialTransferTypeId = putAway.MaterialTransferTypeId.Value,
                                            PalletBarcode = handlinUnit.HUCode + " - " + handlinUnit.Name,
                                            TransactionId = putAway.TransactionId
                                        }).FirstOrDefaultAsync();
            result.MaterialDescription = putAwayDetails != null ? putAwayDetails.MaterialDescription : null;
            result.PalletBarcode = putAwayDetails != null ? putAwayDetails.PalletBarcode : null;
            result.LocationBarcode = putAwayDetails != null ? putAwayDetails.LocationBarcode : null;
            result.TransactionId = putAwayDetails?.TransactionId ?? default;
        }
    }
}