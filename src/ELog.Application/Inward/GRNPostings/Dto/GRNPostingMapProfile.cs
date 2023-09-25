using AutoMapper;

using ELog.Adapter.SAPAjantaAdapter.Entities;
using ELog.Core.Entities;
using ELog.Core.SAP;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    public class GRNPostingMapProfile : Profile
    {
        public GRNPostingMapProfile()
        {
            CreateMap<GRNPostingDto, GRNHeader>();
            CreateMap<GRNPostingDto, GRNHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CreateGRNPostingDto, GRNHeader>();

            CreateMap<GRNPostingDetailsDto, GRNDetail>()
               .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<GRNDetail, GRNPostingDetailsDto>();
            CreateMap<GRNPostingQtyDetailsDto, GRNQtyDetail>()

               .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<GRNQtyDetail, GRNPostingQtyDetailsDto>();
            CreateMap<GRNRequestResponseDto, GRNRequest>();
            CreateMap<SAPGRNPosting, GRNRequestResponseDto>();
            CreateMap<GRNRequestResponseDto, SAPGRNPosting>();
            CreateMap<SAPGRNPosting, Record>();
            CreateMap<Record, SAPGRNPosting>();
        }
    }
}