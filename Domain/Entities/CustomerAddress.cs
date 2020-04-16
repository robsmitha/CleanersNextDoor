using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class CustomerAddress : BaseEntity
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsDefault { get; set; }


        /// <summary>
        /// Overrides Equals() method to compare address location information
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            static bool AreEqual(string s1, string s2) => !string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2)
                ? s1.Trim().ToLower().CompareTo(s2.Trim().ToLower()) == 0
                : false;

            var compare = (CustomerAddress)obj;
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
}
