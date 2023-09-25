using MobiVUE.DAL;
using MobiVUE.Inventory.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiVUE.Inventory.BL
{
    public class PalletTypes
    {
        public PalletType GetPalletType(long PalletTypeId)
        {
            CodeContract.Required<ArgumentException>(PalletTypeId > 0, "Pallet type Id is required for get Pallet Type");
            using (var ctx = DalFactory.GetManager())
            {
                var dal = ctx.GetProvider<IPalletTypeFactory>();
                return dal.Fetch(PalletTypeId);
            }
        }

        public List<PalletType> GetPalletTypes()
        {
            using (var ctx = DalFactory.GetManager())
            {
                var dal = ctx.GetProvider<IPalletTypeFactory>();
                return dal.FetchAll();
            }
        }

        public PalletType SavePalletType(PalletType PalletType)
        {
            CodeContract.Required<ArgumentException>(PalletType != null, "Pallet type should not be null");
            PalletType.Validate();
            using (var ctx = DalFactory.GetManager())
            {
                var dal = ctx.GetProvider<IPalletTypeFactory>();
                if (PalletType.PalletTypeId > 0)
                {
                    return dal.Update(PalletType);
                }
                else
                {
                    return dal.Insert(PalletType);
                }
            }
        }

        public void DeletePalletType(long PalletTypeId)
        {
            CodeContract.Required<ArgumentException>(PalletTypeId > 0, "Pallet type Id is required for delete Pallet Type");
            using (var ctx = DalFactory.GetManager())
            {
                var dal = ctx.GetProvider<IPalletTypeFactory>();
                dal.Delete(PalletTypeId);
            }
        }
    }
}
