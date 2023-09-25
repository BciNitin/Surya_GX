using AutoMapper;

using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
    public class ReportMapProfile : Profile
    {
        public ReportMapProfile()
        {
            CreateMap<ReportConfigurationDto, ReportConfiguration>().ReverseMap();
        }      
    }
}
