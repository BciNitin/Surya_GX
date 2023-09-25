using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class PalletFactory
    {
        #region Delete

        public void Delete(long PalletId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(PalletId > 0, "Pallet id is required for delete.");

            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                Delete(PalletId, deletedBy, cn);
            }
        }

        private void Delete(long PalletId, long deletedBy, SqlConnection cn)
        {
            CodeContract.Required<ArgumentException>(PalletId > 0, "Pallet id is required for delete.");
            CodeContract.Required<ArgumentException>(cn != null, "Database connection should not be null.");
            using (var cm = cn.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "Usp_Pallet";
                cm.Parameters.AddWithValue("@Id", PalletId);
                cm.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                cm.Parameters.AddWithValue("@Type", "DeletePallet");
                cm.ExecuteNonQuery();
            }
        }

        #endregion Delete

        #region Fetch

        public int FetchNoOfPalletOnLocation(long locationId, long siteId)
        {
            CodeContract.Required<MobiVUEException>(locationId > 0, "Bin id is required");
            int capacity = 0;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Pallet";
                    cmd.Parameters.AddWithValue("@LocationId", locationId);
                    cmd.Parameters.AddWithValue("@PlantId", siteId);
                    cmd.Parameters.AddWithValue("@Type", "NoOfPalletOnLocation");

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            capacity = dr.GetInt32("NoOfPallet");
                        }
                    }
                }
            }
            return capacity;
        }

        public Pallet Fetch(long PalletId)
        {
            CodeContract.Required<ArgumentException>(PalletId > 0, "Pallet id is required for get Pallet.");

            Pallet _Pallet = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_Pallet";
                cmd.Parameters.Add(new SqlParameter("@Id", PalletId));
                cmd.Parameters.Add(new SqlParameter("@Type", "GetPalletById"));

                using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                {
                    if (dr.Read())
                    {
                        _Pallet = GetPallet(dr);
                    }
                }
            }
            return _Pallet;
        }

        private Pallet GetPallet(SafeDataReader dr)
        {
            Pallet pallet = new Pallet();
            pallet.PalletId = dr.GetInt64("ID");
            pallet.Code = dr.GetString("Code");
            pallet.Description = dr.GetString("Description");
            pallet.IsActive = dr.GetBoolean("IsActive");
            pallet.Location = new KeyValue<int, string>() { Key = dr.GetInt32("LocationId"), Value = dr.GetString("LocationCode") };
            pallet.palletType = new KeyValue<int, string>() { Key = dr.GetInt32("PalletTypeId"), Value = dr.GetString("PalletType") };
            pallet.Length = dr.GetDecimal("Length");
            pallet.Site = new KeyValue<int, string>(dr.GetInt32("PlantId"), dr.GetString("PlantCode"));
            pallet.Height = dr.GetDecimal("Height");
            pallet.Capacity = dr.GetInt32("Capacity");
            pallet.Width = dr.GetDecimal("Width");
            pallet.TagId = dr.GetString("TagId");
            pallet.DimensionUOM = new KeyValue<short, string>() { Key = dr.GetInt16("DimentionUOM"), Value = dr.GetString("DimentionUOMName") };
            pallet.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            pallet.CreatedDate = dr.GetDateTime("CreatedOn");
            pallet.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("UpdatedByName") };
            pallet.UpdatedDate = dr.GetDateTime("ModifiedOn");
            pallet.MarkAsOld();
            return pallet;
        }

        public DataList<Pallet, long> FetchAll(PalletSearchCriteria criteria)
        {
            DataList<Pallet, long> pallets = new DataList<Pallet, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandTimeout = 0;
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Pallet";
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@PalletCode", criteria.PalletCode.IsNotNullOrWhiteSpace() ? criteria.PalletCode : ""));
                    cm.Parameters.Add(new SqlParameter("@TagId", criteria.TagId.IsNotNullOrWhiteSpace() ? criteria.TagId : ""));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetPallets"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (pallets.HeaderData == 0) pallets.HeaderData = dr.GetInt64("TotalRows");
                            Pallet pallet = GetPallet(dr);
                            pallets.ItemCollection.Add(pallet);
                        }
                    }
                }
            }
            return pallets;
        }

        #endregion Fetch

        #region Insert

        public Pallet Insert(Pallet entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Pallet should not be null.");
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                if (IsExists(entity, cn)) throw new Exception($"Pallet: {entity.Code} already exists.");

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_Pallet";
                        cm.Parameters.AddWithValue("@PalletCode", entity.Code.IsNotNull() ? entity.Code : "");
                        cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                        cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        cm.Parameters.AddWithValue("@Length", entity.Length);
                        cm.Parameters.AddWithValue("@Width", entity.Width);
                        cm.Parameters.AddWithValue("@Height", entity.Height);
                        cm.Parameters.AddWithValue("@Capacity", entity.Capacity);
                        cm.Parameters.AddWithValue("@DimentionUOM", entity.DimensionUOM.IsNotNull() ? entity.DimensionUOM.Key : 0);
                        cm.Parameters.AddWithValue("@PalletTypeId", entity.palletType.IsNotNull() ? entity.palletType.Key : 0);
                        cm.Parameters.AddWithValue("@TagId", entity.TagId);
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "InsertPallet");

                        entity.PalletId = Convert.ToInt64(cm.ExecuteScalar());
                    }
                    transaction.Commit();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        #endregion Insert

        #region Update

        public Pallet Update(Pallet entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Pallet should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Pallet: {entity.Code} already exists.");
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_Pallet";
                        cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        cm.Parameters.AddWithValue("@Length", entity.Length);
                        cm.Parameters.AddWithValue("@Width", entity.Width);
                        cm.Parameters.AddWithValue("@Height", entity.Height);
                        cm.Parameters.AddWithValue("@LocationId", entity.Location.IsNotNull() ? entity.Location.Key : 0);
                        cm.Parameters.AddWithValue("@DimentionUOM", entity.DimensionUOM.IsNotNull() ? entity.DimensionUOM.Key : 0);
                        cm.Parameters.AddWithValue("@PalletTypeId", entity.palletType.IsNotNull() ? entity.palletType.Key : 0);
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Id", entity.PalletId);
                        cm.Parameters.AddWithValue("@Type", "UpdatePallet");

                        cm.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public bool IsExists(Pallet item, SqlConnection con)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PalletCode", item.Code);
                cmd.Parameters.AddWithValue("@Id", item.PalletId);
                cmd.Parameters.AddWithValue("@PlantId", item.Site.IsNotNull() ? item.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_Pallet";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        #endregion Update
    }
}