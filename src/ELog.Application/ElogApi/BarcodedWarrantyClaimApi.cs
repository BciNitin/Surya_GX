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
    public class BarcodedWarrantyClaimApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public BarcodedWarrantyClaimApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetCustomerCode()
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

                    Command.CommandText = Constants.sp_BarcodedWarrantyClaim;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetCustomerCode;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQuantity", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sCustomerCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sApprovalQnty", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["Code"]);
                    selectListDto.Value = Convert.ToString(dtRow["Code"]);
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

        public async Task<Object> GetWarrantyDetails(string Barcode, string CustomerCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_BarcodedWarrantyClaim;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetWarrantyDetails;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = Barcode;
                    Command.Parameters.Add("sQuantity", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sApprovalQnty", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sCustomerCode", MySqlDbType.VarChar).Value = CustomerCode;
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

        public async Task<Object> GetValidateWarrranty(string Barcode, string CustomerCode, string BarCodeApprovedQty)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_BarcodedWarrantyClaim;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetValidateBcodeWarrranty;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = Barcode;
                    Command.Parameters.Add("sQuantity", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sCustomerCode", MySqlDbType.VarChar).Value = CustomerCode;
                    Command.Parameters.Add("sApprovalQnty", MySqlDbType.VarChar).Value = BarCodeApprovedQty;
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
