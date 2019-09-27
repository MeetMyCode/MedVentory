using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterventionalCostings.Inventory_Item_Classes
{
    class ItemToOrder
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public string RefNumber { get; set; }
        public int QuantityUsed { get; set; }
        public int PackSize { get; set; }
        public decimal Cost { get; set; }
        public string Category { get; set; }



        public ItemToOrder(int id, string description, string refNumber, int quantityUsed, int packSize, decimal cost, string category)
        {
            ID = id;
            Description = description;
            RefNumber = refNumber;
            QuantityUsed = quantityUsed;
            PackSize = packSize;
            Cost = cost;
            Category = category;
        }
    }
}
