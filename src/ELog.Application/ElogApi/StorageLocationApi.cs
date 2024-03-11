using Abp.Application.Services;
using ELog.Application.Sessions;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using ELog.Application.Masters.Areas;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ELog.Application.CommomUtility;
using ELog.Application.SelectLists.Dto;
using ELog.Core.Authorization;
using System.Net.NetworkInformation;
using System.Linq;

namespace ELog.Application.ElogApi
{
    [PMMSAuthorize]
    public class StorageLocationApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public StorageLocationApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }


        public async Task<Object> GetMaterialCode(string PlantCode, String FromLocation)
        {
            List<SelectListDto> value = new List<SelectListDto>();

            try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.SP_StorageLocation;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetMaterialCode";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sFromLocation", MySqlDbType.VarChar).Value = FromLocation;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sToLocation", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sMACId", MySqlDbType.VarChar).Value = string.Empty;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }
                if (!dt.Columns.Contains("Error"))
                {
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        SelectListDto selectListDto = new SelectListDto();
                        selectListDto.Id = Convert.ToString(dtRow["materialcode"]);
                        selectListDto.Value = Convert.ToString(dtRow["materialcode"]);

                        value.Add(selectListDto);
                    }
                    return value;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
        public async Task<Object> GetToStorageLocation(string PlantCode)
        {
            List<SelectListDto> value = new List<SelectListDto>();

            try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.SP_StorageLocation;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetStorageLocCode";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sFromLocation", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sToLocation", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sMACId", MySqlDbType.VarChar).Value = string.Empty;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["StrLocCode"]);
                    selectListDto.Value = Convert.ToString(dtRow["StrLocCode"]);

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
        public async Task<Object> GetStorageLocationDetails(string plancode, string LocationID, string MaterialCode, string FromLocation,string ToLocation)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.SP_StorageLocation;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetStorageLocationDetails;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plancode;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sFromLocation", MySqlDbType.VarChar).Value = FromLocation;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
                    Command.Parameters.Add("sToLocation", MySqlDbType.VarChar).Value = ToLocation;
                    Command.Parameters.Add("sMACId", MySqlDbType.VarChar).Value = string.Empty;
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
        public async Task<Object> GetBarcodeScannedDetails(string barcode, string plantcode, string FromLocation, string MaterialCode, string ToLocation)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();

                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces()where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.SP_StorageLocation;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetbarcodeDetails;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = barcode;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantcode;
                    Command.Parameters.Add("sFromLocation", MySqlDbType.VarChar).Value = FromLocation;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
                    Command.Parameters.Add("sToLocation", MySqlDbType.VarChar).Value = ToLocation;
                    Command.Parameters.Add("sMACId", MySqlDbType.VarChar).Value = macAddr;
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
        public async Task<Object> StorageLocationConfirmation(string PlantCode, string barcode, string FromLocation, string ToLocation,string MaterialCode)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();

                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.SP_StorageLocation;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.StorageLocationTransfer;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = barcode;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sFromLocation", MySqlDbType.VarChar).Value = FromLocation;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
                    Command.Parameters.Add("sToLocation", MySqlDbType.VarChar).Value = ToLocation;
                    Command.Parameters.Add("sMACId", MySqlDbType.VarChar).Value = macAddr;
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

        public async Task<Object> GetFromStorageLocation(string PlantCode)
        {

            List<SelectListDto> value = new List<SelectListDto>();

            try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.SP_StorageLocation;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "FromLocation";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sFromLocation", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sToLocation", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sMACId", MySqlDbType.VarChar).Value = string.Empty;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                if (dt.Columns.Contains("Error"))
                {
                    
                    return dt;
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["FromLocation"]);
                    selectListDto.Value = Convert.ToString(dtRow["FromLocation"]);

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

    }
}
