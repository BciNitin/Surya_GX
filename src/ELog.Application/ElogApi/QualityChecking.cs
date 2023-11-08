using Abp.Application.Services;
using Abp.Runtime.Session;
using ELog.Application.CommomUtility;
using ELog.Application.Masters.Areas;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BO.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application.ElogApi
{
    public class QualityChecking : ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;


        public QualityChecking(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetQualityCheckingDetails([Required]string PlantCode, [Required] string LineCode, [Required] string PackingOrderNo)
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

                    Command.CommandText = "sp_QualityChecking";
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = "GetQualityCheckingDetails";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = PackingOrderNo;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineCode;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sStatus", MySqlDbType.VarChar).Value = "";
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

        public async Task<Object> SaveQualityChecking([FromBody] List<QualityCheckingModel> data)
        {
            //MySqlConnection conn = null;
            //conn = new MySqlConnection(connection);

            try
            {
                MySqlDataReader myReader = null;
                DataTable dt = null;
                using (var conn = new MySqlConnection(connection))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (QualityCheckingModel item in data)
                            {
                                if (item.Status != "No" && item.QCStatus != "NG")
                                {
                                    dt = new DataTable();
                                    using (MySqlCommand Command = new MySqlCommand())
                                    {
                                        Command.Connection = conn;
                                        Command.CommandText = "sp_QualityChecking";
                                        Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = "Save";
                                        Command.Transaction = transaction;
                                        Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = item.PlantCode;
                                        Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = item.PackingOrderNo;
                                        Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = item.LineCode;
                                        Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = item.ParentBarcode;
                                        Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = item.ChildBarcode;
                                        Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = item.QCStatus;
                                        Command.Parameters.Add("sStatus", MySqlDbType.VarChar).Value = item.Status;
                                        Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                                        Command.CommandType = CommandType.StoredProcedure;
                                        myReader = await Command.ExecuteReaderAsync();
                                        dt.Load(myReader);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0].Table.Columns.Contains("Error") && !dt.Rows[0].IsNull("Error"))
                                            {
                                                transaction.Rollback();
                                                return dt;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    dt = new DataTable();
                                    using (MySqlCommand Command = new MySqlCommand())
                                    {
                                        Command.Connection = conn;
                                        Command.CommandText = "sp_QualityChecking";
                                        Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = "Save";
                                        Command.Transaction = transaction;
                                        Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = item.PlantCode;
                                        Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = item.PackingOrderNo;
                                        Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = item.LineCode;
                                        Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = item.ParentBarcode;
                                        Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = item.ChildBarcode;
                                        Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = item.QCStatus;
                                        Command.Parameters.Add("sStatus", MySqlDbType.VarChar).Value = item.Status;
                                        Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                                        Command.CommandType = CommandType.StoredProcedure;
                                        myReader = await Command.ExecuteReaderAsync();
                                        dt.Load(myReader);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0].Table.Columns.Contains("Error") && !dt.Rows[0].IsNull("Error"))
                                            {
                                                transaction.Rollback();
                                                return dt;
                                            }
                                        }
                                    }
                                }
                                
                            }
                            
                            transaction.Commit();
                            return dt;
                        }
                        catch (Exception ex)
                        {
                            // If an exception occurs, roll back the entire transaction
                            transaction.Rollback();
                            dt.Columns.Add("Error", typeof(string));
                            DataRow newRow = dt.NewRow();
                            newRow["Error"] = "Error~Error in Transaction.";
                            dt.Rows.Add(newRow);
                            return dt;
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

        public async Task<Object> GetPackingOrderByPlantAndLine([Required] string PlantCode, [Required] string LineNo)
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
                    Command.CommandText = "sp_QualityChecking";
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetPackingOrder;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineNo;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQCStatus", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sStatus", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                    //// On all tables' columns
                    //foreach (DataColumn dc in dt.Columns)
                    //{
                    //    var field1 = dtRow[dc].ToString();

                    //}

                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["PackingOrderNo"]);
                    selectListDto.Value = Convert.ToString(dtRow["PackingOrderNo"]);

                    value.Add(selectListDto);
                    //var result = Utility.ToListof<SelectListDto>(dt);

                }
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
    }
}
