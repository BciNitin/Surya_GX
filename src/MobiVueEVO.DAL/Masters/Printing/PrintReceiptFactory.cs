using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class PrintReceiptFactory
    {
        public void Delete(int prnId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(prnId > 0, "Prn id is required for get printing receipt.");
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_PrintReceipt";
                    cm.Parameters.AddWithValue("@PrnId", prnId);
                    cm.Parameters.AddWithValue("@Type", "Delete");
                    cm.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cm.ExecuteNonQuery();
                }
            }
        }

        public PrintReceipt Fetch(PrintReceiptSearchCriteria criteria)
        {
            PrintReceipt printReceipt = null;
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_PrintReceipt";
                    cm.Parameters.AddWithValue("@ReceiptType", criteria.ReceiptType);
                    cm.Parameters.AddWithValue("@PlantId", criteria.PlantId);
                    cm.Parameters.AddWithValue("@Type", "GetPrintReceipts");

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            printReceipt = GetprintReceipt(dr);
                        }
                    }
                }
            }
            return printReceipt;
        }

        public PrintReceipt Fetch(int prnId)
        {
            CodeContract.Required<ArgumentException>(prnId > 0, "Prn id is required for get printing receipt.");
            PrintReceipt printReceipt = null;
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_PrintReceipt";
                    cm.Parameters.AddWithValue("@PrnId", prnId);
                    cm.Parameters.AddWithValue("@Type", "GetPrintReceiptById");

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            printReceipt = GetprintReceipt(dr);
                        }
                    }
                }
            }
            return printReceipt;
        }

        public PrintReceipt Insert(PrintReceipt entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Print receipt should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_PrintReceipt";
                    cm.Parameters.Add(new SqlParameter("@PlantId", entity.Site.Key));
                    cm.Parameters.Add(new SqlParameter("@ReceiptType", entity.ReceiptType));
                    cm.Parameters.Add(new SqlParameter("@Prn", entity.Prn));
                    cm.Parameters.Add(new SqlParameter("@Type", "Insert"));

                    entity.PrnId = Convert.ToInt32(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public PrintReceipt Update(PrintReceipt entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Print receipt should not be null.");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_PrintReceipt";

                    cm.Parameters.Add(new SqlParameter("@PrnId", entity.PrnId));
                    cm.Parameters.Add(new SqlParameter("@PlantId", entity.Site.Key));
                    cm.Parameters.Add(new SqlParameter("@ReceiptType", entity.ReceiptType));
                    cm.Parameters.Add(new SqlParameter("@Prn", entity.Prn));
                    cm.Parameters.Add(new SqlParameter("@Type", "Update"));

                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        private static PrintReceipt GetprintReceipt(SafeDataReader dr)
        {
            PrintReceipt printReceipt = new PrintReceipt();
            printReceipt.PrnId = dr.GetInt32("PrnId");
            printReceipt.ReceiptType = (PrintReceiptType)dr.GetInt32("ReceiptType");
            printReceipt.Prn = dr.GetString("Prn");
            printReceipt.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            printReceipt.MarkAsOld();
            return printReceipt;
        }
    }
}