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

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for PhysicalPropertiesView.xaml
    /// </summary>
    public partial class PhysicalPropertiesView : UserControl
    {
        public static PhysicalPropertiesView This;

        public PhysicalPropertiesView()
        {
            InitializeComponent();
            This = this;
        }

        private void EditMetadata(object sender, MouseButtonEventArgs e)
        {
            var dialog = new PhysicalPropertiesDialog();
            dialog.Owner = MainWindow.This;
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                Update();
            }
        }

        public void Update()
        {
            var profile = ProcessEditor.Product.Profiles.First();
            var metadata = profile.Metadata;

            ProductConfigurationLabel.Content = string.IsNullOrEmpty(metadata.ProductConfiguration) ? "None" : metadata.ProductConfiguration;
        }

        private void RevertItem(object sender, MouseButtonEventArgs e)
        {
            if (new YesNoDialog { DataContext = "All changes in this section will be lost. Do you want to continue?" }.ShowDialog() == true)
            {
                ProcessEditor.Product.Profiles.First().Metadata.ProductConfiguration = "Current-Hall-Temperature";
                Update();
            }
        }
    }
}
