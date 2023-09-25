using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.UI;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using ELog.Application.Modules;
using ELog.Application.Reports.Dto;
using ELog.Application.Sessions;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using ELog.Core.SQLDtoEntities;
using Microsoft.EntityFrameworkCore.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using static ELog.Core.PMMSEnums;
using ELog.Application.WeighingCalibrations;
using ELog.Application.WeighingCalibrations.Dto;
using Abp.Application.Services.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using ELog.EntityFrameworkCore.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.Extensions.Configuration;


namespace ELog.Application.Masters.Areas
{



    [PMMSAuthorize]
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IConfiguration _configuration;
        private readonly PMMSDbContext _context;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<CheckpointTypeMaster> _checkpointTypeRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<InspectionChecklistMaster> _checklistRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<ProcessOrder> _processOrderRepository;
        private readonly IRepository<InspectionLot> _inspectionLotRepository;
        private readonly IRepository<ProcessOrderMaterial> _processOrderMaterialRepository;
        private readonly IRepository<GateEntry> _gateEntryRepository;
        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IRepository<ModeMaster> _modeRepository;
        private readonly IRepository<VehicleInspectionHeader> _vehicleInspectionHeaderRepository;
        private readonly IRepository<VehicleInspectionDetail> _vehicleInspectionDetailRepository;
        private readonly IRepository<MaterialInspectionHeader> _materialInspectionHeaderRepository;
        private readonly IRepository<MaterialChecklistDetail> _materialChecklistDetailsRepository;
        private readonly IRepository<MaterialInspectionRelationDetail> _materialRelationDetailsRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IRepository<TransactionStatusMaster> _transactionStatusRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<GRNDetail> _grnDetailRepository;
        private readonly IRepository<StatusMaster> _statusRepository;
        private readonly IRepository<GRNMaterialLabelPrintingContainerBarcode> _grnMaterialLabelPrintingContainerBarcodeRepository;
        private readonly IRepository<PutAwayBinToBinTransfer> _putAwayBinToBinTrasferRepository;
        private readonly IRepository<AreaMaster> _areaRepository;
        private readonly IRepository<CubicleAssignmentHeader> _cubicleAssignHeaderRepository;
        private readonly IRepository<CubicleAssignmentDetail> _cubicleAssignDetailRepository;
        private readonly IRepository<CubicleMaster> _cubicleMasterRepository;
        private readonly IRepository<LineClearanceCheckpoint> _lineClearanceCheckPointRepository;
        private readonly IRepository<LineClearanceTransaction> _lineClearanceTransactionRepository;
        private readonly IRepository<DispensingHeader> _dispensingHeaderRepository;
        private readonly IRepository<DispensingDetail> _dispensingDetailRepository;
        private readonly IRepository<DispatchDetail> _dispatchDetailRepository;
        private readonly IRepository<Loading> _loadingRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private readonly IRepository<CubicleCleaningTransaction> _cubicleCleaningTransactionRepository;
        private readonly IRepository<CubicleCleaningCheckpoint> _cubicleCleaningCheckpointRepository;
        private readonly IRepository<CubicleCleaningTypeMaster> _cubicleCleaningTypeMasterRepository;
        private readonly IRepository<EquipmentCleaningTransaction> _equipmentCleaningTransactionRepository;
        private readonly IRepository<EquipmentCleaningCheckpoint> _equipmentCleaningCheckpointRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<EquipmentCleaningTypeMaster> _equipmentCleaningTypeMasterRepository;
        private readonly IRepository<ReportConfiguration> _reportConfigurationRepository;
        private readonly IModuleAppService _moduleAppService;
        private readonly IRepository<MaterialBatchDispensingHeader> _materialBatchDispensingHeaderRepository;
        private readonly IRepository<StageOutHeader> _stageOutHeaderRepository;
        private readonly IRepository<WMCalibratedLatestMachineDetail> _wmcalibratedMachineRepository;
        private readonly IRepository<WMCalibrationHeader> _wMCalibrationHeaderRepository;
        private readonly IWeighingCalibrationAppService _weighingCalibrationAppService;
        public ReportAppService(IRepository<PurchaseOrder> purchaseOrderRepository,
          IRepository<GateEntry> gateEntryRepository, IConfiguration configuration,
          IRepository<InvoiceDetail> invoiceDetailRepository,
          IRepository<CheckpointMaster> checkpointRepository,
          IRepository<CheckpointTypeMaster> checkpointTypeRepository,
          IRepository<InspectionChecklistMaster> checklistRepository,
          IRepository<ModeMaster> modeRepository, IWeighingCalibrationAppService weighingCalibrationAppService,
          IRepository<AreaMaster> areaRepository, IRepository<WMCalibrationHeader> wMCalibrationHeaderRepository,
          IHttpContextAccessor httpContextAccessor,
          IRepository<VehicleInspectionHeader> vehicleInspectionHeaderRepository,
          IRepository<VehicleInspectionDetail> vehicleInspectionDetailRepository,
          IRepository<MaterialInspectionHeader> materialInspectionHeaderRepository,
          ISessionAppService sessionAppService,
           IRepository<CubicleMaster> cubicleMasterRepository,
          IRepository<MaterialChecklistDetail> materialChecklistDetailsRepository,
          IRepository<MaterialInspectionRelationDetail> materialRelationDetailsRepository,
          IRepository<TransactionStatusMaster> transactionStatusRepository,
          IRepository<User, long> userRepository, IRepository<PlantMaster> plantRepository,
          IRepository<HandlingUnitMaster> handlingUnitRepository,
          IRepository<Material> materialRepository, IRepository<GRNDetail> grnDetailRepository,
          IRepository<GRNMaterialLabelPrintingContainerBarcode> grnMaterialLabelPrintingContainerBarcodeRepository,
          IRepository<Palletization> palletizationRepository,
          IRepository<LocationMaster> locationRepository, IRepository<PutAwayBinToBinTransfer> putAwayBinToBinTrasferRepository,
          IRepository<CubicleAssignmentHeader> cubicleAssignHeaderRepository,
          IRepository<CubicleAssignmentDetail> cubicleAssignDetailRepository,
          IRepository<ProcessOrder> processOrderRepository, IRepository<ReportConfiguration> reportConfigurationRepository,
          IRepository<ProcessOrderMaterial> processOrderMaterialRepository,
          IRepository<InspectionLot> inspectionLotRepository, IRepository<WMCalibratedLatestMachineDetail> wmcalibratedMachineRepository,
          IRepository<StatusMaster> statusRepository, IRepository<CubicleCleaningTransaction> cubicleCleaningTransactionRepository,
          IRepository<DispensingHeader> dispensingHeaderRepository, IRepository<EquipmentCleaningTransaction> equipmentCleaningTransactionRepository,
          IRepository<DispensingDetail> dispensingDetailRepository, IRepository<CubicleCleaningCheckpoint> cubicleCleaningCheckpointRepository,
          IRepository<DispatchDetail> dispatchDetailRepository,
            IRepository<Loading> loadingRepository,
          IRepository<WeighingMachineMaster> weighingMachineRepository, IRepository<EquipmentCleaningCheckpoint> equipmentCleaningCheckpointRepository,
          IRepository<LineClearanceCheckpoint> lineClearanceCheckPointRepository, IModuleAppService moduleAppService,
          IRepository<LineClearanceTransaction> lineClearanceTransactionRepository, IRepository<CubicleCleaningTypeMaster> cubicleCleaningTypeMasterRepository,
          IMasterCommonRepository masterCommonRepository, IRepository<EquipmentMaster> equipmentRepository,
          IRepository<EquipmentCleaningTypeMaster> equipmentCleaningTypeMasterRepository,
          IRepository<MaterialBatchDispensingHeader> materialBatchDispensingHeaderRepository,
          IRepository<StageOutHeader> stageOutHeaderRepository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _purchaseOrderRepository = purchaseOrderRepository;
            _gateEntryRepository = gateEntryRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _invoiceDetailRepository = invoiceDetailRepository;
            _checkpointRepository = checkpointRepository;
            _modeRepository = modeRepository;
            _statusRepository = statusRepository;
            _vehicleInspectionHeaderRepository = vehicleInspectionHeaderRepository;
            _vehicleInspectionDetailRepository = vehicleInspectionDetailRepository;
            _sessionAppService = sessionAppService;
            _transactionStatusRepository = transactionStatusRepository;
            _checklistRepository = checklistRepository;
            _areaRepository = areaRepository;
            _checkpointRepository = checkpointRepository;
            _checkpointTypeRepository = checkpointTypeRepository;
            _materialInspectionHeaderRepository = materialInspectionHeaderRepository;
            _materialChecklistDetailsRepository = materialChecklistDetailsRepository;
            _materialRelationDetailsRepository = materialRelationDetailsRepository;
            _userRepository = userRepository;
            _handlingUnitRepository = handlingUnitRepository;
            _materialRepository = materialRepository;
            _grnDetailRepository = grnDetailRepository;
            _plantRepository = plantRepository;
            _grnMaterialLabelPrintingContainerBarcodeRepository = grnMaterialLabelPrintingContainerBarcodeRepository;
            _equipmentRepository = equipmentRepository;
            _httpContextAccessor = httpContextAccessor;
            _palletizationRepository = palletizationRepository;
            _locationRepository = locationRepository;
            _putAwayBinToBinTrasferRepository = putAwayBinToBinTrasferRepository;
            _cubicleMasterRepository = cubicleMasterRepository;
            _processOrderRepository = processOrderRepository;
            _processOrderMaterialRepository = processOrderMaterialRepository;
            _inspectionLotRepository = inspectionLotRepository;
            _cubicleAssignHeaderRepository = cubicleAssignHeaderRepository;
            _cubicleAssignDetailRepository = cubicleAssignDetailRepository;
            _masterCommonRepository = masterCommonRepository;
            _lineClearanceCheckPointRepository = lineClearanceCheckPointRepository;
            _lineClearanceTransactionRepository = lineClearanceTransactionRepository;
            _dispensingHeaderRepository = dispensingHeaderRepository;
            _dispensingDetailRepository = dispensingDetailRepository;
            _dispatchDetailRepository = dispatchDetailRepository;
            _loadingRepository = loadingRepository;
            _weighingMachineRepository = weighingMachineRepository;
            _cubicleCleaningTransactionRepository = cubicleCleaningTransactionRepository;
            _cubicleCleaningCheckpointRepository = cubicleCleaningCheckpointRepository;
            _cubicleCleaningTypeMasterRepository = cubicleCleaningTypeMasterRepository;
            _equipmentCleaningTransactionRepository = equipmentCleaningTransactionRepository;
            _equipmentCleaningCheckpointRepository = equipmentCleaningCheckpointRepository;
            _equipmentCleaningTypeMasterRepository = equipmentCleaningTypeMasterRepository;
            _reportConfigurationRepository = reportConfigurationRepository;
            _moduleAppService = moduleAppService;
            _materialBatchDispensingHeaderRepository = materialBatchDispensingHeaderRepository;
            _stageOutHeaderRepository = stageOutHeaderRepository;
            _wmcalibratedMachineRepository = wmcalibratedMachineRepository;
            _weighingCalibrationAppService = weighingCalibrationAppService;
            _wMCalibrationHeaderRepository = wMCalibrationHeaderRepository;
        }



