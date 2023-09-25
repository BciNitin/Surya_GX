using AutoMapper;
using ELog.Core.Entities;


namespace ELog.Application.WIP.WIPPrintPacking.Dto
{
    public class PrintPackingMapProfile : Profile
    {
        public PrintPackingMapProfile()
        {

            CreateMap<PrintPackingDto, LabelPrintPacking>()
                    .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<LabelPrintPacking, PrintPackingDto>();
            CreateMap<CreatePrintPackingDto, LabelPrintPacking>();
            CreateMap<CreatePrintPackingDto, CreatePrintPackingDto>();
            CreateMap<PrintPackingListDto, LabelPrintPacking>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            //.ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<LabelPrintPacking, PrintPackingListDto>();


        }
    }
}
