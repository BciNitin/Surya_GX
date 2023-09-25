using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.CageLabelPrint.Dto
{

    [AutoMapFrom(typeof(CageLabelPrinting))]
    public class CageLabelPrintingDto : EntityDto<int>
    {

        public int DispensingId { get; set; }
        public string DispensingBarcode { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        //[ForeignKey("ProcessOrders")]
        public string ProductName { get; set; }
        public int ProcessorderID { get; set; }
        public string ProcessOrderNo { get; set; }
        //[ForeignKey("CubicleMaster")]
        public int CubicleID { get; set; }
        public string CubcileCode { get; set; }
        public int? NoOfContainer { get; set; }
        public string CageLabelBarcode { get; set; }
        public int? PrintCount { get; set; }
        public int? PrinterID { get; set; }
        public bool IsActive { get; set; }
        public string BatchNo { get; set; }
        public bool IsPrint { get; set; }

    }
}
