using DevicePulse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Events
{
    public class FallDetectedDomainEvent : IDomainEvent
    {
        public Guid DeviceId { get; }
        public DateTime OccurredAt { get; } = DateTime.UtcNow;

        public double X { get; }
        public double Y { get;  }
        public double Z { get;  }

        public double Old_X { get;  }

        public double Old_Y { get; }

        public double Old_Z { get; }
        
        public FallDetectedDomainEvent(Guid deviceId, double x, double y, double z, double oldX, double oldY, double oldZ)
        {
            DeviceId = deviceId;
            X = x;
            Y = y;
            Z = z;
            Old_X = oldX;
            Old_Y = oldY;
            Old_Z = oldZ;
        }
    }
}
