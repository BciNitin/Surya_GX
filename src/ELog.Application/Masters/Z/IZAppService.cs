using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Masters.Z.Dto;
using System.Threading.Tasks;

namespace MobiVueEvo.Application.Masters.Z
{
    public interface IZAppService : IApplicationService
    {
        Task<ZDto> GetAsync(EntityDto<int> input);


    }
}