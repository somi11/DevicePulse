using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.BatteryChargingEvent
{
    public class BatteryChargingEvent : INotification
    {
        public Guid DeviceId { get; }

        public BatteryChargingEvent(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
