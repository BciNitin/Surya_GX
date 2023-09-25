using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class InwardOrderFactoryForPalletizaiton
    {
        #region Inward Order

        public void Delete(long grnId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_InwardOrderPalletization";
                    cmd.Parameters.AddWithValue("@Id", grnId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DELETEORDER");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public InwardOrder Fetch(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Grn id is required for get grn");
            InwardOrder InwardOrder = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INWARDORDERID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GETORDERBYID"));
                    cmd.CommandText = "Usp_InwardOrderPalletization";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            InwardOrder = GetGRN(dr);
                        }
                    }
                }
            }

            return InwardOrder;
        }

        public List<InwardOrder> FetchInwardOrders(InwardOrderSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<InwardOrder> OrderDetails = new List<InwardOrder>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderPalletization";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHORDERS"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@DocumentNo", criteria.DocumentNo));
                    cm.Parameters.Add(new SqlParameter("@INWARDORDERTYPEID", criteria.InwardOrderTypeId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            OrderDetails.Add(GetGRN(dr));
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public List<InwardOrder> FetchRequisitionOrders(InwardOrderSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<InwardOrder> OrderDetails = new List<InwardOrder>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderPalletization";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHREQUISITIONORDERS"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@DocumentNo", criteria.DocumentNo));
                    cm.Parameters.Add(new SqlParameter("@INWARDORDERTYPEID", criteria.InwardOrderTypeId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            OrderDetails.Add(GetRequisition(dr));
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public List<InwardOrder> FetchVendorReturnOrders(InwardOrderSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<InwardOrder> OrderDetails = new List<InwardOrder>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderPalletization";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHVENDORRETURNORDERS"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@DocumentNo", criteria.DocumentNo));
                    cm.Parameters.Add(new SqlParameter("@INWARDORDERTYPEID", criteria.InwardOrderTypeId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            OrderDetails.Add(GetVendorReturn(dr));
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public List<KeyValue<long, string>> FetchGRNNos(InwardOrderSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<KeyValue<long, string>> OrderDetails = new List<KeyValue<long, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderPalletization";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHORDERNOS"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@DocumentNo", criteria.DocumentNo));
                    cm.Parameters.Add(new SqlParameter("@INWARDORDERTYPEID", criteria.InwardOrderTypeId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            OrderDetails.Add(new KeyValue<long, string>() { Key = dr.GetInt64("Id"), Value = dr.GetString("DocumentNo") });
                        }
                    }
                }
            }
            return OrderDetails;
        }

        private InwardOrder GetGRN(SafeDataReader dr)
        {
            InwardOrder InwardOrder = new InwardOrder();
            InwardOrder.Id = dr.GetInt64("Id");
            InwardOrder.DocumentNo = dr.GetString("DocumentNo");
            InwardOrder.DocumentYear = dr.GetString("DocumentYear");
            InwardOrder.DocumentDate = dr.GetDateTime("DocumentDate");
            InwardOrder.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PLANTCODE") };
            InwardOrder.GRNStatus = (GRNStatus)dr.GetInt32("Status");
            InwardOrder.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrder.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CREATEDBYNAME") };
            InwardOrder.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("MODIFIEDBYNAME") };
            InwardOrder.ModifiedOn = dr.GetDateTime("ModifiedOn");
            InwardOrder.Items = GetGrnItemsByGRNId(InwardOrder.Id);
            return InwardOrder;
        }

        private InwardOrder GetRequisition(SafeDataReader dr)
        {
            InwardOrder InwardOrder = new InwardOrder();
            InwardOrder.Id = dr.GetInt64("Id");
            InwardOrder.DocumentNo = dr.GetString("DocumentNo");
            InwardOrder.DocumentYear = dr.GetString("DocumentYear");
            InwardOrder.DocumentDate = dr.GetDateTime("DocumentDate");
            InwardOrder.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PLANTCODE") };
            InwardOrder.GRNStatus = (GRNStatus)dr.GetInt32("Status");
            InwardOrder.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrder.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CREATEDBYNAME") };
            InwardOrder.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("UpdatedBy"), Value = dr.GetString("MODIFIEDBYNAME") };
            InwardOrder.ModifiedOn = dr.GetDateTime("UpdatedOn");
            InwardOrder.Items = GetRequisitionItemsById(InwardOrder.Id);
            return InwardOrder;
        }

        private InwardOrder GetVendorReturn(SafeDataReader dr)
        {
            InwardOrder InwardOrder = new InwardOrder();
            InwardOrder.Id = dr.GetInt64("Id");
            InwardOrder.DocumentNo = dr.GetString("DocumentNo");
            InwardOrder.DocumentYear = dr.GetString("DocumentYear");
            InwardOrder.DocumentDate = dr.GetDateTime("DocumentDate");
            InwardOrder.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("SiteId"), Value = dr.GetString("PLANTCODE") };
            InwardOrder.GRNStatus = (GRNStatus)dr.GetInt32("Status");
            InwardOrder.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrder.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CREATEDBYNAME") };
            InwardOrder.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("UpdatedBy"), Value = dr.GetString("MODIFIEDBYNAME") };
            InwardOrder.ModifiedOn = dr.GetDateTime("UpdatedOn");
            InwardOrder.Items = GetVendorItemsByGRNId(InwardOrder.Id);
            return InwardOrder;
        }

        public InwardOrder Insert(InwardOrder entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "GRN should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"GRN Order: {entity.DocumentNo} already exists.");

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_InwardOrderPalletization";
                        cm.Parameters.AddWithValue("@DocumentDate", entity.DocumentDate);
                        cm.Parameters.AddWithValue("@DocumentNo", entity.DocumentNo);
                        cm.Parameters.AddWithValue("@DocumentYear", entity.DocumentYear);
                        cm.Parameters.AddWithValue("@PlantId", entity.Plant.IsNotNull() ? entity.Plant.Key : 0);
                        cm.Parameters.AddWithValue("@Status", entity.GRNStatus);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "INSERT");
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

        public bool IsExists(InwardOrder entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Hardware should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DocumentNo", entity.DocumentNo);
                cmd.Parameters.AddWithValue("@PlantId", entity.Plant.Key);
                cmd.Parameters.AddWithValue("@INWARDORDERID", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "ISEXISTS");
                cmd.CommandText = "Usp_InwardOrderPalletization";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public InwardOrder Update(InwardOrder entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "GRN should not be null");
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
                        cm.CommandText = "Usp_InwardOrderPalletization";

                        cm.Parameters.AddWithValue("@DocumentDate", entity.DocumentDate.DateTime);
                        cm.Parameters.AddWithValue("@DocumentNo", entity.DocumentNo);
                        cm.Parameters.AddWithValue("@DocumentYear", entity.DocumentYear);
                        cm.Parameters.AddWithValue("@PlantId", entity.Plant.IsNotNull() ? entity.Plant.Key : 0);
                        cm.Parameters.AddWithValue("@Status", entity.GRNStatus);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "UPDATE");
                        cm.Parameters.AddWithValue("@INWARDORDERID", entity.Id);
                        cm.ExecuteNonQuery();
                        //entity.Id = Convert.ToInt64(cm.ExecuteScalar());
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

        #endregion Inward Order

        #region GRN Items

        public List<InwardOrderItem> GetGrnItemsByGRNId(long grnId)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is required to get items");
            List<InwardOrderItem> items = new List<InwardOrderItem>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderPalletization";
                    cm.Parameters.Add(new SqlParameter("@InwardOrderId", grnId));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHORDERITEMSBYORDERID"));

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

        public List<InwardOrderItem> GetRequisitionItemsById(long grnId)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is required to get items");
            List<InwardOrderItem> items = new List<InwardOrderItem>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderPalletization";
                    cm.Parameters.Add(new SqlParameter("@InwardOrderId", grnId));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHREQORDERITEMSBYORDERID"));

                    using (SafeDataReader read = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (read.Read())
                        {
                            items.Add(GetReqItemdetails(read));
                        }
                    }
                }
            }
            return items;
        }

        public List<InwardOrderItem> GetVendorItemsByGRNId(long grnId)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is required to get items");
            List<InwardOrderItem> items = new List<InwardOrderItem>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_InwardOrderPalletization";
                    cm.Parameters.Add(new SqlParameter("@InwardOrderId", grnId));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHVENDORORDERITEMSBYORDERID"));

                    using (SafeDataReader read = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (read.Read())
                        {
                            items.Add(GetVendorItemdetails(read));
                        }
                    }
                }
            }
            return items;
        }

        private InwardOrderItem GetReqItemdetails(SafeDataReader dr)
        {
            InwardOrderItem item = new InwardOrderItem();
            item.Id = dr.GetInt64("Id");
            item.OrderId = dr.GetInt64("RMPickOrderId");
            item.Material = new KeyInfo<int, string, string>() { Key = dr.GetInt32("MaterialId"), KeyCode = dr.GetString("MaterialCode"), Value = dr.GetString("MaterialDescription") };
            item.MaterialItemLine = dr.GetString("ItemLineNo");
            item.Quantity = dr.GetInt32("Quantity");
            item.PrintedQty = dr.GetInt32("PickedQty");
            //item.Status = (GRNItemStatus)dr.GetInt32("Status");
            item.UOM = new KeyValue<short, string>() { Key = dr.GetInt16("UOMID"), Value = dr.GetString("UOMCOde") };
            item.Batch = dr.GetString("Batch");
            item.StorageLocation = dr.GetString("LocationToPick");
            //item.PurchaseOrderLineItem = dr.GetString("PurchaseOrderLineItem");
            item.PurchaseOrderNo = dr.GetString("ERPDocLineNo");
            //item.CreatedOn = dr.GetDateTime("CreatedOn");
            //item.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            //item.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            //item.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return item;
        }

        private InwardOrderItem GetVendorItemdetails(SafeDataReader dr)
        {
            InwardOrderItem item = new InwardOrderItem();
            item.Id = dr.GetInt64("Id");
            item.OrderId = dr.GetInt64("OutwardInvoiceId");
            item.Material = new KeyInfo<int, string, string>() { Key = dr.GetInt32("MaterialId"), KeyCode = dr.GetString("MaterialCode"), Value = dr.GetString("MaterialDescription") };
            //item.MaterialItemLine = dr.GetString("ItemLineNo");
            item.Quantity = dr.GetInt32("Quantity");
            //item.PrintedQty = dr.GetInt32("PickedQty");
            //item.Status = (GRNItemStatus)dr.GetInt32("Status");
            item.UOM = new KeyValue<short, string>() { Key = dr.GetInt16("UOMID"), Value = dr.GetString("UOMCOde") };
            item.Batch = dr.GetString("Batch");
            item.StorageLocation = dr.GetString("LocationToPick");
            //item.PurchaseOrderLineItem = dr.GetString("PurchaseOrderLineItem");
            item.PurchaseOrderNo = dr.GetString("ERPDocLineNo");
            //item.CreatedOn = dr.GetDateTime("CreatedOn");
            //item.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            //item.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            //item.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return item;
        }

        private InwardOrderItem Getdetails(SafeDataReader dr)
        {
            InwardOrderItem item = new InwardOrderItem();
            item.Id = dr.GetInt64("Id");
            item.OrderId = dr.GetInt64("InwardOrderId");
            item.Material = new KeyInfo<int, string, string>() { Key = dr.GetInt32("MaterialId"), KeyCode = dr.GetString("MaterialCode"), Value = dr.GetString("MaterialDescription") };
            item.MaterialItemLine = dr.GetString("MaterialItemLine");
            item.Quantity = dr.GetInt32("Quantity");
            item.PrintedQty = dr.GetInt32("PrintedQty");
            item.Status = (GRNItemStatus)dr.GetInt32("Status");

            item.UOM = new KeyValue<short, string>() { Key = dr.GetInt16("UOMID"), Value = dr.GetString("UOMCOde") };
            item.Batch = dr.GetString("Batch");
            item.StorageLocation = dr.GetString("StorageLocation");
            item.PurchaseOrderLineItem = dr.GetString("PurchaseOrderLineItem");
            item.PurchaseOrderNo = dr.GetString("PurchaseOrderNo");
            item.CreatedOn = dr.GetDateTime("CreatedOn");
            //item.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            //item.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            item.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return item;
        }

        public List<InwardOrderItem> SaveItems(List<InwardOrderItem> items, long grnId, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN order id is required to Save grn item.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            foreach (InwardOrderItem _item in items)
            {
                if (_item.IsNew && _item.Id <= 0)
                {
                    _item.OrderId = grnId;
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

        private InwardOrderItem InsertItem(InwardOrderItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "GRN order item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_InwardOrderPalletization";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "InsertItem"));
                cmd.Parameters.Add(new SqlParameter("@InwardOrderId", entity.OrderId));
                cmd.Parameters.Add(new SqlParameter("@Quantity", entity.Quantity));
                cmd.Parameters.Add(new SqlParameter("@PrintedQty", entity.PrintedQty > 0 ? entity.PrintedQty : 0));
                cmd.Parameters.Add(new SqlParameter("@MaterialId", entity.Material.Key));
                cmd.Parameters.Add(new SqlParameter("@UOMId", entity.UOM.IsNotNull() ? entity.UOM.Key : 1));
                cmd.Parameters.Add(new SqlParameter("@MaterialItemLine", entity.MaterialItemLine.IsNotNullOrWhiteSpace() ? entity.MaterialItemLine : ""));
                cmd.Parameters.Add(new SqlParameter("@ItemStatus", entity.Status));
                cmd.Parameters.Add(new SqlParameter("@Batch", entity.Batch.IsNotNullOrWhiteSpace() ? entity.Batch : ""));
                cmd.Parameters.Add(new SqlParameter("@PurchaseOrderNo", entity.PurchaseOrderNo.IsNotNullOrWhiteSpace() ? entity.PurchaseOrderNo : ""));
                cmd.Parameters.Add(new SqlParameter("@PurchaseOrderLineItem", entity.PurchaseOrderLineItem.IsNotNullOrWhiteSpace() ? entity.PurchaseOrderLineItem : ""));
                cmd.Parameters.Add(new SqlParameter("@StorageLocation", entity.StorageLocation.IsNotNullOrWhiteSpace() ? entity.StorageLocation : ""));
                entity.Id = Convert.ToInt64(cmd.ExecuteScalar());
                entity.MarkAsOld();
                return entity;
            }
        }

        private InwardOrderItem UpdateItem(InwardOrderItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "GRN order item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_InwardOrderPalletization";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "UpdateItem"));
                cmd.Parameters.Add(new SqlParameter("@PrintedQty", entity.PrintedQty > 0 ? entity.PrintedQty : 0));
                cmd.Parameters.Add(new SqlParameter("@ItemStatus", entity.Status));
                cmd.Parameters.Add(new SqlParameter("@INWARDORDERITEMID", entity.Id));
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

                cmd.CommandText = "Usp_InwardOrderPalletization";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "DeleteItem"));
                cmd.Parameters.Add(new SqlParameter("@INWARDORDERITEMID", itemid));
                cmd.ExecuteNonQuery();
            }
        }

        #endregion GRN Items

        #region QA

        public string SaveQC(QualityStatusInfo quality)
        {
            CodeContract.Required<ArgumentException>(quality.IsNotNull(), "Quality status is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_QUALITYSTATUS";
                    cmd.Parameters.AddWithValue("@PlantId", quality.PlantId);
                    cmd.Parameters.AddWithValue("@OrderId", quality.InwardOrderId);
                    cmd.Parameters.AddWithValue("@CompletedOrderQC", quality.CompletedOrderQC);
                    cmd.Parameters.AddWithValue("@InwardOrderItemId", quality.InwardOrderItemId);
                    cmd.Parameters.AddWithValue("@Status", quality.Status);
                    cmd.Parameters.AddWithValue("@Type", "SaveQC");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        #endregion QA

        public string GetValidPallet(string code, int plantId)
        {
            CodeContract.Required<ArgumentException>(code.IsNotNullOrWhiteSpace(), "Pallet code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_PALLETIZATION";
                    cmd.Parameters.AddWithValue("@PalletCode", code);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "GetPallet");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }
    }
}