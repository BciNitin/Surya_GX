using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class BraidingFactory
    {
        public string SaveBraiding(Braiding braiding)
        {
            CodeContract.Required<ArgumentException>(braiding.IsNotNull(), "Wire Drawing is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", braiding.ProductionId);
                    cmd.Parameters.AddWithValue("@ProductionOrderItemId", braiding.ProductionOrderItemId);  //PalletCode
                    cmd.Parameters.AddWithValue("@MachineTypeId", braiding.MachineId.Key);
                    cmd.Parameters.AddWithValue("@MachineId", braiding.WireTypeId.Key);
                    cmd.Parameters.AddWithValue("@Type", braiding.Type);
                    cmd.Parameters.AddWithValue("@Section", braiding.Section);
                    cmd.Parameters.AddWithValue("@InputSpoolId", braiding.InputSpoolId);

                    cmd.Parameters.AddWithValue("@WireTypeId", braiding.WireTypeId);
                    cmd.Parameters.AddWithValue("@Length", braiding.Length);  //PalletCode
                    cmd.Parameters.AddWithValue("@OutputSpoolId", braiding.OutputSpoolId);
                    // cmd.Parameters.AddWithValue("@StartTime", braiding.StartTime);
                    // cmd.Parameters.AddWithValue("@EndTime", braiding.EndTime);
                    cmd.Parameters.AddWithValue("@IdleReasonId", braiding.IdleReasonId);
                    cmd.Parameters.AddWithValue("@IdleTime", braiding.IdleTime);

                    cmd.Parameters.AddWithValue("@Others", braiding.Others);
                    cmd.Parameters.AddWithValue("@UnderMaintainance", braiding.UnderMaintenance);
                    cmd.Parameters.AddWithValue("@CreatedBy", braiding.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", braiding.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "Braiding");

                    cmd.Parameters.AddWithValue("@IdleCode", braiding.IdleCode);
                    cmd.Parameters.AddWithValue("@TotalIdleTime", braiding.TotalIdleTime);  //PalletCode
                    cmd.Parameters.AddWithValue("@CYScrapKG", braiding.CYScrapKG);
                    cmd.Parameters.AddWithValue("@SYScrapKG", braiding.SYScrapKG);
                    cmd.Parameters.AddWithValue("@Technician", braiding.Technician.Key);

                    cmd.Parameters.AddWithValue("@TotalProduction", braiding.TotalProduction);

                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Braiding, long> FetchBraiding(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Braiding, long> bRaiding = new DataList<Braiding, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetBraiding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Braiding braiding = GetBraidingMaster(dr);
                            bRaiding.ItemCollection.Add(braiding);
                        }
                    }
                }
            }
            return bRaiding;
        }

        private Braiding GetBraidingMaster(SafeDataReader dr)
        {
            Braiding braiding = new Braiding();
            braiding.ProductionId = dr.GetInt64("ProductionOrderItemId");
            braiding.MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") };
            braiding.WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") };
            braiding.Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") };
            // braiding.Quantity = dr.GetInt32("Qty");
            braiding.TotalIdleTime = dr.GetString("TotalIdleTime");
            braiding.TotalProduction = dr.GetString("TotalProduction");
            //braiding.StartTime = dr.GetDateTime("StartTime");
            //braiding.EndTime = dr.GetDateTime("EndTime");

            return braiding;
        }
    }
}