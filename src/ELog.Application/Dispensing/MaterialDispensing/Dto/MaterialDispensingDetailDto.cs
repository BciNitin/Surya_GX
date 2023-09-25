using Abp.Application.Services.Dto;

namespace ELog.Application.Dispensing.MaterialDispensing.Dto
{
    public class MaterialDispensingDetailDto : EntityDto<int>
    {
        public int? NoOfPacks { get; set; }
        public float? GrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string DispensingContainerBarcode { get; set; }
        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }
        public bool Printed { get; set; }
        public string GrossWeightWithUOM
        { get { return $"{GrossWeight ?? 0} {UnitOfMeasurement}"; } }

        public string NetWeightWithUOM
        { get { return $"{NetWeight ?? 0} {UnitOfMeasurement}"; } }

        public string TareWeightWithUOM
        { get { return $"{TareWeight ?? 0} {UnitOfMeasurement}"; } }

        public string NoOfPacksWithUOM
        { get { return $"{NoOfPacks ?? 0} {UnitOfMeasurement}"; } }
    }
}