using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings
{
    public class Referrer
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Specialty { get; set; }


        public Referrer(int id, string prefix, string fName, string sName, string specialty)
        {
            Id = id;
            Prefix = prefix;
            FirstName = fName;
            Surname = sName;
            Specialty = specialty;
        }
    }






    
    




}
