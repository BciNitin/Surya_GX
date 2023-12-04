using Abp.Application.Services;
using Castle.Facilities.TypedFactory.Internal;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Sessions;
using ELog.Core.Authorization;
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

namespace ELog.Application.ElogApi
{
    [PMMSAuthorize]
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
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
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
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_PackingOrderConfirmation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetConfirmationPackingOrderNo";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
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
                foreach (DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["PackingOrderNo"]);
                    selectListDto.Value = Convert.ToString(dtRow["PackingOrderNo"]);

                    value.Add(selectListDto);

                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return value;

        }
       
        public async Task<Object> ConfirmPackingOrder([FromBody] PackingOrderConfirm data)
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
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = data.PlantCode;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sStorageLocation", MySqlDbType.VarChar).Value = data.StrLocCode;
                    Command.Parameters.Add("sPackedQty", MySqlDbType.Double).Value = data.PackedQty;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = data.LineCode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = data.PackingOrderNo;
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
