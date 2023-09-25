using Abp.Application.Services;
//using MobiVueEvo.Application.CommonDto;
//using MobiVueEvo.Core;
using ELog.Core.Authorization;
using Microsoft.Extensions.Configuration;
using MobiVUE.Inventory.BL;
using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using System;
using System.Collections.Generic;


namespace MobiVueEvo.Application.Common
{
    [PMMSAuthorize]

    public class CommonAppService : ApplicationService, ICommonAppService
    {
        public CommonAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
        }



        [PMMSAuthorize]
        public List<Menu> GetAllMenu()
        {
            MenuSearchCriteria machineTypeSearchCriteria = new MenuSearchCriteria();
            machineTypeSearchCriteria.UserId = Convert.ToInt32(AbpSession.UserId);

            List<Menu> machineTypes = new List<Menu>();
            try
            {
                var menuBL = new CommonBL();
                //MenuSearchCriteria criteria
                machineTypes = menuBL.GetMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return machineTypes;
        }

        [PMMSAuthorize]
        public List<Menu> GetAllRolePermission()
        {
            MenuSearchCriteria machineTypeSearchCriteria = new MenuSearchCriteria();
            machineTypeSearchCriteria.UserId = Convert.ToInt32(AbpSession.UserId);

            List<Menu> machineTypes = new List<Menu>();
            try
            {
                var menuBL = new CommonBL();
                //MenuSearchCriteria criteria
                machineTypes = menuBL.GetRolesPermissions(machineTypeSearchCriteria.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return machineTypes;
        }
    }
}
