using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq;

using ELog.Application.Atesttable.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Atesttable
{
    [PMMSAuthorize]
    public class AtesttableMasterService : ApplicationService, IAtesttableMasterService
    {
        private readonly IRepository<PalletMaster> _palletRepository;
        private readonly IRepository<AtesttableMaster> _atesttableMasterRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<HandlingUnitTypeMaster> _handlingUnitTypeMasterRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly IRepository<LabelPrintPacking> _labelPrintPackingRepository;
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;



        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;

        public AtesttableMasterService(IRepository<AtesttableMaster> atesttableMasterRepository, IRepository<PalletMaster> palletRepository, IRepository<DepartmentMaster> departmentRepository, IRepository<PlantMaster> plantRepository,
            IRepository<StandardWeightMaster> standardWeightRepository, IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
            IRepository<LocationMaster> locationRepository, IRepository<CubicleMaster> cubicleRepository,
            IMasterCommonRepository masterCommonRepository, IRepository<HandlingUnitMaster> handlingunitRepository, IRepository<HandlingUnitTypeMaster> _handlingUnitTypeMasterRepository,
            IRepository<LabelPrintPacking> labelPrintPackingRepository,
               IRepository<ProcessOrderAfterRelease> processOrderRepository,
                    IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository)

        {

            _atesttableMasterRepository = atesttableMasterRepository;
            _palletRepository = palletRepository;
            _plantRepository = plantRepository;
            _standardWeightRepository = standardWeightRepository;
            _standardWeightBoxRepository = standardWeightBoxRepository;
            _cubicleRepository = cubicleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _handlingUnitRepository = handlingunitRepository;
            _labelPrintPackingRepository = labelPrintPackingRepository;
            _processOrderRepository = processOrderRepository;
            _processOrdermaterialRepository = processOrdermaterialRepository;
        }

        public async Task<AtesttableMasterDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _palletRepository.GetAsync(input.Id);
            return ObjectMapper.Map<AtesttableMasterDto>(entity);
        }




    }
}
