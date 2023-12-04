using Abp.Application.Services;
using ELog.Application.Sessions;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BO.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application.ElogApi.Report
{
    [PMMSAuthorize]
    public  class MonthlyInspectionReportForDealerApi:ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public MonthlyInspectionReportForDealerApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }
       
        public async Task<Object> GetMonthlyInspectionReportForDealer()
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_Reports";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "MonthlyInspectionReportForDealer";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sfromDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("stoDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sShiperBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sDeliveryNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sChallanNos", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sTransferOrderNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = String.Empty;
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
