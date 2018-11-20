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

namespace OQCTester.Dialogs
{
    /// <summary>
    /// Interaction logic for DiscoverabilityDialog.xaml
    /// </summary>
    public partial class DiscoverabilityDialog : Window
    {
        public DiscoverabilityDialog()
        {
            InitializeComponent();
            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };

            var profile = ProcessEditor.Product.Profiles.First();
            var discoverability = profile.Discoverability;

            Mode.SelectedIndex = discoverability.Mode == "No restriction" ? 0 : 1;

            Setup1.IsChecked = discoverability.DiscoverabilityCollection[0].IsChecked;
            Setup2.IsChecked = discoverability.DiscoverabilityCollection[1].IsChecked;
            Setup3.IsChecked = discoverability.DiscoverabilityCollection[2].IsChecked;
            Setup7.IsChecked = discoverability.DiscoverabilityCollection[3].IsChecked;
        }

        private void CancelAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            var profile = ProcessEditor.Product.Profiles.First();
            var discoverability = profile.Discoverability;

            discoverability.Mode = ((ComboBoxItem)Mode.SelectedItem).Content.ToString();

            discoverability.DiscoverabilityCollection[0].IsChecked = Setup1.IsChecked.Value;
            discoverability.DiscoverabilityCollection[1].IsChecked = Setup2.IsChecked.Value;
            discoverability.DiscoverabilityCollection[2].IsChecked = Setup3.IsChecked.Value;
            discoverability.DiscoverabilityCollection[3].IsChecked = Setup7.IsChecked.Value;
        }

        private void DiscoverabilityChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Setups != null)
            {
                if (Mode.SelectedIndex == 0)
                {
                    Setups.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Setups.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
