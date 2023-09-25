using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class MaterialTypeFactory
    {
        public void Delete(short id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Material Type id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_MaterialType";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteMaterialType");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataList<MaterialType, long> FetchAll(MaterialTypeSearchCriteria criteria)
        {
            DataList<MaterialType, long> materialTypes = new DataList<MaterialType, long>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_MaterialType";
                    cmd.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cmd.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cmd.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cmd.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetMaterialTypes"));
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (materialTypes.HeaderData == 0) materialTypes.HeaderData = dr.GetInt64("TotalRows");
                            MaterialType locationType = GetlocationType(dr);
                            materialTypes.ItemCollection.Add(locationType);
                        }
                    }
                }
            }
            return materialTypes;
        }

        private MaterialType GetlocationType(SafeDataReader dr)
        {
            MaterialType Material = new MaterialType();
            Material.Id = dr.GetInt16("Id");
            Material.Name = dr.GetString("MaterialType");
            Material.Description = dr.GetString("Description");
            Material.IsActive = dr.GetBoolean("IsActive");
            Material.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            Material.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Material.CreatedOn = dr.GetDateTime("CreatedOn");
            Material.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            Material.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return Material;
        }

        public MaterialType Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Material Type id is required for get Material Type.");
            MaterialType locationType = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cm = con.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_MaterialType";
                    cm.Parameters.AddWithValue("@Id", id);
                    cm.Parameters.AddWithValue("@Type", "GeMaterialTypeById");

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

        public MaterialType Insert(MaterialType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Material type id should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Material type: {entity.Name} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_MaterialType";
                    cm.Parameters.AddWithValue("@Name", entity.Name.IsNullOrWhiteSpace() ? "" : entity.Name);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNullOrWhiteSpace() ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertMaterialType");

                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(MaterialType entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Material Type Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_MaterialType";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public MaterialType Update(MaterialType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Material Type should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Material type: {entity.Name} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_MaterialType";
                    cm.Parameters.AddWithValue("@Name", entity.Name.IsNullOrWhiteSpace() ? "" : entity.Name);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNullOrWhiteSpace() ? "" : entity.Description);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "UpdateMaterialType");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public List<KeyValue<short, string>> FetchMaterialType(int plantId)
        {
            //CodeContract.Required<ArgumentException>(plantId > 0, "Material Type id is required for get Material Type.");
            List<KeyValue<short, string>> locationType = new List<KeyValue<short, string>>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cm = con.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_MaterialType";
                    cm.Parameters.AddWithValue("@PlantId", plantId);
                    cm.Parameters.AddWithValue("@Type", "SelectAll");

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            locationType.Add(new KeyValue<short, string>() { Key = dr.GetInt16("Id"), Value = dr.GetString("MaterialType") });
                        }
                    }
                }
            }

            return locationType;
        }
    }
}