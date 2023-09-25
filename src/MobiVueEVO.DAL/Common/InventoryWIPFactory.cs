using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class InventoryWIPFactory
    {
        public InventoryWIP Fetch(long inventeryId)
        {
            CodeContract.Required<ArgumentException>(inventeryId > 0, "Inventery id required for get item.");
            InventoryWIP inventery = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_InventoryWIP";
                    cmd.Parameters.Add(new SqlParameter("@Id", inventeryId));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetInventoryById"));

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            inventery = GetInventery(dr);
                        }
                    }
                }
            }
            return inventery;
        }

        public string SavePalletization(InventoryWIPInfo inventory, long modifiedBy)
        {
            CodeContract.Required<ArgumentException>(inventory.IsNotNull(), "Inventory is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_Palletization";
                    cmd.Parameters.AddWithValue("@PLANTID", inventory.PlantId);
                    cmd.Parameters.AddWithValue("@PALLETID", inventory.PalletId);
                    cmd.Parameters.AddWithValue("@qty", inventory.Qty);
                    cmd.Parameters.AddWithValue("@Barcode", inventory.Barcode);
                    cmd.Parameters.AddWithValue("@grnorderid", inventory.InwardOrderId);
                    cmd.Parameters.AddWithValue("@grnorderitemid", inventory.InwardOrderItemId);
                    cmd.Parameters.AddWithValue("@VENDORBATCH", inventory.VendorBatch);
                    cmd.Parameters.AddWithValue("@Modifiedby", modifiedBy);
                    cmd.Parameters.AddWithValue("@Type", "Save");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        private InventoryWIP GetInventery(SafeDataReader dr)
        {
            InventoryWIP inventery = new InventoryWIP();
            inventery.InventoryId = dr.GetInt64("Id");
            inventery.Quantity = dr.GetDecimal("Qty");
            inventery.GRNOrder = new KeyValue<long, string>() { Key = dr.GetInt64("GRNOrderId"), Value = dr.GetString("DocumentNo") };
            inventery.GRNOrderItemId = dr.GetInt64("GRNOrderItemId");
            inventery.Batch = dr.GetString("Batch");
            inventery.Barcode = dr.GetString("BarcodeNumber");
            inventery.Status = (InventoryStatus)(dr.GetInt32("Status"));
            inventery.Material = new KeyInfo<int, string, string>() { Key = dr.GetInt32("MaterialId"), KeyCode = dr.GetString("MaterialCode"), Value = dr.GetString("MaterialDescription") };
            inventery.Location = new KeyValue<long, string>() { Key = dr.GetInt64("LocationId"), Value = dr.GetString("LocationCode") };
            inventery.Pallet = new KeyValue<long, string>() { Key = dr.GetInt64("PalletId"), Value = dr.GetString("PalletCode") };
            inventery.CreatedDate = dr.GetDateTime("CreatedOn");
            inventery.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            inventery.UpdatedDate = dr.GetDateTime("ModifiedOn");
            inventery.Site = new KeyValue<int, string>() { Key = dr.GetInt32("SiteId"), Value = dr.GetString("PlantCode") };
            inventery.ManufacturingDate = dr.GetDateTime("ManufacturingDate");
            inventery.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            inventery.PutawayOn = dr.GetDateTime("PutawayOn");
            inventery.PalletizationOn = dr.GetDateTime("PalletizationON");
            inventery.GRNOn = dr.GetDateTime("GRNON");
            inventery.MarkAsOld();
            return inventery;
        }

        public List<InventoryWIP> FetchInventorys(InventoryWIPSearchCriteria criteria)
        {
            List<InventoryWIP> Inventerys = new List<InventoryWIP>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_InventoryWIP";
                    cm.Parameters.Add(new SqlParameter("@Type", "GetInventories"));
                    cm.Parameters.Add(new SqlParameter("@BarcodeNumber", criteria.Barcode.IsNotNullOrWhiteSpace() ? criteria.Barcode : ""));
                    cm.Parameters.Add(new SqlParameter("@MaterialCode", criteria.MaterialCode.IsNotNullOrWhiteSpace() ? criteria.MaterialCode : ""));
                    cm.Parameters.Add(new SqlParameter("@MaterialId", criteria.MaterialId));
                    cm.Parameters.Add(new SqlParameter("@PalletId", criteria.PalletId));
                    cm.Parameters.Add(new SqlParameter("@SiteId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@OrderId", criteria.GRNId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@LocationId", criteria.LocationId));
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            InventoryWIP zone = GetInventery(dr);
                            Inventerys.Add(zone);
                        }
                    }
                }
            }
            return Inventerys;
        }

        public List<InventoryWIP> FetchInventorysForReport(InventoryLevelReportSearchCriteria criteria)
        {
            List<InventoryWIP> Inventerys = new List<InventoryWIP>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_InventoryWIP";
                    cm.Parameters.Add(new SqlParameter("@Type", "GetInventories"));
                    cm.Parameters.Add(new SqlParameter("@BarcodeNumber", criteria.Barcode.IsNotNullOrWhiteSpace() ? criteria.Barcode : ""));
                    cm.Parameters.Add(new SqlParameter("@MaterialCode", criteria.MaterialCode.IsNotNullOrWhiteSpace() ? criteria.MaterialCode : ""));
                    cm.Parameters.Add(new SqlParameter("@MaterialId", criteria.MaterialId));
                    cm.Parameters.Add(new SqlParameter("@PalletId", criteria.PalletId));
                    cm.Parameters.Add(new SqlParameter("@SiteId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@OrderId", criteria.GRNId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@LocationId", criteria.LocationId));
                    cm.Parameters.Add(new SqlParameter("@documentNo", criteria.documentNo));
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            InventoryWIP zone = GetInventery(dr);
                            Inventerys.Add(zone);
                        }
                    }
                }
            }
            return Inventerys;
        }



        public InventoryWIP Insert(InventoryWIP entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Inventery should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    entity = Insert(entity, transaction);
                    transaction.Commit();
                }
            }
            return entity;
        }

        private InventoryWIP Insert(InventoryWIP entity, SqlTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Picking should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cm = transaction.Connection.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "USP_InventoryWIP";
                cm.Transaction = transaction;
                cm.Parameters.AddWithValue("@MaterialId", entity.Material.Key);
                cm.Parameters.AddWithValue("@LocationId", entity.Location.Key);
                cm.Parameters.AddWithValue("@PalletId", entity.Pallet.IsNotNull() ? entity.Pallet.Key : 0);
                cm.Parameters.AddWithValue("@SiteId", entity.Site.Key);
                cm.Parameters.AddWithValue("@Qty", entity.Quantity);
                cm.Parameters.AddWithValue("@Status", entity.Status);
                cm.Parameters.AddWithValue("@BarcodeNumber", entity.Barcode.IsNotNullOrWhiteSpace() ? entity.Barcode : "");
                cm.Parameters.AddWithValue("@OrderId", entity.GRNOrder.IsNotNull() ? entity.GRNOrder.Key : 0);
                cm.Parameters.AddWithValue("@GRNOrderItemId", entity.GRNOrderItemId);
                cm.Parameters.AddWithValue("@Batch", entity.Batch.IsNotNullOrWhiteSpace() ? entity.Batch : "");
                cm.Parameters.AddWithValue("@ManufacturingDate", DateTime.Now);
                cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                cm.Parameters.AddWithValue("@Type", "InsertInventory");

                entity.InventoryId = Convert.ToInt64(cm.ExecuteScalar());
                entity.MarkAsOld();
            }
            return entity;
        }

        public InventoryWIP Update(InventoryWIP entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Inventery should not be null");
            //CodeContract.Required<ArgumentException>(entity.Location.IsNotNull() && entity.Location.Key > 0, "Location is mandatory to store material");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    entity = Update(entity, transaction);
                    transaction.Commit();
                }
                return entity;
            }
        }

        private InventoryWIP Update(InventoryWIP entity, SqlTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Picking should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");

            using (var cm = transaction.Connection.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "USP_InventoryWIP";
                cm.Transaction = transaction;

                cm.Parameters.AddWithValue("@LocationId", entity.Location.Key);
                cm.Parameters.AddWithValue("@PalletId", entity.Pallet.Key);
                cm.Parameters.AddWithValue("@Qty", entity.Quantity);
                cm.Parameters.AddWithValue("@Status", entity.Status);
                cm.Parameters.AddWithValue("@Batch", entity.Batch.IsNotNullOrWhiteSpace() ? entity.Batch : "");
                cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                cm.Parameters.AddWithValue("@Id", entity.InventoryId);
                cm.Parameters.AddWithValue("@Type", "UpdateInventory");
                cm.ExecuteNonQuery();
                entity.MarkAsOld();
            }
            return entity;
        }

        public void Delete(long id)
        {
        }
    }
}