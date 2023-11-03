using Abp.Application.Services;
using ELog.Application.Sessions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using ELog.Application.SelectLists.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MobiVueEVO.BO.Models;
using Ionic.Zlib;
using Microsoft.AspNetCore.Mvc;
using Abp.Runtime.Session;
using System.Linq;

namespace ELog.Application.ElogApi
{
    public class SuryaRevalidationDealerLocation : ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        private readonly IList<string> _roles;
        public SuryaRevalidationDealerLocation(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
            _roles = Task.Run(() => _sessionAppService.GetCurrentLoginInformations()).Result.Role;
        }

        public async Task<Object> GetDealerCode()
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_RevalidateDealerLocation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetDealerCode";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQty", MySqlDbType.VarChar).Value = 0;
                    Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sExpiryDate", MySqlDbType.VarChar).Value = default;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }
                foreach (DataRow dtRow in dt.Rows)
                {
                    string[] parts = Convert.ToString(dtRow["DealerCode"]).Split('|');
                    string code = parts[0].Trim();
                    string name = parts[1].Trim();

                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = code;
                    selectListDto.Value = name;

                    value.Add(selectListDto);

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return value;

        }

        public async Task<Object> GetRevalidationOnCarton([Required] string DealerCode, [Required] string BarCode)
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_RevalidateDealerLocation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ScanCartonBarCode";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = DealerCode;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = BarCode;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sQty", MySqlDbType.VarChar).Value = 0;
                    Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sExpiryDate", MySqlDbType.VarChar).Value = default;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return dt;
        }

        public async Task<Object> GetRevalidationOnItem([Required] string DealerCode, [Required] string ItemBarCode)
        {

            MySqlConnection conn = new MySqlConnection(connection);
            MySqlDataReader myReader = null;
            DataTable dt = new DataTable();
            List<SelectListDto> value = new List<SelectListDto>();
            try
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    Command.Connection = conn;
                    Command.CommandText = "sp_RevalidateDealerLocation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ScanItemBarCode";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = DealerCode;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = ItemBarCode;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sQty", MySqlDbType.VarChar).Value = 0;
                    Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sExpiryDate", MySqlDbType.VarChar).Value = default;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return dt;
        }

        public async Task<Object> ApproveRevalidationLocationByCarton(List<DealerRevalidation> dealer)
        {
            DataTable dt = null;
            try
            {
                MySqlDataReader myReader = null;
                var roles = _roles.Where(x => x.Contains("Admin") || x.Contains("Zonal Manager")).SingleOrDefault();
                if (roles != null)
                {
                    using (var conn = new MySqlConnection(connection))
                    {
                        conn.Open();

                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in dealer)
                                {
                                    dt = new DataTable();
                                    using (MySqlCommand Command = new MySqlCommand())
                                    {
                                        Command.Connection = conn;
                                        Command.Transaction = transaction;
                                        Command.CommandText = "sp_RevalidateDealerLocation";
                                        Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ApproveOnDealerLocationByCarton";
                                        Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = item.DealerCode;
                                        Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = item.ItemBarCode;
                                        Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = item.CartonBarCode;
                                        Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = item.MaterialCode;
                                        Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = item.BatchCode;
                                        Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = item.PackingDate;
                                        Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                                        Command.Parameters.Add("sQty", MySqlDbType.Decimal).Value = item.Qty;
                                        Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = roles;
                                        Command.Parameters.Add("sExpiryDate", MySqlDbType.VarChar).Value = default;
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
                                dt.Columns.Add("Error", typeof(string));
                                DataRow newRow = dt.NewRow();
                                newRow["Error"] = "Error~Error in Transaction.";
                                dt.Rows.Add(newRow);
                                return dt;
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return dt;
            }
            return null;

        }

        public async Task<Object> ApproveRevalidationLocationByItem(List<DealerRevalidation> dealer)
        {
            DataTable dt = null;
            try
            {
                MySqlDataReader myReader = null;
                var roles = _roles.Where(x => x.Contains("Admin") || x.Contains("ZonalManager")).SingleOrDefault();

                using (var conn = new MySqlConnection(connection))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in dealer)
                            {
                                dt = new DataTable();
                                using (MySqlCommand Command = new MySqlCommand())
                                {
                                    Command.Connection = conn;
                                    Command.Transaction = transaction;
                                    Command.CommandText = "sp_RevalidateDealerLocation";
                                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ApproveOnDealerLocationByItem";
                                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = item.DealerCode;
                                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = item.ItemBarCode;
                                    Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = item.CartonBarCode;
                                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = item.MaterialCode;
                                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = item.BatchCode;
                                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = item.PackingDate;
                                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                                    Command.Parameters.Add("sQty", MySqlDbType.Decimal).Value = item.Qty;
                                    Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = roles;
                                    Command.Parameters.Add("sExpiryDate", MySqlDbType.VarChar).Value = default;
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
                return dt;
            }
        }

        public async Task<Object> GetDelarLocationApproveDetails()
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                var roles = _roles.Where(x => x.Contains("Admin") || x.Contains("Zonal Manager")).SingleOrDefault();
                if (roles != null)
                {
                    using (MySqlCommand Command = new MySqlCommand())
                    {
                        Command.Connection = conn;
                        Command.CommandText = "sp_Approval_Revalidation_Location";
                        Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetDelarLocationApprovedDetails";
                        Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("sId", MySqlDbType.Int32).Value = 0;
                        Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("sShiperBarCode", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("sMfgDate", MySqlDbType.DateTime).Value = default;
                        Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("Qty", MySqlDbType.Decimal).Value = 0;
                        Command.Parameters.Add("ExpiryDate", MySqlDbType.DateTime).Value = default;
                        Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = roles;
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Connection.Open();
                        myReader = await Command.ExecuteReaderAsync();
                        dt.Load(myReader);
                        Command.Connection.Close();
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

        public async Task<Object> GetDelarLocationApproveDetailsById([Required] string dealercode, [Required] string itemcode)
        {

            try
            {
                var roles = _roles.Where(x => x.Contains("Admin") || x.Contains("Zonal Manager")).SingleOrDefault();
                if (roles != null)
                {
                    MySqlConnection conn = new MySqlConnection(connection);
                    MySqlDataReader myReader = null;
                    DataTable dt = new DataTable();
                    using (MySqlCommand Command = new MySqlCommand())
                    {
                        Command.Connection = conn;
                        Command.CommandText = "sp_Approval_Revalidation_Location";
                        Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetApprovalDtlsById";
                        Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = dealercode;
                        Command.Parameters.Add("sId", MySqlDbType.Int32).Value = 0;
                        Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("sShiperBarCode", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = itemcode;
                        Command.Parameters.Add("sMfgDate", MySqlDbType.DateTime).Value = default;
                        Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                        Command.Parameters.Add("Qty", MySqlDbType.Decimal).Value = 0;
                        Command.Parameters.Add("ExpiryDate", MySqlDbType.DateTime).Value = default;
                        Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = roles;
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Connection.Open();
                        myReader = await Command.ExecuteReaderAsync();
                        dt.Load(myReader);
                        Command.Connection.Close();
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> ApproveRevalidation([Required]List<string> itembarcode)
        {
            DataTable dt = null;
            try
            {
                MySqlDataReader myReader = null;
                var roles = _roles.Where(x => x.Contains("Admin") || x.Contains("Zonal Manager")).SingleOrDefault();
                if (roles != null)
                {
                    using (var conn = new MySqlConnection(connection))
                    {
                        conn.Open();

                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in itembarcode)
                                {
                                    dt = new DataTable();
                                    using (MySqlCommand Command = new MySqlCommand())
                                    {
                                        Command.Connection = conn;
                                        Command.CommandText = "sp_Approval_Revalidation_Location";
                                        Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "Approved";
                                        Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = String.Empty;
                                        Command.Parameters.Add("sId", MySqlDbType.Int32).Value = 0;
                                        Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                                        Command.Parameters.Add("sShiperBarCode", MySqlDbType.VarChar).Value = String.Empty;
                                        Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = itembarcode;
                                        Command.Parameters.Add("sMfgDate", MySqlDbType.DateTime).Value = default;
                                        Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                                        Command.Parameters.Add("Qty", MySqlDbType.Decimal).Value = 0;
                                        Command.Parameters.Add("ExpiryDate", MySqlDbType.DateTime).Value = default;
                                        Command.Parameters.Add("sUserType", MySqlDbType.VarChar).Value = roles;
                                        Command.CommandType = CommandType.StoredProcedure;
                                        Command.Connection.Open();
                                        myReader = await Command.ExecuteReaderAsync();
                                        dt.Load(myReader);
                                        Command.Connection.Close();
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
                                dt.Columns.Add("Error", typeof(string));
                                DataRow newRow = dt.NewRow();
                                newRow["Error"] = "Error~Error in Transaction.";
                                dt.Rows.Add(newRow);
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return dt;
            }
            return dt;
        }
    }
}

