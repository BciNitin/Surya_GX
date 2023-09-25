using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease.Dto;

using System.Threading.Tasks;

namespace ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease
{
    public interface IProcessOrderAfterReleasAppService : IApplicationService
    {
        Task<string> InsertUpdateProcessOrderAfterReleaseAsync(ProcessOrderAfterReleasDto input);
        Task<ProcessOrderAfterReleasDto> GetAsync(EntityDto<int> input);
    }
}
