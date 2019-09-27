using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings
{
    class Radiologist
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Specialty { get; set; }

        public Radiologist(int id, string prefix, string fName, string sName, string specialty)
        {
            Id = id;
            Prefix = prefix;
            FirstName = fName;
            Surname = sName;
            Specialty = specialty;
        }

    }
}
