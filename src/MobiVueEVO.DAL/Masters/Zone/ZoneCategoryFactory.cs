using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO.Masters.Zone;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL.Masters.Zone
{
    public class ZoneCategoryFactory
    {
        public void Delete(int ZoneCategoryId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(ZoneCategoryId > 0, "ZoneCategory id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_ZoneCategory";
                    cmd.Parameters.AddWithValue("@Id", ZoneCategoryId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteZoneCategory");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public ZoneCategory Fetch(int id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "ZoneCategory id is required for get ZoneCategory .");
            ZoneCategory ZoneCategory = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetZoneCategoryById"));
                    cmd.CommandText = "USP_ZoneCategory";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            ZoneCategory = GetZoneCategory(dr);
                        }
                    }
                }
            }

            return ZoneCategory;
        }

        public DataList<ZoneCategory, long> FetchZoneCategories(ZoneCategorySearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<ZoneCategory, long> ZoneCategories = new DataList<ZoneCategory, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_ZoneCategory";
                    cm.Parameters.Add(new SqlParameter("@Code", criteria.Code));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetZoneCategories"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (ZoneCategories.HeaderData == 0) ZoneCategories.HeaderData = dr.GetInt64("TotalRows");
                            ZoneCategory ZoneCategory = GetZoneCategory(dr);
                            ZoneCategories.ItemCollection.Add(ZoneCategory);
                        }
                    }
                }
            }
            return ZoneCategories;
        }

        private ZoneCategory GetZoneCategory(SafeDataReader dr)
        {
            ZoneCategory ZoneCategory = new ZoneCategory();
            ZoneCategory.Id = dr.GetInt32("ID");
            ZoneCategory.Code = dr.GetString("Code");
            ZoneCategory.Description = dr.GetString("Description");
            ZoneCategory.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            ZoneCategory.IsActive = dr.GetBoolean("IsActive");
            ZoneCategory.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            ZoneCategory.CreatedOn = dr.GetDateTime("CreatedOn");
            ZoneCategory.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            ZoneCategory.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return ZoneCategory;
        }

        public bool IsExists(ZoneCategory entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "ZoneCategory should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_ZoneCategory";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public ZoneCategory Insert(ZoneCategory entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "ZoneCategory should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Zone Category: {entity.Code} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_ZoneCategory";
                    cm.Parameters.AddWithValue("@Code", entity.Code);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertZoneCategory");
                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public ZoneCategory Update(ZoneCategory entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "ZoneCategory should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_ZoneCategory";
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Code", entity.Code);
                    cm.Parameters.AddWithValue("@Type", "UpdateZoneCategory");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}