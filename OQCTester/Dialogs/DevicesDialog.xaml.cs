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
using OQCTester.Dtos;

namespace OQCTester.Dialogs
{
    /// <summary>
    /// Interaction logic for DevicesDialog.xaml
    /// </summary>
    public partial class DevicesDialog : Window
    {
        public DevicesDialog()
        {
            InitializeComponent();
            MainWindow.This.Opacity = 0.5;
            Closed += (sender, args) => { MainWindow.This.Opacity = 1; };

            if (DeviceEditor.SelectedIndex >= 0)
            {
                var device = ProcessEditor.Product.Profiles[0].Devices.Devices[DeviceEditor.SelectedIndex];
                IpAddress.Text = device.IpAddress;
                Port.Text = device.Port.ToString();
                Speed.SelectedIndex = device.Speed == -1 ? 0 : device.Speed == 0 ? 1 : device.Speed == 1 ? 2 : device.Speed == 3 ? 3 : 4;
                Direction.Text = device.Direction.ToString();
            }
            else
            {
                IpAddress.Text = string.Empty;
                Port.Text = string.Empty;
                Speed.SelectedIndex = 0;
                Direction.Text = string.Empty;
            }

            IpError.Visibility = PortError.Visibility = DirectionError.Visibility = Visibility.Collapsed;

        }

        private void CancelAction(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveAction(object sender, MouseButtonEventArgs e)
        {
            IpError.Visibility = PortError.Visibility = DirectionError.Visibility = Visibility.Collapsed;

            var isValid = true;
            int a;
            int b;
            int c;
            int d;
            if (string.IsNullOrEmpty(IpAddress.Text)
                || IpAddress.Text.Split('.').Length != 4
                || !int.TryParse(IpAddress.Text.Split('.')[0], out a)
                || !int.TryParse(IpAddress.Text.Split('.')[1], out b)
                || !int.TryParse(IpAddress.Text.Split('.')[2], out c)
                || !int.TryParse(IpAddress.Text.Split('.')[3], out d)
                || a < 0 || a > 255
                || b < 0 || b > 255
                || c < 0 || c > 255
                || d < 0 || d > 255
            )
            {
                isValid = false;
                IpError.Visibility = Visibility.Visible;
            }

            if (string.IsNullOrEmpty(Port.Text)
                || !int.TryParse(Port.Text, out a)
            )
            {
                isValid = false;
                PortError.Visibility = Visibility.Visible;
            }

            if (string.IsNullOrEmpty(Direction.Text)
                || !int.TryParse(Direction.Text, out a)
            )
            {
                isValid = false;
                DirectionError.Visibility = Visibility.Visible;
            }

                if (!isValid)
            {
                return;
            }
            DialogResult = true;

             var device = DeviceEditor.SelectedIndex >= 0 
                ? ProcessEditor.Product.Profiles[0].Devices.Devices[DeviceEditor.SelectedIndex]
                : new DeviceDto(null,null,0,0,0);

            device.Name = DeviceName.Text;
            device.IpAddress = IpAddress.Text;
            device.Port = int.Parse(Port.Text);
            device.Speed = int.Parse(Speed.Text);
            device.Direction = int.Parse(Direction.Text);

            if (DeviceEditor.SelectedIndex < 0)
            {
                ProcessEditor.Product.Profiles[0].Devices.Devices.Add(device);
            }
        }
    }
}
