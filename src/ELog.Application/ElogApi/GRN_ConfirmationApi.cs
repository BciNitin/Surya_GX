﻿using Abp.Application.Services;
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
using Newtonsoft.Json.Linq;

namespace ELog.Application.ElogApi
{
    [PMMSAuthorize]
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
       
        public async Task<Object> GetChallanNoDetails(string DeliveryChallanNo)
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
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetChallanNoDetails";
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = DeliveryChallanNo;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sStatus", MySqlDbType.Bool).Value = false;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sFromPlantCode", MySqlDbType.VarChar).Value = "";
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

        public async Task<Object> GetDeliveryChallanNumber()
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                List<SelectListDto> value = new List<SelectListDto>();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_GRN_Confirm;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetDeliveryChallanNumber";
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sStatus", MySqlDbType.Bool).Value = false;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sFromPlantCode", MySqlDbType.VarChar).Value = "";
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        SelectListDto selectListDto = new SelectListDto();
                        selectListDto.Id = Convert.ToString(dtRow["DeliveryChallanNo"]);
                        selectListDto.Value = Convert.ToString(dtRow["DeliveryChallanNo"]);
                        value.Add(selectListDto);
                    }
                    return value;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> GetValidateGRNConfirmation(string DeliveryChallanNo, string CartonBarcode, bool Status, string ToPlantCode, string FromPlantCode)
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
                    Command.Parameters.Add("sStatus", MySqlDbType.Bool).Value = Status;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = ToPlantCode;
                    Command.Parameters.Add("sFromPlantCode", MySqlDbType.VarChar).Value = FromPlantCode;
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

        public async Task<Object> GetGRNConfirmation(string DeliveryChallanNo, string CartonBarcode, bool Status, string ToPlantCode, string FromPlantCode)
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
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetGRNConfirmation;
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = DeliveryChallanNo;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = CartonBarcode;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sStatus", MySqlDbType.Bool).Value = Status;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = ToPlantCode;
                    Command.Parameters.Add("sFromPlantCode", MySqlDbType.VarChar).Value = FromPlantCode;
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
