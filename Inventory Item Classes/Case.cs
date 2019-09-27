using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterventionalCostings.CustomViews;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class Case
    {

        public string PtNhsNumber { get; set; }
        public string PtFirstName { get; set; }
        public string PtSurname { get; set; }
        public string PtDob { get; set; }
        public string Referrer { get; set; }
        public DateTime CaseDate { get; set; }
        public string Procedure { get; set; }
        public string Location { get; set; }
        public string Radiologist { get; set; }
        public string CaseNotes { get; set; }
		public decimal CaseCost { get; set; }


		public List<SelectedItem> CaseItems { get; set; }


        public Case(string nhsNum, string fName, string sName, string dob, string referrer, DateTime date, string procedure, string location, string rad, string notes, List<SelectedItem> caseItems, decimal caseCost)
        {
            PtNhsNumber = nhsNum;
            PtFirstName = fName;
            PtSurname = sName;
            PtDob = dob;
            Referrer = referrer;
            CaseDate = date;
            Procedure = procedure;
            Location = location;
            Radiologist = rad;
            CaseNotes = notes;
            CaseItems = caseItems;
			CaseCost = caseCost;
        }


    }
}
