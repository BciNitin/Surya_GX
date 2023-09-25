using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class HardwareTypeFactory
    {
        public void Delete(short HardwareTypeId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(HardwareTypeId > 0, "Hardware Type id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_HardwareType";
                    cmd.Parameters.AddWithValue("@Id", HardwareTypeId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteHardwareType");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public HardwareType Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Hardware Type id is required for get HardwareType .");
            HardwareType HardwareType = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetHardwareTypeById"));
                    cmd.CommandText = "USP_HardwareType";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            HardwareType = GetHardwareType(dr);
                        }
                    }
                }
            }

            return HardwareType;
        }

        public DataList<HardwareType, long> FetchHardwareTypes(HardwareTypeSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<HardwareType, long> HardwareTypes = new DataList<HardwareType, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_HardwareType";
                    cm.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetHardwareTypes"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (HardwareTypes.HeaderData == 0) HardwareTypes.HeaderData = dr.GetInt64("TotalRows");
                            HardwareType HardwareType = GetHardwareType(dr);
                            HardwareTypes.ItemCollection.Add(HardwareType);
                        }
                    }
                }
            }
            return HardwareTypes;
        }

        public List<KeyValue<short, string>> FetchHardwareTypes(long siteId)
        {
            List<KeyValue<short, string>> HardwareTypes = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_HardwareType";
                    cm.Parameters.Add(new SqlParameter("@PlantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetHardwareTypeNames"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            HardwareTypes.Add(new KeyValue<short, string>() { Key = dr.GetInt16("id"), Value = dr.GetString("Name") });
                        }
                    }
                }
            }
            return HardwareTypes;
        }

        private HardwareType GetHardwareType(SafeDataReader dr)
        {
            HardwareType HardwareType = new HardwareType();
            HardwareType.Id = dr.GetInt16("ID");
            HardwareType.Name = dr.GetString("Name");
            HardwareType.Description = dr.GetString("Description");
            HardwareType.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            HardwareType.IsActive = dr.GetBoolean("IsActive");
            HardwareType.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            HardwareType.CreatedOn = dr.GetDateTime("CreatedOn");
            HardwareType.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            HardwareType.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return HardwareType;
        }

        public bool IsExists(HardwareType entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "HardwareType should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_HardwareType";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public HardwareType Insert(HardwareType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "HardwareType should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Hardware Type: {entity.Name} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_HardwareType";
                    cm.Parameters.AddWithValue("@Name", entity.Name);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertHardwareType");
                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public HardwareType Update(HardwareType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Conveyor Line should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Hardware Type: {entity.Name} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_HardwareType";
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);

                    cm.Parameters.AddWithValue("@Type", "UpdateHardwareType");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}