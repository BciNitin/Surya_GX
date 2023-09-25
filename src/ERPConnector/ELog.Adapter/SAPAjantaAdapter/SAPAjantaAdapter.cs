
using ELog.Adapter.SAPAjantaAdapter.Entities;
using ELog.Core.SAP;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Adapter.SAPAjantaAdapter
{
    //call WSDL or AJanta SAP webservice to get and post data
    public class SAPAjantaAdapter : ISAPAjantaAdapter, IAdapter
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IConfiguration _configuration;
        public SAPAjantaAdapter(IHttpClientProvider httpClientProvider, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientProvider = httpClientProvider;
        }

        public async Task<GRNRequestResponseDto> GRNPosting(GRNRequestResponseDto grnRequestResponseDto)
        {
            var baseUrl = _configuration["SAPApiCredentials:BaseUrl"];

            //Convert DTO to GRNPostingRequest input request
            var grnPostingRequest = new GRNPostingRequest()
            {
                Record = await GetGRNRequestsDtoAsync(grnRequestResponseDto)
            };
            // Call HttpHandler with baseUrl and Suffix url
            var response = await PostData($"{baseUrl}{APIEndPoints.SAPGrnPost}", grnPostingRequest);
            if (response == null)
            {
                return null;
            }
            //Convert response to Grn DTO and return the object
            var grnPostingResponse = JsonConvert.DeserializeObject<GRNPostingResponse>(response);

            var grnPostedDto = await GetGRNResponseDtoAsync(grnRequestResponseDto, grnPostingResponse);
            return grnPostedDto;
        }
        public async Task<IssueToProductionRequestResponseDto> IssueToProduction(IssueToProductionRequestResponseDto issueToProductionRequestResponseDto)
        {
            var baseUrl = _configuration["SAPApiCredentials:BaseUrl"];

            var issueToProductionRequest = new IssueToProdutionRequest()
            {
                Record = await GetIssueToProductionRequestDtoAsync(issueToProductionRequestResponseDto)
            };
            var response = await PostData($"{baseUrl}{APIEndPoints.SAPIssueToProductionPost}", issueToProductionRequest);
            if (response == null)
            {
                return null;
            }
            var issueToProdcutionResponse = JsonConvert.DeserializeObject<IssueToProductionResponse>(response);
            var issueToProdcutionPostedDto = await GetIssueToProductionResponseDtoAsync(issueToProductionRequestResponseDto, issueToProdcutionResponse);
            return issueToProdcutionPostedDto;
        }

        private async Task<string> PostData(string url, object input)
        {
            var myContent = JsonConvert.SerializeObject(input);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");

            var res = await _httpClientProvider.PostAsync(url, stringContent);
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadAsStringAsync();
            }
            return null;
        }
        private async Task<List<GRNRequest>> GetGRNRequestsDtoAsync(GRNRequestResponseDto grnpostingdtoList)
        {
            var results = new List<GRNRequest>();
            foreach (var record in grnpostingdtoList.Record)
            {
                var grnRequest = new GRNRequest()
                {
                    ExpDate = record.ExpDate,
                    ItemCode = record.ItemCode,
                    LRDate = record.LRDate,
                    LRNo = record.LRNo,
                    Manufacturer = record.Manufacturer,
                    MfgDate = record.MfgDate,
                    NetQty = record.NetQty,
                    NoOfCases = record.NoOfCases,
                    POLineItem = record.POLineItem,
                    PurchaseOrder = record.PurchaseOrder,
                    TransporterName = record.TransporterName,
                    UOM = record.UOM,
                    Vehicle = record.Vehicle,
                    VendorBatch = record.VendorBatch,
                    VendorCode = record.VendorCode,
                    Storage_location = record.Storage_location,
                    Bill_of_lading = record.Bill_of_lading,
                    Delivery_note_no = record.Delivery_note_no,
                    LineItem = record.LineItem,
                    QtyPerContainer = record.QtyPerContainer,
                    GRNPreparedBy = record.GRNPreparedBy,
                    InvoiceQty = record.InvoiceQty,
                    InvoiceNo = record.InvoiceNo,
                    Remark = record.Remark,
                    No_Of_Containers = record.No_Of_Containers,

                };
                results.Add(grnRequest);
            }
            return results;
        }
        private async Task<GRNRequestResponseDto> GetGRNResponseDtoAsync(GRNRequestResponseDto requestResponse, GRNPostingResponse response)
        {
            response.Record.RemoveAt(0);
            foreach (var record in requestResponse.Record)
            {
                var grnResponseForMaterial = response.Record.Find(a => a.ItemCode == record.ItemCode && a.LineItem == record.LineItem);
                record.GRNNo = grnResponseForMaterial != null ? Convert.ToString(grnResponseForMaterial.GRNNo) : null;
                record.SAPBatchNo = grnResponseForMaterial != null ? Convert.ToString(grnResponseForMaterial.SAPBatchNo) : null;
                record.LineItem = grnResponseForMaterial != null ? Convert.ToString(grnResponseForMaterial.LineItem) : null;
                record.NextInspectionDate = grnResponseForMaterial != null ? Convert.ToString(grnResponseForMaterial.NextInspectionDate) : null;
                record.InspectionLotNo = grnResponseForMaterial != null ? Convert.ToString(grnResponseForMaterial.InspectionLotNo) : null;
                record.ItemCode = grnResponseForMaterial != null ? Convert.ToString(grnResponseForMaterial.ItemCode) : null;
            }
            return requestResponse;
        }

        private async Task<List<IssueToProductionRequestRecord>> GetIssueToProductionRequestDtoAsync(IssueToProductionRequestResponseDto issueToProductionRequestResponseDto)
        {
            var request = new IssueToProdutionRequest();
            var requestRecords = new List<IssueToProductionRequestRecord>();
            foreach (var record in issueToProductionRequestResponseDto.IssueToProductionRecords)
            {
                var issueToProductionRequest = new IssueToProductionRequestRecord()
                {
                    ProcessOrderNo = record.ProcessOrderNo,
                    MaterialCode = record.MaterialCode,
                    MaterialDescription = record.MaterialDescription,
                    Product = record.Product,
                    ProductBatchNo = record.ProductBatch,
                    SapBatchNo = record.SAPBatchNo,
                    DispensedQty = record.DispensedQty,
                    MvtType = record.MvtType,
                    StorageLocation = record.Storage_location,
                    UOM = record.UOM
                };
                requestRecords.Add(issueToProductionRequest);
            }
            request.Record = requestRecords;
            return request.Record;
        }

        private async Task<IssueToProductionRequestResponseDto> GetIssueToProductionResponseDtoAsync(IssueToProductionRequestResponseDto requestResponseDto, IssueToProductionResponse response)
        {
            foreach (var record in requestResponseDto.IssueToProductionRecords)
            {
                record.MaterialIssueNoteNo = response.Record.MaterialIssueNoteNo;
            }
            return requestResponseDto;
        }

    }
}