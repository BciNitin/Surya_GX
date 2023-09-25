using System;

namespace ELog.Application.Masters.WeighingMachines.Dto
{
    public class StandardWeightStampingDueListDto
    {
        public string StandardWeightBoxId { get; set; }
        public DateTime StampingDoneOn { get; set; }
        public DateTime StampingDueOn { get; set; }
        public string Department { get; set; }
        public string Area { get; set; }
        public int DueDays { get; set; }
        public int? PlantId { get; set; }
        public string SubPlant { get; set; }
    }
}