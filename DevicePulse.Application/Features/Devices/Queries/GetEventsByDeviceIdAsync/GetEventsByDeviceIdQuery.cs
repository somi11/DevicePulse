using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetEventsByDeviceIdAsync
{
    public class GetEventsByDeviceIdQuery : IRequest<IEnumerable<EventDto>>
    {
        public Guid DeviceId { get; }

        public GetEventsByDeviceIdQuery(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
   
}
