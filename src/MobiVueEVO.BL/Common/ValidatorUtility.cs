using System.Data;

namespace MobiVueEVO.BL.Common
{
    public static class ValidatorUtility
    {
        public static bool IsDtValid(DataTable dataTable)
        {
            // Check if DataTable is null or empty
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return false;
            }

            // Check if DataTable contains any blank rows
            foreach (DataRow row in dataTable.Rows)
            {
                bool isRowBlank = true;

                foreach (var item in row.ItemArray)
                {
                    if (!string.IsNullOrWhiteSpace(item.ToString()))
                    {
                        isRowBlank = false;
                        break;
                    }
                }

                if (isRowBlank)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
