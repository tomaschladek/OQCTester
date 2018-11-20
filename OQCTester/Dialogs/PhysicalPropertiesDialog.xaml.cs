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
    /// Interaction logic for PhysicalPropertiesDialog.xaml
    /// </summary>
    public partial class PhysicalPropertiesDialog : Window
    {
        public PhysicalPropertiesDialog()
        {
            InitializeComponent();
            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };

            var profile = ProcessEditor.Product.Profiles.First();
            CurrentBox.IsChecked = profile.Metadata.ProductConfiguration.Contains("Current");
            HallBox.IsChecked = profile.Metadata.ProductConfiguration.Contains("Hall");
            TemperatureBox.IsChecked = profile.Metadata.ProductConfiguration.Contains("Temperature");
        }

        private void CancelAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            var profile = ProcessEditor.Product.Profiles.First();
            var metadata = profile.Metadata;
            var properties = new List<string>();
            if (CurrentBox.IsChecked.Value)
            {
                properties.Add("Current");
            }
            if (HallBox.IsChecked.Value)
            {
                properties.Add("Hall");
            }
            if (TemperatureBox.IsChecked.Value)
            {
                properties.Add("Temperature");
            }

            var text = "";
            foreach (var property in properties)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    text += "-";
                }

                text += property;
            }
            metadata.ProductConfiguration = text;
        }
    }
}
