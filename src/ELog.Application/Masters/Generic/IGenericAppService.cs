using Abp.Application.Services;
//using MobiVueEvo.Utility;
using MobiVueEVO.BO;

namespace MobiVueEvo.Application.Masters
{
    public interface IGenericAppService : IApplicationService
    {
        Bay GetBay(int id);

        //DataList<Bay, long> GetBays(BaySearchCriteria criteria);

        //long GetBayByCode(BaySearchCriteria BaySearchCriteria);

        string SaveBay(Bay Bay);

        void DeleteBay(int id);

        //List<KeyValue<short, string>> GetUOMs();

        //string PrintBay(string code);
    }
}