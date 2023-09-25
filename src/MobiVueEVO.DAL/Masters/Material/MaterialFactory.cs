using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class MaterialFactory
    {
        public void Delete(int MaterialMasterId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(MaterialMasterId > 0, "MaterialMaster Id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_MaterialMaster";
                    cmd.Parameters.AddWithValue("@Id", MaterialMasterId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Mode", "DELETE");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public MaterialLapp Fetch(int id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "MaterialMaster id is required for get MaterialMaster .");
            MaterialLapp MaterialMaster = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Mode", "SearchById"));
                    cmd.CommandText = "USP_MaterialMaster";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            MaterialMaster = GetMaterialMaster(dr);
                        }
                    }
                }
            }

            return MaterialMaster;
        }

        //FetchMaterialMaster
        public DataList<MaterialLapp, long> FetchMaterialMaster(MaterialSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<MaterialLapp, long> MaterialMasters = new DataList<MaterialLapp, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MaterialMaster";
                    cm.Parameters.AddWithValue("@MaterialCode", criteria.MaterialCode.IsNotNullOrWhiteSpace() ? criteria.MaterialCode : "");
                    cm.Parameters.AddWithValue("@MaterialType", criteria.MaterialType);
                    cm.Parameters.AddWithValue("@IsActive", criteria.Status);
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    cm.Parameters.Add(new SqlParameter("@Mode", "SearchAll"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (MaterialMasters.HeaderData == 0) MaterialMasters.HeaderData = dr.GetInt64("TotalRows");
                            MaterialLapp MaterialMaster = GetMaterialMaster(dr);
                            MaterialMasters.ItemCollection.Add(MaterialMaster);
                        }
                    }
                }
            }
            return MaterialMasters;
        }

        private MaterialLapp GetMaterialMaster(SafeDataReader dr)
        {
            MaterialLapp MaterialCategory = new MaterialLapp();
            MaterialCategory.Id = dr.GetInt32("ID");
            MaterialCategory.MaterialCode = dr.GetString("MaterialCode");
            MaterialCategory.MaterialDescription = dr.GetString("MaterialDescription");
            MaterialCategory.ConversionUOM = new KeyValue<short, string>() { Key = dr.GetInt16("BaseUOM"), Value = dr.GetString("UOMCODE") };
            MaterialCategory.MaterialType = new KeyValue<short, string>() { Key = dr.GetInt16("MaterialType"), Value = dr.GetString("MaterialTypeName") };
            MaterialCategory.IsActive = dr.GetBoolean("IsActive");
            MaterialCategory.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            MaterialCategory.CreatedOn = dr.GetDateTime("CreatedOn");
            MaterialCategory.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            MaterialCategory.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return MaterialCategory;
        }

        public bool IsExists(MaterialLapp entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "RSMaterialMaster Code should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialCode", entity.MaterialCode);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Mode", "IsExists");
                cmd.CommandText = "USP_MaterialMaster";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public MaterialLapp Insert(MaterialLapp entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "RSMaterialMaster Master should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Material Master: {entity.MaterialCode} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MaterialMaster";
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@MaterialCode", entity.MaterialCode);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@CompanyId", entity.CompanyId);
                    cm.Parameters.AddWithValue("@MaterialDescription", entity.MaterialDescription);
                    cm.Parameters.AddWithValue("@ConversionUOM", entity.ConversionUOM.Key);
                    cm.Parameters.AddWithValue("@MaterialType", entity.MaterialType.Key);
                    cm.Parameters.AddWithValue("@Mode", "Insert");
                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public MaterialLapp Update(MaterialLapp entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "RSMaterialMaster Master should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_MaterialMaster";
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@MaterialDescription", entity.MaterialDescription);
                    cm.Parameters.AddWithValue("@ConversionUOM", entity.ConversionUOM.Key);
                    cm.Parameters.AddWithValue("@MaterialType", entity.MaterialType.Key);
                    cm.Parameters.AddWithValue("@Mode", "Update");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}