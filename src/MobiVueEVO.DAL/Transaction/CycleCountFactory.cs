using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class CycleCountFactory
    {
        public string SaveCycleCount(CycleCount cyclecount)
        {
            CodeContract.Required<ArgumentException>(cyclecount.IsNotNull(), "Cycle Count is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_CycleCount";
                    cmd.Parameters.AddWithValue("@CycleCountAdviceId", cyclecount.CycleCountAdviceId);
                    cmd.Parameters.AddWithValue("@PalletCode", cyclecount.PalletCode);  //PalletCode
                    cmd.Parameters.AddWithValue("@PlantId", cyclecount.PlantId);
                    cmd.Parameters.AddWithValue("@BarCode", cyclecount.BarcodeNumber);
                    cmd.Parameters.AddWithValue("@LocationCode", cyclecount.LocationCode);
                    cmd.Parameters.AddWithValue("@ModifiedBy", cyclecount.CreatedBy);
                    cmd.Parameters.AddWithValue("@Remarks", cyclecount.Remarks);
                    cmd.Parameters.AddWithValue("@Type", "InsertCycleCount");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public CycleCount GetAllCycleCount(long cycleCountAdviceId)
        {
            CodeContract.Required<ArgumentException>(cycleCountAdviceId > 0, "CycleCount Advice Id is required to get items");
            CycleCount items = null;
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_CycleCount";
                    cm.Parameters.Add(new SqlParameter("@CycleCountAdviceId", cycleCountAdviceId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetAll"));

                    using (SafeDataReader read = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (read.Read())
                        {
                            items = Getdetails(read);
                        }
                    }
                }
            }
            return items;
        }

        private CycleCount Getdetails(SafeDataReader dr)
        {
            CycleCount cyclecount = new CycleCount();
            cyclecount.CycleCountId = dr.GetInt64("@CycleCountId");
            cyclecount.CycleCountAdviceId = new KeyValue<long, string>() { Key = dr.GetInt64("CycleCountAdviceId"), Value = dr.GetString("CycleCountAdviceId") };
            //  cyclecount.PalletId = new KeyValue<long, string>() { Key = dr.GetInt64("PalletId"), Value = dr.GetString("PalletCode") };
            // cyclecount.MaterialId = new KeyValue<long, string>() { Key = dr.GetInt64("MaterialId"), Value = dr.GetString("MaterialCode") };
            cyclecount.Quantity = dr.GetInt32("Quantity");
            // cyclecount.InventoryId = dr.GetInt64("InventoryId");
            cyclecount.BarcodeNumber = dr.GetString("BarcodeNumber");
            cyclecount.PalletCode = dr.GetString("PalletCode");
            cyclecount.CreatedOn = dr.GetDateTime("CreatedOn");
            cyclecount.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            cyclecount.ModifiedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            cyclecount.ModifiedOn = dr.GetDateTime("ModifiedOn");

            return cyclecount;
        }

        public string ValidateLocation(string LocatiomnCode, int plantId, long CycleCountAdviceId)
        {
            CodeContract.Required<ArgumentException>(LocatiomnCode.IsNotNullOrWhiteSpace(), "Location Code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Putaway";
                    cmd.Parameters.AddWithValue("@LocationCode", LocatiomnCode);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@CycleCountAdviceId", CycleCountAdviceId);

                    cmd.Parameters.AddWithValue("@Type", "IsLocationExist");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public string ValidatePallet(string PalletCode, int plantId, long CycleCountAdviceId)
        {
            CodeContract.Required<ArgumentException>(PalletCode.IsNotNullOrWhiteSpace(), "Pallet Code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Putaway";
                    cmd.Parameters.AddWithValue("@PalletCode", PalletCode);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@CycleCountAdviceId", CycleCountAdviceId);

                    cmd.Parameters.AddWithValue("@Type", "IsPalletExist");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public List<KeyValue<short, string>> GetCycleCountAdviceNames(int platnid)
        {
            List<KeyValue<short, string>> CycleCountFormat = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_CycleCount";
                    cm.Parameters.Add(new SqlParameter("@PlantId", platnid));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetCycleCountAdvice"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            CycleCountFormat.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("CapacityUOM") });
                        }
                    }
                }
            }
            return CycleCountFormat;
        }
    }
}