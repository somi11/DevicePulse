using DevicePulse.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Domain.Statuses
{
    public class GpsStatus
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        private double _previousLatitude { get; set; }
        private double? _previousLongitude { get; set; }

        public void Update(GpsData data)
        {
            _previousLatitude = Latitude;
            _previousLongitude = Longitude;

            Latitude = data.Latitude;
            Longitude = data.Longitude;
        }

        public bool PositionChanged ()
        {
            return _previousLatitude != Latitude || _previousLongitude != Longitude;
        }
    }
}
