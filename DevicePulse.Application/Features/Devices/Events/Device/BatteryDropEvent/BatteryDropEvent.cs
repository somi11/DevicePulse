using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.BatteryDropEvent
{
    public class BatteryDropEvent : INotification
    {
        public Guid DeviceId { get; }

        public BatteryDropEvent(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
