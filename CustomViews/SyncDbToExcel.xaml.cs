using InterventionalCostings.Static_Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;

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
using EnumMiscData = InterventionalCostings.Static_Data.StaticData.EnumMiscData;
using System.IO;
using System.Threading;

namespace InterventionalCostings.CustomViews
{
    /// <summary>
    /// Interaction logic for SyncExcelToDB.xaml
    /// </summary>
    public partial class SyncDbToExcel : System.Windows.Window
    {
        List<string> tables = null;
        public TaskScheduler uiContext;

        public Double ProgressBarMax
        {
            get { return SyncProgressBar.Maximum; }
            set { SyncProgressBar.Maximum = value; }
        }

        public SyncDbToExcel()
        {
            InitializeComponent();

            tables = StaticData.GetSyncCategories();

            if (tables[0] != "All Tables")
            {
                tables.Insert(0, "All Tables");
                tables.Remove("testWires");
            }

            SyncComboBox.ItemsSource = tables;
            SyncComboBox.Text = SyncComboBox.Items[0].ToString();

            CustomTitleBar titleBar = new CustomTitleBar();
            titleBar.TitleBarTitleText.Content = "Sync Database to Excel File(s)";
            MainGrid.Children.Add(titleBar);

        }

        private void SyncCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void SyncNowButtonClicked(object sender, RoutedEventArgs e)
        {
            //make sure user cannot click if sync is already in progress.
            SyncButton.Content = @"Syncing Now...";
            SyncButton.IsEnabled = false;
            SyncCancelButton.IsEnabled = false;
            SyncComboBox.IsEnabled = false;

            uiContext = TaskScheduler.FromCurrentSynchronizationContext();

            if (SyncComboBox.Text == "All Tables")
            {
                SyncProgressBar.Maximum = tables.Count;

                for (int i = 1; i < tables.Count; i++)
                {
                    string currentTable = tables[i].Replace(" ","");

                    Task task = Task.Run(() => {

                        SyncProductToExcelFile(currentTable);
                    });

                    await task;
                    SyncProgressBar.Value = i;
                }

                SyncProgressBar.Value = SyncProgressBar.Maximum;
            }
            else
            {
                SyncProgressBar.Maximum = 100;
                SyncProgressBar.Value = 0;

                CurrentTask.Content = $"Syncing {SyncComboBox.Text} to Excel File...";
                string currentTable = SyncComboBox.Text.Replace(" ",""); ;

                Task task = Task.Run(() => {

                    SyncProductToExcelFile(currentTable);

                });
                await task;

                SyncProgressBar.Value = SyncProgressBar.Maximum;
            }

            //Re-enable the sync button and change the Cancel button to 'Close'.
            CurrentTask.Content = @"Sync Complete.";
            TaskDetails.Content = @"Sync Complete.";

            SyncButton.IsEnabled = true;
            SyncButton.Content = @"Sync Now!";
            SyncCancelButton.IsEnabled = true;
            SyncComboBox.IsEnabled = true;
        }


