using MobiVueEVO.BO;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace MobiVueEVO.DAL
{
    public class MenuFactory
    {
        public List<Menu> FetchAll()
        {
            List<Menu> FetchedMenuList = new List<Menu>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_ModuleSubModule";
                    //cmd.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    //cmd.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    //cmd.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    //cmd.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    //cmd.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    //cmd.Parameters.Add(new SqlParameter("@Type", "GetMaterialTypes"));
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            //if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            Menu men = GetMenu(dr);
                            FetchedMenuList.Add(men);
                        }
                    }
                }
            }
            return FetchedMenuList;

        }

        private Menu GetMenu(SafeDataReader dr)
        {
            Menu MenuFetched = new Menu();
            //MenuFetched.Id = dr.GetInt16("Id");
            MenuFetched.Module = dr.GetString("ModuleName");
            MenuFetched.SubModule = dr.GetString("SubModuleName");
            MenuFetched.DisplayName = dr.GetString("DisplayName");

            MenuFetched.Path = dr.GetString("Path");
            // MenuFetched.PermissionName = dr.GetString("PermissionName");
            //MenuFetched.Logo = dr.GetString("Logo");


            //MenuFetched.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            //MenuFetched.CreatedOn = dr.GetDateTime("CreatedOn");

            return MenuFetched;
        }
        public List<Menu> FetchAllRolePermissions(int UserId)
        {
            List<Menu> FetchedMenuList = new List<Menu>();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_ModuleSubModuleWithRoles";
                    cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                    //cmd.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    //cmd.Parameters.Add(new SqlParameter("@Name", criteria.Name));
                    //cmd.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    //cmd.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    //cmd.Parameters.Add(new SqlParameter("@Type", "GetMaterialTypes"));
                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            //if (criteria.TotalRowCount == 0) criteria.TotalRowCount = dr.GetInt64("TotalRows");
                            Menu men = GetRP(dr);
                            FetchedMenuList.Add(men);
                        }
                    }
                }
            }
            return FetchedMenuList;

        }

        private Menu GetRP(SafeDataReader dr)
        {
            Menu MenuFetched = new Menu();
            //MenuFetched.Id = dr.GetInt16("Id");
            MenuFetched.Module = dr.GetString("ModuleName");
            MenuFetched.SubModule = dr.GetString("SubModuleName");
            MenuFetched.PermissionName = dr.GetString("PermissionName");
            MenuFetched.Path = dr.GetString("Path");
            MenuFetched.ComponentName = dr.GetString("ComponentName");
            return MenuFetched;
        }
    }
}
