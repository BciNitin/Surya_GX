using Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class RMMaterialReturnFromShopFloorDL
    {
        public async Task<DataTable> GetSlipNo()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDSLIPNO");
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

        public async Task<DataTable> GetItemNo(string slipNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDITEMNO");
                    cmd.Parameters.AddWithValue("@SLIPNO", slipNo);
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

        public async Task<DataTable> GetItemLineNo(string slipNo, string itemNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDITEMLINENO");
                    cmd.Parameters.AddWithValue("@SLIPNO", slipNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", itemNo);
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

        public async Task<DataTable> GetItemDEtailsSFG(string slipNo, string itemNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDITEMDETAILS");
                    cmd.Parameters.AddWithValue("@SLIPNO", slipNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", itemNo);
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

        public async Task<DataTable> GetItemDEtailsRM(string slipNo, string itemNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDITEMDETAILSRM");
                    cmd.Parameters.AddWithValue("@SLIPNO", slipNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", itemNo);
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
        public async Task<DataTable> GetScanBarcode(string sScanBarcode)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "SCANBARCODESRM");
                    cmd.Parameters.AddWithValue("@BARCODE", sScanBarcode);
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
        
        public async Task<DataTable> SaveMRNReturnFromShopFLoor(string MaterialNO, string ItemCode,
          string LineNo, string ItemNo, string ScanBarcode, string Qty)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "SAVEMRNRETURN");
                    cmd.Parameters.AddWithValue("@SLIPNO", MaterialNO);
                   // cmd.Parameters.AddWithValue("@ITEMNO", ItemCode);
                   // cmd.Parameters.AddWithValue("@LOCATION", LineNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", ItemNo);
                    cmd.Parameters.AddWithValue("@BARCODE", ScanBarcode);
                    cmd.Parameters.AddWithValue("@QTY", Qty);
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

        public async Task<DataTable> ReturnFromShopFLoor(string slipNo, string itemNo, string location, string barcode)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "VALIDATEBARCODE");
                    cmd.Parameters.AddWithValue("@SLIPNO", slipNo);
                    cmd.Parameters.AddWithValue("@ITEMNO", itemNo);
                    cmd.Parameters.AddWithValue("@LOCATION", location);
                    cmd.Parameters.AddWithValue("@BARCODE", barcode);
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


        // OLD
        public async Task<DataTable> GetMaterialReturnFromShopFloorDetails(string Type, string WorkOrder,
            string BarCode, string siteCode)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "VALIDATEREELBARCODE");
                    cmd.Parameters.AddWithValue("@PARTBARCODE", BarCode);
                    cmd.Parameters.AddWithValue("@WORKORDERNO", WorkOrder);
                    cmd.Parameters.AddWithValue("@RETURNTYPE", Type);
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

        public async Task<DataTable> PrintBarcodeLabel(string _MRNNo, string _Part_Barcode,
            string sScanBy, decimal dReturnQty, string sSiteCode, string _PartCode,
            string sWorkOrderNo, string sScanType, string sUserID, string sLineCode, string sRemarks)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_RM_RETURN";
                    cmd.Parameters.AddWithValue("@TYPE", "MRNLABELPRINTING");
                    cmd.Parameters.AddWithValue("@PARTBARCODE", _Part_Barcode);
                    cmd.Parameters.AddWithValue("@MRNNO", _MRNNo);
                    cmd.Parameters.AddWithValue("@PRINTEDBY", sScanBy);
                    cmd.Parameters.AddWithValue("@RETURNQTY", dReturnQty);
                    cmd.Parameters.AddWithValue("@SITECODE", sSiteCode);
                    cmd.Parameters.AddWithValue("@PARTCODE", _PartCode);
                    cmd.Parameters.AddWithValue("@LINECODE", sLineCode);
                    cmd.Parameters.AddWithValue("@WORKORDERNO", sWorkOrderNo);
                    cmd.Parameters.AddWithValue("@RETURNTYPE", sScanType);
                    cmd.Parameters.AddWithValue("@REMARKS", sRemarks);
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

        public async Task<DataTable> GetPRN(string sType)
        {
            DataTable dtGetPRN = new DataTable();

            string query = "SELECT PRN_VALUE FROM mPRNMASTER WHERE TYPE = @Type";

            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Type", sType);

                try
                {
                    await con.OpenAsync();

                    using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    await Task.Run(() => adapter.Fill(dtGetPRN));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dtGetPRN;
        }

        public string GetReturnPrintingDetails(string sPRN, string sMRNNo, string sPartCode,
            string sPartBarcode, decimal dQty, string sParentBarcode)
        {
            string sFinalPRN = string.Empty;
            try
            {
                DL_Common dlobj = new DL_Common();

                string query = $@"SELECT DISTINCT TR.GRPONO, FORMAT(TR.GRPO_DATE, 'dd/MM/yyyy') PO_DATE, TR.SUP_INV_NO, TRI.RI_ID,
                            TR.SUP_INV_DATE, TR.MPN, TRI.PART_CODE, MM.PART_DESC,
                            TRI.BATCHNO, TR.VENDOR_NAME, TRI.EXP_DATE, TRI.MFG_DATE,
                            TRI.APPROVED_QTY, TRI.REJECT_QTY, TRI.LOT_QTY,
                            TRI.LH_RH, TRI.MSL_INFO, TRI.PRINTED_BY
                        FROM RM_RECEIVING TR
                        INNER JOIN RM_INVENTORY_H TRI ON TRI.RM_ID = TR.RM_ID
                        INNER JOIN MPARTCODEMASTER MM ON MM.PART_CODE = TRI.PART_CODE
                        WHERE PART_BARCODE = @ParentBarcode
                            AND TRI.PART_CODE = @PartCode
                        ORDER BY TRI.RI_ID DESC";

                using SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString);
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ParentBarcode", sParentBarcode);
                cmd.Parameters.AddWithValue("@PartCode", sPartCode);

                con.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                if (dt.Rows.Count > 0)
                {
                    string prn = sPRN;
                    var partBarcodeSplit = sPartBarcode.Split(',');

                    prn = prn.Replace("{MPN}", dt.Rows[0]["MPN"].ToString());
                    prn = prn.Replace("{BATCHNO}", dt.Rows[0]["BATCHNO"].ToString());
                    prn = prn.Replace("{USERID}", dt.Rows[0]["PRINTED_BY"].ToString());
                    prn = prn.Replace("{BARCODE}", sPartBarcode);

                    if (partBarcodeSplit.Length > 1)
                    {
                        prn = prn.Replace("{BARCODE_HR}", $"{partBarcodeSplit[1]}, {partBarcodeSplit[2]}, {partBarcodeSplit[3]}");
                    }
                    else
                    {
                        prn = prn.Replace("{BARCODE_HR}", sPartBarcode);
                    }

                    prn = prn.Replace("{SAPPARTCODE}", dt.Rows[0]["PART_CODE"].ToString());
                    prn = prn.Replace("{PARTDESC}", dt.Rows[0]["PART_DESC"].ToString());
                    prn = prn.Replace("{GRPO}", dt.Rows[0]["GRPONO"].ToString().Split('(')[0]);
                    prn = prn.Replace("{GRPODT}", dt.Rows[0]["PO_DATE"].ToString());
                    prn = prn.Replace("{LOT}", dt.Rows[0]["MFG_DATE"].ToString());
                    prn = prn.Replace("{LF}", dt.Rows[0]["LH_RH"].ToString());
                    prn = prn.Replace("{MSL}", dt.Rows[0]["MSL_INFO"].ToString());
                    prn = prn.Replace("{LOC}", PCommon.sSiteCode);
                    prn = prn.Replace("{Qty}", dt.Rows[0]["LOT_QTY"].ToString());
                    sFinalPRN = prn;
                }
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw;
            }
            return sFinalPRN;
        }
    }
}
