using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
    public class DispensingTrackingReportDto
    {
        public string GroupId { get; set; }
        public string ProcessOrderNo { get; set; }
        public string BatchNo { get; set; }

        public string MaterialCode { get; set; }
        public string SAPBatchNo { get; set; }
        public List<DispensingTrackingActivity> lstActivity { get; set; }
    }

    public class DispensingTrackingActivity
    {
        public string ActivityName { get; set; }
        public string DoneBy { get; set; }
        public DateTime? ActvityDate { get; set; }
        public string ActivityCheckBy { get; set; }
    }
}