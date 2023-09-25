using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class LocationFormatFactory
    {
        public void Delete(short id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location Format id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_LocationFormat";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteLocationFormat");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataList<LocationFormat, long> FetchAllCriteria(LocationFormatSearchCriteria criteria)
        {
            DataList<LocationFormat, long> locationFormate = new DataList<LocationFormat, long>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_LocationFormat";
                    cmd.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cmd.Parameters.Add(new SqlParameter("@Code", criteria.Code));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cmd.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetLocationFormats"));

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (locationFormate.HeaderData == 0) locationFormate.HeaderData = dr.GetInt64("TotalRows");
                            LocationFormat format = GetZoneAll(dr);
                            locationFormate.ItemCollection.Add(format);
                        }
                    }
                }
            }
            return locationFormate;
        }

        private LocationFormat GetZoneAll(SafeDataReader dr)
        {
            LocationFormat locFormat = new LocationFormat();
            locFormat.Id = dr.GetInt16("Id");
            locFormat.Code = dr.GetString("Code");
            locFormat.Description = dr.GetString("Description");
            locFormat.IsActive = dr.GetBoolean("IsActive");
            locFormat.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            locFormat.CreatedOn = dr.GetDateTime("CreatedOn");
            locFormat.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            locFormat.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return locFormat;
        }

        public LocationFormat Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Location Format id is required for get Zone Location.");
            LocationFormat zone = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_LocationFormat";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Type", "GetLocationFormatById");

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            zone = GetZoneAll(dr);
                        }
                    }
                }
            }

            return zone;
        }

        public LocationFormat Insert(LocationFormat entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Fromat Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Location Format: {entity.Code}  already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_LocationFormat";
                    cm.Parameters.AddWithValue("@Code", entity.Code.IsNullOrWhiteSpace() ? "" : entity.Code);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNullOrWhiteSpace() ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertLocationFormat");

                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(LocationFormat entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location formats Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_LocationFormat";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public LocationFormat Update(LocationFormat entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Location Formats Id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Location Format: {entity.Code}  already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_LocationFormat";
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNullOrWhiteSpace() ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "UpdateLocationFormat");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}