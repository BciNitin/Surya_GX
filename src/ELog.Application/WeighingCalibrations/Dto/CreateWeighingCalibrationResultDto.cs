using Abp.Application.Services.Dto;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class CreateWeighingCalibrationResultDto : EntityDto<int>
    {
        public int CalibrationStatusId { get; set; }
    }
}