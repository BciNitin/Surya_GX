using Abp.Application.Services;
using ELog.Application.FinishedGoods.SAP_FG.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.FinishedGoods.SAP_FG
{
    public interface IOBDDetailsService : IApplicationService
    {
        Task<string> InsertOrUpdateOBDDetailsAsync(List<OBDDetailDto> inp);
    }
}
