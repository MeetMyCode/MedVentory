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
    /// Interaction logic for ReferrerPicker.xaml
    /// </summary>
    /// 
    public partial class ReferrerPicker : System.Windows.Window
    {
        public string SelectedReferrer { get; private set; } // Property to store the chosen referrer

        public ReferrerPicker(List<Referrer> referrers)
        {
            InitializeComponent();
            PopulateReferrerList(referrers);
        }

        private void PopulateReferrerList(List<Referrer> referrers)
        {
            foreach (var referrer in referrers)
            {
                Label label = new Label
                {
                    Content = $"{referrer.Prefix} {referrer.FirstName} {referrer.Surname}"
                };

                label.Tag = label.Content; // Store referrer details
                label.MouseUp += Label_Clicked; // Attach click event

                ReferrerListPanel.Children.Add(label);
            }
        }

        // Event handler for label click
        private void Label_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label)
            {
                SelectedReferrer = label.Content.ToString();
                this.DialogResult = true; // Close window 
            }
        }

        private void ReferrerFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && ReferrerListPanel != null)
            {
                string filterText = textBox.Text.Trim().ToLower();

                foreach (UIElement element in ReferrerListPanel.Children)
                {
                    if (element is Label label && label.Content is string content)
                    {
                        label.Visibility = content.ToLower().Contains(filterText)
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                    }
                }
            }
        }
    }
}