        private void SyncProductToExcelFile(string table)
        {
            Task.Factory.StartNew(() => {

                CurrentTask.Content = $"Syncing {table} to Excel File...";

            }, CancellationToken.None, TaskCreationOptions.None, uiContext);

            string tableNoWhiteSpace = table.Replace(" ", "");

            List<List<string>> ProductFromDb = GetProductFromDb(tableNoWhiteSpace);     
            
            Assembly asm = Assembly.GetExecutingAssembly();

            string currentDirectory = Directory.GetCurrentDirectory();
            string DestFilePath = string.Format(@"{0}\{1}\{2}.xlsx", currentDirectory, StaticData.DestinationExcelFilesDirectory, tableNoWhiteSpace);
            string ResourceFilePath = string.Format("{0}.{1}.{2}.xlsx", asm.GetName().Name, StaticData.MasterExcelFilesDirectory, tableNoWhiteSpace);
            
            //copy over Master ?? file to ExcelFiles folder - overwrite file if already existing.
            Stream SourceStream = asm.GetManifestResourceStream(ResourceFilePath);

            string DestDirectory = string.Format(@"{0}\{1}\", currentDirectory, StaticData.DestinationExcelFilesDirectory);
            Directory.CreateDirectory(DestDirectory);

            Task.Factory.StartNew(() => {

                TaskDetails.Content = "Copying Master File...";

            }, CancellationToken.None, TaskCreationOptions.None, uiContext);

            //Copy master file over to ExcelFiles Directory (overwrite existing if necessary).
            SaveStreamToFile(DestFilePath, SourceStream);

            Task.Factory.StartNew(() => {

                TaskDetails.Content = "Writing Data to File...";

            }, CancellationToken.None, TaskCreationOptions.None, uiContext);
                        
            //Write data to newly created file.
            WriteDataToExcelFile(ProductFromDb, DestFilePath);

            Task.Factory.StartNew(() => {

                TaskDetails.Content = "Writing Data to File...DONE!";

            }, CancellationToken.None, TaskCreationOptions.None, uiContext);


        }

        private List<List<string>> GetProductFromDb(string tableNoWhiteSpace)
        {
            List<List<string>> product;
            string sqlQuery = string.Format(@"SELECT * FROM {0}", tableNoWhiteSpace);

            switch (tableNoWhiteSpace)
            {
                case "Balloons":
                    product = DBController.GetBalloons(sqlQuery);
                    break;

                case "Catheters":
                    product = DBController.GetCatheters(sqlQuery);
                    break;

                case "Snares":
                    product = DBController.GetSnares(sqlQuery);
                    break;

                case "Wires":
                    product = DBController.GetWires(sqlQuery);
                    break;

                case "Contrast":
                    product = DBController.GetContrast(sqlQuery);
                    break;

                case "Dressings":
                    product = DBController.GetDressings(sqlQuery);
                    break;

                case "Dilators":
                    product = DBController.GetDilators(sqlQuery);
                    break;

                case "Misc":
                    product = DBController.GetMiscItems(sqlQuery);
                    break;

                case "Coils":
                    product = DBController.GetCoils(sqlQuery);
                    break;

                case "EmbolisationSystems":
                    product = DBController.GetEmbolisationSystems(sqlQuery);
                    break;

                case "Radiologists":
                    product = DBController.GetRadiologists(sqlQuery);
                    break;

                case "Referrers":
                    product = DBController.GetReferrers(sqlQuery);
                    break;

                case "Stents":
                    product = DBController.GetStents(sqlQuery);
                    break;

                case "Sheaths":
                    product = DBController.GetSheaths(sqlQuery);
                    break;

                case "MiscData":
                    product = DBController.GetMiscData(sqlQuery);
                    break;

                case "InterventionalNurses":
                    product = DBController.GetInterventionalNurses(sqlQuery);
                    break;

                default:
                    MessageBox.Show(@"Supplied table not recognised: " + tableNoWhiteSpace);
                    product = null;
                    break;
            }

            return product;
        }

        private void WriteDataToExcelFile(List<List<string>> InfoToAdd, string DestFile)
        {
            Excel.Application xlApp = new Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }
            else
            {
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = Missing.Value;

                xlWorkBook = xlApp.Workbooks.Open(DestFile, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("Sheet1");

                //iterate through list and insert into excel file.
                int currentCol = 1;
                int currentRow = 2;
                foreach (List<string> product in InfoToAdd)
                {
                    Task.Factory.StartNew(() => {

                        TaskDetails.Content = $"Writing Data to File...(Adding Product {currentRow - 2}/{InfoToAdd.Count})";

                    }, CancellationToken.None, TaskCreationOptions.None, uiContext);


                    for (int i = 1; i < product.Count; i++)
                    {
                        //insert each row into excel file.
                        xlWorkSheet.Cells[currentRow, currentCol] = product[i];
                        currentCol++;
                    }

                    Task.Factory.StartNew(() => {

                        TaskDetails.Content = $"Writing Data to File... (Adding Product {currentRow - 2}/{InfoToAdd.Count}...DONE!)";

                    }, CancellationToken.None, TaskCreationOptions.None, uiContext);

                    currentCol = 1;
                    currentRow++;
                }

                xlWorkBook.Save();
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }
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


    }
}
