using Common;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class RMJobworkDL
    {
        public async Task<DataTable> GetIssueSlipNo()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_JOB_WORK";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDISSUENO");
                    cmd.Parameters.AddWithValue("@ORGCODE", "GX_MANESAR_IO");
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

        public async Task<DataTable> GetItemCode(string issueSlipNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_JOB_WORK";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDITEMNO");
                    cmd.Parameters.AddWithValue("@ISSUESLIPNO", issueSlipNo);
                    cmd.Parameters.AddWithValue("@ORGCODE", "GX_MANESAR_IO");
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

        public async Task<DataTable> GetItemLineNo(string issueSlipNo, string itemNumber)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_JOB_WORK";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDLINENO");
                    cmd.Parameters.AddWithValue("@ISSUESLIPNO", issueSlipNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", itemNumber);
                    cmd.Parameters.AddWithValue("@ORGCODE", "GX_MANESAR_IO");
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

        public async Task<DataTable> GetGridDetail(string issueSlipNo, string itemNo, string itemLineNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_JOB_WORK";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDGRIDDETAIL");
                    cmd.Parameters.AddWithValue("@ISSUESLIPNO", issueSlipNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", itemNo);
                    cmd.Parameters.AddWithValue("@ITEMLINENO", itemLineNo);
                    cmd.Parameters.AddWithValue("@ORGCODE", "GX_MANESAR_IO");
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

        public async Task<DataTable> SaveJobWork(string issueSlipNo, string itemNo, string itemLineNo,
            string barCode, string location)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_JOB_WORK";
                    cmd.Parameters.AddWithValue("@TYPE", "SAVEJOBWORK");
                    cmd.Parameters.AddWithValue("@ISSUESLIPNO", issueSlipNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", itemNo);
                    cmd.Parameters.AddWithValue("@ITEMLINENO", itemLineNo);
                    cmd.Parameters.AddWithValue("@LOCATION", location);
                    cmd.Parameters.AddWithValue("@ORGCODE", "GX_MANESAR_IO");
                    cmd.Parameters.AddWithValue("@PARTBARCODE", barCode);

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
    }
}
