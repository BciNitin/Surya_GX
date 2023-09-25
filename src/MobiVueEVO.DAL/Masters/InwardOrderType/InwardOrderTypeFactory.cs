using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class InwardOrderTypeFactory
    {
        public void Delete(short id, long modifiedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Inward order type id is required for delete.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_InwardOrderType";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
                    cmd.Parameters.AddWithValue("@Type", "Delete");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public InwardOrderType Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Inward order type id is required for get order type");
            InwardOrderType _inwardOrderType = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetById"));
                    cmd.CommandText = "Usp_InwardOrderType";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            _inwardOrderType = GetInwardOrderType(dr);
                        }
                    }
                }
            }
            return _inwardOrderType;
        }

        private InwardOrderType GetInwardOrderType(SafeDataReader dr)
        {
            InwardOrderType _inwardOrderType = new InwardOrderType();
            _inwardOrderType.InwardOrderTypeId = dr.GetInt16("Id");
            _inwardOrderType.Name = dr.GetString("Name");
            _inwardOrderType.IsActive = dr.GetBoolean("IsActive");
            _inwardOrderType.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("UpdatedBy"), Value = dr.GetString("UpdatedByName") };
            _inwardOrderType.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            _inwardOrderType.CreatedDate = dr.GetDateTime("CreatedOn");
            _inwardOrderType.UpdatedDate = dr.GetDateTime("ModifiedOn");
            _inwardOrderType.MarkAsOld();
            return _inwardOrderType;
        }

        public DataList<InwardOrderType, long> FetchAll(InwardOrderTypeSearchCriteria criteria)
        {
            DataList<InwardOrderType, long> _inwardOrderTypes = new DataList<InwardOrderType, long>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cm = con.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderType";
                    cm.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetTypes"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (_inwardOrderTypes.HeaderData == 0) _inwardOrderTypes.HeaderData = dr.GetInt64("TotalRows");
                            InwardOrderType _type = GetInwardOrderType(dr);
                            _inwardOrderTypes.ItemCollection.Add(_type);
                        }
                    }
                }
            }
            return _inwardOrderTypes;
        }

        public List<KeyValue<short, string>> FetchInwardOrderTypeNames()
        {
            List<KeyValue<short, string>> _inwardOrderTypes = new List<KeyValue<short, string>>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cm = con.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderType";
                    cm.Parameters.Add(new SqlParameter("@Type", "GETINWARDORDERTYPENAMES"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            _inwardOrderTypes.Add(new KeyValue<short, string>() { Key = dr.GetInt16("ID"), Value = dr.GetString("NAME") });
                        }
                    }
                }
            }
            return _inwardOrderTypes;
        }

        public InwardOrderType Insert(InwardOrderType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Inward order type should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Inward order type: {entity.Name} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderType";
                    cm.Parameters.Add(new SqlParameter("@Name", entity.Name));
                    cm.Parameters.Add(new SqlParameter("@Type", "Insert"));
                    cm.Parameters.Add(new SqlParameter("@IsActive", entity.IsActive));
                    cm.Parameters.Add(new SqlParameter("@UpdatedBy", entity.UpdatedBy.Key));

                    entity.InwardOrderTypeId = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        private bool IsExists(InwardOrderType entity, SqlConnection cn)
        {
            using (SqlCommand cmd = cn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@ID", entity.InwardOrderTypeId);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_InwardOrderType";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public InwardOrderType Update(InwardOrderType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Quality check type should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderType";

                    cm.Parameters.Add(new SqlParameter("@IsActive", entity.IsActive));
                    cm.Parameters.Add(new SqlParameter("@Type", "Update"));
                    cm.Parameters.Add(new SqlParameter("@UpdatedBy", entity.UpdatedBy.Key));
                    cm.Parameters.Add(new SqlParameter("@ID", entity.InwardOrderTypeId));
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}