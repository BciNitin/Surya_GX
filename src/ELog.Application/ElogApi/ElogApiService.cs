
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;

using Microsoft.AspNetCore.Http;

using ELog.Application.Modules;
using ELog.Application.Sessions;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
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

namespace ELog.Application.ElogApi
{


/*
    [PMMSAuthorize]*/
    public class ElogApiService : ApplicationService, IElogApiService
    {
        private readonly IConfiguration _configuration;
        private readonly PMMSDbContext _context;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionAppService _sessionAppService;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Loading> _loadingRepository;
        private readonly IModuleAppService _moduleAppService;
        public  ElogApiService(
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ISessionAppService sessionAppService,
        IRepository<User, long> userRepository, IRepository<PlantMaster> plantRepository,
        IRepository<Loading> loadingRepository,
        IModuleAppService moduleAppService

          )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _sessionAppService = sessionAppService;
            _userRepository = userRepository;
            _plantRepository = plantRepository;
            _httpContextAccessor = httpContextAccessor;
            _loadingRepository = loadingRepository;
            _moduleAppService = moduleAppService;
        }



        public async Task<Object> All_showAllTables()
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("showAllTables", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@DatabaseName", SqlDbType.VarChar).Value = "INFORMATION_SCHEMA.TABLES";

                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/

                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<Object> FormDataPush(string TableName, string ColumnName, string Values, string ActionType)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("spInsertLogWiseData", conn);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = "";
                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = TableName;
                myCommand.Parameters.Add("@ColumnName", SqlDbType.VarChar).Value = ColumnName;
                myCommand.Parameters.Add("@Values", SqlDbType.VarChar).Value = Values;
                myCommand.Parameters.Add("@ActionType", SqlDbType.VarChar).Value = ActionType;
                myCommand.Parameters.Add("@DynamicUpdateQuery", SqlDbType.VarChar).Value = "";
                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);


                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<object> UpdateLogWiseData(int Id, string tablename, string ColumnName, string Value, string ActionType)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);
            string DynamicUpdateQuery = null;
            int returnResult = 1;
            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            string[] columnPlus = ColumnName.Split(',');
            string[] valuePlus = Value.Split(',');

            for (int i = 0; i < columnPlus.Length; i++)
            {
                DynamicUpdateQuery += columnPlus[i].Trim() + '=' + valuePlus[i].Trim() + ',';
            }
            DynamicUpdateQuery = DynamicUpdateQuery.TrimEnd(',');
            DynamicUpdateQuery = "Update " + tablename + " set " + DynamicUpdateQuery + " where ID =" + Id;



            using (var cm = conn.CreateCommand())
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("spInsertLogWiseData", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = Id;
                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@ColumnName", SqlDbType.VarChar).Value = ColumnName;
                myCommand.Parameters.Add("@Values", SqlDbType.VarChar).Value = Value;
                myCommand.Parameters.Add("@ActionType", SqlDbType.VarChar).Value = ActionType;
                myCommand.Parameters.Add("@DynamicUpdateQuery", SqlDbType.VarChar).Value = DynamicUpdateQuery;


                //cm.ExecuteNonQuery();
                //   myReader = myCommand.ExecuteReader();

                returnResult = myCommand.ExecuteNonQuery();

                //DataTable tab = new DataTable();
                //tab.Load(myReader);
                if (returnResult == 0)
                {
                    return "Success";
                }

                //   return tab;
            }
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
        //public async Task<Object> GetcreateFormsAsync(string tablename, int pagenumber, int pagesize)
        //{
        //    string connection = _configuration["ConnectionStrings:Default"];
        //    SqlConnection conn = null;
        //    //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //    conn = new SqlConnection(connection);

        //    try
        //    {
        //        conn.Open();
        //    }
        //    catch (Exception)
        //    {
        //        string e = "Database error contact administrator";

        //    }
        //    try
        //    {
        //        SqlDataReader myReader = null;
        //        SqlCommand myCommand = new SqlCommand("createForms", conn);
        //        myCommand.CommandType = CommandType.StoredProcedure;

        //        myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
        //        myCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pagenumber;
        //        myCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = pagesize;


        //        myReader = myCommand.ExecuteReader();
        //        DataTable tab = new DataTable();
        //        tab.Load(myReader);

        //        /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
        //          while (myReader.Read())
        //          {
        //              JsonObjectCollection jsonObject = new JsonObjectCollection();
        //  for(string columnName in columns)
        //                  jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
        //              jsonArray.Add(jsonObject);
        //          }*/
        //        return tab;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    return null;

        //}

        public async Task<Object> GetcreateFormsAsync(string tablename, int pagenumber, int pagesize)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            DataSet tab = new DataSet();
            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                int totalCount = 0;
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("createForms", conn);
                myCommand.CommandType = CommandType.StoredProcedure;


                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pagenumber;
                myCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = pagesize;
                // myReader = myCommand.ExecuteReader();
                //DataTable tab = new DataTable();

                var adapter = new SqlDataAdapter(myCommand);
                // tab = new DataSet();
                adapter.Fill(tab);

                var result = new
                {
                    Result = tab.Tables[0],
                    TotalCount = tab.Tables[1]
                };

                // Return the result object
                return result;


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return tab;

        }
        public async Task<object> showTableColumns(string tablename)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("showTableColumns", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;

                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);

                /*  JsonArrayCollection jsonArray = new JsonArrayCollection();
                  while (myReader.Read())
                  {
                      JsonObjectCollection jsonObject = new JsonObjectCollection();
          for(string columnName in columns)
                          jsonObject.Add(new JsonStringValue(columnName, myReader[columnName]));
                      jsonArray.Add(jsonObject);
                  }*/

                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<SetfetchNewDto> setFetch(string tablename)
        {
            return new SetfetchNewDto();
        }
        public async Task<SetfetchDto> setoldFetch(string tablename)
        {
            return new SetfetchDto();
        }

        public async Task<Object> GetspTableStructureToBeCreateAsync(string tablename, string ColumnName, string ActionType)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("spTableStructureToBeCreate", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@ColumnName", SqlDbType.VarChar).Value = ColumnName;
                myCommand.Parameters.Add("@ActionType", SqlDbType.VarChar).Value = ActionType;
                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                tab.Load(myReader);


                return tab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<String> FetchTableWiseData(string tablename, string ColumnName, int Limit, string Condition, string ConditionText)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new MySqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {

                // MySqlCommand myCommand = new MySqlCommand("spFetchTableWiseData", conn);
                // myCommand.CommandType = CommandType.StoredProcedure;
                // MySqlDataAdapter myReader = new MySqlDataAdapter(myCommand);
                // myCommand.Parameters.Add("@TableName", MySqlDbType .VarChar).Value = tablename;
                // myCommand.Parameters.Add("@ColumnName", MySqlDbType.VarChar).Value = ColumnName;
                // myCommand.Parameters.Add("@Limit", MySqlDbType.VarChar).Value = Limit;
                // myCommand.Parameters.Add("@Condition", MySqlDbType.VarChar).Value = Condition;
                // myCommand.Parameters.Add("@ConditionText", MySqlDbType.VarChar).Value = ConditionText;

                //// myReader = myCommand.ExecuteReader();
                // DataTable tab = new DataTable();
                // //Dictionary<string, object> row;
                // myReader.Fill(tab);
                //tab.Load(myReader);
                DbDataReader myReader = null;
                DataTable tab = new DataTable();

                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "spFetchTableWiseData";
                    Command.Parameters.Add("p_TableName", MySqlDbType.VarChar).Value = tablename;
                    Command.Parameters.Add("p_ColumnName", MySqlDbType.LongText).Value = ColumnName;
                    Command.Parameters.Add("p_Limit", MySqlDbType.Int32).Value = Limit;
                    Command.Parameters.Add("p_Condition", MySqlDbType.VarChar).Value = Condition;
                    Command.Parameters.Add("p_ConditionText", MySqlDbType.VarChar).Value = ConditionText;


                    Command.CommandType = CommandType.StoredProcedure;
                    myReader = await Command.ExecuteReaderAsync();
                    tab.Load(myReader);
                    Command.Connection.Close();
                }
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(tab);
                //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                //foreach (DataRow dr in tab.Rows)
                //{
                //    row = new Dictionary<string, object>();
                //    foreach (DataColumn col in tab.Columns)
                //    {
                //        row.Add(col.ColumnName, dr[col]);
                //    }
                //    rows.Add(row);
                //}
                string myString = JsonConvert.ToString(JSONresult);
                return JSONresult;
                /* 


                  return tab.ToString();*/
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public async Task<String> GetDistictColumnWiseData(string tablename, string ColumnName, int Limit)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("spGetColumnWiseData", conn);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tablename;
                myCommand.Parameters.Add("@ColumnName", SqlDbType.VarChar).Value = ColumnName;
                myCommand.Parameters.Add("@Limit", SqlDbType.VarChar).Value = Limit;


                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                Dictionary<string, object> row;
                tab.Load(myReader);
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(tab);
                /*  List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                  foreach (DataRow dr in tab.Rows)
                  {
                      row = new Dictionary<string, object>();
                      foreach (DataColumn col in tab.Columns)
                      {
                          row.Add(col.ColumnName, dr[col]);
                      }
                      rows.Add(row);
                  }*/
                //  string myString = JsonConvert.ToString(JSONresult);
                return JSONresult;
                /* 


                  return tab.ToString();*/
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public async Task<String> GetCheckpointReportWise(string ConditionText)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            SqlConnection conn = null;
            //  string connection = "Server=tcp:pmmswip.database.windows.net,1433;Initial Catalog=pmmsdev;Persist Security Info=False;User ID=pmmsadmin;Password=ZGA9uCFA3ZNksGtj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn = new SqlConnection(connection);

            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                string e = "Database error contact administrator";

            }
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("GetCheckpointReportWise", conn);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add("@ConditionText", SqlDbType.VarChar).Value = ConditionText;

                myReader = myCommand.ExecuteReader();
                DataTable tab = new DataTable();
                Dictionary<string, object> row;
                tab.Load(myReader);
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(tab);
                return JSONresult;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public async Task<Object> GetClientForm(int Id)
        {
            string connection = _configuration["ConnectionStrings:Default"];
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);

            try
            {
                DbDataReader myReader = null;
                DataTable dt = new DataTable();

                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = "get_clientforms";
                    Command.Parameters.Add("p_ID", MySqlDbType.Int32).Value = Id;
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