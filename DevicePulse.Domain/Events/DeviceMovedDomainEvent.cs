using DevicePulse.Domain.Interfaces;
using System;

namespace DevicePulse.Domain.Events
{
    public class DeviceMovedDomainEvent : IDomainEvent
    {
        public Guid DeviceId { get; }
        public double OldLatitude { get; }
        public double OldLongitude { get; }
        public double NewLatitude { get; }
        public double NewLongitude { get; }
        public DateTime OccurredAt { get; } = DateTime.UtcNow;

        public DeviceMovedDomainEvent(
            Guid deviceId,
            double oldLatitude,
            double oldLongitude,
            double newLatitude,
            double newLongitude)
        {
            DeviceId = deviceId;
            OldLatitude = oldLatitude;
            OldLongitude = oldLongitude;
            NewLatitude = newLatitude;
            NewLongitude = newLongitude;
        }
    }
}
