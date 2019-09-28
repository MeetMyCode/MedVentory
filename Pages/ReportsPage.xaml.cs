using InterventionalCostings.Static_Data;
using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using InterventionalCostings.Inventory_Item_Classes;
using InterventionalCostings.CustomViews;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;

namespace InterventionalCostings.Pages
{
    /// <summary>
    /// Interaction logic for ReportsPage.xaml
    /// </summary>
    /// 

    public partial class ReportsPage : Page
    {
        private List<string> charts;
        private ObservableCollection<string> chartTypes;

		//QuickStats
		Label MostUsedLabel;
		Label LeastUsedLabel;
		private List<CaseStatsData> caseStats;
        private List<CaseItemCountHistoryStatsData> caseItemCountHistoryStats;
        private int totalCaseCount;
        decimal leastExpensiveCase = 0;
        decimal mostExpensiveCase = 0;
        decimal totalMoneySpentSoFar;
        private (decimal, decimal) avgCaseCountAndCostPerWeek;
        private (decimal, decimal) avgCaseCountAndCostPerMonth;
        private (decimal, decimal) avgCaseCountAndCostPerYear;
        private (string, decimal) mostUsedProductAndCost;
        private (string, decimal) leastUsedProductAndCost;


        //OxyPlot
        public PlotModel MyGraph { get; private set; }
        public LineSeries myLineSeries { get; private set; }
        public ColumnSeries myColumnSeries { get; private set; }
        private ObservableCollection<string> orderedObservableYears;
        public LinearAxis Xaxis { get; private set; }
        public LinearAxis Yaxis { get; private set; }


        public ReportsPage()
        {
            InitializeComponent();

            Setup();

            //set datacontext for graph
            DataContext = this;

        }

        private void Setup()
        {
            charts = StaticData.GetChartTypes();
            chartTypes = new ObservableCollection<string>(charts);
            chartTypeComboBox.ItemsSource = chartTypes;
            chartTypeComboBox.Text = chartTypeComboBox.Items[0].ToString();

            CustomTitleBar titleBar = new CustomTitleBar();
            titleBar.TitleBarTitleText.Content = "Graphs and Trends";
            MainGrid.Children.Insert(0,titleBar);

            YAxisComboBox.IsEnabled = false;
            XAxisComboBox.IsEnabled = false;
            SelectedYearComboBox.IsEnabled = false;

            PopulateQuickStats();

            //Populate Quick Stats Values
            TotalCaseCount.Content = totalCaseCount + @" Cases";
            TotalMoneySpentSoFar.Content = @"£" + totalMoneySpentSoFar;
            LeastExpensiveCase.Content = @"£" + leastExpensiveCase;
            MostExpensiveCase.Content = @"£" + mostExpensiveCase;
            AvgWeeklyCaseCountCost.Content = avgCaseCountAndCostPerWeek.Item1 + " / £" + avgCaseCountAndCostPerWeek.Item2;
            AvgMonthlyCaseCountCost.Content = avgCaseCountAndCostPerMonth.Item1 + " / £" + avgCaseCountAndCostPerMonth.Item2;
            AvgYearlyCaseCountCost.Content = avgCaseCountAndCostPerYear.Item1 + " / £" + avgCaseCountAndCostPerYear.Item2;
            //MostUsedProductAndCost.Content = mostUsedProductAndCost.Item1 + " / £" + mostUsedProductAndCost.Item2;
            //LeastUsedProductAndCost.Content = leastUsedProductAndCost.Item1 + " / £" + leastUsedProductAndCost.Item2;

        }

