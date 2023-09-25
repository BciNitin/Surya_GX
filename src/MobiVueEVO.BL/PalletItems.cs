using MobiVUE.DAL;
using MobiVUE.Inventory.BO;
using MobiVUE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiVUE.Inventory.BL
{
    public class PalletItems
    {
        public PalletItem GetMaterialByPalletId(long palletId)
        {
            CodeContract.Required<ArgumentException>(palletId > 0, "Pallet id is required for get Material");
            using (var ctx = DalFactory.GetManager())
            {
                var dal = ctx.GetProvider<IPalletItemFactory>();
                return dal.Fetch(palletId);
            }
        }
    }
}