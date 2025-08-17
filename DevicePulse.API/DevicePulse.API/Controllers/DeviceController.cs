using DevicePulse.Application.Features.Devices.Queries.GetAllDevices;
using DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery;
using DevicePulse.Application.Features.Devices.Queries.GetEventsByDeviceIdAsync;
using DevicePulse.Application.Features.Devices.Queries.GetTelemetryByDeviceId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevicePulse.API.Controllers
{
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeviceController> _logger;
        public DeviceController(IMediator mediator, ILogger<DeviceController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            var query = new GetAllDevicesQuery();
            var devices = await _mediator.Send(query);

            if (devices == null || !devices.Any())
            {
                _logger.LogWarning("[DevicesController] : No devices found.");
                return NotFound(new { Message = "No devices found." });
            }

            return Ok(devices);
        }

        [HttpGet("{deviceId:guid}")]
        public async Task<IActionResult> GetDeviceById(Guid deviceId)
        {
            var query = new GetDeviceByIdQuery(deviceId);
            var deviceDto = await _mediator.Send(query);

            if (deviceDto == null)
            {
                _logger.LogWarning("[DevicesController] : Device with ID {DeviceId} not found.", deviceId);
                return NotFound(new { Message = $"Device with ID {deviceId} not found." });
            }

            return Ok(deviceDto);
        }

        [HttpGet("Telemetry/{deviceId:guid}")]
        public async Task<IActionResult> GetTelemetryByDeviceId(Guid deviceId)
        {
            var query = new GetTelemetryByDeviceIdQuery(deviceId);
            var telemetryData = await _mediator.Send(query);

            if (telemetryData == null || !telemetryData.Any())
            {
                _logger.LogWarning("[DevicesController] : No telemetry data found for device ID {DeviceId}.", deviceId);
                return NotFound(new { Message = $"No telemetry data found for device ID {deviceId}." });
            }

            return Ok(telemetryData);
        }

        [HttpGet("Events/{deviceId:guid}")]
        public async Task<IActionResult> GetEventsByDeviceId(Guid deviceId)
        {
            var query = new GetEventsByDeviceIdQuery(deviceId);
            var events = await _mediator.Send(query);

            if (events == null || !events.Any())
            {
                _logger.LogWarning("[DevicesController] : No events found for device ID {DeviceId}.", deviceId);
                return NotFound(new { Message = $"No events found for device ID {deviceId}." });
            }

            return Ok(events);
        }
    }
}
