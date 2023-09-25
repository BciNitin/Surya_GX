// using ICSharpCode.SharpZipLib.GZip;
using Ionic.Zip;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobiVueEVO.CommonUtility.Global;
using Nancy;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MobiVueEVO.BL.Common
{
    public static class LabelPrinting
    {
        #region Master Printing Logic
        public static string SubstitutePRNVariables(string sPRN, DataTable dtPRNDetail)
        {
            try
            {
                if (dtPRNDetail.Rows.Count > 0)
                {
                    DataRow row = dtPRNDetail.Rows[0]; // Consider only the first row

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dtPRNDetail.Columns)
                    {
                        string sPRNVariable = column.ColumnName.ToString().Trim() ?? string.Empty;
                        string sPRNValue = row[column].ToString().Trim() ?? string.Empty;

                        bool isVariableMatch = sPRN.IndexOf($"{{{sPRNVariable}}}", StringComparison.OrdinalIgnoreCase) >= 0;

                        if (isVariableMatch)
                        {
                            sPRN = Regex.Replace(sPRN, $"{{{sPRNVariable}}}", sPRNValue, RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sPRN;
        }

        public static string SubstitutePRNVariables(string sPRN, List<DataColumn> dtPRNColumns, DataRow drPRNDetail)
        {
            try
            {
                if (drPRNDetail != null)
                {
                    DataRow row = drPRNDetail;

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dtPRNColumns)
                    {
                        string sPRNVariable = column.ColumnName.ToString().Trim() ?? string.Empty;
                        string sPRNValue = row[column].ToString().Trim() ?? string.Empty;

                        bool isVariableMatch = sPRN.IndexOf($"{{{sPRNVariable}}}", StringComparison.OrdinalIgnoreCase) >= 0;

                        if (isVariableMatch)
                        {
                            sPRN = Regex.Replace(sPRN, $"{{{sPRNVariable}}}", sPRNValue, RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sPRN;
        }

        public static async Task<bool> SavePRNContentToFileAsync(string sPRN)
        {
            try
            {
                string sPRNSavePath = GlobalSettings.PRNFilePath;

                if (string.IsNullOrWhiteSpace(sPRN))
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(sPRNSavePath))
                {
                    return false;
                }

                EnsureDirectoryExists(sPRNSavePath);

                string fileName = GenerateUniqueFileName(".prn");
                string filePath = Path.Combine(sPRNSavePath, fileName);
                bool isFileSaved = false;

                try
                {
                    using StreamWriter writer = new StreamWriter(filePath, true);
                    await writer.WriteAsync(sPRN);
                    writer.Close();
                    writer.Dispose();
                    isFileSaved = true;
                }
                catch (Exception ex)
                {
                    isFileSaved = false;
                    Console.WriteLine(ex.Message);
                }

                return isFileSaved;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task SavePRNContentToFileAsync(string sPRN, int noOfPrints)
        {
            try
            {
                string sPRNSavePath = GlobalSettings.PRNFilePath;

                if (string.IsNullOrWhiteSpace(sPRN))
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(sPRNSavePath))
                {
                    return;
                }

                EnsureDirectoryExists(sPRNSavePath);

                for (int i = 0; i < noOfPrints; i++)
                {

                    string fileName = GenerateUniqueFileName(".prn");
                    string filePath = Path.Combine(sPRNSavePath, fileName);

                    try
                    {
                        using StreamWriter writer = new StreamWriter(filePath, true);
                        await writer.WriteAsync(sPRN);
                        writer.Close();
                        writer.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task<bool> ZipAndDownloadPRNFilesAsync()
        {
            try
            {
                bool isFileSaved = false;
                string downloadsFolder = GlobalSettings.PRNZipDownloadPath;
                string sPRNFilePath = GlobalSettings.PRNFilePath;
                EnsureDirectoryExists(downloadsFolder);

                List<string> sPRNFiles = GetFiles(sPRNFilePath);

                if (!sPRNFiles.Any())
                {
                    return false;
                }

                //Set Zip file name  
                string fileName = GenerateUniqueFileName(".zip");
                string sDownloadZipPath = Path.Combine(downloadsFolder, fileName);
                //Zip all PRN files and download
                
                using (ZipFile zipFile = new ZipFile())
                {
                    foreach (string item in sPRNFiles)
                    {
                        zipFile.AddFile(item, string.Empty);
                    }
                    zipFile.CompressionMethod = CompressionMethod.BZip2;
                    zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    //Download the zip file to specified path
                    
                    try
                    {
                        await Task.Run(() => zipFile.Save(sDownloadZipPath)).ConfigureAwait(false);
                        //string file = System.IO.Path.Combine(sDownloadZipPath, fileName);
                        
                        isFileSaved = true;
                         
                    }
                    catch (Exception ex)
                    {
                        isFileSaved = false;
                        Console.WriteLine("Failed to save the zip file: " + ex.Message);
                    }
                }
                

                // var stream = new FileStream(fileName, FileMode.Open);
                //return File(stream, "application/zip", "my.zip");
                // Read the zip file contents
                byte[] zipBytes = System.IO.File.ReadAllBytes(sDownloadZipPath);
                var zipHeader = new FileStreamResult(new MemoryStream(zipBytes), "application/zip")
                {
                    FileDownloadName = fileName
                };
                //Response.AppendHeader("content-disposition", "attachment; filename=Report.zip");
                //Delete all the PRN files from source path
                await DeleteFilesAsync(sPRNFiles);
                // Return the zip file as a downloadable attachment

                  return isFileSaved;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (IOException ex)
                {
                    // Handle IO exception
                    throw new IOException("Failed to create directory.", ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // Handle unauthorized access exception
                    throw new UnauthorizedAccessException("Failed to create directory due to insufficient permissions.", ex);
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw new Exception("Failed to create directory.", ex);
                }
            }
        }

        private static List<string> GetFiles(string sPRNFilePath)
        {
            try
            {
                string searchKeyword = GlobalSettings.FilePrefix;
                List<string> sPRNFiles = Directory.GetFiles(sPRNFilePath).Where(file =>
                    Regex.IsMatch(file, searchKeyword, RegexOptions.IgnoreCase)).ToList();
                return sPRNFiles;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static async Task DeleteFilesAsync(List<string> sPRNFiles)
        {
            try
            {
                if (sPRNFiles.Any())
                {
                    foreach (string file in sPRNFiles)
                    {
                        if (File.Exists(file))
                        {
                            await Task.Run(() => File.Delete(file));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GenerateUniqueFileName(string fileExtension)
        {
            string filePrefix = GlobalSettings.FilePrefix;
            string timeStamp = DateTime.Now.ToString("dd-MM-yyyy_HHmmss");
            string microSeconds = DateTime.Now.ToString("ffffff");
            string fileName = $"{filePrefix}{timeStamp}_{microSeconds}{fileExtension}";
            return fileName;
        }

        public static DataTable GenerateDataTableFromArray(string[] array)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(string));

                foreach (string item in array)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        dataTable.Rows.Add(item.Trim());
                    }
                }
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SendPrintCommandToPrinter()
        {

        }
        #endregion
    }
}
