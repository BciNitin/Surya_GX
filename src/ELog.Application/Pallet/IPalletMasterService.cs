using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Pallet.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Pallet
{
    public interface IPalletMasterService : IApplicationService
    {
        Task<PalletMasterDto> CreateAsync(PalletMasterDto input);

        Task<PalletMasterDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<PalletMasterListDto>> GetAllAsync(PagedPalletMasterResultRequestDto input);

        Task<PalletMasterDto> UpdateAsync(PalletMasterDto input);
        Task DeleteAsync(EntityDto<int> input);

        Task<List<PalletMasterDto>> GetGridShipperCodeAsync();
        Task<int> GetShipperCountAsync(string input);
    }


}
