using MobiVUE;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class PutawayFactory
    {
        public string SavePutaway(PutawayInfo putaway)
        {
            CodeContract.Required<ArgumentException>(putaway.IsNotNull(), "Putaway is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Putaway";
                    cmd.Parameters.AddWithValue("@PlantId", putaway.PlantId);
                    cmd.Parameters.AddWithValue("@PalletCode", putaway.PalletCode);
                    cmd.Parameters.AddWithValue("@LocationCode", putaway.LocationCode);
                    cmd.Parameters.AddWithValue("@Type", "Save");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public string ValidateLocation(string code, int plantId)
        {
            CodeContract.Required<ArgumentException>(code.IsNotNullOrWhiteSpace(), "Pallet code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Putaway";
                    cmd.Parameters.AddWithValue("@PalletCode", code);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "PALLET");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public string SavePutawayPackingReceiving(PutawayPackingInfo putaway)
        {
            CodeContract.Required<ArgumentException>(putaway.IsNotNull(), "Quality status is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_PutAwayPackingReceiving";
                    cmd.Parameters.AddWithValue("@PlantId", putaway.PlantId);
                    cmd.Parameters.AddWithValue("@PalletCode", putaway.PalletCode);
                    cmd.Parameters.AddWithValue("@MaterialCode", putaway.MaterialCode);
                    cmd.Parameters.AddWithValue("@Type", "Save");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public string ValidateMaterial(string code, int plantId)
        {
            CodeContract.Required<ArgumentException>(code.IsNotNullOrWhiteSpace(), "Material Code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_PutAwayPackingReceiving";
                    cmd.Parameters.AddWithValue("@MaterialCode", code);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "Material");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }
    }
}