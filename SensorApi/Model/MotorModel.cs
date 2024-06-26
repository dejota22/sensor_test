﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SensorApi.Models
{
    public class MotorModel 
    {
        public int MotorId { get; set; }

        [Display(Name = "MachineType")]
        //[Required(ErrorMessage = "Required")]
        public int MachineId { get; set; }
        // public int? ActuationTypeId { get; set; }

        [Display(Name = "FixationType")]
        //[Required(ErrorMessage = "Required")]
        public int? FixationTypeId { get; set; }

        [Display(Name = "CouplingType")]
        //[Required(ErrorMessage = "Required")]
        public int? CouplingTypeId { get; set; }

        [Display(Name = "CardanShaftType")]
        //[Required(ErrorMessage = "Required")]
        public int? CardanShaftTypeId { get; set; }

        //[Display(Name = "PulleyType")]
        //public int? PulleyTypeId { get; set; }

        [Display(Name = "PumpType")]
        //[Required(ErrorMessage = "Required")]
        public int? PumpTypeId { get; set; }

        [Display(Name = "CompressorType")]
        //[Required(ErrorMessage = "Required")]
        public int? CompressorTypeId { get; set; }

        [Display(Name = "DeviceId")]
        //[Required(ErrorMessage = "Required")]
        public int? DeviceId { get; set; }

        [Display(Name = "Name")]
        //[Required(ErrorMessage = "Required")]

        public string Name { get; set; }
        [Display(Name = "Tag")]
        //[Required(ErrorMessage = "Required")]
        public string Tag { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Carcase")]
        public string Carcase { get; set; }

        [Display(Name = "CvPotency")]
        public double? CvPotency { get; set; }

        [Display(Name = "KwPotency")]
        public double? KwPotency { get; set; }

        [Display(Name = "RpmPotency")]
        public double? RpmPotency { get; set; }

        [Display(Name = "Voltage")]
        public double? Voltage { get; set; }

        [Display(Name = "ElectricCurrent")]
        public double? ElectricCurrent { get; set; }

        [Display(Name = "Frequency")]
        public double? Frequency { get; set; }

        [Display(Name = "IsolationClass")]
        public string IsolationClass { get; set; }

        [Display(Name = "Ip")]
        public int? Ip { get; set; }

        [Display(Name = "Yeld")]
        public double? Yeld { get; set; }

        [Display(Name = "Sf")]
        public double? Sf { get; set; }

        [Display(Name = "Pf")]
        public double? Pf { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Bushing")]
        public string Bushing { get; set; }

        [Display(Name = "Lubrification")]
        public string Lubrication { get; set; }

        [Display(Name = "CouplingBrand")]
        public string CouplingBrand { get; set; }

        [Display(Name = "CouplingModel")]
        public string CouplingModel { get; set; }

        [Display(Name = "CouplingDescription")]
        public string CouplingDescription { get; set; }

        [Display(Name = "ReducerDescription")]
        public string ReducerDescription { get; set; }

        [Display(Name = "ReducerTag")]
        public string ReducerTag { get; set; }

        [Display(Name = "ReducerBrand")]
        public string ReducerBrand { get; set; }

        [Display(Name = "ReducerModel")]
        public string ReducerModel { get; set; }

        [Display(Name = "ReducerSerialNumber")]
        public string ReducerSerialNumber { get; set; }

        [Display(Name = "ReducerRatio")]
        public string ReducerRatio { get; set; }

        [Display(Name = "ReducerNumberOfAxles")]
        public string ReducerNumberOfAxles { get; set; }

        //[Display(Name = "ReducerDetails")]
        //public string ReducerDetails { get; set; }

        [Display(Name = "PumpDescription")]
        public string PumpDescription { get; set; }

        [Display(Name = "PumpTag")]
        public string PumpTag { get; set; }

        [Display(Name = "PumpBrand")]
        public string PumpBrand { get; set; }

        [Display(Name = "PumpModel")]
        public string PumpModel { get; set; }

        [Display(Name = "RingDriveGearTeethZ1")]
        public int? RingDriveGearTeethZ1 { get; set; }

        [Display(Name = "RingDriveGearTeethZ2")]
        public int? RingDriveGearTeethZ2 { get; set; }

        [Display(Name = "RingRatio")]
        public string RingRatio { get; set; }

        [Display(Name = "RingBetweenAxles")]
        public double? RingBetweenAxles { get; set; }

        [Display(Name = "PulleyD1")]
        public double? PulleyD1 { get; set; }

        [Display(Name = "PulleyD2")]
        public double? PulleyD2 { get; set; }

        [Display(Name = "PulleyBetweenAxles")]
        public double? PulleyBetweenAxles { get; set; }

        [Display(Name = "PulleyBeltQuantity")]
        public int? PulleyBeltQuantity { get; set; }

        [Display(Name = "PulleyRatio")]
        public double? PulleyRatio { get; set; }

        [Display(Name = "ExhaustFanDescription")]
        //[Required(ErrorMessage = "Required")]

        public string ExhaustFanDescription { get; set; }

        [Display(Name = "ExhaustFanTag")]
        //[Required(ErrorMessage = "Required")]

        public string ExhaustFanTag { get; set; }

        [Display(Name = "ExhaustFanBrand")]
        public string ExhaustFanBrand { get; set; }

        [Display(Name = "ExhaustFanModel")]
        public string ExhaustFanModel { get; set; }

        [Display(Name = "ExhaustFanRotorDiameter")]
        public double? ExhaustFanRotorDiameter { get; set; }

        [Display(Name = "ExhaustFanNumberOfBlades")]
        public int? ExhaustFanNumberOfBlades { get; set; }

        [Display(Name = "CompressorDescription")]
        public string CompressorDescription { get; set; }

        [Display(Name = "CompressorTag")]
        public string CompressorTag { get; set; }

        [Display(Name = "CompressorBrand")]
        public string CompressorBrand { get; set; }

        [Display(Name = "CompressorModel")]
        public string CompressorModel { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

       
        //public string Temperature { get; set; }
        //public string ReadDataType { get; set; }
        //public string XAxle { get; set; }
        //public string YAxle { get; set; }
        //public string ZAxle { get; set; }

        [Display(Name = "Observation")]
        public string Observation { get; set; }

        //public virtual ActuationType ActuationType { get; set; }
        //public virtual CardanShaftType CardanShaftType { get; set; }
        //public virtual Coupling Coupling { get; set; }
        //public virtual Device Device { get; set; }
        //public virtual FixationType FixationType { get; set; }
        //public virtual Machine Machine { get; set; }
        //public virtual Pulley Pulley { get; set; }
        //public virtual Ring Ring { get; set; }
        //public virtual ICollection<DeviceMeasure> DeviceMeasure { get; set; }
    }

    public class SensorData
    {
        public string id { get; set; }
    }
}
