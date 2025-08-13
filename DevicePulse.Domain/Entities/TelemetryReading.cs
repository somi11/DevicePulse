using DevicePulse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Entities
{
    public class TelemetryReading
    {
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public AccelerationData Acceleration { get; set; }
        public BatteryData Battery { get; set; }
        public GpsData Gps { get; set; }
        public NetworkData Network { get; set; }

        public TelemetryReading(Guid deviceId, DateTime timestamp,
            AccelerationData acceleration, BatteryData battery, GpsData gps, NetworkData network)
        {
            DeviceId = deviceId;
            Timestamp = timestamp;
            Acceleration = acceleration;
            Battery = battery;
            Gps = gps;
            Network = network;
        }

        public TelemetryReading() { }
    }
}
