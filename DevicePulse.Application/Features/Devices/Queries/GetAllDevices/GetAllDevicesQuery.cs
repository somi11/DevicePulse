using DevicePulse.Application.Features.Devices.Queries.GetDeviceByIdQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Queries.GetAllDevices
{
    public class GetAllDevicesQuery : IRequest<IEnumerable<DeviceDto>>
    {

    }
}
