using InterventionalCostings.Static_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InterventionalCostings.CustomViews
{
    /// <summary>
    /// Interaction logic for ProgressBarWindow.xaml
    /// </summary>
    public partial class ProgressBarWindow : System.Windows.Window, IUpdateStartupProgressBar
    {
        public Double ProgressBarMax
        {
            get { return TaskProgressBar.Maximum; }
            set { TaskProgressBar.Maximum = value; }
        }

        public ProgressBarWindow()
        {
            InitializeComponent();

            TaskScheduler uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            StaticData.Setup(uiContext, this);


        }

        public void UpdateProgressBar(int newValue)
        {
            TaskProgressBar.Value = newValue;
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
    }
}
