using MobiVUE;
using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class MachineFactory
    {
        public void Delete(short machineId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(machineId > 0, "Machine id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Machine";
                    cmd.Parameters.AddWithValue("@Id", machineId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteMachine");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Machine Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Machine id is required for get Machine .");
            Machine Machine = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetMachineById"));
                    cmd.CommandText = "USP_Machine";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            Machine = GetMachine(dr);
                        }
                    }
                }
            }

            return Machine;
        }

        public DataList<Machine, long> FetchMachines(MachineSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<Machine, long> Machines = new DataList<Machine, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Machine";
                    cm.Parameters.Add(new SqlParameter("@MachineId", criteria.MachineCode));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetMachines"));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));
                    cm.Parameters.Add(new SqlParameter("@MachineTypeId", criteria.MachineTypeId));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (Machines.HeaderData == 0) Machines.HeaderData = dr.GetInt64("TotalRows");
                            Machine Machine = GetMachine(dr);
                            Machines.ItemCollection.Add(Machine);
                        }
                    }
                }
            }
            return Machines;
        }

        private Machine GetMachine(SafeDataReader dr)
        {
            Machine Machine = new Machine();
            Machine.Id = dr.GetInt16("ID");
            Machine.MachineCode = dr.GetString("MachineId");
            Machine.Name = dr.GetString("MachineName");
            Machine.Description = dr.GetString("Description");
            Machine.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            Machine.MachineType = new KeyValue<short, string>() { Key = dr.GetInt16("MachineTypeId"), Value = dr.GetString("MachineTypeName") };
            Machine.IsActive = dr.GetBoolean("IsActive");
            Machine.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Machine.CreatedOn = dr.GetDateTime("CreatedOn");
            Machine.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("ModifiedByName") };
            Machine.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return Machine;
        }

        public bool IsExists(Machine entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Machine should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineId", entity.MachineCode);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_Machine";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public Machine Insert(Machine entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Machine should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Machine Type: {entity.MachineCode} already exists");

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Machine";
                    cm.Parameters.AddWithValue("@MachineId", entity.MachineCode);
                    cm.Parameters.AddWithValue("@Name", entity.Name);
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@MachineTypeId", entity.MachineType.Key);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertMachine");
                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public Machine Update(Machine entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Machine should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (IsExists(entity, cn)) throw new Exception($"Machine Type: {entity.MachineCode} already exists");
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;

                    cm.CommandText = "USP_Machine";
                    cm.Parameters.AddWithValue("@Name", entity.Name.IsNotNullOrWhiteSpace() ? entity.Name : "");
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);

                    cm.Parameters.AddWithValue("@Type", "UpdateMachine");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}