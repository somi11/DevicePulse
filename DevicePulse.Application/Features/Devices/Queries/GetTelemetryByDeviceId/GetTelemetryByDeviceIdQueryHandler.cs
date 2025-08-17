using DevicePulse.Application.Contracts;
using DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry;
using DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetTelemetryByDeviceId
{
    public class GetTelemetryByDeviceIdQueryHandler : IRequestHandler<GetTelemetryByDeviceIdQuery, IEnumerable<TelemetryDto>>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<GetTelemetryByDeviceIdQueryHandler> _logger;

        public GetTelemetryByDeviceIdQueryHandler(
            IDeviceRepository deviceRepository,
            ILogger<GetTelemetryByDeviceIdQueryHandler> logger)
        {
            _deviceRepository = deviceRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TelemetryDto>> Handle(GetTelemetryByDeviceIdQuery request, CancellationToken cancellationToken)
        {
            var telemetryData = await _deviceRepository.
                GetTelemetryByDeviceIdAsync(request.DeviceId, cancellationToken);

           
            return telemetryData.Select(td => new TelemetryDto
            {
                Acceleration = td.Acceleration,
                Battery = td.Battery,
                Gps = td.Gps
            });
        }

    }
}
