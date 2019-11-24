using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    //POCO Class With Login Token.
    public class Customer : IPoco, IUser
    {
        public long Customer_Number { get; set; }
        public long Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone_No { get; set; }
        public string Credit_Card_Number { get; set; }

        //Empty Constractor For Read From Sql.
        public Customer()
        {
        }

        //Constractor Without Id For POCO Instance.
        public Customer(string first_Name, string last_Name, string user_Name, string password, string address, string phone_No, string credit_Card_Number)
        {
            First_Name = first_Name;
            Last_Name = last_Name;
            User_Name = user_Name;
            Password = password;
            Address = address;
            Phone_No = phone_No;
            Credit_Card_Number = credit_Card_Number;
        }

        //Full Constractor For Read From Data Base.
        public Customer(long customer_Number, long id, string first_Name, string last_Name, string user_Name, string password, string address, string phone_No, string credit_Card_Number)
        {
            Customer_Number = customer_Number;
            Id = id;
            First_Name = first_Name;
            Last_Name = last_Name;
            User_Name = user_Name;
            Password = password;
            Address = address;
            Phone_No = phone_No;
            Credit_Card_Number = credit_Card_Number;
        }



        // Function To Change Password For This POCO.
        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }
        // This Function Override The Real Operator == And Check If This.Id And Other.Id Are Equals.
        static public bool operator ==(Customer me, Customer other)
        {
            if (ReferenceEquals(me, other) || ReferenceEquals(me, null) && ReferenceEquals(other, null))
                return true;
            return false;
        }

        // This Function Override The Real Operator != And Check If This.Id And Other.Id Are NOT Equals.
        static public bool operator !=(Customer me, Customer other)
        {
            return !(me == other);
        }

        // This Function Override The Real Function Equals And Compair Between This.Id And Other.Id.
        public override bool Equals(object obj)
        {
            Customer otherCustomer = obj as Customer;
            return (this.Id == otherCustomer.Id);
        }

        // This Function Override The Real HashCode And Return this Id.
        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            return $"Customer Id: {Id}. Full Name: {First_Name} {Last_Name}. User Name: {User_Name}. Address: {Address}. Phone Number: {Phone_No}. Credit Card Number: {Credit_Card_Number}.";
        }
    }
}
