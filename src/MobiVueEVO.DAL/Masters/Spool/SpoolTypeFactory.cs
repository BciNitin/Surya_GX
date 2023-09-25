using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class SpoolTypeFactory
    {
        public void Delete(int id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "SpoolType id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteSpoolType");
                    cmd.CommandText = "USP_SpoolType";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataList<SpoolType, long> FetchAll(SpoolTypeSearchCriteria criteria)
        {
            DataList<SpoolType, long> SpoolTypes = new DataList<SpoolType, long>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SpoolType";
                    cmd.Parameters.AddWithValue("@PlantId", criteria.SiteId);
                    cmd.Parameters.AddWithValue("@Type", "GetSpoolTypes");
                    //cmd.Parameters.AddWithValue("@AttributeGroup", criteria.SpoolTypeCode);
                    cmd.Parameters.AddWithValue("@Code", criteria.Code);
                    //if (criteria.SpoolTypeAttributeIds.HaveItems())
                    //{
                    //    cmd.Parameters.AddWithValue("@SpoolTypeAttriIDs", criteria.SpoolTypeAttributeIds.Join(",", c => c));
                    //}
                    cmd.Parameters.AddWithValue("@IsActive", criteria.Status);
                    cmd.Parameters.AddWithValue("@Page", criteria.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", criteria.PageSize);

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (SpoolTypes.HeaderData == 0) SpoolTypes.HeaderData = dr.GetInt64("TotalRows");
                            SpoolType SpoolType = GetSpoolType(dr);
                            SpoolTypes.ItemCollection.Add(SpoolType);
                        }
                    }
                }
            }
            return SpoolTypes;
        }

        private SpoolType GetSpoolType(SafeDataReader dr)
        {
            SpoolType SpoolType = new SpoolType();
            SpoolType.Id = dr.GetInt16("Id");
            SpoolType.Code = dr.GetString("Code");

            SpoolType.Description = dr.GetString("Description");
            SpoolType.Length = dr.GetDecimal("Length");
            SpoolType.Breadth = dr.GetDecimal("Width");
            SpoolType.Height = dr.GetDecimal("Height");
            SpoolType.Weight = dr.GetDecimal("Weight");
            SpoolType.IsActive = dr.GetBoolean("IsActive");
            SpoolType.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            SpoolType.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            SpoolType.CreatedOn = dr.GetDateTime("CreatedOn");
            SpoolType.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            SpoolType.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return SpoolType;
        }

        public SpoolType Fetch(int id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "SpoolType id is required for get SpoolType Id.");
            SpoolType SpoolType = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetSpoolTypeById"));
                    cmd.CommandText = "USP_SpoolType";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            SpoolType = GetSpoolType(dr);
                        }
                    }
                }
            }

            return SpoolType;
        }

        public SpoolType Insert(SpoolType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "SpoolType Id Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Spool Type: {entity.Code} already exists.");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolType";
                    cm.Parameters.AddWithValue("@Code", entity.Code == null ? "" : entity.Code);
                    cm.Parameters.AddWithValue("@Length", entity.Length);
                    cm.Parameters.AddWithValue("@Width", entity.Breadth);
                    cm.Parameters.AddWithValue("@Height", entity.Height);
                    cm.Parameters.AddWithValue("@Weight", entity.Weight);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertSpoolType");

                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(SpoolType entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "SpoolType Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_SpoolType";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public List<KeyValue<short, string>> FetchSpoolTypeNames(int siteId)
        {
            List<KeyValue<short, string>> SpoolTypeTypes = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolType";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetSpoolTypeNames"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            SpoolTypeTypes.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("Code") });
                        }
                    }
                }
            }
            return SpoolTypeTypes;
        }

        public SpoolType Update(SpoolType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "SpoolType Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolType";
                    cm.Parameters.AddWithValue("@Length", entity.Length);
                    cm.Parameters.AddWithValue("@Width", entity.Breadth);
                    cm.Parameters.AddWithValue("@Height", entity.Height);
                    cm.Parameters.AddWithValue("@Weight", entity.Weight);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "UpdateSpoolType");
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public List<KeyValue<short, string>> FetchUOMs(int siteId)
        {
            List<KeyValue<short, string>> SpoolTypeFormats = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolType";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetCapacityUOM"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            SpoolTypeFormats.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("CapacityUOM") });
                        }
                    }
                }
            }
            return SpoolTypeFormats;
        }
    }
}