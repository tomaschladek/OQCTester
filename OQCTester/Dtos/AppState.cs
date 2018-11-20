using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace OQCTester.Dtos
{
    public enum EMode
    {
        Oqc, Editor, Logs, Results, Home, Bi
    }
    public class AppState
    {
        public MenuItem OQC { get; set; }
        public MenuItem Editor { get; set; }
        public MenuItem Supervision { get; set; }
        public MenuItem LogView { get; set; }
        public MenuItem ProductViewer { get; set; }
        public MenuItem Help { get; set; }
        public MenuItem About { get; set; }

        public EMode IsOqc { get; set; }

        public AppState(MenuItem oqc, MenuItem editor, MenuItem supervision, MenuItem productViewer, MenuItem logView,
            MenuItem help, MenuItem about)
        {
            OQC = oqc;
            Editor = editor;
            Supervision = supervision;
            LogView = logView;
            ProductViewer = productViewer;
            Help = help;
            About = about;
        }
    }

    public class MenuItem
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public SolidColorBrush Color { get; set; }

        public MenuItem(string shortName, string fullName, SolidColorBrush color)
        {
            ShortName = shortName;
            FullName = fullName;
            Color = color;
        }
    }

    public class ProfileDto
    {
        public MetadataDto Metadata { get; set; }
        public ExecutionPlanDto ExecutionPlan { get; set; }
        public DevicesDto Devices { get; set; }
        public DiscoverabilityDto Discoverability { get; set; }

        public ProfileDto(MetadataDto metadata, ExecutionPlanDto executionPlan, DevicesDto devices,
            DiscoverabilityDto discoverability)
        {
            Metadata = metadata;
            ExecutionPlan = executionPlan;
            Devices = devices;
            Discoverability = discoverability;
        }
    }

    public class SetupPresenceDto
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }

        public SetupPresenceDto(bool isChecked, string name)
        {
            IsChecked = isChecked;
            Name = name;
        }
    }

    public class DiscoverabilityDto
    {
        public string Mode { get; set; }

        public IList<SetupPresenceDto> DiscoverabilityCollection { get;set;}

        public DiscoverabilityDto(string mode, IList<SetupPresenceDto> discoverabilityCollection)
        {
            Mode = mode;
            DiscoverabilityCollection = discoverabilityCollection;
        }
    }

    public class DeviceDto
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int Speed { get; set; }
        public int Direction { get; set; }

        public DeviceDto(string name, string ipAddress, int port, int speed, int direction)
        {
            Name = name;
            IpAddress = ipAddress;
            Port = port;
            Speed = speed;
            Direction = direction;
        }
    }

    public class DevicesDto
    {
        public IList<DeviceDto> Devices { get; set; }

        public DevicesDto(IList<DeviceDto> devices)
        {
            Devices = devices;
        }
    }

    public class ExecutionPlanDto
    {
        public IList<ActionWrapperDto> Actions { get; set; }

        public ExecutionPlanDto(IList<ActionWrapperDto> actions)
        {
            Actions = actions;
        }
    }

    public class ProductDto
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public IList<ProfileDto> Profiles { get; set; }

        public ProductDto(string name, int version, string module, string description, IList<ProfileDto> profiles)
        {
            Name = name;
            Version = version;
            Module = module;
            Description = description;
            Profiles = profiles;
        }
    }

    public class MetadataDto
    {
        public ProductDto Product { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string ProductConfiguration { get; set; }
        public string Setup { get; set; }

        public MetadataDto(ProductDto product, string name, int version, string description, string productConfiguration, string setup)
        {
            Product = product;
            Name = name;
            Version = version;
            Description = description;
            ProductConfiguration = productConfiguration;
            Setup = setup;
        }
    }


    public class TestParametersDto
    {
        public double Threshold { get; set; }
        public string Operator { get; set; }
        public bool IsAbsolute { get; set; }
        public string DataField { get; set; }

        public TestParametersDto(double threshold, string @operator, bool isAbsolute, string dataField)
        {
            Threshold = threshold;
            Operator = @operator;
            IsAbsolute = isAbsolute;
            DataField = dataField;
        }
    }

    public class ActionWrapperDto
    {
        public ActionParametersDto Action { get; set; }
        public IList<TestParametersDto> Tests { get; set; }

        public ActionWrapperDto(ActionParametersDto action, IList<TestParametersDto> tests)
        {
            Action = action;
            Tests = tests;
        }
    }

    public class ActionParametersDto
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Steps { get; set; }
        public int SettleTime { get; set; }

        public ActionParametersDto(int min, int max, int steps, int settleTime)
        {
            Min = min;
            Max = max;
            Steps = steps;
            SettleTime = settleTime;
        }
    }
}