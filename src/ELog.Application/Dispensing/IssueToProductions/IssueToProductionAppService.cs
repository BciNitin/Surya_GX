using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommonDto;
using ELog.Application.CommonService.Dispensing;
using ELog.Application.Dispensing.IssueToProductions;
using ELog.Application.Dispensing.IssueToProductions.Dto;
using ELog.Application.Modules;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.Core.SAP;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Dispensing.CubicleAssignments
{
    [PMMSAuthorize]
    public class IssueToProductionAppService : ApplicationService, IIssueToProductionAppService
    {
        private readonly IRepository<ProcessOrder> _processOrderRepository;
        private readonly IRepository<StatusMaster> _statusRepository;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
        private readonly IRepository<IssueToProduction> _issueToProductionRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly ELog.ERPConnectorFactory.ERPConnectorFactory _eRPConnectorFactory;

        private readonly IModuleAppService _moduleAppService;
        private readonly IDispensingAppService _dispensingAppService;
        private readonly IRepository<ProcessOrderMaterial> _processOrdermaterialRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string completedStatus = nameof(PMMSEnums.DispensingHeaderStatus.Completed).ToLower();
        private readonly string dispensingInprogressStatus = nameof(PMMSEnums.DispensingHeaderStatus.InProgress).ToLower();
        public IssueToProductionAppService(IRepository<ProcessOrder> processOrderRepository,
          IRepository<StatusMaster> statusRepository,
          IHttpContextAccessor httpContextAccessor, ELog.ERPConnectorFactory.ERPConnectorFactory eRPConnectorFactory,
          IRepository<DispensingHeader> dispensingHeaderRepository, IRepository<PlantMaster> plantRepository,
          IRepository<DispensingDetail> dispensingDetailRepository, IRepository<IssueToProduction> issueToProductionRepository,
            IModuleAppService moduleAppService, IRepository<ProcessOrderMaterial> processOrdermaterialRepository,
            IDispensingAppService dispensingAppService)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _processOrderRepository = processOrderRepository;
            _httpContextAccessor = httpContextAccessor;
            _statusRepository = statusRepository;
            _issueToProductionRepository = issueToProductionRepository;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _moduleAppService = moduleAppService;
            _processOrdermaterialRepository = processOrdermaterialRepository;
            _dispensingAppService = dispensingAppService;
            _dispensingDetailRepository = dispensingDetailRepository;
            _plantRepository = plantRepository; _eRPConnectorFactory = eRPConnectorFactory;

        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.IssueToProduction_SubModule + "." + PMMSPermissionConst.Add + "," + PMMSPermissionConst.IssueToProduction_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<List<IssueToProductionDto>> CreateAsync(CreateIssueToProductionDto input)
        {
            var PostedIssueToProduction = await PostIssueToProduction(input);
            var issueToProductionDto = new List<IssueToProductionDto>();

            if (PostedIssueToProduction != null)
            {
                foreach (var item in PostedIssueToProduction.IssueToProductionRecords)
                {
                    var issueToProduction = ObjectMapper.Map<IssueToProduction>(item);
                    issueToProduction.TenantId = AbpSession.TenantId ?? 0;
                    issueToProduction.DispensingHeaderId = input.IssueToProductionDetails.FirstOrDefault().DispensingHeaderId;
                    await _issueToProductionRepository.InsertAsync(issueToProduction);

                    CurrentUnitOfWork.SaveChanges();
                    var mappedDto = ObjectMapper.Map<IssueToProductionDto>(issueToProduction);
                    issueToProductionDto.Add(mappedDto);
                }
            }
            return issueToProductionDto;
        }

        private async Task<IssueToProductionRequestResponseDto> PostIssueToProduction(CreateIssueToProductionDto input)
        {
            var issueToProductionDto = new IssueToProductionRequestResponseDto();
            foreach (var item in input.IssueToProductionDetails)
            {
                var record = new List<IssueToProductionRecord>();
                var issueToProductionRequest = ObjectMapper.Map<IssueToProductionRecord>(item);
                var erpConnFactory = _eRPConnectorFactory.GetERPConnector(ERPConnectorType.SAPAjanta);
                record.Add(issueToProductionRequest);
                issueToProductionDto.IssueToProductionRecords = record;
                var postedIssueToProductionDto = await erpConnFactory.IssueToProduction(issueToProductionDto);
            }
            return issueToProductionDto;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetDispensedProcessOrdersAsync(string input)
        {
            var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.DispensingSubModule);

            var dispensingComppletedStatus = await _statusRepository.GetAll()
                                .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == completedStatus).Select(a => a.Id).FirstOrDefaultAsync();
            var dispensinginProgressStatus = await _statusRepository.GetAll()
                              .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == dispensingInprogressStatus).Select(a => a.Id).FirstOrDefaultAsync();
            var dispensedProcessOrders = from po in _processOrderRepository.GetAll()
                                         join dispensinHeader in _dispensingHeaderRepository.GetAll()
                                         on po.Id equals dispensinHeader.ProcessOrderId
                                         join dispensingDetail in _dispensingDetailRepository.GetAll()
                                         on dispensinHeader.Id equals dispensingDetail.DispensingHeaderId
                                         where (dispensinHeader.StatusId == dispensingComppletedStatus || dispensinHeader.StatusId == dispensinginProgressStatus)
                                         select new
                                         {
                                             po.Id,
                                             po.ProcessOrderNo,
                                             po.IsReservationNo,
                                         };
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {
                input = input.Trim();
                dispensedProcessOrders = dispensedProcessOrders.Where(x => x.ProcessOrderNo.Contains(input)).Distinct();
            }
            return await dispensedProcessOrders.Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.ProcessOrderNo, IsReservationNo = x.IsReservationNo })
           .ToListAsync() ?? default;
        }

        public async Task<List<IssueToProductionDto>> GetDispensedProcessOrderMaterialsAsync(int processOrderId)
        {
            var moduleId = await _moduleAppService.GetModuleByName(PMMSConsts.DispensingSubModule);
            var submoduleId = await _moduleAppService.GetSubmoduleByName(PMMSConsts.DispensingSubModule);

            var dispensingComppletedStatus = await _statusRepository.GetAll()
                                .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == completedStatus).Select(a => a.Id).FirstOrDefaultAsync();
            var dispensinginProgressStatus = await _statusRepository.GetAll()
                              .Where(a => a.ModuleId == moduleId && a.SubModuleId == submoduleId && a.Status == dispensingInprogressStatus).Select(a => a.Id).FirstOrDefaultAsync();

            var dispensedProcessOrders = from dispensinHeader in _dispensingHeaderRepository.GetAll()
                                         join dispensingDetail in _dispensingDetailRepository.GetAll()
                                         on dispensinHeader.Id equals dispensingDetail.DispensingHeaderId
                                         join po in _processOrderRepository.GetAll()
                                         on dispensinHeader.ProcessOrderId equals po.Id
                                         join material in _processOrdermaterialRepository.GetAll()
                                         on po.Id equals material.ProcessOrderId
                                         where (dispensinHeader.StatusId == dispensingComppletedStatus || dispensinHeader.StatusId == dispensinginProgressStatus) && dispensinHeader.IsSampling == false
                                         && dispensinHeader.ProcessOrderId == processOrderId
                                         select new IssueToProductionDto
                                         {
                                             ProcessOrderNo = po.ProcessOrderNo,
                                             MaterialCode = material.ItemCode,
                                             LineItemNo = material.ItemNo,
                                             MaterialDescription = material.ItemDescription,
                                             Product = po.ProductCode,
                                             ProductBatch = material.BatchNo,
                                             ArNo = material.ARNo,
                                             SAPBatchNo = material.SAPBatchNo,
                                             DispensedQty = dispensingDetail.NoOfPacks != null ? dispensingDetail.NoOfPacks : dispensingDetail.NetWeight,
                                             UOM = material.UnitOfMeasurement,
                                             MaterialIssueNoteNo = null,
                                             DispensingHeaderId = dispensingDetail.Id,
                                             MvtType = po.IsReservationNo ? "201" : "261"
                                         };
            var result = await dispensedProcessOrders.Distinct().ToListAsync() ?? default;
            return result.GroupBy(x => new { x.MaterialCode }).
                Select(x => new IssueToProductionDto
                {
                    ProcessOrderNo = x.First().ProcessOrderNo,
                    MaterialCode = x.First().MaterialCode,
                    LineItemNo = x.First().LineItemNo,
                    MaterialDescription = x.First().MaterialDescription,
                    Product = x.First().Product,
                    ProductBatch = x.First().ProductBatch,
                    ArNo = x.First().ArNo,
                    SAPBatchNo = x.First().SAPBatchNo,
                    DispensedQty = x.First().DispensedQty,
                    UOM = x.First().UOM,
                    Storage_location = x.First().Storage_location,
                    MaterialIssueNoteNo = null,
                    DispensingHeaderId = x.First().DispensingHeaderId,
                    MvtType = x.First().MvtType
                })
                .OrderBy(x => x.MaterialCode).ToList();
        }
    }
}