using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    //POCO Class With Login Token.
    public class Administrator : IPoco, IUser
    {
        public long Admin_Number { get; set; }
        public long Id { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }

        //Empty Constractor For Read From Sql.
        public Administrator()
        {
        }

        //Constractor Without Id For POCO Instance.
        public Administrator(string user_Name, string password)
        {
            User_Name = user_Name;
            Password = password;
        }

        //Full Constractor For Read From Data base.
        public Administrator(long admin_Number, long id, string user_Name, string password)
        {
            Admin_Number = admin_Number;
            Id = id;
            User_Name = user_Name;
            Password = password;
        }


        // Function To Change Password For This POCO.
        public void ChangePassword(string newPassword)
        {
            this.Password = newPassword;
        }
        // This Function Override The Real Operator == And Check If This.Id And Other.Id Are Equals.
        static public bool operator ==(Administrator me, Administrator other)
        {
            if (ReferenceEquals(me, other) || ReferenceEquals(me, null) && ReferenceEquals(other, null))
                return true;
            return false;
        }

        // This Function Override The Real Operator != And Check If This.Id And Other.Id Are NOT Equals.
        static public bool operator !=(Administrator me, Administrator other)
        {
            return !(me == other);
        }

        // This Function Override The Real Function Equals And Compair Between This.Id And Other.Id.
        public override bool Equals(object obj)
        {
            Administrator otherAdmin = obj as Administrator;
            return (this.Id == otherAdmin.Id);
        }

        // This Function Override The Real HashCode And Return this Id.
        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            return $"Admin Id: {Id}. User Name: {User_Name}.";
        }
    }
}
