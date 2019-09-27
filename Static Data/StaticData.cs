using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using InterventionalCostings.CustomViews;
using InterventionalCostings.Inventory_Item_Classes;
using InterventionalCostings.Inventory_Items;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;

namespace InterventionalCostings.Static_Data
{
    public interface IUpdateStartupProgressBar{

        void UpdateProgressBar(int newValue);
        void UpdateCurrentTask(string updateText);
        void UpdateTaskDetails(string updateText);
        void UpdateProgressMax(Double newValue);
    }


    class StaticData
    {
        //Directories
        #region
        public static string MasterCaseFilePath
        {
            get { return "MasterCase.xlsx"; }
        }

        public static string SavedCaseDirectory
        {
            get { return "\\Saved Cases"; }
        }

        public static string OrdersListDirectory
        {
            get { return "\\Order Requests"; }
        }

        public static string DestinationExcelFilesDirectory
        {
            get { return "ExcelFiles"; }
        }

        public static string MasterExcelFilesDirectory
        {
            get { return "MasterFiles"; }
        }
		#endregion

		//Email Settings
		#region
		//public static string PersonWhoDoesTheOrderingEmailAddress
		//{
		//	get { return Properties.mySettings.Default.OrdererEmail; }
		//}

		//public static string SmtpServerAddress
		//{
		//	//get { return "SEND.NHS.NET"; }
		//	get
		//	{
		//		return Properties.mySettings.Default.SmtpServerAddress;
		//	}
		//}

		//public static int SmtpServerPortNumber
		//{
		//	get { return Properties.mySettings.Default.SmtpServerPortNumber; }
		//}

		//public static string ImapServerAddress
		//{
		//	get { return Properties.mySettings.Default.ImapServerAddress; }
		//}

		//public static int ImapServerPortNumber
		//{
		//	get { return Properties.mySettings.Default.ImapServerPortNumber; }
		//}
		#endregion

