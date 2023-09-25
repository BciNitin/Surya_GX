using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.WIP.CageLabelPrint.Dto
{

    public class CageLabelPrintMapProfile : Profile
    {
        public CageLabelPrintMapProfile()
        {
            CreateMap<CageLabelPrintingDto, CageLabelPrinting>();
            CreateMap<CageLabelPrintingDto, CageLabelPrinting>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            //CreateMap<CreateAreaDto, AreaMaster>();
            CreateMap<CageLabelPrintingListDto, CageLabelPrinting>();
            CreateMap<CageLabelPrinting, CageLabelPrintingListDto>();
        }
    }
}
