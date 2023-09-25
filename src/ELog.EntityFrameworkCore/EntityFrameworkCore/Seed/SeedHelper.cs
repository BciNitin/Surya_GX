using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CalibrationStatusMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CalibrationTestStatusMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CheckpointTypeMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CountryStateMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CubicleCleaningTypeMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.DeviceTypeMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.EquipmentTypeMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.FrequencyTypeMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.HolidayTypeMaster;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Tenants;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.UnitOfMeasurementTypeMaster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Transactions;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed
{
    [ExcludeFromCodeCoverage]
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<PMMSDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(PMMSDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            new ApprovalStatusMasterBuilder(context).Create();
            new InitialHostDbBuilder(context).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
            new ActionBuilder(context).Create();
            new SubModuleTypeMasterBuilder(context).Create();
            new ModuleSubModuleBuilder(context, 1).Create();
            new DefaultRolePermissionBuilder(context, 1).Create();
            new CountryStateBuilder(context, 1).Create();
            new EquipmentTypeMasterBuilder(context).Create();
            new UnitOfMeasurementTypeMasterBuilder(context, 1).Create();
            new ModeMasterBuilder(context).Create();
            new DefaultSettingsForTenantsCreator(context, 1).Create();
            new CheckpointTypeMasterBuilder(context, 1).Create();
            new TransactionStatusMasterBuilder(context).Create();
            new HolidayTypeMasterBuilder(context, 1).Create();
            new MaterialTransferTypeMasterBuilder(context).Create();
            new FrequencyTypeMasterBuilder(context, 1).Create();
            new CalibrationStatusMasterBuilder(context, 1).Create();
            new CalibrationTestStatusMasterBuilder(context, 1).Create();
            new StatusMasterBuilder(context).Create();
            new CubicleCleaningTypeMasterBuilder(context).Create();
            new EquipmentCleaningTypeMasterBuilder(context).Create();
            new SamplingTypeMasterBuilder(context).Create();
            new DeviceTypeMasterBuilder(context, 1).Create();

        }

        public static List<TEntity> SeedEntityData<TEntity>(string fileName)
        {
            var fileFullPath = Path.Combine(AppContext.BaseDirectory + @"EntityFrameworkCore\Resources\" + fileName);
            var result = new List<TEntity>();
            if (File.Exists(fileFullPath))
            {
                using StreamReader reader = new StreamReader(fileFullPath);
                string json = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<List<TEntity>>(json);
            }
            return result;
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>();
            using var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress);
            var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

            contextAction(context);

            uow.Complete();
        }
    }
}