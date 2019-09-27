using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class CaseStatsData
    {
        public int ID { get; set; }
        public DateTime CaseDate { get; set; }
        public decimal CaseCost { get; set; }
        public string CaseName { get; set; }
        public string CaseRad { get; set; }

        public CaseStatsData(int id, string date, decimal cost, string caseName, string rad)
        {
            ID = id;
            CaseDate = DateTime.Parse(date);
            CaseCost = cost;
            CaseName = caseName;
            CaseRad = rad;

        }


    }
}
