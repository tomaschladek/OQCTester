using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for SupervisionSelector.xaml
    /// </summary>
    public partial class SupervisionSelector : UserControl
    {
        public SupervisionSelector()
        {
            InitializeComponent();
            Populate();
        }

        private void Populate()
        {
            TreeView.Items.Clear();
            AddNewNode("Line 1", "Setup 1", "Setup 2");
            AddNewNode("Line 2", "Setup 10", "Setup 11", "Setup 12", "Setup 12");
            AddNewNode("Line 1 EU", "Setup 1", "Setup 2", "Setup 3", "Setup 3P");
            AddNewNode("Line 2 EU", "Setup 10", "Setup 11", "Setup 12", "Setup 12");
            Message.Visibility = TreeView.Items.Count != 0 ? Visibility.Collapsed : Visibility.Visible;
            TreeView.Visibility = TreeView.Items.Count != 0 ? Visibility.Visible : Visibility.Collapsed;

            //< TreeViewItem Header = "Line 1" IsExpanded = "True" >

            //< TreeViewItem Style = "{StaticResource LinkTreeView}" Header = "Setup 1" PreviewMouseDown = "OpenSetupOne" />

            //< TreeViewItem Style = "{StaticResource LinkTreeView}" Header = "Setup 2" PreviewMouseDown = "OpenSetupOne" />

            //< TreeViewItem Style = "{StaticResource LinkTreeView}" Header = "Setup 3" PreviewMouseDown = "OpenSetupOne" />

            //</ TreeViewItem >

            //< TreeViewItem Header = "Line 2" IsExpanded = "True" >

            //< TreeViewItem Style = "{StaticResource LinkTreeView}" Header = "Setup 4" PreviewMouseDown = "OpenSetupOne" />

            //< TreeViewItem Style = "{StaticResource LinkTreeView}" Header = "Setup 5" PreviewMouseDown = "OpenSetupOne" />

            //< TreeViewItem Style = "{StaticResource LinkTreeView}" Header = "Setup 6" PreviewMouseDown = "OpenSetupOne" />

            //</ TreeViewItem >
        }

        private void AddNewNode(string lineName, params string[] lineNames)
        {
            var root = new TreeViewItem
            {
                Header = lineName,
                IsExpanded = true
            };

            foreach (var childNode in lineNames)
            {
                if (lineName.ToLower().Contains(FilterTextBox.Text.ToLower()) || childNode.ToLower().Contains(FilterTextBox.Text.ToLower()))
                {
                    root.Items.Add(new TreeViewItem
                    {
                        Header = childNode,
                        Style = this.FindResource("LinkTreeView") as Style
                    });
                }
            }

            if (root.Items.Count > 0)
            {
                TreeView.Items.Add(root);
            }
        }

        private void OpenSetupOne(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://docs.microsoft.com/en-us/power-bi/media/sample-human-resources/hr1.png");
        }

        private void ScanQr(object sender, MouseButtonEventArgs e)
        {
            FilterTextBox.Text = "Setup 2";
            Populate();
        }

        private void ClearSelection(object sender, MouseButtonEventArgs e)
        {
            FilterTextBox.Text = "";
            Populate();
        }

        private void FilterTextBox_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            Populate();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeView.SelectedItem is TreeViewItem treeViewItem)
            {
                if (treeViewItem.Header.ToString().StartsWith("Setup"))
                {
                    OpenSetupOne(null, null);
                }
            }
        }
    }
}
