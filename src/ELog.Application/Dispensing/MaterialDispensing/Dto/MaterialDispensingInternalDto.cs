﻿namespace ELog.Application.Dispensing.MaterialDispensing.Dto
{
    public class MaterialDispensingInternalDto
    {
        public string ConversionUOMName { get; set; }
        public float? Denominator { get; set; }
        public float? Numerator { get; set; }
        public int? UomId { get; set; }
        public int? UnitOfMeasurementTypeId { get; set; }
        public string UOMType { get; set; }
        public bool IsPackUOM { get; set; }
        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }
    }
}