        private void PopulateQuickStats()
        {
            caseStats = StaticData.GetListOfCaseStatsData();
            caseItemCountHistoryStats = StaticData.GetListOfCaseItemCountHistoryStatsData();

            //get total case count
            totalCaseCount = caseStats.Count();

            List<decimal> costList = new List<decimal>();

			if (caseStats.Count != 0)
			{
				foreach (CaseStatsData Case in caseStats)
				{
					//Create a list of case costs
					costList.Add(Case.CaseCost);

					//Calculate total case cost so far
					totalMoneySpentSoFar += Case.CaseCost;
				}

				//Get most and least expensive cases
				leastExpensiveCase = costList.Min();
				mostExpensiveCase = costList.Max();

				//Get avg weekly case count and cost - and format decimals to 2 places.
				decimal numberOfWeeksSinceStart = decimal.Parse(CalculateNumberOfWeeks().ToString("0.00"));
				decimal avgCaseCountPerWeek = decimal.Parse((totalCaseCount / numberOfWeeksSinceStart).ToString("0.00"));
				decimal avgCaseCostPerWeek = decimal.Parse((totalMoneySpentSoFar / numberOfWeeksSinceStart).ToString("0.00"));
				avgCaseCountAndCostPerWeek = (avgCaseCountPerWeek, avgCaseCostPerWeek);

				//Get avg monthly case count and cost - and format decimals to 2 places.
				decimal numberOfMonthsSinceStart = decimal.Parse(CalculateNumberOfMonths().ToString("0.00"));
				decimal avgCaseCountPerMonth = decimal.Parse((totalCaseCount / numberOfMonthsSinceStart).ToString("0.00"));
				decimal avgCaseCostPerMonth = decimal.Parse((totalMoneySpentSoFar / numberOfMonthsSinceStart).ToString("0.00"));
				avgCaseCountAndCostPerMonth = (avgCaseCountPerMonth, avgCaseCostPerMonth);

				//Get avg annual case count and cost - and format decimals to 2 places.
				decimal numberOfYearsSinceStart = decimal.Parse(CalculateNumberOfYears().ToString("0.00"));
				decimal avgCaseCountPerYear = decimal.Parse((totalCaseCount / numberOfYearsSinceStart).ToString("0.00"));
				decimal avgCaseCostPerYear = decimal.Parse((totalMoneySpentSoFar / numberOfYearsSinceStart).ToString("0.00"));
				avgCaseCountAndCostPerYear = (avgCaseCountPerYear, avgCaseCostPerYear);

				//Create a list of items and a list of quantites used corresponding to each item in the items list.
				List<string> items = new List<string>();
				List<int> quantityUsed = new List<int>();
				//int index = 0;

				foreach (CaseItemCountHistoryStatsData item in caseItemCountHistoryStats)
				{
					//If item already in list, increment quantity.
					if (items.Contains(item.ItemDescription))
					{
						quantityUsed[items.IndexOf(item.ItemDescription)] += item.QuantityUsed;
					}
					else
					{
						//If item not in list, add item and corresponding quantity.
						items.Add(item.ItemDescription);
						quantityUsed.Add(item.QuantityUsed);
					}

					//index++;
				}

				int mostUsedProductCount = quantityUsed.Max();
				int leastUsedProductCount = quantityUsed.Min();

				//There might be multiple items that share the rank of least/most used product,
				//so create lists of the least and most used products to place in the corresponding
				//labels.
				List<string> leastUsedItemsList = new List<string>();
				List<string> mostUsedItemsList = new List<string>();

				int index2 = 0;
				foreach (int quantity in quantityUsed)
				{
					if (quantity == leastUsedProductCount)
					{
						leastUsedItemsList.Add(items[index2]);

					}
					else if (quantity == mostUsedProductCount)
					{
						mostUsedItemsList.Add(items[index2]);
					}

					index2++;
				}

				//Populate most and least used product labels.
				foreach (string item in leastUsedItemsList)
				{
					LeastUsedLabel = new Label();
					LeastUsedLabel.Content += "\n" + item;
					LeaststUsedItemsStack.Children.Add(LeastUsedLabel);
				}
				LeastUsedLabel = new Label();
				LeastUsedLabel.Content += "\n Quantity Used: " + leastUsedProductCount;
				LeaststUsedItemsStack.Children.Add(LeastUsedLabel);

				foreach (string item in mostUsedItemsList)
				{
					MostUsedLabel = new Label();
					MostUsedLabel.Content += "\n" + item;
					MostUsedItemsStack.Children.Add(MostUsedLabel);
				}
				MostUsedLabel = new Label();
				MostUsedLabel.Content += "\n Quantity Used: " + mostUsedProductCount;
				MostUsedItemsStack.Children.Add(MostUsedLabel);
			}
			else
			{
				//NOTIFY USER THAT STATS CANNOT BE GENERATED AS NO CASES HAVE BEEN RECORDED.
				MessageBox.Show("No Statistics can be generated as no Procedures/Cases have yet been recorded.");
			}


		}

		private decimal CalculateNumberOfYears()
        {
            //calculate earliest date
            List<DateTime> datesList = new List<DateTime>();

            foreach (CaseStatsData Case in caseStats)
            {
                DateTime date = Case.CaseDate;
                datesList.Add(date);
            }

            DateTime earliestDate = datesList.Min();
            DateTime now = DateTime.Now;

            decimal numberOfYears = (decimal)(now.Subtract(earliestDate).Days / 365.2425);

			if (numberOfYears < 1)
			{
				return 1;
			}
			else
			{
				return numberOfYears;
			}

		}

        private decimal CalculateNumberOfMonths()
        {
            //calculate earliest date
            List<DateTime> datesList = new List<DateTime>();

            foreach (CaseStatsData Case in caseStats)
            {
                DateTime date = Case.CaseDate;
                datesList.Add(date);
            }

            DateTime earliestDate = datesList.Min();
            DateTime now = DateTime.Now;

            decimal numberOfMonths = (decimal)(now.Subtract(earliestDate).Days / (365.2425 / 12));

			if (numberOfMonths < 1)
			{
				return 1;
			}
			else
			{
				return numberOfMonths;
			}
		}

