using InterventionalCostings.Inventory_Item_Classes;
using InterventionalCostings.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterventionalCostings.CustomViews
{
    /// <summary>
    /// Interaction logic for SelectedItem.xaml
    /// </summary>
    /// 

   
    public interface ICalculateTotalSelectedItemsCost
    {
        void CalculateCostOfAllSelectedItems();
    }

    public interface IDeleteSelectedItem
    {
        void DeleteSelectedItem(string itemRefNumber);
    }

    public partial class SelectedItem : UserControl
    {
        public string RefNumber { get; set; }
        public string Category { get; set; }
        public int PackSize { get; set; }
        public decimal Cost { get; set; }



        //Reference to the NewCase Window
        public NewCasePage NewCaseWindowReference;
        SelectedItemImageWindow selectedItemImageWindow;

        public SelectedItem()
        {
            InitializeComponent();

        }

        private void CalculateCostOfAllSelectedItems()
        {
            ICalculateTotalSelectedItemsCost calculateTotalSelectedItemsCostDelegate = NewCaseWindowReference;
            calculateTotalSelectedItemsCostDelegate.CalculateCostOfAllSelectedItems();

        }


        public decimal CalculateItemBaseCost()
        {
            decimal cost = Decimal.Parse(SelectedItemCost.Content.ToString());
            int itemCount = Int32.Parse(ItemCount.Content.ToString());
            decimal baseCost = cost / itemCount;

            return baseCost;
        }

        private void ParseItemCount(string itemCountString, object sender)
        {
            Button button = (Button)sender;
            int numberFromItemCount = 1;
            decimal baseCost = CalculateItemBaseCost();

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

            ParseItemCost(numberFromItemCount, baseCost);
            ItemCount.Content = numberFromItemCount.ToString();
        }

        private void ParseItemCost(int itemCount, decimal baseCost)
        {
            decimal costOfItems = baseCost * itemCount;

            SelectedItemCost.Content = costOfItems.ToString();

            CalculateCostOfAllSelectedItems();
        }

        private void ParseItemCountAndCost(object sender, RoutedEventArgs e)
        {
            string itemCountString = ItemCount.Content.ToString();
            ParseItemCount(itemCountString, sender);
        }

        private void DeleteSelectedItem(object sender, RoutedEventArgs e)
        {
            Button itemBtn = (Button)sender;
            IDeleteSelectedItem DeleteSelectedItem = NewCaseWindowReference;

            var tag = itemBtn.Tag.ToString();
            DeleteSelectedItem.DeleteSelectedItem(tag);

        }

        //Display a product image window on mouse over.
        private void ShowImage(object sender, MouseButtonEventArgs e)
        {
            selectedItemImageWindow = new SelectedItemImageWindow(this);
            selectedItemImageWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            selectedItemImageWindow.ShowDialog();
        }

        //Remove product image window on mouse exit.
        private void CloseImageWindowOnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            selectedItemImageWindow.Close();
            selectedItemImageWindow = null;
        }
    }//END OF CLASS.




}
