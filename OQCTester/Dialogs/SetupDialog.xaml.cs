using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OQCTester.Dialogs
{
    /// <summary>
    /// Interaction logic for SetupDialog.xaml
    /// </summary>
    public partial class SetupDialog : Window
    {
        public SetupDialog()
        {
            InitializeComponent();
            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };

            var profile = ProcessEditor.Product.Profiles.First();
            var metadata = profile.Metadata;

            Setup.SelectedIndex = 0;
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

            metadata.Setup = Setup.SelectionBoxItem.ToString();
        }
    }
}
