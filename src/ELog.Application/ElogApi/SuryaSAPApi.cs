using Abp.Application.Services;
using ELog.Application.CommomUtility;
using ELog.Application.Sessions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Application.ElogApi
{
    public class SuryaSAPApi : ApplicationService, ISuryaSAPApi
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public SuryaSAPApi(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
        }

        public async Task<Object> DummyAPI()
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {

                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + Constants.SP_Master;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.PlantMaster;
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
