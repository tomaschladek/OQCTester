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
    /// Interaction logic for ExecutionPlanDialog.xaml
    /// </summary>
    public partial class ExecutionPlanDialog : Window
    {
        public ExecutionPlanDialog()
        {
            InitializeComponent();
            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };
            MinError.Visibility = MaxError.Visibility =
                StepsError.Visibility = SettleError.Visibility = Visibility.Collapsed;
        }

        private void CancelAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveAction(object sender, MouseButtonEventArgs e)
        {
            var isValid = true;
            MinError.Visibility = MaxError.Visibility =
                StepsError.Visibility = SettleError.Visibility = Visibility.Collapsed;
            int min;
            if (string.IsNullOrEmpty(MinValue.Text)
                || !int.TryParse(MinValue.Text, out min)
                || min < 0
            )
            {
                isValid = false;
                MinError.Visibility = Visibility.Visible;
            }
            if (string.IsNullOrEmpty(MaxValue.Text)
                || !int.TryParse(MaxValue.Text, out min)
                || min < 0
            )
            {
                isValid = false;
                MaxError.Visibility = Visibility.Visible;
            }
            if (string.IsNullOrEmpty(StepsValue.Text)
                || !int.TryParse(StepsValue.Text, out min)
                || min < 0
            )
            {
                isValid = false;
                StepsError.Visibility = Visibility.Visible;
            }
            if (string.IsNullOrEmpty(SettleValue.Text)
                || !int.TryParse(SettleValue.Text, out min)
                || min < 0
            )
            {
                isValid = false;
                SettleError.Visibility = Visibility.Visible;
            }
            if (!isValid) return;
            DialogResult = true;
        }
    }
}
