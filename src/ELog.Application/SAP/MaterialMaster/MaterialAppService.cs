using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.SAP.MaterialMaster.Dto;
using ELog.Core.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ELog.Application.SAP.MaterialMaster
{
    [PMMSAuthorize]
    public class MaterialAppService : ApplicationService, IMaterialAppService
    {
        private readonly IRepository<Core.Entities.MaterialMaster> _materialMasterRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public MaterialAppService(IRepository<Core.Entities.MaterialMaster> materialMasterRepository)
        {
            _materialMasterRepository = materialMasterRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public async Task CreateAsync(SAPMaterial input)
        {
            var material = ObjectMapper.Map<ELog.Core.Entities.MaterialMaster>(input);
            if (_materialMasterRepository.Count() > 0)
            {
                var existingmaterial = await _materialMasterRepository.GetAll().FirstOrDefaultAsync(x => x.MaterialCode.ToLower() == input.MaterialCode.ToLower());

                if (existingmaterial != null)
                {
                    existingmaterial.BaseUOM = material.BaseUOM;
                    existingmaterial.Numerator = material.Numerator;
                    existingmaterial.Denominator = material.Denominator;
                    existingmaterial.MaterialDescription = material.MaterialDescription;
                    existingmaterial.Grade = material.Grade;
                    existingmaterial.ConversionUOM = material.ConversionUOM;
                    existingmaterial.MaterialType = material.MaterialType;
                    await _materialMasterRepository.UpdateAsync(existingmaterial);
                }
                else
                {
                    await _materialMasterRepository.InsertAsync(material);
                }
            }
            else
            {
                await _materialMasterRepository.InsertAsync(material);
            }


        }

        public async Task<SAPMaterial> GetAsync(string materialCode)
        {
            var existingmaterial = await _materialMasterRepository.GetAll().FirstOrDefaultAsync(x => x.MaterialCode.ToLower() == materialCode);
            var material = ObjectMapper.Map<SAPMaterial>(existingmaterial);
            return material;
        }
    }
}