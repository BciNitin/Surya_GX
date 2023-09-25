using ELog.Core;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.StatusMaster;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class StatusMasterBuilder
    {
        private readonly PMMSDbContext _context;

        public StatusMasterBuilder(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //CreateStatusMaster();
        }

        //private void CreateStatusMaster()
        //{
        //    var moduleTuple = _context.ModuleMaster.IgnoreQueryFilters().Select(x => new { x.Id, x.Name }).ToList();
        //    var subModuleTuple = _context.SubModuleMaster.IgnoreQueryFilters().Select(x => new { x.Id, x.Name, x.SubModuleTypeId }).ToList();

        //    foreach (var status in SeedHelper.SeedEntityData<StatusSeedDto>(@PMMSConsts.StatusSeedFilePath))
        //    {
        //        var module = moduleTuple.FirstOrDefault(x => x.Name == status.ModuleName);
        //        var submodule = subModuleTuple.FirstOrDefault(x => x.Name == status.SubModuleName);
        //        var isSubmoduleStatusPresent = _context.StatusMaster.IgnoreQueryFilters().ToList().Where(l => l.ModuleId == module.Id && l.SubModuleId == submodule.Id && l.Status == status.Status);
        //        if (isSubmoduleStatusPresent.Count() == 0)
        //        {
        //            var submoduleStatus = new Core.Entities.StatusMaster
        //            {
        //                ModuleId = module.Id,
        //                SubModuleId = submodule.Id,
        //                Status = status.Status,
        //            };
        //            _context.StatusMaster.Add(submoduleStatus);
        //        }
        //    }
        //}
    }
}