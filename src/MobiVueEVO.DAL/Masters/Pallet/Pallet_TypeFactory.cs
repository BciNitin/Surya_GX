using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class Pallet_TypeFactory
    {
        #region Delete

        public void Delete(int PalletId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(PalletId > 0, "PalletTypes id is required for delete.");

            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                Delete(PalletId, deletedBy, cn);
            }
        }

        private void Delete(int PalletId, long deletedBy, SqlConnection cn)
        {
            CodeContract.Required<ArgumentException>(PalletId > 0, "PalletTypes id is required for delete.");
            CodeContract.Required<ArgumentException>(cn != null, "Database connection should not be null.");
            using (var cm = cn.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "Usp_PalletType";
                cm.Parameters.AddWithValue("@Id", PalletId);
                cm.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                cm.Parameters.AddWithValue("@Type", "DeletePallet");
                cm.ExecuteNonQuery();
            }
        }

        #endregion Delete

        #region Fetch

        //public int FetchNoOfPalletOnLocation(long locationId, long siteId)
        //{
        //    CodeContract.Required<MobiVUEException>(locationId > 0, "Bin id is required");
        //    int capacity = 0;
        //    using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = con.CreateCommand())
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.CommandText = "Usp_Pallet";
        //            cmd.Parameters.AddWithValue("@LocationId", locationId);
        //            cmd.Parameters.AddWithValue("@PlantId", siteId);
        //            cmd.Parameters.AddWithValue("@Type", "NoOfPalletOnLocation");

        //            using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
        //            {
        //                if (dr.Read())
        //                {
        //                    capacity = dr.GetInt32("NoOfPallet");
        //                }
        //            }
        //        }
        //    }
        //    return capacity;
        //}

        public PalletTypes Fetch(int PalletId)
        {
            CodeContract.Required<ArgumentException>(PalletId > 0, "PalletTypes id is required for get PalletTypes.");

            PalletTypes _Pallet = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_PalletType";
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


        //public List<PalletTypes> FetchPalletTypes()
        //{
        //    List<PalletTypes> _uom = new List<PalletTypes>();

        //    using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = con.CreateCommand())
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.CommandText = "Usp_PalletType";
        //            cmd.Parameters.AddWithValue("@Type", "GetPallets");

        //            using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
        //            {
        //                while (dr.Read())
        //                {
        //                    UnitOfMeasurement uom = GetUOM(dr);
        //                    _uom.Add(uom);
        //                }
        //            }
        //        }
        //    }
        //    return _uom;
        //}
        private PalletTypes GetPallet(SafeDataReader dr)
        {
            PalletTypes pallet = new PalletTypes();
            pallet.PalletId = dr.GetInt32("ID");
            pallet.pallettype = dr.GetString("PalletType");
            pallet.Description = dr.GetString("Description");
            pallet.IsActive = dr.GetBoolean("IsActive");
            //pallet.Location = new KeyValue<int, string>() { Key = dr.GetInt32("LocationId"), Value = dr.GetString("LocationCode") };
            //pallet.Length = dr.GetDecimal("Length");
            pallet.Site = new KeyValue<int, string>(dr.GetInt32("PlantId"), dr.GetString("PlantCode"));
            //pallet.Height = dr.GetDecimal("Height");
            //pallet.Capacity = dr.GetInt32("Capacity");
            //pallet.Width = dr.GetDecimal("Width");
            //pallet.TagId = dr.GetString("TagId");
            //pallet.DimensionUOM = new KeyValue<short, string>() { Key = dr.GetInt16("DimentionUOM"), Value = dr.GetString("DimentionUOMName") };
            pallet.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            pallet.CreatedDate = dr.GetDateTime("CreatedOn");
            pallet.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("UpdatedByName") };
            pallet.UpdatedDate = dr.GetDateTime("ModifiedOn");
            pallet.MarkAsOld();
            return pallet;
        }

        public DataList<PalletTypes, long> FetchAll(PalletTypeSearchCriteria criteria)
        {
            DataList<PalletTypes, long> pallets = new DataList<PalletTypes, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandTimeout = 0;
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_PalletType";
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@PalletCode", criteria.PalletType.IsNotNullOrWhiteSpace() ? criteria.PalletType : ""));
                    //cm.Parameters.Add(new SqlParameter("@TagId", criteria.TagId.IsNotNullOrWhiteSpace() ? criteria.TagId : ""));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetPallets"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (pallets.HeaderData == 0) pallets.HeaderData = dr.GetInt64("TotalRows");
                            PalletTypes pallet = GetPallet(dr);
                            pallets.ItemCollection.Add(pallet);
                        }
                    }
                }
            }
            return pallets;
        }

        #endregion Fetch

        #region Insert

        public PalletTypes Insert(PalletTypes entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "PalletType should not be null.");
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                if (IsExists(entity, cn)) throw new Exception($"PalletType: {entity.pallettype} already exists.");

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_PalletType";
                        cm.Parameters.AddWithValue("@PalletCode", entity.pallettype.IsNotNull() ? entity.pallettype : "");
                        cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                        cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
                        cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "InsertPallet");

                        entity.PalletId = Convert.ToInt32(cm.ExecuteScalar());
                    }
                    transaction.Commit();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        #endregion Insert

        #region Update

        public PalletTypes Update(PalletTypes entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "PalletType should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"PalletType: {entity.pallettype} already exists.");
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_PalletType";
                        cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNull() ? entity.Description : "");
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

        public bool IsExists(PalletTypes item, SqlConnection con)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PalletCode", item.pallettype);
                cmd.Parameters.AddWithValue("@Id", item.PalletId);
                cmd.Parameters.AddWithValue("@PlantId", item.Site.IsNotNull() ? item.Site.Key : 0);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_PalletType";
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        #endregion Update
    }
}