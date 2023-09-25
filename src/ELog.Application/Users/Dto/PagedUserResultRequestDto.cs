using Abp.Application.Services.Dto;

using System;

namespace ELog.Application.Users.Dto
{
    public class PagedUserResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string UserName { get; set; }
        public int? PlantId { get; set; }
        public int? ModeId { get; set; }
        public int? DesignationId { get; set; }
        public int? ApprovalStatusId { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public DateTime? CreationFromTime { get; set; }
        public DateTime? CreationToTime { get; set; }
    }
}