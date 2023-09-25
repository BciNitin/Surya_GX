using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using EFCore.BulkExtensions;
using ELog.Core.Entities;
using ELog.Core.SQLDtoEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    [ExcludeFromCodeCoverage]
    public class PMMSRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<PMMSDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public PMMSRepositoryBase(IDbContextProvider<PMMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        //public int GetNextUOMSequenceValue()
        //{
        //    return Context.GetNextUOMSequenceValue();
        //}

        //public int GetNextGRNSequenceValue()
        //{
        //    return Context.GetNextGRNSequenceValue();
        //}

        //public int GetNextGRNSAPBatchNumberSequenceValue()
        //{
        //    return Context.GetNextGRNSAPBatchNumberSequenceValue();
        //}
        //public int GetNextGRNLineItemSequenceValue()
        //{
        //    return Context.GetNextGRNLineItemSequenceValue();
        //}

        //public long GetNextGateEntryBarcodeSequenceValue()
        //{
        //    return Context.GetNextGateEntryBarcodeSequenceValue();
        //}
        //public long GetNextCageLabelPrintBarcodeSequenceValue()
        //{
        //    return Context.GetNextCageLabelPrintBarcodeSequenceValue();
        //}
        //public void ResetLineItemNumberSequenceValue()
        //{
        //    Context.ResetLineItemNumberSequenceValue();
        //}
        //public long GetNextMaterialLabelBarcodeSequenceValue()
        //{
        //    return Context.GetNextMaterialLabelBarcodeSequenceValue();
        //}

        //public long GetNextDispensingLabelBarcodeSequenceValue()
        //{
        //    return Context.GetNextDispensingLabelBarcodeSequenceValue();
        //}

        //public long GetNextSamplingLabelBarcodeSequenceValue()
        //{
        //    return Context.GetNextSamplingLabelBarcodeSequenceValue();
        //}
        //public long GetNextWIPProcessLabelBarcodeSequenceValue()
        //{
        //    return Context.GetNextWIPProcessLabelBarcodeSequenceValue();
        //}
        public async Task<bool> BulkInsertMaterialLabelBarCodeTable(List<GRNMaterialLabelPrintingContainerBarcode> lstMaterialLabels)
        {
            await Context.BulkInsertAsync(lstMaterialLabels);
            return true;
        }

        public async Task<bool> BulkUpdatePalletizationTable(List<Palletization> palletization)
        {
            await Context.BulkUpdateAsync(palletization);

            return true;
        }

        public async Task<bool> BulkUpdatePutAwayBinToBinTransferTable(List<PutAwayBinToBinTransfer> putAwayBinToBinTransfer)
        {
            await Context.BulkUpdateAsync(putAwayBinToBinTransfer);

            return true;
        }

        public async Task<bool> BulkUpdateMaterialBatchDispensingHeaderTable(List<MaterialBatchDispensingHeader> materialBatchDispensingHeaders)
        {
            await Context.BulkUpdateAsync(materialBatchDispensingHeaders);
            return true;
        }

        public async Task<bool> BulkUpdateMaterialDestructionForSAPPostedTable(List<MaterialDestruction> lstMaterialDestruction, List<string> lstFieldsToUpdate)
        {
            BulkConfig config = new BulkConfig
            {
                PropertiesToInclude = lstFieldsToUpdate
            };
            await Context.BulkUpdateAsync(lstMaterialDestruction, config);
            return true;
        }

        public async Task<bool> BulkDeletePalletizationTable(List<Palletization> lstPallets, List<string> lstFieldsToUpdate)
        {
            BulkConfig config = new BulkConfig
            {
                PropertiesToInclude = lstFieldsToUpdate
            };
            await Context.BulkUpdateAsync(lstPallets, config);
            return true;
        }

        public async Task<bool> BulkDeletePutAwayBinToBinTransferTable(List<PutAwayBinToBinTransfer> lstPutAway, List<string> lstFieldsToUpdate)
        {
            BulkConfig config = new BulkConfig
            {
                PropertiesToInclude = lstFieldsToUpdate
            };
            await Context.BulkUpdateAsync(lstPutAway, config);
            return true;
        }

        public async Task<bool> BulkUpdateBalanceQuantityFromContainerTable(List<GRNMaterialLabelPrintingContainerBarcode> lstContainers, List<string> lstFieldsToUpdate)
        {
            BulkConfig config = new BulkConfig
            {
                PropertiesToInclude = lstFieldsToUpdate
            };
            await Context.BulkUpdateAsync(lstContainers, config);
            return true;
        }

        public IQueryable<PickingReportDto> GetAllPickingReportUsingRawSQL()
        {
            return Context.PickingReportSQLResult.FromSqlRaw(SQLQueryConst.PICKING_REPORT_SQL);
        }

        public async Task<bool> BulkUpdateFgPutAway(List<FgPutAway> fgPutAway)
        {
            await Context.BulkUpdateAsync(fgPutAway);
            return true;
        }


        public IQueryable<AuditReportDetailsDto> GetAuditTrail()
        {
            return Context.AuditSQLResult.FromSqlRaw(SQLQueryConst.AUDIT_REPORT_SQL);
        }
        public IQueryable GetDynamicAuditTrail()
        {

            return Context.Consumptions.FromSqlRaw(SQLQueryConst.AUDIT_DYNAMIC_REPORT);
        }

    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="PMMSRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [ExcludeFromCodeCoverage]
    public class PMMSRepositoryBase<TEntity> : PMMSRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public PMMSRepositoryBase(IDbContextProvider<PMMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        // Do not add any method here, add to the class above (since this inherits it)!!!
    }
}