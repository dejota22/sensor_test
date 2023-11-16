using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Motor
    {
        public int Id { get; set; }
        public int? MachineId { get; set; }
        public int? ActuationTypeId { get; set; }
        public int? FixationTypeId { get; set; }
        public int? CouplingTypeId { get; set; }
        public int? CardanShaftTypeId { get; set; }
        public int? PulleyTypeId { get; set; }
        public int? PumpTypeId { get; set; }
        public int? CompressorTypeId { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Carcase { get; set; }
        public double? CvPotency { get; set; }
        public double? KwPotency { get; set; }
        public double? RpmPotency { get; set; }
        public double? Voltage { get; set; }
        public double? ElectricCurrent { get; set; }
        public double? Frequency { get; set; }
        public string IsolationClass { get; set; }
        public int? Ip { get; set; }
        public double? Yeld { get; set; }
        public double? Sf { get; set; }
        public double? Pf { get; set; }
        public string Category { get; set; }
        public string Bushing { get; set; }
        public string Lubrication { get; set; }
        public string CouplingBrand { get; set; }
        public string CouplingModel { get; set; }
        public string CouplingDescription { get; set; }
        public string ReducerDescription { get; set; }
        public string ReducerTag { get; set; }
        public string ReducerBrand { get; set; }
        public string ReducerModel { get; set; }
        public string ReducerSerialNumber { get; set; }
        public string ReducerRatio { get; set; }
        public string ReducerNumberOfAxles { get; set; }
        public string ReducerDetails { get; set; }
        public string PumpDescription { get; set; }
        public string PumpTag { get; set; }
        public string PumpBrand { get; set; }
        public string PumpModel { get; set; }
        public int? PumpShovelQuantity { get; set; }
        public int? PumpGearTeethQuantity { get; set; }
        public int? PumpBladeQuantity { get; set; }
        public int? PumpRotation { get; set; }
        public string PumpBearings { get; set; }
        public int? RingDriveGearTeethZ1 { get; set; }
        public int? RingDriveGearTeethZ2 { get; set; }
        public string RingRatio { get; set; }
        public double? RingBetweenAxles { get; set; }
        public double? PulleyD1 { get; set; }
        public double? PulleyD2 { get; set; }
        public double? PulleyBetweenAxles { get; set; }
        public int? PulleyBeltQuantity { get; set; }
        public double? PulleyRatio { get; set; }
        public string ExhaustFanDescription { get; set; }
        public string ExhaustFanTag { get; set; }
        public string ExhaustFanBrand { get; set; }
        public string ExhaustFanModel { get; set; }
        public double? ExhaustFanRotorDiameter { get; set; }
        public int? ExhaustFanNumberOfBlades { get; set; }
        public int? ExhaustFanRotation { get; set; }
        public string ExhaustFanBearings { get; set; }
        public string CompressorDescription { get; set; }
        public string CompressorTag { get; set; }
        public string CompressorBrand { get; set; }
        public string CompressorModel { get; set; }
        public string CompressorSerialNumber { get; set; }
        public string CompressorDetails { get; set; }
        public int? CompressorRotation { get; set; }
        public string CompressorBearings { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Observation { get; set; }
        public int? DeviceId { get; set; }
        public int? CompanyId { get; set; }
        public int? TypeId { get; set; }
        public string SectorDesc { get; set; }

        public bool InputAxle { get; set; }
        public string InputAxleRoll { get; set; }
        public int? InputAxleTeeth { get; set; }

        public bool MiddleAxle { get; set; }
        public string MiddleAxleRoll { get; set; }
        public int? MiddleAxleTeeth1 { get; set; }
        public int? MiddleAxleTeeth2 { get; set; }

        public bool MiddleAxle2 { get; set; }
        public string MiddleAxle2Roll { get; set; }
        public int? MiddleAxle2Teeth1 { get; set; }
        public int? MiddleAxle2Teeth2 { get; set; }

        public bool OutputAxle { get; set; }
        public string OutputAxleRoll { get; set; }
        public int? OutputAxleTeeth { get; set; }

        //public virtual Device Device { get; set; }

        public virtual ICollection<DeviceMotor> MotorDevices { get; set; }
    }
}
