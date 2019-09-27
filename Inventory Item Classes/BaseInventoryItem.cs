using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class BaseInventoryItem
    {
        public string ID { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public string Cost { get; set; }
        public string RefNumber { get; set; }
        public string PackSize { get; set; }
        public string Category { get; set; }

        public BaseInventoryItem(string id, string brand, string title, string cost, string refNumber, string packSize, string category)
        {
            ID = id;
            Brand = brand;
            Title = title;
            Cost = cost;
            RefNumber = refNumber;
            PackSize = packSize;
            Category = category;
        }

        //this constructor is for contrast
        public BaseInventoryItem(string id, string brand, string cost, string refNumber, string packSize, string category)
        {
            ID = id;
            Brand = brand;
            Cost = cost;
            RefNumber = refNumber;
            PackSize = packSize;
            Category = category;
        }

        //This constructor is for testWires
        public BaseInventoryItem(string id, string brand, string title, string cost, string refNumber)
        {
            ID = id;
            Brand = brand;
            Cost = cost;
            RefNumber = refNumber;

        }
    }
}
