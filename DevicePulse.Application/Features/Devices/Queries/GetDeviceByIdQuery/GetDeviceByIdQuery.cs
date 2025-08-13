using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery
{
    public class GetDeviceByIdQuery : IRequest<DeviceDto>
    {
        public Guid DeviceId { get; }

        public GetDeviceByIdQuery(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
