using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class PCommon
    {
        public static object BAUDRATE;
        public static object DATABITS;
        public static BcilLib.BcilLogger mBcilLogger;
        public static string PORTNAME;

        public static string sSiteCode { get; set; }
        public static string sUseNetworkPrinter { get; set; }
        public static string sPrinterPort { get; set; }
        public static string sClientPrintingPath { get; set; }
        public static string sServerSidePrintingPath { get; set; }
        public static string sFileName { get; set; }
        public static string sFilePrintingPath { get; set; }



        /// <summary>
        /// USED IN SQL DB PROVIEDR
        /// </summary>

        private static string _strsqlcon = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public static string[] myList;

        public static string StrSqlCon
        {
            get { return _strsqlcon; }
            set
            {
                _strsqlcon = value;
            }
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i].Trim().TrimEnd().TrimStart();
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static DataTable ConvertCSVtoDataTable_ToolData(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    if (header == "CALIBRATIONDATE" || header == "ALERTDATE")
                    {
                        dt.Columns.Add(header, typeof(DateTime));
                    }
                    else
                    {
                        dt.Columns.Add(header);
                    }
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        if (i == 10 || i == 11)
                        {
                            try
                            {
                                dr[i] = Convert.ToDateTime(rows[i].Trim().TrimEnd().TrimStart());
                            }
                            catch (Exception)
                            {
                                if (i == 10)
                                {
                                    throw new Exception("File contains invalid value in the column Calibration date,Please check the date ");
                                }
                                else
                                {
                                    throw new Exception("File contains invalid value in the column aleart date column, Please check the date");
                                }
                            }
                        }
                        else
                        {
                            dr[i] = rows[i].Trim().TrimEnd().TrimStart();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static DataSet ConvertCSVtoDataTable_multiColumn(string strFilePath)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dtWifiBT = new DataTable();
            DataSet ds = new DataSet();
            DataTable dtIMEI = new DataTable();
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                if (str != "")
                {
                    string[] rows = str.Split(',');
                    DataRow dr = dtWifiBT.NewRow();
                    DataRow dr1 = dtIMEI.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                        dr1[i] = rows[i];
                    }
                    if (dr.ToString() == "")
                    {
                        continue;
                    }
                    dtIMEI.Rows.Add(dr1);
                }
            }
            dtIMEI.TableName = "IMEIData";
            dtIMEI.AcceptChanges();
            dtWifiBT.TableName = "WIFIData";
            dtWifiBT.AcceptChanges();
            //dtGoogleKey.TableName = "GoogleKey";
            //dtGoogleKey.AcceptChanges();
            dtIMEI.Columns.RemoveAt(1);
            dtIMEI.Columns.RemoveAt(1);
            dtIMEI.AcceptChanges();
            //dtIMEI.Columns.RemoveAt(1);
            //dtIMEI.AcceptChanges();

            dtWifiBT.Columns.RemoveAt(0);
            dtWifiBT.AcceptChanges();
            //dtWifiBT.Columns.RemoveAt(2);
            //dtWifiBT.AcceptChanges();

            //dtGoogleKey.Columns.RemoveAt(0);
            //dtGoogleKey.AcceptChanges();
            //dtGoogleKey.Columns.RemoveAt(0);
            //dtGoogleKey.AcceptChanges();
            //dtGoogleKey.Columns.RemoveAt(0);
            //dtGoogleKey.AcceptChanges();

            ds.DataSetName = "DS";

            ds.Tables.Add(dtIMEI);
            ds.AcceptChanges();

            ds.Tables.Add(dtWifiBT);
            ds.AcceptChanges();
            //ds.Tables.Add(dtGoogleKey);
            //ds.AcceptChanges();
            return ds;
        }

        public static DataTable ConvertArrayDataTableSingleColumn(ArrayList sIMEI)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SERIAL_NUMBER");
            dt.Columns.Add("GRPONSN");
            dt.Columns.Add("MAC");
            dt.Columns.Add("MAC_2");
            dt.Columns.Add("KEY_PART_NO");
            dt.Columns.Add("WIFI");
            dt.Columns.Add("WIRELESS_SSID");
            dt.Columns.Add("PRE_PASSWORD");
            dt.Columns.Add("IMEI_1");
            dt.Columns.Add("ACS_DATA");
            dt.Columns.Add("HDCP");
            dt.Columns.Add("COL1");
            dt.Columns.Add("COL2");
            dt.Columns.Add("COL3");
            dt.Columns.Add("COL4");
            dt.Columns.Add("COL5");
            dt.Columns.Add("COL6");
            dt.Columns.Add("COL7");
            dt.Columns.Add("COL8");
            dt.Columns.Add("COL9");
            for (int f = 0; f < sIMEI.Count; f++)
            {
                dt.Rows.Add(sIMEI[f].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            return dt;
        }
        public static string ToCSV(DataTable dt)
        {
            string csv = string.Empty;
            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + ',';
            }
            //Add new line.
            csv += "\r\n";
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }
                //Add new line.
                csv += "\r\n";
            }
            return csv;
        }

        public static string DataEncrypt(string clearText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return clearText;
        }

    }
}
