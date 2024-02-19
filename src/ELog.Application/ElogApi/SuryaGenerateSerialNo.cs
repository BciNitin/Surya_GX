using Abp.Application.Services;
using Abp.Runtime.Session;
using ELog.Application.CommomUtility;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Sessions;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BO.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Application.ElogApi
{
    [PMMSAuthorize]
    public class SuryaGenerateSerialNo : ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SuryaGenerateSerialNo(IConfiguration configuration, ISessionAppService sessionAppService, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Object> GenerateSerialNo(GenerateSerialNumber generate)
        {
            DataTable dt = null;
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dtList = new DataTable();
                string Directory = System.Environment.ExpandEnvironmentVariables(AppDomain.CurrentDomain.BaseDirectory + "SerialNumber");
                //string Directory= System.Environment.ExpandEnvironmentVariables("%userprofile%\\Downloads\\SerialNumber");

                bool exists = System.IO.Directory.Exists(Directory);

                if (!exists)
                    System.IO.Directory.CreateDirectory(Directory);

                for (int i = 0; i < generate.PrintingQty; i++)
                {
                    using (MySqlCommand Command = new MySqlCommand())
                    {
                        Command.Connection = conn;
                        Command.CommandText = Constants.SP_GenerateSerialNumber;
                        Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.sType_GenerateBarCode;
                        Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = generate.LineCode;
                        Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = generate.PlantCode;
                        Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = generate.PackingOrderNo;
                        Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                        Command.Parameters.Add("sSupplierCode", MySqlDbType.VarChar).Value = generate.SupplierCode;
                        Command.Parameters.Add("sDriverCode", MySqlDbType.VarChar).Value = generate.DriverCode;
                        Command.Parameters.Add("sQuantity", MySqlDbType.Double).Value = generate.Quantity;
                        Command.Parameters.Add("sPackingDate", MySqlDbType.Date).Value = generate.PackingDate;
                        Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = generate.ItemCode;
                        Command.Parameters.Add("sPrintedQty", MySqlDbType.Double).Value = generate.PrintedQty;
                        Command.Parameters.Add("sPendingQtyToPrint", MySqlDbType.Double).Value = generate.PendingQtyToPrint;
                        Command.Parameters.Add("sPrintingQty", MySqlDbType.Double).Value = generate.PrintingQty;
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Connection.Open();
                        myReader = await Command.ExecuteReaderAsync();
                        dt = new DataTable();
                        dt.Load(myReader);
                        Command.Connection.Close();
                        if (dt.Rows.Count > 0)
                        {
                            if (!dtList.Columns.Contains("BarCode"))
                            {
                                dtList.Columns.Add(new DataColumn("BarCode", typeof(string)));
                            }
                            dtList.Rows.Add(i);
                            dtList.Rows[i]["BarCode"] = dt.Rows[0]["BarCode"].ToString();
                        }

                    }
                }
                if (dtList.Rows.Count > 0)
                {

                    //string FileName = generate.PackingOrderNo + generate.LineCode + DateTime.Now.ToString("ddMMyyyyHHmmss");
                    //string webRootPath = _webHostEnvironment.WebRootPath;
                    //string path = Path.Combine(webRootPath, "SerialNumber", FileName + ".csv");
                    //string CSVFileName = FileName + ".csv";
                    //bool isFileCreated = Utility.DataTableToCSV(dtList, path, FileName);
                    //if (isFileCreated)
                    //{
                    //    dt.Columns.Add(new DataColumn("FileName", typeof(string)));
                    //    dt.Rows[0]["FileName"] = CSVFileName;
                    //}

                    string FileName = generate.PackingOrderNo+"_"+ generate.LineCode+"_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");
                    dtList.Columns.Add(new DataColumn("FileName", typeof(string)));
                    dtList.Rows[0]["FileName"] = FileName;
                    return dtList;

                }
            }
            catch (Exception e)
            {
                return dt;
            }

            return dt;

        }

        public async Task<Object> GetSerialNumberDetails(string packingOrder)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.SP_GenerateSerialNumber;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetSerialNumberDetails;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = packingOrder;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sSupplierCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sDriverCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQuantity", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.Date).Value = default;
                    Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPrintedQty", MySqlDbType.Double).Value = 1;
                    Command.Parameters.Add("sPendingQtyToPrint", MySqlDbType.Double).Value = 1;
                    Command.Parameters.Add("sPrintingQty", MySqlDbType.Double).Value = 0;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }


                return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> GetPackingOrderNo(string plantcode, string linecode)
        {
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.SP_GenerateSerialNumber;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetPackingOrderNo";
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = linecode;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantcode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sSupplierCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sDriverCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQuantity", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.Date).Value = default;
                    Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPrintedQty", MySqlDbType.Double).Value = 1;
                    Command.Parameters.Add("sPendingQtyToPrint", MySqlDbType.Double).Value = 1;
                    Command.Parameters.Add("sPrintingQty", MySqlDbType.Double).Value = 0;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {


                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["packingorderno"]);
                    selectListDto.Value = Convert.ToString(dtRow["packingorderno"]);
                    value.Add(selectListDto);


                }
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<Object> GetLineAsPerPlant(string plantcode)
        {
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.SP_GenerateSerialNumber;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetLineAsPerPlant";
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantcode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sSupplierCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sDriverCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQuantity", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.Date).Value = default;
                    Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPrintedQty", MySqlDbType.Double).Value = 1;
                    Command.Parameters.Add("sPendingQtyToPrint", MySqlDbType.Double).Value = 1;
                    Command.Parameters.Add("sPrintingQty", MySqlDbType.Double).Value = 0;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {


                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["WorkCenterCode"]);
                    selectListDto.Value = Convert.ToString(dtRow["WorkCenterCode"]);
                    value.Add(selectListDto);


                }
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
        
        public IActionResult GetDownloadFile(string FileName)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var filePath = Path.Combine(webRootPath, "SerialNumber", FileName);

            if (!System.IO.File.Exists(filePath))
            {
                return new NotFoundResult();
            }

            var fileStream = System.IO.File.OpenRead(filePath);
            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = FileName
            };
        }
    }
}
