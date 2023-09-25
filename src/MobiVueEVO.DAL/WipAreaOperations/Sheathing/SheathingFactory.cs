using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class SheathingFactory
    {
        public string SaveSheathing(Sheathing sheathing)
        {
            CodeContract.Required<ArgumentException>(sheathing.IsNotNull(), "Sheathing is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", sheathing.ProductionId);
                    cmd.Parameters.AddWithValue("@ProductionOrderItemId", sheathing.ProductionOrderItemId);  //PalletCode
                    cmd.Parameters.AddWithValue("@ShiftId", sheathing.ShiftId);
                    cmd.Parameters.AddWithValue("@MachineId", sheathing.MachineId.Key);
                    // cmd.Parameters.AddWithValue("@MachineTypeId", sheathing.MachineTypeId);
                    cmd.Parameters.AddWithValue("@Number", sheathing.Number);
                    cmd.Parameters.AddWithValue("@Color", sheathing.Color);
                    cmd.Parameters.AddWithValue("@Section", sheathing.Section);
                    cmd.Parameters.AddWithValue("@InputSpoolId", sheathing.InputSpoolId);
                    cmd.Parameters.AddWithValue("@Length", sheathing.Length);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", sheathing.OutputSpoolId);
                    //cmd.Parameters.AddWithValue("@StartTime", sheathing.StartTime);
                    //cmd.Parameters.AddWithValue("@EndTime", sheathing.EndTime);
                    cmd.Parameters.AddWithValue("@PVCGrade", sheathing.PvcGrade);
                    cmd.Parameters.AddWithValue("@Dia", sheathing.Dia);
                    // cmd.Parameters.AddWithValue("@Is", sheathing.Is);
                    // cmd.Parameters.AddWithValue("@Os", sheathing.Os);
                    cmd.Parameters.AddWithValue("@HeatUp", sheathing.Heatup);
                    cmd.Parameters.AddWithValue("@FilterChange", sheathing.FilterChange);
                    cmd.Parameters.AddWithValue("@Cleaning", sheathing.Cleaning);
                    cmd.Parameters.AddWithValue("@Loading", sheathing.Loading);
                    cmd.Parameters.AddWithValue("@HeadSheathing", sheathing.HeadSheathing);
                    cmd.Parameters.AddWithValue("@WaitingForMaterial", sheathing.WaitingForMaterial);
                    cmd.Parameters.AddWithValue("@JetPrinterCleaning", sheathing.JetPrinterCleaning);
                    cmd.Parameters.AddWithValue("@ScrewCleaning", sheathing.ScrewCleaning);
                    cmd.Parameters.AddWithValue("@ColorChange", sheathing.ColorChange);
                    cmd.Parameters.AddWithValue("@PVCScrap", sheathing.PVCScrap);
                    cmd.Parameters.AddWithValue("@CopperScrap", sheathing.CopperScrap);
                    cmd.Parameters.AddWithValue("@WireScrap", sheathing.WireScrap);  //PalletCode
                    cmd.Parameters.AddWithValue("@Unloading", sheathing.Unloading);
                    cmd.Parameters.AddWithValue("@Others", sheathing.Others);
                    cmd.Parameters.AddWithValue("@UnderMaintainance", sheathing.UnderMaintenance);

                    cmd.Parameters.AddWithValue("@ArticleNo", sheathing.ArticleNo);
                    cmd.Parameters.AddWithValue("@TotalKM", sheathing.TotalKM);
                    cmd.Parameters.AddWithValue("@TotalIdleTime", sheathing.TotalIdleTime);
                    cmd.Parameters.AddWithValue("@TotalProduction", sheathing.TotalProduction);

                    cmd.Parameters.AddWithValue("@IsOs", sheathing.IsOs);

                    cmd.Parameters.AddWithValue("@CreatedBy", sheathing.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", sheathing.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "Sheathing");
                    cmd.Parameters.AddWithValue("@Technician", sheathing.Technician.Key);
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Sheathing, long> FetchSheathing(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Sheathing, long> sheathing = new DataList<Sheathing, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetSheathing"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Sheathing sheaThing = GetSheathingMaster(dr);
                            sheathing.ItemCollection.Add(sheaThing);
                        }
                    }
                }
            }
            return sheathing;
        }

        private Sheathing GetSheathingMaster(SafeDataReader dr)
        {
            Sheathing Sheathing = new Sheathing
            {
                ProductionId = dr.GetInt64("ProductionOrderId"),
                MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") },
                // WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") },
                Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") },
                // braiding.Quantity = dr.GetInt32("Qty");
                TotalIdleTime = dr.GetString("TotalIdleTime"),
                TotalProduction = dr.GetString("TotalProduction"),
                //StartTime = dr.GetDateTime("StartTime"),
                //EndTime = dr.GetDateTime("EndTime")
            };

            return Sheathing;
        }
    }
}