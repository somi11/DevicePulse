using DevicePulse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Statuses
{
    public class NetworkStatus
    {
        public bool IsOnline { get; set; }

        public void Update(NetworkData networkData)
        {
            IsOnline = networkData.IsOnline;
        }

    }
}
