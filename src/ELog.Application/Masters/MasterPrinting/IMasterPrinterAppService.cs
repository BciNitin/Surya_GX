using Abp.Application.Services;
using System.Threading.Tasks;

namespace ELog.Application.Masters.MasterPrinting
{
    public interface IMasterPrinterAppService : IApplicationService
    {
        public Task<string> PrintLabel(string labelType);
        public Task<string> PrintBulkLabel(string labelType, string[] labelIds);
    }
}
