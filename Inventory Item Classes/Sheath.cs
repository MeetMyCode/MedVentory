using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class Sheath : BaseInventoryItem
    {

        public string Diameter { get; set; }
        public string Dimensions { get; set; }

        //constructor
        public Sheath(string id, string brand, string title, string cost, string category, string refNumber, string packSize, string diameter, string dimensions)
            : base(id, brand, title, cost, refNumber, packSize, category)
        {
            //Derived Properties
            Diameter = diameter;
            Dimensions = dimensions;
        }


    }

}
