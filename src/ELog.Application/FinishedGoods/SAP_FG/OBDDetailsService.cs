using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.FinishedGoods.SAP_FG.Dto;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.FinishedGoods.SAP_FG
{
    public class OBDDetailsService : ApplicationService, IOBDDetailsService
    {
        private readonly IRepository<OBDDetails> _obdDetails;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OBDDetailsService(IRepository<OBDDetails> obdDetails, IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor)

        {
            _obdDetails = obdDetails;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<string> InsertOrUpdateOBDDetailsAsync(List<OBDDetailDto> data)
        {
            var result = "";
            foreach (var input in data)
            {
                var obdid = await GetOBDCodeIfAlradyExist(input);
                var dictInsertedOrUpdatedObd = await InsertOrUpdateOBDDetails(input, obdid);
                dictInsertedOrUpdatedObd.ToString();
                result = dictInsertedOrUpdatedObd.ToString();
                //return dictInsertedOrUpdatedObd.ToString();
            }

            return result;
        }

        public async Task<int> GetOBDCodeIfAlradyExist(OBDDetailDto input)
        {
            var plantId = input.OBD;

            return await _obdDetails.GetAll().Where(x => x.OBD.ToLower() == input.OBD.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }

        private async Task<int> InsertOrUpdateOBDDetails(OBDDetailDto input, int obdid)
        {


            Dictionary<int, int> dictInsertedOrUpdatedrecipeId = new Dictionary<int, int>();
            if (obdid > 0)
            {
                var obd = await _obdDetails.GetAsync(obdid);
                ObjectMapper.Map(input, obd);
                await _obdDetails.UpdateAsync(obd);
                CurrentUnitOfWork.SaveChanges();

            }
            else
            {
                var obd = ObjectMapper.Map<OBDDetails>(input);
                var insertedObd = await _obdDetails.InsertAsync(obd);
                CurrentUnitOfWork.SaveChanges();
                obdid = insertedObd.Id;
            }


            return obdid;
        }
    }
}
