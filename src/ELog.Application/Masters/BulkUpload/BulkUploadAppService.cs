using Abp.Application.Services;
using Abp.UI;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVUE.Utility;
//using MobiVueEvo.Core.Printer;
//using MobiVueEvo.HardwareConnectorFactory;
using MobiVueEVO.BL;
using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using System;
using System.Text;

namespace MobiVueEvo.Application.Masters
{
    [PMMSAuthorize]
    public class BulkUploadAppService : ApplicationService, IBulkUploadAppService
    {


        public BulkUploadAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;

        }

        public class FileToUpload1
        {
            public string FileName { get; set; }
            public string FileSize { get; set; }
            public string FileType { get; set; }
            public long LastModifiedTime { get; set; }
            public DateTime LastModifiedDate { get; set; }
            public string FileAsBase64 { get; set; }
            public byte[] FileAsByteArray { get; set; }
        }

        public string UploadMasterFileRWAsync([FromBody] FileToUpload1 theFile)
        {
            var fileExt = System.IO.Path.GetExtension(theFile.FileName).Substring(1);

            if (fileExt.ToLower() != "csv")
            {
                throw new UserFriendlyException("Invalid file format");
            }
            //plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

            //string path = $"{_environment.WebRootPath}\\TenantLicense\\TenantLicenseCurrent" + AbpSession.TenantId + "P" + plantId + ".txt";

            if (theFile.FileAsBase64.Contains(","))
            {
                theFile.FileAsBase64 = theFile.FileAsBase64.Substring(theFile.FileAsBase64.IndexOf(",") + 1);
            }

            theFile.FileAsByteArray = Convert.FromBase64String(theFile.FileAsBase64);

            //File.WriteAllText(path, theFile.FileAsByteArray, Encoding.UTF8);
            string utfString = Encoding.UTF8.GetString(theFile.FileAsByteArray, 0, theFile.FileAsByteArray.Length);
            utfString = utfString.Replace("\r\n", "\r");
            char[] charSeparator = new char[] { '\r' };
            char[] commaSeparator = new char[] { ',' };

            string[] results;
            //string[] subresults;
            results = utfString.Split(charSeparator, StringSplitOptions.None);

            for (int i = 1; i < results.Length; i++)
            {
                string[] subresults;
                subresults = results[i].Split(commaSeparator, StringSplitOptions.None);
                Spool machineType = new Spool();
                var machineTypeBL = new Spools();
                machineType.SpoolCode = subresults[0];
                machineType.Description = subresults[1];
                machineType.SpoolType = new KeyValue<short, string>(Convert.ToInt16(subresults[2]), "admin");
                machineType.RFIDTagID = subresults[3];
                machineType.BLEMACAddress = subresults[4];
                machineType.UpdatedBy = new KeyValue<long, string>(1, "admin");
                machineType.IsActive = true;
                machineType.Size = 1;
                //machineType.Plant = new KeyValue<int, string>(Session.PlantId, "");
                var Code = new Spool();
                //GetSpools(new SpoolMasterSearchCriteria() { SpoolCode = machineType.SpoolCode, Status = 1, SiteId = Session.PlantId }).ItemCollection.FirstOrDefault();
                if (Code != null)
                {
                    machineType.Id = Code.Id;
                }


                machineType = machineTypeBL.Save(machineType);
            }

            //using (var fs = new FileStream(path, FileMode.Create))
            //{
            //    fs.Write(theFile.FileAsByteArray, 0, theFile.FileAsByteArray.Length);
            //}

            //string readTexttoSave = File.ReadAllText(path);

            //readTexttoSave = EncryptDecryptHelper.Decrypt(readTexttoSave);

            //char[] charSeparators = new char[] { ' ' }; //another mini salt
            //string[] result;
            //result = readTexttoSave.Split(charSeparators, StringSplitOptions.None);

            //string readText = EncryptDecryptHelper.Encrypt(readTexttoSave);

            //string tenantName = result[0];
            //string numberOfLicense = result[1];
            //string dsalt = result[2];
            //string plantid = result[3];
            //string licenseSerial = result[4];

            //string connection = _configuration["ConnectionStrings:Default"];

            //SqlConnection conn = null;

            //conn = new SqlConnection(connection);

            //try
            //{
            //    conn.Open();
            //}
            //catch (Exception)
            //{
            //    string e = "Database error contact administrator";

            //}
            //try
            //{
            //    SqlDataReader myReader = null;
            //    SqlCommand myCommand = new SqlCommand("UpdateLicense_SP", conn);
            //    myCommand.CommandType = CommandType.StoredProcedure;

            //    myCommand.Parameters.Add("@Tenant_Name", SqlDbType.NVarChar).Value = tenantName;
            //    myCommand.Parameters.Add("@NumberOfLicense", SqlDbType.NVarChar).Value = readText;
            //    myCommand.Parameters.Add("@ModifiedBy", SqlDbType.BigInt).Value = AbpSession.UserId;
            //    myCommand.Parameters.Add("@PlantId", SqlDbType.Int).Value = Convert.ToUInt32(plantid);
            //    myCommand.Parameters.Add("@licenseSerial", SqlDbType.Int).Value = Convert.ToUInt32(licenseSerial);
            //    myReader = myCommand.ExecuteReader();
            //    DataTable tab = new DataTable();
            //    tab.Load(myReader);

            //}
            //catch (Exception e)
            //{
            //    return e.Message;
            //}

            //var existingLogo = await _logoRepository.FirstOrDefaultAsync(x => x.TenantId == AbpSession.TenantId);

            return "Success";// MobiVueEvoValidationConst.LicenseSuccessMsg;
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
        public DataList<Bay, long> GetBays(BaySearchCriteria BaySearchCriteria)
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