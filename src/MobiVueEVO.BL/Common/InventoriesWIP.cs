using MobiVUE;
using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using System;
using System.Collections.Generic;

namespace MobiVueEVO.BL
{
    public class InventoriesWIP
    {
        public InventoryWIP FetchInventery(long InventeryId)
        {
            CodeContract.Required<ArgumentException>(InventeryId > 0, "Inventory id should not be null");

            var dal = new InventoryWIPFactory();
            return dal.Fetch(InventeryId);
        }

        public string SavePalletization(InventoryWIPInfo inventory, long modifiedby)
        {
            CodeContract.Required<ArgumentException>(inventory.IsNotNull(), "Inventory id should not be null");

            var dal = new InventoryWIPFactory();
            return dal.SavePalletization(inventory, modifiedby);
        }

        public List<InventoryWIP> GetInventerys(InventoryWIPSearchCriteria criteria)
        {
            var dal = new InventoryWIPFactory();
            return dal.FetchInventorys(criteria);
        }

        public List<InventoryWIP> GetInventerysForQC(DateTime FromDate, DateTime ToDate, InventoryWIPSearchCriteria criteria)
        {
            var dal = new InventoryWIPFactory();
            return dal.FetchInventorys(criteria);
        }

        public List<InventoryWIP> GetInventerysForReport(InventoryLevelReportSearchCriteria criteria)
        {
            var dal = new InventoryWIPFactory();
            return dal.FetchInventorysForReport(criteria);
        }

        public InventoryWIP SaveInventery(InventoryWIP Inventery)
        {
            CodeContract.Required<ArgumentException>(Inventery != null, "Inventery should not be null");

            Inventery.Validate();
            var dal = new InventoryWIPFactory();
            if (Inventery.IsNew)
            {
                return dal.Insert(Inventery);
            }
            else
            {
                return dal.Update(Inventery);
            }
        }

        public void DeleteInventery(long InventeryId)
        {
            CodeContract.Required<ArgumentException>(InventeryId > 0, "Inventery id required for delete Inventery");

            var dal = new InventoryWIPFactory();
            dal.Delete(InventeryId);
        }

        public List<long> DeleteInventories(List<long> inventoryIds)
        {
            CodeContract.Required<ArgumentException>(inventoryIds.Count > 0, "Inventery id required for delete Inventery");
            using (var scope = new System.Transactions.TransactionScope())
            {
                foreach (var inventoryId in inventoryIds)
                {
                    var dal = new InventoryWIPFactory();
                    dal.Delete(inventoryId);
                }
                scope.Complete();
                return inventoryIds;
            }
        }

        public List<InventoryWIP> SaveInventerys(List<InventoryWIP> Inventerys)
        {
            CodeContract.Required<ArgumentException>(Inventerys.Count > 0, "Atleast one Inventery is required for save Inventery ");
            List<InventoryWIP> items = new List<InventoryWIP>();
            InventoryWIP item = null;
            using (var scope = new System.Transactions.TransactionScope())
            {
                foreach (var Inventery in Inventerys)
                {
                    var dal = new InventoryWIPFactory();
                    if (Inventery.IsNew)
                    {
                        item = dal.Insert(Inventery);
                    }
                    else
                    {
                        item = dal.Update(Inventery);
                    }

                    items.Add(item);
                }
                scope.Complete();
                return items;
            }
        }
    }
}