using System;
using System.Text;

namespace MobiVUE.Utility
{
    public static class Data
    {
        public static string GetPagingSQL(string query, int pageNumber, int pageSize, string orderByClouse)
        {
            query = query.Trim();
            if (query.StartsWith("Select", StringComparison.OrdinalIgnoreCase))
                query = query.Substring(6);

            StringBuilder sb = new StringBuilder();
            sb.Append("WITH RowConstrainedResult \n");
            sb.AppendFormat("     AS (SELECT Row_number() OVER( ORDER BY {0}) as RowNum, \n", orderByClouse);
            sb.Append("	  \n");
            sb.AppendFormat("  {0}	\n", query);
            sb.Append(" ) \n");
            sb.Append("SELECT T.*,(SELECT MAX(RowNum) FROM RowConstrainedResult) AS TotalRows \n");
            sb.Append("FROM   RowConstrainedResult T \n");
            sb.AppendFormat("WHERE  T.RowNum > ( {0} - 1 ) *{1} AND T.RowNum <= ( {0} - 1 ) *{1} + {1} \n", pageNumber, pageSize);
            sb.Append("ORDER  BY T.RowNum");
            return sb.ToString();
        }
    }
}