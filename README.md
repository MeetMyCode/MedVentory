<img src="https://github.com/MeetMyCode/MedVentory/blob/master/Images/SplashScreens/splashScreen.png"/>

# STATUS: INCOMPLETE.

# About
<p>Originally made for stock control and data management of Interventional Radiology procedures. This could be also be modified/used in 
other areas where stock control and "case" management are required.</p>

# Requirements
<p>This application depends on Microsoft Excel to be able to run properly as all data is Excel Spreadsheet based. The sending of emails for ordering purposes within the application uses Microsoft Exchange.</p>

# Installation
<p>This is a stand-alone application. Build to wherever and run. No uninstall required just delete!</p>

# Important file locations
/Databases - This is where the database file is - Inventory.mdf.

/StaticData - Seperate files are here for any data that the application required that does not change e.g. Excel file cell locations, column headings etc.

/ExcelFiles - Contains the Excel Files that form the basis of all data used within the application.

/Images - Product Imagery. The file name of the image must match the product reference number column in the database/excel file in order for the image to appear in the application.

# Usage
<p>This application can do the following:</p>
<ul>
  <li> Create a new case - record relevant information for a new procedure e.g patient data, case data & equipment used. </li>
  <li> View a list of items that have been used over time and that will require ordering. </li>
  <li> Email the individual responsible for ordering a list of items that require purchasing. </li>
  <li> Sync Data from Excel File(s) -> Database table(s) or visa versa. </li>
  <li> View statistics about cases and products used over time and view this information on graphs (currently line and bar). </li>  
</ul>

<p>Currently, the database tables are already populated with everything I originally required. Excel spreadsheets are used as the basis of all data in the application and also to populate/edit the database tables - done via syncing data from Excel sheet -> Database table (or visa versa if necessary).</p>

<p>When you save a case, this information is saved to an Excel file in a 'Saved Cases' folder, created when the first case is saved.</p>
<p>When data is synced from database tables to Excel files, these files are placed in a folder called 'ExcelFiles', created upon first syncing.</p>

# Screenshots
<img src="https://github.com/MeetMyCode/MedVentory/blob/master/Images/SplashScreens/SS_StartScreen.png"/>
<img src="https://github.com/MeetMyCode/MedVentory/blob/master/Images/SplashScreens/SS_NewCase.png"/>
<img src="https://github.com/MeetMyCode/MedVentory/blob/master/Images/SplashScreens/SS_Orders.png"/>
<img src="https://github.com/MeetMyCode/MedVentory/blob/master/Images/SplashScreens/SS_Sync.png"/>
<img src="https://github.com/MeetMyCode/MedVentory/blob/master/Images/SplashScreens/SS_Stats.png"/>

