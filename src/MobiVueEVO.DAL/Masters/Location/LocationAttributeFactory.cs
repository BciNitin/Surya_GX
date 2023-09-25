using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace MobiVueEVO.DAL
{
    public class LocationAttributeFactory
    {
        public void Delete(short id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location Attribute id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteLocationAttribute");
                    cmd.CommandText = "USP_LocationAttribute";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataList<LocationAttribute, long> FetchAll(LocationAttributeSearchCriteria criteria)
        {
            DataList<LocationAttribute, long> LocationAttri = new DataList<LocationAttribute, long>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_LocationAttribute";
                    cmd.Parameters.AddWithValue("@PlantId", criteria.SiteId);
                    cmd.Parameters.AddWithValue("@Type", "LocationAttributes");
                    cmd.Parameters.AddWithValue("@AttributeGroup", criteria.LocationAttributeGroup);
                    cmd.Parameters.AddWithValue("@Code", criteria.Code);
                    if (criteria.LocationAttributeIds.HaveItems())
                    {
                        cmd.Parameters.AddWithValue("@LocationAttriIDs", criteria.LocationAttributeIds.Join(",", c => c));
                    }
                    cmd.Parameters.AddWithValue("@IsActive", criteria.Status);
                    cmd.Parameters.AddWithValue("@Page", criteria.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", criteria.PageSize);

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (LocationAttri.HeaderData == 0) LocationAttri.HeaderData = dr.GetInt64("TotalRows");
                            LocationAttribute LocationAtt = GetLocationAttri(dr);
                            LocationAttri.ItemCollection.Add(LocationAtt);
                        }
                    }
                }
            }
            return LocationAttri;
        }

        private LocationAttribute GetLocationAttri(SafeDataReader dr)
        {
            LocationAttribute Location = new LocationAttribute();
            Location.LocationAttrId = dr.GetInt16("Id");
            Location.AttrGroup = (LocationAttributeGroup)dr.GetInt32("AttributeGroup");
            Location.Code = dr.GetString("Code");
            Location.Description = dr.GetString("Description");
            Location.IsActive = dr.GetBoolean("IsActive");
            Location.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            Location.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Location.CreatedOn = dr.GetDateTime("CreatedOn");
            Location.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            Location.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return Location;
        }

        public LocationAttribute Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location Attribute id is required for get Location Attribute.");
            LocationAttribute LocationAttri = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetLocationAttributeById"));
                    cmd.CommandText = "USP_LocationAttribute";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            LocationAttri = GetLocationAttri(dr);
                        }
                    }
                }
            }

            return LocationAttri;
        }

        public LocationAttribute Insert(LocationAttribute entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Attribute Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Location Attribute- {entity.AttrGroup.DisplayName()}: {entity.Code} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_LocationAttribute";
                    cm.Parameters.AddWithValue("@Code", entity.Code == null ? "" : entity.Code);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@AttributeGroup", entity.AttrGroup == 0 ? 0 : entity.AttrGroup);
                    cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertLocationAttribute");

                    entity.LocationAttrId = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(LocationAttribute entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "LocationAttr Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@AttributeGroup", entity.AttrGroup);
                cmd.Parameters.AddWithValue("@Id", entity.LocationAttrId);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_LocationAttribute";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public LocationAttribute Update(LocationAttribute entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Attribute should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Location Attribute- {entity.AttrGroup.DisplayName()}: {entity.Code} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_LocationAttribute";
                    cm.Parameters.AddWithValue("@Description", entity.Description == null ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@AttributeGroup", entity.AttrGroup == 0 ? 0 : entity.AttrGroup);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Id", entity.LocationAttrId);
                    cm.Parameters.AddWithValue("@Type", "UpdateLocationAttribute");
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}