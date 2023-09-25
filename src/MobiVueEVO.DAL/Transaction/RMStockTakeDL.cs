using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class RMStockTakeDL
    {
        public async Task<DataTable> GetItemNo()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "GETITEMCODE");
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }
        public async Task<DataTable> GetZone()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "GETZONE");
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }
        public async Task<DataTable> GetLocation(string Location)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "VALIDATELOCATION");
                    cmd.Parameters.AddWithValue("@LOCATION", Location);
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }
        public async Task<DataTable> SaveFullStock(DataTable stockTakeModel)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "SAVESTOCKTAKE");
                    cmd.Parameters.AddWithValue("@UTDETAIL", stockTakeModel);
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }
        public async Task<DataTable> GetPhysicalAdviceNo()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PHYSICAL_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "GETADVICENO");
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }
        public async Task<DataTable> GetPhysicalDetails(string adviceNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PHYSICAL_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "GETDETAILD");
                    cmd.Parameters.AddWithValue("@ADVICENO", adviceNo);
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }
        public DataTable GetPhysicalStock(string sStockArea, string sBarcode)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))

                try
                {

                    {
                        con.Open();
                        using (SqlCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "USP_PHYSICAL_STOCK_TAKE";
                            cmd.Parameters.AddWithValue("@TYPE", "GETDETAILS");
                            cmd.Parameters.AddWithValue("@STOCKAREA", sStockArea);
                            cmd.Parameters.AddWithValue("@BARCODE", sBarcode);
                            var dataReader = cmd.ExecuteReader();
                            dt.Load(dataReader);
                        }


                    }



                }
                catch (Exception ex)
                {
                    // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            return dt;
        }

        public async Task<DataTable> SaveFullPhysicalStock(string AdviceNo, string MaterialCode,
          string ScanItem, string Systemqty, string PhysicalStock, string Remarks)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PHYSICAL_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "SAVETDETAILS");
                    cmd.Parameters.AddWithValue("@ADVICENO", AdviceNo);
                    cmd.Parameters.AddWithValue("@MATERIALCODE", MaterialCode);
                    //cmd.Parameters.AddWithValue("@SCANITEM", ScanItem);
                    cmd.Parameters.AddWithValue("@SYSTEMQTY", Systemqty);
                    cmd.Parameters.AddWithValue("@PHYSICALSTOCK", PhysicalStock);
                    cmd.Parameters.AddWithValue("@REMARKS", Remarks);
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }

        public async Task<DataTable> GetScanItem(string iTEMNO)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PHYSICAL_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "VALIDATEITEM");
                    cmd.Parameters.AddWithValue("@ITEMNO", iTEMNO);
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }

        public async Task<DataTable> GetStockReconciliationDetails(string adviceNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PHYSICAL_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "RECONCILIATIONDETAILS");
                    cmd.Parameters.AddWithValue("@ADVICENO", adviceNo);
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }

        public async Task<DataTable> UpdateFullStockReconciliation(DataTable StockReconciliationModel)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_STOCK_TACK";
                    cmd.Parameters.AddWithValue("@TYPE", "SAVSTOCKRECONCILIATION");
                    cmd.Parameters.AddWithValue("@UTDETAILRECON", StockReconciliationModel);
                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return dt;
        }
    }
}
