using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class Dressing: BaseInventoryItem
    {

        public string SubTitle { get; set; }

        public Dressing(string id, string brand, string title, string cost, string refNumber, string category, string packSize)
            : base(id, brand, title, cost, refNumber, packSize, category){}


    }
}
