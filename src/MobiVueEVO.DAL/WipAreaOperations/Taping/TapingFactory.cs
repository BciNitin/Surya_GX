using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class TapingFactory
    {
        public string SaveTaping(Taping taping)
        {
            CodeContract.Required<ArgumentException>(taping.IsNotNull(), "Taping is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", taping.ProductionId);
                    // cmd.Parameters.AddWithValue("@ProductionOrderItemId", taping.ProductionOrderItemId);  //PalletCode
                    cmd.Parameters.AddWithValue("@MachineId", taping.MachineId.Key);
                    cmd.Parameters.AddWithValue("@AlymerTape", taping.AlymerTape);
                    cmd.Parameters.AddWithValue("@PolysterTape", taping.PolysterTape);
                    cmd.Parameters.AddWithValue("@Type", taping.Type);
                    cmd.Parameters.AddWithValue("@Section", taping.Section);
                    //cmd.Parameters.AddWithValue("@IdleReasonId", taping.IdleReasonId.Key);
                    //cmd.Parameters.AddWithValue("@IdleReasonTime", taping.IdleReasonTime);
                    cmd.Parameters.AddWithValue("@Bobbin_Loading", taping.Bobbin_Loading);
                    cmd.Parameters.AddWithValue("@Tape_Loading", taping.Tape_Loading);
                    cmd.Parameters.AddWithValue("@Cable_Joint", taping.Cable_Joint);
                    cmd.Parameters.AddWithValue("@Waiting_For_Planning", taping.Waiting_For_Planning);
                    cmd.Parameters.AddWithValue("@Waiting_For_Raw_Material", taping.Waiting_For_Raw_Material);
                    cmd.Parameters.AddWithValue("@Waiting_For_Spool", taping.Waiting_For_Spool);
                    cmd.Parameters.AddWithValue("@Mic_Problem", taping.Mic_Problem);
                    cmd.Parameters.AddWithValue("@5_S", taping._5_S);
                    cmd.Parameters.AddWithValue("@QC_Operation", taping.QC_Operation);

                    cmd.Parameters.AddWithValue("@PairNo", taping.PairNo);
                    cmd.Parameters.AddWithValue("@Length", taping.Length);
                    //cmd.Parameters.AddWithValue("@StartTime", taping.StartTime);
                    //cmd.Parameters.AddWithValue("@EndTime", taping.EndTime);
                    // cmd.Parameters.AddWithValue("@StartTime", "2023-02-1");
                    //cmd.Parameters.AddWithValue("@EndTime", "2023-02-1");
                    cmd.Parameters.AddWithValue("@InputSpoolId", taping.InputSpoolId);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", taping.OutputSpoolId);

                    cmd.Parameters.AddWithValue("@Technician", taping.Technician.Key);
                    cmd.Parameters.AddWithValue("@TotalIdletime", taping.TotalIdletime);

                    cmd.Parameters.AddWithValue("@CreatedBy", taping.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", taping.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "Taping");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Taping, long> FetchTaping(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Taping, long> taping = new DataList<Taping, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetTaping"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Taping taPing = GetTapingMaster(dr);
                            taping.ItemCollection.Add(taPing);
                        }
                    }
                }
            }
            return taping;
        }

        private Taping GetTapingMaster(SafeDataReader dr)
        {
            Taping Taping = new Taping
            {
                ProductionId = dr.GetInt64("ProductionOrderId"),
                MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") },
                //Taping.WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") },
                Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") },
                TotalIdletime = dr.GetString("TotalIdletime")
            };


            return Taping;
        }
    }
}