using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DeviceEditor.xaml
    /// </summary>
    public partial class DeviceEditor : UserControl
    {
        public DeviceEditor()
        {
            InitializeComponent();
            This = this;
            Key1.Content = "IP Address";
            Key2.Content = "Port";
            Key3.Content = "Direction";
            Key4.Content = "Speed";
        }
        
        public static DeviceEditor This { get; set; }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Devices.SelectedIndex >= 0)
            {
                var device = ProcessEditor.Product.Profiles[0].Devices.Devices[Devices.SelectedIndex];

                Value1.Content = device.IpAddress;
                Value2.Content = device.Port;
                Value3.Content = device.Direction;
                Value4.Content = device.Speed;
            }
        }

        private void AddDevice(object sender, MouseButtonEventArgs e)
        {
            SelectedIndex = -1;
            var dialog = new DevicesDialog();
            dialog.DataContext = "Add device";
            dialog.Owner = MainWindow.This;
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                Update();
            }
        }

        public static int SelectedIndex { get; set; }

        private void EditDevice(object sender, MouseButtonEventArgs e)
        {
            if (!ProcessEditor.Product.Profiles[0].Devices.Devices.Any())
            {
                return;
            }
            SelectedIndex = Devices.SelectedIndex;;
            var dialog = new DevicesDialog();
            dialog.DataContext = "Edit device";
            dialog.Owner = MainWindow.This;
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                Update();
            }
        }

        private void RemoveDevice(object sender, MouseButtonEventArgs e)
        {
            if (Devices.SelectedItem == null)
            {
                return;
            }

            if (new YesNoDialog { DataContext = "Do you really want to remove device?" }.ShowDialog() == true)
            {
                Devices.Items.Remove(Devices.SelectedItem);
            }
        }

        public void Update()
        {
            Devices.Items.Clear();
            var devices = ProcessEditor.Product.Profiles[0].Devices.Devices;
            foreach (var device in devices)
            {
                Devices.Items.Add(new ListBoxItem()
                {
                    Content = device.Name
                });
            }

            Devices.SelectedIndex = 0;
            Selector_OnSelectionChanged(null, null);
        }

        private void RevertItem(object sender, MouseButtonEventArgs e)
        {
            if (new YesNoDialog { DataContext = "All changes in this section will be lost. Do you want to continue?" }.ShowDialog() == true)
            {
                ProcessEditor.Product.Profiles[0].Devices = new DevicesDto(new List<DeviceDto>
                {
                    new DeviceDto("Driver USB", "128.6.2.102",215,1,3),
                    new DeviceDto("XPR", "77.3.1.1",777,5,8)
                });
                Update();
            }
        }
    }
}
