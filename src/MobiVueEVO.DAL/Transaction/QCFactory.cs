using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class QCFactory
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
                    cmd.CommandText = "Usp_QCReport";
                    cmd.Parameters.AddWithValue("@Id", grnId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DELETEORDER");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public InwardOrderForQC Fetch(long id, int forQCorStore)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Grn id is required for get grn");
            InwardOrderForQC InwardOrderForQC = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INWARDORDERID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GETORDERBYID"));
                    cmd.CommandText = "Usp_QCReport";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            InwardOrderForQC = GetGRN(dr, forQCorStore);
                        }
                    }
                }
            }

            return InwardOrderForQC;
        }

        public List<InwardOrderForQC> FetchInwardOrders(InwardOrderForQCSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<InwardOrderForQC> OrderDetails = new List<InwardOrderForQC>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
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
                            OrderDetails.Add(GetGRN(dr, 1));
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public List<InwardOrderForQC> FetchRequisitionOrders(InwardOrderSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<InwardOrderForQC> OrderDetails = new List<InwardOrderForQC>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
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

        public List<InwardOrderForQC> FetchVendorReturnOrders(InwardOrderSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<InwardOrderForQC> OrderDetails = new List<InwardOrderForQC>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
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
                    cm.CommandText = "Usp_QCReport";
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

        public List<KeyValue<long, string>> FetchGRNNosWhoseQCNotDone(InwardOrderForQCSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<KeyValue<long, string>> OrderDetails = new List<KeyValue<long, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHORDERNOSWHEREQCNOTDONE"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    //cm.Parameters.Add(new SqlParameter("@DocumentNo", criteria.DocumentNo));
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


        private InwardOrderForQC GetGRN(SafeDataReader dr, int forQCorStore)
        {
            InwardOrderForQC InwardOrderForQC = new InwardOrderForQC();
            InwardOrderForQC.Id = dr.GetInt64("Id");
            InwardOrderForQC.DocumentNo = dr.GetString("DocumentNo");
            InwardOrderForQC.DocumentYear = dr.GetString("DocumentYear");
            InwardOrderForQC.DocumentDate = dr.GetDateTime("DocumentDate");
            InwardOrderForQC.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PLANTCODE") };
            InwardOrderForQC.GRNStatus = (GRNStatus)dr.GetInt32("Status");
            InwardOrderForQC.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrderForQC.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CREATEDBYNAME") };
            InwardOrderForQC.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("MODIFIEDBYNAME") };
            InwardOrderForQC.ModifiedOn = dr.GetDateTime("ModifiedOn");
            if (forQCorStore == 1)
                InwardOrderForQC.Items = GetGrnItemsByGRNId(InwardOrderForQC.Id);
            else if (forQCorStore == 2)
                InwardOrderForQC.Items = GetGrnItemsByGRNIdForSTore(InwardOrderForQC.Id);
            return InwardOrderForQC;
        }

        private InwardOrderForQC GetRequisition(SafeDataReader dr)
        {
            InwardOrderForQC InwardOrderForQC = new InwardOrderForQC();
            InwardOrderForQC.Id = dr.GetInt64("Id");
            InwardOrderForQC.DocumentNo = dr.GetString("DocumentNo");
            InwardOrderForQC.DocumentYear = dr.GetString("DocumentYear");
            InwardOrderForQC.DocumentDate = dr.GetDateTime("DocumentDate");
            InwardOrderForQC.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PLANTCODE") };
            InwardOrderForQC.GRNStatus = (GRNStatus)dr.GetInt32("Status");
            InwardOrderForQC.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrderForQC.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CREATEDBYNAME") };
            InwardOrderForQC.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("UpdatedBy"), Value = dr.GetString("MODIFIEDBYNAME") };
            InwardOrderForQC.ModifiedOn = dr.GetDateTime("UpdatedOn");
            InwardOrderForQC.Items = GetRequisitionItemsById(InwardOrderForQC.Id);
            return InwardOrderForQC;
        }

        private InwardOrderForQC GetVendorReturn(SafeDataReader dr)
        {
            InwardOrderForQC InwardOrderForQC = new InwardOrderForQC();
            InwardOrderForQC.Id = dr.GetInt64("Id");
            InwardOrderForQC.DocumentNo = dr.GetString("DocumentNo");
            InwardOrderForQC.DocumentYear = dr.GetString("DocumentYear");
            InwardOrderForQC.DocumentDate = dr.GetDateTime("DocumentDate");
            InwardOrderForQC.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("SiteId"), Value = dr.GetString("PLANTCODE") };
            InwardOrderForQC.GRNStatus = (GRNStatus)dr.GetInt32("Status");
            InwardOrderForQC.CreatedOn = dr.GetDateTime("CreatedOn");
            InwardOrderForQC.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CREATEDBYNAME") };
            InwardOrderForQC.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("UpdatedBy"), Value = dr.GetString("MODIFIEDBYNAME") };
            InwardOrderForQC.ModifiedOn = dr.GetDateTime("UpdatedOn");
            InwardOrderForQC.Items = GetVendorItemsByGRNId(InwardOrderForQC.Id);
            return InwardOrderForQC;
        }

        public InwardOrderForQC Insert(InwardOrderForQC entity)
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
                        cm.CommandText = "Usp_QCReport";
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

        public bool IsExists(InwardOrderForQC entity, SqlConnection con)
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
                cmd.CommandText = "Usp_QCReport";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public InwardOrderForQC Update(InwardOrderForQC entity)
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
                        cm.CommandText = "Usp_QCReport";

                        cm.Parameters.AddWithValue("@DocumentDate", entity.DocumentDate.DateTime);
                        cm.Parameters.AddWithValue("@DocumentNo", entity.DocumentNo);
                        cm.Parameters.AddWithValue("@DocumentYear", entity.DocumentYear);
                        cm.Parameters.AddWithValue("@PlantId", entity.Plant.IsNotNull() ? entity.Plant.Key : 0);
                        cm.Parameters.AddWithValue("@Status", entity.GRNStatus);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "UPDATE");
                        cm.Parameters.AddWithValue("@INWARDORDERID", entity.Id);
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

        public List<InwardOrderItemForQC> GetGrnItemsByGRNId(long grnId)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is required to get items");
            List<InwardOrderItemForQC> items = new List<InwardOrderItemForQC>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
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

        public List<InwardOrderItemForQC> GetGrnItemsByGRNIdForSTore(long grnId)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is required to get items");
            List<InwardOrderItemForQC> items = new List<InwardOrderItemForQC>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
                    cm.Parameters.Add(new SqlParameter("@InwardOrderId", grnId));
                    cm.Parameters.Add(new SqlParameter("@Type", "FETCHORDERITEMSBYORDERIDForSTOREPERSON"));

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

        public List<InwardOrderItemForQC> GetRequisitionItemsById(long grnId)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is required to get items");
            List<InwardOrderItemForQC> items = new List<InwardOrderItemForQC>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
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

        public List<InwardOrderItemForQC> GetVendorItemsByGRNId(long grnId)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN id is required to get items");
            List<InwardOrderItemForQC> items = new List<InwardOrderItemForQC>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
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

        private InwardOrderItemForQC GetReqItemdetails(SafeDataReader dr)
        {
            InwardOrderItemForQC item = new InwardOrderItemForQC();
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

        private InwardOrderItemForQC GetVendorItemdetails(SafeDataReader dr)
        {
            InwardOrderItemForQC item = new InwardOrderItemForQC();
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

        private InwardOrderItemForQC Getdetails(SafeDataReader dr)
        {
            InwardOrderItemForQC item = new InwardOrderItemForQC();
            item.Id = dr.GetInt64("Id");
            item.OrderId = dr.GetInt64("InwardOrderId");
            item.Material = new KeyInfo<int, string, string>() { Key = dr.GetInt32("MaterialId"), KeyCode = dr.GetString("MaterialCode"), Value = dr.GetString("MaterialDescription") };
            item.MaterialItemLine = dr.GetString("MaterialItemLine");
            //item.Quantity = dr.GetInt32("Quantity");
            item.Quantity = dr.GetDecimal("Qty");
            item.PrintedQty = dr.GetInt32("PrintedQty");
            item.Status = (GRNItemStatus)dr.GetInt32("Status");
            item.UOM = new KeyValue<short, string>() { Key = dr.GetInt16("UOMID"), Value = dr.GetString("UOMCOde") };
            item.Batch = dr.GetString("Batch");
            item.StorageLocation = dr.GetString("StorageLocation");
            item.PurchaseOrderLineItem = dr.GetString("PurchaseOrderLineItem");
            item.PurchaseOrderNo = dr.GetString("PurchaseOrderNo");
            item.CreatedOn = dr.GetDateTime("CreatedOn");
            item.QCstatus = dr.GetInt32("QCStatus");
            item.PalletCode = dr.GetString("PalletCode");
            //item.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            //item.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            item.UpdatedOn = dr.GetDateTime("ModifiedOn");
            item.InventoryId = dr.GetInt64("InventoryId");
            return item;
        }

        public List<InwardOrderItemForQC> SaveItems(List<InwardOrderItemForQC> items, long grnId, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(grnId > 0, "GRN order id is required to Save grn item.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            foreach (InwardOrderItemForQC _item in items)
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

        private InwardOrderItemForQC InsertItem(InwardOrderItemForQC entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "GRN order item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_QCReport";
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

        private InwardOrderItemForQC UpdateItem(InwardOrderItemForQC entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "GRN order item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_QCReport";
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

                cmd.CommandText = "Usp_QCReport";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "DeleteItem"));
                cmd.Parameters.Add(new SqlParameter("@INWARDORDERITEMID", itemid));
                cmd.ExecuteNonQuery();
            }
        }

        #endregion GRN Items

        public List<QCReportEntries> FetchInventorysForReport(QCReportEntriesSearchCriteria criteria)
        {
            List<QCReportEntries> Inventerys = new List<QCReportEntries>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_QCReport";
                    cm.Parameters.Add(new SqlParameter("@Type", "GetInventories"));
                    //cm.Parameters.Add(new SqlParameter("@BarcodeNumber", criteria.Barcode.IsNotNullOrWhiteSpace() ? criteria.Barcode : ""));
                    //cm.Parameters.Add(new SqlParameter("@MaterialCode", criteria.MaterialCode.IsNotNullOrWhiteSpace() ? criteria.MaterialCode : ""));
                    //cm.Parameters.Add(new SqlParameter("@MaterialId", criteria.MaterialId));
                    //cm.Parameters.Add(new SqlParameter("@PalletId", criteria.PalletId));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@INWARDORDERID", criteria.GRNId));
                    cm.Parameters.Add(new SqlParameter("@Status", criteria.Status));
                    //cm.Parameters.Add(new SqlParameter("@LocationId", criteria.LocationId));
                    cm.Parameters.Add(new SqlParameter("@documentNo", criteria.documentNo));
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            QCReportEntries zone = GetInventery(dr);
                            Inventerys.Add(zone);
                        }
                    }
                }
            }
            return Inventerys;
        }


        private QCReportEntries GetInventery(SafeDataReader dr)
        {
            QCReportEntries inventery = new QCReportEntries();
            inventery.InventoryId = dr.GetInt64("Id");
            inventery.Quantity = dr.GetDecimal("Qty");
            inventery.GRNOrder = new KeyValue<long, string>() { Key = dr.GetInt64("GRNOrderId"), Value = dr.GetString("DocumentNo") };
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
                    cmd.Parameters.AddWithValue("@PartialQty", quality.PartialQty);
                    cmd.Parameters.AddWithValue("@PALLETID", quality.PalletCode);
                    cmd.Parameters.AddWithValue("@InventoryId", quality.inventoryId);
                    cmd.Parameters.AddWithValue("@Type", "SAVEQC");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        #endregion QA

        public string ValidateMaterial(string code, int plantId)
        {
            CodeContract.Required<ArgumentException>(code.IsNotNullOrWhiteSpace(), "Pallet Code is mandatory");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_QUALITYSTATUS";
                    cmd.Parameters.AddWithValue("@PALLETID", code);
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    cmd.Parameters.AddWithValue("@Type", "ValidatePallet");
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }
    }
}