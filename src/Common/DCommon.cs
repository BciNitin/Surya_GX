using System;

namespace Common
{
    public static class DCommon
    {
        // PCommon oPCommon = new PCommon();
        public static DBManager SqlDBProvider()
        {
            try
            {
                DBManager oManager = new DBManager(DataProvider.SqlServer, PCommon.StrSqlCon);
                return oManager;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
