using InterventionalCostings.Static_Data;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data.SqlClient;
using System.Threading;
using System.Text.RegularExpressions;

namespace InterventionalCostings.CustomViews
{
    /// <summary>
    /// Interaction logic for SyncExcelToDb.xaml
    /// </summary>
    public partial class SyncExcelToDb : System.Windows.Window, IUpdateProgressBar
    {
        public List<string> Tables { get; set; }
        public TaskScheduler uiContext;

        public Double ProgressBarMax
        {
            get { return SyncProgressBar.Maximum; }
            set { SyncProgressBar.Maximum = value; }
        }

        string[] ColumnHeadings = { "Brand", "Title", "SubTitle", "Diameter", "Dimensions", "Cost", "RefNumber", "Category", "PackSize" };
        enum ColTitle { Brand, Title, SubTitle, Diameter, Dimensions, Cost, RefNumber, Category, PackSize };
        string ExcelFilesDirectory = StaticData.DestinationExcelFilesDirectory;

        public SyncExcelToDb()
        {
            InitializeComponent();

            Tables = StaticData.GetSyncCategories();

            if (Tables[0] != "All Tables")
            {
                Tables.Insert(0, "All Tables");
                Tables.Remove("testWires");
            }

            SyncComboBox.ItemsSource = Tables;
            SyncComboBox.Text = SyncComboBox.Items[0].ToString();

            CustomTitleBar titleBar = new CustomTitleBar();
            titleBar.TitleBarTitleText.Content = "Sync Excel Files(s) to Database";
            MainGrid.Children.Add(titleBar);

        }

        private async void SyncNowButtonClicked(object sender, RoutedEventArgs e)
        {
            SyncProgressBar.Value = 0;

            //make sure user cannot click if sync is already in progress.
            SyncButton.Content = @"Syncing Now...";
            SyncButton.IsEnabled = false;
            SyncCancelButton.IsEnabled = false;
            SyncComboBox.IsEnabled = false;

            uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            Assembly asm = Assembly.GetExecutingAssembly();
            string ConnectionString = DBController.ConnectionString;
            string ExcelFileName = "";

            if (SyncComboBox.Text == "All Tables")
            {
                //SyncComboBox.SelectedIndex = 1;
                //ExcelFileName = SyncComboBox.Text;
                string FullExcelFilesDirPath = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), ExcelFilesDirectory);

                if (Directory.Exists(FullExcelFilesDirPath))
                {
                    //MessageBox.Show(string.Format("{0} Exists!", FullExcelFilesDirPath));
                    string[] files = Directory.GetFiles(FullExcelFilesDirPath);

                    //Open corresponding excel file
                    Excel.Application xlApp = new Excel.Application();
                    if (xlApp == null)
                    {
                        MessageBox.Show("Excel is not properly installed!!");
                        return;
                    }
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;

                    string filePath;
                    string fileName;
                    List<(int, int)> cellPositions =  new List<(int, int)>();
                    List<List<string>> CellValues = new List<List<string>>();

                    //iterate over each file, extract data and send to DBController for writing to database.
                    foreach (string file in files)
                    {
                        //filePath = string.Format("{0}\\{1}.xlsx", FullExcelFilesDirPath, file);
                        filePath = file;
                        fileName = System.IO.Path.GetFileNameWithoutExtension(file);

                        cellPositions = StaticData.GetCellPositions(fileName);

                        object misValue = Missing.Value;
                        xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("Sheet1");

                        CurrentTask.Content = string.Format($"Validating Excel File: {fileName}...");

                        //Validate File Data
                        var dataIsValid = ValidateExcelFileData(ExcelFileName, xlWorkSheet, cellPositions);

                        if (dataIsValid == (0,0,""))
                        {
                            CurrentTask.Content = string.Format($"Validating Excel File: {fileName}...DONE!");

                            if (DBController.CheckIfTableExists(fileName))
                            {
                                DBController.ClearTable(fileName);

                                CurrentTask.Content = string.Format($"Retrieving data from Excel File: {fileName}...");
                                CellValues = GetCellValues(fileName, cellPositions, xlWorkSheet);

                                CurrentTask.Content = string.Format($"Populating table: {fileName}...");

                                Task task = Task.Run(() =>
                                {
                                    DBController.PopulateDbTable(fileName, CellValues, uiContext, this);
                                });

                                await task;
                            }
                            else
                            {
                                CurrentTask.Content = string.Format($"Retrieving data from Excel File: {fileName}...");
                                CellValues = GetCellValues(fileName, cellPositions, xlWorkSheet);

                                //Background Thread
                                Task task = Task.Run(() =>
                                {
                                    //UI Thread
                                    Task.Factory.StartNew(() =>
                                    {
                                        CurrentTask.Content = string.Format($"Creating table: {fileName}...");
                                    });

                                    DBController.CreateTable(fileName);

                                    //UI Thread
                                    Task.Factory.StartNew(() =>
                                    {
                                        CurrentTask.Content = string.Format($"Populating table: {fileName}...");
                                    });

                                    DBController.PopulateDbTable(fileName, CellValues, uiContext, this);
                                });

                                await task;
                            }

                            //Save WorkBook
                            xlWorkBook.Save();
                            xlWorkBook.Close(true, misValue, misValue);

                            Marshal.ReleaseComObject(xlWorkSheet);
                            Marshal.ReleaseComObject(xlWorkBook);



                        }
                        else
                        {
                            //Invalid data detected.
                            ShowInvalidExcelDataMessageBox(fileName, dataIsValid);

                            //Save WorkBook
                            xlWorkBook.Save();
                            xlWorkBook.Close(true, misValue, misValue);

                            Marshal.ReleaseComObject(xlWorkSheet);
                            Marshal.ReleaseComObject(xlWorkBook);

                            //If Invalid data found, break out of foreach loop. Don't process any more excel files.
                            break;
                        }
                    }

                    //Quit Excel
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlApp);

