using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class BaseAddress : BaseEntity
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>
        /// Overrides Equals() method to compare address location information
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            static bool AreEqual(string s1, string s2)
            {
                if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2)) return true;
                if (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2)) return s1.Trim().ToLower().CompareTo(s2.Trim().ToLower()) == 0;
                return false;
            }

            var compare = (BaseAddress)obj;
            return AreEqual(Street1, compare.Street1)
                && AreEqual(Street2, compare.Street2)
                && AreEqual(City, compare.City)
                && AreEqual(StateAbbreviation, compare.StateAbbreviation)
                && AreEqual(Zip, compare.Zip);
        }

        /// <summary>
        /// If your overridden Equals method returns true when two objects are tested for equality, 
        /// your overridden GetHashCode method must return the same value for the two objects.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (Street1 is string ? Street1.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (Street2 is string ? Street2.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (City is string ? City.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (StateAbbreviation is string ? StateAbbreviation.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (Zip is string ? Zip.GetHashCode() : 0);
                return hash;
            }
        }
    }
    public static class LocationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="mi"></param>
        /// <returns></returns>
        public static bool WithinMiles(this BaseAddress @this, double? lat1, double? lon1, double mi)
        {
            if (lat1 == null || lon1 == null || mi == double.MaxValue) return true;
            return @this.HaversineDistance(lat1.Value, lon1.Value) <= mi;
        }

        /// <summary>
        /// Use Haversine Distance formula to calculate a distance between two points
        /// </summary>
        /// <param name="this"></param>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <returns></returns>
        public static double HaversineDistance(this BaseAddress @this, double lat1, double lon1)
        {
            var lat2 = @this.Latitude;
            var lon2 = @this.Longitude;
            var R = 3960;   //miles
            static double ToRadians(double angle) => Math.PI * angle / 180.0;
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Asin(Math.Sqrt(a));
            return R * c;

        }
    }
}
