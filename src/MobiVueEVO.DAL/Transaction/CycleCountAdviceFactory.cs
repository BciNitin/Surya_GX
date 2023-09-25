using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;


namespace MobiVueEVO.DAL
{
    public class CycleCountAdviceFactory
    {
        #region CycleCountAdvice

        public void Delete(long CycleCountAdviceId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(CycleCountAdviceId > 0, "CycleCountAdviceId is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_CycleCountAdvice";
                    cmd.Parameters.AddWithValue("@CycleCountAdviceId", CycleCountAdviceId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "Delete");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public CycleCountAdvice FetchByCycleCountAdviceId(long CycleCountAdviceId)
        {
            CodeContract.Required<ArgumentException>(CycleCountAdviceId > 0, "CycleCountAdviceId is required");
            CycleCountAdvice Advice = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CycleCountAdviceId", CycleCountAdviceId));
                    cmd.Parameters.Add(new SqlParameter("@Type", "FetchById"));
                    cmd.CommandText = "Usp_CycleCountAdvice";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            Advice = GetDetailsFetchById(dr);// GetdetailswithItems(dr);
                        }
                    }
                }
            }

            return Advice;
        }


        public List<CycleCountAdvice> FetchCycleCountAdvices(CycleCountAdviceSearchCriteria criteria)
        {
            //CodeContract.Required<ArgumentException>(criteria. CycleCountAdviceId > 0, "CycleCountAdviceId is required");
            List<CycleCountAdvice> Advice = new List<CycleCountAdvice>();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Code", criteria.Code));
                    cmd.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cmd.Parameters.Add(new SqlParameter("@Statuses", criteria.Status));

                    cmd.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cmd.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    cmd.Parameters.Add(new SqlParameter("@Type", "FetchCycleCountAdvices"));
                    cmd.CommandText = "Usp_CycleCountAdvice";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            Advice.Add(GetDetailsFetchCycleCountAdvices(dr));// GetdetailswithItems(dr);
                        }
                    }
                }
            }

            return Advice;
        }

        public List<CycleCountAdvice> FetchAdviceNames(CycleCountAdviceSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<CycleCountAdvice> OrderDetails = new List<CycleCountAdvice>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_CycleCountAdvice";
                    cm.Parameters.Add(new SqlParameter("@Type", "FetchAdviceNames"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            //if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            OrderDetails.Add(GetDetailsAdviceNames(dr));
                        }
                    }
                }
            }
            return OrderDetails;
        }

        public List<KeyValue<long, string>> FetchGRNNos(CycleCountAdviceSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            List<KeyValue<long, string>> OrderDetails = new List<KeyValue<long, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_CycleCountAdvice";
                    cm.Parameters.Add(new SqlParameter("@FromDate", criteria.FromDate));
                    cm.Parameters.Add(new SqlParameter("@ToDate", criteria.ToDate));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetAssemblyOrders"));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.PlantId));
                    cm.Parameters.Add(new SqlParameter("@Statuses", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@Code", criteria.Code));

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

        private CycleCountAdvice GetDetailsAdviceNames(SafeDataReader dr)
        {
            CycleCountAdvice AdviceNames = new CycleCountAdvice();
            AdviceNames.CycleCountAdviceId = dr.GetInt64("CycleCountAdviceId");
            AdviceNames.Code = dr.GetString("Code");
            //grnOrder.Items = GetItemsByGRNId(grnOrder.CycleCountAdviceId);
            return AdviceNames;
        }
        private CycleCountAdvice GetDetailsFetchById(SafeDataReader dr)
        {
            CycleCountAdvice cycleAdvice = new CycleCountAdvice();
            cycleAdvice.CycleCountAdviceId = dr.GetInt64("CycleCountAdviceId");
            cycleAdvice.Code = dr.GetString("Code");
            cycleAdvice.CreatedOn = dr.GetDateTime("CreatedOn");
            cycleAdvice.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            cycleAdvice.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = Convert.ToString(dr.GetInt32("PlantId")) };
            cycleAdvice.UpdatedOn = dr.GetDateTime("ModifiedOn");
            cycleAdvice.Status = dr.GetInt32("Status");//Added on 20/02/2023
            cycleAdvice.Items = GetItemsByAdviceId(cycleAdvice.CycleCountAdviceId);
            return cycleAdvice;
        }

        private CycleCountAdvice GetDetailsFetchCycleCountAdvices(SafeDataReader dr)
        {
            CycleCountAdvice advice = new CycleCountAdvice();
            advice.CycleCountAdviceId = dr.GetInt64("CycleCountAdviceId");
            advice.Code = dr.GetString("Code");
            advice.CreatedOn = dr.GetDateTime("CreatedOn");
            advice.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            advice.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = Convert.ToString(dr.GetInt32("PlantId")) };
            advice.UpdatedOn = dr.GetDateTime("ModifiedOn");
            advice.Status = dr.GetInt32("Status");

            //grnOrder.Items = GetItemsByGRNId(grnOrder.CycleCountAdviceId);
            return advice;
        }
        private CycleCountAdvice GetdetailswithItems(SafeDataReader dr)
        {
            CycleCountAdvice advice = new CycleCountAdvice();
            advice.CycleCountAdviceId = dr.GetInt32("Id");
            advice.CreatedOn = dr.GetDateTime("CreatedOn");
            advice.UpdatedOn = dr.GetDateTime("ModifiedOn");
            advice.Items = GetItemsByAdviceId(advice.CycleCountAdviceId);
            return advice;
        }

        public CycleCountAdvice Insert(CycleCountAdvice entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Entity should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                //  if (IsExists(entity, cn)) throw new Exception($"GRN Order: {entity.DocumentNo} already exists.");

                using (var transaction = cn.BeginTransaction())
                {
                    using (var cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.Transaction = transaction;
                        cm.CommandText = "Usp_CycleCountAdvice";

                        //cm.Parameters.AddWithValue("@PlantId", entity.Plant.IsNotNull() ? entity.Plant.Key : 0);
                        cm.Parameters.AddWithValue("@PlantId", entity.SiteId);
                        cm.Parameters.AddWithValue("@Status", entity.Status);
                        cm.Parameters.AddWithValue("@Code", entity.Code);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "Insert");
                        entity.CycleCountAdviceId = Convert.ToInt64(cm.ExecuteScalar());
                        if (entity.Items.HaveItems())
                        {
                            entity.Items = SaveItems(entity.Items, entity.CycleCountAdviceId, transaction, 0);
                            entity.Items.RemoveAll(x => x.IsDeleted);
                        }
                        transaction.Commit();
                        entity.MarkAsOld();
                        return entity;
                    }
                }
            }
        }

        public bool IsExists(CycleCountAdvice entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Hardware should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                //cmd.Parameters.AddWithValue("@PlantId", entity.Plant.Key);
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@CycleCountAdviceId", entity.CycleCountAdviceId);
                //cmd.Parameters.AddWithValue("@Status", entity.Status);
                //cmd.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "Usp_CycleCountAdvice";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public CycleCountAdvice Update(CycleCountAdvice entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Entity should not be null");
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
                        cm.CommandText = "Usp_CycleCountAdvice";

                        cm.Parameters.AddWithValue("@Status", entity.Status);
                        cm.Parameters.AddWithValue("@code", entity.Code);
                        cm.Parameters.AddWithValue("@CycleCountAdviceId", entity.CycleCountAdviceId);
                        cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                        cm.Parameters.AddWithValue("@Type", "Update");
                        // cm.Parameters.AddWithValue("@Type", entity.CycleCountAdviceId);
                        // entity.CycleCountAdviceId = Convert.ToInt64(cm.ExecuteScalar());
                        if (entity.Items.HaveItems())
                        {
                            entity.Items = SaveItems(entity.Items, entity.CycleCountAdviceId, transaction, entity.Items[0].CycleCountAdviceItemId);
                            entity.Items.RemoveAll(x => x.IsDeleted);
                        }
                        transaction.Commit();
                        entity.MarkAsOld();
                        return entity;
                    }
                }
            }
        }

        #endregion CycleCountAdvice


        #region CycleCountAdvice Items

        public List<CycleCountAdviceItem> GetItemsByAdviceId(long cycleAdviceId)
        {
            CodeContract.Required<ArgumentException>(cycleAdviceId > 0, "Cycle Advice id is required to get items");
            List<CycleCountAdviceItem> items = new List<CycleCountAdviceItem>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_CycleCountAdvice";
                    cm.Parameters.Add(new SqlParameter("@CycleCountAdviceId", cycleAdviceId));
                    cm.Parameters.Add(new SqlParameter("@Type", "FetchItemsByAdviceId"));

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
        public List<CycleCountAdviceItem> GetItemsByAdviceItemId(long cycleAdviceId)
        {
            CodeContract.Required<ArgumentException>(cycleAdviceId > 0, "Cycle Advice Item id is required to get items");
            List<CycleCountAdviceItem> items = new List<CycleCountAdviceItem>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Usp_CycleCountAdvice";
                    cm.Parameters.Add(new SqlParameter("@CycleCountAdviceItemId", cycleAdviceId));
                    cm.Parameters.Add(new SqlParameter("@Type", "FetchItemByAdviceItemId"));

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

        private CycleCountAdviceItem Getdetails(SafeDataReader dr)
        {
            CycleCountAdviceItem item = new CycleCountAdviceItem();
            item.CycleCountAdviceItemId = dr.GetInt64("CycleCountAdviceItemId");
            item.CycleCountAdviceId = dr.GetInt64("CycleCountAdviceId");
            item.Zones = dr.GetString("Zones");
            item.Locations = dr.GetString("Locations");
            item.Bins = dr.GetString("Bins");
            item.Locations = dr.GetString("Locations");
            item.Pallets = dr.GetString("Pallets");
            item.Materials = dr.GetString("Materials");
            return item;
        }

        public List<CycleCountAdviceItem> SaveItems(List<CycleCountAdviceItem> items, long adviceId, DbTransaction transaction, long itemid)
        {
            CodeContract.Required<ArgumentException>(adviceId > 0, "Advice Id id is required to Save item.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            foreach (CycleCountAdviceItem _item in items)
            {
                if (itemid == 0)
                {
                    _item.CycleCountAdviceId = adviceId;
                    InsertItem(_item, transaction);
                }
                else if (_item.IsDeleted)
                {
                    DeleteItem(_item.CycleCountAdviceItemId, transaction);
                }
                else
                {
                    _item.CycleCountAdviceId = adviceId;

                    _item.CycleCountAdviceItemId = itemid;
                    UpdateItem(_item, transaction);
                }
            }
            return items;
        }

        private CycleCountAdviceItem InsertItem(CycleCountAdviceItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_CycleCountAdvice";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "InsertItem"));
                cmd.Parameters.Add(new SqlParameter("@CycleCountAdviceId", entity.CycleCountAdviceId));
                cmd.Parameters.Add(new SqlParameter("@Zones", entity.Zones));
                cmd.Parameters.Add(new SqlParameter("@Locations", entity.Locations));
                cmd.Parameters.Add(new SqlParameter("@Bin", entity.Bins));
                cmd.Parameters.Add(new SqlParameter("@Pallets", entity.Pallets));
                cmd.Parameters.Add(new SqlParameter("@Materials", entity.Materials));

                //cmd.Parameters.Add(new SqlParameter("@MaterialId", entity.Material.Key));
                //cmd.Parameters.Add(new SqlParameter("@UOMId", entity.UOM.IsNotNull() ? entity.UOM.Key : 1));
                //cmd.Parameters.Add(new SqlParameter("@MaterialItemLine", entity.MaterialItemLine.IsNotNullOrWhiteSpace() ? entity.MaterialItemLine : ""));
                // cmd.Parameters.Add(new SqlParameter("@ItemStatus", entity.Status));
                //cmd.Parameters.Add(new SqlParameter("@Batch", entity.Batch.IsNotNullOrWhiteSpace() ? entity.Batch : ""));
                //cmd.Parameters.Add(new SqlParameter("@PurchaseOrderNo", entity.PurchaseOrderNo.IsNotNullOrWhiteSpace() ? entity.PurchaseOrderNo : ""));
                //cmd.Parameters.Add(new SqlParameter("@PurchaseOrderLineItem", entity.PurchaseOrderLineItem.IsNotNullOrWhiteSpace() ? entity.PurchaseOrderLineItem : ""));
                //cmd.Parameters.Add(new SqlParameter("@StorageLocation", entity.StorageLocation.IsNotNullOrWhiteSpace() ? entity.StorageLocation : ""));
                entity.CycleCountAdviceId = Convert.ToInt64(cmd.ExecuteScalar());
                entity.MarkAsOld();
                return entity;
            }
        }

        private CycleCountAdviceItem UpdateItem(CycleCountAdviceItem entity, DbTransaction transaction)
        {
            CodeContract.Required<ArgumentException>(entity != null, "GRN order item should not be null.");
            CodeContract.Required<ArgumentException>(transaction != null, "Database transaction should not be null.");
            entity.Validate();
            using (var cmd = transaction.Connection.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Usp_CycleCountAdvice";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "UpdateItem"));
                cmd.Parameters.Add(new SqlParameter("@CycleCountAdviceId", entity.CycleCountAdviceId));
                cmd.Parameters.Add(new SqlParameter("@Zones", entity.Zones));
                cmd.Parameters.Add(new SqlParameter("@Locations", entity.Locations));
                cmd.Parameters.Add(new SqlParameter("@Bin", entity.Bins));
                cmd.Parameters.Add(new SqlParameter("@Pallets", entity.Pallets));
                cmd.Parameters.Add(new SqlParameter("@Materials", entity.Materials));
                //cmd.Parameters.Add(new SqlParameter("@PrintedQty", entity.PrintedQty > 0 ? entity.PrintedQty : 0));
                //cmd.Parameters.Add(new SqlParameter("@ItemStatus", entity.Status));
                cmd.Parameters.Add(new SqlParameter("@CycleCountAdviceItemId", entity.CycleCountAdviceItemId));
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

                cmd.CommandText = "Usp_CycleCountAdvice";
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new SqlParameter("@Type", "DeleteItem"));
                cmd.Parameters.Add(new SqlParameter("@ItemId", itemid));
                cmd.ExecuteNonQuery();
            }
        }

        #endregion CycleCountAdvice Items
    }
}
