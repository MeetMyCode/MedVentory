using InterventionalCostings.Static_Data;
using InterventionalCostings.Controllers;
using System.Reflection;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using InterventionalCostings.Inventory_Item_Classes;
using InterventionalCostings.CustomViews;
using InterventionalCostings.Inventory_Items;
using EnumEmbolisationSystem = InterventionalCostings.Static_Data.StaticData.EnumEmbolisationSystem;
using EnumDilator = InterventionalCostings.Static_Data.StaticData.EnumDilator;
using EnumCatheter = InterventionalCostings.Static_Data.StaticData.EnumCatheter;
using EnumBalloon = InterventionalCostings.Static_Data.StaticData.EnumBalloon;
using EnumWire = InterventionalCostings.Static_Data.StaticData.EnumWire;
using EnumDressing = InterventionalCostings.Static_Data.StaticData.EnumDressing;
using EnumStent = InterventionalCostings.Static_Data.StaticData.EnumStent;
using EnumMisc = InterventionalCostings.Static_Data.StaticData.EnumMisc;
using EnumCoil = InterventionalCostings.Static_Data.StaticData.EnumCoil;
using EnumContrast = InterventionalCostings.Static_Data.StaticData.EnumContrast;
using EnumSnare = InterventionalCostings.Static_Data.StaticData.EnumSnare;
using EnumSheath = InterventionalCostings.Static_Data.StaticData.EnumSheath;
using System.Globalization;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;

namespace InterventionalCostings.Pages
{
    /// <summary>
    /// Interaction logic for NewCasePage.xaml
    /// </summary>
    /// 
    

    public partial class NewCasePage : Page, IUpdateSelectedItemsList, ICalculateTotalSelectedItemsCost, IDeleteSelectedItem
    {
        private static NewCasePage NewCasePageInstance = null;
        public StackPanel SelectedItems = new StackPanel();
        public StackPanel ItemsFromDatabaseStackPanel = new StackPanel();
        public StackPanel SearchStack = new StackPanel();

        private static readonly object padlock = new object();
        private ObservableCollection<string> referrers;
        private ObservableCollection<string> rads;
        private ObservableCollection<string> tables;

        //ComboBox Default Colors
        private Brush ReferringConsBackground;
        private Brush LocationBackground;
        private Brush RadBackground;


        //Constructor
        NewCasePage()
        {
            InitializeComponent();

            //Auto set the current date for each new case.
            caseDate.Text = DateTime.Now.ToShortDateString();

            //Note the default background colors for resue during validation.
            ReferringConsBackground = referringCons.Background;
            LocationBackground = procedureLocation.Background;
            RadBackground = radiologist.Background;

            ItemsFilter.ItemsSource = StaticData.GetDatabaseTables();

            PopulateLocationRadReferrerAndFilterComboBoxes();
            PopulateInventoryItemsList(ItemsFilter.SelectedItem as string);

            //Insert Custom Footer
            Footer footer = new Footer();
            FooterGrid.Children.Add(footer);

            //Insert custom TitleBar
            CustomTitleBar titleBar = new CustomTitleBar();
            titleBar.TitleBarTitleText.Content = "Start a New Interventional Procedure";
            MainGrid.Children.Add(titleBar);


        }




        /*
         **********************************************************************************************************************
         ******************************SEARCH RELATED METHODS******************************************************************
         **********************************************************************************************************************
        */
        //Initiate the search text search.
        private void RunSearch(object sender, TextChangedEventArgs e)
        {
            //Empty SearchStack to clear out old search results.
            SearchStack.Children.Clear();

            TextBox textBox = (TextBox)sender;

            var DbItems = ItemsFromDatabaseStackPanel.Children;

            //If textbox is empty, populate SelectedItems with all items of selected filter, otherwise populate only with items matching textbox text.
            if (textBox.Text == "")
            {
                itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                SearchLabel.Content = "Search:";
            }
            else
            {
                foreach (var item in DbItems)
                {
                    var inventoryItem = (InventoryItem)item;

                    //Search for the item in the selected category - eg Dilators/Stents/Coils - for the search text. Return true/false for found/not found
                    SearchItemCategory(inventoryItem, textBox.Text);
                }

                itemsFromDatabase.Content = SearchStack;

                //Display number of items from search count
                SearchLabel.Content = "Search: (" + SearchStack.Children.Count + " matching records found!)";
            }
        }

        //Search each item in the selected category for the search text. Called from RunSearch.
        private void SearchItemCategory(InventoryItem inventoryItem, string text)
        {
            string lowerCaseText = text.ToLower();

            switch (ItemsFilter.Text)
            {
                case "Dilators":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddDilatorFromSearch(inventoryItem);
                    }
                    break;

                case "Sheaths":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddSheathFromSearch(inventoryItem);
                    }
                    break;

                case "Catheters":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddCatheterFromSearch(inventoryItem);
                    }
                    break;

