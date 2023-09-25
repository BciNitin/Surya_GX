using Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class RMIssuetoshopfloorDL
    {

        public DataTable GetWORKORDERNO()
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
                            cmd.Parameters.AddWithValue("@TYPE", "BINDISSUESLIPNO");
                            cmd.CommandText = "USP_RM_ISSUE_SHOP_FLOR";
                            cmd.Parameters.AddWithValue("@SITECODE", 4);

                            var dataReader = cmd.ExecuteReader();
                            dt.Load(dataReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            return dt;

        }

        public DataSet FATCHFGITEMCODE(string WORKORDERNO)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
                try
                {

                    {
                        con.Open();
                        using (SqlCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TYPE", "BINDISSUEDETAILS");
                            cmd.CommandText = "USP_RM_ISSUE_SHOP_FLOR";
                            cmd.Parameters.AddWithValue("@ISSUESLIPNO", WORKORDERNO);
                            cmd.Parameters.AddWithValue("@SITECODE", 4);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds);
                            //var dataReader = cmd.ExecuteReader();
                            //ds.Load(dataReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            return ds;

        }


        public DataTable ValidateLocation(string _WORKORDERNO, string _ScanLocation)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
                try
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TYPE", "VALIDATELOCATION");
                        cmd.CommandText = "USP_RM_ISSUE_SHOP_FLOR";
                        // cmd.Parameters.AddWithValue("@PARTCODE", _FgitemCode);
                        cmd.Parameters.AddWithValue("@ISSUESLIPNO", _WORKORDERNO);
                        cmd.Parameters.AddWithValue("@LOCATION", _ScanLocation);
                        cmd.Parameters.AddWithValue("@SITECODE", 4);
                        var dataReader = cmd.ExecuteReader();
                        dt.Load(dataReader);

                    }
                }

                catch (Exception ex)
                {
                    PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            return dt;
        }
        public DataTable ValidateBarcode(string _WORKORDERNO, string _FGItemCode, string _ScanLocation, string sPartBarcode)
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
                            cmd.Parameters.AddWithValue("@TYPE", "VALIDATEBARCODE");
                            cmd.CommandText = "USP_RM_ISSUE_SHOP_FLOR";
                            cmd.Parameters.AddWithValue("@PARTCODE", _FGItemCode);
                            cmd.Parameters.AddWithValue("@ISSUESLIPNO", _WORKORDERNO);
                            cmd.Parameters.AddWithValue("@LOCATION", _ScanLocation);
                            cmd.Parameters.AddWithValue("@PARTBARCODE", sPartBarcode);
                            cmd.Parameters.AddWithValue("@SITECODE", 4);
                            cmd.Parameters.AddWithValue("@LINECODE", 1);
                            var dataReader = cmd.ExecuteReader();
                            dt.Load(dataReader);
                        }
                    }
                }

                catch (Exception ex)
                {
                    PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            return dt;
        }

    }
}
