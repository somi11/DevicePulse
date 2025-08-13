using DevicePulse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery
{
    public class DeviceDto
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }

        public AccelerationData Acceleration { get; set; }
        public BatteryData Battery { get; set; }
        public GpsData Gps { get; set; }
        public NetworkData Network { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
