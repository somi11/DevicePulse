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

        public void Update(AccelerationData data)
        {
            X = data.X;
            Y = data.Y;
            Z = data.Z;
        }

        public bool IsFallDetected() =>
        Math.Sqrt(X * X + Y * Y + Z * Z) > 25.0;
    }
}
