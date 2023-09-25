using Abp.Application.Services;
using ELog.Core.Authorization;
using Microsoft.Extensions.Configuration;
using MobiVUE.Utility;


//using MobiVueEvo.Core.Printer;
//using MobiVueEvo.HardwareConnectorFactory;
using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using System;

namespace MobiVueEvo.Application.Masters
{
    [PMMSAuthorize]
    public class GenericAppService : ApplicationService, IGenericAppService
    {


        public GenericAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;

        }

        [PMMSAuthorize]
        public Bay GetBay(int id)
        {
            Bay Bay = null;
            try
            {
                var BayBL = new MobiVueEVO.BL.Bays();
                Bay = BayBL.GetBay(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Bay;
        }

        [PMMSAuthorize]
        public DataList<Bay, long> GetBays(BaySearchCriteria BaySearchCriteria, string tableName)
        {
            DataList<Bay, long> Bays = new DataList<Bay, long>();
            try
            {
                //BaySearchCriteria.SiteId = Session.PlantId;
                var BayBL = new MobiVueEVO.BL.Bays();
                Bays = BayBL.GetBays(BaySearchCriteria);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Bays;
        }

        //[PMMSAuthorize]
        //public long GetBayByCode(BaySearchCriteria BaySearchCriteria)
        //{
        //    try
        //    {
        //        var BayBL = new MobiVueEVO.BL.Bays();
        //        BaySearchCriteria.SiteId = Session.PlantId;
        //        var Bay = BayBL.GetBays(BaySearchCriteria).ItemCollection.FirstOrDefault();
        //        CodeContract.Required<MobiVUEException>(Bay.IsNotNull(), "Scan valid Bay");
        //        return Bay.BayId;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return 0;
        //    }
        //}

        [PMMSAuthorize]
        public string SaveBay(Bay Bay)
        {
            try
            {
                Bay.UpdatedBy = new KeyValue<long, string>((long)AbpSession.UserId, "admin");
                //Bay.Site = new KeyValue<int, string>(Session.PlantId, "");
                var BayBL = new MobiVueEVO.BL.Bays();
                Bay = BayBL.SaveBay(Bay);
                return "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        [PMMSAuthorize]
        public void DeleteBay(int id)
        {
            try
            {
                var BayBL = new MobiVueEVO.BL.Bays();
                BayBL.DeleteBay(id, (long)AbpSession.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        //[PMMSAuthorize]
        //public List<KeyValue<short, string>> GetUOMs()
        //{
        //    List<KeyValue<short, string>> uoms = new List<KeyValue<short, string>>();
        //    try
        //    {
        //        var uomBL = new MobiVueEVO.BL.UnitOfMeasurements();
        //        uoms = uomBL.GetUnitOfMeasurements().Select(c => new KeyValue<short, string>((short)c.UOMId, c.Name)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return uoms;
        //}

        //[PMMSAuthorize]
        //public List<KeyValue<int, string>> GetBayTypes()
        //{
        //    List<KeyValue<int, string>> uoms = new List<KeyValue<int, string>>();
        //    try
        //    {
        //        var BaytypeBL = new MobiVueEVO.BL.BayTypes();
        //        var oms = BaytypeBL.GetBayTypes(criteria: new BayTypeSearchCriteria() {  SiteId = Session.PlantId });
        //        uoms = oms.ItemCollection.Select(c => new KeyValue<int, string>((int)c.BayId, c.Baytype)).ToList();
        //        //.Select(c => new KeyValue<int, string>((int)c.BayId, c.BayType)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return uoms;
        //}


    }
}