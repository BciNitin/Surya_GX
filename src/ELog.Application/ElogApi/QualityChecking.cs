using ELog.Application.CommomUtility;
using ELog.Application.Masters.Areas;
using ELog.Application.Sessions;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BO.Models;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application.ElogApi
{
    public class QualityChecking : IElogApiService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public QualityChecking(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetQualityCheckingDetails([Required]string PlantCode, [Required] string LineCode, [Required] string PackingOrderNo)
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

                    Command.CommandText = "sp_QualityChecking";
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = "GetQualityCheckingDetails";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = PackingOrderNo;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineCode;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sStatus", MySqlDbType.VarChar).Value = "";
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

        public async Task<Object> GetQualityCheckingSave(QualityCheckingList qualityChecking)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {

                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                foreach (var QualityChecking in qualityChecking.QualityChecking) { 
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_QualityChecking";
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = "Save";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = QualityChecking.PlantCode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = QualityChecking.PackingOrderNo;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = QualityChecking.LineCode;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = QualityChecking.CartonBarCode;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = QualityChecking.ItemBarCode;
                    Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = QualityChecking.QCStatus;
                    Command.Parameters.Add("sStatus", MySqlDbType.VarChar).Value = "";
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    }
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
