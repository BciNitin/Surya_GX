using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.WIPPrintPacking.Dto
{
    public class PagedPrintPackingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string ProcessOrderNo { get; set; }



        public string ProductCode { get; set; }

        public string BatchNo { get; set; }


        public string ContainerBarcode { get; set; }

    }
}
