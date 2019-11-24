using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    //POCO Class.
    public class Flight : IPoco
    {
        public long Id { get; set; }
        public long AirlineCompany_Id { get; set; }
        public long Origin_Country_Code { get; set; }
        public long Destination_Country_Code { get; set; }
        private DateTime _departure_Time;
        private DateTime _landing_Time;

        public DateTime Departure_Time
        {
            get
            {
                return _departure_Time;
            }
            set
            {
                if (_departure_Time != DateTime.MinValue && _departure_Time >= _landing_Time) throw new DepartureTimeTooLateException("Departue Time Must Be Earlier Than Landing Time");
                _departure_Time = value;
            }
        }
        public DateTime Landing_Time
        {
            get
            {
                return _landing_Time;
            }
            set
            {
                if (_landing_Time != DateTime.MinValue && _departure_Time >= _landing_Time) throw new DepartureTimeTooLateException("Departue Time Must Be Earlier Than Landing Time");
                _landing_Time = value;

            }
        }
        public int Remaining_Tickets { get; set; }

        //Empty Constractor For Read From Sql.
        public Flight()
        {
        }

        //Constractor Without Id For POCO Instance .
        public Flight(long airLineCompany_Id, long origin_Country_Code, long destination_Country_Code, DateTime departure_Time, DateTime landing_Time, int remaining_Tickets)
        {
            AirlineCompany_Id = airLineCompany_Id;
            Origin_Country_Code = origin_Country_Code;
            Destination_Country_Code = destination_Country_Code;
            if (departure_Time >= landing_Time)
                throw new DepartureTimeTooLateException("Departue Time Must Be Earlier Than Landing Time");
            _departure_Time = departure_Time;
            _landing_Time = landing_Time;
            if (remaining_Tickets < 1)
                throw new ArgumentOutOfRangeException("It's Impossible To Create Flight Without Tickets");
            Remaining_Tickets = remaining_Tickets;
        }

        // This Function Override The Real Operator == And Check If This.Id And Other.Id Are Equals.
        static public bool operator ==(Flight me, Flight other)
        {
            if (ReferenceEquals(me, other) || ReferenceEquals(me, null) && ReferenceEquals(other, null))
                return true;
            return false;
        }

        // This Function Override The Real Operator != And Check If This.Id And Other.Id Are NOT Equals.
        static public bool operator !=(Flight me, Flight other)
        {
            return !(me == other);
        }

        // This Function Override The Real Function Equals And Compair Between This.Id And Other.Id.
        public override bool Equals(object obj)
        {
            Flight otherFlight = obj as Flight;
            return (this.Id == otherFlight.Id);
        }

        // This Function Override The Real HashCode And Return this Id.
        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            return $"Flight Id: {Id}. Belong To Company Number: {AirlineCompany_Id}. From: {Origin_Country_Code}. To: {Destination_Country_Code}. Departure Time: {Departure_Time}. Landing Time: {Landing_Time}. Remaining Tickets: {Remaining_Tickets}.";
        }
    }
}
