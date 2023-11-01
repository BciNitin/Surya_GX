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

namespace ELog.Application.ElogApi
{
    public class SuryaRevalidationDealerLocation : ApplicationService
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public SuryaRevalidationDealerLocation(IConfiguration configuration, ISessionAppService sessionAppService)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
            _sessionAppService = sessionAppService;
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

        public async Task<Object> GetRevalidationDetails(string DealerCode,string BarCode)
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
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ScanBarCode";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = DealerCode;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = BarCode;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sQty", MySqlDbType.VarChar).Value = 0;
                    

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

        public async Task<Object> ApproveRevalidationLocation(DealerRevalidation dealer)
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
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ApproveOnDealerLocation";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = dealer.DealerCode;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = dealer.BatchCode;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = dealer.ItemBarCode;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = dealer.ParentBarCode;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = dealer.PackingDate;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = dealer.MaterialCode;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sQty", MySqlDbType.Decimal).Value = dealer.Qty;

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

        public async Task<Object> GetApproveDetails(DealerRevalidation dealer)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_RevalidateDealerLocation";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ApproveDetails";
                    Command.Parameters.Add("sDealerCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sBatchCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sParantBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.DateTime).Value = default;
                    Command.Parameters.Add("sMaterialCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;
                    Command.Parameters.Add("sQty", MySqlDbType.Decimal).Value = 0;
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

        public async Task<Object> GetApprovalDtlsById(string id)
        {

            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = "sp_Approval_for_ZonalManager";
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "GetApprovalDtlsById";
                    Command.Parameters.Add("sId", MySqlDbType.VarChar).Value = id;
                    Command.Parameters.Add("sShiperBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sMfgDate", MySqlDbType.VarChar).Value = default;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = String.Empty;
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
