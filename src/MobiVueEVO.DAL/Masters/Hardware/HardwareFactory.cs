using MobiVUE;

using MobiVUE.Utility;
using MobiVueEVO.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobiVueEVO.DAL
{
    public class HardwareFactory
    {
        public void Delete(short HardwareId, long deletedBy)
        {
            CodeContract.Required<ArgumentException>(HardwareId > 0, "Hardware id is mandatory.");
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Hardware";
                    cmd.Parameters.AddWithValue("@Id", HardwareId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", deletedBy);
                    cmd.Parameters.AddWithValue("@Type", "DeleteHardware");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Hardware Fetch(short id)
        {
            CodeContract.Required<ArgumentException>(id > 0, "Hardware id is required for get Hardware .");
            Hardware Hardware = null;
            using (SqlConnection con = new SqlConnection(DBConfig.SQLConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@Type", "GetHardwareById"));
                    cmd.CommandText = "USP_Hardware";

                    using (SafeDataReader dr = new SafeDataReader(cmd.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            Hardware = GetHardware(dr);
                        }
                    }
                }
            }

            return Hardware;
        }

        public DataList<Hardware, long> FetchHardwares(HardwareSearchCriteria criteria)
        {
            CodeContract.Required<ArgumentException>(criteria != null, "Search criteria cannot be null");
            DataList<Hardware, long> Hardwares = new DataList<Hardware, long>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Hardware";
                    cm.Parameters.Add(new SqlParameter("@HardwareName", criteria.HardwareName));
                    cm.Parameters.Add(new SqlParameter("@IsActive", criteria.Status));
                    cm.Parameters.Add(new SqlParameter("@PlantId", criteria.SiteId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetHardwares"));
                    cm.Parameters.Add(new SqlParameter("@PrintingModule", criteria.PrintingModule));
                    cm.Parameters.Add(new SqlParameter("@PrintModule", criteria.PrintModule.IsNotNullOrWhiteSpace() ? criteria.PrintModule : ""));
                    cm.Parameters.Add(new SqlParameter("@page", criteria.PageNumber));
                    cm.Parameters.Add(new SqlParameter("@HardwareTypeId", criteria.HardwareTypeId));
                    cm.Parameters.Add(new SqlParameter("@pageSize", criteria.PageSize));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            if (Hardwares.HeaderData == 0) Hardwares.HeaderData = dr.GetInt64("TotalRows");
                            Hardware Hardware = GetHardware(dr);
                            Hardwares.ItemCollection.Add(Hardware);
                        }
                    }
                }
            }
            return Hardwares;
        }

        public List<KeyValue<int, string>> FetchPrintingModules(int plantId)
        {
            List<KeyValue<int, string>> printingModules = new List<KeyValue<int, string>>();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Hardware";
                    cm.Parameters.Add(new SqlParameter("@PlantId", plantId));
                    cm.Parameters.Add(new SqlParameter("@Type", "GetPrintingModules"));

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            printingModules.Add(new KeyValue<int, string>() { Key = dr.GetInt32("Id"), Value = dr.GetString("PrintModule") });
                        }
                    }
                }
            }
            return printingModules;
        }

        private Hardware GetHardware(SafeDataReader dr)
        {
            Hardware Hardware = new Hardware();
            Hardware.Id = dr.GetInt16("ID");
            Hardware.HardwareName = dr.GetString("HardwareName");
            Hardware.HardwareIP = dr.GetString("HardwareIP");
            Hardware.Description = dr.GetString("Description");
            Hardware.PrinterType = (PrinterType)dr.GetInt32("PrinterType");
            Hardware.PrintingModule = new KeyValue<int, string>() { Key = dr.GetInt32("PrintingModule"), Value = dr.GetString("PrintModule") };
            Hardware.HardwareType = new KeyValue<short, string>() { Key = dr.GetInt16("HardwareTypeId"), Value = dr.GetString("HardwareTypeName") };
            Hardware.Site = new KeyValue<int, string>() { Key = dr.GetInt32("PlantId"), Value = dr.GetString("PlantCode") };
            Hardware.IsActive = dr.GetBoolean("IsActive");
            Hardware.Port = dr.GetInt32("HardwarePort");
            Hardware.CreatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("CreatedBy"), Value = dr.GetString("CreatedByName") };
            Hardware.CreatedOn = dr.GetDateTime("CreatedOn");
            Hardware.UpdatedBy = new KeyValue<long, string>() { Key = dr.GetInt64("ModifiedBy"), Value = dr.GetString("UpdatedByName") };
            Hardware.UpdatedOn = dr.GetDateTime("ModifiedOn");
            return Hardware;
        }

        public bool IsLANPrinterExists(Hardware entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Hardware should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HardwareIP", entity.HardwareIP);
                cmd.Parameters.AddWithValue("@HardwarePort", entity.Port);
                cmd.Parameters.AddWithValue("@PrintingModule", entity.PrintingModule.IsNotNull() ? entity.PrintingModule.Key : 0);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsLANPrinterExists");
                cmd.CommandText = "USP_Hardware";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool IsUSBPrinterExists(Hardware entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Hardware should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HardwareName", entity.HardwareName);
                cmd.Parameters.AddWithValue("@PrintingModule", entity.PrintingModule.IsNotNull() ? entity.PrintingModule.Key : 0);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsUSBPrinterExists");
                cmd.CommandText = "USP_Hardware";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool IsExists(Hardware entity, SqlConnection con)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Hardware should not be null");
            entity.Validate();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HardwareName", entity.HardwareName);
                cmd.Parameters.AddWithValue("@HardwareIp", entity.HardwareIP);
                cmd.Parameters.AddWithValue("@HardwarePort", entity.Port);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Type", "IsExists");
                cmd.CommandText = "USP_Hardware";

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public Hardware Insert(Hardware entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Hardware should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();

                if (entity.HardwareType.Value.InvariantEquals("Printer") && entity.PrinterType == PrinterType.LAN)
                {
                    if (IsLANPrinterExists(entity, cn)) throw new Exception($"Hardware: {entity.HardwareIP} already exists against module: {entity.PrintingModule.Value}");
                }
                else if (entity.HardwareType.Value.InvariantEquals("Printer") && entity.PrinterType == PrinterType.USB)
                {
                    if (IsUSBPrinterExists(entity, cn)) throw new Exception($"Hardware: {entity.HardwareName} already exists");
                }
                else
                {
                    if (IsExists(entity, cn)) throw new Exception($"Hardware: {entity.HardwareName} already exists");
                }

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Hardware";

                    cm.Parameters.AddWithValue("@HardwareName", entity.HardwareName.IsNotNullOrWhiteSpace() ? entity.HardwareName : "");
                    cm.Parameters.AddWithValue("@HardwareIP", entity.HardwareIP.IsNotNullOrWhiteSpace() ? entity.HardwareIP : "");
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PrinterType", entity.PrinterType);

                    cm.Parameters.AddWithValue("@PrintingModule", entity.PrintingModule.IsNotNull() ? entity.PrintingModule.Key : 0);
                    cm.Parameters.AddWithValue("@HardwarePort", entity.Port);
                    cm.Parameters.AddWithValue("@PlantId", entity.Site.IsNotNull() ? entity.Site.Key : 0);
                    cm.Parameters.AddWithValue("@HardwareTypeId", entity.HardwareType.IsNotNull() ? entity.HardwareType.Key : 0);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);
                    cm.Parameters.AddWithValue("@Type", "InsertHardware");
                    entity.Id = Convert.ToInt16(cm.ExecuteScalar());
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }

        public Hardware Update(Hardware entity)
        {
            CodeContract.Required<ArgumentException>(entity != null, "Conveyor Line should not be null");
            entity.Validate();
            using (SqlConnection cn = new SqlConnection(DBConfig.SQLConnectionString))
            {
                cn.Open();
                if (entity.HardwareType.Value.InvariantEquals("Printer") && entity.PrinterType == PrinterType.LAN)
                {
                    if (IsLANPrinterExists(entity, cn)) throw new Exception($"Hardware: {entity.HardwareIP} already exists against module: {entity.PrintingModule.Value}");
                }
                else if (entity.HardwareType.Value.InvariantEquals("Printer") && entity.PrinterType == PrinterType.USB)
                {
                    if (IsUSBPrinterExists(entity, cn)) throw new Exception($"Hardware: {entity.HardwareName} already exists");
                }
                else
                {
                    if (IsExists(entity, cn)) throw new Exception($"Hardware: {entity.HardwareName} already exists");
                }
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "USP_Hardware";
                    cm.Parameters.AddWithValue("@HardwareName", entity.HardwareName.IsNotNullOrWhiteSpace() ? entity.HardwareName : "");
                    cm.Parameters.AddWithValue("@HardwareIP", entity.HardwareIP.IsNotNullOrWhiteSpace() ? entity.HardwareIP : "");
                    cm.Parameters.AddWithValue("@Description", entity.Description.IsNotNullOrWhiteSpace() ? entity.Description : "");
                    cm.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cm.Parameters.AddWithValue("@PrintingModule", entity.PrintingModule.IsNotNull() ? entity.PrintingModule.Key : 0);
                    cm.Parameters.AddWithValue("@HardwarePort", entity.Port);
                    cm.Parameters.AddWithValue("@HardwareTypeId", entity.HardwareType.IsNotNull() ? entity.HardwareType.Key : 0);
                    cm.Parameters.AddWithValue("@ModifiedBy", entity.UpdatedBy.Key);

                    cm.Parameters.AddWithValue("@Type", "UpdateHardware");
                    cm.Parameters.AddWithValue("@Id", entity.Id);
                    cm.ExecuteNonQuery();
                    entity.MarkAsOld();
                    return entity;
                }
            }
        }
    }
}