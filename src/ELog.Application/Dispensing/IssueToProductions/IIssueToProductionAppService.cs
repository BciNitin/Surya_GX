using Abp.Application.Services;

using ELog.Application.Dispensing.IssueToProductions;
using ELog.Application.Dispensing.IssueToProductions.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Dispensing.CubicleAssignments
{
    public interface IIssueToProductionAppService : IApplicationService
    {
        Task<List<IssueToProductionDto>> CreateAsync(CreateIssueToProductionDto input);

    }
}