using InterventionalCostings.Pages;
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
    /// Interaction logic for GetUserPassword.xaml
    /// </summary>
    /// 

    public interface IGetUserPassword
    {
        void UpdateUserPassword(string pw);
    }    

    public partial class GetUserPassword : System.Windows.Window
    {

        private OrdersPage OrdersPage;

        public GetUserPassword(OrdersPage rvp)
        {
            InitializeComponent();
            OrdersPage = rvp;
            passwordText.Focus();
        }

        public IGetUserPassword getUserPasswordDelegate;


        private void SendPassword(object sender, RoutedEventArgs e)
        {
            getUserPasswordDelegate = OrdersPage;
            getUserPasswordDelegate.UpdateUserPassword(passwordText.Password.ToString());
            this.Close();
        }

        private void EnterButtonPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendPassword(sender, e);
            }
        }
    }
}
