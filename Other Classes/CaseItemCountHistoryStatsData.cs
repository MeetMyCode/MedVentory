using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class CaseItemCountHistoryStatsData
    {
        public int ID { get; set; }
        public string CaseDate { get; set; }
        public string CaseName { get; set; }
        public string ItemDescription { get; set; }
        public int QuantityUsed { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }




        //constructor
        public CaseItemCountHistoryStatsData(int id, string caseDate, string caseName, string itemDescription, int quantityUsed, decimal unitCost, decimal totalCost)
        {
            ID = id;
            CaseDate = caseDate;
            CaseName = caseName;
            ItemDescription = itemDescription;
            QuantityUsed = quantityUsed;
            UnitCost = unitCost;
            TotalCost = totalCost;
        }


    }
}
