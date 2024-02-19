using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.Plants.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Masters.Plants
{
    [PMMSAuthorize]
    public class PlantAppService : ApplicationService, IPlantAppService
    {
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        private readonly IRepository<UserPlants> _userPlantsRepository;
        private readonly IRepository<ChecklistTypeMaster> _checklistTypeRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public PlantAppService(IRepository<PlantMaster> plantRepository,
            IRepository<User, long> userRepository,
          IRepository<ApprovalStatusMaster> approvalStatusRepository,
          IRepository<UserPlants> userPlantsRepository,
          IRepository<ChecklistTypeMaster> checklistTypeRepository)
        {
            _plantRepository = plantRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _userPlantsRepository = userPlantsRepository;
            _checklistTypeRepository = checklistTypeRepository;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Plant_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PlantDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _plantRepository.GetAsync(input.Id);
            var plant = ObjectMapper.Map<PlantDto>(entity);
            plant.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
            return plant;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Plant_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<PlantListDto>> GetAllAsync(PagedPlantResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<PlantListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Plant_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<PlantDto> CreateAsync(CreatePlantDto input)
        {
            if (await _plantRepository.GetAll().AnyAsync(x => x.PlantId == input.PlantId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PlantAlreadyExist);
            }
            var plant = ObjectMapper.Map<PlantMaster>(input);
            plant.TenantId = AbpSession.TenantId;
            await _plantRepository.InsertAsync(plant);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PlantDto>(plant);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Plant_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<PlantDto> UpdateAsync(PlantDto input)
        {
            if (await _plantRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.PlantId == input.PlantId))
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PlantAlreadyExist);
            }
            if (!input.IsActive)
            {
                var associatedEntities = await GetAllAssociatedMasters(input.Id);
                if (associatedEntities.Count > 0)
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PlantCannotDeactivated);
                }
            }

            var plant = await _plantRepository.GetAsync(input.Id);
            if (plant.PlantTypeId != input.PlantTypeId)
            {
                var associatedEntities = await GetAllAssociatedMasters(input.Id);
                if (associatedEntities.Count > 0)
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PlantTypeCannotChanged);
                }
            }

            if (input.PlantTypeId == (int)PlantType.MasterPlant)
            {
                input.MasterPlantId = null;
            }

            ObjectMapper.Map(input, plant);
            await _plantRepository.UpdateAsync(plant);

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Plant_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var associatedEntities = await GetAllAssociatedMasters(input.Id);
            if (associatedEntities.Count > 0)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PlantDelete);
            }
            var plant = await _plantRepository.GetAsync(input.Id);
            await _plantRepository.DeleteAsync(plant);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Plant_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectPlantAsync(ApprovalStatusDto input)
        {
            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
            {
                var associatedEntities = await GetAllAssociatedMasters(input.Id);
                if (associatedEntities.Count > 0)
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PlantRejected);
                }
            }
            var plant = await _plantRepository.GetAsync(input.Id);
            plant.ApprovalStatusId = input.ApprovalStatusId;
            plant.ApprovalStatusDescription = input.Description;
            await _plantRepository.UpdateAsync(plant);
        }

        private async Task<List<string>> GetAllAssociatedMasters(int id)
        {
            List<string> lstAssociatedEntities = new List<string>();
            var entity = await (from plant in _plantRepository.GetAll()

                                join masterplant in _plantRepository.GetAll()
                                on plant.Id equals masterplant.MasterPlantId into plantMasters
                                from masterplant in plantMasters.DefaultIfEmpty()

                                join user in _userPlantsRepository.GetAll()
                                on plant.Id equals user.PlantId into userps
                                from user in userps.DefaultIfEmpty()

                                

                            

                              

                              

                              

                                join checklistType in _checklistTypeRepository.GetAll()
                                on plant.Id equals checklistType.SubPlantId into checklisttypePlants
                                from checklistType in checklisttypePlants.DefaultIfEmpty()

                                where plant.Id == id
                                select new
                                {
                                    userPlantId = user.PlantId,
                                    masterPlantId = masterplant.Id,
                                    checklisttypePlantId = checklistType.SubPlantId
                                }).FirstOrDefaultAsync() ?? default;
            if (entity?.userPlantId > 0)
            {
                lstAssociatedEntities.Add("User");
            }
            if (entity?.checklisttypePlantId > 0)
            {
                lstAssociatedEntities.Add("Checklist Type");
            }
            return lstAssociatedEntities;
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<PlantListDto> ApplySorting(IQueryable<PlantListDto> query, PagedPlantResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null && !sortInput.Sorting.IsNullOrWhiteSpace())
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
        protected IQueryable<PlantListDto> ApplyPaging(IQueryable<PlantListDto> query, PagedPlantResultRequestDto input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected IQueryable<PlantListDto> CreateUserListFilteredQuery(PagedPlantResultRequestDto input)
        {
            var plantQuery = from plant in _plantRepository.GetAll()
                             join approvalStatus in _approvalStatusRepository.GetAll()
                             on plant.ApprovalStatusId equals approvalStatus.Id into paStatus
                             from approvalStatus in paStatus.DefaultIfEmpty()
                             select new PlantListDto
                             {
                                 CountryId = plant.CountryId,
                                 Id = plant.Id,
                                 IsActive = plant.IsActive,
                                 License = plant.License,
                                 PlantId = plant.PlantId,
                                 PlantTypeId = plant.PlantTypeId,
                                 PlantName = plant.PlantName,
                                 ApprovalStatusId = plant.ApprovalStatusId,
                                 MasterPlantId = plant.MasterPlantId,
                                 UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                             };
            if (input.CountryId != null)
            {
                plantQuery = plantQuery.Where(x => x.CountryId == input.CountryId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                plantQuery = plantQuery.Where(x => x.PlantId.Contains(input.Keyword) || x.PlantName.Contains(input.Keyword));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    plantQuery = plantQuery.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    plantQuery = plantQuery.Where(x => x.IsActive);
                }
            }
            if (input.PlantTypeId != null)
            {
                plantQuery = plantQuery.Where(x => x.PlantTypeId == input.PlantTypeId);
            }
            if (input.ApprovalStatusId != null)
            {
                plantQuery = plantQuery.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }

            return plantQuery;
        }
    }
}