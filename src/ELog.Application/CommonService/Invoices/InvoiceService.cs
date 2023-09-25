using Abp;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using ELog.Application.CommonService.Invoices.Dto;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.CommonService.Invoices
{
    [PMMSAuthorize]
    public class InvoiceService : AbpServiceBase, IInvoiceService, ITransientDependency
    {
        private readonly IRepository<InvoiceDetail> _invoiceRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;

        public InvoiceService(IRepository<InvoiceDetail> invoiceRepository, IRepository<PurchaseOrder> purchaseOrderRepository)
        {
            _invoiceRepository = invoiceRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<bool> IsPurchaseOrderandInvoiceExist(string PO_Number, string invoice_Number, int? invoiceId = null)
        {
            var invoiceDetails = new List<InvoiceDetail>();
            if (invoiceId != null)
            {
                invoiceDetails = await (from invoice in _invoiceRepository.GetAll()
                                        join po in _purchaseOrderRepository.GetAll()
                                        on invoice.PurchaseOrderId equals po.Id
                                        where invoice.PurchaseOrderNo.Trim().ToLower() == PO_Number.Trim().ToLower() && invoice.InvoiceNo.Trim().ToLower() == invoice_Number.Trim().ToLower() && invoice.Id != invoiceId
                                        select invoice).ToListAsync() ?? default;
                return invoiceDetails.Count > 0;
            }

            invoiceDetails = await (from invoice in _invoiceRepository.GetAll()
                                    join po in _purchaseOrderRepository.GetAll()
                                    on invoice.PurchaseOrderId equals po.Id
                                    where invoice.PurchaseOrderNo.Trim().ToLower() == PO_Number.Trim().ToLower() && invoice.InvoiceNo.Trim().ToLower() == invoice_Number.Trim().ToLower()
                                    select invoice).ToListAsync() ?? default;
            return invoiceDetails.Count > 0;
        }

        public async Task<InvoiceDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _invoiceRepository.GetAsync(input.Id);
            return ObjectMapper.Map<InvoiceDto>(entity);
        }

        public async Task<InvoiceDto> UpdateAsync(InvoiceDto input)
        {
            var invoice = await _invoiceRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, invoice);
            await _invoiceRepository.UpdateAsync(invoice);

            return await GetAsync(input);
        }

        public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto input)
        {
            var invoice = ObjectMapper.Map<InvoiceDetail>(input);
            await _invoiceRepository.InsertAsync(invoice);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<InvoiceDto>(invoice);
        }
    }
}