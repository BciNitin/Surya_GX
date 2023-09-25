using System;

namespace ELog.Application.Masters.WeighingMachines.Dto
{
    public class WeighingMachineStampingDueOnListDto
    {
        public string WeighingMachineCode { get; set; }
        public DateTime? StampingDoneOn { get; set; }
        public DateTime? StampingDueOn { get; set; }
        public int DueDays { get; set; }
        public int? PlantId { get; set; }
        public string SubPlant { get; set; }
    }
}