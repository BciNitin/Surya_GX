using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL.Masters
{
    public class SpoolFactory
    {
        public void Delete(int Id, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(Id > 0, "Spool Id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SpoolMaster";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Mode", "DELETE");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Spool Fetch(int id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Spool id is required for get Spool Master .");
            Spool SpoolMaster = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Mode", "SearchById"));
                    cmd.CommandText = "USP_SpoolMaster";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            SpoolMaster = GetSpoolMaster(dr);
                        }
                    }
                }
            }

            return SpoolMaster;
        }

        public DataList<Spool, long> FetchAll(SpoolMasterSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<Spool, long> SpoolMasters = new DataList<Spool, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolMaster";
                    cm.Parameters.Add(new SqlParameter("@SpoolCode", criteria.SpoolCode.IsNotNullOrWhiteSpace() ? criteria.SpoolCode : ""));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.AddWithValue("@Page", criteria.PageNumber);
                    cm.Parameters.AddWithValue("@RFIDTagID", criteria.RFIDTagID);
                    cm.Parameters.AddWithValue("@BLEMACAddress", criteria.BLEMACAddress);
                    cm.Parameters.AddWithValue("@SpoolType", criteria.SpoolType);
                    cm.Parameters.AddWithValue("@PageSize", criteria.PageSize);
                    cm.Parameters.AddWithValue("@PlantId", criteria.SiteId);
                    cm.Parameters.Add(new SqlParameter("@Mode", "SearchAll"));  //have to change in SP
                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (SpoolMasters.HeaderData == 0) SpoolMasters.HeaderData = dr.GetInt64("TotalRows");
                            Spool SpoolMaster = GetSpoolMaster(dr);
                            SpoolMasters.ItemCollection.Add(SpoolMaster);
                        }
                    }
                }
            }
            return SpoolMasters;
        }
        public DataList<Spool, long> FetchAllForBulkCheck(SpoolMasterSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<Spool, long> SpoolMasters = new DataList<Spool, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolMaster";
                    cm.Parameters.Add(new SqlParameter("@SpoolCode", criteria.SpoolCode.IsNotNullOrWhiteSpace() ? criteria.SpoolCode : ""));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.AddWithValue("@Page", criteria.PageNumber);
                    cm.Parameters.AddWithValue("@RFIDTagID", criteria.RFIDTagID);
                    cm.Parameters.AddWithValue("@BLEMACAddress", criteria.BLEMACAddress);
                    cm.Parameters.AddWithValue("@SpoolType", criteria.SpoolType);
                    cm.Parameters.AddWithValue("@PageSize", criteria.PageSize);
                    cm.Parameters.AddWithValue("@PlantId", criteria.SiteId);
                    cm.Parameters.Add(new SqlParameter("@Mode", "SearchAllBulk"));  //have to change in SP
                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (SpoolMasters.HeaderData == 0) SpoolMasters.HeaderData = dr.GetInt64("TotalRows");
                            Spool SpoolMaster = GetSpoolMaster(dr);
                            SpoolMasters.ItemCollection.Add(SpoolMaster);
                        }
                    }
                }
            }
            return SpoolMasters;
        }

        private Spool GetSpoolMaster(SafeDataReader dr)
        {
            Spool SpoolMaster = new Spool();
            SpoolMaster.Id = dr.GetInt32("ID");
            SpoolMaster.SpoolCode = dr.GetString("SpoolCode");
            SpoolMaster.Description = dr.GetString("Description");
            SpoolMaster.SpoolType = new KeyValue<short, string>() { Key = dr.GetInt16("SpoolType"), Value = dr.GetString("Spool_Code") };
            SpoolMaster.Size = dr.GetInt32("Size");
            SpoolMaster.RFIDTagID = dr.GetString("RFIDTagID");
            SpoolMaster.BLEMACAddress = dr.GetString("BLEMACAddress");
            SpoolMaster.Plant = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            SpoolMaster.IsActive = dr.GetBoolean("IsActive");
            SpoolMaster.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            SpoolMaster.CreatedOn = dr.GetDateTime("CreatedOn");
            SpoolMaster.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            SpoolMaster.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return SpoolMaster;
        }

        public bool IsExists(Spool entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Spool Id should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpoolCode", entity.SpoolCode);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Mode", "IsExists");   ///have to updaste SP for IsExists
                cmd.CommandText = "USP_SpoolMaster";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public Spool Insert(Spool entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Spool Master should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Spool: {entity.SpoolCode} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolMaster";
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@SpoolCode", entity.SpoolCode);
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@SpoolType", entity.SpoolType.Key);
                    cm.Parameters.AddWithValue("@Size", entity.Size);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@RFIDTagID", entity.RFIDTagID);
                    cm.Parameters.AddWithValue("@PlantId", entity.Plant.IsNotNull() ? entity.Plant.Key : 0);
                    cm.Parameters.AddWithValue("@BLEMACAddress", entity.BLEMACAddress);
                    cm.Parameters.AddWithValue("@Mode", "Insert");
                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
        public string BulkUploadLogicGeneric(List<GenericData> bulk, string tablename, string[] columns)
        {
            try
            {
                using (var sqlCopy = new SqlBulkCopy(DBConfig.SQLConnectionString))
                {

                    sqlCopy.DestinationTableName = "[duplicate_" + tablename + "]";
                    sqlCopy.BatchSize = 500;

                    //        var copyParameters = new[]
                    //{
                    //            nameof(Spool.SpoolCode),
                    //            //nameof(Spool.SpoolType.Key),
                    //            nameof(Spool.IsActive),
                    //            nameof(Spool.RFIDTagID),
                    //            nameof(Spool.Description)

                    //        };

                    //using (var reader = ObjectReader.Create(bulk, columns))
                    //{
                    //    sqlCopy.WriteToServer(reader);
                    //}
                }

                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }
        public string BulkUploadLogic(List<Spool> bulk)
        {
            try
            {
                using (var sqlCopy = new SqlBulkCopy(DBConfig.SQLConnectionString))
                {
                    sqlCopy.DestinationTableName = "[duplicate_SpoolMaster]";
                    sqlCopy.BatchSize = 500;

                    var copyParameters = new[]
            {
                        nameof(Spool.SpoolCode),
                        //nameof(Spool.SpoolType.Key),
                        nameof(Spool.IsActive),
                        nameof(Spool.RFIDTagID),
                        nameof(Spool.Description)

                    };

                    //using (var reader = ObjectReader.Create(bulk, copyParameters))
                    //{
                    //    sqlCopy.WriteToServer(reader);
                    //}
                }

                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }
        public Spool Update(Spool entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Spool should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_SpoolMaster";
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Isactive", entity.IsActive);
                    cm.Parameters.AddWithValue("@SpoolType", entity.SpoolType.Key);
                    cm.Parameters.AddWithValue("@Size", entity.Size);
                    cm.Parameters.AddWithValue("@Description", entity.Description);
                    cm.Parameters.AddWithValue("@RFIDTagID", entity.RFIDTagID);

                    cm.Parameters.AddWithValue("@BLEMACAddress", entity.BLEMACAddress);
                    cm.Parameters.AddWithValue("@Mode", "Update");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}