using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class MatLocMappingFactory
    {
        #region Delete

        public void Delete(int BayId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "MatLocMapping id is required for delete.");

            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                Delete(BayId, deletedBy, cn);
            }
        }

        private void Delete(int BayId, long deletedBy, SqlConnection cn)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "MatLocMapping id is required for delete.");
            CodeContract.Required<ArgumentException>(cn != null, "Database connection should not be null.");
            using (var cm = cn.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "USP_MaterialLocationMapping";
                cm.Parameters.AddWithValue("@Id", BayId);
                cm.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                cm.Parameters.AddWithValue("@Type", "DeleteMaterialLocationMappingByID");
                cm.ExecuteNonQuery();
            }
        }

        #endregion Delete

        #region Fetch



        public MatLocMapping Fetch(int BayId)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "MatLocMapping id is required for get MatLocMapping.");

            MatLocMapping _Bay = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_MaterialLocationMapping";
                cmd.Parameters.Add(new SqlParameter("@Id", BayId));
                cmd.Parameters.Add(new SqlParameter("@Type", "GetMaterialLocationMappingById"));

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

        private MatLocMapping GetBay(SafeDataReader dr)
        {
            MatLocMapping MatLocMapping = new MatLocMapping();
            MatLocMapping.BayId = dr.GetInt64("ID");
            //MatLocMapping.userId = new KeyValue<long, string>() { Key = dr.GetInt64("UserId"), Value = dr.GetString("MatLocMappingName") };
            MatLocMapping.MaterialId = new KeyValue<int, string>(dr.GetInt32("MaterialId"), dr.GetString("MaterialName"));
            MatLocMapping.LocationId = new KeyValue<int, string>(dr.GetInt32("LocationId"), dr.GetString("LocationCode"));
            //MatLocMapping.Code = dr.GetString("BayCode");
            //MatLocMapping.Description = dr.GetString("Description");
            MatLocMapping.IsActive = dr.GetBoolean("IsActive");
            //MatLocMapping.BayLocationId = dr.GetInt16("BayLocationId");
            MatLocMapping.Site = new KeyValue<int, string>(dr.GetInt32("PlantId"), dr.GetString("PlantCode"));
            MatLocMapping.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            MatLocMapping.CreatedDate = dr.GetDateTime("CreatedOn");
            MatLocMapping.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            MatLocMapping.UpdatedDate = dr.GetDateTime("ModifiedOn");
            MatLocMapping.MarkAsOld();
            return MatLocMapping;
        }

        public DataList<MatLocMapping, long> FetchAll(MatLocMappingSearchCriteria criteria)
        {
            DataList<MatLocMapping, long> Bays = new DataList<MatLocMapping, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandTimeout = 0;
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MaterialLocationMapping";
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    //cm.Parameters.Add(new SqlParameter("@BayCode", criteria.BayCode.IsNotNullOrWhiteSpace() ? criteria.BayCode : ""));                    
                    cm.Parameters.Add(new SqlParameter("@Type", "GetLocationMapping"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (Bays.HeaderData == 0) Bays.HeaderData = dr.GetInt64("TotalRows");
                            MatLocMapping MatLocMapping = GetBay(dr);
                            Bays.ItemCollection.Add(MatLocMapping);
                        }
                    }
                }
            }
            return Bays;
        }

        #endregion Fetch

        #region Insert

        public MatLocMapping Insert(MatLocMapping entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "MatLocMapping should not be null.");
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                //if (IsExists(entity, cn)) throw new Exception($"MatLocMapping: {entity.Code} already exists.");

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "USP_MaterialLocationMapping";
                        //cm.Parameters.AddWithValue("@BayCode", entity.Code.IsNotNull() ? entity.Code : "");
                        cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                        //cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        //cm.Parameters.AddWithValue("@UserID", entity.userId.IsNotNull() ? entity.userId.Key : 0);
                        cm.Parameters.AddWithValue("@MaterialId", entity.MaterialId.IsNotNull() ? entity.MaterialId.Key : 0);
                        cm.Parameters.AddWithValue("@LocationId", entity.LocationId.IsNotNull() ? entity.LocationId.Key : 0);
                        //cm.Parameters.AddWithValue("@BayLocationId", entity.BayLocationId);                        
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "InsertMaterialLocationMapping");

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

        public MatLocMapping Update(MatLocMapping entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "MatLocMapping should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                //if (IsExists(entity, cn)) throw new Exception($"MatLocMapping: {entity.Code} already exists.");
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "USP_MaterialLocationMapping";
                        //cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        //cm.Parameters.AddWithValue("@UserID", entity.userId.IsNotNull() ? entity.userId.Key : 0);
                        cm.Parameters.AddWithValue("@MaterialId", entity.MaterialId.IsNotNull() ? entity.MaterialId.Key : 0);
                        cm.Parameters.AddWithValue("@LocationId", entity.LocationId.IsNotNull() ? entity.LocationId.Key : 0);
                        //cm.Parameters.AddWithValue("@BayLocationId", entity.BayLocationId);                        
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Id", entity.BayId);
                        cm.Parameters.AddWithValue("@Type", "UpdateLocationMapping");

                        cm.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(MatLocMapping item, SqlConnection con)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BayCode", item.Code);
                cmd.Parameters.AddWithValue("@Id", item.BayId);
                cmd.Parameters.AddWithValue("@PlantId", item.Site.IsNotNull() ? item.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_MaterialLocationMapping";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        #endregion Update



        public List<KeyValue<int, string>> FetchUsers(int siteId)
        {
            List<KeyValue<int, string>> Zones = new List<KeyValue<int, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MaterialLocationMapping";
                    cm.Parameters.Add(new SqlParameter("@PlantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetMaterials"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Zones.Add(new KeyValue<int, string>() { Key = dr.GetInt32("Id"), Value = dr.GetString("MaterialCode") });
                        }
                    }
                }
            }
            return Zones;
        }
        public List<KeyValue<int, string>> FetchZoneCategories(int siteId)
        {
            List<KeyValue<int, string>> Zones = new List<KeyValue<int, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MaterialLocationMapping";
                    cm.Parameters.Add(new SqlParameter("@PlantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetLocations"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Zones.Add(new KeyValue<int, string>() { Key = dr.GetInt32("Id"), Value = dr.GetString("LocationCode") });
                        }
                    }
                }
            }
            return Zones;
        }

    }
}