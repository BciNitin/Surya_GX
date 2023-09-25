using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class LocationTypeFactory
    {
        public void Delete(short id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location Type id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_LocationType";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteLocType");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataList<LocationType, long> FetchAll(LocationTypeSearchCriteria criteria)
        {
            DataList<LocationType, long> location = new DataList<LocationType, long>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_LocationType";
                    cmd.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cmd.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cmd.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cmd.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetLocationTypes"));
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (location.HeaderData == 0) location.HeaderData = dr.GetInt64("TotalRows");
                            LocationType locationType = GetlocationType(dr);
                            location.ItemCollection.Add(locationType);
                        }
                    }
                }
            }
            return location;
        }

        private LocationType GetlocationType(SafeDataReader dr)
        {
            LocationType Location = new LocationType();
            Location.Id = dr.GetInt16("Id");
            Location.Name = dr.GetString("LocationType");
            Location.Description = dr.GetString("Description");
            Location.IsActive = dr.GetBoolean("IsActive");
            Location.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            Location.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Location.CreatedOn = dr.GetDateTime("CreatedOn");
            Location.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            Location.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return Location;
        }

        public LocationType Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location Type id is required for get Location Type.");
            LocationType locationType = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cm = con.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_LocationType";
                    cm.Parameters.AddWithValue("@Id", id);
                    cm.Parameters.AddWithValue("@Type", "GetLocTypeById");

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            locationType = GetlocationType(dr);
                        }
                    }
                }
            }

            return locationType;
        }

        public LocationType Insert(LocationType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location type id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Location type: {entity.Name} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_LocationType";
                    cm.Parameters.AddWithValue("@Name", entity.Name.IsNullOrWhiteSpace() ? "" : entity.Name);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNullOrWhiteSpace() ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertLocType");

                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(LocationType entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Type Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_LocationType";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public LocationType Update(LocationType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Type should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Location type: {entity.Name} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_LocationType";
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNullOrWhiteSpace() ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "UpdateLocType");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}