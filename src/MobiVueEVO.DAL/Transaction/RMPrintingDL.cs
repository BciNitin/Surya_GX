using Common;
using Ionic.Zip;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class RMPrintingDL
    {
        public async Task<DataTable> GetGRN()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDGRN");

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

        public async Task<DataTable> BindGRNLineNo(string sGRNNO)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDGRNLINENO");
                    cmd.Parameters.AddWithValue("@GRN_No", sGRNNO);

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

        public async Task<DataTable> BindGRNDetail(string sGRNNO, string GRNLineNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDGRNDETAIL");
                    cmd.Parameters.AddWithValue("@GRN_No", sGRNNO);
                    cmd.Parameters.AddWithValue("@GRN_Line_num", GRNLineNo);
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

        public async Task<DataTable> SaveGRNPrinting(string sGRNNO, string sDOCUMENTTYPE,
            string sRECEVINGTRANSACTION, string sGRPODATE, string sTRANSACTIONTYPE,
            string sGRNLINENO, decimal sGRNQTY, decimal sRemqty, string sSUPPLIERCODE, string sSUPPLIERNAME,
            string sBATCHNO, string sPONO, string sITEMNO, string sPOLINENO, string sPOSCHEDULENO, int spacksize)

        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TYPE", "GRNPRINTING");
                    cmd.Parameters.AddWithValue("@BUSINESS_UNIT", "123");
                    cmd.Parameters.AddWithValue("@ORGANIZATION_ID", "677");
                    cmd.Parameters.AddWithValue("@Doc_Type", sDOCUMENTTYPE);
                    cmd.Parameters.AddWithValue("@GRN_No", sGRNNO);
                    cmd.Parameters.AddWithValue("@RCV_TRANSACTION_ID", sRECEVINGTRANSACTION);
                    cmd.Parameters.AddWithValue("@GRN_Date", sGRPODATE);
                    cmd.Parameters.AddWithValue("@Transaction_Type", sTRANSACTIONTYPE);
                    cmd.Parameters.AddWithValue("@GRN_Line_num", sGRNLINENO);
                    cmd.Parameters.AddWithValue("@GRN_QTY", sGRNQTY);
                    cmd.Parameters.AddWithValue("@REMQTY", sRemqty);
                    cmd.Parameters.AddWithValue("@Supplier_Code", sSUPPLIERCODE);
                    cmd.Parameters.AddWithValue("@Supplier_Name", sSUPPLIERNAME);
                    cmd.Parameters.AddWithValue("@Batch_no", sBATCHNO);
                    cmd.Parameters.AddWithValue("@PO_number", sPOLINENO);
                    cmd.Parameters.AddWithValue("@Item_Number", sITEMNO);
                    cmd.Parameters.AddWithValue("@PO_Line_no", sPOLINENO);
                    cmd.Parameters.AddWithValue("@PO_Schedule_No", sPOSCHEDULENO);
                    cmd.Parameters.AddWithValue("@ORGANIZATION_CODE", 4);
                    cmd.Parameters.AddWithValue("@PRINTEDQTY", spacksize);
                    cmd.CommandText = "USP_RM_PRINTING";
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

        //public void ZipAndDownloadFile(string sPRNFilePath)
        //{
        //    try
        //    {
        //        string sdownlodPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

        //        string sSearchParaMeter = "BCI_" + @"GXPrinting_";
        //        var SearchFile = Directory.GetFiles(sPRNFilePath).Where(file =>
        //            Regex.IsMatch(file, sSearchParaMeter));

        //        String[] files = SearchFile.ToArray();
        //        using (ZipFile zipFile = new ZipFile())
        //        {
        //            foreach (string item in files)
        //            {
        //                zipFile.AddFile(item, string.Empty);
        //            }
        //            //Set zip file name  
        //            zipFile.CompressionMethod = CompressionMethod.BZip2;
        //            zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
        //            //Save the zip content in output stream  
        //            zipFile.Save(sdownlodPath + @"\\" + "BCI_GXPrinting_" + Regex.Replace(DateTime.Now.ToString(), @"[-/:\s]+", "") + ".zip");
        //        }
        //        foreach (string file in files)
        //        {
        //            FileInfo fi = new FileInfo(file);
        //            fi.Delete();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //    }
        //}

        public async Task<DataTable> GetPRN(string sLabelType)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "GETPRN");
                    cmd.Parameters.AddWithValue("@PRNTYPE", "RM");

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

        public async Task<DataTable> GetPRNDetail(string itemNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_REPLACE_DATA";
                    cmd.Parameters.AddWithValue("@TYPE", "REPLACEDATA");
                    cmd.Parameters.AddWithValue("@GRN_NO", itemNo);

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

        public async Task<DataTable> GetREMQTY(string sGRNNO, string sGRNLineNO)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDREMQTY");
                    cmd.Parameters.AddWithValue("@GRN_No", sGRNNO);
                    cmd.Parameters.AddWithValue("@GRN_Line_num", sGRNLineNO);
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

        public async Task<DataTable> GetPrintDetail(string sGRNNO, string sGRNLineNO)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_PRINTING";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDPRINTDETAILS");
                    cmd.Parameters.AddWithValue("@GRN_No", sGRNNO);
                    cmd.Parameters.AddWithValue("@GRN_Line_num", sGRNLineNO);
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