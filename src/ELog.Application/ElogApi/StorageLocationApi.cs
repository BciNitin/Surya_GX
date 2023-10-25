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
namespace ELog.Application.ElogApi
{
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
        public async Task<Object> GetStorageLocation()
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

                    Command.CommandText = Constants.SP_SelectList;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetStorageLocCode;
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
        public async Task<Object> GetStorageLocationDetails(string plancode,string LocationID)
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
                    Command.Parameters.Add("sLocationID", MySqlDbType.VarChar).Value = LocationID;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
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
        public async Task<Object> GetBarcodeScannedDetails(string barcode,string plantcode)
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
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetbarcodeDetails;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = barcode;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantcode;
                    Command.Parameters.Add("sLocationID", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
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
        public async Task<Object> StorageLocationConfirmation(string barcode,string LocationID)
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
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.StorageLocationTransfer;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = barcode;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sLocationID", MySqlDbType.VarChar).Value = LocationID;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;

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
    }
}
