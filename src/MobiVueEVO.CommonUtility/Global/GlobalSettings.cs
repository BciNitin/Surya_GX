using System;
using System.IO;

namespace MobiVueEVO.CommonUtility.Global
{
    public static class GlobalSettings
    {
        #region GX Label Printing
        //public static string abc = DBConfig;
        public static string FilePrefix = "BCI_GXPrinting_";
        //public static string FilePrefix = "SGSTPrinting_";
        public static string PRNZipDownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        //public static string PRNFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PRN");
        public static string PRNFilePath = Path.Combine(Environment.CurrentDirectory, "PRNPATH");
        //public static string PRNFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        //public static string PRNFilePath = KnownFolders.Downloads.Path;


        //string pathss = AppDomain.CurrentDomain.BaseDirectory;
        //string pathssss = System.Reflection.Assembly.GetEntryAssembly().Location;
        //string path = Path.GetDirectoryName(pathssss);
        #endregion
    }
}
