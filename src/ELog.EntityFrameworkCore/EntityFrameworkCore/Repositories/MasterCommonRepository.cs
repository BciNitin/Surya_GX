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

        public IQueryable<AuditReportDetailsDto> GetAuditTrail()
        {
            return base.GetAuditTrail();

        }
        public IQueryable GetDynamicAuditTrail()
        {
            //return base.GetDynamicAuditTrail();
            return null;
        }
    }
}