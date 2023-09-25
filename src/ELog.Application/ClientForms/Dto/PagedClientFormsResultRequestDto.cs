using Abp.Application.Services.Dto;
using System;

namespace Elog.Application.ClientForms.Dto
{
    public class PagedClientFormsResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public string? FormName { get; set; }
        public bool? IsActive { get; set; }
        public int? FormStatus { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public string ?Permissions { get; set; }
        public string ?MenuId { get; set; }
    }
}




