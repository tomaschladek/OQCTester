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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OQCTester.Dialogs;
using OQCTester.Dtos;

namespace OQCTester
{
    /// <summary>
    /// Interaction logic for ExecutionPlanEditor.xaml
    /// </summary>
    public partial class ExecutionPlanEditor : UserControl
    {
        public ExecutionPlanEditor()
        {
            InitializeComponent();
            This = this;
        }

        public static ExecutionPlanEditor This { get; set; }

        private void ExecutionPlan_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var holder = sender as ListBox;
            var item = holder.SelectedItem as ListBoxItem;
            if (item is null)
            {
                return;
            }

            if (item.Content.ToString().Contains("Action"))
            {
                if (item.Content.ToString().EndsWith("1"))
                {
                    ShowAction(50,250,10,100);
                }
                else if (item.Content.ToString().EndsWith("2"))
                {
                    ShowAction(0,100,15,100);
                }
                else if (item.Content.ToString().EndsWith("3"))
                {
                    ShowAction(0,250,35,70);
                }
            }
            else if (item.Content.ToString().Contains("Test"))
            {
                if (item.Content.ToString().EndsWith("1"))
                {
                    ShowTest("<",2.3,true,"Current");
                }
                else if (item.Content.ToString().EndsWith("2"))
                {
                    ShowTest(">",1.7, false, "Hall");
                }
                else if (item.Content.ToString().EndsWith("3"))
                {
                    ShowTest("==",16.5, false, "Temperature");
                }
                else if (item.Content.ToString().EndsWith("4"))
                {
                    ShowTest("<",1,true,"Error");
                }
                else if (item.Content.ToString().EndsWith("5"))
                {
                    ShowTest("<=",-0.5,false,"Swing");
                }
            }
        }

        private void ShowTest(string operator1, double threshold, bool isAbsolute, string dataField)
        {
            Key1.Content = "Operator";
            Value1.Content = operator1;
            Unit1.Content = "a.u";
            Key2.Content = "Threshold";
            Value2.Content = threshold;
            Unit2.Content = "a.u.";
            Key3.Content = "Is absolute value";
            Value3.Content = isAbsolute;
            Unit3.Content = "boolean";
            Key4.Content = "Data field";
            Value4.Content = dataField;
            Unit4.Content = "a.u.";
        }

        private void ShowAction(int from, int to, int steps, int settleTime)
        {
            Key1.Content = "Min";
            Value1.Content = from.ToString();
            Unit1.Content = "mA";
            Key2.Content = "Max";
            Value2.Content = to;
            Unit2.Content = "mA";
            Key3.Content = "Steps";
            Value3.Content = steps;
            Unit3.Content = "#";
            Key4.Content = "Settle time";
            Value4.Content = settleTime;
            Unit4.Content = "ms";
        }

        private void AddItem(object sender, MouseButtonEventArgs e)
        {
            var dialog = new ExecutionPlanDialog();
            dialog.DataContext = "Add Item";
            dialog.Owner = MainWindow.This;
            dialog.ShowDialog();

            if (dialog.DialogResult != true)
            {
                return;
            }

            var newItem = new ListBoxItem
            {
                Content = "Test X.5",
                Background = Brushes.LightYellow,
                Margin = new Thickness(0,0,10, 0),
                Padding = new Thickness(15, 5, 30, 5)
            };

            ExecutionPlan.Items.Add(newItem);
        }

        private void EditItem(object sender, MouseButtonEventArgs e)
        {
            var dialog = new ExecutionPlanDialog();
            dialog.DataContext = "Edit Item";
            dialog.Owner = MainWindow.This;
            dialog.ShowDialog();

            if (dialog.DialogResult != true)
            {
                return;
            }
        }

        private void RemoveItem(object sender, MouseButtonEventArgs e)
        {
            if (ExecutionPlan.SelectedItem == null)
            {
                return;
            }

            if (new YesNoDialog { DataContext = "Do you really want to remove execution item?" }.ShowDialog() == true)
            {
                ExecutionPlan.Items.Remove(ExecutionPlan.SelectedItem);
            }
        }

        public void Update()
        {
            
        }

        private void RevertItem(object sender, MouseButtonEventArgs e)
        {
            if (new YesNoDialog{DataContext = "All changes in this section will be lost. Do you want to continue?"}.ShowDialog() == true)
            {
                while (ExecutionPlan.Items.Count > 13)
                {
                    ExecutionPlan.Items.RemoveAt(ExecutionPlan.Items.Count - 1);
                }
                while (ExecutionPlan.Items.Count < 13)
                {
                    ExecutionPlan.Items.Add("Test 3.X");
                }
            }
        }
    }
}
