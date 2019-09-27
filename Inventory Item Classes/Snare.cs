using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class Snare: BaseInventoryItem
    {
        public string Diameter { get; set; }
        public string Dimensions { get; set; }
        public string SubTitle { get; set; }


        public Snare(string id, string brand, string title, string diameter, string dimensions, string cost, string refNumber, string category, string packSize)
            : base(id, brand, title, cost, refNumber, packSize, category)
        {
            //Derived Properties
            Diameter = diameter;
            Dimensions = dimensions;
        }
    }
}