        private decimal CalculateNumberOfWeeks()
        {
            //calculate earliest date
            List<DateTime> datesList = new List<DateTime>();

            foreach (CaseStatsData Case in caseStats)
            {
                DateTime date = Case.CaseDate;
                datesList.Add(date);
            }

            DateTime earliestDate = datesList.Min();
            DateTime now = DateTime.Now;

            decimal numberOfWeeks = (decimal)((now - earliestDate).TotalDays / 7);

			if (numberOfWeeks < 1)
			{
				return 1;
			}
			else
			{
				return numberOfWeeks;
			}
		}



        private void LoadGraph(object sender, RoutedEventArgs e)
        {
            string chart = chartTypeComboBox.Text;
            string chartPlaceHolder = charts[0];
            string xData = XAxisComboBox.Text;
            string yData = YAxisComboBox.Text;
            string year = SelectedYearComboBox.Text;
            string yearPlaceHolder = "Select a Year...";

            //If a graph has been selected and data for each axis loaded, do the following:
            if ((chart != chartPlaceHolder && yData != "" && xData != "" && year != yearPlaceHolder) ||
                (chart != chartPlaceHolder && yData != "" && xData == "Per Year"))
            {
                switch (chartTypeComboBox.Text)
                {
                    case "Line":
                        switch (YAxisComboBox.Text)
                        {
                            case "Case Cost":

                                Dictionary<int, decimal> weeklyCaseCostData = new Dictionary<int, decimal>();
                                Dictionary<int, decimal> monthlyCaseCostData = new Dictionary<int, decimal>();
                                Dictionary<int, decimal> annualCaseCostData = new Dictionary<int, decimal>();

								Dictionary<int, decimal> weeklyCaseCountData = new Dictionary<int, decimal>();
								Dictionary<int, decimal> monthlyCaseCountData = new Dictionary<int, decimal>();
								Dictionary<int, decimal> annualCaseCountData = new Dictionary<int, decimal>();

								switch (XAxisComboBox.Text)
                                {
                                    case "Per Week":
                                        weeklyCaseCostData = getWeeklyCaseCostData();
                                        SetupWeeklyGraphWithSpecificYear(chartTypeComboBox.Text, weeklyCaseCostData, "cost");
                                        break;

                                    case "Per Month":
                                        monthlyCaseCostData = getMonthlyCaseCostData();
                                        SetupMonthlyGraphWithSpecificYear(chartTypeComboBox.Text, monthlyCaseCostData, "cost");
                                        break;

                                    case "Per Year":
                                        annualCaseCostData = getAnnualCaseCostData();
                                        SetupGraphAcrossYears(chartTypeComboBox.Text, annualCaseCostData, "cost");
                                        break;

                                    default:
                                        MessageBox.Show("Invalid Time Option Specified in ReportPage.LoadGraph()");
                                        break;
                                }

                                break;

                            case "Number of Cases":

                                Dictionary<int, decimal> caseCountData = new Dictionary<int, decimal>();

                                switch (XAxisComboBox.Text)
                                {
                                    case "Per Week":
                                        weeklyCaseCountData = getWeeklyCaseCountData();
										SetupWeeklyGraphWithSpecificYear(chartTypeComboBox.Text, weeklyCaseCountData, "count");
										break;

                                    case "Per Month":
										monthlyCaseCountData = getMonthlyCaseCountData();
										SetupMonthlyGraphWithSpecificYear(chartTypeComboBox.Text, monthlyCaseCountData, "count");
										break;

                                    case "Per Year":
										annualCaseCountData = getAnnualCaseCountData();
										SetupGraphAcrossYears(chartTypeComboBox.Text, annualCaseCountData, "count");
										break;

                                    default:
                                        MessageBox.Show("Invalid Time Option Specified in ReportPage.LoadGraph()");
                                        break;
                                }

                                break;

                            default:
                                break;
                        }
                        break;

                    case "Bar":
                        switch (YAxisComboBox.Text)
                        {
                            case "Case Cost":

                                Dictionary<int, decimal> weeklyCaseCostData = new Dictionary<int, decimal>();
                                Dictionary<int, decimal> monthlyCaseCostData = new Dictionary<int, decimal>();
                                Dictionary<int, decimal> annualCaseCostData = new Dictionary<int, decimal>();

								Dictionary<int, decimal> weeklyCaseCountData = new Dictionary<int, decimal>();
								Dictionary<int, decimal> monthlyCaseCountData = new Dictionary<int, decimal>();
								Dictionary<int, decimal> annualCaseCountData = new Dictionary<int, decimal>();

								switch (XAxisComboBox.Text)
                                {
                                    case "Per Week":
                                        weeklyCaseCostData = getWeeklyCaseCostData();
                                        SetupWeeklyGraphWithSpecificYear(chartTypeComboBox.Text, weeklyCaseCostData, "cost");
                                        break;

                                    case "Per Month":
                                        monthlyCaseCostData = getMonthlyCaseCostData();
                                        SetupMonthlyGraphWithSpecificYear(chartTypeComboBox.Text, monthlyCaseCostData, "cost");
                                        break;

                                    case "Per Year":
                                        annualCaseCostData = getAnnualCaseCostData();
                                        SetupGraphAcrossYears(chartTypeComboBox.Text, annualCaseCostData, "cost");
                                        break;

                                    default:
                                        MessageBox.Show("Invalid Time Option Specified in ReportPage.LoadGraph()");
                                        break;
                                }

                                break;

                            case "Number of Cases":

                                Dictionary<int, decimal> caseCountData = new Dictionary<int, decimal>();

                                switch (XAxisComboBox.Text)
                                {
                                    case "Per Week":
										weeklyCaseCountData = getWeeklyCaseCountData();
										SetupWeeklyGraphWithSpecificYear(chartTypeComboBox.Text, weeklyCaseCountData, "count");
										break;

                                    case "Per Month":
										monthlyCaseCountData = getMonthlyCaseCountData();
										SetupMonthlyGraphWithSpecificYear(chartTypeComboBox.Text, monthlyCaseCountData, "count");
										break;

                                    case "Per Year":
										annualCaseCountData = getAnnualCaseCountData();
										SetupGraphAcrossYears(chartTypeComboBox.Text, annualCaseCountData, "count");
										break;

                                    default:
                                        MessageBox.Show("Invalid Time Option Specified in ReportPage.LoadGraph()");
                                        break;
                                }

                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        MessageBox.Show("Unrecognised graph type requested in ReportsPage.ChartTypeSelected().");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Select some data first.");
            }
        }


        private Dictionary<int, decimal> getWeeklyCaseCostData()
        {
            int selectedYear = Int32.Parse(SelectedYearComboBox.Text);

            //Get X-Axis Data
            Dictionary<int, decimal> WeeksInAYear = new Dictionary<int, decimal>();
            for (int i = 1; i <= 52; i++)
            {
                WeeksInAYear.Add(i, 0);
            }

            List<DateTime> weekStartDates = new List<DateTime>();
            DateTime janFirst = new DateTime(selectedYear, 1, 1);

            weekStartDates.Add(janFirst);
            DateTime nextDate = janFirst;
            for (int i = 2; i <= WeeksInAYear.Count; i++)
            {
                nextDate = nextDate.AddDays(7);
                weekStartDates.Add(nextDate);
            }

            foreach (CaseStatsData caseData in caseStats)
            {
                DateTime nextWeekStartDate;
                int arrayIndex = 1;
                foreach (DateTime weekStartDate in weekStartDates)
                {
                    nextWeekStartDate = weekStartDate.AddDays(7);
                    if (caseData.CaseDate >= weekStartDate && caseData.CaseDate < nextWeekStartDate)
                    {
                        WeeksInAYear[arrayIndex] += caseData.CaseCost;
                    }

                    arrayIndex++;
                }
            }

            return WeeksInAYear;
        }

        private Dictionary<int, decimal> getMonthlyCaseCostData()
        {
            int selectedYear = Int32.Parse(SelectedYearComboBox.Text);

            //Get X-Axis Data
            Dictionary<int, decimal> MonthsInAYear = new Dictionary<int, decimal>();
            for (int i = 0; i < 12; i++)
            {
                MonthsInAYear.Add(i, 0);
            }

            List<DateTime> monthStartDates = new List<DateTime>();
            DateTime janFirst = new DateTime(selectedYear, 1, 1);

            monthStartDates.Add(janFirst);
            DateTime nextDate = janFirst;
            for (int i = 2; i <= MonthsInAYear.Count; i++)
            {
                nextDate = nextDate.AddMonths(1);
                monthStartDates.Add(nextDate);
            }

            foreach (CaseStatsData caseData in caseStats)
            {
                DateTime nextMonthStartDate;
                int arrayIndex = 0;
                foreach (DateTime monthStartDate in monthStartDates)
                {
                    nextMonthStartDate = monthStartDate.AddMonths(1);
                    if (caseData.CaseDate >= monthStartDate && caseData.CaseDate < nextMonthStartDate)
                    {
                        MonthsInAYear[arrayIndex] += caseData.CaseCost;
                    }

                    arrayIndex++;
                }
            }

            return MonthsInAYear;
        }

        private Dictionary<int, decimal> getAnnualCaseCostData()
        {
            //Get X-Axis Data
            Dictionary<int, decimal> YearlyCostData = new Dictionary<int, decimal>();

            foreach (CaseStatsData caseYear in caseStats)
            {
                if (YearlyCostData.ContainsKey(caseYear.CaseDate.Year))
                {
                    //If year alreadey exists, just add cost
                    YearlyCostData[caseYear.CaseDate.Year] += caseYear.CaseCost;
                }
                else
                {
                    //Otherwise add year and cost.
                    YearlyCostData.Add(caseYear.CaseDate.Year, caseYear.CaseCost);
                }
            }

            return YearlyCostData;
        }


		private Dictionary<int, decimal> getWeeklyCaseCountData()
		{
			int selectedYear = Int32.Parse(SelectedYearComboBox.Text);

			//Get X-Axis Data
			Dictionary<int, decimal> WeeklyCountData = new Dictionary<int, decimal>();
			for (int i = 1; i <= 52; i++)
			{
				WeeklyCountData.Add(i, 0);
			}

			List<DateTime> weekStartDates = new List<DateTime>();
			DateTime janFirst = new DateTime(selectedYear, 1, 1);

			weekStartDates.Add(janFirst);
			DateTime nextDate = janFirst;
			for (int i = 2; i <= WeeklyCountData.Count; i++)
			{
				nextDate = nextDate.AddDays(7);
				weekStartDates.Add(nextDate);
			}

			foreach (CaseStatsData caseData in caseStats)
			{
				DateTime nextWeekStartDate;
				int arrayIndex = 1;
				foreach (DateTime weekStartDate in weekStartDates)
				{
					nextWeekStartDate = weekStartDate.AddDays(7);
					if (caseData.CaseDate >= weekStartDate && caseData.CaseDate < nextWeekStartDate)
					{
						WeeklyCountData[arrayIndex] += 1;
					}

					arrayIndex++;
				}
			}

			return WeeklyCountData;

		}

		private Dictionary<int, decimal> getMonthlyCaseCountData()
		{

			int selectedYear = Int32.Parse(SelectedYearComboBox.Text);

			//Get X-Axis Data
			Dictionary<int, decimal> MonthlyCountData = new Dictionary<int, decimal>();
			for (int i = 0; i < 12; i++)
			{
				MonthlyCountData.Add(i, 0);
			}

			List<DateTime> monthStartDates = new List<DateTime>();
			DateTime janFirst = new DateTime(selectedYear, 1, 1);

			monthStartDates.Add(janFirst);
			DateTime nextDate = janFirst;
			for (int i = 2; i <= MonthlyCountData.Count; i++)
			{
				nextDate = nextDate.AddMonths(1);
				monthStartDates.Add(nextDate);
			}

			foreach (CaseStatsData caseData in caseStats)
			{
				DateTime nextMonthStartDate;
				int arrayIndex = 0;
				foreach (DateTime monthStartDate in monthStartDates)
				{
					nextMonthStartDate = monthStartDate.AddMonths(1);
					if (caseData.CaseDate >= monthStartDate && caseData.CaseDate < nextMonthStartDate)
					{
						MonthlyCountData[arrayIndex] += 1;
					}

					arrayIndex++;
				}
			}

			return MonthlyCountData;
		}

		private Dictionary<int, decimal> getAnnualCaseCountData()
		{

			//Get X-Axis Data
			Dictionary<int, decimal> YearlyCountData = new Dictionary<int, decimal>();

			foreach (CaseStatsData caseYear in caseStats)
			{
				if (YearlyCountData.ContainsKey(caseYear.CaseDate.Year))
				{
					//If year alreadey exists, just add +1 case
					YearlyCountData[caseYear.CaseDate.Year] += 1;
				}
				else
				{
					//Otherwise add year and +1 case.
					YearlyCountData.Add(caseYear.CaseDate.Year, 1);
				}
			}

			return YearlyCountData;
		}


		private void ChartTypeSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            if (box.SelectedIndex == 0 && chartTypes[0] == charts[0])
            {
                //Do Nothing.
            }
            else if (box.SelectedIndex != 0 && chartTypes[0] == charts[0])
            {
                chartTypes.RemoveAt(0);

                YAxisComboBox.IsEnabled = true;
                XAxisComboBox.IsEnabled = true;

            }



            switch (box.SelectedValue)
            {
                case "Line":
                case "Bar":

                    YAxisComboBox.ItemsSource = StaticData.GetLineAndBarChartYAxisOptions();
                    XAxisComboBox.ItemsSource = StaticData.GetLineAndBarChartXAxisOptions();

                    break;

                case "Select a Chart Type...":
                    //Do nothing.
                    break;

                default:
                    MessageBox.Show("Unrecognised graph type requested in ReportsPage.ChartTypeSelected().");
                    break;
            }

        }

        private void CloseGraphsAndTrends(object sender, RoutedEventArgs e)
        {
            StartPage start = StartPage.StartInstance;
            NavigationService.Navigate(start);
        }

        private void PopulateSelectedYearComboBox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            ObservableCollection<string> observableYears = new ObservableCollection<string>();

            switch (box.SelectedValue)
            {
                case "Per Week":
                case "Per Month":

                    SelectedYearComboBox.IsEnabled = true;

                    if (SelectedYearComboBox.ItemsSource == null)
                    {
                        foreach (CaseStatsData year in caseStats)
                        {
                            if (observableYears.Contains(year.CaseDate.Year.ToString()))
                            {
                                //do not add to list
                            }
                            else
                            {
                                observableYears.Add(year.CaseDate.Year.ToString());
                            }
                        }

                        orderedObservableYears = new ObservableCollection<string>(observableYears.OrderBy(i => i));
                        SelectedYearComboBox.ItemsSource = orderedObservableYears;
                        orderedObservableYears.Insert(0, "Select a Year...");
                        SelectedYearComboBox.Text = SelectedYearComboBox.Items[0].ToString();
                    }
                    else
                    {
                        SelectedYearComboBox.Text = SelectedYearComboBox.Items[0].ToString();
                    }

                    break;

                case "Per Year":
                    SelectedYearComboBox.Text = "Not Required.";
                    SelectedYearComboBox.IsEnabled = false;

                    break;

                default:
                    break;
            }


        }

