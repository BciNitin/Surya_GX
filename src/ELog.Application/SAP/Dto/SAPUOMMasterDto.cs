using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.SAP.Dto
{
    [AutoMapTo(typeof(SAPUOMMaster))]
    public class SAPUOMMasterDto
    {
        public string UOM { get; set; }
        public string Description { get; set; }
    }
}