using DevicePulse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Events
{
    public class BatteryDropDomainEvent : IDomainEvent
    {
        public Guid DeviceId { get; }
        public DateTime OccurredAt { get; } = DateTime.UtcNow;
        public double Level { get; }
        public double _previousLevel;

        public BatteryDropDomainEvent(Guid deviceId , double previousLevel , double level)
        {
            DeviceId = deviceId;
            _previousLevel = previousLevel;
            Level = level;
        }
    }
}
