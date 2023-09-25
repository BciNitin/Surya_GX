using MobiVUE.DAL;
using System;
using System.Text;
using MobiVUE.Inventory.BO;
using System.Data.Common;
using System.Data.SqlClient;
using MobiVUE.Utility;
using System.Collections.Generic;
using MobiVUE.Inventory.BO.Enums;

namespace MobiVUE.Data.SQL
{
    public class PalletItemFactory : IPalletItemFactory
    {
        public void Delete(long itemid, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(itemid > 0, "Item id is required for delete item.");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_pallet";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@ItemId", itemid));
                cmd.Parameters.Add(new SqlParameter("@Type", "DeleteItem"));
                cmd.ExecuteNonQuery();
            }
        }

        public PalletItem Fetch(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Inward order item id is required for get");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            PalletItem PalletItem = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    PalletItem = Fetch(id, transaction);
                    transaction.Commit();
                }
            }
            return PalletItem;
        }

        public PalletItem Fetch(long itemId, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(itemId > 0, "Item id is required for get item.");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            PalletItem items = null;
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_pallet";
                cmd.Parameters.Add(new SqlParameter("@ItemId", itemId));
                cmd.Parameters.Add(new SqlParameter("@Type", "FetchByItemId"));

                using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                {
                    if (dr.Read())
                    {
                        items = GetPalletItem(dr);
                    }
                }
            }
            return items;
        }

        private PalletItem GetPalletItem(SafeDataReader dr)
        {
            PalletItem palletItem = new PalletItem();
            palletItem.Id = dr.GetInt64("PalletItemId");
            palletItem.PalletId = dr.GetInt64("PalletId");
            palletItem.MaterialDescription = dr.GetString("MaterialDescription");
            palletItem.Quantity = dr.GetInt32("Quantity");
            palletItem.Location = new KeyValue<long, string>() { Key = dr.GetInt64("LocationId"), Value = dr.GetString("LocationCode") };
            palletItem.Inventory = new KeyValue<long, string>() { Key = dr.GetInt64("InventoryId"), Value = dr.GetString("BarcodeNumber") };
            palletItem.Material = new KeyValue<long, string> { Key = dr.GetInt64("MaterialId"), Value = dr.GetString("MaterialCode") };
            palletItem.MarkAsOld();
            return palletItem;
        }

        public BusinessListBase<PalletItem> FetchByPalletId(long palletId)
        {
            CodeContract.Required<ArgumentException>(palletId > 0, "Pallet id is required for get pallet items.");
            BusinessListBase<PalletItem> items = new BusinessListBase<PalletItem>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_pallet";
                    cmd.Parameters.Add(new SqlParameter("@Id", palletId));
                    cmd.Parameters.Add(new SqlParameter("@Type", "FetchByPalletId"));

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            items.Add(GetPalletItem(dr));
                        }
                    }
                }
            }
            return items;
        }

        public List<PalletItem> Save(List<PalletItem> items, long palletId, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(palletId > 0, "Pallet id is required to save pallet items.");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            foreach (PalletItem _item in items)
            {
                if (_item.IsNew)
                {
                    _item.PalletId = palletId;
                    Insert(_item, transaction);
                }
                else if (_item.IsDeleted)
                {
                    Delete(_item.Id, transaction);
                }
                else
                {
                    Update(_item, transaction);
                }
            }
            return items;
        }

        public PalletItem Insert(PalletItem entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Pallet item should not be null.");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            entity.Validate();
            using (SqlConnection sqlConnection = new SqlConnection(DBConfig.SQLConnectionString))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    entity = Insert(entity, transaction);
                    transaction.Commit();
                }
            }
            return entity;
        }

        public PalletItem Insert(PalletItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Pallet item should not be null.");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_pallet";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Id", entity.PalletId));
                cmd.Parameters.Add(new SqlParameter("@MaterialId", entity.Material.Key));
                cmd.Parameters.Add(new SqlParameter("@InventoryId", entity.Inventory.Key));
                cmd.Parameters.Add(new SqlParameter("@BarcodeNumber", entity.Inventory.Value));
                cmd.Parameters.Add(new SqlParameter("@LocationId", entity.Location.IsNotNull() ? entity.Location.Key : 0));
                cmd.Parameters.Add(new SqlParameter("@Qty", entity.Quantity));
                cmd.Parameters.Add(new SqlParameter("@Type", "InsertItem"));
                entity.Id = Convert.ToInt64(cmd.ExecuteScalar());

                entity.MarkAsOld();
                return entity;
            }
        }

        public PalletItem Update(PalletItem entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Pallet item should not be null.");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            entity.Validate();
            using (SqlConnection sqlConnection = new SqlConnection(DBConfig.SQLConnectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    entity = Update(entity, transaction);
                    transaction.Commit();
                }
                return entity;
            }
        }

        public PalletItem Update(PalletItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Receiving item should not be null.");
            CodeContract.Required<ArgumentException>(LogOnContext.LogOnUser.IsNotNull(), "Login context should not be null");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_pallet";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@ItemId", entity.Id));
                cmd.Parameters.Add(new SqlParameter("@Id", entity.PalletId));
                cmd.Parameters.Add(new SqlParameter("@MaterialId", entity.Material.Key));
                cmd.Parameters.Add(new SqlParameter("@InventoryId", entity.Inventory.Key));
                cmd.Parameters.Add(new SqlParameter("@BarcodeNumber", entity.Inventory.Value));
                cmd.Parameters.Add(new SqlParameter("@LocationId", entity.Location.IsNotNull() ? entity.Location.Key : 0));
                cmd.Parameters.Add(new SqlParameter("@Qty", entity.Quantity));
                cmd.Parameters.Add(new SqlParameter("@Type", "UpdateItem"));
                cmd.ExecuteNonQuery();
                entity.MarkAsOld();
                return entity;
            }
        }
    }
}