using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        private ObservableCollection<LogViewItemDto> _logs;

        private const string DefaultValue = "Filter by: level or timestamp e.g.\"level:debug, timestamp:today\"";

        public LogView()
        {
            InitializeComponent();
            _logs = new ObservableCollection<LogViewItemDto>
            {
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.MyClass", "Started execution"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.MyClass", "Finished execution"),
                new LogViewItemDto( DateTime.Now, "Error", "MyProgram.MyClass", "Assertion of results"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.MyClass", "Recovery from failure"),
                new LogViewItemDto( DateTime.Now, "Critical", "MyProgram.MyApp", "Application state is not stable"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.TheirProgram", "Evaluating new arguments"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.TheirProgram", "Finished action"),
                new LogViewItemDto( DateTime.Now, "Error", "MyProgram.TheirProgram", "Re-trigger actions"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.TheirProgram", "Recovery from failure"),
                new LogViewItemDto( DateTime.Now, "Info", "MyProgram.MyApp", "Application state is not stable"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.MyClass", "Started execution"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.MyClass", "Finished execution"),
                new LogViewItemDto( DateTime.Now, "Error", "MyProgram.MyClass", "Assertion of results"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.MyClass", "Recovery from failure"),
                new LogViewItemDto( DateTime.Now, "Critical", "MyProgram.MyApp", "Application state is not stable"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.TheirProgram", "Evaluating new arguments"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.TheirProgram", "Finished action"),
                new LogViewItemDto( DateTime.Now, "Error", "MyProgram.TheirProgram", "Re-trigger actions"),
                new LogViewItemDto( DateTime.Now, "Debug", "MyProgram.TheirProgram", "Recovery from failure"),
                new LogViewItemDto( DateTime.Now, "Info", "MyProgram.MyApp", "Application state is not stable"),
                new LogViewItemDto( new DateTime(1999,10,20,3,5,7), "Info", "MyProgram.MyApp", "Application state is not stable"),
            };
            Populate();
            FilterTextBox.Text = DefaultValue;
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        private void ButtonsDebugChip_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonsDebugChip.Visibility = Visibility.Collapsed;
            Populate();
        }

        private void ButtonsCriticalChip_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonsCriticalChip.Visibility = Visibility.Collapsed;
            Populate();
        }

        private void ButtonsErrorChip_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonsErrorChip.Visibility = Visibility.Collapsed;
            Populate();
        }

        private void ButtonsTodayChip_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonsTodayChip.Visibility = Visibility.Collapsed;
            Populate();
        }

        private void KeyUpAction(object sender, KeyEventArgs e)
        {
            if (FilterTextBox.Text.ToLower().Trim() == "level:debug")
            {
                FilterTextBox.Text = "";
                ButtonsDebugChip.Visibility = Visibility.Visible;
            }
            if (FilterTextBox.Text.ToLower().Trim() == "level:critical")
            {
                FilterTextBox.Text = "";
                ButtonsCriticalChip.Visibility = Visibility.Visible;
            }
            if (FilterTextBox.Text.ToLower().Trim() == "timestamp:today")
            {
                FilterTextBox.Text = "";
                ButtonsTodayChip.Visibility = Visibility.Visible;
            }
            if (FilterTextBox.Text.ToLower().Trim() == "level:info")
            {
                FilterTextBox.Text = "";
                ButtonsInfoChip.Visibility = Visibility.Visible;
            }

            Populate();
        }

        private void Populate()
        {
            var newLogs = _logs.Where(log =>
            {
                if (ButtonsDebugChip.Visibility == Visibility.Collapsed
                    && ButtonsInfoChip.Visibility == Visibility.Collapsed
                    && ButtonsCriticalChip.Visibility == Visibility.Collapsed
                    && ButtonsTodayChip.Visibility == Visibility.Collapsed
                    && (string.IsNullOrEmpty(FilterTextBox.Text.Trim())
                        || FilterTextBox.Text == DefaultValue))
                    return true;
                var isTime = ButtonsTodayChip.Visibility != Visibility.Visible || DateTime.Now.Day == log.Timestamp.Day && DateTime.Now.Month == log.Timestamp.Month && DateTime.Now.Year == log.Timestamp.Year;
                var isLevel = ButtonsDebugChip.Visibility != Visibility.Visible && ButtonsCriticalChip.Visibility != Visibility.Visible && ButtonsInfoChip.Visibility != Visibility.Visible || ((log.Level.ToLower() == "debug" && ButtonsDebugChip.Visibility == Visibility.Visible)
                                                                                                                                                                                                || (log.Level.ToLower() == "info" && ButtonsInfoChip.Visibility == Visibility.Visible)
                                                                                                                                                                                                || (log.Level.ToLower() == "critical" &&
                                                                                                                                                                                                    ButtonsCriticalChip.Visibility == Visibility.Visible));
                if (isLevel && isTime)
                {
                    return true;
                }
                //    if (log.Level.ToLower() == "debug" && ButtonsDebugChip.Visibility == Visibility.Visible) return true;
                //if (log.Level.ToLower() == "info" && ButtonsInfoChip.Visibility == Visibility.Visible) return true;
                //if (log.Level.ToLower() == "critical" && ButtonsCriticalChip.Visibility == Visibility.Visible)
                //    return true;
                //if (ButtonsTodayChip.Visibility == Visibility.Visible && DateTime.Now.Day == log.Timestamp.Day && DateTime.Now.Month == log.Timestamp.Month && DateTime.Now.Year == log.Timestamp.Year)
                //    return true;
                if (!string.IsNullOrEmpty(FilterTextBox.Text.Trim()) && (log.Class.Contains(FilterTextBox.Text) || log.Message.Contains(FilterTextBox.Text)))
                    return true;
                return false;
            });
            DataGridLogView.ItemsSource = newLogs;
        }

        private void ButtonsInfoChip_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonsInfoChip.Visibility = Visibility.Collapsed;
            Populate();
        }

        private void ClearAction(object sender, MouseButtonEventArgs e)
        {
            _logs= new ObservableCollection<LogViewItemDto>();
            Populate();
        }

        private void ExportAction(object sender, MouseButtonEventArgs e)
        {
            new OpenFileDialog().ShowDialog();
        }

        private void FilterTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterTextBox.Text == DefaultValue)
            {
                FilterTextBox.Text = "";
            }
        }

        private void FilterTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterTextBox.Text == "")
            {
                FilterTextBox.Text = DefaultValue;
            }
        }
    }
    public class LogViewItemDto
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Class { get; set; }
        public string Message { get; set; }

        public LogViewItemDto(DateTime timestamp, string level, string _class, string message)
        {
            Timestamp = timestamp;
            Level = level;
            Class = _class;
            Message = message;
        }
    }
}
