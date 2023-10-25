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
   public class GRN_ConfirmationApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public GRN_ConfirmationApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }
        public async Task<Object> GetValidateGRNConfirmation(string DeliveryChallanNo, string CartonBarcode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_GRN_Confirm;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetValidateGRNConfirm;
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = DeliveryChallanNo;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = CartonBarcode;
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
