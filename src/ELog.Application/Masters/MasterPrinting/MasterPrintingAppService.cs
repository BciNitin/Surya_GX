using Abp.Application.Services;
using Microsoft.Extensions.Configuration;
using MobiVUE;
using MobiVueEVO.BL.Common;
using MobiVueEVO.BL.Transaction;
using MobiVueEVO.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.Masters.MasterPrinting
{
    public class MasterPrintingAppService : ApplicationService, IMasterPrinterAppService
    {
        private readonly MasterPrintLabelBL blobj;

        public MasterPrintingAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new MasterPrintLabelBL();
        }

        public async Task<string> PrintLabel(string labelType)
        {
            try
            {
                labelType = labelType.Trim();
                if (string.IsNullOrWhiteSpace(labelType))
                {
                    return $"ERROR~Invalid Label Type : {labelType}";
                }

                DataTable dt = await blobj.GetPRN(labelType);

                if (dt.Rows.Count == 0 || dt is null)
                {
                    return $"ERROR~PRN not found, for label type : {labelType}";
                }

                string sPRN = dt.Rows[0][0].ToString();

                if (string.IsNullOrWhiteSpace(sPRN))
                {
                    return $"ERROR~PRN does not contain any data for label type : {labelType}";
                }

                var isFileSaved = await LabelPrinting.SavePRNContentToFileAsync(sPRN);
                if (isFileSaved)
                {
                    await LabelPrinting.ZipAndDownloadPRNFilesAsync();
                }
                else
                {
                    return "ERROR~Label Printing failed.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "SUCCESS~Label Printed Successfully.";
        }

        //public async void PrintMultipleLabel(string sLabelType, DataTable dtLabel)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(sLabelType))
        //        {
        //            return;
        //        }
        //        if (dtLabel.Rows.Count == 0 || dtLabel is null)
        //        {
        //            return;
        //        }

        //        DataTable dt = await blobj.GetPRN(sLabelType);
        //        if (dt.Rows.Count == 0 || dt is null)
        //        {
        //            return;
        //        }
        //        string sPRN = dt.Rows[0][0].ToString();

        //        if (string.IsNullOrWhiteSpace(sPRN))
        //        {
        //            return;
        //        }

        //        foreach (var row in dtLabel.Rows.Cast<DataRow>())
        //        {
        //            string sFinalPRN = LabelPrinting.SubstitutePRNVariables(sPRN, dtLabel, row);
        //            await LabelPrinting.SavePRNContentToFileAsync(sFinalPRN);
        //        }

        //        await LabelPrinting.ZipAndDownloadPRNFilesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        public async Task<string> PrintBulkLabel(string labelType, string[] labelIds)
        {
            try
            {
                labelType = labelType.Trim();
                if (string.IsNullOrWhiteSpace(labelType))
                {
                    return $"ERROR~Invalid Label Type : {labelType}";
                }
                if (!labelIds.Any() || labelIds is null)
                {
                    return "ERROR~Please select atleast one record";
                }

                DataTable dt = await blobj.GetPRN(labelType);
                if (dt.Rows.Count == 0 || dt is null)
                {
                    return $"ERROR~PRN not found, for label type : {labelType}";
                }
                string sPRN = dt.Rows[0][0].ToString();

                if (string.IsNullOrWhiteSpace(sPRN))
                {
                    return $"ERROR~PRN does not contain any data for label type : {labelType}";
                }

                DataTable dtLabelIds = LabelPrinting.GenerateDataTableFromArray(labelIds);

                DataTable dtPRNDetail = await blobj.GetPRNDetail(labelType, dtLabelIds);
                if (dtPRNDetail.Rows.Count == 0 || dtPRNDetail is null)
                {
                    return "ERROR~PRN details not found for selected record.";
                }

                //Extract Column Names from dt
                List<DataColumn> dtColumnList = dtPRNDetail.Columns
                                                .Cast<DataColumn>()
                                                .ToList();

                foreach (var row in dtPRNDetail.Rows.Cast<DataRow>())
                {
                    string sFinalPRN = LabelPrinting.SubstitutePRNVariables(sPRN, dtColumnList, row);
                    await LabelPrinting.SavePRNContentToFileAsync(sFinalPRN);
                }

                await LabelPrinting.ZipAndDownloadPRNFilesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "SUCCESS~Label Printed Successfully.";
        }

        public async Task<string> PrintBulkLabelUsingQueryParam(string labelType, string labelIds)
        {
            try
            {
                labelType = labelType.Trim();
                if (string.IsNullOrWhiteSpace(labelType))
                {
                    return $"ERROR~Invalid Label Type : {labelType}";
                }
                if (!labelIds.Any() || labelIds is null)
                {
                    return "ERROR~Please select atleast one record";
                }

                string[] labelIdArray = labelIds.Split(',');

                DataTable dt = await blobj.GetPRN(labelType);
                if (!ValidatorUtility.IsDtValid(dt))
                {
                    return $"ERROR~PRN not found, for label type : {labelType}";
                }
                string sPRN = dt.Rows[0][0].ToString();

                if (string.IsNullOrWhiteSpace(sPRN))
                {
                    return $"ERROR~PRN does not contain any data for label type : {labelType}";
                }

                DataTable dtLabelIds = LabelPrinting.GenerateDataTableFromArray(labelIdArray);

                DataTable dtPRNDetail = await blobj.GetPRNDetail(labelType, dtLabelIds);
                if (dtPRNDetail.Rows.Count == 0 || dtPRNDetail is null)
                {
                    return "ERROR~PRN details not found for selected record.";
                }

                //Extract Column Names from dt
                List<DataColumn> dtColumnList = dtPRNDetail.Columns
                                                .Cast<DataColumn>()
                                                .ToList();

                foreach (var row in dtPRNDetail.Rows.Cast<DataRow>())
                {
                    string sFinalPRN = LabelPrinting.SubstitutePRNVariables(sPRN, dtColumnList, row);
                    await LabelPrinting.SavePRNContentToFileAsync(sFinalPRN);
                }

                //var isPrintSuccess = 
                await LabelPrinting.ZipAndDownloadPRNFilesAsync();

                //if (!isPrintSuccess)
                //{
                //    return "ERROR~Printing failed, Unable to save file.";
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "SUCCESS~Label Printed Successfully.";
        }
    }
}
