using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class PalletMergeFactory
    {
        public string SavePutaway(PutawayInfo putaway)
        {
            CodeContract.Required<ArgumentException>(putaway.IsNotNull(), "Quality status is mandatory.");
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
            CodeContract.Required<ArgumentException>(code.IsNotNullOrWhiteSpace(), "Location code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Putaway";
                    cmd.Parameters.AddWithValue("@LocationCode", code);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "Location");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public string SavePutawayPackingReceiving(PalletMerge putaway)
        {
            CodeContract.Required<ArgumentException>(putaway.IsNotNull(), "Pallet Details are mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_PalletMerging";
                    cmd.Parameters.AddWithValue("@PlantId", putaway.PlantId);
                    cmd.Parameters.AddWithValue("@PalletCode", putaway.PalletCode);
                    cmd.Parameters.AddWithValue("@PalletCodeToMerge", putaway.PalletToMergeCode);
                    cmd.Parameters.AddWithValue("@ModifiedBy", putaway.ModifiedBy);

                    //cmd.Parameters.AddWithValue(" @PalletCodeToMerge", (long)AbpSession.UserId);

                    //cmd.Parameters.AddWithValue("@MaterialCode", putaway.MaterialCode);
                    cmd.Parameters.AddWithValue("@Type", "MergePallet");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public string ValidateMaterial(string code, int plantId)
        {
            CodeContract.Required<ArgumentException>(code.IsNotNullOrWhiteSpace(), "Pallet Code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_PalletMerging";
                    cmd.Parameters.AddWithValue("@PalletCode", code);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "ValidatePallet");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }


        public List<PalletMergeReport> FetchInwardOrders(PalletMergeReportSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<PalletMergeReport> OrderDetails = new List<PalletMergeReport>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Reports";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHORDERS"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@MaterialCode", criteria.materialCode));
                    cm.Parameters.Add(new SqlParameter("@PalletCode", criteria.pallet1));
                    cm.Parameters.Add(new SqlParameter("@PalletCodeToMerge", criteria.pallet2));
                    cm.Parameters.Add(new SqlParameter("@ModifiedBy", criteria.userId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            OrderDetails.Add(GetGRN(dr));
                        }
                    }
                }
            }
            return OrderDetails;
        }


        private PalletMergeReport GetGRN(SafeDataReader dr)
        {
            PalletMergeReport InwardOrder = new PalletMergeReport();
            InwardOrder.Id = dr.GetInt64("Id");
            InwardOrder.frompallet = dr.GetString("PalletFrom");
            InwardOrder.topallet = dr.GetString("PalletToMerge");
            InwardOrder.Material = dr.GetString("MaterialCode");
            //InwardOrder.DocumentYear = dr.GetString("DocumentYear");
            //InwardOrder.DocumentDate = dr.GetDateTime("DocumentDate");
            //InwardOrder.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PLANTCODE") };
            //InwardOrder.GRNStatus = (GRNStatus)dr.GetInt32("Status");
            InwardOrder.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrder.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CREATEDBYNAME") };
            //InwardOrder.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("MODIFIEDBYNAME") };
            //InwardOrder.ModifiedOn = dr.GetDateTime("ModifiedOn");
            //InwardOrder.Items = GetGrnItemsByGRNId(InwardOrder.Id);
            return InwardOrder;
        }


        public List<KeyValue<long, string>> FetchUsers(int siteId)
        {
            List<KeyValue<long, string>> Zones = new List<KeyValue<long, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Reports";
                    cm.Parameters.Add(new SqlParameter("@PlantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetUsers"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Zones.Add(new KeyValue<long, string>() { Key = dr.GetInt64("Id"), Value = dr.GetString("UserName") });
                        }
                    }
                }
            }
            return Zones;
        }
    }
}