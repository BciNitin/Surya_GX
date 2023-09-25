using MobiVUE;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class PalletTypeFactory
    {
        public void Delete(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Pallet type id is required for delete.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cm = con.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_PalletType";
                    cm.Parameters.AddWithValue("@Id", id);
                    cm.Parameters.AddWithValue("@Type", "DeletePalletType");
                    cm.ExecuteNonQuery();
                }
            }
        }

        public PalletType Fetch(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Pallet type id is required for get pallet type.");
            PalletType _palletType = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_PalletType";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetPalletTypeById"));

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            _palletType = GetPalletType(dr);
                        }
                    }
                }
            }
            return _palletType;
        }

        private PalletType GetPalletType(SafeDataReader dr)
        {
            PalletType _type = new PalletType();
            _type.PalletTypeId = dr.GetInt16("ID");
            _type.Code = dr.GetString("Code");
            _type.Description = dr.GetString("Description");
            return _type;
        }

        public List<PalletType> FetchAll()
        {
            List<PalletType> _palletTypes = new List<PalletType>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_PalletType";
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetPalletTypes"));

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            PalletType _type = GetPalletType(dr);
                            _palletTypes.Add(_type);
                        }
                    }
                }
            }
            return _palletTypes;
        }

        public PalletType Insert(PalletType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Pallet type should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Pallet type: {entity.Code} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_PalletType";
                    cm.Parameters.Add(new SqlParameter("@Code", entity.Code));
                    cm.Parameters.Add(new SqlParameter("@Description", entity.Description));
                    cm.Parameters.Add(new SqlParameter("@Type", "InsertPalletType"));

                    entity.PalletTypeId = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        private bool IsExists(PalletType entity, SqlConnection cn)
        {
            using (SqlCommand cmd = cn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@Id", entity.PalletTypeId);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_PalletType";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public PalletType Update(PalletType entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Pallet type should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_PalletType";
                    cm.Parameters.Add(new SqlParameter("@Description", entity.Description));
                    cm.Parameters.Add(new SqlParameter("@Type", "InsertPalletType"));
                    cm.Parameters.Add(new SqlParameter("@Id", entity.PalletTypeId));
                    cm.ExecuteNonQuery();

                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}