using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class MachineTypeFactory
    {
        public void Delete(short MachineTypeId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(MachineTypeId > 0, "Machine Type id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_MachineType";
                    cmd.Parameters.AddWithValue("@Id", MachineTypeId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteMachineType");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public MachineType Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Machine Type id is required for get MachineType .");
            MachineType MachineType = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetMachineTypeById"));
                    cmd.CommandText = "USP_MachineType";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            MachineType = GetMachineType(dr);
                        }
                    }
                }
            }

            return MachineType;
        }

        public DataList<MachineType, long> FetchMachineTypes(MachineTypeSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<MachineType, long> MachineTypes = new DataList<MachineType, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MachineType";
                    cm.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetMachineTypes"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (MachineTypes.HeaderData == 0) MachineTypes.HeaderData = dr.GetInt64("TotalRows");
                            MachineType MachineType = GetMachineType(dr);
                            MachineTypes.ItemCollection.Add(MachineType);
                        }
                    }
                }
            }
            return MachineTypes;
        }

        public List<KeyValue<short, string>> FetchMachineTypes(int siteId)
        {
            List<KeyValue<short, string>> MachineTypes = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MachineType";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetMachineTypesforBinding"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            MachineTypes.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("MachineType") });
                        }
                    }
                }
            }
            return MachineTypes;
        }

        private MachineType GetMachineType(SafeDataReader dr)
        {
            MachineType MachineType = new MachineType();
            MachineType.Id = dr.GetInt16("ID");
            MachineType.Name = dr.GetString("MachineType");
            MachineType.Description = dr.GetString("Description");
            MachineType.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            MachineType.IsActive = dr.GetBoolean("IsActive");
            MachineType.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            MachineType.CreatedOn = dr.GetDateTime("CreatedOn");
            MachineType.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            MachineType.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return MachineType;
        }

        public bool IsExists(MachineType entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "MachineType should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_MachineType";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public MachineType Insert(MachineType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "MachineType should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Machine Type: {entity.Name} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MachineType";
                    cm.Parameters.AddWithValue("@Name", entity.Name);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertMachineType");
                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public MachineType Update(MachineType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Conveyor Line should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Machine Type: {entity.Name} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MachineType";
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);

                    cm.Parameters.AddWithValue("@Type", "UpdateMachineType");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }


        public List<KeyValue<short, string>> FetchMachineType(int siteId)
        {
            List<KeyValue<short, string>> SpoolTypeFormats = new List<KeyValue<short, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_MachineType";
                    cm.Parameters.Add(new SqlParameter("@plantId", siteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetMachineType"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            SpoolTypeFormats.Add(new KeyValue<short, string>() { Key = dr.GetInt16("id"), Value = dr.GetString("MachineType") });
                        }
                    }
                }
            }
            return SpoolTypeFormats;
        }
    }
}