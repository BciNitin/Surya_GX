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

        public IQueryable<AuditReportDetailsDto> GetAuditTrail()
        {
            return Context.AuditSQLResult.FromSqlRaw(SQLQueryConst.AUDIT_REPORT_SQL);
        }
        //public IQueryable GetDynamicAuditTrail()
        //{

        //    return Context.Consumptions.FromSqlRaw(SQLQueryConst.AUDIT_DYNAMIC_REPORT);
        //}

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