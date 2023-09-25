using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;

using Microsoft.AspNetCore.Http;

using ELog.Application.Modules;
using ELog.Application.Sessions;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;

using System;
using System.Threading.Tasks;
using ELog.Application.WeighingCalibrations;
using Microsoft.Data.SqlClient;
using System.Data;
using ELog.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ELog.Application.Masters.Areas;
using System.Collections.Generic;
using Newtonsoft.Json;
using PMMS.Application.ElogApi.Dto;

namespace ELog.Application.GXReports
{


    /*
        [PMMSAuthorize]*/
    public class GXReportApiService : ApplicationService, IGXReportApiService
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
        public GXReportApiService(IRepository<PurchaseOrder> purchaseOrderRepository,
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





        //public async Task<Object> GetFirstArticleInspectionApprovalReport(string Work_Order_Number, string Model)
        //{
        //    DataTable result = new DataTable();
        //    string connection = _configuration["ConnectionStrings:Default"];
        //    SqlConnection conn = null;
        //    //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //    conn = new SqlConnection(connection);

        //    try
        //    {
        //        conn.Open();
        //    }
        //    catch (Exception)
        //    {
        //        string e = "Database error contact administrator";

        //    }
        //    try
        //    {
        //        SqlDataReader myReader = null;
        //        SqlCommand myCommand = new SqlCommand("sp_First_Article_Inspection_Approval_Report", conn);
        //        myCommand.CommandType = CommandType.StoredProcedure;

        //        myCommand.Parameters.Add("@Work_Order_Number", SqlDbType.VarChar).Value = Work_Order_Number;
        //        myCommand.Parameters.Add("@Model", SqlDbType.VarChar).Value = Model;

        //        myReader = myCommand.ExecuteReader();
        //        DataTable tab = new DataTable();
        //        tab.Load(myReader);

        //        /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
        //          while (myReader.Read())
        //          {
        //              JsonObjectCollection jsonObject = new JsonObjectCollection();
        //  for(string columnName in columns)
        //                  jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
        //              jsonArray.Add(jsonObject);
        //          }*/
        //        result = tab;
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    return result;

        //}

        public async Task<String> GetFirstArticleInspectionApprovalReport(string Work_Order_Number, string Model)
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
                SqlCommand myCommand = new SqlCommand("sp_First_Article_Inspection_Approval_Report", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@Work_Order_Number", SqlDbType.VarChar).Value = Work_Order_Number;
                 myCommand.Parameters.Add("@Model", SqlDbType.VarChar).Value = Model;

                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                Dictionary<string, object> row;
                tab.Load(myReader);
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(tab);
                /*  List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                  foreach (DataRow dr in tab.Rows)
                  {
                      row = new Dictionary<string, object>();
                      foreach (DataColumn col in tab.Columns)
                      {
                          row.Add(col.ColumnName, dr[col]);
                      }
                      rows.Add(row);
                  }*/
                //  string myString = JsonConvert.ToString(JSONresult);
                return JSONresult;
                /* 


                  return tab.ToString();*/
            }
            catch (Exception e)
            {
                return null;
            }


        }



    }
}