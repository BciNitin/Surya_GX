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

        IQueryable<AuditReportDetailsDto> GetAuditTrail();

        IQueryable GetDynamicAuditTrail();

        //long GetNextWIPProcessLabelBarcodeSequence();

    }
}