                case "Balloons":

                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddBalloonFromSearch(inventoryItem);
                    }
                    break;

                case "Embolisation Coils":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddCoilFromSearch(inventoryItem);
                    }
                    break;

                case "Contrast":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddContrastFromSearch(inventoryItem);
                    }
                    break;

                case "Dressings":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddDressingFromSearch(inventoryItem);
                    }
                    break;


                case "Snares":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddSnareFromSearch(inventoryItem);
                    }
                    break;


                case "Stents":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddStentFromSearch(inventoryItem);
                    }
                    break;


                case "Wires":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddWireFromSearch(inventoryItem);
                    }
                    break;

                case "Embolisation Systems":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddEmbolisationSystemFromSearch(inventoryItem);
                    }
                    break;

                case "Misc":
                    if (inventoryItem.InventoryItemDescription.Content.ToString().ToLower().Contains(lowerCaseText))
                    {
                        AddMiscItemFromSearch(inventoryItem);
                    }
                    break;

                default:
                    Console.WriteLine("item category not found: " + ItemsFilter.Text);
                    break;
            }



        }

        //If a search for item is found, add it to the SearchStack stackpanel. Called from SearchItemCategory.
        private void AddEmbolisationSystemFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM EmbolisationSystems WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetEmbolisationSystems(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var embolisationSystem in data)
            {
                item.ID = embolisationSystem[(int)StaticData.EnumRadOrReferrer.ID];

                item.ID = embolisationSystem[(int)EnumEmbolisationSystem.ID];
                item.Brand = embolisationSystem[(int)EnumEmbolisationSystem.Brand];
                item.Title = embolisationSystem[(int)EnumEmbolisationSystem.Title];
                item.Subtitle = embolisationSystem[(int)EnumEmbolisationSystem.SubTitle];
                item.Cost = decimal.Parse(embolisationSystem[(int)EnumEmbolisationSystem.Cost]);
                item.RefNumber = embolisationSystem[(int)EnumEmbolisationSystem.RefNumber];
                item.Category = embolisationSystem[(int)EnumEmbolisationSystem.Category];
                item.PackSize = Int32.Parse(embolisationSystem[(int)EnumEmbolisationSystem.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = embolisationSystem[(int)EnumEmbolisationSystem.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddCatheterFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Catheters WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetCatheters(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var catheter in data)
            {
                item.ID = catheter[(int)EnumCatheter.ID];
                item.Brand = catheter[(int)EnumCatheter.Brand];
                item.Title = catheter[(int)EnumCatheter.Title];
                item.Category = catheter[(int)EnumCatheter.SubTitle];
                item.Diameter = catheter[(int)EnumCatheter.Diameter];
                item.Dimensions = catheter[(int)EnumCatheter.Dimensions];
                item.Cost = decimal.Parse(catheter[(int)EnumCatheter.Cost]);
                item.RefNumber = catheter[(int)EnumCatheter.RefNumber];
                item.Category = catheter[(int)EnumCatheter.Category];
                item.PackSize = Int32.Parse(catheter[(int)EnumCatheter.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = catheter[(int)EnumCatheter.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddBalloonFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Balloons WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetBalloons(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var balloon in data)
            {
                item.ID = balloon[(int)EnumBalloon.ID];
                item.Brand = balloon[(int)EnumBalloon.Brand];
                item.Title = balloon[(int)EnumBalloon.Title];
                item.Diameter = balloon[(int)EnumBalloon.Diameter];
                item.Dimensions = balloon[(int)EnumBalloon.Dimensions];
                item.Cost = decimal.Parse(balloon[(int)EnumBalloon.Cost]);
                item.RefNumber = balloon[(int)EnumBalloon.RefNumber];
                item.Category = balloon[(int)EnumBalloon.Category];
                item.PackSize = Int32.Parse(balloon[(int)EnumBalloon.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = balloon[(int)EnumBalloon.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddCoilFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Coils WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetCoils(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var coil in data)
            {
                item.ID = coil[(int)EnumCoil.ID];
                item.Brand = coil[(int)EnumCoil.Brand];
                item.Title = coil[(int)EnumCoil.Title];
                item.Subtitle = coil[(int)EnumCoil.SubTitle];
                item.Diameter = coil[(int)EnumCoil.Diameter];
                item.Dimensions = coil[(int)EnumCoil.Dimensions];
                item.Cost = decimal.Parse(coil[(int)EnumCoil.Cost]);
                item.Category = coil[(int)EnumCoil.Category];
                item.RefNumber = coil[(int)EnumCoil.RefNumber];
                item.PackSize = Int32.Parse(coil[(int)EnumCoil.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = coil[(int)EnumCoil.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddContrastFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Contrast WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetContrast(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var contrast in data)
            {
                item.ID = contrast[(int)EnumContrast.ID];
                item.Brand = contrast[(int)EnumContrast.Brand];
                item.Strength = contrast[(int)EnumContrast.Strength];
                item.Volume = contrast[(int)EnumContrast.Volume];
                item.Cost = decimal.Parse(contrast[(int)EnumContrast.Cost]);
                item.RefNumber = contrast[(int)EnumContrast.RefNumber];
                item.Category = contrast[(int)EnumContrast.Category];
                item.PackSize = Int32.Parse(contrast[(int)EnumContrast.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = contrast[(int)EnumCoil.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddDressingFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Dressings WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetDressings(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var dressing in data)
            {
                item.ID = dressing[(int)EnumDressing.ID];
                item.Brand = dressing[(int)EnumDressing.Brand];
                item.Title = dressing[(int)EnumDressing.Title];
                item.Cost = decimal.Parse(dressing[(int)EnumDressing.Cost]);
                item.RefNumber = dressing[(int)EnumDressing.RefNumber];
                item.Category = dressing[(int)EnumDressing.Category];
                item.PackSize = Int32.Parse(dressing[(int)EnumDressing.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = dressing[(int)EnumDressing.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddSnareFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Snares WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetSnares(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var snare in data)
            {
                item.ID = snare[(int)EnumSnare.ID];
                item.Brand = snare[(int)EnumSnare.Brand];
                item.Title = snare[(int)EnumSnare.Title];
                item.Diameter = snare[(int)EnumSnare.Diameter];
                item.Dimensions = snare[(int)EnumSnare.Dimensions];
                item.Cost = decimal.Parse(snare[(int)EnumSnare.Cost]);
                item.Category = snare[(int)EnumSnare.Category];
                item.RefNumber = snare[(int)EnumSnare.RefNumber];
                item.PackSize = Int32.Parse(snare[(int)EnumSnare.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = snare[(int)EnumSnare.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddStentFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Stents WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetStents(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var stent in data)
            {
                item.ID = stent[(int)EnumStent.ID];
                item.Brand = stent[(int)EnumStent.Brand];
                item.Title = stent[(int)EnumStent.Title];
                item.Subtitle = stent[(int)EnumStent.SubTitle];
                item.Diameter = stent[(int)EnumStent.Diameter];
                item.Dimensions = stent[(int)EnumStent.Dimensions];
                item.Cost = decimal.Parse(stent[(int)EnumStent.Cost]);
                item.RefNumber = stent[(int)EnumStent.RefNumber];
                item.Category = stent[(int)EnumStent.Category];
                item.PackSize = Int32.Parse(stent[(int)EnumStent.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = stent[(int)EnumStent.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddWireFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Wires WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetWires(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var wire in data)
            {
                item.ID = wire[(int)EnumWire.ID];
                item.Brand = wire[(int)EnumWire.Brand];
                item.Title = wire[(int)EnumWire.Title];
                item.Subtitle = wire[(int)EnumWire.SubTitle];
                item.Diameter = wire[(int)EnumWire.Diameter];
                item.Dimensions = wire[(int)EnumWire.Dimensions];
                item.Cost = decimal.Parse(wire[(int)EnumWire.Cost]);
                item.RefNumber = wire[(int)EnumWire.RefNumber];
                item.Category = wire[(int)EnumWire.Category];
                item.PackSize = Int32.Parse(wire[(int)EnumWire.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = wire[(int)EnumWire.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddMiscItemFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Misc WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetMiscItems(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var miscItem in data)
            {
                item.ID = miscItem[(int)EnumMisc.ID];
                item.Brand = miscItem[(int)EnumMisc.Brand];
                item.Title = miscItem[(int)EnumMisc.Title];
                item.Subtitle = miscItem[(int)EnumMisc.SubTitle];
                item.Cost = decimal.Parse(miscItem[(int)EnumMisc.Cost]);
                item.RefNumber = miscItem[(int)EnumMisc.RefNumber];
                item.Category = miscItem[(int)EnumMisc.Category];
                item.PackSize = Int32.Parse(miscItem[(int)EnumMisc.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = miscItem[(int)EnumMisc.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddSheathFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Sheaths WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetSheaths(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var sheath in data)
            {
                item.ID = sheath[(int)EnumSheath.ID];
                item.Category = sheath[(int)EnumSheath.Category];
                item.Brand = sheath[(int)EnumSheath.Brand];
                item.Title = sheath[(int)EnumSheath.Title];
                item.Cost = decimal.Parse(sheath[(int)EnumSheath.Cost]);
                item.RefNumber = sheath[(int)EnumSheath.RefNumber];
                item.Diameter = sheath[(int)EnumSheath.Diameter];
                item.Dimensions = sheath[(int)EnumSheath.Dimensions];
                item.PackSize = Int32.Parse(sheath[(int)EnumSheath.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = sheath[(int)EnumSheath.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }

        private void AddDilatorFromSearch(InventoryItem inventoryItem)
        {
            string SqlQuery = "SELECT * FROM Dilators WHERE Id = " + inventoryItem.ID;
            var data = DBController.GetDilators(SqlQuery);

            InventoryItem item = new InventoryItem();

            foreach (var dilator in data)
            {
                item.ID = dilator[(int)EnumDilator.ID];
                item.Category = dilator[(int)EnumDilator.Category];
                item.Brand = dilator[(int)EnumDilator.Brand];
                item.Title = dilator[(int)EnumDilator.Title];
                item.Cost = decimal.Parse(dilator[(int)EnumDilator.Cost]);
                item.RefNumber = dilator[(int)EnumDilator.RefNumber];
                item.Diameter = dilator[(int)EnumDilator.Diameter];
                item.Dimensions = dilator[(int)EnumDilator.Dimensions];
                item.PackSize = Int32.Parse(dilator[(int)EnumDilator.PackSize]);

                item.NewCaseWindowReference = this;
                item.StackOfSelectedItems = SelectedItems;
                item.InventoryItemImage.Tag = dilator[(int)EnumDilator.RefNumber];
                item.InventoryItemDescription.Content = inventoryItem.InventoryItemDescription.Content;
            }

            SearchStack.Children.Add(item);
        }


        /*
         **********************************************************************************************************************
         ******************************SELECTEDITEMSLIST RELATED METHODS******************************************************************
         **********************************************************************************************************************
        */
        //Updates the items in SelectedItems with new data.
        public void UpdateSelectedItemsList(StackPanel selectedItemsList)
        {
            SelectedItemsList.Content = selectedItemsList;
        }

        //Adds an item to the SelectedItemsList.
        internal void AddSelectedItem(StackPanel selectedItemList)
        {
            SelectedItemsList.Content = selectedItemList;
        }

        //Deletes any of the selected items when clicking on the DELETE button, and then recalculates the total cost of the remaining items.
        public void DeleteSelectedItem(string itemRefNumber)
        {
            for (int i = 0; i < SelectedItems.Children.Count; i++)
            {
                var item = SelectedItems.Children[i] as SelectedItem;
                if (item.DeleteSelectedItemButton.Tag.ToString() == itemRefNumber.ToString())
                {
                    SelectedItems.Children.Remove(item);
                }
            }

            CalculateCostOfAllSelectedItems();

            if (SelectedItems.Children.Count == 0)
            {
                Label warning = new Label();
                warning.Content = "No Item Selected!";
                warning.HorizontalAlignment = HorizontalAlignment.Center;
                warning.Margin = new Thickness(0, 200, 0, 0);

                SelectedItems.Children.Add(warning);
            }

        }

        //Calculates the total cost of all selected items.
        public void CalculateCostOfAllSelectedItems()
        {
            decimal totalCost = 0;
            UIElementCollection Items = SelectedItems.Children;

            foreach (var selectedItem in Items)
            {
                SelectedItem item = (SelectedItem)selectedItem;

                Decimal cost = Decimal.Parse(item.SelectedItemCost.Content.ToString());
                totalCost += cost;
            }

            CostOfAllSelectedItems.Content = totalCost.ToString("0.00");

        }


        /*
          **********************************************************************************************************************
          ******************************PRODUCT LIST CREATION METHODS***********************************************************
          **********************************************************************************************************************
         */
        //Populate the items list according to which database table filter is selected.
        private void PopulateInventoryItemsList(string SelectedCategory)
        {
            ItemsFromDatabaseStackPanel.Orientation = Orientation.Vertical;
            ItemsFromDatabaseStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            switch (SelectedCategory)
            {
                case "Dilators":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Dilator> dilators = StaticData.GetDilators();

                    foreach (var dilator in dilators)
                    {
                        CreateDilatorList(dilator, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Snares":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Snare> snares = StaticData.GetSnares();

                    foreach (var snare in snares)
                    {
                        CreateSnaresList(snare, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Sheaths":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Sheath> sheaths = StaticData.GetSheaths();

                    foreach (var sheath in sheaths)
                    {
                        CreateSheathList(sheath, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Catheters":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Catheter> catheters = StaticData.GetCatheters();

                    foreach (var catheter in catheters)
                    {
                        CreateCatheterList(catheter, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Balloons":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Balloon> balloons = StaticData.GetBalloons();

                    foreach (var balloon in balloons)
                    {
                        CreateBalloonList(balloon, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Stents":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Stent> stents = StaticData.GetStents();

                    foreach (var stent in stents)
                    {
                        CreateStentList(stent, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Coils":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<EmbolisationCoil> coils = StaticData.GetCoils();

                    foreach (var coil in coils)
                    {
                        CreateCoilList(coil, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Embolisation Systems":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<EmbolisationSystem> embolisationSystems = StaticData.GetEmbolisationSystems();

                    foreach (var embolisationSystem in embolisationSystems)
                    {
                        CreateEmbolisationSystemList(embolisationSystem, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Wires":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<GuideWire> wires = StaticData.GetWires();

                    foreach (var wire in wires)
                    {
                        CreateWiresList(wire, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Dressings":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Dressing> dressings = StaticData.GetDressings();

                    foreach (var dressing in dressings)
                    {
                        CreateDressingsList(dressing, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Misc":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<MiscItem> miscItems = StaticData.GetMiscItems();

                    foreach (var miscItem in miscItems)
                    {
                        CreateMiscItemsList(miscItem, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                case "Contrast":
                    ItemsFromDatabaseStackPanel.Children.Clear();
                    List<Contrast> contrastList = StaticData.GetContrast();

                    foreach (var contrast in contrastList)
                    {
                        CreateContrastList(contrast, ItemsFromDatabaseStackPanel);
                        itemsFromDatabase.Content = ItemsFromDatabaseStackPanel;
                    }
                    break;

                default:
                    break;
            }

            //Display number of items from database found count
            FilterLabel.Content = "Filter: (" + ItemsFromDatabaseStackPanel.Children.Count + " records found!)";
            SearchLabel.Content = "Search:";

        }

        //Add a dressing from the database to ItemsFromDatabaseStackPanel. 
        private void CreateDressingsList(Dressing dressing, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = dressing.ID;
            item.Brand = dressing.Brand;
            item.Title = dressing.Title;
            item.Subtitle = dressing.SubTitle;
            item.Cost = decimal.Parse(dressing.Cost);
            item.RefNumber = dressing.RefNumber;
            item.Category = dressing.Category;
            item.PackSize = Int32.Parse(dressing.PackSize);

            item.InventoryItemImage.Tag = dressing.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Title + " " + item.Category + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        private void CreateContrastList(Contrast contrastItem, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = contrastItem.ID;
            item.Brand = contrastItem.Brand;
            item.Strength = contrastItem.Strength;
            item.Volume = contrastItem.Volume;
            item.Cost = decimal.Parse(contrastItem.Cost);
            item.RefNumber = contrastItem.RefNumber;
            item.Category = contrastItem.Category;
            item.PackSize = Int32.Parse(contrastItem.PackSize);

            item.InventoryItemImage.Tag = contrastItem.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Strength + " " + item.Volume + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        private void CreateSnaresList(Snare snare, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = snare.ID;
            item.Brand = snare.Brand;
            item.Title = snare.Title;
            item.Subtitle = snare.SubTitle;
            item.Diameter = snare.Diameter;
            item.Dimensions = snare.Dimensions;
            item.Cost = decimal.Parse(snare.Cost);
            item.RefNumber = snare.RefNumber;
            item.Category = snare.Category;
            item.PackSize = Int32.Parse(snare.PackSize);

            item.InventoryItemImage.Tag = snare.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add an EmbolisationSystem from the database to ItemsFromDatabaseStackPanel. 
        private void CreateEmbolisationSystemList(EmbolisationSystem embolisationSystem, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = embolisationSystem.ID;
            item.Brand = embolisationSystem.Brand;
            item.Title = embolisationSystem.Title;
            item.Subtitle = embolisationSystem.SubTitle;
            item.Cost = decimal.Parse(embolisationSystem.Cost);
            item.RefNumber = embolisationSystem.RefNumber;
            item.Category = embolisationSystem.Category;
            item.PackSize = Int32.Parse(embolisationSystem.PackSize);

            item.InventoryItemImage.Tag = embolisationSystem.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Subtitle + " " + item.Category + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add an stent from the database to ItemsFromDatabaseStackPanel. 
        private void CreateStentList(Stent stent, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = stent.ID;
            item.Brand = stent.Brand;
            item.Title = stent.Title;
            item.Subtitle = stent.SubTitle;
            item.Diameter = stent.Diameter;
            item.Dimensions = stent.Dimensions;
            item.Cost = decimal.Parse(stent.Cost);
            item.RefNumber = stent.RefNumber;
            item.Category = stent.Category;
            item.PackSize = Int32.Parse(stent.PackSize);

            item.InventoryItemImage.Tag = stent.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Diameter + " " + item.Brand + " " + item.Dimensions + " " + item.Category + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add an embolisation coil from the database to ItemsFromDatabaseStackPanel. 
        private void CreateCoilList(EmbolisationCoil coil, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = coil.ID;
            item.Brand = coil.Brand;
            item.Title = coil.Title;
            item.Subtitle = coil.SubTitle;
            item.Diameter = coil.Diameter;
            item.Dimensions = coil.Dimensions;
            item.Cost = decimal.Parse(coil.Cost);
            item.Category = coil.Category;
            item.RefNumber = coil.RefNumber;
            item.PackSize = Int32.Parse(coil.PackSize);

            item.InventoryItemImage.Tag = coil.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add a wire from the database to ItemsFromDatabaseStackPanel. 
        private void CreateWiresList(GuideWire wire, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = wire.ID;
            item.Brand = wire.Brand;
            item.Title = wire.Title;
            item.Subtitle = wire.SubTitle;
            item.Diameter = wire.Diameter;
            item.Dimensions = wire.Dimensions;
            item.Cost = decimal.Parse(wire.Cost);
            item.RefNumber = wire.RefNumber;
            item.Category = wire.Category;
            item.PackSize = Int32.Parse(wire.PackSize);

            item.InventoryItemImage.Tag = wire.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add a Misc Item from the database to ItemsFromDatabaseStackPanel. 
        private void CreateMiscItemsList(MiscItem miscItem, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = miscItem.ID;
            item.Brand = miscItem.Brand;
            item.Title = miscItem.Title;
            item.Subtitle = miscItem.SubTitle;
            item.Cost = decimal.Parse(miscItem.Cost);
            item.RefNumber = miscItem.RefNumber;
            item.Category = miscItem.Category;
            item.PackSize = Int32.Parse(miscItem.PackSize);

            item.InventoryItemImage.Tag = miscItem.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;

            if (item.Subtitle == "")
            {
                item.InventoryItemDescription.Content = item.Brand + " " + item.Title + ".";

            }
            else
            {
                item.InventoryItemDescription.Content = item.Brand + " " + item.Title + " " + item.Subtitle + ".";

            }

            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add a balloon from the database to ItemsFromDatabaseStackPanel. 
        private void CreateBalloonList(Balloon balloon, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = balloon.ID;
            item.Brand = balloon.Brand;
            item.Title = balloon.Title;
            item.Diameter = balloon.Diameter;
            item.Dimensions = balloon.Dimensions;
            item.Cost = decimal.Parse(balloon.Cost);
            item.RefNumber = balloon.RefNumber;
            item.Category = balloon.Category;
            item.PackSize = Int32.Parse(balloon.PackSize);

            item.InventoryItemImage.Tag = balloon.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Diameter + " " + item.Brand + " " + item.Dimensions + " " + item.Category + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add a sheath from the database to ItemsFromDatabaseStackPanel. 
        private void CreateCatheterList(Catheter catheter, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = catheter.ID;
            item.Brand = catheter.Brand;
            item.Title = catheter.Title;
            item.Subtitle = catheter.SubTitle;
            item.Diameter = catheter.Diameter;
            item.Dimensions = catheter.Dimensions;
            item.Cost = decimal.Parse(catheter.Cost);
            item.RefNumber = catheter.RefNumber;
            item.Category = catheter.Category;
            item.PackSize = Int32.Parse(catheter.PackSize);

            item.InventoryItemImage.Tag = catheter.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle +  ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add a sheath from the database to ItemsFromDatabaseStackPanel. 
        private void CreateSheathList(Sheath sheath, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = sheath.ID;
            item.Brand = sheath.Brand;
            item.Category = sheath.Category;
            item.RefNumber = sheath.RefNumber;
            item.Diameter = sheath.Diameter;
            item.Dimensions = sheath.Dimensions;
            item.Title = sheath.Title;
            item.Cost = decimal.Parse(sheath.Cost);
            item.PackSize = Int32.Parse(sheath.PackSize);
            item.InventoryItemImage.Tag = sheath.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Brand + " " + item.Diameter + " " + item.Dimensions + " " + item.Title + " " + item.Subtitle +  ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Add a dilator from the database to ItemsFromDatabaseStackPanel. 
        private void CreateDilatorList(Dilator dilator, StackPanel ItemsFromDatabaseStackPanel)
        {
            InventoryItem item = new InventoryItem();
            item.ID = dilator.ID;
            item.Brand = dilator.Brand;
            item.Category = dilator.Category;
            item.RefNumber = dilator.RefNumber;
            item.Diameter = dilator.Diameter;
            item.Dimensions = dilator.Dimensions;
            item.Title = dilator.Title;
            item.Cost = decimal.Parse(dilator.Cost);
            item.PackSize = Int32.Parse(dilator.PackSize);
            item.InventoryItemImage.Tag = dilator.RefNumber;
            item.InventoryItemPlusButton.Tag = dilator.RefNumber;

            item.NewCaseWindowReference = this;
            item.StackOfSelectedItems = SelectedItems;
            item.InventoryItemDescription.Content = item.Diameter + " " + item.Brand + " " + item.Category + ".";
            item.ToolTip = item.InventoryItemDescription.Content;

            ItemsFromDatabaseStackPanel.Children.Add(item);
        }

        //Change of item category event handler.
        private void ChangeCategory(object sender, SelectionChangedEventArgs e)
        {
            searchBox.Text = "";
            string SelectedCategory = ((ComboBox)sender).SelectedItem as string;
            PopulateInventoryItemsList(SelectedCategory);
        }



        /*
        **********************************************************************************************************************
        ******************************MISC METHODS****************************************************************************
        **********************************************************************************************************************
        */
        private void PopulateLocationRadReferrerAndFilterComboBoxes()
        {
            //Populate Referring Consultant Combo box.
            //List<Referrer> refList = StaticData.GetReferrers();
            //List<string> referrerNames = ExtractReferrerNames(refList);
            //referrers = new ObservableCollection<string>(referrerNames);
            //referringCons.ItemsSource = referrers;


            //Populate Radiologist Combo box.
            List<Radiologist> radList = StaticData.GetRadiologists();
            List<string> radNames = ExtractRadNames(radList);
            rads = new ObservableCollection<string>(radNames);
            radiologist.ItemsSource = rads;

            //Populate Procedure Location Combo box.
            List<string> procedureLocations = StaticData.GetProcedureLocations();
            tables = new ObservableCollection<string>(procedureLocations);
            procedureLocation.ItemsSource = tables;

            //Populate Category Combo box.
            List<string> dataTables = StaticData.GetDatabaseTables();
            tables = new ObservableCollection<string>(dataTables);
            ItemsFilter.ItemsSource = tables;
            ItemsFilter.SelectedIndex = 0;

        }

        //For use in combo boxes
        private List<string> ExtractReferrerNames(List<Referrer> referrers)
        {
            List<string> referrerList = new List<string>();

            foreach (var referrer in referrers)
            {
                referrerList.Add(referrer.FirstName + " " + referrer.Surname);
            }

            return referrerList;
        }

        //For use in combo boxes
        private List<string> ExtractRadNames(List<Radiologist> radiologists)
        {
            List<string> radList = new List<string>();

            foreach (var rad in radiologists)
            {
                radList.Add(rad.FirstName + " " + rad.Surname);
            }

            return radList;
        }

        //Singleton Access
        public static NewCasePage NewCaseInstance
        {
            get
            {
                lock (padlock)
                {
                    if (NewCasePageInstance == null)
                    {
                        NewCasePageInstance = new NewCasePage();
                    }
                    return NewCasePageInstance;
                }
            }

        }

        private void CancelNewCase(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
            StartPage startPage = StartPage.StartInstance;
            NavigationService.Navigate(startPage);
        }

        private void ClearAllFields()
        {

            ptNhsNumber.Text = "";
            ptFirstName.Text = "";
            ptSurname.Text = "";
            ptDob.Text = "";
            referringCons.Text = "";

            caseDate.Text = "";
            caseProcedure.Text = "";
            procedureLocation.Text = "";
            radiologist.Text = "";
            caseNotes.Text = "";

            SelectedItems.Children.Clear();
            CostOfAllSelectedItems.Content = @"0";
        }


        /*
        **********************************************************************************************************************
        ******************************VALIDATION & FILE SAVING METHODS********************************************************
        **********************************************************************************************************************
        */

        //Validate and then Save the current case to an XLSX file if necessary.
        private void SaveCaseButtonClicked(object sender, RoutedEventArgs e)
        {
            var isValid = ValidateCase();

            if (isValid)
            {
                string WrittenFile = WriteCaseToFile();
                MessageBoxResult result = MessageBox.Show("Case Saved to File!. Open File?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(WrittenFile);
                }

                CloseCaseAndReOpenMainPage();
            }
            else
            {
                MessageBox.Show("No Items Selected or Data Field is Empty - Please Select at Least One Item and/or Provide the Required Information.", "Missing Items/Information!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void CloseCaseAndReOpenMainPage()
        {
            ClearAllFields();
            StartPage startPage = StartPage.StartInstance;
            NavigationService.Navigate(startPage);

        }

        private string WriteCaseToFile()
        {
            //check id saved case directory exists - if not, create it.
            string currentCaseDirectory = CheckForSavedCaseDirectory();

            //Copy master xls file to saved cases directory and rename it appropriately.
            string CaseFileNamePath = CopyAndRenameMasterXlsFile(currentCaseDirectory);

            //save case information to appropriate places in xls file.
            Case caseObject = CreateCaseObject();

            //Save case to file
            SaveCaseController CaseController = new SaveCaseController(caseObject, CaseFileNamePath);

			//Save case to Database
			DBController.SaveCaseDataToDatabase(caseObject, Path.GetFileName(CaseFileNamePath).Replace(" ",""));

            return CaseFileNamePath;
        }

		private bool ValidateCase()
        {
            var isValid = true;
            SolidColorBrush RedBackground = new SolidColorBrush(Colors.Red) { Opacity = 0.5 };
            

            if (ptNhsNumber.Text == "") 
            {
                ptNhsNumber.Background = RedBackground;
                isValid = false;
            }
            else
            {
                ptNhsNumber.Background = Brushes.White;
            }

            if (ptFirstName.Text == "")
            {
                ptFirstName.Background = RedBackground;
                isValid = false;
            }
            else
            {
                ptFirstName.Background = Brushes.White;
            }

            if (ptSurname.Text == "")
            {
                ptSurname.Background = RedBackground;
                isValid = false;
            }
            else
            {
                ptSurname.Background = Brushes.White;
            }

            if (ptDob.Text == "")
            {
                ptDob.Background = RedBackground;
                isValid = false;
            }
            else
            {
                ptDob.Background = Brushes.White;
            }

            if (referringCons.Text == "")
            {
                referringCons.Background = RedBackground;
                isValid = false;
            }
            else
            {
                referringCons.Background = ReferringConsBackground;
            }

            if (caseDate.Text == "")
            {
                caseDate.Background = RedBackground;
                isValid = false;
            }
            else
            {
                caseDate.Background = Brushes.White;
            }

            if (caseProcedure.Text == "")
            {
                caseProcedure.Background = RedBackground;
                isValid = false;
            }
            else
            {
                caseProcedure.Background = Brushes.White;
            }

            if (procedureLocation.Text == "")
            {
                procedureLocation.Background = RedBackground;
                isValid = false;
            }
            else
            {
                procedureLocation.Background = LocationBackground;
            }

            if (radiologist.Text == "")
            {
                radiologist.Background = RedBackground;
                isValid = false;
            }
            else
            {
                radiologist.Background = RadBackground;
            }

            var selectedItems = SelectedItems.Children;
            if (selectedItems.Count == 0)
            {
                isValid = false;
            }


            return isValid;
        }

        //This checks for the directory first, before creating.
        private string CheckForSavedCaseDirectory()
        {
            DateTime Now = DateTime.Now;
            string year = "\\" + Now.ToString("yyyy");
            string month = "\\" + Now.ToString("MMMM");

            string currentDirectory = Directory.GetCurrentDirectory();
            string FullPath = currentDirectory + StaticData.SavedCaseDirectory + year + month;
            Directory.CreateDirectory(FullPath);

            string currentCaseDirectory = Path.GetFullPath(FullPath);

            return currentCaseDirectory;
           
        }

        //Copy and rename MAster.xlsx to current case directory, ready for populating.
        private string CopyAndRenameMasterXlsFile(string currentCaseDirectory)
        {
            int caseCount = Directory.GetFiles(currentCaseDirectory).Length;
            string caseDateTrimmed = caseDate.Text.Replace("/", "");
            string DestCaseFileNamePath = currentCaseDirectory + "\\" + caseDateTrimmed + " " + "Case" + " " + (caseCount + 1) + ".xlsx";

            Assembly asm = Assembly.GetExecutingAssembly();
            string file = string.Format("{0}.MasterFiles.MasterCase.xlsx", asm.GetName().Name);
            Stream InputFileStream = asm.GetManifestResourceStream(file);

            SaveStreamToFile(DestCaseFileNamePath, InputFileStream);  //<--here is where to save to disk

            return DestCaseFileNamePath;

        }

        private void SaveStreamToFile(string DestCaseFileNamePath, Stream SourceStream)
        {
            if (SourceStream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = File.Create(DestCaseFileNamePath, (int)SourceStream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[SourceStream.Length];
                SourceStream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }

        private Case CreateCaseObject()
        {
            string nhsNum = ptNhsNumber.Text;
            string fName = ptFirstName.Text;
            string sName = ptSurname.Text;
            string dob = ptDob.Text;
            string referrer = referringCons.Text;
            DateTime date = Convert.ToDateTime(caseDate.Text);
            string procedure = caseProcedure.Text;
            string location = procedureLocation.Text;
            string rad = radiologist.Text;
            string notes = caseNotes.Text;
            List<SelectedItem> caseItems = GetSelectedCaseItems();
			decimal caseCost = decimal.Parse(CostOfAllSelectedItems.Content.ToString());

            Case caseObject = new Case(nhsNum, fName, sName, dob, referrer, date, procedure, location, rad, notes, caseItems, caseCost);

            return caseObject;
        }

        private List<SelectedItem> GetSelectedCaseItems()
        {
            var items = SelectedItems.Children;
            List<SelectedItem> ItemsList = new List<SelectedItem>();
            foreach (var item in items)
            {
                ItemsList.Add((SelectedItem)item);
            }

            return ItemsList;
        }


        private void GoToStartPage(object sender, RoutedEventArgs e)
        {
            StartPage start = StartPage.StartInstance;
            NavigationService.Navigate(start);
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow;
            main.WindowState = WindowState.Maximized;
            main.ResizeMode = ResizeMode.CanResize;
        }

        //Formats the nhs numbe to appear as 000-000-0000
        private void FormatNhsNumber(object sender, TextChangedEventArgs e)
        {
            TextBox nhsNum = (TextBox)sender;

            int charCount = nhsNum.Text.Length;

            //place a dash accordingly
            if (charCount == 3 || charCount == 7)
            {
                ptNhsNumber.Text = ptNhsNumber.Text + "-";
                ptNhsNumber.SelectionStart = ptNhsNumber.Text.Length;
            }

            //remove all chars after 12 chars
            if (charCount == 13)
            {
                ptNhsNumber.Text = ptNhsNumber.Text.Remove(12, 1);
                ptNhsNumber.CaretIndex = ptNhsNumber.Text.Length;
            }


        }

        //Open a popup window and populate it with all the referring consultant details
        private void PickReferringConsultant(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


    }//END OF CLASS
}
