using Abp.Application.Services;
using MobiVUE.Utility;
//using Elog.Application.CommonDto;
using MobiVueEVO.BO;

namespace MobiVueEvo.Application.Masters
{
    public interface ISpoolAppService : IApplicationService
    {
        Spool GetSpool(int id);

        DataList<Spool, long> GetSpools(SpoolMasterSearchCriteria machineTypeSearchCriteria);

        string SaveSpool(Spool machineType);

        void DeleteSpool(int id);
    }
}