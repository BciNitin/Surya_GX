using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.CommonService.Invoices.Dto
{
    public class InvoiceMapProfile : Profile
    {
        public InvoiceMapProfile()
        {
            CreateMap<InvoiceDto, InvoiceDetail>();
            CreateMap<InvoiceDto, InvoiceDetail>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<InvoiceDetail, InvoiceDto>();
            CreateMap<CreateInvoiceDto, InvoiceDetail>();
            CreateMap<InvoiceDto, CreateInvoiceDto>();
        }
    }
}