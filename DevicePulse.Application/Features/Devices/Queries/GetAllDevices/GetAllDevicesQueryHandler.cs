using DevicePulse.Application.Contracts;
using DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetAllDevices
{
    public class GetAllDevicesQueryHandler : IRequestHandler<GetAllDevicesQuery, IEnumerable<DeviceDto>>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<GetAllDevicesQueryHandler> _logger;

        public GetAllDevicesQueryHandler(IDeviceRepository deviceRepository, ILogger<GetAllDevicesQueryHandler> logger)
        {
            _deviceRepository = deviceRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<DeviceDto>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
        {
            var devices = await _deviceRepository.GetAllAsync(); // assuming repo method returns List<Device>

            return devices.Select(d =>
            {
                var lastReading = d.TelemetryReadings.OrderByDescending(t => t.Timestamp).FirstOrDefault();
                return new DeviceDto
                {
                    DeviceId = d.DeviceId,
                    Name = d.Name,
                    Acceleration = lastReading?.Acceleration,
                    Battery = lastReading?.Battery,
                    Gps = lastReading?.Gps,
                    LastUpdated = lastReading?.Timestamp ?? default
                };
            }).ToList();
        }
    }
}
