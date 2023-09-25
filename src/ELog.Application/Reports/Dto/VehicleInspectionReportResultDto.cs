using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    [AutoMapFrom(typeof(VehicleInspectionHeader))]
    public class VehicleInspectionReportResultDto : EntityDto<int>
    {
        public DateTime InspectionDate { get; set; }
        public string GatePassNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ChecklistName { get; set; }

        public int PurchaseOrderId { get; set; }
        public int InvoiceId { get; set; }
        public int SubPlantId { get; set; }
        public int GateEntryId { get; set; }
        public int VehicleInspectionDetailId { get; set; }
        public string CheckpointName { get; set; }
        public string CheckpointType { get; set; }
        public string ValueTag { get; set; }
        public string AcceptanceValue { get; set; }
        public string UserdEnteredValue { get; set; }
        public string Remark { get; set; }
        public int TransactionStatusId { get; set; }
        public string TransactionStatus { get; set; }
        public string MaterialCode { get; set; }
        public int MaterialId { get; set; }
        public int CheckPointId { get; set; }
        public string InspectedBy { get; set; }
        public List<VehicleInspectionCheckPointReportResultDto> CheckPoints { get; set; }
    }

    public class VehicleInspectionCheckPointReportResultDto : EntityDto<int>
    {
        public string CheckpointName { get; set; }
        public string CheckpointType { get; set; }
        public string ValueTag { get; set; }
        public string AcceptanceValue { get; set; }
        public string UserdEnteredValue { get; set; }
        public string Remark { get; set; }
    }
}