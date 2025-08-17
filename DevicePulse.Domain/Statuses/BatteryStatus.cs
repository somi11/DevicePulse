using DevicePulse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Statuses
{
    public class BatteryStatus
    {
        public double Level { get; private set; }
        public double _previousLevel;

        public void Update(BatteryData data)
        {
            _previousLevel = Level;
            Level = data.Level;

        }

        public bool HasDroppedByAtLeastOnePercent()
        {

            if (_previousLevel <= 0) return false;

            return (_previousLevel - Level) >= 1.0;
        }

        public bool IsCharging()
        {
            
            if (_previousLevel <= 0) return false;
            return Level > _previousLevel;
        }

    }

}
