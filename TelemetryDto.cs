using System;

namespace DevicePulse.Domain.Entities
{
    public class TelemetryDto
    {
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public AccelerationData Acceleration { get; set; }
        public BatteryData Battery { get; set; }
        public GpsData Gps { get; set; }
        public NetworkData Network { get; set; }

        public TelemetryDto(Guid deviceId, DateTime timestamp,
            AccelerationData acceleration, BatteryData battery, GpsData gps, NetworkData network)
        {
            DeviceId = deviceId;
            Timestamp = timestamp;
            Acceleration = acceleration;
            Battery = battery;
            Gps = gps;
            Network = network;
        }

        public TelemetryDto() { }
    }
}
