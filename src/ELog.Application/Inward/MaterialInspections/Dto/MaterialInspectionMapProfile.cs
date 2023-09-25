using AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class MaterialInspectionMapProfile : Profile
    {
        public MaterialInspectionMapProfile()
        {
            CreateMap<CreateMaterialInspectionDto, MaterialInspectionHeader>()
                 .ForMember(x => x.InvoiceId, opt => opt.MapFrom(x => x.InvoiceDetails.Id))
                 .ForMember(x => x.TransactionStatusId, opt => opt.MapFrom(x => x.MaterialInspectionTransactionId))
                 .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CreateMaterialInspectionDto, MaterialInspectionRelationDetail>()
                .ForMember(x => x.TransactionStatusId, opt => opt.MapFrom(x => x.MaterialTransactionId))
                .ForMember(x => x.MaterialDamageDetails, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CheckpointDto, MaterialChecklistDetail>();
            CreateMap<MaterialChecklistDetail, CheckpointDto>();

            CreateMap<MaterialInspectionRelationDetail, MaterialInspectionDto>()
               .ForMember(x => x.MaterialTransactionId, opt => opt.MapFrom(x => x.TransactionStatusId))
               .ForMember(x => x.MaterialRelationId, opt => opt.MapFrom(x => x.Id))
               .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<MaterialInspectionDto, MaterialInspectionRelationDetail>()
              .ForMember(x => x.TransactionStatusId, opt => opt.MapFrom(x => x.MaterialTransactionId))
              .ForMember(x => x.Id, opt => opt.MapFrom(x => x.MaterialRelationId))
              .ForMember(x => x.MaterialDamageDetails, opt => opt.Ignore())
              .ForMember(x => x.MaterialHeaderId, opt => opt.MapFrom(x => x.Id));
            CreateMap<MaterialDamageDto, MaterialDamageDetail>();
        }
    }
}