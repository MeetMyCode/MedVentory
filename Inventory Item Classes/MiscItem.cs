using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{

    class MiscItem: BaseInventoryItem
    {
        public string SubTitle { get; set; }

        public MiscItem(string id, string brand, string title, string subTitle, string cost, string refNumber, string category, string packSize)
            : base(id, brand, title, cost, refNumber, packSize, category)
        {
            //Derived Properties
            SubTitle = subTitle;
        }
    }
}
