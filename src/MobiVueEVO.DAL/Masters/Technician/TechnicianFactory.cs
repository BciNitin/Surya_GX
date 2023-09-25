using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class TechnicianFactory
    {
        #region Delete

        public void Delete(int BayId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "Technician id is required for delete.");

            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                Delete(BayId, deletedBy, cn);
            }
        }

        private void Delete(int BayId, long deletedBy, SqlConnection cn)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "Technician id is required for delete.");
            CodeContract.Required<ArgumentException>(cn != null, "Database connection should not be null.");
            using (var cm = cn.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "USP_TechnicianMaster";
                cm.Parameters.AddWithValue("@Id", BayId);
                cm.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                cm.Parameters.AddWithValue("@Type", "DeleteTechnicianByID");
                cm.ExecuteNonQuery();
            }
        }

        #endregion Delete

        #region Fetch



        public Technician Fetch(int BayId)
        {
            CodeContract.Required<ArgumentException>(BayId > 0, "Technician id is required for get Technician.");

            Technician _Bay = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_TechnicianMaster";
                cmd.Parameters.Add(new SqlParameter("@Id", BayId));
                cmd.Parameters.Add(new SqlParameter("@Type", "GetTechnicianById"));

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

        private Technician GetBay(SafeDataReader dr)
        {
            Technician Technician = new Technician();
            Technician.BayId = dr.GetInt32("ID");
            Technician.userId = new KeyValue<long, string>() { Key = dr.GetInt64("UserId"), Value = dr.GetString("TechnicianName") };
            Technician.processId = new KeyValue<int, string>(dr.GetInt32("ProcessId"), dr.GetString("ProcessName"));
            //Technician.Code = dr.GetString("BayCode");
            //Technician.Description = dr.GetString("Description");
            Technician.IsActive = dr.GetBoolean("IsActive");
            //Technician.BayLocationId = dr.GetInt16("BayLocationId");
            Technician.Site = new KeyValue<int, string>(dr.GetInt32("PlantId"), dr.GetString("PlantCode"));
            Technician.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Technician.CreatedDate = dr.GetDateTime("CreatedOn");
            Technician.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            Technician.UpdatedDate = dr.GetDateTime("ModifiedOn");
            Technician.MarkAsOld();
            return Technician;
        }

        public DataList<Technician, long> FetchAll(TechnicianSearchCriteria criteria)
        {
            DataList<Technician, long> Bays = new DataList<Technician, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandTimeout = 0;
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_TechnicianMaster";
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    //cm.Parameters.Add(new SqlParameter("@BayCode", criteria.BayCode.IsNotNullOrWhiteSpace() ? criteria.BayCode : ""));                    
                    cm.Parameters.Add(new SqlParameter("@Type", "GetTechnician"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (Bays.HeaderData == 0) Bays.HeaderData = dr.GetInt64("TotalRows");
                            Technician Technician = GetBay(dr);
                            Bays.ItemCollection.Add(Technician);
                        }
                    }
                }
            }
            return Bays;
        }

        #endregion Fetch

        #region Insert

        public Technician Insert(Technician entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Technician should not be null.");
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                //if (IsExists(entity, cn)) throw new Exception($"Technician: {entity.Code} already exists.");

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "USP_TechnicianMaster";
                        //cm.Parameters.AddWithValue("@BayCode", entity.Code.IsNotNull() ? entity.Code : "");
                        cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                        //cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        cm.Parameters.AddWithValue("@UserID", entity.userId.IsNotNull() ? entity.userId.Key : 0);
                        cm.Parameters.AddWithValue("@ProcessId", entity.processId.IsNotNull() ? entity.processId.Key : 0);
                        //cm.Parameters.AddWithValue("@BayLocationId", entity.BayLocationId);                        
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "InsertTechnician");

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

        public Technician Update(Technician entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Technician should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                //if (IsExists(entity, cn)) throw new Exception($"Technician: {entity.Code} already exists.");
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "USP_TechnicianMaster";
                        //cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        cm.Parameters.AddWithValue("@UserID", entity.userId.IsNotNull() ? entity.userId.Key : 0);
                        cm.Parameters.AddWithValue("@ProcessId", entity.processId.IsNotNull() ? entity.processId.Key : 0);
                        //cm.Parameters.AddWithValue("@BayLocationId", entity.BayLocationId);                        
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Id", entity.BayId);
                        cm.Parameters.AddWithValue("@Type", "UpdateTechnician");

                        cm.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(Technician item, SqlConnection con)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BayCode", item.Code);
                cmd.Parameters.AddWithValue("@Id", item.BayId);
                cmd.Parameters.AddWithValue("@PlantId", item.Site.IsNotNull() ? item.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_TechnicianMaster";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        #endregion Update



        public List<KeyValue<long, string>> FetchUsers(int siteId)
        {
            List<KeyValue<long, string>> Zones = new List<KeyValue<long, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_TechnicianMaster";
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
        public List<KeyValue<int, string>> FetchZoneCategories(int siteId)
        {
            List<KeyValue<int, string>> Zones = new List<KeyValue<int, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_TechnicianMaster";
                    cm.Parameters.Add(new SqlParameter("@PlantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetZoneCategoriesforBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Zones.Add(new KeyValue<int, string>() { Key = dr.GetInt32("Id"), Value = dr.GetString("ProcessName") });
                        }
                    }
                }
            }
            return Zones;
        }

    }
}