using Abp.Application.Services;
using ELog.Application.Sessions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Application.ElogApi
{
    public class PackingOrderConfirmation : ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public PackingOrderConfirmation(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetPackingOrderDetails(string packingOrder, string PlantCode)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_PackingOrderConfirmation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetPackingOrderConfirmation";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sBranchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sStorageLocation", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackedQty", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = packingOrder;
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

        public async Task<Object> GetConfirmationPackingOrderNo(string PlantCode)
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_PackingOrderConfirmation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetConfirmationPackingOrderNo";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sBranchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sStorageLocation", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackedQty", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = String.Empty;
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
       
        public async Task<Object> ConfirmPackingOrder(string PlantCode,string PackingOrderNo)
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_PackingOrderConfirmation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ConfirmPackingOrder";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sBranchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sStorageLocation", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackedQty", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = PackingOrderNo;
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
