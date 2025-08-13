using DevicePulse.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery
{
    public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, DeviceDto>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<GetDeviceByIdQueryHandler> _logger;

        public GetDeviceByIdQueryHandler(
            IDeviceRepository deviceRepository,
            ILogger<GetDeviceByIdQueryHandler> logger)
        {
            _deviceRepository = deviceRepository;
            _logger = logger;
        }

        public async Task<DeviceDto> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.GetByDeviceIdAsync(request.DeviceId);

            if (device == null)
            {
                _logger.LogWarning("[GetDeviceByIdQueryHandler] : Device with ID {DeviceId} not found.", request.DeviceId);
                return null;
            }

            var lastReading = device.TelemetryReadings
                                    .OrderByDescending(t => t.Timestamp)
                                    .FirstOrDefault();


            return new DeviceDto
            {
                DeviceId = device.DeviceId,
                Name = device.Name,
                Acceleration = lastReading?.Acceleration,
                Battery = lastReading?.Battery,
                Gps = lastReading?.Gps,
                Network = lastReading?.Network,
                LastUpdated = lastReading?.Timestamp ?? DateTime.MinValue
            };
        }
    }
}
