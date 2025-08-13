using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry
{
    public class ProcessTelemetryCommand : IRequest<Unit>
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        public TelemetryDto Telemetry { get; set; }

        public ProcessTelemetryCommand (Guid deviceId, String deviceName, TelemetryDto telemetry)
        {
            if (deviceId == Guid.Empty)
                throw new ArgumentException("DeviceId cannot be empty.", nameof(deviceId));
            DeviceId = deviceId;
            DeviceName = deviceName;
            Telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
        }
    }
}
