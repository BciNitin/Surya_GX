using Abp.Application.Services;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVUE.Utility;
using MobiVueEVO.BL;
using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobiVueEvo.Application.Masters
{
    public class SpoolAppService : ApplicationService, ISpoolAppService
    {

        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public SpoolAppService(IConfiguration configuration, IRepository<Setting, long> settingRepository,
            IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            DBConfig.Configuration = configuration;

            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [PMMSAuthorize]
        public void DeleteSpool(int id)
        {
            try
            {
                var machineTypeBL = new Spools();
                machineTypeBL.DeleteSpoolMaster(id, (long)AbpSession.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [PMMSAuthorize]
        public Spool GetSpool(int id)
        {
            Spool machineType = null;
            try
            {
                var machineTypeBL = new Spools();
                machineType = machineTypeBL.GetSpoolMaster(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return machineType;
        }

        //[PMMSAuthorize]
        public DataList<Spool, long> GetSpools(SpoolMasterSearchCriteria machineTypeSearchCriteria)
        {
            DataList<Spool, long> machineTypes = new DataList<Spool, long>();
            try
            {
                machineTypeSearchCriteria.SiteId = 1;
                var machineTypeBL = new Spools();
                machineTypes = machineTypeBL.GetSpoolMasters(machineTypeSearchCriteria);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return machineTypes;
        }

        public DataList<Spool, long> GetSpoolsForBulkCheck(SpoolMasterSearchCriteria machineTypeSearchCriteria)
        {
            DataList<Spool, long> machineTypes = new DataList<Spool, long>();
            try
            {
                machineTypeSearchCriteria.SiteId = 1;
                var machineTypeBL = new Spools();
                machineTypes = machineTypeBL.GetSpoolMastersForBulkCheck(machineTypeSearchCriteria);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return machineTypes;
        }

        [PMMSAuthorize]
        public String SaveSpool(Spool machineType)
        {
            try
            {
                //var plantId = "";
                //plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();


                machineType.UpdatedBy = new KeyValue<long, string>((long)AbpSession.UserId, "admin");
                machineType.Plant = new KeyValue<int, string>(1, "");
                var machineTypeBL = new Spools();
                machineType = machineTypeBL.Save(machineType);
                return "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public class FileToUpload
        {
            public string FileName { get; set; }
            public string FileSize { get; set; }
            public string FileType { get; set; }
            public long LastModifiedTime { get; set; }
            public DateTime LastModifiedDate { get; set; }
            public string FileAsBase64 { get; set; }
            public byte[] FileAsByteArray { get; set; }
        }

        public string UploadSpoolFileRWAsync([FromBody] FileToUpload theFile)
        {

            var fileExt = System.IO.Path.GetExtension(theFile.FileName).Substring(1);

            if (fileExt.ToLower() != "csv")
            {
                throw new UserFriendlyException("Wrong file type. Only csv files are allowed.");
            }
            try
            {
                var plantId = "";
                plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

                string path = $"{_environment.WebRootPath}\\TenantLicense\\TenantLicenseCurrent" + AbpSession.TenantId + "P" + plantId + ".txt";

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
                    machineType.Plant = new KeyValue<int, string>(1, "Test");//change for the GXMEs
                                                                             //var Code = GetSpools(new SpoolMasterSearchCriteria() { SpoolCode = machineType.SpoolCode, Status = 1, SiteId = Session.PlantId }).ItemCollection.FirstOrDefault();
                    var Code = GetSpoolsForBulkCheck(new SpoolMasterSearchCriteria() { SpoolCode = machineType.SpoolCode, Status = 1, SiteId = 1 }).ItemCollection.FirstOrDefault();
                    if (Code != null)
                    {
                        machineType.Id = Code.Id;
                    }
                    //var hardware = new Hardwares().GetHardwares(new HardwareSearchCriteria() { PrintModule = "Location", SiteId = Session.PlantId }).ItemCollection.FirstOrDefault();

                    //machineType.IsActive = Convert.ToBoolean(subresults[5]);

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

                return "Uploaded Successfully";
            }
            catch (Exception)
            {
                return "Upload failed";
            }
        }

        public string UploadSpoolFileGeneric([FromBody] FileToUpload theFile)
        {

            var fileExt = System.IO.Path.GetExtension(theFile.FileName).Substring(1);

            if (fileExt.ToLower() != "csv")
            {
                throw new UserFriendlyException("Wrong file type. Only csv files are allowed.");
            }
            try
            {
                var plantId = "";
                plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

                string path = $"{_environment.WebRootPath}\\TenantLicense\\TenantLicenseCurrent" + AbpSession.TenantId + "P" + plantId + ".txt";

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
                List<Spool> emplist = new List<Spool>();
                List<Object> list = new List<Object>();

                results = utfString.Split(charSeparator, StringSplitOptions.None);

                for (int i = 1; i < results.Length; i++)
                {
                    string[] subresults;
                    subresults = results[i].Split(commaSeparator, StringSplitOptions.None);
                    Spool machineType = new Spool();
                    //var machineTypeBL = new Spools();
                    machineType.SpoolCode = subresults[0];
                    machineType.Description = subresults[1];
                    machineType.SpoolType = new KeyValue<short, string>(Convert.ToInt16(subresults[2]), "admin");
                    machineType.RFIDTagID = subresults[3];
                    machineType.BLEMACAddress = subresults[4];
                    machineType.UpdatedBy = new KeyValue<long, string>(1, "admin");
                    machineType.IsActive = true;
                    machineType.Size = 1;
                    //machineType.Plant = new KeyValue<int, string>(Session.PlantId, "");
                    machineType.Plant = new KeyValue<int, string>(1, "Test");//change for the GXMEs
                                                                             //var Code = GetSpools(new SpoolMasterSearchCriteria() { SpoolCode = machineType.SpoolCode, Status = 1, SiteId = Session.PlantId }).ItemCollection.FirstOrDefault();
                                                                             //var Code = GetSpoolsForBulkCheck(new SpoolMasterSearchCriteria() { SpoolCode = machineType.SpoolCode, Status = 1, SiteId = 1 }).ItemCollection.FirstOrDefault();
                                                                             //if (Code != null)
                                                                             //{
                                                                             //    machineType.Id = Code.Id;
                                                                             //}
                                                                             //var hardware = new Hardwares().GetHardwares(new HardwareSearchCriteria() { PrintModule = "Location", SiteId = Session.PlantId }).ItemCollection.FirstOrDefault();

                    //machineType.IsActive = Convert.ToBoolean(subresults[5]);

                    //machineType = machineTypeBL.Save(machineType);
                    emplist.Add(machineType);
                }

                var BulkUpload = new Spools();

                string result = BulkUpload.bulkUpload(emplist);

                if (result == "Failed")
                {
                    return "Failed";
                }
                return "Uploaded Successfully";
            }
            catch (Exception)
            {
                return "Upload failed";
            }
        }


        //public class Data
        //{
        //    //public Object column1 = null;
        //    //public Object column2 = null;
        //    //public Object column3 = null;
        //    //public Object column4 = null;
        //    //public Object column5 = null;
        //    //public Object column6 = null;
        //    //public Object column7 = null;
        //    //public Object column8 = null;
        //    //public Object column9 = null;
        //    public List<Object> column;

        //    public Data()
        //    {
        //        column = new List<Object>();
        //    }
        //}


        public string UploadSpoolFileGenericObjecy([FromBody] FileToUpload theFile, string Columns, string tablename)
        {
            Columns = "abcd,asdf,asdf,asdf";
            tablename = "SpoolMaster";
            var fileExt = System.IO.Path.GetExtension(theFile.FileName).Substring(1);

            if (fileExt.ToLower() != "csv")
            {
                throw new UserFriendlyException("Wrong file type. Only csv files are allowed.");
            }
            try
            {
                var plantId = "";
                plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();

                string path = $"{_environment.WebRootPath}\\TenantLicense\\TenantLicenseCurrent" + AbpSession.TenantId + "P" + plantId + ".txt";

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
                //List<Spool> emplist = new List<Spool>();
                List<GenericData> emplist = new List<GenericData>();

                results = utfString.Split(charSeparator, StringSplitOptions.None);

                string[] columns;
                columns = Columns.Split(charSeparator, StringSplitOptions.None);

                int range = columns.Length;
                //List<string> cats = new List<string>();
                //cats.Add(new string(columns[0])); // Add the cats however you want to set them up
                //                        // Ditto dogs, goats etc

                //List<Object> backyardObjects = new List<Object>;
                //for (int i = 0; i < 10; i++)
                //{
                //    backyardObjects.Add(new Object(cats[i]));
                //}
                for (int i = 1; i < results.Length; i++)
                {
                    string[] subresults;
                    subresults = results[i].Split(commaSeparator, StringSplitOptions.None);
                    GenericData machineType = new GenericData();
                    //var machineTypeBL = new Spools();
                    for (int j = 0; j < range; j++)
                    {
                        machineType.column[j] = subresults[j];
                    }
                    //machineType.column[i-1] = subresults[0];
                    //machineType.column[i] = subresults[1];
                    //machineType.column[i+1] = subresults[2];
                    //machineType.column[i+2] = subresults[3];
                    //machineType.column[i+3] = subresults[4];
                    //machineType.column[i + 4] = subresults[5];
                    //machineType.column7 = subresults[6];
                    //machineType.column8 = subresults[7];

                    //machineType.column9 = subresults[8];
                    //change for the GXMEs
                    //var Code = GetSpools(new SpoolMasterSearchCriteria() { SpoolCode = machineType.SpoolCode, Status = 1, SiteId = Session.PlantId }).ItemCollection.FirstOrDefault();
                    //var Code = GetSpoolsForBulkCheck(new SpoolMasterSearchCriteria() { SpoolCode = machineType.SpoolCode, Status = 1, SiteId = 1 }).ItemCollection.FirstOrDefault();
                    //if (Code != null)
                    //{
                    //    machineType.Id = Code.Id;
                    //}
                    //var hardware = new Hardwares().GetHardwares(new HardwareSearchCriteria() { PrintModule = "Location", SiteId = Session.PlantId }).ItemCollection.FirstOrDefault();

                    //machineType.IsActive = Convert.ToBoolean(subresults[5]);

                    //machineType = machineTypeBL.Save(machineType);
                    emplist.Add(machineType);
                }



                var BulkUpload = new Spools();

                string result = null; //BulkUpload.bulkUploadGeneric(emplist,tablename,columns);

                if (result == "Failed")
                {
                    return "Failed";
                }
                return "Uploaded Successfully";
            }
            catch (Exception)
            {
                return "Upload failed";
            }
        }
    }
}