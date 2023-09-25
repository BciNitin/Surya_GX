
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.Modules;
using ELog.Application.Sessions;
using ELog.Application.WeighingCalibrations;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application.CSVBulkUpload
{


    /*
        [PMMSAuthorize]*/
    public class CSVBulkUploadService : ApplicationService, ICSVBulkUploadService
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
        public CSVBulkUploadService(IRepository<PurchaseOrder> purchaseOrderRepository,
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

        public async Task<object> BulkUploadForChecklistReport(string tablename,  string json, string CheckDuplicateColumns)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            DataSet tab = new DataSet();
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            int rowsaffected = 0;
            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("InsertBulkUploadNew", conn);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@json", SqlDbType.VarChar).Value = json;
                myCommand.Parameters.Add("@CheckDuplicateColumns", SqlDbType.VarChar).Value = CheckDuplicateColumns;
             

                rowsaffected = myCommand.ExecuteNonQuery();
                var adapter = new SqlDataAdapter(myCommand);

                adapter.Fill(tab);

                var result = new
                {
                    InsertedRows = tab.Tables[0],
                    DuplicateRows = tab.Tables[1]
                };

                return result;


                //myReader = myCommand.ExecuteReader();
                //DataSet tab = new DataSet();
                //tab.Load(myReader);

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/
                //       return rowsaffected;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return rowsaffected;


        }

        public async Task<object> BulkUpload(string tablename, string ColumnName, string Value)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            int rowsaffected = 0;
            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
            }
            try
            {
                SqlCommand myCommand = new SqlCommand("spInsertLogWiseDataForGX", conn);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@ColumnName", SqlDbType.VarChar).Value = ColumnName;
                myCommand.Parameters.Add("@Values", SqlDbType.VarChar).Value = Value;
               

                rowsaffected = myCommand.ExecuteNonQuery();
                //myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                //tab.Load(myReader);

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/
                //       return rowsaffected;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return rowsaffected;


        }

    }
}