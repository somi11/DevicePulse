using DevicePulse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Statuses
{
    public class AccelerationStatus
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public double Old_X { get; private set; }

        public double Old_Y { get; private set; }

        public double Old_Z { get; private set; }

        public void Update(AccelerationData data)
        {
            Old_X = X;
            Old_Y = Y;
            Old_Z = Z;
            
            
            X = data.X;
            Y = data.Y;
            Z = data.Z;
        }

        public bool IsFallDetected()
        {
            
            if (X == 0 && Y == 0 && Z == 0)
                return false;

            return Math.Sqrt(X * X + Y * Y + Z * Z) > 12.0;
        }
    }
}
