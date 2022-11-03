using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Pump
    {
        public int Id { get; set; }
        public int PumpTypeId { get; set; }
        public int StageTypeId { get; set; }
        public int MachineId { get; set; }
        public int DeviceId { get; set; }
        public int? RingId { get; set; }
        public int? CouplingId { get; set; }
        public int? ActuationTypeId { get; set; }
        public int? CardanShaftTypeId { get; set; }
        public int? PulleyId { get; set; }
        public int? FixationTypeId { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ActuationType ActuationType { get; set; }
        public virtual CardanShaftType CardanShaftType { get; set; }
        public virtual Coupling Coupling { get; set; }
        public virtual Device Device { get; set; }
        public virtual FixationType FixationType { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Pulley Pulley { get; set; }
        public virtual PumpType PumpType { get; set; }
        public virtual Ring Ring { get; set; }
        public virtual StageType StageType { get; set; }
    }
}
