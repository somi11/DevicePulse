using DevicePulse.Domain.ValueObjects;

namespace DevicePulse.Domain.Statuses
{
    public class GpsStatus
    {
        // Current values
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        // Previous values (nullable so first update is handled safely)
        public double? PreviousLatitude { get; private set; }
        public double? PreviousLongitude { get; private set; }

        // Update current values, store previous
        public void Update(GpsData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            PreviousLatitude = Latitude;
            PreviousLongitude = Longitude;

            Latitude = data.Latitude;
            Longitude = data.Longitude;
        }

        // Detect change
        public bool PositionChanged()
        {
            // First reading, no previous to compare
            if (!PreviousLatitude.HasValue || !PreviousLongitude.HasValue)
                return false;

            // Ignore if both old and new values are zero
            if (PreviousLatitude == 0 && PreviousLongitude == 0 )
                return false;
            if (Latitude == 0 && Longitude == 0)
                return false;

            return PreviousLatitude != Latitude || PreviousLongitude != Longitude;
        }
    }
}
