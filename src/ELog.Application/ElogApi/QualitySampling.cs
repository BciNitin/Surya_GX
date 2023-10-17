using Abp.Application.Services;
using ELog.Application.CommomUtility;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Sessions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Application.ElogApi
{
    public class QualitySampling: ApplicationService
    {
        private readonly IConfiguration _configuration;
        private readonly string connection;
        public QualitySampling(IConfiguration configuration) 
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
        }

        public async Task<Object> QualityCheckingSave([Required] string PackingOrderNo, [Required] string PlantCode, [Required] string CartonBarCode,[Required] string LineCode)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_QualitySampling;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.SaveQualitySampling;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineCode;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = CartonBarCode;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = PackingOrderNo;
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

        public async Task<Object> GetQualityCheckingQty([Required] string PackingOrderNo, [Required] string PlantCode,[Required] string LineCode)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_QualitySampling;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetQualitySamplingQty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineCode;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = "";
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = PackingOrderNo;
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

        public async Task<Object> ScanCartonBarCode([Required] string PackingOrderNo, [Required] string PlantCode, [Required] string CartonBarCode, [Required] string LineCode)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_QualitySampling;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ScanCartonBarCode";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineCode;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = CartonBarCode;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = PackingOrderNo;
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

        public async Task<Object> ScanItemBarCode([Required] string PackingOrderNo, [Required] string PlantCode, [Required] string CartonBarCode, [Required] string ItemBarCode, [Required] string LineCode)
        {
            try
            {

                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText = Constants.sp_QualitySampling;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = "ScanItemBarCode";
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineCode;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = CartonBarCode;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = ItemBarCode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = PackingOrderNo;
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

                    Command.CommandText = Constants.sp_QualitySampling;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetPackingOrder;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineNo;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = String.Empty;
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
