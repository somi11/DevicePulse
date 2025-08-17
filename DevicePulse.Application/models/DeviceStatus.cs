using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.models
{
    public class DeviceStatus
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        public DateTime LastSeen { get; set; }
        public string Status { get; set; } // e.g., "Online", "Offline", "Charging"
        public double BatteryLevel { get; set; } // Percentage
        public string Location { get; set; } // e.g., "37.7749° N, 122.4194° W"
        public bool IsCharging { get; set; }
        public bool IsMoving { get; set; }
        public bool FallDetected { get; set; }
    }
}
