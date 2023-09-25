using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class ZoneInventoryFactory
    {
        //public InventoryWIP Fetch(long inventeryId)
        //{
        //    CodeContract.Required<ArgumentException>(inventeryId > 0, "Inventery id required for get item.");
        //    InventoryWIP inventery = null;
        //    using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = con.CreateCommand())
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.CommandText = "USP_InventoryWIP";
        //            cmd.Parameters.Add(new SqlParameter("@Id", inventeryId));
        //            cmd.Parameters.Add(new SqlParameter("@Type", "GetInventoryById"));

        //            using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
        //            {
        //                if (dr.Read())
        //                {
        //                    inventery = GetInventery(dr);
        //                }
        //            }
        //        }
        //    }
        //    return inventery;
        //}

        //public string SavePalletization(InventoryWIPInfo inventory, long modifiedBy)
        //{
        //    CodeContract.Required<ArgumentException>(inventory.IsNotNull(), "Inventory is mandatory.");
        //    using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = con.CreateCommand())
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.CommandText = "USP_Palletization";
        //            cmd.Parameters.AddWithValue("@plantid", inventory.PlantId);
        //            cmd.Parameters.AddWithValue("@palletid", inventory.PalletId);
        //            cmd.Parameters.AddWithValue("@qty", inventory.Qty);
        //            cmd.Parameters.AddWithValue("@grnorderid", inventory.InwardOrderId);
        //            cmd.Parameters.AddWithValue("@grnorderitemid", inventory.InwardOrderItemId);
        //            cmd.Parameters.AddWithValue("@vendorbatch", inventory.VendorBatch);
        //            cmd.Parameters.AddWithValue("@Modifiedby", modifiedBy);
        //            return cmd.ExecuteScalar().ToString();
        //        }
        //    }
        //}



        public DataList<ZoneInventory, long> FetchInventorys(ZoneInventorySearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<ZoneInventory, long> ZoneInventorys = new DataList<ZoneInventory, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_BLE_ZoneInventory";
                    //cm.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    //cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    //cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "SELECT"));
                    //cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    //cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            //if (ZoneInventorys.HeaderData == 0) ZoneInventorys.HeaderData = dr.GetInt64("TotalRows");
                            ZoneInventory ZoneInventory = GetZoneInventory(dr);
                            ZoneInventorys.ItemCollection.Add(ZoneInventory);
                        }
                    }
                }
            }
            return ZoneInventorys;
        }

        private ZoneInventory GetZoneInventory(SafeDataReader dr)
        {
            ZoneInventory ZoneInventory = new ZoneInventory();
            ZoneInventory.InventoryId = dr.GetInt32("ID");
            ZoneInventory.SMAC = dr.GetString("ZONE");
            ZoneInventory.Beacon = dr.GetString("BEACON");
            ZoneInventory.RSSI = dr.GetDecimal("RSSI");
            ZoneInventory.Battery = dr.GetDecimal("BATTERY");
            //ZoneInventory.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            //ZoneInventory.IsActive = dr.GetBoolean("IsActive");
            //ZoneInventory.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            //ZoneInventory.CreatedOn = dr.GetDateTime("CreatedOn");
            //ZoneInventory.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            ZoneInventory.UpdatedDate = dr.GetDateTime("UPDATEDON");
            return ZoneInventory;
        }

        //public InventoryWIP Insert(InventoryWIP entity)
        //{
        //    CodeContract.Required<ArgumentException>(entity != null, "Inventery should not be null");
        //    entity.Validate();
        //    using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
        //    {
        //        cn.Open();
        //        using (var transaction = cn.BeginTransaction())
        //        {
        //            entity = Insert(entity, transaction);
        //            transaction.Commit();
        //        }
        //    }
        //    return entity;
        //}

        //private InventoryWIP Insert(InventoryWIP entity, SqlTransaction transaction)
        //{
        //    CodeContract.Required<ArgumentException>(entity != null, "Picking should not be null.");
        //    CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
        //    entity.Validate();
        //    using (var cm = transaction.Connection.CreateCommand())
        //    {
        //        cm.CommandType = System.Data.CommandType.StoredProcedure;
        //        cm.CommandText = "USP_InventoryWIP";
        //        cm.Transaction = transaction;
        //        cm.Parameters.AddWithValue("@MaterialId", entity.Material.Key);
        //        cm.Parameters.AddWithValue("@LocationId", entity.Location.Key);
        //        cm.Parameters.AddWithValue("@PalletId", entity.Pallet.IsNotNull() ? entity.Pallet.Key : 0);
        //        cm.Parameters.AddWithValue("@SiteId", entity.Site.Key);
        //        cm.Parameters.AddWithValue("@Qty", entity.Quantity);
        //        cm.Parameters.AddWithValue("@Status", entity.Status);
        //        cm.Parameters.AddWithValue("@BarcodeNumber", entity.Barcode.IsNotNullOrWhiteSpace() ? entity.Barcode : "");
        //        cm.Parameters.AddWithValue("@OrderId", entity.GRNOrderId);
        //        cm.Parameters.AddWithValue("@GRNOrderItemId", entity.GRNOrderItemId);
        //        cm.Parameters.AddWithValue("@Batch", entity.Batch.IsNotNullOrWhiteSpace() ? entity.Batch : "");
        //        cm.Parameters.AddWithValue("@ManufacturingDate", DateTime.Now);
        //        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
        //        cm.Parameters.AddWithValue("@Type", "InsertInventory");

        //        entity.InventoryId = Convert.ToInt64(cm.ExecuteScalar());
        //        entity.MarkAsOld();
        //    }
        //    return entity;
        //}

        //public InventoryWIP Update(InventoryWIP entity)
        //{
        //    CodeContract.Required<ArgumentException>(entity != null, "Inventery should not be null");
        //    CodeContract.Required<ArgumentException>(entity.Location.IsNotNull() && entity.Location.Key > 0, "Location is mandatory to store material");
        //    entity.Validate();
        //    using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
        //    {
        //        cn.Open();
        //        using (var transaction = cn.BeginTransaction())
        //        {
        //            entity = Update(entity, transaction);
        //            transaction.Commit();
        //        }
        //        return entity;
        //    }
        //}

        //private InventoryWIP Update(InventoryWIP entity, SqlTransaction transaction)
        //{
        //    CodeContract.Required<ArgumentException>(entity != null, "Picking should not be null.");
        //    CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");

        //    using (var cm = transaction.Connection.CreateCommand())
        //    {
        //        cm.CommandType = System.Data.CommandType.StoredProcedure;
        //        cm.CommandText = "USP_InventoryWIP";
        //        cm.Transaction = transaction;

        //        cm.Parameters.AddWithValue("@LocationId", entity.Location.Key);
        //        cm.Parameters.AddWithValue("@PalletId", entity.Pallet.Key);
        //        cm.Parameters.AddWithValue("@Qty", entity.Quantity);
        //        cm.Parameters.AddWithValue("@Status", entity.Status);
        //        cm.Parameters.AddWithValue("@Batch", entity.Batch.IsNotNullOrWhiteSpace() ? entity.Batch : "");
        //        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
        //        cm.Parameters.AddWithValue("@Id", entity.InventoryId);
        //        cm.Parameters.AddWithValue("@Type", "UpdateInventory");
        //        cm.ExecuteNonQuery();
        //        entity.MarkAsOld();
        //    }
        //    return entity;
        //}

        //public void Delete(long id)
        //{
        //}
    }
}