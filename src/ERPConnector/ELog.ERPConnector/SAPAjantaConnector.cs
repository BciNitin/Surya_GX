using ELog.Adapter.SAPAjantaAdapter;
using ELog.Core.SAP;
using System.Threading.Tasks;

namespace ELog.ERPConnector
{
    public class SAPAjantaConnector : IConnector
    {
        private readonly ISAPAjantaAdapter _iSAPAjantaAdapter;

        public SAPAjantaConnector(ISAPAjantaAdapter iSAPAjantaAdapter)
        {
            _iSAPAjantaAdapter = iSAPAjantaAdapter;
        }

        public async Task<GRNRequestResponseDto> GRNPosting(GRNRequestResponseDto grnpostingdto)
        {
            var res = await _iSAPAjantaAdapter.GRNPosting(grnpostingdto);
            return res;
        }
        public async Task<IssueToProductionRequestResponseDto> IssueToProduction(IssueToProductionRequestResponseDto issueToProductionDto)
        {
            var res = await _iSAPAjantaAdapter.IssueToProduction(issueToProductionDto);
            return res;
        }
    }
}