using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    //POCO Class.
    public class Country : IPoco
    {
        public long Id { get; set; }
        public string Country_Name { get; set; }

        //Empty Constractor For Read From Sql.
        public Country()
        {
        }

        //Constractor Without Id For POCO Instance.
        public Country(string country_Name)
        {
            Country_Name = country_Name;
        }


        // This Function Override The Real Operator == And Check If This.Id And Other.Id Are Equals.
        static public bool operator ==(Country me, Country other)
        {
            if (ReferenceEquals(me, other) || ReferenceEquals(me, null) && ReferenceEquals(other, null))
                return true;
            return false;
        }

        // This Function Override The Real Operator != And Check If This.Id And Other.Id Are NOT Equals.
        static public bool operator !=(Country me, Country other)
        {
            return !(me == other);
        }

        // This Function Override The Real Function Equals And Compair Between This.Id And Other.Id.
        public override bool Equals(object obj)
        {
            Country otherCountry = obj as Country;
            return (this.Id == otherCountry.Id);
        }

        // This Function Override The Real HashCode And Return this Id.
        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            return $"Country Number: {Id}. Name: {Country_Name}.";
        }
    }
}
