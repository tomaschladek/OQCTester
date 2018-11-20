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
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : Window
    {
        public YesNoDialog()
        {
            InitializeComponent();

            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };
        }

        private void YesAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        private void NoAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }
    }
}
