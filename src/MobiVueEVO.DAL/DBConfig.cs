using Microsoft.Extensions.Configuration;
using MobiVUE;

namespace MobiVueEVO.DAL
{
    public static class DBConfig
    {
        public static IConfiguration Configuration;

        public static string SQLConnectionString
        {
            get
            {
                //string domain = "";
                //if (Configuration["appSeDomain"].IsNotNullOrWhiteSpace())
                //    domain = Configuration["Domain"].ToString();

                //CodeContract.Required<MobiVUEException>(domain.IsNotNullOrWhiteSpace(), "Domain name is required");

                var sqlString = Configuration["ConnectionStrings:Default"];
                CodeContract.Required<MobiVUEException>(sqlString.IsNotNullOrWhiteSpace(), "Invalid connection string");
                return sqlString;
            }
        }
    }
}