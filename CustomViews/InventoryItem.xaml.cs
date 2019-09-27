using InterventionalCostings.Pages;
using InterventionalCostings;
using System;
using System.Windows;
using System.Windows.Controls;
using InterventionalCostings.Inventory_Item_Classes;
using System.Windows.Input;

namespace InterventionalCostings.CustomViews
{
    public interface IUpdateSelectedItemsList
    {
        void UpdateSelectedItemsList(StackPanel StackOfSelectedItems);
    }


    /// <summary>
    /// Interaction logic for InventoryItem.xaml
    /// </summary>
    public partial class InventoryItem : UserControl
    {
        public string ID { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Dimensions { get; set; }
        public string Diameter { get; set; }
        public string RefNumber { get; set; }
        public decimal Cost { get; set; }
        public string Strength { get; set; }
        public string Category { get; set; }
        public string Volume { get; set; }
        public int PackSize { get; set; }



        public StackPanel StackOfSelectedItems;
        private SelectedItem selectedItem;
        public IUpdateSelectedItemsList IUpdateSelectedItemsList;
        public ICalculateTotalSelectedItemsCost ICalculateTotalSelectedItemsCost;
        public NewCasePage NewCaseWindowReference;
        private SelectedItem matchedChildItem;

        private InventoryItemImageWindow inventoryItemImageWindow;

        public InventoryItem()
        {
            InitializeComponent();
        }

        //Add an item to the selected items list or update it if it is already selected.
        private void ItemWasSelected(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Button && ((Button)e.OriginalSource).Name == "InventoryItemPlusButton")
            {
                CommonAddSelectedItem((InventoryItem)sender, e);
            }
        }

        //Adds a new item to the selected items list.
        private void AddItemToSelectedItemsList(object sender)
        {
            InventoryItem item = (InventoryItem)sender;

            switch (item.Category)
            {
                case "Dilators":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Diameter + " " + item.Brand + " " + item.Category  + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;


					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Stent":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Diameter + " " + item.Brand + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Sheath":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Catheter":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Balloon":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Diameter + " " + item.Brand + " " + item.Dimensions + " " + item.Title + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Coils":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					selectedItem.Category = item.Category;
                    StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Contrast":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " +  item.Strength + " " + item.Volume + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Dressings":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " + item.Title + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					selectedItem.Category = item.Category;
                    StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Embolisation System":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " + item.Title + " " + item.Subtitle + " " + item.Category + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Snares":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Wires":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Diameter + " " + item.Dimensions + " " + item.Brand + " " + item.Title + " " + item.Subtitle + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                case "Misc":
                    GenericAddItem(item);
                    selectedItem.SelectedItemDescription.Content = item.Brand + " " + item.Title + " " + ".";
                    selectedItem.RefNumber = item.RefNumber;
                    selectedItem.Category = item.Category;
                    selectedItem.PackSize = item.PackSize;
                    selectedItem.Cost = item.Cost;
					selectedItem.ToolTip = selectedItem.SelectedItemDescription.Content;

					StackOfSelectedItems.Children.Add(selectedItem);
                    NewCaseWindowReference.SelectedItems = StackOfSelectedItems;
                    UpdateSelectedItemListAndRecalculateCosts();
                    break;

                default:
                    break;
            }
        }

        private void GenericAddItem(InventoryItem item)
        {
            selectedItem = new SelectedItem();
            selectedItem.NewCaseWindowReference = NewCaseWindowReference;

            //SET THE BUTTON TAG TO BE THE SAME AS THE SELECTED ITEM'S ID PROPERTY - TO AID INDIVIDUAL SELECTION LATER.
            selectedItem.DeleteSelectedItemButton.Tag = item.RefNumber;
            selectedItem.DeleteSelectedItemButton.ToolTip = "id = " + item.RefNumber;
            selectedItem.SelectedItemCost.Content = item.Cost;
            selectedItem.InventoryItemImage.Tag = item.RefNumber;
        }

        private void UpdateSelectedItemListAndRecalculateCosts()
        {
            //UPDATE THE LIST OF SELECTED ITEMS TO INCLUDE THE NEWLY ADDED ITEM VIA THE INTERFACE.
            IUpdateSelectedItemsList = NewCaseWindowReference;
            IUpdateSelectedItemsList.UpdateSelectedItemsList(StackOfSelectedItems);

            //RECALCULATE THE COST OF ALL THE SELECTED ITEMS VIA THE INTERFACE.
            ICalculateTotalSelectedItemsCost = NewCaseWindowReference;
            ICalculateTotalSelectedItemsCost.CalculateCostOfAllSelectedItems();
        }


        private void CommonAddSelectedItem(object sender, MouseButtonEventArgs e)
        {
            string clickedItemRefNumber = "";

            if (sender is InventoryItem)
            {
                clickedItemRefNumber = ((InventoryItem)sender).RefNumber.ToString();
            }
            else if(sender is Button){
                clickedItemRefNumber = ((Button)sender).Tag.ToString();
            }

            if (NewCaseWindowReference.SelectedItems.Children.Count > 0)
            {
                var childrenCount = NewCaseWindowReference.SelectedItems.Children.Count;
                var children = NewCaseWindowReference.SelectedItems.Children;

                bool matchFound = false;

                //CHECK IF SELECTEDITEM ALREADY EXISTS.
                for (int i = 0; i < childrenCount; i++)
                {
                    SelectedItem childItem = (SelectedItem)children[i];

                    if (childItem.DeleteSelectedItemButton.Tag.ToString() == clickedItemRefNumber)
                    {
                        matchedChildItem = childItem;
                        matchFound = true;
                    }
                }

                //IF ITEM ALREADY EXISTS, UPDATE ACCORDINGLY.
                if (matchFound)
                {
                    int currentItemCount = int.Parse(matchedChildItem.ItemCount.Content.ToString());
                    Double currentItemCost = Double.Parse(matchedChildItem.SelectedItemCost.Content.ToString());
                    Double baseCost = currentItemCost / currentItemCount;

                    //Convert itemCount string to int, add 1, then convert back to string and set as new itemCount content.
                    matchedChildItem.ItemCount.Content = (currentItemCount + 1).ToString();

                    //Convert itemCost string to Double, multiply baseCost by itemCount, convert back to string and set as SelectedItemCost.content.
                    matchedChildItem.SelectedItemCost.Content = (baseCost * (currentItemCount + 1)).ToString();

                    //UPDATE THE LIST OF SELECTED ITEMS TO INCLUDE THE NEWLY ADDED ITEM VIA THE INTERFACE.
                    IUpdateSelectedItemsList = NewCaseWindowReference;
                    IUpdateSelectedItemsList.UpdateSelectedItemsList(StackOfSelectedItems);

                    //RECALCULATE THE COST OF ALL THE SELECTED ITEMS VIA THE INTERFACE.
                    ICalculateTotalSelectedItemsCost = NewCaseWindowReference;
                    ICalculateTotalSelectedItemsCost.CalculateCostOfAllSelectedItems();
                }
                else
                {
                    AddItemToSelectedItemsList(sender);
                }
            }
            else
            {
                AddItemToSelectedItemsList(sender);
            }
        }

        //Display a product image window on click.
        private void ShowImage(object sender, RoutedEventArgs e)
        {
            inventoryItemImageWindow = new InventoryItemImageWindow(this);
            inventoryItemImageWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inventoryItemImageWindow.ShowDialog();
        }

        //Remove product image window on mouse exit.
        private void CloseImageWindowOnMouseLeave(object sender, MouseEventArgs e)
        {
            inventoryItemImageWindow.Close();
            inventoryItemImageWindow = null;
        }

	}
}
