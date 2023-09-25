using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class InsulationFactory
    {
        public string SaveInsulation(Insulation insulation)
        {
            CodeContract.Required<ArgumentException>(insulation.IsNotNull(), "Insulation is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", insulation.ProductionId);
                    cmd.Parameters.AddWithValue("@ProductionOrderItemId", insulation.ProductionOrderItemId);  //PalletCode
                    cmd.Parameters.AddWithValue("@MachineId", insulation.MachineId.Key);
                    //  cmd.Parameters.AddWithValue("@MachineTypeId", insulation.MachineTypeId.Key);
                    cmd.Parameters.AddWithValue("@FgNo", insulation.FGNO);
                    cmd.Parameters.AddWithValue("@Type", insulation.Type);
                    cmd.Parameters.AddWithValue("@Section", insulation.Section);
                    cmd.Parameters.AddWithValue("@Dia", insulation.Dia);
                    cmd.Parameters.AddWithValue("@Number", insulation.Number);
                    cmd.Parameters.AddWithValue("@Color", insulation.Color);
                    cmd.Parameters.AddWithValue("@Length", insulation.Length);
                    // cmd.Parameters.AddWithValue("@StartTime", insulation.StartTime);
                    // cmd.Parameters.AddWithValue("@EndTime", insulation.EndTime);
                    cmd.Parameters.AddWithValue("@InputPalletId", insulation.InputPalletId);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", insulation.OutputSpoolId);
                    cmd.Parameters.AddWithValue("@PvcGrade", insulation.PvcGrade);
                    cmd.Parameters.AddWithValue("@Changeover", insulation.ChangeOver);  //PalletCode
                    cmd.Parameters.AddWithValue("@ColorChange", insulation.ColorChange);
                    cmd.Parameters.AddWithValue("@Loading", insulation.Loading);
                    cmd.Parameters.AddWithValue("@Unloading", insulation.Unloading);

                    cmd.Parameters.AddWithValue("@Welding", insulation.Welding);
                    //cmd.Parameters.AddWithValue("@WaitingForSpool", insulation.WaitingForSpool);
                    cmd.Parameters.AddWithValue("@WaitingForPVCPreHeating", insulation.WaitingForPVCPreHeat);
                    cmd.Parameters.AddWithValue("@UnderMaintainance", insulation.UnderMaintenance);

                    cmd.Parameters.AddWithValue("@CreatedBy", insulation.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", insulation.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "Insulation");

                    cmd.Parameters.AddWithValue("@TotalProduction", insulation.TotalProduction);
                    cmd.Parameters.AddWithValue("@PVCSCrapKG", insulation.PVCSCrapKG);
                    cmd.Parameters.AddWithValue("@CopperScrapKG", insulation.CopperScrapKG);
                    cmd.Parameters.AddWithValue("@WireScrapKG", insulation.WireScrapKG);
                    cmd.Parameters.AddWithValue("@Technician", insulation.Technician.Key);

                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Insulation, long> FetchInsulation(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Insulation, long> insuLation = new DataList<Insulation, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetInsulation"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Insulation insulation = GetInsulationMaster(dr);
                            insuLation.ItemCollection.Add(insulation);
                        }
                    }
                }
            }
            return insuLation;
        }

        private Insulation GetInsulationMaster(SafeDataReader dr)
        {
            Insulation insulation = new Insulation();
            insulation.ProductionId = dr.GetInt64("ProductionOrderId");
            insulation.MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") };
            // insulation.WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") };
            insulation.Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") };
            // braiding.Quantity = dr.GetInt32("Qty");
            // insulation.TotalIdleTime = dr.GetString("TotalIdleTime");
            insulation.TotalProduction = dr.GetString("TotalProduction");
            // insulation.StartTime = dr.GetDateTime("StartTime");
            // insulation.EndTime = dr.GetDateTime("EndTime");

            return insulation;
        }
    }
}