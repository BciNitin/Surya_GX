using ELog.Core.Entities;
using ELog.Core.SQLDtoEntities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static ELog.Core.PMMSEnums;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories
{
    public interface IMasterCommonRepository
    {
        //int GetNextUOMSequence();

        //long GetNextGRNSequence();

        //long GetNextGateEntryBarcodeSequence();
        //long GetNextCageLabelPrintBarcodeSequence();
        //void ResetLineItemNumberSequenceValue();

        //long GetNextGRNSAPBatchNumberSequence();
        //long GetNextGRNLineItemSequence();

        //long GetNextMaterialLabelBarcodeSequence();

        //long GetNextSamplingLabelBarcodeSequenceValue();

        //long GetNextDispensingLabelBarcodeSequenceValue();

        Task<ApprovalStatus> GetApprovalForAdd(string subModuleName);

        Task<ApprovalStatus> GetApprovalForEdit(string subModuleName, int existingApprovalStatusId);

        Task<bool> IsApprovalRequired(string subModuleName);

        Task<bool> BulkInsertMaterialLabelBarCode(List<GRNMaterialLabelPrintingContainerBarcode> lstMaterialLabels);

        Task<bool> BulkUpdatePalletization(List<Palletization> palletizations);

        Task<bool> BulkUpdatePutAwayBinToBinTransfer(List<PutAwayBinToBinTransfer> putAwayBinToBinTransfes);

        Task<bool> BulkUpdateMaterialBatchDispensingHeader(List<MaterialBatchDispensingHeader> materialBatchDispensingHeaders);

        Task<bool> BulkUpdateMaterialDestructionForSAPPosted(List<MaterialDestruction> lstMaterialDestruction, List<string> lstFieldsToUpdate);

        Task<bool> BulkDeletePalletization(List<Palletization> lstPallets, List<string> lstFieldsToUpdate);

        Task<bool> BulkDeletePutAwayBinToBinTransfer(List<PutAwayBinToBinTransfer> lstPutAway, List<string> lstFieldsToUpdate);

        Task<bool> BulkUpdateBalanceQuantityFromContainer(List<GRNMaterialLabelPrintingContainerBarcode> lstContainers, List<string> lstFieldsToUpdate);
        IQueryable<PickingReportDto> GetAllPickingReportRawSQL();

        IQueryable<AuditReportDetailsDto> GetAuditTrail();

        IQueryable GetDynamicAuditTrail();

        //long GetNextWIPProcessLabelBarcodeSequence();

        Task<bool> BulkUpdateFgPutAway(List<FgPutAway> fgPutAway);
    }
}