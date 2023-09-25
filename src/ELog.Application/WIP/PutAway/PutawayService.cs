using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;

using ELog.Application.WIP.PutAway.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;

using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.WIP.PutAway
{
    [PMMSAuthorize]

    public class PutawayService : ApplicationService, IPutawayService
    {
        private readonly IRepository<Putaway> _putawayrepository;
        private readonly IRepository<LocationMaster> _locationMasterrepository;
        private readonly IRepository<MaterialMaster> _materialmasterrepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrderMaterialRepository;
        private readonly IRepository<InProcessLabelDetails> _inProcessLagelRepository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public PutawayService(IRepository<Putaway> putawayrepository,
            IRepository<LocationMaster> locationMasterrepository,
            IRepository<MaterialMaster> materialmasterrepository,
            IRepository<ProcessOrderMaterialAfterRelease> processOrderMaterialRepository,
            IRepository<InProcessLabelDetails> inProcessLagelRepository




            )
        {
            _putawayrepository = putawayrepository;
            _locationMasterrepository = locationMasterrepository;
            _materialmasterrepository = materialmasterrepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _inProcessLagelRepository = inProcessLagelRepository;



            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;

        }

        [PMMSAuthorize(Permissions = "Putaway(WIP).Add")]
        public async Task<PutawayDto> CreateAsync(CreatePutawayDto input)
        {
            var putaway = ObjectMapper.Map<Putaway>(input);
            await _putawayrepository.InsertAsync(putaway);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PutawayDto>(putaway);
        }

        [PMMSAuthorize(Permissions = "Putaway(WIP).Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var approvalUserModule = await _putawayrepository.GetAsync(input.Id).ConfigureAwait(false);
            await _putawayrepository.DeleteAsync(approvalUserModule).ConfigureAwait(false);
        }


        [PMMSAuthorize(Permissions = "Putaway(WIP).View")]
        public async Task<PutawayDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _putawayrepository.GetAsync(input.Id);

            var a = getallDetails(input);
            // var entities = await AsyncQueryableExecuter.;

            // return (PutawayDto)rtn;


            return ObjectMapper.Map<PutawayDto>(a.FirstOrDefault());
            //return entities;

        }

        [PMMSAuthorize(Permissions = "Putaway(WIP).Edit")]
        public async Task<PutawayDto> UpdateAsync(PutawayDto input)
        {
            var putaway = await _putawayrepository.GetAsync(input.Id);
            ObjectMapper.Map(input, putaway);
            await _putawayrepository.UpdateAsync(putaway);
            return await GetAsync(input);
        }

        public async Task<PagedResultDto<PutawayListDto>> GetAllAsync(PagedPutawayResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<PutawayListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = "Putaway(WIP).View")]
        public async Task<PagedResultDto<PutawayListDto>> GetListAsync()
        {
            var query = from put in _putawayrepository.GetAll()
                        join loc in _locationMasterrepository.GetAll()
                        on put.LocationId equals loc.Id into ps
                        from loc in ps.DefaultIfEmpty()


                        select new PutawayListDto
                        {
                            Id = put.Id,
                            LocationId = put.LocationId,
                            LocationName = loc.LocationCode,
                            ContainerId = put.ContainerId,
                            ContainerCode = put.ContainerCode,
                            isActive = put.isActive
                        };


            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);


            return new PagedResultDto<PutawayListDto>(
                totalCount,
                entities
            );
        }

        protected IQueryable<PutawayDto> getallDetails(EntityDto<int> input)
        {
            var rtn = from a in _putawayrepository.GetAll()
                      join mm in _materialmasterrepository.GetAll()
                      on a.ProductCodeId equals mm.Id
                      join pom in _processOrderMaterialRepository.GetAll()
                       on a.ProcessOrderId equals pom.ProcessOrderId
                      join inp in _inProcessLagelRepository.GetAll()
                      on a.ContainerCode equals inp.ProcessLabelBarcode
                      where a.Id == input.Id
                      select new PutawayDto
                      {
                          BatchNo = pom.ProductBatchNo,
                          ContainerCode = a.ContainerCode,
                          ContainerId = a.ContainerId,
                          Id = a.Id,
                          isActive = a.isActive,
                          LocationId = a.LocationId,
                          StorageLocation = a.StorageLocation,
                          ProcessOrderId = a.ProcessOrderId,
                          ProcessOrderNo = a.ProcessOrderNo,
                          ProductCode = mm.MaterialCode,
                          ProductName = mm.MaterialDescription,
                          ProductCodeId = a.ProductCodeId,
                          NoOfContainer = inp.NoOfContainer

                      };

            return rtn;
        }

        protected IQueryable<PutawayListDto> CreateUserListFilteredQuery(PagedPutawayResultRequestDto input)
        {
            var ApprovalusermodulemappingQuery = from put in _putawayrepository.GetAll()
                                                 join loc in _locationMasterrepository.GetAll()
                                                 on put.LocationId equals loc.Id
                                                 join mm in _materialmasterrepository.GetAll()
                                                 on put.ProductCodeId equals mm.Id



                                                 select new PutawayListDto
                                                 {
                                                     Id = put.Id,
                                                     LocationId = put.LocationId,
                                                     LocationName = loc.LocationCode,
                                                     ContainerId = put.ContainerId,
                                                     ContainerCode = put.ContainerCode,
                                                     isActive = put.isActive,
                                                     ProcessOrder = put.ProcessOrderNo,
                                                     ProductCode = mm.MaterialCode,
                                                     ProductName = mm.MaterialDescription,
                                                     StorageLocation = put.StorageLocation
                                                 };
            if (input.LocationId != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.LocationId == input.LocationId);
            }
            if (input.ContainerCode != null)
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.ContainerCode == input.ContainerCode);
            }
            //if (input.ModuleId != null)
            //{
            //    ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.ModuleId == input.ModuleId);
            //}
            //if (input.SubModuleId != null)
            //{
            //    ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.SubModuleId == input.SubModuleId);
            //}
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x =>
                x.ContainerCode.Contains(input.Keyword) || x.LocationName.Contains(input.Keyword));
            }
            //if (input.ActiveInactiveStatusId != null)
            //{
            //    if (input.ActiveInactiveStatusId == (int)Status.In_Active)
            //    {
            //        ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => !x.IsActive);
            //    }
            //    else if (input.ActiveInactiveStatusId == (int)Status.Active)
            //    {
            //        ApprovalusermodulemappingQuery = ApprovalusermodulemappingQuery.Where(x => x.IsActive);
            //    }
            //}
            return ApprovalusermodulemappingQuery;
        }

        protected IQueryable<PutawayListDto> ApplySorting(IQueryable<PutawayListDto> query, PagedPutawayResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
            {
                return query.OrderBy(sortInput.Sorting);
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<PutawayListDto> ApplyPaging(IQueryable<PutawayListDto> query, PagedPutawayResultRequestDto input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }
    }
}
