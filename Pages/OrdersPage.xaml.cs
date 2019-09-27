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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InterventionalCostings.Inventory_Item_Classes;
using InterventionalCostings.CustomViews;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using InterventionalCostings.Static_Data;
using Microsoft.Exchange.WebServices.Data;
using System.Security.Cryptography.X509Certificates;
using WpfAnimatedGif;

namespace InterventionalCostings.Pages
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    /// 

    public interface IUpdateEmailProgress{

        void TickEmailComplete();
    }

    public partial class OrdersPage : Page, ICalculateTotalCostOfMarkedItems, IGetUserPassword
    {
        string userPassword = "";
        List<ItemToOrder> ItemsToOrderList;
        private StackPanel StackOfItemsToOrder = new StackPanel();
        EmailSend emailSendWindowReference;

        Label UpToDate = new Label();

        public OrdersPage()
        {
            InitializeComponent();

            SetupUpToDateButton();
            RefreshScrollViewer();

            CustomTitleBar titleBar = new CustomTitleBar();
            titleBar.TitleBarTitleText.Content = "These Items Need Ordering:";
            MainGrid.Children.Add(titleBar);

        }

        private void SetupUpToDateButton()
        {
            UpToDate.Content = @"Ordering is Up to Date!";
            UpToDate.FontSize = 20;
            UpToDate.VerticalAlignment = VerticalAlignment.Center;
            UpToDate.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void PopulateStackOfItemsToOrder()
        {
            foreach (ItemToOrder item in ItemsToOrderList)
            {
                CustomItemToOrder ItemForOrdering = new CustomItemToOrder(item.ID, item.Description, item.RefNumber, item.QuantityUsed, item.PackSize, item.Cost, item.Category);
                ItemForOrdering.OrdersPageReference = this;


                //If pack size > quantity used, display a cross.
                if (item.PackSize > item.QuantityUsed)
                {
                    ItemForOrdering.CrossOrTick.Source = new BitmapImage(new Uri(@"/Images/RedCross.png", UriKind.Relative));
                }

                StackOfItemsToOrder.Children.Add(ItemForOrdering);
            }

            OrderingScrollViewer.Content = StackOfItemsToOrder;


        }

        private void ClearOrdersConfirmation(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you Sure? This will clear the ordered Items from the database.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                WriteOrdersListToFileAndEmail();
            }
        }

        private async void WriteOrdersListToFileAndEmail()
        {
            var itemsToOrder = StackOfItemsToOrder.Children;
            int numItems = itemsToOrder.Count;
            string emailBody = "";
            string emailHeading = "<Body style='margin:auto; background:darkgray; height:1000px; width:80%;'><h1 style='background:lightskyblue; Margin:auto; text-align:center'>Please Order the Following:</h1>";
            string emailTailing = "</Body>";

            for (int i = 0; i <= numItems - 1; i++)
            {
                if (((CustomItemToOrder)itemsToOrder[i]).CrossOrTick.Source.ToString() == @"pack://application:,,,/Images/GreenTick.png")
                {
                    int count = i + 1;//This is a counter used in numbering the items to order within the email.
                    emailBody = emailBody + CreateOrderString((CustomItemToOrder)itemsToOrder[i], ref count);
                }
            }

            emailBody = emailHeading + emailBody + emailTailing;

            string password = getUserPassword();

            emailSendWindowReference = new EmailSend();
            emailSendWindowReference.Show();

            try
            {
                System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(() => SendEmail(emailBody, password));
                await task;

                IUpdateEmailProgress emailProgress = emailSendWindowReference;
                emailProgress.TickEmailComplete();

                ClearOrderedItemsFromDbTable();
            }
            catch (Exception e)
            {
                switch (e.HResult.ToString())
                {
                    case "-2146233088":
                        emailSendWindowReference.emailStatusText.Content = "Sending failed. Did you enter your password in correctly?";
                        break;

                    default:
                        MessageBox.Show("Sending failed. Unknown Error Code Received: " + e.HResult.ToString());
                        break;
                }

                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("/Images/RedCross.png", UriKind.Relative);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(emailSendWindowReference.emailSending, image);
            }
        }

        private void SendEmail(string emailBody, string password)
        {

            //string userName = DBController.GetEmailAddressFromUserName(Environment.UserName);
			string OrdererEmailAddress = Properties.mySettings.Default.OrdererEmail;

            //Debugging
            //MessageBox.Show("Environment.Username is: " + Environment.UserName);
            //MessageBox.Show("Order Address is: "+ OrdererEmailAddress);

            // Create instance of IEWSClient class by giving credentials
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010);
            service.Credentials = new WebCredentials(OrdererEmailAddress, password);
            service.Url = new Uri("https://mail.nhs.net/ews/exchange.asmx");
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

            //**DON'T DELETE** Using AutodiscoverUrl seems to fail intermittently. Error of "Cannot locate AutoDiscover Service".
            //service.AutodiscoverUrl(OrdererEmailAddress, RedirectionUrlValidationCallback);

            EmailMessage email = new EmailMessage(service);
            email.ToRecipients.Add(OrdererEmailAddress);

            email.Subject = "**TEST** Interventional Order Request";
            email.Body = new MessageBody(emailBody);

			try
			{
				email.Send();
			}
			catch (Exception)
			{
				MessageBox.Show(@"Error - Could not send. Is the password correct?");
			}


        }

        private string getUserPassword()
        {
            GetUserPassword pwWindow = new GetUserPassword(this);
            pwWindow.ShowDialog();

            //MessageBox.Show("password is: "+ userPassword);

            return userPassword;
        }

        public void UpdateUserPassword(string pw)
        {
            userPassword = pw;
        }

        private string CreateOrderString(CustomItemToOrder itemToOrder, ref int count)
        {
            //Format as HTML otherwise text won't be displayed on new lines
            string orderString = "";
            orderString = "<p style='padding-top:20px; margin:auto; margin-left:100px; width:80%'>"+ count + ") " +
                itemToOrder.QuantityUsed + "x " +
                itemToOrder.RefNumber + " - " +
                itemToOrder.Description + " - " +
                itemToOrder.PackSize + " per pack - £" +
                itemToOrder.Cost + " per pack.</p>";

            return orderString;
        }

        private void CancelOrdersConfirmation(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow;
            main.SizeToContent = SizeToContent.Manual;
			main.MaxHeight = 10000;
			main.MaxWidth = 10000;
			main.HorizontalAlignment = HorizontalAlignment.Stretch;
			main.VerticalAlignment = VerticalAlignment.Stretch;
			main.WindowState = WindowState.Maximized;

			StartPage startPage = StartPage.StartInstance;
            NavigationService.Navigate(startPage);
        }

        private void ClearOrderedItemsFromDbTable()
        {
            var itemsToOrder = StackOfItemsToOrder.Children;
            int numItems = itemsToOrder.Count;
            List<string> listOfRefNums = new List<string>();

            for (int i = 0; i <= numItems - 1; i++)
            {
                if (((CustomItemToOrder)itemsToOrder[i]).CrossOrTick.Source.ToString() == @"pack://application:,,,/Images/GreenTick.png")
                {
                    string refNum = ((CustomItemToOrder)itemsToOrder[i]).RefNumber;
                    string sqlQuery = @"DELETE FROM ItemsToOrder WHERE RefNumber = @refNum";
                    DBController.RemoveItemFromDatabase(sqlQuery, refNum);
                }
            }
            RefreshScrollViewer();

        }

        private void RefreshScrollViewer()
        {
            ItemsToOrderList = DBController.GetListOfItemsToOrder();

            StackOfItemsToOrder.Children.Clear();
            PopulateStackOfItemsToOrder();
            CalculateCostOfMarkedItems();
            
            if (StackOfItemsToOrder.Children.Count == 0)
            {
                StackOfItemsToOrder.Children.Add(UpToDate);
                MarkAsOrdered.IsEnabled = false;
            }
        }

        public void CalculateCostOfMarkedItems()
        {
            var StackOfItems = StackOfItemsToOrder.Children;
            decimal cost = 0;

            foreach (CustomItemToOrder item in StackOfItems)
            {
                if (item.CrossOrTick.Source.ToString() == @"pack://application:,,,/Images/RedCross.png")
                {
                    //Don't add cost.
                }
                else
                {
                    cost += Decimal.Parse(item.OrderCost.Content.ToString());
                }
            }

            TotalOrderCost.Content = string.Format(@"£{0}",cost);

        }

        private static bool CertificateValidationCallBack(object sender, 
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }


        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow;
            main.SizeToContent = SizeToContent.WidthAndHeight;
            main.WindowState = WindowState.Normal;
			main.WindowStyle = WindowStyle.SingleBorderWindow;
			main.MaxWidth = 1500;
			main.MaxHeight = 900;
        }
    }//END OF CLASS
}
