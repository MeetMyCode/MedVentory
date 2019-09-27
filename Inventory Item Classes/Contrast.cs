using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class Contrast: BaseInventoryItem
    {
        public string Strength { get; set; }
        public string Volume { get; set; }


        public Contrast(string id, string brand, string strength, string volume, string cost, string refNumber, string category, string packSize)
            : base(id, brand, cost, refNumber, packSize, category)
        {          
            //Derived Properties
            Strength = strength;
            Volume = volume;
        }
    }
}
