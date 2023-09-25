using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO.Masters.Location;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL.Masters.Location
{
    public class LocationV1Factory
    {
        public void Delete(int id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteLocation");
                    cmd.CommandText = "USP_Location";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataList<LocationV1, long> FetchAll(LocationV1SearchCriteria criteria)
        {
            DataList<LocationV1, long> LocationV1s = new DataList<LocationV1, long>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Location";
                    cmd.Parameters.AddWithValue("@PlantId", criteria.SiteId);
                    cmd.Parameters.AddWithValue("@Type", "GetLocations");
                    //cmd.Parameters.AddWithValue("@AttributeGroup", criteria.LocationCode);
                    cmd.Parameters.AddWithValue("@Code", criteria.LocationCode);
                    //if (criteria.LocationAttributeIds.HaveItems())
                    //{
                    //    cmd.Parameters.AddWithValue("@LocationAttriIDs", criteria.LocationAttributeIds.Join(",", c => c));
                    //}
                    cmd.Parameters.AddWithValue("@IsActive", criteria.Status);
                    cmd.Parameters.AddWithValue("@Page", criteria.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", criteria.PageSize);

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (LocationV1s.HeaderData == 0) LocationV1s.HeaderData = dr.GetInt64("TotalRows");
                            LocationV1 Location = GetLocation(dr);
                            LocationV1s.ItemCollection.Add(Location);
                        }
                    }
                }
            }
            return LocationV1s;
        }

        private LocationV1 GetLocation(SafeDataReader dr)
        {
            LocationV1 Location = new LocationV1();
            Location.Id = dr.GetInt32("Id");
            Location.LocationName = dr.GetString("LocationName");
            Location.LocationAttributeId1 = new KeyValue<short, string>() { Key = dr.GetInt16("LocationAttributeId1"), Value = dr.GetString("Attribute1") };
            Location.LocationAttributeId2 = new KeyValue<short, string>() { Key = dr.GetInt16("LocationAttributeId2"), Value = dr.GetString("Attribute2") };
            Location.LocationAttributeId3 = new KeyValue<short, string>() { Key = dr.GetInt16("LocationAttributeId3"), Value = dr.GetString("Attribute3") };
            Location.Zone = new KeyValue<int, string>() { Key = dr.GetInt32("ZoneId"), Value = dr.GetString("ZoneName") };
            //Location.ZoneId = dr.GetInt32("ZoneId");
            Location.LocationCode = dr.GetString("LocationCode"); /*     IsDeleted   DeletedBy DeletedOn   CreatedBy CreatedOn   ModifiedBy ModifiedOn  CreatedByName ModifiedByName  PlantCode TotalRows*/
            //Location.LocationTypeId = dr.GetInt32("LocationTypeId");
            Location.LocationType = new KeyValue<short, string>() { Key = dr.GetInt16("LocationTypeId"), Value = dr.GetString("LocationTypeName") };
            Location.SortSequence = dr.GetInt32("SortSequence");
            Location.Length = dr.GetDecimal("Length");
            Location.Breadth = dr.GetDecimal("Breadth");
            Location.Height = dr.GetDecimal("Height");
            Location.Weight = dr.GetDecimal("Weight");
            Location.LocationCapacity = dr.GetInt32("LocationCapacity");
            Location.LocationFormatId = new KeyValue<short, string>() { Key = dr.GetInt16("LocationFormatId"), Value = dr.GetString("LocationFormatCode") };
            //Location.LocationFormatId = dr.GetInt16("LocationFormatId");
            Location.RackPosition = dr.GetInt32("RackPosition");
            //Location.CapacityUOMId = dr.GetInt32("CapacityUOMId");
            Location.CapacityUOMId = new KeyValue<short, string>() { Key = dr.GetInt16("CapacityUOMId"), Value = dr.GetString("CapacityUOM") };
            Location.MaterialCode = dr.GetString("MaterialCode");
            Location.IsAllowMultipleItems = dr.GetBoolean("IsAllowMultipleItems");
            //Location.Description = dr.GetString("Description");
            Location.IsActive = dr.GetBoolean("IsActive");
            Location.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            Location.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Location.CreatedOn = dr.GetDateTime("CreatedOn");
            Location.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            Location.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return Location;
        }

        public LocationV1 Fetch(int id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location id is required for get Location Id.");
            LocationV1 Location = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetLocationById"));
                    cmd.CommandText = "USP_Location";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            Location = GetLocation(dr);
                        }
                    }
                }
            }

            return Location;
        }

        public LocationV1 Insert(LocationV1 entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Id Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Location Code: {entity.LocationCode} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.AddWithValue("@Code", entity.LocationCode == null ? "" : entity.LocationCode);
                    cm.Parameters.AddWithValue("@LocationName", entity.LocationName.IsNotNullOrWhiteSpace() ? entity.LocationName : "");
                    cm.Parameters.AddWithValue("@LocationAttributeId1", entity.LocationAttributeId1.Key);
                    cm.Parameters.AddWithValue("@LocationAttributeId2", entity.LocationAttributeId2.Key);
                    cm.Parameters.AddWithValue("@LocationAttributeId3", entity.LocationAttributeId3.Key);
                    cm.Parameters.AddWithValue("@ZoneId", entity.Zone.Key);
                    cm.Parameters.AddWithValue("@LocationTypeId", entity.LocationType.Key);
                    cm.Parameters.AddWithValue("@SortSequence", entity.SortSequence);
                    cm.Parameters.AddWithValue("@Length", entity.Length);
                    cm.Parameters.AddWithValue("@Breadth", entity.Breadth);
                    cm.Parameters.AddWithValue("@Height", entity.Height);
                    cm.Parameters.AddWithValue("@LocationCapacity", entity.LocationCapacity);
                    cm.Parameters.AddWithValue("@LocationFormatId", entity.LocationFormatId.Key);
                    cm.Parameters.AddWithValue("@RackPosition", entity.RackPosition);
                    cm.Parameters.AddWithValue("@CapacityUOMId", entity.CapacityUOMId.Key);
                    cm.Parameters.AddWithValue("@IsAllowMultipleItems", entity.IsAllowMultipleItems);
                    cm.Parameters.AddWithValue("@MaterialCode", entity.MaterialCode);
                    cm.Parameters.AddWithValue("@Weight", entity.Weight);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    //cm.Parameters.AddWithValue("@AttributeGroup", entity.AttrGroup == 0 ? 0 : entity.AttrGroup);
                    //cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertLocation");

                    entity.Id = Convert.ToInt32(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(LocationV1 entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Code", entity.LocationCode);
                //cmd.Parameters.AddWithValue("@AttributeGroup", entity.AttrGroup);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_Location";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public List<KeyValue<short, string>> FetchLocationTypes(int siteId)
        {
            List<KeyValue<short, string>> LocationTypes = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetLocationTypesforBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            LocationTypes.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("LocationType") });
                        }
                    }
                }
            }
            return LocationTypes;
        }

        public List<KeyValue<int, string>> FetchZones(int siteId)
        {
            List<KeyValue<int, string>> Zones = new List<KeyValue<int, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetZonesforBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Zones.Add(new KeyValue<int, string>() { Key = dr.GetInt32("Id"), Value = dr.GetString("ZoneCode") });
                        }
                    }
                }
            }
            return Zones;
        }

        public List<KeyValue<short, string>> FetchAttributes1(int siteId)
        {
            List<KeyValue<short, string>> Attributes1 = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetAttributes1forBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Attributes1.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("Attribute1") });
                        }
                    }
                }
            }
            return Attributes1;
        }

        public List<KeyValue<short, string>> FetchAttributes2(int siteId)
        {
            List<KeyValue<short, string>> Attributes2 = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetAttributes2forBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Attributes2.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("Attribute2") });
                        }
                    }
                }
            }
            return Attributes2;
        }

        public List<KeyValue<short, string>> FetchAttributes3(int siteId)
        {
            List<KeyValue<short, string>> Attributes3 = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetAttributes3forBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Attributes3.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("Attribute3") });
                        }
                    }
                }
            }
            return Attributes3;
        }

        public LocationV1 Update(LocationV1 entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.AddWithValue("@LocationName", entity.LocationName.IsNotNullOrWhiteSpace() ? entity.LocationName : "");
                    cm.Parameters.AddWithValue("@LocationAttributeId1", entity.LocationAttributeId1.Key);
                    cm.Parameters.AddWithValue("@LocationAttributeId2", entity.LocationAttributeId2.Key);
                    cm.Parameters.AddWithValue("@LocationAttributeId3", entity.LocationAttributeId3.Key);
                    cm.Parameters.AddWithValue("@ZoneId", entity.Zone.Key);
                    cm.Parameters.AddWithValue("@LocationTypeId", entity.LocationType.Key);
                    cm.Parameters.AddWithValue("@SortSequence", entity.SortSequence);
                    cm.Parameters.AddWithValue("@Length", entity.Length);
                    cm.Parameters.AddWithValue("@Breadth", entity.Breadth);
                    cm.Parameters.AddWithValue("@Height", entity.Height);
                    cm.Parameters.AddWithValue("@LocationCapacity", entity.LocationCapacity);
                    cm.Parameters.AddWithValue("@LocationFormatId", entity.LocationFormatId.Key);
                    cm.Parameters.AddWithValue("@RackPosition", entity.RackPosition);
                    cm.Parameters.AddWithValue("@CapacityUOMId", entity.CapacityUOMId.Key);
                    cm.Parameters.AddWithValue("@IsAllowMultipleItems", entity.IsAllowMultipleItems);
                    cm.Parameters.AddWithValue("@MaterialCode", entity.MaterialCode);
                    cm.Parameters.AddWithValue("@Weight", entity.Weight);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    //cm.Parameters.AddWithValue("@AttributeGroup", entity.AttrGroup == 0 ? 0 : entity.AttrGroup);
                    //cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    //cm.Parameters.AddWithValue("@AttributeGroup", entity.AttrGroup == 0 ? 0 : entity.AttrGroup);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    //cm.Parameters.AddWithValue("@Id", entity.LocationAttrId);
                    cm.Parameters.AddWithValue("@Type", "UpdateLocation");
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public List<KeyValue<short, string>> FetchLocationFormats(int siteId)
        {
            List<KeyValue<short, string>> LocationFormats = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetLocationFormatsforBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            LocationFormats.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("LocationFormatCode") });
                        }
                    }
                }
            }
            return LocationFormats;
        }

        public List<KeyValue<short, string>> FetchUOMs(int siteId)
        {
            List<KeyValue<short, string>> LocationFormats = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Location";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetCapacityUOM"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            LocationFormats.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("CapacityUOM") });
                        }
                    }
                }
            }
            return LocationFormats;
        }
    }
}