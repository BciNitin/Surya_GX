using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Microsoft.AspNetCore.Http;
using ELog.Application.Modules;
using ELog.Application.Sessions;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using ELog.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ELog.Application.Masters.Areas;
using System.Collections.Generic;
using Newtonsoft.Json;
using PMMS.Application.ElogApi.Dto;
using MySql.Data.MySqlClient;
using System.Data.Common;
using BarcodeLib;
using Common;
using ELog.Application.CommomUtility;
using System.Linq;
using System.Reflection;
using MobiVueEVO.BO.Models;
using System.Reflection.PortableExecutable;
using System.Xml;
using Castle.Facilities.TypedFactory.Internal;
using System.IO;
using ELog.Application.SelectLists.Dto;
using System.Net.NetworkInformation;

namespace ELog.Application.ElogApi
{
   public class QualityTested_ItemPlacementApi : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public QualityTested_ItemPlacementApi(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetQualityItemTestedDtls(string itemBarcode, string plantCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_QualityTested_ItemPlacement;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetQualityItemTestedDtls;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantCode;
                    Command.Parameters.Add("sChildBarcode", MySqlDbType.VarChar).Value = itemBarcode;
                    Command.Parameters.Add("sParentBarcode", MySqlDbType.VarChar).Value = String.Empty;
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

        public async Task<Object> GetValidateShiperBarcode(string itemBarcode, string plantCode, string ShiperBarcode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_QualityTested_ItemPlacement;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.ValidateShiperBarcode;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantCode;
                    Command.Parameters.Add("sChildBarcode", MySqlDbType.VarChar).Value = itemBarcode;
                    Command.Parameters.Add("sParentBarcode", MySqlDbType.VarChar).Value = ShiperBarcode;
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
