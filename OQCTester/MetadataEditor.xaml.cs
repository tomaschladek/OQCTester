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
    /// Interaction logic for MetadataEditor.xaml
    /// </summary>
    public partial class MetadataEditor : UserControl
    {
        public static MetadataEditor This;

        public MetadataEditor()
        {
            InitializeComponent();
            This = this;
        }

        private void EditMetadata(object sender, MouseButtonEventArgs e)
        {
            var dialog = new MetadataDialog();
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

            ProductLabel.Content = ProcessEditor.Product.Name;
            NameLabel.Content = metadata.Name;
            VersionLabel.Content = metadata.Version;
            DescriptionLabel.Content = metadata.Description;
        }

        private void RevertItem(object sender, MouseButtonEventArgs e)
        {
            if (new YesNoDialog { DataContext = "All changes in this section will be lost. Do you want to continue?" }.ShowDialog() == true)
            {
                var profile = ProcessEditor.Product.Profiles.First().Metadata;
                profile.Name = "Soft test";
                profile.Description = "Soft test of the device";
                profile.Product = ProcessEditor.Product;
                Update();
            }
        }
    }
}
