using MobiVUE;

using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class ZoneFactory
    {
        public void Delete(int id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "LAPPZone id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteZone");
                    cmd.CommandText = "USP_Zone";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataList<LAPPZone, long> FetchAll(ZoneSearchCriteria criteria)
        {
            DataList<LAPPZone, long> LAPPZones = new DataList<LAPPZone, long>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Zone";
                    cmd.Parameters.AddWithValue("@PlantId", criteria.SiteId);
                    cmd.Parameters.AddWithValue("@Type", "GetZones");
                    cmd.Parameters.AddWithValue("@ZoneCode", criteria.Code);
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
                            if (LAPPZones.HeaderData == 0) LAPPZones.HeaderData = dr.GetInt64("TotalRows");
                            LAPPZone LAPPZone = GetZone(dr);
                            LAPPZones.ItemCollection.Add(LAPPZone);
                        }
                    }
                }
            }
            return LAPPZones;
        }

        private LAPPZone GetZone(SafeDataReader dr)
        {
            LAPPZone LAPPZone = new LAPPZone();
            LAPPZone.Id = dr.GetInt32("Id");
            LAPPZone.ZoneCode = dr.GetString("ZoneCode");
            LAPPZone.ZoneCategoryId = new KeyValue<int, string>() { Key = dr.GetInt32("ZoneCategoryId"), Value = dr.GetString("ZoneCategoryName") };
            LAPPZone.Description = dr.GetString("Description");
            LAPPZone.Alias = dr.GetString("Alias");
            LAPPZone.MAC = dr.GetString("BLEMACAddress");
            LAPPZone.IPAddress = dr.GetString("IPAddress");
            LAPPZone.IsActive = dr.GetBoolean("IsActive");
            LAPPZone.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            LAPPZone.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            LAPPZone.CreatedOn = dr.GetDateTime("CreatedOn");
            LAPPZone.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            LAPPZone.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return LAPPZone;
        }

        public LAPPZone Fetch(int id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "LAPPZone id is required for get LAPPZone Id.");
            LAPPZone LAPPZone = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetZoneById"));
                    cmd.CommandText = "USP_Zone";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            LAPPZone = GetZone(dr);
                        }
                    }
                }
            }

            return LAPPZone;
        }

        public LAPPZone Insert(LAPPZone entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Zone Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Zone: {entity.ZoneCode} already exists.");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Zone";
                    cm.Parameters.AddWithValue("@ZoneCode", entity.ZoneCode == null ? "" : entity.ZoneCode);
                    cm.Parameters.AddWithValue("@ZoneCategoryId", entity.ZoneCategoryId.Key);
                    cm.Parameters.AddWithValue("@Alias", entity.Alias);
                    cm.Parameters.AddWithValue("@BLEMACAddress", entity.MAC);
                    cm.Parameters.AddWithValue("@IPAddress", entity.IPAddress);

                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertZone");

                    entity.Id = Convert.ToInt32(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(LAPPZone entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "LAPPZone Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ZoneCode", entity.ZoneCode);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_Zone";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public List<KeyValue<short, string>> FetchSpoolTypeTypes(int siteId)
        {
            List<KeyValue<short, string>> SpoolTypeTypes = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Zone";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetSpoolTypeTypesforBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            SpoolTypeTypes.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("SpoolTypeType") });
                        }
                    }
                }
            }
            return SpoolTypeTypes;
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
                    cm.CommandText = "USP_Zone";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetZoneCategoriesforBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Zones.Add(new KeyValue<int, string>() { Key = dr.GetInt32("Id"), Value = dr.GetString("Code") });
                        }
                    }
                }
            }
            return Zones;
        }

        public LAPPZone Update(LAPPZone entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "LAPPZone Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Zone";
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ZoneCategoryId", entity.ZoneCategoryId.Key);
                    cm.Parameters.AddWithValue("@BLEMACAddress", entity.MAC);
                    cm.Parameters.AddWithValue("@IPAddress", entity.IPAddress);
                    cm.Parameters.AddWithValue("@Alias", entity.Alias);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "UpdateZone");
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}