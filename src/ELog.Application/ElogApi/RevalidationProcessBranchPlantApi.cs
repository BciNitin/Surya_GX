using Abp.Application.Services;
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
using MobiVueEVO.BO.Models;
using Microsoft.AspNetCore.Mvc;

namespace ELog.Application.ElogApi 
{
    [PMMSAuthorize]
    public class RevalidationProcessBranchPlantApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public RevalidationProcessBranchPlantApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetPlantCode()
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

                    Command.CommandText = Constants.sp_Revalidation_Process_Branch;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetPlantCode;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
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
                    selectListDto.Id = Convert.ToString(dtRow["PlantCode"]);
                    selectListDto.Value = Convert.ToString(dtRow["PlantCode"]);
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

        public async Task<Object> GetExpiredItemCode()
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

                    Command.CommandText = Constants.sp_Revalidation_Process_Branch;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetExpiredItemCode;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
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
                    selectListDto.Id = Convert.ToString(dtRow["materialCode"]);
                    selectListDto.Value = Convert.ToString(dtRow["materialCode"]);
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

        public async Task<Object> GetExpiredItemCodeDetails(string PlantCode,string MaterialCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_Revalidation_Process_Branch;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetExpiredItemDetails;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = MaterialCode;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
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

        public async Task<Object> ValidateItem([FromBody] List<RevalidationProcessBranch> revalidationList)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                DataTable dt = new DataTable();

                using (MySqlCommand Command = new MySqlCommand())
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (RevalidationProcessBranch revalidation in revalidationList)
                            {
                                Command.Connection = conn;
                                Command.Transaction = transaction;

                                Command.CommandText = Constants.sp_Revalidation_Process_Branch;
                                Command.Parameters.Clear();  
                                Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetValidateItem;
                                Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = revalidation.Barcode;
                                Command.Parameters.Add("sItemBarcode", MySqlDbType.VarChar).Value = revalidation.ItemBarcode;
                                Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = revalidation.MaterialCode;
                                Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = revalidation.PlantCode;
                                Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                                Command.CommandType = CommandType.StoredProcedure;

                                using (MySqlDataReader myReader = await Command.ExecuteReaderAsync())
                                {
                                    dt.Load(myReader);
                                }
                            }

                            // Commit the transaction if everything went well
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
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

        public async Task<Object> ValidateItemByBarCode(string Barcode)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_Revalidation_Process_Branch;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.ValidateItemByBarCode ;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = Barcode;
                    Command.Parameters.Add("sItemBarcode", MySqlDbType.VarChar).Value = string.Empty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = string.Empty;
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
