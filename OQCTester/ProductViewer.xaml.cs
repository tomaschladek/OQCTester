using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using OQCTester.Dtos;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for ProductViewer.xaml
    /// </summary>
    public partial class ProductViewer : UserControl
    {
        private const string DefaultValue = "Enter the serial number of your product to see the history";
        public ProductViewer()
        {
            InitializeComponent();
            FilterTextBox.Text = DefaultValue;
            InitializeChart(null);
        }

        private void InitializeChart(ChartDataSet dataset)
        {
            if (dataset == null)
            {
                return;
            }
            DataChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title =  dataset.Title,
                    Values = new ChartValues<double>(CurrentDataset.Select(dataset.Action).ToList()),
                    LineSmoothness = 0
                }
            };
        }

        private void ClearSelection(object sender, MouseButtonEventArgs e)
        {
            FilterTextBox.Text = "";
            ProductDetails.Visibility = Visibility.Collapsed;
            Profiles.SelectedIndex = 0;
            ExecutionPlan.SelectedIndex = 0;
            Selector_OnSelectionChanged(null,null);
            ExecutionPlan_OnSelectionChanged(null, null);
        }

        private double GetCurrent(DataItemDto item)
        {
            return  item?.Current ?? 0;
        }

        private double GetHall(DataItemDto item)
        {
            return  item?.Hall ?? 0;
        }

        private double GetTemperature(DataItemDto item)
        {
            return  item?.Temperature ?? 0;
        }

        private double GetStepId(DataItemDto item)
        {
            return  item?.StepId ?? 0;
        }

        private void ExecutionPlan_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Header1.Content = "Test vector";
            Header2.Content = "Test parameters";
            Description.Content = "Threshold test: Asserts value from the test vector and according to selected operator executes the assertion. e.g. |Value| <= threshold";
            DataGridParametersLayout.Visibility = Metadata.Visibility = Visibility.Collapsed;
            DataGridParametersTestLayout.Visibility = Data.Visibility = Visibility.Visible;
            if (ExecutionPlan.SelectedIndex == 0)
            {
                DataGridLayout.ItemsSource = CurrentDataset = new ObservableCollection<DataItemDto>
                {
                    new DataItemDto(3, 100, 200, 25),
                    new DataItemDto(3, 100, 200, 26),
                    new DataItemDto(3, 100, 200, 26),
                    new DataItemDto(7, 300, 600, 26),
                    new DataItemDto(7, 300, 600, 27),
                    new DataItemDto(7, 300, 600, 27),
                };
                Header1.Content = "Overview";
                Header2.Content = "Description";
                Description.Content = "Key values for the profile are represented in this table";
                DataGridParametersLayout.Visibility = Visibility.Collapsed;
                DataGridParametersTestLayout.Visibility = Visibility.Collapsed;
                ChartValues.SelectedIndex = 0;
            }
            if (ExecutionPlan.SelectedIndex == 1)
            {
                Metadata.Visibility = Visibility.Visible;
                DataGridParametersTestLayout.Visibility = Data.Visibility = Visibility.Collapsed;
            }
            if (ExecutionPlan.SelectedIndex == 2
                || ExecutionPlan.SelectedIndex == 6
                || ExecutionPlan.SelectedIndex == 9)
            {
                DataGridLayout.ItemsSource = CurrentDataset = GetActionResult();
                DataGridParametersLayout.ItemsSource = new ObservableCollection<ActionParametersDto>
                {

                    new ActionParametersDto(0,300,7,100)
                };
                Header1.Content = "Measured data";
                Header2.Content = "Action parameters";
                Description.Content = "Current sweep: Action sweeps through current from min to max current with given number of steps. Before the measurement it settles for given amount of time";
                DataGridParametersLayout.Visibility = Visibility.Visible;
                DataGridParametersTestLayout.Visibility = Visibility.Collapsed;
                ChartValues.SelectedIndex = 0;
            }
            else if (((ListBoxItem)ExecutionPlan.SelectedItem).ToString().EndsWith("1"))
            {
                {
                    DataGridLayout.ItemsSource = new ObservableCollection<DataItemDto>
                    {
                        new DataItemDto(null, null, 0, null),
                    };
                    DataGridParametersTestLayout.ItemsSource = new ObservableCollection<TestParametersDto>
                    {
                        new TestParametersDto(11,"<=",true, "Hall")
                    };
                    CurrentDataset = GetActionResult();
                    ChartValues.SelectedIndex = 2;
                }
            }
            else if (((ListBoxItem)ExecutionPlan.SelectedItem).ToString().EndsWith("2.2"))
            {
                {
                    CurrentDataset = GetActionResult();
                    DataGridLayout.ItemsSource = new ObservableCollection<DataItemDto>
                    {
                        new DataItemDto(null, null, null, 26),
                    };
                    DataGridParametersTestLayout.ItemsSource = new ObservableCollection<TestParametersDto>
                    {
                        new TestParametersDto(7,">=",true, "Temperature")
                    };
                    ChartValues.SelectedIndex = 1;
                }
            }
            else if (((ListBoxItem)ExecutionPlan.SelectedItem).ToString().EndsWith("2"))
            {
                {
                    CurrentDataset = GetActionResult();
                    DataGridLayout.ItemsSource = new ObservableCollection<DataItemDto>
                    {
                        new DataItemDto(null, null, null, 26),
                    };
                    DataGridParametersTestLayout.ItemsSource = new ObservableCollection<TestParametersDto>
                    {
                        new TestParametersDto(7,"<=",true, "Temperature")
                    };
                    ChartValues.SelectedIndex = 1;
                }
            }
            else if (((ListBoxItem)ExecutionPlan.SelectedItem).ToString().EndsWith("3"))
            {
                {
                    CurrentDataset = GetActionResult();
                    DataGridLayout.ItemsSource = new ObservableCollection<DataItemDto>
                    {
                        new DataItemDto(3, null, null, null),
                    };
                    DataGridParametersTestLayout.ItemsSource = new ObservableCollection<TestParametersDto>
                    {
                        new TestParametersDto(3,"==",false,"Step ID")
                    };
                    ChartValues.SelectedIndex = 3;
                }
            }
            else if (((ListBoxItem)ExecutionPlan.SelectedItem).ToString().EndsWith("4"))
            {
                {
                    CurrentDataset = GetActionResult();
                    DataGridLayout.ItemsSource = new ObservableCollection<DataItemDto>
                    {
                        new DataItemDto(null, 150, null, null),
                    };
                    DataGridParametersTestLayout.ItemsSource = new ObservableCollection<TestParametersDto>
                    {
                        new TestParametersDto(-20,">",false,"Current")
                    };
                    ChartValues.SelectedIndex = 1;
                }
            }
            else if (((ListBoxItem)ExecutionPlan.SelectedItem).ToString().EndsWith("5"))
            {
                {
                    CurrentDataset = GetActionResult();
                    DataGridLayout.ItemsSource = new ObservableCollection<DataItemDto>
                    {
                        new DataItemDto(5, null, null, null),
                    };
                    DataGridParametersTestLayout.ItemsSource = new ObservableCollection<TestParametersDto>
                    {
                        new TestParametersDto(10,"<",true,"Step ID")
                    };
                    ChartValues.SelectedIndex = 3;
                }
            }
            ChartValues_OnSelectionChanged(null, null);
        }

        private IEnumerable<DataItemDto> GetActionResult()
        {
            return new ObservableCollection<DataItemDto>
                {
                    new DataItemDto(1, 0, 0, 25),
                    new DataItemDto(2, 50, 100, 26),
                    new DataItemDto(3, 100, 200, 26),
                    new DataItemDto(4, 150, 300, 26),
                    new DataItemDto(5, 200, 400, 26),
                    new DataItemDto(6, 250, 500, 26),
                    new DataItemDto(7, 300, 600, 27),
                };
        }

        public IEnumerable<DataItemDto> CurrentDataset { get; set; }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Profiles.SelectedIndex == 0)
            {
                ModifyExecutionPlan(5);
            }
            if (Profiles.SelectedIndex == 1)
            {
                ModifyExecutionPlan(8, 6, 8);
            }
            if (Profiles.SelectedIndex == 2)
            {
                ModifyExecutionPlan(14);
            }
        }

        private void ModifyExecutionPlan(int itemsCount, params int[] failed)
        {
            var counter = 0d;
            foreach (var executionPlanItem in ExecutionPlan.Items)
            {
                var listBoxItem = ((ListBoxItem) executionPlanItem);
                listBoxItem.Visibility = counter <= itemsCount
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                listBoxItem.Background = Brushes.LightGreen;
                counter++;
            }

            foreach (var failedIndex in failed)
            {
                ((ListBoxItem) ExecutionPlan.Items.GetItemAt(failedIndex)).Background = Brushes.PaleVioletRed;
            }
        }

        private void FilterTextBox_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (FilterTextBox.Text.Length == 8)
            {
                ProductDetails.Visibility = Visibility.Visible;
                Profiles.SelectedIndex = 0;
                ExecutionPlan.SelectedIndex = 0;
                Selector_OnSelectionChanged(null, null);
                ExecutionPlan_OnSelectionChanged(null, null);
            }
            Message.Visibility = FilterTextBox.Text.Length == 8 ? Visibility.Collapsed : Visibility.Visible;
            ProductDetails.Visibility = FilterTextBox.Text.Length == 8 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ScanQr(object sender, MouseButtonEventArgs e)
        {
            FilterTextBox.Text = "AWAA7852";
            FilterTextBox_OnPreviewKeyUp(null, null);
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

        private void ChartValues_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ChartValues.SelectedIndex)
            {
                case 0:
                    InitializeChart(new ChartDataSet("Current [mA]",GetCurrent));
                    break;
                case 1:
                    InitializeChart(new ChartDataSet("Temperature [°C]", GetTemperature));
                    break;
                case 2:
                    InitializeChart(new ChartDataSet("Hall [a.u.]", GetHall));
                    break;
                case 3:
                    InitializeChart(new ChartDataSet("Step ID [a.u.]", GetStepId));
                    break;
            }
        }
    }

    public class DataItemDto
    {
        public int? StepId { get; set; }
        public int? Current { get; set; }
        public int? Hall { get; set; }
        public int? Temperature { get; set; }

        public DataItemDto(int? stepId, int? current, int? hall, int? temperature)
        {
            StepId = stepId;
            Current = current;
            Hall = hall;
            Temperature = temperature;
        }
    }

    public class ChartDataSet
    {
        public string Title { get; set; }
        public Func<DataItemDto,double> Action { get; set; }

        public ChartDataSet(string title, Func<DataItemDto, double> action)
        {
            Title = title;
            Action = action;
        }

    }
}
