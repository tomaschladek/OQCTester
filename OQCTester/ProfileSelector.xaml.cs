using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for ProfileSelector.xaml
    /// </summary>
    public partial class ProfileSelector : UserControl
    {
        private const string DefaultValue = "Select process or use filter";

        public ProfileSelector()
        {
            InitializeComponent();
            This = this;
            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            TreeView.Items.Clear();
            AddNewNode("EL 10-30", "Core test", "Calibration", "Final testing");
            AddNewNode("EL 10-42", "Wafer","Pressure test","Humidity test");
            AddNewNode("EL 3-10",  "Core","Image processing");
            AddNewNode("MR 15-30", "Single product","Integrated");
            AddNewNode("MR 10-20", "Auto-calibration");
            Message.Visibility = TreeView.Items.Count > 0 
                ? Visibility.Collapsed
                : Visibility.Visible;
            Preview.Visibility = TreeView.Items.Count == 0
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void AddNewNode(string nodeName, params string[] childNodes)
        {
            var root = new TreeViewItem
            {
                Header = nodeName
            };

            var comparisonString = FilterTextBox.Text == DefaultValue
                ? ""
                : FilterTextBox.Text;

            foreach (var childNode in childNodes)
            {
                if (nodeName.ToLower().Contains(comparisonString.ToLower()) || childNode.ToLower().Contains(comparisonString.ToLower()))
                {
                    root.Items.Add(new TreeViewItem
                    {
                        Header = childNode
                    });
                }
            }

            if (root.Items.Count > 0)
            {
                TreeView.Items.Add(root);
                if (TreeView.Items.Count == 1 && !string.IsNullOrEmpty(comparisonString))
                {
                    root.IsExpanded = true;
                }
            }
        }

        public static ProfileSelector This { get; set; }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeView.SelectedItem is TreeViewItem treeViewItem)
            {
                var itemName = treeViewItem.Header.ToString();
                Name.Content = itemName;
                Preview.Visibility = itemName.ToLower().StartsWith("add")
                    ? Visibility.Collapsed
                    : Visibility.Visible;
                if (treeViewItem.Parent is TreeView)
                {
                    Type.Content = "Product";
                    PopulateProduct(new Random().Next(0, 3), new Random().Next(0, 3), new Random().Next(0, 3),
                        new Random().Next(0, 3));

                }
                else
                {
                    Type.Content = "Profile";
                    PopulateProfile(new Random().Next(0, 3), new Random().Next(0, 3), new Random().Next(0, 3),
                        new Random().Next(0, 3), new Random().Next(0, 3), new Random().Next(0, 3),
                        new Random().Next(0, 3), new Random().Next(0, 3), new Random().Next(0, 3));
                }

                EditButton.Visibility = Visibility.Visible;
                Preview.Visibility = Visibility.Visible;
            }
            else
            {
                Preview.Visibility = Visibility.Collapsed;
            }
        }

        private void PopulateProfile(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9)
        {
            Property1.Content = new[] { "Workload test", "Test only with glasses", "Not finished", "Not working" }[index1];
            Property1Name.Content = "Description";
            Property2.Content = new[] { "Matrix", "Single station", "Orinoco", "Neo" }[index2];
            Property2Name.Content = "Setup";
            Property3Name.Content = "Physical properties";
            Property3.Content = new[] { "Current", "Current, Temperature", "Current, Hall", "Current,Hall, Temperature" }[index3];
            Property4Name.Content = "Export path";
            Property4.Content = new[] { "C:/BackupPath/Production/", "C:/BackupPath/Production_C/", "C:/BackupPath/Testing/", "C:/BackupPath/Development/" }[index4];
            Property5Name.Content = "Devices";
            Property5.Content = new[] { "SHS, LD",  "SHS, PLC, Light source, RFID, LD",  "SHS, Light source, LD",  "LD"}[index4];
            Property6Name.Content = "Discoverability";
            Property6.Content = new[] { "No restriction",  "PC 1,3", "PC 1", "PC 2"}[index5];
            Property7Name.Content = "Actions";
            Property7.Content = new[] { "Hardware check", "Hardware check", "Hardware check", "Hardware check" }[index6];
            Property8.Content = new[] { "Burn-in",  "Room sweep",  "Measure",  "Apply pressure"}[index7];            
            Property9.Content = new[] { "Heat-up, LD", "Hysteresis", "Particles detection", "" }[index8];
            Property10.Content = new[] { "Sweep", "Export", "", "" }[index9];
        }

        private void PopulateProduct(int index1, int index2, int index3, int index4)
        {
            Property1.Content = new[] { "New prototype.", " Tests only", "Mass production", "Not working" }[index1];
            Property1Name.Content = "Description";
            Property2.Content = new[] { "3", "7", "10", "14" }[index2];
            Property2Name.Content = "Firmware";
            Property3.Content = new[] { "A[A-Z]{7}[0-9]{4}", "C[A-Z]{1}[0-9]{4}", "W[A-Z]{2}[0-9]{8}", "A[A-Z]{3}[0-9]{4}QW" }[index3];
            Property3Name.Content = "Serial number";
            Property4.Content = new[] { "[A-Z]{4}[0-9]{4}", "OSW-[A-Z]{3}-[0-9]{4}", "OCH-[A-Z]{3}[0-9]{4}" , "FFE-[A-Z]{3}[0-9]{4}" }[index4];
            Property4Name.Content = new[] { "Batch number", "Tray number", "Wafer number", "Wafer number" }[index4];
            Property5.Content = new[] { "", "[A-Z]{4}[0-9]{4}", "[A-Z]{4}[0-9]{4}", "[A-Z]{4}[0-9]{4}"  }[index4];
            Property5Name.Content = new[] { "", "Batch number", "Batch number", "Batch number" }[index4];
            Property6.Content = "";
            Property7.Content = "";
            Property8.Content = "";
            Property9.Content = "";
            Property10.Content = "";
        }

        public void ClearSelection(object sender, MouseButtonEventArgs e)
        {
            MainWindow.This.AppBarTitle.Content = $"Product editor";
            FilterTextBox.Text = DefaultValue;
            var selectedItem = TreeView.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                selectedItem.IsSelected = false;
            }
            TreeView.Visibility = Visibility.Visible;
            ProcessEditor.This.Editor.Visibility
                = Preview.Visibility
                = ProcessEditor.This.SaveCancelContainer.Visibility
                    = Visibility.Collapsed;
            PopulateTreeView();
        }

        private void FilterTextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            PopulateTreeView();
        }

        private void ScanQr(object sender, MouseButtonEventArgs e)
        {
            FilterTextBox.Text = "Calibration";
            PopulateTreeView();
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
            if (FilterTextBox.Text == string.Empty)
            {
                FilterTextBox.Text = DefaultValue;
            }
        }

        private void TreeView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeView item)
            {
                var selectedItem = item.SelectedItem as TreeViewItem;
                string header = "new";
                if (selectedItem == null && item.Items.Count != 0)
                {
                    header = ((TreeViewItem) item.Items[0]).Header.ToString();
                    //return;
                }
                FilterTextBox.Text = header;
                ProcessEditor.This.SelectorView.Visibility = Visibility.Collapsed;
                TreeView.Visibility = Visibility.Collapsed;
                ProcessEditor.This.Editor.Visibility
                    = ProcessEditor.This.SaveCancelContainer.Visibility
                        = Visibility.Visible;
                MainWindow.This.AppBarTitle.Content = $"Product editor - {header}";
                ProcessEditor.Product.Name = header;
                MetadataEditor.This.Update();
                DeviceEditor.This.Update();
                ExecutionPlanEditor.This.Update();
                DiscoverabilityEditor.This.Update();
            }
            else
            {
                FilterTextBox.Text = DefaultValue;
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            TreeView_OnMouseDoubleClick(TreeView,null);
        }
    }
}
