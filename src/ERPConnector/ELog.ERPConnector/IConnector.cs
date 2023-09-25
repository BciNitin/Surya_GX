using ELog.Core.SAP;
using System.Threading.Tasks;

namespace ELog.ERPConnector
{
    public interface IConnector
    {
        Task<GRNRequestResponseDto> GRNPosting(GRNRequestResponseDto grnpostingdto);
        Task<IssueToProductionRequestResponseDto> IssueToProduction(IssueToProductionRequestResponseDto grnpostingdto);
    }
}