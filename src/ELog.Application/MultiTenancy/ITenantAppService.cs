using Abp.Application.Services;

using ELog.Application.MultiTenancy.Dto;

namespace ELog.Application.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

