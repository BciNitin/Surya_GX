using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class AuditReportDetailsDto : PagedAndSortedResultRequestDto
    {
        public string PlantName { get; set; }

        public string TenancyName { get; set; }
        public string CreatedBy { get; set; }
          public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
           public DateTime? ModifiedOn { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }


        public string City { get; set; }
        public string CountryName { get; set; }
        public string Email { get; set; }
        public string GS1Prefix { get; set; }
        public string License { get; set; }
        public string PhoneNumber { get; set; }
        public string PlantId { get; set; }
        public string PostalCode { get; set; }
        public string StateName { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string Website { get; set; }
        //   public int IsActive { get; set; }
        public int PlantTypeId { get; set; }
        public int MasterPlantId { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
            public DateTime SysStartTime { get; set; }
           public DateTime SysEndTime { get; set; }


    }
}