                    //Re-enable the sync button and change the Cancel button to 'Close'.
                    CurrentTask.Content = @"Sync Complete.";
                    TaskDetails.Content = @"Sync Complete.";

                    SyncButton.IsEnabled = true;
                    SyncButton.Content = @"Sync Now!";
                    SyncCancelButton.IsEnabled = true;
                    SyncComboBox.IsEnabled = true;
                }
                else
                {
                    //Directory and/or files not found.
                    MessageBox.Show(string.Format("{0} does not exist. Please Sync Database to Excel Files First.", FullExcelFilesDirPath));
                }

                //Refresh Data Stores
                ProgressBarWindow progress = new ProgressBarWindow();
                progress.ShowDialog();
            }
            else
            {
                //Get rid of all white space
                ExcelFileName = SyncComboBox.Text.Replace(" ","");

                //Handle single file sync
                string FullExcelFilesDirPath = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), ExcelFilesDirectory);
                string filePath = string.Format("{0}\\{1}.xlsx", FullExcelFilesDirPath, ExcelFileName);

                if (Directory.Exists(FullExcelFilesDirPath))
                {
                    //Open corresponding excel file
                    Excel.Application xlApp = new Excel.Application();
                    if (xlApp == null)
                    {
                        MessageBox.Show("Excel is not properly installed!!");
                        return;
                    }
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    object misValue = Missing.Value;
                    xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("Sheet1");

                    //iterate over each file, extract data and send to DBController for writing to database.
                    List<(int, int)> cellPositions = StaticData.GetCellPositions(ExcelFileName);

                    //string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                    //Validate File Data
                    var dataIsValid = ValidateExcelFileData(ExcelFileName, xlWorkSheet, cellPositions);

                    //True tuple is (0,0, ""), false is anything else - duff cell locations are returned.
                    if (dataIsValid == (0,0, ""))
                    {
                        CurrentTask.Content = "Validating Data From File: " + ExcelFileName + ".xlsx...SUCCESS!";

                        List<List<string>> CellValues = GetCellValues(ExcelFileName, cellPositions, xlWorkSheet);

                        //If table DOES already exist.
                        if (DBController.CheckIfTableExists(ExcelFileName))
                        {
                            DBController.ClearTable(ExcelFileName);

                            CurrentTask.Content = string.Format($"Populating table: {ExcelFileName}...");

                            Task task = Task.Run(() => {

                                DBController.PopulateDbTable(ExcelFileName, CellValues, uiContext, this);

                            });

                            await task;
                        }
                        else
                        {
                            //If table does NOT already exist.
                            //New Thread
                            Task task = Task.Run(() => {

                                //UI Thread
                                Task.Factory.StartNew(() => {

                                    CurrentTask.Content = string.Format($"Creating table: {ExcelFileName}...");

                                }, CancellationToken.None, TaskCreationOptions.None, uiContext);

                                DBController.CreateTable(ExcelFileName);

                                //UI Thread
                                Task.Factory.StartNew(() => {

                                    CurrentTask.Content = string.Format($"Populating table: {ExcelFileName}...");

                                }, CancellationToken.None, TaskCreationOptions.None, uiContext);

                                DBController.PopulateDbTable(ExcelFileName, CellValues, uiContext, this);

                            });

                            await task;
                        }

                        //Re-enable the sync button and change the Cancel button to 'Close'.
                        CurrentTask.Content = @"Sync Complete.";
                        TaskDetails.Content = @"Sync Complete.";

                        SyncButton.IsEnabled = true;
                        SyncButton.Content = @"Sync Now!";
                        SyncCancelButton.IsEnabled = true;
                        SyncComboBox.IsEnabled = true;


                    }
                    else
                    {
                        //Invalid data detected.
                        ShowInvalidExcelDataMessageBox(ExcelFileName, dataIsValid);
                        //Allow the user to close the window.
                        SyncCancelButton.IsEnabled = true;

                    }

                    //Save WorkBook
                    xlWorkBook.Save();
                    xlWorkBook.Close(true, misValue, misValue);

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);

                    //Quit Excel
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlApp);

                    //Refresh Data Stores
                    ProgressBarWindow progress = new ProgressBarWindow();
                    progress.ShowDialog();
                }
                else
                {
                    //Directory/files not found.
                    MessageBox.Show(string.Format("{0} does not exist. Please Sync Database to Excel Files First.", FullExcelFilesDirPath));
                }
            }
        }

        private void ShowInvalidExcelDataMessageBox(string ExcelFileName, (int, int, string) dataIsValid)
        {
            MessageBox.Show("Invalid Data detected in " + ExcelFileName + ". Row " + dataIsValid.Item1 + " Column: " + dataIsValid.Item2 + " contains the following: " + dataIsValid.Item3 + ".");

        }

        private (int,int, string) ValidateExcelFileData(string fileName, Excel.Worksheet xlSheet, List<(int, int)> cellPositions)
        {
            CurrentTask.Content = "Validating Data From File: " + fileName + ".xlsx...";

            var badCellDataTuple = (0,0,"");
            bool dataIsValid = true;
            int badRow = 0;
            int badCol = 0;
            string badCellValue = "";

            int maxColcount = cellPositions.Count;
            Excel.Range UsedRange = xlSheet.UsedRange;
            int lastUsedRow = UsedRange.Row + UsedRange.Rows.Count - 1;
            string cellValue = "";

            switch (fileName)
            {
                case "Balloons":
                case "Dilators":
                case "Sheaths":

                    for (int rowNumber = 2; rowNumber <= lastUsedRow; rowNumber++)
                    {
                        for (int cellNumber = 1; cellNumber <= maxColcount; cellNumber++)
                        {
                            cellValue = (xlSheet.Cells[rowNumber, cellNumber] as Excel.Range).Value.ToString();

                            switch (cellNumber)
                            {
                                case 7:
                                case 2:
                                    //category - string - alphabetical - 7
                                    //title - string - alphabetical - 2
                                    dataIsValid = ValidateForAlphabetical(cellValue);
                                    break;

                                case 3:
                                case 4:
                                case 6:
                                case 1:
                                    //brand - string - alphanumerical - 1
                                    //diameter - string - alphanumeric - 3
                                    //dimensions - string - alphanumeric - 4
                                    //refNum - string - alphanumeric - 6
                                    dataIsValid = ValidateForAlphaNumeric(cellValue);
                                    break;

                                case 5:
                                    //cost - decimal - 5
                                    dataIsValid = ValidateForDecimal(cellValue);
                                    break;

                                case 8:
                                    //packsize - int - 8
                                    dataIsValid = ValidateForInt32(cellValue);
                                    break;

                                default:
                                    break;
                            }

                            if (dataIsValid == false)
                            {
                                badCellValue = cellValue;
                                badCol = cellNumber;
                                break;
                            }

                        }

                        if (dataIsValid == false)
                        {
                            badRow = rowNumber;
                            break;
                        }
                    }

                    break;

                case "Catheters":
                case "Coils":
                case "Stents":
                case "Wires":
                case "Snares":

                    for (int rowNumber = 2; rowNumber == lastUsedRow; rowNumber++)
                    {
                        for (int cellNumber = 1; cellNumber == maxColcount; cellNumber++)
                        {
                            cellValue = (xlSheet.Cells[rowNumber, cellNumber] as Excel.Range).Value;

                            switch (cellNumber)
                            {
                                case 1:
                                case 7:
                                    //brand - string - alphabetical - 1
                                    //subtitle - string - alphabetical - 3
                                    //category - string - alphabetical - 8
                                    dataIsValid = ValidateForAlphabetical(cellValue);
                                    break;

                                case 3:
                                case 4:
                                case 6:
                                case 2:
                                    //title - string - alphanumercal - 2
                                    //diameter - string - alphanumeric - 4
                                    //dimensions - string - alphanumeric - 5
                                    //refNum - string - alphanumeric - 7
                                    dataIsValid = ValidateForAlphaNumeric(cellValue);
                                    break;

                                case 5:
                                    //cost - decimal - 6
                                    dataIsValid = ValidateForDecimal(cellValue);
                                    break;

                                case 8:
                                    //packsize - int - 9
                                    dataIsValid = ValidateForInt32(cellValue);
                                    break;

                                default:
                                    break;
                            }

                            if (dataIsValid == false)
                            {
                                badCellValue = cellValue;
                                badCol = cellNumber;
                                break;
                            }

                        }

                        if (dataIsValid == false)
                        {
                            badRow = rowNumber;
                            break;
                        }
                    }
                    break;

                case "Contrast":

                    for (int rowNumber = 2; rowNumber == lastUsedRow; rowNumber++)
                    {
                        for (int cellNumber = 1; cellNumber == maxColcount; cellNumber++)
                        {
                            cellValue = (xlSheet.Cells[rowNumber, cellNumber] as Excel.Range).Value;

                            switch (cellNumber)
                            {
                                case 1:
                                case 7:
                                    //brand - string - alphabetical - 1
                                    //category - string - alphabetical - 6
                                    dataIsValid = ValidateForAlphabetical(cellValue);
                                    break;

                                case 3:
                                case 4:
                                case 6:
                                    //volume - string - alphanumeric - 3
                                    //refNum - string - alphanumeric - 5
                                    dataIsValid = ValidateForAlphaNumeric(cellValue);
                                    break;

                                case 5:
                                    //cost - decimal - 4
                                    dataIsValid = ValidateForDecimal(cellValue);
                                    break;

                                case 8:
                                case 2:
                                    //packsize - int - 7
                                    //strength - string - int - 2
                                    dataIsValid = ValidateForInt32(cellValue);
                                    break;

                                default:
                                    break;
                            }
                            if (dataIsValid == false)
                            {
                                badCellValue = cellValue;
                                badCol = cellNumber;
                                break;
                            }

                        }

                        if (dataIsValid == false)
                        {
                            badRow = rowNumber;
                            break;
                        }
                    }

                    break;

                case "Dressings":

                    for (int rowNumber = 2; rowNumber == lastUsedRow; rowNumber++)
                    {
                        for (int cellNumber = 1; cellNumber == maxColcount; cellNumber++)
                        {
                            cellValue = (xlSheet.Cells[rowNumber, cellNumber] as Excel.Range).Value;

                            switch (cellNumber)
                            {
                                case 5:
                                    //category - string - alphabetical - 5
                                    dataIsValid = ValidateForAlphabetical(cellValue);
                                    break;

                                case 1:
                                case 2:
                                case 4:
                                    //brand - string - alphanumerical - 1
                                    //title - string - alphanumerical - 2
                                    //refNum - string - alphanumeric - 4
                                    dataIsValid = ValidateForAlphaNumeric(cellValue);
                                    break;

                                case 3:
                                    //cost - decimal - 3
                                    dataIsValid = ValidateForDecimal(cellValue);
                                    break;

                                case 6:
                                    //packsize - int - 6
                                    dataIsValid = ValidateForInt32(cellValue);
                                    break;

                                default:
                                    break;
                            }
                            if (dataIsValid == false)
                            {
                                badCellValue = cellValue;
                                badCol = cellNumber;
                                break;
                            }

                        }

                        if (dataIsValid == false)
                        {
                            badRow = rowNumber;
                            break;
                        }
                    }
                    break;

                case "EmbolisationSystems":
                case "Misc":

                    for (int rowNumber = 2; rowNumber == lastUsedRow; rowNumber++)
                    {
                        for (int cellNumber = 1; cellNumber == maxColcount; cellNumber++)
                        {
                            cellValue = (xlSheet.Cells[rowNumber, cellNumber] as Excel.Range).Value;

                            switch (cellNumber)
                            {
                                case 6:
                                    //category - string - alphabetical - 6
                                    dataIsValid = ValidateForAlphabetical(cellValue);
                                    break;

                                case 1:
                                case 2:
                                case 3:
                                case 5:
                                    //brand - string - alphanumerical - 1
                                    //title - string - alphanumerical - 2
                                    //subtitle - string - alphanumerical - 3
                                    //refNum - string - alphanumeric - 5
                                    dataIsValid = ValidateForAlphaNumeric(cellValue);
                                    break;

                                case 4:
                                    //cost - decimal - 4
                                    dataIsValid = ValidateForDecimal(cellValue);
                                    break;

                                case 7:
                                    //packsize - int - 7
                                    dataIsValid = ValidateForInt32(cellValue);
                                    break;

                                default:
                                    break;
                            }
                            if (dataIsValid == false)
                            {
                                badCellValue = cellValue;
                                badCol = cellNumber;
                                break;
                            }

                        }

                        if (dataIsValid == false)
                        {
                            badRow = rowNumber;
                            break;
                        }
                    }

                    break;
                    
                case "Radiologists":
                case "Referrers":

                    for (int rowNumber = 2; rowNumber == lastUsedRow; rowNumber++)
                    {
                        for (int cellNumber = 1; cellNumber == maxColcount; cellNumber++)
                        {
                            cellValue = (xlSheet.Cells[rowNumber, cellNumber] as Excel.Range).Value;

                            switch (cellNumber)
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                    //prefix - string - alphabetical - 1
                                    //first name - string - alphabetical - 2
                                    //surname - string - alphabetical - 3
                                    //specialty - string - alphabetical - 4
                                    dataIsValid = ValidateForAlphabetical(cellValue);
                                    break; 

                                default:
                                    break;
                            }

                            if (dataIsValid == false)
                            {
                                badCellValue = cellValue;
                                badCol = cellNumber;
                                break;
                            }

                        }

                        if (dataIsValid == false)
                        {
                            badRow = rowNumber;
                            break;
                        }
                    }

                    break;

                case "InterventionalNurses":

                    for (int rowNumber = 2; rowNumber == lastUsedRow; rowNumber++)
                    {
                        for (int cellNumber = 1; cellNumber == maxColcount; cellNumber++)
                        {
                            cellValue = (xlSheet.Cells[rowNumber, cellNumber] as Excel.Range).Value;

                            switch (cellNumber)
                            {
                                case 1:
                                case 3:
                                case 4:
                                    //userName - string - alphabetical - 1
                                    //first name - string - alphabetical - 3
                                    //surname - string - alphabetical - 4
                                    dataIsValid = ValidateForAlphabetical(cellValue);
                                    break;

                                case 2:
                                    //email - string - email - 2
                                    dataIsValid = ValidateForEmail(cellValue);
                                    break;


                                default:
                                    break;
                            }
                            if (dataIsValid == false)
                            {
                                badCellValue = cellValue;
                                badCol = cellNumber;
                                break;
                            }

                        }

                        if (dataIsValid == false)
                        {
                            badRow = rowNumber;
                            break;
                        }
                    }

                    break;

                default:
                    break;
            }

            return badCellDataTuple = (badRow, badCol, badCellValue);
        }


        private bool ValidateForDecimal(string input)
        {
            bool isValid = false;

            if (Decimal.TryParse(input, out _))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;

        }

        private bool ValidateForAlphabetical(string input)
        {
            bool isValid = false;

            if (Regex.IsMatch(input, @"^[a-zA-Z\s\.\-\s]+$"))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateForAlphaNumeric(string input)
        {
            bool isValid = false;

            if (Regex.IsMatch(input, @"^[a-zA-Z0-9\s\/\-\.]+$"))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateForInt32(string input)
        {
            bool isValid = false;

            if (Regex.IsMatch(input, @"^[0-9]+$"))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateForEmail(string email)
        {
            bool isValid = false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                if (addr.Address == email)
                {
                    isValid = true;
                }
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }

        /*RefreshDataStore RELOADS SELECTED ARRAYS FROM DATABASE*/
        private void RefreshDataStore(string tableToRefresh)
        {
            switch (tableToRefresh)
            {
                case "Dilators":
                    StaticData.PopulateListOfDilators();
                    break;

                case "Balloons":
                    StaticData.PopulateListOfBalloons();
                    break;

                case "Snares":
                    StaticData.PopulateListOfSnares();
                    break;

                case "Stents":
                    StaticData.PopulateListOfStents();
                    break;

                case "Contrast":
                    StaticData.PopulateListOfContrast();
                    break;

                case "Catheters":
                    StaticData.PopulateListOfCatheters();
                    break;

                case "Sheaths":
                    StaticData.PopulateListOfSheaths();
                    break;

                case "Referrers":
                    StaticData.PopulateReferrerList();
                    break;

                case "Radiologists":
                    StaticData.PopulateRadiologistList();
                    break;

                case "Coils":
                    StaticData.PopulateListOfEmbolisationCoils();
                    break;

                case "Wires":
                    StaticData.PopulateListOfWires();
                    break;

                case "EmbolisationSystems":
                    StaticData.PopulateListOfEmbolisationSystems();
                    break;

                case "Dressings":
                    StaticData.PopulateListOfDressings();
                    break;

                case "Misc":
                    StaticData.PopulateListOfMiscItems();
                    break;

                case "InterventionalNurses":
                    StaticData.PopulateInterventionalNursesList();
                    break;

                case "MiscData":
                    StaticData.PopulateListOfDbTables();
                    StaticData.PopulateListOfProcedureLocations();
                    StaticData.PopulateListOfSyncCategories();
                    break;

                default:
                    MessageBox.Show($"Error - Supplied Table Does Not Exist: {tableToRefresh}.");
                    break;
            }
        }

        private List<List<string>> GetCellValues(string fileName, List<(int, int)> cellPositions, Excel.Worksheet xlWorkSheet)
        {
            List<List<string>> cellValues = new List<List<string>>();

            //Get the last occupied row number and number of columns required.
            int MaxRows = (xlWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell)).Row;
            int MaxCols = cellPositions.Count;

            //Convert the column index to it's equivalent alphabetical letter.
            char MaxColChar = (char)(MaxCols + 64);

            //Grab the range of occupied cells, EXCLUDING the title row, A1...
            Excel.Range AllRows = xlWorkSheet.Range[$"A2:{MaxColChar}{MaxRows}"];

            //For info purposes, to check the number of retrieved rows and cols in the range.
            int rowCount = AllRows.Rows.Count;
            int cellsPerRowCount = AllRows.Rows[1].Cells.Count;

            for (int dataRow = 1; dataRow <= rowCount; dataRow++)
            {
                List<string> row = new List<string>();
                Excel.Range eachRow = AllRows.Rows[dataRow];

                for (int dataCell = 1; dataCell <= eachRow.Cells.Count; dataCell++)
                {
                    Excel.Range cell = eachRow.Cells[dataCell];

                    //Extract data from non-empty cells.
                    if (cell != null)
                    {
                        if (cell.Value2 != null)
                        {
                            string cellVal = cell.Value2.ToString();
                            Console.WriteLine($"cell value is: {cellVal}");
                            row.Add(cell.Value2.ToString());
                        }
                        else
                        {
                            //return null if null cell found.
                            row.Add("");
                        }


                    }
                }

                cellValues.Add(row);
            }

            return cellValues;
        }


        private void SyncCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void UpdateProgressBar(int newValue)
        {
            SyncProgressBar.Value = newValue;
        }

        public void UpdateCurrentTask(string updateText)
        {
            CurrentTask.Content = updateText;
        }

        public void UpdateTaskDetails(string updateText)
        {
            TaskDetails.Content = updateText;
        }

        public void UpdateProgressMax(Double newValue)
        {
            ProgressBarMax = newValue;
        }






















    }//END OF CLASS
}
