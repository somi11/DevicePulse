using DevicePulse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Events
{
    public class BatteryChargingDomainEvent : IDomainEvent
    {
        public Guid DeviceId { get; }
        public double PreviousLevel { get; }
        public double Level { get; }
        public DateTime OccurredAt { get; } = DateTime.UtcNow;

        public BatteryChargingDomainEvent(Guid deviceId, double previousLevel, double level)
        {
            DeviceId = deviceId;
            PreviousLevel = previousLevel;
            Level = level;
        }
    }
}
