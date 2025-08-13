using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.FallDetectedEvent
{
    public class FallDetectedEvent : INotification
    {
        public Guid DeviceId { get; }

        public FallDetectedEvent(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