        private void YearSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            if (box.SelectedIndex != 0 && orderedObservableYears[0] == "Select a Year...")
            {
                orderedObservableYears.RemoveAt(0);
            }

        }



        private void SetupWeeklyGraphWithSpecificYear(string graphType, Dictionary<int, decimal> data, string costOrCount)
        {
            switch (graphType)
            {
                case "Line":

                    MyGraph = new PlotModel() {
                        Title = YAxisComboBox.Text + " " + XAxisComboBox.Text + " in " + SelectedYearComboBox.Text
                    };

                    myLineSeries = new LineSeries()
					{
						CanTrackerInterpolatePoints = false,
						MarkerFill = OxyColors.SteelBlue,
						MarkerType = MarkerType.Circle
					};

                    foreach (KeyValuePair<int, decimal> datum in data)
                    {
                        myLineSeries.Points.Add(new DataPoint((double)datum.Key, (double)datum.Value));
                    }

                    // Define X-Axis
                    Xaxis = new OxyPlot.Axes.LinearAxis();
                    Xaxis.Maximum = 52;
                    Xaxis.Minimum = 0;
                    Xaxis.Position = OxyPlot.Axes.AxisPosition.Bottom;
                    Xaxis.Title = "Time (Weeks)";
                    Xaxis.MajorGridlineStyle = LineStyle.Solid;
                    Xaxis.MinorGridlineStyle = LineStyle.Solid;

					//Define Y-Axis
					Yaxis = new OxyPlot.Axes.LinearAxis();

					switch (costOrCount)
					{
						case "cost":

							Yaxis.MajorStep = 100;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;
							Yaxis.MinorStep = 50;
							Yaxis.Title = "Cost (£)";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;

							break;

						case "count":

							Yaxis.MajorStep = 1;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 1;
							Yaxis.Title = "Number of Cases";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;

							break;

						default:
							MessageBox.Show("Invalid flag. Expected 'case' or 'cost' - received: " + costOrCount);
							break;
					}

					MyGraph.Axes.Add(Xaxis);
					MyGraph.Axes.Add(Yaxis);
					MyGraph.Series.Add(myLineSeries);

					GraphView.Model = MyGraph;

                    break;

                case "Bar":

                    MyGraph = new PlotModel()
                    {
                        Title = YAxisComboBox.Text + " " + XAxisComboBox.Text + " in " + SelectedYearComboBox.Text
                    };

                    List<ColumnItem> barData = new List<ColumnItem>();
                    foreach (KeyValuePair<int, decimal> datum in data)
                    {
                        barData.Add(new ColumnItem((double)datum.Value));
                    }

                    myColumnSeries = new ColumnSeries()
                    {
                        ItemsSource = barData,
						//LabelFormatString = "{0:F2}",
						LabelPlacement = LabelPlacement.Inside
                    };

                    // Define X-Axis
                    List<int> xAxisCategories = new List<int>();
                    foreach (KeyValuePair<int, decimal> datum in data)
                    {
                        xAxisCategories.Add(datum.Key);
                    }

                    Xaxis = new OxyPlot.Axes.CategoryAxis()
                    {
                        Key = "Time",
                        ItemsSource = xAxisCategories,
                        Position = AxisPosition.Bottom,
                        Title = "Time (Weeks)",
                        MinorGridlineStyle = LineStyle.Solid
                    };


					//Define Y-Axis
					Yaxis = new LinearAxis();
					switch (costOrCount)
					{
						case "cost":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 200;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 50;
							Yaxis.Title = "Cost (£)";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;

							break;

						case "count":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 5;							
							Yaxis.Minimum = 0;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.MinorStep = 1;
							Yaxis.Title = "Number of Cases";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;
							break;

						default:
							MessageBox.Show("Invalid flag. Expected 'case' or 'cost' - received: " + costOrCount);
							break;
					}



                    MyGraph.Axes.Add(Yaxis);
					MyGraph.Axes.Add(Xaxis);
					MyGraph.Series.Add(myColumnSeries);

                    GraphView.Model = MyGraph;

                    break;

                    default:
                        MessageBox.Show("Invalid Chart Type Specified in ReportsPage.SetupGraphWithSpecificYear().");
                    break;
            }               
        }

