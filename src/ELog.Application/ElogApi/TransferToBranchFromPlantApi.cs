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

namespace ELog.Application.ElogApi
{
    [PMMSAuthorize]
    public class TransferToBranchFromPlantApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public TransferToBranchFromPlantApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }
        public async Task<Object> GetchallanNo()
        {
            List<SelectListDto> value = new List<SelectListDto>();
             try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.sp_Transfer_To_BranchFrom_Plant;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetChallanNo;
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sFromPlant", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["DeliveryChallanNo"]);
                    selectListDto.Value = Convert.ToString(dtRow["DeliveryChallanNo"]);
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

        public async Task<Object> GetMaterialCode(string DeliveryChallanNo)
        {
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.sp_Transfer_To_BranchFrom_Plant;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetGetMaterialCode;
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = DeliveryChallanNo;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sFromPlant", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
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

        public async Task<Object> GetChallanDetails(string DeliveryChallanNo,string MaterialCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_Transfer_To_BranchFrom_Plant;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetChallanDetails;
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = DeliveryChallanNo;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
                    Command.Parameters.Add("sFromPlant", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = string.Empty;
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

        public async Task<Object> GetValidateScanCartonBarcode(string DeliveryChallanNo, string CartonBarcode, string MaterialCode,string FromPlantCode,string ToPlantCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_Transfer_To_BranchFrom_Plant;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetValidateScanCartonBarcode;
                    Command.Parameters.Add("sDeliveryChallanNo", MySqlDbType.VarChar).Value = DeliveryChallanNo;
                    Command.Parameters.Add("sCartonBarcode", MySqlDbType.VarChar).Value = CartonBarcode;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
                    Command.Parameters.Add("sFromPlant", MySqlDbType.VarChar).Value = FromPlantCode;
                    Command.Parameters.Add("sToPlantCode", MySqlDbType.VarChar).Value = ToPlantCode;
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
