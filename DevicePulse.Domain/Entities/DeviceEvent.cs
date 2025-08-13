using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Entities
{
    public class DeviceEvent
    {
        public Guid DeviceId { get; set; }
        public string EventType { get; set; }    
        public string Description { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public DeviceEvent(string eventType, string description)
        {
            EventType = eventType;
            Description = description;
        }

        public DeviceEvent() { } // For EF
    }
}
