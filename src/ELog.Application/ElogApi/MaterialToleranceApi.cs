using Abp.Application.Services;
using ELog.Application.Sessions;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using ELog.Application.Masters.Areas;
using MySql.Data.MySqlClient;
using ELog.Application.CommomUtility;
using System.Net.NetworkInformation;
using ELog.Application.SelectLists.Dto;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobiVueEVO.BO.Models;

namespace ELog.Application.ElogApi
{
    [PMMSAuthorize]
    public class MaterialToleranceApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public MaterialToleranceApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }
       
       

        public async Task<Object> GetMaterialCode()
        {
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_MaterialTolerance;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetMaterialCode";
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sMinWeight", MySqlDbType.Int16).Value = 0;
                    Command.Parameters.Add("sMax_Weight", MySqlDbType.Int16).Value = 0;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach(DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["MaterialCode"]);
                    selectListDto.Value = Convert.ToString(dtRow["MaterialCode"]);
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

        public async Task<Object> GetMaterailDescription(string ItemCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_MaterialTolerance;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetMaterialDescription";
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = ItemCode;
                    Command.Parameters.Add("sMinWeight", MySqlDbType.Int16).Value = 0;
                    Command.Parameters.Add("sMax_Weight", MySqlDbType.Int16).Value = 0;
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
        public async Task<Object> MaterialTolerance(string materialCode, string minWeight, string maxWeight)
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_MaterialTolerance;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "MaterialTolerance";
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = materialCode;
                    Command.Parameters.Add("sMinWeight", MySqlDbType.Int16).Value = minWeight;
                    Command.Parameters.Add("sMax_Weight", MySqlDbType.Int16).Value = maxWeight;
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
