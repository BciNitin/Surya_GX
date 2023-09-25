using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Atesttable.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Atesttable
{
    public interface IAtesttableMasterService : IApplicationService
    {


        Task<AtesttableMasterDto> GetAsync(EntityDto<int> input);


    }


}
