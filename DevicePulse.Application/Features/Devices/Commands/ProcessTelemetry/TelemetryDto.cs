using DevicePulse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry
{
    public class TelemetryDto
    {
        public AccelerationData Acceleration { get; set; }
        public BatteryData Battery { get; set; }
        public GpsData Gps { get; set; }
        public NetworkData Network { get; set; }
    }
}
