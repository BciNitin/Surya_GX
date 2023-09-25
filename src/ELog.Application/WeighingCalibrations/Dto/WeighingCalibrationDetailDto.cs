using Abp.Application.Services.Dto;

using ELog.Core;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WeighingCalibrationDetailDto : EntityDto<int>
    {
        public int? WMCalibrationHeaderId { get; set; }

        public string StandardWeight { get; set; }
        public string WeightRange { get; set; }
        public int? CalibrationLevelId { get; set; }

        public int? StandardWeightBoxId { get; set; }

        public double CapturedWeight { get; set; }
        public string CapturedWeightString { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string Remark { get; set; }
        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }
        public string InitialZeroReading { get; set; }
        public string SpriritLevelBubble { get; set; }
        public int? CalibrationStatusId { get; set; }
        public int? WeighingMachineId { get; set; }
        public List<int> lstWeightId { get; set; }
        public string WeighingMachineCode { get; set; }
        public string UserEnteredCalibrationLevel { get; set; }
        public string UserEnteredWeightBox { get; set; }
        public string UserEnteredWeightId { get; set; }
    }
}