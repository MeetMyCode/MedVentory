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
using WpfAnimatedGif;

namespace InterventionalCostings.CustomViews
{
    /// <summary>
    /// Interaction logic for EmailSend.xaml
    /// </summary>
    public partial class EmailSend : System.Windows.Window, IUpdateEmailProgress
    {
        public EmailSend()
        {
            InitializeComponent();
        }

        public void TickEmailComplete()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("/Images/GreenTick.png", UriKind.Relative);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(emailSending, image);

            emailStatusText.Content = "Email Sent!";
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
