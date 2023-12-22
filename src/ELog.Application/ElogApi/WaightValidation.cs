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

namespace ELog.Application.ElogApi
{
    //[PMMSAuthorize]
    public class WaightValidation : ApplicationService, IElogApiService
    {

        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        private IPrinterConnector _printer;
       // private readonly IList<string> _roles;
        public WaightValidation(IConfiguration configuration, ISessionAppService sessionAppService, IPrinterConnector printer)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
            _printer = printer;
        }

        public async Task<Object> CaptureWeight(string BarCode,decimal Weight)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_WeightValidation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "CaptureWeight";
                    Command.Parameters.Add("sBarcode", MySqlDbType.VarChar).Value = BarCode;
                    Command.Parameters.Add("sWeight", MySqlDbType.Decimal).Value = Weight;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                    if(dt.Rows.Count == 0)
                    {
                        return "Invalid BarCode.";
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

        public async Task<Object> PrintBarCode(string BarCode)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                PrinterInput printer = new PrinterInput();

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
                    if (dt.Rows.Count == 0)
                    {
                        return "Invalid BarCode.";
                    }
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
                    }
                    else
                    {
                        return "Printer Not Available.";
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
    }
}
