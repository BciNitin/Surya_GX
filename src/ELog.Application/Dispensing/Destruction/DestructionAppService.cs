using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.Destruction.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.Destruction
{
    [PMMSAuthorize]
    public class DestructionAppService : ApplicationService, IDestructionAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<SAPQualityControlDetail> _qualityControlDetailRepository;
        private readonly IRepository<MaterialDestruction> _materialDestructionRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _materialContainerRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<Palletization> _palletRepository;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayBinToBinRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ReturnToVendorDetail> _returnToVendorDetailRepository;
        private readonly IRepository<ReturnToVendorHeader> _returnToVendorHeaderRepository;

        #endregion fields

        #region constructor

        public DestructionAppService(IHttpContextAccessor httpContextAccessor,
            IDispensingAppService dispensingAppService,
            IRepository<SAPQualityControlDetail> qualityControlDetailRepository,
            IRepository<MaterialDestruction> materialDestructionRepository,
            IRepository<GRNMaterialLabelPrintingContainerBarcode> materialContainerRepository,
            IRepository<GRNDetail> grnDetailRepository,
            IRepository<Material> materialRepository,
            IRepository<Palletization> palletRepository,
            IRepository<PutAwayBinToBinTransfer> putAwayBinToBinRepository,
            IMasterCommonRepository masterCommonRepository,
            IRepository<ReturnToVendorDetail> returnToVendorDetailRepository,
            IRepository<ReturnToVendorHeader> returnToVendorHeaderRepository
            )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _dispensingAppService = dispensingAppService;
            _qualityControlDetailRepository = qualityControlDetailRepository;
            _materialDestructionRepository = materialDestructionRepository;
            _materialContainerRepository = materialContainerRepository;
            _grnDetailRepository = grnDetailRepository;
            _materialRepository = materialRepository;
            _palletRepository = palletRepository;
            _putAwayBinToBinRepository = putAwayBinToBinRepository;
            _masterCommonRepository = masterCommonRepository;
            _returnToVendorHeaderRepository = returnToVendorHeaderRepository;
            _returnToVendorDetailRepository = returnToVendorDetailRepository;
        }

        public async Task<HTTPResponseDto> UpdateMaterialDetailToDestructionDtoByBarcode(DestructionDto input)
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();
            //Validate movement type
            if (input.MovementType != PMMSConsts.MovementType_Approved && input.MovementType != PMMSConsts.MovementType_Rejected)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.MovementTypeNotValid);
            }
            if (string.IsNullOrEmpty(input.MaterialContainerBarCode) || string.IsNullOrWhiteSpace(input.MaterialContainerBarCode))
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerBarcodeNotValid);
            }

            //Validate Material Container Barcode is not already scanned
            var IsContainerBarcodeisAlreadyScanned = await _materialDestructionRepository.GetAll().AnyAsync(x =>
                                                              x.MaterialContainerBarCode == input.MaterialContainerBarCode);
            if (IsContainerBarcodeisAlreadyScanned)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerAlreadyScanned);
            }
            //Validate Material Container Barcode is not already return to vendor
            var IsContainerBarcodeisReturnToVendor = await _returnToVendorDetailRepository.GetAll().AnyAsync(x =>
                                                              x.ContainerMaterialBarcode == input.MaterialContainerBarCode);
            if (IsContainerBarcodeisReturnToVendor)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerReturnedToVendor);
            }
            //Validate Material Container Barcode as per the movement type
            var qcDetailwithContainer = await (from grndetail in _grnDetailRepository.GetAll()
                                               join materialContainer in _materialContainerRepository.GetAll()
                                               on grndetail.Id equals materialContainer.GRNDetailId
                                               join qcDetail in _qualityControlDetailRepository.GetAll()
                                               on grndetail.SAPBatchNumber equals qcDetail.SAPBatchNo
                                               join material in _materialRepository.GetAll()
                                               on grndetail.MaterialId equals material.Id
                                               where qcDetail.MovementType == input.MovementType
                                               && materialContainer.MaterialLabelContainerBarCode == input.MaterialContainerBarCode
                                               select new DestructionDto
                                               {
                                                   MovementType = qcDetail.MovementType,
                                                   MaterialCode = qcDetail.ItemCode,
                                                   ContainerId = materialContainer.Id,
                                                   MaterialContainerBarCode = materialContainer.MaterialLabelContainerBarCode,
                                                   Quantity = materialContainer.BalanceQuantity,
                                                   SAPBatchNo = grndetail.SAPBatchNumber,
                                                   UnitOfMeasurement = material.UnitOfMeasurement
                                               }).FirstOrDefaultAsync();
            if (qcDetailwithContainer == null)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.ContainerNotFoundMovementType);
            }
            //Insert into MaterialDestruction table and Update Input model
            MaterialDestruction destruction = new MaterialDestruction();
            destruction.MovementType = qcDetailwithContainer.MovementType;
            destruction.ContainerId = qcDetailwithContainer.ContainerId;
            destruction.MaterialContainerBarCode = qcDetailwithContainer.MaterialContainerBarCode;
            destruction.MaterialCode = qcDetailwithContainer.MaterialCode;
            destruction.SAPBatchNo = qcDetailwithContainer.SAPBatchNo;
            destruction.ARNo = qcDetailwithContainer.ARNo;
            destruction.Quantity = qcDetailwithContainer.Quantity;
            destruction.UnitOfMeasurement = qcDetailwithContainer.UnitOfMeasurement;
            await _materialDestructionRepository.InsertAsync(destruction);
            input = qcDetailwithContainer;
            responseDto.ResultObject = input;
            return responseDto;
        }

        public async Task<HTTPResponseDto> PostDestructionDetailToSAP()
        {
            HTTPResponseDto responseDto = new HTTPResponseDto();

            //Get all data from MaterialDestruction Table where IsPostedToSAP is false
            var lstDestructionDataToSent = await _materialDestructionRepository.GetAll().Where(x => !x.IsPostedToSAP).Select(x => new DestructionDto
            {
                Id = x.Id,
                MovementType = x.MovementType,
                ContainerId = x.ContainerId,
                MaterialContainerBarCode = x.MaterialContainerBarCode,
                MaterialCode = x.MaterialCode,
                SAPBatchNo = x.SAPBatchNo,
                ARNo = x.ARNo,
                Quantity = x.Quantity,
                UnitOfMeasurement = x.UnitOfMeasurement
            }).ToListAsync();

            if (lstDestructionDataToSent == null || lstDestructionDataToSent?.Count() == 0)
            {
                return _dispensingAppService.UpdateErrorResponse(responseDto, PMMSValidationConst.NoContainerFoundForSAPPosting);
            }
            //TO Do : Post data to SAP, Call SAP API

            //Update IsPostedToSAP to true
            var lstDestructionId = lstDestructionDataToSent.Select(x => x.Id);
            var lstMaterialDestruciton = new List<MaterialDestruction>();
            foreach (var id in lstDestructionId)
            {
                var destruction = new MaterialDestruction();
                destruction.Id = id;
                destruction.IsPostedToSAP = true;
                lstMaterialDestruciton.Add(destruction);
            }
            await _masterCommonRepository.BulkUpdateMaterialDestructionForSAPPosted(lstMaterialDestruciton, new List<string> { nameof(MaterialDestruction.IsPostedToSAP) });

            //Clear balance quantity from material container table
            var lstContainerId = lstDestructionDataToSent.Select(x => (int?)x.ContainerId).ToList();
            if (lstContainerId?.Count() > 0)
            {
                List<GRNMaterialLabelPrintingContainerBarcode> lstContainers = new List<GRNMaterialLabelPrintingContainerBarcode>();
                foreach (var containerId in lstContainerId)
                {
                    var container = new GRNMaterialLabelPrintingContainerBarcode();
                    container.Id = containerId.GetValueOrDefault();
                    container.BalanceQuantity = 0;
                    lstContainers.Add(container);
                }
                await _masterCommonRepository.BulkUpdateBalanceQuantityFromContainer(lstContainers, new List<string> { nameof(GRNMaterialLabelPrintingContainerBarcode.BalanceQuantity) });
            }

            //Remove all matching containers from pallet
            var lstPalletId = await _palletRepository.GetAll().Where(x => lstContainerId.Contains(x.ContainerId)).Select(x => x.Id).ToListAsync();
            if (lstPalletId?.Count() > 0)
            {
                List<Palletization> lstPalletization = new List<Palletization>();
                foreach (var palletId in lstPalletId)
                {
                    var pallet = new Palletization();
                    pallet.Id = palletId;
                    pallet.IsDeleted = true;
                    lstPalletization.Add(pallet);
                }
                await _masterCommonRepository.BulkDeletePalletization(lstPalletization, new List<string> { nameof(Palletization.IsDeleted) });
            }
            //Remove all matching containers from bin
            var lstPutAwayBinToBinTransfer = await _putAwayBinToBinRepository.GetAll().Where(x => lstContainerId.Contains(x.ContainerId)).Select(x => x.Id).ToListAsync();
            if (lstPutAwayBinToBinTransfer?.Count() > 0)
            {
                List<PutAwayBinToBinTransfer> lstPutAway = new List<PutAwayBinToBinTransfer>();
                foreach (var putAwayId in lstPutAwayBinToBinTransfer)
                {
                    var putAway = new PutAwayBinToBinTransfer();
                    putAway.Id = putAwayId;
                    putAway.IsDeleted = true;
                    lstPutAway.Add(putAway);
                }
                await _masterCommonRepository.BulkDeletePutAwayBinToBinTransfer(lstPutAway, new List<string> { nameof(PutAwayBinToBinTransfer.IsDeleted) });
            }

            return responseDto;
        }

        #endregion constructor
    }
}