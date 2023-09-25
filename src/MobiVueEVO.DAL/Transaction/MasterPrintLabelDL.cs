using Common;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL.Transaction
{
    public class MasterPrintLabelDL
    {
        public async Task<DataTable> GetPRN(string labelType)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_LABEL_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "GETPRN");
                    cmd.Parameters.AddWithValue("@LABELTYPE", labelType);

                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException ex)
                {
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                    // Handle the specific SQL exception appropriately (e.g., logging, error reporting, etc.)
                    throw ex;
                }
            }

            return dt;
        }

        public async Task<DataTable> GetPRNDetail(string labelType)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_LABEL_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDPRNDETAILS");
                    cmd.Parameters.AddWithValue("@LABELTYPE", labelType);

                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException ex)
                {
                    // Handle the specific SQL exception appropriately (e.g., logging, error reporting, etc.)
                    throw ex;
                }
            }

            return dt;
        }

        public async Task<DataTable> GetPRNDetail(string labelType, DataTable Ids)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_LABEL_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDPRNDETAILS");
                    cmd.Parameters.AddWithValue("@LABELTYPE", labelType);
                    cmd.Parameters.AddWithValue("@Ids", Ids);

                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException ex)
                {
                    // Handle the specific SQL exception appropriately (e.g., logging, error reporting, etc.)
                    throw ex;
                }
            }

            return dt;
        }

        public async Task<DataSet> GetMultipleQueryDataSetFromStoredProcedures()
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    await con.OpenAsync();
                    // Execute the first stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "StoredProcedure1";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDPRNDETAILS");

                    using SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(dataSet);
                }
                catch (SqlException ex)
                {
                    // Handle the specific SQL exception appropriately (e.g., logging, error reporting, etc.)
                    throw ex;
                }
            }

            return dataSet;
        }
    }
}
