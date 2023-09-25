using ELog.Core;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.ModuleSubModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class ModuleSubModuleBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public ModuleSubModuleBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateModules();
            CreateSubModules();
            CreateModuleSubModuleRelation();
        }

        private void CreateModules()
        {
            foreach (var module in SeedHelper.SeedEntityData<ModuleMaster>(PMMSConsts.ModuleSeedFilePath))
            {
                var existingModule = _context.ModuleMaster.IgnoreQueryFilters().FirstOrDefault(l => l.TenantId == _tenantId && l.Name.ToLower().Trim() == module.Name.ToLower().Trim());
                if (existingModule == null)
                {
                    module.TenantId = _tenantId;
                    module.CreationTime = DateTime.UtcNow;
                    _context.ModuleMaster.Add(module);
                }
            }
            _context.SaveChanges();
        }

        private void CreateSubModules()
        {
            var subModuleTypes = _context.SubModuleTypeMaster.IgnoreQueryFilters().Select(x => new { x.Id, x.SubModuleType }).ToList();
            foreach (var seedSubModule in SeedHelper.SeedEntityData<SubModuleSeedDto>(PMMSConsts.SubModuleSeedFilePath))
            {
                var existingModule = _context.SubModuleMaster.IgnoreQueryFilters().FirstOrDefault(l => l.TenantId == _tenantId && l.Name.ToLower().Trim() == seedSubModule.Name.ToLower().Trim());
                if (existingModule == null)
                {
                    var subModuleType = subModuleTypes.FirstOrDefault(x => x.SubModuleType == seedSubModule.SubModuleType)?.Id;
                    var subModule = new SubModuleMaster
                    {
                        Name = seedSubModule.Name,
                        DisplayName = seedSubModule.DisplayName,
                        Description = seedSubModule.Description,
                        Sequence = seedSubModule.Sequence,
                        IsActive = seedSubModule.IsActive,
                        SubModuleTypeId = subModuleType,
                        TenantId = _tenantId,
                        CreationTime = DateTime.UtcNow,
                        IsApprovalRequired = seedSubModule.IsApprovalRequired,
                        IsApprovalWorkflowRequired = seedSubModule.IsApprovalWorkflowRequired
                    };
                    _context.SubModuleMaster.Add(subModule);
                }
            }
            _context.SaveChanges();
        }

        private void CreateModuleSubModuleRelation()
        {
            var moduleTuple = _context.ModuleMaster.IgnoreQueryFilters().Select(x => new { x.Id, x.Name }).ToList();
            var subModuleTuple = _context.SubModuleMaster.IgnoreQueryFilters().Select(x => new { x.Id, x.Name, x.SubModuleTypeId }).ToList();
            var moduleSubModules = _context.ModuleSubModule.IgnoreQueryFilters().ToList();
            foreach (var moduleDto in SeedHelper.SeedEntityData<ModuleSubModuleSeedDto>(@PMMSConsts.ModuleSubModuleSeedFilePath))
            {
                var module = moduleTuple.Find(x => x.Name == moduleDto.ModuleName);
                var submodule = subModuleTuple.Find(x => x.Name == moduleDto.SubModuleName);
                var moduleSubModuleToUpdate = moduleSubModules.Find(l => l.TenantId == _tenantId && l.ModuleId == module.Id && l.SubModuleId == submodule.Id);
                if (moduleSubModuleToUpdate == null)
                {
                    var moduleSubModule = new Core.Entities.ModuleSubModule
                    {
                        ModuleId = module.Id,
                        SubModuleId = submodule.Id,
                        TenantId = _tenantId,
                        IsSelected = submodule.SubModuleTypeId == (int)PMMSEnums.SubModuleType.Mandatory,
                        IsMandatory = submodule.SubModuleTypeId == (int)PMMSEnums.SubModuleType.Mandatory,
                        CreationTime = DateTime.UtcNow
                    };
                    _context.ModuleSubModule.Add(moduleSubModule);
                }
            }
            _context.SaveChanges();
        }
    }
}