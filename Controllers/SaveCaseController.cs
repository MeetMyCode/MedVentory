using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterventionalCostings.Inventory_Item_Classes;
using System.Windows;
using InterventionalCostings.CustomViews;

namespace InterventionalCostings.Controllers
{
    class SaveCaseController
    {

        public SaveCaseController(Case caseToSave, string caseFileNamePath)
        {
            SaveCase(caseToSave, caseFileNamePath);
        }

		//Save Case to File.
        private void SaveCase(Case caseToSave, string caseFileNamePath)
        {
            Excel.Application xlApp = new Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Open(caseFileNamePath, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("Sheet1");

            xlWorkSheet.Cells[2, 3] = caseToSave.PtFirstName;
            xlWorkSheet.Cells[3, 3] = caseToSave.PtSurname;
            xlWorkSheet.Cells[4, 3] = caseToSave.PtDob;
            xlWorkSheet.Cells[5, 3] = caseToSave.PtNhsNumber;
            xlWorkSheet.Cells[6, 3] = caseToSave.Referrer;
            xlWorkSheet.Cells[2, 8] = caseToSave.CaseDate;
            xlWorkSheet.Cells[3, 8] = caseToSave.Procedure;
            xlWorkSheet.Cells[4, 8] = caseToSave.Location;
            xlWorkSheet.Cells[5, 8] = caseToSave.Radiologist;
            xlWorkSheet.Cells[9, 2] = caseToSave.CaseNotes;
            
            int StartRow = 13;
            foreach (SelectedItem item in caseToSave.CaseItems)
            {
                xlWorkSheet.Cells[StartRow + 1, 2] = item.SelectedItemDescription.Content;
                xlWorkSheet.Cells[StartRow + 1, 6] = item.DeleteSelectedItemButton.Tag; //<-- tag = refNumber
                xlWorkSheet.Cells[StartRow + 1, 7] = item.SelectedItemCost.Content;
                xlWorkSheet.Cells[StartRow + 1, 8] = item.ItemCount.Content;

                //Add case item to ItemsToOrder database table
                DBController.AddTo_ItemsToOrder(item);

                StartRow += 1;
            }

            xlWorkBook.Save();
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            
        }




    }
}
