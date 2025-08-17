using DevicePulse.Application.Contracts;
using DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetEventsByDeviceIdAsync
{
    public class GetEventsByDeviceIdQueryHandler : IRequestHandler<GetEventsByDeviceIdQuery, IEnumerable<EventDto>>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<GetEventsByDeviceIdQueryHandler> _logger;

        public GetEventsByDeviceIdQueryHandler(
            IDeviceRepository deviceRepository,
            ILogger<GetEventsByDeviceIdQueryHandler> logger)
        {
            _deviceRepository = deviceRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<EventDto>> Handle(GetEventsByDeviceIdQuery request, CancellationToken cancellationToken)
        {
            var events = await _deviceRepository.GetEventsByDeviceIdAsync(request.DeviceId, cancellationToken);

            if (events == null || !events.Any())
            {
                _logger.LogWarning("[GetEventsByDeviceIdQueryHandler] : No events found for Device ID {DeviceId}.", request.DeviceId);
                return Enumerable.Empty<EventDto>();
            }

            return events.Select(e => new EventDto
            {
                DeviceId = e.DeviceId,
                EventType = e.EventType,
                Description = e.Description,
                OccurredAt = e.OccurredAt
            });
        }
    }
}
