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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TaskIdDialog : Window
    {
        public TaskIdDialog()
        {
            InitializeComponent();
            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };
        }

        private void ConfirmAction(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;

        }

        private void CancelAction(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
