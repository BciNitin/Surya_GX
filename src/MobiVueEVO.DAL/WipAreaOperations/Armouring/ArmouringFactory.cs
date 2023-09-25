using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class ArmouringFactory
    {
        public string SaveArmuring(Armouring armuring)
        {
            CodeContract.Required<ArgumentException>(armuring.IsNotNull(), "armuring is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", armuring.ProductionOrderId);
                    cmd.Parameters.AddWithValue("@ProductionOrderItemId", armuring.ProductionOrderItemId);  //PalletCode
                                                                                                            //  cmd.Parameters.AddWithValue("@MachineId", armuring.MachineId);
                                                                                                            // cmd.Parameters.AddWithValue("@MachineTypeId", armuring.MachineTypeId);

                    cmd.Parameters.AddWithValue("@Type", armuring.Type);
                    cmd.Parameters.AddWithValue("@Section", armuring.Section);
                    cmd.Parameters.AddWithValue("@ISDia", armuring.ISDia);

                    cmd.Parameters.AddWithValue("@Length", armuring.Length);
                    // cmd.Parameters.AddWithValue("@StartTime", armuring.StartTime);
                    // cmd.Parameters.AddWithValue("@EndTime", armuring.EndTime);
                    cmd.Parameters.AddWithValue("@InputSpoolId", armuring.InputSpoolId);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", armuring.OutputSpoolId);
                    cmd.Parameters.AddWithValue("@NoOfWires", armuring.NoOfWires);
                    cmd.Parameters.AddWithValue("@WireSize", armuring.WireSize);        //wireStripSize
                    cmd.Parameters.AddWithValue("@WaitingForG1Wire", armuring.WaitingForG1Wire);
                    cmd.Parameters.AddWithValue("@Loading", armuring.Loading);
                    cmd.Parameters.AddWithValue("@Unloading", armuring.Unloading);

                    cmd.Parameters.AddWithValue("@Welding", armuring.Welding);
                    cmd.Parameters.AddWithValue("@CableJoint", armuring.CableJoint);
                    cmd.Parameters.AddWithValue("@WaitForSpool", armuring.WaitingForSpool);
                    cmd.Parameters.AddWithValue("@WireCut", armuring.UnderMaintenance);

                    cmd.Parameters.AddWithValue("@WaitingForCable", armuring.WaitingForCable);
                    cmd.Parameters.AddWithValue("@LooseWinding", armuring.LooseWinding);
                    cmd.Parameters.AddWithValue("@NoLoad", armuring.NoLoad);
                    cmd.Parameters.AddWithValue("@Others", armuring.Others);
                    cmd.Parameters.AddWithValue("@UnderMaintainance", armuring.UnderMaintenance);

                    cmd.Parameters.AddWithValue("@CreatedBy", armuring.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", armuring.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "Armouring");

                    cmd.Parameters.AddWithValue("@TotalIdleTime", armuring.TotalIdleTime);
                    cmd.Parameters.AddWithValue("@TotalProduction", armuring.TotalProduction);
                    cmd.Parameters.AddWithValue("@GIScrap", armuring.GIScrap);
                    cmd.Parameters.AddWithValue("@ArticleNo", armuring.Articleno);

                    cmd.Parameters.AddWithValue("@Technician", armuring.Technician.Key);

                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Armouring, long> FetchArmouring(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Armouring, long> arMouring = new DataList<Armouring, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetArmouring"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Armouring armouring = GetArmouringMaster(dr);
                            arMouring.ItemCollection.Add(armouring);
                        }
                    }
                }
            }
            return arMouring;
        }

        private Armouring GetArmouringMaster(SafeDataReader dr)
        {
            Armouring arMouring = new Armouring();
            arMouring.ProductionOrderId = dr.GetInt64("ProductionOrderId");
            //arMouring.MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") };
            // arMouring.WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") };
            arMouring.Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") };
            arMouring.Articleno = dr.GetString("ArticleNo");
            arMouring.TotalIdleTime = dr.GetString("TotalIdleTime");
            arMouring.TotalProduction = dr.GetString("TotalProduction");
            // arMouring.StartTime = dr.GetDateTime("StartTime");
            // arMouring.EndTime = dr.GetDateTime("EndTime");

            return arMouring;
        }
    }
}