        [PMMSAuthorize(Permissions = PMMSPermissionConst.VehicleInspectionReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<VehicleInspectionReportResultDto>> GetVehicleInspectionReportDetailsAsync(VehicleInspectionReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.VehicleInspection);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = CreateVehicleInspectionReportFilteredQuery(input);
            var vehicleInspectionDetails = await AsyncQueryableExecuter.ToListAsync(query);
            return ApplyGroupingOnVehicleInspectionReportDetails(vehicleInspectionDetails);
        }

        /// <summary>
        /// To get material inspection report details.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.MaterialInspectionReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<VehicleInspectionReportResultDto>> GetMaterialInspectionReportDetailsAsync(VehicleInspectionReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.MaterialInspection);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = CreateMaterialInspectionReportFilteredQuery(input);
            var materialInspectionDetails = await AsyncQueryableExecuter.ToListAsync(query);
            return ApplyGroupingOnMaterialInspectionReportDetails(materialInspectionDetails);
        }

        /// <summary>
        /// To get allocation report details.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.AllocationReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<AllocationReportResultDto>> GetAllocationReportDetailsAsync(AllocationReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.Allocation);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = CreateAllocationReportFilteredQuery(input);
            var allocationReportResults = await AsyncQueryableExecuter.ToListAsync(query);
            return ApplyGroupingOnAllocationReportDetails(allocationReportResults);
        }


        public async Task<Object> GetAuditTrailAsync(string tablename,string param,int pageNumber,int pageSize)
        {
            string connection =  _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
          //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                int totalCount = 0;
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("Dynamic_SPForGXMES", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@Table_Name", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@param", SqlDbType.VarChar).Value = param;
                myCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                myCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                // myReader = myCommand.ExecuteReader();
                //DataTable tab = new DataTable();

                var adapter = new SqlDataAdapter(myCommand);
                DataSet tab = new DataSet();
                adapter.Fill(tab);

                var result = new
                {
                    Result = tab.Tables[0],
                    TotalCount = tab.Tables[1]
                };

                // Return the result object
                return result;

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/
                //return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<IQueryable> GetDynamicAuditTrailAsync(Reports.Dto.AuditDynamicReportDto input)
        {
            /*if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }*/

            var query = _masterCommonRepository.GetDynamicAuditTrail();

           // var pickingReportResults = await AsyncQueryableExecuter.ToListAsync(query);
            return query;

        }


        //public async Task<IQueryable> showTableColumns(Reports.Dto.AuditDynamicReportDto input)
        //{
        //    /*if (input is null)
        //    {
        //        throw new ArgumentNullException(nameof(input));
        //    }*/

        //    var query = _masterCommonRepository.GetDynamicAuditTrail();

        //    // var pickingReportResults = await AsyncQueryableExecuter.ToListAsync(query);
        //    return query;

        //}



        public async Task<Object> showAllTables()
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("showAllTables", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@DatabaseName", SqlDbType.VarChar).Value = "INFORMATION_SCHEMA.TABLES";

                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/
                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<Object> GetspTableStructureToBeCreateAsync(string tablename, string ColumnName, string ActionType)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("spTableStructureToBeCreate", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@ColumnName", SqlDbType.VarChar).Value = ColumnName;
                myCommand.Parameters.Add("@ActionType", SqlDbType.VarChar).Value = ActionType;
                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);


                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        ////public async Task<IQueryable> GetspTableStructureToBeCreateAsync(Reports.Dto.AuditDynamicReportDto input)
        ////{
        ////    /*if (input is null)
        ////    {
        ////        throw new ArgumentNullException(nameof(input));
        ////    }*/

        ////    var query = _masterCommonRepository.GetDynamicAuditTrail();

        ////    // var pickingReportResults = await AsyncQueryableExecuter.ToListAsync(query);
        ////    return query;

        ////}


        public async Task<Object> createForms(string tablename, int pagenumber, int pagesize)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("createForms", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pagenumber;
                myCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = pagesize;


                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/
                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<Object> showTableColumns(string tablename)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("showTableColumns", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;

                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/
                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<Object> FormDataPush(string TableName, string ColumnName, string Values, string ActionType)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("spInsertLogWiseData", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = TableName;
                myCommand.Parameters.Add("@ColumnName", SqlDbType.VarChar).Value = ColumnName;
                myCommand.Parameters.Add("@Values", SqlDbType.VarChar).Value = Values;
                myCommand.Parameters.Add("@ActionType", SqlDbType.VarChar).Value = ActionType;
                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);


                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }




        /// <summary>
        /// To get cubicle asssigned report details.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleAssignmentReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<CubicleAssignedReportResultDto>> GetCubicleAssginedReportDetailsAsync(CubicleAssignedReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.CubicleAssignment);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = CreateCubicleAssignedReportDetailsQuery(input);
            var cubicleAssignedReportResults = await AsyncQueryableExecuter.ToListAsync(query);
            return ApplyGroupingOnCubicleAssigemntReportDetails(cubicleAssignedReportResults);
        }

        /// <summary>
        /// To get picking report details.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.PickingReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<PickingReportDto>> GetPickingReportDetailsAsync(PickingReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.Picking);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = _masterCommonRepository.GetAllPickingReportRawSQL();
            var filterdQuery = CreatePickingReportDetailsQuery(input, query);
            var pickingReportResults = await AsyncQueryableExecuter.ToListAsync(filterdQuery);
            return ApplyGroupingOnPickingReportDetails(pickingReportResults);
        }

        /// <summary>
        /// To get line Clearance report details.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.LineClearanceReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<LineClearanceReportResultDto>> GeLineClearanceReportDetailsAsync(LineClearanceReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.LineClearance);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = CreateLineClearanceReportFilteredQuery(input);
            var lineClearanceReportResults = await AsyncQueryableExecuter.ToListAsync(query);
            return ApplyGroupingOnLineClearanceReportDetails(lineClearanceReportResults);
        }

        //[PMMSAuthorize(Permissions = PMMSPermissionConst.DispensingReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<DispensingReportDto>> DispensingReportGetDetailsAsync(DispensingReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.Dispensing);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var dispensingReportResultQuery = CreateDispensingReportFilteredQuery(input);
            return await AsyncQueryableExecuter.ToListAsync(dispensingReportResultQuery);

        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.DispatchReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<DispatchReportDto>> DispatchReportGetDetailsAsync(DispatchReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.Disptach);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var dispatchReportResultQuery = CreateDispatchReportFilteredQuery(input);
            var val = await AsyncQueryableExecuter.ToListAsync(dispatchReportResultQuery);
            return val;
        }

        /// <summary>
        /// To get cubicle cleaning report details.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.CubicleCleaningReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<CleaningLogReportResultDto>> GetcubicleCleaningReportDetailsAsync(CleaningLogReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.CubicleCleaning);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = CreateCubicleCleaningReportFilteredQuery(input);
            var CubicleCleaningReportResults = await AsyncQueryableExecuter.ToListAsync(query);
            return ApplyGroupingOnCubicleCleaningReportDetails(CubicleCleaningReportResults);
        }

        /// <summary>
        /// To get equipment cleaning report details.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        [PMMSAuthorize(Permissions = PMMSPermissionConst.EquipmentCleaningReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<CleaningLogReportResultDto>> GetEquipmentCleaningReportDetailsAsync(CleaningLogReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.EquipmentCleaning);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var query = CreateEquipmentCleaningReportFilteredQuery(input);
            var EquipmentCleaningReportResults = await AsyncQueryableExecuter.ToListAsync(query);
            return ApplyGroupingOnEquipmentCleaningReportDetails(EquipmentCleaningReportResults);
        }

        /// <summary>
        /// To get report configuration details.
        /// </summary>
        public async Task<ReportConfigurationDto> GetReportConfigurationAsync(string submoduleName)
        {
            var submoduleId = await _moduleAppService.GetSubmoduleByName(submoduleName);
            var existingConfiguration = await _reportConfigurationRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId && x.SubModuleId == submoduleId);
            if (existingConfiguration != null)
            {
                return ObjectMapper.Map<ReportConfigurationDto>(existingConfiguration);
            }
            return new ReportConfigurationDto();
        }

        /// <summary>
        /// To insert or update report configuration details.
        /// </summary>
        /// <param name="input">The input.</param>
        public async Task<ReportConfigurationDto> InsertUpdateReportConfigurationAsync(ReportConfigurationDto input)
        {
            input.UserId = AbpSession.UserId;
            var submoduleId = await _moduleAppService.GetSubmoduleByName(input.SubModuleName);
            var existingReportConfiguration = await _reportConfigurationRepository.FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId && x.SubModuleId == submoduleId);
            if (existingReportConfiguration != null)
            {
                input.Id = existingReportConfiguration.Id;
                ObjectMapper.Map(input, existingReportConfiguration);
                existingReportConfiguration.SubModuleId = submoduleId;
                await _reportConfigurationRepository.UpdateAsync(existingReportConfiguration);
            }
            else
            {
                var reportConfiguration = ObjectMapper.Map<ReportConfiguration>(input);
                reportConfiguration.SubModuleId = submoduleId;
                await _reportConfigurationRepository.InsertAsync(reportConfiguration);
            }
            CurrentUnitOfWork.SaveChanges();
            return input;
        }

        public async Task<List<DispensingTrackingReportDto>> DispensingTrackingReportGetDetailsAsync(DispensingTrackingReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.Dispensing);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var dispensingTrackingReportResultQuery = CreateDispensingTrackingReportFilteredQuery(input);

            var QueryResult = await AsyncQueryableExecuter.ToListAsync(dispensingTrackingReportResultQuery);
            return await GetDispensingTrackingReportDto(QueryResult);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibrationReport_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<WeighingCalibrationDto> GetWMCalibrationReportDetailsAsync(WeighingCalibrationReportRequestDto input)
        {
            var isValidUser = await ValidateUserMode((int)PMMSEnums.ReportType.WeighingCalibration);
            if (!isValidUser)
            {
                throw new UserFriendlyException(PMMSValidationConst.NotValidUser);
            }
            var result = new WeighingCalibrationDto();
            var query = CreateWeighingCalibrationReportDetailsQuery(input);
            var wMCalibrationHeaderId = await query.Select(x => x.Id).FirstOrDefaultAsync();
            if (wMCalibrationHeaderId != 0)
            {
                result = await _weighingCalibrationAppService.GetCalibrationAsync(wMCalibrationHeaderId);
            }
            return result;
        }

        private async Task<List<DispensingTrackingReportDto>> GetDispensingTrackingReportDto(List<DispensingTrackingReportInnerDto> lstInnerOutput)
        {
            List<DispensingTrackingReportDto> lstReportResult = new List<DispensingTrackingReportDto>();
            List<long?> lstUserId = new List<long?>();
            lstUserId.AddRange(lstInnerOutput.Select(x => x.DispensingDoneBy));
            lstUserId.AddRange(lstInnerOutput.Select(x => x.CubicleAssignmentDoneBy));
            lstUserId.AddRange(lstInnerOutput.Select(x => x.LineClearanceDoneBy));
            lstUserId.AddRange(lstInnerOutput.Select(x => x.LineClearanceCheckBy));
            lstUserId.AddRange(lstInnerOutput.Select(x => x.PickingDoneBy));
            lstUserId.AddRange(lstInnerOutput.Select(x => x.PreStagingDoneBy));
            lstUserId.AddRange(lstInnerOutput.Select(x => x.StagingDoneBy));
            lstUserId.AddRange(lstInnerOutput.Select(x => x.StageOutDoneBy));
            lstUserId = lstUserId.Distinct()?.ToList();
            if (lstUserId?.Count > 0)
            {
                var NameDictionary = await GetNameDictionary(lstUserId);
                foreach (var innerOutput in lstInnerOutput)
                {
                    DispensingTrackingReportDto report = new DispensingTrackingReportDto();
                    report.ProcessOrderNo = innerOutput.ProcessOrderNo;
                    report.GroupId = innerOutput.GroupId;
                    report.MaterialCode = innerOutput.MaterialCodeId;
                    report.SAPBatchNo = innerOutput.SAPBatchNumber;
                    report.BatchNo = innerOutput.BatchNo;
                    report.lstActivity = new List<DispensingTrackingActivity>();
                    if (innerOutput.CubicleAssignmentDoneBy != null && innerOutput.CubicleAssignmentTime != null)
                    {
                        DispensingTrackingActivity trackingActivity = new DispensingTrackingActivity();
                        trackingActivity.ActivityName = "Cubicle Assignment";
                        trackingActivity.ActvityDate = innerOutput.CubicleAssignmentTime;
                        trackingActivity.DoneBy = NameDictionary[innerOutput.CubicleAssignmentDoneBy.Value];
                        report.lstActivity.Add(trackingActivity);
                    }

                    if (innerOutput.LineClearanceDoneBy != null && innerOutput.LineClearanceDoneBy != null && innerOutput.LineClearanceCheckBy != null)
                    {
                        DispensingTrackingActivity trackingActivity = new DispensingTrackingActivity();
                        trackingActivity.ActivityName = "Line Clearance";
                        trackingActivity.ActvityDate = innerOutput.LineClearanceTime;
                        trackingActivity.DoneBy = NameDictionary[innerOutput.LineClearanceDoneBy.Value];
                        trackingActivity.ActivityCheckBy = NameDictionary[innerOutput.LineClearanceCheckBy.Value];
                        report.lstActivity.Add(trackingActivity);
                    }

                    if (innerOutput.PickingDoneBy != null && innerOutput.PickingTime != null)
                    {
                        DispensingTrackingActivity trackingActivity = new DispensingTrackingActivity();
                        trackingActivity.ActivityName = "Picking";
                        trackingActivity.ActvityDate = innerOutput.PickingTime;
                        trackingActivity.DoneBy = NameDictionary[innerOutput.PickingDoneBy.Value];
                        report.lstActivity.Add(trackingActivity);
                    }
                    if (innerOutput.PreStagingDoneBy != null && innerOutput.PreStagingTime != null)
                    {
                        DispensingTrackingActivity trackingActivity = new DispensingTrackingActivity();
                        trackingActivity.ActivityName = "Pre-Staging";
                        trackingActivity.ActvityDate = innerOutput.PreStagingTime;
                        trackingActivity.DoneBy = NameDictionary[innerOutput.PreStagingDoneBy.Value];
                        report.lstActivity.Add(trackingActivity);
                    }
                    if (innerOutput.StagingDoneBy != null && innerOutput.StagingTime != null)
                    {
                        DispensingTrackingActivity trackingActivity = new DispensingTrackingActivity();
                        trackingActivity.ActivityName = "Staging";
                        trackingActivity.ActvityDate = innerOutput.StagingTime;
                        trackingActivity.DoneBy = NameDictionary[innerOutput.StagingDoneBy.Value];
                        report.lstActivity.Add(trackingActivity);
                    }

                    if (innerOutput.DispensingDoneBy != null && innerOutput.DispensingTime != null)
                    {
                        DispensingTrackingActivity trackingActivity = new DispensingTrackingActivity();
                        trackingActivity.ActivityName = "Dispensing";
                        trackingActivity.ActvityDate = innerOutput.DispensingTime;
                        trackingActivity.DoneBy = NameDictionary[innerOutput.DispensingDoneBy.Value];
                        report.lstActivity.Add(trackingActivity);
                    }
                    if (innerOutput.StageOutDoneBy != null && innerOutput.StageOutTime != null)
                    {
                        DispensingTrackingActivity trackingActivity = new DispensingTrackingActivity();
                        trackingActivity.ActivityName = "Stage-Out";
                        trackingActivity.ActvityDate = innerOutput.StageOutTime;
                        trackingActivity.DoneBy = NameDictionary[innerOutput.StageOutDoneBy.Value];
                        report.lstActivity.Add(trackingActivity);
                    }

                    lstReportResult.Add(report);
                }
            }
            return lstReportResult;
        }

        private async Task<Dictionary<long, string>> GetNameDictionary(List<long?> lstUserId)
        {
            return await _userRepository.GetAll().Where(x => lstUserId.Contains(x.Id)).Select(x => new { x.Id, x.FullName }).ToDictionaryAsync(x => x.Id, x => x.FullName);
        }

        private IQueryable<DispensingReportDto> CreateDispensingReportFilteredQuery(DispensingReportRequestDto input)
        {
            IQueryable<DispensingReportDto> filteredQuery = from dispensingHeader in _dispensingHeaderRepository.GetAll()
                                                            join dispensingDetail in _dispensingDetailRepository.GetAll()
                                                            on dispensingHeader.Id equals dispensingDetail.DispensingHeaderId
                                                            join processOrder in _processOrderRepository.GetAll()
                                                            on dispensingHeader.ProcessOrderId equals processOrder.Id
                                                            join plantMaster in _plantRepository.GetAll()
                                                            on processOrder.PlantId equals plantMaster.Id
                                                            join processOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                            on new { Id = (int?)processOrder.Id, MaterialCodeId = dispensingHeader.MaterialCodeId }
                                                            equals new { Id = processOrderMaterial.ProcessOrderId, MaterialCodeId = processOrderMaterial.ItemCode }
                                                            join pmmsUser in _userRepository.GetAll()
                                                            on dispensingHeader.CreatorUserId equals pmmsUser.Id
                                                            join weighingMachine in _weighingMachineRepository.GetAll()
                                                            on dispensingDetail.WeighingMachineId equals weighingMachine.Id into dw
                                                            from weighingMachine in dw.DefaultIfEmpty()
                                                            where dispensingHeader.ProcessOrderId != null
                                                            orderby dispensingHeader.CreationTime descending
                                                            select new DispensingReportDto
                                                            {
                                                                PlantId = plantMaster.Id,
                                                                ProcessOrderId = processOrder.Id,
                                                                UserEnteredPlantId = plantMaster.PlantId,
                                                                ProductCode = processOrder.ProductCode,
                                                                BatchNo = processOrderMaterial.BatchNo,
                                                                ItemCode = processOrderMaterial.ItemCode,
                                                                SAPBatchNumber = dispensingDetail.SAPBatchNumber,
                                                                CreatedOn = dispensingHeader.CreationTime,
                                                                WeighingMachineCode = weighingMachine.WeighingMachineCode,
                                                                WeighingMachineId = dispensingDetail.WeighingMachineId,
                                                                UserName = pmmsUser.FullName,
                                                                NoOfPacks = dispensingDetail.NoOfPacks,
                                                                GrossWeight = dispensingDetail.GrossWeight,
                                                                NetWeight = dispensingDetail.NetWeight,
                                                                TareWeight = dispensingDetail.TareWeight,
                                                                IsSampling = dispensingHeader.IsSampling,
                                                                ProductBatch = processOrderMaterial.BatchNo
                                                            };

            if (input.PlantId != null)
            {
                filteredQuery = filteredQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (input.LstProcessOrderId != null && input.LstProcessOrderId.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstProcessOrderId.Contains(x.ProcessOrderId));
            }
            if (input.LstSAPBatchNo != null && input.LstSAPBatchNo.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstSAPBatchNo.Contains(x.SAPBatchNumber));
            }

            if (input.LstWeighingMachineId != null && input.LstWeighingMachineId.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstWeighingMachineId.Contains(x.WeighingMachineId));
            }
            if (input.LstMaterialId != null && input.LstMaterialId.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstMaterialId.Contains(x.ItemCode));
            }
            if (input.LstProductCode != null && input.LstProductCode.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstProductCode.Contains(x.ProductCode));
            }
            if (input.LstProductBatch != null && input.LstProductBatch.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstProductBatch.Contains(x.BatchNo));
            }

            if (input.FromDate != null && input.ToDate != null)
            {
                var startDate = input.FromDate.Value.StartOfDay();
                var endDate = input.ToDate.Value.EndOfDay();
                filteredQuery = filteredQuery.Where(x => x.CreatedOn >= startDate && x.CreatedOn <= endDate);
            }
            return filteredQuery;
        }

        private IQueryable<DispatchReportDto> CreateDispatchReportFilteredQuery(DispatchReportRequestDto input)
        {
            IQueryable<DispatchReportDto> filteredQuery = from dispatchDetail in _loadingRepository.GetAll()
                                                          orderby dispatchDetail.CreationTime descending
                                                          select new DispatchReportDto
                                                          {
                                                              PlantId = dispatchDetail.PlantId,
                                                              Batch = dispatchDetail.Batch,
                                                              CustomerAddress = dispatchDetail.CustomerAddress,
                                                              CustomerName = dispatchDetail.CustomerName,
                                                              Description = dispatchDetail.Description,
                                                              HUCode = dispatchDetail.HUCode,
                                                              isActive = dispatchDetail.isActive,
                                                              isPicked = dispatchDetail.isPicked,
                                                              LineItem = dispatchDetail.LineItem,
                                                              NoOfPacks = dispatchDetail.NoOfPacks,
                                                              OBD = dispatchDetail.OBD,
                                                              PalletBarcode = dispatchDetail.PalletBarcode,
                                                              PalletCount = dispatchDetail.PalletCount,
                                                              PickingId = dispatchDetail.PickingId,
                                                              ProductBatchNo = dispatchDetail.ProductBatchNo,
                                                              ProductCode = dispatchDetail.ProductCode,
                                                              ProductId = dispatchDetail.ProductId,
                                                              ProductName = dispatchDetail.ProductName,
                                                              PutawayId = dispatchDetail.PutawayId,
                                                              Quantity = dispatchDetail.Quantity,
                                                              TransportName = dispatchDetail.TransportName,
                                                              UOM = dispatchDetail.UOM,
                                                              VehicleNo = dispatchDetail.VehicleNo,
                                                              CreatedBy = dispatchDetail.CreatorUserId,
                                                              CreatedOn = dispatchDetail.CreationTime,
                                                              
                                                             

                                                          };

            if (input.PlantId != null)
            {
                filteredQuery = filteredQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (input.OBD != null)
            {
                filteredQuery = filteredQuery.Where(x => x.OBD == input.OBD);
            }
            if (input.ProductBatchNo != null)
            {
                filteredQuery = filteredQuery.Where(x => x.ProductBatchNo == input.ProductBatchNo);
            }
            if (input.ProductCode != null)
            {
                filteredQuery = filteredQuery.Where(x => x.ProductCode == input.ProductCode);
            }
            if (input.Batch != null)
            {
                filteredQuery = filteredQuery.Where(x => x.Batch == input.Batch);
            }
            if (input.CustomerName != null)
            {
                filteredQuery = filteredQuery.Where(x => x.CustomerName == input.CustomerName);
            }
            if (input.FromDate != null && input.ToDate != null)
            {
                var startDate = input.FromDate.Value.StartOfDay();
                var endDate = input.ToDate.Value.EndOfDay();
                filteredQuery = filteredQuery.Where(x => x.CreatedOn >= startDate && x.CreatedOn <= endDate);
            }
            return filteredQuery;
        }

        private IQueryable<DispensingTrackingReportInnerDto> CreateDispensingTrackingReportFilteredQuery(DispensingTrackingReportRequestDto input)
        {
            IQueryable<DispensingTrackingReportInnerDto> filteredQuery = from dHeader in _dispensingHeaderRepository.GetAll()
                                                                         join dDetails in _dispensingDetailRepository.GetAll()
                                                                         on dHeader.Id equals dDetails.DispensingHeaderId
                                                                         join pOrder in _processOrderRepository.GetAll()
                                                                         on dHeader.ProcessOrderId equals pOrder.Id
                                                                         join pOrderMaterial in _processOrderMaterialRepository.GetAll()
                                                                         on new { ProcessOrderId = (int?)pOrder.Id, ItemCode = dHeader.MaterialCodeId }
                                                                         equals new { ProcessOrderId = pOrderMaterial.ProcessOrderId, ItemCode = pOrderMaterial.ItemCode }
                                                                         join cAssignmentDetail in _cubicleAssignDetailRepository.GetAll()
                                                                         on pOrderMaterial.Id equals cAssignmentDetail.ProcessOrderMaterialId
                                                                         join CAssignmentHeader in _cubicleAssignHeaderRepository.GetAll()
                                                                         on cAssignmentDetail.CubicleAssignmentHeaderId equals CAssignmentHeader.Id
                                                                         join lineClearance in _lineClearanceTransactionRepository.GetAll()
                                                                         on CAssignmentHeader.Id equals lineClearance.GroupId into las
                                                                         from lineClearance in las.DefaultIfEmpty()
                                                                         join pickingDispensing in _materialBatchDispensingHeaderRepository.GetAll()
                                                                         on new { GroupId = CAssignmentHeader.GroupId, ItemCode = pOrderMaterial.ItemCode, MaterialBatchDispensingHeaderType = 1 }
                                                                         equals new { GroupId = pickingDispensing.GroupCode, ItemCode = pickingDispensing.MaterialCode, MaterialBatchDispensingHeaderType = pickingDispensing.MaterialBatchDispensingHeaderType } into pickingBatch
                                                                         from pickingDispensing in pickingBatch.DefaultIfEmpty()
                                                                         join preStagingDispensing in _materialBatchDispensingHeaderRepository.GetAll()
                                                                         on new { GroupId = CAssignmentHeader.GroupId, ItemCode = pOrderMaterial.ItemCode, MaterialBatchDispensingHeaderType = 2 }
                                                                         equals new { GroupId = preStagingDispensing.GroupCode, ItemCode = preStagingDispensing.MaterialCode, MaterialBatchDispensingHeaderType = preStagingDispensing.MaterialBatchDispensingHeaderType } into preStagingBatch
                                                                         from preStagingDispensing in preStagingBatch.DefaultIfEmpty()
                                                                         join stagingDispensing in _materialBatchDispensingHeaderRepository.GetAll()
                                                                         on new { GroupId = CAssignmentHeader.GroupId, ItemCode = pOrderMaterial.ItemCode, MaterialBatchDispensingHeaderType = 2 }
                                                                         equals new { GroupId = stagingDispensing.GroupCode, ItemCode = stagingDispensing.MaterialCode, MaterialBatchDispensingHeaderType = stagingDispensing.MaterialBatchDispensingHeaderType } into stagingBatch
                                                                         from stagingDispensing in stagingBatch.DefaultIfEmpty()
                                                                         join stageOutHeader in _stageOutHeaderRepository.GetAll()
                                                                         on new { GroupId = CAssignmentHeader.GroupId, ItemCode = dHeader.MaterialCodeId }
                                                                         equals new { GroupId = stageOutHeader.GroupId, ItemCode = stageOutHeader.MaterialCode } into stageOutAssignment
                                                                         from stageOutHeader in stageOutAssignment.DefaultIfEmpty()
                                                                         orderby dHeader.CreationTime descending
                                                                         select new DispensingTrackingReportInnerDto
                                                                         {
                                                                             ProcessOrderNo = pOrder.ProcessOrderNo,
                                                                             BatchNo = pOrderMaterial.BatchNo,
                                                                             MaterialCodeId = dHeader.MaterialCodeId,
                                                                             SAPBatchNumber = dDetails.SAPBatchNumber,
                                                                             DispensingTime = dHeader.EndTime,
                                                                             DispensingDoneBy = dHeader.CreatorUserId,
                                                                             CubicleAssignmentTime = CAssignmentHeader.CubicleAssignmentDate,
                                                                             CubicleAssignmentDoneBy = CAssignmentHeader.CreatorUserId,
                                                                             LineClearanceTime = lineClearance.ClearanceDate,
                                                                             LineClearanceDoneBy = lineClearance.CreatorUserId,
                                                                             LineClearanceCheckBy = lineClearance.VerifiedBy,
                                                                             PickingTime = pickingDispensing.PickingTime,
                                                                             PickingDoneBy = pickingDispensing.CreatorUserId,
                                                                             PreStagingTime = preStagingDispensing.PickingTime,
                                                                             PreStagingDoneBy = preStagingDispensing.CreatorUserId,
                                                                             StagingTime = stagingDispensing.PickingTime,
                                                                             StagingDoneBy = stagingDispensing.CreatorUserId,
                                                                             StageOutTime = stageOutHeader.CreationTime,
                                                                             StageOutDoneBy = stageOutHeader.CreatorUserId,
                                                                             GroupId = CAssignmentHeader.GroupId,
                                                                             ProductCode = pOrder.ProductCode,
                                                                             ProcessOrderId = pOrder.Id,
                                                                             PlantId = pOrder.PlantId
                                                                         };
            if (input.PlantId != null)
            {
                filteredQuery = filteredQuery.Where(x => x.PlantId == input.PlantId);
            }
            if (input.LstProcessOrderId != null && input.LstProcessOrderId.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstProcessOrderId.Contains(x.ProcessOrderId));
            }
            if (input.LstSAPBatchNo != null && input.LstSAPBatchNo.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstSAPBatchNo.Contains(x.SAPBatchNumber));
            }

            if (input.LstMaterialId != null && input.LstMaterialId.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstMaterialId.Contains(x.MaterialCodeId));
            }
            if (input.LstProductCode != null && input.LstProductCode.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstProductCode.Contains(x.ProductCode));
            }
            if (input.LstProductBatch != null && input.LstProductBatch.Count > 0)
            {
                filteredQuery = filteredQuery.Where(x => input.LstProductBatch.Contains(x.BatchNo));
            }

            return filteredQuery.Distinct();
        }

        protected IQueryable<VehicleInspectionReportResultDto> CreateVehicleInspectionReportFilteredQuery(VehicleInspectionReportRequestDto input)
        {
            var VehicleInspectionReportQuery = from vehicleInspection in _vehicleInspectionHeaderRepository.GetAll()
                                               join vehicleInspectionDetail in _vehicleInspectionDetailRepository.GetAll()
                                               on vehicleInspection.Id equals vehicleInspectionDetail.VehicleInspectionHeaderId
                                               join transactionStatus in _transactionStatusRepository.GetAll()
                                               on vehicleInspection.TransactionStatusId equals transactionStatus.Id
                                               join gateEntry in _gateEntryRepository.GetAll()
                                               on vehicleInspection.GateEntryId equals gateEntry.Id into insgate
                                               from gateEntry in insgate.DefaultIfEmpty()
                                               join invoice in _invoiceDetailRepository.GetAll()
                                               on vehicleInspection.InvoiceId equals invoice.Id
                                               join purchaseOrder in _purchaseOrderRepository.GetAll()
                                               on invoice.PurchaseOrderId equals purchaseOrder.Id
                                               join checkpoint in _checkpointRepository.GetAll()
                                               on vehicleInspectionDetail.CheckpointId equals checkpoint.Id
                                               join checklist in _checklistRepository.GetAll()
                                               on checkpoint.InspectionChecklistId equals checklist.Id
                                               join checkpointType in _checkpointTypeRepository.GetAll()
                                               on checkpoint.CheckpointTypeId equals checkpointType.Id
                                               join user in _userRepository.GetAll()
                                               on vehicleInspection.CreatorUserId equals user.Id
                                               select new VehicleInspectionReportResultDto
                                               {
                                                   Id = vehicleInspection.Id,
                                                   InspectionDate = vehicleInspection.CreationTime,
                                                   GatePassNo = gateEntry.GatePassNo,
                                                   PurchaseOrderNo = invoice.PurchaseOrderNo,
                                                   InvoiceNo = invoice.InvoiceNo,
                                                   ChecklistName = checklist.Name,
                                                   VehicleInspectionDetailId = vehicleInspectionDetail.Id,
                                                   CheckpointName = checkpoint.CheckpointName,
                                                   CheckpointType = checkpointType.Title,
                                                   ValueTag = checkpoint.ValueTag,
                                                   AcceptanceValue = checkpoint.AcceptanceValue,
                                                   UserdEnteredValue = vehicleInspectionDetail.Observation,
                                                   Remark = vehicleInspectionDetail.DiscrepancyRemark,
                                                   PurchaseOrderId = invoice.PurchaseOrderId ?? 0,
                                                   GateEntryId = vehicleInspection.GateEntryId ?? 0,
                                                   SubPlantId = purchaseOrder.PlantId,
                                                   TransactionStatusId = vehicleInspection.TransactionStatusId ?? 0,
                                                   TransactionStatus = transactionStatus.TransactionStatus,
                                                   InspectedBy = user.Name + ' ' + user.Surname
                                               };
            if (input.SubPlantId != null)
            {
                VehicleInspectionReportQuery = VehicleInspectionReportQuery.Where(x => Convert.ToInt32(input.SubPlantId) == x.SubPlantId);
            }
            if (input.PurchaseOrderId != null)
            {
                VehicleInspectionReportQuery = VehicleInspectionReportQuery.Where(x => input.PurchaseOrderId.Contains(x.PurchaseOrderId));
            }
            if (input.TransactionStatusId != null)
            {
                VehicleInspectionReportQuery = VehicleInspectionReportQuery.Where(x => input.TransactionStatusId.Contains(x.TransactionStatusId));
            }

            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                VehicleInspectionReportQuery = VehicleInspectionReportQuery.Where(x => x.InspectionDate >= StartDate && x.InspectionDate <= EndDate);
            }
            return VehicleInspectionReportQuery;
        }

        protected IQueryable<VehicleInspectionReportResultDto> CreateMaterialInspectionReportFilteredQuery(VehicleInspectionReportRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var materialInspetionReportQuery = from materialInspection in _materialInspectionHeaderRepository.GetAll()
                                               join materialRelation in _materialRelationDetailsRepository.GetAll()
                                               on materialInspection.Id equals materialRelation.MaterialHeaderId
                                               join materialChecklistDetail in _materialChecklistDetailsRepository.GetAll()
                                               on materialRelation.Id equals materialChecklistDetail.MaterialRelationId
                                               join transactionStatus in _transactionStatusRepository.GetAll()
                                               on materialInspection.TransactionStatusId equals transactionStatus.Id
                                               join gateEntry in _gateEntryRepository.GetAll()
                                               on materialInspection.GateEntryId equals gateEntry.Id into insgate
                                               from gateEntry in insgate.DefaultIfEmpty()
                                               join invoice in _invoiceDetailRepository.GetAll()
                                               on materialInspection.InvoiceId equals invoice.Id
                                               join purchaseOrder in _purchaseOrderRepository.GetAll()
                                               on invoice.PurchaseOrderId equals purchaseOrder.Id
                                               join material in _materialRepository.GetAll()
                                               on materialRelation.MaterialId equals material.Id
                                               join checkpoint in _checkpointRepository.GetAll()
                                               on materialChecklistDetail.CheckPointId equals checkpoint.Id
                                               join checklist in _checklistRepository.GetAll()
                                               on checkpoint.InspectionChecklistId equals checklist.Id
                                               join checkpointType in _checkpointTypeRepository.GetAll()
                                               on checkpoint.CheckpointTypeId equals checkpointType.Id
                                               join user in _userRepository.GetAll()
                                               on materialInspection.CreatorUserId equals user.Id
                                               select new VehicleInspectionReportResultDto
                                               {
                                                   Id = materialInspection.Id,
                                                   InspectionDate = materialInspection.CreationTime,
                                                   GatePassNo = gateEntry.GatePassNo,
                                                   PurchaseOrderNo = invoice.PurchaseOrderNo,
                                                   InvoiceNo = invoice.InvoiceNo,
                                                   ChecklistName = checklist.Name,
                                                   CheckpointName = checkpoint.CheckpointName,
                                                   CheckpointType = checkpointType.Title,
                                                   ValueTag = checkpoint.ValueTag,
                                                   AcceptanceValue = checkpoint.AcceptanceValue,
                                                   UserdEnteredValue = materialChecklistDetail.Observation,
                                                   Remark = materialChecklistDetail.DiscrepancyRemark,
                                                   PurchaseOrderId = invoice.PurchaseOrderId ?? 0,
                                                   GateEntryId = materialInspection.GateEntryId ?? 0,
                                                   SubPlantId = purchaseOrder.PlantId,
                                                   TransactionStatusId = materialInspection.TransactionStatusId ?? 0,
                                                   TransactionStatus = transactionStatus.TransactionStatus,
                                                   MaterialCode = material.ItemCode,
                                                   MaterialId = material.Id,
                                                   InspectedBy = user.Name + ' ' + user.Surname
                                               };
            materialInspetionReportQuery = materialInspetionReportQuery.Where(x => x.MaterialId > 0);
            if (input.SubPlantId != null)
            {
                materialInspetionReportQuery = materialInspetionReportQuery.Where(x => Convert.ToInt32(input.SubPlantId) == x.SubPlantId);
            }
            if (input.PurchaseOrderId != null)
            {
                materialInspetionReportQuery = materialInspetionReportQuery.Where(x => input.PurchaseOrderId.Contains(x.PurchaseOrderId));
            }
            if (input.TransactionStatusId != null)
            {
                materialInspetionReportQuery = materialInspetionReportQuery.Where(x => input.TransactionStatusId.Contains(x.TransactionStatusId));
            }
            if (input.MaterialListId != null)
            {
                materialInspetionReportQuery = materialInspetionReportQuery.Where(x => input.MaterialListId.Contains(x.MaterialId));
            }
            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                materialInspetionReportQuery = materialInspetionReportQuery.Where(x => x.InspectionDate >= StartDate && x.InspectionDate <= EndDate);
            }
            return materialInspetionReportQuery;
        }

        protected IQueryable<AllocationReportResultDto> CreateAllocationReportFilteredQuery(AllocationReportRequestDto input)
        {
            string availableStockStatus = nameof(StockStatus.Available);
            string notAvailableStockStatus = nameof(StockStatus.NotAvailable);
            var putAwayQuery = from putAway in _putAwayBinToBinTrasferRepository.GetAll()
                               join matContainer in _grnMaterialLabelPrintingContainerBarcodeRepository.GetAll()
                               on putAway.ContainerId equals matContainer.Id
                               join material in _materialRepository.GetAll()
                               on putAway.MaterialId equals material.Id
                               join location in _locationRepository.GetAll()
                               on putAway.LocationId equals location.Id
                               join area in _areaRepository.GetAll()
                               on location.AreaId equals area.Id
                               join plant in _plantRepository.GetAll()
                               on location.PlantId equals plant.Id
                               join user in _userRepository.GetAll()
                               on putAway.CreatorUserId equals user.Id
                               where !putAway.IsUnloaded
                               select new AllocationReportResultDto
                               {
                                   Id = putAway.Id,
                                   SubPlantId = location.PlantId,
                                   AreaId = location.AreaId,
                                   MaterialId = putAway.MaterialId,
                                   PlantCode = plant.PlantId,
                                   AreaCode = area.AreaCode,
                                   LocationCode = location.LocationCode,
                                   MaterialCode = material.ItemCode,
                                   SapBatchNo = putAway.SAPBatchNumber,
                                   Qty = matContainer.Quantity,
                                   StockStatus = matContainer.BalanceQuantity == 0 ? notAvailableStockStatus : availableStockStatus,
                                   AllocationDate = putAway.CreationTime,
                                   AllocatedBy = user.Name + ' ' + user.Surname
                               };

            var palletizationQuery = from putAway in _putAwayBinToBinTrasferRepository.GetAll()
                                     join pallet in _palletizationRepository.GetAll()
                                     on putAway.PalletId equals pallet.PalletId
                                     join matContainer in _grnMaterialLabelPrintingContainerBarcodeRepository.GetAll()
                                     on pallet.ContainerId equals matContainer.Id
                                     join material in _materialRepository.GetAll()
                                     on pallet.MaterialId equals material.Id
                                     join handlingUnit in _handlingUnitRepository.GetAll()
                                     on pallet.PalletId equals handlingUnit.Id
                                     join location in _locationRepository.GetAll()
                                     on putAway.LocationId equals location.Id
                                     join plant in _plantRepository.GetAll()
                                     on handlingUnit.PlantId equals plant.Id
                                     join area in _areaRepository.GetAll()
                                     on location.AreaId equals area.Id
                                     join user in _userRepository.GetAll()
                                     on putAway.CreatorUserId equals user.Id
                                     where !putAway.IsUnloaded && !pallet.IsUnloaded
                                     select new AllocationReportResultDto
                                     {
                                         Id = putAway.Id,
                                         PlantCode = plant.PlantId,
                                         SubPlantId = handlingUnit.PlantId,
                                         AreaId = location.AreaId,
                                         MaterialId = pallet.MaterialId,
                                         AreaCode = area.AreaCode,
                                         LocationCode = location.LocationCode,
                                         MaterialCode = material.ItemCode,
                                         SapBatchNo = pallet.SAPBatchNumber,
                                         Qty = matContainer.Quantity,
                                         StockStatus = matContainer.BalanceQuantity == 0 ? PMMSConsts.NotAvailable : PMMSConsts.Available,
                                         AllocationDate = putAway.CreationTime,
                                         AllocatedBy = user.Name + ' ' + user.Surname
                                     };

            var allocationQuery = putAwayQuery.Union(palletizationQuery);
            if (input.SubPlantId != null)
            {
                allocationQuery = allocationQuery.Where(x => input.SubPlantId == x.SubPlantId);
            }
            if (input.AreaIds != null)
            {
                allocationQuery = allocationQuery.Where(x => input.AreaIds.Contains(x.AreaId));
            }
            if (input.MaterialCodes != null)
            {
                allocationQuery = allocationQuery.Where(x => input.MaterialCodes.Contains(x.MaterialCode));
            }
            if (input.SapBatchNos != null)
            {
                allocationQuery = allocationQuery.Where(x => input.SapBatchNos.Contains(x.SapBatchNo));
            }
            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                allocationQuery = allocationQuery.Where(x => x.AllocationDate >= StartDate && x.AllocationDate <= EndDate);
            }
            return allocationQuery;
        }

        protected IQueryable<CubicleAssignedReportResultDto> CreateCubicleAssignedReportDetailsQuery(CubicleAssignedReportRequestDto input)
        {
            var cubicleAssignments = from cubicleHeader in _cubicleAssignHeaderRepository.GetAll()
                                     join cubicleDetail in _cubicleAssignDetailRepository.GetAll()
                                     on cubicleHeader.Id equals cubicleDetail.CubicleAssignmentHeaderId
                                     join material in _processOrderMaterialRepository.GetAll()
                                     on cubicleDetail.ProcessOrderMaterialId equals material.Id
                                     join status in _statusRepository.GetAll()
                                     on cubicleHeader.GroupStatusId equals status.Id
                                     join user in _userRepository.GetAll()
                                     on cubicleHeader.CreatorUserId equals user.Id
                                     select new
                                     {
                                         Id = cubicleHeader.Id,
                                         PlantCode = string.Empty,
                                         SubPlantId = 0,
                                         CubicleId = cubicleDetail.CubicleId,
                                         CubicleCode = string.Empty,
                                         GroupCode = cubicleHeader.GroupId,
                                         MaterialCode = material.ItemNo + " - " + material.ItemCode,
                                         ProductCode = cubicleHeader.ProductCode,
                                         BatchNo = material.BatchNo,
                                         SapBatchNo = material.SAPBatchNo,
                                         Qty = material.OrderQuantity,
                                         InspectionLotId = cubicleDetail.InspectionLotId,
                                         ProcessOrderId = cubicleDetail.ProcessOrderId,
                                         CubicleAssignmentDate = cubicleHeader.CreationTime,
                                         IsCubicleAssigned = cubicleDetail.CubicleId != null,
                                         GroupStatus = status.Status,
                                         GroupStatusId = cubicleHeader.GroupStatusId,
                                         IsSampling = cubicleHeader.IsSampling,
                                         AssignmentBy = user.Name + ' ' + user.Surname
                                     };
            var dispensingCubicleAssignments = from assignments in cubicleAssignments
                                               join po in _processOrderRepository.GetAll()
                                               on assignments.ProcessOrderId equals po.Id
                                               join plant in _plantRepository.GetAll()
                                               on po.PlantId equals plant.Id into ps
                                               from plant in ps.DefaultIfEmpty()
                                               join cubicleMaster in _cubicleMasterRepository.GetAll()
                                               on assignments.CubicleId equals cubicleMaster.Id into cms
                                               from cubicleMaster in cms.DefaultIfEmpty()
                                               select new CubicleAssignedReportResultDto
                                               {
                                                   Id = assignments.Id,
                                                   PlantCode = plant.PlantName,
                                                   SubPlantId = po.PlantId,
                                                   CubicleId = assignments.CubicleId,
                                                   CubicleCode = cubicleMaster.CubicleCode,
                                                   GroupCode = assignments.GroupCode,
                                                   MaterialCode = assignments.MaterialCode,
                                                   ProductCode = assignments.ProductCode,
                                                   BatchNo = assignments.BatchNo,
                                                   SapBatchNo = assignments.SapBatchNo,
                                                   Qty = assignments.Qty,
                                                   CubicleAssignmentDate = assignments.CubicleAssignmentDate,
                                                   IsCubicleAssigned = assignments.IsCubicleAssigned,
                                                   GroupStatus = assignments.GroupStatus,
                                                   GroupStatusId = assignments.GroupStatusId,
                                                   IsSampling = assignments.IsSampling,
                                                   AssignmentBy = assignments.AssignmentBy
                                               };

            var samplingCubicleAssignments = from assignments in cubicleAssignments
                                             join inspectionLot in _inspectionLotRepository.GetAll()
                                             on assignments.InspectionLotId equals inspectionLot.Id
                                             join plant in _plantRepository.GetAll()
                                             on inspectionLot.PlantId equals plant.Id
                                             join cubicleMaster in _cubicleMasterRepository.GetAll()
                                             on assignments.CubicleId equals cubicleMaster.Id into cms
                                             from cubicleMaster in cms.DefaultIfEmpty()
                                             select new CubicleAssignedReportResultDto
                                             {
                                                 Id = assignments.Id,
                                                 PlantCode = plant.PlantName,
                                                 SubPlantId = inspectionLot.PlantId,
                                                 CubicleId = assignments.CubicleId,
                                                 CubicleCode = cubicleMaster.CubicleCode,
                                                 GroupCode = assignments.GroupCode,
                                                 MaterialCode = assignments.MaterialCode,
                                                 ProductCode = assignments.ProductCode,
                                                 BatchNo = assignments.BatchNo,
                                                 SapBatchNo = assignments.SapBatchNo,
                                                 Qty = assignments.Qty,
                                                 CubicleAssignmentDate = assignments.CubicleAssignmentDate,
                                                 IsCubicleAssigned = assignments.IsCubicleAssigned,
                                                 GroupStatus = assignments.GroupStatus,
                                                 GroupStatusId = assignments.GroupStatusId,
                                                 IsSampling = assignments.IsSampling,
                                                 AssignmentBy = assignments.AssignmentBy
                                             };
            var allCubicleAssignementQuery = dispensingCubicleAssignments.Union(samplingCubicleAssignments);

            if (input.SubPlantIds != null)
            {
                allCubicleAssignementQuery = allCubicleAssignementQuery.Where(x => x.SubPlantId == input.SubPlantIds);
            }
            if (input.CubicleIds != null)
            {
                allCubicleAssignementQuery = allCubicleAssignementQuery.Where(x => input.CubicleIds.Contains(x.CubicleId));
            }
            if (input.ProductCodes != null)
            {
                allCubicleAssignementQuery = allCubicleAssignementQuery.Where(x => input.ProductCodes.Contains(x.ProductCode));
            }
            if (input.BatchNos != null)
            {
                allCubicleAssignementQuery = allCubicleAssignementQuery.Where(x => input.BatchNos.Contains(x.SapBatchNo));
            }
            if (input.GroupStatusIds != null)
            {
                allCubicleAssignementQuery = allCubicleAssignementQuery.Where(x => input.GroupStatusIds.Contains((int)x.GroupStatusId));
            }
            if (input.IsSampling != null)
            {
                allCubicleAssignementQuery = allCubicleAssignementQuery.Where(x => x.IsSampling == (bool)input.IsSampling);
            }
            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                allCubicleAssignementQuery = allCubicleAssignementQuery.Where(x => x.CubicleAssignmentDate >= StartDate && x.CubicleAssignmentDate <= EndDate);
            }
            return allCubicleAssignementQuery;
        }

        protected IQueryable<PickingReportDto> CreatePickingReportDetailsQuery(PickingReportRequestDto input, IQueryable<PickingReportDto> allPickingDetailQuery)
        {
            if (input.SubPlantId != null)
            {
                allPickingDetailQuery = allPickingDetailQuery.Where(x => x.SubPlantId == input.SubPlantId);
            }
            if (input.MaterialCodes != null)
            {
                allPickingDetailQuery = allPickingDetailQuery.Where(x => input.MaterialCodes.Contains(x.MaterialCode));
            }
            if (input.SapBatchNos != null)
            {
                allPickingDetailQuery = allPickingDetailQuery.Where(x => input.SapBatchNos.Contains(x.SapBatchNo));
            }
            if (input.ProcessOrderIds != null)
            {
                allPickingDetailQuery = allPickingDetailQuery.Where(x => input.ProcessOrderIds.Contains(x.ProcessOrderId));
            }
            if (input.InspectionLotIds != null)
            {
                allPickingDetailQuery = allPickingDetailQuery.Where(x => input.InspectionLotIds.Contains(x.InspectionLotId));
            }
            if (input.SapBatchNos != null)
            {
                allPickingDetailQuery = allPickingDetailQuery.Where(x => input.SapBatchNos.Contains(x.SapBatchNo));
            }
            if (input.IsSampling != null)
            {
                allPickingDetailQuery = allPickingDetailQuery.Where(x => x.IsSampling == (bool)input.IsSampling);
            }
            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                allPickingDetailQuery = allPickingDetailQuery.Where(x => x.PickingTime >= StartDate && x.PickingTime <= EndDate);
            }
            return allPickingDetailQuery;
        }

        protected IQueryable<LineClearanceReportResultDto> CreateLineClearanceReportFilteredQuery(LineClearanceReportRequestDto input)
        {
            var lineClearanceReportQuery = from lineClearance in _lineClearanceTransactionRepository.GetAll()
                                           join lineChecklistDetail in _lineClearanceCheckPointRepository.GetAll()
                                           on lineClearance.Id equals lineChecklistDetail.LineClearanceTransactionId
                                           join transactionStatus in _statusRepository.GetAll()
                                           on lineClearance.StatusId equals transactionStatus.Id
                                           join cubicleHeader in _cubicleAssignHeaderRepository.GetAll()
                                           on lineClearance.GroupId equals cubicleHeader.Id
                                           join cubicleDetails in _cubicleAssignDetailRepository.GetAll()
                                           on cubicleHeader.Id equals cubicleDetails.CubicleAssignmentHeaderId
                                           join material in _processOrderMaterialRepository.GetAll()
                                           on cubicleDetails.ProcessOrderMaterialId equals material.Id
                                           join cubicleMaster in _cubicleMasterRepository.GetAll()
                                           on lineClearance.CubicleId equals cubicleMaster.Id
                                           join checkpoint in _checkpointRepository.GetAll()
                                           on lineChecklistDetail.CheckPointId equals checkpoint.Id
                                           join checkpointType in _checkpointTypeRepository.GetAll()
                                           on checkpoint.CheckpointTypeId equals checkpointType.Id
                                           select new LineClearanceReportResultDto
                                           {
                                               Id = lineClearance.Id,
                                               ClearanceDate = lineClearance.CreationTime,
                                               CheckpointName = checkpoint.CheckpointName,
                                               CheckpointType = checkpointType.Title,
                                               ValueTag = checkpoint.ValueTag,
                                               AcceptanceValue = checkpoint.AcceptanceValue,
                                               UserdEnteredValue = lineChecklistDetail.Observation,
                                               Remark = lineChecklistDetail.Remark,
                                               MaterialCode = material.ItemCode,
                                               MaterialId = material.Id,
                                               GroupCode = cubicleHeader.GroupId,
                                               ProductCode = cubicleHeader.ProductCode,
                                               BatchNo = material.BatchNo,
                                               SapBatchNo = material.SAPBatchNo,
                                               SubPlantId = cubicleMaster.PlantId,
                                               IsSampling = lineClearance.IsSampling,
                                               TransactionStatus = transactionStatus.Status,
                                               CubicleCode = cubicleMaster.CubicleCode,
                                               CubicleId = cubicleMaster.Id,
                                               StartTime = lineClearance.StartTime,
                                               StopTime = lineClearance.StopTime,
                                               VerifiedBy = lineClearance.VerifiedBy,
                                               CreatedBy = lineClearance.CreatorUserId,
                                               LineClearanceCheckPointId = lineChecklistDetail.Id,
                                           };
            if (input.SubPlantId != null)
            {
                lineClearanceReportQuery = lineClearanceReportQuery.Where(x => Convert.ToInt32(input.SubPlantId) == x.SubPlantId);
            }
            if (input.ProductSAPBatchNos != null && input.ProductSAPBatchNos.Any())
            {
                lineClearanceReportQuery = lineClearanceReportQuery.Where(x => input.ProductSAPBatchNos.Contains(x.SapBatchNo));
            }
            if (input.CubicleListIds != null && input.CubicleListIds.Any())
            {
                lineClearanceReportQuery = lineClearanceReportQuery.Where(x => input.CubicleListIds.Contains(x.CubicleId));
            }
            if (input.MaterialListId != null && input.MaterialListId.Any())
            {
                lineClearanceReportQuery = lineClearanceReportQuery.Where(x => input.MaterialListId.Contains(x.MaterialCode));
            }
            if (input.ProductCodes != null && input.ProductCodes.Any())
            {
                lineClearanceReportQuery = lineClearanceReportQuery.Where(x => input.ProductCodes.Contains(x.ProductCode));
            }
            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                lineClearanceReportQuery = lineClearanceReportQuery.Where(x => x.ClearanceDate >= StartDate && x.ClearanceDate <= EndDate);
            }
            if (input.IsSampling != null)
            {
                lineClearanceReportQuery = lineClearanceReportQuery.Where(x => x.IsSampling == input.IsSampling);
            }
            return lineClearanceReportQuery;
        }

        protected IQueryable<CleaningLogReportResultDto> CreateCubicleCleaningReportFilteredQuery(CleaningLogReportRequestDto input)
        {
            var cubicleCleaningReportQuery = from cubicleCleaning in _cubicleCleaningTransactionRepository.GetAll()
                                             join cleanType in _cubicleCleaningTypeMasterRepository.GetAll()
                                             on cubicleCleaning.TypeId equals cleanType.Id
                                             join checkListDetails in _cubicleCleaningCheckpointRepository.GetAll()
                                           on cubicleCleaning.Id equals checkListDetails.CubicleCleaningTransactionId
                                             join transactionStatus in _statusRepository.GetAll()
                                             on cubicleCleaning.StatusId equals transactionStatus.Id
                                             join cubicleMaster in _cubicleMasterRepository.GetAll()
                                             on cubicleCleaning.CubicleId equals cubicleMaster.Id
                                             join checkpoint in _checkpointRepository.GetAll()
                                             on checkListDetails.CheckPointId equals checkpoint.Id
                                             join checkpointType in _checkpointTypeRepository.GetAll()
                                             on checkpoint.CheckpointTypeId equals checkpointType.Id
                                             select new CleaningLogReportResultDto
                                             {
                                                 Id = cubicleCleaning.Id,
                                                 CheckpointName = checkpoint.CheckpointName,
                                                 CheckpointType = checkpointType.Title,
                                                 ValueTag = checkpoint.ValueTag,
                                                 AcceptanceValue = checkpoint.AcceptanceValue,
                                                 UserdEnteredValue = checkListDetails.Observation,
                                                 Remark = checkListDetails.Remark,
                                                 SubPlantId = cubicleMaster.PlantId,
                                                 IsSampling = cubicleCleaning.IsSampling,
                                                 TransactionStatus = transactionStatus.Status,
                                                 CubicleCode = cubicleMaster.CubicleCode,
                                                 CubicleId = cubicleMaster.Id,
                                                 StartTime = cubicleCleaning.StartTime,
                                                 StopTime = cubicleCleaning.StopTime,
                                                 VerifiedBy = cubicleCleaning.VerifierId,
                                                 CleanerBy = cubicleCleaning.CleanerId,
                                                 CleaningDate = cubicleCleaning.CleaningDate,
                                                 CleaningType = cleanType.Value,
                                                 CleaningTypeId = cleanType.Id,
                                             };
            if (input.SubPlantId != null)
            {
                cubicleCleaningReportQuery = cubicleCleaningReportQuery.Where(x => Convert.ToInt32(input.SubPlantId) == x.SubPlantId);
            }

            if (input.CubicleListIds != null && input.CubicleListIds.Any())
            {
                cubicleCleaningReportQuery = cubicleCleaningReportQuery.Where(x => input.CubicleListIds.Contains(x.CubicleId));
            }
            if (input.cleaningTypeIds != null && input.cleaningTypeIds.Any())
            {
                cubicleCleaningReportQuery = cubicleCleaningReportQuery.Where(x => input.cleaningTypeIds.Contains(x.CleaningTypeId));
            }
            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                cubicleCleaningReportQuery = cubicleCleaningReportQuery.Where(x => x.CleaningDate >= StartDate && x.CleaningDate <= EndDate);
            }
            if (input.IsSampling != null)
            {
                cubicleCleaningReportQuery = cubicleCleaningReportQuery.Where(x => x.IsSampling == input.IsSampling);
            }
            return cubicleCleaningReportQuery;
        }

        protected IQueryable<CleaningLogReportResultDto> CreateEquipmentCleaningReportFilteredQuery(CleaningLogReportRequestDto input)
        {
            var equipmentCleaningReportQuery = from equipmentCleaning in _equipmentCleaningTransactionRepository.GetAll()
                                               join cleanType in _equipmentCleaningTypeMasterRepository.GetAll()
                                               on equipmentCleaning.CleaningTypeId equals cleanType.Id
                                               join checkListDetails in _equipmentCleaningCheckpointRepository.GetAll()
                                               on equipmentCleaning.Id equals checkListDetails.EquipmentCleaningTransactionId
                                               join transactionStatus in _statusRepository.GetAll()
                                               on equipmentCleaning.StatusId equals transactionStatus.Id
                                               join EquipmentMaster in _equipmentRepository.GetAll()
                                               on equipmentCleaning.EquipmentId equals EquipmentMaster.Id
                                               join checkpoint in _checkpointRepository.GetAll()
                                               on checkListDetails.CheckPointId equals checkpoint.Id
                                               join checkpointType in _checkpointTypeRepository.GetAll()
                                               on checkpoint.CheckpointTypeId equals checkpointType.Id
                                               select new CleaningLogReportResultDto
                                               {
                                                   Id = equipmentCleaning.Id,
                                                   CheckpointName = checkpoint.CheckpointName,
                                                   CheckpointType = checkpointType.Title,
                                                   ValueTag = checkpoint.ValueTag,
                                                   AcceptanceValue = checkpoint.AcceptanceValue,
                                                   UserdEnteredValue = checkListDetails.Observation,
                                                   Remark = checkListDetails.Remark,
                                                   SubPlantId = EquipmentMaster.PlantId,
                                                   IsSampling = equipmentCleaning.IsSampling,
                                                   TransactionStatus = transactionStatus.Status,
                                                   EquipmentCode = EquipmentMaster.EquipmentCode,
                                                   EquipmentId = EquipmentMaster.Id,
                                                   StartTime = equipmentCleaning.StartTime,
                                                   StopTime = equipmentCleaning.StopTime,
                                                   VerifiedBy = equipmentCleaning.VerifierId,
                                                   CleanerBy = equipmentCleaning.CleanerId,
                                                   CleaningDate = equipmentCleaning.CleaningDate,
                                                   CleaningType = cleanType.Value,
                                                   CleaningTypeId = cleanType.Id,
                                                   IsPortable = (bool)EquipmentMaster.IsPortable,
                                               };
            if (input.SubPlantId != null)
            {
                equipmentCleaningReportQuery = equipmentCleaningReportQuery.Where(x => Convert.ToInt32(input.SubPlantId) == x.SubPlantId);
            }

            if (input.EquipmentListIds != null && input.EquipmentListIds.Any())
            {
                equipmentCleaningReportQuery = equipmentCleaningReportQuery.Where(x => input.EquipmentListIds.Contains(x.EquipmentId));
            }
            if (input.cleaningTypeIds != null && input.cleaningTypeIds.Any())
            {
                equipmentCleaningReportQuery = equipmentCleaningReportQuery.Where(x => input.cleaningTypeIds.Contains(x.CleaningTypeId));
            }
            if (input.FromDate != default && input.ToDate != default)
            {
                var StartDate = input.FromDate.StartOfDay();
                var EndDate = input.ToDate.EndOfDay();
                equipmentCleaningReportQuery = equipmentCleaningReportQuery.Where(x => x.CleaningDate >= StartDate && x.CleaningDate <= EndDate);
            }
            if (input.IsSampling != null)
            {
                equipmentCleaningReportQuery = equipmentCleaningReportQuery.Where(x => x.IsSampling == input.IsSampling);
            }
            return equipmentCleaningReportQuery;
        }

        protected List<VehicleInspectionReportResultDto> ApplyGroupingOnVehicleInspectionReportDetails(List<VehicleInspectionReportResultDto> query)
        {
            var groupVehicleInspectionReportResults = query.OrderByDescending(a => a.InspectionDate).GroupBy(p => new { p.Id, p.MaterialId }).Select(s => new VehicleInspectionReportResultDto()
            {
                Id = s.First().Id,
                InspectionDate = s.First().InspectionDate,
                GatePassNo = s.First().GatePassNo,
                PurchaseOrderNo = s.First().PurchaseOrderNo,
                InvoiceNo = s.First().InvoiceNo,
                ChecklistName = s.First().ChecklistName,
                CheckPoints = s.Select(checkpoint => new VehicleInspectionCheckPointReportResultDto
                {
                    Id = checkpoint.VehicleInspectionDetailId,
                    CheckpointName = checkpoint.CheckpointName,
                    CheckpointType = checkpoint.CheckpointType,
                    ValueTag = checkpoint.ValueTag,
                    UserdEnteredValue = checkpoint.UserdEnteredValue,
                    AcceptanceValue = checkpoint.AcceptanceValue,
                    Remark = checkpoint.Remark
                }).ToList(),
                PurchaseOrderId = s.First().PurchaseOrderId,
                GateEntryId = s.First().GateEntryId,
                SubPlantId = s.First().SubPlantId,
                TransactionStatus = s.First().TransactionStatus,
                InspectedBy = s.First().InspectedBy
            }).ToList();
            return groupVehicleInspectionReportResults;
        }

        protected List<VehicleInspectionReportResultDto> ApplyGroupingOnMaterialInspectionReportDetails(List<VehicleInspectionReportResultDto> query)
        {
            var groupMaterialInspectionReportResults = query.OrderByDescending(a => a.InspectionDate).GroupBy(p => new { p.Id, p.MaterialId }).Select(s => new VehicleInspectionReportResultDto()
            {
                Id = s.First().Id,
                InspectionDate = s.First().InspectionDate,
                GatePassNo = s.First().GatePassNo,
                PurchaseOrderNo = s.First().PurchaseOrderNo,
                InvoiceNo = s.First().InvoiceNo,
                ChecklistName = s.First().ChecklistName,
                MaterialCode = s.First().MaterialCode,
                MaterialId = s.First().MaterialId,
                CheckPoints = s.Select(checkpoint => new VehicleInspectionCheckPointReportResultDto
                {
                    Id = checkpoint.VehicleInspectionDetailId,
                    CheckpointName = checkpoint.CheckpointName,
                    CheckpointType = checkpoint.CheckpointType,
                    ValueTag = checkpoint.ValueTag,
                    UserdEnteredValue = checkpoint.UserdEnteredValue,
                    AcceptanceValue = checkpoint.AcceptanceValue,
                    Remark = checkpoint.Remark
                }).ToList(),
                PurchaseOrderId = s.First().PurchaseOrderId,
                GateEntryId = s.First().GateEntryId,
                SubPlantId = s.First().SubPlantId,
                TransactionStatus = s.First().TransactionStatus,
                InspectedBy = s.First().InspectedBy
            }).Distinct().ToList();
            return groupMaterialInspectionReportResults;
        }

        protected List<AllocationReportResultDto> ApplyGroupingOnAllocationReportDetails(List<AllocationReportResultDto> query)
        {
            var allocationReportResults = query.OrderByDescending(a => a.AllocationDate).GroupBy(p => new { p.SubPlantId, p.AreaCode, p.MaterialCode, p.SapBatchNo }).Select(s => new AllocationReportResultDto()
            {
                Id = s.First().Id,
                SubPlantId = s.First().SubPlantId,
                AreaId = s.First().AreaId,
                MaterialId = s.First().MaterialId,
                PlantCode = s.First().PlantCode,
                AreaCode = s.First().AreaCode,
                LocationCode = string.Join(",", s.Select(x => x.LocationCode).Distinct()),
                MaterialCode = s.First().MaterialCode,
                SapBatchNo = s.First().SapBatchNo,
                Qty = s.First().Qty,
                StockStatus = s.First().StockStatus,
                AllocationDate = s.First().AllocationDate,
                AllocatedBy = s.First().AllocatedBy
            }).ToList();
            return allocationReportResults;
        }

        protected List<CubicleAssignedReportResultDto> ApplyGroupingOnCubicleAssigemntReportDetails(List<CubicleAssignedReportResultDto> query)
        {
            var allocationReportResults = query.OrderByDescending(a => a.CubicleAssignmentDate).GroupBy(p => new { p.SubPlantId, p.GroupCode, p.CubicleId, p.ProductCode, p.MaterialCode, p.BatchNo, p.SapBatchNo }).Select(s => new CubicleAssignedReportResultDto()
            {
                Id = s.First().Id,
                SubPlantId = s.First().SubPlantId,
                CubicleId = s.First().CubicleId,
                PlantCode = s.First().PlantCode,
                CubicleCode = s.First().CubicleCode,
                GroupCode = s.First().GroupCode,
                MaterialCode = s.First().MaterialCode,
                ProductCode = s.First().ProductCode,
                BatchNo = s.First().BatchNo,
                SapBatchNo = s.First().SapBatchNo,
                Qty = s.First().Qty,
                CubicleAssignmentDate = s.First().CubicleAssignmentDate,
                IsCubicleAssigned = s.First().IsCubicleAssigned,
                IsSampling = s.First().IsSampling,
                AssignmentBy = s.First().AssignmentBy
            }).ToList();
            return allocationReportResults;
        }

        protected List<PickingReportDto> ApplyGroupingOnPickingReportDetails(List<PickingReportDto> query)
        {
            var pickingReportResults = query.OrderByDescending(a => a.PickingTime).GroupBy(p => new { p.PlantCode, p.ProcessOrderNo, p.InspectionLotNo, p.ProductCode, p.BatchNo, p.MaterialCode, p.SapBatchNo }).Select(s => new PickingReportDto()
            {
                Id = s.First().Id,
                PlantCode = s.First().PlantCode,
                CubicleCode = s.First().CubicleCode,
                GroupId = s.First().GroupId,
                ProductCode = s.First().ProductCode,
                MaterialCode = s.First().MaterialCode,
                BatchNo = s.First().BatchNo,
                SapBatchNo = s.First().SapBatchNo,
                Quantity = s.First().Quantity,
                ContainerBarcode = s.First().ContainerBarcode,
                LocationCode = string.Join(",", s.Select(x => x.LocationCode).Distinct()),
                PickingTime = s.First().PickingTime,
                IsSampling = s.First().IsSampling,
                PickedBy = s.First().PickedBy,
                ProcessOrderNo = s.First().ProcessOrderNo,
                InspectionLotNo = s.First().InspectionLotNo,
                NoOfContainers = s.Select(z => z.ContainerBarcode).Distinct().Count()
            }).ToList();
            return pickingReportResults;
        }

        protected List<LineClearanceReportResultDto> ApplyGroupingOnLineClearanceReportDetails(List<LineClearanceReportResultDto> data)
        {
            var CreatedteduserList = data.OrderByDescending(a => a.ClearanceDate).Select(x => (int?)x.CreatedBy).Union(data.Select(x => x.VerifiedBy)).Where(x => x != null);
            var users = _userRepository.GetAll().Where(x => CreatedteduserList.Contains((int?)x.Id)).ToList();
            var groupLineClearanceReportResults = data.GroupBy(p => new { p.Id, p.GroupCode }).Select(s => new LineClearanceReportResultDto()
            {
                Id = s.First().Id,
                ClearanceDate = s.First().ClearanceDate,
                ChecklistName = s.First().ChecklistName,
                MaterialCode = string.Join(",", s.Select(x => x.MaterialCode).Distinct()),
                CubicleCode = s.First().CubicleCode,
                ProductCode = s.First().ProductCode,
                CheckPoints = s.GroupBy(s => s.LineClearanceCheckPointId).Select(checkpoint => new VehicleInspectionCheckPointReportResultDto
                {
                    CheckpointName = checkpoint.First().CheckpointName,
                    CheckpointType = checkpoint.First().CheckpointType,
                    ValueTag = checkpoint.First().ValueTag,
                    UserdEnteredValue = checkpoint.First().UserdEnteredValue,
                    AcceptanceValue = checkpoint.First().AcceptanceValue,
                    Remark = checkpoint.First().Remark
                }).Distinct().ToList(),
                SubPlantId = s.First().SubPlantId,
                TransactionStatus = s.First().TransactionStatus,
                SapBatchNo = string.Join(",", s.Select(x => x.SapBatchNo).Distinct()),
                BatchNo = s.First().BatchNo,
                CheckedBy = users.FirstOrDefault(x => x.Id == s.First().CreatedBy)?.FullName,
                DoneBy = users.FirstOrDefault(x => x.Id == s.First().VerifiedBy)?.FullName,
                StartTime = s.First().StartTime,
                StopTime = s.First().StopTime
            }).Distinct().ToList();

            return groupLineClearanceReportResults;
        }

        protected List<CleaningLogReportResultDto> ApplyGroupingOnCubicleCleaningReportDetails(List<CleaningLogReportResultDto> data)
        {
            var CreatedteduserList = data.OrderByDescending(a => a.CleaningDate).Select(x => (int?)x.CleanerBy).Union(data.Select(x => x.VerifiedBy)).Where(x => x != null);
            var users = _userRepository.GetAll().Where(x => CreatedteduserList.Contains((int?)x.Id)).ToList();
            var groupLineClearanceReportResults = data.GroupBy(p => new { p.Id }).Select(s => new CleaningLogReportResultDto()
            {
                Id = s.First().Id,
                CleaningDate = s.First().CleaningDate,
                CubicleCode = s.First().CubicleCode,
                CleaningType = s.First().CleaningType,
                CheckPoints = s.Select(checkpoint => new VehicleInspectionCheckPointReportResultDto
                {
                    CheckpointName = checkpoint.CheckpointName,
                    CheckpointType = checkpoint.CheckpointType,
                    ValueTag = checkpoint.ValueTag,
                    UserdEnteredValue = checkpoint.UserdEnteredValue,
                    AcceptanceValue = checkpoint.AcceptanceValue,
                    Remark = checkpoint.Remark
                }).Distinct().ToList(),
                SubPlantId = s.First().SubPlantId,
                TransactionStatus = s.First().TransactionStatus,
                CleanerName = users.FirstOrDefault(x => x.Id == s.First().CleanerBy)?.FullName,
                VerifiedName = users.FirstOrDefault(x => x.Id == s.First().VerifiedBy)?.FullName,
                StartTime = s.First().StartTime,
                StopTime = s.First().StopTime,
            }).Distinct().ToList();

            return groupLineClearanceReportResults;
        }

        protected List<CleaningLogReportResultDto> ApplyGroupingOnEquipmentCleaningReportDetails(List<CleaningLogReportResultDto> data)
        {
            var CreatedteduserList = data.OrderByDescending(a => a.CleaningDate).Select(x => (int?)x.CleanerBy).Union(data.Select(x => x.VerifiedBy)).Where(x => x != null);
            var users = _userRepository.GetAll().Where(x => CreatedteduserList.Contains((int?)x.Id)).ToList();
            var groupLineClearanceReportResults = data.GroupBy(p => new { p.Id }).Select(s => new CleaningLogReportResultDto()
            {
                Id = s.First().Id,
                CleaningDate = s.First().CleaningDate,
                EquipmentCode = s.First().EquipmentCode,
                CleaningType = s.First().CleaningType,
                IsPortable = s.First().IsPortable,
                CheckPoints = s.Select(checkpoint => new VehicleInspectionCheckPointReportResultDto
                {
                    CheckpointName = checkpoint.CheckpointName,
                    CheckpointType = checkpoint.CheckpointType,
                    ValueTag = checkpoint.ValueTag,
                    UserdEnteredValue = checkpoint.UserdEnteredValue,
                    AcceptanceValue = checkpoint.AcceptanceValue,
                    Remark = checkpoint.Remark
                }).Distinct().ToList(),
                SubPlantId = s.First().SubPlantId,
                TransactionStatus = s.First().TransactionStatus,
                CleanerName = users.FirstOrDefault(x => x.Id == s.First().CleanerBy)?.FullName,
                VerifiedName = users.FirstOrDefault(x => x.Id == s.First().VerifiedBy)?.FullName,
                StartTime = s.First().StartTime,
                StopTime = s.First().StopTime,
            }).Distinct().ToList();

            return groupLineClearanceReportResults;
        }
        protected IQueryable<WeighingCalibrationReportRequestDto> CreateWeighingCalibrationReportDetailsQuery(WeighingCalibrationReportRequestDto input)
        {
            var weighingCalibrationQuery = from calibration in _wMCalibrationHeaderRepository.GetAll()
                                           join wm in _weighingMachineRepository.GetAll()
                                           on calibration.WeighingMachineId equals wm.Id
                                           where calibration.CalibrationStatusId == 1
                                           select new WeighingCalibrationReportRequestDto
                                           {
                                               SubPlantId = wm.SubPlantId,
                                               Id = calibration.Id,
                                               WeighingMachineId = calibration.WeighingMachineId,
                                               CalibrationDate = calibration.CalibrationTestDate,
                                               FrequencyModeld = calibration.CalibrationFrequencyId,
                                           };
            if (input.SubPlantId != null)
            {
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.SubPlantId == input.SubPlantId);
            }
            if (input.WeighingMachineId != null)
            {
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.WeighingMachineId == input.WeighingMachineId);
            }
            if (input.FrequencyModeld == (int)WeighingMachineFrequencyType.Monthly && input.FrequencyModeld != null && input.CalibrationDate != default)
            {
                var firstDay = new DateTime(input.CalibrationStartDate.Year, input.CalibrationStartDate.Month, input.CalibrationStartDate.Day, 0, 0, 0, input.CalibrationStartDate.Kind);
                var lastDay = new DateTime(input.CalibrationEndDate.Year, input.CalibrationEndDate.Month, input.CalibrationEndDate.Day, 23, 59, 0, input.CalibrationEndDate.Kind);
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.CalibrationDate >= firstDay && x.CalibrationDate <= lastDay && x.FrequencyModeld == (int)WeighingMachineFrequencyType.Monthly);
            }
            if (input.FrequencyModeld == (int)WeighingMachineFrequencyType.Weekly && input.FrequencyModeld != null && input.CalibrationDate != default)
            {
                var firstDay = input.CalibrationStartDate.AddDays(-(int)input.CalibrationDate.DayOfWeek);
                var lastDay = input.CalibrationEndDate.AddDays(6 - (int)input.CalibrationDate.DayOfWeek);
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.CalibrationDate >= firstDay && x.CalibrationDate <= lastDay && x.FrequencyModeld == (int)WeighingMachineFrequencyType.Weekly);
            }
            if (input.FrequencyModeld == (int)WeighingMachineFrequencyType.Daily && input.FrequencyModeld != null && input.CalibrationDate != default)
            {
                var StartDate = input.CalibrationStartDate.StartOfDay();
                var EndDate = input.CalibrationEndDate.EndOfDay();
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.CalibrationDate >= StartDate && x.CalibrationDate <= EndDate && x.FrequencyModeld == (int)WeighingMachineFrequencyType.Daily);
            }
            //if (input.CalibrationDate != default)
            //{
            //    var StartDate = new DateTime(input.CalibrationStartDate.Year, input.CalibrationStartDate.Month, 1, 0, 0, 0, input.CalibrationStartDate.Kind);
            //    var EndDate = new DateTime(input.CalibrationEndDate.Year, input.CalibrationEndDate.Month, 1, 0, 0, 0, input.CalibrationEndDate.Kind);
            //    weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.CalibrationDate >= StartDate && x.CalibrationDate <= EndDate);
            //}
            return weighingCalibrationQuery.OrderByDescending(x => x.Id);
        }

        public async Task<bool> ValidateUserMode(int reportType)
        {
            var sessionUser = await _sessionAppService.GetCurrentLoginInformations();
            var storeUserModeId = _modeRepository.GetAll().Where(a => a.ModeName == PMMSConsts.StoreMode).FirstOrDefault().Id;
            var isValidUser = reportType == (int)PMMSEnums.ReportType.VehicleInspection || reportType == (int)PMMSEnums.ReportType.MaterialInspection || reportType == (int)PMMSEnums.ReportType.Allocation
                 || reportType == (int)PMMSEnums.ReportType.CubicleAssignment || reportType == (int)PMMSEnums.ReportType.Picking || reportType == (int)PMMSEnums.ReportType.LineClearance
                 || reportType == (int)PMMSEnums.ReportType.CubicleCleaning || reportType == (int)PMMSEnums.ReportType.EquipmentCleaning || reportType == (int)ReportType.WeighingCalibration
                ? sessionUser.User.ModeId == storeUserModeId || sessionUser.User.IsControllerMode || sessionUser.User.ModeId == 0
                : true;
            return isValidUser;
        }
    }
}