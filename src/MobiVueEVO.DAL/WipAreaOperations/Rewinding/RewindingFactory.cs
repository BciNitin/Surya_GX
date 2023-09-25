using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class RewindingFactory
    {
        public string SaveRewinding(Rewinding rewinding)
        {
            CodeContract.Required<ArgumentException>(rewinding.IsNotNull(), "Rewinding is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", rewinding.ProductionId);
                    cmd.Parameters.AddWithValue("@ProductionOrderItemId", rewinding.ProductionOrderItemId);  //PalletCode
                    cmd.Parameters.AddWithValue("@MachineId", rewinding.MachineId.Key);
                    // cmd.Parameters.AddWithValue("@MachineTypeId", rewinding.MachineTypeId);
                    cmd.Parameters.AddWithValue("@Type", rewinding.Type);
                    cmd.Parameters.AddWithValue("@Section", rewinding.Section);
                    cmd.Parameters.AddWithValue("@InputSpoolId", rewinding.InputSpoolId);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", rewinding.OutputSpoolId);
                    cmd.Parameters.AddWithValue("@ActualLength", rewinding.Length);
                    // cmd.Parameters.AddWithValue("@StartTime", rewinding.StartTime);
                    // cmd.Parameters.AddWithValue("@EndTime", rewinding.EndTime);
                    cmd.Parameters.AddWithValue("@NoOfChangeovers", rewinding.NoOfChangeovers);
                    cmd.Parameters.AddWithValue("@Loading", rewinding.Loading);
                    cmd.Parameters.AddWithValue("@ChangingTools", rewinding.ChangingTools);
                    cmd.Parameters.AddWithValue("@Rupture", rewinding.Rupture);
                    cmd.Parameters.AddWithValue("@Cleaning", rewinding.Cleaning);
                    // cmd.Parameters.AddWithValue("@Loading", rewinding.Loading);
                    // cmd.Parameters.AddWithValue("@IdleTime", rewinding.IdleReasonTime);
                    cmd.Parameters.AddWithValue("@MachineProblem", rewinding.MachineProblem);
                    cmd.Parameters.AddWithValue("@NoMaterial", rewinding.NoMaterial);
                    cmd.Parameters.AddWithValue("@Others", rewinding.Others);
                    cmd.Parameters.AddWithValue("@UnderMaintainance", rewinding.UnderMaintenance);

                    //new fields
                    cmd.Parameters.AddWithValue("@Wirercrap", rewinding.Wirercrap);
                    cmd.Parameters.AddWithValue("@Coppercrap", rewinding.Coppercrap);
                    cmd.Parameters.AddWithValue("@PVCScrap", rewinding.PVCScrap);
                    cmd.Parameters.AddWithValue("@TotalProduction", rewinding.TotalProduction);
                    cmd.Parameters.AddWithValue("@Technician", rewinding.Technician.Key);

                    cmd.Parameters.AddWithValue("@CreatedBy", rewinding.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", rewinding.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "Rewinding");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Rewinding, long> FetchRewinding(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Rewinding, long> reWinding = new DataList<Rewinding, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetRewinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Rewinding rewinding = GetRewindingMaster(dr);
                            reWinding.ItemCollection.Add(rewinding);
                        }
                    }
                }
            }
            return reWinding;
        }

        private Rewinding GetRewindingMaster(SafeDataReader dr)
        {
            Rewinding rewinding = new Rewinding
            {
                ProductionId = dr.GetInt64("ProductionOrderId"),
                MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") },
                // Rewinding.WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") };
                Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") },
                // braiding.Quantity = dr.GetInt32("Qty");
                // Rewinding.TotalIdleTime = dr.GetString("TotalIdleTime");
                TotalProduction = dr.GetString("TotalProduction"),
                //StartTime = dr.GetDateTime("StartTime"),
                //EndTime = dr.GetDateTime("EndTime")
            };

            return rewinding;
        }
    }
}