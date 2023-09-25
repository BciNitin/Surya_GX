using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using System.Collections.Generic;


namespace MobiVUE.Inventory.BL
{
    public class CommonBL
    {

        public List<Menu> GetMenu()
        {
            var men = new MenuFactory();
            return men.FetchAll();
        }

        public List<Menu> GetRolesPermissions(int UserId)
        {
            var men = new MenuFactory();
            return men.FetchAllRolePermissions(UserId);
        }


    }
}
