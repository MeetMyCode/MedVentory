using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings
{
    class IrNurse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }

        public IrNurse(int id, string userName, string email, string firstName, string surname)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            Surname = surname;
        }

    }
}
