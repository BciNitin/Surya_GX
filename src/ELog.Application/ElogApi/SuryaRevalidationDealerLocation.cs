using Abp.Application.Services;
using ELog.Application.Sessions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using ELog.Application.SelectLists.Dto;
using System.Collections.Generic;

namespace ELog.Application.ElogApi
{
    public class SuryaRevalidationDealerLocation : ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public SuryaRevalidationDealerLocation(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetDealerCode()
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_RevalidateDealerLocation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetDealerCode";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sManufacturingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;

                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }
                foreach (DataRow dtRow in dt.Rows)
                {
                    string[] parts = Convert.ToString(dtRow["DealerCode"]).Split('|');
                    string code = parts[0].Trim();
                    string name = parts[1].Trim();

                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = code;
                    selectListDto.Value = name;

                    value.Add(selectListDto);

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return value;

        }

        public async Task<Object> GetRevalidationDetails()
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_RevalidateDealerLocation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetDealerCode";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sManufacturingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;

                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }
                foreach (DataRow dtRow in dt.Rows)
                {
                    string[] parts = Convert.ToString(dtRow["DealerCode"]).Split('|');
                    string code = parts[0].Trim();
                    string name = parts[1].Trim();

                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = code;
                    selectListDto.Value = name;

                    value.Add(selectListDto);

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return value;

        }
    }
}
