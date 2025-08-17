using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Interfaces
{
    public interface IDomainEvent
    {
        Guid DeviceId { get; }
        DateTime OccurredAt { get; }

    }
}
