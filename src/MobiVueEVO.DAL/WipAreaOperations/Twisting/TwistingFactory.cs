using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class TwistingFactory
    {
        public string SaveTwisting(Twisting twisting)
        {
            CodeContract.Required<ArgumentException>(twisting.IsNotNull(), "Twisting is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", twisting.ProductionOrderId);
                    cmd.Parameters.AddWithValue("@ProductionOrderItemId", twisting.ProductionOrderItemId);  //PalletCode
                                                                                                            //  cmd.Parameters.AddWithValue("@MachineTypeId", twisting.MachineTypeId);
                    cmd.Parameters.AddWithValue("@MachineId", twisting.MachineId.Key);
                    cmd.Parameters.AddWithValue("@Type", twisting.Type);
                    cmd.Parameters.AddWithValue("@Section", twisting.Section);
                    cmd.Parameters.AddWithValue("@InputSpoolId", twisting.InputSpoolId);

                    cmd.Parameters.AddWithValue("@AlymerTape", twisting.AlymerTape);
                    cmd.Parameters.AddWithValue("@PolysterTape", twisting.PolysterTape);  //PalletCode
                    cmd.Parameters.AddWithValue("@LineSpeed", twisting.LineSpeed);
                    cmd.Parameters.AddWithValue("@Length", twisting.Length);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", twisting.OutputSpoolId);
                    //  cmd.Parameters.AddWithValue("@StartTime", twisting.StartTime);
                    // cmd.Parameters.AddWithValue("@EndTime", twisting.EndTime);

                    cmd.Parameters.AddWithValue("@Layer", twisting.Layer);
                    cmd.Parameters.AddWithValue("@ArrangingSpool", twisting.ArrangingSpool);
                    cmd.Parameters.AddWithValue("@Loading", twisting.Loading);
                    cmd.Parameters.AddWithValue("@Unloading", twisting.Unloading);
                    cmd.Parameters.AddWithValue("@Welding", twisting.Welding);
                    cmd.Parameters.AddWithValue("@CableJoint", twisting.CableJoint);
                    cmd.Parameters.AddWithValue("@WireCut", twisting.WireCut);
                    cmd.Parameters.AddWithValue("@Others", twisting.Others);
                    cmd.Parameters.AddWithValue("@UnderMaintainance", twisting.UnderMaintainance);
                    cmd.Parameters.AddWithValue("@Remarks", twisting.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", twisting.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@OperationType", "Twisting");

                    cmd.Parameters.AddWithValue("@TotalProduction", twisting.TotalProduction);
                    cmd.Parameters.AddWithValue("@WireScrap", twisting.WireScrap);
                    cmd.Parameters.AddWithValue("@CopperScrap", twisting.CopperScrap);
                    cmd.Parameters.AddWithValue("@TapeScrap", twisting.TapeScrap);
                    cmd.Parameters.AddWithValue("@Technician", twisting.Technician.Key);

                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Twisting, long> FetchTwisting(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Twisting, long> insuLation = new DataList<Twisting, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetTwisting"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Twisting insulation = GetTwistingMaster(dr);
                            insuLation.ItemCollection.Add(insulation);
                        }
                    }
                }
            }
            return insuLation;
        }

        private Twisting GetTwistingMaster(SafeDataReader dr)
        {
            Twisting Twisting = new Twisting
            {
                ProductionOrderId = dr.GetInt64("ProductionOrderId"),
                MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") },
                // WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") },
                Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") },
                // braiding.Quantity = dr.GetInt32("Qty");
                // Twisting.TotalIdleTime = dr.GetString("TotalIdleTime");
                TotalProduction = dr.GetString("TotalProduction"),
                // StartTime = dr.GetDateTime("StartTime"),
                // EndTime = dr.GetDateTime("EndTime")
            };

            return Twisting;
        }
    }
}