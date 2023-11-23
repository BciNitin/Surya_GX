
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
using Microsoft.AspNetCore.Mvc;

namespace ELog.Application.ElogApi
{


    /*
        [PMMSAuthorize]*/
    public class ElogSuryaApiService : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public ElogSuryaApiService(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
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

                    Command.CommandText =  Constants.SP_Master;
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

                    Command.CommandText = Constants.SP_Master;
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

                Command.CommandText =  Constants.SP_Master;
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

                    Command.CommandText = Constants.SP_Master;
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

        public async Task<Object> GetLineNumber()
        {

            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                DataTable dt = new DataTable();
                MySqlDataReader myReader = null;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.SP_SelectList;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetLineCode;
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

                    Command.CommandText = Constants.SP_Master;
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

                    Command.CommandText =  Constants.SP_Master;
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

        public async Task<Object> GetBinById(int id)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();

                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText =  Constants.sp_masters_bin;
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = Constants.GetBinById;
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = 0;
                    Command.Parameters.Add("@sId", MySqlDbType.Int32).Value = id;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    await Command.Connection.CloseAsync();
                }
                return dt;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> UpdateBin(Bin bin)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                int result = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText =  Constants.sp_masters_bin;
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = Constants.GetBinById;
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = bin.PlantCode;
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = bin.BinCode;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = bin.Description;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = bin.Active;
                    Command.Parameters.Add("@sId", MySqlDbType.Int32).Value = bin.Id;
                    Command.CommandType = CommandType.StoredProcedure;
                    await Command.Connection.OpenAsync();
                    result = await Command.ExecuteNonQueryAsync();
                    await Command.Connection.CloseAsync();
                }
                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> GetBinCode()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText =  Constants.SP_Master;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetBinCode;
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

                    Command.CommandText = Constants.SP_Master;
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

        public async Task<Object> DeleteSiftMasterById(int id)
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
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = "DeleteSiftMasterbyid";
                    Command.Parameters.Add("sid", MySqlDbType.Int32).Value = id;
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

        public async Task<Object> GetPackingMaster()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText =  Constants.SP_Master;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetPackingMasters;
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

        public async Task<Object> CreateBinMaster(Bin bin)
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
                    Command.CommandText = "sp_masters_bin";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "InsertBin";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = bin.PlantCode;
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = bin.BinCode;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = bin.Description;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = bin.Active;
                    Command.Parameters.Add("@sId", MySqlDbType.Int32).Value = 0;
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

        public async Task<Object> CreateSiftMaster(string ShiftCode, string ShiftDescription, string sShiftStartTime, string sShiftEndTime)
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

                    Command.CommandText = "sp_Masters_ShiftMaster";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "InsertShift";
                    Command.Parameters.Add("@sShiftCode", MySqlDbType.VarChar).Value = ShiftCode;
                    Command.Parameters.Add("@sShiftDescription", MySqlDbType.VarChar).Value = ShiftDescription;
                    Command.Parameters.Add("@sShiftStartTime", MySqlDbType.VarChar).Value = sShiftStartTime;
                    Command.Parameters.Add("@sShiftEndTime", MySqlDbType.VarChar).Value = sShiftEndTime;
                    Command.Parameters.Add("@sId", MySqlDbType.Int32).Value = 0;
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
        [HttpPost]
        public async Task<Object> DeleteSift(string id)
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
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "DeleteShift";
                    Command.Parameters.Add("@sid", MySqlDbType.VarChar).Value = id;
                    Command.Parameters.Add("@sShiftCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sShiftDescription", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sShiftStartTime", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sShiftEndTime", MySqlDbType.VarChar).Value = String.Empty;
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

        [HttpPost]
        public async Task<Object> DeleteBin(string id)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_masters_bin";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "DeleteBin";
                    Command.Parameters.Add("@sid", MySqlDbType.VarChar).Value = id;
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sBinCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sDescription", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("@sActive", MySqlDbType.Bit).Value = String.Empty;
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
        //-------------------Packing order Master(Start) by Abhishek------------------------------------------------
        public async Task<Object> GetPackingOrderDetails()
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + "sp_PackingOrderMaster";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "GETPackingOrder";
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
        //-------------------Packing order Master(END)--------------------------------------------------------------
        //-------------------Line Work Center and Bin Mapping order Master(Start) by Abhishek------------------------------------------------
        public async Task<Object> LineBinMapping_ScanLine(string PlantCode, string Userid, string LineBarCode)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + "sp_LineBinMapping";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "CheckLineBarCode";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("@sUserID", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("@sLineCode", MySqlDbType.VarChar).Value = LineBarCode;
                    Command.Parameters.Add("@sBarCode", MySqlDbType.VarChar).Value = "";
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
        public async Task<Object> LineBinMapping_ScanBarcode(string PlantCode, string Userid, string Barcode)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + "sp_LineBinMapping";
                    Command.Parameters.Add("@sType", MySqlDbType.VarChar).Value = "CheckBarcode";
                    Command.Parameters.Add("@sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("@sBarCode", MySqlDbType.VarChar).Value = Barcode;
                    Command.Parameters.Add("@sUserID", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("@sLineCode", MySqlDbType.VarChar).Value = "";
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

        public async Task<Object> LineBinMapping_Mapping(string barcode, string lineBarCode)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            conn = new MySqlConnection(connection);

            try
            {
                int ressult = 0;
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + "sp_LineBinMapping";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "MappingLineBin";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserID", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sBarCode", MySqlDbType.VarChar).Value = barcode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = lineBarCode;

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
        //-------------------Line Work Center and Bin Mapping order (End)------------------------------------------------------------------------------------
       
    }
}

