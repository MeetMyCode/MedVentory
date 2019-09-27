using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using InterventionalCostings.Pages;
using System;
using InterventionalCostings.Static_Data;
using InterventionalCostings.CustomViews;
using System.Threading.Tasks;

namespace InterventionalCostings.Pages
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    /// 

    public sealed partial class StartPage: Page
    {
        private static StartPage startInstance = null;
        private static readonly object padlock = new object();
        public event EventHandler CenterWindow;

        private StartPage() {
            InitializeComponent();
            CenterMainWindowRequested(new EventArgs());
        }

        public void CenterMainWindowRequested(EventArgs e)
        {
            CenterWindow?.Invoke(this, e);
        }

        public static StartPage StartInstance
        {
            get
            {
                lock (padlock)
                {
                    if (startInstance == null)
                    {
                        startInstance = new StartPage();
                    }
                    return startInstance;
                }
            }

        }

		private void Settings_Click(object sender, RoutedEventArgs e)
		{
			//Open a Settings Window.
			SettingsWindow mySettings = new SettingsWindow();
			mySettings.ShowDialog();

		}
		//New -> Case has been clicked.
		private void NewCase_Click(object sender, RoutedEventArgs e)
        {
            //Open a New Interventional Case Window.

            NewCasePage newPage = NewCasePage.NewCaseInstance;
            NavigationService.Navigate(newPage);

        }

        private void GoToOrdersPage(object sender, RoutedEventArgs e)
        {
            OrdersPage OrdersPageViewer = new OrdersPage();
            NavigationService.Navigate(OrdersPageViewer);
        }

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GoToDbToExcelSyncWindow(object sender, RoutedEventArgs e)
        {
            SyncDbToExcel SyncWindow = new SyncDbToExcel();
            SyncWindow.ShowDialog();
        }

        private void GoToExcelToDbSyncWindow(object sender, RoutedEventArgs e)
        {
            SyncExcelToDb SyncWindow = new SyncExcelToDb();
            SyncWindow.ShowDialog();
        }

        private void GoToReportsPage(object sender, RoutedEventArgs e)
        {
            ReportsPage ReportsPageViewer = new ReportsPage();
            NavigationService.Navigate(ReportsPageViewer);
        }




        private void OnPageLoad(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow;
            main.ResizeMode = ResizeMode.CanResize;
			main.WindowState = WindowState.Maximized;

			HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
        }
    }
}



