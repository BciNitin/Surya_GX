using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.Inward.WeightCaptures.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ELog.Application.Inward.WeightCaptures
{
    [PMMSAuthorize]
    public class WeightCaptureAppService : ApplicationService, IWeightCaptureAppService
    {
        #region fields

        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<WeightCaptureHeader> _weightCaptureHeaderRepository;
        private readonly IRepository<WeightCaptureDetail> _weightCaptureDetailRepository;
        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IRepository<MaterialConsignmentDetail> _materialConsignmentRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineMasterRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion fields

        #region constructor

        public WeightCaptureAppService(IRepository<PurchaseOrder> purchaseOrderRepository,
            IRepository<WeightCaptureHeader> weightCaptureHeaderRepository,
            IRepository<WeightCaptureDetail> weightCaptureDetailRepository,
            IRepository<InvoiceDetail> invoiceDetailRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<MaterialConsignmentDetail> materialConsignmentRepository,
            IRepository<Material> materialRepository, IRepository<WeighingMachineMaster> weighingMachineMasterRepository
            )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _purchaseOrderRepository = purchaseOrderRepository;
            _weightCaptureHeaderRepository = weightCaptureHeaderRepository;
            _weightCaptureDetailRepository = weightCaptureDetailRepository;
            _materialConsignmentRepository = materialConsignmentRepository;
            _materialRepository = materialRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _weighingMachineMasterRepository = weighingMachineMasterRepository;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeightCapture_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<WeightCaptureDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _weightCaptureHeaderRepository.GetAsync(input.Id);
            var weightCapture = ObjectMapper.Map<WeightCaptureDto>(entity);
            var weightCaptureDetails = await (from detail in _weightCaptureDetailRepository.GetAll()
                                              join weighingMachineMaster in _weighingMachineMasterRepository.GetAll()
                                              on detail.ScanBalanceId equals weighingMachineMaster.Id
                                              where detail.WeightCaptureHeaderId == input.Id
                                              select new WeightCaptureDetailsDto
                                              {
                                                  Id = detail.Id,
                                                  ContainerNo = detail.ContainerNo,
                                                  GrossWeight = detail.GrossWeight,
                                                  NetWeight = detail.NetWeight,
                                                  ScanBalanceId = detail.ScanBalanceId,
                                                  WeightCaptureHeaderId = detail.WeightCaptureHeaderId,
                                                  TareWeight = detail.TareWeight,
                                                  NoOfPacks = detail.NoOfPacks,
                                                  WeighingMachineCode = weighingMachineMaster.WeighingMachineCode,
                                                  LeastCountDigitAfterDecimal = weighingMachineMaster.LeastCountDigitAfterDecimal
                                              }).ToListAsync() ?? default;

            weightCapture.WeightCaptureDetailsDto = new WeightCaptureDetailsDto();
            if (weightCaptureDetails?.Count > 0)
            {
                weightCapture.WeightCaptureHeaderDetails = weightCaptureDetails;
            }
            await MapPoAndInvoice(weightCapture);
            return weightCapture;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeightCapture_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<WeightCaptureListDto>> GetAllAsync(PagedWeightCaptureResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new PagedResultDto<WeightCaptureListDto>(
                totalCount,
                entities
            );
        }

        /// <summary>
        /// This method will insert Weight Capture Header and details.
        /// </summary>
        /// <param name="input"></param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeightCapture_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<WeightCaptureDto> CreateAsync(CreateWeightCaptureDto input)
        {
            var weightCapture = ObjectMapper.Map<WeightCaptureHeader>(input);
            weightCapture.TenantId = AbpSession.TenantId;
            await _weightCaptureHeaderRepository.InsertAsync(weightCapture);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<WeightCaptureDto>(weightCapture);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeightCapture_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<WeightCaptureDetailsDto> InsertWeightCaptureDetail(WeightCaptureDetailsDto input)
        {
            var weightCapture = ObjectMapper.Map<WeightCaptureDetail>(input);
            weightCapture.TenantId = AbpSession.TenantId;
            await _weightCaptureDetailRepository.InsertAsync(weightCapture);
            CurrentUnitOfWork.SaveChanges();
            var WeightCaptureDetailsDto = ObjectMapper.Map<WeightCaptureDetailsDto>(weightCapture);
            await MapDeviceCode(WeightCaptureDetailsDto);
            return WeightCaptureDetailsDto;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeightCapture_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var weightCapture = await _weightCaptureHeaderRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _weightCaptureHeaderRepository.DeleteAsync(weightCapture).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeightCapture_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task DeleteWeightCaptureDetailsAsync(EntityDto<int> input)
        {
            var weightCaptureDetails = await _weightCaptureDetailRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _weightCaptureDetailRepository.DeleteAsync(weightCaptureDetails).ConfigureAwait(false);
        }
        public async Task<int?> IsWeightCapturePresent(int? PurchaseOrderId, int? InvoiceId, int? MaterialId, int? MfgBatchNoId)
        {
            return await _weightCaptureHeaderRepository.GetAll().Where(x => x.PurchaseOrderId == PurchaseOrderId && x.InvoiceId == InvoiceId && x.MaterialId == MaterialId
             && x.MfgBatchNoId == MfgBatchNoId).Select(x => x.Id).FirstOrDefaultAsync();
        }
        #endregion public

        #region private

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<WeightCaptureListDto> ApplySorting(IQueryable<WeightCaptureListDto> query, PagedWeightCaptureResultRequestDto input)
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
        protected IQueryable<WeightCaptureListDto> ApplyPaging(IQueryable<WeightCaptureListDto> query, PagedWeightCaptureResultRequestDto input)
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

        protected IQueryable<WeightCaptureListDto> CreateUserListFilteredQuery(PagedWeightCaptureResultRequestDto input)
        {
            var subPlantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var WeightCaptureHeaderQuery = from weightCapture in _weightCaptureHeaderRepository.GetAll()
                                           join invoice in _invoiceDetailRepository.GetAll()
                                           on weightCapture.InvoiceId equals invoice.Id
                                           join purchaseOrder in _purchaseOrderRepository.GetAll()
                                           on invoice.PurchaseOrderId equals purchaseOrder.Id
                                           join material in _materialRepository.GetAll()
                                           on weightCapture.MaterialId equals material.Id
                                           join consignment in _materialConsignmentRepository.GetAll()
                                           on weightCapture.MfgBatchNoId equals consignment.Id
                                           select new WeightCaptureListDto
                                           {
                                               Id = weightCapture.Id,
                                               PurchaseOrderId = purchaseOrder.Id,
                                               InvoiceNo = invoice.InvoiceNo,
                                               PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                                               MaterialItemCode = material.ItemCode + "-" + material.ItemNo,
                                               ManufacturedBatchNo = consignment.ManufacturedBatchNo,
                                               SubPlantId = purchaseOrder.PlantId,
                                               MaterialId = material.Id,
                                               ItemDescription = material.ItemDescription,
                                           };
            if (input.PurchaseOrderId != null)
            {
                WeightCaptureHeaderQuery = WeightCaptureHeaderQuery.Where(x => x.PurchaseOrderId == input.PurchaseOrderId);
            }
            if (input.MaterialId != null)
            {
                WeightCaptureHeaderQuery = WeightCaptureHeaderQuery.Where(x => x.MaterialId == input.MaterialId);
            }
            if (!(string.IsNullOrEmpty(subPlantId) || string.IsNullOrWhiteSpace(subPlantId)))
            {
                WeightCaptureHeaderQuery = WeightCaptureHeaderQuery.Where(x => x.SubPlantId == Convert.ToInt32(subPlantId));
            }
            return WeightCaptureHeaderQuery;
        }

        private WeightCaptureDetailsDto MapWeightCaptureDetail(WeightCaptureDetail input)
        {
            return ObjectMapper.Map<WeightCaptureDetailsDto>(input);
        }

        private async Task MapPoAndInvoice(WeightCaptureDto weightCapture)
        {
            var poDetails = await _purchaseOrderRepository.FirstOrDefaultAsync(a => a.Id == weightCapture.PurchaseOrderId);
            var invoiceDetails = await _invoiceDetailRepository.FirstOrDefaultAsync(a => a.Id == weightCapture.InvoiceId);
            weightCapture.invoiceNo = invoiceDetails.InvoiceNo;
            weightCapture.PurchaseOrderNo = poDetails.PurchaseOrderNo;
        }
        private async Task MapDeviceCode(WeightCaptureDetailsDto weightCapture)
        {
            var weighingMachine = await _weighingMachineMasterRepository.FirstOrDefaultAsync(a => a.Id == weightCapture.ScanBalanceId);
            weightCapture.WeighingMachineCode = weighingMachine.WeighingMachineCode;
            weightCapture.LeastCountDigitAfterDecimal = weighingMachine.LeastCountDigitAfterDecimal;
        }

        #endregion private
    }
}