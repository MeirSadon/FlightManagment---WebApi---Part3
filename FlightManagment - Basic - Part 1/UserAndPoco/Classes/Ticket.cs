using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    //POCO Class.
    public class Ticket : IPoco
    {
        public long Id { get; set; }
        public long Flight_Id { get; set; }
        public long Customer_Id { get; set; }

        //Empty Constractor For Read From SQL.
        public Ticket()
        {
        }

        //Constractor Without Id For POCO Instance.
        public Ticket(long flight_Id, long customer_Id)
        {
            Flight_Id = flight_Id;
            Customer_Id = customer_Id;
        }

        // This Function Override The Real Operator == And Check If This.Id And Other.Id Are Equals.
        static public bool operator ==(Ticket me, Ticket other)
        {
            if (ReferenceEquals(me, other) || ReferenceEquals(me, null) && ReferenceEquals(other, null))
                return true;
            return false;
        }

        // This Function Override The Real Operator != And Check If This.Id And Other.Id Are NOT Equals.
        static public bool operator !=(Ticket me, Ticket other)
        {
            return !(me == other);
        }

        // This Function Override The Real Function Equals And Compair Between This.Id And Other.Id.
        public override bool Equals(object obj)
        {
            Ticket otherTicket = obj as Ticket;
            return (this.Id == otherTicket.Id);
        }

        // This Function Override The Real HashCode And Return this Id.
        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            Customer customer = new LoggedInAdministratorFacade().GetCustomerById(FlyingCenterConfig.basicToken, (int)Customer_Id);
            Flight flight = new LoggedInAdministratorFacade().GetFlightById((int)Flight_Id);
            return $"Buyer: {customer}. Flight: {flight}";
        }
    }
}
