using InterventionalCostings.Pages;
using InterventionalCostings.Static_Data;
using System.Windows;
using System;
using System.Threading.Tasks;
using InterventionalCostings.CustomViews;

namespace InterventionalCostings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window : System.Windows.Window
    {
        public Window()
        {
            InitializeComponent();

            ProgressBarWindow progress = new ProgressBarWindow();
            progress.ShowDialog();

            StartPage startPage = StartPage.StartInstance;
            StartFrame.Navigate(startPage);
        }

	}


}

 