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
using ELog.Connector;
using System.Reflection;
using ELog.Core.Printer;
using System.Text.RegularExpressions;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using MobiVueEVO.BO;


namespace ELog.Application.ElogApi
{
    //[PMMSAuthorize]
    public class WeightValidation : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        private IPrinterConnector _printer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string SessionPlantCode = null;
        // private readonly IList<string> _roles;
        public WeightValidation(IConfiguration configuration, ISessionAppService sessionAppService, IPrinterConnector printer, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
            _printer = printer;
            _httpContextAccessor = httpContextAccessor;
            SessionPlantCode = _httpContextAccessor.HttpContext.Session.GetString("PlantCode");
        }

        public async Task<Object> CaptureWeight(string BarCode,decimal Weight)
        {

            try
            {
                var Plant = _httpContextAccessor.HttpContext.Session.GetString("PlantCode");
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                Response response = new Response();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_WeightValidation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "CaptureWeight";
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = BarCode;
                    Command.Parameters.Add("sWeight", MySqlDbType.Decimal).Value = Weight;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.Decimal).Value = _httpContextAccessor.HttpContext.Session.GetString("PlantCode");
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    if (dt.Rows.Count > 0)
                    {
                        response.Message = Convert.ToString(dt.Rows[0]["Success"]);
                        response.Status = true;
                        return response;
                    }
                    else
                    {
                        response.Message = Convert.ToString(dt.Rows[0]["Error"]);
                        response.Status = false;
                        return response;
                    }
                }


                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> PrintBarCode(string BarCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                PrinterInput printer = new PrinterInput();
                Response response = new Response();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_WeightValidation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "PrintCapureWeight";
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = BarCode;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    if (!dt.Columns.Contains("Error"))
                    {
                        
                        dt.Columns.Add("OTP", typeof(System.String));
                        dt.Rows[0]["OTP"] = AbpSession.UserId;
                        printer.IPAddress = Convert.ToString(dt.Rows[0]["PrinterIP"]);
                        printer.Port = Convert.ToInt32(dt.Rows[0]["IP"]);
                        printer.PrintBody = Convert.ToString(dt.Rows[0]["PRNFile"]);
                        printer.PrintBody = ReplacePRNKeysValues(printer.PrintBody, dt);
                        bool isActive = await _printer.PrinterAvailable(printer);
                        if (isActive)
                        {
                            await _printer.Print(printer);
                            response.Message = "Print Successfully.";
                            response.Status = true;
                            return response;
                        }
                        else
                        {
                            response.Message = "Printer Not Available.";
                            response.Status = false;
                            return response;
                        }
                    }
                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> GetBarCodeDetails(string SessionPlantCode)
        {

            try
            {
                var Plant = _httpContextAccessor.HttpContext.Session.GetString("PlantCode");
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                Response response = new Response();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_WeightValidation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetBarCodeDetails";
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sWeight", MySqlDbType.Decimal).Value = 0;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.Decimal).Value = SessionPlantCode;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    if (!dt.Columns.Contains("Error"))
                    {
                        response.Message = "";
                        response.Result = dt;
                        response.Status = true;
                        return response;
                    }
                    else
                    {
                        response.Message = Convert.ToString(dt.Rows[0]["Error"]);
                        response.Result = dt;
                        response.Status = false;
                        return response;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> CaptureWeightNonBarCoded(string BarCode, decimal Weight)
        {

            try
            {
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                Response response = new Response();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_WeightValidation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "CaptureWeightNoneBarCoded";
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = BarCode;
                    Command.Parameters.Add("sWeight", MySqlDbType.Decimal).Value = Weight;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.Decimal).Value = SessionPlantCode;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    if (!dt.Columns.Contains("Error"))
                    {
                        response.Message = Convert.ToString(dt.Rows[0]["Success"]); 
                        response.Result = dt;
                        response.Status = true;
                        return response;
                    }
                    else
                    {
                        response.Message = Convert.ToString(dt.Rows[0]["Error"]);
                        response.Status = false;
                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        private string ReplacePRNKeysValues(string inputText, DataTable dataTable)
        {

            // Use regular expression to find values within curly braces
            string pattern = @"\{([^}]+)\}";
            string replacedText = Regex.Replace(inputText, pattern, match =>
            {
                // Retrieve the column name (e.g., "BARCODE" or "PODate")
                string columnName = match.Groups[1].Value;

                // Check if the DataTable contains the column
                if (dataTable.Columns.Contains(columnName))
                {
                    // Replace the match with the corresponding column value
                    return dataTable.Rows[0][columnName].ToString()!;
                }

                // If the column is not found, keep the original match
                return match.Value;
            });

            return replacedText;
        }


        public async Task<Object> GetPlantCode()
        {

            try
            {
                var Plant = _httpContextAccessor.HttpContext.Session.GetString("PlantCode");


                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                Response response = new Response();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_WeightValidation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GETPLANT";
                    
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sWeight", MySqlDbType.Decimal).Value = 0;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.Decimal).Value = SessionPlantCode;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    if (!dt.Columns.Contains("Error"))
                    {
                        response.Message = "";
                        response.Result = dt;
                        response.Status = true;
                        return response;
                    }
                    else
                    {
                        response.Message ="No data Found.";
                        response.Result = dt;
                        response.Status = false;
                        return response;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

    }
}
