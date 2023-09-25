using Abp.Application.Services;

namespace ELog.Application
{
    public interface IRMStockTakeAppService : IApplicationService
    {
        string GetPhysicalStock(string sStockArea, string sBarcode);

    }
}
