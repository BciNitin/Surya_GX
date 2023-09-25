using ELog.Core.SAP;
using System.Threading.Tasks;

namespace ELog.Adapter.SAPAjantaAdapter
{
    public interface ISAPAjantaAdapter
    {
        Task<GRNRequestResponseDto> GRNPosting(GRNRequestResponseDto grnpostingdto);
        Task<IssueToProductionRequestResponseDto> IssueToProduction(IssueToProductionRequestResponseDto issueToProductionDto);

    }
}