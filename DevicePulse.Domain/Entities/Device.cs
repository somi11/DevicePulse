using DevicePulse.Domain.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Entities
{
    public class Device
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }

        //device status
        public AccelerationStatus Acceleration { get; set; } = new();
        public BatteryStatus Battery { get; set; } = new();
        public GpsStatus Gps { get; set; } = new();
        public NetworkStatus Network { get; set; } = new();

        // Internal collections
        private readonly List<TelemetryReading> _telemetryReadings = new();
        public IReadOnlyCollection<TelemetryReading> TelemetryReadings => _telemetryReadings.AsReadOnly();

        private readonly List<DeviceEvent> _events = new();
        public IReadOnlyCollection<DeviceEvent> Events => _events.AsReadOnly();

        // Add telemetry
        public void AddTelemetryReading(TelemetryReading telemetryReading)
        {
            if (telemetryReading == null) throw new ArgumentNullException(nameof(telemetryReading));
            _telemetryReadings.Add(telemetryReading);

            // Optional: apply cap to prevent data overload
            const int maxReadings = 1000;
            if (_telemetryReadings.Count > maxReadings)
                _telemetryReadings.RemoveAt(0); // remove oldest
        }

        //device update status
        public void UpdateFromTelemetry(TelemetryReading telemetryReading)
        {
            Acceleration.Update(telemetryReading.Acceleration);
            Battery.Update(telemetryReading.Battery);
            Gps.Update(telemetryReading.Gps);
            Network.Update(telemetryReading.Network);
        }
        public void AddEvent(DeviceEvent deviceEvent)
        {
            if (deviceEvent == null) throw new ArgumentNullException(nameof(deviceEvent));
            _events.Add(deviceEvent);
            const int maxReadings = 1000;
            if (_events.Count > maxReadings)
                _events.RemoveAt(0); // remove oldest
        }
        public Device() { } // Required for EF
    }
}
