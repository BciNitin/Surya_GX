using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;

using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

namespace ELog.Web.Core.Auditing
{
    public class PMMSAuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly ILogger<PMMSAuditingStore> _logger;

        /// <summary>
        /// Creates  a new <see cref="AuditingStore"/>.
        /// </summary>
        public PMMSAuditingStore(IRepository<AuditLog, long> auditLogRepository, ILogger<PMMSAuditingStore> logger)
        {
            _auditLogRepository = auditLogRepository;
            _logger = logger;
        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            await WriteLog(auditInfo);

        }

        public Task WriteLog(AuditInfo auditInfo)
        {

            if (auditInfo.Exception == null)
            {
                _logger.LogInformation("{BrowserInfo} {ClientIpAddress} {ClientName} {CustomData} {Exception}{ExecutionDuration}{ExecutionTime}{ImpersonatorTenantId}{ImpersonatorUserId}{MethodName}{Parameters}{ServiceName}{TenantId}{UserId}{ReturnValue}",
                auditInfo.BrowserInfo,
                auditInfo.ClientIpAddress,
                auditInfo.ClientName,
                auditInfo.CustomData,
                auditInfo.Exception,
                auditInfo.ExecutionDuration,
                auditInfo.ExecutionTime,
                auditInfo.ImpersonatorTenantId,
                auditInfo.ImpersonatorUserId,
                auditInfo.MethodName,
                auditInfo.Parameters,
                auditInfo.ServiceName,
                auditInfo.TenantId,
                auditInfo.UserId,
                auditInfo.ReturnValue);
            }
            else
            {
                _logger.LogError(auditInfo.Exception, "Exception Caught");
            }
            return Task.CompletedTask;
        }
        public void Save(AuditInfo auditInfo)
        {
            WriteLog(auditInfo);
        }
    }
}
