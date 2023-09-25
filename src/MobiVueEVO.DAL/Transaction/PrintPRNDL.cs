using Common;
using MobiVUE;
using MobiVueEVO.BO;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MobiVueEVO.DAL
{
    public class PrintPRNDL
    {
        public void Delete(long id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Fresh article id required for delete.");

            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                Delete(id, deletedBy, cn);
            }
        }

        private void Delete(long id, long deletedBy, SqlConnection cn)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Fresh article id is required for delete.");
            CodeContract.Required<ArgumentException>(cn != null, "Database connection should not be null.");
            using (var cm = cn.CreateCommand())
            {
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = DeleteSQL();
                cm.Parameters.AddWithValue("@ID", id);
                cm.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                cm.ExecuteNonQuery();
            }
        }

        private string DeleteSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE T \n");
            sb.Append("FROM   mLabelsToPrint T \n");
            sb.Append("WHERE  ID = @Id");
            return sb.ToString();
        }

        public LabelToPrint Fetch(long id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Fresh article id is required for delete.");

            LabelToPrint labelToPrint = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                cmd.CommandText = FetchQuery();

                using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                {
                    if (dr.Read())
                    {
                        labelToPrint = GetLabelToPrint(dr);
                    }
                }
            }
            return labelToPrint;
        }

        public LabelToPrint FetchLabelToPrintByPlant(string plant)
        {
            CodeContract.Required<ArgumentException>(plant.IsNotNullOrWhiteSpace(), "Plant is not required to get the data");

            LabelToPrint labelToPrint = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Type", "GETDATATOPRINT"));
                cmd.Parameters.Add(new SqlParameter("@Plant", plant));
                cmd.CommandText = "USP_PRINT";

                using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                {
                    if (dr.Read())
                    {
                        labelToPrint = GetLabelToPrint(dr);
                    }
                }
            }
            return labelToPrint;
        }

        private LabelToPrint GetLabelToPrint(SafeDataReader dr)
        {
            var labelToPrint = new LabelToPrint();
            labelToPrint.ID = dr.GetInt64("ID");
            labelToPrint.Labels = dr.GetString("Labels");
            labelToPrint.Qty = dr.GetInt32("Qty");
            labelToPrint.PrinterName = dr.GetString("PrinterName");
            labelToPrint.PrinterPort = dr.GetInt32("Port");
            labelToPrint.PrinterType = (PrinterType)dr.GetInt32("PrinterType");
            labelToPrint.MarkAsOld();
            return labelToPrint;
        }

        private string FetchQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Select * from mLabelsToPrint \n");
            sb.Append(" WHERE ID=@Id \n");
            return sb.ToString();
        }

        public List<LabelToPrint> FetchAll()
        {
            throw new NotImplementedException();
        }

        public LabelToPrint Insert(LabelToPrint entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Picking should not be null.");
            entity.Validate();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Insert Into mLabelsToPrint (Labels,PlantId,Qty,PrinterName,Port,PrinterType,CreatedBy,CreatedOn) Values (@Labels, @SiteId, @Qty, @PrinterName, @Port, @PrinterType, @CreatedBy, @CreatedOn)";
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Labels", entity.Labels);
                    cmd.Parameters.AddWithValue("@SiteId", entity.Site.Key);
                    cmd.Parameters.AddWithValue("@Qty", entity.Qty);
                    cmd.Parameters.AddWithValue("@PrinterName", entity.PrinterName);
                    cmd.Parameters.AddWithValue("@Port", entity.PrinterPort);
                    cmd.Parameters.AddWithValue("@PrinterType", entity.PrinterType);
                    cmd.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy.Key);
                    cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT @@IDENTITY AS 'Identity';";
                    entity.ID = Convert.ToInt64(cmd.ExecuteScalar());
                    entity.MarkAsOld();
                }
            }
            return entity;
        }

        public LabelToPrint Update(LabelToPrint entity)
        {
            CodeContract.Required<ArgumentException>(entity.IsNotNull(), "Label should not be null");
            entity.Validate();
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "USP_PRINT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", entity.ID);
                    cmd.Parameters.AddWithValue("@Type", "UPDATE");

                    cmd.ExecuteNonQuery();
                }
            }
            return entity;
        }

       
    }
}