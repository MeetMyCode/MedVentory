using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{

        
    class Dilator: BaseInventoryItem
    {

        //Inherits properties ID, Category, Brand, TItle, Cost, RefNumber.
        public string Diameter { get; set; }
        public string Dimensions { get; set; }


        public Dilator(string id, string category, string brand, string title, string cost, string refNumber, string diameter, string dimensions, string packSize)
            : base(id, brand, title, cost, refNumber, packSize, category)
        {
            //Derived Properties
            Diameter = diameter;
            Dimensions = dimensions;
        }

    }
}
