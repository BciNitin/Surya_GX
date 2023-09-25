using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using MobiVueEVO.BO.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class WireDrawingFactory
    {
        public Shift GetShift(int plantid)
        {
            CodeContract.Required<ArgumentException>(plantid > 0, "Plant is required to get the shift");
            Shift shift = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@PlantId", 1);
                    cmd.Parameters.AddWithValue("@OperationType", "GetShift");

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            shift = new Shift() { ShiftEndTime = dr.GetString("ShiftEnd"), ShiftName = dr.GetString("ShiftName"), ShiftStartTime = dr.GetString("ShiftStart") };
                        }
                    }
                }
                return shift;
            }
        }

        public string SaveWireDrawing(WireDrawing wiredrawing)
        {
            CodeContract.Required<ArgumentException>(wiredrawing.IsNotNull(), "Wire Drawing is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                // '@ProductionOrderId

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", wiredrawing.ProductionId);
                    cmd.Parameters.AddWithValue("ProductionOrderItemId", wiredrawing.ProductionOrderItemId);  //PalletCode
                    cmd.Parameters.AddWithValue("@MachineId", wiredrawing.MachineId.Key);
                    cmd.Parameters.AddWithValue("@WireTypeId", wiredrawing.WireTypeId.Key);
                    cmd.Parameters.AddWithValue("@Qty", wiredrawing.Quantity);
                    //    cmd.Parameters.AddWithValue("@StartTime", wiredrawing.StartTime);
                    //  cmd.Parameters.AddWithValue("@EndTime", wiredrawing.EndTime);

                    cmd.Parameters.AddWithValue("@InputPalletId", wiredrawing.InputPalletId);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", wiredrawing.OutputSpoolId);  //PalletCode
                    cmd.Parameters.AddWithValue("@IdleTime", wiredrawing.IdleReasonTime);
                    cmd.Parameters.AddWithValue("@IdleReasonId", wiredrawing.IdleReasonId.Key);
                    cmd.Parameters.AddWithValue("@ConstructionControlInLargeTank", wiredrawing.CContLargeTank);
                    cmd.Parameters.AddWithValue("@ConstructionControlinAnnealer", wiredrawing.CContAnnealer);
                    cmd.Parameters.AddWithValue("@TempControlInLargeTank", wiredrawing.TempContLargeTank);

                    cmd.Parameters.AddWithValue("@TempControlInAnnealer", wiredrawing.TempContAnnealer);
                    cmd.Parameters.AddWithValue("@AdjustmentValueportraitmeterannealer", wiredrawing.AdjustValueAnnealer);
                    cmd.Parameters.AddWithValue("@ConstructionControl", wiredrawing.ConstructionControl);
                    cmd.Parameters.AddWithValue("@TempControl", wiredrawing.TempControl);
                    cmd.Parameters.AddWithValue("@CreatedBy", wiredrawing.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", wiredrawing.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "WireDrawing");
                    cmd.Parameters.AddWithValue("@Technician", wiredrawing.Technician.Key);
                    cmd.Parameters.AddWithValue("@TotalProduction", wiredrawing.TotalProduction);
                    cmd.Parameters.AddWithValue("@TotalScrap", wiredrawing.TotalScrap);

                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public short GetMachineNo(string machineNo, int plantId)
        {
            WireDrawing entity = new WireDrawing();
            CodeContract.Required<ArgumentException>(machineNo.IsNotNullOrWhiteSpace(), "Machine No is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Machine";
                    cmd.Parameters.AddWithValue("@MachineId", machineNo);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "GetMachineByMachineId");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            return dr.GetInt16("Id");
                        }
                        // else
                        //   return 0;
                    }
                }
            }
            return 0;
        }

        public int GetSpoolId(string spoolCode, int plantId)
        {
            WireDrawing entity = new WireDrawing();
            CodeContract.Required<ArgumentException>(spoolCode.IsNotNullOrWhiteSpace(), "Spool Code No is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SpoolMaster";
                    cmd.Parameters.AddWithValue("@SpoolCode", spoolCode);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Mode", "GetSpoolIdbyCode");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            entity.OutputSpoolId = dr.GetInt32("Id");
                        }
                    }
                }
            }
            return entity.OutputSpoolId;
        }

        public int GetPalletId(string palletCode, int plantId)
        {
            WireDrawing entity = new WireDrawing();
            CodeContract.Required<ArgumentException>(palletCode.IsNotNullOrWhiteSpace(), "Pallet Code No is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Pallet";
                    cmd.Parameters.AddWithValue("@PalletCode", palletCode);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "GetPalletIDByCode");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            entity.InputPalletId = dr.GetInt32("Id");
                        }
                    }
                }
            }
            return entity.InputPalletId;
        }

        public List<KeyValue<short, string>> GetConstruction(int plantId, short machineid)
        {
            List<KeyValue<short, string>> wireTypeFormat = new List<KeyValue<short, string>>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WIPAreaGetValues";

                    cmd.Parameters.AddWithValue("@plantid", plantId);
                    cmd.Parameters.AddWithValue("@MachineId", machineid);

                    cmd.Parameters.AddWithValue("@Type", "WireType");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            wireTypeFormat.Add(new KeyValue<short, string>() { Key = dr.GetInt16("ID"), Value = dr.GetString("WireType") });
                        }
                    }
                }
            }
            return wireTypeFormat;
        }

        public List<KeyValue<short, string>> GetMachine(int plantId, short machinetypeid)
        {
            List<KeyValue<short, string>> wireTypeFormat = new List<KeyValue<short, string>>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WIPAreaGetValues";

                    cmd.Parameters.AddWithValue("@plantid", plantId);
                    cmd.Parameters.AddWithValue("@MachineTypeId", machinetypeid);

                    cmd.Parameters.AddWithValue("@Type", "Machine");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            wireTypeFormat.Add(new KeyValue<short, string>() { Key = dr.GetInt16("ID"), Value = dr.GetString("MachineName") });
                        }
                    }
                }
            }
            return wireTypeFormat;
        }

        public List<KeyValue<short, string>> GetIdleReason(int plantId)
        {
            List<KeyValue<short, string>> reasonFormat = new List<KeyValue<short, string>>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WIPAreaGetValues";

                    cmd.Parameters.AddWithValue("@plantid", plantId);
                    cmd.Parameters.AddWithValue("@Type", "Reason");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            reasonFormat.Add(new KeyValue<short, string>() { Key = dr.GetInt16("ID"), Value = dr.GetString("IdleCode") });
                        }
                    }
                }
            }
            return reasonFormat;
        }

        public short GetMachineTypeId(string machinetypeNo, int plantId)
        {
            WireDrawing entity = new WireDrawing();
            CodeContract.Required<ArgumentException>(machinetypeNo.IsNotNullOrWhiteSpace(), "Machine No is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_MachineType";
                    cmd.Parameters.AddWithValue("@Name", machinetypeNo);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "GetMachineTypeId");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            return dr.GetInt16("Id");
                        }
                        // else
                        //   return 0;
                    }
                }
            }
            return 0;
        }

        public List<KeyValue<long, string>> GetTechnician(int processid)
        {
            List<KeyValue<long, string>> wireTypeFormat = new List<KeyValue<long, string>>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_TechnicianMaster";

                    cmd.Parameters.AddWithValue("@ProcessId", processid);
                    cmd.Parameters.AddWithValue("@Type", "GetTechnicianProcessId");
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            wireTypeFormat.Add(new KeyValue<long, string>() { Key = dr.GetInt64("UserId"), Value = dr.GetString("TechnicianName") });
                        }
                    }
                }
            }
            return wireTypeFormat;
        }

        public DataList<WireDrawing, long> FetchWireDrawing(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<WireDrawing, long> WireDrawing = new DataList<WireDrawing, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetWireDrawing"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            WireDrawing wireDrawing = GetWireDrawingMaster(dr);
                            WireDrawing.ItemCollection.Add(wireDrawing);
                        }
                    }
                }
            }
            return WireDrawing;
        }

        private WireDrawing GetWireDrawingMaster(SafeDataReader dr)
        {
            WireDrawing WireCategory = new WireDrawing();
            WireCategory.ProductionId = dr.GetInt64("ProductionOrderId");
            WireCategory.MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") };
            WireCategory.WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") };
            WireCategory.Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") };
            WireCategory.Quantity = dr.GetInt32("Qty");
            WireCategory.TotalProduction = dr.GetString("TotalProduction");
            // TimeSpan ttime = new TimeSpan(dr.GetDateTime("StartTime"));
            //WireCategory.StartTime = DateTime.ParseExact(dr.GetDateTime("StartTime"), CultureInfo.InvariantCulture).ToString();
            // WireCategory.StartTime = dr.GetString("StartTime").ToString();
            //WireCategory.EndTime = dr.GetString("EndTime").ToString();


            return WireCategory;
        }
    }
}