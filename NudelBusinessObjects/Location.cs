using MongoDB.Bson.Serialization.Attributes;

namespace Nudel.BusinessObjects
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static bool operator==(Location location1, Location location2) {
            return location1.Equals(location2);
        }

        public static bool operator !=(Location location1, Location location2)
        {
            return !location1.Equals(location2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
            {
                return false;
            }

            var location = (Location)obj;
            return Latitude == location.Latitude &&
                   Longitude == location.Longitude;
        }

        public override int GetHashCode()
        {
            var hashCode = -1416534245;
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            return hashCode;
        }
    }
}
