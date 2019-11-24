using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    //POCO Class With Login Token.
    public class AirlineCompany : IPoco, IUser
    {
        public long Airline_Number { get; set; }
        public long Id { get; set; }
        public string Airline_Name { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public long Country_Code { get; set; }

        //Empty Constractor For Read From Sql.
        public AirlineCompany()
        {
        }

        //Constractor Without Id For POCO Instance.
        public AirlineCompany(string airline_Name, string user_Name, string password, int country_Code)
        {
            Airline_Name = airline_Name;
            User_Name = user_Name;
            Password = password;
            Country_Code = country_Code;
        }

        //Full Constractor For Read From Data Base.
        public AirlineCompany(long airline_Number, long id, string airline_Name, string user_Name, string password, long country_Code)
        {
            Airline_Number = airline_Number;
            Id = id;
            Airline_Name = airline_Name;
            User_Name = user_Name;
            Password = password;
            Country_Code = country_Code;
        }



        // Function To Change Password For This POCO.
        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }

        // This Function Override The Real Operator == And Check If This.Id And Other.Id Are Equals.
        static public bool operator ==(AirlineCompany me, AirlineCompany other)
        {
            if (ReferenceEquals(me, other) || ReferenceEquals(me, null) && ReferenceEquals(other, null))
                return true;
            return false;
        }

        // This Function Override The Real Operator != And Check If This.Id And Other.Id Are NOT Equals.
        static public bool operator !=(AirlineCompany me, AirlineCompany other)
        {
            return !(me == other);
        }

        // This Function Override The Real Function Equals And Compair Between This.Id And Other.Id.
        public override bool Equals(object obj)
        {
            AirlineCompany otherAirline = obj as AirlineCompany;
            return (this.Id == otherAirline.Id);
        }

        // This Function Override The Real HashCode And Return this Id.
        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            return $"Airline Name: {Airline_Name}. User Name: {User_Name}. Country Number: {Country_Code}.";
        }
    }
}
