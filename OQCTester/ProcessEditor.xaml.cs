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
    /// Interaction logic for ProcessEditor.xaml
    /// </summary>
    public partial class ProcessEditor : UserControl
    {
        public static ProductDto Product;

        public ProcessEditor()
        {
            InitializeComponent();
            This = this;
            var profiles = new List<ProfileDto>();
            Product = new ProductDto("Awesome product", 3, "XPQ", "New awesome product for the market!",profiles);
            var metadata = new MetadataDto(Product,"Soft test",5,"Soft test of the device", "Current-Hall-Temperature", "1x1");
            var executionPlan = new ExecutionPlanDto(new List<ActionWrapperDto>
            {
                new ActionWrapperDto(new ActionParametersDto(0,50,10,100), new List<TestParametersDto>
                {
                    new TestParametersDto(10,"<",true,"Temperature"),
                    new TestParametersDto(7.1,">",true,"Hall"),
                    new TestParametersDto(-1,"==",true,"Current")
                } ),
            });
            var devices = new DevicesDto(new List<DeviceDto>
            {
                new DeviceDto("Driver USB", "128.6.2.102",215,1,3),
                new DeviceDto("XPR", "77.3.1.1",777,5,8)
            });
            var discoverability = new DiscoverabilityDto("Restricted access", new List<SetupPresenceDto>
            {
                new SetupPresenceDto(false,"PC 1"),
                new SetupPresenceDto(false,"PC 2"),
                new SetupPresenceDto(false,"PC 3"),
                new SetupPresenceDto(false,"PC 7")
            });
            profiles.Add(new ProfileDto(metadata,executionPlan,devices,discoverability));
        }

        public static ProcessEditor This { get; set; }

        private void ProfileSelector_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Metadata.Visibility = Section.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            PhysicalProperties.Visibility = Section.SelectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            Setup.Visibility = Section.SelectedIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
            Devices.Visibility = Section.SelectedIndex == 3 ? Visibility.Visible : Visibility.Collapsed;
            ExecutionPlan.Visibility = Section.SelectedIndex == 4 ? Visibility.Visible : Visibility.Collapsed;
            Discoverability.Visibility = Section.SelectedIndex == 5 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectorView.Visibility = Visibility.Visible;
            ProfileSelector.This.ClearSelection(null, null);
        }

        private void CancelAction(object sender, MouseButtonEventArgs e)
        {
            var dialog = new YesNoDialog
            {
                DataContext = "All changes will be lost. Do you want to continue?"
            };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                UIElement_OnMouseLeftButtonDown(null, null);
            }
        }
    }
}