        private void SetupMonthlyGraphWithSpecificYear(string graphType, Dictionary<int, decimal> data, string costOrCount)
        {
            List<string> monthTitles = StaticData.GetMonthTitles();
            List<string> dateTimeMonthStrings = StaticData.GetDateTimeMonthTitleStrings();
            List<DateTime> dateTimeMonthDateTimes = new List<DateTime>();

            //convert date strings to dateTimes
            int arrayIndex = 0;
            foreach (string month in dateTimeMonthStrings)
            {
                dateTimeMonthDateTimes.Add(DateTime.Parse(dateTimeMonthStrings[arrayIndex]));
                arrayIndex++;
            }

            switch (graphType)
            {
                case "Line":

                    MyGraph = new PlotModel()
                    {
                        Title = YAxisComboBox.Text + " " + XAxisComboBox.Text + " in " + SelectedYearComboBox.Text
                    };

                    //Define X-Axis
                    Xaxis = new DateTimeAxis()
                    {
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        IntervalType = DateTimeIntervalType.Months,
                        Position = AxisPosition.Bottom,
                        Title = "Month",
                        StringFormat = "MMM"
                    };

					//Define Y-Axis
					Yaxis = new LinearAxis();
					switch (costOrCount)
					{
						case "cost":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 100;
							Yaxis.Minimum = 0;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.MinorStep = 50;
							Yaxis.Title = "Cost (£)";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;

							break;

						case "count":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 1;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 1;
							Yaxis.Title = "Number of Cases";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;
							break;

						default:
							MessageBox.Show("Invalid flag. Expected 'case' or 'cost' - received: " + costOrCount);
							break;
					}

					//create line series
					myLineSeries = new LineSeries() {

                        CanTrackerInterpolatePoints = false,
                        MarkerFill = OxyColors.SteelBlue,
                        MarkerType = MarkerType.Circle
                    };

                    //add line series points
                    int index = 0;
                    foreach (KeyValuePair<int, decimal> datum in data)
                    {
                        myLineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTimeMonthDateTimes[index]), (double)datum.Value));
                        index++;
                    }

                    MyGraph.Axes.Add(Yaxis);
                    MyGraph.Axes.Add(Xaxis);
                    MyGraph.Series.Add(myLineSeries);

                    GraphView.Model = MyGraph;

                    break;

                case "Bar":

                    MyGraph = new PlotModel()
                    {
                        Title = YAxisComboBox.Text + " " + XAxisComboBox.Text + " in " + SelectedYearComboBox.Text
                    };

                    List<ColumnItem> barData = new List<ColumnItem>();
                    foreach (KeyValuePair<int, decimal> datum in data)
                    {
                        barData.Add(new ColumnItem((double)datum.Value));
                    }

                    myColumnSeries = new ColumnSeries()
                    {
                        ItemsSource = barData,
                        //LabelFormatString = "{0:F2}",
						LabelPlacement = LabelPlacement.Inside
					};

                    Xaxis = new CategoryAxis()
                    {
                        Key = "Time",
                        ItemsSource = monthTitles,
                        Position = AxisPosition.Bottom,
                        Title = "Time (Weeks)",
                        MinorGridlineStyle = LineStyle.Solid
                    };

					//Define Y-Axis
					Yaxis = new LinearAxis();
					switch (costOrCount)
					{
						case "cost":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 100;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 50;
							Yaxis.Title = "Cost (£)";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;

							break;

						case "count":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 1;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.Title = "Number of Cases";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;
							break;

						default:
							MessageBox.Show("Invalid flag. Expected 'case' or 'cost' - received: " + costOrCount);
							break;
					}

					MyGraph.Axes.Add(Yaxis);
					MyGraph.Axes.Add(Xaxis);
					MyGraph.Series.Add(myColumnSeries);

                    GraphView.Model = MyGraph;

                    break;

                default:
                    MessageBox.Show("Invalid Chart Type Specified in ReportsPage.SetupGraphWithSpecificYear().");
                    break;
            }
        }

        private void SetupGraphAcrossYears(string graphType, Dictionary<int, decimal> data, string costOrCount)
        {
            List<string> years = new List<string>();

            foreach (CaseStatsData year in caseStats)
            {
                if (years.Contains(year.CaseDate.Year.ToString()))
                {
                    //do nothing - list already contains the current year.
                }
                else
                {
                    years.Add(year.CaseDate.Year.ToString());
                }
            }

            years.OrderBy(i => i);


            switch (graphType)
            {
                case "Line":

                    MyGraph = new PlotModel()
                    {
                        Title = YAxisComboBox.Text + " " + XAxisComboBox.Text + " Across All Years."
                    };

					//Define X-Axis
					Xaxis = new DateTimeAxis()
					{
						MajorGridlineStyle = LineStyle.Solid,
						MinorGridlineStyle = LineStyle.Dot,
						IntervalType = DateTimeIntervalType.Years,
                        Position = AxisPosition.Bottom,
                        Title = "Years",
                        StringFormat = "yyyy"
                    };

					

					//Define Y-Axis
					Yaxis = new LinearAxis();
					switch (costOrCount)
					{
						case "cost":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 100;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 50;
							Yaxis.Title = "Cost (£)";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;
							break;

						case "count":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 1;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 1;
							Yaxis.Title = "Number of Cases";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;
							break;

						default:
							MessageBox.Show("Invalid flag. Expected 'case' or 'cost' - received: " + costOrCount);
							break;
					}

					//create line series
					myLineSeries = new LineSeries()
					{
						CanTrackerInterpolatePoints = false,
						MarkerFill = OxyColors.SteelBlue,
						MarkerType = MarkerType.Circle
                    };

                    //add line series points
                    foreach (KeyValuePair<int, decimal> datum in data)
                    {
                        DateTime date = DateTime.Parse(string.Format("01/01/{0}", datum.Key));
                        myLineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date) , (double)datum.Value));
                    }

                    MyGraph.Axes.Add(Yaxis);
                    MyGraph.Axes.Add(Xaxis);
                    MyGraph.Series.Add(myLineSeries);

                    GraphView.Model = MyGraph;

                    break;

                case "Bar":

                    MyGraph = new PlotModel()
                    {
                        Title = YAxisComboBox.Text + " " + XAxisComboBox.Text + " Across All Years."
                    };

                    List<ColumnItem> barData = new List<ColumnItem>();
                    foreach (KeyValuePair<int, decimal> datum in data)
                    {
                        barData.Add(new ColumnItem((double)datum.Value));
                    }

                    myColumnSeries = new ColumnSeries()
                    {
                        ItemsSource = barData,
                        //LabelFormatString = "{0:F2}",
						LabelPlacement = LabelPlacement.Inside
                    };

					Xaxis = new CategoryAxis()
					{
						Key = "Time",
						ItemsSource = years,
						Position = AxisPosition.Bottom,
                        Title = "Time (Years)",
                        MinorGridlineStyle = LineStyle.Solid
                    };

					//Define Y-Axis
					Yaxis = new LinearAxis();
					switch (costOrCount)
					{
						case "cost":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 100;
							//Yaxis.Maximum = (int)mostExpensiveCase;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 50;
							Yaxis.Title = "Cost (£)";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;

							break;

						case "count":

							Yaxis.Position = AxisPosition.Left;
							Yaxis.MajorStep = 1;
							//Yaxis.Maximum = totalCaseCount;
							Yaxis.MinimumPadding = 0.05;
							Yaxis.Minimum = 0;							
							Yaxis.MinorStep = 1;
							Yaxis.Title = "Number of Cases";
							Yaxis.MajorGridlineStyle = LineStyle.Solid;
							Yaxis.MinorGridlineStyle = LineStyle.Solid;
							break;

						default:
							MessageBox.Show("Invalid flag. Expected 'case' or 'cost' - received: " + costOrCount);
							break;
					}

					MyGraph.Axes.Add(Yaxis);
                    MyGraph.Axes.Add(Xaxis);
                    MyGraph.Series.Add(myColumnSeries);

                    GraphView.Model = MyGraph;

                    break;

                default:
                    MessageBox.Show("Invalid Chart Type Specified in ReportsPage.SetupGraphWithSpecificYear().");
                    break;
            }





        }

    }
}
