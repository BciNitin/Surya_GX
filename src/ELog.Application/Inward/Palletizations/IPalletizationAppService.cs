using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Inward.Palletizations.Dto;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Inward.Palletizations
{
    public interface IPalletizationAppService
    {
        Task<List<PalletizationDto>> GetAsync(EntityDto<Guid> input);

        Task<PagedResultDto<PalletizationListDto>> GetAllAsync(PagedPalletizationResultRequestDto input);

        Task<PalletizationDto> CreateAsync(CreatePalletizationDto input);

        Task UnloadPalletsAsync(Guid transactionId, List<PalletizationDto> PalletizationSelected);

        Task<List<SelectListDtoWithPlantId>> GetAllPalletsAsync();
    }
}