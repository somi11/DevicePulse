using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetEventsByDeviceIdAsync
{
    public class EventDto
    {
        public Guid DeviceId { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    }
}
