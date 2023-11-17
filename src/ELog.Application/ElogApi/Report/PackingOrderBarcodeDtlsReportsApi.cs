using Abp.Application.Services;
using Castle.Facilities.TypedFactory.Internal;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MobiVueEVO.BO.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace ELog.Application.ElogApi.Report
{
    public class PackingOrderBarcodeDtlsReportsApi : ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public PackingOrderBarcodeDtlsReportsApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

       

        [HttpPost]
        public async Task<Object> GetPackingOrderbarCodeDtlsReport([FromBody] PackingOrderBarcodeDetails reports)
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
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "PackingOrderbarCodeDtlsReports";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = reports.PlantCode;
                    Command.Parameters.Add("sfromDate", MySqlDbType.DateTime).Value = reports.FromDate;
                    Command.Parameters.Add("stoDate", MySqlDbType.DateTime).Value = reports.ToDate;
                    Command.Parameters.Add("sChallanNos", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sDeliveryNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = reports.MaterialCode;
                    Command.Parameters.Add("sShiperBarcode", MySqlDbType.VarChar).Value = reports.ShiperBarcode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = reports.LineCode;
                    Command.Parameters.Add("sTransferOrderNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = reports.PackingOrder;
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
