using Abp.Application.Services;

using ELog.Application.CommonDto;

using System.Threading.Tasks;

namespace ELog.Application.MaterialStatusLabel
{
    public interface IMaterialStatusLabelAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetMaterialStatusLabelByMaterialBarcode(string materialBarcode);
    }
}