		//Dictionaries (used in syncing excel files to database tables)
		#region
		public static List<(int, int)> BalloonTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //Diameter
            ( 1, 4 ), //Dimensions
            ( 1, 5 ), //Cost
            ( 1, 6 ), //RefNumber
            ( 1, 7 ), //Category
            ( 1, 8 ) //PackSize
        };
		public static List<(int, int)> CatheterTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //SubTitle
            ( 1, 4 ), //Diameter
            ( 1, 5 ), //Dimensions
            ( 1, 6 ), //Cost
            ( 1, 7 ), //RefNumber
            ( 1, 8 ), //Category
            ( 1, 9) //PackSize
        };
		public static List<(int, int)> CoilTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //SubTitle
            ( 1, 4 ), //Diameter
            ( 1, 5 ), //Dimensions
            ( 1, 6 ), //Cost
            ( 1, 7 ), //RefNumber
            ( 1, 8 ), //Category
            ( 1, 9) //PackSize
        };
		public static List<(int, int)> ContrastTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Strength
            ( 1, 3 ), //Volume
            ( 1, 4 ), //Cost
            ( 1, 5 ), //RefNumber
            ( 1, 6 ), //Category
            ( 1, 7 ) //PackSize
        };
		public static List<(int, int)> DilatorTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //Diameter
            ( 1, 4 ), //Dimensions
            ( 1, 5 ), //Cost
            ( 1, 6 ), //RefNumber
            ( 1, 7 ), //Category
            ( 1, 8 ) //PackSize
        };
		public static List<(int, int)> DressingTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //Cost
            ( 1, 4 ), //RefNumber
            ( 1, 5 ), //Category
            ( 1, 6 ) //PackSize
        };
		public static List<(int, int)> EmbolisationSystemTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //SubTitle
            ( 1, 4 ), //Cost
            ( 1, 5 ), //RefNumber
            ( 1, 6 ), //Category
            ( 1, 7 ) //PackSize
        };
		public static List<(int, int)> MiscTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //SubTitle
            ( 1, 4 ), //Cost
            ( 1, 5 ), //RefNumber
            ( 1, 6 ), //Category
            ( 1, 7 ) //PackSize
        };
		public static List<(int, int)> MiscDataTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //DbTables
            ( 1, 2 ), //CaseLocations
            ( 1, 3 ), //ProductCategories

        };
		public static List<(int, int)> RadiologistTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Prefix
            ( 1, 2 ), //FirstName
            ( 1, 3 ), //Surname
            ( 1, 4 ), //Specialty

        };
		public static List<(int, int)> ReferrerTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Prefix
            ( 1, 2 ), //FirstName
            ( 1, 3 ), //Surname
            ( 1, 4 ), //Specialty

        };
		public static List<(int, int)> SheathTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //Diameter
            ( 1, 4 ), //Dimensions
            ( 1, 5 ), //Cost
            ( 1, 6 ), //RefNumber
            ( 1, 7 ), //Category
            ( 1, 8 ) //PackSize
        };
		public static List<(int, int)> StentTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //SubTitle
            ( 1, 4 ), //Diameter
            ( 1, 5 ), //Dimensions
            ( 1, 6 ), //Cost
            ( 1, 7 ), //RefNumber
            ( 1, 8 ), //Category
            ( 1, 9) //PackSize
        };
		public static List<(int, int)> WireTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //SubTitle
            ( 1, 4 ), //Diameter
            ( 1, 5 ), //Dimensions
            ( 1, 6 ), //Cost
            ( 1, 7 ), //RefNumber
            ( 1, 8 ), //Category
            ( 1, 9) //PackSize
        };
		public static List<(int, int)> SnareTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Brand
            ( 1, 2 ), //Title
            ( 1, 3 ), //SubTitle
            ( 1, 4 ), //Diameter
            ( 1, 5 ), //Dimensions
            ( 1, 6 ), //Cost
            ( 1, 7 ), //RefNumber
            ( 1, 8 ), //Category
            ( 1, 9) //PackSize
        };
		public static List<(int, int)> InterventionalNursesTitleCellLocations = new List<(int, int)> {

			( 1, 1 ), //Username
            ( 1, 2 ), //Email
            ( 1, 3 ), //First Name
            ( 1, 4 ), //Surname

        };
		#endregion

		//DbController.PopulateTable() SqlQuery Strings & GETTER Method.
		#region
		public static (string, List<string>) GetNewTableQueryStringAndEnum(string table)
        {
            string QueryString = "";
            List<string> stringsFromEnum = new List<string>();

            switch (table)
            {
                case "Balloons":
                    QueryString = NewBalloonTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumBalloon)));
                    break;

                case "Catheters":
                    QueryString = NewCatheterTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumCatheter)));
                    break;

                case "Coils":
                    QueryString = NewCoilTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumCoil)));
                    break;

                case "Contrast":
                    QueryString = NewContrastTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumContrast)));
                    break;

                case "Dilators":
                    QueryString = NewDilatorTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumDilator)));
                    break;

                case "Dressings":
                    QueryString = NewDressingTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumDressing)));
                    break;

                case "EmbolisationSystems":
                    QueryString = NewEmboSystemTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumEmbolisationSystem)));
                    break;

                case "Misc":
                    QueryString = NewMiscTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumMisc)));
                    break;

                case "MiscData":
                    QueryString = NewMiscDataTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumMiscData)));
                    break;

                case "Radiologists":
                    QueryString = NewRadiologistTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumRadOrReferrer)));
                    break;

                case "Referrers":
                    QueryString = NewReferrerTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumRadOrReferrer)));
                    break;

                case "Sheaths":
                    QueryString = NewSheathTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumSheath)));
                    break;

                case "Snares":
                    QueryString = NewSnareTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumSnare)));
                    break;

                case "Stents":
                    QueryString = NewStentTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumStent)));
                    break;

                case "Wires":
                    QueryString = NewWireTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumWire)));
                    break;

                case "InterventionalNurses":
                    QueryString = NewIrNursesTableQueryString;
                    stringsFromEnum = new List<string>(Enum.GetNames(typeof(EnumNurses)));
                    break;

                default:
                    break;
            }

            return (QueryString, stringsFromEnum);
        }

        public static string NewBalloonTableQueryString {
            get { return @"INSERT INTO Balloons (Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewCatheterTableQueryString
        {
            get { return @"INSERT INTO Catheters (Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @SubTitle, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewCoilTableQueryString
        {
            get { return @"INSERT INTO Coils (Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @SubTitle, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewContrastTableQueryString
        {
            get { return @"INSERT INTO Contrast (Brand, Strength, Volume, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Strength, @Volume, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewDilatorTableQueryString
        {
            get { return @"INSERT INTO Dilators (Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewDressingTableQueryString
        {
            get { return @"INSERT INTO Dressings (Brand, Title, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewEmboSystemTableQueryString
        {
            get { return @"INSERT INTO EmbolisationSystems (Brand, Title, SubTitle, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @SubTitle, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewMiscTableQueryString
        {
            get { return @"INSERT INTO Misc (Brand, Title, SubTitle, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @SubTitle, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewMiscDataTableQueryString
        {
            get { return @"INSERT INTO MiscData (DbTables, CaseLocations, SyncCategories) VALUES (@DbTables, @CaseLocations, @SyncCategories)"; }
        }

        public static string NewRadiologistTableQueryString
        {
            get { return @"INSERT INTO Radiologists (Prefix, FirstName, Surname, Specialty) VALUES (@Prefix, @FirstName, @Surname, @Specialty)"; }
        }

        public static string NewReferrerTableQueryString
        {
            get { return @"INSERT INTO Referrers (Prefix, FirstName, Surname, Specialty) VALUES (@Prefix, @FirstName, @Surname, @Specialty)"; }
        }

        public static string NewSheathTableQueryString
        {
            get { return @"INSERT INTO Sheaths (Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewSnareTableQueryString
        {
            get { return @"INSERT INTO Snares (Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewStentTableQueryString
        {
            get { return @"INSERT INTO Stents (Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @SubTitle, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewWireTableQueryString
        {
            get { return @"INSERT INTO Wires (Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize ) VALUES (@Brand, @Title, @SubTitle, @Diameter, @Dimensions, @Cost, @RefNumber, @Category, @PackSize )"; }
        }

        public static string NewIrNursesTableQueryString
        {
            get { return @"INSERT INTO InterventionalNurses (UserName, Email, FirstName, Surname ) VALUES (@UserName, @Email, @FirstName, @Surname )"; }
        }
        #endregion

        private static List<Referrer> Referrers = new List<Referrer>();
        private static List<Radiologist> Radiologists = new List<Radiologist>();
        private static List<IrNurse> IrNurses = new List<IrNurse>();


        private static List<string> ProcedureLocations = new List<string>();
        private static List<string> DatabaseTables = new List<string>();
        private static List<string> SyncCategories = new List<string>();


        //Stats & Reports - QuickStats
        #region
        private static List<CaseStatsData> ListOfCaseStatsData = new List<CaseStatsData>();
        private static List<CaseItemCountHistoryStatsData> ListOfCaseItemCountHistoryStatsData = new List<CaseItemCountHistoryStatsData>();

        #endregion

        //Chart Data
        #region
        private static List<string> ChartTypes = new List<string>() { "Select a Chart Type...", "Line", "Bar" };
        private static List<string> LineAndBarChartYAxisOptions = new List<string>() { "Case Cost", "Number of Cases" };
        private static List<string> LineAndBarChartXAxisOptions = new List<string>() { "Per Week", "Per Month", "Per Year" };
        private static List<string> MonthTitles = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" };
        private static List<string> DateTimeMonthTitleStrings = new List<string>() { "01/01/2018 00:00:00", "01/02/2018 00:00:00", "01/03/2018 00:00:00", "01/04/2018 00:00:00", "01/05/2018 00:00:00", "01/06/2018 00:00:00", "01/07/2018 00:00:00", "01/08/2018 00:00:00", "01/09/2018 00:00:00", "01/10/2018 00:00:00", "01/11/2018 00:00:00", "01/12/2018 00:00:00" };

        #endregion


        //Inventory Item Lists
        #region
        private static List<Dilator> Dilators = new List<Dilator>();
        private static List<Sheath> Sheaths = new List<Sheath>();
        private static List<Catheter> Catheters = new List<Catheter>();
        private static List<Snare> Snares = new List<Snare>();
        private static List<Balloon> Balloons = new List<Balloon>();
        private static List<Stent> Stents = new List<Stent>();
        private static List<GuideWire> Wires = new List<GuideWire>();
        private static List<EmbolisationCoil> EmbolisationCoils = new List<EmbolisationCoil>();
        private static List<EmbolisationSystem> EmbolisationSystems = new List<EmbolisationSystem>();
        private static List<Dressing> Dressings = new List<Dressing>();
        private static List<Contrast> ContrastList = new List<Contrast>();
        private static List<MiscItem> Miscellaneous = new List<MiscItem>();

        #endregion

        private static Action[] Methods = {
            () => PopulateListOfSyncCategories(),
            () => PopulateListOfDbTables(),
            () => PopulateListOfProcedureLocations(),
            () => PopulateRadiologistList(),
            () => PopulateInterventionalNursesList(),
            () => PopulateReferrerList(),
            () => PopulateListOfDilators(),
            () => PopulateListOfSheaths(),
            () => PopulateListOfCatheters(),
            () => PopulateListOfBalloons(),
            () => PopulateListOfContrast(),
            () => PopulateListOfDressings(),
            () => PopulateListOfEmbolisationCoils(),
            () => PopulateListOfEmbolisationSystems(),
            () => PopulateListOfWires(),
            () => PopulateListOfMiscItems(),
            () => PopulateListOfSnares(),
            () => PopulateListOfStents(),
            () => PopulateCaseStatsTableData(),
            () => PopulateListOfCaseItemCountHistoryStatsData()
        };


        //Enumerations
        #region
        public enum EnumRadOrReferrer { ID, Prefix, FirstName, Surname, Specialty };
        public enum EnumDilator { ID, Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumSheath { ID, Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumCatheter { ID, Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumBalloon { ID, Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumEmbolisationSystem { ID, Brand, Title, SubTitle, Cost, RefNumber, Category, PackSize };
        public enum EnumWire { ID, Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumDressing { ID, Brand, Title, Cost, RefNumber, Category, PackSize };
        public enum EnumStent { ID, Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumMisc { ID, Brand, Title, SubTitle, Cost, RefNumber, Category, PackSize };
        public enum EnumCoil { ID, Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumContrast { ID, Brand, Strength, Volume, Cost, RefNumber, Category, PackSize };
        public enum EnumSnare { ID, Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        public enum EnumItemOrder { ID, Description, RefNumber, Quantity, PackSize, Cost, Category };
        public enum EnumMiscData { ID, DbTables, CaseLocations, SyncCategories };
        public enum EnumNurses { ID, userName, email, firstName, surname };
        public enum EnumCaseStatsData { ID, date, cost, name, rad };
        public enum EnumCaseItemCountStats { ID, Date, Name, Description, QuantityUsed, UnitCost, TotalCost };

        #endregion


        public static async void Setup(TaskScheduler uiContext, ProgressBarWindow pbWindow)
        {
            IUpdateStartupProgressBar progress = pbWindow;

            //Background Thread
            Task task = Task.Run(() =>
            {
                //UI Thread
                Task.Factory.StartNew(() =>
                {
                    progress.UpdateCurrentTask("Setting up....");
                    progress.UpdateProgressMax(Methods.Length);

                }, CancellationToken.None, TaskCreationOptions.None, uiContext);

                int count = 0;
                foreach (Action method in Methods)
                {
                    count++;
                    method();

                    //UI Thread
                    Task.Factory.StartNew(() =>
                    {
                        progress.UpdateProgressBar(count);
                        progress.UpdateTaskDetails($"Retrieving dataset {count}/{Methods.Length}...");

                    }, CancellationToken.None, TaskCreationOptions.None, uiContext);
                }

                //UI Thread
                Task.Factory.StartNew(() =>
                {
                    progress.UpdateTaskDetails($"Retrieving dataset {count}/{Methods.Length}...COMPLETED!");

                }, CancellationToken.None, TaskCreationOptions.None, uiContext);

                Thread.Sleep(2000);
            });

            await task;

            //Sleep to allow better view of 'splash screen' image.
            Thread.Sleep(3000);
            pbWindow.Close();
        }


        //Populate Methods
        #region
        public static void PopulateListOfProcedureLocations()
        {
            //Query the database.
            string sqlQuery = @"SELECT CaseLocations FROM MiscData";
            ProcedureLocations = DBController.GetProcedureLocations(sqlQuery);
        }

        public static void PopulateListOfDbTables()
        {
            //Query the database.
            string sqlQuery = @"SELECT DbTables FROM MiscData";
            DatabaseTables = DBController.GetDbTables(sqlQuery);
        }

        public static void PopulateListOfSyncCategories()
        {
            //Query the database.
            string sqlQuery = @"SELECT SyncCategories FROM MiscData";
            SyncCategories = DBController.GetSyncCategories(sqlQuery);
        }

        public static void PopulateListOfStents()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Stents";
            var data = DBController.GetStents(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumStent.ID];
                string Brand = data[i][(int)EnumStent.Brand];
                string Title = data[i][(int)EnumStent.Title];
                string SubTitle = data[i][(int)EnumStent.SubTitle];
                string Diameter = data[i][(int)EnumSnare.Diameter];
                string Dimensions = data[i][(int)EnumStent.Dimensions];
                string Cost = data[i][(int)EnumStent.Cost];
                string RefNumber = data[i][(int)EnumStent.RefNumber];
                string Category = data[i][(int)EnumStent.Category];
                string PackSize = data[i][(int)EnumStent.PackSize];

                Stent stent = new Stent(Id, Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize);

                Stents.Add(stent);

            }
        }

        public static void PopulateListOfSnares()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Snares";
            var data = DBController.GetSnares(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumSnare.ID];
                string Brand = data[i][(int)EnumSnare.Brand];
                string Title = data[i][(int)EnumSnare.Title];
                string Diameter = data[i][(int)EnumSnare.Diameter];
                string Dimensions = data[i][(int)EnumSnare.Dimensions];
                string Cost = data[i][(int)EnumSnare.Cost];
                string RefNumber = data[i][(int)EnumSnare.RefNumber];
                string Category = data[i][(int)EnumSnare.Category];
                string PackSize = data[i][(int)EnumSnare.PackSize];

                Snare snare = new Snare(Id, Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize);

                Snares.Add(snare);

            }
        }

        public static void PopulateListOfEmbolisationCoils()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Coils";
            var data = DBController.GetCoils(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumCoil.ID];
                string Brand = data[i][(int)EnumCoil.Brand];
                string Title = data[i][(int)EnumCoil.Title];
                string SubTitle = data[i][(int)EnumCoil.SubTitle];
                string Diameter = data[i][(int)EnumCoil.Diameter];
                string Dimensions = data[i][(int)EnumCoil.Dimensions];
                string Cost = data[i][(int)EnumCoil.Cost];
                string Category = data[i][(int)EnumCoil.Category];
                string RefNumber = data[i][(int)EnumCoil.RefNumber];
                string PackSize = data[i][(int)EnumCoil.PackSize];

                EmbolisationCoil coil = new EmbolisationCoil(Id, Brand, Title, SubTitle, Diameter, Dimensions, Cost, Category, RefNumber, PackSize);

                EmbolisationCoils.Add(coil);

            }
        }

        public static void PopulateListOfEmbolisationSystems()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM EmbolisationSystems";
            var data = DBController.GetEmbolisationSystems(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumEmbolisationSystem.ID];
                string Brand = data[i][(int)EnumEmbolisationSystem.Brand];
                string Title = data[i][(int)EnumEmbolisationSystem.Title];
                string SubTitle = data[i][(int)EnumEmbolisationSystem.SubTitle];
                string Cost = data[i][(int)EnumEmbolisationSystem.Cost];
                string RefNumber = data[i][(int)EnumEmbolisationSystem.RefNumber];
                string Category = data[i][(int)EnumEmbolisationSystem.Category];
                string PackSize = data[i][(int)EnumEmbolisationSystem.PackSize];

                EmbolisationSystem embolisationSystem = new EmbolisationSystem(Id, Brand, Title, SubTitle, Cost, RefNumber, Category, PackSize);

                EmbolisationSystems.Add(embolisationSystem);

            }
        }

        public static void PopulateListOfWires()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Wires";
            var data = DBController.GetWires(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumWire.ID];
                string Brand = data[i][(int)EnumWire.Brand];
                string Title = data[i][(int)EnumWire.Title];
                string SubTitle = data[i][(int)EnumWire.SubTitle];
                string Diameter = data[i][(int)EnumWire.Diameter];
                string Dimensions = data[i][(int)EnumWire.Dimensions];
                string Cost = data[i][(int)EnumWire.Cost];
                string RefNumber = data[i][(int)EnumWire.RefNumber];
                string Category = data[i][(int)EnumWire.Category];
                string PackSize = data[i][(int)EnumWire.PackSize];

                GuideWire wire = new GuideWire(Id, Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize);

                Wires.Add(wire);

            }
        }

        public static void PopulateListOfDressings()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Dressings";
            var data = DBController.GetDressings(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumDressing.ID];
                string Brand = data[i][(int)EnumDressing.Brand];
                string Title = data[i][(int)EnumDressing.Title];
                string Cost = data[i][(int)EnumDressing.Cost];
                string RefNumber = data[i][(int)EnumDressing.RefNumber];
                string Category = data[i][(int)EnumDressing.Category];
                string PackSize = data[i][(int)EnumDressing.PackSize];

                Dressing dressing = new Dressing(Id, Brand, Title, Cost, RefNumber, Category, PackSize);

                Dressings.Add(dressing);

            }
        }

        public static void PopulateListOfContrast()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Contrast";
            var data = DBController.GetContrast(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumContrast.ID];
                string Brand = data[i][(int)EnumContrast.Brand];
                string Strength = data[i][(int)EnumContrast.Strength];
                string Volume = data[i][(int)EnumContrast.Volume];
                string Cost = data[i][(int)EnumContrast.Cost];
                string RefNumber = data[i][(int)EnumContrast.RefNumber];
                string Category = data[i][(int)EnumContrast.Category];
                string PackSize = data[i][(int)EnumContrast.PackSize];

                Contrast contrast = new Contrast(Id, Brand, Strength, Volume, Cost, RefNumber, Category, PackSize);

                ContrastList.Add(contrast);

            }
        }

        public static void PopulateListOfBalloons()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Balloons";
            var data = DBController.GetBalloons(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumBalloon.ID];
                string Brand = data[i][(int)EnumBalloon.Brand];
                string Title = data[i][(int)EnumBalloon.Title];
                string Diameter = data[i][(int)EnumBalloon.Diameter];
                string Dimensions = data[i][(int)EnumBalloon.Dimensions];
                string Cost = data[i][(int)EnumBalloon.Cost];
                string RefNumber = data[i][(int)EnumBalloon.RefNumber];
                string Category = data[i][(int)EnumBalloon.Category];
                string PackSize = data[i][(int)EnumBalloon.PackSize];

                Balloon balloon = new Balloon(Id, Brand, Title, Diameter, Dimensions, Cost, RefNumber, Category, PackSize);

                Balloons.Add(balloon);

            }
        }

        public static void PopulateListOfCatheters()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Catheters";
            var data = DBController.GetCatheters(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumCatheter.ID];
                string Brand = data[i][(int)EnumCatheter.Brand];
                string Title = data[i][(int)EnumCatheter.Title];
                string SubTitle = data[i][(int)EnumCatheter.SubTitle];
                string Diameter = data[i][(int)EnumCatheter.Diameter];
                string Dimensions = data[i][(int)EnumCatheter.Dimensions];
                string Cost = data[i][(int)EnumCatheter.Cost];
                string RefNumber = data[i][(int)EnumCatheter.RefNumber];
                string Category = data[i][(int)EnumCatheter.Category];
                string PackSize = data[i][(int)EnumCatheter.PackSize];



                Catheter catheter = new Catheter(Id, Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize);

                Catheters.Add(catheter);

            }
        }

        public static void PopulateListOfSheaths()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Sheaths";
            var data = DBController.GetSheaths(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                //private enum EnumSheath { ID, Brand, Category, Title, Diameter, Dimensions, Cost, RefNumber, PackSize };

                string Id = data[i][(int)EnumSheath.ID];
                string Brand = data[i][(int)EnumSheath.Brand];
                string Category = data[i][(int)EnumSheath.Category];
                string Title = data[i][(int)EnumSheath.Title];
                string Diameter = data[i][(int)EnumSheath.Diameter];
                string Dimensions = data[i][(int)EnumSheath.Dimensions];
                string Cost = data[i][(int)EnumSheath.Cost];
                string RefNumber = data[i][(int)EnumSheath.RefNumber];
                string PackSize = data[i][(int)EnumSheath.PackSize];

                Sheath sheath = new Sheath(Id, Brand, Title, Cost, Category, RefNumber, PackSize, Diameter, Dimensions);

                Sheaths.Add(sheath);

            }
        }

        public static void PopulateListOfMiscItems()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Misc";
            var data = DBController.GetMiscItems(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumMisc.ID];
                string Brand = data[i][(int)EnumMisc.Brand];
                string Title = data[i][(int)EnumMisc.Title];
                string SubTitle = data[i][(int)EnumMisc.SubTitle];
                string Cost = data[i][(int)EnumMisc.Cost];
                string RefNumber = data[i][(int)EnumMisc.RefNumber];
                string Category = data[i][(int)EnumMisc.Category];
                string PackSize = data[i][(int)EnumMisc.PackSize];

                MiscItem miscItem = new MiscItem(Id, Brand, Title, SubTitle, Cost, RefNumber, Category, PackSize);

                Miscellaneous.Add(miscItem);

            }
        }

        public static void PopulateListOfDilators()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Dilators";
            var data = DBController.GetDilators(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumDilator.ID].Trim();
                string Category = data[i][(int)EnumDilator.Category].Trim();
                string Brand = data[i][(int)EnumDilator.Brand].Trim();
                string Title = data[i][(int)EnumDilator.Title].Trim();
                string Cost = data[i][(int)EnumDilator.Cost].ToString().Trim();
                string RefNumber = data[i][(int)EnumDilator.RefNumber].Trim();
                string Diameter = data[i][(int)EnumDilator.Diameter].Trim();
                string Dimensions = data[i][(int)EnumDilator.Dimensions].Trim();
                string PackSize = data[i][(int)EnumDilator.PackSize].Trim();

                Dilator dilator = new Dilator(Id, Category, Brand, Title, Cost, RefNumber, Diameter, Dimensions, PackSize);

                Dilators.Add(dilator);

            }
        }

        public static void PopulateReferrerList()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Referrers";
            var data = DBController.GetReferrers(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumRadOrReferrer.ID];
                string Prefix = data[i][(int)EnumRadOrReferrer.Prefix];
                string FirstName = data[i][(int)EnumRadOrReferrer.FirstName];
                string Surname = data[i][(int)EnumRadOrReferrer.Surname];
                string Specialty = data[i][(int)EnumRadOrReferrer.Specialty];

                Referrer referrer = new Referrer(Int32.Parse(Id), Prefix, FirstName, Surname, Specialty);

                Referrers.Add(referrer);
                
            }
        }

        public static void PopulateRadiologistList()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM Radiologists";
            var data = DBController.GetRadiologists(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumRadOrReferrer.ID];
                string Prefix = data[i][(int)EnumRadOrReferrer.Prefix];
                string FirstName = data[i][(int)EnumRadOrReferrer.FirstName];
                string Surname = data[i][(int)EnumRadOrReferrer.Surname];
                string Specialty = data[i][(int)EnumRadOrReferrer.Specialty];

                Radiologist rad = new Radiologist(int.Parse(Id), Prefix, FirstName, Surname, Specialty);

                Radiologists.Add(rad);
            }
        }

        public static void PopulateInterventionalNursesList()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM InterventionalNurses";
            var data = DBController.GetInterventionalNurses(sqlQuery);

            for (int i = 0; i < data.Count; i++)
            {
                string Id = data[i][(int)EnumNurses.ID];
                string email = data[i][(int)EnumNurses.email];
                string userName = data[i][(int)EnumNurses.userName];
                string firstName = data[i][(int)EnumNurses.firstName];
                string surname = data[i][(int)EnumNurses.surname];

                IrNurse nurse = new IrNurse(int.Parse(Id), email, userName, firstName, surname);

                IrNurses.Add(nurse);
            }
        }

        public static void PopulateCaseStatsTableData()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM CaseStats";
            ListOfCaseStatsData = DBController.GetCaseStatsData(sqlQuery);
        }

        public static void PopulateListOfCaseItemCountHistoryStatsData()
        {
            //Query the database.
            string sqlQuery = "SELECT * FROM CaseItemCountHistory";
            ListOfCaseItemCountHistoryStatsData = DBController.CaseItemCountHistoryStatsData(sqlQuery);
        }
        #endregion


        //Getters
        #region

        //Populated via FetchMiscData

        public static List<string> GetDatabaseTables()
        {
            return DatabaseTables;
        }

        public static List<string> GetSyncCategories()
        {
            return SyncCategories;
        }

        public static List<string> GetProcedureLocations()
        {
            return ProcedureLocations;
        }

        public static List<(int, int)> GetCellPositions(string table)
        {
            List<(int, int)> cellPositions = new List<(int, int)>();

            switch (table)
            {
                case "Balloons":
                    cellPositions = BalloonTitleCellLocations;
                    break;

                case "Catheters":
                    cellPositions = CatheterTitleCellLocations;
                    break;

                case "Coils":
                    cellPositions = CoilTitleCellLocations;
                    break;

                case "Contrast":
                    cellPositions = ContrastTitleCellLocations;
                    break;

                case "Dilators":
                    cellPositions = DilatorTitleCellLocations;
                    break;

                case "Dressings":
                    cellPositions = DressingTitleCellLocations;
                    break;

                case "EmbolisationSystems":
                    cellPositions = EmbolisationSystemTitleCellLocations;
                    break;

                case "Misc":
                    cellPositions = MiscTitleCellLocations;
                    break;

                case "Radiologists":
                    cellPositions = RadiologistTitleCellLocations;
                    break;

                case "Referrers":
                    cellPositions = ReferrerTitleCellLocations;
                    break;

                case "Sheaths":
                    cellPositions = SheathTitleCellLocations;
                    break;

                case "Snares":
                    cellPositions = SnareTitleCellLocations;
                    break;

                case "Stents":
                    cellPositions = StentTitleCellLocations;
                    break;

                case "Wires":
                    cellPositions = WireTitleCellLocations;
                    break;

                case "MiscData":
                    cellPositions = MiscDataTitleCellLocations;
                    break;

                case "InterventionalNurses":
                    cellPositions = InterventionalNursesTitleCellLocations;
                    break;

                default:
                    MessageBox.Show($"Unknown table name supplied: {table} - in StaticData.GetCellPositions()");
                    break;
            }

            return cellPositions;
        }

        public static List<Catheter> GetCatheters()
        {
            return Catheters;
        }

        public static List<Sheath> GetSheaths()
        {
            return Sheaths;
        }

        public static List<Dilator> GetDilators()
        {
            return Dilators;
        }

        public static List<Referrer> GetReferrers()
        {
            return Referrers;
        }

        public static List<Radiologist> GetRadiologists()
        {
            return Radiologists;
        }

        public static List<Balloon> GetBalloons()
        {
            return Balloons;
        }

        public static List<Stent> GetStents()
        {
            return Stents;
        }

        public static List<GuideWire> GetWires()
        {
            return Wires;
        }

        public static List<EmbolisationCoil> GetCoils()
        {
            return EmbolisationCoils;
        }

        public static List<Dressing> GetDressings()
        {
            return Dressings;
        }

        public static List<MiscItem> GetMiscItems()
        {
            return Miscellaneous;
        }

        public static List<EmbolisationSystem> GetEmbolisationSystems()
        {
            return EmbolisationSystems;
        }

        public static List<Snare> GetSnares()
        {
            return Snares;
        }

        public static List<Contrast> GetContrast()
        {
            return ContrastList;
        }

        public static string GetImagePath(string category)
        {
            string imagePath = "";

            switch (category)
            {

                case "Balloon":
                    imagePath = @"\Images\Interventional Items\Balloons\";
                    break;

                case "Catheter":
                    imagePath = @"\Images\Interventional Items\Catheters\";
                    break;

                case "Stent":
                    imagePath = @"\Images\Interventional Items\Stents\";
                    break;

                case "Coils":
                    imagePath = @"\Images\Interventional Items\Coils\";
                    break;

                case "Sheath":
                    imagePath = @"\Images\Interventional Items\Sheaths\";
                    break;

                case "Contrast":
                    imagePath = @"\Images\Interventional Items\Contrast\";
                    break;

                case "Dilator":
                    imagePath = @"\Images\Interventional Items\Dilators\";
                    break;

                case "Dressing":
                    imagePath = @"\Images\Interventional Items\Dressings\";
                    break;

                case "Embolisation":
                    imagePath = @"\Images\Interventional Items\Embolisation\";
                    break;

                case "Embolisation System":
                    imagePath = @"\Images\Interventional Items\Embolisation Systems\";
                    break;

                case "MiscItems":
                    imagePath = @"\Images\Interventional Items\Misc\";
                    break;

                case "Snare":
                    imagePath = @"\Images\Interventional Items\Snares\";
                    break;

                case "Wire":
                    imagePath = @"\Images\Interventional Items\Wires\";
                    break;

                case "Testwire":
                    imagePath = "default";
                    break;

                default:
                    break;
            }

            return imagePath;
        }

        public static BitmapImage GetImage(string refNum, string category)
        {
            BitmapImage myImage;
            Uri ImagePath;
            string imagePath = GetImagePath(category);

            //check if the path point to a valid existing file. If not, display default image.
            if (imagePath != "default")
            {
                ImagePath = new Uri(imagePath + refNum + ".jpg", UriKind.Relative);
                myImage = new BitmapImage(ImagePath);
            }
            else
            {                
                ImagePath = new Uri(@"\Images\imagePlaceholder.jpg", UriKind.Relative);
                myImage = new BitmapImage(ImagePath);
            }

            return myImage;

        }

        public static List<string> GetChartTypes()
        {
            return ChartTypes;
        }

        public static List<CaseStatsData> GetListOfCaseStatsData()
        {
            return ListOfCaseStatsData;
        }

        public static List<CaseItemCountHistoryStatsData> GetListOfCaseItemCountHistoryStatsData()
        {
            return ListOfCaseItemCountHistoryStatsData;
        }

        public static List<string> GetLineAndBarChartYAxisOptions()
        {
            return LineAndBarChartYAxisOptions;
        }

        public static List<string> GetLineAndBarChartXAxisOptions()
        {
            return LineAndBarChartXAxisOptions;
        }

        public static List<string> GetMonthTitles()
        {
            return MonthTitles;
        }

        public static List<string> GetDateTimeMonthTitleStrings()
        {
            return DateTimeMonthTitleStrings;
        }

        #endregion
			   		 	  	  	   	



    }//END OF CLASS
}
