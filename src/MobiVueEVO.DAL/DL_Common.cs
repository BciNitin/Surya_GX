using Common;
using MobiVueEVO.BO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MobiVueEVO.DAL
{
    public class DL_Common
    {
        DBManager oDbm;
        public DL_Common()
        {
            //DCommon c = new DCommon();
            oDbm = DCommon.SqlDBProvider();
        }
        public DataTable dtGetPRN(StringBuilder sb)
        {
            DataTable dtPRN = new DataTable();
            try
            {
                oDbm.Open();
                dtPRN = oDbm.ExecuteDataSet(System.Data.CommandType.Text, sb.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
            }
            return dtPRN;
        }
        public DataTable dtBindData(StringBuilder sb)
        {
            DataTable dtBindPrinter = new DataTable();
            try
            {
                oDbm.Open();
                dtBindPrinter = oDbm.ExecuteDataSet(System.Data.CommandType.Text, sb.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                //PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
            }
            return dtBindPrinter;
        }
        public string GenerateSN(string sType)
        {
            string sSerialNo = string.Empty;
            DataTable dt = new DataTable();
            string sResult = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(oDbm.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GETSERIAL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SITECODE", PCommon.sSiteCode);
                        cmd.Parameters.AddWithValue("@PARAM", sType);
                        cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 30);
                        cmd.Parameters["@RESULT"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        sSerialNo = cmd.Parameters["@RESULT"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message + ", Query " + "USP_GETLASERSERIAL");
                throw ex;
            }
            finally
            {
                //oDbm.Close();
                //oDbm.Dispose();
            }
            return sSerialNo;
        }
        public DataTable dtBindModel()
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "BINDMODEL");
                oDbm.AddParameters(1, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA").Tables[0];
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }
        public DataTable dtBindReasonReprint()
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(1);
                oDbm.AddParameters(0, "@TYPE", "BindReasonReprint");
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_ReasonReprint_MASTER").Tables[0];
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }
        public DataSet dtGetModelDetails(PL_Printing plobj)
        {
            DataSet dt = new DataSet();
            try
            {
                oDbm.CreateParameters(3);
                oDbm.AddParameters(0, "@TYPE", "GETMODELDETAILS");
                oDbm.AddParameters(1, "@MODELCODE", plobj.sModelName);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                //PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }
        public DataSet dtGetProductModelDetails(PL_Printing plobj)
        {
            DataSet dt = new DataSet();
            try
            {
                oDbm.CreateParameters(3);
                oDbm.AddParameters(0, "@TYPE", "GETProductMODELDETAILS");
                oDbm.AddParameters(1, "@MODELCODE", plobj.sModelName);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                //PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }
        public DataSet dtGetdataforinnpoiaModelDetails(PL_Printing plobj)
        {
            DataSet dt = new DataSet();
            try
            {
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@TYPE", "dtGetdataforinnpoiaModelDetails");
                oDbm.AddParameters(1, "@MODELCODE", plobj.sModelName);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.AddParameters(3, "@sSNBarcode", plobj.sSNBarcode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }
        public DataSet dtGetBOXdataforinnpoiaModelDetails(PL_Printing plobj)
        {
            DataSet dt = new DataSet();
            try
            {
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@TYPE", "dtGetdataforinnpoiaModelDetails");
                oDbm.AddParameters(1, "@MODELCODE", plobj.sModelName);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.AddParameters(3, "@sSNBarcode", plobj.RsnGetInnopia);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }
        public DataSet dtGetdataforReprintinnpoiaModelDetails(PL_Printing plobj)
        {
            DataSet dt = new DataSet();
            try
            {
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@TYPE", "dtGetdataforinnpoiaModelDetails");
                oDbm.AddParameters(1, "@MODELCODE", plobj.sModelName);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.AddParameters(3, "@sSNBarcode", plobj.sBarcodestring);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                //PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }

        public DataSet dlGetLabelPrintingDetails(PL_Printing plobj)
        {
            DataSet ds = new DataSet();
            try
            {
                oDbm.CreateParameters(3);
                oDbm.AddParameters(0, "@TYPE", "GETLABELPRINTINGDETAILS");
                oDbm.AddParameters(1, "@BARCODE", plobj.sBarcodestring);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                ds = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return ds;
        }
        public DataSet dlGetFGPackingDetailForPrn(PL_Printing plobj)
        {
            DataSet ds = new DataSet();
            try
            {
                oDbm.CreateParameters(3);
                oDbm.AddParameters(0, "@TYPE", "GETFGPRININGDETAILS");
                oDbm.AddParameters(1, "@BOXID", plobj.sBoxId);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                ds = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return ds;
        }
        public DataSet dtGetPalletPrintingData(PL_Printing plobj)
        {
            DataSet dt = new DataSet();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@MODELCODE", plobj.sModelName);
                oDbm.AddParameters(1, "@SITECODE", PCommon.sSiteCode);
                oDbm.AddParameters(2, "@TYPE", "GETPALLETPRINTINGDATA");
                oDbm.AddParameters(3, "@BOXID", plobj.sPalletID);
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_BIND_PRINTINGDATA");
            }
            catch (Exception ex)
            {
                //PCommon.mBcilLogger.LogMessage(EventNotice.EventTypes.evtError, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dt;
        }
    }
}
