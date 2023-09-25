using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.LogsData.Dto
{
    public class LogDataMapProfile : Profile
    {
        public LogDataMapProfile()
        {
            CreateMap<LogsDataDto, Logs>();

            CreateMap<CreateLogDataDto, Logs>();


            CreateMap<LogDataListDto, Logs>();
            CreateMap<Logs, LogDataListDto>();
        }
    }
}