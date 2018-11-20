using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using OQCTester.Dialogs;
using Timer = System.Timers.Timer;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for OqcLayout.xaml
    /// </summary>
    public partial class OqcLayout : UserControl
    {
        public EOqcMode _mode;
        private Timer _timer;
        private Timer _timerNegative;
        private int _counter;
        private static double _max = 100;
        private int _lastValue = (int) _max;
        private int _counterNegative;

        public OqcLayout()
        {
            InitializeComponent();
            _mode = EOqcMode.Stopped;
            ActionIcon.Visibility = Visibility.Visible;
            ActionIcon.Kind = PackIconKind.Timer;
            this.Loaded += OqcLayout_Loaded;
            this.Loaded += InitializedConnection();
            this.SizeChanged += OqcLayout_Loaded;
            PositionLayout.Visibility = Visibility.Collapsed;
            This = this;
        }

        public static OqcLayout This { get; set; }

        private RoutedEventHandler InitializedConnection()
        {
            return async (a, b) =>
            {
                await Task.Delay(2000);
                UIElement_OnMouseDown(null, null);
            };
        }

        private void OqcLayout_Loaded(object sender, RoutedEventArgs e)
        {
            var size = Math.Min(ContentContainer.ActualWidth, ContentContainer.ActualHeight);
            OriginalEllipse.Width = OriginalEllipse.Height = size / 2;
            UpdateButtonHeight();
        }

        private void UpdateButtonHeight()
        {
            ActionButton.Height = ContentContainer.ActualHeight - 260;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (_mode)
            {
                case EOqcMode.Stopped:
                    _mode = EOqcMode.Connected;
                    ProgressMenu.Visibility = Visibility.Collapsed;
                    ActionButton.Content = "Scan Serial number";
                    ActionIcon.Visibility = Visibility.Visible;
                    ActionIcon.Kind = PackIconKind.QrcodeScan;

                    SystemValue.Content = "Ready";
                    DevicesValue.Content = "Connected";
                    TemperatureValue.Content = "25°C";
                    break;
                case EOqcMode.Running:
                    _timer.Stop();
                    _mode = EOqcMode.Paused;
                    ActionButton.Content = "Resume";
                    ActionIcon.Visibility = Visibility.Visible;
                    ActionIcon.Kind = PackIconKind.CursorPointer;
                    ActionContainer.Visibility = Visibility.Visible;
                    PositionLayout.Visibility = Visibility.Collapsed;

                    SystemValue.Content = "Paused";
                    _lastValue = _counter;
                    break;
                case EOqcMode.Connected:
                    SetProgress(0);
                    ActionIcon.Visibility = Visibility.Visible;
                    ActionIcon.Kind = PackIconKind.QrcodeScan;

                    SerialNumberValue.Content = "AEAS4275";
                    ActionButton.Content = "Scan Batch";
                    _mode = EOqcMode.AwaitingBatch;
                    PositionLayout.Visibility = Visibility.Collapsed;
                    ActionContainer.Visibility = Visibility.Visible;
                    PositionLayout.Background = Brushes.Transparent;

                    ResultContainerPassed.Visibility = Visibility.Collapsed;
                    ResultContainerFailed.Visibility = Visibility.Collapsed;

                    ProgressContainer.Visibility = Visibility.Visible;
                    _timerNegative?.Stop();
                    break;

                case EOqcMode.AwaitingBatch:
                    BatchValue.Content = "QSEW2063";
                    _mode = EOqcMode.WaitingForStart;
                    ActionButton.Content = "Start OQC";
                    ActionIcon.Kind = PackIconKind.CursorPointer;
                    ActionContainer.Background = ActionButton.Background;
                    break;
                case EOqcMode.WaitingForStart:
                    _mode = EOqcMode.Running;
                    SystemValue.Content = "Running";
                    ProgressNegativeDuration.Content = "00:00";
                    ProgressNegativeDuration2.Content = "00:00";

                    _counter = _lastValue;
                    _timer = new Timer(50);
                    _timer.Elapsed += TimerOnElapsed;
                    _timer.Start();

                    ProgressMenu.Visibility = Visibility.Visible;
                    ActionContainer.Visibility = Visibility.Collapsed;
                    PositionLayout.Visibility = Visibility.Visible;
                    break;
                case EOqcMode.Finished:
                    _mode = EOqcMode.Connected;
                    UIElement_OnMouseDown(sender,e);
                    break;
                case EOqcMode.Paused:
                    _mode = EOqcMode.WaitingForStart;
                    UIElement_OnMouseDown(sender,e);

                    break;
                case EOqcMode.Canceled:
                    _mode = EOqcMode.Connected;
                    _lastValue = (int) _max;
                    UIElement_OnMouseDown(sender,e);

                    break;
            }

            UpdateButtonHeight();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var progress = Math.Round((_max - _counter) / _max * 100);
            _counter--;
            if (_counter == 0)
            {
                _timer.Stop();
                _mode = EOqcMode.Finished;
                SerialNumberValue.Dispatcher.Invoke(() =>
                {
                    SetTime(new DateTime(2000, 1, 1, 1, 0, 0));
                    StateValue.Content = "";
                    SetProgress(100);

                    ActionButton.Content = "Scan next product's serial number";
                    SystemValue.Content = "Finished";
                    ActionButton.Visibility = Visibility.Visible;
                    ActionIcon.Visibility = Visibility.Visible; 
                    ProgressContainer.Visibility = Visibility.Collapsed;
                    if (new Random().Next(0, 100) > 60)
                    {
                        ResultContainerPassed.Visibility = Visibility.Visible;
                        PositionLayout.Background = Brushes.Green;
                    }
                    else
                    {
                        PositionLayout.Background = Brushes.Red;
                        ResultContainerFailed.Visibility = Visibility.Visible;
                    }
                    ProgressMenu.Visibility = Visibility.Collapsed;
                    _lastValue = (int)_max;


                    _counter = _lastValue;
                    _counterNegative = 0;
                    _timerNegative = new Timer(1000);
                    _timerNegative.Elapsed += TimerNegativeOnElapsed;
                    _timerNegative.Start();
                });

            }
            else
            {
                SerialNumberValue.Dispatcher.Invoke(() =>
                {
                    StateValue.Content = _counter/_max > 0.5 ? "Measuring" : "Calibrating";
                    var time = new DateTime(2018, 10, 2, 6, _counter / 60, _counter % 60);
                    SetTime(time);
                    SetProgress(progress);
                });
            }

            }

        private void SetProgress(double progress)
        {
            ProgressText.Text = $"{progress}%";
            ProgressElipse.Width = ProgressElipse.Height = OriginalEllipse.Height * (progress/100);
        }

        private void TimerNegativeOnElapsed(object sender, ElapsedEventArgs e)
        {
            _counterNegative++;
            var time = new DateTime(2018, 10, 2, 6, _counterNegative / 60, _counterNegative % 60);
            ProgressNegativeDuration.Dispatcher.Invoke(() =>
            {
                ProgressNegativeDuration.Content = $"{time.Minute:D2}:{time.Second:D2}";
                ProgressNegativeDuration2.Content = $"{time.Minute:D2}:{time.Second:D2}";
            });
        }

        public enum EOqcMode
        {
            Stopped,Connected,AwaitingBatch,Running,Paused,Finished, WaitingForStart, Canceled
        }

        private void CancelExecution(object sender, MouseButtonEventArgs e)
        {
            if (new YesNoDialog {DataContext = "Are you sure you want to cancel execution?"}.ShowDialog() != true)
            {
                return;
            }
            _timer.Stop();
            _mode = EOqcMode.Canceled;
            SetTime(new DateTime(2000, 1, 1, 1, 0, 0));
            SetProgress(0);
            StateValue.Content = "";

            ActionButton.Content = "Scan Next Product";
            ActionIcon.Kind = PackIconKind.QrcodeScan;
            SystemValue.Content = "Canceled";
            ActionContainer.Visibility = Visibility.Visible;
            PositionLayout.Visibility = Visibility.Collapsed;
            ProgressMenu.Visibility = Visibility.Collapsed;
        }

        private void SetTime(DateTime time)
        {
            ProgressDuration.Content = $"{time.Minute:D2}:{time.Second:D2}";
            
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

        private void PositionLayout_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_mode == EOqcMode.Finished || _mode == EOqcMode.Connected)
            {
                UIElement_OnMouseDown(null,null);
            }
        }

        private void ActionIcon_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_mode == EOqcMode.AwaitingBatch)
            {
                ActionContainer.Background = Brushes.Red; //= "{StaticResource PrimaryHueMidBrush}"
                var timer = new Timer();
                timer.Interval = 500;
                timer.Elapsed += (o, args) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ActionContainer.Background = ActionButton.Background;
                    });
                    timer.Stop();
                };
                timer.Start();
            }
            else
            {
                UIElement_OnMouseDown(null, null);
            }
        }
    }
}
