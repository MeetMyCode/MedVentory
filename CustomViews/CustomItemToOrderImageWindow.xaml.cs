using InterventionalCostings.CustomViews;
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
using System.Windows.Shapes;

namespace InterventionalCostings.Inventory_Item_Classes
{
    /// <summary>
    /// Interaction logic for CustomItemToOrderImageWindow.xaml
    /// </summary>
    public partial class CustomItemToOrderImageWindow : System.Windows.Window
    {
        public CustomItemToOrderImageWindow(CustomItemToOrder item)
        {
            InitializeComponent();

            string imagePath = StaticData.GetImagePath(item.Category);

            Uri ImagePath = new Uri(imagePath + item.RefNumber + ".jpg", UriKind.Relative);
            BitmapImage myImage = new BitmapImage(ImagePath);
            ProductImage.Source = myImage;
        }

		private void Close(object sender, MouseButtonEventArgs e)
		{
			Close();
		}
	}
}
