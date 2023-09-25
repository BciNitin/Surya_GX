using Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class RMQualityCheckDL
    {
        public async Task<DataTable> GetGRNNo()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_QUALITY";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDGRNO");
                    //cmd.Parameters.AddWithValue("@GRNNO", sGRNNo);

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

        public async Task<DataTable> BindInventoryDetail(string sGRNNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_QUALITY";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDINVENTORYDETAILS");
                    cmd.Parameters.AddWithValue("@GRNNO", sGRNNo);

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
        public async Task<DataTable> SCANBARCODEBindInventoryDetail(string sGRNNo, string ItemNO)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_QUALITY";
                    cmd.Parameters.AddWithValue("@TYPE", "SCANBARCODE");
                    cmd.Parameters.AddWithValue("@GRNNO", sGRNNo);
                    cmd.Parameters.AddWithValue("@PARTBARCODE", ItemNO);

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

        public async Task<DataTable> SaveQualityCheckFullBatch(string type, int status,
             string sGRNNo, DataTable sItemNo, string sRemarks, bool isFullBatch
        )
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_QUALITY";
                    cmd.Parameters.AddWithValue("@TYPE", type);
                    cmd.Parameters.AddWithValue("@STATUS", status); //OK - 1, REJECT -2 , REWORK - 3, SCRAP -4
                    cmd.Parameters.AddWithValue("@GRNNO", sGRNNo);
                    cmd.Parameters.AddWithValue("@UTDETAILS", sItemNo);
                    cmd.Parameters.AddWithValue("@REMARKS", sRemarks);
                    cmd.Parameters.AddWithValue("@ISFULLBATCH", isFullBatch);

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

        public async Task<DataTable> GetItemDetail(DataTable dtItemNos, string sGRNNo)
        {

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_QUALITY";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDITEMDETAILS");
                    cmd.Parameters.AddWithValue("@GRNNO", sGRNNo);
                    cmd.Parameters.AddWithValue("@UTDETAILS", dtItemNos);

                    await con.OpenAsync();

                    using SqlDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    dt.Load(dataReader);
                }
                catch (SqlException ex)
                {

                    throw ex;
                }
            }

            return dt;
        }
    }
}
