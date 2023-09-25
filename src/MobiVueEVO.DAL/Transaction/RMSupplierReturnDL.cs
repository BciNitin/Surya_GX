using Common;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class RMSupplierReturnDL
    {
        public async Task<DataTable> GetDocumentNo()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_SUPPLIER_RETURN_SGST";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDPONO");
                    //cmd.Parameters.AddWithValue("@SITECODE", sSiteCode);
                    cmd.Parameters.AddWithValue("@SITECODE", 4);
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

        public async Task<DataTable> GetItemCode(string sDocumentNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_SUPPLIER_RETURN_SGST";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDPARTCODE");
                    cmd.Parameters.AddWithValue("@PONO", sDocumentNo);
                    cmd.Parameters.AddWithValue("@SITECODE", 4);
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

        public async Task<DataTable> GetItemLineNo(string sDocumentNo, string sItemCode
            )
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_SUPPLIER_RETURN_SGST";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDLINENO");
                    cmd.Parameters.AddWithValue("@PONO", sDocumentNo);
                    cmd.Parameters.AddWithValue("@PARTCODE", sItemCode);
                    cmd.Parameters.AddWithValue("@SITECODE", 4);
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

        public async Task<DataTable> GetOrderDetails(string sDocumentNo, string sItemCode, 
            string sItemLineNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_SUPPLIER_RETURN_SGST";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDDETAILS");
                    cmd.Parameters.AddWithValue("@PONO", sDocumentNo);
                    cmd.Parameters.AddWithValue("@PARTCODE", sItemCode);
                    cmd.Parameters.AddWithValue("@ITEMLINENO", sItemLineNo);
                    cmd.Parameters.AddWithValue("@SITECODE", 4);
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

        public async Task<DataTable> ValidateSupplierData(string sPONo, string sPartCode,
            string sItemLineNo, string sLocationCode, string sScannedBarcode, string UserID,
            string sSiteCode, string sLineCode)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_SUPPLIER_RETURN_SGST";
                    cmd.Parameters.AddWithValue("@TYPE", "VALIDATEBARCODE");
                    cmd.Parameters.AddWithValue("@PONO", sPONo);
                    cmd.Parameters.AddWithValue("@PARTCODE", sPartCode);
                    cmd.Parameters.AddWithValue("@ITEMLINENO", sItemLineNo);
                    cmd.Parameters.AddWithValue("@LOCATION", sLocationCode);
                    cmd.Parameters.AddWithValue("@PARTBARCODE", sScannedBarcode);
                    cmd.Parameters.AddWithValue("@SCANNEDBY", UserID);
                    cmd.Parameters.AddWithValue("@SITECODE", sSiteCode);
                    cmd.Parameters.AddWithValue("@LINECODE", sLineCode);
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
