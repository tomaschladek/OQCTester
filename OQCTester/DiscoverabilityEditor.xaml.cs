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
using OQCTester.Dialogs;
using OQCTester.Dtos;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for DiscoverabilityEditor.xaml
    /// </summary>
    public partial class DiscoverabilityEditor : UserControl
    {
        public DiscoverabilityEditor()
        {
            InitializeComponent();
            This = this;
        }

        public static DiscoverabilityEditor This { get; set; }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new DiscoverabilityDialog();
            dialog.Owner = MainWindow.This;
            dialog.ShowDialog();

            Update();
        }

        public void Update()
        {
            var profile = ProcessEditor.Product.Profiles.First();
            var discoverability = profile.Discoverability;

            Mode.Content = discoverability.Mode;

            Setup1.Content = "PC 1";
            Setup1.Visibility = discoverability.DiscoverabilityCollection[0].IsChecked ? Visibility.Visible : Visibility.Collapsed;
            Setup2.Visibility = discoverability.DiscoverabilityCollection[1].IsChecked ? Visibility.Visible : Visibility.Collapsed;
            Setup3.Visibility = discoverability.DiscoverabilityCollection[2].IsChecked ? Visibility.Visible : Visibility.Collapsed;
            Setup7.Visibility = discoverability.DiscoverabilityCollection[3].IsChecked ? Visibility.Visible : Visibility.Collapsed;

            if (discoverability.Mode == "No restriction")
            {
                Setup2.Visibility = Setup3.Visibility = Setup7.Visibility = Visibility.Collapsed;
            }

            if (!discoverability.DiscoverabilityCollection[0].IsChecked
            && !discoverability.DiscoverabilityCollection[1].IsChecked
                && !discoverability.DiscoverabilityCollection[2].IsChecked
                && !discoverability.DiscoverabilityCollection[3].IsChecked)
            {
                Setup1.Visibility = Visibility.Visible;
                Setup1.Content = "No PC";
            }
        }

        private void RevertItem(object sender, MouseButtonEventArgs e)
        {

            if (new YesNoDialog { DataContext = "All changes in this section will be lost. Do you want to continue?" }.ShowDialog() == true)
            {
                ProcessEditor.Product.Profiles.First().Discoverability = new DiscoverabilityDto("Restricted access", new List<SetupPresenceDto>
                {
                    new SetupPresenceDto(false,"PC 1"),
                    new SetupPresenceDto(false,"PC 2"),
                    new SetupPresenceDto(false,"PC 3"),
                    new SetupPresenceDto(false,"PC 7")
                });
                Update();
            }
        }
    }
}
