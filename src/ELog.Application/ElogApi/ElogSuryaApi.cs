﻿
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

namespace ELog.Application.ElogApi
{


/*
    [PMMSAuthorize]*/
    public class ElogSuryaApiService : ApplicationService, IElogApiService
    {
       
        private readonly IConfiguration _configuration;
        private string connection;


       public ElogSuryaApiService(IConfiguration configuration) 
        { 
          _configuration = configuration;
          connection = _configuration["ConnectionStrings:Default"];
        }

        public async Task<Object> GetPlantMaster()
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

        public async Task<object> GetMaterialMaster()
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
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.MaterialMaster;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }
                var result = Utility.DataTableToList<Material>(dt);
                //var result = Utility.ToListof<Material>(dt);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> GetCustomerMaster()
        {
            DataTable dt = new DataTable();
          
                MySqlConnection conn = null;
                conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
               
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + Constants.SP_Master;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.CustomerMaster;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                return dt;
           
        }

        public async Task<Object> GetLineMaster()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + Constants.SP_Master;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.LineMaster;
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

        public async Task<Object> GetStorageLocationMaster()
        {
           
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + Constants.SP_Master;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.StorageLocationMaster;
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
        public async Task<Object> GetBinMaster()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + Constants.SP_Master;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.BinMaster;
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

        public async Task<Object> GetSiftMaster()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "sp_Masters_ShiftMaster";
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.ShiftMaster;
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
        public async Task<Object> UpdateSiftMaster(string ShiftCode, string ShiftDescription, DateTime sShiftStartTime, DateTime sShiftEndTime)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.sp_mshift";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "SHIFTUPDATE";
                    Command.Parameters.Add("@sShiftCode", MySqlDbType.VarChar).Value = ShiftCode;
                    Command.Parameters.Add("@sShiftDescription", MySqlDbType.VarChar).Value = ShiftDescription;
                    Command.Parameters.Add("@sShiftStartTime", MySqlDbType.DateTime).Value = sShiftStartTime;
                    Command.Parameters.Add("@sShiftEndTime", MySqlDbType.DateTime).Value = sShiftEndTime;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                    Command.Connection.Close();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
        public async Task<Object> DeleteSiftMaster(string ShiftCode)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.sp_mshift";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "SHIFTDELETE";
                    Command.Parameters.Add("@sShiftCode", MySqlDbType.VarChar).Value = ShiftCode;
                    Command.Parameters.Add("@sShiftDescription", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("@sShiftStartTime", MySqlDbType.DateTime).Value = "";
                    Command.Parameters.Add("@sShiftEndTime", MySqlDbType.DateTime).Value = "";
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                    Command.Connection.Close();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> CreateBinMaster(string PlantCode, string BinCode, string Description, bool Active)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.Masters";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "BININSERT";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = BinCode;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = Description;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = Active;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> UpdateBinMaster(string PlantCode, string BinCode, string Description, bool Active)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.sp_masters";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "BINUPDATE";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = BinCode;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = Description;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = Active;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> DeleteBinMaster(string BinCode)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.sp_masters";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "BINDELETE";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = BinCode;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = true;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<Object> CreateSiftMaster(string ShiftCode, string ShiftDescription, DateTime sShiftStartTime, DateTime sShiftEndTime)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.sp_mshift";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "SHIFTINSERT";
                    Command.Parameters.Add("@sShiftCode", MySqlDbType.VarChar).Value = ShiftCode;
                    Command.Parameters.Add("@sShiftDescription", MySqlDbType.VarChar).Value = ShiftDescription;
                    Command.Parameters.Add("@sShiftStartTime", MySqlDbType.DateTime).Value = sShiftStartTime;
                    Command.Parameters.Add("@sShiftEndTime", MySqlDbType.DateTime).Value = sShiftEndTime;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                    Command.Connection.Close();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
        public async Task<Object> PostPlantMaster(string PlantCode, string PlantType, string Description, string Address, bool Active)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.sp_sap_plant";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "PlantPost";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("@sPlantType", MySqlDbType.VarChar).Value = PlantType;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = Description;
                    Command.Parameters.Add("@sAddress", MySqlDbType.VarChar).Value = Address;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = Active;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                    Command.Connection.Close();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
        public async Task<Object> PostBinMaster(string sPlantCode, string BinCode, string Description, bool Active)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "surya_db.sp_sap_bin";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "INSERT";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = sPlantCode;
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = BinCode;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = Description;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = Active;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    ressult = await Command.ExecuteNonQueryAsync();
                    Command.Connection.Close();
                }
                return ressult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

    }
    }