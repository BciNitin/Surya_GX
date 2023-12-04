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

namespace ELog.Application.ElogApi
{
    [PMMSAuthorize]
    public class NonBarcodedWarrantyClaimApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public NonBarcodedWarrantyClaimApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        

        public async Task<Object> GetNonBarcodedWarrantyDetails(string CustomerCode, string MaterialCode, string Qty, string ApprovedQty)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_NonBarcoded_Warranty;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetNonBarcodeWarrantyDetails;
                    Command.Parameters.Add("sQuantity", MySqlDbType.VarChar).Value = Qty;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
                    Command.Parameters.Add("sApprovalQnty", MySqlDbType.VarChar).Value = ApprovedQty;
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

        public async Task<Object> GetValidateNonBarcodedWarrranty(string CustomerCode, string MaterialCode, string Qty, string ApprovedQty)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_NonBarcoded_Warranty;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetValidateNonBcodeWarrranty;
                    Command.Parameters.Add("sQuantity", MySqlDbType.VarChar).Value = Qty;
                    Command.Parameters.Add("sApprovalQnty", MySqlDbType.VarChar).Value = ApprovedQty;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
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
    }
}
