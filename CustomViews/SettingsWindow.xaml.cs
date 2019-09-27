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
using System.Windows.Shapes;

namespace InterventionalCostings.CustomViews
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : System.Windows.Window
	{
		public SettingsWindow()
		{
			InitializeComponent();

			//Insert the Custom Title Bar
			CustomTitleBar settingsTitle = new CustomTitleBar();
			settingsTitle.TitleBarTitleText.Content = "Settings";
			SettingsHeading.Children.Add(settingsTitle);

			//Load Settings Values into textboxes
			OrdererEmail.Text = Properties.mySettings.Default.OrdererEmail;
			SmtpServer.Text = Properties.mySettings.Default.SmtpServerAddress;
			SmtpPort.Text = Properties.mySettings.Default.SmtpServerPortNumber.ToString();
			ImapServer.Text = Properties.mySettings.Default.ImapServerAddress;
			ImapPort.Text = Properties.mySettings.Default.ImapServerPortNumber.ToString();

		}

		private void SaveSettingsButtonClicked(object sender, RoutedEventArgs e)
		{
			//Write new Settings and Save
			Properties.mySettings.Default.OrdererEmail = OrdererEmail.Text;
			Properties.mySettings.Default.SmtpServerAddress = SmtpServer.Text;
			Properties.mySettings.Default.SmtpServerPortNumber = Int32.Parse(SmtpPort.Text);
			Properties.mySettings.Default.ImapServerAddress = ImapServer.Text;
			Properties.mySettings.Default.ImapServerPortNumber = Int32.Parse(ImapPort.Text);
			Properties.mySettings.Default.Save();
			this.Close();
		}
	}



}
