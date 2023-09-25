using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.LogFormsHistoryApi.Dto
{
    public class LogFormHistoryMapProfile : Profile
    {
        public LogFormHistoryMapProfile()
        {
            CreateMap<LogFormHistoryDto, LogFormHistory>();
            CreateMap<LogFormHistoryDto, LogFormHistory>();
            CreateMap<CreateLogFormHistoryDto, LogFormHistory>();

            CreateMap<LogFormHistoryDto, LogFormHistory>();
            CreateMap<LogFormHistory, LogFormHistoryDto>();
        }
    }
}
