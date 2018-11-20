using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using OQCTester.Dialogs;
using OQCTester.Dtos;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow This { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            DataContext = new AppState(
                new Dtos.MenuItem("TE", "Testing", Brushes.Goldenrod),
                new Dtos.MenuItem("ED", "Editor", Brushes.LightSeaGreen),
                new Dtos.MenuItem("BI", "BI", Brushes.LightSalmon),
                new Dtos.MenuItem("RE", "Results", Brushes.Orange),
                new Dtos.MenuItem("LG", "Logs", Brushes.MediumPurple),
                new Dtos.MenuItem("?", "Help", Brushes.CornflowerBlue),
                new Dtos.MenuItem("@", "About", Brushes.DeepSkyBlue)
            );
            This = this;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LogoutAction(sender, null);
        }

        private void ProgressMenu_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is PackIcon icon && icon.ContextMenu != null)
            {
                icon.ContextMenu.IsOpen = true;
                icon.ContextMenu.Visibility = Visibility.Visible;
                icon.ContextMenu.PlacementTarget = icon;
            }
            if (e.LeftButton == MouseButtonState.Pressed && sender is DockPanel panel && panel.ContextMenu != null)
            {
                panel.ContextMenu.IsOpen = true;
                panel.ContextMenu.Visibility = Visibility.Visible;
                panel.ContextMenu.PlacementTarget = panel;
            }
        }

        private void LogoutAction(object sender, MouseButtonEventArgs e)
        {
            UserContainer.Visibility = Visibility.Collapsed;
            while (true)
            {
                var dialog = new TaskIdDialog();
                dialog.Owner = MainWindow.This;
                dialog.DataContext = "Scan user ID";
                dialog.ShowDialog();

                if (dialog.DialogResult == true)
                {
                    break;
                }
            }
            UserContainer.Visibility = Visibility.Visible;
        }

        private void InitialScreenAction(object sender, MouseButtonEventArgs e)
        {
            if (((AppState) DataContext).IsOqc == EMode.Oqc)
            {
                if (OqcLayout.This._mode == OqcLayout.EOqcMode.Running)
                {
                    var dialog = new YesNoDialog();
                    dialog.Owner = MainWindow.This;
                    dialog.DataContext = "Are you sure you want to cancel current session?";
                    dialog.ShowDialog();

                    if (dialog.DialogResult != true)
                    {
                        return;
                    }
                }
                ((AppState)DataContext).IsOqc = EMode.Home;
            }
            else if (((AppState) DataContext).IsOqc == EMode.Editor)
            {
                if (ProcessEditor.This.SelectorView.Visibility != Visibility.Visible)
                {
                    var dialog = new YesNoDialog();
                    dialog.Owner = MainWindow.This;
                    dialog.DataContext = "Are you sure you want discard changes made to the profile?";
                    dialog.ShowDialog();

                    if (dialog.DialogResult != true)
                    {
                        return;
                    }
                }
                ((AppState)DataContext).IsOqc = EMode.Home;
            }
            AppBarTitle.Content = "Welcome page";
            var appState = (AppState)DataContext;
            ContentContainer.Children.Clear();
            var oqcLayout = new WelcomePage();
            oqcLayout.DataContext = appState;
            ContentContainer.Children.Add(oqcLayout);
            HomeIcon.Visibility = Visibility.Hidden;
        }
    }
}
