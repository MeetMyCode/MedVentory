using InterventionalCostings.Inventory_Item_Classes;
using InterventionalCostings.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterventionalCostings.CustomViews
{
    /// <summary>
    /// Interaction logic for ItemToOrder.xaml
    /// </summary>
    /// 
    public interface ICalculateTotalCostOfMarkedItems
    {
        void CalculateCostOfMarkedItems();
    }

    public partial class CustomItemToOrder : UserControl
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string RefNumber { get; set; }
        public int QuantityUsed { get; set; }
        public int PackSize { get; set; }
        public decimal Cost { get; set; }
        public string Category { get; set; }

        private CustomItemToOrderImageWindow customItemToOrderImageWindow;
        public OrdersPage OrdersPageReference;

        public CustomItemToOrder(int id, string description, string refNumber, int quantityUsed, int packSize, decimal cost, string category)
        {
            InitializeComponent();

            ID = id;
            Description = description;
            RefNumber = refNumber;
            QuantityUsed = quantityUsed;
            PackSize = packSize;
            Cost = cost;
            Category = category;

            PopulateLabels();
        }

        private void PopulateLabels()
        {
            OrderItemDescription.Content = Description;
            OrderItemRefNumber.Content = RefNumber;
            OrderItemQuantityUsed.Content = QuantityUsed;
            UnitPrice.Content = string.Format("at £{0}/Pack", Cost);
            OrderItemPackSize.Content = string.Format("{0}/Pack", PackSize);
            ItemCountToOrder.Content = QuantityUsed;
            OrderCost.Content = QuantityUsed * Cost;
        }


        //Display a product image window on mouse over.
        private void ShowImage(object sender, MouseEventArgs e)
        {
            customItemToOrderImageWindow = new CustomItemToOrderImageWindow(this);
            customItemToOrderImageWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            customItemToOrderImageWindow.ShowDialog();
        }

        //Remove product image window on mouse exit.
        private void CloseImageWindowOnMouseLeave(object sender, MouseEventArgs e)
        {
            customItemToOrderImageWindow.Close();
            customItemToOrderImageWindow = null;
        }

        private void ParseItemCount(string itemCountString, object sender)
        {
            Button button = (Button)sender;
            int numberFromItemCount = Int32.Parse(ItemCountToOrder.Content.ToString());

            switch (button.Name.ToString())
            {
                case "IncrementItemCountButton":
                    numberFromItemCount = Int32.Parse(itemCountString);
                    numberFromItemCount += 1;
                    break;
                case "DecrementItemCountButton":
                    numberFromItemCount = Int32.Parse(itemCountString);
                    numberFromItemCount -= 1;

                    if (numberFromItemCount < 1)
                    {
                        numberFromItemCount = 1;
                    }

                    break;
                default:

                    Console.WriteLine("No recognised incrementing/decrementing button name supplied.");
                    break;
            }

            ItemCountToOrder.Content = numberFromItemCount.ToString();
            ParseItemCost(numberFromItemCount);

        }

        private void ParseItemCost(int itemCount)
        {
            decimal costOfItems = Cost * itemCount;
            OrderCost.Content = costOfItems.ToString();

            CalculateCostOfAllMarkedItems();
        }

        private void ParseItemCountAndCost(object sender, RoutedEventArgs e)
        {
            string itemCountString = ItemCountToOrder.Content.ToString();
            ParseItemCount(itemCountString, sender);
        }

        private void ToggleTickCross(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;
            string imageSource = image.Source.ToString();

            if (imageSource == @"pack://application:,,,/Images/GreenTick.png")
            {
                image.Source = new BitmapImage(new Uri("/Images/RedCross.png", UriKind.Relative));

                //Recalculate total cost to account for any change in RedCross.png vs GreenTick.png.
                CalculateCostOfAllMarkedItems();
            }
            else
            {
                image.Source = new BitmapImage(new Uri("/Images/GreenTick.png", UriKind.Relative));

                //Recalculate total cost to account for any change in RedCross.png vs GreenTick.png.
                CalculateCostOfAllMarkedItems();
            }
        }

        public void CalculateCostOfAllMarkedItems()
        {
            ICalculateTotalCostOfMarkedItems calculateTotalSelectedItemsCostDelegate = OrdersPageReference;
            calculateTotalSelectedItemsCostDelegate.CalculateCostOfMarkedItems();
        }

	}//END OF CLASS
}
