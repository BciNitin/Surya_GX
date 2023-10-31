using Abp.Application.Services;
using Abp.Runtime.Session;
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
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ELog.Application.ElogApi
{
    public class SuryaQualityConfirmation: ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public SuryaQualityConfirmation(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
        }

        public async Task<Object> GetPackingOrderNo(string plantcode)
        {
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_QualityConfirmation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetPackingOrderNo";
                    Command.Parameters.Add(new MySqlParameter("sPackingOrderNo", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sItemCode", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sLocation", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sBatchCode", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sOkQty", MySqlDbType.Decimal) { Value = 0 });
                    Command.Parameters.Add(new MySqlParameter("sNgQty", MySqlDbType.Decimal) { Value = 0 });
                    Command.Parameters.Add(new MySqlParameter("sPackedQty", MySqlDbType.Decimal) { Value = 0 });
                    Command.Parameters.Add(new MySqlParameter("sQCDate", MySqlDbType.Date) { Value = default });
                    Command.Parameters.Add(new MySqlParameter("sLineCode", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sPlantCode", MySqlDbType.VarChar) { Value = plantcode});
                    Command.Parameters.Add(new MySqlParameter("sItemBarCode", MySqlDbType.VarChar) { Value = plantcode});
                    Command.Parameters.Add(new MySqlParameter("sParantBarCode", MySqlDbType.VarChar) { Value = plantcode});
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["packingorderno"]);
                    selectListDto.Value = Convert.ToString(dtRow["packingorderno"]);
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

        public async Task<Object> GetQCCheckingDetails([Required]string plantcode, [Required] string packingorderno)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_QualityConfirmation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetQualityCheckingDetails";
                    Command.Parameters.Add(new MySqlParameter("sPackingOrderNo", MySqlDbType.VarChar) { Value = packingorderno });
                    Command.Parameters.Add(new MySqlParameter("sItemCode", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sLocation", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sBatchCode", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sOkQty", MySqlDbType.Decimal) { Value = 0 });
                    Command.Parameters.Add(new MySqlParameter("sNgQty", MySqlDbType.Decimal) { Value = 0 });
                    Command.Parameters.Add(new MySqlParameter("sPackedQty", MySqlDbType.Decimal) { Value = 0 });
                    Command.Parameters.Add(new MySqlParameter("sQCDate", MySqlDbType.Date) { Value = default });
                    Command.Parameters.Add(new MySqlParameter("sLineCode", MySqlDbType.VarChar) { Value = String.Empty });
                    Command.Parameters.Add(new MySqlParameter("sPlantCode", MySqlDbType.VarChar) { Value = plantcode });
                    Command.Parameters.Add(new MySqlParameter("sItemBarCode", MySqlDbType.VarChar) { Value = plantcode });
                    Command.Parameters.Add(new MySqlParameter("sParantBarCode", MySqlDbType.VarChar) { Value = plantcode });
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

        public async Task<Object> QCConfirm([FromBody] List<QualityConfirmation> confirmationlist)
        {
            DataTable dt = null;
            try
            {
                MySqlDataReader myReader = null;

                using (var conn = new MySqlConnection(connection))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var confirmation in confirmationlist)
                            {
                                dt = new DataTable();
                                using (MySqlCommand Command = new MySqlCommand())
                                {
                                    Command.Connection = conn;
                                    Command.Transaction = transaction;
                                    Command.CommandText = "sp_QualityConfirmation";
                                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "QC-Confirm";
                                    Command.Parameters.Add(new MySqlParameter("sPackingOrderNo", MySqlDbType.VarChar) { Value = confirmation.PackingOrderNo });
                                    Command.Parameters.Add(new MySqlParameter("sItemCode", MySqlDbType.VarChar) { Value = confirmation.MaterialCode });
                                    Command.Parameters.Add(new MySqlParameter("sLocation", MySqlDbType.VarChar) { Value = confirmation.StrLocCode });
                                    Command.Parameters.Add(new MySqlParameter("sBatchCode", MySqlDbType.VarChar) { Value = confirmation.BatchCode });
                                    Command.Parameters.Add(new MySqlParameter("sOkQty", MySqlDbType.Decimal) { Value = confirmation.OKQty });
                                    Command.Parameters.Add(new MySqlParameter("sNgQty", MySqlDbType.Decimal) { Value = confirmation.NGQty });
                                    Command.Parameters.Add(new MySqlParameter("sPackedQty", MySqlDbType.Decimal) { Value = confirmation.PackedQty });
                                    Command.Parameters.Add(new MySqlParameter("sQCDate", MySqlDbType.Date) { Value = confirmation.QCDate });
                                    Command.Parameters.Add(new MySqlParameter("sLineCode", MySqlDbType.VarChar) { Value = confirmation.LineNo });
                                    Command.Parameters.Add(new MySqlParameter("sPlantCode", MySqlDbType.VarChar) { Value = confirmation.PlantCode });
                                    Command.Parameters.Add(new MySqlParameter("sItemBarCode", MySqlDbType.VarChar) { Value = confirmation.ChildBarcode });
                                    Command.Parameters.Add(new MySqlParameter("sParantBarCode", MySqlDbType.VarChar) { Value = confirmation.CartonBarCode });
                                    Command.CommandType = CommandType.StoredProcedure;
                                    myReader = await Command.ExecuteReaderAsync();
                                    dt.Load(myReader);
                                }
                            }

                            // If all operations are successful, commit the transaction
                            transaction.Commit();
                            return dt;
                        }
                        catch (Exception ex)
                        {
                            // If an exception occurs, roll back the entire transaction
                            transaction.Rollback();
                        }
                    }
                }


                //foreach (var confirmation in confirmationlist)
                //{
                //    dt = new DataTable();
                //    using (MySqlCommand Command = new MySqlCommand())
                //    {
                //        Command.Connection = conn;
                //        Command.CommandText = "sp_QualityConfirmation";
                //        Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "QC-Confirm";
                //        Command.Parameters.Add(new MySqlParameter("sPackingOrderNo", MySqlDbType.VarChar) { Value = confirmation.PackingOrderNo });
                //        Command.Parameters.Add(new MySqlParameter("sItemCode", MySqlDbType.VarChar) { Value = confirmation.MaterialCode });
                //        Command.Parameters.Add(new MySqlParameter("sLocation", MySqlDbType.VarChar) { Value = confirmation.StrLocCode });
                //        Command.Parameters.Add(new MySqlParameter("sBatchCode", MySqlDbType.VarChar) { Value = confirmation.BatchCode });
                //        Command.Parameters.Add(new MySqlParameter("sOkQty", MySqlDbType.Decimal) { Value = confirmation.OKQty });
                //        Command.Parameters.Add(new MySqlParameter("sNgQty", MySqlDbType.Decimal) { Value = confirmation.NGQty });
                //        Command.Parameters.Add(new MySqlParameter("sPackedQty", MySqlDbType.Decimal) { Value = confirmation.PackedQty });
                //        Command.Parameters.Add(new MySqlParameter("sQCDate", MySqlDbType.Date) { Value = confirmation.QCDate });
                //        Command.Parameters.Add(new MySqlParameter("sLineCode", MySqlDbType.VarChar) { Value = confirmation.LineNo });
                //        Command.Parameters.Add(new MySqlParameter("sPlantCode", MySqlDbType.VarChar) { Value = confirmation.PlantCode });
                //        Command.CommandType = CommandType.StoredProcedure;
                //        Command.Connection.Open();
                //        myReader = await Command.ExecuteReaderAsync();
                //        dt.Load(myReader);
                //        Command.Connection.Close();
                //    }
                //}

               // return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return dt;
            }
            return null;

        }
    }
}
