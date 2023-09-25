using Common;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class RMPutawayDL
    {
        public async Task<DataTable> ValidateLocation(string Location)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PUTWAY";
                    cmd.Parameters.AddWithValue("@TYPE", "VALIDATELOCATION");
                    cmd.Parameters.AddWithValue("@LOCATION", Location);
                    cmd.Parameters.AddWithValue("@SITECODE", 4);
                    cmd.Parameters.AddWithValue("@PUTWAYBY", "1");

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

        public async Task<DataTable> PutawayAndValidateBarcode(string Location, string barCode)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PUTWAY";
                    cmd.Parameters.AddWithValue("@TYPE", "VALIDATEBARCODE");
                    cmd.Parameters.AddWithValue("@LOCATION", Location);
                    cmd.Parameters.AddWithValue("@BARCODE", barCode);
                    cmd.Parameters.AddWithValue("@SITECODE", 4);
                    cmd.Parameters.AddWithValue("@PUTWAYBY", "");

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