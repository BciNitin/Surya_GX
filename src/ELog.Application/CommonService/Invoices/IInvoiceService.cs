using Abp.Application.Services.Dto;

using ELog.Application.CommonService.Invoices.Dto;

using System.Threading.Tasks;

namespace ELog.Application.CommonService.Invoices
{
    public interface IInvoiceService
    {
        Task<bool> IsPurchaseOrderandInvoiceExist(string PO_Number, string invoice_Number, int? invoiceId = null);

        Task<InvoiceDto> GetAsync(EntityDto<int> input);

        Task<InvoiceDto> UpdateAsync(InvoiceDto input);

        Task<InvoiceDto> CreateAsync(CreateInvoiceDto input);
    }
}