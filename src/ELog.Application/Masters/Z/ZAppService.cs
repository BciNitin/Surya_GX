using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.Masters.Z.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using MobiVueEvo.Application.Masters.Z;
using System.Threading.Tasks;

namespace ELog.Application.Masters.Z
{
    [PMMSAuthorize]
    public class ZAppService : ApplicationService, IZAppService
    {
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;

        public ZAppService(IRepository<AreaMaster> areaRepository, IRepository<DepartmentMaster> departmentRepository, IRepository<PlantMaster> plantRepository,
            IRepository<StandardWeightMaster> standardWeightRepository, IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
            IRepository<LocationMaster> locationRepository, IRepository<CubicleMaster> cubicleRepository,
            IMasterCommonRepository masterCommonRepository, IHttpContextAccessor httpContextAccessor, IRepository<ApprovalStatusMaster> approvalStatusRepository)

        {
            _areaRepository = areaRepository;
            _plantRepository = plantRepository;
            _standardWeightRepository = standardWeightRepository;
            _standardWeightBoxRepository = standardWeightBoxRepository;
            _cubicleRepository = cubicleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _httpContextAccessor = httpContextAccessor;
            _approvalStatusRepository = approvalStatusRepository;
        }


        public async Task<ZDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _areaRepository.GetAsync(input.Id);
            var area = ObjectMapper.Map<ZDto>(entity);
            return area;
        }


    }
}