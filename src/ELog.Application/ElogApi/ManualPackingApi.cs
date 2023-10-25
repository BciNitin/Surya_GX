﻿using Abp.Application.Services;
using ELog.Application.Sessions;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using ELog.Application.Masters.Areas;
using MySql.Data.MySqlClient;
using ELog.Application.CommomUtility;
using System.Net.NetworkInformation;

namespace ELog.Application.ElogApi
{
    public class ManualPackingApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public ManualPackingApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }
        public async Task<Object> GetManualPackingDetails(string plantcode, string linecode,string packingorder)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_Manual_Packing;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetManualPackingDetails;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantcode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = linecode;
                    Command.Parameters.Add("sPackingOrder", MySqlDbType.VarChar).Value = packingorder;
                    Command.Parameters.Add("sMacID", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sBinBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sparentBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
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
        public async Task<Object> ValidateBarcode(string BinBarCode,string macAddresses, string plantcode, string linecode, string packingorder)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_Manual_Packing;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.validateBarcode;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantcode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = linecode;
                    Command.Parameters.Add("sPackingOrder", MySqlDbType.VarChar).Value = packingorder;
                    Command.Parameters.Add("sMacID", MySqlDbType.VarChar).Value = macAddresses;
                    Command.Parameters.Add("sBinBarCode", MySqlDbType.VarChar).Value = BinBarCode;
                    Command.Parameters.Add("sparentBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
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
        public async Task<Object> GetMacAddress(string BinBarCode)
        {
            try
            {

                string macAddresses = string.Empty;

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        macAddresses += nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }

                return macAddresses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
       
    }
}
