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
    /// Interaction logic for MetadataDialog.xaml
    /// </summary>
    public partial class MetadataDialog : Window
    {
        public MetadataDialog()
        {
            InitializeComponent();
            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };

            var profile = ProcessEditor.Product.Profiles.First();
            var metadata = profile.Metadata;

            Product.SelectedIndex = 0;
            Name.Text = metadata.Name;
            Version.Content = metadata.Version;
            Description.Text = metadata.Description;
            NameError.Visibility = Visibility.Collapsed;
        }

        private void CancelAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveAction(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text.Trim()))
            {
                NameError.Visibility = Visibility.Visible;
                return;
            }
            DialogResult = true;
            var profile = ProcessEditor.Product.Profiles.First();
            var metadata = profile.Metadata;

            metadata.Name = Name.Text;
            metadata.Version = int.Parse(Version.Content.ToString());
            metadata.Description = Description.Text;
        }
    }
}
