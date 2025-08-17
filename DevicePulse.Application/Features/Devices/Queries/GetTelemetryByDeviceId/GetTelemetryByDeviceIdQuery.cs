using DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetTelemetryByDeviceId
{
    public class GetTelemetryByDeviceIdQuery : IRequest<IEnumerable<TelemetryDto>>
    {
        public Guid DeviceId { get; }

        public GetTelemetryByDeviceIdQuery(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
    

}
