using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class BunchingFactory
    {
        public string SaveBunching(Bunching bunching)
        {
            CodeContract.Required<ArgumentException>(bunching.IsNotNull(), "Bunching is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_WipAreaOperations";
                    cmd.Parameters.AddWithValue("@ProductionOrderId", bunching.ProductionId);
                    cmd.Parameters.AddWithValue("@ProductionOrderItemId", bunching.ProductionOrderItemId);  //PalletCode
                    cmd.Parameters.AddWithValue("@MachineId", bunching.MachineId.Key);
                    cmd.Parameters.AddWithValue("@WireTypeId", bunching.WireTypeId.Key);
                    cmd.Parameters.AddWithValue("@CrossSection", bunching.CrossSection);
                    // cmd.Parameters.AddWithValue("@ShiftId", bunching.ShiftId);
                    cmd.Parameters.AddWithValue("@Length", bunching.Lenght);

                    cmd.Parameters.AddWithValue("@WeldLength", bunching.WeldLength);
                    cmd.Parameters.AddWithValue("@WireDrawingBatchNo", bunching.WireDrawingBatchNo);  //PalletCode
                    cmd.Parameters.AddWithValue("@CR20", bunching.CR20);
                    cmd.Parameters.AddWithValue("@CRPercentage", bunching.CR);
                    cmd.Parameters.AddWithValue("@QCInspector", bunching.QCInspector);
                    cmd.Parameters.AddWithValue("@InputPalletId", bunching.InputPalletId);
                    cmd.Parameters.AddWithValue("@OutputSpoolId", bunching.OutputSpoolId);

                    cmd.Parameters.AddWithValue("@IdleTime", bunching.IdleReasonTime);
                    // cmd.Parameters.AddWithValue("@IdleReasonId", bunching.IdleReasonId.Key);
                    cmd.Parameters.AddWithValue("@CreatedBy", bunching.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@Remarks", bunching.Remarks);
                    cmd.Parameters.AddWithValue("@OperationType", "Bunching");

                    cmd.Parameters.AddWithValue("@ClassofConductor", bunching.ClassofConductor);
                    cmd.Parameters.AddWithValue("@RoomTemp", bunching.RoomTemp);
                    cmd.Parameters.AddWithValue("@CRRoomTemp", bunching.CRRoomTemp);
                    cmd.Parameters.AddWithValue("@CRAT20", bunching.CRAT20);

                    cmd.Parameters.AddWithValue("@TotalLength", bunching.CRRoomTemp);
                    cmd.Parameters.AddWithValue("@TotalNetWeight", bunching.CRAT20);
                    cmd.Parameters.AddWithValue("@Technician", bunching.Technician.Key);

                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public DataList<Bunching, long> FetchBunching(long techId)
        {
            CodeContract.Required<ArgumentException>(techId > 0, "Search criteria cannot be null");
            DataList<Bunching, long> bunching = new DataList<Bunching, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_WipAreaOperations";
                    cm.Parameters.AddWithValue("@Technician", techId);
                    cm.Parameters.Add(new SqlParameter("@OperationType", "GetBunching"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            // if (WireDrawing.HeaderData == 0) WireDrawing.HeaderData = dr.GetInt64("TotalRows");
                            Bunching wireDrawing = GetBunching(dr);
                            bunching.ItemCollection.Add(wireDrawing);
                        }
                    }
                }
            }
            return bunching;
        }

        private Bunching GetBunching(SafeDataReader dr)
        {
            Bunching bunching = new Bunching();
            bunching.ProductionId = dr.GetInt64("ProductionOrderId");
            bunching.MachineId = new KeyValue<short, string>() { Key = dr.GetInt16("MachineId"), Value = dr.GetString("Machine") };
            bunching.WireTypeId = new KeyValue<short, string>() { Key = dr.GetInt16("WireTypeId"), Value = dr.GetString("WireType") };
            bunching.Technician = new KeyValue<long, string>() { Key = dr.GetInt64("Technician"), Value = dr.GetString("TechnicianName") };
            bunching.StartTime = dr.GetDateTime("StartTime");
            bunching.EndTime = dr.GetDateTime("EndTime");
            // WireCategory.Quantity = dr.GetInt32("Qty");
            // WireCategory.TotalProduction = dr.GetString("TotalProduction");

            return bunching;
        }
    }
}