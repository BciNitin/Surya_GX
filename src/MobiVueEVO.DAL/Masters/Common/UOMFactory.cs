using MobiVUE;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class UomFactory
    {
        public void Delete(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "UOM id is required for delete.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_UOM";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Type", "DeleteUOM");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public UnitOfMeasurement Fetch(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "UOM id is required for get UOM.");
            UnitOfMeasurement _uom = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_UOM";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.AddWithValue("@Type", "GetUOMById");

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            _uom = GetUOM(dr);
                        }
                    }
                }
            }
            return _uom;
        }

        private UnitOfMeasurement GetUOM(SafeDataReader dr)
        {
            UnitOfMeasurement uom = new UnitOfMeasurement();
            uom.UOMId = dr.GetInt16("ID");
            uom.Code = dr.GetString("UOMCode");
            uom.Name = dr.GetString("UOMName");
            uom.MarkAsOld();
            return uom;
        }

        public List<UnitOfMeasurement> FetchAll()
        {
            List<UnitOfMeasurement> _uom = new List<UnitOfMeasurement>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_UOM";
                    cmd.Parameters.AddWithValue("@Type", "GetUOMs");

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            UnitOfMeasurement uom = GetUOM(dr);
                            _uom.Add(uom);
                        }
                    }
                }
            }
            return _uom;
        }

        public UnitOfMeasurement Insert(UnitOfMeasurement entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "UOM should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"UOM: {entity.Code} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_UOM";
                    cm.Parameters.Add(new SqlParameter("@Code", entity.Code));
                    cm.Parameters.Add(new SqlParameter("@Name", entity.Name));
                    cm.Parameters.Add(new SqlParameter("@Type", "InsertUOM"));

                    entity.UOMId = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        private bool IsExists(UnitOfMeasurement entity, SqlConnection cn)
        {
            using (SqlCommand cmd = cn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@ID", entity.UOMId);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_UOM";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public UnitOfMeasurement Update(UnitOfMeasurement entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "UOM should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_UOM";
                    cm.Parameters.Add(new SqlParameter("@Name", entity.Name));
                    cm.Parameters.Add(new SqlParameter("@ID", entity.UOMId));
                    cm.Parameters.Add(new SqlParameter("@Type", "UpdateUOM"));
                    cm.ExecuteNonQuery();

                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}