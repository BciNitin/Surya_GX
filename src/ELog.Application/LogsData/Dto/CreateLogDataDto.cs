using Abp.AutoMapper;
using ELog.Core.Entities;
using System;

namespace ELog.Application.LogsData.Dto
{

    [AutoMapTo(typeof(Logs))]
    public class CreateLogDataDto
    {
        //  public int Id { get; set; }
        public string Action { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Data { get; set; }
        public string CreatedBy { get; set; }
        public string Submodule { get; set; }
        public string ModuleName { get; set; }
    }
}
