using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class BayFactory
    {
        #region Delete

        public void Delete(int BayId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "Bay id is required for delete.");

            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                Delete(BayId, deletedBy, cn);
            }
        }

        private void Delete(int BayId, long deletedBy, SqlConnection cn)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "Bay id is required for delete.");
            CodeContract.Required<ArgumentException>(cn != null, "Database connection should not be null.");
            using (var cm = cn.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "Usp_BayMaster";
                cm.Parameters.AddWithValue("@Id", BayId);
                cm.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                cm.Parameters.AddWithValue("@Type", "DeleteBay");
                cm.ExecuteNonQuery();
            }
        }

        #endregion Delete

        #region Fetch



        public Bay Fetch(int BayId)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "Bay id is required for get Bay.");

            Bay _Bay = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_BayMaster";
                cmd.Parameters.Add(new SqlParameter("@Id", BayId));
                cmd.Parameters.Add(new SqlParameter("@Type", "GetBayById"));

                using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                {
                    if (dr.Read())
                    {
                        _Bay = GetBay(dr);
                    }
                }
            }
            return _Bay;
        }

        private Bay GetBay(SafeDataReader dr)
        {
            Bay Bay = new Bay();
            Bay.BayId = dr.GetInt32("ID");
            Bay.Code = dr.GetString("BayCode");
            Bay.Description = dr.GetString("Description");
            Bay.IsActive = dr.GetBoolean("IsActive");
            Bay.BayLocationId = dr.GetInt16("BayLocationId");
            Bay.Site = new KeyValue<int, string>(dr.GetInt32("PlantId"), dr.GetString("PlantCode"));
            Bay.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Bay.CreatedDate = dr.GetDateTime("CreatedOn");
            Bay.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("UpdatedByName") };
            Bay.UpdatedDate = dr.GetDateTime("ModifiedOn");
            Bay.MarkAsOld();
            return Bay;
        }

        public DataList<Bay, long> FetchAll(BaySearchCriteria criteria)
        {
            DataList<Bay, long> Bays = new DataList<Bay, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandTimeout = 0;
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_BayMaster";
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@BayCode", criteria.BayCode.IsNotNullOrWhiteSpace() ? criteria.BayCode : ""));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetBays"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (Bays.HeaderData == 0) Bays.HeaderData = dr.GetInt64("TotalRows");
                            Bay Bay = GetBay(dr);
                            Bays.ItemCollection.Add(Bay);
                        }
                    }
                }
            }
            return Bays;
        }

        #endregion Fetch

        #region Insert

        public Bay Insert(Bay entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Bay should not be null.");
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                if (IsExists(entity, cn)) throw new Exception($"Bay: {entity.Code} already exists.");

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_BayMaster";
                        cm.Parameters.AddWithValue("@BayCode", entity.Code.IsNotNull() ? entity.Code : "");
                        cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                        cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        cm.Parameters.AddWithValue("@BayLocationId", entity.BayLocationId);
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "InsertBay");

                        entity.BayId = Convert.ToInt32(cm.ExecuteScalar());
                    }
                    transaction.Commit();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        #endregion Insert

        #region Update

        public Bay Update(Bay entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Bay should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Bay: {entity.Code} already exists.");
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_BayMaster";
                        cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        cm.Parameters.AddWithValue("@BayLocationId", entity.BayLocationId);
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Id", entity.BayId);
                        cm.Parameters.AddWithValue("@Type", "UpdateBay");

                        cm.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(Bay item, SqlConnection con)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BayCode", item.Code);
                cmd.Parameters.AddWithValue("@Id", item.BayId);
                cmd.Parameters.AddWithValue("@PlantId", item.Site.IsNotNull() ? item.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_BayMaster";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        #endregion Update
    }
}