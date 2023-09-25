using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Dispensing.CubicleAssignments.Dto
{
    [AutoMapFrom(typeof(CubicleAssignmentHeader))]
    public class CubicleAssignmentListDto : EntityDto<int>
    {
        public int? ProcessOrderId { get; set; }
        public string ProcessOrderNo { get; set; }
        public int? InspectionLotId { get; set; }
        public string InspectionLotNo { get; set; }
        public string GroupStatus { get; set; }
        public int? GroupStatusId { get; set; }
        public string GroupCode { get; set; }
        public int? SubPlantId { get; set; }
    }
}