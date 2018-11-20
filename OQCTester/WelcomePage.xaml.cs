using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using OQCTester.Dialogs;
using OQCTester.Dtos;
using UserControl = System.Windows.Controls.UserControl;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : UserControl
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new TaskIdDialog();
            dialog.Owner = MainWindow.This;
            dialog.DataContext = "Scan Task ID";
            dialog.ShowDialog();

            if (dialog.DialogResult == false)
            {
                return;
            }
            ExchangeView(new OqcLayout(), "Awesome product 10-30 - Core test", EMode.Oqc);
        }

        private void ExchangeView(UserControl layout, string title, EMode mode)
        {
            var appState = (AppState)DataContext;
            MainWindow.This.ContentContainer.Children.Clear();
            layout.DataContext = appState;
            MainWindow.This.ContentContainer.Children.Add(layout);
            MainWindow.This.AppBarTitle.Content = title;
            MainWindow.This.HomeIcon.Visibility = Visibility.Visible;
            ((AppState)DataContext).IsOqc = mode;
        }

        private void OpenProcessEditor(object sender, MouseButtonEventArgs e)
        {
            ExchangeView(new ProcessEditor(), "Editor",EMode.Editor);
        }

        private void OpenSupervision(object sender, MouseButtonEventArgs e)
        {
            ExchangeView(new SupervisionSelector(), "BI",EMode.Bi);
        }

        private void OpenLogView(object sender, MouseButtonEventArgs e)
        {
            ExchangeView(new LogView(), "Log view", EMode.Logs);
        }

        private void OpenProductViewer(object sender, MouseButtonEventArgs e)
        {
            ExchangeView(new ProductViewer(), "Results", EMode.Results);
        }

        private void OpenHelp(object sender, MouseButtonEventArgs e)
        {
            Help.ShowHelp(null, "C:\\Program Files\\TortoiseSVN\\bin\\TortoiseMerge_en.chm");
        }

        private void OpenAbout(object sender, MouseButtonEventArgs e)
        {
            var dialog = new AboutDialog();
            dialog.Owner = MainWindow.This;
            dialog.ShowDialog();
        }
    }
}
