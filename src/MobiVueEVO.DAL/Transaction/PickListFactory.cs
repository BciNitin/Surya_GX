using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class PickListFactory
    {
        #region Picklist

        public void Delete(long pickId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(pickId > 0, "PickList is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Picklist";
                    cmd.Parameters.AddWithValue("@Id", pickId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeletePicklist");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public PickList Fetch(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "PickList id is required");
            PickList Pick = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetPicklistById"));
                    cmd.CommandText = "Usp_Picklist";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            Pick = GetPick(dr);
                        }
                    }
                }
            }

            return Pick;
        }

        public List<RMPickOrder> FetchRMPickOrders(RMPickrderSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<RMPickOrder> OrderDetails = new List<RMPickOrder>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetRMPickOrders"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@RMPickOrderNo", criteria.OrderNo));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            OrderDetails.Add(GetRMPickOrder(dr));
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public RMPickOrder FetchRMPickOrder(long rmPickOrderId)
        {
            CodeContract.Required<ArgumentException>(rmPickOrderId > 0, "Search criteria cannot be null");
            RMPickOrder OrderDetail = null;
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.Add(new SqlParameter("@RMPickOrderId", rmPickOrderId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetRMPickOrder"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            OrderDetail = GetRMPickOrder(dr);
                        }
                    }
                }
            }
            return OrderDetail;
        }

        private RMPickOrder GetRMPickOrder(SafeDataReader dr)
        {
            RMPickOrder InwardOrder = new RMPickOrder();
            InwardOrder.Id = dr.GetInt64("Id");
            InwardOrder.OrderNo = dr.GetString("OrderNo");
            InwardOrder.OrderDate = dr.GetDateTime("OrderDate");
            InwardOrder.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PLANTCODE") };
            InwardOrder.OrderStatus = (RMPickOrderStatus)dr.GetInt32("Status");
            InwardOrder.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrder.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            InwardOrder.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("UpdatedBy"), Value = dr.GetString("ModifiedByName") };
            //InwardOrder.ModifiedOn = dr.GetDateTime("ModifiedOn");
            //InwardOrder.Items = GetRMPickItemsByOrderId(InwardOrder.Id);
            return InwardOrder;
        }

        public List<RMPickOrderItem> GetRMPickItemsByOrderId(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "GRN id is required to get items");
            List<RMPickOrderItem> items = new List<RMPickOrderItem>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.Add(new SqlParameter("@RmPickOrderID", id));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetRMPickOrderItemById"));

                    using (SafeDataReader read = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (read.Read())
                        {
                            items.Add(GetOrderdetails(read));
                        }
                    }
                }
            }
            return items;
        }

        private RMPickOrderItem GetOrderdetails(SafeDataReader dr)
        {
            RMPickOrderItem item = new RMPickOrderItem();
            item.Id = dr.GetInt64("Id");
            item.OrderId = dr.GetInt64("RMPickOrderId");
            item.Material = new KeyInfo<int, string, string>() { Key = dr.GetInt32("MaterialId"), KeyCode = dr.GetString("MaterialCode"), Value = dr.GetString("MaterialDescription") };
            item.MaterialItemLine = dr.GetString("ItemLineNo");
            item.Quantity = dr.GetInt32("Quantity");
            item.PickedQty = dr.GetInt32("PickedQty");
            item.UOM = new KeyValue<short, string>() { Key = dr.GetInt16("UOMID"), Value = dr.GetString("UOMCode") };
            item.Batch = dr.GetString("Batch");
            item.LocationToPick = dr.GetString("LocationToPick");
            return item;
        }

        public List<PickList> Fetchpicklists(PickListSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<PickList> OrderDetails = new List<PickList>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FetchPicklists"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@RMPickOrderNo", criteria.RMOrderNo));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            OrderDetails.Add(GetPick(dr));
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public List<KeyValue<long, string>> FetchPicklists(PickListSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<KeyValue<long, string>> OrderDetails = new List<KeyValue<long, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FetchPicklistNos"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            OrderDetails.Add(new KeyValue<long, string>() { Key = dr.GetInt64("Id"), Value = dr.GetString("Name") });
                        }
                    }
                }
            }
            return OrderDetails;
        }

        private PickList GetPick(SafeDataReader dr)
        {
            PickList pick = new PickList();
            pick.Id = dr.GetInt64("Id");
            pick.Name = dr.GetString("Name");
            pick.RmPickOrder = new KeyValue<long, string>() { Key = dr.GetInt64("RmPickOrderId"), Value = dr.GetString("OrderNo") };
            pick.CreatedOn = dr.GetDateTime("CreatedOn");
            pick.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            pick.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            pick.ModifiedOn = dr.GetDateTime("ModifiedOn");
            pick.Items = GetPickItemsBypickId(pick.Id);
            return pick;
        }

        public PickList Insert(PickList entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "PickList should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_Picklist";
                        cm.Parameters.AddWithValue("@PlantId", entity.Plant.IsNotNull() ? entity.Plant.Key : 0);
                        cm.Parameters.AddWithValue("@RmPickOrderID", entity.RmPickOrder.Key);
                        cm.Parameters.AddWithValue("@Status", entity.Status);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);

                        cm.Parameters.AddWithValue("@Type", "Insert");
                        entity.Id = Convert.ToInt64(cm.ExecuteScalar());
                        if (entity.Items.HaveItems())
                        {
                            entity.Items = SaveItems(entity.Items, entity.Id, transaction);
                            entity.Items.RemoveAll(x => x.IsDeleted);
                        }
                        transaction.Commit();
                        entity.MarkAsOld();
                        return entity;
                    }
                }
            }
        }

        public string PickListGeneration(long orderId)
        {
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.AddWithValue("@RmPickOrderID", orderId);
                    cm.Parameters.AddWithValue("@Type", "PicklistGeneration");
                    return cm.ExecuteScalar().ToString();
                    // int result = (int)cm.ExecuteScalar();
                    // return result.ToString();
                }
            }
        }

        public bool IsExists(PickList entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "PickList should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@PlantId", entity.Plant.Key);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_Picklist";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public PickList Update(PickList entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "PickList should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_Picklist";
                        cm.Parameters.AddWithValue("@status", entity.Status);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "Update");
                        cm.Parameters.AddWithValue("@Id", entity.Id);
                        entity.Id = Convert.ToInt64(cm.ExecuteScalar());
                        if (entity.Items.HaveItems())
                        {
                            entity.Items = SaveItems(entity.Items, entity.Id, transaction);
                            entity.Items.RemoveAll(x => x.IsDeleted);
                        }
                        transaction.Commit();
                        entity.MarkAsOld();
                        return entity;
                    }
                }
            }
        }

        #endregion Picklist

        #region Picklist Items

        public List<PickListItem> GetPickItemsBypickId(long pickId)
        {
            CodeContract.Required<ArgumentException>(pickId > 0, "PickList id is required to get items");
            List<PickListItem> items = new List<PickListItem>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.Add(new SqlParameter("@Id", pickId));
                    cm.Parameters.Add(new SqlParameter("@Type", "FetchItemsByPicklistId"));

                    using (SafeDataReader read = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (read.Read())
                        {
                            items.Add(Getdetails(read));
                        }
                    }
                }
            }
            return items;
        }

        private PickListItem Getdetails(SafeDataReader dr)
        {
            PickListItem item = new PickListItem();
            item.Id = dr.GetInt64("Id");
            item.Location = new KeyValue<long, string>() { Key = dr.GetInt32("LocationId"), Value = dr.GetString("LocationCode") };
            item.Pallet = new KeyValue<long, string>() { Key = dr.GetInt64("PalletId"), Value = dr.GetString("PalletCode") };
            item.Material = new KeyInfo<int, string, string>() { Key = dr.GetInt32("MaterialId"), KeyCode = dr.GetString("MaterialCode"), Value = dr.GetString("MaterialDescription") };
            item.Qty = dr.GetInt32("Qty");
            item.PickedQty = dr.GetInt32("PickedQty");
            item.PicklistId = dr.GetInt64("PicklistId");
            item.IsPicked = dr.GetBoolean("IsPicked");
            item.CreatedOn = dr.GetDateTime("CreatedOn");

            return item;
        }

        public List<PickListItem> SaveItems(List<PickListItem> items, long picklistId, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(picklistId > 0, "PickList id is required to Save item.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            foreach (PickListItem _item in items)
            {
                if (_item.IsNew)
                {
                    _item.PicklistId = picklistId;
                    InsertItem(_item, transaction);
                }
                else if (_item.IsDeleted)
                {
                    DeleteItem(_item.Id, transaction);
                }
                else
                {
                    UpdateItem(_item, transaction);
                }
            }
            return items;
        }

        private PickListItem InsertItem(PickListItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "PickList item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_Picklist";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "InsertItem"));
                cmd.Parameters.Add(new SqlParameter("@Id", entity.PicklistId));
                cmd.Parameters.Add(new SqlParameter("@Qty", entity.Qty));
                cmd.Parameters.Add(new SqlParameter("@MaterialId", entity.Material.Key));
                cmd.Parameters.Add(new SqlParameter("@LocationId", entity.Location.Key));
                cmd.Parameters.Add(new SqlParameter("@PalletId", entity.Pallet.Key));
                cmd.Parameters.Add(new SqlParameter("@Status", entity.Status));
                cmd.Parameters.Add(new SqlParameter("@RmPickOrderID", entity.RMPickOrderId));
                cmd.Parameters.Add(new SqlParameter("@RMPickOrderItemId", entity.RMPickOrderItemId));
                cmd.Parameters.Add(new SqlParameter("@IsPicked", entity.IsPicked));
                cmd.Parameters.Add(new SqlParameter("@ModifiedBy", entity.IsPicked));

                entity.Id = Convert.ToInt64(cmd.ExecuteScalar());
                entity.MarkAsOld();
                return entity;
            }
        }

        private PickListItem UpdateItem(PickListItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "PickList item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_Picklist";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "UpdateItem"));
                cmd.Parameters.Add(new SqlParameter("@PIckedQty", entity.PickedQty));
                cmd.Parameters.Add(new SqlParameter("@IsPicked", entity.IsPicked));
                cmd.Parameters.Add(new SqlParameter("@ItemStatus", entity.Status));
                cmd.Parameters.Add(new SqlParameter("@PickListItemId", entity.Id));
                cmd.ExecuteNonQuery();
                entity.MarkAsOld();
                return entity;
            }
        }

        private void DeleteItem(long itemid, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(itemid > 0, "Item id is required for delete item.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "Usp_Picklist";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "DeleteItem"));
                cmd.Parameters.Add(new SqlParameter("@PickListItemId", itemid));
                cmd.ExecuteNonQuery();
            }
        }

        #endregion Picklist Items

        #region rsPalletOrders

        public List<KeyValue<long, string>> FetchPickPalletOrders(PickListSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<KeyValue<long, string>> OrderDetails = new List<KeyValue<long, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_Picklist";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "PALLETORDER"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            OrderDetails.Add(new KeyValue<long, string>() { Key = dr.GetInt64("ID"), Value = dr.GetString("PORDER") });
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public string GetValidPallet(string code, int plantId, long picklistid)
        {
            CodeContract.Required<ArgumentException>(code.IsNotNullOrWhiteSpace(), "Pallet code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Picklist";
                    cmd.Parameters.AddWithValue("@PalletCode", code);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "GetPallet");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        #endregion rsPalletOrders
    }
}