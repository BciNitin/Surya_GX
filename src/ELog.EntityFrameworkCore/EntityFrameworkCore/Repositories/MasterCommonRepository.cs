using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.Core.SQLDtoEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories
{
    public class MasterCommonRepository : PMMSRepositoryBase<User, long>, IMasterCommonRepository
    {
        private readonly IRepository<SubModuleMaster> _subModuleRepository;

        public MasterCommonRepository(IDbContextProvider<PMMSDbContext> dbContextProvider, IRepository<SubModuleMaster> subModuleRepository)
       : base(dbContextProvider)
        {
            _subModuleRepository = subModuleRepository;
        }

        //public int GetNextUOMSequence()
        //{
        //    return base.GetNextUOMSequenceValue();
        //}

        //public long GetNextGRNSequence()
        //{
        //    return base.GetNextGRNSequenceValue();
        //}

        //public long GetNextGRNSAPBatchNumberSequence()
        //{
        //    return base.GetNextGRNSAPBatchNumberSequenceValue();
        //}
        //public long GetNextGRNLineItemSequence()
        //{
        //    return base.GetNextGRNLineItemSequenceValue();
        //}
        //public long GetNextGateEntryBarcodeSequence()
        //{
        //    return base.GetNextGateEntryBarcodeSequenceValue();
        //}
        //public long GetNextCageLabelPrintBarcodeSequence()
        //{
        //    return base.GetNextCageLabelPrintBarcodeSequenceValue();
        //}
        //public void ResetLineItemNumberSequenceValue()
        //{
        //    base.ResetLineItemNumberSequenceValue();
        //}

        //public long GetNextMaterialLabelBarcodeSequence()
        //{
        //    return base.GetNextMaterialLabelBarcodeSequenceValue();
        //}

        //public long GetNextDispensingLabelBarcodeSequenceValue()
        //{
        //    return base.GetNextDispensingLabelBarcodeSequenceValue();
        //}

        //public long GetNextSamplingLabelBarcodeSequenceValue()
        //{
        //    return base.GetNextSamplingLabelBarcodeSequenceValue();
        //}
        //public long GetNextWIPProcessLabelBarcodeSequence()
        //{
        //    return base.GetNextWIPProcessLabelBarcodeSequenceValue();
        //}

        public async Task<ApprovalStatus> GetApprovalForAdd(string subModuleName)
        {
            if (await IsApprovalRequired(subModuleName))
            {
                return ApprovalStatus.Submitted;
            }
            return ApprovalStatus.Approved;
        }

        public async Task<ApprovalStatus> GetApprovalForEdit(string subModuleName, int existingApprovalStatusId)
        {
            var existingApprovalEnum = (ApprovalStatus)existingApprovalStatusId;
            if (await IsApprovalRequired(subModuleName))
            {
                if (existingApprovalEnum == ApprovalStatus.Approved)
                {
                    return ApprovalStatus.Approved;
                }
                else if (existingApprovalEnum == ApprovalStatus.Rejected)
                {
                    return ApprovalStatus.Submitted;
                }
                return ApprovalStatus.Submitted;
            }
            return ApprovalStatus.Approved;
        }

        public async Task<bool> IsApprovalRequired(string subModuleName)
        {
            return await _subModuleRepository.GetAll().AnyAsync(x => x.IsActive
                                                                  && x.Name == subModuleName
                                                                  && x.IsApprovalRequired
                                                                  && x.IsApprovalWorkflowRequired);
        }

        public async Task<bool> BulkInsertMaterialLabelBarCode(List<GRNMaterialLabelPrintingContainerBarcode> lstMaterialLabels)
        {
            return await base.BulkInsertMaterialLabelBarCodeTable(lstMaterialLabels);
        }

        public async Task<bool> BulkUpdatePalletization(List<Palletization> palletizations)
        {
            return await base.BulkUpdatePalletizationTable(palletizations);
        }

        public async Task<bool> BulkUpdatePutAwayBinToBinTransfer(List<PutAwayBinToBinTransfer> putAwayBinToBinTransfer)
        {
            return await base.BulkUpdatePutAwayBinToBinTransferTable(putAwayBinToBinTransfer);
        }

        public async Task<bool> BulkUpdateMaterialBatchDispensingHeader(List<MaterialBatchDispensingHeader> materialBatchDispensingHeaders)
        {
            return await base.BulkUpdateMaterialBatchDispensingHeaderTable(materialBatchDispensingHeaders);
        }

        public async Task<bool> BulkUpdateMaterialDestructionForSAPPosted(List<MaterialDestruction> lstMaterialDestruction, List<string> lstFieldsToUpdate)
        {
            return await base.BulkUpdateMaterialDestructionForSAPPostedTable(lstMaterialDestruction, lstFieldsToUpdate);
        }

        public async Task<bool> BulkDeletePalletization(List<Palletization> lstPallets, List<string> lstFieldsToUpdate)
        {
            return await base.BulkDeletePalletizationTable(lstPallets, lstFieldsToUpdate);
        }

        public async Task<bool> BulkDeletePutAwayBinToBinTransfer(List<PutAwayBinToBinTransfer> lstPutAway, List<string> lstFieldsToUpdate)
        {
            return await base.BulkDeletePutAwayBinToBinTransferTable(lstPutAway, lstFieldsToUpdate);
        }

        public async Task<bool> BulkUpdateBalanceQuantityFromContainer(List<GRNMaterialLabelPrintingContainerBarcode> lstContainers, List<string> lstFieldsToUpdate)
        {
            return await base.BulkUpdateBalanceQuantityFromContainerTable(lstContainers, lstFieldsToUpdate);
        }
        public IQueryable<PickingReportDto> GetAllPickingReportRawSQL()
        {
            return base.GetAllPickingReportUsingRawSQL();
        }

        public async Task<bool> BulkUpdateFgPutAway(List<FgPutAway> fgPutAway)
        {
            return await base.BulkUpdateFgPutAway(fgPutAway);
        }

        public IQueryable<AuditReportDetailsDto> GetAuditTrail()
        {
            return base.GetAuditTrail();

        }
        public IQueryable GetDynamicAuditTrail()
        {
            return base.GetDynamicAuditTrail();

        }
    }
}