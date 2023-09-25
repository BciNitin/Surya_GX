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
    }
}
